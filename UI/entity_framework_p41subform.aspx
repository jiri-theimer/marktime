<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Clue.Master" CodeBehind="entity_framework_p41subform.aspx.vb" Inherits="UI.entity_framework_p41subform" %>

<%@ MasterType VirtualPath="~/Clue.Master" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<%@ Register TagPrefix="uc" TagName="datagrid" Src="~/datagrid.ascx" %>


<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <link href="Scripts/jquery.qtip.min.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript" src="Scripts/jquery.qtip.min.js"></script>

    <script type="text/javascript">
        $(document).ready(function () {
            window.parent.stoploading();

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

        });
        function p41_subgrid_setting(j74id) {
            window.parent.sw_decide("grid_designer.aspx?prefix=p41&masterprefix=<%=Me.CurrentMasterPrefix%>&pid=" + j74id, "Images/griddesigner.png", true);
        }
        function RowSelected_p41(sender, args) {
            document.getElementById("<%=hiddatapid_p41.ClientID%>").value = args.getDataKeyValue("pid");
        }

        function RowDoubleClick_p41(sender, args) {
            var pid = document.getElementById("<%=hiddatapid_p41.ClientID%>").value;
            window.open("p41_framework.aspx?pid=" + pid, "_top");
        }



        function p41_record(pid) {
            window.parent.sw_decide("p41_record.aspx?pid=" + pid, "Images/project.png", true);


        }
        function p41_clone() {

            var pid = document.getElementById("<%=hiddatapid_p41.ClientID%>").value;
            if (pid == "" || pid == null) {
                alert("Není vybrán záznam.");
                return;
            }
            window.parent.sw_decide("p41_record.aspx?clone=1&pid=" + pid, "Images/project.png", true);

        }
    </script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <div class="commandcell">
        <img src="Images/project.png" alt="Dokumenty" />
        <asp:Label ID="lblHeaderP41" CssClass="framework_header_span" runat="server" Text="Projekty"></asp:Label>
    </div>
    <div class="commandcell" style="margin-left: 10px;">

        <asp:DropDownList ID="cbxGroupBy" runat="server" AutoPostBack="true" ToolTip="Datové souhrny" Visible="false">
            <asp:ListItem Text="Bez souhrnů" Value=""></asp:ListItem>
            <asp:ListItem Text="Typ dokumentu" Value="p57Name"></asp:ListItem>

        </asp:DropDownList>
    </div>
    <div class="commandcell">
        <asp:DropDownList ID="cbxValidity" runat="server" AutoPostBack="true">
            <asp:ListItem Text="Otevřené i archivované" Value="1" Selected="true"></asp:ListItem>
            <asp:ListItem Text="Pouze otevřené projekty" Value="2"></asp:ListItem>
            <asp:ListItem Text="Pouze přesunuté do archivu" Value="3"></asp:ListItem>
        </asp:DropDownList>
    </div>
    <div class="commandcell" style="margin-left: 10px;">
        <telerik:RadMenu ID="recmenu1" Skin="Telerik" runat="server" ClickToOpen="true">
            <Items>
                <telerik:RadMenuItem Text="Záznam" ImageUrl="Images/menuarrow.png">
                    <Items>
                        <telerik:RadMenuItem Text="Nový" Value="new" NavigateUrl="javascript:p41_record(0,false)"></telerik:RadMenuItem>
                        <telerik:RadMenuItem Text="Kopírovat" Value="clone" NavigateUrl="javascript:p41_clone()"></telerik:RadMenuItem>
                    </Items>
                </telerik:RadMenuItem>
                <telerik:RadMenuItem Text="Akce" Value="akce" ImageUrl="Images/menuarrow.png">
                    <Items>

                        <telerik:RadMenuItem Text="Zobrazit přehled na celou stránku" Value="cmdFullScreen" NavigateUrl="javascript:p41_fullscreen()"></telerik:RadMenuItem>
                    </Items>
                </telerik:RadMenuItem>
                <telerik:RadMenuItem Text="Další" ImageUrl="Images/menuarrow.png">
                    <ContentTemplate>
                        <div style="padding: 20px; min-width: 200px;">
                            <button type="button" onclick="p41_subgrid_setting(<%=ViewState("j74id")%>)">Sloupce</button>
                            <div>
                                <span>Stránkování:</span>
                                <asp:DropDownList ID="cbxPaging" runat="server" AutoPostBack="true" ToolTip="Stránkování">
                                    <asp:ListItem Text="10"></asp:ListItem>
                                    <asp:ListItem Text="20"></asp:ListItem>
                                    <asp:ListItem Text="50" Selected="True"></asp:ListItem>
                                    <asp:ListItem Text="100"></asp:ListItem>
                                    <asp:ListItem Text="200"></asp:ListItem>
                                    <asp:ListItem Text="500"></asp:ListItem>
                                </asp:DropDownList>
                            </div>
                        </div>

                        <asp:Panel ID="panExport" runat="server" Style="margin-top: 10px;">
                            <div><strong>Export</strong></div>

                            <img src="Images/export.png" alt="export" />
                            <asp:LinkButton ID="cmdExport" runat="server" Text="Export" ToolTip="Export do MS EXCEL tabulky, plný počet záznamů" />

                            <img src="Images/xls.png" alt="xls" />
                            <asp:LinkButton ID="cmdXLS" runat="server" Text="XLS" ToolTip="Export do XLS vč. souhrnů s omezovačem na maximálně 2000 záznamů" />

                            <img src="Images/pdf.png" alt="pdf" />
                            <asp:LinkButton ID="cmdPDF" runat="server" Text="PDF" ToolTip="Export do PDF vč. souhrnů s omezovačem na maximálně 2000 záznamů" />

                            <img src="Images/doc.png" alt="doc" />
                            <asp:LinkButton ID="cmdDOC" runat="server" Text="DOC" ToolTip="Export do DOC vč. souhrnů s omezovačem na maximálně 2000 záznamů" />
                        </asp:Panel>
                    </ContentTemplate>
                </telerik:RadMenuItem>
            </Items>
        </telerik:RadMenu>



    </div>


    <div style="clear: both; width: 100%;"></div>

    <uc:datagrid ID="grid1" runat="server" ClientDataKeyNames="pid" OnRowSelected="RowSelected_p41" OnRowDblClick="RowDoubleClick_p41"></uc:datagrid>

    <asp:HiddenField ID="hidDefaultSorting" runat="server" />
    <asp:HiddenField ID="hidCols" runat="server" />
    <asp:HiddenField ID="hidFrom" runat="server" />
    <asp:HiddenField ID="hidMasterPrefix" runat="server" />
    <asp:HiddenField ID="hidMasterPID" runat="server" />
    <asp:HiddenField ID="hiddatapid_p41" runat="server" />

    <script type="text/javascript">
        $(document).ready(function () {



        });

        function GetAllSelectedP41IDs() {

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


        function p41_fullscreen() {
            window.open("p41_Framework.aspx?masterpid=<%=Me.CurrentMasterPID%>&masterprefix=<%=Me.CurrentMasterPrefix%>", "_top");

        }
    </script>




</asp:Content>
