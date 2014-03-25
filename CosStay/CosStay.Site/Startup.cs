using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(CosStay.Site.Startup))]
namespace CosStay.Site
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
