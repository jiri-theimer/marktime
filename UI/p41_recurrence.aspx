<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/ModalForm.Master" CodeBehind="p41_recurrence.aspx.vb" Inherits="UI.p41_recurrence" %>

<%@ MasterType VirtualPath="~/ModalForm.Master" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="OverMainContent" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="server">

    <div class="content-box2" style="margin-top:20px;">
        <div class="title">
            <img src="Images/recurrence.png" />
            Matka (šablona) opakovaných projektů
        </div>
        <div class="content">
            <div class="div6">
                <span>Typ opakování:</span>
                <asp:DropDownList ID="p65ID" runat="server" DataTextField="NameWithFlag" DataValueField="pid" AutoPostBack="true"></asp:DropDownList>
            </div>
            <asp:Panel ID="panRecurrence" runat="server">
                <div class="div6">
                    <span>Maska názvu nových projektů:</span>
                    <asp:TextBox ID="p41RecurNameMask" runat="server" Width="300px"></asp:TextBox>
                </div>
                <div class="div6">
                    <span>Úvodní rozhodné datum tohoto projektu:</span>
                    <telerik:RadDateInput ID="p41RecurBaseDate" runat="server" DisplayDateFormat="d.M.yyyy" DateFormat="d.M.yyyy" Width="100px"></telerik:RadDateInput>

                </div>
            </asp:Panel>
        </div>
    </div>


</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="FootContent" runat="server">
</asp:Content>
