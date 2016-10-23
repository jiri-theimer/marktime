<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="p56_subgrid.ascx.vb" Inherits="UI.p56_subgrid" %>
<%@ Register TagPrefix="uc" TagName="datagrid" Src="~/datagrid.ascx" %>

<div class="innerform_light_square">
    <table>
        <tr>
            <td>
                <img src="Images/task.png" alt="Úkoly" />
                <asp:Label ID="lblHeaderP56" CssClass="framework_header_span" runat="server" Text="Úkoly"></asp:Label>
            </td>
            <td>
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
            </td>
            <td>
                <asp:DropDownList ID="cbxP56Validity" runat="server" AutoPostBack="true">
                    <asp:ListItem Text="Otevřené i uzavřené úkoly" Value="1" Selected="true"></asp:ListItem>
                    <asp:ListItem Text="Pouze otevřené úkoly" Value="2"></asp:ListItem>
                    <asp:ListItem Text="Pouze uzavřené úkoly" Value="3"></asp:ListItem>
                </asp:DropDownList>
            </td>
            <td style="padding-left:20px;">
                <asp:ImageButton ID="cmdP56_p31new" runat="server" ImageUrl="Images/worksheet.png" ToolTip="Zapsat úkon k úkolu" OnClientClick="return p31_entry_p56()" CssClass="button-link" />
                <asp:ImageButton ID="cmdP56_new" runat="server" ImageUrl="Images/new.png" ToolTip="Nový úkol" OnClientClick="return p56_record(0,true)" CssClass="button-link" />
                <asp:ImageButton ID="cmdP56_clone" runat="server" ImageUrl="Images/copy.png" ToolTip="Kopírovat úkol" OnClientClick="return p56_clone()" CssClass="button-link" />
                <asp:ImageButton ID="cmdFullScreen" runat="server" ImageUrl="Images/fullscreen.png" ToolTip="Přehled úkolů na celou stránku" OnClientClick="return p56_fullscreen()" CssClass="button-link" />
            </td>
            <td style="padding-left:20px;">
                <button type="button" id="cmdSetting" class="show_hide1xxp56" style="padding: 3px; border-radius: 4px; border-top: solid 1px silver; border-left: solid 1px silver; border-bottom: solid 1px gray; border-right: solid 1px gray; background: buttonface;height:23px;">
                    
                    <img src="Images/arrow_down.gif" alt="Nastavení" />
                </button>
            </td>
        </tr>
    </table>
</div>







<div class="slidingDiv1xxp56" style="padding: 20px;">
    <button type="button" onclick="p56_subgrid_setting(<%=ViewState("j74id")%>)">Sloupce</button>
    <span style="padding-left: 40px;">Stránkování:</span>
    <asp:DropDownList ID="cbxPaging" runat="server" AutoPostBack="true" ToolTip="Stránkování">
        <asp:ListItem Text="10"></asp:ListItem>
        <asp:ListItem Text="20"></asp:ListItem>
        <asp:ListItem Text="50" Selected="True"></asp:ListItem>
        <asp:ListItem Text="100"></asp:ListItem>
        <asp:ListItem Text="200"></asp:ListItem>
        <asp:ListItem Text="500"></asp:ListItem>
    </asp:DropDownList>

    <asp:image ID="imgApprove" ImageUrl="Images/approve.png" runat="server" style="margin-left:20px;" />
    <asp:HyperLink ID="cmdApprove" runat="server" Text="Schvalovat úkony za vybrané úkoly" NavigateUrl="javascript:approving();"></asp:HyperLink>

    <span style="padding-left: 20px;"></span>
    <img src="Images/export.png" alt="export" />
        <asp:LinkButton ID="cmdExport" runat="server" Text="Export" ToolTip="Export do MS EXCEL tabulky, plný počet záznamů" />

        <img src="Images/xls.png" alt="xls" />
        <asp:LinkButton ID="cmdXLS" runat="server" Text="XLS" ToolTip="Export do XLS vč. souhrnů s omezovačem na maximálně 2000 záznamů" />

        <img src="Images/pdf.png" alt="pdf" />
        <asp:LinkButton ID="cmdPDF" runat="server" Text="PDF" ToolTip="Export do PDF vč. souhrnů s omezovačem na maximálně 2000 záznamů" />

        <img src="Images/doc.png" alt="doc" />
        <asp:LinkButton ID="cmdDOC" runat="server" Text="DOC" ToolTip="Export do DOC vč. souhrnů s omezovačem na maximálně 2000 záznamů" />

    <asp:HiddenField ID="hidReceiversInLine" runat="server" />
    <asp:HiddenField ID="hidTasksWorksheetColumns" runat="server" />
    <asp:HiddenField ID="hidDefaultSorting" runat="server" />
    <asp:HiddenField ID="hidCols" runat="server" />
    <asp:HiddenField ID="hidFrom" runat="server" />
</div>


<uc:datagrid ID="gridP56" runat="server" ClientDataKeyNames="pid" OnRowSelected="RowSelected_p56" OnRowDblClick="RowDoubleClick_p56"></uc:datagrid>

<script type="text/javascript">
    $(document).ready(function () {

        $(".slidingDiv1xxp56").hide();
        $(".show_hide1xxp56").show();

        $('.show_hide1xxp56').click(function () {
            $(".slidingDiv1xxp56").slideToggle();
        });


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
        return (false);
    }
</script>


