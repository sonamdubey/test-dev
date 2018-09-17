<%@ Page Language="C#" AutoEventWireup="false" Inherits="Bikewale.Generic.BikeListing" %>
<%@ Register Src="~/UI/controls/BestBikes.ascx" TagName="BestBikes" TagPrefix="BW" %>
<%@ Import Namespace="System.Linq" %>
<%@ Import Namespace="Bikewale.Utility" %>
<%@ Import Namespace="Bikewale.Entities.GenericBikes" %>
<!DOCTYPE html>

<html>
<head>
    <%
        if(pageMetas!=null)
        {
            title = pageMetas.Title;
            description = pageMetas.Description;
            canonical = pageMetas.CanonicalUrl;
            alternate = pageMetas.AlternateUrl;   
        }
       
        isAd970x90Shown = false;
        isTransparentHeader = true;
        isAd300x250Shown = false;
        isAd300x250BTFShown = false;
        isAd970x90BottomShown = false;
            
    %>
    <!-- #include file="/UI/includes/headscript_desktop_min.aspx" -->
    <link rel="stylesheet" type="text/css" href="/UI/sass/generic/listing.css" />
    <script type="text/javascript">
        <!-- #include file="\UI\includes\gacode_desktop.aspx" -->
    </script>
</head>
<body>
    <form id="form1" runat="server">
        <!-- #include file="/UI/includes/headBW.aspx" -->

        <% 
            switch (ctrlBestBikes.CurrentPage.Value)
            {
                case EnumBikeBodyStyles.AllBikes:
                case EnumBikeBodyStyles.Sports:
                    bannerImagePos = "center-pos";
                    break;
                case EnumBikeBodyStyles.Scooter:
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

        <header>
            <div class="generic-banner <%= bannerImagePos %>" style="background-image: url(<%=bannerImageUrl %>)">
                <div class="container">
                    <div class="banner-box text-center">
                        <h1 class="font30 text-uppercase margin-bottom5 text-white">Best <%= pageName %> in India</h1>
                        <h2 class="font20 text-unbold text-white">Explore the list of top 10 <%= pageName.ToLower() %> in India</h2>
                    </div>
                </div>
            </div>
        </header>

        <% if (!String.IsNullOrEmpty(pageContent)){ %>
        <section>
            <div class="container section-bottom-margin">
                <div class="grid-12">
                    <div class="content-box-shadow content-inner-block-20 margin-minus50 description-content font14 text-light-grey">
                        <% if (pageContent.Length > 410){ %>
                        <p class="desc-main-content"><%= pageContent.Substring(0,410) %></p><p class="desc-more-content"><%= pageContent.Substring(410) %></p><a href="javascript:void(0)" class="read-more-desc-target" rel="nofollow">... Read more</a>
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
                <div class="grid-12">
                    <div class="content-box-shadow">
                        <div class="padding-right20 padding-left20">
                            <h2 class="font18 text-black padding-top15 padding-bottom15 border-light-bottom">And the top 10 <%= pageName.ToLower() %> are...</h2>
                        </div>

                        <ul id="bike-list" class="font14">
                            <%  int i = 10; string prevMonth = DateTime.Now.AddMonths(-1).ToString("MMMM");
                                foreach(var bike in objBestBikes) { %>

                            <li class="list-item">
                                <div class="item-details-content">
                                    <div class="grid-3 padding-left20">
                                        <a href="<%= string.Format("/{0}-bikes/{1}/",bike.Make.MaskingName,bike.Model.MaskingName) %>" title="<%= bike.BikeName %>" class="item-image-content">
                                            <span class="item-rank">#<%= i-- %></span>
                                            <img class="lazy" data-original="<%= Bikewale.Utility.Image.GetPathToShowImages(bike.OriginalImagePath,bike.HostUrl,Bikewale.Utility.ImageSize._227x128) %>" alt="<%= bike.BikeName %>" src="" />
                                        </a>
                                    </div>
                                    <div class="grid-6 bike-details-block border-grey-right padding-right20">
                                        <h3><a href="<%= string.Format("/{0}-bikes/{1}/",bike.Make.MaskingName,bike.Model.MaskingName) %>" title="<%= bike.BikeName %>" class="bikeTitle"><%= bike.BikeName %></a></h3>
                                        <ul class="key-specs-list text-light-grey padding-bottom15 margin-bottom15 border-solid-bottom">
                                             <%if (bike.MinSpecs.Displacement != 0)
                                        { %>
                                            <li>
                                                <span class="bwsprite capacity-sm"></span>
                                                 <span><%= Bikewale.Utility.FormatMinSpecs.ShowAvailable(bike.MinSpecs.Displacement.ToString(),"cc") %></span>
                                            </li>
                                             <% } %>
                                            <%if (bike.MinSpecs.FuelEfficiencyOverall != 0)
                                        { %>
                                            <li>
                                                <span class="bwsprite mileage-sm"></span>
                                                   <span><%= Bikewale.Utility.FormatMinSpecs.ShowAvailable(bike.MinSpecs.FuelEfficiencyOverall.ToString(),"kmpl") %></span>
                                            </li>
                                            <% } %>
                                    <%if (bike.MinSpecs.MaxPower != 0)
                                        { %>
                                            <li>
                                                <span class="bwsprite power-sm"></span>
                                                   <span><%= Bikewale.Utility.FormatMinSpecs.ShowAvailable(bike.MinSpecs.MaxPower.ToString(),"bhp") %></span>
                                            </li>                                            
                                    <% } %>
                                        </ul>
                                        <table class="item-table-content" width="100%" cellspacing="0" cellpadding="0">
                                            <thead>
                                                <tr class="table-head-row">
                                                    <th valign="top" width="35%">Available in</th>
                                                    <th valign="top" width="25%">Launched in</th>
                                                    <th valign="top" width="30%">Unit sold <% if(bike.UnitsSold > 0){ %>(<%= bike.LastUpdatedModelSold.Value.ToString("MMMM") %>)<%}%></th>
                                                </tr>
                                            </thead>
                                            <tbody>
                                                <tr>
                                                    <td valign="top" class="text-bold text-grey">
                                                       <% if((bike.TotalModelColors + bike.TotalVersions) > 0){ %> <%= bike.TotalVersions %><%= (bike.TotalVersions > 1 ? " variants" : " variant") %><% if (bike.TotalModelColors > 0){ %>, <%= bike.TotalModelColors %><%= (bike.TotalModelColors > 1 ? " colors" : " color")  %> <% }
                                                        } %>
                                                    </td>
                                                    <td valign="top" class="text-bold text-grey"><%= bike.LaunchDate.HasValue ? Bikewale.Utility.FormatDate.GetFormatDate(bike.LaunchDate.ToString(),"MMMM yyyy") : "Before 2012" %></td>
                                                    <td valign="top" class="text-bold text-grey"><%= bike.UnitsSold > 0 ? Bikewale.Utility.Format.FormatNumeric(bike.UnitsSold.ToString()) : "Not Available" %></td>
                                                </tr>
                                            </tbody>
                                        </table>
                                    </div>
                                    <div class="grid-3 padding-left20 padding-right20">
                                        <p class="font14 text-grey margin-bottom5">Ex-showroom price, <%= bike.PriceInCity %></p>
                                        <div class="margin-bottom10">
                                            <span class="bwsprite inr-lg"></span> <span class="font18 text-bold"><%= Bikewale.Utility.Format.FormatPrice(bike.Price.ToString()) %></span>
                                        </div>
                                        <%if(bike.Price > 0) { %>
                                        <a 
                                            href="javascript:void(0)" data-pagecatid="0" 
                                            data-pqsourceid="<%= (int)pqSource %>" data-makename="<%= bike.Make.MakeName %>" 
                                            data-modelname="<%= bike.Model.ModelName %>" data-modelid="<%= bike.Model.ModelId %>" 
                                            class="btn btn-grey btn-sm font14 getquotation">Check on-road price</a>
                                        <% } %>
                                    </div>
                                    <div class="clear"></div>

                                    <div class="margin-top15 padding-right20 description-content padding-left20">
                                        <p class="font14 text-light-grey margin-bottom15"><%= bike.SmallModelDescription %></p>
                                        <div class="leftfloat">
                                            <span class="text-light-grey inline-block">More about <%= bike.Model.ModelName %>:</span>
                                            <ul class="item-more-details-list inline-block">
                                                <% if(bike.NewsCount > 0) { %>
                                                <li>
                                                    <a href="<%= UrlFormatter.FormatNewsUrl(bike.Make.MaskingName,bike.Model.MaskingName) %>" title="<%= bike.BikeName %> News">
                                                        <span class="bwsprite news-sm"></span>
                                                        <span class="icon-label">News</span>
                                                    </a>
                                                </li>
                                                <%} %>
                                                 <% if(bike.ExpertReviewsCount > 0) { %>
                                                <li>
                                                    <a href="<%= UrlFormatter.FormatExpertReviewUrl(bike.Make.MaskingName,bike.Model.MaskingName) %>" title="<%= bike.BikeName %> Reviews">
                                                        <span class="bwsprite reviews-sm"></span>
                                                        <span class="icon-label">Reviews</span>
                                                    </a>
                                                </li>
                                                <%} %>
                                                <% if(bike.PhotosCount > 0) { %>
                                                <li>
                                                    <a href="<%= UrlFormatter.FormatPhotoPageUrl(bike.Make.MaskingName,bike.Model.MaskingName) %>" title="<%= bike.BikeName %> Images">
                                                        <span class="bwsprite photos-sm"></span>
                                                        <span class="icon-label">Images</span>
                                                    </a>
                                                </li>
                                                <% } %>
                                                  <% if(bike.VideosCount > 0) { %>
                                                <li>
                                                    <a href="<%= UrlFormatter.FormatVideoPageUrl(bike.Make.MaskingName,bike.Model.MaskingName) %>" title="<%= bike.BikeName %> Videos">
                                                        <span class="bwsprite videos-sm"></span>
                                                        <span class="icon-label">Videos</span>
                                                    </a>
                                                </li>
                                               <% } %>
                                                  <% if(bike.MinSpecs !=null) { %>
                                                <li>
                                                    <a href="<%= UrlFormatter.ViewAllFeatureSpecs(bike.Make.MaskingName,bike.Model.MaskingName) %>" title="<%= bike.BikeName %> Specs">
                                                        <span class="bwsprite specs-sm"></span>
                                                        <span class="icon-label">Specs</span>
                                                    </a>
                                                </li>
                                                <% } %>
                                            </ul>
                                        </div>

                                        <% if (bike.UsedBikesCount > 0 && bike.UsedCity != null && bike.Make != null && bike.Model != null)
                                           { %>
                                        <div class="used-bike-target-link leftfloat">
                                            <span class="text-light-grey">Check out&nbsp;:</span>&nbsp;
                                            <a href="<%= UrlFormatter.ViewMoreUsedBikes(bike.UsedCity.CityId, bike.UsedCity.CityMaskingName, bike.Make.MaskingName, bike.Model.MaskingName) %>" title="Used <%= bike.Model.ModelName %> bikes"><%= bike.UsedBikesCount %> Used <%= bike.Model.ModelName %> bike<%if(bike.UsedBikesCount > 1){%>s<%}%></a>
                                        </div>                                       
                                        <% } %>
                                         <div class="clear"></div>
                                    </div>
                                </div>
                            
                            </li>
                            <%} %>
                        </ul>
                    </div>
                </div>
                <div class="clear"></div>
            </div>
        </section>
        <%} %>

        <section>
            <div class="container section-bottom-margin">
                <h2 class="section-heading">Best bikes in other categories</h2>
                <div class="grid-12">
                    <div class="content-box-shadow padding-top20 padding-bottom20">
                        <BW:BestBikes runat="server" ID="ctrlBestBikes" />
                    </div>
                </div>
                <div class="clear"></div>
            </div>
        </section>

        <section>
            <div class="container section-bottom-margin">
                <div class="grid-12 font12">
                    <p>
                        <span class="font14"><strong>Disclaimer</strong>: </span>The list of top 10 <%= pageName.ToLower() %> has been curated based on data collected from users of BikeWale. The best <%= pageName.ToLower() %>'s list doesn't intend to comment anything on the quality of bikes. The list is revised every month based on interest shown by users and hence the list shows only monthly trends. The data for monthly unit sold which has been used for top 10 <%= pageName.ToLower() %> has been taken from www.autopunditz.com. The unit sold is presented to help users make an informed decision.<br />Note: Data of unit sold for top models of Passion, Splendor, Glamour, Xtreme, Karizma, Activa, Gusto, Access, GIXXER, Apache, Ray and FZ series bikes has been produced same as for entire series as the data of individual model is not available.
                    </p>
                </div>
                <div class="clear"></div>
            </div>
        </section>

        <script type="text/javascript" src="<%= staticUrl  %>/UI/src/frameworks.js?<%=staticFileVersion %>"></script>

        <!-- #include file="/UI/includes/footerBW.aspx" -->
        
        <link href="<%= staticUrl  %>/UI/css/bw-common-btf.css?<%=staticFileVersion %>" rel="stylesheet" type="text/css" />
        <!-- #include file="/UI/includes/footerscript.aspx" -->
        <!-- #include file="/UI/includes/fontBW.aspx" -->

        <script type="text/javascript">

            $(document).ready(function () {
                if ($(window).scrollTop() > 40) {
                    $('#header').removeClass("header-landing").addClass("header-fixed");
                }
            });

            $(window).on("scroll", function () {
                if ($(window).scrollTop() > 40)
                    $('#header').removeClass("header-landing").addClass("header-fixed");
                else
                    $('#header').removeClass("header-fixed").addClass("header-landing");
            });

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
                        readMoreTarget.text('...Read more');
                    }
                }
            };
        </script>

    </form>
</body>
</html>
