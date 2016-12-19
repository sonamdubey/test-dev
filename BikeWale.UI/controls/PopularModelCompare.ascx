<%@ Control Language="C#" AutoEventWireup="false" Inherits="Bikewale.Controls.PopularModelCompare" %>

<% if(fetchedCount > 0 && objSimilarBikes!=null) { %>
<div id="modelComparisonContent" class="bw-model-tabs-data padding-top20 padding-bottom20 font14">
    <h2 class="padding-left20 padding-right20 margin-bottom15">Popular comparisons for  <%=ModelName%> </h2>
    <div class="jcarousel-wrapper inner-content-carousel margin-bottom20">
        <div class="jcarousel">
            <ul class="model-comparison-list">
                     <% foreach(var bike in  objSimilarBikes) { %>
                        <li class="position-rel">
                            <a href="/<%= Bikewale.Utility.UrlFormatter.CreateCompareUrl(bike.MakeMasking1,bike.ModelMasking1,bike.MakeMasking2,bike.ModelMasking2,bike.VersionId1,bike.VersionId2,Bikewale.Entities.Compare.CompareSources.Desktop_Model_MostPopular_Compare_Widget) %>" title ="<%= Bikewale.Utility.UrlFormatter.CreateCompareTitle(bike.Model1,bike.Model2) %>">
                                <h3 class="text-black text-center"><%= Bikewale.Utility.UrlFormatter.CreateCompareTitle(bike.Model1,bike.Model2) %></h3>
                                <% if(SponsoredVersionId == Convert.ToUInt32(bike.VersionId2)) { %>  <span class="content-inner-block-5 text-grey position-abt font12" style="right:0;top:0">Sponsored</span>   <% } %>
                                
                                <div class="grid-6 alpha omega border-light-right padding-bottom20">
                                    <div class="imageWrapper margin-bottom10">
                                        <div class="comparison-image">
                                            <img  src="<%= Bikewale.Utility.Image.GetPathToShowImages(bike.OriginalImagePath1,bike.HostUrl1,Bikewale.Utility.ImageSize._174x98) %>" alt="<%= bike.Model1 %>" title="<%= bike.Model1 %>" />    
                                        </div>
                                    </div>
                                    <p class="text-light-grey block text-truncate">Ex-showroom, <%= bike.City1  %></p>
                                    <span class="bwsprite inr-md-lg"></span>&nbsp;<span class="font20 text-default text-bold"><%= Bikewale.Utility.Format.FormatPrice(bike.Price1.ToString())  %></span>
                                </div>
                                <div class="grid-6 padding-left30 omega">
                                    <div class="imageWrapper margin-bottom10">
                                        <div class="comparison-image">
                                           <img src="<%= Bikewale.Utility.Image.GetPathToShowImages(bike.OriginalImagePath2,bike.HostUrl2,Bikewale.Utility.ImageSize._174x98) %>" alt="<%= bike.Model2 %>" title="<%= bike.Model2 %>" />
                                        </div>
                                    </div>
                                    <p class="text-light-grey block text-truncate">Ex-showroom, <%= bike.City2  %></p>
                                    <span class="bwsprite inr-md-lg"></span>&nbsp;<span class="font20 text-default text-bold"><%= Bikewale.Utility.Format.FormatPrice(bike.Price2.ToString())  %></span>
                                     <% if (SponsoredVersionId == Convert.ToUInt32(bike.VersionId2) && !string.IsNullOrEmpty(FeaturedBikeLink))
                                        { %> <br /> <span class="text-truncate font12 block" data-href="<%= FeaturedBikeLink %>" title="View <%= bike.Model2  %> details on <%=bike.Make2 %>'s site"  id="sponsored-comparebike-link">More info at <%=bike.Make2 %> auto</span>   <% } %>
                                </div>
                                <div class="clear"></div>
                                <div class="margin-top10 text-center">
                                    <span class="btn btn-white btn-size-1">Compare now</span>
                                </div>
                            </a>
                        </li>
                  <% } %>
            </ul>
        </div>

        <span class="jcarousel-control-left"><a href="#" class="bwsprite jcarousel-control-prev inactive" rel="nofollow"></a></span>
        <span class="jcarousel-control-right"><a href="#" class="bwsprite jcarousel-control-next" rel="nofollow"></a></span>
    </div>

    <div class="margin-left20">
        <a href="/comparebikes/">View more comparisons<span class="bwsprite blue-right-arrow-icon"></span></a>
    </div>
</div>
    <% if (SponsoredVersionId > 0 && !string.IsNullOrEmpty(FeaturedBikeLink))
       { %>
    <script type="text/javascript">
        $("#sponsored-comparebike-link").click(function (e) {_sponsoredLink = $(this).attr("data-href"); window.open(_sponsoredLink, '_blank').focus();return false;})
    </script>
    <% } %>
<% }  %>
