using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(ShakuroMarketplaceNetMVC.Startup))]
namespace ShakuroMarketplaceNetMVC
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
