using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(TaskilyWeb.Startup))]
namespace TaskilyWeb
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
