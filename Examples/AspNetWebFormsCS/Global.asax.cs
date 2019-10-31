using System;
using System.IO;
using System.Web;
using GleamTech.AspNet;
using GleamTech.VideoUltimate;

namespace GleamTech.VideoUltimateExamples.AspNetWebFormsCS
{
    public class Global : HttpApplication
    {
        protected void Application_Start(object sender, EventArgs e)
        {
            var gleamTechConfig = Hosting.ResolvePhysicalPath("~/App_Data/GleamTech.config");
            if (File.Exists(gleamTechConfig))
                GleamTechConfiguration.Current.Load(gleamTechConfig);

            var videoUltimateConfig = Hosting.ResolvePhysicalPath("~/App_Data/VideoUltimate.config");
            if (File.Exists(videoUltimateConfig))
                VideoUltimateConfiguration.Current.Load(videoUltimateConfig);
        }
    }
}