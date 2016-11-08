<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Clue.Master" CodeBehind="entity_framework_p31summary.aspx.vb" Inherits="UI.entity_framework_p31summary" %>

<%@ MasterType VirtualPath="~/Clue.Master" %>
<%@ Register TagPrefix="uc" TagName="periodcombo" Src="~/periodcombo.ascx" %>
<%@ Register TagPrefix="uc" TagName="datagrid" Src="~/datagrid.ascx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <link href="Scripts/jquery.qtip.min.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript" src="Scripts/jquery.qtip.min.js"></script>

    <script type="text/javascript">
        $(document).ready(function () {
            window.parent.stoploading();

            $(".slidingDiv1xx").hide();
            $(".show_hide1xx").show();

            $('.show_hide1xx').click(function () {
                $(".slidingDiv1xx").slideToggle();
            });


            var iframeWidth = '100%';
            var iframeHeight = '270';


            $("a.reczoom").each(function () {

                // Extract your variables here:
                var $this = $(this);
                var myurl = $this.attr('rel');

                var mytitle = $this.attr('title');
                if (mytitle == null)
                    mytitle = 'Detail';


                $this.qtip({
                    content: {
                        text: '<iframe scrolling=no src="' + myurl + '"' + ' width=' + iframeWidth + '"' + ' height=' + '"' + iframeHeight + '"  frameborder="0"><p>Your browser does not support iframes.</p></iframe>',
                        title: {
                            text: mytitle
                        },

                    },
                    position: {
                        my: 'top center',  // Position my top left...
                        at: 'bottom center', // at the bottom right of...
                        viewport: $(window)
                    },

                    hide: {

                        fixed: true,
                        delay: 100

                    },
                    style: {
                        classes: 'qtip-tipped',
                        width: 700,
                        height: 300

                    }
                });
            });

            $("a.button-reczoom").each(function () {

                // Extract your variables here:
                var $this = $(this);
                var myurl = $this.attr('rel');

                var mytitle = $this.attr('title');
                if (mytitle == null)
                    mytitle = 'Modal dialog';


                $this.qtip({
                    content: {
                        text: '<iframe src="' + myurl + '"' + ' width=' + iframeWidth + '"' + ' height=' + '"' + iframeHeight + '"  frameborder="0"><p>Your browser does not support iframes.</p></iframe>',
                        title: {
                            text: mytitle,
                            button: true
                        },

                    },
                    position: {
                        my: 'top center',  // Position my top left...
                        at: 'bottom center', // at the bottom right of...
                        viewport: $(window)
                    },
                    show: {
                        event: 'click', // Show it on click...
                        solo: true, // ...and hide all other tooltips...
                        modal: true // ...and make it modal
                    },
                    hide: false,
                    style: {
                        classes: 'qtip-tipped',
                        width: 700,
                        height: 300

                    }
                });
            });

        });
        function periodcombo_setting() {

            window.parent.sw_local("periodcombo_setting.aspx", "Images/settings_32.png");
        }
        function p31_bs_approve_all() {

            window.parent.parent.sw_master("p31_approving_step1.aspx?masterprefix=<%=Me.CurrentMasterPrefix%>&masterpid=<%=Me.CurrentMasterPID%>&datefrom=<%=Format(period1.DateFrom, "dd.MM.yyyy")%>&dateuntil=<%=Format(period1.DateUntil,"dd.MM.yyyy")%>", "Images/approve_32.png", true);
        }

        function p31_bs_reapprove_all() {

            window.parent.parent.sw_master("p31_approving_step1.aspx?reapprove=1&masterprefix=<%=Me.CurrentMasterPrefix%>&masterpid=<%=Me.CurrentMasterPID%>&datefrom=<%=Format(period1.DateFrom, "dd.MM.yyyy")%>&dateuntil=<%=Format(period1.DateUntil,"dd.MM.yyyy")%>", "Images/approve_32.png", true);
        }
        function p31_bs_clearapprove_all() {

            window.parent.parent.sw_master("p31_approving_step1.aspx?clearapprove=1&masterprefix=<%=Me.CurrentMasterPrefix%>&masterpid=<%=Me.CurrentMasterPID%>&datefrom=<%=Format(period1.DateFrom, "dd.MM.yyyy")%>&dateuntil=<%=Format(period1.DateUntil,"dd.MM.yyyy")%>", "Images/clear_32.png", true);
        }
        function p31_bs_invoice() {

            window.parent.parent.sw_master("p91_create_step1.aspx?nogateway=1&prefix=<%=Me.CurrentMasterPrefix%>&pid=<%=Me.CurrentMasterPID%>", "Images/invoice_32.png", true);
        }
        function querybuilder() {
            var j70id = "<%=Me.CurrentJ70ID%>";
            window.parent.sw_decide("query_builder.aspx?prefix=p31&pid=" + j70id, "Images/query_32.png");
            return (false);
        }

        function drilldownbuilder() {
            var j75id = "<%=Me.CurrentJ75ID%>";
            window.parent.sw_decide("drilldown_designer.aspx?masterprefix=<%=me.currentmasterprefix%>&pid=" + j75id, "Images/drilldown_32.png");
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

            hardrefresh(pid, "drill");
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


        function hardrefresh(pid, flag) {


            document.getElementById("<%=Me.hidHardRefreshPID.ClientID%>").value = pid;
            document.getElementById("<%=Me.hidHardRefreshFlag.ClientID%>").value = flag;

            <%=Me.ClientScript.GetPostBackEventReference(Me.cmdRefresh, "", False)%>;

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
        function approve(pid) {
            var aw = document.getElementById("<%=Me.hidAdditionalWhere.ClientID%>").value;
            if (aw == "")
                aw = "<%=Me.CurrentLevel.GroupByField%>=" + pid;
            else
                aw = aw + " AND <%=Me.CurrentLevel.GroupByField%>=" + pid;

            var aw = encodeURI(aw);
            
            try {
                window.parent.parent.sw_master("entity_modal_approving.aspx?prefix=<%=me.CurrentMasterPrefix%>&pid=<%=me.CurrentMasterPID%>&aw="+aw, "Images/approve_32.png", true);
            }
            catch (err) {
                window.parent.sw_decide("entity_modal_approving.aspx?prefix=<%=me.CurrentMasterPrefix%>&pid=<%=me.CurrentMasterPID%>&aw=" + aw, "Images/approve_32.png", true);
            }

        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
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

                <img src="Images/arrow_down.gif" alt="Nastavení" />
            </button>
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

    <asp:HiddenField ID="hidHardRefreshFlag" runat="server" />
    <asp:HiddenField ID="hidHardRefreshPID" runat="server" />
    <asp:HiddenField ID="hidMasterPrefix" runat="server" Value="p31_drilldown" />
    <asp:HiddenField ID="hidMasterPID" runat="server" />
    <asp:HiddenField ID="hiddatapid" runat="server" />
    <asp:HiddenField ID="hidMaxLevel" runat="server" Value="1" />
    <asp:HiddenField ID="hidCurLevelIndex" runat="server" />
    <asp:HiddenField ID="hidPath_Pids" runat="server" />
    <asp:HiddenField ID="hidPath_Names" runat="server" />
    <asp:HiddenField ID="hidIsApprovingPerson" runat="server" />
    <asp:HiddenField ID="hidAdditionalWhere" runat="server" />

    <asp:Button ID="cmdRefresh" runat="server" Style="display: none;" />

</asp:Content>
