<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Clue.Master" CodeBehind="entity_framework_p91subform.aspx.vb" Inherits="UI.entity_framework_p91subform" %>
<%@ MasterType VirtualPath="~/Clue.Master" %>
<%@ Register TagPrefix="uc" TagName="p91_subgrid" Src="~/p91_subgrid.ascx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
            <link href="Scripts/jquery.qtip.min.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript" src="Scripts/jquery.qtip.min.js"></script>


    <script type="text/javascript">
        $(document).ready(function () {
            window.parent.stoploading();

            $("a.reczoom").each(function () {

                // Extract your variables here:
                var $this = $(this);
                var myurl = $this.attr('rel');

                var mytitle = $this.attr('title');
                if (mytitle == null)
                    mytitle = 'Detail';


                $this.qtip({
                    content: {
                        text: '<iframe scrolling=no src="' + myurl + '"' + ' width=' + iframeWidth + '"' + ' height=' + '"' + iframeHeight + '"  frameborder="0"><p>Your browser does not support iframes.</p></iframe>',
                        title: {
                            text: mytitle
                        },

                    },
                    position: {
                        my: 'top center',  // Position my top left...
                        at: 'bottom center', // at the bottom right of...
                        viewport: $(window)
                    },

                    hide: {

                        fixed: true,
                        delay: 100

                    },
                    style: {
                        classes: 'qtip-tipped',
                        width: 700,
                        height: 300

                    }
                });
            });
        });
        function periodcombo_setting() {

            window.parent.sw_decide("periodcombo_setting.aspx", "Images/settings_32.png");
        }
        function RowSelected_p91(sender, args) {
            document.getElementById("<%=hiddatapid_p91.ClientID%>").value = args.getDataKeyValue("pid");
        }

        function RowDoubleClick_p91(sender, args) {
            <%If Master.Factory.SysUser.j04IsMenu_Invoice Then%>
            window.parent.window.open("p91_framework.aspx?pid=" + document.getElementById("<%=hiddatapid_p91.ClientID%>").value, "_top")
            <%End If%>
        }

        function p91_subgrid_setting(j74id,masterprefix) {
            
            window.parent.sw_decide("grid_designer.aspx?prefix=p91&masterprefix="+masterprefix+"&pid=" + j74id, "Images/griddesigner.png", true);
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <uc:p91_subgrid ID="gridP91" runat="server" x29ID="p41Project" />


     <asp:HiddenField ID="hidMasterPrefix" runat="server" />
    <asp:HiddenField ID="hidMasterPID" runat="server" />
    <asp:HiddenField ID="hiddatapid_p91" runat="server" />


</asp:Content>
