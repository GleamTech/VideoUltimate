Imports GleamTech.ExamplesCore

Namespace Models

    Public Class OverviewViewModel
        Public Sub New()
            VideoInfo = New Dictionary(Of String, String)()
            VideoMetadata = New Dictionary(Of String, String)()
        End Sub

        Public Property ExampleFileSelector As ExampleFileSelector

        Public Property VideoInfo As Dictionary(Of String, String)

        Public Property VideoMetadata As Dictionary(Of String, String)

        Public Property ThumbnailUrl As String
    End Class
End NameSpace