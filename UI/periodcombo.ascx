<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="periodcombo.ascx.vb" Inherits="UI.periodcombo" %>
<asp:DropDownList ID="period1" runat="server" AutoPostBack="true" ToolTip="Filtrování období" DataValueField="ComboValue" DataTextField="NameWithDates" style="width:160px;"></asp:DropDownList>
<asp:HyperLink ID="clue_period" runat="server" CssClass="reczoom" ImageUrl="Images/datepicker.png" rel="clue_periodcombo.aspx" title="Filtrování období" style="vertical-align:middle;text-align:left;"></asp:HyperLink>

<asp:HiddenField ID="hidCustomQueries" runat="server" />
<asp:HiddenField ID="hidExplicitValue" runat="server" />
<asp:HiddenField ID="hidLogin" runat="server" />
<asp:Button ID="cmdPeriodComboRefresh" runat="server" Style="display: none;" />
<script type="text/javascript">
    function hardrefresh_periodcombo(explicitVal) {
        
        document.getElementById("<%=Me.hidExplicitValue.ClientID%>").value = explicitVal;
        var clickButton = document.getElementById("<%= cmdPeriodComboRefresh.ClientID%>");
        clickButton.click();
        
        
    }
</script>