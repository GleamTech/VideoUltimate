Imports System.Drawing.Imaging
Imports System.Globalization
Imports System.IO
Imports GleamTech.ExamplesCore
Imports GleamTech.Util
Imports GleamTech.VideoUltimate
Imports GleamTech.VideoUltimateExamples.Mvc.VB.Models
Imports GleamTech.Web

Namespace Controllers
    Partial Public Class HomeController
        Inherits Controller

	    Private Shared ReadOnly ThumbnailCache As New DiskCache() With {
	        .Path = HostingPathHelper.MapPath("~/App_Data/ThumbnailCache")
	    }

        Function Overview() As ActionResult

            Dim model = New OverviewViewModel() With { _
	            .ExampleFileSelector = New ExampleFileSelector() With {
		            .ID = "exampleFileSelector",
		            .InitialFile = "MP4 Video.mp4"
	            }
            }

		    Dim videoPath = model.ExampleFileSelector.SelectedFile
		    Dim fileInfo = New FileInfo(videoPath)
		    Dim thumbnailCacheKey = ThumbnailCache.GenerateCacheKey(fileInfo.Extension, fileInfo.Length, fileInfo.LastWriteTimeUtc)

		    model.ThumbnailUrl = ExamplesCoreConfiguration.GetDownloadUrl(
                ThumbnailCache.GetOrAdd(thumbnailCacheKey + ".jpg", Sub(thumbnailPath) 
		            Using videoThumbnailer = New VideoThumbnailer(videoPath)
		            Using thumbnail = videoThumbnailer.GenerateThumbnail(300)
                        thumbnail.Save(thumbnailPath, ImageFormat.Jpeg)
		            End Using
		            End Using
                End Sub).FilePath, 
                thumbnailCacheKey)

		    Using videoFrameReader = New VideoFrameReader(videoPath)
			    model.VideoInfo.Add("Duration", videoFrameReader.Duration.ToString())
			    model.VideoInfo.Add("Width", videoFrameReader.Width.ToString())
			    model.VideoInfo.Add("Height", videoFrameReader.Height.ToString())
			    model.VideoInfo.Add("CodecName", videoFrameReader.CodecName)
			    model.VideoInfo.Add("CodecDescription", videoFrameReader.CodecDescription)
			    model.VideoInfo.Add("CodecTag", videoFrameReader.CodecTag)
			    model.VideoInfo.Add("BitRate", videoFrameReader.BitRate.ToString())
			    model.VideoInfo.Add("FrameRate", videoFrameReader.FrameRate.ToString(CultureInfo.InvariantCulture))

			    For Each entry In videoFrameReader.Metadata
				    model.VideoMetadata.Add(entry.Key, entry.Value)
			    Next

			    If model.VideoMetadata.Count = 0 Then
				    model.VideoMetadata.Add("", "")
			    End If
		    End Using

            Return View(model)
        End Function

    End Class
End Namespace
