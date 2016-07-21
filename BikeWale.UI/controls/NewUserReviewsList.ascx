<%@ Control Language="C#" AutoEventWireup="false" Inherits="Bikewale.Controls.NewUserReviewsList" %>

<h3 class="model-section-subtitle padding-right10 padding-left10">User reviews</h3>
<div class="bw-tabs-data" id="ctrlUserReviews"><!-- Reviews data code starts here-->
    <div class="model-user-review-container grid-12 alpha omega">
        <asp:Repeater ID="rptUserReview" runat="server">
            <ItemTemplate>
                <div class="grid-<%=CssWidth %> margin-bottom15">
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
                        <p class="text-light-grey"><%#Eval("ReviewDate", "{0:dd-MMM-yyyy}") %> by <%#Eval("WrittenBy").ToString() %></p>
                        <%if (FetchedRecordsCount == 1)
                          { %>
                            <p class="margin-top15 line-height17">
                                <%# Bikewale.Utility.FormatDescription.TruncateDescription(DataBinder.Eval(Container.DataItem, "Comments").ToString(),160) %>
                            </p>
                        <%} %>
                    </div>
                    <div class="clear"></div>
                    <%if (FetchedRecordsCount != 1)
                       { %>
                        <p class="margin-top20 line-height17">
                            <%# Bikewale.Utility.FormatDescription.TruncateDescription(DataBinder.Eval(Container.DataItem, "Comments").ToString(),160) %>
                        </p>
                    <%} %>
                </div>
            </ItemTemplate>
        </asp:Repeater>
    </div>
    <div class="clear"></div>
</div>
<div class="padding-left10">
    <a href="/<%=MakeMaskingName%>-bikes/<%=ModelMaskingName%>/user-reviews/"  class="bw-ga" c="Model_Page" a="Read_all_user_reviews_link_cliked" v="myBikeName">Read all user reviews<span class="bwsprite blue-right-arrow-icon"></span></a>   
</div>

