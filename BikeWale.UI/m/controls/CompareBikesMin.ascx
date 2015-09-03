<%@ Control Language="C#" AutoEventWireup="false" Inherits="Bikewale.m.controls.CompareBikesMin" %>
<div class="container bg-white">
    <h2 class="text-center padding-top30 margin-bottom20">Compare Now</h2>
    <div class="grid-12 margin-bottom10">
        <h3 class="font16 text-center padding-top20 padding-bottom15">
            <a href="<%= FormatComparisonUrl(TopRecord.MakeMaskingName1,TopRecord.ModelMaskingName1,TopRecord.MakeMaskingName2,TopRecord.ModelMaskingName2)%>" class="text-grey">
                <%= FormatBikeCompareAnchorText(TopRecord.Bike1,TopRecord.Bike2) %>
            </a>
        </h3>
        <div class="bike-preview margin-bottom25">
            <img src="<%= TopCompareImage %>" title="CompareBike" alt="CompareBike">
        </div>

        <div class="clear">
            <div class="grid-6 alpha">
                <div class="content-inner-block-15 beta gamma">
                    <div class="font16 margin-bottom10 padding-left10">
                        <span class="fa fa-rupee"></span><strong class="font18"><%= Bikewale.Utility.Format.FormatPrice(TopRecord.Price1.ToString()) %></strong>
                    </div>
                    <div>
                        <span class="margin-bottom10">
                            <%= Bikewale.Utility.ReviewRating.GetRateImage(Convert.ToDouble(TopRecord.Review1)) %>
                        </span>
                    </div>
                    <div>
                        <a class="margin-left5" href="<%= Bike1ReviewLink %>"><%= Bike1ReviewText %></a>
                    </div>
                </div>
            </div>
            <div class="grid-6 omega border-left1">
                <div class="content-inner-block-15 beta gamma">
                    <div class="font16 margin-bottom10 padding-left10">
                        <span class="fa fa-rupee"></span><strong class="font18"><%= Bikewale.Utility.Format.FormatPrice(TopRecord.Price2.ToString()) %></strong>
                    </div>
                    <div>
                        <span class="margin-bottom10">
                            <%= Bikewale.Utility.ReviewRating.GetRateImage(Convert.ToDouble(TopRecord.Review2)) %>
                        </span>
                    </div>
                    <div>
                        <a class="margin-left5" href="<%= Bike2ReviewLink %>"><%= Bike2ReviewText %></a>
                    </div>
                </div>
            </div>
            <div class="clear"></div>
        </div>

        <div class="font16 text-center padding-top20 padding-bottom15">
            <a href="/m/comparebikes/">View more Comparisons</a>
        </div>
    </div>
    <div class="clear"></div>
</div>
