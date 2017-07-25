Imports System.Web.Routing
Imports Microsoft.AspNet.FriendlyUrls

Public Module RouteConfig
    Sub RegisterRoutes(ByVal routes As RouteCollection)
        routes.EnableFriendlyUrls()

        routes.MapPageRoute("DatovkaUH", "datovka/upload_hierarchy", "~/datovka_upload_hierarchy.aspx", False)
        routes.MapPageRoute("DatovkaSI", "datovka/service_info", "~/datovka_service_info.aspx", False)
        routes.MapPageRoute("DatovkaUF", "datovka/upload_file", "~/datovka_upload_file.aspx", False)
    End Sub
End Module
