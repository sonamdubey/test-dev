<%@ Control Language="C#" AutoEventWireup="false" CodeBehind="PopularModelComparison.ascx.cs" Inherits="Bikewale.Mobile.Controls.PopularModelComparison" %>
<% if(fetchedCount > 0 && objSimilarBikes!=null) { %>
<div id="ctrlCompareBikes">
    <div id="makeComparisonContent" class="bw-model-tabs-data padding-top15 padding-bottom20 font14">
        <h2 class="padding-left20 padding-right20 margin-bottom20">Popular Comparisons for <%=versionName %> </h2>
        <div class="swiper-container padding-top5 padding-bottom5">
            <div class="swiper-wrapper model-comparison-list">
                <% foreach(var bike in  objSimilarBikes) { %>
                        <div class="swiper-slide">
                            <a href="/m/<%= Bikewale.Utility.UrlFormatter.CreateCompareUrl(bike.MakeMasking1,bike.ModelMasking1,bike.MakeMasking2,bike.ModelMasking2,bike.VersionId1,bike.VersionId2) %>" title ="<%= Bikewale.Utility.UrlFormatter.CreateCompareTitle(bike.Model1,bike.Model2) %>">
                            <h3 class="font12 text-black text-center margin-top10"><%= Bikewale.Utility.UrlFormatter.CreateCompareTitle(bike.Model1,bike.Model2) %></h3>
                                <% if(SponsoredVersionId == Convert.ToUInt32(bike.VersionId2)) { %>  <span class="content-inner-block-5 text-default position-abt font12" style="right:0;top:0">Sponsored</span>   <% } %>
                            <div class="grid-6 padding-bottom20">
                                <div class="model-img-content">
                                    <img class="swiper-lazy" src="<%= Bikewale.Utility.Image.GetPathToShowImages(bike.OriginalImagePath1,bike.HostUrl1,Bikewale.Utility.ImageSize._310x174) %>" alt="<%= bike.Model1 %>" title="<%= bike.Model1 %>" />
                                    <span class="swiper-lazy-preloader"></span>
                                </div>
                                <p class="font11 text-light-grey margin-bottom5 block text-truncate">Ex-showroom, <%= bike.City1  %></p>
                                <span class="bwmsprite inr-dark-md-icon"></span>&nbsp;<span class="font16 text-default text-bold"><%= Bikewale.Utility.Format.FormatPrice(bike.Price1.ToString())  %></span>
                            </div>
                            <div class="grid-6">
                                <div class="model-img-content">
                                    <img class="swiper-lazy" src="<%= Bikewale.Utility.Image.GetPathToShowImages(bike.OriginalImagePath2,bike.HostUrl2,Bikewale.Utility.ImageSize._310x174) %>" alt="<%= bike.Model2 %>" title="<%= bike.Model2 %>" />
                                    <span class="swiper-lazy-preloader"></span>
                                </div>
                                <p class="font11 text-light-grey margin-bottom5 block text-truncate">Ex-showroom, <%= bike.City2  %></p>
                                <span class="bwmsprite inr-dark-md-icon"></span>&nbsp;<span class="font16 text-default text-bold"><%= Bikewale.Utility.Format.FormatPrice(bike.Price2.ToString())  %></span>
                                <% if (SponsoredVersionId == Convert.ToUInt32(bike.VersionId2) && !string.IsNullOrEmpty(FeaturedBikeLink) )
                                   { %> <br /> <span class="text-truncate font12 block" data-href="<%= FeaturedBikeLink %>" title="View <%= bike.Model2  %> details on <%=bike.Make2 %>'s site"  id="sponsored-comparebike-link">More info at <%=bike.Make2 %> auto</span>   <% } %>
                            </div>
                            <div class="clear"></div>
                            <div class="margin-top10 text-center">
                                <span class="btn btn-white btn-size-1">Compare now</span>
                            </div>
                            </a>
                        </div>
                   <% } %>
            </div>
        </div>
        <div class="margin-top15 margin-left20">
            <a href="/comparebikes/">View more comparisons<span class="bwmsprite blue-right-arrow-icon"></span></a>
        </div>
    </div>
    <div class="margin-right20 margin-left20 border-solid-bottom"></div>
</div>

    <% if (SponsoredVersionId > 0 && !string.IsNullOrEmpty(FeaturedBikeLink))
       { %>
    <script type="text/javascript">
        $("#sponsored-comparebike-link").click(function (e) {_sponsoredLink = $(this).attr("data-href"); window.open(_sponsoredLink, '_blank').focus();return false;})
    </script>
    <% } %>

<% }  %>
