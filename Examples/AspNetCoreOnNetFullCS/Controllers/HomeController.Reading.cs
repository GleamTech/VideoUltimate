using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using GleamTech.AspNet;
using GleamTech.Drawing;
using GleamTech.Examples;
using GleamTech.VideoUltimate;
using GleamTech.VideoUltimateExamples.AspNetCoreOnNetFullCS.Models;
using Microsoft.AspNetCore.Mvc;

namespace GleamTech.VideoUltimateExamples.AspNetCoreOnNetFullCS.Controllers
{
    public partial class HomeController
    {
        public IActionResult Reading()
        {
            var model = new ReadingViewModel
            {
                PageCssBundle = PageCssBundle,
                PageJsBundle = PageJsBundle,
                ExampleFileSelector = new ExampleFileSelector
                {
                    Id = "exampleFileSelector",
                    InitialFile = "MP4 Video.mp4"
                }
            };

            var videoPath = model.ExampleFileSelector.SelectedFile;
            var fileInfo = new FileInfo(videoPath);

            model.FrameDownloaderUrl = ExamplesConfiguration.GetDynamicDownloadUrl(
                FrameDownloaderHandlerName,
                new Dictionary<string, string>
                {
                    {"videoPath", ExamplesConfiguration.ProtectString(videoPath)},
                    {"version", fileInfo.LastWriteTimeUtc.Ticks + "-" + fileInfo.Length},
                    {"frameTime", "0"}
                });


            var duration = GetDuration(videoPath);
            model.TotalSeconds = ((int)duration.TotalSeconds).ToString(CultureInfo.InvariantCulture);

            return View(model);
        }

        public static TimeSpan GetDuration(string videoPath)
        {
            using (var videoFrameReader = new VideoFrameReader(videoPath))
            {
                return videoFrameReader.Duration;
            }
        }

        public static Image GetFrame(string videoPath, double frameTime)
        {
            using (var videoFrameReader = new VideoFrameReader(videoPath))
            {
                if (frameTime > 0)
                    videoFrameReader.Seek(frameTime);

                //videoFrameReader.SetFrameWidth(300);

                if (videoFrameReader.Read())
                    return videoFrameReader.GetFrame();

                return GetErrorFrame(videoFrameReader.Width, videoFrameReader.Height, "Reading frame failed");
            }
        }

        public static Image GetErrorFrame(int width, int height, string error)
        {
	        var image = new Image(width, height, Color.Black);

	        image.DrawTextOverlay(
		        error,
		        new Font("Arial", FontStyle.Bold, 0), //0 to use AutoFontSize
		        new Point(0, 0),
		        new TextOverlayOptions
		        {
			        AutoFontSize = 0.1f
		        }
	        );

	        return image;
        }

		public static void DownloadVideoFrame(IHttpContext context)
        {
            var videoPath = ExamplesConfiguration.UnprotectString(context.Request["videoPath"]);
            var frameTime = int.Parse(context.Request["frameTime"]);

            using (var image = GetFrame(videoPath, frameTime))
            using (var stream = new MemoryStream())
            {
                image.Save(stream, ImageFormat.Jpg);
                stream.Position = 0;

                var fileResponse = new FileResponse(context);
                fileResponse.Transmit(
                    stream,
                    "frame.jpg",
                    System.IO.File.GetLastWriteTimeUtc(videoPath),
                    stream.Length,
                    neverExpires: true);
            }
        }

        protected string FrameDownloaderHandlerName
        {
            get
            {
                if (frameDownloaderHandlerName == null)
                {
                    frameDownloaderHandlerName = "FrameDownloader";
                    ExamplesConfiguration.RegisterDynamicDownloadHandler(frameDownloaderHandlerName, DownloadVideoFrame);
                }

                return frameDownloaderHandlerName;
            }
        }
        private static string frameDownloaderHandlerName;

        protected ResourceBundle PageCssBundle
        {
            get
            {
                if (pageCssBundle == null)
                {
                    pageCssBundle = new ResourceBundle("page.css")
                    {
                        Hosting.ResolvePhysicalPath("~/resources/nouislider.min.css"),
                        Hosting.ResolvePhysicalPath("~/resources/table.css")
                    };

                    ResourceManager.Register(pageCssBundle);
                }

                return pageCssBundle;
            }
        }
        private static ResourceBundle pageCssBundle;

        protected ResourceBundle PageJsBundle
        {
            get
            {
                if (pageJsBundle == null)
                {
                    pageJsBundle = new ResourceBundle("page.js")
                    {
                        Hosting.ResolvePhysicalPath("~/resources/nouislider.min.js"),
                        Hosting.ResolvePhysicalPath("~/resources/timeslider.js")
                    };

                    ResourceManager.Register(pageJsBundle);
                }

                return pageJsBundle;
            }
        }
        private static ResourceBundle pageJsBundle;
    }
}
