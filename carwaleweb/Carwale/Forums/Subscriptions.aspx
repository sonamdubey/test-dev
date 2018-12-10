<%@ Page Language="C#" Inherits="Carwale.UI.Forums.Subscriptions" trace="false" AutoEventWireup="false" %>
<%@ Import Namespace="Carwale.UI.Forums" %>
<!doctype html>
<html itemscope itemtype="http://schema.org/WebPage">
<head>
<!-- #include file="/includes/global/head-script.aspx" -->
<%
	// Define all the necessary meta-tags info here.
	// To know what are the available parameters,
	// check page, headerCommon.aspx in common folder.
	
	PageId 			= 305;
	Title 			= "My Forum Subscriptions | CarWale Forums";
	Description 	= "";
	Keywords		= "";
	Revisit 		= "15";
	DocumentState 	= "Static";
    noIndex         = true;
    AdId            = "1397024466973";
    AdPath          = "/1017752/Carwale_Forums_";
    noIndex = true;
%>
<script type='text/javascript'>
googletag.cmd.push(function () {
    googletag.defineSlot('<%= AdPath %>970x90', [[220, 90], [728, 90], [950, 90], [960, 90], [970, 66], [970, 90]], 'div-gpt-ad-<%= AdId %>-2').addService(googletag.pubads());
    googletag.defineSlot('<%= AdPath %>160X600', [[120, 240], [120, 600], [160, 600]], 'div-gpt-ad-<%= AdId %>-4').addService(googletag.pubads());

    //googletag.pubads().enableSyncRendering();
    googletag.pubads().collapseEmptyDivs();
    googletag.pubads().enableSingleRequest();
    googletag.enableServices();
});
</script>
<link rel="stylesheet" href="/static/css/forums.css" type="text/css" >
<style>
.footerStrip { background-color:#FFFFD9;border:#FFFF79 1px solid; padding:5px; }
.footerStrip, .footerStrip a, .footerStrip a:link, .footerStrip a:visited, .footerStrip a:active { font-weight:bold; }
.ac {padding:3px;}
.iac {padding:3px;}
.message { padding:8px; margin:5px; background:#E9FEEA; color:#006231; font-size:13px; font-weight:bold; border:1px solid #A9D5A8; width:350px; }
</style>
</head>
<body class="bg-white header-fixed-inner special-page special-skin-body no-bg-color">
<form runat="server">
    <!-- #include file="/includes/header.aspx" -->
<section class="container">
	<div class="grid-12">
    	<div class="padding-bottom10 padding-top10 text-center">
        	<%= Carwale.UI.HtmlHelpers.Helpers.GetAdBarForAspx(AdId, 0, 90, 0, 0, true, 2) %>
        </div>
    </div>
</section>
<section class="bg-light-grey padding-top10 padding-bottom20 no-bg-color">
<div class="container">
<div class="grid-12" id="youHere" >
<div class="breadcrumb">
    <ul class="special-skin-text">
    <li><span class="fa margin-right10"></span><a href="/">Home</a></li>
    <li><span class="fa fa-angle-right margin-right10"></span><a href="/forums/">Forums</a></li>
    <li><span class="fa fa-angle-right margin-right10"></span>My Subscriptions</li>
    </ul>
    <h1>My Subscribed Discussions</h1>
    <div class="border-solid-bottom margin-top10 margin-bottom15"></div>
</div>
</div>
<div class="grid-8">
	<div id="left_container_onethird">
			<br> 
			<div id="divMessage" visible="false" class="message" runat="server"></div>
			<div id="divForum" runat="server">
				<asp:Repeater ID="rptForums" runat="server">
					<headertemplate>
						<table border="0" width="100%" class="bdr" cellpadding="5" cellspacing="0">
							<tr class="dtHeader">
								<td width="2" style="border-right:0px;">&nbsp;</td>
								<td><strong>Thread</strong></td>
								<td width="140"><strong>Last Post</strong></td>
								<td width="140"><strong>Forum Category</strong></td>
								<td width="75"><strong>Alert Type</strong></td>
							</tr>
					</headertemplate>
					<itemtemplate>
						<asp:label ID="lblId" Text='<%# DataBinder.Eval(Container.DataItem, "TopicId") %>' Visible="false" runat="server" />
							<tr>
								<td style="padding-top:10px;" valign="top"><asp:CheckBox ID="chkThread" runat="server" /></td>
								<td>
									<%#GetLastPost
										(
											DataBinder.Eval(Container.DataItem, "Topic").ToString(),
											DataBinder.Eval(Container.DataItem, "CustomerName").ToString(),
											DataBinder.Eval(Container.DataItem, "StartDateTime").ToString(),
											DataBinder.Eval(Container.DataItem, "TopicId").ToString(),
											DataBinder.Eval(Container.DataItem, "Replies").ToString(),
											DataBinder.Eval(Container.DataItem, "StartedById").ToString(),
                                            DataBinder.Eval(Container.DataItem, "Url").ToString()
										)
									%>
								</td>
								<td>
									<%#GetLastPostThread
										(
											DataBinder.Eval(Container.DataItem, "LastPostBy").ToString(),
											DataBinder.Eval(Container.DataItem, "LastPostTime").ToString(),
											DataBinder.Eval(Container.DataItem, "LastPostedById").ToString()
										)
									%>
								</td>
								<td>
									<a href="/forums/<%# DataBinder.Eval(Container.DataItem, "forumUrl").ToString() %>/"><%# DataBinder.Eval(Container.DataItem, "ForumCategory").ToString() %></a>
								</td>
								<td align="right">
									<%# DataBinder.Eval(Container.DataItem, "SubscriptionType") %>
								</td>
							</tr>
					</itemtemplate>
					<footertemplate>
						</table>
					</footertemplate>
				</asp:Repeater>
				<div align="right" style="padding:5px;">
					Action on Selected Threads : <asp:Button ID="btnUnsubscribe" Text="Unsubscribe" runat="server" /> 
					or 
					<asp:DropDownList ID="cmbAlertType" runat="server">
						<asp:ListItem Value="0">-- Update Alert Type --</asp:ListItem>
						<asp:ListItem Value="1">No Email Notification</asp:ListItem>
						<asp:ListItem Value="2">Instant Email Notification</asp:ListItem>
					</asp:DropDownList>
					<asp:Button ID="btnUpdateSubscription" Text="Update" runat="server" /> 
				</div>
			</div>
		
	</div>
</div>
<div class="grid-4">
	<div class="addbox"><%= Carwale.UI.HtmlHelpers.Helpers.GetAdBarForAspx("1396440332273", 160, 600, 0, 0, false, 4) %></div>
</div>
</div>
<div class="clear"></div>
</section> 
</form>
<script type="text/javascript">
Common.showCityPopup = false;
document.getElementById("btnUnsubscribe").onclick = verifyChecks;
document.getElementById("btnUpdateSubscription").onclick = verifyStatus;

function verifyChecks(e)
{
	var chks = document.getElementById( "divForum" ).getElementsByTagName( "input" );
	var checkCount = 0;
	
	for ( var i=0; i<chks.length; i++ )
	{
		if ( chks[i].checked )
			checkCount ++;
	}
	
	if ( checkCount == 0 )
	{
		alert("Please select at least one discussion to continue.");
		return false;
	}
	
	return true;
}

function verifyStatus(e)
{
	var noError = true;
	
	if ( !verifyChecks() )
		return false;
		
	if ( document.getElementById('cmbAlertType').options[0].selected )
	{
		alert( "Please select a Notification Mode to update." );
		document.getElementById('cmbAlertType').focus();
		return false;	
	}
}
</script>
<!-- #include file="/includes/footer.aspx" -->
<!-- #include file="/includes/global/footer-script.aspx" -->

</body>
</html>
