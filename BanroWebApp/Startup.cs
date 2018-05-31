using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(BanroWebApp.Startup))]
namespace BanroWebApp
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
