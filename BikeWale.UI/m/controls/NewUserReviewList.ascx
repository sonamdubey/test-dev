<%@ Control Language="C#" AutoEventWireup="false" Inherits="Bikewale.Mobile.Controls.NewUserReviewList" %>

<h3 class="margin-top20 model-section-subtitle">User reviews</h3>
<div class="model-user-review-container">

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
                    <h4><%#Eval("ReviewTitle").ToString() %> </h4>
                    <p class="font12 text-truncate text-light-grey">April 15, 2016, by Parth Shukla</p>
                </div>
                <p class="margin-top17"><%#Eval("Comments").ToString() %> ...
                    <a href="/m/<%# Eval("MakeMaskingName") %>-bikes/<%# Eval("ModelMaskingName") %>/user-reviews/<%# DataBinder.Eval(Container.DataItem, "ReviewId")%>.html">Read full review</a>
                </p>
            </div>
    </ItemTemplate>
  </asp:Repeater>
</div>
<div>
    <a class="font16" href="/m/<%=MakeMaskingName%>-bikes/<%=ModelMaskingName%>/user-reviews/">Read all user reviews<span class="bwmsprite blue-right-arrow-icon"></span></a>
</div>