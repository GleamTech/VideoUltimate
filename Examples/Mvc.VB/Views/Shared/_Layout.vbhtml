<!DOCTYPE html>
<html>
<head>
    <title>@ViewBag.Title</title>
    @If (IsSectionDefined("VideoUltimateCss")) Then
        @RenderSection("VideoUltimateCss")
    End If
    @If (IsSectionDefined("VideoUltimateJs")) Then
        @RenderSection("VideoUltimateJs")
    End If
</head>
<body style="margin: 20px;">
    @RenderBody()
</body>
</html>
