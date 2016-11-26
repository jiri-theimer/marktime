<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="datagrid.ascx.vb" Inherits="UI.datagrid" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<div id="gridContainer">
<telerik:RadGrid ID="grid1" AutoGenerateColumns="false" runat="server" ShowFooter="true" EnableViewState="true" AllowPaging="true" AllowSorting="true" Skin="Default" EnableLinqExpressions="false">
    <MasterTableView Width="100%"/>    
    <ExportSettings ExportOnlyData="true" OpenInNewWindow="true" FileName="marktime_export" UseItemStyles="true">
        <Excel Format="Biff" />
        <Pdf BorderStyle="Thin" BorderType="AllBorders" DefaultFontFamily="Calibri" PageBottomMargin="20" PageTopMargin="20" PageLeftMargin="30" PageRightMargin="20"></Pdf>
    </ExportSettings>
    <GroupingSettings CaseSensitive="false" ExpandTooltip="Rozbalit řádky" CollapseTooltip="Sbalit řádky" />

    <ClientSettings>
        <Selecting AllowRowSelect="true" />
        <ClientEvents OnRowContextMenu="ContextSelect" OnGridCreated="GridCreated" OnCommand="OnGridCommand" />
       
    </ClientSettings>
    <PagerStyle Position="Top" AlwaysVisible="false" />
    <SortingSettings SortToolTip="Klikněte zde pro třídění" SortedDescToolTip="Setříděno sestupně" SortedAscToolTip="Setříděno vzestupně" />
    <FooterStyle BackColor="#bcc7d8" HorizontalAlign="Right" />
    
</telerik:RadGrid>
</div>

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
        
        <%If grid1.ClientSettings.Scrolling.AllowScroll Then%>
        
        var hx1 = new Number;
       
        hx1 = $(window).height();
        var scrollArea = sender.GridDataDiv;                
        var ss = self.document.getElementById("gridContainer");
        offset = $(ss).offset();
        hx1 = hx1 - offset.top;

        ss.style.height = hx1 + "px";       
        
        var scrollArea = sender.GridDataDiv;
        var parent = $get("gridContainer");
        var gridHeader = sender.GridHeaderDiv;
        
        
        scrollArea.style.height = hx1 + "px";                       
        <%end if%>
        <%If grid1.ClientSettings.Scrolling.UseStaticHeaders Then%>
        hx1 = parent.clientHeight - gridHeader.clientHeight;
        <%if grid1.ShowFooter then%>
        hx1 = hx1 - 60;
        <%End If%>
        scrollArea.style.height = hx1 + "px";
        <%end if%>
       
    }

</script>
