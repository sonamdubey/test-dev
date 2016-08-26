<%@ Control Language="C#" AutoEventWireup="false" Inherits="Bikewale.Controls.NewUserReviewsList" %>

<h3 class="model-section-subtitle padding-right10 padding-left10">User reviews</h3>
<div class="bw-tabs-data" id="ctrlUserReviews"><!-- Reviews data code starts here-->
    <div class="model-user-review-container">
        <asp:Repeater ID="rptUserReview" runat="server">
            <ItemTemplate>
                <div class="grid-12 margin-bottom20">
                    <div class="model-user-review-rating-container leftfloat">
                        <p><%#Eval("OverAllRating.OverAllRating") %></p>
                        <p class="inline-block margin-bottom5 margin-top5">
                            <%# Bikewale.Utility.ReviewsRating.GetRateImage(Convert.ToDouble(Eval("OverAllRating.OverAllRating"))) %>
                        </p>
                    </div>
                    <div class="model-user-review-title-container">
                        <a class="article-target-link line-height" href="/<%# Eval("MakeMaskingName") %>-bikes/<%# Eval("ModelMaskingName") %>/user-reviews/<%# DataBinder.Eval(Container.DataItem, "ReviewId")%>.html">
                            <%#Eval("ReviewTitle").ToString() %>
                        </a>
                        <div class="article-stats-left-grid">
                            <span class="bwsprite calender-grey-sm-icon"></span>
                            <span class="article-stats-content"><%#Eval("ReviewDate", "{0:MMM dd, yyyy}") %></span>
                        </div>
                        <div class="article-stats-right-grid">
                            <span class="bwsprite author-grey-sm-icon"></span>
                            <span class="article-stats-content"><%#Eval("WrittenBy").ToString() %></span>
                        </div>
                        <p class="margin-top12 line-height17">
                            <%# Bikewale.Utility.FormatDescription.TruncateDescription(DataBinder.Eval(Container.DataItem, "Comments").ToString(),200) %> ...
                        </p>
                    </div>
                    <div class="clear"></div>
                </div>
            </ItemTemplate>
        </asp:Repeater>
    </div>
    <div class="clear"></div>
</div>
<div class="padding-left10">
    <a title="<%= linkTitle %>" href="/<%=MakeMaskingName%>-bikes/<%=ModelMaskingName%>/user-reviews/"  class="bw-ga" c="Model_Page" a="Read_all_user_reviews_link_cliked" v="myBikeName">Read all user reviews<span class="bwsprite blue-right-arrow-icon"></span></a>   
</div>

