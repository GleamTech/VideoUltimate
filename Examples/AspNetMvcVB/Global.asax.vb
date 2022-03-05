Imports System.Web.Mvc
Imports System.Web.Routing
Imports System.IO
Imports GleamTech.AspNet
Imports GleamTech.VideoUltimate

Public Class MvcApplication
    Inherits System.Web.HttpApplication

    Protected Sub Application_Start()
        AreaRegistration.RegisterAllAreas()
        RouteConfig.RegisterRoutes(RouteTable.Routes)

        Dim gleamTechConfig = Hosting.ResolvePhysicalPath("~/App_Data/GleamTech.config")
        If File.Exists(gleamTechConfig) Then
            GleamTechConfiguration.Current.Load(gleamTechConfig)
        End If

        Dim videoUltimateConfig = Hosting.ResolvePhysicalPath("~/App_Data/VideoUltimate.config")
        If File.Exists(videoUltimateConfig) Then
            VideoUltimateConfiguration.Current.Load(videoUltimateConfig)
        End If
    End Sub
End Class
