﻿using System.Threading;
using System.Threading.Tasks;
using TaskoMask.Application.Workspace.Cards.Commands.Models;
using TaskoMask.Application.Share.Resources;
using TaskoMask.Application.Core.Commands;
using TaskoMask.Application.Core.Notifications;
using TaskoMask.Application.Core.Exceptions;
using TaskoMask.Domain.Share.Resources;
using MediatR;
using TaskoMask.Application.Core.Bus;
using TaskoMask.Application.Share.Helpers;
using TaskoMask.Domain.WriteModel.Workspace.Boards.Data;
using TaskoMask.Domain.WriteModel.Workspace.Boards.Entities;

namespace TaskoMask.Application.Workspace.Cards.Commands.Handlers
{
    public class CardCommandHandlers : BaseCommandHandler,
        IRequestHandler<CreateCardCommand, CommandResult>,
         IRequestHandler<UpdateCardCommand, CommandResult>
    {
        #region Fields

        private readonly IBoardAggregateRepository _boardAggregateRepository;


        #endregion

        #region Ctors

        public CardCommandHandlers( IDomainNotificationHandler notifications, IBoardAggregateRepository boardAggregateRepository, IInMemoryBus inMemoryBus) : base(notifications, inMemoryBus)
        {
            _boardAggregateRepository = boardAggregateRepository;
        }


        #endregion

        #region Handlers



        /// <summary>
        /// 
        /// </summary>
        public async Task<CommandResult> Handle(CreateCardCommand request, CancellationToken cancellationToken)
        {
            var board = await _boardAggregateRepository.GetByIdAsync(request.BoardId);
            if (board == null)
                throw new ApplicationException(ApplicationMessages.Data_Not_exist, DomainMetadata.Board);

            var card =  Card.Create(name: request.Name, type: request.Type);
            board.CreateCard(card);

            await _boardAggregateRepository.UpdateAsync(board);
            return new CommandResult(ApplicationMessages.Create_Success, card.Id);

        }



        /// <summary>
        /// 
        /// </summary>
        public async Task<CommandResult> Handle(UpdateCardCommand request, CancellationToken cancellationToken)
        {
            var board = await _boardAggregateRepository.GetByCardIdAsync(request.Id);
            if (board == null)
                throw new ApplicationException(ApplicationMessages.Data_Not_exist, DomainMetadata.Board);

            board.UpdateCard(request.Id,request.Name, request.Type);
            await _boardAggregateRepository.UpdateAsync(board);

            return new CommandResult(ApplicationMessages.Update_Success, request.Id);
        }


        #endregion

    }
}
