<%@ Page Language="C#" Inherits="Carwale.UI.Community.Mods.modReportAbuse" trace="false" AutoEventWireup="false" %>
<%@ Import Namespace="Carwale.UI.Forums" %>
<!doctype html>
<html>
<head>

<%
	// Define all the necessary meta-tags info here.
	// To know what are the available parameters,
	// check page, headerCommon.aspx in common folder.
	
	PageId 			= 306;
	Title 			= "Moderator | Forums Report Abuse Summary";
	Description 	= "";
	Keywords		= "";
	Revisit 		= "15";
	DocumentState 	= "Static";
%>
<!-- #include file="/includes/global/head-script.aspx" -->
<link rel="stylesheet" href="/static/css/forums.css" type="text/css" >
<style>

.footerStrip { background-color:#FFFFD9;border:#FFFF79 1px solid; padding:5px; }
.footerStrip, .footerStrip a, .footerStrip a:link, .footerStrip a:visited, .footerStrip a:active { font-weight:bold; }
.ac {padding:3px;}
.iac {padding:3px;}
.message { padding:8px; margin:5px; background:#E9FEEA; color:#006231; font-size:13px; font-weight:bold; border:1px solid #A9D5A8; width:350px; }

</style>
<script language="javascript">
	function selectAll(type,chkId) 
	{
		var obj = document.getElementsByTagName("input");
		
		if(type == "all"){
			bolVal = true;
		}
		else{
			bolVal = false;
		}
		for ( var i = 0 ; i < obj.length ; i++ ) {
			if ( obj[i].type == "checkbox" && obj[i].id.indexOf(chkId) != -1 ) {
				obj[i].checked = bolVal;
			}
		}
	}
</script>
</head>
<body class="bg-white header-fixed-inner special-page special-skin-body no-bg-color">
<!-- #include file="/includes/header.aspx" -->
<form runat="server">
    <section class="bg-light-grey padding-top10 padding-bottom10 no-bg-color">
            <div id="youHere">
            <div class="container">
                <div class="grid-12 alpha omega margin-bottom10">
			        <a href="/community/">Community</a>
                    <span class="fa fa-angle-right margin-left10 margin-right10"></span>
                    <a href="default.aspx">Moderator's Home</a>
                    <span class="fa fa-angle-right margin-left10 margin-right10"></span>
                    Reported Posts
		        </div>
		        <div class="clear"></div>
			    <h1 class="font30 text-black special-skin-text">Reported Posts</h1>
                <div class="border-solid-bottom margin-top10 margin-bottom15"></div> 
                <div class="clear"></div>
		    </div>
            </div>
    </section>
    <div class="clear"></div>

    <section class="bg-light-grey">
        <div class="container">
        <div class="grid-10 alpha">
        <div class="content-box-shadow content-inner-block-10 rounded-corner2 margin-bottom20">
		<div id="divMessage" visible="false" class="message" runat="server"></div>
		<div id="divForum" runat="server">
			<asp:Repeater ID="rptReport" runat="server">
				<headertemplate>
					<table width="100%" cellspacing="0" cellpadding="5" class="bdr" border="0">
						<tr class="dtHeader">
							<td width="10">&nbsp;</td>
							<td>Thread</td>
							<td>Post</td>
							<td >Abused by</td>
							<td>Reason</td>
							<td>Date</td>
						</tr>
				</headertemplate>
				<itemtemplate>
						<tr>
							<td valign="top"><asp:CheckBox ID="chkID" runat="server"></asp:CheckBox> <asp:Label ID="lblId" Visible="false" runat="server" Text='<%# DataBinder.Eval(Container.DataItem,"ID") %>'></asp:Label>
							<asp:Label ID="lblReportId" Visible="false" runat="server" Text='<%# DataBinder.Eval(Container.DataItem,"RID") %>'></asp:Label>
							</td>
							<td valign="top"><%# "<a href='/forums/ViewThread.aspx?thread="+ DataBinder.Eval(Container.DataItem, "ThreadId") + "&post=" + DataBinder.Eval(Container.DataItem,"ID") +"' target='_blank'>" + DataBinder.Eval(Container.DataItem,"Thread")  + "</a>" %></td>
							<td valign="top"><%# DataBinder.Eval(Container.DataItem,"Post") %></td>
							<td width="70" valign="top"><%# "<a href='/Users/Profile-" + CarwaleSecurity.EncryptUserId( long.Parse( DataBinder.Eval(Container.DataItem, "CustomerId").ToString() ) ) + ".html' target='_blank'>" + DataBinder.Eval(Container.DataItem, "Customer") + "</a>" %></td>
							<td width="100" valign="top"><%# DataBinder.Eval(Container.DataItem,"Comment") %></td>
							<td width="60" valign="top"><%# DataBinder.Eval(Container.DataItem,"Date1", "{0:dd-MMM-yy}") %></td>
						</tr>
				</itemtemplate>
				<footertemplate>
					</table>
				</footertemplate>
			</asp:Repeater>	
			<div align="left" style="padding:5px;">
			<strong>Select :</strong>
			<a href="javascript:selectAll('all','chkID')"><strong>All</strong></a>
			<a href="javascript:selectAll('none','chkID')"><strong>None</strong></a>
			<asp:Button ID="btnApprove" CssClass="submit btn btn-grey btn-xs" Text="Approve Post(s)" runat="server"></asp:Button>
			<asp:Button ID="btnDelete" CssClass="submit btn btn-grey btn-xs" Text="Delete Post(s)" runat="server"></asp:Button> 
			</div>
		</div>
		</div>
	    </div>
        <div class="grid-2 omega">
	        <div class="addbox">
                <%= Carwale.UI.HtmlHelpers.Helpers.GetAdBarForAspx("1396440332273", 160, 600, 0, 0, false, 4) %>
	        </div>
        </div>
        <div class="clear"></div>
        </div>
    </section>
    <div class="clear"></div>

<script language="javascript">

document.getElementById("btnDelete").onclick = btnDelete_Change;

function btnDelete_Change(e)
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
		alert("Please select at least one Post to continue.");
		return false;
	}
	
	return true;
}

</script>
</form>
<!-- #include file="/includes/footer.aspx" -->
<!-- #include file="/includes/global/footer-script.aspx" -->
</body>
</html>