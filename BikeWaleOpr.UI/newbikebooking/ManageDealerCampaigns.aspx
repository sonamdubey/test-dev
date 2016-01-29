<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ManageDealerCampaigns.aspx.cs" Inherits="BikewaleOpr.newbikebooking.ManageDealerCampaigns" %>
<!-- #Include file="/includes/headerNew.aspx" -->
<%--<script type="text/javascript" src="/src/common/common.js?V1.1"></script>--%>
<script type="text/ecmascript" src="/src/AjaxFunctions.js"></script>
<script src="/src/knockout.js" type="text/javascript"></script>
<style>
    .dtItem{border-bottom:1px solid #808080;}
    select { padding:10px; cursor:pointer;}
    .footer {margin-top:20px;}
    .top_info_left { text-transform:capitalize; }
    .dtItem {font-size:larger;}
</style>
<div>
    <div>
        <h3> Manage Manufacturer's Campaigns</h3>
        <hr />
        <div class="margin-top10">
            <asp:DropDownList ID="ddlMake" runat="server"><asp:ListItem Value="0" Text="--Selected Make--" ></asp:ListItem></asp:DropDownList>
            <select id="ddlModel"><option value="0">--Selected Model--</option></select>
            <input type="button" id="btnGetPriceQuote" value="Get Dealer Price Quote" style="padding:10px; margin-left:20px; cursor:pointer;"/>
        </div> 
        <hr />      
    </div>
    </div>
<!-- #Include file="/includes/footerNew.aspx" -->
