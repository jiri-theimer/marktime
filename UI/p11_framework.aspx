<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master" CodeBehind="p11_framework.aspx.vb" Inherits="UI.p11_framework" %>

<%@ MasterType VirtualPath="~/Site.Master" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<%@ Register TagPrefix="uc" TagName="p31_subgrid" Src="~/p31_subgrid.ascx" %>



<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <script type="text/javascript">
        $(document).ready(function () {
            $(".slidingDiv2").hide();
            $(".show_hide2").show();
                      
            $(".slidingDiv1").hide();
            $(".show_hide1").show();

            $('.show_hide1').click(function () {
                $(".slidingDiv2").hide();               
                $(".slidingDiv1").slideToggle();
            });

            $('.show_hide2').click(function () {
                $(".slidingDiv1").hide();                
                $(".slidingDiv2").slideToggle();
            });



        });

        function p31_entry() {
            
            sw_everywhere("p31_record.aspx?pid=0&p31date=<%=Format(Me.datToday.SelectedDate, "dd.MM.yyyy")%>&j02id=<%=Master.Factory.SysUser.j02ID%>", "Images/worksheet.png");

        }

        function p31_clone() {
            ///volá se z p31_subgrid
            var pid = document.getElementById("<%=hiddatapid_p31.clientid%>").value;
            sw_everywhere("p31_record.aspx?clone=1&pid=" + pid, "Images/worksheet.png");

        }




        function hardrefresh(pid, flag) {
            


        }





        function p31_RowSelected(sender, args) {

            document.getElementById("<%=hiddatapid_p31.clientid%>").value = args.getDataKeyValue("pid");

        }

        function p31_RowDoubleClick(sender, args) {
            record_p31_edit();
        }

        function record_p31_edit() {
            var pid = document.getElementById("<%=hiddatapid_p31.clientid%>").value;
            sw_everywhere("p31_record.aspx?pid=" + pid, "Images/worksheet.png");

        }

        function p31_subgrid_setting(j74id) {
            sw_everywhere("grid_designer.aspx?prefix=p31&masterprefix=j02&pid=" + j74id, "Images/griddesigner.png", true);

        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <div style="background-color: white; padding: 10px;">
        <table cellpadding="10">
            <tr>
                <td>
                    <asp:Image ID="img1" runat="server" ImageUrl="Images/worksheet_32.png" />
                </td>
                <td>
                    <asp:Label ID="lblHeader" runat="server" CssClass="page_header_span" Text="Rozhraní docházky"></asp:Label>

                    <telerik:RadDatePicker ID="datToday" runat="server" Width="120px" DateInput-EmptyMessage="Povinný údaj." DateInput-EmptyMessageStyle-ForeColor="red" AutoPostBack="true" SharedCalendarID="SharedCalendar">
                    <DateInput ID="DateInput1" DisplayDateFormat="d.M.yyyy ddd" runat="server"></DateInput>
                </telerik:RadDatePicker>
                </td>
                <td>
                    <div class="show_hide1">
                    <asp:HyperLink ID="linkStart" runat="server" CssClass="button-link" Text="Příchod" Font-Size="Large" BackColor="green" ForeColor="white"></asp:HyperLink>
                    </div>
                    <div class="slidingDiv1" style="display:none;">
                        
                        <telerik:RadDateTimePicker ID="timeStart" runat="server" Width="190px" SharedCalendarID="SharedCalendar" >
                    
                    <TimePopupButton Visible="true" />
                            <DatePopupButton Visible="false" />
                    <TimeView StartTime="06:00" EndTime="22:00" ShowHeader="false" ShowFooter="false"></TimeView>
                    
                </telerik:RadDateTimePicker>

                    </div>
                </td>
                <td>
                    <div class="show_hide2">
                    <asp:HyperLink ID="linkEnd" runat="server" CssClass="button-link" Text="Odchod" Font-Size="Large" BackColor="red" ForeColor="white"></asp:HyperLink>
                    </div>
                    <div class="slidingDiv2" style="display:none;">
                        Odchod
                    </div>
                </td>
            </tr>
        </table>



       


        <uc:p31_subgrid ID="gridP31" runat="server" EntityX29ID="j02Person" OnRowSelected="p31_RowSelected" OnRowDblClick="p31_RowDoubleClick"></uc:p31_subgrid>

        <asp:HiddenField ID="hiddatapid_p31" runat="server" />


        <telerik:RadCalendar ID="SharedCalendar" runat="server" EnableMultiSelect="False" UseColumnHeadersAsSelectors="False" UseRowHeadersAsSelectors="False">
        <SpecialDays>
            <telerik:RadCalendarDay Repeatable="Today" ItemStyle-BackColor="SkyBlue"></telerik:RadCalendarDay>
        </SpecialDays>
    </telerik:RadCalendar>
    </div>

</asp:Content>
