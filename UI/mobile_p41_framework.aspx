<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Mobile.Master" CodeBehind="mobile_p41_framework.aspx.vb" Inherits="UI.mobile_p41_framework" %>
<%@ MasterType VirtualPath="~/Mobile.Master" %>
<%@ Register TagPrefix="uc" TagName="entityrole_assign_inline" Src="~/entityrole_assign_inline.ascx" %>
<%@ Register TagPrefix="uc" TagName="o23_list" Src="~/o23_list.ascx" %>
<%@ Register TagPrefix="uc" TagName="contactpersons" Src="~/contactpersons.ascx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript">
        function o23_record(o23id) {
            alert("nic");
            
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">



    <nav class="navbar navbar-default">
            <div class="navbar-header">
                <button type="button" class="navbar-toggle" data-toggle="collapse" data-target="#myNavbarOnSite">
                    <span class="icon-bar"></span>
                    <span class="icon-bar"></span>
                    <span class="icon-bar"></span>
                </button>
                <img src="Images/project_32.png" class="navbar-brand"/>
                <asp:hyperlink ID="RecordHeader" runat="server" CssClass="navbar-brand"></asp:hyperlink>

               
                
            </div>
            <div class="collapse navbar-collapse" id="myNavbarOnSite">
                <ul class="nav navbar-nav">
                    <li><a href="p31_framework_mobile.aspx?p41id=<%=Master.DataPID%>">Zapsat úkon</a></li>
                    <li role="separator" class="divider"></li>
                    <li><a href="p41_framework_mobile.aspx">Sestava</a></li>
                  <li role="separator" class="divider"></li>
                    
                    <li><a href="default.aspx">Vytvořit úkol</a></li>

                   
                    
                </ul>
               
            </div>

    </nav>

    <div class="container-fluid">
        <div id="row1" class="row">                       
            <div class="col-sm-6 col-md-4">
                <div class="thumbnail">
                    <div class="caption">
                        <img src="Images/record.png" />
                        <asp:Label ID="RecordName" runat="server"></asp:Label>                        
                    </div>
                    <table class="table table-hover">
                        <tr>
                            <td>
                                <span>Klient:</span>
                            </td>
                            <td>
                                <asp:HyperLink ID="Client" runat="server"></asp:HyperLink>
                                <asp:Image ID="imgFlag_Client" runat="server" />
                            </td>
                        </tr>
                        <tr id="trB02" runat="server">
                            <td>
                                Workflow stav:
                            </td>
                            <td>
                                <asp:Label ID="b02Name" runat="server"></asp:Label>
                            </td>
                        </tr>
                        <tr id="trPlanPeriod" runat="server">
                            <td>
                                Zahájení/dokončení:
                            </td>
                            <td>
                                <asp:Label ID="p41PlanFrom" runat="server" CssClass="label label-success"></asp:Label>
                                <asp:Label ID="p41PlanUntil" runat="server" CssClass="label label-danger"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <span>Typ projektu:</span>
                            </td>
                            <td>
                                <asp:Label ID="p42Name" runat="server"></asp:Label>
                                <asp:Image ID="imgFlag_Project" runat="server" />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <span>Fakturační ceník:</span>
                            </td>
                            <td>
                                <asp:Label ID="PriceList_Billing" runat="server"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2">
                                <asp:Image ID="imgDraft" runat="server" ImageUrl="Images/draft_icon.gif" Visible="true" AlternateText="DRAFT záznam" />
                                 
                                
                            </td>
                        </tr>
                    </table>
                    
                 
                    
                </div>
            </div>

             <div class="col-sm-6 col-md-4">
                <div class="thumbnail">
                    <div class="caption">
                        <img src="Images/projectrole.png" />
                        Projektové role
                    </div>
                    <table class="table table-hover">
                    <uc:entityrole_assign_inline ID="roles_project" runat="server" IsShowClueTip="false" IsRenderAsTable="true" EntityX29ID="p41Project" NoDataText="V projektu nejsou přiřazeny projektové role!"></uc:entityrole_assign_inline>
                    </table>
                </div>
            </div>

            <asp:panel ID="boxP30" runat="server" CssClass="col-sm-6 col-md-4">
                <div class="thumbnail">
                    <div class="caption">
                        <img src="Images/person.png" />
                        <asp:HyperLink ID="titleP30" runat="server" Text="Kontaktní osoby" NavigateUrl="#"></asp:HyperLink>
                        <asp:label runat="server" ID="CountP30" cssclass="badge"></asp:label>
                        
                    </div>
                    <table class="table table-hover">
                        <uc:contactpersons ID="persons1" runat="server" IsShowClueTip="false"></uc:contactpersons>
                    </table>
                </div>
            </asp:panel>

            <asp:panel ID="boxO23" runat="server" CssClass="col-sm-6 col-md-4">
                <div class="thumbnail">
                    <div class="caption">
                        <img src="Images/notepad.png" />
                        <asp:HyperLink ID="titleO23" runat="server" Text="Dokumenty" NavigateUrl="#"></asp:HyperLink>
                        <asp:label runat="server" ID="CountO23" cssclass="badge"></asp:label>
                        
                    </div>
                    <table class="table table-hover">
                    <uc:o23_list ID="notepad1" runat="server" EntityX29ID="p41Project" IsShowClueTip="false"></uc:o23_list>
                    </table>
                </div>
            </asp:panel>


            
        </div>
    </div>


    
</asp:Content>
