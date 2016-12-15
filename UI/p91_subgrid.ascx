﻿<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="p91_subgrid.ascx.vb" Inherits="UI.p91_subgrid" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<%@ Register TagPrefix="uc" TagName="datagrid" Src="~/datagrid.ascx" %>
<%@ Register TagPrefix="uc" TagName="periodcombo" Src="~/periodcombo.ascx" %>

<div class="commandcell">
    <img src="Images/invoice.png" alt="Faktury" />
    <asp:Label ID="lblHeaderP91" CssClass="framework_header_span" runat="server" Text="Vystavené faktury"></asp:Label>
</div>
<div class="commandcell" style="margin-left: 10px;">
    <asp:DropDownList ID="cbxPeriodType" AutoPostBack="true" runat="server">
        <asp:ListItem Text="Datum plnění" Value="p91DateSupply" Selected="true"></asp:ListItem>
        <asp:ListItem Text="Datum splatnosti" Value="p91DateMaturity"></asp:ListItem>
        <asp:ListItem Text="Datum vystavení" Value="p91Date"></asp:ListItem>
    </asp:DropDownList>
    <uc:periodcombo ID="period1" runat="server" Width="200px"></uc:periodcombo>
</div>
<div class="commandcell" style="margin-left: 10px;">
    <telerik:RadMenu ID="recmenu1" Skin="Metro" runat="server" EnableRoundedCorners="false" EnableShadows="false" ClickToOpen="true" style="z-index:2000;" RenderMode="Auto" ExpandDelay="0" ExpandAnimation-Type="None" EnableAutoScroll="true">
        <Items>
            <telerik:RadMenuItem Text="Akce" Value="akce" ImageUrl="Images/menuarrow.png">
                <Items>

                    <telerik:RadMenuItem Text="Zobrazit na celou stránku" Value="cmdFullScreen" NavigateUrl="javascript:p91_fullscreen()"></telerik:RadMenuItem>
                </Items>
            </telerik:RadMenuItem>
            <telerik:RadMenuItem Text="Další" ImageUrl="Images/menuarrow.png">
                <ContentTemplate>
                    <div style="min-width: 200px;padding:10px;">
                        <div style="margin-top: 20px;">
                            <button type="button" onclick="p91_subgrid_setting(<%=ViewState("j74id")%>,'<%=BO.BAS.GetDataPrefix(Me.x29ID)%>')">Sloupce</button>

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
                        <div style="margin-top: 20px;">
                            <img src="Images/export.png" />
                            <asp:LinkButton ID="cmdExport" runat="server" Text="Export do MS Excel" />
                        </div>
                    </div>


                </ContentTemplate>
            </telerik:RadMenuItem>
        </Items>
    </telerik:RadMenu>
</div>



<div style="clear: both; width: 100%;"></div>

<asp:HiddenField ID="hidDefaultSorting" runat="server" />
<asp:HiddenField ID="hidCols" runat="server" />
<asp:HiddenField ID="hidFrom" runat="server" />



<uc:datagrid ID="gridP91" runat="server" ClientDataKeyNames="pid" OnRowSelected="RowSelected_p91" OnRowDblClick="RowDoubleClick_p91"></uc:datagrid>


<script type="text/javascript">
    $(document).ready(function () {



    });

    function p91_fullscreen() {
        window.open("p91_framework.aspx?masterpid=<%=Me.MasterDataPID%>&masterprefix=<%=BO.BAS.GetDataPrefix(Me.x29ID)%>", "_top");

    }
</script>
