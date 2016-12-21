<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="p56_subgrid.ascx.vb" Inherits="UI.p56_subgrid" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<%@ Register TagPrefix="uc" TagName="datagrid" Src="~/datagrid.ascx" %>

<div class="commandcell">
    <img src="Images/task.png" alt="Úkoly" />
    <asp:Label ID="lblHeaderP56" CssClass="framework_header_span" runat="server" Text="Úkoly"></asp:Label>
</div>
<div class="commandcell" style="margin-left: 10px;">
    <asp:DropDownList ID="cbxGroupBy" runat="server" AutoPostBack="true" ToolTip="Datové souhrny">
        <asp:ListItem Text="Bez souhrnů" Value=""></asp:ListItem>
        <asp:ListItem Text="Typ úkolu" Value="p57Name"></asp:ListItem>
        <asp:ListItem Text="Klient" Value="Client"></asp:ListItem>
        <asp:ListItem Text="Projekt" Value="ProjectCodeAndName"></asp:ListItem>
        <asp:ListItem Text="Příjemce" Value="ReceiversInLine"></asp:ListItem>
        <asp:ListItem Text="Aktuální stav" Value="b02Name"></asp:ListItem>
        <asp:ListItem Text="Milník" Value="o22Name"></asp:ListItem>
        <asp:ListItem Text="Vlastník" Value="Owner"></asp:ListItem>
        <asp:ListItem Text="Uzavřeno" Value="IsClosed"></asp:ListItem>
    </asp:DropDownList>

    <asp:DropDownList ID="cbxP56Validity" runat="server" AutoPostBack="true">
        <asp:ListItem Text="Otevřené i uzavřené úkoly" Value="1" Selected="true"></asp:ListItem>
        <asp:ListItem Text="Pouze otevřené úkoly" Value="2"></asp:ListItem>
        <asp:ListItem Text="Pouze uzavřené úkoly" Value="3"></asp:ListItem>
    </asp:DropDownList>
</div>
<div class="commandcell" style="margin-left: 10px;">
    <telerik:RadMenu ID="recmenu1" Skin="Metro" runat="server" ClickToOpen="true" EnableRoundedCorners="false" EnableShadows="false" style="z-index:2000;" RenderMode="Auto" ExpandDelay="0" ExpandAnimation-Type="None" EnableAutoScroll="true">
        <Items>
            <telerik:RadMenuItem Text="Záznam" ImageUrl="Images/menuarrow.png">
                <Items>
                    <telerik:RadMenuItem Text="Nový úkol" Value="new" NavigateUrl="javascript:p56_record()"></telerik:RadMenuItem>
                    <telerik:RadMenuItem Text="Kopírovat úkol" Value="clone" NavigateUrl="javascript:p56_clone()"></telerik:RadMenuItem>
                </Items>
            </telerik:RadMenuItem>
            <telerik:RadMenuItem Text="Akce" Value="akce" ImageUrl="Images/menuarrow.png">
                <Items>
                    <telerik:RadMenuItem Text="Zapsat worksheet úkon k úkolu" Value="p31new" NavigateUrl="javascript:p31_entry_p56()"></telerik:RadMenuItem>
                    <telerik:RadMenuItem Text="Schvalovat worksheet úkony za označené úkoly" Value="cmdApprove" NavigateUrl="javascript:approving()"></telerik:RadMenuItem>
                    <telerik:RadMenuItem Text="Zobrazit na celou stránku" Value="cmdFullScreen" NavigateUrl="javascript:p56_fullscreen()"></telerik:RadMenuItem>
                </Items>
            </telerik:RadMenuItem>
            <telerik:RadMenuItem Text="Další" ImageUrl="Images/menuarrow.png">
                <ContentTemplate>
                <div style="padding:8px;">
                    <div style="margin-top: 20px;">
                        <button type="button" onclick="p56_subgrid_setting(<%=ViewState("j74id")%>)">Sloupce</button>
                    </div>
                    <div>
                        <span>Stránkování:</span>
                        <asp:DropDownList ID="cbxPaging" runat="server" AutoPostBack="true" ToolTip="Stránkování">
                            <asp:ListItem Text="10"></asp:ListItem>
                            <asp:ListItem Text="20"></asp:ListItem>
                            <asp:ListItem Text="50" Selected="True"></asp:ListItem>
                            <asp:ListItem Text="100"></asp:ListItem>
                            <asp:ListItem Text="200"></asp:ListItem>
                            <asp:ListItem Text="500"></asp:ListItem>
                        </asp:DropDownList>
                    </div>

                    <asp:Panel ID="panExport" runat="server" Style="margin-top: 10px;">
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

</div>






<asp:HiddenField ID="hidReceiversInLine" runat="server" />
<asp:HiddenField ID="hidTasksWorksheetColumns" runat="server" />
<asp:HiddenField ID="hidDefaultSorting" runat="server" />
<asp:HiddenField ID="hidCols" runat="server" />
<asp:HiddenField ID="hidSumCols" runat="server" />
<asp:HiddenField ID="hidFrom" runat="server" />
<asp:HiddenField ID="hidFooterString" runat="server" />

<div style="clear: both; width: 100%;"></div>

<uc:datagrid ID="gridP56" runat="server" ClientDataKeyNames="pid" OnRowSelected="RowSelected_p56" OnRowDblClick="RowDoubleClick_p56"></uc:datagrid>

<script type="text/javascript">
    $(document).ready(function () {




    });

    function GetAllSelectedP56IDs() {

        var masterTable = $find("<%=gridP56.radGridOrig.ClientID%>").get_masterTableView();
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

        function approving() {
            var pids = GetAllSelectedP56IDs();
            if (pids == "") {
                alert("Není vybrán ani jeden záznam.");
                return;

            }
            p56_subgrid_approving(pids);



        }

        function p56_fullscreen() {
            window.open("p56_Framework.aspx?masterpid=<%=Me.MasterDataPID%>&masterprefix=<%=BO.BAS.GetDataPrefix(Me.x29ID)%>", "_top");
        
    }
</script>
