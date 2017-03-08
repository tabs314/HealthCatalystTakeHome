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

        private UserInformationContext context;

        public AddUsersController()
        {
            context = new UserInformationContext();
        }

        public AddUsersController(UserInformationContext context)
        {
            this.context = context;
        }

        // GET: AddUsers
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public JsonResult AddUser()
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
        public JsonResult RemoveAll()
        {
            foreach(var id in context.UserInformation.Select(e => e.id))
            {
                var entity = new UserInformationModel() { id = id };
                context.UserInformation.Attach(entity);
                context.UserInformation.Remove(entity);
            }

            context.SaveChanges();
         
            //Let's delete user picture too, for tidiness
            string pictureDirectory = Server.MapPath("~/Pictures/");

            DirectoryInfo dirInfo = new DirectoryInfo(pictureDirectory);

            foreach (FileInfo file in dirInfo.GetFiles())
            {
                file.Delete();
            }


            return Json("ok");
        }

        [HttpPost]
        public JsonResult Random() {

            int totalUsers = Int32.Parse(this.Request.Form["number_random_users"]);

          
            for (int i = 0; i < totalUsers; i++)
            {
                UserInformationModel userInformation = new UserInformationModel();
                userInformation.FirstName = "testFirstName"+i.ToString();
                userInformation.LastName = "testLastName"+i.ToString();
                userInformation.Address = "testAddress at "+i.ToString();
                userInformation.Age = i;
                userInformation.Interests = "testInterest " + i.ToString();
                context.UserInformation.Add(userInformation);
            }

            context.SaveChanges();
           
            return Json("ok");
        }
    }
}