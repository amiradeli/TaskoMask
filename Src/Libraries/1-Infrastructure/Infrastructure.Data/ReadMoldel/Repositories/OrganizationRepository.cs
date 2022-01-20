﻿using MongoDB.Driver;
using MongoDB.Driver.Linq;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TaskoMask.Domain.ReadModel.Data;
using TaskoMask.Domain.ReadModel.Entities;
using TaskoMask.Infrastructure.Data.Common.Contracts;
using TaskoMask.Infrastructure.Data.WriteMoldel.Repositories;

namespace TaskoMask.Infrastructure.Data.ReadMoldel.Repositories
{
    public class OrganizationRepository : BaseAggregateRepository<Organization>, IOrganizationRepository
    {
        #region Fields

        private readonly IMongoCollection<Organization> _organizations;

        #endregion

        #region Ctors

        public OrganizationRepository(IMongoDbContext dbContext) : base(dbContext)
        {
            _organizations = dbContext.GetCollection<Organization>();
        }

        #endregion

        #region Public Methods



        #endregion

        #region Private Methods



        #endregion

    }
}
