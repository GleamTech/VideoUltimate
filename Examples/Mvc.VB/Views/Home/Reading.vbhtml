@Imports GleamTech.Web
@Imports GleamTech.Web.Mvc
@ModelType GleamTech.VideoUltimateExamples.Mvc.VB.Models.ReadingViewModel

<!DOCTYPE html>

<html>
<head>
    <title>Reading video frames</title>
    <link href="@ResourceManager.GetUrl(Model.PageCssBundle)" rel="stylesheet" />
    <script src="@ResourceManager.GetUrl(Model.PageJsBundle)"></script>
</head>
<body style="margin: 20px;">

    @Html.RenderControl(Model.ExampleFileSelector)

    <table class="info" style="min-width: 300px;">
        <caption>Current Frame</caption>
        <tr>
            <td style="text-align: center;">
                <img id="videoFrame" src="@Model.FrameDownloaderUrl" style="max-width: 600px;" />
            </td>
        </tr>
        <tr>
            <td><div id="timeSlider"></div></td>
        </tr>
    </table>

    <script type="text/javascript">
        createTimeSlider("timeSlider", "videoFrame", @Model.TotalSeconds);
    </script>

</body>
</html>
