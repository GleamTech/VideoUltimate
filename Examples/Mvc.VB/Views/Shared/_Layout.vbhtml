<!DOCTYPE html>
<html>
<head>
    <title>@ViewBag.Title</title>
    @If (IsSectionDefined("VideoUltimateHead")) Then
        @RenderSection("VideoUltimateHead")
    End If
</head>
<body style="margin: 20px;">
    @RenderBody()
</body>
</html>
