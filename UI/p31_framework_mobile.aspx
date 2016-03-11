<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Mobile.Master" CodeBehind="p31_framework_mobile.aspx.vb" Inherits="UI.p31_framework_mobile" %>

<%@ MasterType VirtualPath="~/Mobile.Master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        tr.trTextRow {
            border-bottom: solid 2px gray;
        }

        .task_in_table {
            background-color: #5bc0de;
            color: white;
            text-align: center;
            white-space: pre-wrap;
            padding: 4px;
            margin-bottom: 5px;
            font-weight: bold;
            border-radius: 4px;
            font-size: 90%;
        }
    </style>
    <script type="text/javascript">
        $(document).ready(function () {
            $("#<%=p31Date.ClientID%>").datepicker({
                dateFormat: 'dd.mm.yy'
            });



        });

        function hardrefresh(flag, value) {

            document.getElementById("<%=HardRefreshValue.ClientID%>").value = value;
            document.getElementById("<%=HardRefreshFlag.ClientID%>").value = flag;
            <%=Me.ClientScript.GetPostBackEventReference(Me.cmdHardRefresh, "", False)%>;


        }

        function trydel() {

            if (confirm("Opravdu odstranit záznam?")) {
                hardrefresh('delete', '1');
                return (true);
            }
            else {
                return (false);
            }
        }

    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <a id="record_start"></a>
 

    <nav class="navbar navbar-default">
            <div class="navbar-header">
                <button type="button" class="navbar-toggle" data-toggle="collapse" data-target="#myNavbarOnSite">
                    <span class="icon-bar"></span>
                    <span class="icon-bar"></span>
                    <span class="icon-bar"></span>
                </button>
                
                <asp:Image ID="imgHeader" runat="server" ImageUrl="Images/new.png" CssClass="navbar-brand" Visible="false" />
                <asp:Label ID="lblEntryHeader" runat="server" Text="Zapsat úkon" CssClass="navbar-brand"></asp:Label>
                
            </div>
            <div class="collapse navbar-collapse" id="myNavbarOnSite">
                <ul class="nav navbar-nav">
                    <li><a href="javascript:hardrefresh('clear','1')">Nový úkon</a></li>
                    <li role="separator" class="divider"></li>
                    <li class="dropdown">
                        <a class="dropdown-toggle" data-toggle="dropdown"  href="#">Struktura nabídky projektů<span class='caret'></span></a>                        
                        <ul class="dropdown-menu">
                            <li <%=IIf(Me.hidPLS.Value = "1", "class='active'", "")%>><a href="javascript:hardrefresh('pls','1')">Klasický seznam [Klient] + [Projekt]</a></li>
                            <li <%=IIf(Me.hidPLS.Value = "2", "class='active'", "")%>><a href="javascript:hardrefresh('pls','2')">2-úrovňový strom [Klient]->[Projekt]</a></li>
                        </ul>
                    </li>
                    <li class="dropdown">
                        <a class="dropdown-toggle" data-toggle="dropdown"  href="#">Maska zobrazení projektu v nabídce<span class='caret'></span></a>                        
                        <ul class="dropdown-menu">
                            <li <%=IIf(Me.hidMask.Value = "1", "class='active'", "")%>><a href="javascript:hardrefresh('mask','1')">[Název projektu]</a></li>
                            <li <%=IIf(Me.hidMask.Value = "2", "class='active'", "")%>><a href="javascript:hardrefresh('mask','2')">[Název projektu]+([Kód projektu])</a></li>
                            <li <%=IIf(Me.hidMask.Value = "3", "class='active'", "")%>><a href="javascript:hardrefresh('mask','3')">[Název projektu]+([Typ projektu])</a></li>
                            <li <%=IIf(Me.hidMask.Value = "4", "class='active'", "")%>><a href="javascript:hardrefresh('mask','4')">[Název projektu]+([Středisko projektu])</a></li>
                        </ul>
                    </li>
                    <li class="dropdown">
                        <a class="dropdown-toggle" data-toggle="dropdown"  href="#">Kolik dní zpětně zobrazuje přehled úkonů<span class='caret'></span></a>                        
                        <ul class="dropdown-menu">
                            <li <%=IIf(Me.hidDaysQueryBefore.Value = "20", "class='active'", "")%>><a href="javascript:hardrefresh('daysquerybefore','20')">20 dní</a></li>
                            <li <%=IIf(Me.hidDaysQueryBefore.Value = "10", "class='active'", "")%>><a href="javascript:hardrefresh('daysquerybefore','10')">10 dní</a></li>
                            <li <%=IIf(Me.hidDaysQueryBefore.Value = "5", "class='active'", "")%>><a href="javascript:hardrefresh('daysquerybefore','5')">5 dní</a></li>
                            <li <%=IIf(Me.hidDaysQueryBefore.Value = "2", "class='active'", "")%>><a href="javascript:hardrefresh('daysquerybefore','2')">2 dny</a></li>
                        </ul>
                    </li>
                    <li>
                      
                        <asp:CheckBox ID="chkShowTop10" runat="server" AutoPostBack="true" Text="Nabídku projektů omezit na max.10 naposledy vykazovaných" />
                     
                    </li>
                </ul>
               
            </div>

    </nav>
    





    <asp:Panel ID="panDropdownSelectP41ID" runat="server" CssClass="dropdown">
        <button class="btn btn-default dropdown-toggle" type="button" id="dropdownMenu1" data-toggle="dropdown" aria-haspopup="true" aria-expanded="true">
            <asp:Label ID="lblVybratProjekt" runat="server" Text="Vybrat projekt"></asp:Label>
            <span class="caret"></span>
        </button>

        <ul class="dropdown-menu" aria-labelledby="dropdownMenu1">
            <asp:Repeater ID="rpP41_cbx" runat="server">
                <ItemTemplate>
                    <asp:HyperLink ID="linkClient" runat="server" CssClass="list-group-item disabled" Visible="false"></asp:HyperLink>
                    <li id="li1" runat="server">
                        <asp:HyperLink ID="link1" runat="server"></asp:HyperLink>
                        <asp:HiddenField ID="pid" runat="server" />
                    </li>
                </ItemTemplate>
            </asp:Repeater>

        </ul>
    </asp:Panel>

    <asp:Panel ID="panDropdownSelectP56ID" runat="server" CssClass="dropdown">
        <button class="btn btn-default dropdown-toggle" type="button" id="dropdownMenu2" data-toggle="dropdown" aria-haspopup="true" aria-expanded="true">
            <asp:Label ID="lblVybratUkol" runat="server" Text="Vybrat úkol"></asp:Label>
            <span class="caret"></span>
        </button>

        <ul class="dropdown-menu" aria-labelledby="dropdownMenu2">
            <asp:Repeater ID="rpP56_cbx" runat="server">
                <ItemTemplate>
                    <asp:HyperLink ID="linkProject" runat="server" CssClass="list-group-item disabled" Visible="false"></asp:HyperLink>
                    <li id="li1" runat="server">
                        <asp:HyperLink ID="link1" runat="server"></asp:HyperLink>
                        <asp:HiddenField ID="pid" runat="server" />
                    </li>
                </ItemTemplate>
            </asp:Repeater>

        </ul>
    </asp:Panel>


    <h4>
        <div class="btn-group" role="group" aria-label="...">
        <asp:hyperlink ID="Client" runat="server" CssClass="btn btn-success btn-xs"></asp:hyperlink>
        <asp:HyperLink ID="Project" CssClass="btn btn-primary btn-xs" runat="server"></asp:HyperLink>
        
        </div>
        <div>
            <asp:hyperlink ID="Task" runat="server" CssClass="btn btn-info btn-xs" style="margin-top:4px;"></asp:hyperlink>
        </div>
    </h4>
    <div>
        <asp:TextBox ID="p31Date" runat="server" CssClass="form-control"></asp:TextBox>
    </div>
    <div style="margin-top: 15px;">
        <asp:DropDownList ID="p34ID" runat="server" AutoPostBack="true" DataTextField="p34Name" DataValueField="pid" ToolTip="Sešit úkonu" CssClass="form-control"></asp:DropDownList>
    </div>
    <div style="margin-top: 10px;">
        <asp:DropDownList ID="p32ID" runat="server" AutoPostBack="true" DataTextField="p32Name" DataValueField="pid" ToolTip="Aktivita úkonu" CssClass="form-control"></asp:DropDownList>
        <a id="record_p32id"></a>
    </div>
    <asp:Panel ID="panT" runat="server" Visible="false">
        <table>
            <tr>
                <td>Hodiny:</td>
                <td>
                    <asp:TextBox ID="p31Hours_Orig" runat="server" CssClass="form-control"></asp:TextBox>
                </td>
            </tr>
        </table>

    </asp:Panel>
    <asp:Panel ID="panM" runat="server" Visible="false">
        <table>
            <tr>
                <td>
                    <asp:Label ID="lblp31Amount_WithoutVat_Orig" runat="server" Text="Částka:"></asp:Label></td>
                <td>
                    <asp:TextBox ID="p31Amount_WithoutVat_Orig" runat="server" CssClass="form-control"></asp:TextBox>

                </td>
                <td>
                    <asp:DropDownList ID="j27ID_Orig" runat="server" DataTextField="j27Code" DataValueField="pid" CssClass="form-control"></asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Label ID="lblp31VatRate_Orig" runat="server" Text="DPH:"></asp:Label></td>
                <td>
                    <asp:DropDownList ID="p31VatRate_Orig" runat="server" DataTextField="p53Value" DataValueField="p53Value" CssClass="form-control">
                    </asp:DropDownList>

                </td>
                <td>
                    <asp:TextBox ID="p31Amount_Vat_Orig" runat="server" CssClass="form-control"></asp:TextBox>
                </td>
            </tr>

            <tr>
                <td>
                    <asp:Label ID="lblp31Amount_WithVat_Orig" runat="server" Text="Vč.DPH:"></asp:Label></td>
                <td>
                    <asp:TextBox ID="p31Amount_WithVat_Orig" runat="server" CssClass="form-control"></asp:TextBox>
                </td>
                <td></td>
            </tr>
        </table>
    </asp:Panel>
    <asp:Panel ID="panU" runat="server" Visible="false">
        <table>
            <tr>
                <td>Počet:</td>
                <td>
                    <asp:TextBox ID="p31Value_Orig" runat="server" CssClass="form-control"></asp:TextBox>
                </td>
            </tr>
        </table>

    </asp:Panel>
    <div>
        <span>Popis:</span>
        <asp:TextBox ID="p31Text" runat="server" CssClass="form-control" Style="height: 90px; background-color: #ffffcc;" TextMode="MultiLine"></asp:TextBox>
    </div>
    <div style="margin-top: 20px;">
        <button type="button" class="btn btn-default" onclick="hardrefresh('clear','1')">Zrušit</button>
        <button type="button" class="btn btn-primary" onclick="hardrefresh('save','1')">Uložit</button>
        <button type="button" class="btn btn-primary" onclick="hardrefresh('saveandcopy','1')" style="margin-left: 20px; float: right;">Uložit & kopírovat</button>
    </div>
    <div>
        <button type="button" class="btn btn-danger" onclick="trydel()" id="cmdDelete" runat="server" style="margin-top:20px;">Odstranit záznam</button>
    </div>

    <a id="record_message"></a>
    <asp:Panel ID="panMessage" runat="server" CssClass="alert alert-danger" role="alert" Visible="false">
        <asp:Label ID="WarningMessage" runat="server"></asp:Label>
    </asp:Panel>

    </asp:Panel>
    </div>

    <a id="record_list"></a>
    <div class="panel panel-default" style="margin-top: 20px;">
        <!-- Default panel contents -->
        <div class="panel-heading">
            <img src="Images/record.png" /><asp:Label ID="lblListP31ListHeader" runat="server" Text="Zapsané úkony"></asp:Label>
        </div>
        <table class="table table-condensed">

            <asp:Repeater ID="rpP31" runat="server">
                <ItemTemplate>
                    <tr style="background-color: whitesmoke;" id="trDate" runat="server">
                        <td colspan="4">
                            <asp:Label ID="p31Date" runat="server" Font-Bold="true"></asp:Label>

                            <asp:Label ID="Pocet" runat="server" Style="padding-left: 20px;"></asp:Label>

                            <asp:Label ID="Hodiny" runat="server" Style="padding-left: 20px;" class="badge1"></asp:Label>
                        </td>

                    </tr>
                    <tr>

                        <td>
                            <asp:Label ID="Project" runat="server"></asp:Label>

                        </td>
                        <td>
                            <asp:Label ID="p32Name" runat="server"></asp:Label>
                        </td>
                        <td style="text-align: right;">
                            <asp:Label ID="p31Value_Orig" runat="server"></asp:Label>
                        </td>
                        <td>
                            <a href="javascript:hardrefresh('edit',<%# Eval("PID")%>)">
                                <img border="0" src="Images/edit.png" /></a>
                        </td>
                    </tr>
                    <tr class="trTextRow">
                        <td style="font-size: 90%;" colspan="4">
                            <div>
                                <asp:Label ID="Task" runat="server" CssClass="task_in_table"></asp:Label>
                            </div>
                            <asp:Label ID="p31Text" runat="server" Font-Italic="true"></asp:Label>
                        </td>
                    </tr>
                </ItemTemplate>
            </asp:Repeater>
        </table>

        <asp:HiddenField ID="HardRefreshValue" runat="server" />
        <asp:HiddenField ID="HardRefreshFlag" runat="server" />
        <asp:Button ID="cmdHardRefresh" runat="server" Style="display: none;" />
        <asp:HiddenField ID="hidPLS" runat="server" Value="1" />
        <asp:HiddenField ID="hidMask" runat="server" Value="1" />
        <asp:HiddenField ID="hidP41ID" runat="server" Value="" />
        <asp:HiddenField ID="hidP56ID" runat="server" />
        <asp:HiddenField ID="hidP33ID" runat="server" />
        <asp:HiddenField ID="hidP31ID" runat="server" />
        <asp:HiddenField ID="hidDaysQueryBefore" runat="server" Value="10" />
        <asp:HiddenField ID="hidDirectCallP41ID" runat="server" />
</asp:Content>
