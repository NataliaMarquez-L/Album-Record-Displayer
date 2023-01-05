using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

using AlbumsApp.Models;
using AlbumsApp.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;

namespace AlbumsApp.Controllers

{
    public class StudioController : Controller
    {            private static string _auditLogSessionKeyName = "auditLog";

        public StudioController(AlbumsDbContext albumsDbContext)
        {
            _albumsDbContext = albumsDbContext;
        }

        public IActionResult List()
        {
           
            List<Studio> studios = _albumsDbContext.Studios.OrderBy(s => s.Name).ToList();
            return View(studios);
        }

        [HttpGet]
        [Authorize]

        public IActionResult Add()

        {
            string actionName = GetControllerActionMethodTypeName();
            string auditLog = GetAuditLogSessionVar();
            AddToAuditLogSessionVar(auditLog, actionName);

            var viewModel = new AddOrDeleteStudioViewModel();
            return View(viewModel);
        }

        [HttpPost]
        [Authorize]

        public IActionResult Add(AddOrDeleteStudioViewModel viewModel)
        {
            string actionName = GetControllerActionMethodTypeName();
            string auditLog = GetAuditLogSessionVar();
            AddToAuditLogSessionVar(auditLog, actionName);

            if (ModelState.IsValid)
            {
                _albumsDbContext.Studios.Add(viewModel);
                _albumsDbContext.SaveChanges();

                return RedirectToAction("List", "Album");
            }
            else
            {
                ModelState.AddModelError("", "There were errors in the form - please fix them and try adding again.");
                return View(viewModel);
            }
        }

        [HttpGet]
        [Authorize]
        public IActionResult Edit(int id)
        {
            string actionName = GetControllerActionMethodTypeName();
            string auditLog = GetAuditLogSessionVar();
            AddToAuditLogSessionVar(auditLog, actionName);

            Studio studio = _albumsDbContext.Studios.Where(a => a.StudioId == id).FirstOrDefault();
            return View(studio);
        }

        [HttpPost]
        [Authorize]
        public IActionResult Edit(AddOrDeleteStudioViewModel viewModel)
        {
            string actionName = GetControllerActionMethodTypeName();
            string auditLog = GetAuditLogSessionVar();
            AddToAuditLogSessionVar(auditLog, actionName);

            if (ModelState.IsValid)
            {
             _albumsDbContext.Studios.Update(viewModel);
            _albumsDbContext.SaveChanges();

            return RedirectToAction("List", "Album");

            }
            else
            {
                ModelState.AddModelError("", "There were errors in the form - please fix them and try updating again.");
                return View(viewModel);
            }

        }

        private AlbumsDbContext _albumsDbContext;
        private string GetControllerActionMethodTypeName()
        {
            string controllerName = ControllerContext.RouteData.Values["controller"].ToString();
            string actionName = ControllerContext.RouteData.Values["action"].ToString();
            string methodType = ControllerContext.HttpContext.Request.Method;
            return $"Controller: {controllerName}/ Action: {actionName}/ Method type: {methodType}";
        }

        private string GetAuditLogSessionVar()
        {
            if (!HttpContext.Session.Keys.Contains(_auditLogSessionKeyName))
            {
                HttpContext.Session.SetString(_auditLogSessionKeyName, " ");
            }
            return HttpContext.Session.GetString(_auditLogSessionKeyName);
        }
        private void AddToAuditLogSessionVar(string auditLog, string actionName)
        {
            HttpContext.Session.SetString(_auditLogSessionKeyName, auditLog + "| " + actionName);
        }
    }
}
