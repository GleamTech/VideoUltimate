﻿using System.Collections.Specialized;
using System.Drawing;
using System.Drawing.Imaging;
using System.Globalization;
using System.IO;
using System.Web;
using System.Web.Mvc;
using GleamTech.ExamplesCore;
using GleamTech.VideoUltimate;
using GleamTech.VideoUltimateExamples.Mvc.CS.Models;
using GleamTech.Web;

namespace GleamTech.VideoUltimateExamples.Mvc.CS.Controllers
{
    public partial class HomeController
    {
        public ActionResult Reading()
        {
            var model = new ReadingViewModel
            {
                PageCssBundle = PageCssBundle,
                PageJsBundle = PageJsBundle,
                ExampleFileSelector = new ExampleFileSelector
                {
                    ID = "exampleFileSelector",
                    InitialFile = "MP4 Video.mp4"
                }
            };

            var videoPath = model.ExampleFileSelector.SelectedFile;
            var fileInfo = new FileInfo(videoPath);

            model.FrameDownloaderUrl = ExamplesCoreConfiguration.GetDynamicDownloadUrl(
                FrameDownloaderHandlerName,
                new NameValueCollection
                {
                    {"videoPath", ExamplesCoreConfiguration.ProtectString(videoPath)},
                    {"version", fileInfo.LastWriteTimeUtc.Ticks + "-" + fileInfo.Length},
                    {"frameTime", "0"}
                });

            using (var videoFrameReader = new VideoFrameReader(videoPath))
            {
                model.TotalSeconds = ((int)videoFrameReader.Duration.TotalSeconds).ToString(CultureInfo.InvariantCulture);
            }

            return View(model);
        }

        public static void DownloadVideoFrame(HttpContext context)
        {
            var videoPath = ExamplesCoreConfiguration.UnprotectString(context.Request["videoPath"]);

            Bitmap bitmap = null;

            using (var videoFrameReader = new VideoFrameReader(videoPath))
            {
                var frameTime = int.Parse(context.Request["frameTime"]);

                if (frameTime > 0)
                    videoFrameReader.Seek(frameTime);

                //videoFrameReader.SetFrameWidth();

                if (videoFrameReader.Read())
                    bitmap = videoFrameReader.GetFrame();

                if (bitmap == null)
                    bitmap = GetErrorFrame(videoFrameReader.Width, videoFrameReader.Height, "Reading frame failed");
            }


            using (bitmap)
            using (var stream = new MemoryStream())
            {
                bitmap.Save(stream, ImageFormat.Jpeg);
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

        public static Bitmap GetErrorFrame(int width, int height, string error)
        {
            var bitmap = new Bitmap(width, height);

            using (var graphics = Graphics.FromImage(bitmap))
            {
                graphics.Clear(Color.Black);

                graphics.DrawString(
                    error,
                    new Font(FontFamily.GenericSansSerif, 12),
                    new SolidBrush(Color.White),
                    new RectangleF(0, 0, width, height),
                    new StringFormat
                    {
                        Alignment = StringAlignment.Center,
                        LineAlignment = StringAlignment.Center
                    });
            }

            return bitmap;
        }

        protected string FrameDownloaderHandlerName
        {
            get
            {
                if (frameDownloaderHandlerName == null)
                {
                    frameDownloaderHandlerName = "FrameDownloader";
                    ExamplesCoreConfiguration.RegisterDynamicDownloadHandler(frameDownloaderHandlerName, DownloadVideoFrame);
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
                        Server.MapPath("~/resources/nouislider.min.css"),
                        Server.MapPath("~/resources/table.css")
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
                        Server.MapPath("~/resources/nouislider.min.js"),
                        Server.MapPath("~/resources/timeslider.js")
                    };

                    ResourceManager.Register(pageJsBundle);
                }

                return pageJsBundle;
            }
        }
        private static ResourceBundle pageJsBundle;
    }
}
