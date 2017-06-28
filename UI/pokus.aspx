<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master" CodeBehind="pokus.aspx.vb" Inherits="UI.pokus" %>

<%@ MasterType VirtualPath="~/Site.Master" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<%@ Register TagPrefix="uc" TagName="datagrid" Src="~/datagrid.ascx" %>
<%@ Register TagPrefix="uc" TagName="periodcombo" Src="~/periodcombo.ascx" %>
<%@ Register TagPrefix="uc" TagName="mygrid" Src="~/mygrid.ascx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <style type="text/css">   
        html .RadNavigation_Windows7 .rnvRootGroupWrapper {
    background-color: #25a0da;
    background-image: none;
    color:white;
}
            
        html .RadNavigation .rnvMore,
html .RadNavigation .rnvRootLink {
    padding-left:15px;
   padding-top:5px;
   padding-bottom:5px;
   
   
   
   }

      
    </style>

    <script type="text/javascript">
        function gogo(par) {
            window.history.pushState({}, "Ahoj-titulek", "/pokus.aspx?gogo="+par);
            
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <img src="Images/logo_transparent.png" style="position:absolute;left:10px;top:40px;z-index:8000;" />
   
    <telerik:RadNavigation ID="nav1" runat="server" MenuButtonPosition="Right" Skin="Bootstrap" EnableViewState="false">
      
        <CollapseAnimation Type="None" />
        <ExpandAnimation Type="None" />
        <Nodes>
            <telerik:NavigationNode Text=" " Width="200px" Enabled="false">
                
            </telerik:NavigationNode>
            <telerik:NavigationNode ImageUrl="Images/new_menu.png">
                <Nodes>
                    <telerik:NavigationNode Text="Worksheet úkon"></telerik:NavigationNode>
                    <telerik:NavigationNode Text="Projekt"></telerik:NavigationNode>
                </Nodes>
               
            </telerik:NavigationNode>
           <telerik:NavigationNode Text="ÚVOD"></telerik:NavigationNode>
            <telerik:NavigationNode Text="WORKSHEET">
                <Nodes>
                    <telerik:NavigationNode Text="Zapisovat"></telerik:NavigationNode>
                    <telerik:NavigationNode Text="Datový přehled"></telerik:NavigationNode>
                    <telerik:NavigationNode Text="Schvalovat, připravit podklady k fakturaci"></telerik:NavigationNode>
                </Nodes>
            </telerik:NavigationNode>
            <telerik:NavigationNode Text="PROJEKTY"></telerik:NavigationNode>
            <telerik:NavigationNode text="KLIENTI"></telerik:NavigationNode>
            <telerik:NavigationNode Text="FAKTURY"></telerik:NavigationNode>
            <telerik:NavigationNode Text="ÚKOLY"></telerik:NavigationNode>
            <telerik:NavigationNode Text="DALŠÍ"></telerik:NavigationNode>
            <telerik:NavigationNode ID="Posledni">  
                <NodeTemplate>
                    
                </NodeTemplate>             
            </telerik:NavigationNode>
        </Nodes>
    </telerik:RadNavigation>
   
    <div style="clear:both;"></div>
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



