<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master" CodeBehind="p31_drilldown.aspx.vb" Inherits="UI.p31_drilldown" %>

<%@ MasterType VirtualPath="~/Site.Master" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<%@ Register TagPrefix="uc" TagName="periodcombo" Src="~/periodcombo.ascx" %>



<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
        <link rel="stylesheet" type="text/css" href="Scripts/datatable/css/jquery.dataTables.min.css" />
    <style type="text/css">
        .numericCol{
  float:right;
}
    </style>
    <script type="text/javascript" src="Scripts/datatable/jquery.dataTables.min.js"></script>

    <script type="text/javascript">
        

        $(document).ready(function () {

            
            oTable1 = $('#tabData').dataTable({
                bPaginate: true,
                bFilter: true,
                pageLength: 50,
                bStateSave: true,
                bSort: true,
              


                oLanguage: {
                    sUrl: 'Scripts/datatable/cs_CZ.txt'

                }


            });
     

            
            
        });


        function periodcombo_setting() {

            sw_master("periodcombo_setting.aspx", "Images/settings_32.png");
        }



        function hardrefresh(pid, flag) {
            if (flag == "j70-run") {
                location.replace("p31_pivot.aspx");
                return;
            }

            document.getElementById("<%=Me.hidHardRefreshPID.ClientID%>").value = pid;
            document.getElementById("<%=Me.hidHardRefreshFlag.ClientID%>").value = flag;

            <%=Me.ClientScript.GetPostBackEventReference(Me.cmdRefresh, "", False)%>;
        }

        function querybuilder() {
            var j70id = "<%=Me.CurrentJ70ID%>";
            sw_master("query_builder.aspx?prefix=p31&pid=" + j70id, "Images/query_32.png");
            return (false);
        }
        function drilldownbuilder() {
            var j75id = "<%=Me.CurrentJ75ID%>";
            sw_master("drilldown_designer.aspx?masterprefix=<%=me.currentmasterprefix%>&pid=" + j75id, "Images/drilldown_32.png");
            return (false);
        }


        

    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div style="background-color: white;">
        <div style="float: left; background-color: white;">
            <img src="Images/drilldown_32.png" />
            <asp:Label ID="Label1" runat="server" CssClass="page_header_span" Text="Worksheet DRILL-DOWN"></asp:Label>
        </div>
        <div class="commandcell" style="padding-left: 20px;">
            <uc:periodcombo ID="period1" runat="server" Width="250px"></uc:periodcombo>
        </div>
        <div class="commandcell" style="padding-left: 20px;">
            <asp:HyperLink ID="clue_query" runat="server" CssClass="reczoom" ToolTip="Detail filtru" Text="i"></asp:HyperLink>
            <asp:DropDownList ID="j70ID" runat="server" AutoPostBack="true" DataTextField="NameWithMark" DataValueField="pid" Style="width: 200px;" ToolTip="Pojmenovaný filtr"></asp:DropDownList>
            <asp:ImageButton ID="cmdQuery" runat="server" OnClientClick="return querybuilder()" ImageUrl="Images/query.png" ToolTip="Návrhář filtrů" CssClass="button-link" />
        </div>
        <div class="commandcell" style="padding-left: 20px;">
            <asp:DropDownList ID="cbxPaging" runat="server" AutoPostBack="true" ToolTip="Stránkování">
                <asp:ListItem Text="5"></asp:ListItem>
                <asp:ListItem Text="10"></asp:ListItem>
                <asp:ListItem Text="20" Selected="true"></asp:ListItem>
                <asp:ListItem Text="50"></asp:ListItem>
                <asp:ListItem Text="100"></asp:ListItem>
            </asp:DropDownList>
        </div>
        <div class="commandcell">
            <img src="Images/refresh.png" />
            <asp:LinkButton ID="cmdRebind" runat="server" Text="Obnovit." />
        </div>


        <div style="clear: both;"></div>
    </div>

    <asp:Panel ID="panQueryByEntity" runat="server" CssClass="div6" Visible="false">
        <asp:Image ID="imgEntity" runat="server" />
        <asp:HyperLink ID="MasterEntity" runat="server" Style="margin-left: 10px;"></asp:HyperLink>

    </asp:Panel>
    <div class="div6">
        <asp:Label ID="lblJ75ID" runat="server" Text="DRILL-DOWN šablona:"></asp:Label>
        <asp:DropDownList ID="j75ID" runat="server" AutoPostBack="true" DataTextField="j75Name" DataValueField="pid" Style="width: 200px;" ToolTip="DRILL-DOWN šablona"></asp:DropDownList>
        <asp:ImageButton ID="cmdJ75" runat="server" OnClientClick="return drilldownbuilder()" ImageUrl="Images/drilldown.png" ToolTip="Návrhář DRILL-DOWN šablon" CssClass="button-link" />
    </div>


    <asp:Button ID="cmdPokus" runat="server" Text="Run" />

    <table id="tabData" class="display" cellspacing="0" width="100%">
        <thead>
            <tr>
                <th>pid</th>
                <asp:Repeater ID="rpH" runat="server">
                    <ItemTemplate>
                        <th>
                            <asp:Label ID="lbl1" runat="server"></asp:Label>
                        </th>
                    </ItemTemplate>
                </asp:Repeater>
            </tr>
        </thead>
        <tfoot>
            <tr>
                <td>pid</td>
                <asp:Repeater ID="rpF" runat="server">
                    <ItemTemplate>
                        <td align="right">
                            <asp:Label ID="lbl1" runat="server"></asp:Label>
                        </td>
                    </ItemTemplate>
                </asp:Repeater>
            </tr>

        </tfoot>
        <tbody>
            <asp:Repeater ID="rpData" runat="server">
                <ItemTemplate>
                    <tr>
                        <td>
                            <asp:HiddenField ID="pid" runat="server" />
                        </td>
                        <td>
                            <asp:Label ID="lbl0" runat="server"></asp:Label>
                        </td>
                        <asp:Repeater ID="rp1" runat="server">
                            <ItemTemplate>
                                <td align="right">
                                    <asp:Label ID="lbl1" runat="server"></asp:Label>
                                </td>
                            </ItemTemplate>
                        </asp:Repeater>
                    </tr>
                </ItemTemplate>
            </asp:Repeater>
        </tbody>
    </table>

    <asp:Button ID="cmdPostback" runat="server" Text="Postback" />

    <asp:HiddenField ID="hidCols1" runat="server" />
    <asp:HiddenField ID="hidCols2" runat="server" />
    <asp:HiddenField ID="hidCols3" runat="server" />
    <asp:HiddenField ID="hidCols4" runat="server" />
    <asp:HiddenField ID="hidGroup1" runat="server" />
    <asp:HiddenField ID="hidGroup2" runat="server" />
    <asp:HiddenField ID="hidGroup3" runat="server" />
    <asp:HiddenField ID="hidGroup4" runat="server" />
    <asp:HiddenField ID="hidHardRefreshFlag" runat="server" />
    <asp:HiddenField ID="hidHardRefreshPID" runat="server" />
    <asp:HiddenField ID="hidMasterPrefix" runat="server" Value="p31_drilldown" />
    <asp:HiddenField ID="hidMasterPID" runat="server" />
    <asp:HiddenField ID="hiddatapid" runat="server" />
    <asp:HiddenField ID="hidSelPID1" runat="server" />
    <asp:HiddenField ID="hidSelPID2" runat="server" />
    <asp:HiddenField ID="hidSelPID3" runat="server" />


    <asp:Button ID="cmdRefresh" runat="server" Style="display: none;" />




    

</asp:Content>
