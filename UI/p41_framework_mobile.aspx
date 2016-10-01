<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Mobile.Master" CodeBehind="p41_framework_mobile.aspx.vb" Inherits="UI.p41_framework_mobile" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<%@ MasterType VirtualPath="~/Mobile.Master" %>
<%@ Register TagPrefix="uc" TagName="entityrole_assign_inline" Src="~/entityrole_assign_inline.ascx" %>
<%@ Register TagPrefix="uc" TagName="datepicker" Src="~/datepicker.ascx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
  
    <script type="text/javascript">
        $(document).ready(function () {

            if (window.chrome && window.chrome.webstore) {
                //prohlížeč je chrome

            } else {
                //prohlížeč není chrome



            }


            $("#<%=txt1.ClientID%>").datepicker({
                dateFormat: 'dd.mm.yy'
            });


            $("#<%=txtTime.ClientID%>").click(function () {
                $("#cmdTime").click();
            });


            $('#datum1').datepicker({
                format: "dd.mm.yyyy",
                todayBtn: "linked",
                clearBtn: true,
                language: "cs",
                autoclose: true
            });

        });





        function st(time)
        {
            
            $("#<%=txtTime.ClientID%>").val(time);
            $("#<%=txtTime.ClientID%>").focus();
        }


        

    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">


    <nav class="navbar navbar-default">
            <div class="navbar-header">
                <button type="button" class="navbar-toggle" data-toggle="collapse" data-target="#myNavbarOnSite">
                    <span class="icon-bar"></span>
                    <span class="icon-bar"></span>
                    <span class="icon-bar"></span>
                </button>
                
                <asp:hyperlink ID="ProjectHeader" runat="server" CssClass="navbar-brand"></asp:hyperlink>
                
            </div>
            <div class="collapse navbar-collapse" id="myNavbarOnSite">
                <ul class="nav navbar-nav">
                    <li><a href="p31_framework_mobile.aspx?p41id=<%=Master.DataPID%>">Zapsat worksheet úkon</a></li>
                    <li role="separator" class="divider"></li>
                    <li><a href="p41_framework_mobile.aspx">Nový úkol</a></li>
                  
                    
                    <li><a href="default.aspx">Nová poznámka</a></li>

                    <li><a href="#">Rozpracovanost <span class="badge">14x</span></a></li>
                    <li><a href="#">Schváleno & čeká na fakturaci <span class="badge">11x</span></a></li>
                    
                </ul>
               
            </div>

    </nav>

    <div class="form-group">
        <asp:HyperLink ID="clue_client" runat="server" CssClass="reczoom" Text="i" title="Detail klienta"></asp:HyperLink>
        <label>Klient:</label>
        <asp:HyperLink ID="Client" runat="server"></asp:HyperLink>

    </div>
    <asp:Panel ID="panPricelist" runat="server" CssClass="form-group">
        <asp:HyperLink ID="clue_p51id_billing" runat="server" CssClass="reczoom" Text="i" title="Detail fakturačního ceníku"></asp:HyperLink>
        <label>Fakturační ceník:</label>
        <asp:Label ID="PriceList_Billing" runat="server"></asp:Label>
        <asp:Label ID="PriceList_Billing_Message" runat="server"></asp:Label>
    </asp:Panel>
    <div class="form-group">
        <asp:HyperLink ID="clue_p42id" runat="server" CssClass="reczoom" Text="i" title="Detail typu projektu"></asp:HyperLink>
        <label>Typ projektu:</label>
        <asp:Label ID="p42Name" runat="server"></asp:Label>
    </div>

    <uc:entityrole_assign_inline ID="roles_project" runat="server" EntityX29ID="p41Project" NoDataText="V projektu nejsou přiřazeny projektové role!"></uc:entityrole_assign_inline>

    <h1>
        <asp:LinkButton ID="cmdLinkCMD" CssClass="btn btn-default btn-lg" runat="server"><span class="glyphicon glyphicon-cog"></span></asp:LinkButton>
    </h1>

    <div class="btn-group btn-group">
        <asp:LinkButton ID="LinkButton1" CssClass="btn btn-primary" runat="server"><span aria-hidden="true" class="glyphicon glyphicon-floppy-save"></span>Uložit změny</asp:LinkButton>
        <asp:LinkButton ID="LinkButton3" CssClass="btn btn-warning" runat="server"><span aria-hidden="true" class="glyphicon glyphicon-trash"></span>Přesunout do koše</asp:LinkButton>
        <asp:LinkButton ID="LinkButton2" CssClass="btn btn-danger disabled" runat="server"><span aria-hidden="true" class="glyphicon glyphicon-remove"></span>Odstranit</asp:LinkButton>

        <asp:HyperLink ID="LinkButton5" CssClass="btn btn-default" runat="server" NavigateUrl="javascript:sw_modal()" Text="Přidané tlačítko a pěkně dlouhé"></asp:HyperLink>

    </div>

    <asp:LinkButton ID="LinkButton4" CssClass="btn btn-primary" runat="server" Text="<img src='Images/new.png'/>Nový záznam"></asp:LinkButton>
    <hr />
    <div style="background-color:whitesmoke;padding:4px;">
    <ul class="nav nav-pills" >
  <li role="presentation" ><a href="#">Worksheet summary</a></li>
  <li role="presentation" class="active"><a href="#">Worksheet přehled <span class="badge">56</span></a></li>
  <li role="presentation"><a href="#">Úkoly <span class="badge">2</span></a></li>
        <li role="presentation"><a href="#">Vystavené faktury</a></li>
        <li role="presentation"><a href="#">Komentáře</a></li>
        <li role="presentation"><a href="#">Žádný pod-přehled</a></li>
