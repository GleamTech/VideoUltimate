<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Overview.aspx.cs" Inherits="GleamTech.VideoUltimateExamples.WebForms.CS.OverviewPage" %>
<%@ Register TagPrefix="GleamTech" Namespace="GleamTech.ExamplesCore" Assembly="GleamTech.ExamplesCore" %>

<!DOCTYPE html>

<html>
<head runat="server">
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
<body style="margin: 20px;">

    <GleamTech:ExampleFileSelector ID="exampleFileSelector" runat="server"
        InitialFile="MP4 Video.mp4" />
    
    <table class="info">
        <caption>Smart Thumbnail</caption>
        <tr>
            <td><img src="<%=ThumbnailUrl%>"/></td>
        </tr>
    </table>

    <table class="info">
        <caption>Video Info</caption>
        <% foreach (var kvp in VideoInfo) {%>
        <tr>
            <th><%=kvp.Key%></th>
            <td><%=kvp.Value%></td>
        </tr>
        <%}%>
    </table>
    
    <table class="info">
        <caption>Video Metadata</caption>
        <% foreach (var kvp in VideoMetadata) {%>
        <tr>
            <th><%=kvp.Key%></th>
            <td><%=kvp.Value%></td>
        </tr>
        <%}%>
    </table>

</body>
</html>
