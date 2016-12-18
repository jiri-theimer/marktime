<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="entity_menu.ascx.vb" Inherits="UI.entity_menu" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<%@ Register TagPrefix="uc" TagName="searchbox" Src="~/searchbox.ascx" %>
<asp:Panel ID="panMenuContainer" runat="server" Style="height:43px;border-bottom: solid 1px gray;background-color:#E8E8E8;">

    <telerik:RadMenu ID="menu1" RenderMode="Auto" Skin="Default" runat="server" Style="z-index: 2900;" ExpandDelay="0" ExpandAnimation-Type="None" ClickToOpen="true" EnableAutoScroll="true" Width="100%">
        <Items>
            <telerik:RadMenuItem Value="begin"></telerik:RadMenuItem>
            <telerik:RadMenuItem Value="level1" NavigateUrl="#" Width="280px"></telerik:RadMenuItem>
            <telerik:RadMenuItem Value="saw" Text="<img src='Images/open_in_new_window.png'/>" Target="_blank" NavigateUrl="p41_framework_detail.aspx?saw=1" ToolTip="Otevřít aktuální záznam v nové záložce prohlížeče"></telerik:RadMenuItem>
                       
            <telerik:RadMenuItem Text="ZÁZNAM PROJEKTU" ImageUrl="Images/arrow_down_menu.png" Value="record"></telerik:RadMenuItem>      
            
            <telerik:RadMenuItem Value="searchbox"></telerik:RadMenuItem>                 

        </Items>
    </telerik:RadMenu>

</asp:Panel>

<telerik:RadTabStrip ID="tabs1" runat="server" Skin="Default" Width="100%" AutoPostBack="false" ShowBaseLine="true">              
</telerik:RadTabStrip>

<asp:HiddenField ID="hidIsCanApprove" runat="server" />
<asp:HiddenField ID="hidPOS" runat="server" Value="1" />
 <asp:HiddenField ID="hidParentWidth" runat="server" />
<asp:HiddenField ID="hidDataPrefix" runat="server" />
<uc:searchbox id="sb1" runat="server"></uc:searchbox>

