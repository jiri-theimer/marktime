<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/ModalForm.Master" CodeBehind="capacity_plan_entry.aspx.vb" Inherits="UI.capacity_plan_entry" %>

<%@ MasterType VirtualPath="~/ModalForm.Master" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<%@ Register TagPrefix="uc" TagName="pageheader" Src="~/PageHeader.ascx" %>


<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <script type="text/javascript">
        $(document).ready(function () {
            $(".slidingDiv1").hide();
            $(".show_hide1").show();

            $('.show_hide1').click(function () {
                $(".slidingDiv1").slideToggle();
            });

            

        });


        
        function plan_item(j02id)
        {
            dialog_master("capacity_plan_entry_item.aspx?guid=<%=ViewState("guid")%>&pid=" + j02id + "&p41id=<%=master.datapid%>", false, "400", "500");


        }
        function hardrefresh(pid, flag) {
            <%=Me.ClientScript.GetPostBackEventReference(Me.cmdHardRefreshOnBehind, "", False)%>

        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="OverMainContent" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="server">
   
    <table>
        <tr>

            
            <td>
                <button type="button" class="show_hide1" id="cmdAddPerson" runat="server">Přidat osobu do plánu<img src="Images/arrow_down.gif" /></button>
            </td>
            <td>
                <asp:RadioButtonList ID="opgPageIndex" runat="server" AutoPostBack="true" RepeatDirection="Horizontal"></asp:RadioButtonList>
            </td>
           <td>
                <asp:Label ID="lblHeader" runat="server" CssClass="valboldblue"></asp:Label>
            </td>
            
        </tr>
    </table>
    <div class="slidingDiv1">
        <div class="content-box2">
            <div class="title">
                Osoby s rolí v projektu
            </div>
            <div class="content">
                <asp:Repeater ID="rpJ02" runat="server">
                    <ItemTemplate>
                        <div class="div6">
                            <asp:HyperLink ID="clue_person" runat="server" CssClass="reczoom" Text="i" title="Kapacita osoby"></asp:HyperLink>
                            
                            <asp:label ID="Person" runat="server"></asp:label>
                            <asp:HyperLink ID="cmdInsert" runat="server" Text="Vložit" NavigateUrl="javascript:add()"></asp:HyperLink>
                            <asp:HiddenField ID="hidJ02ID" runat="server" />
                        </div>
                    </ItemTemplate>
                </asp:Repeater>
            </div>
        </div>

    </div>

    <asp:Panel ID="panMatrix" runat="server">
        <table cellpadding="10">
            <tr id="trHeader" runat="server">
                <th></th>
                <th>
                    <asp:label ID="Person" runat="server"></asp:label>

                </th>
               
                <th colspan="2" >
                    <asp:Label ID="m1" runat="server"></asp:Label></th>
                <th colspan="2">
                    <asp:Label ID="m2" runat="server"></asp:Label></th>
                <th colspan="2" >
                    <asp:Label ID="m3" runat="server"></asp:Label></th>
                <th colspan="2">
                    <asp:Label ID="m4" runat="server"></asp:Label></th>
                <th colspan="2" >
                    <asp:Label ID="m5" runat="server"></asp:Label></th>
                <th colspan="2">
                    <asp:Label ID="m6" runat="server"></asp:Label></th>
                <th colspan="2" >
                    <asp:Label ID="m7" runat="server"></asp:Label></th>
                <th colspan="2">
                    <asp:Label ID="m8" runat="server"></asp:Label></th>
                <th colspan="2" >
                    <asp:Label ID="m9" runat="server"></asp:Label></th>
                <th colspan="2">
                    <asp:Label ID="m10" runat="server"></asp:Label></th>
                <th colspan="2" >
                    <asp:Label ID="m11" runat="server"></asp:Label></th>
                <th colspan="2">
                    <asp:Label ID="m12" runat="server"></asp:Label></th>
                <th colspan="3">
                    <img src="Images/sum.png" />
                </th>
            </tr>
            <asp:Repeater ID="rp1" runat="server">
                <ItemTemplate>
                    <tr class="trHover">
                        <td>
                            <asp:ImageButton ID="cmdDelete" runat="server" ImageUrl="Images/delete.png" CommandName="delete" ToolTip="Odstranit osobu z plánu" />
                        </td>
                        <td>
                            <asp:HyperLink ID="clue_person" runat="server" CssClass="reczoom" Text="i" title="Kapacita osoby"></asp:HyperLink>
                            <asp:hyperlink ID="Person" runat="server"></asp:hyperlink>
                            
                            <asp:HiddenField ID="hidJ02ID" runat="server" />
                        </td>
                       
                        <td >
                            <asp:label ID="m1a" runat="server"></asp:label>
                        </td>
                        <td align="right" >
                            <asp:label ID="m1c" runat="server"></asp:label>
                        </td>
                        <td >
                            <asp:label ID="m2a" runat="server">
                        
                            </asp:label>
                        </td>
                        
                        <td align="right">
                            <asp:label ID="m2c" runat="server"></asp:label>
                        </td>
                        <td >
                            <asp:label ID="m3a" runat="server">
                        
                            </asp:label>
                        </td>
                       
                        <td align="right" >
                            <asp:label ID="m3c" runat="server"></asp:label>
                        </td>
                        <td>
                            <asp:label ID="m4a" runat="server">
                        
                            </asp:label>
                        </td>
                        
                        <td>
                            <asp:label ID="m4c" runat="server"></asp:label>
                        </td>
                        <td >
                            <asp:label ID="m5a" runat="server">
                        
                            </asp:label>
                        </td>
                      
                        <td align="right" >
                            <asp:label ID="m5c" runat="server"></asp:label>
                        </td>
                        <td>
                            <asp:label ID="m6a" runat="server">
                        
                            </asp:label>
                        </td>
                        
                        <td align="right">
                            <asp:label ID="m6c" runat="server"></asp:label>
                        </td>
                        <td >
                            <asp:label ID="m7a" runat="server">
                        
                            </asp:label>
                        </td>
                       
                        <td align="right" >
                            <asp:label ID="m7c" runat="server"></asp:label>
                        </td>
                        <td>
                            <asp:label ID="m8a" runat="server">
                        
                            </asp:label>
                        </td>
                        
                        <td align="right">
                            <asp:label ID="m8c" runat="server"></asp:label>
                        </td>
                        <td >
                            <asp:label ID="m9a" runat="server">
                        
                            </asp:label>
                        </td>
                        
                        <td align="right" >
                            <asp:label ID="m9c" runat="server"></asp:label>
                        </td>
                        <td>
                            <asp:label ID="m10a" runat="server">
                        
                            </asp:label>
                        </td>
                     
                        <td align="right">
                            <asp:label ID="m10c" runat="server"></asp:label>
                        </td>
                        <td >
                            <asp:label ID="m11a" runat="server">
                        
                            </asp:label>
                        </td>
                        
                        <td align="right" >
                            <asp:label ID="m11c" runat="server"></asp:label>
                        </td>
                        <td>
                            <asp:label ID="m12a" runat="server">
                        
                            </asp:label>
                        </td>
                       
                        <td align="right">
                            <asp:label ID="m12c" runat="server"></asp:label>
                        </td>
                        <td align="right">
                            <asp:Label ID="totala" runat="server" BackColor="#98FB98"></asp:Label>
                        </td>
                        <td align="right" >
                            <asp:Label ID="totalb" runat="server" BackColor="#FFA07A"></asp:Label>
                        </td>
                        <td align="right">
                            <asp:Label ID="totalc" runat="server"></asp:Label>
                        </td>
                    </tr>
                </ItemTemplate>
            </asp:Repeater>
            <tr id="trFooter" runat="server" class="trHover" style="font-weight: bold; text-align: right;">
                <td></td>
                <td>
                    <img src="Images/sum.png" />

                </td>
                
                <td >
                    <asp:Label ID="t1a" runat="server"></asp:Label>
                </td>
              
                <td align="right" >
                    <asp:Label ID="t1c" runat="server"></asp:Label>
                </td>
                <td>
                    <asp:Label ID="t2a" runat="server">
                        
                    </asp:Label>
                </td>
               
                <td align="right">
                    <asp:Label ID="t2c" runat="server"></asp:Label>
                </td>
                <td >
                    <asp:Label ID="t3a" runat="server">
                        
                    </asp:Label>
                </td>
                
                <td align="right" >
                    <asp:Label ID="t3c" runat="server"></asp:Label>
                </td>
                <td>
                    <asp:Label ID="t4a" runat="server">
                        
                    </asp:Label>
                </td>
              
                <td align="right">
                    <asp:Label ID="t4c" runat="server"></asp:Label>
                </td>
                <td >
                    <asp:Label ID="t5a" runat="server">
                        
                    </asp:Label>
                </td>
            
                <td align="right" >
                    <asp:Label ID="t5c" runat="server"></asp:Label>
                </td>
                <td>
                    <asp:Label ID="t6a" runat="server">
                        
                    </asp:Label>
                </td>
                
                <td align="right">
                    <asp:Label ID="t6c" runat="server"></asp:Label>
                </td>
                <td >
                    <asp:Label ID="t7a" runat="server">
                        
                    </asp:Label>
                </td>
                
                <td align="right" >
                    <asp:Label ID="t7c" runat="server"></asp:Label>
                </td>
                <td>
                    <asp:Label ID="t8a" runat="server">
                        
                    </asp:Label>
                </td>
                
                <td align="right">
                    <asp:Label ID="t8c" runat="server"></asp:Label>
                </td>
                <td >
                    <asp:Label ID="t9a" runat="server">
                        
                    </asp:Label>
                </td>
               
                <td align="right" >
                    <asp:Label ID="t9c" runat="server"></asp:Label>
                </td>
                <td>
                    <asp:Label ID="t10a" runat="server">
                        
                    </asp:Label>
                </td>
               
                <td align="right">
                    <asp:Label ID="t10c" runat="server"></asp:Label>
                </td>
                <td >
                    <asp:Label ID="t11a" runat="server">
                        
                    </asp:Label>
                </td>
              
                <td align="right" >
                    <asp:Label ID="t11c" runat="server"></asp:Label>
                </td>
                <td>
                    <asp:Label ID="t12a" runat="server">
                        
                    </asp:Label>
                </td>
               
                <td align="right">
                    <asp:Label ID="t12c" runat="server"></asp:Label>
                </td>
                <td>
                    <asp:Label ID="totala" runat="server" BackColor="#98FB98"></asp:Label>
                </td>
                <td>
                    <asp:Label ID="totalb" runat="server" BackColor="#FFA07A"></asp:Label>
                </td>
                <td>
                    <asp:Label ID="totalc" runat="server"></asp:Label>
                </td>
            </tr>



        </table>
    </asp:Panel>

    <asp:HiddenField ID="hidPrefix" runat="server" />
    <asp:HiddenField ID="hidLimD1" runat="server" />
    <asp:HiddenField ID="hidLimD2" runat="server" />
    <asp:HiddenField ID="hidCurrentPageIndex" runat="server" Value="0" />
    <asp:Button ID="cmdHardRefreshOnBehind" runat="server" Style="display: none;" />
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="FootContent" runat="server">
</asp:Content>
