<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="pokus_navigation.aspx.vb" Inherits="UI.pokus_navigation" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Strict//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-strict.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">

    <title runat="server" id="title1" enableviewstate="true">MARKTIME 5.0</title>
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <meta http-equiv="x-ua-compatible" content="IE=edge" />
    <meta name="viewport" content="width=device-width, initial-scale=1" />
    <link href="~/Styles/Site_v9.css" rel="stylesheet" type="text/css" />
    <link href="Scripts/jquery.qtip.min.css" rel="stylesheet" type="text/css" />


    <link href="~/Images/favicon.ico" rel="shortcut icon" type="image/x-icon" />
    <script src="Scripts/jquery-1.11.1.min.js" type="text/javascript"></script>
    <script type="text/javascript" src="Scripts/jquery.qtip.min.js"></script>


    <style type="text/css">
        html .RadNavigation_Windows7 .rnvRootGroupWrapper {
            background-color: #25a0da;
            background-image: none;
            color: white;
        }

        html .RadNavigation .rnvMore,
        html .RadNavigation .rnvRootLink {
            padding-top: 5px;
            padding-bottom: 5px;
            padding-right: 10px;
            padding-left: 10px;
        }


        .rnvPopup {
            border-bottom-right-radius: 8px;
            border-bottom-left-radius: 8px;
            box-shadow: 3px 2px 10px silver;
        }



        html .RadMenu_Metro .rmRootGroup {
            background-image: none;
            color: white;
        }

        html .RadMenu_Metro ul.rmRootGroup {
            background-color: #25a0da;
            color: white;
        }

        html .RadMenu_Metro .rmGroup {
            background-color: #F0F8FF !important;
            border-color: #A9A9A9 !important;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <telerik:RadScriptManager ID="RadScriptManager1" runat="server" />

        <telerik:RadNavigation ID="menu1" runat="server" MenuButtonPosition="Right" Skin="Metro" EnableViewState="false">
            
            <Nodes>
                <telerik:NavigationNode ID="begin" Width="50px" Enabled="false" Visible="true">
                </telerik:NavigationNode>
                <telerik:NavigationNode ID="fs" NavigateUrl="javascript:menu_fullscreen()" ImageUrl="Images/fullscreen.png"></telerik:NavigationNode>
                <telerik:NavigationNode ID="reload" ImageUrl="Images/refresh.png" Text=" " ToolTip="Obnovit stránku"></telerik:NavigationNode>

                <telerik:NavigationNode Text="ZÁZNAM FAKTURY" ID="record">
                    <Nodes>
                        <telerik:NavigationNode ID="cmdEdit" Text="Upravit kartu faktury" NavigateUrl="#" ImageUrl="Images/edit.png" ToolTip="Zahrnuje i možnost přesunutí do archivu nebo nenávratného odstranění."></telerik:NavigationNode>
                        <telerik:NavigationNode ID="cmdCreateInvoice" Text="Vystavit novou fakturu" NavigateUrl="#" ImageUrl="Images/new.png"></telerik:NavigationNode>

                        <telerik:NavigationNode ID="cmdPay" Text="Zapsat úhradu faktury" NavigateUrl="javascript:pay();" ImageUrl="Images/payment.png" ToolTip="Je možné zapsat úplnou nebo částečnou úhradu faktury."></telerik:NavigationNode>

                        <telerik:NavigationNode ID="cmdAppendWorksheet" Text="Přidat do faktury další položky (úkony)" NavigateUrl="#" ImageUrl="Images/worksheet.png" ToolTip="Způsob, jak do faktury přidat slevu, přirážku, fixní odměnu, další časové úkony, výdaje apod."></telerik:NavigationNode>
                        <telerik:NavigationNode ID="cmdChangeCurrency" Text="Převést fakturu na jinou měnu" NavigateUrl="#" ImageUrl="Images/recalc.png"></telerik:NavigationNode>
                        <telerik:NavigationNode ID="cmdChangeVat" Text="Převést fakturu kompletně na jinou DPH sazbu" NavigateUrl="#" ImageUrl="Images/recalc.png"></telerik:NavigationNode>


                    </Nodes>

                </telerik:NavigationNode>
                <telerik:NavigationNode Text="ZÁZNAM FAKTURY" ID="record2">
                    <Nodes>
                        <telerik:NavigationNode ID="xxx" Text="Test 1" NavigateUrl="#" ImageUrl="Images/edit.png"></telerik:NavigationNode>

                    </Nodes>

                </telerik:NavigationNode>
                <telerik:NavigationNode Text="DALŠÍ" ID="more">
                    <Nodes>
                        <telerik:NavigationNode ID="switchHeight" Text="Nastavení vzhledu stránky" ImageUrl="Images/setting.png" NavigateUrl="javascript:page_setting()">
                        </telerik:NavigationNode>

                        <telerik:NavigationNode ID="cmdPivot" Text="WORKSHEET statistika faktury" NavigateUrl="#" Target="_top" ImageUrl="Images/pivot.png"></telerik:NavigationNode>

                        <telerik:NavigationNode ID="cmdX40" Text="Historie odeslané pošty" Target="_top" ImageUrl="Images/email.png"></telerik:NavigationNode>

                        <telerik:NavigationNode ID="cmdO23" Text="Vytvořit dokument" NavigateUrl="#" ImageUrl="Images/notepad.png"></telerik:NavigationNode>
                        <telerik:NavigationNode ID="cmdO22" Text="Zapsat událost do kalendáře" NavigateUrl="#" ImageUrl="Images/calendar.png"></telerik:NavigationNode>
                        <telerik:NavigationNode ID="cmdB07" Text="Zapsat komentář/poznámku" NavigateUrl="#" ImageUrl="Images/comment.png"></telerik:NavigationNode>


                    </Nodes>


                </telerik:NavigationNode>
                <telerik:NavigationNode Text="DALŠÍ 1">
                    <Nodes>
                        <telerik:NavigationNode Text="Pod-další 1" NavigateUrl="#">
                        </telerik:NavigationNode>
                    </Nodes>
                </telerik:NavigationNode>
                <telerik:NavigationNode Text="DALŠÍ 2">
                    <Nodes>
                        <telerik:NavigationNode Text="Pod-další 2" NavigateUrl="#">
                        </telerik:NavigationNode>
                    </Nodes>
                </telerik:NavigationNode>
                <telerik:NavigationNode Text="DALŠÍ 3">
                    <Nodes>
                        <telerik:NavigationNode Text="Pod-další 3" NavigateUrl="#">
                        </telerik:NavigationNode>
                    </Nodes>
                </telerik:NavigationNode>
            </Nodes>
        </telerik:RadNavigation>
    </form>
</body>
</html>
