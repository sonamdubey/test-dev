<tr>
    <td class="headerSpecs" width="196">&nbsp;</td>
	<td><strong><a href='<%= GetLandingURL(MakeMaskingName[0], ModelMaskingName[0], "", versionId[0]) %>' title='<%= makeName[0] + " " + modelName[0] + " Details"%>'><%=makeName[0] + " " + modelName[0]%></a></strong><br /><strong><a href='<%= GetLandingURL(MakeMaskingName[0], ModelMaskingName[0], versionName[0], versionId[0]) %>' title="<%=bike[0]%> Details"><%=versionName[0]%></a></strong></td>
	<td><strong><a href='<%= GetLandingURL(MakeMaskingName[1], ModelMaskingName[1], "", versionId[1]) %>' title='<%= makeName[1] + " " + modelName[1] + " Details"%>'><%=makeName[1] + " " + modelName[1]%></a></strong><br /><strong><a href="<%= GetLandingURL(MakeMaskingName[1], ModelMaskingName[1], versionName[1], versionId[1]) %>" title="<%=bike[1]%> Details"><%=versionName[1]%></a></strong></td>
	<td><strong><a href='<%= GetLandingURL(MakeMaskingName[2], ModelMaskingName[2], "", versionId[2]) %>' title='<%= makeName[2] + " " + modelName[2] + " Details"%>'><%=makeName[2] + " " + modelName[2]%></a></strong><br /><strong><a href="<%= GetLandingURL(MakeMaskingName[2], ModelMaskingName[2], versionName[2], versionId[2]) %>" title="<%=bike[2]%> Details"><%=versionName[2]%></a></strong></td>
	<td><strong><a href='<%= GetLandingURL(MakeMaskingName[3], ModelMaskingName[3], "", versionId[3]) %>' title='<%= makeName[3] + " " + modelName[3] + " Details"%>'><%=makeName[3] + " " + modelName[3]%></a></strong><br /><strong><a href="<%= GetLandingURL(MakeMaskingName[3], ModelMaskingName[3], versionName[3], versionId[3]) %>" title="<%=bike[3]%> Details"><%=versionName[3]%></a></strong></td>
	<td><strong><a href='<%= GetLandingURL(MakeMaskingName[4], ModelMaskingName[4], "", versionId[4]) %>' title='<%= makeName[4] + " " + modelName[4] + " Details"%>'><%=makeName[4] + " " + modelName[4]%></a></strong><br /><strong><a href="<%= GetLandingURL(MakeMaskingName[4], ModelMaskingName[3], versionName[4], versionId[4]) %>" title="<%=bike[4]%> Details"><%=versionName[4]%></a></strong></td>
</tr>
<tr>
	<td class="headerSpecs">&nbsp;</td>
	<td><a title="View complete details of <%=bike[0]%>" href='<%= GetLandingURL(MakeMaskingName[0], ModelMaskingName[0], versionName[0], versionId[0]) %>'><img alt="<%=bike[0]%>" src="<%=Bikewale.Common.ImagingFunctions.GetPathToShowImages("/bikewaleimg/models/", hostURL[0]) + versionId[0] %>b.jpg" border="0" style="width:160px;" /></a></td>
	<td><a title="View complete details of <%=bike[1]%>" href="<%= GetLandingURL(MakeMaskingName[1], ModelMaskingName[1], versionName[1], versionId[1]) %>"><img alt="<%=bike[1]%>" src="<%=Bikewale.Common.ImagingFunctions.GetPathToShowImages("/bikewaleimg/models/", hostURL[1]) + versionId[1] %>b.jpg" border="0"  style="width:160px;"/></a></td>
	<td><a title="View complete details of <%=bike[2]%>" href="<%= GetLandingURL(MakeMaskingName[2], ModelMaskingName[2], versionName[2], versionId[2]) %>"><img alt="<%=bike[2]%>" src="<%=Bikewale.Common.ImagingFunctions.GetPathToShowImages("/bikewaleimg/models/", hostURL[2]) + versionId[2] %>b.jpg" border="0"  style="width:160px;" /></a></td>
	<td><a title="View complete details of <%=bike[3]%>" href="<%= GetLandingURL(MakeMaskingName[3], ModelMaskingName[3], versionName[3], versionId[3]) %>"><img alt="<%=bike[3]%>" src="<%=Bikewale.Common.ImagingFunctions.GetPathToShowImages("/bikewaleimg/models/", hostURL[3]) + versionId[3] %>b.jpg" border="0"  style="width:160px;" /></a></td>
	<td><a title="View complete details of <%=bike[4]%>" href="<%= GetLandingURL(MakeMaskingName[4], ModelMaskingName[4], versionName[4], versionId[4]) %>"><img alt="<%=bike[4]%>" src="<%=Bikewale.Common.ImagingFunctions.GetPathToShowImages("/bikewaleimg/models/", hostURL[4]) + versionId[4] %>b.jpg" border="0"  style="width:160px;"/></a></td>	
