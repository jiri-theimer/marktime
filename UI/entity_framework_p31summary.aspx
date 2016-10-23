<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Clue.Master" CodeBehind="entity_framework_p31summary.aspx.vb" Inherits="UI.entity_framework_p31summary" %>

<%@ MasterType VirtualPath="~/Clue.Master" %>
<%@ Register TagPrefix="uc" TagName="periodcombo" Src="~/periodcombo.ascx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <script type="text/javascript">
        $(document).ready(function () {
            window.parent.stoploading();

        });
        function periodcombo_setting() {

            window.parent.sw_local("periodcombo_setting.aspx", "Images/settings_32.png");
        }
        function p31_bs_approve_all() {

            window.parent.parent.sw_master("p31_approving_step1.aspx?masterprefix=<%=Me.CurrentMasterPrefix%>&masterpid=<%=Me.CurrentMasterPID%>&datefrom=<%=Format(period1.DateFrom, "dd.MM.yyyy")%>&dateuntil=<%=Format(period1.DateUntil,"dd.MM.yyyy")%>", "Images/approve_32.png", true);
        }

        function p31_bs_reapprove_all() {

            window.parent.parent.sw_master("p31_approving_step1.aspx?reapprove=1&masterprefix=<%=Me.CurrentMasterPrefix%>&masterpid=<%=Me.CurrentMasterPID%>&datefrom=<%=Format(period1.DateFrom, "dd.MM.yyyy")%>&dateuntil=<%=Format(period1.DateUntil,"dd.MM.yyyy")%>", "Images/approve_32.png", true);
        }
        function p31_bs_clearapprove_all() {

            window.parent.parent.sw_master("p31_approving_step1.aspx?clearapprove=1&masterprefix=<%=Me.CurrentMasterPrefix%>&masterpid=<%=Me.CurrentMasterPID%>&datefrom=<%=Format(period1.DateFrom, "dd.MM.yyyy")%>&dateuntil=<%=Format(period1.DateUntil,"dd.MM.yyyy")%>", "Images/clear_32.png", true);
        }
        function p31_bs_invoice() {

            window.parent.parent.sw_master("p91_create_step1.aspx?nogateway=1&prefix=<%=Me.CurrentMasterPrefix%>&pid=<%=Me.CurrentMasterPID%>", "Images/invoice_32.png", true);
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="div6">
        <uc:periodcombo ID="period1" runat="server" Width="300px"></uc:periodcombo>
        <p></p>

        <table border="1" class="tabulka" cellpadding="4" cellspacing="2">
            <tr>
                <td colspan="7" style="text-align: center; background-color: ThreeDFace;">Rozpracováno (čeká na schvalování)</td>
            </tr>
            <tr style="font-weight: bold;">
                <td></td>
                <td style="text-align: right;">Hodiny</td>
                <td style="text-align: right;">Výdaje</td>
                <td style="text-align: right;">Odměny</td>
                <td style="text-align: right;">Celkem</td>
                <td style="text-align: center;">#</td>
                <td></td>

            </tr>
            <asp:Repeater ID="rpWaiting" runat="server">
                <ItemTemplate>
                    <tr>
                        <td>
                            <asp:Label ID="rozpracovano_j27Code" runat="server" Font-Bold="true"></asp:Label>
                        </td>
                        <td style="text-align: right;">
                            <asp:Label ID="rozpracovano_hodiny" runat="server"></asp:Label>

                        </td>
                        <td style="text-align: right;">
                            <asp:Label ID="rozpracovano_vydaje" runat="server"></asp:Label>
                        </td>
                        <td style="text-align: right;">
                            <asp:Label ID="rozpracovano_odmeny" runat="server"></asp:Label>

                        </td>
                        <td style="text-align: right;">
                            <asp:Label ID="rozpracovano_celkem" runat="server"></asp:Label>

                        </td>

                        <td>
                            <asp:Label ID="rozpracovano_pocet" CssClass="badgebox1red" runat="server"></asp:Label>
                            <asp:Label ID="rozpracovano_obdobi" runat="server"></asp:Label>

                        </td>
                        <td>
                            <asp:HyperLink ID="cmdApprove" runat="server" Text="Schválit (připravit k fakturaci)" NavigateUrl="javascript:p31_bs_approve_all()"></asp:HyperLink>


                        </td>
                    </tr>

                </ItemTemplate>
            </asp:Repeater>
        </table>



        <asp:Panel ID="panApproved" runat="server" style="padding-top:10px;">
            <table border="1" class="tabulka" cellpadding="4" cellspacing="2">
                <tr>
                    <td colspan="7" style="text-align: center; background-color: ThreeDFace;">Schváleno | Čeká na fakturaci</td>
                </tr>
                <tr style="font-weight: bold;">
                    <td></td>
                    <td style="text-align: right;">Hodiny</td>
                    <td style="text-align: right;">Výdaje</td>
                    <td style="text-align: right;">Odměny</td>
                    <td style="text-align: right;">Celkem</td>
                    <td style="text-align: center;">#</td>
                    <td></td>

                </tr>
                <asp:Repeater ID="rpApproved" runat="server">
                    <ItemTemplate>


                        <tr>
                            <td>
                                <asp:Label ID="schvaleno_j27Code" runat="server" Font-Bold="true"></asp:Label>
                            </td>
                            <td style="text-align: right;">
                                <asp:Label ID="schvaleno_hodiny" runat="server"></asp:Label>

                            </td>
                            <td style="text-align: right;">
                                <asp:Label ID="schvaleno_vydaje" runat="server"></asp:Label>
                            </td>
                            <td style="text-align: right;">
                                <asp:Label ID="schvaleno_odmeny" runat="server"></asp:Label>

                            </td>
                            <td style="text-align: right;">
                                <asp:Label ID="schvaleno_celkem" runat="server"></asp:Label>

                            </td>
                            <td style="text-align: right;">
                                <asp:Label ID="schvaleno_pocet" ForeColor="red" runat="server"></asp:Label>
                                <asp:Label ID="schvaleno_obdobi" runat="server"></asp:Label>

                            </td>
                            <td>
                                <asp:HyperLink ID="cmdReApprove" runat="server" Text="Pře-schválit" NavigateUrl="javascript:p31_bs_reapprove_all()"></asp:HyperLink>
                                <asp:HyperLink ID="cmdClearApprove" runat="server" Text="Vrátit do rozpracovanosti" NavigateUrl="javascript:p31_bs_clearapprove_all()" ForeColor="DarkOrange"></asp:HyperLink>
                                <asp:HyperLink ID="cmdInvoice" runat="server" Text="Vyfakturovat" NavigateUrl="javascript:p31_bs_invoice()" ForeColor="green"></asp:HyperLink>


                            </td>

                        </tr>
                    </ItemTemplate>
                </asp:Repeater>
            </table>
        </asp:Panel>
    </div>


    <asp:HiddenField ID="hidIsApprovingPerson" runat="server" />
    <asp:HiddenField ID="hidIsInvoicingPerson" runat="server" />

    <asp:HiddenField ID="hidMasterPrefix" runat="server" />
    <asp:HiddenField ID="hidMasterPID" runat="server" />
</asp:Content>
