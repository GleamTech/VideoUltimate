using System.Collections.Generic;

namespace GleamTech.VideoUltimateExamples.Mvc.CS.Models
{
    public class VideoInfoModel
    {
        public VideoInfoModel()
        {
            Properties = new Dictionary<string, string>();
            Metadata = new Dictionary<string, string>();
        }

        public Dictionary<string, string> Properties { get; }

        public Dictionary<string, string> Metadata { get; }
    }
}