﻿<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Clue.Master" CodeBehind="entity_framework_p56subform.aspx.vb" Inherits="UI.entity_framework_p56subform" %>
<%@ MasterType VirtualPath="~/Clue.Master" %>
<%@ Register TagPrefix="uc" TagName="p56_subgrid" Src="~/p56_subgrid.ascx" %>

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
        function p56_subgrid_setting(j74id) {
            ///volá se z p56_subgrid
            window.parent.sw_decide("grid_designer.aspx?prefix=p56&masterprefix=<%=Me.CurrentMasterPrefix%>&pid=" + j74id, "Images/griddesigner.png", true);
        }
        function RowSelected_p56(sender, args) {
            document.getElementById("<%=hiddatapid_p56.ClientID%>").value = args.getDataKeyValue("pid");
        }

        function RowDoubleClick_p56(sender, args) {
            var pid = document.getElementById("<%=hiddatapid_p56.ClientID%>").value;
            window.parent.sw_decide("p56_record.aspx?pid=" + pid, "Images/task.png", false);
        }
        function p56_subgrid_approving(pids) {
            window.parent.parent.sw_master("p31_approving_step1.aspx?masterpid=<%=Me.CurrentMasterPID%>&masterprefix=<%=Me.CurrentMasterPrefix%>&prefix=p56&pid=" + pids, "Images/approve_32.png", true);

           
        }

        function p31_entry_p56() {
            ///volá se z gridu úkolů
            var p56id = document.getElementById("<%=hiddatapid_p56.ClientID%>").value;
            if (p56id == "" || p56id == null) {
                alert("Není vybrán úkol.");
                return (false);
            }
            window.parent.sw_decide("p31_record.aspx?pid=0&p41id=<%=Master.DataPID%>&p56id=" + p56id, "Images/worksheet.png", true);
            return (false);
        }
        function p56_record(pid, bolReturnFalse) {
            window.parent.sw_decide("p56_record.aspx?masterprefix=<%=Me.CurrentMasterPrefix%>&masterpid=<%=Me.CurrentMasterPID%>&pid=" + pid, "Images/task.png", true);
            if (bolReturnFalse == true)
                return (false)

        }
        function p56_clone() {
            ///volá se z gridu úkolů
            var pid = document.getElementById("<%=hiddatapid_p56.ClientID%>").value;
            if (pid == "" || pid == null) {
                alert("Není vybrán záznam.");
                return (false);
            }
            window.parent.sw_decide("p56_record.aspx?clone=1&p41id=<%=Master.DataPID%>&pid=" + pid, "Images/task.png", true);
            return (false);
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <uc:p56_subgrid ID="gridP56" runat="server" x29ID="p41Project" />


    <asp:HiddenField ID="hidMasterPrefix" runat="server" />
    <asp:HiddenField ID="hidMasterPID" runat="server" />
    <asp:HiddenField ID="hiddatapid_p56" runat="server" />
</asp:Content>
