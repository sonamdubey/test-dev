<%@ Control Language="C#" AutoEventWireup="false" Inherits="Bikewale.Controls.UserReviewsList" %>
<div class="bw-tabs-data" id="ctrlUserReviews"><!-- Reviews data code starts here-->
    <div class="user-reviews">
        <asp:Repeater ID="rptUserReview" runat="server">
            <ItemTemplate>
                <div class="padding-bottom20 font14">
                        <div class="grid-2">
                            <div class="content-inner-block-5 border-solid text-center">                        
                        <p><%#Eval("OverAllRating.OverAllRating") %></p>
                                <p class="inline-block margin-bottom5 margin-top5">
                                    <%# Bikewale.Utility.ReviewsRating.GetRateImage(Convert.ToDouble(Eval("OverAllRating.OverAllRating"))) %>
                                </p>                               
                            </div>
                        </div>
                        <div class="grid-10">
                    <p class="margin-bottom5 font18"><%#Eval("ReviewTitle").ToString() %> <span class="font14 text-unbold text-light-grey margin-left5">on <%#Eval("ReviewDate", "{0:dd-MMM-yyyy}") %> by <%#Eval("WrittenBy").ToString() %></span></p>
                    <p><%#Eval("Comments").ToString() %> ... 
                           <a href="/<%# Eval("MakeMaskingName") %>-bikes/<%# Eval("ModelMaskingName") %>/user-reviews/<%# DataBinder.Eval(Container.DataItem, "ReviewId")%>.html">Read full story</a></p>
                        </div>
                    <div class="clear"></div>
                </div>
            </ItemTemplate>
        </asp:Repeater>
        <div class="padding-bottom30 text-center">
            <a href="/<%=MakeMaskingName%>-bikes/<%=ModelMaskingName%>/user-reviews/" class="font16">View more reviews</a>
        </div>
    </div>
</div>
