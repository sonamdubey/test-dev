<%@ Page Language="C#" Inherits="Carwale.UI.MyCarwale.MyInvoice" trace="false" AutoEventWireup="false" %>
<%@ Register TagPrefix="Carwale" TagName="CarRating" src="/Controls/CarRating.ascx" %>
<%@ Import NameSpace="Carwale.UI.Common" %>
<!doctype html>
<html>
<head>
<%
	// Define all the necessary meta-tags info here.
	// To know what are the available parameters,
	// check page, headerCommon.aspx in common folder.
	
	PageId 			= 1;
	Title 			= "MyCarwale: My Invoice";
	Description 	= "";
	Keywords		= "";
	Revisit 		= "15";
	DocumentState 	= "Static";
    AdId            = "1337162297840";
    AdPath          = "/7590/CarWale_MyCarWale/CarWale_MyCarWale_Misc/CarWale_MyCarWale_Misc_";
%>
<!-- #include file="/includes/global/head-script.aspx" -->
<style type="text/css">
	.tbl th{font-weight:bold}
</style>
<script language="javascript">
	var mySideMenu = document.getElementById('myCarwaleMenu');
	var myContents = document.getElementById('myCarwaleContents');
	
	if ( mySideMenu && myContents )
	{
		mySideMenu.style.display = 'none';
		myContents.colSpan = 2;
	}
</script>
<style>
    .invoice-tbl th {text-align:left;}
</style>
</head>
<body class="bg-light-grey header-fixed-inner">
    <form runat="server">
    <!-- #include file="/includes/header.aspx" -->
    <section>
        <div class="container">
            <div class="grid-12">
                <div class="breadcrumb">
                    <!-- breadcrumb code starts here -->
                    <ul class="special-skin-text">
                        <li><a href="/">Home</a></li>
                        <li><span class="fa fa-angle-right margin-right10"></span><a href="/MyCarwale/default.aspx">My CarWale</a></li>
                        <li><span class="fa fa-angle-right margin-right10"></span><a href="/mycarwale/mypayments.aspx">My Payments</a></li>
                        <li><span class="fa fa-angle-right margin-right10"></span>My Invoice : INV-<%= Request.QueryString["inv"] %></li>
                    </ul>
                    <div class="clear"></div>
                    <h1 class="font30 text-black special-skin-text">Carwale.com Automotive Exchange Pvt. Ltd. INVOICE</h1>
                    <div class="border-solid-bottom margin-top10 margin-bottom15"></div>
                </div>
            </div>
            <div class="clear"></div>
        </div>
        <div class="container margin-bottom20">
            <div class="grid-12 margin-top10">
                <div class="content-box-shadow">    
                    <div class="content-inner-block-10">
                    <div>
                        <table class="tblDefault margin-top10 invoice-tbl" width="75%" cellpadding="3" align="center">
			                <tr>
				                <th width="160">Invoice No.</th>
				                <td>INV-<%= Request.QueryString["inv"] %></td>
			                </tr>
			                <tr>
				                <th>Invoice Date</th>
				                <td><%= entryDateTime%></td>
			                </tr>
			                <tr>
				                <th>Customer Name</th>
				                <td><%= consumerName%></td>
			                </tr>
			                <tr>
				                <th>Customer Email</th>
				                <td><%= consumerEmail%></td>
			                </tr>
			                <tr>
				                <th>Customer Address</th>
				                <td><%= consumerAddress%></td>
			                </tr>
			                <tr>
				                <th>Payment Mode</th>
				                <td><%= paymentModeDetails%></td>
			                </tr>
			                <tr>
				                <th>Amount</th>
				                <td>₹ <%= amount%>/-</td>
			                </tr>
			                <tr>
				                <th>Package</th>
				                <td><%= packageName%></td>
			                </tr>
			                <tr>
				                <th valign="top">Package Details</th>
				                <td><%= packageDetails%></td>
			                </tr>
			                <tr>
				                <td colspan="2" align="center">
					                <input type="button" onClick="javascript: print()" value="Print Invoice" class="doNotPrint">
				                </td>
			                </tr>	
		                </table>
                    </div>
                </div>
                </div>
            </div>
            <div class="clear"></div>
        </div>
    </section>
    <div class="clear"></div>
    <!-- #include file="/includes/footer.aspx" -->
    <!-- all other js plugins -->
    <!-- #include file="/includes/global/footer-script.aspx" -->
    </form>
</body>

