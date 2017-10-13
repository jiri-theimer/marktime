<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Clue.Master" CodeBehind="clue_popup.aspx.vb" Inherits="UI.clue_popup" %>

<%@ MasterType VirtualPath="~/Clue.Master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <style>
       
        
        /* Dropdown Content (Hidden by Default) */
        .dropdown-content {
            
            background-color:#f0f8ff;
            width:100%;
            
            z-index: 1;
        }

            /* Links inside the dropdown */
            .dropdown-content a {
                color: black;
                padding: 12px 16px;
                text-decoration: none;
                display: block;
            }

                /* Change color of dropdown links on hover */
                .dropdown-content a:hover {
                    background-color:#e0e0e0;
                }

       

       
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div style="max-height: 200px; width: 100%;">
        <div class="dropdown-content">
            <a href="#">Link 1</a>
            <a href="#">Link 2</a>
            <a href="#">Link 3</a>
        </div>


        <asp:Label ID="gogo1" runat="server"></asp:Label>


    </div>
</asp:Content>
