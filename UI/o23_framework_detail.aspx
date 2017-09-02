<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/SubForm.Master" CodeBehind="o23_framework_detail.aspx.vb" Inherits="UI.o23_framework_detail" %>

<%@ MasterType VirtualPath="~/SubForm.Master" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<%@ Register TagPrefix="uc" TagName="b07_list" Src="~/b07_list.ascx" %>
<%@ Register TagPrefix="uc" TagName="entityrole_assign_inline" Src="~/entityrole_assign_inline.ascx" %>
<%@ Register TagPrefix="uc" TagName="o23_record_readonly" Src="~/o23_record_readonly.ascx" %>
<%@ Register TagPrefix="uc" TagName="fileupload_list" Src="~/fileupload_list.ascx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <script type="text/javascript">

        function hardrefresh(pid, flag) {              
            <%If hidSource.Value <> "" Then%>
            var url="o23_framework.aspx?source=<%=hidSource.Value%>";
            <%Else%>
            var url="o23_fixwork.aspx?x18id=<%=me.CurrentX18ID%>";
            <%End If%>
            if (flag == "o23-delete") {
                window.open(url,"_top");
                return;
            }
            if (flag == "o23-save") {                
                window.open(url+"&pid="+pid,"_top");
                return;
            }
            
            if (flag != "o23-save") {
                pid=<%=Master.DataPID%>;
                
            }

            location.replace("o23_framework_detail.aspx?x18id=<%=me.CurrentX18ID%>&pid="+pid);

        }


        function b07_reaction(b07id) {
            sw_everywhere("b07_create.aspx?parentpid=" + b07id + "&masterprefix=o23&masterpid=<%=Master.DataPID%>", "Images/comment.png", true)

        }
        function b07_delete(b07id, flag) {
            sw_everywhere("b07_delete.aspx?pid=" + b07id, "Images/delete.png", true)

        }

        function menu_b07_record() {
            sw_everywhere("b07_create.aspx?masterprefix=o23&masterpid=<%=Master.DataPID%>", "Images/comment.png", true);
        }

        function record_edit() {
            window.parent.sw_everywhere("o23_record.aspx?x18id=<%=Me.CurrentX18ID%>&pid=<%=Master.DataPID%>", "Images/report.png", true);
        }
        function record_create() {
            window.parent.sw_everywhere("o23_record.aspx?x18id=<%=Me.CurrentX18ID%>&pid=0", "Images/report.png", true);
        }
        function record_clone() {
            window.parent.sw_everywhere("o23_record.aspx?clone=1&x18id=<%=Me.CurrentX18ID%>&pid=<%=Master.DataPID%>", "Images/report.png", true);
        }

        function report() {            

            window.parent.sw_everywhere("report_modal.aspx?prefix=o23&pid=<%=Master.DataPID%>", "Images/report.png", true);

        }
        function b07_create() {           
            sw_everywhere("b07_create.aspx?masterprefix=o23&masterpid=<%=Master.DataPID%>", "Images/comment.png", true);

        }
        function b07_create_upload() {           
            sw_everywhere("b07_create.aspx?masterprefix=o23&masterpid=<%=Master.DataPID%>&forceupload=1", "Images/comment.png", true);

        }
        function workflow() {         
            sw_everywhere("workflow_dialog.aspx?prefix=o23&pid=<%=Master.DataPID%>", "Images/workflow.png", true);
        }
        function sendmail() {
            sw_everywhere("sendmail.aspx?prefix=o23&pid=<%=Master.DataPID%>", "Images/email.png")


        }
        function plugin() {
            
            sw_everywhere("plugin_modal.aspx?prefix=o23&pid=<%=Master.DataPID%>&x18id=<%=hidX18ID.Value%>","Images/plugin.png",true);

        }
        function menu_fullscreen(){
            <%If hidSource.Value = "3" Then%>
            location.replace("o23_framework.aspx?pid=<%=Master.DataPID%>");
            <%Else%>
            window.open("o23_framework_detail.aspx?pid=<%=Master.DataPID%>&saw=1","_blank");
            <%End If%>
            
        }
        function barcode() {
            sw_everywhere("barcode.aspx?prefix=o23&pid=<%=master.datapid%>", "Images/barcode.png", true);
        }
        function file_preview(prefix,pid) {
            ///náhled na soubor            
            sw_everywhere("fileupload_preview.aspx?prefix="+prefix+"&pid="+pid,"Images/attachment.png",true);
            
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">



    <telerik:RadNavigation ID="menu1" runat="server" MenuButtonPosition="Right" Skin="Metro" EnableViewState="false">
        <Nodes>
            <telerik:NavigationNode ID="begin" Width="50px" Enabled="false" Visible="true">
            </telerik:NavigationNode>


            <telerik:NavigationNode ID="record" Text="ZÁZNAM DOKUMENTU">
                <Nodes>
                    <telerik:NavigationNode ID="cmdNew" Text="Nový" NavigateUrl="javascript:record_create();" ImageUrl="Images/new.png"></telerik:NavigationNode>
                    <telerik:NavigationNode ID="cmdEdit" Text="Upravit" NavigateUrl="javascript:record_edit();" ImageUrl="Images/edit.png"></telerik:NavigationNode>
                    <telerik:NavigationNode ID="cmdClone" Text="Kopírovat" NavigateUrl="javascript:record_clone();" ImageUrl="Images/copy.png" Visible="false"></telerik:NavigationNode>

                    <telerik:NavigationNode ID="cmdWorkflow" Text="Zapsat komentář/souborovou přílohu" NavigateUrl="javascript:b07_create();" ImageUrl="Images/comment.png"></telerik:NavigationNode>
                    <telerik:NavigationNode ID="cmdReport" Text="Tisková sestava" NavigateUrl="javascript:report();" ImageUrl="Images/report.png"></telerik:NavigationNode>
                    <telerik:NavigationNode ID="cmdEmail" Text="Odeslat e-mail" NavigateUrl="javascript:sendmail();" ImageUrl="Images/email.png"></telerik:NavigationNode>

                    <telerik:NavigationNode ID="cmdPlugin" Text="Plugin" NavigateUrl="javascript:plugin();" ImageUrl="Images/plugin.png"></telerik:NavigationNode>

                    <telerik:NavigationNode ID="cmdBarCode" Text="Čárový kód" NavigateUrl="javascript:barcode();" ImageUrl="Images/barcode.png"></telerik:NavigationNode>
                </Nodes>
            </telerik:NavigationNode>
            <telerik:NavigationNode ID="thePage" Text="STRÁNKA">
                <Nodes>
                    <telerik:NavigationNode ID="fs" NavigateUrl="javascript:menu_fullscreen()" ImageUrl="Images/fullscreen.png" Text="Otevřít v nové záložce"></telerik:NavigationNode>

                    <telerik:NavigationNode ID="reload" ImageUrl="Images/refresh.png" text="Obnovit stránku"></telerik:NavigationNode>
                </Nodes>
            </telerik:NavigationNode>

        </Nodes>
    </telerik:RadNavigation>

    <asp:Image ID="imgIcon32" runat="server" ImageUrl="Images/label_32.png" Style="position: absolute; top: 5px; left: 5px;" />


    <div style="clear: both;"></div>
    <asp:Panel ID="panEncrypted" runat="server" CssClass="div6" Visible="false">
        <p class="infoNotificationRed">Obsah dokumentu je zašifrován.</p>
        <span class="lbl">Zadejte heslo:</span>
        <asp:TextBox ID="txtPassword" runat="server" TextMode="Password"></asp:TextBox>
        <asp:Button ID="cmdDecrypt" runat="server" CssClass="cmd" Text="Odšifrovat" />
    </asp:Panel>

    <div class="div6">
        <uc:o23_record_readonly ID="rec1" runat="server" />
    </div>
    <div class="div6" style="border-top: dashed 1px silver;">
        <uc:entityrole_assign_inline ID="roles1" runat="server" EntityX29ID="o23Doc" NoDataText=""></uc:entityrole_assign_inline>
    </div>

    <asp:Panel ID="panUpload" runat="server" CssClass="content-box2">
        <div class="title">

            <img src="Images/attachment.png" style="margin-right: 10px;" />
            <asp:HyperLink ID="filesPreview" runat="server" Text="Přílohy dokumentu"></asp:HyperLink>
            <button type="button" onclick="b07_create_upload()" runat="server" id="cmdUpload">Nahrát přílohy</button>
            <asp:Button ID="cmdLockUnlock" runat="server" Text="Uzamknout přístup k přílohám" CssClass="cmd" />
        </div>
        <div class="content">
            <uc:fileupload_list ID="Fileupload_list__readonly" runat="server" OnClientClickPreview="file_preview" />
        </div>
    </asp:Panel>

    <div style="clear: both; margin-top: 20px;">
        <uc:b07_list ID="comments1" runat="server" JS_Create="b07_create()" JS_Reaction="b07_reaction" ShowInsertButton="false" />
    </div>



    <asp:HiddenField ID="hidX18ID" runat="server" />
    <asp:HiddenField ID="hidB01ID" runat="server" />
    <asp:HiddenField ID="hidSource" runat="server" />


</asp:Content>
