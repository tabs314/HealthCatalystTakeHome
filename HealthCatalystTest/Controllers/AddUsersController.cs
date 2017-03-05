using HealthCatalystTest.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HealthCatalystTest.Controllers
{
    public class AddUsersController : Controller
    {
        // GET: AddUsers
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult RemoveAll()
        {
            using (var db = new UserInformationContext())
            {
               foreach(var id in db.UserInformation.Select(e => e.id))
                {
                    var entity = new UserInformationModel() { id = id };
                    db.UserInformation.Attach(entity);
                    db.UserInformation.Remove(entity);
                }
                db.SaveChanges();
            }

            return Json("ok");
        }

        [HttpPost]
        public ActionResult Random() {

            int totalUsers = Int32.Parse(this.Request.Form["number_random_users"]);

            using (var db = new UserInformationContext())
            {
                for (int i = 0; i < totalUsers; i++)
                {
                    UserInformationModel userInformation = new UserInformationModel();
                    userInformation.FirstName = "testFirstName"+i.ToString();
                    userInformation.LastName = "testLastName"+i.ToString();
                    userInformation.Address = "testAddress at "+i.ToString();
                    userInformation.Age = i;
                    userInformation.Interests = new List<string>();
                    userInformation.Interests.Add("stuff");
                    db.UserInformation.Add(userInformation);
                }
                db.SaveChanges();
            }

            return Json("ok");
        }
    }
}