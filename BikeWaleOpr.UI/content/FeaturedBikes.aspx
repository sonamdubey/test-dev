<%@ Page Language="C#" Inherits="BikeWaleOpr.Content.FeaturedBikes" AutoEventWireUp="false" %>
<table cellpadding="5" cellspacing="0" style="border:1px solid;border-collapse:collapse;width:300px;" >
	<tr>
		<td style="background-color:#CCCCCC;width:100%;font-size:12px;font-weight:bold;">Featured Bikes</td>
	</tr>
	<asp:Repeater ID="rptFeaturedBikes" runat="server">
		<itemtemplate>
			<tr>
				<td>
					<span style="text-decoration:underline;color:#0066FF;cursor:pointer;" onClick="FeatureBikeClicked('<%# DataBinder.Eval( Container.DataItem, "MakeId" ) %>','<%# DataBinder.Eval( Container.DataItem, "ModelId" ) %>','<%# DataBinder.Eval( Container.DataItem, "VersionId" ) %>')">
						<%# DataBinder.Eval( Container.DataItem, "FeaturedBike" ) %>
					</span> 
					<img onclick="DeleteFeaturedBike('<%# DataBinder.Eval( Container.DataItem, "VersionId" ) %>')" src="../images/delete.gif" title="Delete featured Bike" style="margin-left:7px;cursor:pointer;" />
				</td>
			</tr>
		</itemtemplate>
	</asp:Repeater>	
</table>