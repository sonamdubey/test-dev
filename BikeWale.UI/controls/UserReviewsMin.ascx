<%@ Control Language="C#" AutoEventWireup="false" Inherits="Bikewale.Controls.UserReviewsMin" %>
<%@ Import namespace="Bikewale.Common" %>
<div id="divControl" runat="server" class="hide">
    <div>
        <h2 style="width:300px;" class="left-float"> <a href="<%= !String.IsNullOrEmpty(ModelId) ? "user-reviews/" : "/user-reviews/" %>" class="link-decoration" title="User Reviews"> User Reviews </a></h2>
        <div class="margin-top5 readmore right-float">
            <a href="/user-reviews/">View All</a>
       </div>
       <div class="clear"></div>
   </div>
    <ul class="ul-normal margin-top5">
    <asp:Repeater ID="rptReviews" runat="server">
	    <itemtemplate>
		    <li>
			    <a title="<%# DataBinder.Eval(Container.DataItem,"Title") %>" href="/<%# DataBinder.Eval(Container.DataItem,"MakeMaskingName").ToString().ToLower() %>-bikes/<%# DataBinder.Eval(Container.DataItem,"ModelMaskingName").ToString().ToLower() %>/user-reviews/<%# DataBinder.Eval(Container.DataItem,"Id") %>.html"><%# DataBinder.Eval(Container.DataItem,"Title") %></a>
			    <p>
				    <%# CommonOpn.GetRateImage(Convert.ToDouble(DataBinder.Eval(Container.DataItem,"OverallR"))) %> <span class="text-grey">on <b><%# DataBinder.Eval(Container.DataItem,"Car") %></b> by <%# GetAuthorName(DataBinder.Eval(Container.DataItem,"CustomerName").ToString(), DataBinder.Eval(Container.DataItem,"HandleName").ToString()) %>, <%# Convert.ToDateTime(DataBinder.Eval(Container.DataItem,"EntryDateTime")).ToString("dd-MMM-yyyy") %></span>
				    <br/><%# GetModifiedComment(RemoveHtmlTags(DataBinder.Eval(Container.DataItem,"Comments").ToString())) %>
			    </p>
		    </li>
	    </itemtemplate>
    </asp:Repeater>
    </ul>
</div>