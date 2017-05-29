Namespace Models
    Public Class VideoInfoModel
        Public Sub New()
            Properties = New Dictionary(Of String, String)()
            Metadata = New Dictionary(Of String, String)()
        End Sub

        Public ReadOnly Property Properties() As Dictionary(Of String, String)

        Public ReadOnly Property Metadata() As Dictionary(Of String, String)
    End Class
End NameSpace