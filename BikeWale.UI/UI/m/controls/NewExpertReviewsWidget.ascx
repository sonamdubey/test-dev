<%@ Control Language="C#" AutoEventWireup="false" Inherits="Bikewale.Mobile.Controls.NewExpertReviewsWidget" %>
 <% if(ShowWidgetTitle) { %>
<h3 class="model-section-subtitle">Expert Reviews</h3>
<% } %>
<asp:Repeater ID="rptExpertReviews" runat="server">
    <ItemTemplate>
        <div class="model-expert-review-container">
            <div class="margin-bottom20">
                <div class="review-image-wrapper">
                    <a href="/m<%# Bikewale.Utility.UrlFormatter.GetArticleUrl(Convert.ToString(DataBinder.Eval(Container.DataItem,"BasicId")),Convert.ToString(DataBinder.Eval(Container.DataItem,"ArticleUrl")),Bikewale.Entities.CMS.EnumCMSContentType.RoadTest.ToString()) %>">
                        <img alt="<%# DataBinder.Eval(Container.DataItem, "Title").ToString()%>" title="<%# DataBinder.Eval(Container.DataItem, "Title").ToString()%>" src="<%# Bikewale.Utility.Image.GetPathToShowImages(DataBinder.Eval(Container.DataItem, "OriginalImgUrl").ToString(),DataBinder.Eval(Container.DataItem, "HostUrl").ToString(),Bikewale.Utility.ImageSize._144x81) %>" />
                    </a>
                </div>
                <div class="review-heading-wrapper">
                    <a href="/m/expert-reviews/<%# DataBinder.Eval(Container.DataItem,"ArticleUrl").ToString() + "-" + DataBinder.Eval(Container.DataItem,"BasicId").ToString() %>.html" title="<%#DataBinder.Eval(Container.DataItem,"Title").ToString() %>"  class="font14 target-link"><%# Bikewale.Utility.FormatDescription.TruncateDescription(DataBinder.Eval(Container.DataItem, "Title").ToString(), 44) %></a>
                    <div class="grid-7 alpha padding-right5">
                        <span class="bwmsprite calender-grey-sm-icon"></span>
                        <span class="article-stats-content"><%# Bikewale.Utility.FormatDate.GetFormatDate(DataBinder.Eval(Container.DataItem, "DisplayDate").ToString(), "dd MMM yyyy") %></span>
                    </div>
                    <div class="grid-5 alpha omega">
                        <span class="bwmsprite author-grey-sm-icon"></span>
                        <span class="article-stats-content"><%# DataBinder.Eval(Container.DataItem, "AuthorName").ToString()%></span>
                    </div>
                    <div class="clear"></div>
                </div>
                <p class="margin-top10">
                    <%# Bikewale.Utility.FormatDescription.TruncateDescription(DataBinder.Eval(Container.DataItem, "Description").ToString(),180) %>
                </p>
            </div>
        </div>
    </ItemTemplate>
</asp:Repeater>
<div class="view-all-btn-container">
    <a title="<%= linkTitle %>" href="<%=MoreExpertReviewUrl%>"  class="bw-ga btn view-all-target-btn" data-cat="Model_Page" data-act="Read_all_expert_reviews_link_cliked" data-var="myBikeName">Read all reviews<span class="bwmsprite teal-right"></span></a>
</div>