</tr>
<tr>
  <td class="headerSpecs">Bike Rating</td>
  <td><%=GetModelRatings( versionId[0] ) %></td>
  <td><%=GetModelRatings( versionId[1] ) %></td>
  <td><%=GetModelRatings( versionId[2] ) %></td>
  <td><%=GetModelRatings( versionId[3] ) %></td>
  <td><%=GetModelRatings( versionId[4] ) %></td>
</tr>
<tr>
	<td class="headerSpecs">Ex-Showroom Price (<asp:Literal id="ltrDefaultCityName" runat="server"></asp:Literal>)</td>
	<td><strong><%= price[0] != null ? "Rs. " +price[0]  : "NA" %></strong><br />
		<a style="display:none;" href="<%= GetLandingURL(MakeMaskingName[0], ModelMaskingName[0], versionName[0], versionId[0]) %>">More Details</a>
		<%= 
			leadAggregator[0] == "" ? "" : "<div class='la' style='margin-top:5px;'><a href=\"/pricequote/default.aspx?version=" + versionId[0] + "class='fillPopupData' \" >Check On-Road Price</a></div>"
		%>
		<div style="display:none;" class="la">
			<a  href="/finance/financeinquiries.aspx?version=<%=versionId[0]%>">Get quick loan</a> 
		</div>
	</td>
	<td><strong><%= price[1] != null ? "Rs. " +price[1]  : "NA" %></strong><br />
		<a style="display:none;" href="<%= GetLandingURL(MakeMaskingName[1], ModelMaskingName[1], versionName[1], versionId[1]) %>">More Details</a> 
		<%= 
			leadAggregator[1] == "" ? "" : "<div class='la' style='margin-top:5px;'><a href=\"/pricequote/default.aspx?version=" + versionId[1] + "class='fillPopupData' \">Check On-Road Price</a></div>"
		%>
		<div style="display:none;" class="la">
			<a href="/finance/financeinquiries.aspx?version=<%=versionId[1]%>">Get quick loan</a> 
		</div>
	</td>
	<td><strong><%= price[2] != null ? "Rs. " +price[2]  : "NA" %></strong><br />
		<a style="display:none;" href="<%= GetLandingURL(MakeMaskingName[2], ModelMaskingName[2], versionName[2], versionId[2]) %>">More Details</a> 
		<%= 
			leadAggregator[2] == "" ? "" : "<div class='la' style='margin-top:5px;'><a href=\"/pricequote/default.aspx?version=" + versionId[2] + "class='fillPopupData' \">Check On-Road Price</a></div>"
		%>
		<div style="display:none;" class="la">
			<a href="/finance/financeinquiries.aspx?version=<%=versionId[2]%>">Get quick loan</a> 
		</div>
	</td>
	<td><strong><%= price[3] != null ? "Rs. " +price[3]  : "NA" %></strong><br />
		<a style="display:none;" href="<%= GetLandingURL(MakeMaskingName[3], ModelMaskingName[3], versionName[3], versionId[3]) %>">More Details</a> 
		<%= 
			leadAggregator[3] == "" ? "" : "<div class='la' style='margin-top:5px;'><a href=\"/pricequote/default.aspx?version=" + versionId[3] + "class='fillPopupData' \">Check On-Road Price</a></div>"
		%>
		<div style="display:none;" class="la">
			<a href="/finance/financeinquiries.aspx?version=<%=versionId[3]%>">Get quick loan</a> 
		</div>
	</td>
	<td><strong><%= price[4] != null ? "Rs. " +price[4]  : "NA" %></strong><br />
		<a style="display:none;" href="<%= GetLandingURL(MakeMaskingName[4], ModelMaskingName[4], versionName[4], versionId[4]) %>">More Details</a> 
		<%= 
			leadAggregator[4] == "" ? "" : "<div class='la' style='margin-top:5px;'><a href=\"/pricequote/default.aspx?version=" + versionId[4] + "class='fillPopupData' \">Check On-Road Price</a></div>"
		%>
		<div style="display:none;" class="la">
			<a href="/finance/financeinquiries.aspx?version=<%=versionId[4]%>">Get quick loan</a> 
		</div>
	</td>		
</tr>

