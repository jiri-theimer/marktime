<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master" CodeBehind="pokus.aspx.vb" Inherits="UI.pokus" %>

<%@ MasterType VirtualPath="~/Site.Master" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>


<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
  

    <script type="text/javascript">
       
        function contMenu(url) {
            sw_everywhere(url, "", true);
        }
        function contReload(url) {
            window.open(url, "_top");
        }

        function hovado() {
            alert("ahoj");
        }

        function RCM2(curPREFIX, curPID, ctl) {
            alert(ctl);
            var curPAGE = "";
            $.ajax({
                method: "POST",
                url: "Handler/handler_popupmenu.ashx",
                beforeSend: function () {
                    
                },
                async: true,
                timeout: 3000,
                data: { prefix: curPREFIX, pid: curPID, page: curPAGE },
                success: function (data) {
                    //alert("načítání");
                    //$('#html5menu').html('');

                    var contextMenu = $find("<%= RadContextMenu1.ClientID %>");
                    contextMenu.get_items().clear();

                    for (var i in data) {
                        var c = data[i];
                        var mi = new Telerik.Web.UI.RadMenuItem();

                        if (c.IsSeparator == true) {
                            mi.set_isSeparator(true);                            
                        }

                        if (c.IsSeparator == false) {
                            mi.set_text(c.Text);

                           
                            if (c.Target == "_top")
                                mi.set_navigateUrl("contReload('" & c.NavigateUrl & "')");
                            else
                                mi.set_navigateUrl(c.NavigateUrl);
                        }

                        if (c.ImageUrl != null) {
                            mi.set_imageUrl(c.ImageUrl);
                        }

                        contextMenu.get_items().add(mi);
                    }

                    

                    var x = $(ctl).offset().left
                    var y = $(ctl).offset().top
                    

                    contextMenu.showAt(x + 20, y);



                },
                complete: function () {
                    // do the job here
                    
                }
            });

            ;

        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div style="height:100px;width:100%;">

    </div>
    <img src='Images/notepad_32.png' style="position:absolute;left:445px;float:right;"/>
    <hr />
    <div style="position:absolute;top:200px; left:500px;">
    <a id="cmdPP" class="pp2" onclick="RCM2('p56','9',this)"></a>
    </div>
    <hr />
    <div>
    <a id="cmdPP2" class="pp2" onclick="RCM2('p56','44',this)"></a>
    </div>
    
     <hr />
       <a id="cmdPP3" class="pp2" href="javascript:RCM2('p56','9',event)"></a>
    
    
<telerik:RadContextMenu ID="RadContextMenu1" runat="server" EnableViewState="false" Skin="Metro" ExpandDelay="0"  >
    <CollapseAnimation Type="None" />
    <ExpandAnimation Type="None" />    
</telerik:RadContextMenu>
</asp:Content>



