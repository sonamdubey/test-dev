<%@ Control Language="C#" AutoEventWireup="false" Inherits="Bikewale.Mobile.Controls.CompareBikeMin" %>

<% if(FetchedRecordsCount > 0) { %>
<h2 class="font18 text-center margin-top20 margin-bottom10">Popular Comparisons</h2>
<div class="content-box-shadow grid-12">
    <ul class="compare-bikes-list">
         <%foreach(var bike in compareList) { %>
        <li>
            <a href="/m/<%= Bikewale.Utility.UrlFormatter.CreateCompareUrl(bike.MakeMaskingName1,bike.ModelMaskingName1,bike.MakeMaskingName2,bike.ModelMaskingName2, bike.VersionId1.ToString(), bike.VersionId2.ToString(), bike.ModelId1,bike.ModelId2, Bikewale.Entities.Compare.CompareSources.Mobile_Featured_Compare_Widget)%>" title="Compare <%= FormatBikeCompareAnchorText(bike.Bike1,bike.Bike2) %>">
                <div class="grid-6">
                    <div class="comparison-image">
                        <img class="lazy" data-original="<%= Bikewale.Utility.Image.GetPathToShowImages(bike.VersionImgUrl1,bike.HostUrl1,Bikewale.Utility.ImageSize._110x61) %>" src="https://imgd.aeplcdn.com/0x0/bw/static/sprites/m/circleloader.gif">
                    </div>
                    <h3 class="font12 text-black margin-bottom5 padding-right10"><%= bike.Bike1 %></h3>
                    <div class="text-default text-bold">
                        <span class="bwmsprite inr-xxsm-icon"></span><span class="font16"><%= string.Format("{0}{1}",Bikewale.Utility.Format.FormatPrice(Convert.ToString(bike.Price1)),bike.Price1 > 0 ? "<span class='font14'> onwards</span>" : string.Empty)  %></span>
                    </div>
                </div>
                <div class="grid-6 padding-left15">
                    <div class="comparison-image">
                        <img class="lazy" data-original="<%= Bikewale.Utility.Image.GetPathToShowImages(bike.VersionImgUrl2,bike.HostUrl2,Bikewale.Utility.ImageSize._110x61) %>" src="https://imgd.aeplcdn.com/0x0/bw/static/sprites/m/circleloader.gif">
                    </div>
                    <h3 class="font12 text-black margin-bottom5"><%= bike.Bike2 %></h3>
                    <div class="text-default text-bold">
                        <span class="bwmsprite inr-xxsm-icon"></span><span class="font16"><%= string.Format("{0}{1}",Bikewale.Utility.Format.FormatPrice(Convert.ToString(bike.Price2)),bike.Price2 > 0 ? "<span class='font14'> onwards</span>" : string.Empty)  %></span>
                    </div>
                </div>
                <div class="clear"></div>
            </a>
        </li>
        <% }  %>
    </ul>
</div>
<% }  %>