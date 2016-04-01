<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/ModalForm.Master" CodeBehind="report_modal.aspx.vb" Inherits="UI.report_modal" %>

<%@ MasterType VirtualPath="~/ModalForm.Master" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<%@ Register Assembly="Telerik.ReportViewer.WebForms, Version=8.1.14.804, Culture=neutral, PublicKeyToken=a9d7983dfcc261be" Namespace="Telerik.ReportViewer.WebForms" TagPrefix="telerik" %>
<%@ Register TagPrefix="uc" TagName="periodcombo" Src="~/periodcombo.ascx" %>


<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <script type="text/javascript">
        $(document).ready(function () {

            $(".slidingDiv1").hide();
            $(".show_hide1").show();

            $('.show_hide1').click(function () {

                $(".slidingDiv1").slideToggle();
            });

            var h1 = new Number;
            var h2 = new Number;
            var hh = new Number;

            h1 = $(window).height();

            ss = self.document.getElementById("offsetY");
            var offset = $(ss).offset();

            h2 = offset.top;

        <%If LCase(Request.Browser.Browser) = "ie" Then%>
            hh = h1 - h2 - 60-10;
        <%Else%>
            hh = h1 - h2 - 60-10;
        <%End If%>


            self.document.getElementById("divReportViewer").style.height = hh + "px";

            <%If Me.MultiPIDs<>"" then%>
            self.document.getElementById("divReportViewer").style.display = "none";
            <%end If%>

        })



        function hardrefresh(pid, flag) {

            document.getElementById("<%=hidHardRefreshFlag.clientid%>").value = flag;
            <%=Me.ClientScript.GetPostBackEventReference(Me.cmdRefreshOnBehind, "", False)%>;

        }



    </script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="OverMainContent" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="server">
    <table cellpadding="3" cellspacing="2">
        <tr>

            <td>
                <asp:Label ID="lblX31ID" runat="server" Text="Sestava:" CssClass="lbl"></asp:Label>
            </td>
            <td>
                <asp:DropDownList ID="x31ID" runat="server" AutoPostBack="true" DataValueField="pid" DataTextField="x31Name" Style="width: 350px;" BackColor="yellow"></asp:DropDownList>
            </td>
            <td>
                <uc:periodcombo ID="period1" runat="server" Width="250px" Visible="false"></uc:periodcombo>
            </td>

            <td>
                <asp:Button ID="cmdRefresh" runat="server" Text="Obnovit" CssClass="cmd" />
            </td>


            <td align="right"></td>
        </tr>
    </table>

    <div id="offsetY"></div>
    <div class="slidingDiv1" style="padding: 10px;">
        <div class="content-box2">
            <div class="title">
                <img src="Images/merge.png" />
                <img src="Images/pdf.png" />
                <span>Sloučení sestavy s až 3 dalšími výstupy do jediného PDF dokumentu</span>                
            </div>
            <div class="content">
                <table cellpadding="3" cellspacing="2">
                    <tr>
                        <td>
                            <asp:Label ID="Label1" runat="server" Text="Dále sloučit s:" CssClass="lbl"></asp:Label>
                        </td>
                        <td>
                            <asp:DropDownList ID="x31ID_Merge1" runat="server" AutoPostBack="false" DataValueField="pid" DataTextField="x31Name" Style="width: 400px;"></asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label ID="Label2" runat="server" Text="Dále sloučit s:" CssClass="lbl"></asp:Label>
                        </td>
                        <td>
                            <asp:DropDownList ID="x31ID_Merge2" runat="server" AutoPostBack="false" DataValueField="pid" DataTextField="x31Name" Style="width: 400px;"></asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label ID="Label3" runat="server" Text="Dále sloučit s:" CssClass="lbl"></asp:Label>
                        </td>
                        <td>
                            <asp:DropDownList ID="x31ID_Merge3" runat="server" AutoPostBack="false" DataValueField="pid" DataTextField="x31Name" Style="width: 400px;"></asp:DropDownList>
                        </td>
                    </tr>
                </table>                
                <asp:Button ID="cmdMergePDF_Download" runat="server" Text="Vygenerovat PDF dokument" CssClass="cmd"  />
                <asp:Button ID="cmdMergePDF_SendMail" runat="server" Text="Odeslat poštou" CssClass="cmd" style="margin-left:10px;" />
                <asp:Button ID="cmdMergePDF_Preview" runat="server" Text="Přejít na PDF náhled" CssClass="cmd" style="margin-left:10px;" />
                
            </div>
        </div>
    </div>
    <asp:Label ID="multiple_records" runat="server"></asp:Label>
    <div id="divReportViewer" style="height: 300px;">
        <telerik:ReportViewer ID="rv1" runat="server" Width="100%" Height="100%" ShowParametersButton="true" ShowHistoryButtons="false">
        </telerik:ReportViewer>        
    </div>
    
    <asp:HiddenField ID="hidX29ID" runat="server" Value="141" />
    <asp:HiddenField ID="hidPrefix" runat="server" Value="p41" />
    <asp:HiddenField ID="hidHardRefreshFlag" runat="server" />
    <asp:HiddenField ID="hidHardRefreshPID" runat="server" />
    <asp:HiddenField ID="hidPIDS" runat="server" />
    <asp:LinkButton ID="cmdRefreshOnBehind" runat="server" Text="refreshonbehind" Style="display: none;"></asp:LinkButton>


</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="FootContent" runat="server">
</asp:Content>
