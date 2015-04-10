using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(APTEventAssignment.Startup))]
namespace APTEventAssignment
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
