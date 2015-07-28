<%@ Control Language="C#" AutoEventWireUp="false" Inherits="BikeWaleOpr.EditCms.DisplayBasicInfo" %>

<div style="width:300px;border:1px solid #DBDBCE;padding:10px;">
	<div><h3><span id="tdDBITitle"><%=title%></span></h3></div>
	<table>
		<tr>
			<td>Category</td>
			<td>:</td>
			<td><%=catName%></td>
		</tr>
		<tr>
			<td style="width:80px;">Author name</td>
			<td>:</td>
			<td id="tdDBIAuthorName"><%=author%></td>
		</tr>
		<tr>
			<td>Display date</td>
			<td>:</td>
			<td id="tdDBIDisplayDate"><%=displayDate.ToString("dd-MMM-yyyy")%></td>
		</tr>
		<tr>
			<td style="vertical-align:top;">Description</td>
			<td>:</td>
			<td id="tdDBIDesription"><%=description%></td>
		</tr>
		<tr>
			<td colspan="3">
				
				<!--Basic Info-->
				<div><a id="a1" style="cursor:pointer;text-decoration:none;" href="basicinfo.aspx?bid=<%=basicId%>&new=<%=IsNew%>">View basic info</a></div>
				
				<!--Bikes-->
				<% bool addLinkShown = false; %>
				<% if (allowBikeSelection == "1") {%>
				<% if (Convert.ToInt32(bikesCount) >= minBikeSelection) {%>
					<div><a id="a2" style="cursor:pointer;text-decoration:none;" href="selectbikes.aspx?bid=<%=basicId%>&new=<%=IsNew%>">View Bikes</a></div>
				<%}else if(Convert.ToInt32(bikesCount) < minBikeSelection && addLinkShown == false){ addLinkShown = true; %>
					<div><a id="a2" style="cursor:pointer;text-decoration:none;" href="selectbikes.aspx?bid=<%=basicId%>&new=<%=IsNew%>">Add Bikes (Pending)</a></div>
				<%}%>
				<%}%>
				
				<!--Other Info-->
				<% if (otherCount != "0") {%>
					<div><a id="a3" style="cursor:pointer;text-decoration:none;" href="OtherInfo.aspx?bid=<%=basicId%>&new=<%=IsNew%>">View other info</a></div>
				<%}else if(Convert.ToInt32(bikesCount) >= minBikeSelection && otherCount == "0" && addLinkShown == false){  addLinkShown = true; %>
					<div><a id="a3" style="cursor:pointer;text-decoration:none;" href="OtherInfo.aspx?bid=<%=basicId%>&new=<%=IsNew%>">Add other info (Pending)</a></div>
				<%}else if (Convert.ToInt32(bikesCount) < minBikeSelection){ %>
					<div style="color:#B7B79D;">Add other info (Pending)</div>
				<%}%>
				
				<!--Images-->
				<% if (imageCount != "0") {%>
					<div><a id="a4" style="cursor:pointer;text-decoration:none;" href="createalbum.aspx?bid=<%=basicId%>&new=<%=IsNew%>">View album</a></div>
				<%}else if(imageCount == "0" && otherCount != "0" && addLinkShown == false){  addLinkShown = true; %>
					<div><a id="a4" style="cursor:pointer;text-decoration:none;" href="createalbum.aspx?bid=<%=basicId%>&new=<%=IsNew%>">Add album (Pending)</a></div>
				<%}else{%>
					<div style="color:#B7B79D;">Add album (Pending)</div>
				<%}%>
				
				<!--Pages-->
				<% if (pagesCount != "0") {%>
					<div><a id="a5" style="cursor:pointer;text-decoration:none;" href="addPages.aspx?bid=<%=basicId%>&new=<%=IsNew%>">View pages</a></div>
				<%}else if(pagesCount == "0" && (imageCount != "0" || System.IO.File.Exists(BikeWaleOpr.Common.CommonOpn.ImagePath + "editcms/" + basicId + "/m/" + basicId + ".jpg")) && addLinkShown == false){  addLinkShown = true; %>
					<div><a id="a5" style="cursor:pointer;text-decoration:none;" href="addPages.aspx?bid=<%=basicId%>&new=<%=IsNew%>">Add pages (Pending)</a></div>
				<%}else{%>
					<div style="color:#B7B79D;">Add pages (Pending)</div>
				<%}%>
			</td>
		</tr>
	</table>
<script language="javascript" type="text/javascript">
	$(document).ready(function(){
		$("#a<%=PageId.ToString()%>").hide();
		//if(<%=IsNew%> == "1")
		//{
			//for(var i=<%=PageId%>; i<=5; i++)
			//{
				//$("#a"+i).hide();
			//}
		//}
	});
</script>	
</div>