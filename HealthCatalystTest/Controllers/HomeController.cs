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
            string[] searchCriteria = this.Request.QueryString.Get("search_criteria").Split(' ');
            bool strictSearch = this.Request.QueryString.Get("strict_match") != null;
            int pageSize = 20;
            int pageNumber = 0;

            try
            {
                pageSize = Math.Max(int.Parse(this.Request.QueryString.Get("page_size")), 0); //Always positive page size
                pageNumber = Math.Max(int.Parse(this.Request.QueryString.Get("page_number")), 0); //Always positive page number
            }
            catch (Exception e) //Catch invalid numbers here - defaults should hold so continue
            {

            }

            string firstName = "", lastName = "";
            List<string> normalizedSearchCriteria = new List<string>();
            if (strictSearch)
            {
                firstName = searchCriteria[0].Trim();
                if (searchCriteria.Length >= 2)
                {
                    lastName = searchCriteria[1].Trim();
                }
            }
            else
            {
                //We'll normalize to lower case
                searchCriteria.ToList().ForEach(x => {
                    if (!String.IsNullOrWhiteSpace(x))
                        normalizedSearchCriteria.Add(x.ToLower());
                    });
            }
            List<UserInformationModel> userList = new List<UserInformationModel>();

            using(var db = new UserInformationContext())
            {
                if (strictSearch)
                {
                    if (!String.IsNullOrWhiteSpace(lastName))
                    {
                        var users = (from u in db.UserInformation
                                     where u.FirstName.Equals(firstName) && u.LastName.Equals(lastName)
                                     orderby u.LastName
                                     select u).Skip(pageSize * pageNumber).Take(pageSize);

                        userList.AddRange(users.ToList());
                    }
                    else
                    {
                        //In this case, consider "first name" across first and last name
                        var users = (from u in db.UserInformation
                                     where u.FirstName.Equals(firstName) || u.LastName.Equals(firstName)
                                     orderby u.LastName
                                     select u).Skip(pageSize * pageNumber).Take(pageSize);

                        userList.AddRange(users.ToList());
                    }
                }
                else
                {
                    Dictionary<int, UserInformationModel> uniqueUsers = new Dictionary<int, UserInformationModel>();

                    foreach(string searchTerm in normalizedSearchCriteria)
                    {
                       
                        var users = (from u in db.UserInformation
                                     where u.FirstName.ToLower().Contains(searchTerm) || u.LastName.ToLower().Contains(searchTerm)
                                     select u);
                        users.ToList().ForEach(u => uniqueUsers[u.id] = u);
                    }

                    userList = uniqueUsers.Values.OrderBy(u => u.LastName).ToList();

                    userList = userList.Skip(pageNumber * pageSize).ToList();

                    userList = userList.Take(pageSize).ToList();

                    //userList = uniqueUsers.Values.OrderBy(u => u.LastName).Skip(pageNumber * pageSize).Take(pageSize).ToList();
                }
                
            }

            return Json(userList, JsonRequestBehavior.AllowGet);
        }
    }
}