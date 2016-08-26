<%@ Control Language="C#" AutoEventWireup="false" Inherits="Bikewale.Mobile.Controls.NewExpertReviewsWidget" %>

<h3 class="model-section-subtitle">Expert Reviews</h3>
<asp:Repeater ID="rptExpertReviews" runat="server">
    <ItemTemplate>
        <div class="model-expert-review-container">
            <div class="margin-bottom20">
                <div class="review-image-wrapper">
                    <a href="/m<%# Bikewale.Utility.UrlFormatter.GetArticleUrl(Convert.ToString(DataBinder.Eval(Container.DataItem,"BasicId")),Convert.ToString(DataBinder.Eval(Container.DataItem,"ArticleUrl")),Bikewale.Entities.CMS.EnumCMSContentType.RoadTest.ToString()) %>">
                        <img alt="<%# DataBinder.Eval(Container.DataItem, "Title").ToString()%>" title="<%# DataBinder.Eval(Container.DataItem, "Title").ToString()%>" src="<%# Bikewale.Utility.Image.GetPathToShowImages(DataBinder.Eval(Container.DataItem, "OriginalImgUrl").ToString(),DataBinder.Eval(Container.DataItem, "HostUrl").ToString(),Bikewale.Utility.ImageSize._370x208) %>" />
                    </a>
                </div>
                <div class="review-heading-wrapper">
                    <a href="/m/expert-reviews/<%# DataBinder.Eval(Container.DataItem,"ArticleUrl").ToString() + "-" + DataBinder.Eval(Container.DataItem,"BasicId").ToString() %>.html" class="font14 target-link"><%# Bikewale.Utility.FormatDescription.TruncateDescription(DataBinder.Eval(Container.DataItem, "Title").ToString(), 44) %></a>
                    <div class="grid-7 alpha padding-right5">
                        <span class="bwmsprite calender-grey-sm-icon"></span>
                        <span class="article-stats-content"><%# Bikewale.Utility.FormatDate.GetFormatDate(DataBinder.Eval(Container.DataItem, "DisplayDate").ToString(), "MMM dd, yyyy") %></span>
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
<div>
    <a title="<%= linkTitle %>" href="<%=MoreExpertReviewUrl%>"  class="bw-ga" c="Model_Page" a="Read_all_expert_reviews_link_cliked" v="myBikeName">Read all expert reviews<span class="bwmsprite blue-right-arrow-icon"></span></a>
</div>
<script type="text/javascript">
    $(document).ready(function () { $("img.lazy").lazyload(); });
</script>



