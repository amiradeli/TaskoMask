﻿using AutoMapper;
using TaskoMask.Application.Share.Helpers;
using System.Threading.Tasks;
using TaskoMask.Application.Core.Commands;
using TaskoMask.Application.Core.Notifications;
using TaskoMask.Application.Core.Bus;
using TaskoMask.Application.Common.Users.Services;
using TaskoMask.Application.Share.Dtos.Administration.Operators;
using TaskoMask.Domain.Administration.Entities;
using TaskoMask.Application.Share.ViewModels;
using System.Collections.Generic;
using TaskoMask.Domain.Administration.Data;
using TaskoMask.Application.Share.Resources;
using TaskoMask.Domain.Core.Services;
using TaskoMask.Domain.Share.Resources;
using System.Linq;
using TaskoMask.Domain.Core.ValueObjects;

namespace TaskoMask.Application.Administration.Operators.Services
{
    public class OperatorService : UserService<Operator>, IOperatorService
    {
        #region Fields

        private readonly IOperatorRepository _operatorRepository;
        private readonly IEncryptionService _encryptionService;
        private readonly IRoleRepository _roleRepository;


        #endregion

        #region Ctors

        public OperatorService(IInMemoryBus inMemoryBus, IMapper mapper, IDomainNotificationHandler notifications, IOperatorRepository operatorRepository, IEncryptionService encryptionService, IRoleRepository roleRepository) : base(inMemoryBus, mapper, notifications, operatorRepository, encryptionService)
        {
            _operatorRepository = operatorRepository;
            _encryptionService = encryptionService;
            _roleRepository = roleRepository;
        }


        #endregion

        #region Public Methods



        /// <summary>
        /// 
        /// </summary>
        public async Task<Result<CommandResult>> CreateAsync(OperatorUpsertDto input)
        {
            var existOperator = await _operatorRepository.GetByUserNameAsync(input.Email);
            if (existOperator != null)
                return Result.Failure<CommandResult>(message: ApplicationMessages.User_Email_Already_Exist);


            var identity = UserIdentity.Create(new UserDisplayName(input.DisplayName), new UserEmail(input.Email), new UserPhoneNumber(input.PhoneNumber));
            var authentication = new UserAuthentication(new UserName(input.UserName));
           
            var @operator = new Operator(identity, authentication);
            
            @operator.SetPassword(input.Password, _encryptionService);

            await _operatorRepository.CreateAsync(@operator);

            return Result.Success(new CommandResult(entityId: @operator.Id), ApplicationMessages.Create_Success);

        }



        /// <summary>
        /// 
        /// </summary>
        public async Task<Result<CommandResult>> UpdateAsync(OperatorUpsertDto input)
        {
            var existOperator = await _operatorRepository.GetByUserNameAsync(input.Email);
            if (existOperator != null && existOperator.Id.ToString() != input.Id)
                return Result.Failure<CommandResult>(message: ApplicationMessages.User_Email_Already_Exist);

            var @operator = await _operatorRepository.GetByIdAsync(input.Id);
            if (@operator == null)
                return Result.Failure<CommandResult>(message: string.Format(ApplicationMessages.Data_Not_exist, DomainMetadata.Operator));

            @operator.Update(UserDisplayName.Create(input.DisplayName), UserEmail.Create(input.Email), UserPhoneNumber.Create(input.PhoneNumber), UserName.Create(input.UserName));

            await _operatorRepository.UpdateAsync(@operator);

            return Result.Success(new CommandResult(entityId: @operator.Id), ApplicationMessages.Update_Success);
        }



        /// <summary>
        /// 
        /// </summary>
        public async Task<Result<CommandResult>> UpdateRolesAsync(string id, string[] rolesId)
        {
            var @operator = await _operatorRepository.GetByIdAsync(id);
            if (@operator == null)
                return Result.Failure<CommandResult>(message: string.Format(ApplicationMessages.Data_Not_exist, DomainMetadata.Operator));

            @operator.RolesId = rolesId;

            await _operatorRepository.UpdateAsync(@operator);

            return Result.Success(new CommandResult(entityId: @operator.Id), ApplicationMessages.Update_Success);

        }



        /// <summary>
        /// 
        /// </summary>
        public async Task<Result<OperatorBasicInfoDto>> GetByIdAsync(string id)
        {
            var @operator = await _operatorRepository.GetByIdAsync(id);
            if (@operator == null)
                return Result.Failure<OperatorBasicInfoDto>(message: string.Format(ApplicationMessages.Data_Not_exist, DomainMetadata.Operator));

            return Result.Success(_mapper.Map<OperatorBasicInfoDto>(@operator));
        }



        /// <summary>
        /// 
        /// </summary>
        public async Task<Result<IEnumerable<OperatorOutputDto>>> GetListAsync()
        {
            var operators = await _operatorRepository.GetListAsync();
            var operatorsDto = _mapper.Map<IEnumerable<OperatorOutputDto>>(operators);

            foreach (var item in operatorsDto)
                item.RolesCount = item.RolesId.Length;

            return Result.Success(operatorsDto);
        }



        /// <summary>
        /// 
        /// </summary>
        public async Task<Result<OperatorDetailsViewModel>> GetDetailsAsync(string id)
        {
            var @operator = await _operatorRepository.GetByIdAsync(id);
            if (@operator == null)
                return Result.Failure<OperatorDetailsViewModel>(message: string.Format(ApplicationMessages.Data_Not_exist, DomainMetadata.Operator));

            var roles = await _roleRepository.GetListAsync();

            var model = new OperatorDetailsViewModel
            {
                Operator = _mapper.Map<OperatorUpsertDto>(@operator),
                Roles = roles.Select(role => new SelectListItem
                {
                    Selected = @operator.RolesId != null && @operator.RolesId.Contains(role.Id),
                    Text = role.Name,
                    Value = role.Id,
                }).AsEnumerable(),
            };

            return Result.Success(model);
        }


        #endregion
    }
}
