﻿<%@ Page Language="C#" AutoEventWireup="false" Inherits="Bikewale.Generic.BikeListing" %>
<%@ Register Src="~/controls/BestBikes.ascx" TagName="BestBikes" TagPrefix="BW" %>
<%@ Import Namespace="System.Linq" %>
<%@ Import Namespace="Bikewale.Utility" %>
<!DOCTYPE html>

<html>
<head>
    <%
        title = (pageMetas!=null) ? pageMetas.Title : string.Empty;
        description = (pageMetas!=null) ? pageMetas.Description : string.Empty;
        isAd970x90Shown = false;
        isTransparentHeader = true;
        isAd300x250Shown = false;
        isAd300x250BTFShown = false;
        isAd970x90BottomShown = false;
        canonical = pageMetas.CanonicalUrl;        
    %>
    <!-- #include file="/includes/headscript_desktop_min.aspx" -->
    <link rel="stylesheet" type="text/css" href="/css/generic/listing.css" />
    <script type="text/javascript">
        <!-- #include file="\includes\gacode_desktop.aspx" -->
    </script>
</head>
<body>
    <form id="form1" runat="server">
        <!-- #include file="/includes/headBW.aspx" -->

        <header>
            <div class="generic-banner" style="background: #988f7f url(<%=bannerImageUrl %>) no-repeat center right">
                <div class="container">
                    <div class="banner-box text-center">
                        <h1 class="font30 text-uppercase margin-bottom5 text-white">Best <%= pageName %> in India</h1>
                        <h2 class="font20 text-unbold text-white">Explore the list of top 10 <%= pageMaskingName %> in India</h2>
                    </div>
                </div>
            </div>
        </header>

        <section>
            <div class="container section-bottom-margin">
                <div class="grid-12">
                    <div class="content-box-shadow content-inner-block-20 margin-minus50 description-content font14 text-light-grey">
                        <p class="desc-main-content"><%= pageContent %></p>
                    </div>
                </div>
                <div class="clear"></div>
            </div>
        </section>

        <% if (objBestBikes != null && objBestBikes.Count() > 0)
           {
               %>
        <section>
            <div class="container section-bottom-margin">
                <div class="grid-12">
                    <div class="content-box-shadow">
                        <div class="padding-right20 padding-left20">
                            <h2 class="font18 text-black padding-top15 padding-bottom15 border-light-bottom">And the top 10 <%= pageMaskingName %> are...</h2>
                        </div>

                        <ul id="bike-list" class="font14">
                            <%  int i = 1;
                                foreach(var bike in objBestBikes) { %>

                            <li class="list-item">
                                <div class="item-details-content">
                                    <div class="grid-3 padding-left20">
                                        <a href="" title="<% %>" class="item-image-content">
                                            <span class="item-rank">#<%= i++ %></span>
                                            <img class="lazy" data-original="<%= Bikewale.Utility.Image.GetPathToShowImages(bike.OriginalImagePath,bike.HostUrl,Bikewale.Utility.ImageSize._227x128) %>" alt="<%= bike.BikeName %>" src="" />
                                        </a>
                                    </div>
                                    <div class="grid-6 bike-details-block border-grey-right padding-right20">
                                        <h3><a href="<%= string.Format("/{0}-bikes/{1}/",bike.Make.MaskingName,bike.Model.MaskingName) %>" class="bikeTitle"><%= bike.BikeName %></a></h3>
                                        <ul class="key-specs-list text-light-grey margin-bottom15">
                                             <%if (bike.MinSpecs.Displacement != 0)
                                        { %>
                                            <li>
                                                <span class="generic-sprite capacity-sm"></span>
                                                 <span><%= Bikewale.Utility.FormatMinSpecs.ShowAvailable(bike.MinSpecs.Displacement.ToString(),"cc") %></span>
                                            </li>
                                             <% } %>
                                            <%if (bike.MinSpecs.FuelEfficiencyOverall != 0)
                                        { %>
                                            <li>
                                                <span class="generic-sprite mileage-sm"></span>
                                                   <span><%= Bikewale.Utility.FormatMinSpecs.ShowAvailable(bike.MinSpecs.FuelEfficiencyOverall.ToString(),"kmpl") %></span>
                                            </li>
                                            <% } %>
                                    <%if (bike.MinSpecs.MaxPower != 0)
                                        { %>
                                            <li>
                                                <span class="generic-sprite power-sm"></span>
                                                   <span><%= Bikewale.Utility.FormatMinSpecs.ShowAvailable(bike.MinSpecs.MaxPower.ToString(),"bhp") %></span>
                                            </li>                                            
                                    <% } %>
                                        </ul>
                                        <table class="item-table-content" width="100%" cellspacing="0" cellpadding="0">
                                            <thead>
                                                <tr class="table-head-row">
                                                    <th valign="top" width="35%">Available in</th>
                                                    <th valign="top" width="25%">Launched in</th>
                                                    <th valign="top" width="30%">Unit sold</th>
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
                                        <p class="font14 text-grey margin-bottom5">Ex-showroom price, <%= Bikewale.Utility.BWConfiguration.Instance.DefaultName %></p>
                                        <div class="margin-bottom10">
                                            <span class="bwsprite inr-lg"></span> <span class="font18 text-bold"><%= Bikewale.Utility.Format.FormatPrice(bike.Price.ToString()) %></span>
                                        </div>
                                        <%if(bike.Price > 0) { %>
                                        <a 
                                            href="javascript:void(0)" data-pagecatid="<%= 0 %>" 
                                            data-pqsourceid="<%= (int)pqSource %>" data-makename="<%= bike.Make.MakeName %>" 
                                            data-modelname="<%= bike.Model.ModelName %>" data-modelid="<%= bike.Model.ModelId %>" 
                                            class="btn btn-grey btn-sm margin-top15 font14 getquotation">Check on-road price</a>
                                        <% } %>
                                    </div>
                                    <div class="clear"></div>

                                    <div class="margin-top15 padding-right20 padding-left20">
                                        <p class="text-light-grey margin-bottom15"><%= bike.SmallModelDescription %></p>
                                        <div>
                                            <span class="text-light-grey inline-block">More info about <%= bike.Model.ModelName %>:</span>
                                            <ul class="item-more-details-list inline-block">
                                                 <% if(bike.ExpertReviewsCount > 0) { %>
                                                <li>
                                                    <a href="<%= UrlFormatter.FormatExpertReviewUrl(bike.Make.MaskingName,bike.Model.MaskingName) %>" title="<%= bike.BikeName %> Reviews">
                                                        <span class="generic-sprite reviews-sm"></span>
                                                        <span class="icon-label">Reviews</span>
                                                    </a>
                                                </li>
                                                <%} %>
                                                <% if(bike.PhotosCount > 0) { %>
                                                <li>
                                                    <a href="<%= UrlFormatter.FormatPhotoPageUrl(bike.Make.MaskingName,bike.Model.MaskingName) %>" title="<%= bike.BikeName %> Photos">
                                                        <span class="generic-sprite news-sm"></span>
                                                        <span class="icon-label">Photos</span>
                                                    </a>
                                                </li>
                                                <% } %>
                                                  <% if(bike.VideosCount > 0) { %>
                                                <li>
                                                    <a href="<%= UrlFormatter.FormatVideoPageUrl(bike.Make.MaskingName,bike.Model.MaskingName) %>" title="<%= bike.BikeName %> Videos">
                                                        <span class="generic-sprite videos-sm"></span>
                                                        <span class="icon-label">Videos</span>
                                                    </a>
                                                </li>
                                               <% } %>
                                                  <% if(bike.MinSpecs !=null) { %>
                                                <li>
                                                    <a href="<%= UrlFormatter.ViewAllFeatureSpecs(bike.Make.MaskingName,bike.Model.MaskingName) %>" title="<%= bike.BikeName %> Specs">
                                                        <span class="generic-sprite specs-sm"></span>
                                                        <span class="icon-label">Specs</span>
                                                    </a>
                                                </li>
                                                <% } %>
                                            </ul>
                                        </div>
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
                        <span class="font14"><strong>Disclaimer</strong>: </span>The list of top 10 bikes has been curated based on data collected from users of BikeWale. The best bike's list doesn't intend to comment anything on the quality of bikes in absolute terms. We don't comment anything about bikes or scooters which are not included in this list. The list is revised every month based on interest shown by users. The data for monthly unit sold which has been used for top 10 bikes has been taken from www.autopunditz.com. The unit sold is presented to help users make an informed decision.
                    </p>
                </div>
                <div class="clear"></div>
            </div>
        </section>

        <script type="text/javascript" src="<%= staticUrl != "" ? "https://st1.aeplcdn.com" + staticUrl : "" %>/src/frameworks.js?<%=staticFileVersion %>"></script>

        <!-- #include file="/includes/footerBW.aspx" -->
        
        <link href="<%= staticUrl != "" ? "https://st2.aeplcdn.com" + staticUrl : "" %>/css/bw-common-btf.css?<%=staticFileVersion %>" rel="stylesheet" type="text/css" />
        <script type="text/javascript" src="<%= staticUrl != string.Empty ? "https://st2.aeplcdn.com" + staticUrl : string.Empty %>/src/common.min.js?<%= staticFileVersion %>"></script>
        
        <link href='https://fonts.googleapis.com/css?family=Open+Sans:400,700' rel='stylesheet' type='text/css' />

        <!--[if lt IE 9]>
            <script src="/src/html5.js"></script>
        <![endif]-->

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
                        readMoreTarget.text('... Read more');
                    }
                }
            };
        </script>

    </form>
</body>
</html>
