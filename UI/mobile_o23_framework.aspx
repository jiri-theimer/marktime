<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Mobile.Master" CodeBehind="mobile_o23_framework.aspx.vb" Inherits="UI.mobile_o23_framework" %>

<%@ MasterType VirtualPath="~/Mobile.Master" %>
<%@ Register TagPrefix="uc" TagName="entityrole_assign_inline" Src="~/entityrole_assign_inline.ascx" %>
<%@ Register TagPrefix="uc" TagName="x18_readonly" Src="~/x18_readonly.ascx" %>
<%@ Register TagPrefix="uc" TagName="fileupload_list" Src="~/fileupload_list.ascx" %>


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
                <asp:hyperlink ID="RecordHeader" runat="server" CssClass="navbar-brand" style="text-decoration:underline;"></asp:hyperlink>

               
                
            </div>
            <div class="collapse navbar-collapse" id="myNavbarOnSite">
                <ul class="nav navbar-nav">                   
                   
                    <li><a href="mobile_report.aspx?prefix=o23&pid=<%=Master.DataPID%>">Sestava</a></li>
                    
                    
                </ul>
               
            </div>

    </nav>


    <asp:Panel ID="panEntryPassword" runat="server" Visible="false" CssClass="panel panel-default">
        <div class="panel-heading">
            <img src="Images/spy.png" style="margin-right: 6px;" />Obsah dokumentu je zašifrován, zadejte heslo...
        </div>
        <div class="panel-body">
            <asp:TextBox ID="txtEntryPassword" runat="server" Style="width: 130px;" TextMode="Password" AutoCompleteType="Disabled"></asp:TextBox>
            <asp:LinkButton ID="cmdDecrypt" CssClass="btn btn-primary btn-xs" runat="server" Text="Odemknout"></asp:LinkButton>
        </div>
    </asp:Panel>
    <asp:Panel ID="panRecord" runat="server" CssClass="container-fluid">
        <div id="row1" class="row">
            <div class="col-sm-6 col-md-4">
                <div class="thumbnail">
                    <div class="caption">
                        <asp:Image ID="imgRecord" runat="server" ImageUrl="Images/notepad.png" />                        
                        <asp:Label ID="RecordName" runat="server"></asp:Label>

                    </div>
                    <table class="table table-hover">
                        <tr>
                            <td>
                                <span>Typ:</span>
                            </td>
                            <td>
                                <asp:Label ID="o24Name" runat="server"></asp:Label>
                                <asp:Image ID="imgDraft" runat="server" ImageUrl="Images/draft_icon.gif" Visible="false" AlternateText="DRAFT záznam" />

                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="lblBind" runat="server"></asp:Label>
                            </td>
                            <td>
                                <asp:HyperLink ID="BindEntity" runat="server" CssClass="alinked"></asp:HyperLink>

                            </td>
                        </tr>
                        <tr valign="top" id="trBindTemp" runat="server" visible="false">
                            <td>
                                <asp:Label ID="lblBindTemp" runat="server" Text="Dočasná vazba:" CssClass="lbl"></asp:Label>

                            </td>
                            <td>
                                <asp:HyperLink ID="BindTempEntity" runat="server" CssClass="alinked"></asp:HyperLink>

                            </td>

                        </tr>

                        <tr id="trB02" runat="server">
                            <td>Workflow stav:
                            </td>
                            <td>
                                <asp:Label ID="b02Name" runat="server"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td>Datum:
                            </td>
                            <td>
                                <asp:Label ID="o23Date" runat="server" CssClass="label label-success"></asp:Label>

                            </td>
                        </tr>




                    </table>



                </div>
            </div>
            
            <asp:panel ID="panFiles" cssclass="col-sm-6 col-md-4" runat="server">
                <div class="thumbnail">
                    <div class="caption">
                        Souborové přílohy
                    </div>
                    <uc:fileupload_list ID="Fileupload_list__readonly" runat="server" OnClientClickPreview="" />
                </div>
            </asp:panel>

            <asp:panel ID="panBody" cssclass="col-sm-6 col-md-4" runat="server">
                <div class="thumbnail">
                    <div class="caption">
                        Podrobný popis
                    </div>
                    <asp:Label ID="o23BodyPlainText" runat="server" Font-Italic="true"></asp:Label>
                </div>
            </asp:panel>



            <div class="col-sm-6 col-md-4">
                <div class="thumbnail">

                    <div class="caption">
                        <img src="Images/projectrole.png" />
                        Přiřazené role
                    </div>
                    <table class="table table-hover">
                        <uc:entityrole_assign_inline ID="roles_project" runat="server" IsShowClueTip="false" IsRenderAsTable="true" EntityX29ID="o23Notepad" NoDataText=""></uc:entityrole_assign_inline>
                    </table>
                    <div>
                        <span>Vlastník:</span>
                        <asp:Label ID="Owner" runat="server"></asp:Label>
                    </div>
                    <div>
                        <asp:Label ID="Timestamp" runat="server" Font-Italic="true"></asp:Label>
                    </div>
                </div>
            </div>




            <asp:Panel ID="boxX18" runat="server" CssClass="col-sm-6 col-md-4">
                <div class="thumbnail">
                    <div class="caption">
                        <img src="Images/label.png" />
                        Štítky
                        <asp:Label runat="server" ID="CountX18" CssClass="badge"></asp:Label>

                    </div>

                    <uc:x18_readonly ID="labels1" runat="server"></uc:x18_readonly>

                </div>
            </asp:Panel>

        </div>
    </asp:Panel>



</asp:Content>

