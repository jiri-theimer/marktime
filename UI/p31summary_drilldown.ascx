<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="p31summary_drilldown.ascx.vb" Inherits="UI.p31summary_drilldown" %>
<%@ Register TagPrefix="uc" TagName="periodcombo" Src="~/periodcombo.ascx" %>
<%@ Register TagPrefix="uc" TagName="datagrid" Src="~/datagrid.ascx" %>


<div style="background-color: white;">
       
        <div class="commandcell" style="margin-left: 10px;">

            <asp:DropDownList ID="j75ID" runat="server" AutoPostBack="true" DataTextField="j75Name" DataValueField="pid" Style="width: 200px;" ToolTip="DRILL-DOWN šablona"></asp:DropDownList>
            <asp:ImageButton ID="cmdJ75" runat="server" OnClientClick="return drilldownbuilder()" ImageUrl="Images/griddesigner.png" ToolTip="Návrhář DRILL-DOWN šablon" CssClass="button-link" />
        </div>
        <div class="commandcell" style="padding-left: 20px;">
            <uc:periodcombo ID="period1" runat="server" Width="150px"></uc:periodcombo>
        </div>
        <div class="commandcell" style="padding-left: 20px;">
            <asp:HyperLink ID="clue_query" runat="server" CssClass="reczoom" ToolTip="Detail filtru" Text="i"></asp:HyperLink>
            <asp:DropDownList ID="j70ID" runat="server" AutoPostBack="true" DataTextField="NameWithMark" DataValueField="pid" Style="width: 160px;" ToolTip="Pojmenovaný filtr"></asp:DropDownList>
            <asp:ImageButton ID="cmdQuery" runat="server" OnClientClick="return querybuilder()" ImageUrl="Images/query.png" ToolTip="Návrhář filtrů" CssClass="button-link" />
        </div>
        <div class="commandcell" style="padding-left: 10px;">
            <button type="button" id="cmdSetting" class="show_hide1xx" style="padding: 3px; border-radius: 4px; border-top: solid 1px silver; border-left: solid 1px silver; border-bottom: solid 1px gray; border-right: solid 1px gray; background: buttonface; height: 23px;" title="Více nastavení">
                <span>Další</span>
                <img src="Images/arrow_down.gif" alt="Nastavení" />
            </button>
            <asp:CheckBox ID="chkIncludeChilds" runat="server" AutoPostBack="true" text="Zahrnout i pod-projekty" CssClass="chk" Visible="false" />
        </div>



        <div style="clear: both;"></div>
    </div>
    <div class="slidingDiv1xx" style="padding: 20px;">
        <div class="div6">
            <img src="Images/xls.png" alt="xls" />
            <asp:LinkButton ID="cmdXLS" runat="server" Text="XLS" ToolTip="Export do XLS" />

            <img src="Images/pdf.png" alt="pdf" />
            <asp:LinkButton ID="cmdPDF" runat="server" Text="PDF" ToolTip="Export do PDF" />

            <img src="Images/doc.png" alt="doc" />
            <asp:LinkButton ID="cmdDOC" runat="server" Text="DOC" ToolTip="Export do DOC" />

        

        <span style="margin-left:100px;">Stránkování:</span>
        <asp:DropDownList ID="cbxPaging" runat="server" AutoPostBack="true" ToolTip="Stránkování">
            <asp:ListItem Text="5"></asp:ListItem>
            <asp:ListItem Text="10"></asp:ListItem>
            <asp:ListItem Text="20" Selected="true"></asp:ListItem>
            <asp:ListItem Text="50"></asp:ListItem>
            <asp:ListItem Text="100"></asp:ListItem>
        </asp:DropDownList>
        </div>
    </div>

    <asp:Repeater ID="path1" runat="server">
        <ItemTemplate>
            <div style="float: left; text-align: center; width: 30px;">
                <asp:Image ID="sipka" runat="server" ImageUrl="Images/arrow_right_dd_24.png"></asp:Image>
            </div>
            <div style="float: left;">

                <asp:LinkButton ID="link1" runat="server" CommandName="drill" Font-Bold="true"></asp:LinkButton>

                <asp:HiddenField ID="pid" runat="server" />
                <asp:HiddenField ID="levelindex" runat="server" />
            </div>
        </ItemTemplate>
    </asp:Repeater>
    <div style="clear: both;"></div>

    <uc:datagrid ID="grid1" runat="server" ClientDataKeyNames="pid" OnRowSelected="RowSelected" OnRowDblClick="RowDoubleClick"></uc:datagrid>

    <asp:HiddenField ID="hidCols" runat="server" />

    <asp:HiddenField ID="hidGroup" runat="server" />

   
    <asp:HiddenField ID="hidMasterPrefix" runat="server" Value="p31_drilldown" />
    <asp:HiddenField ID="hidMasterPID" runat="server" />
    <asp:HiddenField ID="hiddatapid" runat="server" />
    <asp:HiddenField ID="hidMaxLevel" runat="server" Value="1" />
    <asp:HiddenField ID="hidCurLevelIndex" runat="server" />
    <asp:HiddenField ID="hidPath_Pids" runat="server" />
    <asp:HiddenField ID="hidPath_Names" runat="server" />
    <asp:HiddenField ID="hidIsApprovingPerson" runat="server" />
    <asp:HiddenField ID="hidAdditionalWhere" runat="server" />
