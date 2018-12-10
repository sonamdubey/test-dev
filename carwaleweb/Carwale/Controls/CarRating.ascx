<%@ Control Language="C#" AutoEventWireup="false" Inherits="Carwale.UI.Controls.CarRating" Codebehind="CarRating.ascx.cs" %>
        <asp:Literal ID="ltrRatingStars" runat="server"></asp:Literal>
        <span itemscope itemtype="http://schema.org/Car" class="margin-left5">
            <meta itemprop="name" content="<%=ModelDetails.MakeName %> <%= ModelDetails.ModelName%>"/>
            <span itemprop="aggregateRating" itemscope itemtype="http://schema.org/AggregateRating">
                    <meta itemprop="ratingValue" content="<%=Rating %>"/>
                    <meta itemprop="worstRating" content="1" />
                    <meta itemprop="bestRating" content="5" />
                <a href="/research/<%=Carwale.UI.Common.UrlRewrite.FormatSpecial(ModelDetails.MakeName)%>-cars/<%=ModelDetails.MaskingName%>/userreviews/">
                    <span itemprop="reviewCount">
                        <asp:Literal ID="ltrReviewCount" runat="server"></asp:Literal>
                    </span>reviews
                </a>|
                <a href=<%=Carwale.Utility.ManageCarUrl.CreateRatingPageUrl(ModelDetails.ModelId)%> >Write a review</a>
                </span>
        </span>