<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Mobile.Master" CodeBehind="mobile_p31_calendar.aspx.vb" Inherits="UI.mobile_p31_calendar" %>

<%@ MasterType VirtualPath="~/Mobile.Master" %>
<%@ Register TagPrefix="uc" TagName="timesheet_calendar" Src="~/timesheet_calendar.ascx" %>
<%@ Register TagPrefix="uc" TagName="mobile_p31_list" Src="~/mobile_p31_list.ascx" %>


<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript">
        function hardrefresh(flag, value) {
            if (flag == "edit") {
                location.replace("mobile_p31_framework.aspx?source=calendar&pid=" + value);
            }
            if (flag == "new") {
                var d = document.getElementById("<%=me.hidCurDate.ClientID%>").value;
                location.replace("mobile_p31_framework.aspx?source=calendar&defdate=" + d);

            }


        }

        function report() {
            location.replace("mobile_report.aspx?prefix=j02&pid=<%=Master.Factory.SysUser.j02ID%>");
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">


    <uc:timesheet_calendar ID="cal1" runat="server" />
    <div style="margin-top: 5px; margin-left: 5px;">        
        <div class="btn-group">
            <button type="button" class="btn btn-primary" data-toggle="dropdown">
                Zapsat <asp:Label ID="lblCurDate" runat="server" CssClass="badge"></asp:Label>
                <span class="caret"></span>
            </button>
            <ul class="dropdown-menu" role="menu">
                <li><a href="javascript:hardrefresh('new','1')">Nový worksheet úkon</a></li>
                <asp:Repeater ID="rp1" runat="server">
                    <ItemTemplate>
                         <li><a href="mobile_p31_framework.aspx?p41id=<%#Eval("pid")%>"><%#Eval("FullName")%></a></li>
                    </ItemTemplate>
                </asp:Repeater>
               
            </ul>
        </div>
        <button type="button" class="btn btn-primary" onclick="report()">
            Sestava                    
        </button>
        <img src="Images/sum.png" />
        <asp:Label ID="Hours_All" runat="server"></asp:Label>
    </div>



    </div>

    <a id="record_list"></a>
    <uc:mobile_p31_list ID="list1" runat="server"></uc:mobile_p31_list>
    <asp:HiddenField ID="hidCurDate" runat="server" />

</asp:Content>
