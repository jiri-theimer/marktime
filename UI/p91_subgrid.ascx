<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="p91_subgrid.ascx.vb" Inherits="UI.p91_subgrid" %>
<%@ Register TagPrefix="uc" TagName="datagrid" Src="~/datagrid.ascx" %>
<%@ Register TagPrefix="uc" TagName="periodcombo" Src="~/periodcombo.ascx" %>

<div class="div6">
    <img src="Images/invoice.png" alt="Faktury" />
    <asp:Label ID="lblHeaderP91" CssClass="framework_header_span" runat="server" Text="Vystavené faktury"></asp:Label>
    <asp:DropDownList ID="cbxPeriodType" AutoPostBack="true" runat="server">
        <asp:ListItem Text="Datum plnění" Value="p91DateSupply" Selected="true"></asp:ListItem>
        <asp:ListItem Text="Datum splatnosti" Value="p91DateMaturity"></asp:ListItem>
        <asp:ListItem Text="Datum vystavení" Value="p91Date"></asp:ListItem>
    </asp:DropDownList>
    <uc:periodcombo ID="period1" runat="server" Width="200px"></uc:periodcombo>


    <button type="button" id="cmdSetting" class="show_hide1xxp91" style="padding: 3px; border-radius: 4px; border-top: solid 1px silver; border-left: solid 1px silver; border-bottom: solid 1px gray; border-right: solid 1px gray; background: buttonface; height: 23px;">

        <img src="Images/arrow_down.gif" alt="Nastavení" />
    </button>

</div>

<div class="slidingDiv1xxp91" style="padding: 20px;">
    <button type="button" onclick="p91_subgrid_setting(<%=ViewState("j74id")%>,'<%=BO.BAS.GetDataPrefix(Me.x29ID)%>')">Sloupce</button>
    <span style="padding-left: 40px;">Stránkování:</span>
    <asp:DropDownList ID="cbxPaging" runat="server" AutoPostBack="true" ToolTip="Stránkování">
        <asp:ListItem Text="10"></asp:ListItem>
        <asp:ListItem Text="20"></asp:ListItem>
        <asp:ListItem Text="50" Selected="True"></asp:ListItem>
        <asp:ListItem Text="100"></asp:ListItem>
        <asp:ListItem Text="200"></asp:ListItem>
        <asp:ListItem Text="500"></asp:ListItem>
    </asp:DropDownList>


    <span style="padding-left: 20px;"></span>
    <img src="Images/export.png" />
    <asp:LinkButton ID="cmdExport" runat="server" Text="Export do MS Excel" />


    <asp:HiddenField ID="hidDefaultSorting" runat="server" />
    <asp:HiddenField ID="hidCols" runat="server" />
    <asp:HiddenField ID="hidFrom" runat="server" />
</div>

<uc:datagrid ID="gridP91" runat="server" ClientDataKeyNames="pid" OnRowSelected="RowSelected_p91" OnRowDblClick="RowDoubleClick_p91"></uc:datagrid>


<script type="text/javascript">
    $(document).ready(function () {

        $(".slidingDiv1xxp91").hide();
        $(".show_hide1xxp91").show();

        $('.show_hide1xxp91').click(function () {
            $(".slidingDiv1xxp91").slideToggle();
        });


    });
</script>
