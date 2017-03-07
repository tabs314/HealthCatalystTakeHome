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
            string firstName = this.Request.QueryString.Get("first_name_search");
            string lastName = this.Request.QueryString.Get("last_name_search");
            bool strictSearch = this.Request.QueryString.Get("strict_match") != null;
            int pageSize = 20;
            int pageNumber = 0;

            try
            {
                pageSize = int.Parse(this.Request.QueryString.Get("page_size"));
                pageNumber = int.Parse(this.Request.QueryString.Get("page_number"));
            }
            catch (Exception e) //Catch invalid numbers here
            {

            }

            if (!strictSearch)
            {
                //Normalize strings (in english) for cast
                firstName = firstName.ToLower();
                lastName = lastName.ToLower();
            }
            List<UserInformationModel> userList = new List<UserInformationModel>();

            using(var db = new UserInformationContext())
            {
                if (strictSearch)
                {
                    var users = (from u in db.UserInformation
                                 where u.FirstName.Equals(firstName) && u.LastName.Equals(lastName)
                                 orderby u.LastName
                                 select u).Skip(pageSize * pageNumber).Take(pageSize);

                    userList.AddRange(users.ToList());
                }
                else
                {
                    var users = (from u in db.UserInformation
                                where u.FirstName.ToLower().Contains(firstName) && u.LastName.ToLower().Contains(lastName)
                                orderby u.LastName
                                select u).Skip(pageSize * pageNumber).Take(pageSize);

                    userList.AddRange(users.ToList());
                }
                
            }

            return Json(userList, JsonRequestBehavior.AllowGet);
        }
    }
}