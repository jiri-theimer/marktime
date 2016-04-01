<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master" CodeBehind="j03_mypage_greeting.aspx.vb" Inherits="UI.j03_mypage_greeting" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<%@ Register TagPrefix="uc" TagName="contact" Src="~/contact.ascx" %>
<%@ Register TagPrefix="uc" TagName="project" Src="~/project.ascx" %>

<%@ MasterType VirtualPath="~/Site.Master" %>

<asp:Content ID="HeaderContent" runat="server" ContentPlaceHolderID="HeadContent">
    <script type="text/javascript">
        function sw_local(url, img, is_maximize) {
            sw_master(url, img, is_maximize);
        }

        function personalpage() {
            sw_master("j03_myprofile_defaultpage.aspx", "Images/plugin_32.png");


        }
        function p28_create() {
            sw_master("p28_record.aspx?pid=0", "Images/contact_32.png");


        }
        function p41_create() {
            sw_master("p41_create.aspx", "Images/project_32.png");
        }

        function p56_create() {
            sw_master("p56_record.aspx?masterprefix=p41&masterpid=0", "Images/task_32.png");

        }
        function p91_create() {
            sw_master("p91_create_step1.aspx?prefix=p28", "Images/invoice_32.png", true);


        }
        function p31_create() {
            sw_master("p31_record.aspx?pid=0", "Images/worksheet_32.png")


        }
        function o23_create() {
            sw_master("o23_record.aspx?pid=0", "Images/notepad_32.png")


        }
        function hardrefresh(pid, flag) {
            if (flag == "p41-create" || flag == "p41-save") {
                location.replace("p41_framework.aspx?pid=" + pid);
                return;
            }
            if (flag == "p56-save" || flag == "p56-create") {
                location.replace("p31_framework.aspx");
                return;
            }
            if (flag == "p91-create" || flag == "p91-save") {
                location.replace("p91_framework.aspx?pid=" + pid);
                return;
            }

            if (flag == "p28-save" || flag == "p28-create") {
                location.replace("p28_framework.aspx?pid=" + pid);
                return;
            }
            if (flag == "o23-save" || flag == "o23-create") {
                location.replace("o23_framework.aspx?pid=" + pid);
                return;
            }

            location.replace("j03_mypage_greeting.aspx");

        }

        function p48_record(pid) {

            sw_master("p48_multiple_edit_delete.aspx?p48ids=" + pid, "Images/oplan_32.png")
        }
        function p48_convert(pid) {

            sw_master("p31_record.aspx?pid=0&p48id=" + pid, "Images/worksheet_32.png")
        }
    </script>
