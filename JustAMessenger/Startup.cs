using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(JustAMessenger.Startup))]
namespace JustAMessenger
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
            app.MapSignalR();
        }
    }
}
