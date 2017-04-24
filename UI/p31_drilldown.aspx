<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/ModalForm.Master" CodeBehind="p31_drilldown.aspx.vb" Inherits="UI.p31_drilldown" %>

<%@ MasterType VirtualPath="~/ModalForm.Master" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<%@ Register TagPrefix="uc" TagName="datagrid" Src="~/datagrid.ascx" %>
<%@ Register TagPrefix="uc" TagName="periodcombo" Src="~/periodcombo.ascx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="slidingDiv1">
        <div class="div6">
            <asp:CheckBox ID="chkSpecificSumCols" runat="server" Text="Nastavit si vlastní sledované veličiny" AutoPostBack="true" />
        </div>
        <asp:Panel ID="panSumCols" runat="server" CssClass="content-box2">
            <div class="title">
                Nastavení sledovaných veličin
                <asp:Button ID="cmdRefresh" runat="server" CssClass="cmd" Text="Obnovit statistiku" />
            </div>
            <div class="content">

                <table cellpadding="8">
                    <tr valign="top">
                        <td>
                            <div><%=Resources.grid_designer.DostupneSloupce %></div>
                            <telerik:RadListBox ID="colsSource" Height="200px" runat="server" AllowTransfer="true" TransferMode="Move" TransferToID="colsDest" SelectionMode="Single" Culture="cs-CZ" AllowTransferOnDoubleClick="true" Width="350px" AutoPostBackOnReorder="false" AutoPostBackOnDelete="false" AutoPostBackOnTransfer="false">
                                <ButtonSettings TransferButtons="All" ShowTransferAll="false" />

                                <Localization ToRight="Přesunout" ToLeft="Odebrat" AllToRight="Přesunout vše" AllToLeft="Odbrat vše" MoveDown="Posunout dolu" MoveUp="Posunout nahoru" />
                            </telerik:RadListBox>
                        </td>
                        <td>
                            <div><%=Resources.grid_designer.VybraneSloupce %></div>
                            <telerik:RadListBox ID="colsDest" runat="server" AllowReorder="true" AllowTransferOnDoubleClick="true" Culture="cs-CZ" Width="350px" SelectionMode="Single">

                                <EmptyMessageTemplate>
                                    <div style="padding-top: 50px;">
                                        <%=Resources.grid_designer.ZadneVybraneSloupce %>
                                    </div>
                                </EmptyMessageTemplate>
                            </telerik:RadListBox>

                        </td>

                    </tr>
                </table>

            </div>
        </asp:Panel>
    </div>

    <div class="commandcell">
        <img src="Images/drilldown.png" />
        <span>#1:</span>
        <telerik:RadComboBox ID="dd1" runat="server" AutoPostBack="true"></telerik:RadComboBox>
    </div>
    <div class="commandcell" style="padding-left:10px;">
        <img src="Images/drilldown.png" />
        <span>#2:</span>
        <telerik:RadComboBox ID="dd2" runat="server" AutoPostBack="true"></telerik:RadComboBox>
    </div>
    <div class="commandcell" style="padding-left:10px;">
        <uc:periodcombo ID="period1" runat="server" Width="170px" Visible="false"></uc:periodcombo>
        <asp:Label ID="lblQuery" runat="server" CssClass="valboldred"></asp:Label>
    </div>
    <div class="commandcell" style="padding-left:30px;">

        <img src="Images/export.png" alt="export" style="display:none;" />
        <asp:LinkButton ID="cmdExport" runat="server" Text="Export" Visible="false" />

        <img src="Images/xls.png" alt="xls" />
        <asp:LinkButton ID="cmdXLS" runat="server" Text="XLS" ToolTip="Export do XLS vč. souhrnů s omezovačem na maximálně 2000 záznamů" />

        <img src="Images/pdf.png" alt="pdf" />
        <asp:LinkButton ID="cmdPDF" runat="server" Text="PDF" ToolTip="Export do PDF vč. souhrnů s omezovačem na maximálně 2000 záznamů" />

        <img src="Images/doc.png" alt="doc" />
        <asp:LinkButton ID="cmdDOC" runat="server" Text="DOC" ToolTip="Export do DOC vč. souhrnů s omezovačem na maximálně 2000 záznamů" />

    </div>
    <div class="commandcell" style="float:right;">
        <i>Dvojklik -> Odfiltrování zdrojového datového přehledu</i>
    </div>
    <div style="clear: both;"></div>





    <uc:datagrid ID="grid1" runat="server" ClientDataKeyNames="pid" OnRowSelected="RowSelected" OnRowDblClick="RowDoubleClick"></uc:datagrid>


    <asp:HiddenField ID="hiddatapid" runat="server" />
    <asp:HiddenField ID="hidJ74ID" runat="server" />
    <asp:HiddenField ID="hidJ70ID" runat="server" />
    <asp:HiddenField ID="hidMasterPrefix" runat="server" />
    <asp:HiddenField ID="hidMasterPID" runat="server" />
    <asp:HiddenField ID="hidFrom" runat="server" />
  
    <asp:HiddenField ID="hidMasterAW" runat="server" />
    <asp:HiddenField ID="hidGridColumnSql" runat="server" />

    <script type="text/javascript">
        $(document).ready(function () {
            $(".slidingDiv1").hide();
            $(".show_hide1").show();

            $('.show_hide1').click(function () {
                $(".slidingDiv1").slideToggle();
            });





        });


        function RowSelected(sender, args) {
            document.getElementById("<%=hiddatapid.clientid%>").value = args.getDataKeyValue("pid");

        }

        function RowDoubleClick(sender, args) {
            var pid = document.getElementById("<%=hiddatapid.clientid%>").value;
            if (pid == "" || pid == null) {
                alert("Není vybrán záznam.");
                return
            }
            go2grid(pid);
        }

        function go2grid(pid) {
            alert(pid);
        }
    </script>
</asp:Content>
