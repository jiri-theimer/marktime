<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/ModalForm.Master" CodeBehind="o23_queue.aspx.vb" Inherits="UI.o23_queue" %>

<%@ MasterType VirtualPath="~/ModalForm.Master" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<%@ Register TagPrefix="uc" TagName="datagrid" Src="~/datagrid.ascx" %>
<%@ Register TagPrefix="uc" TagName="periodcombo" Src="~/periodcombo.ascx" %>
<%@ Register TagPrefix="uc" TagName="mygrid" Src="~/mygrid.ascx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <script type="text/javascript">
        function griddesigner() {
            var j74id = "<%=Me.CurrentJ74ID%>";
            
            dialog_master("grid_designer.aspx?prefix=o23&pid=" + j74id,  true);
        }

        function periodcombo_setting() {

            dialog_master("periodcombo_setting.aspx",false);
        }

        function hardrefresh(pid, flag) {

            location.replace("o23_queue.aspx?masterpid=<%=Me.CurrentMasterPID%>&masterprefix=<%=me.CurrentMasterPrefix%>");

        }

        function file_preview(url) {
            ///náhled na soubor
            sw_local(url, "Images/attachment_32.png", true);

        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="OverMainContent" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="server">
    <div class="content-box2">
        <div class="title">
            <asp:Label ID="EntityName" runat="server"></asp:Label>
        </div>
        <div class="content">
            <asp:Label ID="EntityRecord" runat="server" CssClass="valboldblue"></asp:Label>
        </div>
    </div>
    <table>
        <tr>
            <td>
                <asp:DropDownList ID="cbxGroupBy" runat="server" AutoPostBack="true" ToolTip="Datové souhrny">
                    <asp:ListItem Text="Bez souhrnů" Value=""></asp:ListItem>
                    <asp:ListItem Text="Typ dokumentu" Value="o24Name"></asp:ListItem>
                    <asp:ListItem Text="Klient" Value="p28Name"></asp:ListItem>
                    
                    <asp:ListItem Text="Projekt" Value="Project"></asp:ListItem>
                    <asp:ListItem Text="Typ dokumentu" Value="o24Name"></asp:ListItem>
                    <asp:ListItem Text="Aktuální stav" Value="b02Name"></asp:ListItem>
                    <asp:ListItem Text="Vlastník" Value="Owner"></asp:ListItem>
                </asp:DropDownList>
            </td>
            <td>
                <uc:mygrid id="designer1" runat="server" prefix="o23" masterprefix="" x36key="o23_queue-j70id" AllowSettingButton="false" ReloadUrl="o23_queue.aspx"></uc:mygrid>
            </td>
            <td>

                <asp:DropDownList ID="cbxPaging" runat="server" AutoPostBack="true" ToolTip="Stránkování datového přehledu">
                    <asp:ListItem Text="20"></asp:ListItem>
                    <asp:ListItem Text="50" Selected="True"></asp:ListItem>
                    <asp:ListItem Text="100"></asp:ListItem>
                    <asp:ListItem Text="200"></asp:ListItem>
                    <asp:ListItem Text="500"></asp:ListItem>
                </asp:DropDownList>
            </td>
            <td>
               <uc:periodcombo ID="period1" runat="server" Width="170px"></uc:periodcombo>
           </td>
            <td>
                <asp:DropDownList ID="o24ID" runat="server" DataValueField="pid" DataTextField="o24Name" AutoPostBack="true" ToolTip="Filtrování podle typu dokumentu"></asp:DropDownList>
            </td>
           
        </tr>
    </table>


    <uc:datagrid ID="grid1" runat="server" ClientDataKeyNames="pid"></uc:datagrid>

    <asp:HiddenField ID="hidMasterPID" runat="server" />
    <asp:HiddenField ID="hidMasterPrefix" runat="server" />
    <asp:HiddenField ID="hidMasterGUID" runat="server" />
    <asp:hiddenfield ID="hidDefaultSorting" runat="server" />
    <asp:HiddenField ID="hidCols" runat="server" />
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="FootContent" runat="server">
</asp:Content>
