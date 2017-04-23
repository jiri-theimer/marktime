<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/ModalForm.Master" CodeBehind="p31_drilldown.aspx.vb" Inherits="UI.p31_drilldown" %>

<%@ MasterType VirtualPath="~/ModalForm.Master" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<%@ Register TagPrefix="uc" TagName="datagrid" Src="~/datagrid.ascx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="slidingDiv1">
        <div class="content-box2">
            <div class="title">
                Sledované veličiny
                <asp:Button ID="cmdRefresh" runat="server" CssClass="cmd" Text="Obnovit statistiku" />
            </div>
            <div class="content">

                <table cellpadding="8">
                    <tr valign="top">
                        <td>
                            <div><%=Resources.grid_designer.DostupneSloupce %></div>
                            <telerik:RadListBox ID="colsSource" Height="200px" runat="server" AllowTransfer="true" TransferMode="Move" TransferToID="colsDest" SelectionMode="Single" Culture="cs-CZ" AllowTransferOnDoubleClick="true" Width="350px" AutoPostBackOnReorder="false" AutoPostBackOnDelete="false" AutoPostBackOnTransfer="false">
                                <ButtonSettings TransferButtons="All" ShowTransferAll="false" />

                                <Localization ToRight="Přesunout" ToLeft="Odebrat" AllToRight="Přesunout vše" AllToLeft="Odbrat vše" MoveDown="Posunout dolu" MoveUp="Posunout nahoru" />
                            </telerik:RadListBox>
                        </td>
                        <td>
                            <div><%=Resources.grid_designer.VybraneSloupce %></div>
                            <telerik:RadListBox ID="colsDest" runat="server" AllowReorder="true" AllowTransferOnDoubleClick="true" Culture="cs-CZ" Width="350px" SelectionMode="Single">

                                <EmptyMessageTemplate>
                                    <div style="padding-top: 50px;">
                                        <%=Resources.grid_designer.ZadneVybraneSloupce %>
                                    </div>
                                </EmptyMessageTemplate>
                            </telerik:RadListBox>

                        </td>

                    </tr>
                </table>

            </div>
        </div>
    </div>

    <div class="div6">
        <telerik:RadComboBox ID="dd" runat="server" AutoPostBack="true"></telerik:RadComboBox>
        
    </div>


    <uc:datagrid ID="grid1" runat="server" ClientDataKeyNames="pid"></uc:datagrid>



    <asp:HiddenField ID="hidJ74ID" runat="server" />
    <asp:HiddenField ID="hidJ70ID" runat="server" />
    <asp:HiddenField ID="hidMasterPrefix" runat="server" />
    <asp:HiddenField ID="hidMasterPID" runat="server" />
    <asp:HiddenField ID="hidFrom" runat="server" />
    <asp:HiddenField ID="hidD1" runat="server" />
    <asp:HiddenField ID="hidD2" runat="server" />
    <asp:HiddenField ID="hidMasterAW" runat="server" />

    <script type="text/javascript">
        $(document).ready(function () {
            $(".slidingDiv1").hide();
            $(".show_hide1").show();

            $('.show_hide1').click(function () {
                $(".slidingDiv1").slideToggle();
            });





        });
    </script>
</asp:Content>
