<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="billingmemo.ascx.vb" Inherits="UI.billingmemo" %>
<%@ Register TagPrefix="uc" TagName="o23_list" Src="~/o23_list.ascx" %>

<asp:Panel ID="panO23" runat="server" Visible="false">
    <button type="button"  id="cmdO23" class="button-link">
        <img src="Images/notepad.png" />
        <asp:Label ID="lblO23" runat="server" Text="Fakturační poznámky z dokumentů"></asp:Label>
        <img src="Images/arrow_down.gif" alt="Nastavení" />
    </button>
</asp:Panel>
<div id="billingmemo_slidingDiv1">
    <uc:o23_list id="notepad1" runat="server"></uc:o23_list>
    
</div>
<asp:Image ID="img1" ImageUrl="Images/billing.png" runat="server" Visible="false" />
<asp:Label ID="BillingMemo" runat="server" CssClass="infoNotification" Font-Italic="true"></asp:Label>


<script type="text/javascript">
    <%if panO23.visible then%>
    $(document).ready(function () {
        $("#billingmemo_slidingDiv1").hide();
        $("#cmdO23").show();

        $('#cmdO23').click(function () {
            $("#billingmemo_slidingDiv1").slideToggle();
        });

       
        
    });
    <%end if%>
</script>