</asp:Content>
<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="MainContent">
    <div style="padding: 10px; background-color: white;">
        <div style="float: left;">
            <asp:Label ID="lblHeader" runat="server" CssClass="framework_header_span" Style="font-size: 200%;" Text="Vítejte v systému"></asp:Label>
        </div>
        <div style="float: left; margin-top: 7px; padding-left: 10px;">
            <img src="Images/logo_transparent.png" />
        </div>
        <div style="clear: both;"></div>


        <div style="min-height: 430px;">
            <div style="float: left;">
                <telerik:RadPanelBar ID="menu1" runat="server" RenderMode="Auto" Skin="Default" Width="300px">
                    <Items>
                        <telerik:RadPanelItem Text="Pracuji v MARKTIME..." Expanded="true">
                            <Items>
                                <telerik:RadPanelItem Text="Založit nového klienta" Value="p28_create" NavigateUrl="javascript:p28_create()" ImageUrl="Images/contact.png"></telerik:RadPanelItem>
                                <telerik:RadPanelItem Text="Založit nový projekt" Value="p41_create" NavigateUrl="javascript:p41_create()" ImageUrl="Images/project.png"></telerik:RadPanelItem>
                                <telerik:RadPanelItem Text="Zapsat worksheet úkon" Value="p31_create" NavigateUrl="p31_framework.aspx" ImageUrl="Images/worksheet.png"></telerik:RadPanelItem>
                                <telerik:RadPanelItem Text="Worksheet KALENDÁŘ" Value="p31_scheduler" NavigateUrl="p31_scheduler.aspx" ImageUrl="Images/worksheet.png"></telerik:RadPanelItem>

                                <telerik:RadPanelItem Text="Vytvořit nový úkol" Value="p56_create" NavigateUrl="javascript:p56_create()" ImageUrl="Images/task.png"></telerik:RadPanelItem>
                                <telerik:RadPanelItem Text="Vytvořit nový dokument" Value="o23_create" NavigateUrl="javascript:o23_create()" ImageUrl="Images/notepad.png"></telerik:RadPanelItem>

                                <telerik:RadPanelItem Text="Schvalovat | Připravit podklady k fakturaci" Value="approve" NavigateUrl="approving_framework.aspx" ImageUrl="Images/approve.png"></telerik:RadPanelItem>
                                <telerik:RadPanelItem Text="Vystavit fakturu" Value="p91_create" NavigateUrl="javascript:p91_create()" ImageUrl="Images/invoice.png"></telerik:RadPanelItem>

                                <telerik:RadPanelItem Text="Tiskové sestavy" Value="report" NavigateUrl="report_framework.aspx" ImageUrl="Images/report.png"></telerik:RadPanelItem>
                                <telerik:RadPanelItem Text="Administrace systému" Value="admin" NavigateUrl="admin_framework.aspx" ImageUrl="Images/setting.png"></telerik:RadPanelItem>
                            </Items>
                        </telerik:RadPanelItem>
                        <telerik:RadPanelItem Text="Osobní nastavení">
                            <Items>
                                <telerik:RadPanelItem Text="Zvolit si jinou startovací (výchozí) stránku" NavigateUrl="javascript:personalpage()" ImageUrl="Images/plugin.png"></telerik:RadPanelItem>
                                <telerik:RadPanelItem Text="Můj profil" NavigateUrl="j03_myprofile.aspx" ImageUrl="Images/user.png"></telerik:RadPanelItem>
                                <telerik:RadPanelItem Text="Změnit si heslo" NavigateUrl="changepassword.aspx" ImageUrl="Images/password.png"></telerik:RadPanelItem>
                            </Items>
                        </telerik:RadPanelItem>
                    </Items>

                </telerik:RadPanelBar>
            </div>
            <asp:Panel ID="panP56" runat="server" CssClass="content-box1">
                <div class="title">
                    <img src="Images/task.png" alt="Úkol" />
                    Blízké úkoly (otevřené) s termínem (+-1 den)
                    <asp:Label ID="p56Count" runat="server" CssClass="badge1"></asp:Label>
                </div>
                <div class="content">
                    <asp:Repeater ID="rpP56" runat="server">
                        <ItemTemplate>
                            <div class="div6">
                                <asp:HyperLink ID="clue1" runat="server" CssClass="reczoom" Text="i" title="Detail úkolu"></asp:HyperLink>
                                <asp:HyperLink ID="link1" runat="server"></asp:HyperLink>
                                <asp:Label ID="p56PlanUntil" runat="server"></asp:Label>
                                <asp:Image ID="img1" runat="server" ImageUrl="Images/reminder.png" ToolTip="Připomenutí" />
                            </div>
                        </ItemTemplate>
                    </asp:Repeater>
                </div>
            </asp:Panel>
            <asp:Panel ID="panO22" runat="server" CssClass="content-box1">
                <div class="title">
                    <img src="Images/calendar.png" alt="Kalendářová událost" />
                    Blízké kalendářové události/milníky (+-1 den)
                    <asp:Label ID="o22Count" runat="server" CssClass="badge1"></asp:Label>
                    <a href="entity_scheduler.aspx">Kalendář</a>
                </div>
                <div class="content">
                    <asp:Repeater ID="rpO22" runat="server">
                        <ItemTemplate>
                            <div class="div6">
                                <asp:HyperLink ID="clue1" runat="server" CssClass="reczoom" Text="i" title="Detail milníku"></asp:HyperLink>
                                <asp:HyperLink ID="link1" runat="server"></asp:HyperLink>
                                <asp:Image ID="img1" runat="server" ImageUrl="Images/reminder.png" ToolTip="Připomenutí" />
                            </div>
                        </ItemTemplate>
                    </asp:Repeater>
                </div>
            </asp:Panel>
            <asp:Panel ID="panO23" runat="server" CssClass="content-box1">
                <div class="title">
                    <img src="Images/notepad.png" alt="Dokument" />
                    Dokumenty s připomenutím +-1 den
                    
                    <asp:Label ID="o23Count" runat="server" CssClass="badge1"></asp:Label>
                </div>
                <div class="content">
                    <asp:Repeater ID="rpO23" runat="server">
                        <ItemTemplate>
                            <div class="div6">
                                <asp:HyperLink ID="clue1" runat="server" CssClass="reczoom" Text="i" title="Detail dokumentu"></asp:HyperLink>
                                <asp:HyperLink ID="link1" runat="server"></asp:HyperLink>
                                <asp:Image ID="img1" runat="server" ImageUrl="Images/reminder.png" ToolTip="Připomenutí" />
                            </div>
                        </ItemTemplate>
                    </asp:Repeater>
                </div>
            </asp:Panel>

            <asp:Panel ID="panP39" runat="server" CssClass="content-box1">
                <div class="title">
                    <img src="Images/worksheet_recurrence.png" />
                    Blízké generování opakovaných odměn/paušálů/úkonů
                    <asp:Label ID="p39Count" runat="server" CssClass="badge1"></asp:Label>
                </div>
                <div class="content">

                    <asp:Repeater ID="rpP39" runat="server">
                        <ItemTemplate>
                            <div class="div6">
                                <asp:HyperLink ID="cmdProject" runat="server"></asp:HyperLink>

                                <asp:Label ID="p39Text" runat="server" Font-Italic="true"></asp:Label>
                            </div>
                            <div class="div6">
                                <span>Kdy generovat:</span>
                                <asp:Label ID="p39DateCreate" runat="server" ForeColor="red"></asp:Label>
                                <span>Datum úkonu:</span>
                                <asp:Label ID="p39Date" runat="server" ForeColor="green"></asp:Label>
                            </div>

                        </ItemTemplate>
                    </asp:Repeater>

                </div>
            </asp:Panel>
            <asp:Panel ID="panP48" runat="server" CssClass="content-box1">
                <div class="title">
                    <img src="Images/oplan.png" alt="Plán" />
                    Operativní plán (čeká na překlopení)
                    <asp:Label ID="p48Count" runat="server" CssClass="badge1"></asp:Label>
                    <a href="entity_scheduler.aspx">Kalendář</a>
                </div>
                <div class="content">
                    <asp:Repeater ID="rpP48" runat="server">
                        <ItemTemplate>
                            <div class="div6">
                                <asp:HyperLink ID="clue1" runat="server" CssClass="reczoom" Text="i" title="Detail plánu"></asp:HyperLink>
                                <asp:Label ID="p48Date" runat="server" ForeColor="green"></asp:Label>
                                <asp:Label ID="p48Hours" runat="server" CssClass="valboldblue"></asp:Label>
                                <asp:HyperLink ID="link1" runat="server"></asp:HyperLink>
                                <asp:HyperLink ID="convert1" runat="server" Text="Překlopit" ForeColor="green"></asp:HyperLink>

                            </div>
                        </ItemTemplate>
                    </asp:Repeater>
                </div>
            </asp:Panel>
            <asp:Panel ID="panChart1" runat="server" Style="float: right;" Visible="false">
                <telerik:RadHtmlChart runat="server" ID="chart1" width="400px">
                    <PlotArea>
                        <Series>
                            <telerik:ColumnSeries Name="Hodiny Fa" DataFieldY="HodinyFa" Stacked="true">
                                <Appearance FillStyle-BackgroundColor="LightGreen"></Appearance>
                            </telerik:ColumnSeries>
                            <telerik:ColumnSeries Name="Hodiny NeFa" DataFieldY="HodinyNeFa">
                                <Appearance FillStyle-BackgroundColor="#ff9999"></Appearance>
                            </telerik:ColumnSeries>
                        </Series>
                        <XAxis DataLabelsField="Datum"></XAxis>
                        <YAxis></YAxis>
                    </PlotArea>
                </telerik:RadHtmlChart>
            </asp:Panel>
            <asp:Panel ID="panChart2" runat="server" Style="float: right;" Visible="false">
                <telerik:RadHtmlChart runat="server" ID="chart2" Width="500px" >  
                    <Legend>
                        <Appearance Position="Right"></Appearance>
                    </Legend>                  
                    <PlotArea>
                        <Series>
                            <telerik:PieSeries NameField="Podle" DataFieldY="Hodiny" StartAngle="90">                                
                                <LabelsAppearance Position="OutsideEnd" DataFormatString="{0} h.">                                   
                        </LabelsAppearance>                                                  
                            </telerik:PieSeries>
                        </Series>
                       
                    </PlotArea>
                </telerik:RadHtmlChart>
            </asp:Panel>

            <div style="float: right;">

                <asp:Image runat="server" ID="imgWelcome" ImageUrl="Images/welcome/start.jpg" Visible="false" />
            </div>




            <div style="clear: both;"></div>

            <div style="margin-top: 20px;">
                <asp:Label ID="lblBuild" runat="server" Style="color: gray;" />


            </div>
        </div>
</asp:Content>

