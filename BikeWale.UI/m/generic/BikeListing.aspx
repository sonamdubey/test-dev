<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="BikeListing.aspx.cs" Inherits="Bikewale.Mobile.Generic.BikeListing" %>
<%@ Register Src="~/m/controls/BestBikes.ascx" TagName="BestBikes" TagPrefix="BW" %>

<!DOCTYPE html>
<html>
<head>
    <title>Generic listing page</title>

    <%
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

        <%--
            background images:
            1. top bikes
                style="background: #96a4b3 url(https://imgd1.aeplcdn.com/0x0/bw/static/landing-banners/m/top-bikes-banner.jpg) no-repeat center;  background-size: cover"
            2. scooters
                style="background: #988f7f url(https://imgd1.aeplcdn.com/0x0/bw/static/landing-banners/m/scooter-style-banner.jpg) no-repeat center; background-size: cover"
            3. mileage
                style="background: #988f7f url(https://imgd1.aeplcdn.com/0x0/bw/static/landing-banners/m/mileage-bikes-banner.jpg) no-repeat center right; background-size: cover"
            4. sports bikes
                style="background: #d6ceb9 url(https://imgd1.aeplcdn.com/0x0/bw/static/landing-banners/m/sports-style-banner.jpg) no-repeat center; background-size: cover"
            5. cruiser bikes
                style="background: #988f7f url(https://imgd1.aeplcdn.com/0x0/bw/static/landing-banners/m/cruiser-style-banner.jpg) no-repeat left center; background-size: cover"
        --%>

        <section>
            <div class="container generic-banner text-center" style="background: #988f7f url(https://imgd1.aeplcdn.com/0x0/bw/static/landing-banners/m/scooter-style-banner.jpg) no-repeat center; background-size: cover">
                <h1 class="font24 text-uppercase text-white margin-bottom10">Best Scooters in India</h1>
                <h2 class="font14 text-unbold text-white">Explore the list of top 10 scooters in India</h2>
            </div>
        </section>

        <section>
            <div class="container section-bottom-margin">
                <div class="grid-12">
                    <div class="banner-box-shadow content-inner-block-20 description-content font14 text-light-grey">
                        <p class="desc-main-content">Whether you live in a metro or a small town, you will find lots of scooters around! Scooters in India have gained immense popularity in the last decade. With more than 10 brands and over 50 models, it gets really</p><p class="desc-more-content"> difficult to pick the best scooter. We have more than 50 lakh people researching scooters on BikeWale every month, so this list of best scooters in India is made out of our users’ choice and truly reflects the popularity of scooters. We bring you information about ex-showroom price, colors, variants, monthly units sold, popularity and launch date of best scooters to help you pick the best one. Have a look at the list of best scooters in India to find the most suitable scooter for you. </p><a href="javascript:void(0)" class="read-more-desc-target" rel="nofollow">... Read more</a>
                    </div>
                </div>
                <div class="clear"></div>
            </div>
        </section>

        <section>
            <div class="container section-bottom-margin">
                <div class="box-shadow bg-white">
                    <div class="padding-right20 padding-left20">
                        <h2 class="padding-top10 padding-bottom10 border-solid-bottom">And the top 10 scooters are...</h2>
                    </div>
                    <ul id="bike-list">
                        <li class="list-item">
                            <div class="padding-bottom15 border-light-bottom">
                                <a href="" title="Honda Activa 3G" class="item-image-content vertical-top">
                                    <span class="item-rank">#1</span>
                                    <img class="lazy" data-original="https://imgd1.aeplcdn.com/110x61/bw/models/vespa-fly125.jpg" alt="Honda Activa 3G" src="" />
                                </a>
                                <div class="bike-details-block vertical-top">
                                    <h3 class="margin-bottom5"><a href="" class="target-link">Honda Activa 3G</a></h3>
                                    <ul class="key-specs-list font12 text-xx-light">
                                        <li>
                                            <span>209.85 cc</span>
                                        </li>
                                        <li>
                                            <span>209.85 kmpl</span>
                                        </li>
                                        <li>
                                            <span>99 bhp</span>
                                        </li>
                                    </ul>
                                </div>
                                <div class="clear"></div>
                            </div>
                            <table class="item-table-content border-light-bottom" width="100%" cellspacing="0" cellpadding="0">
                                <thead>
                                    <tr class="table-head-row">
                                        <th valign="top" width="35%">Available in</th>
                                        <th valign="top" width="35%">Launched in</th>
                                        <th valign="top" width="30%">Unit sold (May)</th>
                                    </tr>
                                </thead>
                                <tbody>
                                    <tr>
                                        <td valign="top" class="text-bold text-grey">2 colors</td>
                                        <td valign="top" class="text-bold text-grey">May 2016</td>
                                        <td valign="top" class="text-bold text-grey">32,293</td>
                                    </tr>
                                </tbody>
                            </table>
                            <div class="padding-top10 margin-bottom10">
                                <p class="font13 text-grey">Ex-showroom price, Mumbai</p>
                                <div class="margin-bottom10">
                                    <span class="bwmsprite inr-xsm-icon"></span>
                                    <span class="font16 text-bold">60,873</span>
                                </div>
                                <button type="button" class="btn btn-white font14 btn-size-180">Check on-road price</button>
                            </div>
                            <p class="font14 text-light-grey margin-bottom15">The Jupiter is an 110cc scooter from TVS, positioned above the Wego. The country’s second best-selling scooter after the Activa, the Jupiter has been primarily targeted towards men, women also seem to buy it alot.</p>
                            <ul class="item-more-details-list">
                                <li>
                                    <a href="" title="Honda Activa 3G Reviews">
                                        <span class="generic-sprite reviews-sm"></span>
                                        <span class="icon-label">Reviews</span>
                                    </a>
                                </li>
                                <li>
                                    <a href="" title="Honda Activa 3G News">
                                        <span class="generic-sprite news-sm"></span>
                                        <span class="icon-label">News</span>
                                    </a>
                                </li>
                                <li>
                                    <a href="" title="Honda Activa 3G Videos">
                                        <span class="generic-sprite videos-sm"></span>
                                        <span class="icon-label">Videos</span>
                                    </a>
                                </li>
                                <li>
                                    <a href="" title="Honda Activa 3G Specs">
                                        <span class="generic-sprite specs-sm"></span>
                                        <span class="icon-label">Specs</span>
                                    </a>
                                </li>
                            </ul>
                            <div class="clear"></div>
                        </li>
                        <li class="list-item">
                            <div class="padding-bottom15 border-light-bottom">
                                <a href="" title="Honda Activa 3G" class="item-image-content vertical-top">
                                    <span class="item-rank">#2</span>
                                    <img class="lazy" data-original="https://imgd1.aeplcdn.com/110x61/bw/models/vespa-fly125.jpg" alt="Honda Activa 3G" src="" />
                                </a>
                                <div class="bike-details-block vertical-top">
                                    <h3 class="margin-bottom5"><a href="" class="target-link">Honda Activa 3G</a></h3>
                                    <ul class="key-specs-list font12 text-xx-light">
                                        <li>
                                            <span>209.85 cc</span>
                                        </li>
                                        <li>
                                            <span>209.85 kmpl</span>
                                        </li>
                                        <li>
                                            <span>99 bhp</span>
                                        </li>
                                    </ul>
                                </div>
                                <div class="clear"></div>
                            </div>
                            <table class="item-table-content border-light-bottom" width="100%" cellspacing="0" cellpadding="0">
                                <thead>
                                    <tr class="table-head-row">
                                        <th valign="top" width="35%">Available in</th>
                                        <th valign="top" width="35%">Launched in</th>
                                        <th valign="top" width="30%">Unit sold (May)</th>
                                    </tr>
                                </thead>
                                <tbody>
                                    <tr>
                                        <td valign="top" class="text-bold text-grey">2 colors</td>
                                        <td valign="top" class="text-bold text-grey">May 2016</td>
                                        <td valign="top" class="text-bold text-grey">32,293</td>
                                    </tr>
                                </tbody>
                            </table>
                            <div class="padding-top10 margin-bottom10">
                                <p class="font13 text-grey">Ex-showroom price, Mumbai</p>
                                <div class="margin-bottom10">
                                    <span class="bwmsprite inr-xsm-icon"></span>
                                    <span class="font16 text-bold">60,873</span>
                                </div>
                                <button type="button" class="btn btn-white font14 btn-size-180">Check on-road price</button>
                            </div>
                            <p class="font14 text-light-grey margin-bottom15">The Jupiter is an 110cc scooter from TVS, positioned above the Wego. The country’s second best-selling scooter after the Activa, the Jupiter has been primarily targeted towards men, women also seem to buy it alot.</p>
                            <ul class="item-more-details-list">
                                <li>
                                    <a href="" title="Honda Activa 3G Reviews">
                                        <span class="generic-sprite reviews-sm"></span>
                                        <span class="icon-label">Reviews</span>
                                    </a>
                                </li>
                                <li>
                                    <a href="" title="Honda Activa 3G News">
                                        <span class="generic-sprite news-sm"></span>
                                        <span class="icon-label">News</span>
                                    </a>
                                </li>
                                <li>
                                    <a href="" title="Honda Activa 3G Videos">
                                        <span class="generic-sprite videos-sm"></span>
                                        <span class="icon-label">Videos</span>
                                    </a>
                                </li>
                                <li>
                                    <a href="" title="Honda Activa 3G Specs">
                                        <span class="generic-sprite specs-sm"></span>
                                        <span class="icon-label">Specs</span>
                                    </a>
                                </li>
                            </ul>
                            <div class="clear"></div>
                        </li>
                        <li class="list-item">
                            <div class="padding-bottom15 border-light-bottom">
                                <a href="" title="Honda Activa 3G" class="item-image-content vertical-top">
                                    <span class="item-rank">#3</span>
                                    <img class="lazy" data-original="https://imgd1.aeplcdn.com/110x61/bw/models/vespa-fly125.jpg" alt="Honda Activa 3G" src="" />
                                </a>
                                <div class="bike-details-block vertical-top">
                                    <h3 class="margin-bottom5"><a href="" class="target-link">Honda Activa 3G</a></h3>
                                    <ul class="key-specs-list font12 text-xx-light">
                                        <li>
                                            <span>209.85 cc</span>
                                        </li>
                                        <li>
                                            <span>209.85 kmpl</span>
                                        </li>
                                        <li>
                                            <span>99 bhp</span>
                                        </li>
                                    </ul>
                                </div>
                                <div class="clear"></div>
                            </div>
                            <table class="item-table-content border-light-bottom" width="100%" cellspacing="0" cellpadding="0">
                                <thead>
                                    <tr class="table-head-row">
                                        <th valign="top" width="35%">Available in</th>
                                        <th valign="top" width="35%">Launched in</th>
                                        <th valign="top" width="30%">Unit sold (May)</th>
                                    </tr>
                                </thead>
                                <tbody>
                                    <tr>
                                        <td valign="top" class="text-bold text-grey">2 colors</td>
                                        <td valign="top" class="text-bold text-grey">May 2016</td>
                                        <td valign="top" class="text-bold text-grey">32,293</td>
                                    </tr>
                                </tbody>
                            </table>
                            <div class="padding-top10 margin-bottom10">
                                <p class="font13 text-grey">Ex-showroom price, Mumbai</p>
                                <div class="margin-bottom10">
                                    <span class="bwmsprite inr-xsm-icon"></span>
                                    <span class="font16 text-bold">60,873</span>
                                </div>
                                <button type="button" class="btn btn-white font14 btn-size-180">Check on-road price</button>
                            </div>
                            <p class="font14 text-light-grey margin-bottom15">The Jupiter is an 110cc scooter from TVS, positioned above the Wego. The country’s second best-selling scooter after the Activa, the Jupiter has been primarily targeted towards men, women also seem to buy it alot.</p>
                            <ul class="item-more-details-list">
                                <li>
                                    <a href="" title="Honda Activa 3G Reviews">
                                        <span class="generic-sprite reviews-sm"></span>
                                        <span class="icon-label">Reviews</span>
                                    </a>
                                </li>
                                <li>
                                    <a href="" title="Honda Activa 3G News">
                                        <span class="generic-sprite news-sm"></span>
                                        <span class="icon-label">News</span>
                                    </a>
                                </li>
                                <li>
                                    <a href="" title="Honda Activa 3G Videos">
                                        <span class="generic-sprite videos-sm"></span>
                                        <span class="icon-label">Videos</span>
                                    </a>
                                </li>
                                <li>
                                    <a href="" title="Honda Activa 3G Specs">
                                        <span class="generic-sprite specs-sm"></span>
                                        <span class="icon-label">Specs</span>
                                    </a>
                                </li>
                            </ul>
                            <div class="clear"></div>
                        </li>
                    </ul>
                </div>
            </div>
        </section>

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
                <span class="font14"><strong>Disclaimer:</strong></span> The list of top 10 bikes has been curated based on data collected from users of BikeWale. The best bike's list doesn't intend to comment anything on the quality of bikes in absolute terms. We don't comment anything about bikes or scooters which are not included in this list. The list is revised every month based on interest shown by users. The data for monthly unit sold which has been used for top 10 bikes has been taken from www.autopunditz.com. The unit sold is presented to help users make an informed decision.
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
