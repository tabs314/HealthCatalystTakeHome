using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;

namespace HealthCatalystTest.Models
{
    public class UserInformationContext : DbContext
    {
        public DbSet<UserInformationModel> UserInformation { get; set; }
    }
}