<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="pokus_dock.aspx.vb" Inherits="UI.pokus_dock" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>

    <script type="text/javascript">
        function OnClientItemSelected(sender, args) {
            if (!args.get_item().isDirectory())
                previewImage(args.get_path());
        }
        function OnClientDelete(explorer, args) {
            previewImage("");
        }

        function previewImage(src) {
            var isImageSrc = src.match(/\.(gif|jpg|png)$/gi);

            var pvwImage = $get("pvwImage");
            pvwImage.src = src;
            pvwImage.style.display = isImageSrc ? "inline-block" : "none";
            pvwImage.alt = src.substring(src.lastIndexOf('/') + 1);
        }
    </script>
</head>
<body>
    <form id="form1" runat="server">
        <telerik:RadScriptManager ID="RadScriptManager1" runat="server" />
      

         <telerik:RadFileExplorer RenderMode="Lightweight" Skin="MetroTouch" runat="server" ID="FileExplorer1" Width="100%" Height="310px"
                OnClientItemSelected="OnClientItemSelected" OnClientDelete="OnClientDelete">
                <Configuration ViewPaths="~/public/images" UploadPaths="~/public/images" MaxUploadFileSize="1000000"
                    DeletePaths="~/public/images/deleted"></Configuration>
            </telerik:RadFileExplorer>



        <fieldset class="previmage">
                <legend>Preview</legend>
                <img id="pvwImage" src="Images/Northwind/Flowers/Flower1.jpg" alt="Flower1.jpg" />
            </fieldset>



        <embed src="http://stroj02.net/vozab/public/images/b01_record.PNG">
    </form>
</body>
</html>
