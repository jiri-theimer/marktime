<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master" CodeBehind="p31_framework.aspx.vb" Inherits="UI.p31_framework" %>

<%@ MasterType VirtualPath="~/Site.Master" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<%@ Register TagPrefix="uc" TagName="datagrid" Src="~/datagrid.ascx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <style type="text/css">
        div.divHover:hover {
            background-color: #ffffcc;
        }

       
    </style>

    <script type="text/javascript">
        var _initResizing = "1";

        $(document).ready(function () {
            $(".slidingDiv1").hide();
            $(".show_hide1").show();

            $('.show_hide1').click(function () {
                $(".slidingDiv1").slideToggle();
            });

            

        });

        function loadSplitter(sender) {
            var h1 = new Number;
            var h2 = new Number;
            var h3 = new Number;

            h1 = $(window).height();

            var ss = self.document.getElementById("offsetY");
            var offset = $(ss).offset();

            h2 = offset.top;

            h3 = h1 - h2;

            sender.set_height(h3);
            
            
            var pane = sender.getPaneById("<%=contentPane.ClientID%>");            
            pane.set_contentUrl("p31_framework_detail.aspx?parentWidth=" + pane.get_width());
            
        }

       

        function RowSelected(sender, args) {
            var pid = args.getDataKeyValue("pid");
            document.getElementById("<%=hiddatapid.clientid%>").value = pid;
           
        }

        function RowDoubleClick(sender, args) {
            //vyvolat dialog pro zápis úkonu
            <%If Me.GridPrefix = "p41" Then%>
            var p41id = document.getElementById("<%=hiddatapid.clientid%>").value;
            nw(p41id);
            <%End If%>
            <%If Me.GridPrefix = "p56" Then%>
            var p56id = document.getElementById("<%=hiddatapid.clientid%>").value;
            nw_p56(p56id);
            <%End If%>

        }

        function nw(p41id) {
            //vyvolat dialog pro zápis úkonu
            <%If Me.CurrentJ02ID <> Master.Factory.SysUser.j02ID Then%>
            sw_master("p31_record.aspx?pid=0&j02id=<%=Me.CurrentJ02ID%>&p41id=" + p41id, "worksheet_32.png");
            <%Else%>
            sw_master("p31_record.aspx?pid=0&p41id=" + p41id, "worksheet_32.png");
            <%End If%>


        }

        function nw_p56(p56id) {
            //vyvolat dialog pro zápis úkonu
            <%If Me.CurrentJ02ID <> Master.Factory.SysUser.j02ID Then%>
            sw_master("p31_record.aspx?pid=0&j02id=<%=Me.CurrentJ02ID%>&p56id=" + p56id, "worksheet_32.png");
            <%Else%>
            sw_master("p31_record.aspx?pid=0&p56id=" + p56id, "worksheet_32.png");
            <%End If%>

        }

        function p56_edit(pid) {
            sw_master("p56_record.aspx?pid=" + pid, "task_32.png");
        }
        function p56_create() {
            sw_master("p56_record.aspx?pid=0&masterprefix=p41&masterpid=0", "task_32.png");
        }

        function sw_local(url, img, is_maximize) {
            sw_master(url,img,is_maximize);
        }

        function SavePaneWidth(w) {
            $.post("Handler/handler_userparam.ashx", { x36value: w, x36key: "p31_framework-navigationPane_width", oper: "set" }, function (data) {
                if (data == ' ') {
                    return;
                }

            });
        }

        function AfterPaneResized(sender, args) {
            if (_initResizing == "1") {
                _initResizing = "0";
                return;
            }

            var w = sender.get_width();

            SavePaneWidth(w);

        }

        function AfterPaneCollapsed(pane)
        {
            var w = "-1";
            SavePaneWidth(w);
        }
        function AfterPaneExpanded(pane) {
            var w = pane.get_width();
            SavePaneWidth(w);            
        }

        function hardrefresh(pid, flag) {
            if (flag == "p31-save") {
                var splitter = $find("<%= RadSplitter1.ClientID %>");
                var pane = splitter.getPaneById("<%=contentPane.ClientID%>");
                pane.set_contentUrl("p31_framework_detail.aspx?pid=" + pid);

                <%If RadSplitter1.GetPanes.count=3 then%>
                pane = splitter.getPaneById("<%=rightPane.ClientID%>");
                pane.set_contentUrl("p31_framework_timer.aspx");               
                <%End If%>
                return;
            }
            location.replace("p31_framework.aspx");


        }

        function griddesigner() {
            var j74id = "<%=Me.CurrentJ74ID%>";
            sw_master("grid_designer.aspx?nodrilldown=1&prefix=<%=me.gridprefix%>&masterprefix=p31_framework&pid=" + j74id, "Images/griddesigner_32.png");
        }



    </script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    
    <div id="offsetY"></div>
    <telerik:RadSplitter ID="RadSplitter1" runat="server" Width="100%" ResizeMode="Proportional" OnClientLoaded="loadSplitter" PanesBorderSize="0" Skin="Metro" RenderMode="Lightweight">
        <telerik:RadPane ID="navigationPane" runat="server" Width="350px" OnClientResized="AfterPaneResized" OnClientCollapsed="AfterPaneCollapsed" OnClientExpanded="AfterPaneExpanded" MaxWidth="1000" BackColor="white">

            <telerik:RadTabStrip ID="tabs1" runat="server" ShowBaseLine="true" Width="100%" Skin="Default" AutoPostBack="true">
                <Tabs>
                    <telerik:RadTab Text="<%$ Resources:p31_framework, tabs1_p41 %>" Value="p41" Selected="true" ToolTip="<%$Resources:p31_framework, tabs1_p41_tooltip %>"></telerik:RadTab>
                    <telerik:RadTab Text="TOP 10" Value="top10" ToolTip="Maximálně 10 mnou naposledy vykazovaných projektů"></telerik:RadTab>
                    <telerik:RadTab Text="<%$ Resources:p31_framework, tabs1_todo %>" Value="todo" ToolTip="Otevřené úkoly k řešení a vykazování"></telerik:RadTab>
                    <telerik:RadTab ImageUrl="Images/favourite.png" Value="favourites" ToolTip="Seznam mých oblíbených projektů"></telerik:RadTab>
                </Tabs>
            </telerik:RadTabStrip>

            <asp:Panel ID="panSearch" runat="server" CssClass="div6" DefaultButton="cmdSearch" Style="height: 28px;">

                <div style="float: left;">
                    <asp:Image ID="img1" runat="server" ImageUrl="Images/project_32.png" />
                    <asp:Label ID="lblFormHeader" runat="server" CssClass="page_header_span" Text="Projekty" Style="vertical-align: top;"></asp:Label>

                </div>
                <div class="commandcell" style="padding-left: 20px;">
                    <asp:TextBox ID="txtSearch" runat="server" Style="width: 140px;" ToolTip="Filtrovat podle názvu/kódu projektu nebo klienta"></asp:TextBox>
                    <asp:LinkButton ID="cmdCĺearFilter" runat="server" Text="<%$Resources:p31_framework, cmdCĺearFilter%>" Style="margin-left: 10px; font-weight: bold; color: red;" Visible="false"></asp:LinkButton>
                </div>
                <div class="commandcell">
                    <asp:ImageButton ID="cmdSearch" runat="server" ImageUrl="Images/search.png" ToolTip="Spustit filtr" CssClass="button-link" />
                    <asp:HyperLink ID="cmdNewTask" runat="server" Text="<%$Resources:p31_framework,cmdNewTask %>" NavigateUrl="javascript:p56_create()"></asp:HyperLink>
                </div>

                <div class="commandcell" style="float: right; margin-right: 10px;">
                    
                    <button type="button" id="cmdSetting" class="show_hide1" style="float: right; padding: 3px; border-radius: 4px; border-top: solid 1px silver; border-left: solid 1px silver; border-bottom: solid 1px gray; border-right: solid 1px gray; background: buttonface;" title="Další nastavení přehledu">

                        <img src="Images/arrow_down.gif" alt="Nastavení" />
                    </button>
                                      
                </div>


            </asp:Panel>
            <div style="clear: both; width: 100%;"></div>
            <div class="slidingDiv1">
                <div class="content-box2">
                    <div class="title">
                        <%=Resources.p31_framework.NastaveniProjektu%>
                    </div>
                    <div class="content">
                        <asp:DropDownList ID="cbxGroupBy" runat="server" AutoPostBack="true" ToolTip="Datové souhrny"></asp:DropDownList>
                        <asp:DropDownList ID="j74id" runat="server" AutoPostBack="true" DataTextField="j74Name" DataValueField="pid" Style="width: 180px;" ToolTip="Šablony datového přehledu"></asp:DropDownList>
                        <button type="button" onclick="griddesigner()"><%=Resources.p31_framework.Sloupce%></button>

                        <div class="div6">
                            <asp:Label ID="lblPaging" runat="server" CssClass="lbl" Text="<%$Resources:p31_framework,lblPaging%>"></asp:Label>
                            <asp:DropDownList ID="cbxPaging" runat="server" AutoPostBack="true" ToolTip="Stránkování" TabIndex="3">
                                <asp:ListItem Text="20"></asp:ListItem>
                                <asp:ListItem Text="50" Selected="True"></asp:ListItem>
                                <asp:ListItem Text="100"></asp:ListItem>
                                <asp:ListItem Text="200"></asp:ListItem>
                                <asp:ListItem Text="500"></asp:ListItem>
                            </asp:DropDownList>
                            <asp:CheckBox ID="chkGroupsAutoExpanded" runat="server" Text="<%$Resources:p31_framework, AutoRozbaleneSouhrny%>" AutoPostBack="true" Checked="false" />
                        </div>
                    </div>
                </div>
                <div class="div6">
                    <img src="Images/help.png" /><i><%=Resources.p31_framework.Napoveda %></i>
                </div>
            </div>
            <uc:datagrid ID="grid1" runat="server" ClientDataKeyNames="pid" OnRowSelected="RowSelected" OnRowDblClick="RowDoubleClick" Skin="Default"></uc:datagrid>


            <asp:HiddenField ID="hiddatapid" runat="server" />
            <asp:HiddenField ID="hidHardRefreshFlag" runat="server" />
            <asp:HiddenField ID="hidHardRefreshPID" runat="server" />
            <asp:Button ID="cmdRefresh" runat="server" Style="display: none;" />
            <asp:HiddenField ID="hidCurrentJ02ID" runat="server" />
            <asp:HiddenField ID="hidReceiversInLine" runat="server" />
            <asp:HiddenField ID="hidTasksWorksheetColumns" runat="server" />
            <asp:HiddenField ID="hidCols" runat="server" />
            <asp:HiddenField ID="hidFrom" runat="server" />            
        </telerik:RadPane>
        <telerik:RadSplitBar ID="RadSplitbar1" runat="server" CollapseMode="Forward">
        </telerik:RadSplitBar>
        <telerik:RadPane ID="contentPane" runat="server">
            Detail projektu
        </telerik:RadPane>
        <telerik:RadPane ID="rightPane" runat="server" Width="350px" ContentUrl="blank.aspx">
            Stopky
        </telerik:RadPane>
    </telerik:RadSplitter>

</asp:Content>

