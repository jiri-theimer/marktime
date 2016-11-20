<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="o23_subgrid.ascx.vb" Inherits="UI.o23_subgrid" %>
<%@ Register TagPrefix="uc" TagName="datagrid" Src="~/datagrid.ascx" %>

<div class="innerform_light_square">
    <table>
        <tr>
            <td>
                <img src="Images/notepad.png" alt="Dokumenty" />
                <asp:Label ID="lblHeaderO23" CssClass="framework_header_span" runat="server" Text="Dokumenty"></asp:Label>
            </td>
            <td>
                <asp:DropDownList ID="cbxGroupBy" runat="server" AutoPostBack="true" ToolTip="Datové souhrny" Visible="false">
                    <asp:ListItem Text="Bez souhrnů" Value=""></asp:ListItem>
                    <asp:ListItem Text="Typ dokumentu" Value="p57Name"></asp:ListItem>
                   
                </asp:DropDownList>
            </td>
            
            <td style="padding-left: 20px;">                
                <asp:ImageButton ID="cmdO23_new" runat="server" ImageUrl="Images/new.png" ToolTip="Nový úkol" OnClientClick="return o23_record(0,true)" CssClass="button-link" />
                <asp:ImageButton ID="cmdO23_clone" runat="server" ImageUrl="Images/copy.png" ToolTip="Kopírovat úkol" OnClientClick="return o23_clone()" CssClass="button-link" />
                <asp:ImageButton ID="cmdFullScreen" runat="server" ImageUrl="Images/fullscreen.png" ToolTip="Přehled dokumentů na celou stránku" OnClientClick="return o23_fullscreen()" CssClass="button-link" />
            </td>
            <td style="padding-left: 20px;">
                <button type="button" id="cmdSetting" class="show_hide1xxo23" style="padding: 3px; border-radius: 4px; border-top: solid 1px silver; border-left: solid 1px silver; border-bottom: solid 1px gray; border-right: solid 1px gray; background: buttonface; height: 23px;">
                    <span>Další</span>
                    <img src="Images/arrow_down.gif" alt="Nastavení" />
                </button>
            </td>
        </tr>
    </table>
</div>


<div class="slidingDiv1xxo23" style="padding: 20px;">
    <button type="button" onclick="o23_subgrid_setting(<%=ViewState("j74id")%>)">Sloupce</button>
    <span style="padding-left: 40px;">Stránkování:</span>
    <asp:DropDownList ID="cbxPaging" runat="server" AutoPostBack="true" ToolTip="Stránkování">
        <asp:ListItem Text="10"></asp:ListItem>
        <asp:ListItem Text="20"></asp:ListItem>
        <asp:ListItem Text="50" Selected="True"></asp:ListItem>
        <asp:ListItem Text="100"></asp:ListItem>
        <asp:ListItem Text="200"></asp:ListItem>
        <asp:ListItem Text="500"></asp:ListItem>
    </asp:DropDownList>

    <asp:HiddenField ID="hidDefaultSorting" runat="server" />
    <asp:HiddenField ID="hidCols" runat="server" />
    <asp:HiddenField ID="hidFrom" runat="server" />

  
</div>


<uc:datagrid ID="gridO23" runat="server" ClientDataKeyNames="pid" OnRowSelected="RowSelected_o23" OnRowDblClick="RowDoubleClick_o23"></uc:datagrid>

<script type="text/javascript">
    $(document).ready(function () {

        $(".slidingDiv1xxo23").hide();
        $(".show_hide1xxo23").show();

        $('.show_hide1xxo23').click(function () {
            $(".slidingDiv1xxo23").slideToggle();
        });


    });

    function GetAllSelectedO23IDs() {

        var masterTable = $find("<%=gridO23.radGridOrig.ClientID%>").get_masterTableView();
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

    
    function o23_fullscreen() {
        window.open("o23_Framework.aspx?masterpid=<%=Me.MasterDataPID%>&masterprefix=<%=BO.BAS.GetDataPrefix(Me.x29ID)%>", "_top");
        return (false);
    }
</script>


