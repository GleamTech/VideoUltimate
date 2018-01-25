@Imports GleamTech.AspNet.Mvc
@Imports GleamTech.Examples
@Imports GleamTech.VideoUltimate
@Code
    Dim exampleExplorer = New ExampleExplorer() With {
        .DisplayMode = GleamTech.AspNet.UI.DisplayMode.Viewport,
        .NavigationTitle = "VideoUltimate Examples",
        .VersionTitle = "v" & VideoUltimateConfiguration.AssemblyInfo.FileVersion.ToString,
        .Examples = New ExampleBase() {
            New Example() With {
                .Title = "Overview",
                .Url = "home/Overview",
                .SourceFiles = New String() {"Views/Home/Overview.vbhtml", "Controllers/HomeController.Overview.vb"},
                .DescriptionFile = "Descriptions/Overview.html"
            },
            New Example() With {
                .Title = "Reading video frames",
                .Url = "home/Reading",
                .SourceFiles = New String() {"Views/Home/Reading.vbhtml", "Controllers/HomeController.Reading.vb"},
                .DescriptionFile = "Descriptions/Reading.html"
            }
        },
        .ExampleProjectName = "ASP.NET MVC (VB)",
        .ExampleProjects = ExamplesConfiguration.LoadExampleProjects(Server.MapPath("~/App_Data/ExampleProjects.json"))
    }
End Code
<!DOCTYPE html>

<html>
<head>
    <title>VideoUltimate Examples - ASP.NET MVC (VB)</title>

    @Me.RenderHead(exampleExplorer)
</head>
<body>
    @Me.RenderBody(exampleExplorer)
</body>
</html>
