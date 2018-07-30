<%@ Control Language="C#" AutoEventWireup="false" Inherits="Bikewale.Controls.ComparisonMin" %>
 <% if(FetchedRecordsCount > 0) { %>
<div class="grid-6">
    <% if(TopRecord!=null) { %>
    <a href="/<%= Bikewale.Utility.UrlFormatter.CreateCompareUrl(TopRecord.MakeMaskingName1,TopRecord.ModelMaskingName1,TopRecord.MakeMaskingName2,TopRecord.ModelMaskingName2, TopRecord.VersionId1.ToString(), TopRecord.VersionId2.ToString(),TopRecord.ModelId1,TopRecord.ModelId2, Bikewale.Entities.Compare.CompareSources.Desktop_Featured_Compare_Widget) %>" title="Compare <%= FormatBikeCompareAnchorText(TopRecord.Bike1,TopRecord.Bike2)  %>" id="main-compare-bikes-target">
        <div class="grid-6 padding-left20 border-light-right">
            <div class="imageWrapper margin-bottom30">
                <div class="comparison-image">
                    <img class="lazy" data-original="<%= Bikewale.Utility.Image.GetPathToShowImages(TopRecord.VersionImgUrl1,TopRecord.HostUrl1,Bikewale.Utility.ImageSize._210x118) %>" src="" alt="Compare <%= FormatBikeCompareAnchorText(TopRecord.Bike1,TopRecord.Bike2) %>" />
                </div>
            </div>
            <h3 class="font18 text-black margin-bottom5"><%= TopRecord.Bike1 %></h3>
            <div class="text-default text-bold">
                <span class="bwsprite inr-md-lg"></span>&nbsp;<span class="font22"><%= Bikewale.Utility.Format.FormatPrice(Convert.ToString(TopRecord.Price1)) %></span>
                  <% if(TopRecord.Price1 > 0) { %>
                <span class="font16">&nbsp;onwards</span>
                 <% } %>
            </div>
            <p class="text-light-grey margin-bottom5 font14">Ex-showroom, Mumbai</p>
        </div>
        <div class="grid-6 padding-right20">
            <div class="imageWrapper margin-bottom30">
                <div class="comparison-image">
                    <img class="lazy" data-original="<%= Bikewale.Utility.Image.GetPathToShowImages(TopRecord.VersionImgUrl2,TopRecord.HostUrl2,Bikewale.Utility.ImageSize._210x118) %>" src="" alt="Compare <%= FormatBikeCompareAnchorText(TopRecord.Bike1,TopRecord.Bike2) %>" />
                </div>
            </div>
            <h3 class="font18 text-black margin-bottom5"><%= TopRecord.Bike2 %></h3>
            <div class="text-default text-bold">
                <span class="bwsprite inr-md-lg"></span>&nbsp;<span class="font22"><%= Bikewale.Utility.Format.FormatPrice(Convert.ToString(TopRecord.Price2)) %></span>
                 <% if(TopRecord.Price2 > 0) { %>
                <span class="font16">&nbsp;onwards</span>
                 <% } %>
            </div>
            <p class="text-light-grey margin-bottom5 font14">Ex-showroom, Mumbai</p>
        </div>
        <div class="clear"></div>
         <div class="margin-top20 text-center">
            <span class="btn btn-orange">Compare now</span>
        </div>
    </a>
</div>
<% } %>
<div class="grid-6 font14">
    <ul id="compare-sidebar-list">
        <%foreach(var bike in compareList) { %>
        <li>
            <a href="/<%= Bikewale.Utility.UrlFormatter.CreateCompareUrl(bike.MakeMaskingName1,bike.ModelMaskingName1,bike.MakeMaskingName2,bike.ModelMaskingName2, bike.VersionId1.ToString(), bike.VersionId2.ToString(), bike.ModelId1,bike.ModelId2, Bikewale.Entities.Compare.CompareSources.Desktop_Featured_Compare_Widget)
            %>" title="Compare <%= FormatBikeCompareAnchorText(bike.Bike1,bike.Bike2) %>" class="compare-bikes-target">
                <div class="grid-6 compare-bike-box">
                    <div class="sidebar-image inline-block">
                        <img class="lazy" data-original="<%= Bikewale.Utility.Image.GetPathToShowImages(bike.VersionImgUrl1,bike.HostUrl1,Bikewale.Utility.ImageSize._110x61) %>" src="" alt="Compare <%= FormatBikeCompareAnchorText(bike.Bike1,bike.Bike2) %>" />
                    </div>
                    <div class="sidebar-image-label inline-block text-default">
                        <h3 class="text-black text-bold margin-bottom5"><%= bike.Bike1 %></h3>
                        <span class="bwsprite inr-sm-dark"></span>&nbsp;<%= string.Format("{0}{1}",Bikewale.Utility.Format.FormatPrice(Convert.ToString(bike.Price1)),bike.Price1 > 0 ? " onwards" : string.Empty)  %> 
                    </div>
                </div>
                <div class="grid-6 compare-bike-box">
                    <div class="sidebar-image inline-block">
                        <img class="lazy" data-original="<%= Bikewale.Utility.Image.GetPathToShowImages(bike.VersionImgUrl2,bike.HostUrl2,Bikewale.Utility.ImageSize._110x61) %>" src="" alt="Compare <%= FormatBikeCompareAnchorText(bike.Bike1,bike.Bike2) %>" />
                    </div>
                    <div class="sidebar-image-label inline-block text-default">
                        <h3 class="text-black text-bold margin-bottom5"><%= bike.Bike2 %></h3>
                        <span class="bwsprite inr-sm-dark"></span>&nbsp;<%= string.Format("{0}{1}",Bikewale.Utility.Format.FormatPrice(Convert.ToString(bike.Price2)),bike.Price2 > 0 ? " onwards" : string.Empty)  %>
                    </div>
                </div>
                <div class="clear"></div>
            </a>
        </li>
        <% } %>
    </ul>
     <% if (ShowCompButton) {%>
        <div class="view-all-btn-container">
            <a href="/comparebikes/" class="btn view-all-target-btn" title="New Bike Comparisons in India">View more comparisons<span class="bwsprite teal-right"></span></a>
        </div>
    <%} %>
</div>
<div class="clear"></div>
<% } %>

