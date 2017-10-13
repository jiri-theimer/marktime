<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master" CodeBehind="pokus.aspx.vb" Inherits="UI.pokus" %>

<%@ MasterType VirtualPath="~/Site.Master" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>


<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
  

    <script type="text/javascript">
        $(document).ready(function () {
         


            $("a.recpop").each(function () {

                // Extract your variables here:
                var $this = $(this);
                var myurl = $this.attr('rel');

                $this.qtip({
                    content: {
                        text: '<iframe src="' + myurl + '"' + ' width="100%" height="200"  frameborder="0"><p>Your browser does not support iframes.</p></iframe>',

                    },
                    position: {
                        at: 'right center',
                        my: 'left top',
                        viewport: $(window)

                    },
                    show: {
                        event: 'click', // Show it on click...
                        solo: true, // ...and hide all other tooltips...                        
                    },
                    hide: 'unfocus',
                    style: {
                        classes: 'qtip-bootstrap',
                        width: 300,
                        height: 220

                    }
                });
            });


        });
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <asp:Button ID="cmdRemoveCookie" runat="server" Text="Remove cookie" />
    
    
    <asp:TextBox ID="txtMaskaFolders" runat="server" Width="300px" ></asp:TextBox>

    <hr />
    <asp:Button ID="cmdFolders" runat="server" Text="Přejmenovat složky" />

    <table>
    <asp:Repeater ID="rp1" runat="server">
        <ItemTemplate>
            <tr>
                <td>
                    <asp:Label ID="Project" runat="server"></asp:Label>
                </td>
            </tr>
        </ItemTemplate>
    </asp:Repeater>
    </table>


    <hr />
    <div style="left:100px;border:solid 1px red;padding:10px;">
        <a class="recpop" rel="clue_popup.aspx?pid=1"></a>
    </div>
    <div style="left:100px;border:solid 1px red;padding:10px;">
        <a class="recpop" rel="clue_popup.aspx?pid=2"></a>
    </div>
    <div style="left:100px;border:solid 1px red;padding:10px;">
        <a class="recpop" rel="clue_popup.aspx?pid=3"></a>
    </div>
    <div style="left:100px;border:solid 1px red;padding:10px;">
        <a class="recpop" rel="clue_popup.aspx?pid=4"></a>
    </div>
    <div style="left:100px;border:solid 1px red;padding:10px;">
        <a class="recpop" rel="clue_popup.aspx?pid=4"></a>
    </div>
    <div style="left:100px;border:solid 1px red;padding:10px;">
        <a class="recpop" rel="clue_popup.aspx?pid=4"></a>
    </div>
    <div style="left:100px;border:solid 1px red;padding:10px;">
        <a class="recpop" rel="clue_popup.aspx?pid=4"></a>
    </div>
    <div style="left:100px;border:solid 1px red;padding:10px;">
        <a class="recpop" rel="clue_popup.aspx?pid=4"></a>
    </div>
    <div style="left:100px;border:solid 1px red;padding:10px;">
        <a class="recpop" rel="clue_popup.aspx?pid=4"></a>
    </div>
    <div style="left:100px;border:solid 1px red;padding:10px;">
        <a class="recpop" rel="clue_popup.aspx?pid=4"></a>
    </div>
    <div style="left:100px;border:solid 1px red;padding:10px;">
        <a class="recpop" rel="clue_popup.aspx?pid=4"></a>
    </div>
    <div style="left:100px;border:solid 1px red;padding:10px;">
        <a class="recpop" rel="clue_popup.aspx?pid=4"></a>
    </div>
    <div style="left:100px;border:solid 1px red;padding:10px;">
        <a class="recpop" rel="clue_popup.aspx?pid=4"></a>
    </div>
    <div style="left:100px;border:solid 1px red;padding:10px;">
        <a class="recpop" rel="clue_popup.aspx?pid=4"></a>
    </div>
    <div style="left:100px;border:solid 1px red;padding:10px;">
        <a class="recpop" rel="clue_popup.aspx?pid=4"></a>
    </div>
    
    <hr />
</asp:Content>



