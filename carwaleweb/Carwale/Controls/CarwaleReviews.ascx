<%@ Control Language="C#" AutoEventWireUp="false" Inherits="Carwale.UI.Controls.CarwaleReviews" %>
<%@ Import Namespace="Carwale.UI.Common"%>
<%@ Import Namespace="Carwale.Utility"%>
<style>
	.noBorder{border:0px;}
	.lineSep { border-bottom:1px dashed #D9D9C1;padding:4px 0; }
	a.startBy, a.startBy:link, a.startBy:visited, a.startBy:active { color:#888888; text-decoration:underline; }
	a.startBy:hover { text-decoration:none; }
</style>
<div id="divReviews" runat="server" style="line-height:15px;">
<asp:Repeater ID="rptUserReviews" runat="server">
	<itemtemplate>
		<div class="lineSep">
			&raquo;
			<a href='/<%# UrlRewrite.FormatSpecial(DataBinder.Eval(Container.DataItem,"MakeName").ToString())%>-cars/<%# DataBinder.Eval(Container.DataItem,"MaskingName").ToString()%>/userreviews/<%# DataBinder.Eval(Container.DataItem,"ReviewId")%>/' title="Read detailed review"><%# DataBinder.Eval(Container.DataItem,"Title")%></a> <br /><%# CommonOpn.GetRateImage( Convert.ToDouble( DataBinder.Eval(Container.DataItem,"OverallR")) )%>
			on <%# DataBinder.Eval(Container.DataItem,"ModelName")%>
			<%# this.ReviewerId == "-1" ? "by <a class='startBy' href='/community/members/profile.aspx?uid=" + CarwaleSecurity.EncryptUserId( Convert.ToInt64(DataBinder.Eval(Container.DataItem,"CustomerId"))) + "'>" + DataBinder.Eval(Container.DataItem,"CustomerName") + "</a>" : "" %>
			<%# GetComments( DataBinder.Eval(Container.DataItem,"ShortComment").ToString(), DataBinder.Eval(Container.DataItem, "ReviewId").ToString())%>
		</div>
	</itemtemplate>
</asp:Repeater>
</div>
