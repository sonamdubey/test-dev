<%@ Page Language="C#" AutoEventWireup="false" Inherits="Bikewale.Mobile.Generic.BikeListing" %>
<%@ Register Src="~/m/controls/BestBikes.ascx" TagName="BestBikes" TagPrefix="BW" %>
<%@ Import Namespace="Bikewale.Entities.GenericBikes" %>
<!DOCTYPE html>
<html>
<head>
    <%
        if (pageMetas != null)
        {
            title = pageMetas.Title;
            description = pageMetas.Description;
            canonical = pageMetas.CanonicalUrl;
        }
        Ad_320x50 = false;
        Ad_Bot_320x50 = false;
    %>

    <!-- #include file="/includes/headscript_mobile_min.aspx" -->
    <link rel="stylesheet" type="text/css" href="/m/sass/generic/listing.css" />
    <script type="text/javascript">
        <!-- #include file="\includes\gacode_mobile.aspx" -->
    </script>
</head>
<body>
    <form id="form1" runat="server">
        <!-- #include file="/includes/headBW_Mobile.aspx" -->
         <% 
            switch (ctrlBestBikes.CurrentPage.Value)
            {
                case EnumBikeBodyStyles.AllBikes:
                case EnumBikeBodyStyles.Sports:
                case EnumBikeBodyStyles.Scooter:
                    bannerImagePos = "center-pos";
                    break;
                case EnumBikeBodyStyles.Mileage:
                    bannerImagePos = "center-right-pos";
                    break;
                case EnumBikeBodyStyles.Cruiser:
                    bannerImagePos = "left-center-pos";
                    break;
                default:
                    bannerImagePos = "center-pos";
                    break;
            }               
        %>
        <section>
            <div class="container generic-banner text-center <%= bannerImagePos %>" style="background-image: url(<%=bannerImageUrl %>)">
                <h1 class="font24 text-uppercase text-white margin-bottom10">Best <%= pageName %> in India</h1>
                <h2 class="font14 text-unbold text-white">Explore the list of top 10 <%= pageName.ToLower() %> in India</h2>
            </div>
        </section>
        <% if (!String.IsNullOrEmpty(pageContent)){ %>
        <section>
            <div class="container section-bottom-margin">
                <div class="grid-12">
                    <div class="banner-box-shadow content-inner-block-20 description-content font14 text-light-grey">
                         <% if (pageContent.Length > 200){ %>
                        <p class="desc-main-content"><%= pageContent.Substring(0,200) %></p><p class="desc-more-content"><%= pageContent.Substring(200) %></p><a href="javascript:void(0)" class="read-more-desc-target" rel="nofollow">... Read more</a>
                        <%} else{ %>
                        <p class="desc-main-content"><%= pageContent %></p>
                        <%} %>
                    </div>
                </div>
                <div class="clear"></div>
            </div>
        </section>
                <% } %>
        <% if (objBestBikes != null && objBestBikes.Count() > 0)
           {
               %>
        <section>
            <div class="container section-bottom-margin">
                <div class="box-shadow bg-white">
                    <div class="padding-right20 padding-left20">
                        <h2 class="padding-top10 padding-bottom10 border-solid-bottom">And the top 10 <%= pageName.ToLower() %> are...</h2>
                    </div>
                    <ul id="bike-list">
                        <%  int i = 10; string prevMonth = DateTime.Now.AddMonths(-1).ToString("MMM");
                                foreach(var bike in objBestBikes) { %>
                        <li class="list-item">
                            <div class="padding-bottom15 border-light-bottom">
                                <a href="<%= string.Format("/m/{0}-bikes/{1}/",bike.Make.MaskingName,bike.Model.MaskingName) %>" title="<%= bike.BikeName %>" class="item-image-content vertical-top">
                                    <span class="item-rank">#<%= i-- %></span>
                                    <img class="lazy" data-original="<%= Bikewale.Utility.Image.GetPathToShowImages(bike.OriginalImagePath,bike.HostUrl,Bikewale.Utility.ImageSize._110x61) %>" alt="<%= bike.BikeName %>" src="" />
                                </a>
                                <div class="bike-details-block vertical-top">
                                    <h3 class="margin-bottom5"><a href="<%= string.Format("/m/{0}-bikes/{1}/",bike.Make.MaskingName,bike.Model.MaskingName) %>" title="<%= bike.BikeName %>" class="target-link"><%= bike.BikeName %></a></h3>
                                    <ul class="key-specs-list font12 text-xx-light">
                                         <%if (bike.MinSpecs.Displacement != 0)
                                        { %>
                                        <li>
                                            <span><%= Bikewale.Utility.FormatMinSpecs.ShowAvailable(bike.MinSpecs.Displacement.ToString(),"cc") %></span>
                                        </li>
                                         <% } %>
                                            <%if (bike.MinSpecs.FuelEfficiencyOverall != 0)
                                        { %>
                                        <li>
                                            <span><%= Bikewale.Utility.FormatMinSpecs.ShowAvailable(bike.MinSpecs.FuelEfficiencyOverall.ToString(),"kmpl") %></span>
                                        </li>
                                        <% } %>
                                    <%if (bike.MinSpecs.MaxPower != 0)
                                        { %>
                                        <li>
                                            <span><%= Bikewale.Utility.FormatMinSpecs.ShowAvailable(bike.MinSpecs.MaxPower.ToString(),"bhp") %></span>
                                        </li>
                                         <% } %>
                                    </ul>
                                </div>
                                <div class="clear"></div>
                            </div>
                            <table class="item-table-content border-light-bottom" width="100%" cellspacing="0" cellpadding="0">
                                <thead>
                                    <tr class="table-head-row" style="text-align:left">
                                        <th valign="top" width="35%" align="left">Available in</th>
                                        <th valign="top" width="35%" align="left">Launched in</th>
                                        <th valign="top" width="30%" align="left">Unit sold <% if(bike.UnitsSold > 0){ %>(<%= bike.LastUpdatedModelSold.Value.ToString("MMM") %>)<%}%></th>
                                    </tr>
                                </thead>
                                <tbody>
                                    <tr>
                                        <td valign="top" class="text-bold text-grey">
                                                <%= bike.TotalModelColors > 0 ? bike.TotalModelColors : 1 %><%= (bike.TotalModelColors > 1 ? " colors" : " color") %>
                                        </td>
                                        <td valign="top" class="text-bold text-grey"><%= bike.LaunchDate.HasValue ? Bikewale.Utility.FormatDate.GetFormatDate(bike.LaunchDate.ToString(),"MMMM yyyy") : "Before 2012" %></td>
                                        <td valign="top" class="text-bold text-grey"><%= Bikewale.Utility.Format.FormatPrice(bike.UnitsSold.ToString()) %></td>
                                    </tr>
                                </tbody>
                            </table>
                            <div class="padding-top10 margin-bottom10">
                                <p class="font13 text-grey">Ex-showroom price, <%= bike.PriceInCity %></p>
                                <div class="margin-bottom10">
                                    <span class="bwmsprite inr-xsm-icon"></span>
                                    <span class="font16 text-bold"><%= Bikewale.Utility.Format.FormatPrice(bike.Price.ToString()) %></span>
                                </div>
                                <%if(bike.Price > 0) { %>
                                <button type="button" data-pagecatid="0" 
                                            data-pqsourceid="<%= (int)pqSource %>" data-makename="<%= bike.Make.MakeName %>" 
                                            data-modelname="<%= bike.Model.ModelName %>" data-modelid="<%= bike.Model.ModelId %>" 
                                    class="btn btn-white font14 btn-size-180 getquotation">Check on-road price</button>
                                 <% } %>
                            </div>
                            <p class="font14 text-light-grey margin-bottom15"><%= bike.SmallModelDescription %></p>
                            <ul class="item-more-details-list">
                                <% if(bike.NewsCount > 0) { %>
                                <li>
                                    <a href="/m<%= Bikewale.Utility.UrlFormatter.FormatNewsUrl(bike.Make.MaskingName,bike.Model.MaskingName) %>" title="<%= bike.BikeName %> News">
                                        <span class="bwmsprite news-sm"></span>
                                        <span class="icon-label">News</span>
                                    </a>
                                </li>
                                 <%} %>
                                  <% if(bike.ExpertReviewsCount > 0) { %>
                                <li>
                                    <a href="/m<%= Bikewale.Utility.UrlFormatter.FormatExpertReviewUrl(bike.Make.MaskingName,bike.Model.MaskingName) %>" title="<%= bike.BikeName %> Reviews">
                                        <span class="bwmsprite reviews-sm"></span>
                                        <span class="icon-label">Reviews</span>
                                    </a>
                                </li>
                                 <%} %>
                            <% if(bike.PhotosCount > 0) { %>
                                <li>
                                    <a href="/m<%= Bikewale.Utility.UrlFormatter.FormatPhotoPageUrl(bike.Make.MaskingName,bike.Model.MaskingName) %>" title="<%= bike.BikeName %> Images">
                                        <span class="bwmsprite photos-sm"></span>
                                        <span class="icon-label">Images</span>
                                    </a>
                                </li>
                                 <% } %>
                            <% if(bike.VideosCount > 0) { %>
                                <li>
                                    <a href="/m<%= Bikewale.Utility.UrlFormatter.FormatVideoPageUrl(bike.Make.MaskingName,bike.Model.MaskingName) %>" title="<%= bike.BikeName %> Videos">
                                        <span class="bwmsprite videos-sm"></span>
                                        <span class="icon-label">Videos</span>
                                    </a>
                                </li>
                                 <% } %>
                            <% if(bike.MinSpecs !=null) { %>
                                <li>
                                    <a href="/m<%= Bikewale.Utility.UrlFormatter.ViewAllFeatureSpecs(bike.Make.MaskingName,bike.Model.MaskingName) %>" title="<%= bike.BikeName %> Specs">
                                        <span class="bwmsprite specs-sm"></span>
                                        <span class="icon-label">Specs</span>
                                    </a>
                                </li>
                                 <% } %>
                            </ul>
                            <div class="clear"></div>

                            <% if (bike.UsedBikesCount > 0 && bike.UsedCity != null && bike.Make != null && bike.Model != null)
                                           { %>
                            <div class="border-light-bottom margin-top10 margin-bottom15"></div>
                            <a href="/m<%= Bikewale.Utility.UrlFormatter.ViewMoreUsedBikes(bike.UsedCity.CityId, bike.UsedCity.CityMaskingName, bike.Make.MaskingName, bike.Model.MaskingName) %>" title="Used <%= bike.Model.ModelName %> bikes" class="block text-light-grey margin-bottom5">
                                <span class="used-target-label inline-block">
                                    <span class="font14 text-bold"><%= bike.UsedBikesCount %> Used <%= bike.Model.ModelName %> bike<%if(bike.UsedBikesCount > 1){%>s<%}%></span><br>
                                </span>
                                <span class="bwmsprite next-grey-icon"></span>
                            </a>
                             <% } %>
                        </li>
                        <%} %>
                    </ul>
                </div>
            </div>
        </section>
          <%} %>
        <section>
            <div class="container section-bottom-margin">
                <h2 class="section-heading">Best bikes in other categories</h2>
                <div class="box-shadow bg-white padding-top10 padding-bottom10">
                    <BW:BestBikes runat="server" ID="ctrlBestBikes" />
                </div>
            </div>
        </section>

        <section>
            <div class="container margin-bottom20 font12 padding-top5 padding-right20 padding-left20">
                <span class="font14"><strong>Disclaimer</strong>: </span>The list of top 10 <%= pageName.ToLower() %> has been curated based on data collected from users of BikeWale. The best <%= pageName.ToLower() %>'s list doesn't intend to comment anything on the quality of bikes. The list is revised every month based on interest shown by users and hence the list shows only monthly trends. The data for monthly unit sold which has been used for top 10 <%= pageName.ToLower() %> has been taken from www.autopunditz.com. The unit sold is presented to help users make an informed decision.<br />Note: Data of unit sold for top models of Passion, Splendor, Glamour, Xtreme, Karizma, Activa, Gusto, Access, GIXXER, Apache, Ray and FZ series bikes has been produced same as for entire series as the data of individual model is not available.
            </div>
        </section>

        <script type="text/javascript" src="<%= staticUrl  %>/m/src/frameworks.js?<%= staticFileVersion %>"></script>

        <!-- #include file="/includes/footerBW_Mobile.aspx" -->

        <link href="<%= staticUrl  %>/m/css/bwm-common-btf.css?<%= staticFileVersion %>" rel="stylesheet" type="text/css" />
        <!-- #include file="/includes/footerscript_mobile.aspx" -->
        <!-- #include file="/includes/fontBW_Mobile.aspx" -->

        <script type="text/javascript">
            $('.read-more-desc-target').on('click', function () {
                var descWrapper = $(this).closest('.description-content');
                description.toggleContent(descWrapper);
            });
            var description = {
                toggleContent: function (descWrapper) {
                    var readMoreTarget = $(descWrapper).find('.read-more-desc-target');
                    if (!$(descWrapper).hasClass('active')) {
                        $(descWrapper).addClass('active');
                        readMoreTarget.text('Collapse');
                    }
                    else {
                        $(descWrapper).removeClass('active');
                        readMoreTarget.text('... Read more');
                    }
                }
            };
        </script>
    </form>
</body>
</html>
