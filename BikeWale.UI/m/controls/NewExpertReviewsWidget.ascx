﻿<%@ Control Language="C#" AutoEventWireup="false" Inherits="Bikewale.Mobile.Controls.NewExpertReviewsWidget" %>

<h3 class="model-section-subtitle">Expert reviews</h3>
<asp:Repeater ID="rptExpertReviews" runat="server">
    <ItemTemplate>
        <div class="model-expert-review-container">
            <div class="margin-bottom20">
                <div class="review-image-wrapper">
                    <a href="/m/road-tests/<%# DataBinder.Eval(Container.DataItem,"ArticleUrl").ToString() + "-" + DataBinder.Eval(Container.DataItem,"BasicId").ToString() %>.html">
                        <img alt="<%# DataBinder.Eval(Container.DataItem, "Title").ToString()%>" title="<%# DataBinder.Eval(Container.DataItem, "Title").ToString()%>" src="<%# Bikewale.Utility.Image.GetPathToShowImages(DataBinder.Eval(Container.DataItem, "OriginalImgUrl").ToString(),DataBinder.Eval(Container.DataItem, "HostUrl").ToString(),Bikewale.Utility.ImageSize._370x208) %>" />
                    </a>
                </div>
                <div class="review-heading-wrapper">
                    <h4>
                        <a href="/m/road-tests/<%# DataBinder.Eval(Container.DataItem,"ArticleUrl").ToString() + "-" + DataBinder.Eval(Container.DataItem,"BasicId").ToString() %>.html" class="font14 text-black"><%# DataBinder.Eval(Container.DataItem, "Title").ToString()%></a>
                    </h4>
                    <p class="font12 text-truncate text-light-grey"><%# Bikewale.Utility.FormatDate.GetFormatDate(DataBinder.Eval(Container.DataItem, "DisplayDate").ToString(), "MMMM dd, yyyy") %>, by <span class="text-light-grey"><%# DataBinder.Eval(Container.DataItem, "AuthorName").ToString()%></span></p>
                </div>
                <p class="margin-top17">
                    <%# DataBinder.Eval(Container.DataItem, "Description").ToString()%>
                    <a href="/m/road-tests/<%# DataBinder.Eval(Container.DataItem,"ArticleUrl").ToString() + "-" + DataBinder.Eval(Container.DataItem,"BasicId").ToString() %>.html">Read full review</a>
                </p>
            </div>
        </div>
    </ItemTemplate>
</asp:Repeater>
<div>
    <a href="<%=MoreExpertReviewUrl%>"  class="bw-ga" c="Model_Page" a="Read_all_expert_reviews_link_cliked" v="myBikeName">Read all expert reviews<span class="bwmsprite blue-right-arrow-icon"></span></a>
</div>
<script type="text/javascript">
    $(document).ready(function () { $("img.lazy").lazyload(); });
</script>



