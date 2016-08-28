@Imports GleamTech.Web.Mvc
@ModelType GleamTech.VideoUltimateExamples.Mvc.VB.Models.OverviewViewModel

<!DOCTYPE html>

<html>
<head>
    <title>Overview</title>
    <style>
        table.info {
            background-color: white;
            border-collapse: collapse;
            margin-bottom: 20px;
            margin-left: 20px;
            float: left;
            min-width: 100px;
        }

        table.info caption {
            background-color: dodgerblue;
            color: white;
            font-weight: bold;
            padding: 2px;
        }

        table.info th, 
        table.info td {
            text-align: left;
            padding: 8px;
            border: 1px solid #ddd;
        }

        table.info tr:nth-child(even){background-color: #f2f2f2}
    </style>
</head>
<body style="margin: 20px;" >

    @Html.RenderControl(Model.ExampleFileSelector)

    <table class="info">
        <caption>Smart Thumbnail</caption>
        <tr>
            <td><img src="@Model.ThumbnailUrl" /></td>
        </tr>
    </table>

    <table class="info">
        <caption>Video Info</caption>
        @For Each kvp in Model.VideoInfo
        @<tr>
            <th>@kvp.Key</th>
            <td>@kvp.Value</td>
        </tr>
        Next
    </table>

    <table class="info">
        <caption>Video Metadata</caption>
        @For Each kvp in Model.VideoMetadata
        @<tr>
            <th>@kvp.Key</th>
            <td>@kvp.Value</td>
        </tr>
        Next
    </table>

</body>
</html>