<script type="text/javascript">
    function report() {
            
        sw_decide("report_modal.aspx?prefix=<%=Me.DataPrefix%>&pid=<%=Me.DataPID%>","Images/reporting.png",true);

    }

    function sw_decide(url, iconUrl, is_maximize) {
        var isInIFrame = (window.location != window.parent.location);
        if (isInIFrame==true){

            var w = parseInt(document.getElementById("<%=hidParentWidth.ClientID%>").value);
            var h = screen.availHeight;

            if ((w < 901 || h < 800) && w>0) {
                window.parent.sw_master(url, iconUrl);
                return;
            }                

            if (w < 910)
                is_maximize = true;
        }
        sw_local(url, iconUrl, is_maximize);
    }
        
        
        
    function p31_entry_menu(p34id) {
        ///z menu1
        <%If Me.DataPrefix="p41" then%>
        sw_decide("p31_record.aspx?pid=0&p41id=<%=Me.DataPID%>&p34id="+p34id,"Images/worksheet.png",true);
        <%End If%>
        <%If Me.DataPrefix = "p56" Then%>
        sw_decide("p31_record.aspx?pid=0&p56id=<%=Me.DataPID%>&p34id="+p34id,"Images/worksheet.png",true);
        <%End If%>
        

    }

    function record_edit() {
        var pid = <%=Me.DataPID%>;
        if (pid == "" || pid == null) {
            alert("Není vybrán záznam.");
            return
        }
        sw_decide("<%=Me.DataPrefix%>_record.aspx?pid=" + pid,"Images/project.png",true);

    }
        
        
    function record_clone() {
        var pid = <%=Me.DataPID%>;
        if (pid == "" || pid == null) {
            alert("Není vybrán záznam.");
            return
        }
        <%If Me.DataPrefix="p41" then%>
        sw_decide("p41_create.aspx?clone=1&pid=" + pid,"Images/project.png",true);
        <%Else%>
        sw_decide("<%=Me.DataPrefix%>_record.aspx?clone=1&pid=" + pid,"Images/copy.png",true);
        <%End If%>

    }
    function record_new() {
        <%If Me.DataPrefix="p41" then%>
        sw_decide("p41_create.aspx?client_family=1&pid=<%=Me.DataPID%>","Images/project.png",true);
        <%end If%>
        <%If Me.DataPrefix="p28" then%>
        sw_decide("p28_record.aspx?pid=0","Images/contact.png",true);
        <%end If%>
        <%If Me.DataPrefix="j02" then%>
        sw_decide("j02_record.aspx?pid=0","Images/person.png",true);
        <%end If%>       
        <%If Me.DataPrefix = "o23" Then%>
        sw_decide("o23_record.aspx?pid=0","Images/notepad.png",true);
        <%end If%>

    }
    function record_new_child(){
        var pid = <%=Me.DataPID%>;
        sw_decide("p41_create.aspx?client_family=1&pid=<%=Me.DataPID%>&create_parent=1","Images/project.png",true);
    }
    function p28_p41_new() {
            
        sw_decide("p41_create.aspx?p28id=<%=Me.DataPID%>","Images/project_32.png",true);

    }
    function p31_recurrence_record(pid) {
        sw_decide("p31_record.aspx?pid=" + pid, "Images/worksheet.png");

    }
    function p30_binding() {
        sw_decide("p30_binding.aspx?masterprefix=<%=Me.DataPrefix%>&masterpid=<%=me.datapid%>", "Images/person.png", false);
    }
    function page_setting(){
        sw_decide("entity_framework_detail_setting.aspx?prefix=<%=Me.DataPrefix%>", "Images/setting.png",false);
    }
    function p30_record(pid) {            
        sw_decide("p30_binding.aspx?masterprefix=p41&masterpid=<%=Me.DataPID%>&pid="+pid,"Images/person.png",true);
    }
        
    function draft2normal() {

        if (confirm("Převést záznam z režimu DRAFT?")) {
            hardrefresh(<%=Me.DataPID%>,'draft2normal');
        }
        else {
            return (false);
        }
    }
        
        
    function menu_b07_record() {

        sw_decide("b07_create.aspx?masterprefix=<%=Me.DataPrefix%>&masterpid=<%=Me.DataPID%>", "Images/comment.png", true);

    }
    function scheduler(){            
        window.open("entity_scheduler.aspx?masterprefix=<%=Me.DataPrefix%>&masterpid=<%=Me.DataPID%>","_top")
    }
    
        
    function p48_plan(){            
        window.open("p48_framework.aspx?masterprefix=<%=me.DataPrefix%>&masterpid=<%=Me.DataPID%>","_top");
    }

    function workflow(){            
        sw_decide("workflow_dialog.aspx?prefix=<%=Me.DataPrefix%>&pid=<%=me.datapid%>","Images/workflow.png",false);
    }
    function p31_move2bin(){            
        sw_decide("p31_move2bin.aspx?prefix=p41&pid=<%=Me.DataPID%>","Images/bin.png",true);
    }
    function p31_move2project(){            
        sw_decide("p31_move2project.aspx?prefix=p41&pid=<%=Me.DataPID%>","Images/cut.png",true);
    }
    function p31_recalc(){            
        sw_decide("p31_recalc.aspx?prefix=p41&pid=<%=me.datapid%>","Images/recalc.png",true);
    }
    function menu_o23_record(pid) {
            
        sw_decide("o23_record.aspx?masterprefix=<%=Me.DataPrefix%>&masterpid=<%=Me.DataPID%>&pid="+pid,"Images/notepad.png",true);

    }
    function menu_o22_record(pid) {
            
        sw_decide("o22_record.aspx?masterprefix=<%=Me.DataPrefix%>&masterpid=<%=Me.DataPID%>&pid="+pid,"Images/calendar.png",true);

    }
    

    function approve(){             
        var isInIFrame = (window.location != window.parent.location);
        if (isInIFrame==true){
            window.parent.sw_master("entity_modal_approving.aspx?prefix=<%=Me.DataPrefix%>&pid=<%=Me.DataPID%>","Images/approve_32.png",true);
        }
        else{
            sw_decide("entity_modal_approving.aspx?prefix=<%=Me.DataPrefix%>&pid=<%=Me.DataPID%>","Images/approve_32.png",true);
        }
            
    }
    
    function timeline(){            
        sw_decide("entity_timeline.aspx?prefix=<%=Me.DataPrefix%>&pid=<%=Me.DataPID%>","Images/timeline.png",true);
    }
    function menu_sendmail() {
        sw_decide("sendmail.aspx?prefix=<%=me.DataPrefix%>&pid=<%=me.DataPID%>", "Images/email_32.png")


    }

    <%if me.DataPrefix="p41" then%>
    function menu_p40_record(p40id){            
        sw_decide("p40_record.aspx?p41id=<%=Me.DataPID%>&pid="+p40id,"Images/worksheet_recurrence.png",true);
    }

    function menu_p56_record(pid,bolReturnFalse) {
        sw_decide("p56_record.aspx?masterprefix=p41&masterpid=<%=Me.DataPID%>&pid="+pid,"Images/task.png",true);
        if (bolReturnFalse==true)
            return(false)
    }
    <%end if%>
</script>