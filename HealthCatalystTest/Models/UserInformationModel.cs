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

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Address { get; set; }

        public int Age { get; set; }

        public List<string> Interests { get; set; }

        public byte[] picture { get; set; }
    }
}