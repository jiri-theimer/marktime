<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="ErrorPage.aspx.vb" Inherits="UI.ErrorPage" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>MARKTIME 5.0 | Chybová stránka</title>
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <link href="~/Styles/Site.css" rel="stylesheet" type="text/css" />
</head>
<body>
    <form id="form1" runat="server">
    <div class="div6">
    <img src="Images/logo_transparent.png" alt="MARKTIME" />
    </div>
    <br />
    <br />
    <table cellpadding="4" cellspacing="0">
			<tr valign="top">
				<td width="150px">
				<img border="0" src="Images/exclaim_32.png" width="32px" height="32px">
				</td>
				<td>
								
				<asp:Label ID="lblError" Runat="server" style="font-weight:bold;color:Red;"></asp:Label>
				
				</td>
			</tr>
            <tr>
				<td>Datum:</td>
				<td><asp:Label ID="lblDate" Runat="server"></asp:Label></td>
			</tr>
			<tr>
				<td>Stránka:</td>
				<td><asp:Label ID="lblPage" Runat="server" CssClass="val"></asp:Label></td>
			</tr>
			<tr>
				<td>Volající stránka:</td>
				<td><asp:Label ID="lblRefPage" Runat="server" CssClass="val"></asp:Label></td>
			</tr>
			
			<tr>
				<td>Uživatel:</td>
				<td><asp:Label ID="lblUser" Runat="server" CssClass="val"></asp:Label></td>
			</tr>
			
		</table>
        
		<hr>
		<asp:Label ID="lblHTTPError" Runat="server" style="font-weight:normal;color:green;"></asp:Label>

    </form>
</body>
</html>
