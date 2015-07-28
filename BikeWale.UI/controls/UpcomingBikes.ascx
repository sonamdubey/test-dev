<%@ Control Language="C#" AutoEventWireup="false" Inherits="Bikewale.Controls.UpcomingBikes" %>
<%@ Import Namespace="Bikewale.Common" %>
<style type="text/css">
	.uc-ver {width:100%;}
	.uc-ver .lt {width:100%;height:69px;float:left;padding:8px 0px;}
	.uc-ver .lt img {float:left;border:1px solid #E1E1E1;padding:5px;margin-right:5px;background-color:#ffffff;}
	
	.uc-hor {width:100%;}
	.uc-hor .lt {width:158px;float:left;padding:8px 0px;}
	.uc-hor .lt img {float:left;border:1px solid #E1E1E1;padding:5px;margin-right:46px;background-color:#ffffff;}
</style>
<div id="divUpcomingBike" runat="server" class="uc-ver">
<asp:Repeater ID="rptData" runat="server">
	<itemtemplate>
			<div class="lt">
				<a href="/<%# DataBinder.Eval(Container.DataItem,"MakeMaskingName").ToString()%>-bikes/<%# DataBinder.Eval(Container.DataItem,"ModelMaskingName").ToString() %>/"><img src="<%# Bikewale.Common.ImagingFunctions.GetPathToShowImages("/bikewaleimg/models/", DataBinder.Eval(Container.DataItem,"HostUrl").ToString() ) + DataBinder.Eval(Container.DataItem,"SmallPic").ToString() %>" width="100px" height="57px" title="<%# DataBinder.Eval(Container.DataItem, "MakeName").ToString() + " " + DataBinder.Eval(Container.DataItem, "ModelName").ToString() %>" alt="<%# DataBinder.Eval(Container.DataItem, "MakeName").ToString() + " " + DataBinder.Eval(Container.DataItem, "ModelName").ToString() %>" /></a>
				<a href="/<%# DataBinder.Eval(Container.DataItem,"MakeMaskingName").ToString()%>-bikes/<%# DataBinder.Eval(Container.DataItem,"ModelMaskingName").ToString()%>"><%# DataBinder.Eval(Container.DataItem, "MakeName").ToString() + " " + DataBinder.Eval(Container.DataItem, "ModelName").ToString() %></a>
				<br/><span class="text-grey"><%# DataBinder.Eval(Container.DataItem,"ExpectedLaunch")%></span>
				<br/><span class="text-grey">Rs. <%# (DataBinder.Eval(Container.DataItem, "EstimatedPriceMin")).ToString().Replace(".00", "")%>-<%# (DataBinder.Eval(Container.DataItem, "EstimatedPriceMax")).ToString().Replace(".00", "")%> lakhs</span>
			</div></itemtemplate>
</asp:Repeater>
<asp:Label ID="lblNotFound" runat="server" Visible="false"></asp:Label>
</div>
<a class="redirect-rt" href="/upcoming-bikes/">All Upcoming bikes</a><span id="spnUC" class="icon-sheet more-link"></span>
<div class="clear"></div>


