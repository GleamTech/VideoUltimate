using System.Globalization;
using System.IO;
using GleamTech.AspNet;
using GleamTech.Caching;
using GleamTech.Drawing;
using GleamTech.Examples;
using GleamTech.IO;
using GleamTech.VideoUltimate;
using GleamTech.VideoUltimateExamples.AspNetCoreCS.Models;
using Microsoft.AspNetCore.Mvc;

namespace GleamTech.VideoUltimateExamples.AspNetCoreCS.Controllers
{
    public partial class HomeController
    {
        private static readonly ForwardSlashPath ThumbnailCachePath = "~/App_Data/ThumbnailCache";
        private static readonly FileCache ThumbnailCache = new FileCache(ThumbnailCachePath.ToString());

        private static void GetAndSaveThumbnail(string videoPath, Stream thumbnailStream)
        {
            using (var videoThumbnailer = new VideoThumbnailer(videoPath))
            using (var thumbnail = videoThumbnailer.GenerateThumbnail(300))
                thumbnail.Save(thumbnailStream, ImageFormat.Jpg);
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

        public IActionResult Overview()
        {
            var model = new OverviewViewModel
            {
                ExampleFileSelector = new ExampleFileSelector
                {
                    Id = "exampleFileSelector",
                    InitialFile = "MP4 Video.mp4"
                }
            };

            var videoPath = model.ExampleFileSelector.SelectedFile;
            var fileInfo = new FileInfo(videoPath);
            var thumbnailCacheKey = new FileCacheKey(new FileCacheSourceKey(fileInfo.Name, fileInfo.Length, fileInfo.LastWriteTimeUtc), "jpg");
            var cacheItem = ThumbnailCache.GetOrAdd(
                thumbnailCacheKey,
                thumbnailStream => GetAndSaveThumbnail(videoPath, thumbnailStream)
            );

            model.ThumbnailUrl = ExamplesConfiguration.GetDownloadUrl(
                Hosting.ResolvePhysicalPath(ThumbnailCachePath.Append(cacheItem.RelativeName)),
                thumbnailCacheKey.FullValue
            );

            model.VideoInfo = GetVideoInfo(videoPath);

            return View(model);
        }
    }
}