</ul>
</div>

    
    <h1>DateTime picker</h1>
    <div class="form-group">
        <label>Datum vystavení:</label>
        <uc:datepicker ID="dat1" runat="server" IsUseTimepicker="true" NumberOfMonths="2"></uc:datepicker>
        <asp:Button ID="cmdDatum" runat="server" Text="Datum" />
    </div>
    <uc:datepicker ID="dat2" runat="server" IsUseTimepicker="true" IsTimePer30Minutes="true" starthour="9" endhour="22"></uc:datepicker>
    <asp:Button ID="cmdDatum2" runat="server" Text="Datum 2" />



    <div class="container-fluid">
        <asp:Panel ID="row1" runat="server" CssClass="row">


            <div class="col-sm-6 col-md-4">
                <div class="thumbnail">
                    <div class="caption">
                        <img src="Images/record.png" />
                        Základní vlastnosti
                    </div>
                    <h5>první</h5>
                    <h5>druhý</h5>
                    <h5>třetí</h5>
                </div>
            </div>



            <div class="col-sm-6 col-md-4">
                <div class="thumbnail">

                    <div class="caption">
                        <img src="Images/project.png" />
                        Otevřené projekty

                    </div>

                </div>

            </div>

            <asp:Panel runat="server" ID="inv1" CssClass="col-sm-6 col-md-4">
                <div class="thumbnail">

                    <div class="caption">
                        <h3>Thumbnail label 5, box 5</h3>
                        <p>...</p>

                    </div>
                    <span>Obsah článku</span>
                </div>

            </asp:Panel>



            <asp:Panel ID="inv2" runat="server" CssClass="col-sm-6 col-md-4">
                <div class="thumbnail">

                    <div>
                        <div>
                            <img src="Images/project.png" style="padding-right: 10px;" /><asp:Label ID="Label1" runat="server" Text="box 6 Základní vlastnosti"></asp:Label>
                        </div>
                        <div>
                            wrewrw
                        </div>
                    </div>

                </div>
            </asp:Panel>

            <div class="col-sm-6 col-md-4">
                <div class="thumbnail">

                    <div class="caption">
                        <h3>Thumbnail label, box 7</h3>
                        <p>...</p>

                    </div>
                    <span>Obsah článku</span>
                </div>

            </div>

            <div class="col-sm-6 col-md-4">
                <div class="thumbnail">

                    <div class="caption">
                        <h3>Thumbnail label, box 7</h3>
                        <p>...</p>

                    </div>
                    <span>Obsah článku</span>
                </div>

            </div>

            <asp:Panel ID="inv3" runat="server" CssClass="col-sm-6 col-md-4">
                <div class="thumbnail">

                    <div>
                        <div>
                            <img src="Images/project.png" style="padding-right: 10px;" /><asp:Label ID="Label3" runat="server" Text="box 66 Základní vlastnosti"></asp:Label>
                        </div>
                        <div>
                            wrewrw
                        </div>
                    </div>

                </div>
            </asp:Panel>

            <div class="col-sm-6 col-md-4">
                <div class="thumbnail">

                    <div class="caption">
                        <h3>Thumbnail label, box 7</h3>
                        <p>...</p>

                    </div>
                    <span>Obsah článku</span>
                </div>

            </div>

            <div class="col-sm-6 col-md-4">
                <div class="thumbnail">

                    <div class="caption">
                        <h3>Thumbnail label, box 7</h3>
                        <p>...</p>

                    </div>
                    <span>Obsah článku</span>
                </div>

            </div>
            <div class="col-sm-6 col-md-4">
                <div class="thumbnail">

                    <div class="caption">
                        <h3>Thumbnail label, box 7</h3>
                        <p>...</p>

                    </div>
                    <span>Obsah článku</span>
                </div>

            </div>
            <div class="col-sm-6 col-md-4">
                <div class="thumbnail">

                    <div class="caption">
                        <h3>Thumbnail label, box 7</h3>
                        <p>...</p>

                    </div>
                    <span>Obsah článku</span>
                </div>

            </div>
        </asp:Panel>
    </div>
    <asp:TextBox ID="txt1" runat="server" Width="110px" CssClass="form-control"></asp:TextBox>

    <h1>Záložky</h1>
    <ul class="nav nav-tabs">
        <li class="active"><a data-toggle="tab" href="#home">První záložka</a></li>
        <li><a data-toggle="tab" href="#menu1">Druhá žáložka</a></li>
        <li><a data-toggle="tab" href="#menu2">Třetí a poslední záložka</a></li>
    </ul>

    <div class="tab-content">
        <div id="home" class="tab-pane fade in active" style="width: 700px;">

            <div class="form-group">
                <div class="checkbox">
                    <asp:RadioButtonList ID="opgSubject" runat="server" RepeatDirection="Vertical">
                        <asp:ListItem Text="Sazbu definovat pro konkrétní osobu" Value="j02" Selected="true"></asp:ListItem>
                        <asp:ListItem Text="Sazbu definovat pro pozici osoby" Value="j07"></asp:ListItem>
                        <asp:ListItem Text="Sazba platí pro všechny osoby" Value="all"></asp:ListItem>
                    </asp:RadioButtonList>
                </div>
            </div>

            <div class="form-group">
                <div class="checkbox">
                    <label class="btn btn-default">
                        <asp:CheckBox ID="chkP32" Text="Sazbu definovat i pro konkrétní aktivitu z sešitu" runat="server" />
                    </label>
                </div>
            </div>
          

            <div class="form-group">

                <asp:Label ID="lblRate" Text="Sazba:" runat="server" AssociatedControlID="textbox1" CssClass="col-sm-2 control-label"></asp:Label>
                <div class="col-sm-10">
                    <asp:TextBox ID="Textbox1" runat="server" CssClass="form-control" Width="150px"></asp:TextBox>
                </div>
            </div>
            <div class="form-group">

                <asp:Label ID="Label2" Text="Datum narození:" runat="server" AssociatedControlID="txtDate" CssClass="col-sm-2 control-label"></asp:Label>
                <div class="col-sm-10">
                    <asp:TextBox ID="txtDate" runat="server" CssClass="form-control" Width="150px" type="date"></asp:TextBox>
                    <asp:LinkButton ID="cmdDate" runat="server" Text="Hodnota datumu"></asp:LinkButton>
                </div>
            </div>
            <div class="form-group">
                <asp:Label ID="lblName" runat="server" Text="Poznámka:" CssClass="col-sm-2 control-label" AssociatedControlID="p52Name"></asp:Label>
                <div class="col-sm-10">
                    <asp:TextBox ID="p52Name" runat="server" CssClass="form-control"></asp:TextBox>
                </div>
            </div>


        </div>
        <div id="menu1" class="tab-pane fade">
            <h3>Menu 1</h3>
            <p>Some content in menu 1.</p>
        </div>
        <div id="menu2" class="tab-pane fade">
            <h3>Menu 2</h3>
            <p>Some content in menu 2.</p>
        </div>
    </div>

    

    <div class="btn-group">
        
        <asp:TextBox ID="txtTime" runat="server" CssClass="btn btn-default" Width="70px" style="cursor:text;" Visible="true">            
        </asp:TextBox>
        <button type="button" id="cmdTime" class="btn btn-default dropdown-toggle" data-toggle="dropdown" aria-haspopup="true" aria-expanded="false">
            <span class="caret"></span>
            
        </button>
        <ul class="dropdown-menu">
            <li><a href="javascript:st('10:00')">10:00</a></li>
            <li><a href="javascript:st('11:00')">11:00</a></li>
            <li><a href="javascript:st('12:00')">12:00</a></li>
            
            <li><a href="#">13:00</a></li>
        </ul>
        
    </div>

    
    <h1>Odpad</h1>
    <asp:Panel ID="panKos" runat="server">
    </asp:Panel>

    <script type="text/javascript">
        
        
    </script>
</asp:Content>
