Imports System.Drawing.Imaging
Imports System.Globalization
Imports System.IO
Imports GleamTech.Caching
Imports GleamTech.ExamplesCore
Imports GleamTech.VideoUltimate
Imports GleamTech.Web

Public Class OverviewPage
    Inherits Page

	Protected ThumbnailUrl As String
	Protected VideoInfo As VideoInfoModel
	Private Shared ReadOnly ThumbnailCache As New DiskCache() With {
	    .Path = HostingPathHelper.MapPath("~/App_Data/ThumbnailCache")
	}

	Private Shared Sub GetAndSaveThumbnail(videoPath As String, thumbnailPath As String)
	    Using videoThumbnailer = New VideoThumbnailer(videoPath)
	        Using thumbnail = videoThumbnailer.GenerateThumbnail(300)
	            thumbnail.Save(thumbnailPath, ImageFormat.Jpeg)
	        End Using
	    End Using
	End Sub

	Private Shared Function GetVideoInfo(videoPath As String) As VideoInfoModel
	    Dim model = New VideoInfoModel()

	    Using videoFrameReader = New VideoFrameReader(videoPath)
	        model.Properties.Add("Duration", videoFrameReader.Duration.ToString())
	        model.Properties.Add("Width", videoFrameReader.Width.ToString())
	        model.Properties.Add("Height", videoFrameReader.Height.ToString())
	        model.Properties.Add("CodecName", videoFrameReader.CodecName)
	        model.Properties.Add("CodecDescription", videoFrameReader.CodecDescription)
	        model.Properties.Add("CodecTag", videoFrameReader.CodecTag)
	        model.Properties.Add("BitRate", videoFrameReader.BitRate.ToString())
	        model.Properties.Add("FrameRate", videoFrameReader.FrameRate.ToString(CultureInfo.InvariantCulture))

	        For Each entry As Object In videoFrameReader.Metadata
	            model.Metadata.Add(entry.Key, entry.Value)
	        Next

	        If model.Metadata.Count = 0 Then
	            model.Metadata.Add("", "")
	        End If
	    End Using

	    Return model
	End Function

	Protected Sub Page_Load(sender As Object, e As EventArgs)
	    Dim videoPath = exampleFileSelector.SelectedFile
	    Dim fileInfo = New FileInfo(videoPath)
	    Dim thumbnailCacheKey = New DiskCacheKey(New DiskCacheSourceKey(fileInfo.Name, fileInfo.Length, fileInfo.LastWriteTimeUtc), "jpg")

        ThumbnailUrl = ExamplesCoreConfiguration.GetDownloadUrl(
            ThumbnailCache.GetOrAdd(
                thumbnailCacheKey, 
                Sub(thumbnailPath)
                    GetAndSaveThumbnail(videoPath, thumbnailPath)
                End Sub
            ).FilePath,
            thumbnailCacheKey.FullValue)

        VideoInfo = GetVideoInfo(videoPath)
	End Sub
End Class

Public Class VideoInfoModel
    Public Sub New()
        Properties = New Dictionary(Of String, String)()
        Metadata = New Dictionary(Of String, String)()
    End Sub

    Public ReadOnly Property Properties() As Dictionary(Of String, String)

    Public ReadOnly Property Metadata() As Dictionary(Of String, String)
End Class