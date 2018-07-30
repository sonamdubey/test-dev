<%@ Control Language="C#" AutoEventWireup="false" Inherits="Bikewale.Controls.NewExpertReviews" %>
<% if(ShowWidgetTitle) { %>
<h3 class="model-section-subtitle padding-right10 padding-left10">Expert Reviews</h3>
<% } %>
<div class="model-expert-review-container" id="ctrlExpertReviews">
    <asp:Repeater ID="rptExpertReviews" runat="server">
        <ItemTemplate>
            <div class="margin-bottom20">
                <div class="grid-4">
                    <div class="model-preview-image-container">
                        <a href="<%# Bikewale.Utility.UrlFormatter.GetArticleUrl(Convert.ToString(DataBinder.Eval(Container.DataItem,"BasicId")),Convert.ToString(DataBinder.Eval(Container.DataItem,"ArticleUrl")),Bikewale.Entities.CMS.EnumCMSContentType.RoadTest.ToString()) %>">
                            <img class="lazy" data-original="<%# Bikewale.Utility.Image.GetPathToShowImages(DataBinder.Eval(Container.DataItem, "OriginalImgUrl").ToString(),DataBinder.Eval(Container.DataItem, "HostUrl").ToString(),Bikewale.Utility.ImageSize._310x174) %>" title="<%# DataBinder.Eval(Container.DataItem, "Title").ToString()%>" alt="<%# DataBinder.Eval(Container.DataItem, "Title").ToString()%>" src="" />
                        </a>
                    </div>
                </div>
                <div class="grid-8">
                    <a href="<%# Bikewale.Utility.UrlFormatter.GetArticleUrl(Convert.ToString(DataBinder.Eval(Container.DataItem,"BasicId")),Convert.ToString(DataBinder.Eval(Container.DataItem,"ArticleUrl")),Bikewale.Entities.CMS.EnumCMSContentType.RoadTest.ToString()) %>" class="article-target-link"  title="<%#DataBinder.Eval(Container.DataItem,"Title").ToString() %>"><%# DataBinder.Eval(Container.DataItem, "Title").ToString()%></a>
                    <div class="article-stats-left-grid">
                        <span class="bwsprite calender-grey-sm-icon"></span>
                        <span class="article-stats-content"><%# Bikewale.Utility.FormatDate.GetFormatDate(DataBinder.Eval(Container.DataItem, "DisplayDate").ToString(),"dd MMMM yyyy") %></span>
                    </div>
                    <div class="article-stats-right-grid">
                        <span class="bwsprite author-grey-sm-icon"></span>
                        <span class="article-stats-content"><%# DataBinder.Eval(Container.DataItem, "AuthorName").ToString() %></span>
                    </div>
                    <p class="line-height17 margin-top10">
                        <%# Bikewale.Utility.FormatDescription.TruncateDescription(DataBinder.Eval(Container.DataItem, "Description").ToString(),280) %>
                    </p>
                </div>
                <div class="clear"></div>
            </div>
        </ItemTemplate>
    </asp:Repeater>    
    <div class="padding-left10 more-article-target view-all-btn-container">
        <a title="<%= linkTitle %>" href="<%=MoreExpertReviewUrl %>" class="bw-ga btn view-all-target-btn" data-cat="Model_Page" data-act="Read_all_expert_reviews_link_cliked" data-var="myBikeName">Read all reviews<span class="bwsprite teal-right"></span></a>        
    </div>
</div><!-- Ends here-->


    

