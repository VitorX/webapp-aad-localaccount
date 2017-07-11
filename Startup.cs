using Microsoft.Owin;
using Owin;
using System.Web.Services.Description;

[assembly: OwinStartupAttribute(typeof(WebApplication1.Startup))]
namespace WebApplication1
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }

        ////For more information on how to configure your application,
        ////visit http://go.microsoft.com/fwlink/?LinkID=398940
        //public void ConfigureServices(ServiceCollection services)
        //{

        //}

        //public void Configure(IApplicationBuilder app)
        //{

        //}
    }
}

