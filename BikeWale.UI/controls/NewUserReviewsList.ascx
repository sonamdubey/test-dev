<%@ Control Language="C#" AutoEventWireup="false" Inherits="Bikewale.Controls.NewUserReviewsList" %>
<%if(string.IsNullOrEmpty(WidgetHeading)){ %>
<h3 class="model-section-subtitle padding-right10 padding-left10">User reviews</h3>
<%}else{%>
 <h2 class="font18 margin-bottom15"><%=WidgetHeading %></h2>
<%} %>
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
                        <a class="article-target-link line-height" title="<%#DataBinder.Eval(Container.DataItem,"ReviewTitle").ToString() %>" href="/<%# Eval("MakeMaskingName") %>-bikes/<%# Eval("ModelMaskingName") %>/user-reviews/<%# DataBinder.Eval(Container.DataItem, "ReviewId")%>.html">
                           <%#DataBinder.Eval(Container.DataItem,"ReviewTitle").ToString() %>
                        </a>
                        <div class="article-stats-left-grid">
                            <span class="bwsprite calender-grey-sm-icon"></span>
                            <span class="article-stats-content"><%#Eval("ReviewDate", "{0:dd MMMM yyyy}") %></span>
                        </div>
                        <div class="article-stats-right-grid">
                            <span class="bwsprite author-grey-sm-icon"></span>
                            <span class="article-stats-content"><%#Eval("WrittenBy").ToString() %></span>
                        </div>
                        <p class="margin-top12 line-height17">
                            <%# Bikewale.Utility.FormatDescription.TruncateDescription(DataBinder.Eval(Container.DataItem, "Comments").ToString(),200) %>
                        </p>
                    </div>
                    <div class="clear"></div>
                </div>
            </ItemTemplate>
        </asp:Repeater>
    </div>
    <div class="clear"></div>
</div>
<div class="padding-left10 view-all-btn-container">
    <a title="<%= linkTitle %>" href="/<%=MakeMaskingName%>-bikes/<%=ModelMaskingName%>/reviews/"  class="bw-ga btn view-all-target-btn" data-cat="Model_Page" data-act="Read_all_user_reviews_link_cliked" data-var="myBikeName">Read all reviews<span class="bwsprite teal-right"></span></a>   
</div>

