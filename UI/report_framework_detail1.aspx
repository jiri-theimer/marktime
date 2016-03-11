<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/SubForm.Master" CodeBehind="report_framework_detail1.aspx.vb" Inherits="UI.report_framework_detail1" %>

<%@ MasterType VirtualPath="~/SubForm.Master" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<%@ Register TagPrefix="uc" TagName="periodcombo" Src="~/periodcombo.ascx" %>
<%@ Register Assembly="Telerik.ReportViewer.WebForms, Version=8.1.14.804, Culture=neutral, PublicKeyToken=a9d7983dfcc261be" Namespace="Telerik.ReportViewer.WebForms" TagPrefix="telerik" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <script type="text/javascript">
        $(document).ready(function () {
            var h1 = new Number;
            var h2 = new Number;
            var hh = new Number;

            h1 = $(window).height();

            ss = self.document.getElementById("offsetY");
            var offset = $(ss).offset();

            h2 = offset.top;

            <%If LCase(Request.Browser.Browser) = "ie" Then%>
            hh = h1 - h2 - 4;
            <%Else%>
            hh = h1 - h2 - 2;
            <%End If%>

            self.document.getElementById("divReportViewer").style.height = hh + "px";
        })


        function periodcombo_setting() {
            sw_local("periodcombo_setting.aspx", "Images/settings_32.png")
        }

        function hardrefresh(pid, flag) {


            document.getElementById("<%=Me.hidHardRefreshPID.ClientID%>").value = pid;
            document.getElementById("<%=Me.hidHardRefreshFlag.ClientID%>").value = flag;

            <%=Me.ClientScript.GetPostBackEventReference(Me.cmdRefresh, "", False)%>;

        }

        function sendbymail(){
            sw_local("sendmail.aspx?x31id=<%=me.CurrentX31ID%>&datfrom=<%=Format(period1.DateFrom,"dd.MM.yyyy")%>&datuntil=<%=Format(period1.DateUntil,"dd.MM.yyyy")%>", "Images/email_32.png")
        }

        function x31_record() {
            sw_local("x31_record.aspx?pid=<%=me.CurrentX31ID%>", "Images/settings_32.png")
        }

        
    </script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <table cellpadding="10">
        <tr>
            <td>
                <asp:Image ID="img1" runat="server" ImageUrl="Images/report.png" />
            </td>
            <td>
                <uc:periodcombo ID="period1" runat="server" Width="250px"></uc:periodcombo>
            </td>
            <td>
                <a href="javascript:sendbymail()">Odeslat sestavu poštou jako PDF</a>
            </td>
            <td>
                <asp:Label ID="lblHeader" runat="server" CssClass="framework_header_span"></asp:Label>
            </td>
            <td>
                <asp:HyperLink ID="cmdSetting" runat="server" Text="Nastavení šablony" NavigateUrl="javascript:x31_record()"></asp:HyperLink>
            </td>
        </tr>
    </table>
    <div id="offsetY"></div>

    <div id="divReportViewer">
        <telerik:ReportViewer ID="rv1" runat="server" Width="100%" Height="100%" ShowParametersButton="true" ShowHistoryButtons="false" ValidateRequestMode="Disabled">
        </telerik:ReportViewer>

    </div>
    <asp:HiddenField ID="flag" runat="server" />
    <asp:HiddenField ID="hidCurX31ID" runat="server" />
    <asp:HiddenField ID="hidHardRefreshFlag" runat="server" />
    <asp:HiddenField ID="hidHardRefreshPID" runat="server" />
    <asp:Button ID="cmdRefresh" runat="server" Style="display: none;" />
</asp:Content>
