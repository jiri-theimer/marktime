<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="pokus_select2x.aspx.vb" Inherits="UI.pokus_select2x" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Strict//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-strict.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">

<head>
    <title >MARKTIME 5.0</title>
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <meta http-equiv="x-ua-compatible" content="IE=edge" />
    <meta name="viewport" content="width=device-width, initial-scale=1" />
    <link rel="stylesheet" href="~/Scripts/bootstrap/css/bootstrap.min.css" />
    <link rel="stylesheet" href="~/Styles/bootstrap_custom.css" />
    
   
    <link href="Scripts/select2/css/select2.min.css" rel="stylesheet" type="text/css" />

   

    <script src="Scripts/jquery-1.11.1.min.js" type="text/javascript"></script>
    <script src="Scripts/bootstrap/js/bootstrap.min.js" type="text/javascript"></script>
    

    <script src="Scripts/select2/js/select2.min.js" type="text/javascript"></script>
    


    <script type="text/javascript">
        $(document).ready(function () {
            $("#combo1").select2({
                placeholder: "Select a state",
                allowClear: true
            });

           
            $("#hledat1").select2({
                placeholder: "Search for a recipe",
                //minimumInputLength: 1,
                ajax: {
                    url: "/Handler/handler_search_project.ashx",
                    dataType: 'json',
                    
                    data: function (term, page) {
                        return {
                            languageId: 1,
                            orderBy: "TA"
                        };
                    },
                    results: function (data, page) {
                        alert(data.total);
                        var more = (page * 10) < data.total; // whether or not there are more results available

                        // notice we return the value of more so Select2 knows if more results can be loaded
                        return { results: data.recipes, more: more };
                    }
                }
                
            });

            
            




        });

       
       
    </script>


</head>
<body>
    <form runat="server" >
        <telerik:RadScriptManager ID="RadScriptManager1" runat="server" />
        <telerik:RadNotification ID="notify1" runat="server" RenderMode="Mobile" ShowCloseButton="true" Position="Center" Title="Info" EnableRoundedCorners="true" EnableShadow="true" Animation="Fade" Skin="BlackMetroTouch"></telerik:RadNotification>

       
        <h1>Select2 example:</h1>
    <input id="hledat1" type="text" />


            <hr />
        <div class="select2-container">
        <select id="combo1" class="select2-dropdown--above" >
  <option value="AL">Alabama</option>
  <option value="CZ">Czech</option>
  <option value="WY">Wyoming</option>
  <option value="AL">Aljaška</option>
</select>
        </div>
    </form>
</body>
</html>
