Imports Telerik.Web.UI
'Imports Aspose.Words



Public Class pokus
    Inherits System.Web.UI.Page
    Protected WithEvents _MasterPage As Site
    'Private fileFormatProvider As IFormatProvider


    
    Private Sub pokus_Init(sender As Object, e As EventArgs) Handles Me.Init
        _MasterPage = Me.Master
    End Sub
    

    
    Private Sub pokus_Load(sender As Object, e As EventArgs) Handles Me.Load

        ''Dim u As String = Request.ServerVariables("HTTP_USER_AGENT")
        ''Dim b As New Regex("(android|bb\d+|meego).+mobile|avantgo|bada\/|blackberry|blazer|compal|elaine|fennec|hiptop|iemobile|ip(hone|od)|iris|kindle|lge |maemo|midp|mmp|mobile.+firefox|netfront|opera m(ob|in)i|palm( os)?|phone|p(ixi|re)\/|plucker|pocket|psp|series(4|6)0|symbian|treo|up\.(browser|link)|vodafone|wap|windows ce|xda|xiino", RegexOptions.IgnoreCase)
        ''Dim v As New Regex("1207|6310|6590|3gso|4thp|50[1-6]i|770s|802s|a wa|abac|ac(er|oo|s\-)|ai(ko|rn)|al(av|ca|co)|amoi|an(ex|ny|yw)|aptu|ar(ch|go)|as(te|us)|attw|au(di|\-m|r |s )|avan|be(ck|ll|nq)|bi(lb|rd)|bl(ac|az)|br(e|v)w|bumb|bw\-(n|u)|c55\/|capi|ccwa|cdm\-|cell|chtm|cldc|cmd\-|co(mp|nd)|craw|da(it|ll|ng)|dbte|dc\-s|devi|dica|dmob|do(c|p)o|ds(12|\-d)|el(49|ai)|em(l2|ul)|er(ic|k0)|esl8|ez([4-7]0|os|wa|ze)|fetc|fly(\-|_)|g1 u|g560|gene|gf\-5|g\-mo|go(\.w|od)|gr(ad|un)|haie|hcit|hd\-(m|p|t)|hei\-|hi(pt|ta)|hp( i|ip)|hs\-c|ht(c(\-| |_|a|g|p|s|t)|tp)|hu(aw|tc)|i\-(20|go|ma)|i230|iac( |\-|\/)|ibro|idea|ig01|ikom|im1k|inno|ipaq|iris|ja(t|v)a|jbro|jemu|jigs|kddi|keji|kgt( |\/)|klon|kpt |kwc\-|kyo(c|k)|le(no|xi)|lg( g|\/(k|l|u)|50|54|\-[a-w])|libw|lynx|m1\-w|m3ga|m50\/|ma(te|ui|xo)|mc(01|21|ca)|m\-cr|me(rc|ri)|mi(o8|oa|ts)|mmef|mo(01|02|bi|de|do|t(\-| |o|v)|zz)|mt(50|p1|v )|mwbp|mywa|n10[0-2]|n20[2-3]|n30(0|2)|n50(0|2|5)|n7(0(0|1)|10)|ne((c|m)\-|on|tf|wf|wg|wt)|nok(6|i)|nzph|o2im|op(ti|wv)|oran|owg1|p800|pan(a|d|t)|pdxg|pg(13|\-([1-8]|c))|phil|pire|pl(ay|uc)|pn\-2|po(ck|rt|se)|prox|psio|pt\-g|qa\-a|qc(07|12|21|32|60|\-[2-7]|i\-)|qtek|r380|r600|raks|rim9|ro(ve|zo)|s55\/|sa(ge|ma|mm|ms|ny|va)|sc(01|h\-|oo|p\-)|sdk\/|se(c(\-|0|1)|47|mc|nd|ri)|sgh\-|shar|sie(\-|m)|sk\-0|sl(45|id)|sm(al|ar|b3|it|t5)|so(ft|ny)|sp(01|h\-|v\-|v )|sy(01|mb)|t2(18|50)|t6(00|10|18)|ta(gt|lk)|tcl\-|tdg\-|tel(i|m)|tim\-|t\-mo|to(pl|sh)|ts(70|m\-|m3|m5)|tx\-9|up(\.b|g1|si)|utst|v400|v750|veri|vi(rg|te)|vk(40|5[0-3]|\-v)|vm40|voda|vulc|vx(52|53|60|61|70|80|81|83|85|98)|w3c(\-| )|webc|whit|wi(g |nc|nw)|wmlb|wonu|x700|yas\-|your|zeto|zte\-", RegexOptions.IgnoreCase)
        ''If b.IsMatch(u) Or v.IsMatch(Left(u, 4)) Then
        ''    'detekováno mobilní zařízení
        ''    Response.Redirect("http://detectmobilebrowser.com/mobile")
        ''End If
        If Not Page.IsPostBack Then
            SetupGrid()

            
        End If
    End Sub

    Private Sub SetupGrid()
        With grid1
            .ClearColumns()
            '.radGridOrig.HeaderStyle.HorizontalAlign = HorizontalAlign.Right
            .AllowMultiSelect = True
            .AllowCustomPaging = False
            .AddCheckboxSelector()
            '.AddSystemColumn(5)

            .DataKeyNames = "pid"
            .PageSize = 20
            .AddSystemColumn(5)

            .AllowFilteringByColumn = False

            



            .AddColumn("Person", "Osoba")
            .AddColumn("RowsCount", "Počet", BO.cfENUM.Numeric0)
            .AddColumn("p31Hours_Orig", "Hodiny", BO.cfENUM.Numeric)

            Dim gtv As New GridTableView(.radGridOrig)
            With gtv
                .HierarchyLoadMode = GridChildLoadMode.ServerOnDemand
                ''.RetainExpandStateOnRebind = False
                .Name = "level1"
                .AllowCustomPaging = True

                .AllowFilteringByColumn = False
                .AllowSorting = True
                .PageSize = 20
                .DataKeyNames = Split("pid", ",")
                .ClientDataKeyNames = Split("pid", ",")
                .ShowHeadersWhenNoRecords = False
                .ShowFooter = False

            End With
            .radGridOrig.MasterTableView.DetailTables.Add(gtv)



            .AddColumn("p31Date", "Datum", BO.cfENUM.DateOnly, , , , , , , , gtv)
            .AddColumn("Person", "Osoba", , , , , , , , , gtv)
            .AddColumn("p34Name", "Sešit", , , , , , , , , gtv)
            .AddColumn("p32Name", "Aktivita", , , , , , , , , gtv)
            .AddColumn("p31Hours_Orig", "Hodiny", BO.cfENUM.Numeric, , , , , True, , , gtv)
            .AddColumn("p31Amount_WithoutVat_Orig", "Částka", BO.cfENUM.Numeric, , , , , True, , , gtv)
            .AddColumn("p31Text", "Text", , , , , , , , , gtv)
        End With
    End Sub

   
    Private Sub cmdPokus_Click(sender As Object, e As EventArgs) Handles cmdPokus.Click



        
    End Sub

    Private Sub grid1_DetailTableDataBind(sender As Object, e As GridDetailTableDataBindEventArgs) Handles grid1.DetailTableDataBind
        Dim dataItem As GridDataItem = DirectCast(e.DetailTableView.ParentItem, GridDataItem)
        Dim mq As New BO.myQueryP31
        With mq
            .j02ID = dataItem.GetDataKeyValue("pid")
            .MG_PageSize = 20
            .MG_CurrentPageIndex = e.DetailTableView.CurrentPageIndex
            .MG_SortString = e.DetailTableView.SortExpressions.GetSortString()
        End With
        With e.DetailTableView
            .AllowCustomPaging = True
            .AllowSorting = True
            If .VirtualItemCount = 0 Then .VirtualItemCount = GetVirtualCount(mq)
            .DataSource = Master.Factory.p31WorksheetBL.GetList(mq)

        End With
        
    End Sub

    Private Sub grid1_ItemCommand(sender As Object, e As GridCommandEventArgs, strPID As String) Handles grid1.ItemCommand

        If e.CommandName = RadGrid.ExpandCollapseCommandName Then
            Dim item As GridItem
            For Each item In e.Item.OwnerTableView.Items
                If item.Expanded AndAlso Not item Is e.Item Then
                    item.Expanded = False
                End If
            Next item
        End If
        
    End Sub

    
    
  
    Private Sub grid1_NeedDataSource(sender As Object, e As GridNeedDataSourceEventArgs) Handles grid1.NeedDataSource
        If e.IsFromDetailTable Then
            Return
        End If

        
        Dim pr As New BO.PluginDbParameter("nic", 0)
        Dim prs As New List(Of BO.PluginDbParameter)
        prs.Add(pr)
        Dim dt As DataTable = Master.Factory.pluginBL.GetDataTable("select a.j02ID as pid,min(j02LastName) as Person,count(*) as RowsCount,sum(p31Hours_Orig) as p31Hours_Orig FROM p31worksheet a INNER JOIN j02Person b ON a.j02ID=b.j02id group by a.j02ID order by min(j02LastName)", prs)
        grid1.VirtualRowCount = dt.Rows.Count
        grid1.DataSourceDataTable = dt


        'grid1.DataSource = Master.Factory.p31WorksheetBL.GetList(mq)
    End Sub

    Private Function GetVirtualCount(mq As BO.myQueryP31) As Integer
        Dim cSum As BO.p31WorksheetSum = Master.Factory.p31WorksheetBL.LoadSumRow(mq, False, False)
        If Not cSum Is Nothing Then
            Return cSum.RowsCount

            'ViewState("footersum") = grid1.GenerateFooterItemString(cSum)
        Else
            'ViewState("footersum") = ""
            Return 0
        End If
    End Function
End Class