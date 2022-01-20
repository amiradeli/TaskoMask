﻿using TaskoMask.Application.Share.Helpers;
using System.Collections.Generic;
using System.Threading.Tasks;
using TaskoMask.Application.Share.Dtos.Workspace.Organizations;
using TaskoMask.Application.Share.ViewModels;
using TaskoMask.Application.Common.Services;

namespace TaskoMask.Application.Workspace.Organizations.Services
{
    public interface IOrganizationService : IBaseService
    {
        Task<Result<CommandResult>> CreateAsync(OrganizationUpsertDto input);
        Task<Result<CommandResult>> UpdateAsync(OrganizationUpsertDto input);
        Task<Result<OrganizationDetailsViewModel>> GetDetailsAsync(string id);
        Task<Result<IEnumerable<OrganizationDetailsViewModel>>> GetListWithDetailsByOwnerIdAsync(string ownerId);
        Task<Result<OrganizationBasicInfoDto>> GetByIdAsync(string id);
        Task<Result<OrganizationReportDto>> GetReportAsync(string id);
        Task<Result<IEnumerable<OrganizationBasicInfoDto>>> GetListByOwnerIdAsync(string ownerId);
        Task<Result<PaginatedListReturnType<OrganizationOutputDto>>> SearchAsync(int page, int recordsPerPage, string term);
        Task<Result<IEnumerable<SelectListItem>>> GetSelectListAsync(string ownerId);

    }
}
