using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.Globalization;
using System.IO;
using System.Web.UI;
using GleamTech.ExamplesCore;
using GleamTech.Util;
using GleamTech.VideoUltimate;
using GleamTech.Web;

namespace GleamTech.VideoUltimateExamples.WebForms.CS
{
    public partial class OverviewPage : Page
    {
        protected string ThumbnailUrl;
        protected Dictionary<string, string> VideoInfo = new Dictionary<string, string>();
        protected Dictionary<string, string> VideoMetadata = new Dictionary<string, string>();
        private static readonly DiskCache ThumbnailCache = new DiskCache { Path = HostingPathHelper.MapPath("~/App_Data/ThumbnailCache")};

        protected void Page_Load(object sender, EventArgs e)
        {
            var videoPath = exampleFileSelector.SelectedFile;
            var fileInfo = new FileInfo(videoPath);
            var thumbnailCacheKey = ThumbnailCache.GenerateCacheKey(fileInfo.Extension, fileInfo.Length, fileInfo.LastWriteTimeUtc);

            ThumbnailUrl = ExamplesCoreConfiguration.GetDownloadUrl(
                ThumbnailCache.GetOrAdd(thumbnailCacheKey + ".jpg", thumbnailPath =>
                {
                    using (var videoThumbnailer = new VideoThumbnailer(videoPath))
                    using (var thumbnail = videoThumbnailer.GenerateThumbnail(300))
                        thumbnail.Save(thumbnailPath, ImageFormat.Jpeg);
                }).FilePath,
                thumbnailCacheKey
            );

            using (var videoFrameReader = new VideoFrameReader(videoPath))
            {
                VideoInfo.Add("Duration", videoFrameReader.Duration.ToString());
                VideoInfo.Add("Width", videoFrameReader.Width.ToString());
                VideoInfo.Add("Height", videoFrameReader.Height.ToString());
                VideoInfo.Add("CodecName", videoFrameReader.CodecName);
                VideoInfo.Add("CodecDescription", videoFrameReader.CodecDescription);
                VideoInfo.Add("CodecTag", videoFrameReader.CodecTag);
                VideoInfo.Add("BitRate", videoFrameReader.BitRate.ToString());
                VideoInfo.Add("FrameRate", videoFrameReader.FrameRate.ToString(CultureInfo.InvariantCulture));

                foreach (var entry in videoFrameReader.Metadata)
                    VideoMetadata.Add(entry.Key, entry.Value);

                if (VideoMetadata.Count == 0)
                    VideoMetadata.Add("", "");
            }
        }
    }
}