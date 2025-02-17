﻿using MongoDB.Driver;
using MongoDB.Driver.Linq;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TaskoMask.Domain.ReadModel.Data;
using TaskoMask.Domain.ReadModel.Entities;
using TaskoMask.Infrastructure.Data.Common.Repositories;
using TaskoMask.Infrastructure.Data.ReadModel.DbContext;

namespace TaskoMask.Infrastructure.Data.ReadModel.Repositories
{
    public class CommentRepository : BaseRepository<Comment>, ICommentRepository
    {
        #region Fields

        private readonly IMongoCollection<Comment> _comments;

        #endregion

        #region Ctors

        public CommentRepository(IReadDbContext dbContext) : base(dbContext)
        {
            _comments = dbContext.GetCollection<Comment>();
        }

        #endregion

        #region Public Methods



        #endregion

        #region Private Methods



        #endregion

    }
}
