﻿<%@ Control Language="C#" AutoEventWireup="false" Inherits="Bikewale.controls.UserReviewsList" %>
<asp:Repeater ID="rptUserReview" runat="server">
    <ItemTemplate>
        <div class="padding-bottom20 font14">
                <div class="grid-2">
                    <div class="content-inner-block-5 border-solid text-center">
                        <p class="inline-block margin-bottom5 margin-top5">
                            <%# Bikewale.Utility.ReviewsRating.GetRateImage(Convert.ToDouble(Eval("OverAllRating.OverAllRating"))) %>
                        </p>
                        <p><%#Eval("OverAllRating.OverAllRating") %></p>
                    </div>
                </div>
                <div class="grid-10">
                    <p class="margin-bottom5 font18 text-bold"><%#Eval("ReviewTitle").ToString() %> <span class="font14 text-unbold text-light-grey margin-left5"><%#Eval("ReviewDate").ToString() %>, by <%#Eval("WrittenBy").ToString() %></span></p>
                    <p><%#Eval("Comments").ToString() %>...<a href="/<%# Eval("MakeMaskingName") %>-bikes/<%# Eval("ModelMaskingName") %>/user-reviews/<%# DataBinder.Eval(Container.DataItem, "ReviewId")%>.html">Read full story</a></p>
                </div>
            <div class="clear"></div>
        </div>

    </ItemTemplate>
</asp:Repeater>
