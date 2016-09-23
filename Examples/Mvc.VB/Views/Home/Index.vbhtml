@Imports GleamTech.ExamplesCore
@Imports GleamTech.Web.Mvc
@Code
    Dim exampleExplorer = New ExampleExplorer() With {
        .FullViewport = True,
        .NavigationTitle = "VideoUltimate Examples",
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
        .ExampleProjects = ExamplesCoreConfiguration.LoadExampleProjects(Server.MapPath("~/App_Data/ExampleProjects.json"))
    }
End Code
<!DOCTYPE html>

<html>
<head>
    <title>VideoUltimate Examples - ASP.NET MVC (VB)</title>

    @Html.RenderCss(exampleExplorer)
    @Html.RenderJs(exampleExplorer)

</head>
<body>
    @Html.RenderControl(exampleExplorer)
</body>
</html>
