using GleamTech.AspNet;
using GleamTech.Examples;

namespace GleamTech.VideoUltimateExamples.AspNetMvcCS.Models
{
    public class ReadingViewModel
    {
        public ResourceBundle PageCssBundle { get; set; }

        public ResourceBundle PageJsBundle { get; set; }

        public ExampleFileSelector ExampleFileSelector { get; set; }

        public string FrameDownloaderUrl { get; set; }

        public string TotalSeconds { get; set; }

    }
}