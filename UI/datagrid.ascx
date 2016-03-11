<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="datagrid.ascx.vb" Inherits="UI.datagrid" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<telerik:RadGrid ID="grid1" AutoGenerateColumns="false" runat="server" ShowFooter="true" EnableViewState="true" AllowFilteringByColumn="false" AllowPaging="true" AllowSorting="true" Skin="Default">
   
    <ExportSettings ExportOnlyData="true" OpenInNewWindow="true" FileName="marktime_export" UseItemStyles="false">
        <Excel Format="Biff" />
    </ExportSettings>
    <GroupingSettings CaseSensitive="false" />

    <ClientSettings>
        
        <Selecting AllowRowSelect="true" />
        <ClientEvents OnRowContextMenu="ContextSelect" OnGridCreated="GridCreated" />
    </ClientSettings>
    <PagerStyle Position="TopAndBottom" AlwaysVisible="false" />
    <SortingSettings SortToolTip="Klikněte zde pro třídění" SortedDescToolTip="Setříděno sestupně" SortedAscToolTip="Setříděno vzestupně" />
    <FooterStyle BackColor="#bcc7d8" HorizontalAlign="Right" />
    
</telerik:RadGrid>


<asp:HiddenField ID="hidAutoScrollHashID" runat="server" Value="" />

<script type="text/javascript">



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

    }

</script>
