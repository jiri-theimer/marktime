Imports System.Web
Imports System.Web.Services
Imports System.Web.Script.Serialization

Public Class Datovka_Directories
    Implements System.Web.IHttpHandler

    Sub ProcessRequest(ByVal context As HttpContext) Implements IHttpHandler.ProcessRequest

        context.Response.ContentType = "application/json"

        Dim factory As New BL.Factory(Nothing, "mtservice")


        Dim c0 As New BO.DatovkaRoot
        Dim mqP28 As New BO.myQueryP28
        mqP28.Closed = BO.BooleanQueryMode.FalseQuery
        Dim lisP28 As IEnumerable(Of BO.p28Contact) = factory.p28ContactBL.GetList(mqP28)
        Dim mqP41 As New BO.myQueryP41
        mqP41.Closed = BO.BooleanQueryMode.FalseQuery
        ''mqP41.SpecificQuery = BO.myQueryP41_SpecificQuery.AllowedForRead
        Dim lisP41 As IEnumerable(Of BO.p41Project) = factory.p41ProjectBL.GetList(mqP41)

        For Each cP28 In lisP28
            Dim c1 As New BO.DatovkaItem
            c1.id = cP28.PID.ToString
            c1.name = cP28.p28Name
            For Each cP41 In lisP41.Where(Function(p) p.p28ID_Client = cP28.PID)
                Dim c2 As New BO.DatovkaItem
                c2.id = cP41.PID.ToString
                c2.name = cP41.PrefferedName
                If c1.sub Is Nothing Then c1.sub = New List(Of BO.DatovkaItem)
                c1.sub.Add(c2)
            Next
            If c0.sub Is Nothing Then c0.sub = New List(Of BO.DatovkaItem)
            c0.sub.Add(c1)


        Next

        Dim serializer As New JavaScriptSerializer()
        Dim serializedResult = serializer.Serialize(c0)


        context.Response.Write(serializedResult)

    End Sub

    ReadOnly Property IsReusable() As Boolean Implements IHttpHandler.IsReusable
        Get
            Return False
        End Get
    End Property

End Class