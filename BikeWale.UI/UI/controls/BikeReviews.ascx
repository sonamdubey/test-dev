<%@ Control Language="C#" AutoEventWireup="false" Inherits="Bikewale.Controls.BikeReviews" %>
<%@ Import Namespace="Bikewale.Common"%>
<style>
	.noBorder{border:0px;}
	.lineSep { border-bottom:1px dashed #D9D9C1;padding:4px 0; }
	a.startBy, a.startBy:link, a.startBy:visited, a.startBy:active { color:#888888; text-decoration:underline; }
	a.startBy:hover { text-decoration:none; }
</style>
<div id="divReviews" runat="server" style="line-height: 15px;">
    <asp:Repeater ID="rptUserReviews" runat="server">
        <ItemTemplate>
            <div class="lineSep">
                &raquo;
			    <a href='/<%# DataBinder.Eval(Container.DataItem,"MakeMaskingName").ToString() %>-bikes/<%# DataBinder.Eval(Container.DataItem,"ModelMaskingName").ToString() %>/user-reviews/<%# DataBinder.Eval(Container.DataItem,"ReviewId")%>.html' title="Read detailed review"><%# DataBinder.Eval(Container.DataItem,"Title")%></a>
                <br />
                <%# GetRateImage( Convert.ToDouble( DataBinder.Eval(Container.DataItem,"OverallR")) )%>
			    on <%# DataBinder.Eval(Container.DataItem,"ModelName")%>
                <%--<%# this.ReviewerId == "-1" ? "by <a class='startBy' href='/community/members/profile.aspx?uid=" + BikewaleSecurity.EncryptUserId( Convert.ToInt64(DataBinder.Eval(Container.DataItem,"CustomerId"))) + "'>" + DataBinder.Eval(Container.DataItem,"CustomerName") + "</a>" : "" %>--%>
                <%# this.ReviewerId == "-1" ? "by " + DataBinder.Eval(Container.DataItem,"CustomerName")  : "" %>
                <%# GetComments( DataBinder.Eval(Container.DataItem,"ShortComment").ToString(), DataBinder.Eval(Container.DataItem, "ReviewId").ToString())%>
            </div>
        </ItemTemplate>
    </asp:Repeater>
</div>