using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(DAW.Startup))]
namespace DAW
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
