﻿<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Mobile.Master" CodeBehind="mobile_p28_framework.aspx.vb" Inherits="UI.mobile_p28_framework" %>

<%@ MasterType VirtualPath="~/Mobile.Master" %>
<%@ Register TagPrefix="uc" TagName="entityrole_assign_inline" Src="~/entityrole_assign_inline.ascx" %>
<%@ Register TagPrefix="uc" TagName="o23_list" Src="~/o23_list.ascx" %>
<%@ Register TagPrefix="uc" TagName="contactpersons" Src="~/contactpersons.ascx" %>
<%@ Register TagPrefix="uc" TagName="x18_readonly" Src="~/x18_readonly.ascx" %>
<%@ Register TagPrefix="uc" TagName="p31summary" Src="~/p31summary.ascx" %>
<%@ Register TagPrefix="uc" TagName="p28_address" Src="~/p28_address.ascx" %>
<%@ Register TagPrefix="uc" TagName="p28_medium" Src="~/p28_medium.ascx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript">
        function o23_record(o23id) {
            location.replace("mobile_o23_framework.aspx?pid=" + o23id);

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
                <asp:hyperlink ID="RecordHeader" runat="server" CssClass="navbar-brand" style="text-decoration:underline;"></asp:hyperlink>

            </div>
            <div class="collapse navbar-collapse" id="myNavbarOnSite">
                <ul class="nav navbar-nav">

                    <li><a href="mobile_report.aspx?prefix=p28&pid=<%=Master.DataPID%>">Sestava</a></li>
                    <li role="separator" class="divider"></li>
                    
                 

                   
                    
                </ul>
               
            </div>

    </nav>

    <div style="background-color:whitesmoke;margin-bottom:6px;">
    <ul class="nav nav-pills"> 
        <li role="presentation" id="liP41" runat="server" visible="false"><a href="mobile_grid.aspx?prefix=p41&masterprefix=p28&masterpid=<%=Master.DataPID%>">Projekty klienta
            <asp:Label runat="server" ID="CountP41" CssClass="badge"></asp:Label></a>

        </li>       
        <li role="presentation" id="liP56_Actual" runat="server" visible="false">
            <a href="mobile_grid.aspx?prefix=p56&masterprefix=p28&masterpid=<%=Master.DataPID%>&closed=0">Otevřené úkoly
            <asp:Label runat="server" ID="CountP56_Actual" CssClass="badge"></asp:Label></a>

        </li>
        <li role="presentation" id="liP56_Closed" runat="server" visible="false">
            <a href="mobile_grid.aspx?prefix=p56&masterprefix=p28&masterpid=<%=Master.DataPID%>&closed=1">Uzavřené úkoly
            <asp:Label runat="server" ID="CountP56_Closed" CssClass="badge"></asp:Label></a>

        </li>
        <li role="presentation" id="lisP91" runat="server" visible="false"><a href="mobile_grid.aspx?prefix=p91&masterprefix=p28&masterpid=<%=Master.DataPID%>">Vystavené faktury
            <asp:Label runat="server" ID="CountP91" CssClass="badge"></asp:Label></a>

        </li>

    </ul>
    </div>

    <div class="container-fluid">
        <div id="row1" class="row">
            <div class="col-sm-6 col-md-4" style="padding-left:1px;padding-right:1px;">
                <div class="thumbnail">
                    <div class="caption">
                        <img src="Images/contact.png" />
                        <asp:Label ID="RecordName" runat="server"></asp:Label>
                        <asp:Image ID="imgFlag_Project" runat="server" />
                    </div>
                    <table class="table table-hover">
                     
                        <tr id="trParent" runat="server" visible="false">
                        <td>
                            Nadřízený klient:

                        </td>
                        <td>                            
                            <asp:HyperLink ID="ParentClient" runat="server" Target="_top" CssClass="alinked"></asp:HyperLink>
                        </td>

                        </tr>
                        <tr id="trB02" runat="server" visible="false">
                            <td>Workflow stav:
                            </td>
                            <td>
                                <asp:Label ID="b02Name" runat="server"></asp:Label>
                            </td>
                        </tr>
                       
                        <tr id="trType" runat="server" visible="false">
                            <td>
                                <span>Typ klienta:</span>
                            </td>
                            <td>
                                <asp:Label ID="p29Name" runat="server"></asp:Label>
                                <asp:Image ID="imgDraft" runat="server" ImageUrl="Images/draft_icon.gif" Visible="false" AlternateText="DRAFT záznam" />
                            </td>
                        </tr>
                        <tr id="trP51" runat="server">
                            <td>
                                <span>Fakturační ceník:</span>
                            </td>
                            <td>
                                <asp:Label ID="PriceList_Billing" runat="server"></asp:Label>
                            </td>
                        </tr>
                      
                    </table>
                    <div style="padding:6px;">
                        <uc:p28_address ID="address1" runat="server" Visible="false"></uc:p28_address>
                        <uc:p28_medium ID="medium1" runat="server" Visible="false"></uc:p28_medium>
                    </div>
                </div>
            </div>



            <div class="col-sm-6 col-md-4" style="padding-left:1px;padding-right:1px;">
                <div class="thumbnail">
                    <div class="caption">
                        <img src="Images/projectrole.png" />
                        Klientské role
                    </div>
                    <table class="table table-hover">
                        <uc:entityrole_assign_inline ID="roles_project" runat="server" IsShowClueTip="false" IsRenderAsTable="true" EntityX29ID="p28Contact" NoDataText="V projektu nejsou přiřazeny projektové role!"></uc:entityrole_assign_inline>
                    </table>
                </div>
            </div>

            <asp:Panel ID="boxP31" runat="server" CssClass="col-sm-6 col-md-4" style="padding-left:1px;padding-right:1px;">
                <div class="thumbnail">
                    <div class="caption">
                        <img src="Images/worksheet.png" />
                        Nevyfakturované úkony
                        <asp:HyperLink ID="cmdP31Grid" runat="server" Text="Worksheet přehled" CssClass="alinked"></asp:HyperLink>                        
                       
                    </div>
                  
                        <uc:p31summary ID="worksheet1" runat="server"></uc:p31summary>
                    
                </div>
            </asp:Panel>

            <asp:Panel ID="boxP30" runat="server" CssClass="col-sm-6 col-md-4" style="padding-left:1px;padding-right:1px;">
                <div class="thumbnail">
                    <div class="caption">
                        <img src="Images/person.png" />
                        Kontaktní osoby
                        <asp:Label runat="server" ID="CountP30" CssClass="badge"></asp:Label>

                    </div>
                    <table class="table table-hover">
                        <uc:contactpersons ID="persons1" runat="server" IsShowClueTip="false"></uc:contactpersons>
                    </table>
                </div>
            </asp:Panel>

            <asp:Panel ID="boxO23" runat="server" CssClass="col-sm-6 col-md-4" style="padding-left:1px;padding-right:1px;">
                <div class="thumbnail">
                    <div class="caption">
                        <img src="Images/notepad.png" />

                        <a href="mobile_grid.aspx?prefix=o23&masterprefix=p28&masterpid=<%=Master.DataPID%>" class="alinked">Dokumenty <asp:Label runat="server" ID="CountO23" CssClass="badge"></asp:Label></a>
                        
                    </div>
                    <table class="table table-hover">
                        <uc:o23_list ID="notepad1" runat="server" EntityX29ID="p28Contact" IsShowClueTip="false"></uc:o23_list>
                    </table>
                </div>
            </asp:Panel>


            <asp:Panel ID="boxX18" runat="server" CssClass="col-sm-6 col-md-4" style="padding-left:1px;padding-right:1px;">
                <div class="thumbnail">
                    <div class="caption">
                        <img src="Images/label.png" />
                        Štítky
                        <asp:Label runat="server" ID="CountX18" CssClass="badge"></asp:Label>

                    </div>
                    
                        <uc:x18_readonly id="labels1" runat="server" AlwaysNoLinksAndButtons="true"></uc:x18_readonly>
                    
                </div>
            </asp:Panel>

        </div>
    </div>



</asp:Content>

