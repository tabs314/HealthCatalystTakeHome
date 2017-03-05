using HealthCatalystTest.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HealthCatalystTest.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            return View();
        }

        [HttpGet]
        public JsonResult Search()
        {
            string[] searchTokens = this.Request.QueryString.Get("search_criteria").Split(' ');
            List<UserInformationModel> userList = new List<UserInformationModel>();

            using(var db = new UserInformationContext())
            {
                foreach (string searchTerm in searchTokens)
                {

                    var users = from u in db.UserInformation
                                where u.FirstName.Contains(searchTerm) || u.LastName.Contains(searchTerm)
                                select u;

                    userList.AddRange(users.ToList());
                }
            }

            return Json(userList, JsonRequestBehavior.AllowGet);
        }
    }
}