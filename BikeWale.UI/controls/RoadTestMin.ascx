<%@ Control Language="C#" AutoEventWireup="false" Inherits="Bikewale.Controls.RoadTestMin" %>
<%@ Import Namespace="Bikewale.Common" %>
<style>
	.rt-ver {width:100%;}
	.rt-ver .lt {width:100%;height:69px;float:left;padding:8px 0px;}
	.rt-ver .lt img {float:left;border:1px solid #E1E1E1;padding:5px;margin-right:5px;}
	
	.rt-hor {width:632px;}
	.rt-hor .lt {width:158px;float:left;padding:8px 0px;}
	.rt-hor .lt img {float:left;border:1px solid #E1E1E1;padding:5px;margin-right:46px;}
</style>
<div id="divRoadTest" runat="server" class="rt-ver">
<asp:Repeater ID="rptData" runat="server">
	<itemtemplate>
			<div class="lt">
				<a href="<%# detailsRTURL + UrlRewrite.FormatSpecial(DataBinder.Eval(Container.DataItem,"MakeName").ToString()) + "-cars/" + UrlRewrite.FormatSpecial(DataBinder.Eval(Container.DataItem, "ModelName").ToString()) + "/roadtest-" + DataBinder.Eval(Container.DataItem,"Id")%>/"><img src="<%# Bikewale.Common.ImagingFunctions.GetImagePath("/ec/", DataBinder.Eval(Container.DataItem,"ImageHostURL").ToString() ) + DataBinder.Eval(Container.DataItem,"Id")%>/img/m/<%# DataBinder.Eval(Container.DataItem,"ImageName").ToString() %>" width="100px" height="57px" title="<%# DataBinder.Eval(Container.DataItem,"Car") %>" alt="<%# DataBinder.Eval(Container.DataItem,"Car") %>" /></a>
				<a href="<%# detailsRTURL + UrlRewrite.FormatSpecial(DataBinder.Eval(Container.DataItem,"MakeName").ToString()) + "-cars/" + UrlRewrite.FormatSpecial(DataBinder.Eval(Container.DataItem, "ModelName").ToString()) + "/roadtest-" + DataBinder.Eval(Container.DataItem,"Id")%>/"><%# DataBinder.Eval(Container.DataItem,"Car") %></a>				
			</div>
	</itemtemplate>
</asp:Repeater>
</div>
<a class="redirect-rt" href="/new/road-test/">All Road Tests</a><span class="icon-sheet more-link"></span>
<div class="clear"></div>
