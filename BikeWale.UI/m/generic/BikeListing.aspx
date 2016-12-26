﻿<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="BikeListing.aspx.cs" Inherits="Bikewale.Mobile.Generic.BikeListing" %>
<%@ Register Src="~/m/controls/BestBikes.ascx" TagName="BestBikes" TagPrefix="BW" %>

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
    <link rel="stylesheet" type="text/css" href="/m/css/generic/listing.css">
    <script type="text/javascript">
        <!-- #include file="\includes\gacode_mobile.aspx" -->
    </script>
</head>
<body>
    <form id="form1" runat="server">
        <!-- #include file="/includes/headBW_Mobile.aspx" -->
        <section>
            <div class="container generic-banner text-center" style="background: #988f7f url(<%=bannerImageUrl %>) no-repeat center; background-size: cover">
                <h1 class="font24 text-uppercase text-white margin-bottom10">Best <%= pageName %> in India</h1>
                <h2 class="font14 text-unbold text-white">Explore the list of top 10 <%= pageName.ToLower() %> in India</h2>
            </div>
        </section>
        <% if (!String.IsNullOrEmpty(pageContent)){ %>
        <section>
            <div class="container section-bottom-margin">
                <div class="grid-12">
                    <div class="banner-box-shadow content-inner-block-20 description-content font14 text-light-grey">
                         <% if (pageContent.Length > 410){ %>
                        <p class="desc-main-content"><%= pageContent.Substring(0,200) %></p>
                        <p class="desc-more-content"><%= pageContent.Substring(200) %></p>
                        <a href="javascript:void(0)" class="read-more-desc-target" rel="nofollow">... Read more</a>
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
                        <%  int i = 1;
                                foreach(var bike in objBestBikes) { %>
                        <li class="list-item">
                            <div class="padding-bottom15 border-light-bottom">
                                <a href="<%= string.Format("/m/{0}-bikes/{1}/",bike.Make.MaskingName,bike.Model.MaskingName) %>" title="<%= bike.BikeName %>" class="item-image-content vertical-top">
                                    <span class="item-rank">#<%= i++ %></span>
                                    <img class="lazy" data-original="<%= Bikewale.Utility.Image.GetPathToShowImages(bike.OriginalImagePath,bike.HostUrl,Bikewale.Utility.ImageSize._110x61) %>" alt="<%= bike.BikeName %>" src="" />
                                </a>
                                <div class="bike-details-block vertical-top">
                                    <h3 class="margin-bottom5"><a href="" class="target-link"><%= bike.BikeName %></a></h3>
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
                                    <tr class="table-head-row">
                                        <th valign="top" width="35%">Available in</th>
                                        <th valign="top" width="35%">Launched in</th>
                                        <th valign="top" width="30%">Unit sold (<%= Bikewale.Utility.FormatDate.CurrentMonth() %>)</th>
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
                            <div class="padding-top10 margin-bottom10">
                                <p class="font13 text-grey">Ex-showroom price, <%= Bikewale.Utility.BWConfiguration.Instance.DefaultName %></p>
                                <div class="margin-bottom10">
                                    <span class="bwmsprite inr-xsm-icon"></span>
                                    <span class="font16 text-bold"><%= Bikewale.Utility.Format.FormatPrice(bike.Price.ToString()) %></span>
                                </div>
                                <%if(bike.Price > 0) { %>
                                <button type="button" data-pagecatid="0" 
                                            data-pqsourceid="<%= (int)pqSource %>" data-makename="<%= bike.Make.MakeName %>" 
                                            data-modelname="<%= bike.Model.ModelName %>" data-modelid="<%= bike.Model.ModelId %>" 
                                    class="btn btn-white font14 btn-size-180">Check on-road price</button>
                                 <% } %>
                            </div>
                            <p class="font14 text-light-grey margin-bottom15"><%= bike.SmallModelDescription %></p>
                            <ul class="item-more-details-list">
                                  <% if(bike.ExpertReviewsCount > 0) { %>
                                <li>
                                    <a href="<%= Bikewale.Utility.UrlFormatter.FormatExpertReviewUrl(bike.Make.MaskingName,bike.Model.MaskingName) %>" title="<%= bike.BikeName %> Reviews">
                                        <span class="generic-sprite reviews-sm"></span>
                                        <span class="icon-label">Reviews</span>
                                    </a>
                                </li>
                                 <%} %>
                            <% if(bike.PhotosCount > 0) { %>
                                <li>
                                    <a href="<%= Bikewale.Utility.UrlFormatter.FormatPhotoPageUrl(bike.Make.MaskingName,bike.Model.MaskingName) %>" title="<%= bike.BikeName %> Photos">
                                        <span class="generic-sprite news-sm"></span>
                                        <span class="icon-label">Photos</span>
                                    </a>
                                </li>
                                 <% } %>
                            <% if(bike.VideosCount > 0) { %>
                                <li>
                                    <a href="<%= Bikewale.Utility.UrlFormatter.FormatVideoPageUrl(bike.Make.MaskingName,bike.Model.MaskingName) %>" title="<%= bike.BikeName %> Videos">
                                        <span class="generic-sprite videos-sm"></span>
                                        <span class="icon-label">Videos</span>
                                    </a>
                                </li>
                                 <% } %>
                            <% if(bike.MinSpecs !=null) { %>
                                <li>
                                    <a href="<%= Bikewale.Utility.UrlFormatter.ViewAllFeatureSpecs(bike.Make.MaskingName,bike.Model.MaskingName) %>" title="<%= bike.BikeName %> Specs">
                                        <span class="generic-sprite specs-sm"></span>
                                        <span class="icon-label">Specs</span>
                                    </a>
                                </li>
                                 <% } %>
                            </ul>
                            <div class="clear"></div>
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
                <span class="font14"><strong>Disclaimer</strong>: </span>The list of top 10 <%= pageName.ToLower() %> has been curated based on data collected from users of BikeWale. The best <%= pageName.ToLower() %>'s list doesn't intend to comment anything on the quality of bikes in absolute terms. We don't comment anything about bikes or scooters which are not included in this list. The list is revised every month based on interest shown by users. The data for monthly unit sold which has been used for top 10 <%= pageName.ToLower() %> has been taken from www.autopunditz.com. The unit sold is presented to help users make an informed decision.
            </div>
        </section>

        <script type="text/javascript" src="<%= staticUrl != "" ? "https://st1.aeplcdn.com" + staticUrl : "" %>/m/src/frameworks.js?<%= staticFileVersion %>"></script>

        <!-- #include file="/includes/footerBW_Mobile.aspx" -->

        <link href="<%= staticUrl != "" ? "https://st2.aeplcdn.com" + staticUrl : "" %>/m/css/bwm-common-btf.css?<%= staticFileVersion %>" rel="stylesheet" type="text/css" />
        <script type="text/javascript" src="<%= staticUrl != "" ? "https://st2.aeplcdn.com" + staticUrl : "" %>/m/src/common.min.js?<%= staticFileVersion %>"></script>

        <link href='https://fonts.googleapis.com/css?family=Open+Sans:400,700' rel='stylesheet' type='text/css' />

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
