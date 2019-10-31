using GleamTech.Examples;

namespace GleamTech.VideoUltimateExamples.AspNetCoreCS.Models
{
    public class OverviewViewModel
    {
        public ExampleFileSelector ExampleFileSelector { get; set; }

        public VideoInfoModel VideoInfo { get; set; }

        public string ThumbnailUrl { get; set; }
    }
}