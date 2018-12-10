<%@ Page trace="false" Language="C#" %>
<%@ Register TagPrefix="Carwale" TagName="Login" src="/Controls/LoginControl.ascx" %>
<%@ Register TagPrefix="Carwale" TagName="Register" src="/Controls/RegisterControl.ascx" %>
<%@ Import NameSpace="Carwale.UI.Common" %>
<script language="c#" runat="server">
	bool UserExist = true;
</script>
<%

	PageId 			= 305;
	Title 			= "Please Login";
	Description 	= "CarWale Member Login";
	Keywords		= "";
	Revisit 		= "15";
	DocumentState 	= "Static";
    AdId            = "1397024466973";
    AdPath          = "/1017752/Carwale_Forums_";
%>

<!-- #include file="/includes/headCommunity.aspx" -->
<script  language="javascript"  src="/static/src/ui-tabs-dp.js"  type="text/javascript"></script>
 <script type="text/javascript">
     Common.showCityPopup = false;
	$(document).ready(function(){
		$("#uiTabs").tabs();
	});
</script>
<form runat="server">
<div class="left_container_top">
	<div class="content" style="width:600px; float:left;">
		<div id="youHere">
			<img src="<%=ImagingFunctions.GetRootImagePath()%>/images/bullet/arrow.gif" align="absmiddle" /> 
			<span>You are here</span> : <a href="/community/">Community</a> &raquo; <a href="./">Forums</a> &raquo; Login / Register				
		</div>
		<br />
		<div class="note">
			You need to be logged in to start a new discussion / post a reply in CarWale forum. If you are new to CarWale, please register yourself below. It takes less than a minute to register and it's absolutely free.
		</div>
		</br>
		<br>
		<div id="uiTabs">
			<ul class="ui-tabs-nav">
				<li class='<%= UserExist == true ? "ui-tabs-selected" : "" %>'>
					<%= UserExist == true ? "<a href='#tb1'><span>Registered Members</span></a>" : "<a href='#tb1'><span>Registered Members</span></a>" %>
				</li>				
				<li class='<%= UserExist == false ? "ui-tabs-selected" : "" %>'>
					<%= UserExist == false ? "<a href='#tb1'><span>New Members</span></a>" : "<a href='#tb2'><span>New Member</span></a>" %>
				</li>
			</ul>
			<div id="tb1" class="ui-tabs-panel"><Carwale:Login id="ctlLogin" runat="server"/></div>					
			<div id="tb2"><Carwale:Register id="ctlRegister" runat="server" /></div>
		</div>
	</div><!-- content eends -->
</div>
<div class="left_container" style="float:left"> </div>
</form>	
<!-- #include file="/includes/footer-old.aspx" -->
