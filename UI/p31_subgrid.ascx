<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="p31_subgrid.ascx.vb" Inherits="UI.p31_subgrid" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<%@ Register TagPrefix="uc" TagName="datagrid" Src="~/datagrid.ascx" %>
<%@ Register TagPrefix="uc" TagName="periodcombo" Src="~/periodcombo.ascx" %>

<asp:Panel runat="server" ID="panCommand" CssClass="div6">
    <div class="commandcell">
        <img src="Images/worksheet.png" alt="Worksheet" />
        <asp:Label ID="lblHeaderP31" CssClass="framework_header_span" runat="server" Text=""></asp:Label>
    </div>

    <div class="commandcell" style="margin-left:10px;">
        <uc:periodcombo ID="period1" runat="server" Width="150px"></uc:periodcombo>
        <asp:Label ID="ExplicitPeriod" runat="server" CssClass="valboldblue"></asp:Label>
        
        <asp:ImageButton ID="cmdClearExplicitPeriod" runat="server" ImageUrl="Images/close.png" ToolTip="Zrušit filtr podle kalendáře" CssClass="button-link" />
    </div>

    <div class="commandcell" style="padding-left: 10px;">
        <asp:HyperLink ID="clue_query" runat="server" CssClass="reczoom" ToolTip="Detail filtru" Text="i"></asp:HyperLink>
    </div>
    <div class="commandcell">
        <asp:DropDownList ID="j70ID" runat="server" AutoPostBack="true" DataTextField="NameWithMark" DataValueField="pid" Style="width: 150px;" ToolTip="Pojmenovaný filtr"></asp:DropDownList>


        <asp:ImageButton ID="cmdQuery" runat="server" OnClientClick="return querybuilder()" ImageUrl="Images/query.png" ToolTip="Návrhář filtrů" CssClass="button-link" />

    </div>
    <div class="commandcell" style="margin-left:10px;">
        <telerik:RadMenu ID="recmenu1" Skin="Metro" runat="server" ClickToOpen="true" style="z-index:2000;" RenderMode="Auto" ExpandDelay="0" ExpandAnimation-Type="None">
            <Items>
                <telerik:RadMenuItem Text="Záznam" ImageUrl="Images/menuarrow.png">
                    <Items>
                        <telerik:RadMenuItem Text="Nový" Value="new" NavigateUrl="javascript:p31_entry()"></telerik:RadMenuItem>
                        <telerik:RadMenuItem Text="Kopírovat" Value="clone" NavigateUrl="javascript:p31_clone()"></telerik:RadMenuItem>
                        <telerik:RadMenuItem Text="Rozdělit časový úkon na 2 kusy" Value="split" NavigateUrl="javascript:p31_split()"></telerik:RadMenuItem>
                    </Items>
                </telerik:RadMenuItem>
                <telerik:RadMenuItem Text="Akce" Value="akce" ImageUrl="Images/menuarrow.png">
                    <Items>
                        <telerik:RadMenuItem Text="Schvalovat/pře-schvalovat označené" Value="cmdApprove" NavigateUrl="javascript:approving()"></telerik:RadMenuItem>
                        <telerik:RadMenuItem Text="Zobrazit přehled na celou stránku" Value="cmdFullScreen" NavigateUrl="javascript:p31_fullscreen()"></telerik:RadMenuItem>
                    </Items>
                </telerik:RadMenuItem>
                <telerik:RadMenuItem Text="Další" ImageUrl="Images/menuarrow.png">
                    <ContentTemplate>
                        <div style="padding:10px;">
                        <div style="margin-top: 20px;">
                            <div style="padding-right:10px;">
                            <asp:DropDownList ID="j74id" runat="server" AutoPostBack="true" DataTextField="j74Name" DataValueField="pid" Style="min-width: 200px;" ToolTip="Pojmenované šablony sloupců"></asp:DropDownList>
                            </div>
                            <div>
                                <asp:HyperLink ID="linkGridDesigner" runat="server" NavigateUrl="javascript:p31_subgrid_columns()" Text="Sloupce"></asp:HyperLink>
                            </div>


                        </div>

                        <div style="margin-top: 20px;">
                            <span><%=Resources.common.Strankovani%>:</span>
                            <asp:DropDownList ID="cbxPaging" runat="server" AutoPostBack="true" ToolTip="Stránkování">
                                <asp:ListItem Text="10"></asp:ListItem>
                                <asp:ListItem Text="20"></asp:ListItem>
                                <asp:ListItem Text="50" Selected="True"></asp:ListItem>
                                <asp:ListItem Text="100"></asp:ListItem>
                                <asp:ListItem Text="200"></asp:ListItem>
                                <asp:ListItem Text="500"></asp:ListItem>
                            </asp:DropDownList>
                        </div>

                        <asp:Panel ID="panGroupBy" runat="server" Style="margin-top: 20px;">
                            <span><%=Resources.common.DatoveSouhrny%>:</span>
                            <asp:DropDownList ID="cbxGroupBy" runat="server" AutoPostBack="true">
                                <asp:ListItem Text="<%$Resources:common,BezSouhrnu%>" Value=""></asp:ListItem>
                                <asp:ListItem Text="<%$Resources:common,Sesit%>" Value="p34Name"></asp:ListItem>
                                <asp:ListItem Text="<%$Resources:common,Aktivita%>" Value="p32Name"></asp:ListItem>
                                <asp:ListItem Text="<%$Resources:common,Osoba%>" Value="Person"></asp:ListItem>
                                <asp:ListItem Text="<%$Resources:common,Klient%>" Value="ClientName"></asp:ListItem>
                                <asp:ListItem Text="<%$Resources:common,Projekt %>" Value="p41Name"></asp:ListItem>
                                <asp:ListItem Text="<%$Resources:common,Faktura%>" Value="p91Code"></asp:ListItem>
                                <asp:ListItem Text="<%$Resources:common,Ukol%>" Value="p56Name"></asp:ListItem>
                                <asp:ListItem Text="<%$Resources:common,Schvalovani%>" Value="p71Name"></asp:ListItem>
                                <asp:ListItem Text="<%$Resources:common,FaktStatus%>" Value="p70Name"></asp:ListItem>
                                <asp:ListItem Text="Billing dávka" Value="p31ApprovingSet"></asp:ListItem>
                            </asp:DropDownList>
                            <asp:CheckBox ID="chkGroupsAutoExpanded" runat="server" Text="Auto-rozbalené souhrny" AutoPostBack="true" Checked="true" />

                        </asp:Panel>

                        <asp:Panel ID="panExport" runat="server" Style="margin-top: 20px;">
                            <div><strong>Export</strong></div>
                            
                                <img src="Images/export.png" alt="export" />
                                <asp:LinkButton ID="cmdExport" runat="server" Text="Export" ToolTip="Export do MS EXCEL tabulky, plný počet záznamů" />

                                <img src="Images/xls.png" alt="xls" />
                                <asp:LinkButton ID="cmdXLS" runat="server" Text="XLS" ToolTip="Export do XLS vč. souhrnů s omezovačem na maximálně 2000 záznamů" />

                                <img src="Images/pdf.png" alt="pdf" />
                                <asp:LinkButton ID="cmdPDF" runat="server" Text="PDF" ToolTip="Export do PDF vč. souhrnů s omezovačem na maximálně 2000 záznamů" />

                                <img src="Images/doc.png" alt="doc" />
                                <asp:LinkButton ID="cmdDOC" runat="server" Text="DOC" ToolTip="Export do DOC vč. souhrnů s omezovačem na maximálně 2000 záznamů" />
                           
                        </asp:Panel>
                        </div>
                    </ContentTemplate>
                </telerik:RadMenuItem>
            </Items>
        </telerik:RadMenu>
        <asp:CheckBox ID="chkIncludeChilds" runat="server" AutoPostBack="true" text="Zahrnout i pod-projekty" CssClass="chk" Visible="false" />
    </div>



