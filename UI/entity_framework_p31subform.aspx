﻿<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Clue.Master" CodeBehind="entity_framework_p31subform.aspx.vb" Inherits="UI.entity_framework_p31subform" %>
<%@ MasterType VirtualPath="~/Clue.Master" %>

<%@ Register TagPrefix="uc" TagName="p31_subgrid" Src="~/p31_subgrid.ascx" %>

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

            $("a.button-reczoom").each(function () {

                // Extract your variables here:
                var $this = $(this);
                var myurl = $this.attr('rel');

                var mytitle = $this.attr('title');
                if (mytitle == null)
                    mytitle = 'Modal dialog';


                $this.qtip({
                    content: {
                        text: '<iframe src="' + myurl + '"' + ' width=' + iframeWidth + '"' + ' height=' + '"' + iframeHeight + '"  frameborder="0"><p>Your browser does not support iframes.</p></iframe>',
                        title: {
                            text: mytitle,
                            button: true
                        },

                    },
                    position: {
                        my: 'top center',  // Position my top left...
                        at: 'bottom center', // at the bottom right of...
                        viewport: $(window)
                    },
                    show: {
                        event: 'click', // Show it on click...
                        solo: true, // ...and hide all other tooltips...
                        modal: true // ...and make it modal
                    },
                    hide: false,
                    style: {
                        classes: 'qtip-tipped',
                        width: 700,
                        height: 300

                    }
                });
            });

        });

        function p31_RowSelected(sender, args) {
            ///volá se z p31_subgrid
            document.getElementById("<%=hiddatapid_p31.clientid%>").value = args.getDataKeyValue("pid");

        }

        function p31_RowDoubleClick(sender, args) {
            ///volá se z p31_subgrid
            var pid = document.getElementById("<%=hiddatapid_p31.clientid%>").value;
            
            window.parent.sw_decide("p31_record.aspx?pid=" + pid, "Images/worksheet.png",false);

        }
        function p31_clone() {
            ///volá se z p31_subgrid
            var pid = document.getElementById("<%=hiddatapid_p31.clientid%>").value;
            if (pid == "") {
                alert("Musíte vybrat záznam")
                return;
            }
            window.parent.sw_local("p31_record.aspx?clone=1&pid=" + pid, "Images/worksheet.png", false);
            
        }
        function p31_split() {
            ///volá se z p31_subgrid
            var pid = document.getElementById("<%=hiddatapid_p31.clientid%>").value;
            if (pid == "")
            {
                alert("Musíte vybrat záznam")
                return;
            }
            window.parent.sw_decide("p31_record_split.aspx?pid=" + pid, "Images/split.png", false);
            
        }
        function p31_entry() {
            ///volá se z p31_subgrid
            var url = "p31_record.aspx?pid=0";            
            <%if Me.CurrentMasterPrefix="p41" then%>
            url=url+"&p41id=<%=me.CurrentMasterPID%>";
            <%End If%>
             <%If Me.CurrentMasterPrefix = "p28" Then%>
            url = url + "&p28id=<%=me.CurrentMasterPID%>";
            <%End If%>
            <%If Me.CurrentMasterPrefix = "j02" Then%>
            url = url + "&j02id=<%=me.CurrentMasterPID%>";
            <%End If%>
            <%If gridP31.MasterTabAutoQueryFlag<>"" then%>
            url = url + "&tabqueryflag=<%=gridP31.MasterTabAutoQueryFlag%>";
            <%End If%>
            window.parent.sw_decide(url, "Images/worksheet.png", false);
            
        }
        function p31_subgrid_setting(j74id) {
            ///volá se z p31_subgrid
            window.parent.sw_decide("grid_designer.aspx?prefix=p31&masterprefix=<%=gridP31.MasterPrefixWithQueryFlag%>&pid=" + j74id, "Images/griddesigner_32.png", true);

        }
        function p31_subgrid_approving(pids) {
            try{
                window.parent.parent.sw_master("p31_approving_step2.aspx?pids=" + pids, "Images/approve_32.png", true);
            }
            catch(err){
                window.parent.sw_master("p31_approving_step2.aspx?pids=" + pids, "Images/approve_32.png", true);
            }
        }
        function p31_subgrid_querybuilder(j70id) {           
            window.parent.sw_decide("query_builder.aspx?prefix=p31&x36key=p31_subgrid-j70id&pid=" + j70id, "Images/query_32.png", true);
           
        }
        function p31_subgrid_periodcombo() {
            window.parent.sw_decide("periodcombo_setting.aspx", "Images/settings_32.png");
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div id="load1"></div>
    <uc:p31_subgrid ID="gridP31" runat="server" EntityX29ID="p41Project" AllowMultiSelect="true"></uc:p31_subgrid>


    <asp:HiddenField ID="hidMasterPrefix" runat="server" />
    <asp:HiddenField ID="hidMasterPID" runat="server" />
    <asp:HiddenField ID="hiddatapid_p31" runat="server" />


</asp:Content>