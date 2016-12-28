<%@ Control Language="C#" AutoEventWireup="false" Inherits="Bikewale.Mobile.Controls.CompareBikesMin" %>

<div class="container">
    <h2 class="font18 text-center margin-top20 margin-bottom10">Compare bikes</h2>
    <div class="grid-12 content-box-shadow padding-top20 padding-bottom10">
        <a href="/m/<%= Bikewale.Utility.UrlFormatter.CreateCompareUrl(TopRecord.MakeMaskingName1,TopRecord.ModelMaskingName1,TopRecord.MakeMaskingName2,TopRecord.ModelMaskingName2,TopRecord.VersionId1.ToString(),TopRecord.VersionId2.ToString(),Bikewale.Entities.Compare.CompareSources.Mobile_Featured_Compare_Widget) %>" title="Compare <%= FormatBikeCompareAnchorText(TopRecord.Bike1,TopRecord.Bike2) %>" id="main-compare-bikes-target">
            <div class="grid-6">
                <div class="comparison-image">
                    <img class="lazy" data-original="<%= Bikewale.Utility.Image.GetPathToShowImages(TopRecord.VersionImgUrl1,TopRecord.HostUrl1,Bikewale.Utility.ImageSize._210x118) %>" src="https://imgd3.aeplcdn.com/0x0/bw/static/sprites/m/circleloader.gif" />
                </div>
                <h3 class="font14 text-black margin-bottom5"><%= TopRecord.Bike1 %></h3>
                <div class="text-default text-bold">
                    <span class="bwmsprite inr-xsm-icon"></span>&nbsp;<span class="font16"><%= Bikewale.Utility.Format.FormatPrice(Convert.ToString(TopRecord.Price1))  %></span>
                    <% if(TopRecord.Price1 > 0) { %>
                    <span class="font12">&nbsp;onwards</span>
                    <% } %>
                </div>
                <p class="text-light-grey margin-bottom5 font12">Ex-showroom, Mumbai</p>
            </div>
            <div class="grid-6">
                <div class="comparison-image">
                    <img class="lazy" data-original="<%= Bikewale.Utility.Image.GetPathToShowImages(TopRecord.VersionImgUrl2,TopRecord.HostUrl2,Bikewale.Utility.ImageSize._210x118) %>" src="https://imgd3.aeplcdn.com/0x0/bw/static/sprites/m/circleloader.gif" />
                </div>
                <h3 class="font14 text-black margin-bottom5"><%= TopRecord.Bike2 %></h3>
                <div class="text-default text-bold">
                    <span class="bwmsprite inr-xsm-icon"></span>&nbsp;<span class="font16"><%= Bikewale.Utility.Format.FormatPrice(Convert.ToString(TopRecord.Price2)) %></span>
                        <% if(TopRecord.Price2 > 0) { %>
                    <span class="font12">&nbsp;onwards</span>
                    <% } %>
                </div>
                <p class="text-light-grey margin-bottom5 font12">Ex-showroom, Mumbai</p>
            </div>
            <div class="clear"></div>
            <div class="margin-top10 text-center">
                <span class="btn btn-orange">Compare now</span>
            </div>
        </a>
        <div class="text-center margin-bottom10">
            <a href="/m/comparebikes/" class="font14">View more comparisons</a>
        </div>
    </div>
    <div class="clear">
</div>
    <div class="clear"></div>
</div>
