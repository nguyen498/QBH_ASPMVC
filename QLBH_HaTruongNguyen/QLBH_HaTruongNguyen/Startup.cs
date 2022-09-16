using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(QLBH_HaTruongNguyen.Startup))]
namespace QLBH_HaTruongNguyen
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
