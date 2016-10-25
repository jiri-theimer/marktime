﻿<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Clue.Master" CodeBehind="p31_approving_step3_subform.aspx.vb" Inherits="UI.p31_approving_step3_subform" %>

<%@ MasterType VirtualPath="~/Clue.Master" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<%@ Register TagPrefix="uc" TagName="p31_approve_onerec" Src="~/p31_approve_onerec.ascx" %>
<%@ Register TagPrefix="uc" TagName="b07_list" Src="~/b07_list.ascx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    

    <uc:p31_approve_onerec ID="approve1" runat="server" IsVertical="true" CommandSaveText="Potvrdit" HeaderText="Vybraný úkon" showCancelCommand="false" />
    <div>
    <button type="button" ID="cmdSplitRecord" runat="server" onclick="javascript:split_record()">Rozdělit úkon na 2 kusy</button>    
    </div>
    <table cellpadding="5" cellspacing="2">
        <tr>
            <td>
                <asp:Label runat="server" ID="lblPerson" Text="Osoba:" CssClass="lbl"></asp:Label>
            </td>
            <td>
                <asp:Label ID="Person" runat="server" CssClass="valbold" />
            </td>

        </tr>
        <tr>
            <td>
                <asp:Label runat="server" ID="lblClient" Text="Klient:" CssClass="lbl"></asp:Label></td>
            <td>
                <asp:Label ID="Client" runat="server" CssClass="valbold" />
            </td>
        </tr>
        <tr>
            <td>
                <asp:Label runat="server" ID="lblProject" Text="Projekt:" CssClass="lbl"></asp:Label>

            </td>
            <td>
                <asp:Label ID="p41Name" runat="server" CssClass="valbold" />
            </td>
        </tr>
        <tr>
            <td>
                <asp:Label runat="server" ID="lblP56" Text="Úkol:" CssClass="lbl"></asp:Label></td>
            <td>
                <asp:Label ID="Task" runat="server" CssClass="valbold" />
            </td>
        </tr>

        <tr>
            <td>
                <asp:Label runat="server" ID="lblP34" Text="Sešit:" CssClass="lbl"></asp:Label></td>
            <td>
                <asp:Label ID="p34name" runat="server" CssClass="valbold" />
            </td>
        </tr>
        <tr valign="top">
            <td>
                <asp:Label runat="server" ID="lblP32" Text="Aktivita:" CssClass="lbl"></asp:Label></td>
            <td>
                <asp:Label ID="p32name" runat="server" CssClass="valbold" />
                <asp:Label ID="billable" runat="server" />
            </td>
        </tr>
        <tr>
            <td>
                <asp:Label runat="server" ID="lblDate" Text="Datum:" CssClass="lbl"></asp:Label>
            </td>
            <td>
                <asp:Label ID="p31date" runat="server" CssClass="valbold" />

            </td>
        </tr>
        <tr>
            <td>
                <asp:Label runat="server" ID="lblValueOrig" Text="Vykázaná hodnota:" CssClass="lbl"></asp:Label>
            </td>
            <td>
                <asp:Label ID="p31value_orig" runat="server" CssClass="valbold" />
                <asp:Label ID="j27ident_orig" runat="server" />
            </td>
        </tr>
        <tr>
            <td>
                <asp:Label runat="server" ID="lblBillingRate_Orig" Text="Výchozí fakturační sazba:" CssClass="lbl"></asp:Label>
            </td>
            <td>
                <asp:Label ID="p31Rate_Billing_Orig" runat="server" CssClass="valbold" />
                <asp:Label ID="rate_j27ident" runat="server" />
            </td>
        </tr>

    </table>
    <uc:b07_list ID="comments1" runat="server" JS_Create="p31_comment_create()" JS_Reaction="p31_comment_reaction" />

    <asp:HiddenField ID="hidRefreshParent" Value="0" runat="server" />

    <script type="text/javascript">
        function p31_comment_create() {
            
            window.parent.sw_orig("b07_create.aspx?masterprefix=p31&masterpid=<%=Master.DataPID%>");

        }

        function p31_comment_reaction(b07id) {

            window.parent.sw_orig("b07_create.aspx?parentpid="+b07id+"&masterprefix=p31&masterpid=<%=Master.DataPID%>");


        }

        function split_record() {
            window.parent.sw_orig("p31_record_split.aspx?pid=<%=Master.DataPID%>&guid=<%=ViewState("guid")%>");
        }

        <%If hidRefreshParent.Value = "1" Then%>
        document.getElementById("<%=Me.hidRefreshParent.ClientID%>").value="0";
        window.parent.hardrefresh(<%=Master.DataPID%>, "refresh");
        <%End If%>
    </script>
</asp:Content>
