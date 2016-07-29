<%@ Control Language="C#" AutoEventWireup="false" Inherits="Bikewale.Controls.NewExpertReviews" %>
<h3 class="model-section-subtitle padding-right10 padding-left10">Expert Reviews</h3>
<div class="model-expert-review-container" id="ctrlExpertReviews">
    <asp:Repeater ID="rptExpertReviews" runat="server">
        <ItemTemplate>
            <div class="margin-bottom20">
                <div class="grid-4">
                    <div class="model-preview-image-container">
                        <a href="/expert-reviews/<%# DataBinder.Eval(Container.DataItem,"ArticleUrl").ToString() + "-" + DataBinder.Eval(Container.DataItem,"BasicId").ToString() %>.html">
                            <img class="lazy" data-original="<%# Bikewale.Utility.Image.GetPathToShowImages(DataBinder.Eval(Container.DataItem, "OriginalImgUrl").ToString(),DataBinder.Eval(Container.DataItem, "HostUrl").ToString(),Bikewale.Utility.ImageSize._310x174) %>" title="<%# DataBinder.Eval(Container.DataItem, "Title").ToString()%>" alt="<%# DataBinder.Eval(Container.DataItem, "Title").ToString()%>" src="" />
                        </a>
                    </div>
                </div>
                <div class="grid-8">
                    <a href="/expert-reviews/<%# DataBinder.Eval(Container.DataItem,"ArticleUrl").ToString() + "-" + DataBinder.Eval(Container.DataItem,"BasicId").ToString() %>.html" class="article-target-link"><%# DataBinder.Eval(Container.DataItem, "Title").ToString()%></a>
                    <p class="text-light-grey margin-bottom15"><%# Bikewale.Utility.FormatDate.GetFormatDate(DataBinder.Eval(Container.DataItem, "DisplayDate").ToString(),"MMMM dd, yyyy") %>, by <span class="text-light-grey"><%# DataBinder.Eval(Container.DataItem, "AuthorName").ToString()%></span></p>
                    <p class="line-height17">
                        <%# Bikewale.Utility.FormatDescription.TruncateDescription(DataBinder.Eval(Container.DataItem, "Description").ToString(),170) %>
                    </p>
                </div>
                <div class="clear"></div>
            </div>
        </ItemTemplate>
    </asp:Repeater>    
    <div class="padding-left10">
        <a title="<%= linkTitle %>" href="<%=MoreExpertReviewUrl %>" class="bw-ga" c="Model_Page" a="Read_all_expert_reviews_link_cliked" v="myBikeName">Read all expert reviews<span class="bwsprite blue-right-arrow-icon"></span></a>        
    </div>
</div><!-- Ends here-->


    

