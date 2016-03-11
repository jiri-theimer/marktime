<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/ModalForm.Master" CodeBehind="sendmail.aspx.vb" Inherits="UI.sendmail" %>

<%@ MasterType VirtualPath="~/ModalForm.Master" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<%@ Register TagPrefix="uc" TagName="fileupload" Src="~/fileupload.ascx" %>
<%@ Register TagPrefix="uc" TagName="fileupload_list" Src="~/fileupload_list.ascx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <script type="text/javascript">
        function j61_create() {
           
            dialog_master("j61_record.aspx?pid=0&x29id=940", true);
            return (false);
        }
        function j61_edit() {

            dialog_master("j61_record.aspx?pid=<%=me.j61id.selectedvalue%>&x29id=940", true);
            return (false);
        }
        function j61_clone() {

            dialog_master("j61_record.aspx?clone=1&pid=<%=me.j61id.selectedvalue%>&x29id=940", true);
            return (false);
        }

        function hardrefresh(pid, flag) {

           
            document.getElementById("<%=Me.hidHardRefreshPID.ClientID%>").value = pid;
            document.getElementById("<%=Me.hidHardRefreshFlag.ClientID%>").value = flag;

            <%=Me.ClientScript.GetPostBackEventReference(Me.cmdRefresh, "", False)%>;

        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="OverMainContent" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="server">
    
    <div class="content-box2">
        <div class="title">
            Příjemce zprávy
            <asp:DropDownList ID="cbxAddPerson" runat="server" DataTextField="FullNameDescWithEmail" DataValueField="pid" AutoPostBack="true" Style="width: 150px;"></asp:DropDownList>
            <asp:DropDownList ID="cbxAddContactPerson" runat="server" DataTextField="FullNameDescWithJobTitle" DataValueField="pid" AutoPostBack="true" Style="width: 150px;"></asp:DropDownList>


            <asp:DropDownList ID="cbxAddTeam" runat="server" DataTextField="j11Name" DataValueField="pid" AutoPostBack="true" Style="width: 150px;"></asp:DropDownList>


            <asp:DropDownList ID="cbxAddPosition" runat="server" DataTextField="j07Name" DataValueField="pid" AutoPostBack="true" Style="width: 150px;"></asp:DropDownList>

        </div>
        <div class="content">
            <table cellpadding="5" cellspacing="2">

                <tr>
                    <td>
                        <asp:Label ID="lblTo" runat="server" CssClass="lblReq" Text="Komu:"></asp:Label>

                    </td>
                    <td>
                        <div>
                        </div>
                        <asp:TextBox ID="txtTo" runat="server" Style="width: 700px; height: 44px;" TextMode="MultiLine"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="lblCC" runat="server" CssClass="lbl" Text="V kopii (Cc):"></asp:Label>

                    </td>
                    <td>
                        <asp:TextBox ID="txtCC" runat="server" Style="width: 700px;"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="lblBCC" runat="server" CssClass="lbl" Text="Skrytá kopie (Bcc):"></asp:Label>

                    </td>
                    <td>
                        <asp:TextBox ID="txtBCC" runat="server" Style="width: 700px;"></asp:TextBox>
                    </td>
                </tr>
                <tr valign="top">
                    <td>
                        <asp:Label ID="lblSubject" runat="server" CssClass="lbl" Text="Předmět zprávy:"></asp:Label>

                    </td>
                    <td>
                        <asp:TextBox ID="txtSubject" runat="server" Style="width: 700px;"></asp:TextBox>
                    </td>
                </tr>
            </table>
        </div>
    </div>

    <uc:fileupload ID="upload1" runat="server" EntityX29ID="x40MailQueue" />
    <uc:fileupload_list ID="uploadlist1" runat="server" />

    <div class="content-box2" style="margin-top:10px;">
        <div class="title">
            Obsah zprávy (Body)
           
            <asp:DropDownList ID="j61ID" runat="server" AutoPostBack="true" DataValueField="PID" DataTextField="j61Name" style="width:350px;"></asp:DropDownList>

            
                <asp:ImageButton ID="cmdEdit" runat="server" ImageUrl="Images/edit.png" ToolTip="Upravit šablonu" OnClientClick="return j61_edit()" CssClass="button-link" />
                <asp:ImageButton ID="cmdNew" runat="server" ImageUrl="Images/new.png" ToolTip="Nová šablona" OnClientClick="return j61_create()" CssClass="button-link" />
                <asp:ImageButton ID="cmdClone" runat="server" ImageUrl="Images/copy.png" ToolTip="Kopírovat šablonu" OnClientClick="return j61_clone()" CssClass="button-link" />


        </div>
        <div class="content">
            <asp:TextBox ID="txtBody" runat="server" TextMode="MultiLine" Style="width: 99%; height: 200px;"></asp:TextBox>
        </div>
    </div>


    
    
    <asp:HiddenField ID="hidX29ID" runat="server" Value="102" />
    <asp:HiddenField ID="hidPrefix" runat="server" Value="j02" />
    <asp:HiddenField ID="hidHardRefreshPID" runat="server" />
    <asp:HiddenField ID="hidHardRefreshFlag" runat="server" />
    <asp:Button ID="cmdRefresh" runat="server" Style="display: none;" />
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="FootContent" runat="server">
</asp:Content>
