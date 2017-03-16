using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(DDWA.Startup))]
namespace DDWA
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {

        }
    }
}
