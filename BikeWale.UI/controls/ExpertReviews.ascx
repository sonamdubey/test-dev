﻿<%@ Control Language="C#" AutoEventWireup="false" Inherits="Bikewale.Controls.ExpertReviews" %>
<div class="bw-tabs-data news-expert-video-content" id="ctrlExpertReviews"><!-- Reviews data code starts here-->
    <asp:Repeater ID="rptExpertReviews" runat="server">
        <ItemTemplate>
            <div class="padding-bottom20">
                <div class="grid-4">
                    <div class="img-preview">
                        <a href="<%# Bikewale.Utility.UrlFormatter.GetArticleUrl(Convert.ToString(DataBinder.Eval(Container.DataItem,"BasicId")),Convert.ToString(DataBinder.Eval(Container.DataItem,"ArticleUrl")),Bikewale.Entities.CMS.EnumCMSContentType.RoadTest.ToString()) %>"><img class="lazy" data-original="<%# Bikewale.Utility.Image.GetPathToShowImages(DataBinder.Eval(Container.DataItem, "OriginalImgUrl").ToString(),DataBinder.Eval(Container.DataItem, "HostUrl").ToString(),Bikewale.Utility.ImageSize._310x174) %>" title="<%# DataBinder.Eval(Container.DataItem, "Title").ToString()%>" alt="<%# DataBinder.Eval(Container.DataItem, "Title").ToString()%>" src=""></a>
                    </div>
                </div>
                <div class="grid-8 padding-top5 font14">
                    <a href="<%# Bikewale.Utility.UrlFormatter.GetArticleUrl(Convert.ToString(DataBinder.Eval(Container.DataItem,"BasicId")),Convert.ToString(DataBinder.Eval(Container.DataItem,"ArticleUrl")),Bikewale.Entities.CMS.EnumCMSContentType.RoadTest.ToString()) %>" class="article-target-link margin-bottom10"><%# DataBinder.Eval(Container.DataItem, "Title").ToString()%></a>
                    <p class="margin-bottom10 text-light-grey"><%# Bikewale.Utility.FormatDate.GetFormatDate(DataBinder.Eval(Container.DataItem, "DisplayDate").ToString(),"MMMM dd, yyyy") %>, by <span class="text-light-grey"><%# DataBinder.Eval(Container.DataItem, "AuthorName").ToString()%></span></p>
                    <p class="line-height"><%# Bikewale.Utility.FormatDescription.TruncateDescription(DataBinder.Eval(Container.DataItem, "Description").ToString()) %></p>
                </div>
                <div class="clear"></div>
            </div>
        </ItemTemplate>
    </asp:Repeater>    
    <div class="padding-bottom30 text-center">
        <a href="<%=MoreExpertReviewUrl%>" class="font16">View more reviews</a>
    </div>
</div><!-- Ends here-->