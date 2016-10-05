<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Mobile.Master" CodeBehind="mobile_start.aspx.vb" Inherits="UI.mobile_start" %>

<%@ MasterType VirtualPath="~/Mobile.Master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript">
       
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">


    <div class="container">
       
        
        <ul class="nav nav-pills nav-stacked">
            <h3>
                <asp:Label ID="lblWelcome" runat="server"></asp:Label><small>,vítejte v MARKTIME!</small>
            </h3>
            <li>
                <a href="mobile_p31_calendar.aspx">
                    <img src="Images/worksheet.png" />
                    Worksheet</a>
            </li>
            <li>
                <div style="width: 100%; border-top: 1px; border-color: whitesmoke; height: 1px; border-style: solid;"></div>
            </li>

            <%If Master.Factory.SysUser.j04IsMenu_Project Then%>
            <li>
                <a href="mobile_grid.aspx?prefix=p41">
                    <img src="Images/project.png" />
                    Projekty </a>
            </li>
            <%End If%>
            <li>
                <asp:HyperLink ID="linkLastProject" runat="server" Style="display: none;" NavigateUrl="mobile_p41_framework.aspx"></asp:HyperLink>
            </li>
            <li>
                <div style="width: 100%; border-top: 1px; border-color: whitesmoke; height: 1px; border-style: solid;"></div>
            </li>
            <%If Master.Factory.SysUser.j04IsMenu_Contact Then%>
            <li>
                <a href="mobile_grid.aspx?prefix=p28">
                    <img src="Images/contact.png" />
                    Klienti</a>
            </li>
            <li>
                <asp:HyperLink ID="linkLastClient" runat="server" Style="display: none;" NavigateUrl="mobile_p28_framework.aspx"></asp:HyperLink>
            </li>
            <li>
                <div style="width: 100%; border-top: 1px; border-color: whitesmoke; height: 1px; border-style: solid;"></div>
            </li>

            <%End If%>

            <%If Master.Factory.SysUser.j04IsMenu_Invoice Then%>
            <li>
                <a href="mobile_grid.aspx?prefix=p91">
                    <img src="Images/invoice.png" />
                    Faktury</a>
            </li>
            <li>
                <asp:HyperLink ID="linkLastInvoice" runat="server" Style="display: none;" NavigateUrl="mobile_p91_framework.aspx"></asp:HyperLink>
            </li>
            <li>
                <div style="width: 100%; border-top: 1px; border-color: whitesmoke; height: 1px; border-style: solid;"></div>
            </li>
            <%End If%>

            <li>
                <a href="mobile_grid.aspx?prefix=p56">
                    <img src="Images/task.png" />
                    Úkoly</a>
            </li>

            <%If Master.Factory.SysUser.j04IsMenu_Report Then%>
            <li>
                <a href="mobile_report.aspx">
                    <img src="Images/report.png" />
                    Sestavy</a>
            </li>
            <%End If%>

            <li>
                <a href="mobile_myprofile.aspx">
                    <img src="Images/user.png" />
                    Můj profil</a>
            </li>
        </ul>
    </div>



</asp:Content>
