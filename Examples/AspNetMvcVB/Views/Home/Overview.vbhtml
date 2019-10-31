﻿@Imports GleamTech.AspNet.Mvc
@Imports GleamTech.Examples
@ModelType GleamTech.VideoUltimateExamples.AspNetMvcVB.Models.OverviewViewModel

<!DOCTYPE html>

<html>
<head>
    <title>Overview</title>
    <link href="@ExamplesConfiguration.GetVersionedUrl("~/resources/table.css")" rel="stylesheet" />
</head>
<body style="margin: 20px;">

    @Me.RenderBody(Model.ExampleFileSelector)

    <table class="info">
        <caption>Smart Thumbnail</caption>
        <tr>
            <td><img src="@Model.ThumbnailUrl" /></td>
        </tr>
    </table>

    <table class="info">
        <caption>Video Properties</caption>
        @For Each kvp In Model.VideoInfo.Properties
            @<tr>
                <th>@kvp.Key</th>
                <td>@kvp.Value</td>
            </tr>
        Next
    </table>

    <table class="info">
        <caption>Video Metadata</caption>
        @For Each kvp In Model.VideoInfo.Metadata
            @<tr>
                <th>@kvp.Key</th>
                <td>@kvp.Value</td>
            </tr>
        Next
    </table>

</body>
</html>
