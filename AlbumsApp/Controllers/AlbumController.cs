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
    public class AlbumController : Controller
    {
        public  StudioController albumsDbContext;

        private static string _auditLogSessionKeyName = "auditLog";

        public AlbumController(AlbumsDbContext albumsDbContext)
        {
            _albumsDbContext = albumsDbContext;
        }

        [Route("/")]
        public IActionResult List()
        {
            List<Album> albums = _albumsDbContext.Albums.Include(a => a.Studio).ToList();
            return View(albums);
        }

      
      

        [HttpGet]
        [Authorize]
        public IActionResult Add()
        {
            string actionName = GetControllerActionMethodTypeName();
            string auditLog = GetAuditLogSessionVar();
            AddToAuditLogSessionVar(auditLog, actionName);

            var viewModel = new AddOrEditAlbumViewModel();
            viewModel.AllStudios = GetAllStudios();
            return View(viewModel);
        }

        [HttpPost]
        [Authorize]
        public IActionResult Add(AddOrEditAlbumViewModel viewModel)
        {
            string actionName = GetControllerActionMethodTypeName();
            string auditLog = GetAuditLogSessionVar();
            AddToAuditLogSessionVar(auditLog, actionName);
            if (ModelState.IsValid)
            {
                _albumsDbContext.Albums.Add(viewModel);
                _albumsDbContext.SaveChanges();

                return RedirectToAction("List", "Album");
            }
            else
            {
                ModelState.AddModelError("", "There were errors in the form - please fix them and try adding again.");
                viewModel.AllStudios = GetAllStudios();
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

            AddOrEditAlbumViewModel viewModel = new AddOrEditAlbumViewModel(_albumsDbContext.Albums.Include(a => a.Studio).Where(a => a.AlbumId == id).FirstOrDefault());
            viewModel.AllStudios = GetAllStudios();
            return View(viewModel);
        }


        [HttpPost]
        [Authorize]
        public IActionResult Edit(AddOrEditAlbumViewModel viewModel)
        {
            string actionName = GetControllerActionMethodTypeName();
            string auditLog = GetAuditLogSessionVar();
            AddToAuditLogSessionVar(auditLog, actionName);

            if (ModelState.IsValid)
            {
                _albumsDbContext.Albums.Update(viewModel);
                _albumsDbContext.SaveChanges();

                return RedirectToAction("List", "Album");
            }
            else
            {
                ModelState.AddModelError("", "There were errors in the form - please fix them and try updating again.");
                viewModel.AllStudios = GetAllStudios();
                return View(viewModel);
            }
        }

        [HttpGet]
        public IActionResult DeleteConfirmation(int id)
        {
            string actionName = GetControllerActionMethodTypeName();
            string auditLog = GetAuditLogSessionVar();
            AddToAuditLogSessionVar(auditLog, actionName);

            Album album = _albumsDbContext.Albums.Include(a => a.Studio).Where(a => a.AlbumId == id).FirstOrDefault();
            return View(album);
        }

        [HttpPost]
        public IActionResult Delete(int id)
        {
            string actionName = GetControllerActionMethodTypeName();
            string auditLog = GetAuditLogSessionVar();
            AddToAuditLogSessionVar(auditLog, actionName);

            Album album = _albumsDbContext.Albums.Include(a => a.Studio).Where(a => a.AlbumId == id).FirstOrDefault();
            _albumsDbContext.Albums.Remove(album);
            _albumsDbContext.SaveChanges();

            return RedirectToAction("List", "Album");
        }

        [HttpGet("/Error")]
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        private List<Studio> GetAllStudios()
        {
            return _albumsDbContext.Studios.OrderBy(s => s.Name).ToList();
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
