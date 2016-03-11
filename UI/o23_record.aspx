<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/ModalDataRecord.Master" CodeBehind="o23_record.aspx.vb" Inherits="UI.o23_record" %>

<%@ MasterType VirtualPath="~/ModalDataRecord.Master" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<%@ Register TagPrefix="uc" TagName="pageheader" Src="~/pageheader.ascx" %>
<%@ Register TagPrefix="uc" TagName="datacombo" Src="~/datacombo.ascx" %>
<%@ Register TagPrefix="uc" TagName="fileupload" Src="~/fileupload.ascx" %>
<%@ Register TagPrefix="uc" TagName="fileupload_list" Src="~/fileupload_list.ascx" %>
<%@ Register TagPrefix="uc" TagName="b07_list" Src="~/b07_list.ascx" %>
<%@ Register TagPrefix="uc" TagName="freefields" Src="~/freefields.ascx" %>
<%@ Register TagPrefix="uc" TagName="freefields_readonly" Src="~/freefields_readonly.ascx" %>
<%@ Register TagPrefix="uc" TagName="contact" Src="~/contact.ascx" %>
<%@ Register TagPrefix="uc" TagName="person" Src="~/person.ascx" %>
<%@ Register TagPrefix="uc" TagName="project" Src="~/project.ascx" %>
<%@ Register TagPrefix="uc" TagName="entityrole_assign" Src="~/entityrole_assign.ascx" %>


