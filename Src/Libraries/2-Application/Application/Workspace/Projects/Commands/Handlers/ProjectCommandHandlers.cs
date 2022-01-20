﻿using MediatR;
using System.Threading;
using System.Threading.Tasks;
using TaskoMask.Application.Workspace.Projects.Commands.Models;
using TaskoMask.Application.Share.Resources;
using TaskoMask.Application.Core.Commands;
using TaskoMask.Application.Core.Notifications;
using TaskoMask.Application.Core.Exceptions;
using TaskoMask.Domain.Share.Resources;
using TaskoMask.Application.Core.Bus;
using TaskoMask.Application.Share.Helpers;
using TaskoMask.Domain.WriteModel.Workspace.Owners.Data;
using TaskoMask.Domain.WriteModel.Workspace.Owners.Entities;

namespace TaskoMask.Application.Workspace.Projects.Commands.Handlers
{
    public class ProjectCommandHandlers : BaseCommandHandler,
        IRequestHandler<CreateProjectCommand, CommandResult>,
         IRequestHandler<UpdateProjectCommand, CommandResult>
    {
        #region Fields

        private readonly IOwnerAggregateRepository _ownerAggregateRepository;


        #endregion

        #region Ctors

        public ProjectCommandHandlers(IOwnerAggregateRepository ownerAggregateRepository, IDomainNotificationHandler notifications, IInMemoryBus inMemoryBus) : base(notifications, inMemoryBus)
        {
            _ownerAggregateRepository = ownerAggregateRepository;
        }

        #endregion

        #region Handlers


        /// <summary>
        /// 
        /// </summary>
        public async Task<CommandResult> Handle(CreateProjectCommand request, CancellationToken cancellationToken)
        {
            var owner = await _ownerAggregateRepository.GetByOrganizationIdAsync(request.OrganizationId);
            if (owner == null)
                throw new ApplicationException(ApplicationMessages.Data_Not_exist, DomainMetadata.Owner);

            var project = Project.Create(request.Name,request.Description,request.OrganizationId);
            owner.CreateProject(request.OrganizationId,project);

            await _ownerAggregateRepository.UpdateAsync(owner);
            return new CommandResult(ApplicationMessages.Create_Success, project.Id);

        }



        /// <summary>
        /// 
        /// </summary>
        public async Task<CommandResult> Handle(UpdateProjectCommand request, CancellationToken cancellationToken)
        {
            var owner = await _ownerAggregateRepository.GetByOrganizationIdAsync(request.OrganizationId);
            if (owner == null)
                throw new ApplicationException(ApplicationMessages.Data_Not_exist, DomainMetadata.Owner);

            owner.UpdateProject(request.OrganizationId, request.Id, request.Name, request.Description);

            await _ownerAggregateRepository.UpdateAsync(owner);
            return new CommandResult(ApplicationMessages.Update_Success, request.Id);

        }


        #endregion

    }
}
