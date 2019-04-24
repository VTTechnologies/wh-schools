using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Waad_Academy_School.Startup))]
namespace Waad_Academy_School
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
