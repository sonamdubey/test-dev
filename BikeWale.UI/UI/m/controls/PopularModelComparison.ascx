<%@ Control Language="C#" AutoEventWireup="false" CodeBehind="PopularModelComparison.ascx.cs" Inherits="Bikewale.Mobile.Controls.PopularModelComparison" %>
<% if(fetchedCount > 0 && objSimilarBikes!=null) { %>
<div id="ctrlCompareBikes">
    <div id="comparisonSwiper" class="swiper-container comparison-swiper card-container padding-bottom25">
        <div class="swiper-wrapper">
            <% foreach(var bike in  objSimilarBikes) { %>
                <div class="swiper-slide">
                    <div class="swiper-card rounded-corner2">
                        <a href="/m/<%= Bikewale.Utility.UrlFormatter.CreateCompareUrl(bike.MakeMasking1,bike.ModelMasking1,bike.MakeMasking2,bike.ModelMasking2,bike.VersionId1,bike.VersionId2, bike.ModelId1,bike.ModelId2, Bikewale.Entities.Compare.CompareSources.Mobile_Model_MostPopular_Compare_Widget) %>" title ="<%= Bikewale.Utility.UrlFormatter.CreateCompareTitle(bike.Model1,bike.Model2) %>" class="block">
                        <% if(SponsoredVersionId == Convert.ToUInt32(bike.VersionId2)) { %>  <span class="text-default position-abt pos-top5 pos-right5 font12">Sponsored</span>   <% } %>
                        <h3 class="font12 text-black text-center margin-bottom10"><%= Bikewale.Utility.UrlFormatter.CreateCompareTitle(bike.Model1,bike.Model2) %></h3>
                        <div class="grid-6">
                            <div class="model-img-content">
                                <img class="swiper-lazy" data-src="<%= Bikewale.Utility.Image.GetPathToShowImages(bike.OriginalImagePath1,bike.HostUrl1,Bikewale.Utility.ImageSize._144x81) %>" alt="<%= bike.Model1 %>" title="<%= bike.Model1 %>" src="" />
                                <span class="swiper-lazy-preloader"></span>
                            </div>
                            <p class="font11 text-light-grey text-truncate">Ex-showroom, <%= bike.City1  %></p>
                            <span class="bwmsprite inr-xsm-icon"></span>&nbsp;<span class="font16 text-default text-bold"><%= Bikewale.Utility.Format.FormatPrice(bike.Price1.ToString())  %></span>
                        </div>
                        <div class="grid-6">
                            <div class="model-img-content">
                                <img class="swiper-lazy" data-src="<%= Bikewale.Utility.Image.GetPathToShowImages(bike.OriginalImagePath2,bike.HostUrl2,Bikewale.Utility.ImageSize._144x81) %>" alt="<%= bike.Model2 %>" title="<%= bike.Model2 %>" src="" />
                                <span class="swiper-lazy-preloader"></span>
                            </div>
                            <p class="font11 text-light-grey text-truncate">Ex-showroom, <%= bike.City2  %></p>
                            <span class="bwmsprite inr-xsm-icon"></span>&nbsp;<span class="font16 text-default text-bold"><%= Bikewale.Utility.Format.FormatPrice(bike.Price2.ToString())  %></span>
                            <% if (SponsoredVersionId == Convert.ToUInt32(bike.VersionId2) && !string.IsNullOrEmpty(FeaturedBikeLink) )
                                { %> <br /> <span class="text-truncate font12 block" data-href="<%= FeaturedBikeLink %>" title="View <%= bike.Model2  %> details on <%=bike.Make2 %>'s site"  id="sponsored-comparebike-link">More info at <%=bike.Make2 %> auto</span>   <% } %>
                        </div>
                        <div class="clear"></div>
                        <div class="margin-top15 text-center">
                            <span class="btn btn-white btn-size-1">Compare now</span>
                        </div>
                        </a>
                    </div>
                </div>
            <% } %>
        </div>
    </div>
</div>
<div class="margin-right20 margin-left20 border-solid-bottom"></div>

<% if (SponsoredVersionId > 0 && !string.IsNullOrEmpty(FeaturedBikeLink))
    { %>
<script type="text/javascript">
    $("#sponsored-comparebike-link").click(function (e) {_sponsoredLink = $(this).attr("data-href"); window.open(_sponsoredLink, '_blank').focus();return false;})
</script>
<% } %>

<% }  %>
