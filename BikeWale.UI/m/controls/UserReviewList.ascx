<%@ Control Language="C#" AutoEventWireup="false" Inherits="Bikewale.Mobile.Controls.UserReviewList" %>
<div class="bw-tabs-data" id="ctrlUserReviews">
    <div class="jcarousel-wrapper">
        <div class="jcarousel">
            <ul>
                <asp:Repeater ID="rptUserReview" runat="server">
                    <ItemTemplate>
                        <li>
                            <div class="front padding-bottom20">
                                <div class="contentWrapper content-inner-block-10">
                                    <div class="grid-12 alpha omega padding-top10">
                                        <div class="reviews-rating leftfloat text-center text-center border-solid margin-bottom5 margin-right10">
                                            <span class="margin-bottom5 margin-top5 show">
                                                <%# Bikewale.Utility.ReviewsRating.GetRateImage(Convert.ToDouble(Eval("OverAllRating.OverAllRating"))) %>
                                            </span>
                                            <span class="font14 text-dark-grey"><%#Eval("OverAllRating.OverAllRating") %></span>
                                        </div>
                                        <span class="font16 text-bold"><%#Eval("ReviewTitle").ToString() %> </span>
                                    </div>

                                    <div class="font12 text-grey grid-12 margin-bottom15"><%#Eval("ReviewDate").ToString() %>, by <%#Eval("WrittenBy").ToString() %></div>
                                    <p class="font14 grid-12"><%#Eval("Comments").ToString() %>...
                                        <a href="/m/<%# Eval("MakeMaskingName") %>-bikes/<%# Eval("ModelMaskingName") %>/user-reviews/<%# DataBinder.Eval(Container.DataItem, "ReviewId")%>.html" class="text-bold">Read full story</a>
                                    </p>
                                </div>
                                <div class="clear"></div>
                            </div>
                        </li>
                    </ItemTemplate>
                </asp:Repeater>

            </ul>
        </div>
        <span class="jcarousel-control-left"><a href="javascript:void(0)" class="bwmsprite jcarousel-control-prev"></a></span>
        <span class="jcarousel-control-right"><a href="javascript:void(0)" class="bwmsprite jcarousel-control-next"></a></span>
        <p class="text-center jcarousel-pagination margin-bottom30"></p>
    </div>
    <div class="text-center">
        <a class="font16" href="/m/<%=MakeMaskingName%>-bikes/<%=ModelMaskingName%>/user-reviews/">View more reviews</a>
    </div>
</div>
