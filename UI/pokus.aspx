<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master" CodeBehind="pokus.aspx.vb" Inherits="UI.pokus" %>

<%@ MasterType VirtualPath="~/Site.Master" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<%@ Register TagPrefix="uc" TagName="datagrid" Src="~/datagrid.ascx" %>


<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">


    <script type="text/javascript">
        function gogo(par) {
            window.history.pushState({}, "Ahoj-titulek", "/pokus.aspx?gogo="+par);
            
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    Hodiny: <asp:TextBox ID="txtHours" runat="server" Text="2,58333"></asp:TextBox>
    
   
    
    <asp:Button ID="cmdPokus" runat="server" Text="test" />

     
   
    <asp:TextBox ID="txtResult" runat="server"></asp:TextBox>

    <asp:HiddenField ID="hiddatapid" runat="server" />


    <button type="button" onclick="gogo('1')">Změnit URL 1</button>
    <button type="button" onclick="gogo('2')">Změnit URL 2</button>

    <p></p><p></p>

    <telerik:RadScheduler ID="scheduler1" SelectedView="AgendaView" RenderMode="Lightweight" FirstDayOfWeek="Monday" LastDayOfWeek="Sunday" Width="600px" Height="300px" EnableViewState="false" Skin="Default" AppointmentStyleMode="Simple" ShowFooter="false" runat="server" ShowViewTabs="false" EnableAdvancedForm="false" ShowHeader="true"
                Culture="cs-CZ" AllowEdit="false" AllowDelete="false" AllowInsert="false"             
                HoursPanelTimeFormat="HH:mm" ShowNavigationPane="true"
                DataSubjectField="o22Name" DataStartField="o22DateFrom" DataEndField="o22DateUntil" DataKeyField="pid">
                <Localization HeaderAgendaDate="Datum" AllDay="Bez času od/do" HeaderMonth="Měsíc" HeaderDay="Den" HeaderMultiDay="Multi-den" HeaderWeek="Týden" ShowMore="více..." HeaderToday="Dnes" HeaderAgendaAppointment="Úkol nebo Událost" HeaderAgendaTime="Čas" />
                
                <AgendaView UserSelectable="false" NumberOfDays="200" ShowDateHeaders="true" ReadOnly="true" ShowColumnHeaders="false" />
                <DayView UserSelectable="true" DayStartTime="06:00" DayEndTime="22:00" ShowInsertArea="false" />

                <AppointmentTemplate>
                    <a class="reczoom" rel="<%# Eval("Description")%>">i</a>
                    <a href="javascript:re(<%# Eval("ID")%>)"><%# Eval("Subject")%></a>


                </AppointmentTemplate>
        
                
                
            </telerik:RadScheduler>
            

    



    
</asp:Content>



