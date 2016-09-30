<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="datagrid.ascx.vb" Inherits="UI.datagrid" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<div id="gogo1"></div>
<telerik:RadGrid ID="grid1" AutoGenerateColumns="false" runat="server" ShowFooter="true" EnableViewState="true" AllowPaging="true" AllowSorting="true" Skin="Default" EnableLinqExpressions="false">
  
    <ExportSettings ExportOnlyData="true" OpenInNewWindow="true" FileName="marktime_export" UseItemStyles="true">
        <Excel Format="Biff" />
        <Pdf BorderStyle="Thin" BorderType="AllBorders" DefaultFontFamily="Calibri" PageBottomMargin="20" PageTopMargin="20" PageLeftMargin="30" PageRightMargin="20"></Pdf>
    </ExportSettings>
    <GroupingSettings CaseSensitive="false" ExpandTooltip="Rozbalit řádky" CollapseTooltip="Sbalit řádky" />

    <ClientSettings>
        <Selecting AllowRowSelect="true" />
        <ClientEvents OnRowContextMenu="ContextSelect" OnGridCreated="GridCreated" OnCommand="OnGridCommand" />
       
    </ClientSettings>
    <PagerStyle Position="TopAndBottom" AlwaysVisible="false" />
    <SortingSettings SortToolTip="Klikněte zde pro třídění" SortedDescToolTip="Setříděno sestupně" SortedAscToolTip="Setříděno vzestupně" />
    <FooterStyle BackColor="#bcc7d8" HorizontalAlign="Right" />
    
</telerik:RadGrid>

<telerik:RadAjaxLoadingPanel runat="server" ID="RadAjaxLoadingPanel1" RenderMode="Lightweight" Transparency="30" BackColor="#E0E0E0">
    <div style="float:none;padding-top:80px;">
    <img src="Images/loading.gif" />
    <h2>LOADING...</h2>
    </div>
</telerik:RadAjaxLoadingPanel>

<asp:HiddenField ID="hidAutoScrollHashID" runat="server" Value="" />

<script type="text/javascript">
    function OnGridCommand(sender, args) {
        //alert(args.get_commandName());
        var loadingPanel = $find("<%= RadAjaxLoadingPanel1.ClientID %>");
        loadingPanel.show(sender.get_id());
    }


    if (document.getElementById("<%=hidAutoScrollHashID.ClientID%>").value != "") {
        //window.location.hash = document.getElementById("<%=hidAutoScrollHashID.ClientID%>").value;
    }

    function ContextSelect(sender, args) {

        var row = args.get_itemIndexHierarchical();
        if (row >= 0) {
            var masterTable = $find("<%=grid1.ClientID%>").get_masterTableView();

            masterTable.clearSelectedItems();
            var dataItem = masterTable.get_dataItems()[row];
            dataItem.set_selected(true);

        }
    }

    function GridCreated(sender, eventArgs) {
        var row = sender.get_masterTableView().get_selectedItems()[0];

        //if the position of the selected row is below the viewable grid area  
        if (row) {
            var xx = row.get_element().offsetTop + row.get_element().offsetHeight + 20;
            //alert("top: " + xx + ", height: " + $(window).height());
            if (xx > $(window).height()) {
                //alert(row.get_element().getAttribute("id"));

                window.location.hash = row.get_element().getAttribute("id");

            }


        }
        
        <%if grid1.ClientSettings.Scrolling.EnableVirtualScrollPaging=True then%>
        
        var hx1 = new Number;
        var hx2 = new Number;
        var hx3 = new Number;
        hx1 = $(window).height();
        var scrollArea = sender.GridDataDiv;                
        var ss = self.document.getElementById("gogo1");
        var offset = $(ss).offset();        
        hx2 = offset.top;        
        hx3 = hx1 - hx2-100;
        
        var gridHeader = sender.GridHeaderDiv;
        
        scrollArea.style.height = hx3 - gridHeader.clientHeight + "px";
        
        <%end if%>
        
    }

</script>
