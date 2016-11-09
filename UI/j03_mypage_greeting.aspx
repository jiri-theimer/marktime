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
        function report() {
            sw_master("report_modal.aspx?prefix=j02&pid=<%=Master.Factory.SysUser.j02ID%>", "Images/reporting.png", true);

        }
        function sendmail() {
            sw_master("sendmail.aspx", "Images/email_32.png")


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
                                
                                <telerik:RadPanelItem Text="Zapisovat úkony" Value="p31_create" NavigateUrl="p31_framework.aspx" ImageUrl="Images/worksheet.png"></telerik:RadPanelItem>
                                <telerik:RadPanelItem Text="Worksheet KALENDÁŘ" Value="p31_scheduler" NavigateUrl="p31_scheduler.aspx" ImageUrl="Images/worksheet.png"></telerik:RadPanelItem>
                                
                                
                                <telerik:RadPanelItem Text="Schvalovat | Připravit podklady k fakturaci" Value="approve" NavigateUrl="approving_framework.aspx" ImageUrl="Images/approve.png"></telerik:RadPanelItem>
                                
                                <telerik:RadPanelItem Text="Osobní tiskové sestavy" Value="myreport" NavigateUrl="javascript:report()" ImageUrl="Images/report.png"></telerik:RadPanelItem>
                                <telerik:RadPanelItem Text="Tiskové sestavy" Value="report" NavigateUrl="report_framework.aspx" ImageUrl="Images/report.png"></telerik:RadPanelItem>
                                <telerik:RadPanelItem Text="Administrace systému" Value="admin" NavigateUrl="admin_framework.aspx" ImageUrl="Images/setting.png"></telerik:RadPanelItem>

                                
                            </Items>
                        </telerik:RadPanelItem>

                        <telerik:RadPanelItem Text="Oblíbené projekty" Value="favourites" ImageUrl="Images/favourite.png" Visible="false">

                        </telerik:RadPanelItem>
                        <telerik:RadPanelItem Text="Další" Expanded="true">
                            <Items>
                            <telerik:RadPanelItem Text="Vytvořit úkol" Value="p56_create" NavigateUrl="javascript:p56_create()" ImageUrl="Images/task.png"></telerik:RadPanelItem>
                            <telerik:RadPanelItem Text="Napsat článek na nástěnku" Value="o10_create" NavigateUrl="javascript:o10_create()" ImageUrl="Images/article.png"></telerik:RadPanelItem>
                                

                                <telerik:RadPanelItem Text="Rozhraní pro mobilní zařízení" Value="mobile" NavigateUrl="Mobile/default.aspx" ImageUrl="Images/mobile.png"></telerik:RadPanelItem>
                            </Items>
                        </telerik:RadPanelItem>
                        <telerik:RadPanelItem Text="Osobní nastavení">
                            <Items>
                                <telerik:RadPanelItem Text="Zvolit si jinou startovací (výchozí) stránku" NavigateUrl="javascript:personalpage()" ImageUrl="Images/plugin.png"></telerik:RadPanelItem>
                                <telerik:RadPanelItem Text="Můj profil" NavigateUrl="j03_myprofile.aspx" ImageUrl="Images/user.png"></telerik:RadPanelItem>
                                <telerik:RadPanelItem Text="Odeslat poštovní zprávu" Value="sendmail" NavigateUrl="javascript:sendmail()" ImageUrl="Images/email.png"></telerik:RadPanelItem>
                                <telerik:RadPanelItem Text="Změnit si heslo" NavigateUrl="changepassword.aspx" ImageUrl="Images/password.png"></telerik:RadPanelItem>
                                
                            </Items>
                        </telerik:RadPanelItem>
                        
                    </Items>

                </telerik:RadPanelBar>
                
            </div>
            
            <asp:Panel ID="panP56" runat="server" CssClass="content-box1">
                <div class="title">
                    <img src="Images/task.png" alt="Úkol" />
                    Blízké úkoly (otevřené) s termínem
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
            <asp:Panel ID="panX47" runat="server" CssClass="content-box1" style="margin-left:30px;">
                <div class="title">
                    <img src="Images/timeline.png" />
                    Poslední významnější akce
                   
                    
                </div>
                <div class="content">
                    <table cellpadding="4" >
                    <asp:Repeater ID="rpX47" runat="server">
                        <ItemTemplate>
                            <tr class="trHover" valign="top">
                                <td>
                                <asp:Image ID="img1" runat="server" />
                                    <asp:Label ID="lbl1" runat="server" CssClass="timestamp"></asp:Label>
                                </td>
                               
                                <td>
                                    <asp:HyperLink ID="link1" runat="server"></asp:HyperLink>
                                    <asp:Label ID="lbl2" runat="server"></asp:Label>
                                </td>
                                
                                
                                
                                    <td>
                                    
                                    <asp:Label ID="timestamp" runat="server" CssClass="timestamp"></asp:Label>                                
                                </td>
                                
                            </tr>
                            
                        </ItemTemplate>
                    </asp:Repeater>
                    </table>
                    <hr />
                    <asp:CheckBox ID="chkP41" runat="server" Text="Nové projekty" AutoPostBack="true" Checked="true" Visible="false" />
                    <asp:CheckBox ID="chkP28" runat="server" Text="Noví klienti" AutoPostBack="true" Checked="true" Visible="false" />
                    <asp:CheckBox ID="chkP91" runat="server" Text="Nové faktury" AutoPostBack="true" Checked="false" Visible="false" />
                    <asp:CheckBox ID="chkP56" runat="server" Text="Nové úkoly" AutoPostBack="true" Checked="false" Visible="false" />
                    <asp:CheckBox ID="chkO23" runat="server" Text="Nové dokumenty" AutoPostBack="true" Checked="false" Visible="false" />
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
                <telerik:RadHtmlChart runat="server" ID="chart1" width="600px" Font-Size="Small">
                    <ChartTitle Text="Vykázané hodiny po dnech (14 dní dozadu)">                        
                    </ChartTitle>
                    <PlotArea>
                        <Series>                          
                            <telerik:ColumnSeries Name="Hodiny Fa" DataFieldY="HodinyFa" Stacked="true">
                                <Appearance FillStyle-BackgroundColor="LightGreen"></Appearance>
                            </telerik:ColumnSeries>
                            <telerik:ColumnSeries Name="Hodiny NeFa" DataFieldY="HodinyNeFa">
                                <Appearance FillStyle-BackgroundColor="#ff9999"></Appearance>
                            </telerik:ColumnSeries>
                        </Series>
                        <XAxis DataLabelsField="Datum">
                            <LabelsAppearance RotationAngle="90" DataFormatString="dd.MM. ddd"></LabelsAppearance>
                            <MinorGridLines Visible="false" />
                            <MajorGridLines Visible="false" />
                        </XAxis>
                        <YAxis>
                            <MinorGridLines Visible="false" />
                            <MajorGridLines Visible="false" />
                        </YAxis>
                    </PlotArea>
                </telerik:RadHtmlChart>
            </asp:Panel>
            <asp:Panel ID="panChart3" runat="server" Style="float: right;" Visible="false">
                <telerik:RadHtmlChart runat="server" ID="chart3" width="400px" Height="700px" Font-Size="10px">
                    <ChartTitle Text="Vykázané hodiny po dnech (30 dní dozadu)">                        
                    </ChartTitle>
                    <PlotArea>
                        <Series>                             
                            <telerik:BarSeries Name="Hodiny Fa" DataFieldY="HodinyFa" Stacked="true">
                                <Appearance FillStyle-BackgroundColor="LightGreen"></Appearance>
                            </telerik:BarSeries>
                            <telerik:BarSeries Name="Hodiny NeFa" DataFieldY="HodinyNeFa">
                                <Appearance FillStyle-BackgroundColor="#ff9999"></Appearance>
                            </telerik:BarSeries>
                        </Series>
                        <XAxis DataLabelsField="Datum" Reversed="true">
                            <LabelsAppearance DataFormatString="dd.MM. ddd"></LabelsAppearance>
                            <MinorGridLines Visible="false" />
                            <MajorGridLines Visible="true" />
                        </XAxis>
                        <YAxis>
                            <MinorGridLines Visible="false" />
                            <MajorGridLines Visible="false" />
                        </YAxis>
                    </PlotArea>
                </telerik:RadHtmlChart>
            </asp:Panel>
            <asp:Panel ID="panChart2" runat="server" Style="float: right;" Visible="false">
                <telerik:RadHtmlChart runat="server" ID="chart2" Width="600px" >  
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

           <asp:Repeater ID="rpNoticeBoard" runat="server">
            <ItemTemplate>
                <div class="noticeboard-box" style="margin-top:20px;">
                    <div class="title">
                        <img src="Images/article.png" />
                        <span style="font-weight:bold;font-variant: small-caps;font-size:120%;"><%#Eval("o10Name")%></span>
                        
                                                
                        <span style="font-style:italic;float:right;"> <%#Eval("ValidFrom")%> | <%#Eval("Owner") %></span>
                    </div>
                    <div class="content" style="color:black;background-color: <%#Eval("o10BackColor")%>;">
                        <%#Eval("o10BodyHtml")%>
                    </div>
                </div>
            </ItemTemplate>
        </asp:Repeater>

            <div style="clear: both;"></div>

            <div style="margin-top: 20px;">
                <asp:Label ID="lblBuild" runat="server" Style="color: gray;" />

                <span style="padding-left:30px;">&nbsp</span>
                <asp:HyperLink ID="cmdReadUpgradeInfo" runat="server" NavigateUrl="log_app_update.aspx" ImageUrl="Images/upgraded_32.png" ToolTip="Nedávno proběhla aktualizace MARKTIME. Přečti si informace o novinkách a změnách v systému."></asp:HyperLink>
                
                <a href="log_app_update.aspx">Historie novinek a změn v systému</a>
                
                <asp:CheckBox ID="chkShowCharts" runat="server" AutoPostBack="true" Text="Zobrazovat na stránce grafy z mých hodin" Checked="true" style="float:right;" />
            </div>
        </div>

        

        <script type="text/javascript">
            $(document).ready(function () {
                //pokud je rozlišení displeje menší než 1280px, automaticky  nahodit režim SAW
                if (screen.availWidth < 1280) {
                    if (readCookie("MT50-SAW") == "1") {
                        return;
                    }
                    else {
                        createCookie('MT50-SAW', "1", 30);
                    }
                }




            });


            

    </script>
</asp:Content>

