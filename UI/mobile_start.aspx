<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Mobile.Master" CodeBehind="mobile_start.aspx.vb" Inherits="UI.mobile_start" %>

<%@ MasterType VirtualPath="~/Mobile.Master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript">
        $(document).ready(function () {

            document.getElementById("resolution").innerText = screen.width + "x" + screen.height;

        });

        function p31_entry() {
            location.replace("mobile_p31_framework.aspx?pid=0&source=calendar");
        }
        function p31_calendar() {
            location.replace("mobile_p31_calendar.aspx");
        }
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
                        <span style="padding-left:20px;">Displej:</span>
                        <span id="resolution"></span>
                    </li>               
                    <li>                        
                        <asp:HyperLink ID="linkEntryWorksheet" runat="server" NavigateUrl="mobile_p31_calendar.aspx" Text="<img src='Images/worksheet.png' /> WORKSHEET kalendář"></asp:HyperLink>
                    </li>
                    <li role="separator" class="divider"></li>
                    <li>
                        <asp:HyperLink ID="linkCreateTask" runat="server" NavigateUrl="mobile_p56_create.aspx?source=start" Text="<img src='Images/task.png' /> Vytvořit úkol"></asp:HyperLink>
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


    <div class="container-fluid">
        <div id="row1" class="row">
            
            <div class="col-sm-6 col-md-4">
                <div>Můj poslední úkon:</div>
                <div>
                    <asp:HyperLink ID="LastWorksheet" runat="server" CssClass="alinked"></asp:HyperLink>
                    <button type="button" class="btn btn-primary btn-xs" onclick="javascript:p31_entry()" style="margin: 6px;">Zapsat nový</button>
                    <button type="button" class="btn btn-primary btn-xs" onclick="javascript:p31_calendar()" style="margin: 6px;">Kalendář</button>
                </div>


            </div>

            <asp:Panel ID="panP56" runat="server" CssClass="col-sm-6 col-md-4" style="padding-left:1px;padding-right:1px;">
                <div class="thumbnail">
                    <div class="caption">
                        <img src="Images/task.png" />
                        Otevřené úkoly s termínem
                        <asp:Label runat="server" ID="CountP56" CssClass="badge"></asp:Label>
                    </div>
                    <table class="table table-condensed">
                        <asp:Repeater ID="rp1" runat="server">
                            <ItemTemplate>
                                <tr>
                                    <td>
                                        <a href="mobile_p56_framework.aspx?pid=<%#Eval("pid")%>" class="alinked"><%#Eval("p56Code")%></a>
                                    </td>
                                    <td>
                                        <i><%#Eval("p56Name")%></i>
                                        <span style="color:red;font-weight:bold;"><%#BO.BAS.FD(Eval("p56PlanUntil"),True,true)%></span>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2">
                                        <span><%#Eval("p41Name")%></span>
                                    </td>
                                </tr>
                            </ItemTemplate>
                        </asp:Repeater>
                    </table>
                </div>
            </asp:Panel>


        </div>
    </div>
</asp:Content>
