<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Clue.Master" CodeBehind="entity_framework_o23subform.aspx.vb" Inherits="UI.entity_framework_o23subform" %>
<%@ MasterType VirtualPath="~/Clue.Master" %>
<%@ Register TagPrefix="uc" TagName="o23_subgrid" Src="~/o23_subgrid.ascx" %>


<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <link href="Scripts/jquery.qtip.min.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript" src="Scripts/jquery.qtip.min.js"></script>

    <script type="text/javascript">
        $(document).ready(function () {
            window.parent.stoploading();

            var iframeWidth = '100%';
            var iframeHeight = '270';

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
        function o23_subgrid_setting(j74id) {
            ///volá se z o23_subgrid
            window.parent.sw_decide("grid_designer.aspx?prefix=o23&masterprefix=<%=Me.CurrentMasterPrefix%>&pid=" + j74id, "Images/griddesigner.png", true);
        }
        function RowSelected_o23(sender, args) {
            document.getElementById("<%=hiddatapid_o23.ClientID%>").value = args.getDataKeyValue("pid");
        }

        function RowDoubleClick_o23(sender, args) {
            var pid = document.getElementById("<%=hiddatapid_o23.ClientID%>").value;
            window.parent.sw_decide("o23_record.aspx?pid=" + pid, "Images/notepad.png", false);
        }
        

        
        function o23_record(pid, bolReturnFalse) {
            window.parent.sw_decide("o23_record.aspx?masterprefix=<%=Me.CurrentMasterPrefix%>&masterpid=<%=Me.CurrentMasterPID%>&pid=" + pid, "Images/notepad.png", true);
            if (bolReturnFalse == true)
                return (false)

        }
        function o23_clone() {
            ///volá se z gridu úkolů
            var pid = document.getElementById("<%=hiddatapid_o23.ClientID%>").value;
            if (pid == "" || pid == null) {
                alert("Není vybrán záznam.");
                return (false);
            }
            window.parent.sw_decide("o23_record.aspx?clone=1&p41id=<%=Master.DataPID%>&pid=" + pid, "Images/notepad.png", true);
            return (false);
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <uc:o23_subgrid ID="gridO23" runat="server" x29ID="p41Project" />


    <asp:HiddenField ID="hidMasterPrefix" runat="server" />
    <asp:HiddenField ID="hidMasterPID" runat="server" />
    <asp:HiddenField ID="hiddatapid_o23" runat="server" />
</asp:Content>
