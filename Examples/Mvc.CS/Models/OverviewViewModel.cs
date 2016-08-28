using System.Collections.Generic;
using GleamTech.ExamplesCore;

namespace GleamTech.VideoUltimateExamples.Mvc.CS.Models
{
    public class OverviewViewModel
    {
        public OverviewViewModel()
        {
            VideoInfo = new Dictionary<string, string>();
            VideoMetadata = new Dictionary<string, string>();
        }

        public ExampleFileSelector ExampleFileSelector { get; set; }

        public Dictionary<string, string> VideoInfo { get; set; }

        public Dictionary<string, string> VideoMetadata { get; set; }

        public string ThumbnailUrl { get; set; }
    }
}