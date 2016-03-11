<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master" CodeBehind="j03_mypage_greeting.aspx.vb" Inherits="UI.j03_mypage_greeting" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<%@ Register TagPrefix="uc" TagName="contact" Src="~/contact.ascx" %>
<%@ Register TagPrefix="uc" TagName="project" Src="~/project.ascx" %>

<%@ MasterType VirtualPath="~/Site.Master" %>

<asp:Content ID="HeaderContent" runat="server" ContentPlaceHolderID="HeadContent">
    <script type="text/javascript">
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
            if (flag == "p56-save" || flag=="p56-create") {
                location.replace("p31_framework.aspx");
                return;
            }
            if (flag == "p91-create" || flag == "p91-save") {
                location.replace("p91_framework.aspx?pid=" + pid);
                return;
            }
            if (flag == "p31-save") {
                location.replace("p31_framework.aspx?pid=" + pid);
                return;
            }
            if (flag == "p28-save" || flag=="p28-create") {
                location.replace("p28_framework.aspx?pid=" + pid);
                return;
            }
            if (flag == "o23-save" || flag=="o23-create") {
                location.replace("o23_framework.aspx?pid=" + pid);
                return;
            }

            location.replace("default.aspx");

        }


    </script>
</asp:Content>
<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="MainContent">
    <div style="padding: 10px; background-color: white;">
        <table cellpadding="5">
            <tr>

                <td>
                    <asp:Label ID="lblHeader" runat="server" CssClass="framework_header_span" Style="font-size: 200%;" Text="Vítejte v systému"></asp:Label>

                </td>
                <td>
                    <img src="Images/logo_transparent.png" />
                </td>

            </tr>
        </table>



        <div style="min-height:430px;">
            <table style="width: 100%;">
                <tr valign="top">
                    <td>
                        <telerik:RadPanelBar ID="menu1" runat="server" RenderMode="Lightweight" Skin="Metro" Width="300px">
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
                    </td>
                    <td style="text-align: left;">
                        <asp:Image runat="server" ID="imgWelcome" ImageUrl="Images/welcome/start.jpg" />
                    </td>


                </tr>
            </table>


        </div>



        <div style="margin-top: 20px;">
            <asp:label ID="lblBuild" runat="server" style="color:gray;"/>

            
        </div>
    </div>


</asp:Content>

