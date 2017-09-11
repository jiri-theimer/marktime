<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/ModalForm.Master" CodeBehind="tag_binding.aspx.vb" Inherits="UI.tag_binding" %>
<%@ MasterType VirtualPath="~/ModalForm.Master" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<%@ Register TagPrefix="uc" TagName="datacombo" Src="~/datacombo.ascx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <script type="text/javascript">
        function requesting(sender, eventArgs) {
            var context = eventArgs.get_context();
            
            context["prefix"] = document.getElementById("<%=hidPrefix.ClientID%>").value;
        }

        function entryAdding(sender, eventArgs) {
            if (eventArgs.get_entry().get_value() == "") {
                eventArgs.set_cancel(true);
            }

        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="OverMainContent" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="server">


    <telerik:RadAutoCompleteBox ID="tags1" runat="server" RenderMode="Lightweight" EmptyMessage="Uveďte název štítku" Width="400px" OnClientEntryAdding="entryAdding" OnClientRequesting="requesting">     
        <WebServiceSettings Method="LoadComboData" Path="~/Services/tag_service.asmx"/>   
        <Localization ShowAllResults="Zobrazit všechny výsledky" RemoveTokenTitle="Vyjmout štítek z výběru" />
        
    </telerik:RadAutoCompleteBox>

    <asp:Panel ID="panCreate" runat="server" CssClass="content-box2">
        <div class="title">
            Vytvořit nový štítek
            <asp:Button ID="cmdCreate" runat="server" CssClass="cmd" Text="Uložit" />
        </div>
        <div class="content">
            <asp:TextBox ID="txtCreate" runat="server" Width="200px"></asp:TextBox>
            <div>
            <asp:CheckBox ID="chkCreate4All" runat="server" CssClass="chk" Text="Pro všechny entity" Checked="true" AutoPostBack="true" />
            <telerik:RadComboBox ID="cbxScope" runat="server" CheckBoxes="true" Visible="false">
                <Items>
                    <telerik:RadComboBoxItem Text="Projekty" Value="p41"/>
                    <telerik:RadComboBoxItem Text="Klienti" Value="p28"/>
                    <telerik:RadComboBoxItem Text="Úkoly" Value="p56"/>
                    <telerik:RadComboBoxItem Text="Osoby" Value="j02"/>
                    <telerik:RadComboBoxItem Text="Worksheet" Value="p31"/>
                    <telerik:RadComboBoxItem Text="Faktury" Value="p91"/>
                    <telerik:RadComboBoxItem Text="Dokumenty" Value="o23"/>
                    <telerik:RadComboBoxItem Text="Zálohy" Value="p90"/>
                </Items>
            </telerik:RadComboBox>
            </div>
        </div>
    </asp:Panel>


    <asp:HiddenField ID="hidPrefix" runat="server" />
    <asp:HiddenField ID="hidPIDs" runat="server" />
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="FootContent" runat="server">
</asp:Content>
