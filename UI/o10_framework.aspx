<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master" CodeBehind="o10_framework.aspx.vb" Inherits="UI.o10_framework" %>

<%@ MasterType VirtualPath="~/Site.Master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <script type="text/javascript">
        function clone(pid) {

            sw_master("o10_record.aspx?clone=1&pid=" + pid, "Images/article_32.png", true);

        }

        function edit(pid) {
            sw_master("o10_record.aspx?pid=" + pid, "Images/article_32.png", true);

        }

        function create() {
            sw_master("o10_record.aspx?pid=0", "Images/billing_32.png", true);

        }



        function hardrefresh(pid, flag) {


            location.replace("o10_framework.aspx")

        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div style="padding: 10px; background-color: white;">
        <div style="float: left;">
            <img src="Images/article_32.png" alt="Ceníky sazeb" />
        </div>
        <div style="float: left; padding-left: 10px;">
            <asp:Label ID="lblFormHeader" runat="server" CssClass="page_header_span" Text="Nástěnka" Style="vertical-align: top;"></asp:Label>
        </div>
        <div style="float: left; padding-left: 10px;">
            <asp:HyperLink ID="linkNew" runat="server" Text="Nový článek" NavigateUrl="javascript:create()"></asp:HyperLink>
        </div>
        <div style="float: left; padding-left: 20px;">
            <asp:DropDownList ID="cbxValidity" runat="server" AutoPostBack="true">
                <asp:ListItem Text="Zobrazovat pouze otevřené" Value="1" Selected="true"></asp:ListItem>
                <asp:ListItem Text="Zobrazovat otevřené i uzavřené" Value="2"></asp:ListItem>
                <asp:ListItem Text="Zobrazovat pouze uzavřené" Value="3"></asp:ListItem>
            </asp:DropDownList>

        </div>
        <div style="float: left; padding-left: 10px;">
            <asp:DropDownList ID="cbxOwner" runat="server" AutoPostBack="true">
                <asp:ListItem Text="" Value="" Selected="true"></asp:ListItem>
                <asp:ListItem Text="Pouze, kde jsem vlastníkem (autorem)" Value="1"></asp:ListItem>

            </asp:DropDownList>

        </div>
        <div style="clear: both;"></div>
        
        <asp:Repeater ID="rp1" runat="server">
            <ItemTemplate>
                <div class="<%#Eval("CssClassBox")%>" style="margin-top:20px;">
                    <div class="title">
                        <img src="Images/article.png" />
                        <span style="font-weight:bold;font-variant: small-caps;font-size:120%;"><%#Eval("o10Name")%></span>
                        
                        <a style="float: right;margin-left:10px;display:<%#Eval("StyleDisplayEdit")%>;" href="javascript:clone(<%#Eval("pid")%>)">Kopírovat</a>
                        <a style="float: right;margin-left:6px;display:<%#Eval("StyleDisplayEdit")%>;" href="javascript:edit(<%#Eval("pid")%>)">Upravit</a>
                        
                        <span style="font-style:italic;float:right;"> <%#Eval("ValidFrom")%> | <%#Eval("Owner") %></span>
                    </div>
                    <div class="content" style="color:black;background-color: <%#Eval("o10BackColor")%>;">
                        <%#Eval("o10BodyHtml")%>
                    </div>
                </div>
            </ItemTemplate>
        </asp:Repeater>

        <div style="clear: both;"></div>
    </div>

</asp:Content>