</asp:Panel>
<div style="clear: both; width: 100%;"></div>

<uc:datagrid ID="grid2" runat="server" ClientDataKeyNames="pid" OnRowSelected="p31_RowSelected" OnRowDblClick="p31_RowDoubleClick_first" HeaderText="Projektový worksheet"></uc:datagrid>

<asp:HiddenField ID="hidMasterDataPID" runat="server" />
<asp:HiddenField ID="hidX29ID" runat="server" />
<asp:HiddenField ID="hidNeedRefreshP31_subgrid" runat="server" />
<asp:HiddenField ID="hidExplicitDateFrom" runat="server" Value="01.01.1900" />
<asp:HiddenField ID="hidExplicitDateUntil" runat="server" Value="01.01.3000" />
<asp:HiddenField ID="hidJ74RecordState" runat="server" />
<asp:HiddenField ID="hidDefaultSorting" runat="server" />
<asp:HiddenField ID="hidJ74ID" runat="server" />
<asp:HiddenField ID="hidDrillDownField" runat="server" />
<asp:HiddenField ID="hidCols" runat="server" />
<asp:HiddenField ID="hidSumCols" runat="server" />
<asp:HiddenField ID="hidFooterString" runat="server" />
<asp:HiddenField ID="hidFrom" runat="server" />
<asp:HiddenField ID="hidAllowFullScreen" runat="server" Value="1" />
<asp:HiddenField ID="hidMasterTabAutoQueryFlag" runat="server" />

<script type="text/javascript">

    $(document).ready(function () {




    });



    function periodcombo_setting() {
        p31_subgrid_periodcombo();

    }

    function querybuilder() {
        var j70id = "<%=Me.CurrentJ70ID%>";
        p31_subgrid_querybuilder(j70id);
        return (false);
    }

    function approving() {
        var pids = GetAllSelectedPIDs();
        if (pids == "") {
            alert("Není vybrán ani jeden záznam.");
            return;

        }
        p31_subgrid_approving(pids);


    }

    function GetAllSelectedPIDs() {

        var masterTable = $find("<%=grid2.radGridOrig.ClientID%>").get_masterTableView();
        var sel = masterTable.get_selectedItems();
        var pids = "";

        for (i = 0; i < sel.length; i++) {
            if (pids == "")
                pids = sel[i].getDataKeyValue("pid");
            else
                pids = pids + "," + sel[i].getDataKeyValue("pid");
        }

        return (pids);
    }

    function p31_subgrid_columns() {
        p31_subgrid_setting(document.getElementById("<%=Me.j74id.ClientID%>").value);
        //return (false);
    }

    function p31_RowDoubleClick_first(sender, args) {
        if (args.get_tableView().get_name() == "grid") {
            p31_RowDoubleClick();
        }
        if (args.get_tableView().get_name() == "drilldown") {
            var item = sender.get_masterTableView().get_dataItems()[args.get_itemIndexHierarchical()];

            var rowid = item.get_id();
            var firstInput = $('#' + rowid).find('input[type=submit]').filter(':visible:first');
            if (firstInput != null) {
                firstInput.click();
            }

        }

    }

    function p31_fullscreen() {
        window.open("p31_grid.aspx?masterpid=" + document.getElementById("<%=me.hidMasterDataPID.ClientID%>").value + "&masterprefix=<%=BO.BAS.GetDataPrefix(Me.EntityX29ID)%>&p31tabautoquery=<%=me.MasterTabAutoQueryFlag%>", "_top");
        return (false);
    }
</script>
