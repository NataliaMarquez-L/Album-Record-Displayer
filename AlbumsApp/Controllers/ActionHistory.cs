using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AlbumsApp.Controllers
{
    public class ActionHistory : Controller
    {
        

        public IActionResult Index()
        {
            List <string> actionLog= new List<string>();

            if (HttpContext.Session.Keys.Contains("auditLog"))
            {

                string auditLog = HttpContext.Session.GetString("auditLog");
                string[] strList = auditLog.Split(" | ");

                foreach(string strEntry in strList)
                {
                    actionLog.Add(strEntry);
                }
            }
            return View(actionLog);
        }
    }
}
