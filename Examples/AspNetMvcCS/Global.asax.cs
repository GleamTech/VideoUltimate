using System.IO;
using System.Web.Mvc;
using System.Web.Routing;
using GleamTech.AspNet;
using GleamTech.VideoUltimate;

namespace GleamTech.VideoUltimateExamples.AspNetMvcCS
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            RouteConfig.RegisterRoutes(RouteTable.Routes);

            var gleamTechConfig = Hosting.ResolvePhysicalPath("~/App_Data/GleamTech.config");
            if (File.Exists(gleamTechConfig))
                GleamTechConfiguration.Current.Load(gleamTechConfig);
            
            var videoUltimateConfig = Hosting.ResolvePhysicalPath("~/App_Data/VideoUltimate.config");
            if (File.Exists(videoUltimateConfig))
                VideoUltimateConfiguration.Current.Load(videoUltimateConfig);
        }
    }
}