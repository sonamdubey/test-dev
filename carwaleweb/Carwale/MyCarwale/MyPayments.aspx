<%@ Page Language="C#" Inherits="Carwale.UI.MyCarwale.MyPayments" trace="false" AutoEventWireup="false" %>
<%@ Register TagPrefix="Carwale" TagName="CarRating" src="/Controls/CarRating.ascx" %>
<%@ Import Namespace="Carwale.UI.Common" %>

<%
	// Define all the necessary meta-tags info here.
	// To know what are the available parameters,
	// check page, headerCommon.aspx in common folder.
	
	PageId 			= 1;
	Title 			= "MyCarwale: My Payments";
	Description 	= "";
	Keywords		= "";
	Revisit 		= "15";
	DocumentState 	= "Static";
    AdId            = "1337162297840";
    AdPath          = "/7590/CarWale_MyCarWale/CarWale_MyCarWale_Misc/CarWale_MyCarWale_Misc_";
%>
<!doctype html>
<html>
    <head>
    <!-- #include file="/includes/global/head-script.aspx" -->
    <style type="text/css">
	    .bdr {border:1px #D5D5D5 solid;}
	    .bdr td{border-right:1px #D5D5D5 solid;}
	    .bdr td{border-bottom:1px #D5D5D5 solid;}
	
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
    </head>
    <body  class="bg-light-grey">
        <form id="Form1" runat="server">
            <!-- #include file="/includes/header.aspx" -->
            <section class="margin-top70">
            <div class="container">
                <div class="grid-12">
                <ul class="breadcrumb margin-top10">
                    <li>You are here: </li>
                    <li><a href="/">Home</a></li>
                    <li><span class="fa fa-angle-right margin-right10"/></li>
                    <li><a href="/MyCarwale/default.aspx">My CarWale</a></li>
                    <li><span class="fa fa-angle-right margin-right10"/></li>
                    <li class="current"><strong> My Payments</strong></li>
                </ul>
                <div class="clear"></div>
                <h1 class="content-inner-block">Payment(s) made by you to Carwale.com</h1>
                    <div class="border-solid-bottom margin-top10 margin-bottom15"></div>
                </div>
            </div>
            </section>
            <div class="clear"></div>
            <section>
                <div class="container">
                <div class="grid-12">    
                <div class="content-box-shadow content-inner-block-10 rounded-corner2 margin-bottom20">
                    
                        <asp:Label ID="lblMessage" runat="server" EnableViewState="false" CssClass="error" />
                        <div class="Repeator">&nbsp;</div>	
			            <div class="RedBarMid" style="min-height:120px;">
				            <asp:Repeater ID="rptPayments" runat="server">
					            <headertemplate>
						            <table width="100%" class="bdr" cellspacing="0" cellpadding="5">
							            <tr class="dtHeader">
								            <td width="20">S.No.</td>
								            <td>Package Name</td>
								            <td align="right" width="60">Amount(₹)</td>
								            <td width="60">Date</td>
								            <td width="80">Invoice No.</td>
							            </tr>
					            </headertemplate>
						            <itemtemplate>
							            <tr>
								            <td><%# ++serial%></td>
								            <td><%# DataBinder.Eval(Container.DataItem, "Package")%></td>
								            <td align="right"><%# CommonOpn.FormatNumeric(DataBinder.Eval(Container.DataItem, "ActualAmount").ToString())%></td>
								            <td><%# Convert.ToDateTime(DataBinder.Eval(Container.DataItem, "EntryDate")).ToString("dd-MMM,yyyy") %></td>
								            <td>
									            <a href='MyInvoice.aspx?inv=<%# DataBinder.Eval(Container.DataItem, "Invoice")%>'>
										            INV-<%# DataBinder.Eval(Container.DataItem, "Invoice")%>
									            </a>
								            </td>
							            </tr>
						            </itemtemplate>
					            <footertemplate>
						            </table>
					            </footertemplate>
				            </asp:Repeater>
			            </div>		
                </div>
            </div>
                </div>
            </section>
            <div class="clear"></div>
            <!-- #include file="/includes/footer.aspx" -->
            <!-- #include file="/includes/global/footer-script.aspx" -->
        </form>
        <!-- Footer ends here -->
    </body>
</html>
