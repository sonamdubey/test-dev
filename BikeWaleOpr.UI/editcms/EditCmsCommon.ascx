<%@ Control Language="C#" AutoEventWireUp="false" Inherits="BikeWaleOpr.EditCms.EditCmsCommon"%>

<style type="text/css">
	.tab{width:15%;background-color:#EEEEEE;text-align:center;font-weight:bold;}
	.h3 { font-size: 1em; margin-bottom: 5px; color: #000; font-weight:bold;}
</style>
<div style="height:60px;">
	<div style="display:none"><h1 style="padding-left:0px;"><%=heading%>  <a href="default.aspx" style="font-size:12px;text-decoration:underline;">Back to All Articles</a></h1></div>
	<div class="h3" style="float:left;"><%=title%>: <%=catName%></div><div style="float:left;">&nbsp;&nbsp;|&nbsp;&nbsp;</div>
    <div style="padding-bottom:0px; float:left;">Status: <b><%=isPublished%></b></div><div style="float:left;">&nbsp;&nbsp;|&nbsp;&nbsp;</div><div style="padding-bottom:0px;">Page Name: <b><%=PageName %></b></div>
    <div style="clear:both;"></div>
	<table style="width:75%;">
		<tr>
			<td class="tab" ><a id="1" href="basicInfo.aspx?bid=<%=basicId%>">Primary Info</a></td>
			<td class="tab"><a id="2" href="selectbikes.aspx?bid=<%=basicId%>">Tag Bike(s)</a></td>
			<td class="tab" style="display:none;"><a id="3" href="otherInfo.aspx?bid=<%=basicId%>">Extended Info</a></td>
			<td class="tab" ><a id="4" href="createalbum.aspx?bid=<%=basicId%>">Manage Album</a></td>
              <% if (catId == "13" || catId == "11"){ %><td class="tab"><a id="6" href="addvideos.aspx?bid=<%=basicId%>">Manage Videos</a></td><%} %>
			<td class="tab"><a id="5" href="addPages.aspx?bid=<%=basicId%>">Manage Article</a></td>
		</tr>
	</table>
    
</div>

<script type="text/javascript">
	$(document).ready(function() {
		var pageId = <%=pageId%>;
		//alert(pageId);
		$("#"+pageId).removeAttr("href");
		$("#"+pageId).attr("font","bold");
	});
</script>