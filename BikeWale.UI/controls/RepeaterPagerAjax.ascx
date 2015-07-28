<%@ Control Language="C#" AutoEventWireUp="false" Inherits="Bikewale.Controls.RepeaterPagerAjax" %>
<%@ Import Namespace="Bikewale.Common" %>
<style>	
	.fearured td{background-color:#FBF5B2;}
	.dtHeader a{text-decoration:none;color:#445566;}
</style>
<table id="tbl_res" border="0" width="100%" cellpadding="0" cellspacing="0">
	<tr class="dgNavDivTop" id="TopPager" runat="server">		
		<td width="140"><asp:label CssClass="headers" ID="lblRecords" runat="server" /></td>
		<td id="pgTop" align="center">
			<div class="hide"><span id="divFirstNav" runat="server"></span><span id="divPages" runat="server" align="center"></span><span id="divLastNav" runat="server"></span></div>
		</td>
		<td align="right" width="60"><input type="button" name="button" class="buttons" value="Compare" /></td>
	</tr>
	<tr>
		<td colspan="3">
			<table class="tbl-search" border="0" cellspacing="0" cellpadding="0" width="100%">
				<tr class="dt_header">
					<td width="120">Photo</td>
					<td>Model</td>
					<td width="120">Ex-Showroom Price</td>
					<td width="70" align="right">User Rating</td>
					<td width="20">&nbsp;</td>
				</tr>
				<asp:Repeater ID="rptFeaturedBike" runat="server">
					<itemtemplate>
						<%# GetModelRow(DataBinder.Eval(Container.DataItem, "ModelId").ToString(), DataBinder.Eval(Container.DataItem, "ModelCount").ToString(), DataBinder.Eval(Container.DataItem, "BikeModel").ToString(), DataBinder.Eval(Container.DataItem, "MoReviewRate").ToString(), DataBinder.Eval(Container.DataItem, "MinPrice").ToString(), DataBinder.Eval(Container.DataItem, "MaxPrice").ToString(), DataBinder.Eval(Container.DataItem,"smallpic").ToString(), DataBinder.Eval(Container.DataItem,"MoReviewCount").ToString(), DataBinder.Eval(Container.DataItem,"MakeName").ToString(), DataBinder.Eval(Container.DataItem,"ModelName").ToString(),DataBinder.Eval(Container.DataItem,"MakeMaskingName").ToString(),DataBinder.Eval(Container.DataItem,"ModelMaskingName").ToString())%>
						<tr class="hide cls_<%# DataBinder.Eval(Container.DataItem, "modelId") %> version-row"> 						
							<th>&nbsp;</th>
							<td class="ver-pdd"><a href="/<%# DataBinder.Eval(Container.DataItem, "MakeMaskingName").ToString()  +"-bikes/"+  DataBinder.Eval(Container.DataItem,"ModelMaskingName").ToString() +"/"+ UrlRewrite.FormatSpecial(DataBinder.Eval(Container.DataItem,"VersionName").ToString()) + "-details-" + DataBinder.Eval(Container.DataItem,"VersionId") %>.html"><%# DataBinder.Eval(Container.DataItem, "ModelName") %> <%# DataBinder.Eval(Container.DataItem, "VersionName") %></a><br /><span class="text-grey"><%# DataBinder.Eval(Container.DataItem, "Power") + "bhp, " + DataBinder.Eval(Container.DataItem, "FuelType") + ", " + DataBinder.Eval(Container.DataItem, "TransmissionType") + GetMiledge( DataBinder.Eval(Container.DataItem, "FuelEfficiencyOverall").ToString() )  %></span></td>
							<td valign="top">Rs.<%# CommonOpn.FormatPrice( DataBinder.Eval(Container.DataItem, "MinPrice").ToString(), DataBinder.Eval(Container.DataItem, "MaxPrice").ToString() ) %> </td>
							<td nowrap="nowrap"><%# CommonOpn.GetRateImage( Convert.ToDouble( DataBinder.Eval(Container.DataItem, "VsReviewRate") ) ) %><br /><a href="/<%#  DataBinder.Eval(Container.DataItem, "MakeMaskingName").ToString()  +"-bikes/"+ DataBinder.Eval(Container.DataItem,"ModelMaskingName").ToString()%>/userreviews-p1-<%# DataBinder.Eval(Container.DataItem,"VersionId") %>/" class="href-grey"><%# DataBinder.Eval(Container.DataItem, "VsReviewCount") %> Reviews</a></td>
							<td class="chk-compare"><input title="Select to compare" type="checkbox" name="chk_compare" value="<%# DataBinder.Eval(Container.DataItem, "VersionId") %>"/></td>
						</tr>
					</itemtemplate>
				</asp:Repeater>
				<asp:Panel ID="pnlGrid" runat="server"></asp:Panel>
		</td>
	</tr>
	<tr class="dgNavDivTop" id="BottomPager" runat="server">
		<td><asp:label CssClass="headers" ID="lblRecordsFooter" runat="server" /></td>
		<td id="pgBot" align="center"><span id="divFirstNav1" runat="server"></span><span id="divPages1" runat="server" align="center"></span><span id="divLastNav1" runat="server"></span></td>		
		<td align="right"><input type="button" name="button" class="buttons" value="Compare" /></td>
	</tr>
</table>
<script type="text/javascript" type="text/javascript">
	function changePageSize(e){
		var baseUrl = '<%= baseUrlForPs%>';
		var val = e.value;
		location.href = baseUrl + "&ps=" + val + "&pn=1";
	}
</script>	