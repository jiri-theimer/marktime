<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master" CodeBehind="pokus.aspx.vb" Inherits="UI.pokus" %>

<%@ MasterType VirtualPath="~/Site.Master" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<%@ Register TagPrefix="uc" TagName="datagrid" Src="~/datagrid.ascx" %>
<%@ Register TagPrefix="uc" TagName="periodcombo" Src="~/periodcombo.ascx" %>
<%@ Register TagPrefix="uc" TagName="mygrid" Src="~/mygrid.ascx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <style type="text/css">   
        html .RadNavigation_Windows7 .rnvRootGroupWrapper {
    background-color: #25a0da;
    background-image: none;
    color:white;
}
            
        html .RadNavigation .rnvMore,
html .RadNavigation .rnvRootLink {
    padding-left:15px;
   padding-top:5px;
   padding-bottom:5px;
   
   
   
   }

      
    </style>

    <script type="text/javascript">
        function gogo(par) {
            window.history.pushState({}, "Ahoj-titulek", "/pokus.aspx?gogo="+par);
            
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <img src="Images/logo_transparent.png" style="position:absolute;left:10px;top:40px;z-index:8000;" />
   
    <telerik:RadNavigation ID="nav1" runat="server" MenuButtonPosition="Right" Skin="Windows7">
      
        <CollapseAnimation Type="None" />
        <ExpandAnimation Type="None" />
        <Nodes>
            <telerik:NavigationNode Text=" " Width="200px" Enabled="false">
                
            </telerik:NavigationNode>
            <telerik:NavigationNode ImageUrl="Images/new_menu.png">
                <Nodes>
                    <telerik:NavigationNode Text="Worksheet úkon"></telerik:NavigationNode>
                    <telerik:NavigationNode Text="Projekt"></telerik:NavigationNode>
                </Nodes>
               
            </telerik:NavigationNode>
           <telerik:NavigationNode Text="ÚVOD"></telerik:NavigationNode>
            <telerik:NavigationNode Text="WORKSHEET">
                <Nodes>
                    <telerik:NavigationNode Text="Zapisovat"></telerik:NavigationNode>
                    <telerik:NavigationNode Text="Datový přehled"></telerik:NavigationNode>
                    <telerik:NavigationNode Text="Schvalovat, připravit podklady k fakturaci"></telerik:NavigationNode>
                </Nodes>
            </telerik:NavigationNode>
            <telerik:NavigationNode Text="PROJEKTY"></telerik:NavigationNode>
            <telerik:NavigationNode text="KLIENTI"></telerik:NavigationNode>
            <telerik:NavigationNode Text="FAKTURY"></telerik:NavigationNode>
            <telerik:NavigationNode Text="ÚKOLY"></telerik:NavigationNode>
            <telerik:NavigationNode Text="DALŠÍ"></telerik:NavigationNode>
            <telerik:NavigationNode Text="Akce">
                <ContentTemplate>
                    <div class="content-box3">
                                        <div class="title">
                                            <img src="Images/query.png" />
                                            <span>Dodatečné filtrování záznamů</span>
                                        </div>
                                        <div class="content">
                                            <div>
                                                <button type="button" onclick="x18_querybuilder()">
                                                    <img src="Images/label.png" />Štítky</button>
                                                <asp:ImageButton ID="cmdClearX18" runat="server" ToolTip="Vyčistit štítkovací filtr" ImageUrl="Images/delete.png" Visible="false" CssClass="button-link" />
                                                <asp:Label ID="x18_querybuilder_info" runat="server" ForeColor="Red"></asp:Label>
                                            </div>
                                            <div style="margin-top: 20px;">

                                                <asp:DropDownList ID="cbxQueryFlag" runat="server" AutoPostBack="true">
                                                    <asp:ListItem Text="" Value=""></asp:ListItem>
                                                </asp:DropDownList>
                                            </div>
                                            <div style="margin-top: 20px;">
                                                <asp:DropDownList ID="cbxPeriodType" AutoPostBack="true" runat="server" ToolTip="Druh filtrovaného období">
                                                </asp:DropDownList>
                                                <uc:periodcombo ID="period1" runat="server" Width="160px"></uc:periodcombo>
                                            </div>
                                        </div>
                                    </div>

                                    <div class="content-box3" style="margin-top: 20px;">
                                        <div class="title">
                                            <img src="Images/batch.png" />
                                            <span>Operace pro označené (zaškrtlé) záznamy</span>
                                        </div>
                                        <div class="content" style="text-align:center;">
                                            <div class="div6">
                                            <button type="button" id="buttonBatch" onclick="batch()" title="Hromadné operace nad označenými záznamy v přehledu" style="display: none;">Hromadné operace</button>
                                            </div>
                                            <div class="div6">
                                            <button id="cmdApprove" runat="server" type="button" visible="false" onclick="approve()">Schválit/připravit k fakturaci</button>
                                                </div>
                                            <div class="div6">
                                            <button id="cmdInvoice" runat="server" type="button" visible="false" onclick="invoice()">Zrychlená fakturace bez schvalování</button>
                                                </div>
                                            <div class="div6">
                                            <button type="button" onclick="report()" title="Tisková sestava">Tisková sestava</button>
                                            </div>
                                            <button type="button" id="buttonBatchMail" onclick="sendmail_batch()" style="display: none;">Hromadně odeslat faktury (e-mail)</button>

                                            <button type="button" id="cmdSummary" runat="server" onclick="drilldown()">WORKSHEET statistika</button>

                                        </div>
                                    </div>
                                    <asp:Panel ID="panExport" runat="server" CssClass="content-box3" Style="margin-top: 20px;">
                                        <div class="title">
                                            <img src="Images/export.png" />
                                            <span>Export záznamů v aktuálním přehledu</span>

                                        </div>
                                        <div class="content">

                                            <img src="Images/export.png" alt="export" />
                                            <asp:LinkButton ID="cmdExport" runat="server" Text="Export" ToolTip="Export do MS EXCEL tabulky, plný počet záznamů" />

                                            <img src="Images/xls.png" alt="xls" />
                                            <asp:LinkButton ID="cmdXLS" runat="server" Text="XLS" ToolTip="Export do XLS vč. souhrnů s omezovačem na maximálně 2000 záznamů" />

                                            <img src="Images/pdf.png" alt="pdf" />
                                            <asp:LinkButton ID="cmdPDF" runat="server" Text="PDF" ToolTip="Export do PDF vč. souhrnů s omezovačem na maximálně 2000 záznamů" />

                                            <img src="Images/doc.png" alt="doc" />
                                            <asp:LinkButton ID="cmdDOC" runat="server" Text="DOC" ToolTip="Export do DOC vč. souhrnů s omezovačem na maximálně 2000 záznamů" />


                                        </div>
                                    </asp:Panel>
                                    <div class="content-box3" style="margin-top: 20px;">
                                        <div class="title">
                                            <img src="Images/griddesigner.png" />
                                            <span>Sloupce v přehledu</span>

                                        </div>
                                        <div class="content">

                                            <asp:DropDownList ID="cbxGroupBy" runat="server" AutoPostBack="true" ToolTip="Datové souhrny" DataTextField="ColumnHeader" DataValueField="ColumnField">
                                            </asp:DropDownList>




                                            <div class="div6">
                                                <span class="val">Stránkování přehledu:</span>
                                                <asp:DropDownList ID="cbxPaging" runat="server" AutoPostBack="true" ToolTip="Stránkování" TabIndex="3">
                                                    <asp:ListItem Text="20"></asp:ListItem>
                                                    <asp:ListItem Text="50" Selected="True"></asp:ListItem>
                                                    <asp:ListItem Text="100"></asp:ListItem>
                                                    <asp:ListItem Text="200"></asp:ListItem>
                                                    <asp:ListItem Text="500"></asp:ListItem>
                                                </asp:DropDownList>
                                                <asp:CheckBox ID="chkGroupsAutoExpanded" runat="server" Text="Auto-rozbalené souhrny" AutoPostBack="true" Checked="false" />
                                                <div>
                                                <asp:CheckBox ID="chkCheckboxSelector" runat="server" Text="Možnost označovat záznamy zaškrtnutím (checkbox)" AutoPostBack="true" />
                                                    </div>
                                            </div>


                                        </div>
                                    </div>
                                    <div class="content-box3" style="margin-top: 20px;">
                                        <div class="title">
                                            <img src="Images/saw_turn_on.png" /><img src="Images/saw_turn_off.png" />
                                            <span>Rozvržení panelů</span>
                                        </div>
                                        <div class="content">

                                            <asp:RadioButtonList ID="opgLayout" runat="server" AutoPostBack="true" RepeatDirection="Vertical">
                                                <asp:ListItem Text="Levý panel = přehled, pravý panel = detail" Value="1" Selected="True"></asp:ListItem>
                                                <asp:ListItem Text="Pouze jeden panel - buď přehled nebo vybraný záznam na dvoj-klik" Value="3"></asp:ListItem>
                                                <asp:ListItem Text="Horní panel = přehled, spodní panel = detail" Value="2"></asp:ListItem>
                                            </asp:RadioButtonList>
                                            <asp:Label ID="lblLayoutMessage" runat="server" CssClass="infoNotification" Text="Z důvodu malého rozlišení displeje (pod 1280px) se automaticky zapnul režim jediného panelu s datovým přehledem." Visible="false"></asp:Label>

                                        </div>
                                    </div>
                </ContentTemplate>
            </telerik:NavigationNode>
        </Nodes>
    </telerik:RadNavigation>
   
    <div style="clear:both;"></div>
    Hodiny: <asp:TextBox ID="txtHours" runat="server" Text="2,58333"></asp:TextBox>
    
   
    
    <asp:Button ID="cmdPokus" runat="server" Text="test" />

     
   
    <asp:TextBox ID="txtResult" runat="server"></asp:TextBox>

    <asp:HiddenField ID="hiddatapid" runat="server" />


    <button type="button" onclick="gogo('1')">Změnit URL 1</button>
    <button type="button" onclick="gogo('2')">Změnit URL 2</button>

    <p></p><p></p>

    <telerik:RadScheduler ID="scheduler1" SelectedView="AgendaView" RenderMode="Lightweight" FirstDayOfWeek="Monday" LastDayOfWeek="Sunday" Width="600px" Height="300px" EnableViewState="false" Skin="Default" AppointmentStyleMode="Simple" ShowFooter="false" runat="server" ShowViewTabs="false" EnableAdvancedForm="false" ShowHeader="true"
                Culture="cs-CZ" AllowEdit="false" AllowDelete="false" AllowInsert="false"             
                HoursPanelTimeFormat="HH:mm" ShowNavigationPane="true"
                DataSubjectField="o22Name" DataStartField="o22DateFrom" DataEndField="o22DateUntil" DataKeyField="pid">
                <Localization HeaderAgendaDate="Datum" AllDay="Bez času od/do" HeaderMonth="Měsíc" HeaderDay="Den" HeaderMultiDay="Multi-den" HeaderWeek="Týden" ShowMore="více..." HeaderToday="Dnes" HeaderAgendaAppointment="Úkol nebo Událost" HeaderAgendaTime="Čas" />
                
                <AgendaView UserSelectable="false" NumberOfDays="200" ShowDateHeaders="true" ReadOnly="true" ShowColumnHeaders="false" />
                <DayView UserSelectable="true" DayStartTime="06:00" DayEndTime="22:00" ShowInsertArea="false" />

                <AppointmentTemplate>
                    <a class="reczoom" rel="<%# Eval("Description")%>">i</a>
                    <a href="javascript:re(<%# Eval("ID")%>)"><%# Eval("Subject")%></a>


                </AppointmentTemplate>
        
                
                
            </telerik:RadScheduler>
            

    



    
</asp:Content>



