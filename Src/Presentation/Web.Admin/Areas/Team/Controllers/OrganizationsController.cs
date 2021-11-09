﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using AutoMapper;
using TaskoMask.Web.Common.Controllers;
using TaskoMask.Application.Team.Organizations.Services;
using System.Threading.Tasks;
using TaskoMask.Web.Common.Filters;
using TaskoMask.Web.Common.Helpers;

namespace Aghoosh.Web.Admin.Areas.Accounting.Controllers
{
    [Authorize]
    [Area("Accounting")]
    public class OrganizationsController : BaseMvcController
    {
        #region Fields

        private readonly IOrganizationService _organizationService;


        #endregion

        #region Ctor

        public OrganizationsController(IOrganizationService organizationService, IMapper mapper) : base(mapper)
        {
            _organizationService = organizationService;
        }

        #endregion

        #region Public Methods



        /// <summary>
        /// 
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var organizationQueryResult = await _organizationService.SearchAsync(page: 1, recordsPerPage: recordsPerPage, term: "");

            #region ViewBags

            ViewBag.PageSize = organizationQueryResult.Value.PageNumber;
            ViewBag.CurrentPage = 1;
            ViewBag.TotalItemCount = organizationQueryResult.Value.TotalCount;

            #endregion

            return View(organizationQueryResult);
        }



        /// <summary>
        /// 
        /// </summary>
        [HttpGet]
        [AjaxOnly]
        public async Task<IActionResult> Search(int page = 1, string term = "")
        {
            var organizationQueryResult = await _organizationService.SearchAsync(page: page, recordsPerPage: recordsPerPage, term: term);

            if (!organizationQueryResult.IsSuccess)
                return RedirectToErrorPage(organizationQueryResult);

            #region ViewBags

            ViewBag.PageSize = organizationQueryResult.Value.PageNumber;
            ViewBag.CurrentPage = page;
            ViewBag.TotalItemCount = organizationQueryResult.Value.TotalCount;

            #endregion

            return PartialView("_ItemList", organizationQueryResult.Value.Items);
        }



        /// <summary>
        /// 
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> Details(string id)
        {
            var organizationQueryResult = await _organizationService.GetDetailsAsync(id);

            return View(organizationQueryResult);

        }





        #endregion

        #region Private Methods



        #endregion

    }
}
