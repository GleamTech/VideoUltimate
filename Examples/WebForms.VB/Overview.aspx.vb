Imports System.Drawing.Imaging
Imports System.Globalization
Imports System.IO
Imports GleamTech.ExamplesCore
Imports GleamTech.Util
Imports GleamTech.VideoUltimate
Imports GleamTech.Web

Public Class OverviewPage
    Inherits System.Web.UI.Page

	Protected ThumbnailUrl As String
	Protected VideoInfo As New Dictionary(Of String, String)()
	Protected VideoMetadata As New Dictionary(Of String, String)()
	Private Shared ReadOnly ThumbnailCache As New DiskCache() With {
	    .Path = HostingPathHelper.MapPath("~/App_Data/ThumbnailCache")
	}

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
		Dim videoPath = exampleFileSelector.SelectedFile
		Dim fileInfo = New FileInfo(videoPath)
		Dim thumbnailCacheKey = ThumbnailCache.GenerateCacheKey(fileInfo.Extension, fileInfo.Length, fileInfo.LastWriteTimeUtc)

		ThumbnailUrl = ExamplesCoreConfiguration.GetDownloadUrl(
            ThumbnailCache.GetOrAdd(thumbnailCacheKey + ".jpg", Sub(thumbnailPath) 
		        Using videoThumbnailer = New VideoThumbnailer(videoPath)
		        Using thumbnail = videoThumbnailer.GenerateThumbnail(300)
                    thumbnail.Save(thumbnailPath, ImageFormat.Jpeg)
		        End Using
		        End Using
            End Sub), 
            thumbnailCacheKey)

		Using videoFrameReader = New VideoFrameReader(videoPath)
			VideoInfo.Add("Duration", videoFrameReader.Duration.ToString())
			VideoInfo.Add("Width", videoFrameReader.Width.ToString())
			VideoInfo.Add("Height", videoFrameReader.Height.ToString())
			VideoInfo.Add("CodecName", videoFrameReader.CodecName)
			VideoInfo.Add("CodecDescription", videoFrameReader.CodecDescription)
			VideoInfo.Add("CodecTag", videoFrameReader.CodecTag)
			VideoInfo.Add("BitRate", videoFrameReader.BitRate.ToString())
			VideoInfo.Add("FrameRate", videoFrameReader.FrameRate.ToString(CultureInfo.InvariantCulture))

			For Each entry In videoFrameReader.Metadata
				VideoMetadata.Add(entry.Key, entry.Value)
			Next

			If VideoMetadata.Count = 0 Then
				VideoMetadata.Add("", "")
			End If
		End Using
    End Sub

End Class