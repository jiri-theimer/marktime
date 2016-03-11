<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/SubForm.Master" CodeBehind="clue_o23_record.aspx.vb" Inherits="UI.clue_o23_record" %>

<%@ MasterType VirtualPath="~/SubForm.Master" %>
<%@ Register TagPrefix="uc" TagName="fileupload_list" Src="~/fileupload_list.ascx" %>
<%@ Register TagPrefix="uc" TagName="b07_list" Src="~/b07_list.ascx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <script type="text/javascript">
        function detail() {

            window.parent.sw_local("o23_record.aspx?pid=<%=Master.DataPID%>", "Images/notepad_32.png", true);

        }
        function go2module() {

            window.open("o23_framework.aspx?pid=<%=Master.DataPID%>", "_top");

        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <asp:Panel ID="panContainer" runat="server" Style="max-height: 270px; overflow-x: auto; overflow-y: auto; width: 100%;">
       <div>
            <asp:Image ID="img1" runat="server" ImageUrl="Images/notepad_32.png" />
            <asp:Label ID="ph1" runat="server" CssClass="clue_header_span"></asp:Label>
        </div>

        
        <div class="content-box2">
            <div class="title">
                
                Záznam dokumentu
               
                
                 <a style="margin-left:20px;" href="javascript:go2module()">Přejít do rozhraní [Dokumenty]</a>
            </div>
            <div class="content">
                <table cellpadding="6">
                    
                    <tr id="trO23Name" runat="server">
                        <td>Název:</td>
                        <td>
                            <asp:Label ID="o23Name" runat="server" CssClass="valbold"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td>Datum:</td>
                        <td>
                            <asp:Label ID="o23Date" runat="server" CssClass="valbold"></asp:Label>
                        </td>
                    </tr>
                    <tr id="trBind" runat="server">
                        <td>
                            <asp:Label ID="BindName" runat="server"></asp:Label>
                        </td>
                        <td>
                            <asp:Label ID="BindValue" runat="server" CssClass="valboldred"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td>Vlastník:</td>
                        <td>
                            <asp:Label ID="Owner" runat="server" CssClass="valbold"></asp:Label>
                        </td>
                    </tr>
                    <tr id="trWorkflow" runat="server">
                        <td>Workflow stav:</td>
                        <td>
                            <asp:Label ID="b02Name" runat="server" CssClass="valbold"></asp:Label>
                        </td>
                    </tr>
                </table>
            </div>
        </div>
        <div class="div6">
            <asp:Label ID="Timestamp" runat="server" CssClass="timestamp"></asp:Label>
        </div>

        <asp:Panel ID="panBody" runat="server" CssClass="content-box1">
            <div class="title">Podrobný popis</div>
            <div class="content" style="background-color: #ffffcc;">
                <asp:Label ID="o23BodyPlainText" runat="server" CssClass="val" Style="font-family: 'Courier New'; word-wrap: break-word; display: block; font-size: 120%;"></asp:Label>
            </div>
        </asp:Panel>

        <asp:Label ID="lblMessage" runat="server" CssClass="failureNotification"></asp:Label>

        <asp:panel ID="panFiles" runat="server" cssclass="content-box1">
            <div class="title">
                Souborové přílohy
                

            </div>
            <div class="content">
                <uc:fileupload_list ID="files1" runat="server" />
            </div>
        </asp:panel>

        

        <uc:b07_list ID="comments1" runat="server" ShowInsertButton="false" ShowHeader="false" />

    </asp:Panel>


</asp:Content>

