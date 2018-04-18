using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Mixr.Startup))]
namespace Mixr
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
