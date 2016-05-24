<%@ Control Language="C#" AutoEventWireup="false" Inherits="Bikewale.Controls.NewUserReviewsList" %>

<h3 class="margin-top25 padding-left20 model-section-subtitle">User reviews</h3>
<div class="bw-tabs-data" id="ctrlUserReviews"><!-- Reviews data code starts here-->
    <div class="user-reviews">
    </div>
</div>


<div class="model-user-review-container grid-12">
    <% if(FetchedRecordsCount == 1){ %>
    <div class="grid-12 margin-bottom15">
        <!-- when one review -->
        <div class="model-user-review-rating-container leftfloat">
            <p>4</p>
            <p class="inline-block margin-bottom5 margin-top5">
                <span class="star-one-icon"></span><span class="star-one-icon"></span><span class="star-one-icon"></span><span class="star-one-icon"></span><span class="star-zero-icon"></span>
            </p>
        </div>
        <div class="model-single-user-review padding-left20 leftfloat">
            <h3>not upto the mark compared to its price</h3>
            <p class="text-light-grey">April 15, 2016, by Parth Shukla</p>
            <p class="margin-top10 line-height17">
                Style Good. Engine Performance, Fuel Economy and Gearbox Good. Ride Quality & Handling Good. Areas of improve...
                                        <a href="">Read full story</a>
            </p>
        </div>
        <div class="clear"></div>
    </div>
    <% } else { %>
    <asp:Repeater ID="rptUserReview" runat="server">
        <ItemTemplate>
            <div class="grid-6 margin-bottom15">
                <div class="model-user-review-rating-container leftfloat">
                    <p><%#Eval("OverAllRating.OverAllRating") %></p>
                    <p class="inline-block margin-bottom5 margin-top5">
                        <%# Bikewale.Utility.ReviewsRating.GetRateImage(Convert.ToDouble(Eval("OverAllRating.OverAllRating"))) %>
                    </p>
                </div>
                <div class="model-user-review-title-container padding-left20 leftfloat">
                    <h3><%#Eval("ReviewTitle").ToString() %></h3>
                    <p class="text-light-grey"><%#Eval("ReviewDate", "{0:dd-MMM-yyyy}") %> by <%#Eval("WrittenBy").ToString() %></p>
                </div>
                <div class="clear"></div>
                <p class="margin-top20 line-height17">
                    <%#Eval("Comments").ToString() %> ... 
             <a href="/<%# Eval("MakeMaskingName") %>-bikes/<%# Eval("ModelMaskingName") %>/user-reviews/<%# DataBinder.Eval(Container.DataItem, "ReviewId")%>.html">Read full story</a>
                </p>
            </div>
        </ItemTemplate>
    </asp:Repeater>
    <% } %>
</div>
<div class="clear"></div>
<div class="padding-left20">
    <a href="/<%=MakeMaskingName%>-bikes/<%=ModelMaskingName%>/user-reviews/">Read all user reviews<span class="bwsprite blue-right-arrow-icon"></span></a>
</div>

