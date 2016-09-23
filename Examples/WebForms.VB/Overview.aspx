﻿<%@ Page Language="vb" AutoEventWireup="true" CodeBehind="Overview.aspx.vb" Inherits="GleamTech.VideoUltimateExamples.WebForms.VB.OverviewPage" %>
<%@ Register TagPrefix="GleamTech" Namespace="GleamTech.ExamplesCore" Assembly="GleamTech.ExamplesCore" %>

<!DOCTYPE html>

<html>
<head runat="server">
    <title>Overview</title>
    <link href="<%=ExamplesCoreConfiguration.GetVersionedUrl("~/resources/table.css")%>" rel="stylesheet" />
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
        <%For Each kvp in VideoInfo%>
        <tr>
            <th><%=kvp.Key%></th>
            <td><%=kvp.Value%></td>
        </tr>
        <%Next%>
    </table>
    
    <table class="info">
        <caption>Video Metadata</caption>
        <%For Each kvp in VideoMetadata%>
        <tr>
            <th><%=kvp.Key%></th>
            <td><%=kvp.Value%></td>
        </tr>
        <%Next%>
    </table>

</body>
</html>
