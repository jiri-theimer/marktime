<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Clue.Master" CodeBehind="clue_o23_record.aspx.vb" Inherits="UI.clue_o23_record" %>
<%@ MasterType VirtualPath="~/Clue.Master" %>
<%@ Register TagPrefix="uc" TagName="b07_list" Src="~/b07_list.ascx" %>
<%@ Register TagPrefix="uc" TagName="entityrole_assign_inline" Src="~/entityrole_assign_inline.ascx" %>
<%@ Register TagPrefix="uc" TagName="o23_record_readonly" Src="~/o23_record_readonly.ascx" %>


<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <script type="text/javascript">
        function detail() {

            window.parent.sw_everywhere("o23_record.aspx?pid=<%=Master.DataPID%>", "Images/label.png", true);

        }
        function go2module() {

            window.open("o23_framework.aspx?pid=<%=Master.DataPID%>", "_top");

        }
        function go2workflow() {

            window.parent.sw_everywhere("workflow_dialog.aspx?prefix=o23&pid=<%=Master.DataPID%>", "Images/label.png", true);

        }

    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div style="max-height: 270px; overflow-x: auto; overflow-y: auto; width: 100%;">
        <div>
            <img src="Images/label_32.png" />
            <asp:Label ID="ph1" runat="server" CssClass="clue_header_span"></asp:Label>
            <a  href="javascript:go2module()">Skočit na stránku dokumentu</a>
        </div>
        <div class="div6">
            <uc:o23_record_readonly ID="rec1" runat="server" />
        </div>
        <div class="div6">
            <uc:entityrole_assign_inline ID="roles1" runat="server" EntityX29ID="o23Doc" NoDataText=""></uc:entityrole_assign_inline>
        </div>

        <div style="clear: both;"></div>
        <uc:b07_list ID="comments1" runat="server" ShowHeader="true" ShowInsertButton="false" />
    </div>
</asp:Content>
