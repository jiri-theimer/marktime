<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/ModalForm.Master" CodeBehind="p31_approving_step3.aspx.vb" Inherits="UI.p31_approving_step3" %>

<%@ MasterType VirtualPath="~/ModalForm.Master" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<%@ Register TagPrefix="uc" TagName="datagrid" Src="~/datagrid.ascx" %>
<%@ Register TagPrefix="uc" TagName="periodcombo" Src="~/periodcombo.ascx" %>
<%@ Register TagPrefix="uc" TagName="billingmemo" Src="~/billingmemo.ascx" %>


<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <script type="text/javascript">
        $(document).ready(function () {
            $(".slidingDiv1").hide();
            $(".show_hide1").show();
            $(".slidingDiv2").hide();
            $(".slidingDiv3").hide();

            $('.show_hide1').click(function () {
                $(".slidingDiv2").hide();
                $(".slidingDiv3").hide();
                $(".slidingDiv4").hide();
                $(".slidingDiv1").slideToggle();
            });

            $(".slidingDiv2").hide();
            $(".slidingDiv3").hide();
            
            $(".show_hide2").show();

            $('.show_hide2').click(function () {
                $(".slidingDiv1").hide();
                $(".slidingDiv3").hide();
                
                $(".slidingDiv2").slideToggle();
            });

            $('.show_hide3').click(function () {
                $(".slidingDiv1").hide();
                $(".slidingDiv2").hide();
                
                $(".slidingDiv3").slideToggle();
            });

          

        });



        function RowSelected(sender, args) {
            var pid = args.getDataKeyValue("pid");
            document.getElementById("<%=hiddatapid.clientid%>").value = pid;

            self.document.getElementById("<%=fraSubform.ClientID%>").src = "p31_approving_step3_subform.aspx?guid=<%=ViewState("guid")%>&pid=" + pid;

        }

        function RowDoubleClick(sender, args) {
            //nic
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

        function record_clone() {
            var pid = document.getElementById("<%=hiddatapid.clientid%>").value;
            if (pid == "" || pid == null) {
                alert("Není vybrán záznam.");
                return
            }
            sw_orig("p31_record.aspx?clone=1&pid=" + pid);

        }

        function hardrefresh(pid, flag) {

            document.getElementById("<%=Me.hidHardRefreshPID.ClientID%>").value = pid;
            document.getElementById("<%=Me.hidHardRefreshFlag.ClientID%>").value = flag;

            <%=Me.ClientScript.GetPostBackEventReference(Me.cmdRefresh, "", False)%>;

        }

        function griddesigner() {
            var j74id = "<%=Me.CurrentJ74ID%>";
            sw_orig("grid_designer.aspx?x29id=331&masterprefix=approving_step3&pid=" + j74id);
        }

        function batch_p31text() {

            //dialog_master("p31_approving_batch_p31text.aspx?guid=<%=viewstate("guid")%>", true);
            location.replace("p31_approving_batch_p31text.aspx?guid=<%=viewstate("guid")%>");
            
        }

        function p31_create(field,pid) {

            dialog_master("p31_record.aspx?"+field+"="+pid+"&pid=0&guid_approve=<%=viewstate("guid")%>",false,800,600);
        }


        function sw_orig(url, is_maximize, window_width, window_height) {
            var wnd = $find("<%=okno1.ClientID%>");
            wnd.setUrl(url);
            if (window_width != null)
                wnd.setSize(window_width, window_height);

            wnd.show();
            if (is_maximize == true) {
                wnd.maximize();
            }
            else {
                wnd.center();
            }

        }

        function o23_record(pid) {
           
            dialog_master("o23_record.aspx?billing=1&masterprefix=<%=Me.CurrentMasterPrefix%>&masterpid=<%=Me.CurrentMasterPID%>&pid=" + pid, true);

        }

        function report() {

            dialog_master("report_modal.aspx?prefix=app&guid=<%=viewstate("guid")%>", true);
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="OverMainContent" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="server">
    <div class="slidingDiv1" style="padding: 10px;">
        <div class="innerform_light">
            <div class="div6">
                <asp:Button ID="cmdBatch_4" Text="Vybrané záznamy [Fakturovat]" runat="server" CssClass="cmd" Width="280px" />
                <span style="padding-left: 50px;"></span>
                <asp:Button ID="cmdBatch_Clear" Text="Vybraným záznamům vyčistit schvalování" runat="server" CssClass="cmd" Width="280px" />
            </div>
            <div class="div6">
                <asp:Button ID="cmdBatch_6" Text="Vybrané záznamy [Zahrnout do paušálu]" runat="server" CssClass="cmd" Width="280px" />
                <span style="padding-left: 50px;"></span>
                <asp:Button ID="cmdBatch_ApprovingSet" Text="Vybrané záznamy zařadit do billing dávky:" runat="server" CssClass="cmd" Width="280px" />
                <telerik:RadComboBox ID="p31ApprovingSet" runat="server" ShowToggleImage="false" ShowDropDownOnTextboxClick="true" MarkFirstMatch="true" Width="200px" AllowCustomText="true" ToolTip="Název billing dávky"></telerik:RadComboBox>
            </div>
            <div class="div6">
                <asp:Button ID="cmdBatch_3" Text="Vybrané záznamy [Skrytý odpis]" runat="server" CssClass="cmd" Width="280px" />
                <span style="padding-left: 50px;"></span>
                <asp:Button ID="cmdBatch_ApprovingSet_Clear" Text="Vybraným vyčistit přiřazení billing dávky" runat="server" CssClass="cmd" Width="280px" />
            </div>
            <div class="div6">
                <asp:Button ID="cmdBatch_2" Text="Vybrané záznamy [Viditelný odpis]" runat="server" CssClass="cmd" Width="280px" />
                <span style="padding-left: 50px;"></span>
                <button type="button" onclick="batch_p31text()" style="width: 280px;">Hromadná úprava popisu, hodnoty a sazby úkonu (všechny úkony)</button>
            </div>
            <div class="div6">
                <asp:Button ID="cmdBatch_7" Text="Vybrané záznamy [Fakturovat později]" runat="server" CssClass="cmd" Width="280px" />
                
            </div>

        </div>
    </div>
    <div class="slidingDiv2" style="padding: 10px;">
        <div class="innerform_light">
            <div class="div6">
              
                Šablona datového přehledu (sloupce):
                <asp:DropDownList ID="j74id" runat="server" AutoPostBack="true" DataTextField="j74Name" DataValueField="pid" Style="width: 250px;" ToolTip="Šablony datového přehledu"></asp:DropDownList>
                <button type="button" onclick="griddesigner()">Návrhář sloupců</button>
            </div>
            <div class="div6">
                <asp:CheckBox ID="chkAutoFilter" runat="server" Text="Nabízet nad sloupci filtrování dat" AutoPostBack="true" CssClass="chk" />
                <asp:CheckBox ID="chkUseInternalApproving" runat="server" Text="Využívat i interní (vnitropodnikové) schvalování" AutoPostBack="true" CssClass="chk" />                
            </div>
            <fieldset>
                <legend>Souhrny</legend>
            <div class="div6">
                    <asp:RadioButtonList ID="opgGroupBy" runat="server" RepeatDirection="Horizontal" AutoPostBack="true">
                        <asp:ListItem Text="Bez souhrnů" Value="" Selected="true"></asp:ListItem>
                        <asp:ListItem Text="Fakturační oddíl" Value="p95Name"></asp:ListItem>
                        <asp:ListItem Text="Fakturační status" Value="approve_p72Name"></asp:ListItem>
                        <asp:ListItem Text="Datum úkonu" Value="p31Date"></asp:ListItem>
                        <asp:ListItem Text="Sešit" Value="p34Name"></asp:ListItem>
                        <asp:ListItem Text="Aktivita" Value="p32Name"></asp:ListItem>
                        <asp:ListItem Text="Osoba" Value="Person"></asp:ListItem>
                        <asp:ListItem Text="Projekt" Value="p41Name"></asp:ListItem>
                        <asp:ListItem Text="Klient" Value="ClientName"></asp:ListItem>
                        <asp:ListItem Text="Úkol" Value="p56Name"></asp:ListItem>
                    </asp:RadioButtonList>
                </div>
            </fieldset>
        </div>
    </div>
    <div class="slidingDiv3">
        <uc:billingmemo ID="bm1" runat="server" />
    </div>
    

    <div style="height: 60px; width: 100%;">
        <table cellpadding="5" cellspacing="2">
            <tr>
                <td>Počet úkonů:
                </td>
                <td align="right">
                    <asp:Label ID="RowCount" runat="server" CssClass="valbold"></asp:Label>
                </td>
                <td>Vykázaný čas (fakturovatelný):
                </td>
                <td align="right">
                    <asp:Label ID="hours_billable_orig" runat="server" CssClass="valbold"></asp:Label>h.
                <asp:Label ID="fee_billable_orig" runat="server" CssClass="valboldblue"></asp:Label>
                </td>
                <td>
                    <img src='Images/a14.gif' />Fakturovat čas:
                </td>
                <td align="right">
                    <asp:Label ID="hours_4" runat="server" CssClass="valbold"></asp:Label>h.
                <asp:Label ID="fee_4" runat="server" CssClass="valbold" ForeColor="green"></asp:Label>
                </td>
                <td>
                    <asp:Image ID="imgProfitLost_Time" runat="server" runat="server" />
                    <asp:Label ID="profit_lost_time" runat="server" CssClass="valbold" BackColor="Yellow"></asp:Label>
                </td>
                <td>
                    <img src='Images/a12.gif' />Viditelný odpis:
                </td>
                <td align="right">
                    <asp:Label ID="hours_2" runat="server" CssClass="valbold"></asp:Label>h.
                </td>
                <td></td>
                <td></td>
                <td></td>
            </tr>
            <tr>
                <td>Schválené úkony:
                </td>
                <td>
                    <asp:Label ID="RowsCount_Approved" runat="server" CssClass="valbold"></asp:Label>
                </td>
                <td>Ostatní vykázané příjmy:
                </td>
                <td align="right">
                    <asp:Label ID="other_income_orig" runat="server" CssClass="valboldblue"></asp:Label>
                </td>
                <td>
                    <img src='Images/a14.gif' />Fakturovat ostatní:
                </td>
                <td align="right">
                    <asp:Label ID="other_income_approved" runat="server" CssClass="valbold" ForeColor="green"></asp:Label>
                </td>
                <td>
                    <asp:Image ID="imgProfitLost_Other" runat="server" runat="server" />
                    <asp:Label ID="profit_lost_other" runat="server" CssClass="valbold" BackColor="Yellow"></asp:Label>
                </td>
                <td>
                    <img src='Images/a13.gif' />Skrytý odpis:
                </td>
                <td align="right">
                    <asp:Label ID="hours_3" runat="server" CssClass="valbold"></asp:Label>h.
                </td>
                <td>
                    <img src='Images/a16.gif' />Zahrnout do paušálu:
                </td>
                <td align="right">
                    <asp:Label ID="hours_6" runat="server" CssClass="valbold"></asp:Label>h.
                </td>
                <td></td>
            </tr>
        </table>
    </div>
    <asp:Button ID="cmdRefresh" runat="server" Style="display: none;" />
    <div style="clear:both;"></div>
    <table width="100%" cellpadding="0" cellspacing="0">
        <tr valign="top">
            <td style="min-width: 400px; min-height: 450px;">
                <iframe id="fraSubform" runat="server" width="100%" height="460px" frameborder="0" src=""></iframe>
               

            </td>
            <td>
                <uc:datagrid ID="grid1" runat="server" ClientDataKeyNames="pid" OnRowSelected="RowSelected" OnRowDblClick="RowDoubleClick"></uc:datagrid>
                

            </td>

        </tr>
    </table>
    


    <asp:HiddenField ID="hiddatapid" runat="server" />
    <asp:HiddenField ID="hidHardRefreshFlag" runat="server" />
    <asp:HiddenField ID="hidHardRefreshPID" runat="server" />
    
    <asp:HiddenField ID="hidMasterPrefix" runat="server" />
    <asp:HiddenField ID="hidMasterPID" runat="server" />
    <asp:HiddenField ID="hidCols" runat="server" />
    <asp:HiddenField ID="hidFrom" runat="server" />


    <telerik:RadWindow ID="okno1" runat="server" Modal="true" VisibleTitlebar="true" VisibleStatusbar="false" Skin="WebBlue" ShowContentDuringLoad="false" Width="800px" Height="600px" Behaviors="Close,Move,Maximize" IconUrl="Images/window.png" KeepInScreenBounds="true" Style="z-index: 9900;">
        <Shortcuts>
            <telerik:WindowShortcut CommandName="Close" Shortcut="Esc" />
        </Shortcuts>
    </telerik:RadWindow>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="FootContent" runat="server">
</asp:Content>
