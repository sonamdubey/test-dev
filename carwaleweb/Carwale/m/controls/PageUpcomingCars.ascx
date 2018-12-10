<%@ Control Language="C#" AutoEventWireUp="false" Inherits="MobileWeb.Controls.PageUpcomingCars" %>
<style>
    .arr-small { color: red; font-size: 15px; }
</style>
<asp:Repeater ID="rptUpcomingCars" runat="server">
	<itemtemplate>
    <a class="normal" style="text-decoration:none;" href='/m/<%# MobileWeb.Common.CommonOpn.FormatSpecial(DataBinder.Eval(Container.DataItem, "MakeName").ToString()) %>-cars/<%# DataBinder.Eval(Container.DataItem, "MaskingName") %>' >
		<div class="content-inner-block-10 content-box-shadow margin-bottom10 text-black rounded-corner2">
			<table cellspacing="0" cellpadding="0" style="width:100%;">
				<tr>
					<%--<td style="width:100px;" valign="top"><img alt="<%# DataBinder.Eval(Container.DataItem, "MakeName").ToString() + " " + DataBinder.Eval(Container.DataItem, "ModelName").ToString() %>" title="<%# DataBinder.Eval(Container.DataItem, "MakeName").ToString() + " " + DataBinder.Eval(Container.DataItem, "ModelName").ToString() %>" src='<%# GetImagePathCWImg("/cars/", DataBinder.Eval(Container.DataItem, "HostURL").ToString()) + DataBinder.Eval(Container.DataItem, "SmallPic").ToString() %>'></td>--%>
                    <td style="width:100px;" valign="top"><img alt="<%# DataBinder.Eval(Container.DataItem, "MakeName").ToString() + " " + DataBinder.Eval(Container.DataItem, "ModelName").ToString() %>" title="<%# DataBinder.Eval(Container.DataItem, "MakeName").ToString() + " " + DataBinder.Eval(Container.DataItem, "ModelName").ToString() %>" src='<%# Carwale.Utility.CWConfiguration._imgHostUrl + Carwale.Utility.ImageSizes._110X61 + (DataBinder.Eval(Container.DataItem, "Image.ImagePath")!=null?DataBinder.Eval(Container.DataItem, "Image.ImagePath").ToString():string.Empty)%>'></td>
					<td valign="top" class="padding-left10">
						<div class="sub-heading"><b><%# DataBinder.Eval(Container.DataItem, "MakeName").ToString() + " " + DataBinder.Eval(Container.DataItem, "ModelName").ToString() %></b>&nbsp;&nbsp;<span class="arr-small">&raquo;</span></div>
						<div>When to expect</div>
						<div><%# DataBinder.Eval(Container.DataItem, "ExpectedLaunch") %></div>
						<div>Estimated price</div>
						<div>₹ <%# (Eval("Price.MinPrice").ToString() != "0"  ? Carwale.UI.Common.FormatPrice.GetFormattedPriceV2(Eval("Price.MinPrice").ToString(),Eval("Price.MaxPrice").ToString()) : "N/A") %></div>
					</td>
				</tr>
			</table>
		</div>
	</a>	
	</itemtemplate>
</asp:Repeater>	

