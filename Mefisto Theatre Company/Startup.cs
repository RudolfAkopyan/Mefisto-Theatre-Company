using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Mefisto_Theatre_Company.Startup))]
namespace Mefisto_Theatre_Company
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
