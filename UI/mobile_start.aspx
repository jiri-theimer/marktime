<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Mobile.Master" CodeBehind="mobile_start.aspx.vb" Inherits="UI.mobile_start" %>

<%@ MasterType VirtualPath="~/Mobile.Master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <div class="page-header">
        <h3>
            <asp:Label ID="lblWelcome" runat="server"></asp:Label><small>,vítejte v MARKTIME!</small>
        </h3>

    </div>
    <div class="list-group">
        <a class="list-group-item" href="mobile_grid.aspx?prefix=p31"><img src="Images/worksheet.png" /> Worksheet</a>

        <%If Master.Factory.SysUser.j04IsMenu_Project Then%>
        
        <a class="list-group-item" href="mobile_grid.aspx?prefix=p41"><img src="Images/project.png" /> Projekty </a>
                
        <%End If%>
        <asp:HyperLink ID="linkLastProject" runat="server" style="display:none;"  CssClass="list-group-item" NavigateUrl="mobile_p41_framework.aspx"></asp:HyperLink>

        <%If Master.Factory.SysUser.j04IsMenu_Contact Then%>

        <a class="list-group-item" href="mobile_grid.aspx?prefix=p28"><img src="Images/contact.png" /> Klienti</a>
        <asp:HyperLink ID="linkLastClient" runat="server" style="display:none;"  CssClass="list-group-item" NavigateUrl="mobile_p28_framework.aspx"></asp:HyperLink>

        <%End If%>

        <a class="list-group-item" href="mobile_grid.aspx?prefix=p56"><img src="Images/task.png" /> Úkoly</a>

        <%If Master.Factory.SysUser.j04IsMenu_Invoice Then%>

        <a class="list-group-item" href="mobile_grid.aspx?prefix=p91"><img src="Images/invoice.png" /> Faktury</a>

        <asp:HyperLink ID="linkLastInvoice" runat="server" style="display:none;" CssClass="list-group-item" NavigateUrl="mobile_p91_framework.aspx"></asp:HyperLink>

        <%End If%>

        <%If Master.Factory.SysUser.j04IsMenu_Report Then%>

        <a class="list-group-item" href="mobile_report.aspx"><img src="Images/report.png" /> Sestavy</a>
        
        <%End If%>


        <a class="list-group-item" href="mobile_myprofile.aspx"><img src="Images/user.png" /> Můj profil</a>

    </div>

</asp:Content>