<asp:HiddenField ID="hidHardRefreshFlag" runat="server" />
    <asp:HiddenField ID="hidHardRefreshPID" runat="server" />

    <asp:Button ID="cmdRefresh" runat="server" Style="display: none;" />


<script type="text/javascript">
        $(document).ready(function () {
            
            $(".slidingDiv1xx").hide();
            $(".show_hide1xx").show();

            $('.show_hide1xx').click(function () {
                $(".slidingDiv1xx").slideToggle();
            });


           

        });
        function periodcombo_setting() {

            sw_decide("periodcombo_setting.aspx", "Images/settings_32.png");
        }
        function p31_bs_approve_all() {

            window.parent.sw_master("p31_approving_step1.aspx?masterprefix=<%=Me.CurrentMasterPrefix%>&masterpid=<%=Me.CurrentMasterPID%>&datefrom=<%=Format(period1.DateFrom, "dd.MM.yyyy")%>&dateuntil=<%=Format(period1.DateUntil,"dd.MM.yyyy")%>", "Images/approve_32.png", true);
        }

        function p31_bs_reapprove_all() {

            window.parent.sw_master("p31_approving_step1.aspx?reapprove=1&masterprefix=<%=Me.CurrentMasterPrefix%>&masterpid=<%=Me.CurrentMasterPID%>&datefrom=<%=Format(period1.DateFrom, "dd.MM.yyyy")%>&dateuntil=<%=Format(period1.DateUntil,"dd.MM.yyyy")%>", "Images/approve_32.png", true);
        }
        function p31_bs_clearapprove_all() {

            window.parent.sw_master("p31_approving_step1.aspx?clearapprove=1&masterprefix=<%=Me.CurrentMasterPrefix%>&masterpid=<%=Me.CurrentMasterPID%>&datefrom=<%=Format(period1.DateFrom, "dd.MM.yyyy")%>&dateuntil=<%=Format(period1.DateUntil,"dd.MM.yyyy")%>", "Images/clear_32.png", true);
        }
        function p31_bs_invoice() {

            window.parent.sw_master("p91_create_step1.aspx?nogateway=1&prefix=<%=Me.CurrentMasterPrefix%>&pid=<%=Me.CurrentMasterPID%>", "Images/invoice_32.png", true);
        }
        function querybuilder() {
            var j70id = "<%=Me.CurrentJ70ID%>";
            sw_decide("query_builder.aspx?prefix=p31&pid=" + j70id, "Images/query_32.png");
            return (false);
        }

        function drilldownbuilder() {
            var j75id = "<%=Me.CurrentJ75ID%>";
            sw_decide("drilldown_designer.aspx?masterprefix=<%=me.currentmasterprefix%>&pid=" + j75id, "Images/drilldown_32.png");
            return (false);
        }

        function RowSelected(sender, args) {
            var pid = args.getDataKeyValue("pid");
            document.getElementById("<%=hiddatapid.clientid%>").value = pid;

        }

        function drill(pid) {
            if (pid == null || pid == "")
                var pid = document.getElementById("<%=hiddatapid.clientid%>").value;

            if (pid == "") {
                alert("Chybí ID záznamu");
                return;
            }
            document.getElementById("<%=hidHardRefreshPID.ClientID%>").value = pid;
            document.getElementById('<%= cmdRefresh.ClientID%>').click();
            
            
        }

       
   

        function RowDoubleClick(sender, args) {
            drill();

        }

        function GetAllSelectedPIDs() {

            var masterTable = $find("<%=grid1.radGridOrig.ClientID%>").get_masterTableView();
            var sel = masterTable.get_selectedItems();
            var pids = "";

            for (i = 0; i < sel.length; i++) {
                if (pids == "")
                    pids = sel[i].getDataKeyValue("pid");
                else
                    pids = pids + "," + sel[i].getDataKeyValue("pid");
            }

            return (pids);
        }


       

        function fullscreen(pid, prefix) {
            window.open(prefix + "_framework.aspx?pid=" + pid, "_top");
        }
        function p31(pid) {
            var aw = document.getElementById("<%=Me.hidAdditionalWhere.ClientID%>").value;
            if (aw == "")
                aw="<%=Me.CurrentLevel.GroupByField%>=" + pid;
            else
                aw = aw + " AND <%=Me.CurrentLevel.GroupByField%>=" + pid;

            var aw = encodeURI(aw);

            window.open("p31_grid.aspx?masterprefix=<%=Me.CurrentMasterPrefix%>&masterpid=<%=Me.CurrentMasterPID%>&aw="+aw, "_top");
        }
        function approve_local(pid) {
            var aw = document.getElementById("<%=Me.hidAdditionalWhere.ClientID%>").value;
            if (aw == "")
                aw = "<%=Me.CurrentLevel.GroupByField%>=" + pid;
            else
                aw = aw + " AND <%=Me.CurrentLevel.GroupByField%>=" + pid;

            var aw = encodeURI(aw);
            
            try {
                window.parent.sw_master("entity_modal_approving.aspx?prefix=<%=me.CurrentMasterPrefix%>&pid=<%=me.CurrentMasterPID%>&aw="+aw, "Images/approve_32.png", true);
            }
            catch (err) {
                sw_decide("entity_modal_approving.aspx?prefix=<%=me.CurrentMasterPrefix%>&pid=<%=me.CurrentMasterPID%>&aw=" + aw, "Images/approve_32.png", true);
            }

        }
    </script>
