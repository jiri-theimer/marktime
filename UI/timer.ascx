<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="timer.ascx.vb" Inherits="UI.timer" %>
<%@ Register TagPrefix="uc" TagName="project" Src="~/project.ascx" %>

<script type="text/javascript">
    <%For Each ri As RepeaterItem In rp1.Items%>
    <%If CType(ri.FindControl("isrunning"), TextBox).Text = "true" Then%>
    var saldo_<%=ri.FindControl("p85id").ClientID%>=<%=CType(ri.FindControl("CurrentDuration"), TextBox).Text%>;
    var run_<%=ri.FindControl("p85id").ClientID%> = new (function () {
        var $stopwatch, // Stopwatch element on the page
            incrementTime = 100, // Timer speed in milliseconds
            currentTime = 0, // Current time in hundredths of a second
            updateTimer = function () {
                var strD = "<%=CType(ri.FindControl("DateLastDuration"), TextBox).Text%>";
                
                var d = ConvertStringToDate(strD);
                secs = GetDur(d);
                currentTime = saldo_<%=ri.FindControl("p85id").ClientID%>+parseInt(secs * 100);

                $stopwatch.html(formatTime(currentTime));
                
                document.getElementById("<%=ri.FindControl("CurrentDuration").ClientID%>").value = currentTime;
                
            },
            init = function () {
                $stopwatch = $('#<%=ri.FindControl("timer").ClientID%>');
                
                run_<%=ri.FindControl("p85id").ClientID%>.Timer = $.timer(updateTimer, incrementTime, true);
            };
        this.resetStopwatch = function () {
            currentTime = 0;
            this.Timer.stop().once();
        };
        $(init);
    });
    <%End If%>
    <%next%>


    // Common functions
    function pad(number, length) {
        var str = '' + number;
        while (str.length < length) { str = '0' + str; }
        return str;
    }
    
    function formatTime(time) {
        var hrs=parseInt(time / 6000/60)
        var min = parseInt(time / 6000) - (hrs*60);
        var sec = parseInt(time / 100) - (min * 60)-(hrs*60*60);
        var hundredths = pad(time - (sec * 100) - (min * 6000)-(hrs*60*6000), 2);

        return pad(hrs, 2) +":"+ pad(min, 2) + ":" + pad(sec, 2) + ":" + hundredths;
        
    }

   

    
    function ConvertStringToDate(strD) {
        
        var a = strD.split("-");
        
        var year = new Number(a[2]);        
        var month = new Number(a[1]);
        var day = new Number(a[0]);
        var hrs = new Number(a[3]);
        var min = new Number(a[4]);
        var sec = new Number(a[5]);
        

        var d = new Date(year, month - 1, day, hrs,min, sec, 0);
        return d;
    }

    function GetDur(d1) {
        var d2 = new Date()

        var dif = d2.getTime() - d1.getTime();
        var Seconds_from_T1_to_T2 = dif / 1000;
        var Seconds_Between_Dates = Math.abs(Seconds_from_T1_to_T2);

        return Seconds_Between_Dates;
       
    }

    function ChangeTimerMode() {
        
        var val=document.getElementById("<%=me.cbxTimerMode.ClientID%>").value;

        $.post("Handler/handler_userparam.ashx", { x36value: val, x36key: "timer-mode", oper: "set" }, function (data) {               
            if (data == ' ') {
                return;
            }


        });
            
            
    }

    function SaveP31Text(pid,txt) {
        $.post("Handler/handler_tempbox.ashx", {p85id: pid, value: txt.value, field: "p85Message", oper: "save" }, function (data) {               
            if (data == ' ') {
                alert("Chyba v uložení podrobného popisu.")
                return;
            }


        });
         
    }

    function ProjectChanged(sender, eventArgs){
        var item = eventArgs.get_item();
        var p41id=item.get_value();
        var pid=sender.get_attributes().getAttribute("pid");
        
        $.post("Handler/handler_tempbox.ashx", {p85id: pid, value: p41id, field: "p85OtherKey1", oper: "save" }, function (data) {             
            if (data == ' ') {
                alert("Chyba v uložení vazby na projekt.")
                return;
            }
            

        });
         
    }

    function p31_save(p85id) {
        ///volá se z p31_subgrid
        sw_master("p31_record.aspx?p85id="+p85id,"Images/worksheet_32.png",false);
        return(false);

    }
</script>

<div class="div6">
    <table style="width:100%;">
        <tr>
            <td style="width:100px;">
                 <asp:Button ID="cmdAddRow" runat="server" CssClass="cmd" Text="Přidat" />
            </td>
            <td style="width:100px;">
                <asp:Button ID="cmdClear" runat="server" CssClass="cmd" Text="Vyčistit" />
            </td>
            <td></td>
            <td style="text-align:right;">
                <asp:DropDownList ID="cbxTimerMode" runat="server" onchange="ChangeTimerMode()">
                    <asp:ListItem Text="Povolit spuštění více časovačů souběžně" Value="1"></asp:ListItem>
                    <asp:ListItem Text="V jednom okamžiku pouze jeden spuštěný časovač" Value="2"></asp:ListItem>
                   
                </asp:DropDownList>
                
            </td>
        </tr>
    </table>
   
    
</div>

<asp:Panel ID="panContainer" runat="server">
<table cellpadding="4">
    <tr>
        <th style="width:23px;"></th>
        <th></th>
        <th>Projekt</th>
        
       
        <th>Popis</th>
        <th></th>
    </tr>
    <asp:Repeater ID="rp1" runat="server">
        <ItemTemplate>
            <tr style="vertical-align:top;">
                <td>
                    <asp:ImageButton ID="cmdFinalSave" runat="server" ImageUrl="Images/save.png" CssClass="button-link" />
                </td>
                <td>
                    <asp:Label ID="RowIndex" runat="server"></asp:Label>
                </td>
                <td>
                    <uc:project ID="p41ID" runat="server" Width="400px" AutoPostBack="false" Flag="p31_entry" />
                    <div style="padding-top:4px;">
                        
                        <asp:label ID="timer" runat="server" style="font-weight:bold;" Text="00:00:00" Width="70px"></asp:label>
                        
                        <asp:ImageButton ID="cmdStart" runat="server" ImageUrl="Images/timer_start_24.png" ToolTip="Start" CommandName="start" />
                        <asp:ImageButton ID="cmdPause" runat="server" ImageUrl="Images/timer_pause_24.png" ToolTip="Pozastavit" CommandName="pause" />
                        <span style="margin-left:20px;"></span>
                        <asp:ImageButton ID="cmdReset" runat="server" ImageUrl="Images/timer_reset_24.png" ToolTip="Zastavit a vynulovat"  CommandName="reset"/>
                        
                    </div>
                </td>
           
                
                <td>
                    <asp:TextBox ID="p31Text" runat="server" TextMode="MultiLine" style="width:450px;height:60px;"></asp:TextBox>
                </td>
                <td>
                    <asp:ImageButton ID="del" runat="server" ImageUrl="Images/delete_row.png" ToolTip="Odstranit položku" CssClass="button-link" CommandName="delete" />
                    <asp:HiddenField ID="p85id" runat="server" />                   
                    <asp:TextBox ID="isrunning" runat="server" value="false" style="display:none;"></asp:TextBox>
                    <asp:TextBox ID="CurrentDuration" runat="server" value="0" style="display:none;"></asp:TextBox>
                    <asp:TextBox ID="DateLastDuration" runat="server" Width="400px" style="display:none;"></asp:TextBox>
                </td>
            </tr>
        </ItemTemplate>
    </asp:Repeater>
</table>
   
</asp:Panel>