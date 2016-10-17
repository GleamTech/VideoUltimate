using System.Drawing.Imaging;
using System.Globalization;
using System.IO;
using System.Web.Mvc;
using GleamTech.Caching;
using GleamTech.ExamplesCore;
using GleamTech.VideoUltimate;
using GleamTech.VideoUltimateExamples.Mvc.CS.Models;
using GleamTech.Web;

namespace GleamTech.VideoUltimateExamples.Mvc.CS.Controllers
{
    public partial class HomeController
    {
        private static readonly DiskCache ThumbnailCache = new DiskCache { Path = HostingPathHelper.MapPath("~/App_Data/ThumbnailCache") };

        public ActionResult Overview()
        {
            var model = new OverviewViewModel
            {
                ExampleFileSelector = new ExampleFileSelector
                {
                    ID = "exampleFileSelector",
                    InitialFile = "MP4 Video.mp4"
                }
            };

            var videoPath = model.ExampleFileSelector.SelectedFile;
            var fileInfo = new FileInfo(videoPath);
            var thumbnailCacheKey = new DiskCacheKey(new DiskCacheSourceKey(fileInfo.Extension, fileInfo.Length, fileInfo.LastWriteTimeUtc), "jpg");

            model.ThumbnailUrl = ExamplesCoreConfiguration.GetDownloadUrl(
                ThumbnailCache.GetOrAdd(thumbnailCacheKey, thumbnailPath =>
                {
                    using (var videoThumbnailer = new VideoThumbnailer(videoPath))
                    using (var thumbnail = videoThumbnailer.GenerateThumbnail(300))
                        thumbnail.Save(thumbnailPath, ImageFormat.Jpeg);
                }).FilePath,
                thumbnailCacheKey.FullValue
            );

            using (var videoFrameReader = new VideoFrameReader(videoPath))
            {
                model.VideoInfo.Add("Duration", videoFrameReader.Duration.ToString());
                model.VideoInfo.Add("Width", videoFrameReader.Width.ToString());
                model.VideoInfo.Add("Height", videoFrameReader.Height.ToString());
                model.VideoInfo.Add("CodecName", videoFrameReader.CodecName);
                model.VideoInfo.Add("CodecDescription", videoFrameReader.CodecDescription);
                model.VideoInfo.Add("CodecTag", videoFrameReader.CodecTag);
                model.VideoInfo.Add("BitRate", videoFrameReader.BitRate.ToString());
                model.VideoInfo.Add("FrameRate", videoFrameReader.FrameRate.ToString(CultureInfo.InvariantCulture));

                foreach (var entry in videoFrameReader.Metadata)
                    model.VideoMetadata.Add(entry.Key, entry.Value);

                if (model.VideoMetadata.Count == 0)
                    model.VideoMetadata.Add("", "");
            }

            return View(model);
        }
    }
}
