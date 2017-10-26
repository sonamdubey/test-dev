<%@ Control Language="C#" AutoEventWireup="false" Inherits="Bikewale.Mobile.Controls.NewUserReviewList" %>
<%if(string.IsNullOrEmpty(WidgetHeading)){ %>
<h3 class="margin-top20 model-section-subtitle">User reviews</h3>
<%}else{%>
 <h2 class="margin-bottom20 model-section-subtitle"><%=WidgetHeading %></h2>
<%} %>
<div class="model-user-review-container padding20">
  <asp:Repeater ID="rptUserReview" runat="server">
    <ItemTemplate>
            <div class="margin-bottom20">
                <div class="model-user-review-rating-container">
                    <p class="text-bold"><%#Eval("OverAllRating.OverAllRating") %></p>
                    <p class="inline-block margin-bottom5 margin-top5">
                         <%# Bikewale.Utility.ReviewsRating.GetRateImage(Convert.ToDouble(Eval("OverAllRating.OverAllRating"))) %>
                    </p>
                </div>
                <div class="model-user-review-title-container">
                    <a class="target-link" href="/m/<%# Eval("MakeMaskingName") %>-bikes/<%# Eval("ModelMaskingName") %>/user-reviews/<%# DataBinder.Eval(Container.DataItem, "ReviewId")%>.html"  title="<%#DataBinder.Eval(Container.DataItem,"ReviewTitle").ToString() %>"><%# Bikewale.Utility.FormatDescription.TruncateDescription(Eval("ReviewTitle").ToString(),44) %></a>
                    <div class="grid-7 alpha padding-right5">
                        <span class="bwmsprite calender-grey-sm-icon"></span>
                        <span class="article-stats-content"><%#Eval("ReviewDate", "{0:dd MMMM yyyy}") %></span>
                    </div>
                    <div class="grid-5 alpha omega">
                        <span class="bwmsprite author-grey-sm-icon"></span>
                        <span class="article-stats-content"><%#Eval("WrittenBy").ToString() %></span>
                    </div>
                    <div class="clear"></div>
                </div>
                
                <p class="margin-top10"><%# Bikewale.Utility.FormatDescription.TruncateDescription(DataBinder.Eval(Container.DataItem, "Comments").ToString(),180) %></p>
            </div>
    </ItemTemplate>
  </asp:Repeater>
</div>
<div class="view-all-btn-container">
    <a title="<%= linkTitle %>" class="bw-ga btn view-all-target-btn" href="/m/<%=MakeMaskingName%>-bikes/<%=ModelMaskingName%>/user-reviews/"  data-cat="Model_Page" data-act="Read_all_user_reviews_link_cliked" data-var="myBikeName">Read all reviews<span class="bwmsprite teal-right"></span></a>
</div>