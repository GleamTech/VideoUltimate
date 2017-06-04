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
        private static readonly DiskCache ThumbnailCache = new DiskCache(HostingPathHelper.MapPath("~/App_Data/ThumbnailCache").ToString());

        private static void GetAndSaveThumbnail(string videoPath, string thumbnailPath)
        {
            using (var videoThumbnailer = new VideoThumbnailer(videoPath))
            using (var thumbnail = videoThumbnailer.GenerateThumbnail(300))
                thumbnail.Save(thumbnailPath, ImageFormat.Jpeg);
        }

        private static VideoInfoModel GetVideoInfo(string videoPath)
        {
            var model = new VideoInfoModel();

            using (var videoFrameReader = new VideoFrameReader(videoPath))
            {
                model.Properties.Add("Duration", videoFrameReader.Duration.ToString());
                model.Properties.Add("Width", videoFrameReader.Width.ToString());
                model.Properties.Add("Height", videoFrameReader.Height.ToString());
                model.Properties.Add("CodecName", videoFrameReader.CodecName);
                model.Properties.Add("CodecDescription", videoFrameReader.CodecDescription);
                model.Properties.Add("CodecTag", videoFrameReader.CodecTag);
                model.Properties.Add("BitRate", videoFrameReader.BitRate.ToString());
                model.Properties.Add("FrameRate", videoFrameReader.FrameRate.ToString(CultureInfo.InvariantCulture));

                foreach (var entry in videoFrameReader.Metadata)
                    model.Metadata.Add(entry.Key, entry.Value);

                if (model.Metadata.Count == 0)
                    model.Metadata.Add("", "");
            }

            return model;
        }

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
            var thumbnailCacheKey = new DiskCacheKey(new DiskCacheSourceKey(fileInfo.Name, fileInfo.Length, fileInfo.LastWriteTimeUtc), "jpg");

            model.ThumbnailUrl = ExamplesCoreConfiguration.GetDownloadUrl(
                ThumbnailCache.GetOrAdd(
                    thumbnailCacheKey,
                    thumbnailPath => GetAndSaveThumbnail(videoPath, thumbnailPath)
                ).FilePath,
                thumbnailCacheKey.FullValue
            );

            model.VideoInfo = GetVideoInfo(videoPath);

            return View(model);
        }
    }
}
