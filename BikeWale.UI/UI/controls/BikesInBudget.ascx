<%@ Control Language="C#" AutoEventWireUp="false" Inherits="Bikewale.Controls.BikesInBudget" %>
<%@ Import Namespace="Bikewale.Common"%>
<style type="text/css">
	#spnCities, #spnCities span {font-size:11px;color:#555555;}
</style>
<%if(RecordCount > 0) {%>
<h2 class="margin-top10"><%=HeaderText %></h2> 	
<div class="margin-top10 grey-bg">
<table border="0" cellspacing="0" style="border:1px solid #DDDDDD;" width="100%">
	<tr>
		<td style="padding:5px;" class="mainHead" align="left">
			<span>
				<span>More </span>bikes in this budget
			</span>
			<span id="spnCities">From 
				<asp:Label ID="lblCities" runat="server" />
			</span>
		</td>
	</tr>
	<tr>
		<td>
			<asp:DataList 
					ID="dlHighlights" 
					RepeatDirection="Horizontal" ItemStyle-HorizontalAlign="left"
					RepeatColumns="3"
					runat="server" Width="100%">
				<itemtemplate>
				  <table>
					<tr>
					   <td><strong>&raquo;</strong><a title="View Bike Details" href='/used/bikes-in-<%# DataBinder.Eval(Container.DataItem, "CityMaskingName").ToString() %>/<%# DataBinder.Eval(Container.DataItem, "MakeMaskingName").ToString() %>-<%# DataBinder.Eval(Container.DataItem, "ModelMaskingName").ToString() %>-<%# DataBinder.Eval(Container.DataItem, "ProfileId") %>/'> <%# DataBinder.Eval(Container.DataItem,"BikeMake") %> </a> </td>
					</tr>
					<tr>
					  <td>
						<div><%# Convert.ToDateTime( DataBinder.Eval(Container.DataItem,"MakeYear") ).ToString("MMM-yyyy") %>;
						<%# 
							DataBinder.Eval(Container.DataItem,"Kilometers").ToString() == "0" ? "--" :
												CommonOpn.FormatNumeric( DataBinder.Eval(Container.DataItem,"Kilometers").ToString()) 
						%>  kms, </div><strong>Rs.<%# CommonOpn.FormatNumeric( DataBinder.Eval(Container.DataItem,"Price").ToString() ) %>/-</strong>
					</td>
					</tr>
				  </table>
				</itemtemplate>
			  </asp:DataList>
		</td>
	</tr>
</table>
</div>
<%} %>