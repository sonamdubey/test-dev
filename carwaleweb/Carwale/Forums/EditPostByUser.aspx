<%@ Page Language="C#" Inherits="Carwale.UI.Forums.EditPostByUser" validateRequest="false" trace="false" AutoEventWireup="false" %>
<%@ Register TagPrefix="Vspl" TagName="RTE" src="/Controls/RichTextEditor.ascx" %>
<%
	// Define all the necessary meta-tags info here.
	// To know what are the available parameters,
	// check page, headerCommon.aspx in common folder.
	
	PageId 			= 305;
	Title 			= "Forums: Edit Post";
	Description 	= "";
	Keywords		= "";
	Revisit 		= "15";
	DocumentState 	= "Static";
    AdId            = "1397024466973";
    AdPath          = "/1017752/Carwale_Forums_";
%>
<!-- #include file="/includes/headCommunity.aspx" -->

<link rel="stylesheet" href="/static/css/forums.css" type="text/css" >

<script language="javascript">
	function showCharactersLeft(ftb)
	{
		var maxSize = 4000;
		var size = ftb.GetHtml().length;
		
		if(size >= maxSize)
		{
			ftb.SetHtml( ftb.GetHtml().substring(0, maxSize -1) );
			size = maxSize;
		}
		
		document.getElementById("spnDesc").innerHTML = "Characters Left : " + (maxSize - size);
		//Inherits="Carwale.MyCarwale.Forums.EditPost"
	}
</script>

<div class="left_container_top">
	<div id="left_container_onethird">
		<div id="youHere"><img src="<%=ImagingFunctions.GetRootImagePath()%>/images/bullet/arrow.gif" align="absmiddle" /> 
			<span style="font-weight: bold">You are here</span> : 
			<a href="/community/">Community</a> &raquo; <a href="./">Forums</a> &raquo; <a href="<%= ForumUrl%>/"><%= ForumName%></a> &raquo; 
			<a href="<%= threadId%>-<%= ThreadUrl%>.html"><%= ThreadName%></a> &raquo; Edit This Topic
		</div>
		<form runat="server">
			<asp:Label ID="lblMessage" ForeColor="#FF0000" runat="server" EnableViewState="false" CssClass="error" Font-Bold="true" />
			<table class="writePost" border="0" cellpadding="3" cellspacing="0" width="100%">
				<tr>
					<td colspan="2">
						<h1>Edit This Thread: <%= GetTitle(ThreadName) %> </h1>
					</td>
				</tr>
				<tr>
					<td width="120" valign="top">Reply</td>
					<td>
						<Vspl:RTE id="rteET" Rows="15" Cols="60" runat="server" />
						<span id="spnDesc"></span><span id="spnDescription" class="error"></span>
					</td>
				</tr>
				<tr>
					<td colspan="2" class="dtHeader" align="center">
						<asp:Button ID="butSave" runat="server" Text="Update" CssClass="buttons" />
						<asp:Button ID="butCancel" runat="server" Text="Cancel" CssClass="buttons" />
					</td>
				</tr>
			</table>
						
					</td>
				</tr>
			</table>
		</form>
	</div>
</div>
<div class="right_container">	
	<div class="addbox"><%= Carwale.UI.HtmlHelpers.Helpers.GetAdBarForAspx("1396440332273", 160, 600, 0, 0, false, 4) %></div>
</div>
<script type="text/javascript">
    Common.showCityPopup = false;
	document.getElementById("butSave").onclick = verifyForm;
	
	function verifyForm(e)
	{
		var isError = false;
		
		var desc = tinyMCE.get('rteET_txtContent').getContent();
			
		if( desc == "" )
		{
			isError = true;
			document.getElementById("spnDescription").innerHTML = "&nbsp;Why blank reply? Please do write something!";
		}
		
		else
			document.getElementById("spnDescription").innerHTML = "";
		
		if(isError == true)
			return false;
	}	
</script>

<!-- #include file="/includes/footer-old.aspx" -->
<!-- Footer ends here -->
