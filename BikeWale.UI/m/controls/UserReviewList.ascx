<%@ Control Language="C#" AutoEventWireup="false" Inherits="Bikewale.Mobile.Controls.UserReviewList" %>
<div class="bw-tabs-data" id="ctrlUserReviews">
    <div class="swiper-container padding-bottom60">
         <div class="swiper-wrapper">
                <asp:Repeater ID="rptUserReview" runat="server">
                    <ItemTemplate>
                        <div class="swiper-slide">
                            <div class="front padding-bottom20">
                                <div class="contentWrapper content-inner-block-10">
                                    <div class="grid-12 alpha omega padding-top10">
                                        <div class="reviews-rating leftfloat text-center text-center border-solid margin-bottom5 margin-right10">
                                            <span class="font14 text-dark-grey"><%#Eval("OverAllRating.OverAllRating") %></span>
                                            <span class="margin-bottom5 margin-top5 show">
                                                <%# Bikewale.Utility.ReviewsRating.GetRateImage(Convert.ToDouble(Eval("OverAllRating.OverAllRating"))) %>
                                            </span>
                                        </div>
                                        <h4 class="font16"><%#Eval("ReviewTitle").ToString() %> </h4>
                                    </div>

                                    <div class="font12 text-grey grid-12 margin-bottom15">on <%#Eval("ReviewDate", "{0:dd-MMM-yyyy}") %> by <%#Eval("WrittenBy").ToString() %></div>
                                    <p class="font14 grid-12"><%#Eval("Comments").ToString() %> ...
                                        <a href="/m/<%# Eval("MakeMaskingName") %>-bikes/<%# Eval("ModelMaskingName") %>/user-reviews/<%# DataBinder.Eval(Container.DataItem, "ReviewId")%>.html" >Read full story</a>
                                    </p>
                                </div>
                                <div class="clear"></div>
                            </div>
                        </div>
                    </ItemTemplate>
                </asp:Repeater>

        </div>
        <!-- Add Pagination -->
        <div class="swiper-pagination"></div>
        <!-- Navigation -->
        <div class="bwmsprite swiper-button-next hide"></div>
        <div class="bwmsprite swiper-button-prev hide"></div>
    </div>
    <div class="text-center">
        <a class="font16" href="/m/<%=MakeMaskingName%>-bikes/<%=ModelMaskingName%>/user-reviews/">View more reviews</a>
    </div>
</div>
