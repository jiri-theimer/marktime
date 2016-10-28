<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Mobile.Master" CodeBehind="mobile_start.aspx.vb" Inherits="UI.mobile_start" %>

<%@ MasterType VirtualPath="~/Mobile.Master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript">
       
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">


    <div class="container" >
       
        
        <ul class="nav nav-pills nav-stacked">
            <h3>
                <asp:Label ID="lblWelcome" runat="server"></asp:Label>
            </h3>
            <li style="background-color:#25a0da;">
                <a href="mobile_p31_calendar.aspx" style="color:white;">
                    <img src="Images/worksheet.png" />
                    WORKSHEET</a>
            </li>
         

            <%If Master.Factory.SysUser.j04IsMenu_Project Then%>
            <li style="background-color:#25a0da;">
                <a href="mobile_grid.aspx?prefix=p41" style="color:white;">
                    <img src="Images/project.png" />
                    PROJEKTY </a>
            </li>
            <%End If%>
            <li style="margin-left:10px;">
                <asp:HyperLink ID="linkLastProject" runat="server" Style="display: none;" NavigateUrl="mobile_p41_framework.aspx"></asp:HyperLink>
            </li>
            <li>
                <div style="width: 100%; border-top: 1px; border-color: whitesmoke; height: 1px; border-style: solid;"></div>
            </li>
            <%If Master.Factory.SysUser.j04IsMenu_Contact Then%>
            <li style="background-color:#72c0e5;">
                <a href="mobile_grid.aspx?prefix=p28" style="color:white;">
                    <img src="Images/contact.png" />
                    KLIENTI</a>
            </li>
            <li style="margin-left:10px;">
                <asp:HyperLink ID="linkLastClient" runat="server" Style="display: none;" NavigateUrl="mobile_p28_framework.aspx"></asp:HyperLink>
            </li>
            <li>
                <div style="width: 100%; border-top: 1px; border-color: whitesmoke; height: 1px; border-style: solid;"></div>
            </li>

            <%End If%>

            <%If Master.Factory.SysUser.j04IsMenu_Invoice Then%>
            <li style="background-color:#25a0da;">
                <a href="mobile_grid.aspx?prefix=p91" style="color:white;">
                    <img src="Images/invoice.png" />
                    FAKTURY</a>
            </li>
            <li style="margin-left:10px;">
                <asp:HyperLink ID="linkLastInvoice" runat="server" Style="display: none;" NavigateUrl="mobile_p91_framework.aspx"></asp:HyperLink>
            </li>
            <li>
                <div style="width: 100%; border-top: 1px; border-color: whitesmoke; height: 1px; border-style: solid;"></div>
            </li>
            <%End If%>

            <li style="background-color:#25a0da;">
                <a href="mobile_grid.aspx?prefix=p56" style="color:white;">
                    <img src="Images/task.png" />
                    ÚKOLY</a>
            </li>

            <%If Master.Factory.SysUser.j04IsMenu_Report Then%>
            <li style="background-color:#25a0da;">
                <a href="mobile_report.aspx" style="color:white;">
                    <img src="Images/report.png" />
                    SESTAVY</a>
            </li>
            <%End If%>

            <li style="background-color:#25a0da;">
                <a href="mobile_myprofile.aspx" style="color:white;">
                    <img src="Images/user.png" />
                    MůJ PROFIL</a>
            </li>
        </ul>
    </div>



</asp:Content>
