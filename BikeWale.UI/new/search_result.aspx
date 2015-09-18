<%@ Page Language="C#" AutoEventWireup="false" Inherits="Bikewale.New.search_result" Trace="false" %>
<%@ Register TagPrefix="BikeWale" TagName="RepeaterPager" src="/Controls/RepeaterPagerAjax.ascx" %>
<%@ Import Namespace="Bikewale.Common" %>
<%@ Import Namespace="System.Data" %>
<div id="res_msg" runat="server" visible="false" class="grey-bg content-block">
	<h3>Oops! No bikes found.</h3>
	<p>Try broadening your search criteria</p>
</div>
<BikeWale:RepeaterPager id="rpgListings" PageSize="10" ResultName="Bikes" ShowHeadersVisible="true" PagerPosition="TopBottom" runat="server">
	<asp:Repeater ID="rptListings" runat="server">	
        <itemtemplate>
			<%--<%# GetModelRow(DataBinder.Eval(Container.DataItem, "ModelRank").ToString(), DataBinder.Eval(Container.DataItem, "ModelId").ToString(), DataBinder.Eval(Container.DataItem, "ModelCount").ToString(), DataBinder.Eval(Container.DataItem, "BikeModel").ToString(), DataBinder.Eval(Container.DataItem, "MoReviewRate").ToString(), MakeModelVersion.GetFormattedPrice( DataBinder.Eval(Container.DataItem, "MinPrice").ToString()), MakeModelVersion.GetFormattedPrice( DataBinder.Eval(Container.DataItem, "MaxPrice").ToString()), DataBinder.Eval(Container.DataItem,"hostUrl").ToString(), DataBinder.Eval(Container.DataItem,"smallpic").ToString(), DataBinder.Eval(Container.DataItem,"MoReviewCount").ToString(), DataBinder.Eval(Container.DataItem,"MakeName").ToString(), DataBinder.Eval(Container.DataItem,"ModelMappingName").ToString(), DataBinder.Eval(Container.DataItem,"MakeMaskingName").ToString())%>--%>
            <%# GetModelRow(DataBinder.Eval(Container.DataItem, "ModelRank").ToString(), DataBinder.Eval(Container.DataItem, "ModelId").ToString(), DataBinder.Eval(Container.DataItem, "ModelCount").ToString(), DataBinder.Eval(Container.DataItem, "BikeModel").ToString(), DataBinder.Eval(Container.DataItem, "MoReviewRate").ToString(), MakeModelVersion.GetFormattedPrice( DataBinder.Eval(Container.DataItem, "MinPrice").ToString()), MakeModelVersion.GetFormattedPrice( DataBinder.Eval(Container.DataItem, "MaxPrice").ToString()), DataBinder.Eval(Container.DataItem,"hostUrl").ToString(), DataBinder.Eval(Container.DataItem,"OriginalImagePath").ToString(), DataBinder.Eval(Container.DataItem,"MoReviewCount").ToString(), DataBinder.Eval(Container.DataItem,"MakeName").ToString(), DataBinder.Eval(Container.DataItem,"ModelMappingName").ToString(), DataBinder.Eval(Container.DataItem,"MakeMaskingName").ToString())%>
			<tr class="hide cls_<%# DataBinder.Eval(Container.DataItem, "modelId") %> version-row">
				<th>&nbsp;</th>
				<td class="ver-pdd"><a href="/<%#  DataBinder.Eval(Container.DataItem, "MakeMaskingName").ToString() +"-bikes/"+  DataBinder.Eval(Container.DataItem,"ModelMappingName").ToString()%>/"><%# DataBinder.Eval(Container.DataItem, "ModelName") %> <%# DataBinder.Eval(Container.DataItem, "VersionName") %></a><br /><span class="text-grey"><%# DataBinder.Eval(Container.DataItem, "Power") + "bhp, " + DataBinder.Eval(Container.DataItem, "FuelType") + ", " + DataBinder.Eval(Container.DataItem, "TransmissionType") + GetMiledge( DataBinder.Eval(Container.DataItem, "FuelEfficiencyOverall").ToString() )  %></span></td>
				<td valign="top">Starts At Rs. <%# MakeModelVersion.GetFormattedPrice( DataBinder.Eval(Container.DataItem, "Price").ToString() ) %></td>
				<td align="center" nowrap="nowrap"><%# CommonOpn.GetRateImage( Convert.ToDouble( DataBinder.Eval(Container.DataItem, "VsReviewRate") ) ) %><br /><a href="/<%#DataBinder.Eval(Container.DataItem, "MakeMaskingName").ToString()+"-bikes/"+ DataBinder.Eval(Container.DataItem,"ModelMappingName").ToString()%>/user-reviews-p1-<%# DataBinder.Eval(Container.DataItem,"VersionId") %>/" class="href-grey"><%# DataBinder.Eval(Container.DataItem, "VsReviewCount") %> Reviews</a></td>
				<td class="chk-compare"><input title="Select to compare" type="checkbox" name="chk_compare" value="<%# DataBinder.Eval(Container.DataItem, "VersionId") %>" modelName="<%#  DataBinder.Eval(Container.DataItem, "MakeMaskingName").ToString() + "-"+  DataBinder.Eval(Container.DataItem,"ModelMappingName").ToString()%>"/></td>
			</tr>
		</itemtemplate>
		<footertemplate>
			</table>
		</footertemplate>
	</asp:Repeater>	
</BikeWale:RepeaterPager>
