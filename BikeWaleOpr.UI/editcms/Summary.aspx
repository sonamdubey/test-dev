<%@ Page AutoEventWireUp="false" Language="C#" Inherits="BikeWaleOpr.EditCms.Summary" Trace="false" debug="false" %>
<%@ Import Namespace="BikeWaleOpr.Common" %>
<%@ Import Namespace="BikeWaleOpr" %>
<!-- #Include file="/includes/headerNew.aspx" -->

<div class="urh">
	<a href="/default.aspx">BikeWale operations</a> &raquo; <a href="/editcms/default.aspx">Editorial Home</a> &raquo; Manage Articles
</div>
<script language="javascript" src="/src/AjaxFunctions.js"></script>
<script language="javascript" src="http://opr.carwale.com/src/common/bt.js"></script>

<div style="clear:both;">
	<h1 style="padding-left:0px;"><%=heading%></h1>
	<div style="padding-bottom:6px;"><b><%=isPublished%></b></div><br />
	<table cellpadding="0">
		<tr>
			<td style="border:1px solid #DBDBCE;vertical-align:top;width:650px;height:150px;">
				<h1>Basic Info <a href="basicinfo.aspx?bid=<%=basicId%>" style="font-size:12px;text-decoration:underline;">[ Edit ]</a></h1><br />
				<table style="padding:10px;padding-top:0px;vertical-align:top;">
					<%=basicInfo%>					
				</table>
			</td>
			<td style="width:20px;"></td>
			<td style="border:1px solid #DBDBCE;vertical-align:top;width:200px;height:150px;">
				<h1>Bikes Tagged <a href="selectbikes.aspx?bid=<%=basicId%>" style="font-size:12px;text-decoration:underline;">[ Edit ]</a></h1><br />
				<table style="padding:10px;padding-top:0px;vertical-align:top;">
					<%=GetTaggedBikes()%>
					<%=GetTagNames() %>				
				</table>
			</td>
			<td style="width:20px;"></td>
			<td rowspan="3" style="vertical-align:top;">
				<div style="border:1px solid #DBDBCE;height:250px;">
					<h1>Gallery <a href="createAlbum.aspx?bid=<%=basicId%>" style="font-size:12px;text-decoration:underline;">[ Edit ]</a></h1><br />
					<div style="padding-left:10px;"><%=GetImageCount()%> Photo(s)</div><br />
					<%--<%
                        if (MainImageSet)
                        {
					%>
					<div style="border:1px solid #DBDBCE;width:100px;margin-left:14px;padding:5px;">
						<img src='<%= ImagingOperations.GetPathToShowImages("//bikewaleimg/ec//", HostUrl) + basicId + "/img/m/" + basicId + ".jpg"%>' />
						<br/>
						Main Image
					</div>
					<% }else{ %>	
					<div style="border:1px solid #DBDBCE;width:100px;margin-left:14px;padding:5px;height:100px;">
						&nbsp;&nbsp;No Main Image
					</div>
					<% } %>--%>
                    <div style="border:1px solid #DBDBCE;margin:0 14px;padding:5px;">
						<img src='<%= ImagingOperations.GetPathToShowImages( imagePathThumbnail , HostUrl)%>' />
						<br/>
						Main Image
					</div>
				</div>			
			</td>
		</tr>
		<tr style="height:20px;"></tr>
		<tr>
          <td style="border:1px solid #DBDBCE;vertical-align:top;width:200px;height:150px;">
				<h1>Pages <a href="AddPages.aspx?bid=<%=basicId%>" style="font-size:12px;text-decoration:underline;">[ Edit ]</a></h1><br />
				<table style="padding:10px;padding-top:0px;vertical-align:top;">
					<%=GetPages()%>					
				</table>
			</td>
			<td style="border:1px solid #DBDBCE;vertical-align:top;display:none;">
				<h1>Extended Info <a href="otherInfo.aspx?bid=<%=basicId%>" style="font-size:12px;text-decoration:underline;">[ Edit ]</a></h1><br />
				<table style="padding:10px;padding-top:0px;vertical-align:top;">
					<%=GetOtherInfo()%>					
				</table>
			</td>
			<td style="width:20px;"></td>
			
			<td style="width:20px;"></td>
		</tr>
	</table>
</div>
</form>