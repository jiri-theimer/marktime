<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/ModalForm.Master" CodeBehind="p31_sumgrid_pivot.aspx.vb" Inherits="UI.p31_sumgrid_pivot" %>
<%@ MasterType VirtualPath="~/ModalForm.Master" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>


<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="OverMainContent" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="server">

    <telerik:RadPivotGrid runat="server" ID="pivot1" AllowPaging="true" ShowColumnHeaderZone="true" AllowSorting="true" AllowFiltering="false" ShowRowHeaderZone="true" ShowFilterHeaderZone="true" ShowDataHeaderZone="true" AllowNaturalSort="false" Skin="Metro"
         FilterHeaderZoneText="Sem přetáhněte nevyužívané sloupce/veličiny" ColumnHeaderZoneText="Sem přetáhněte pivot pole" RowHeaderZoneText="Sem přetáhněte řádková pole" DataHeaderZoneText="Sem přetáhněte součtové veličiny" NoRecordsText="Žádná data k zobrazení">
        <DataCellStyle BackColor="white" />
        <PagerStyle Mode="NextPrevAndNumeric" AlwaysVisible="false"></PagerStyle>
        
        
        <Fields>
            <telerik:PivotGridReportFilterField DataField="Person" Caption="Osoba"></telerik:PivotGridReportFilterField>
            
            <telerik:PivotGridReportFilterField DataField="colPoziceOsoby" Caption="Pozice" ></telerik:PivotGridReportFilterField>
            <telerik:PivotGridReportFilterField DataField="colMesic" Caption="Měsíc" ></telerik:PivotGridReportFilterField>
            
            
            <telerik:PivotGridAggregateField DataField="sum1" Caption="Vykázané hodiny" Aggregate="Sum" DataFormatString="{0:F2}">
            </telerik:PivotGridAggregateField>
            <telerik:PivotGridAggregateField DataField="sum3" Caption="Vyfakturované hodiny" Aggregate="Sum" DataFormatString="{0:F2}">
            </telerik:PivotGridAggregateField>

        </Fields>
        
        <ConfigurationPanelSettings EnableDragDrop="false" />
        <ClientSettings EnableFieldsDragDrop="true" ClientMessages-DragToReorder="Drag to reorder"></ClientSettings>
    </telerik:RadPivotGrid>

</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="FootContent" runat="server">
</asp:Content>
