using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(HealthCatalystTest.Startup))]
namespace HealthCatalystTest
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
