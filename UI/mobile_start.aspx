<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Mobile.Master" CodeBehind="mobile_start.aspx.vb" Inherits="UI.mobile_start" %>

<%@ MasterType VirtualPath="~/Mobile.Master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript">
       
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <nav class="navbar navbar-default" style="margin-bottom: 0px !important;">
            <div class="navbar-header">
                <button type="button" class="navbar-toggle" data-toggle="collapse" data-target="#myNavbarOnSite">
                    <span class="icon-bar"></span>
                    <span class="icon-bar"></span>
                    <span class="icon-bar"></span>
                </button>
                <asp:hyperlink ID="RecordHeader" runat="server" CssClass="navbar-brand" style="text-decoration:underline;" NavigateUrl="mobile_start.aspx"></asp:hyperlink>

               
                
            </div>
            <div class="collapse navbar-collapse" id="myNavbarOnSite">
                <ul class="nav navbar-nav">                   
                    <li>                        
                        <asp:HyperLink ID="linkEntryWorksheet" runat="server" NavigateUrl="mobile_p31_calendar.aspx" Text="<img src='Images/worksheet.png' /> WORKSHEET kalendář"></asp:HyperLink>
                    </li>
                    <li role="separator" class="divider"></li>                    
                    <li>
                        <asp:HyperLink ID="linkPersonalReports" runat="server" NavigateUrl="mobile_p41_framework.aspx" Text="<img src='Images/report.png' /> Osobní sestavy"></asp:HyperLink>
                    </li>
                    <li role="separator" class="divider"></li>
                    <li><asp:HyperLink ID="linkLastProject" runat="server" NavigateUrl="mobile_p41_framework.aspx"></asp:HyperLink></li>                    
                    <li><asp:HyperLink ID="linkLastClient" runat="server" NavigateUrl="mobile_p28_framework.aspx"></asp:HyperLink></li>
                    
                    <li role="separator" class="divider"></li>
                    
                    

                   
                    
                </ul>
               
            </div>

    </nav>
</asp:Content>
