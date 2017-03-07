using HealthCatalystTest.Models;
using System;
using System.Collections.Generic;
using System.IO;
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
        public ActionResult AddUser()
        {

            UserInformationModel userInformationModel = new UserInformationModel()
            {
                FirstName = this.Request.Form["first_name"],
                LastName = this.Request.Form["last_name"],
                Age = int.Parse(this.Request.Form["age"]),
                Address = this.Request.Form["address"],
                Interests = this.Request.Form["interests"]
            };

            foreach(string file in this.Request.Files)
            {
                HttpPostedFileBase hpf = Request.Files[file] as HttpPostedFileBase;
                if (hpf.ContentLength == 0) continue;

                string savedFileName = Path.Combine("~/Pictures/", Path.GetFileName(hpf.FileName));
                hpf.SaveAs(Server.MapPath(savedFileName));

                userInformationModel.PicturePath = Path.Combine("/Pictures/", Path.GetFileName(hpf.FileName));
            }


            using (var db = new UserInformationContext())
            {
                db.UserInformation.Add(userInformationModel);
                db.SaveChanges();
            }

            return Json("ok");
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
                    userInformation.Interests = "testInterest " + i.ToString();
                    db.UserInformation.Add(userInformation);
                }
                db.SaveChanges();
            }

            return Json("ok");
        }
    }
}