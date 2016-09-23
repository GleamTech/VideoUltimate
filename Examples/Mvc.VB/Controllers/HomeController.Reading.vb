Imports System.Drawing
Imports System.Drawing.Imaging
Imports System.Globalization
Imports System.IO
Imports GleamTech.ExamplesCore
Imports GleamTech.VideoUltimate
Imports GleamTech.VideoUltimateExamples.Mvc.VB.Models
Imports GleamTech.Web

Namespace Controllers
    Partial Public Class HomeController
        Inherits Controller

        Function Reading() As ActionResult

            Dim model = New ReadingViewModel() With {
                .PageCssBundle = PageCssBundle,
                .PageJsBundle = PageJsBundle,
	            .ExampleFileSelector = New ExampleFileSelector() With {
		            .ID = "exampleFileSelector",
		            .InitialFile = "MP4 Video.mp4"
	            }
            }

		    Dim videoPath = model.ExampleFileSelector.SelectedFile
		    Dim fileInfo = New FileInfo(videoPath)

		    model.FrameDownloaderUrl = ExamplesCoreConfiguration.GetDynamicDownloadUrl(
                FrameDownloaderHandlerName, 
                New NameValueCollection() From {
			        {"videoPath", ExamplesCoreConfiguration.ProtectString(videoPath)},
			        {"version", fileInfo.LastWriteTimeUtc.Ticks.ToString() + "-" + fileInfo.Length.ToString()},
			        {"frameTime", "0"}
		        })

		    Using videoFrameReader = New VideoFrameReader(videoPath)
			    model.TotalSeconds = CInt(Int(videoFrameReader.Duration.TotalSeconds)).ToString(CultureInfo.InvariantCulture)
		    End Using

            Return View(model)
        End Function

	Public Shared Sub DownloadVideoFrame(context As HttpContext)
		Dim videoPath = ExamplesCoreConfiguration.UnprotectString(context.Request("videoPath"))

		Dim bitmap As Bitmap = Nothing

		Using videoFrameReader = New VideoFrameReader(videoPath)
			Dim frameTime = Integer.Parse(context.Request("frameTime"))

			If frameTime > 0 Then
				videoFrameReader.Seek(frameTime)
			End If

			'videoFrameReader.SetFrameWidth();

			If videoFrameReader.Read() Then
				bitmap = videoFrameReader.GetFrame()
			End If

			If bitmap Is Nothing Then
				bitmap = GetErrorFrame(videoFrameReader.Width, videoFrameReader.Height, "Reading frame failed")
			End If
		End Using


		Using bitmap
			Using stream = New MemoryStream()
				bitmap.Save(stream, ImageFormat.Jpeg)
				stream.Position = 0

				Dim fileResponse = New FileResponse(context)
				fileResponse.Transmit(
                    stream, 
                    "frame.jpg", 
                    System.IO.File.GetLastWriteTimeUtc(videoPath), 
                    stream.Length, 
                    neverExpires := True)
			End Using
		End Using
	End Sub

	Public Shared Function GetErrorFrame(width As Integer, height As Integer, [error] As String) As Bitmap
		Dim bitmap = New Bitmap(width, height)

		Using _graphics = Graphics.FromImage(bitmap)
			_graphics.Clear(Color.Black)

			_graphics.DrawString(
                [error], 
                New Font(FontFamily.GenericSansSerif, 12), 
                New SolidBrush(Color.White), 
                New RectangleF(0, 0, width, height), 
                New StringFormat() With { 
				    .Alignment = StringAlignment.Center, 
				    .LineAlignment = StringAlignment.Center 
			    })
		End Using

		Return bitmap
	End Function

    Protected ReadOnly Property FrameDownloaderHandlerName As String
	    Get
		    If _frameDownloaderHandlerName Is Nothing Then
			    _frameDownloaderHandlerName = "FrameDownloader"
			    ExamplesCoreConfiguration.RegisterDynamicDownloadHandler(_frameDownloaderHandlerName, AddressOf DownloadVideoFrame)
		    End If

		    Return _frameDownloaderHandlerName
	    End Get
    End Property
    Private Shared _frameDownloaderHandlerName As String

	Protected ReadOnly Property PageCssBundle As ResourceBundle
		Get
			If _pageCssBundle Is Nothing Then
				_pageCssBundle = New ResourceBundle("page.css") From {
					Server.MapPath("~/resources/nouislider.min.css"), 
                    Server.MapPath("~/resources/table.css")
				}

				ResourceManager.Register(_pageCssBundle)
			End If

			Return _pageCssBundle
		End Get
	End Property
	Private Shared _pageCssBundle As ResourceBundle

	Protected ReadOnly Property PageJsBundle As ResourceBundle
		Get
			If _pageJsBundle Is Nothing Then
				_pageJsBundle = New ResourceBundle("page.js") From {
					Server.MapPath("~/resources/nouislider.min.js"),
					Server.MapPath("~/resources/timeslider.js")
				}

				ResourceManager.Register(_pageJsBundle)
			End If

			Return _pageJsBundle
		End Get
	End Property
	Private Shared _pageJsBundle As ResourceBundle

    End Class
End Namespace