<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <script type="text/javascript">
        function comment_create() {
            dialog_master("b07_create.aspx?masterprefix=o23&masterpid=<%=master.datapid%>", true)

        }
        function comment_reaction(b07id) {
            dialog_master("b07_create.aspx?parentpid=" + b07id + "&masterprefix=o23&masterpid=<%=master.datapid%>", true)

        }
        function hardrefresh(pid, flag) {
            location.replace("o23_record.aspx?pid=<%=Master.datapid%>&masterprefix=<%=Me.CurrentMasterPrefix%>&masterpid=<%=Me.CurrentMasterPID%>");

        }
        function go2module() {
            window.open("o23_framework.aspx?pid=<%=master.datapid%>", "_top");

        }


    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <asp:Panel ID="panSelectDocType" runat="server" Visible="false" DefaultButton="cmdSelectDocType" CssClass="content-box2">
        <div class="title">
            <span>Vyberte typ dokumentu</span>
            <asp:Button ID="cmdSelectDocType" runat="server" CssClass="cmd" Text="Pokračovat..." />
            <button type="button" onclick="onclick=CloseOnly()">Zrušit</button>
            
        </div>
        <div class="content">
            <div class="div6" style="background-color:#F0F8FF;">
                <asp:RadioButtonList ID="opgQueue" runat="server" RepeatDirection="Horizontal" Font-Bold="true" CellPadding="10">
                    <asp:ListItem Text="<img src='Images/new.png'/>&nbsp&nbspVytvořit nový dokument" Value="0" Selected="true"></asp:ListItem>
                    <asp:ListItem Text="<img src='Images/queue.png'/>&nbsp&nbspPřiřadit (spárovat) čekající dokument ve frontě" Value="1"></asp:ListItem>
                </asp:RadioButtonList>
            </div>

            <asp:RadioButtonList ID="opgSelectDocType" runat="server" RepeatDirection="Vertical" CellPadding="6" DataTextField="o24Name" DataValueField="pid"></asp:RadioButtonList>
        </div>

    </asp:Panel>

    <asp:Panel ID="panEntryPassword" runat="server" DefaultButton="cmdUnlock" Visible="false">
        <p class="failureNotification">Obsah je chráněn heslem!</p>
        <fieldset>
            <legend>
                <img src="Images/spy.png" style="margin-right: 6px;" />Zadejte heslo</legend>
            <div class="div6">
                <asp:TextBox ID="txtEntryPassword" runat="server" Style="width: 130px;" TextMode="Password"></asp:TextBox>
                <asp:Button ID="cmdUnLock" runat="server" CssClass="cmd" Text="Odemknout" />
            </div>
        </fieldset>
    </asp:Panel>
    <asp:Panel ID="panReadonly" runat="server" Visible="false">
        <div class="content-box1">
            <div class="title">
                Dokument:
                <asp:Label ID="o23Code_readonly" runat="server" CssClass="valbold" Style="padding-left: 10px;"></asp:Label>

            </div>
            <div class="content">
                <table cellpadding="3" cellspacing="2">
                    <tr>
                        <td style="width: 120px;">Typ dokumentu:
                        </td>
                        <td>
                            <asp:Label ID="o24Name_readonly" runat="server" CssClass="valbold"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td>Název:
                        </td>
                        <td>
                            <asp:Label ID="o23Name_readonly" runat="server" CssClass="valbold"></asp:Label>
                        </td>
                    </tr>
                </table>


                <uc:freefields_readonly ID="ff2" runat="server" />
            </div>

        </div>




        <asp:Panel ID="panBodyReadonly" runat="server" CssClass="content-box1">
            <div class="title">Podrobný popis</div>
            <div class="content" style="background-color: #ffffcc;">
                <asp:Label ID="o23BodyPlainText_readonly" runat="server" CssClass="val" Style="font-family: 'Courier New'; word-wrap: break-word; display: block; font-size: 120%;"></asp:Label>
            </div>
        </asp:Panel>
        <div class="content-box1">
            <div class="title">
                <img src="Images/attachment.png" style="margin-right: 10px;" />
                <asp:HyperLink ID="filesPreview_readonly" runat="server" Text="Souborové přílohy"></asp:HyperLink>

            </div>
            <div class="content">
                <uc:fileupload_list ID="Fileupload_list__readonly" runat="server" />
            </div>
        </div>


        <div class="div6">
            <asp:Label ID="Timestamp_readonly" runat="server" CssClass="timestamp"></asp:Label>
        </div>
    </asp:Panel>
    <asp:Panel ID="panRecord" runat="server">
        <telerik:RadTabStrip ID="RadTabStrip1" runat="server" MultiPageID="RadMultiPage1" ShowBaseLine="true">
            <Tabs>
                <telerik:RadTab Text="Vlastnosti" Selected="true" Value="core"></telerik:RadTab>
                <telerik:RadTab Text="Souborové přílohy" Value="files"></telerik:RadTab>
                <telerik:RadTab Text="Ostatní" Value="other"></telerik:RadTab>
            </Tabs>
        </telerik:RadTabStrip>

        <telerik:RadMultiPage ID="RadMultiPage1" runat="server">
            <telerik:RadPageView ID="core" runat="server" Selected="true">
                <table cellpadding="5" cellspacing="2" style="margin-top: 10px;">
                    <tr valign="top">

                        <td>
                            <asp:HyperLink ID="clue_o24" runat="server" CssClass="reczoom" Text="i" title="Detail typu dokumentu"></asp:HyperLink>
                            <span class="lblReq">Typ dokumentu:</span>
                        </td>
                        <td>
                            <uc:datacombo ID="o24ID" runat="server" DataTextField="o24Name" DataValueField="pid" AutoPostBack="true" IsFirstEmptyRow="true" Width="500px"></uc:datacombo>
                            <asp:CheckBox ID="o23IsDraft" runat="server" Text="DRAFT režim" Visible="false" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label ID="lblName" runat="server" CssClass="lbl" Text="Název:"></asp:Label>

                        </td>
                        <td>
                            <asp:TextBox ID="o23Name" runat="server" Style="width: 500px;"></asp:TextBox>

                        </td>

                    </tr>
                    <tr>
                        <td>
                            <asp:Label ID="lblDate" runat="server" CssClass="lblReq" Text="Datum:"></asp:Label>

                        </td>
                        <td>
                            <telerik:RadDateTimePicker ID="o23Date" runat="server" RenderMode="Lightweight" Width="160px" DateInput-EmptyMessage="Povinný údaj" SharedCalendarID="SharedCalendar">
                                <DateInput ID="DateInput1" DisplayDateFormat="d.M.yyyy HH:mm ddd" DateFormat="d.M.yyyy HH:mm ddd" runat="server"></DateInput>
                                <TimePopupButton Visible="false" />
                            </telerik:RadDateTimePicker>
                        </td>
                    </tr>
                </table>
                <asp:Label ID="lblMessage" runat="server" CssClass="infoNotification"></asp:Label>
                <asp:Panel ID="panP41" runat="server" Visible="false" CssClass="content-box2">
                    <div class="title">
                        <img src="Images/project.png" />
                        <asp:HyperLink ID="clue_p41" runat="server" CssClass="reczoom" Text="i" title="Detail projektu"></asp:HyperLink>
                        <span style="padding-left: 10px;">Projekt</span>

                    </div>
                    <div class="content">
                        <uc:project ID="p41ID" runat="server" Width="400px" AutoPostBack="true" />
                    </div>
                </asp:Panel>
                <asp:Panel ID="panP28" runat="server" Visible="false" CssClass="content-box2">
                    <div class="title">
                        <asp:HyperLink ID="clue_p28" runat="server" CssClass="reczoom" Text="i" title="Detail klienta"></asp:HyperLink>
                        <img src="Images/contact.png" /><span style="padding-left: 10px;">Klient</span>
                    </div>
                    <div class="content">
                        <uc:contact ID="p28ID" runat="server" Width="400px" AutoPostBack="true" />
                    </div>

                </asp:Panel>
                <asp:Panel ID="panJ02" runat="server" Visible="false" CssClass="content-box2">
                    <div class="title">
                        <asp:HyperLink ID="clue_j02" runat="server" CssClass="reczoom" Text="i" title="Detail osoby"></asp:HyperLink>
                        <img src="Images/person.png" /><span style="padding-left: 10px;">Osoba</span>
                    </div>
                    <div class="content">
                        <uc:person ID="j02ID" runat="server" Width="400px" Flag="all" AutoPostBack="true" />
                    </div>
                </asp:Panel>
                <asp:Panel ID="panP91" runat="server" Visible="false" CssClass="content-box2">
                    <div class="title">
                        <asp:HyperLink ID="clue_p91" runat="server" CssClass="reczoom" Text="i" title="Detail faktury"></asp:HyperLink>
                        <img src="Images/invoice.png" /><span style="padding-left: 10px;">Faktura</span>
                    </div>
                    <div class="content"></div>
                </asp:Panel>
                <asp:Panel ID="panP31" runat="server" Visible="false" CssClass="content-box2">
                    <div class="title">
                        <asp:HyperLink ID="clue_p31" runat="server" CssClass="reczoom" Text="i" title="Detail úkonu"></asp:HyperLink>
                        <img src="Images/worksheet.png" /><span style="padding-left: 10px;">Worksheet úkon</span>
                    </div>
                    <div class="content">
                        <asp:Label ID="p31_record" runat="server" CssClass="valboldblue"></asp:Label>
                        <asp:HiddenField ID="p31ID" runat="server" />
                        <asp:Button ID="cmdClearP31ID" runat="server" CssClass="cmd" Text="Vyčistit vazbu dokumentu na úkon" Visible="false" />
                    </div>
                </asp:Panel>
                <asp:Panel ID="panP56" runat="server" Visible="false" CssClass="content-box2">
                    <div class="title">
                        <asp:HyperLink ID="clue_p56" runat="server" CssClass="reczoom" Text="i" title="Detail úkolu"></asp:HyperLink>
                        <img src="Images/task.png" /><span style="padding-left: 10px;">Úkol</span>
                    </div>
                    <div class="content">
                        <asp:Label ID="p56_record" runat="server" CssClass="valboldblue"></asp:Label>
                        <asp:HiddenField ID="p56ID" runat="server" />
                    </div>
                </asp:Panel>

                <asp:Panel ID="panFF" runat="server" CssClass="content-box2">
                    <div class="title">
                        <img src="Images/form.png" />
                        <span style="padding-left: 10px;">Další pole dokumentu</span>
                    </div>
                    <div class="content">
                        <uc:freefields ID="ff1" runat="server" />
                    </div>
                </asp:Panel>
                <div class="content-box2">
                    <div class="title">Podrobný popis</div>
                    <div class="content">
                        <asp:TextBox ID="o23BodyPlainText" runat="server" TextMode="MultiLine" Style="width: 99%; height: 150px; font-family: 'Courier New';" ToolTip="Podrobný popis dokumentu"></asp:TextBox>
                    </div>
                </div>


            </telerik:RadPageView>

            <telerik:RadPageView ID="files" runat="server">
                <uc:fileupload ID="upload1" runat="server" EntityX29ID="o23Notepad" />
                <div style="height:40px;"></div>
                <uc:fileupload_list ID="uploadlist1" runat="server" />
                <div style="margin-top:20px;">
                    <asp:Label ID="lblFilesMessage" runat="server" CssClass="infoNotificationRed"></asp:Label>
                </div>
                
            </telerik:RadPageView>

            <telerik:RadPageView ID="other" runat="server">
                <table cellpadding="6">
                    <tr>
                        <td>Čas připomenutí:
                        </td>
                        <td>
                            <telerik:RadDateTimePicker ID="o23ReminderDate" runat="server" RenderMode="Lightweight" Width="180px" SharedCalendarID="SharedCalendar">
                                <DateInput ID="DateInput2" DisplayDateFormat="d.M.yyyy HH:mm ddd" DateFormat="d.M.yyyy HH:mm ddd" runat="server"></DateInput>
                                <TimePopupButton Visible="true" />                               
                            </telerik:RadDateTimePicker>
                        </td>
                    </tr>
                    <tr>
                        <td>Vlastník dokumentu:
                        </td>
                        <td>
                            <uc:person ID="j02ID_Owner" runat="server" Width="150px" Flag="all" />
                        </td>
                    </tr>
                </table>
                <div class="content-box2">
                    <div class="title">
                        <img src="Images/projectrole.png" width="16px" height="16px" />
                        <asp:Label ID="ph1" runat="server" Text="Příjemci (čtenáři) dokumentu"></asp:Label>
                        <asp:Button ID="cmdAddX69" runat="server" CssClass="cmd" Text="Přidat" />
                    </div>
                    <div class="content">
                        <uc:entityrole_assign ID="roles1" runat="server" EntityX29ID="o23Notepad"></uc:entityrole_assign>
                    </div>
                </div>

                <div class="div6">
                    <asp:CheckBox ID="o23IsEncrypted" runat="server" Text="Obsah zašifrovat a ochránit heslem" AutoPostBack="true" />
                </div>
                <asp:Panel ID="panPassword" runat="server" Style="padding: 10px;">
                    <table cellpadding="5">
                        <tr>
                            <td>
                                <asp:Label ID="lblPassword" runat="server" CssClass="lbl" Text="Heslo:"></asp:Label>
                            </td>
                            <td>
                                <asp:TextBox ID="o23password" runat="server" Style="width: 130px;" TextMode="Password"></asp:TextBox>
                            </td>
                            <td>
                                <asp:Label ID="lblVerify" runat="server" CssClass="lbl" Text="Ověření:"></asp:Label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtVerify" runat="server" Style="width: 130px;" TextMode="Password"></asp:TextBox>
                            </td>
                        </tr>
                    </table>
                </asp:Panel>
            </telerik:RadPageView>

        </telerik:RadMultiPage>

         <telerik:RadCalendar ID="SharedCalendar" runat="server" EnableMultiSelect="False" UseColumnHeadersAsSelectors="False" UseRowHeadersAsSelectors="False">
        
        <SpecialDays>
                    <telerik:RadCalendarDay Repeatable="Today" ItemStyle-BackColor="SkyBlue"></telerik:RadCalendarDay>
                </SpecialDays>
    </telerik:RadCalendar>











    </asp:Panel>

    <uc:b07_list ID="comments1" runat="server" JS_Create="comment_create()" JS_Reaction="comment_reaction" />
    <asp:HiddenField ID="hidMasterPID" runat="server" />
    <asp:HiddenField ID="hidMasterPrefix" runat="server" />
    <asp:HiddenField ID="hidMasterGUID" runat="server" />
    <asp:HiddenField ID="hidX29ID" runat="server" Value="0" />
    <asp:HiddenField ID="hidMode" runat="server" Value="1" />
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="FootContent" runat="server">
</asp:Content>
