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
            ViewBag.Message = "Your application description page.";

            return View();
        }

        [HttpPost]
        public ActionResult Search()
        {
            string[] searchTokens = this.Request.Form["search_criteria"].Split(' ');
            List<UserInformationModel> userList = new List<UserInformationModel>();

            using(var db = new UserInformationContext())
            {
                db.UserInformation.Add(new UserInformationModel { id =0, FirstName = "test", LastName = "address" });
                db.SaveChanges();
            
                foreach (string searchTerm in searchTokens)
                {

                    var users = from u in db.UserInformation
                                where u.FirstName == searchTerm || u.LastName == searchTerm
                                select u;

                    var theList = users.ToList();

                    userList.AddRange(theList);
                }
                
               
            }

            return Json(userList);
        }
    }
}