using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace HealthCatalystTest.Models
{
    public class UserInformationModel
    {
        [Key]
        public int id { get; set; }

        [Display(Name = "First Name")]
        [Required(ErrorMessage = "Please enter the first name")]
        public string FirstName { get; set; }

        [Display(Name = "Last Name")]
        [Required(ErrorMessage = "Please enter the last name")]
        public string LastName { get; set; }

        [Display(Name = "Address")]
        public string Address { get; set; }

        [Display(Name = "Age")]
        [Required(ErrorMessage = "Please enter age")]
        public int Age { get; set; }

        [Display(Name = "Interests")]
        public string Interests { get; set; }

        public string PicturePath { get; set; }
    }
}