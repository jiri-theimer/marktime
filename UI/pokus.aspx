<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master" CodeBehind="pokus.aspx.vb" Inherits="UI.pokus" %>

<%@ MasterType VirtualPath="~/Site.Master" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<%@ Register TagPrefix="uc" TagName="datagrid" Src="~/datagrid.ascx" %>


<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">


    <script type="text/javascript">
      
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">


    




    <hr />
    <asp:Button ID="cmdImap" runat="server" Text="TEST IMAP" />
    <hr />
    <asp:Button ID="cmd1" runat="server" Text="Zaškrtnuté položky" />
    <telerik:RadComboBox ID="cbx1" runat="server" CheckBoxes="true" Width="300px">
        <Items>
            <telerik:RadComboBoxItem Text="Arts" Value="1" />
            <telerik:RadComboBoxItem Text="Biographies" Value="2" />
            <telerik:RadComboBoxItem Text="Children's Books" Value="3" />
            <telerik:RadComboBoxItem Text="Computers &amp; Internet" Value="4" />
            <telerik:RadComboBoxItem Text="Cooking" Value="5" />
            <telerik:RadComboBoxItem Text="History" Value="6" />
            <telerik:RadComboBoxItem Text="Fiction" Value="7" />
            <telerik:RadComboBoxItem Text="Mystery" Value="8" />
            <telerik:RadComboBoxItem Text="Nonfiction" Value="9" />
            <telerik:RadComboBoxItem Text="Romance" Value="10" />
            <telerik:RadComboBoxItem Text="Science Fiction" Value="11" />
            <telerik:RadComboBoxItem Text="Travel" Value="12" />
        </Items>
    </telerik:RadComboBox>

    <hr />

    <telerik:RadRadialGauge runat="server" ID="RadRadialGauge1" Width="250px" Height="200px">
        <Pointer Value="130" Cap-Color="red"></Pointer>

        <Scale Min="0" Max="130" MajorUnit="15" MinorUnit="10">
            <Labels Template="#=value# %" />
            <Ranges>
                <telerik:GaugeRange Color="Red" From="0" To="33" />
                <telerik:GaugeRange Color="Green" From="33" To="130" />


            </Ranges>
        </Scale>
    </telerik:RadRadialGauge>
    <hr />

    <asp:Button ID="cmdPokus" runat="server" Text="test mobile device" />

    <hr />
    <asp:TextBox ID="txtIC" runat="server" Text="25722034"></asp:TextBox>
    <asp:Button ID="cmdARES" runat="server" Text="ARES" />
    <asp:Label ID="aresRESULT" runat="server">
    </asp:Label>
    <hr />
    <asp:Button ID="cmdPostback" runat="server" Text="Načíst dataset z xml souboru" />

    <hr />
    <asp:Button ID="cmdPDF" runat="server" Text="PDF pokus" />

</asp:Content>


