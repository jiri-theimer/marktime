<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/SubForm.Master" CodeBehind="o23_framework_detail.aspx.vb" Inherits="UI.o23_framework_detail" %>
<%@ MasterType VirtualPath="~/SubForm.Master" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<%@ Register TagPrefix="uc" TagName="entity_menu" Src="~/entity_menu.ascx" %>
<%@ Register TagPrefix="uc" TagName="freefields_readonly" Src="~/freefields_readonly.ascx" %>
<%@ Register TagPrefix="uc" TagName="entityrole_assign_inline" Src="~/entityrole_assign_inline.ascx" %>
<%@ Register TagPrefix="uc" TagName="b07_list" Src="~/b07_list.ascx" %>
<%@ Register TagPrefix="uc" TagName="fileupload_list" Src="~/fileupload_list.ascx" %>
<%@ Register TagPrefix="uc" TagName="dropbox" Src="~/dropbox.ascx" %>
<%@ Register TagPrefix="uc" TagName="fileupload" Src="~/fileupload.ascx" %>
<%@ Register TagPrefix="uc" TagName="imap_record" Src="~/imap_record.ascx" %>
<%@ Register TagPrefix="uc" TagName="x18_readonly" Src="~/x18_readonly.ascx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
   
    <script type="text/javascript">
       
        function hardrefresh(pid, flag) {
            if (flag=="o23-save"){                
                parent.window.location.replace("o23_framework.aspx?pid="+pid);
                return;
            }
            if (flag=="o23-delete"){
                parent.window.location.replace("o23_framework.aspx");
                return;
            }

            document.getElementById("<%=Me.hidHardRefreshPID.ClientID%>").value = pid;
            document.getElementById("<%=Me.hidHardRefreshFlag.ClientID%>").value = flag;

            <%=Me.ClientScript.GetPostBackEventReference(Me.cmdRefresh, "", False)%>;

        }

       
        function b07_reaction(b07id) {
            sw_local("b07_create.aspx?parentpid="+b07id+"&masterprefix=o23&masterpid=<%=master.datapid%>","Images/comment.png", true)
           
        }
        function b07_delete(b07id, flag) {
            sw_decide("b07_delete.aspx?pid=" + b07id, "Images/delete.png", true)

        }

        
        function dropbox_folder(o25id) {
            sw_local("dropbox_folder.aspx?x29id=223&pid=<%=Master.DataPID%>&o25id=" + o25id, "Images/dropbox.png", false);
        }

        
        
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <uc:entity_menu ID="menu1" runat="server"></uc:entity_menu>
    <div style="height: 10px; clear: both;"></div>
   
    <asp:Panel ID="tableRecord" runat="server">

        <div class="content-box1">
            <div class="title">
                <asp:Image ID="imgRecord" runat="server" ImageUrl="Images/properties.png" Style="margin-right: 10px;" />
                <asp:Label ID="boxCoreTitle" Text="Záznam dokumentu" runat="server"></asp:Label>
                
            </div>
            <div class="content">
                <asp:Label ID="lblPermissionMessage" runat="server" CssClass="infoNotificationRed"></asp:Label>
                <table cellpadding="10" cellspacing="2" id="responsive">
                    <tr valign="top">
                        
                        <td colspan="2">

                            <asp:Label ID="o23Name" runat="server" CssClass="valbold"></asp:Label>
                            <asp:Image ID="imgDraft" runat="server" Visible="false" ImageUrl="Images/draft_icon.gif" AlternateText="DRAFT záznam" Style="float: right;" />
                            <asp:Panel ID="panDraftCommands" runat="server" Visible="false">
                                <button type="button" onclick="draft2normal()">
                                    Převést z režimu DRAFT na oficiální záznam
                                </button>
                            </asp:Panel>

                        </td>


                    </tr>

                    <tr valign="top">
                        <td>
                            <asp:Label ID="lblType" runat="server" Text="Typ dokumentu:" CssClass="lbl"></asp:Label>

                        </td>
                        <td>
                            <asp:HyperLink ID="clue_o24" runat="server" CssClass="reczoom" Text="i" title="Detail typu dokumentu"></asp:HyperLink>
                            <asp:Label ID="o24Name" runat="server" CssClass="valbold"></asp:Label>

                        </td>

                    </tr>

                    <tr id="trWorkflow" runat="server">
                        <td>
                            <asp:Label ID="lblB02ID" runat="server" Text="Workflow stav:" CssClass="lbl"></asp:Label>
                        </td>
                        <td>
                            <asp:Label ID="b02Name" runat="server" CssClass="valboldred"></asp:Label>
                            <img src="Images/workflow.png" />
                            <asp:HyperLink ID="cmdWorkflow" runat="server" Text="Posunout/doplnit" NavigateUrl="javascript: workflow()"></asp:HyperLink>
                        </td>
                    </tr>
                    <tr valign="top">
                        <td>
                            <asp:Label ID="lblBind" runat="server" Text="Primární vazba:" CssClass="lbl"></asp:Label>

                        </td>
                        <td>
                            <asp:HyperLink ID="clue_bind_entity" runat="server" CssClass="reczoom" Text="i" title="Detail projektu"></asp:HyperLink>
                            <asp:HyperLink ID="BindEntity" runat="server" Target="_parent"></asp:HyperLink>

                        </td>

                    </tr>
                    <tr valign="top" id="trBindTemp" runat="server" visible="false">
                        <td>
                            <asp:Label ID="lblBindTemp" runat="server" Text="Dočasná vazba:" CssClass="lbl"></asp:Label>

                        </td>
                        <td>
                            <asp:HyperLink ID="clue_bindtemp_entity" runat="server" CssClass="reczoom" Text="i" title="Detail projektu"></asp:HyperLink>
                            <asp:HyperLink ID="BindTempEntity" runat="server" Target="_parent"></asp:HyperLink>

                        </td>

                    </tr>
                    <tr>
                        <td>
                            <asp:Label ID="lblDate" runat="server" Text="Datum:" CssClass="lbl"></asp:Label>
                        </td>
                        <td>
                            <asp:Label ID="o23Date" runat="server" CssClass="valbold"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label ID="Label1" runat="server" CssClass="lbl" Text="Vlastník dokumentu:"></asp:Label>

                        </td>
                        <td>
                            <asp:Label ID="Owner" runat="server" CssClass="valbold"></asp:Label>


                        </td>
                    </tr>

                </table>
                
                <div class="div6">
                    <uc:entityrole_assign_inline ID="roles_notepad" runat="server" EntityX29ID="o23Notepad" NoDataText=""></uc:entityrole_assign_inline>
                </div>
                <asp:Label ID="Timestamp" runat="server" CssClass="timestamp"></asp:Label>
            </div>
        </div>

        <asp:panel ID="boxX18" runat="server" CssClass="content-box1">
            <div class="title">
                <img src="Images/label.png" style="margin-right: 10px;" />
                <asp:Label ID="boxX18Title" runat="server" Text="Štítky"></asp:Label>
                <asp:HyperLink ID="x18_binding" runat="server" Text="Přiřadit"></asp:HyperLink>
            </div>
            <div class="content">
                <uc:x18_readonly id="labels1" runat="server"></uc:x18_readonly>
            </div>
        </asp:panel>

        <div class="content-box1">
            <div class="title">
                <img src="Images/attachment.png" style="margin-right: 10px;" />
                <asp:HyperLink ID="filesPreview" runat="server" Text="Souborové přílohy"></asp:HyperLink>

            </div>
            <div class="content">
                <uc:fileupload_list ID="Fileupload_list__readonly" runat="server" OnClientClickPreview="file_preview" />

                <uc:fileupload ID="upload1" runat="server" EntityX29ID="o23Notepad" MaxFileSize="5242880" />

            </div>


        </div>


        <asp:Panel ID="boxFF" runat="server" CssClass="content-box1">
            <div class="title">
                <img src="Images/form.png" style="margin-right: 10px;" />
                <asp:Label ID="boxFFTitle" runat="server" Text="Další pole dokumentu"></asp:Label>
                <asp:CheckBox ID="chkFFShowFilledOnly" runat="server" AutoPostBack="true" Text="Zobrazovat pouze vyplněná pole" />
            </div>
            <div class="content">
                <uc:freefields_readonly ID="ff1" runat="server" />
            </div>

        </asp:Panel>
        <asp:Panel ID="boxIMAP" runat="server" CssClass="content-box1" Visible="false">
            <div class="title">
                <img src="Images/imap.png" style="margin-right: 10px;" />
                <span>Dokument byl vygenerován poštovní zprávou</span>
            </div>
            <div class="content">
                <uc:imap_record ID="imap1" runat="server"></uc:imap_record>
            </div>
        </asp:Panel>
        <asp:Panel ID="boxDropbox" runat="server" CssClass="content-box1">
            <div class="title">
                <img src="Images/dropbox.png" style="margin-right: 10px;" />
                <span>Dropbox</span>
            </div>
            <div class="content">
                <uc:dropbox ID="dropbox1" runat="server" />
            </div>

        </asp:Panel>

    </asp:Panel>
    <div style="clear: both; width: 100%;"></div>

    <asp:Panel ID="panBody" runat="server" CssClass="content-box1">
        <div class="title">Podrobný popis</div>
        <div class="content" style="background-color: #ffffcc;">
            <asp:Label ID="o23BodyPlainText" runat="server" CssClass="val" Style="font-family: 'Courier New'; word-wrap: break-word; display: block; font-size: 120%;"></asp:Label>
        </div>
    </asp:Panel>
    <asp:Panel ID="panEntryPassword" runat="server" Visible="false" CssClass="content-box2">

        <div class="title">
            <img src="Images/spy.png" style="margin-right: 6px;" />Obsah dokumentu je zašifrován, zadejte heslo...
        </div>
        <div class="content">
            <asp:TextBox ID="txtEntryPassword" runat="server" Style="width: 130px;" TextMode="Password"></asp:TextBox>
            <asp:Button ID="cmdDecrypt" runat="server" CssClass="cmd" Text="Odemknout" />
        </div>

    </asp:Panel>
    
    <div style="clear:both;"></div>
    <uc:b07_list ID="comments1" runat="server" JS_Create="menu_b07_record()" JS_Reaction="b07_reaction" />


    <asp:HiddenField ID="hidHardRefreshFlag" runat="server" />
    <asp:HiddenField ID="hidHardRefreshPID" runat="server" />
    

    <asp:Button ID="cmdRefresh" runat="server" Style="display: none;" />
    
    
    <script type="text/javascript">
        function file_preview(prefix,pid) {
            ///náhled na soubor
            //window.parent.sw_master("fileupload_preview.aspx?prefix="+prefix+"&pid="+pid,"Images/attachment_32.png",true);
            sw_local("fileupload_preview.aspx?prefix="+prefix+"&pid="+pid,"Images/attachment.png",true);
            
        }
    </script>
</asp:Content>

