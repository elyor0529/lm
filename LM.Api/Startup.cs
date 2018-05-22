using log4net.Config;
using LM.Api;
using Microsoft.Owin;
using Owin;

[assembly: OwinStartup(typeof(Startup))]
[assembly: XmlConfigurator(ConfigFile = "log4net.config", Watch = true)]

namespace LM.Api
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {  
            ConfigureAuth(app);
        }
    }
}
