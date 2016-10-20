<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master" CodeBehind="o10_framework.aspx.vb" Inherits="UI.o10_framework" %>
<%@ MasterType VirtualPath="~/Site.Master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <script type="text/javascript">
        function record_clone(pid) {
            
            sw_master("o10_record.aspx?clone=1&pid=" + pid, "Images/article_32.png",true);

        }

        function record_edit(pid) {            
            sw_master("o10_record.aspx?pid=" + pid, "Images/article_32.png");

        }

        function record_new() {
            sw_master("p51_record.aspx?pid=0", "Images/billing_32.png");

        }



        function hardrefresh(pid, flag) {


            location.replace("o10_framework.aspx")

        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div style="background-color: white;">
        <div style="float: left;">
            <img src="Images/article_32.png" alt="Ceníky sazeb" />
        </div>
        <div class="commandcell" style="padding-left: 10px;">
            <asp:Label ID="lblFormHeader" runat="server" CssClass="page_header_span" Text="Nástěnka" Style="vertical-align: top;"></asp:Label>
        </div>
        <div class="commandcell">
            <asp:DropDownList ID="cbxValidity" runat="server" AutoPostBack="true">

            </asp:DropDownList>
        </div>
    </div>
    <div style="clear:both;"></div>
</asp:Content>
