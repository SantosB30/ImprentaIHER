Imports System.Web.Optimization

Public Class MvcApplication
    Inherits System.Web.HttpApplication
    Dim accesos As String
    Dim usuario As String
    Dim mensaje As String

    Protected Sub Application_Start()
        AreaRegistration.RegisterAllAreas()
        FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters)
        RouteConfig.RegisterRoutes(RouteTable.Routes)
        BundleConfig.RegisterBundles(BundleTable.Bundles)
    End Sub

    Protected Sub Session_Start()
        Session("accesos") = Nothing
        Session("usuario") = ""
        Session("mensaje") = Nothing
    End Sub
End Class
