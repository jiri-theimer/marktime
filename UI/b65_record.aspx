<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/ModalDataRecord.Master" CodeBehind="b65_record.aspx.vb" Inherits="UI.b65_record" %>
<%@ MasterType VirtualPath="~/ModalDataRecord.Master" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<%@ Register TagPrefix="uc" TagName="datacombo" src="~/datacombo.ascx"%>


<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
<table cellpadding="3" cellspacing="2">		
    <tr>
        <td><asp:Label ID="Label3" Text="Entita:" runat="server" cssclass="lblReq" AssociatedControlID="x29id"></asp:Label></td>
        <td>
        <asp:DropDownList ID="x29ID" runat="server">
                    
                    <asp:ListItem Text="Projekt" Value="141"></asp:ListItem>
                    <asp:ListItem Text="Úkol" Value="356"></asp:ListItem>
            <asp:ListItem Text="Štítek" Value="925"></asp:ListItem>
                    
                </asp:DropDownList>
        </td>
    </tr>        
	<tr>
		<td class="frif"><asp:Label ID="lblB65Name" Text="Název:" runat="server" cssclass="lblReq" AssociatedControlID="b65name"></asp:Label></td>
		<td>
		<asp:TextBox ID="b65name" Runat="server" style="width:300px;"></asp:TextBox>
		</td>
            
	</tr>  
   
    <tr>
		<td class="frif"><asp:Label ID="lblb65MessageSubject" Text="Předmět zprávy:" runat="server" cssclass="lblReq" AssociatedControlID="b65MessageSubject"></asp:Label></td>
		<td>
		<asp:TextBox ID="b65MessageSubject" Runat="server" style="width:600px;"></asp:TextBox>
		</td>
            
	</tr>  
   
</table>
<div style="padding:6px;">
<div>
<asp:Label ID="lblb65MessageBody" Text="Tělo zprávy:" runat="server" cssclass="lbl" AssociatedControlID="b65MessageBody"></asp:Label>
</div>
<asp:TextBox ID="b65MessageBody" Runat="server" TextMode="MultiLine" style="width:97%;height:300px;"></asp:TextBox>
</div>

</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="FootContent" runat="server">
</asp:Content>


