<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/SubForm.Master" CodeBehind="x25_framework_detail.aspx.vb" Inherits="UI.x25_framework_detail" %>

<%@ MasterType VirtualPath="~/SubForm.Master" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<%@ Register TagPrefix="uc" TagName="b07_list" Src="~/b07_list.ascx" %>
<%@ Register TagPrefix="uc" TagName="entityrole_assign_inline" Src="~/entityrole_assign_inline.ascx" %>
<%@ Register TagPrefix="uc" TagName="x25_record_readonly" Src="~/x25_record_readonly.ascx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <script type="text/javascript">

        function hardrefresh(pid, flag) {            
            if (flag == "x25-delete") {
                window.open("x25_framework.aspx?x18id=<%=me.CurrentX18ID%>","_top");
                return;
            }
            if (flag == "x25-save") {
                window.open("x25_framework.aspx?x18id=<%=me.CurrentX18ID%>&pid="+pid,"_top");
                return;
            }
            
            if (flag != "x25-save") {
                pid=<%=Master.DataPID%>;
                
            }

            location.replace("x25_framework_detail.aspx?x18id=<%=me.CurrentX18ID%>&pid="+pid);

        }


        function b07_reaction(b07id) {
            sw_everywhere("b07_create.aspx?parentpid=" + b07id + "&masterprefix=o23&masterpid=<%=Master.DataPID%>", "Images/comment.png", true)

        }
        function b07_delete(b07id, flag) {
            sw_everywhere("b07_delete.aspx?pid=" + b07id, "Images/delete.png", true)

        }

        function menu_b07_record() {
            sw_everywhere("b07_create.aspx?masterprefix=x25&masterpid=<%=Master.DataPID%>", "Images/comment.png", true);
        }

        function record_edit() {
            window.parent.sw_everywhere("x25_record.aspx?x18id=<%=Me.CurrentX18ID%>&pid=<%=Master.DataPID%>", "Images/report.png", true);
        }
        function record_create() {
            window.parent.sw_everywhere("x25_record.aspx?x18id=<%=Me.CurrentX18ID%>&pid=0", "Images/report.png", true);
        }
        function record_clone() {
            window.parent.sw_everywhere("x25_record.aspx?clone=1&x18id=<%=Me.CurrentX18ID%>&pid=<%=Master.DataPID%>", "Images/report.png", true);
        }

        function report() {            

            window.parent.sw_everywhere("report_modal.aspx?prefix=x25&pid=<%=Master.DataPID%>", "Images/report.png", true);

        }
        function b07_create() {           
            sw_everywhere("b07_create.aspx?masterprefix=x25&masterpid=<%=Master.DataPID%>", "Images/comment.png", true);

        }
        function workflow() {         
            sw_everywhere("workflow_dialog.aspx?prefix=x25&pid=<%=Master.DataPID%>", "Images/workflow.png", true);
        }
        function sendmail() {
            sw_everywhere("sendmail.aspx?prefix=x25&pid=<%=Master.DataPID%>", "Images/email.png")


        }

    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <asp:Panel ID="panMenuContainer" runat="server" Style="height: 43px; border-bottom: solid 1px gray; background-color: #E8E8E8;">

        <telerik:RadMenu ID="menu1" RenderMode="Auto" Skin="Default" runat="server" Width="100%" Style="z-index: 2900;" ExpandDelay="0" ExpandAnimation-Type="None" ClickToOpen="true" EnableAutoScroll="true">
            <Items>
                <telerik:RadMenuItem Value="begin"></telerik:RadMenuItem>

                <telerik:RadMenuItem Value="reload" ImageUrl="Images/refresh.png" Text=" " ToolTip="Obnovit stránku" Width="28px"></telerik:RadMenuItem>



                <telerik:RadMenuItem Text="ZÁZNAM" ImageUrl="Images/arrow_down_menu.png" Value="record">
                    <Items>
                        <telerik:RadMenuItem Value="cmdNew" Text="Nový" NavigateUrl="javascript:record_create();" ImageUrl="Images/new.png"></telerik:RadMenuItem>
                        <telerik:RadMenuItem Value="cmdEdit" Text="Upravit" NavigateUrl="javascript:record_edit();" ImageUrl="Images/edit.png"></telerik:RadMenuItem>
                        <telerik:RadMenuItem Value="cmdClone" Text="Kopírovat" NavigateUrl="javascript:record_clone();" ImageUrl="Images/copy.png" Visible="false"></telerik:RadMenuItem>
                        <telerik:RadMenuItem IsSeparator="true"></telerik:RadMenuItem>                        
                        <telerik:RadMenuItem Value="cmdWorkflow" Text="Zapsat komentář/souborovou přílohu" NavigateUrl="javascript:b07_create();" ImageUrl="Images/comment.png"></telerik:RadMenuItem>
                        <telerik:RadMenuItem IsSeparator="true"></telerik:RadMenuItem>
                        <telerik:RadMenuItem Value="cmdReport" Text="Tisková sestava" NavigateUrl="javascript:report();" ImageUrl="Images/report.png"></telerik:RadMenuItem>                        
                        <telerik:RadMenuItem IsSeparator="true"></telerik:RadMenuItem>
                        <telerik:RadMenuItem Value="cmdEmail" Text="Odeslat e-mail" NavigateUrl="javascript:sendmail();" ImageUrl="Images/email.png"></telerik:RadMenuItem>
                    </Items>

                </telerik:RadMenuItem>






            </Items>
        </telerik:RadMenu>

    </asp:Panel>


    <div style="clear: both;"></div>

    <div class="div6">
        <uc:x25_record_readonly ID="rec1" runat="server" />
    </div>
    <div class="div6">
        <uc:entityrole_assign_inline ID="roles1" runat="server" EntityX29ID="x25EntityField_ComboValue" NoDataText=""></uc:entityrole_assign_inline>
    </div>

    <div style="clear: both;margin-top:20px;">
        <uc:b07_list ID="comments1" runat="server" JS_Create="b07_create()" JS_Reaction="b07_reaction" ShowInsertButton="false" />
    </div>
    


    <asp:HiddenField ID="hidX18ID" runat="server" />
    <asp:HiddenField ID="hidB01ID" runat="server" />
</asp:Content>
