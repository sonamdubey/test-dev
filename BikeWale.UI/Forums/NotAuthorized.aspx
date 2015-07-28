<%@ Page trace="false" Language="C#" %>
<%@ Register TagPrefix="BikeWale" TagName="Login" src="/Controls/LoginControl.ascx" %>
<%@ Register TagPrefix="BikeWale" TagName="Register" src="/Controls/RegisterControl.ascx" %>
<%@ Import NameSpace="Bikewale.Common" %>
<script language="c#" runat="server">
	bool UserExist = true;
</script>
<%
	// Define all the necessary meta-tags info here.
	// To know what are the available parameters,
	// check page, headerCommon.aspx in common folder.
	
	
%>
%>

<script language="javascript" src="/src/common/ui-tabs-dp.js" type="text/javascript"></script>
<script language="javascript">
	$(document).ready(function(){
		$("#uiTabs").tabs();
	});
</script>
<form runat="server">
<div class="left_container_top">
	<div class="content" style="width:600px; float:left;">
		<div id="youHere">
			<img src="<%=Bikewale.Common.ImagingFunctions.GetRootImagePath()%>/images/bullet/arrow.gif" align="absmiddle" /> 
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
			<div id="tb1" class="ui-tabs-panel"><BikeWale:Login id="ctlLogin" runat="server"/></div>					
			<div id="tb2"><BikeWale:Register id="ctlRegister" runat="server" /></div>
		</div>
	</div><!-- content eends -->
</div>
<div class="left_container" style="float:left"> </div>
</form>	
<!-- #include file="../includes/footer.aspx" -->