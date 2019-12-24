using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(TheatreCMS.Startup))]
namespace TheatreCMS
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
