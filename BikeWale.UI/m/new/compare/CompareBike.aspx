<%@ Page Language="C#" AutoEventWireup="false" Inherits="Bikewale.Mobile.New.CompareBike" EnableViewState="false" %>

<%@ Register TagPrefix="CB" TagName="CompareBike" Src="/m/controls/CompareBikeMin.ascx" %>
<!DOCTYPE html>
<html>
<head>
<%

    if (pageMetas != null)
    {
        title = pageMetas.Title;
        keywords = pageMetas.Keywords;
        description = pageMetas.Description;
        canonical = pageMetas.CanonicalUrl;
        AdPath = "/1017752/Bikewale_Mobile_NewBikes";
        AdId = "1398766302464";
        Ad_320x50 = true;
        Ad_Bot_320x50 = true;
    }
    
%>

<!-- #include file="/includes/headscript_mobile_min.aspx" -->
<link rel="stylesheet" type="text/css" href="/m/css/compare/landing.css" />
<script type="text/javascript">
    <!-- #include file="\includes\gacode_mobile.aspx" -->
</script>
</head>
<body>
    <form id="form1" runat="server">
        <!-- #include file="/includes/headBW_Mobile.aspx" -->

        <section>
            <div class="container compare-landing-banner text-center">
                <h1 class="font24 text-uppercase text-white padding-top20 margin-bottom5">Compare bikes</h1>
                <h2 class="font14 text-unbold text-white">Making a decision is fairly easy</h2>
            </div>
        </section>

        <section id="compare-bike-landing" style="display: none" data-bind="visible: true">
            <div class="grid-12">
                <div class="container banner-box-shadow">
                    <div class="comparison-main-card">
                        <div class="bike-details-block" data-bind="css: matchBoxHeight() ? 'match-height' : ''">
                            <!-- ko if : !bike1() -->
                            <div class="compare-box-placeholder" data-bind="click: function (d, e) { openBikeSelection(bike1) }">
                                <div class="bike-icon-wrapper">
                                    <span class="grey-bike"></span>
                                    <p class="font14 text-light-grey">Add bike 1</p>
                                </div>
                            </div>
                            <!-- /ko -->
                            <!-- ko if : bike1() -->
                            <div class="selected-bike-box">
                                <span class="close-selected-bike position-abt pos-right5 bwmsprite cross-sm-dark-grey" data-bind="click: function () { bike1(null); }"></span>
                                <a data-bind="attr: { 'title': bike1().bikeName, 'data-versionid' : bike1().version.versionId  }" href="javascript:void(0)" title="" class="block margin-top10" rel="nofollow">
                                    <span class="font12 text-default text-truncate" data-bind="text: bike1().make.name"></span>
                                    <h2 class="font12 text-truncate margin-bottom5" data-bind="text: bike1().model.modelName"></h2>
                                    <img class="bike-image-block" data-bind="attr: { 'alt': bike1().bikeName, 'src': bike1().hostUrl + '/110x61/' + bike1().originalImagePath }" src="" alt="">
                                </a>
                                <p class="label-text">Version:</p>
                                <p class="font12 margin-bottom5 text-bold text-truncate" data-bind="text: bike1().version.versionName"></p>
                                <p class="text-truncate label-text">Ex-showroom, Mumbai</p>
                                <p>
                                    <span class="bwmsprite inr-xsm-icon" data-bind="visible: bike1().price != '0'"></span><span class="font16 text-bold" data-bind="text: bike1().price != '0' ? bike1().price : 'Price not available'"></span>
                                </p>
                            </div>
                            <!-- /ko -->
                        </div>
                        <div class="bike-details-block" data-bind="css: matchBoxHeight() ? 'match-height' : ''">
                            <!-- ko if : !bike2() -->

                            <div class="compare-box-placeholder" data-bind="click: function (d, e) { openBikeSelection(bike2); }">
                                <div class="bike-icon-wrapper">
                                    <span class="grey-bike"></span>
                                    <p class="font14 text-light-grey">Add bike 2</p>
                                </div>
                            </div>
                            <!-- /ko -->
                            <!-- ko if : bike2() -->
                            <div class="selected-bike-box">
                                <span class="close-selected-bike position-abt pos-right5 bwmsprite cross-sm-dark-grey" data-bind="click: function () { bike2(null); }"></span>
                                <a data-bind="attr: { 'title': bike2().bikeName, 'data-versionid' : bike2().version.versionId }" href="javascript:void(0)" title="" class="block margin-top10" rel="nofollow">
                                    <span class="font12 text-default text-truncate" data-bind="text: bike2().make.name"></span>
                                    <h2 class="font12 text-truncate margin-bottom5" data-bind="text: bike2().model.modelName"></h2>
                                    <img class="bike-image-block" data-bind="attr: { 'alt': bike2().bikeName, 'src': bike2().hostUrl + '/110x61/' + bike2().originalImagePath }" src="" alt="">
                                </a>
                                <p class="label-text">Version:</p>
                                <p class="font12 margin-bottom5 text-bold text-truncate" data-bind="text: bike2().version.versionName"></p>
                                <p class="text-truncate label-text">Ex-showroom, Mumbai</p>
                                <p>
                                    <span class="bwmsprite inr-xsm-icon" data-bind="visible: bike2().price != '0'"></span><span class="font16 text-bold" data-bind="    text: bike2().price != '0' ? bike2().price : 'Price not available'"></span>
                                </p>
                            </div>
                            <!-- /ko -->
                        </div>
                        <div class="padding-bottom15 text-center" data-bind="visible: bike1() && bike2()">
                            <a href="javascript:void(0)" data-bind="attr: { 'href': compareLink }" class="btn btn-orange btn-size-1" rel="nofollow">Compare now</a>
                        </div>
                        <div class="clear"></div>
                    </div>
                </div>
            </div>
            <div class="clear"></div>

            <% if (objMakes != null)
               { %>
            <!-- select bike starts here -->
            <div id="select-bike-cover-popup" data-bind="with: bikeSelection" class="cover-window-popup">
                <div class="ui-corner-top">
                    <div id="close-bike-popup" class="cover-popup-back cur-pointer leftfloat" data-bind="click: closeBikePopup">
                        <span class="bwmsprite fa-angle-left"></span>
                    </div>
                    <div class="cover-popup-header leftfloat">Select bikes</div>
                    <div class="clear"></div>
                </div>
                <div class="bike-banner"></div>
                <div id="select-make-wrapper" class="cover-popup-body" data-bind="visible: currentStep() == 1">
                    <div class="cover-popup-body-head">
                        <p class="no-back-btn-label head-label inline-block">Select Make</p>
                    </div>
                    <ul class="cover-popup-list with-arrow">
                        <% foreach (var make in objMakes)
                           { %>
                        <li data-bind="click: makeChanged"><span data-masking="<%= make.MaskingName %>" data-id="<%= make.MakeId %>"><%= make.MakeName %></span></li>
                        <% } %>
                    </ul>
                </div>

                <div id="select-model-wrapper" class="cover-popup-body" data-bind="visible: currentStep() == 2">
                    <div class="cover-popup-body-head">
                        <div data-bind="click: modelBackBtn" class="body-popup-back cur-pointer inline-block">
                            <span class="bwmsprite back-long-arrow-left"></span>
                        </div>
                        <p class="head-label inline-block">Select Model</p>
                    </div>
                    <ul class="cover-popup-list with-arrow" data-bind="foreach: modelArray">
                        <li data-bind="click: $parent.modelChanged">
                            <span data-bind="text: modelName, attr: { 'data-id': modelId, 'data-masking': maskingName }"></span>
                        </li>
                    </ul>
                </div>

                <div id="select-version-wrapper" class="cover-popup-body" data-bind="visible : currentStep()==3">
                    <div class="cover-popup-body-head">
                        <div data-bind="click: versionBackBtn" class="body-popup-back cur-pointer inline-block">
                            <span id="arrow-version-back" class="bwmsprite back-long-arrow-left"></span>
                        </div>
                        <p class="head-label inline-block">Select Version</p>
                    </div>
                    <ul class="cover-popup-list" data-bind="foreach: versionArray">
                        <li data-bind="click: $parent.versionChanged">
                            <span data-bind="text: versionName, attr: { 'data-id': versionId }"></span>
                        </li>
                    </ul>
                </div>

                <div class="cover-popup-loader-body" data-bind="visible : IsLoading()">
                    <div class="cover-popup-loader"></div>
                    <div class="cover-popup-loader-text font14" data-bind="text: LoadingText()"></div>
                </div>
            </div>
            <!-- select bike ends here -->
            <% } %>

            <div class="same-version-toast">
                <p>Please select different bike for comparision.</p>
            </div>
        </section>

        <section>
            <div class="container card-bottom-margin">
                <h2 class="font18 font-bold text-center margin-top20 margin-bottom10">Bike comparison</h2>
                <div class="content-box-shadow content-inner-block-20 collapsible-content font14 text-slate-grey">
                    <p class="main-content">Lorem ipsum dolor sit amet, consectetur adipiscing elit. Pellentesque semper sem id elit volutpat, eget lobortis neque auctor. Nunc auctor quam in ipsum euismod porta. Integer lobortis cursus ultricies</p>
                    <p class="more-content">Vivamus sit amet ultricies justo. Integer in leo dapibus, tincidunt libero eu, euismod leo. Suspendisse pellentesque risus dolor</p>
                    <a href="javascript:void(0)" class="read-more-target" rel="nofollow">...Read more</a>
                </div>
            </div>
        </section>

        <section>
            <div class="container card-bottom-margin">
                <h2 class="font18 font-bold text-center margin-top20 margin-bottom10">Popular comparison</h2>
                <div class="bw-tabs-panel content-box-shadow">
                    <div class="bw-tabs bw-tabs-flex">
                        <ul>
                            <li class="active" data-tabs="typeBike">Bikes</li>
                            <li data-tabs="typeScooter">Scooters</li>
                            <li data-tabs="typeCruiser">Cruisers</li>
                            <li data-tabs="typeSport">Sports</li>
                        </ul>
                    </div>
                    <div id="typeBike" class="bw-tabs-data">
                        <div class="swiper-container comparison-swiper card-container">
                            <div class="swiper-wrapper">
                                <div class="swiper-slide">
                                    <div class="swiper-card">
                                        
                                    </div>
                                </div>                               
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </section>

        <section>
            <div class="container card-bottom-margin">
                <h2 class="font18 font-bold text-center margin-top20 margin-bottom10">Comparison tests</h2>
                <div class="content-box-shadow content-inner-block-20 font14">
                    <div class="model-expert-review-container">
                        <div class="margin-bottom20">
                            <div class="review-image-wrapper">
                                <a href="/m/expert-reviews/tvs-apache-rtr-200-4v-carb-vs-bajaj-pulsar-as200-vs-honda-hornet-160r-comparison-test-23801.html">
                                    <img alt="TVS Apache RTR 200 4V Carb vs Bajaj Pulsar AS200 vs Honda Hornet 160R: Comparison Test" title="TVS Apache RTR 200 4V Carb vs Bajaj Pulsar AS200 vs Honda Hornet 160R: Comparison Test" src="https://imgd.aeplcdn.com//144x81//bw/ec/23801/TVS-Apache-RTR-200-4V-Action-74532.jpg?wm=2&amp;q=70">
                                </a>
                            </div>
                            <div class="review-heading-wrapper">
                                <a href="/m/expert-reviews/tvs-apache-rtr-200-4v-carb-vs-bajaj-pulsar-as200-vs-honda-hornet-160r-comparison-test-23801.html" title="TVS Apache RTR 200 4V Carb vs Bajaj Pulsar AS200 vs Honda Hornet 160R: Comparison Test" class="font14 target-link">TVS Apache RTR 200 4V Carb vs Bajaj...</a>
                                <div class="grid-7 alpha padding-right5">
                                    <span class="bwmsprite calender-grey-sm-icon"></span>
                                    <span class="article-stats-content">Jun 17, 2016</span>
                                </div>
                                <div class="grid-5 alpha omega">
                                    <span class="bwmsprite author-grey-sm-icon"></span>
                                    <span class="article-stats-content">Vikrant Singh</span>
                                </div>
                                <div class="clear"></div>
                            </div>
                            <p class="margin-top10">
                                What we have here are two obvious players in the affordable and performance centric commuter class and one outsider of sorts The TVS Apache 200 4V ndash one of the obvious...
                            </p>
                        </div>
                        <div class="margin-bottom20">
                            <div class="review-image-wrapper">
                                <a href="/m/expert-reviews/tvs-apache-rtr-200-4v-carb-vs-bajaj-pulsar-as200-vs-honda-hornet-160r-comparison-test-23801.html">
                                    <img alt="TVS Apache RTR 200 4V Carb vs Bajaj Pulsar AS200 vs Honda Hornet 160R: Comparison Test" title="TVS Apache RTR 200 4V Carb vs Bajaj Pulsar AS200 vs Honda Hornet 160R: Comparison Test" src="https://imgd.aeplcdn.com//144x81//bw/ec/23801/TVS-Apache-RTR-200-4V-Action-74532.jpg?wm=2&amp;q=70">
                                </a>
                            </div>
                            <div class="review-heading-wrapper">
                                <a href="/m/expert-reviews/tvs-apache-rtr-200-4v-carb-vs-bajaj-pulsar-as200-vs-honda-hornet-160r-comparison-test-23801.html" title="TVS Apache RTR 200 4V Carb vs Bajaj Pulsar AS200 vs Honda Hornet 160R: Comparison Test" class="font14 target-link">TVS Apache RTR 200 4V Carb vs Bajaj...</a>
                                <div class="grid-7 alpha padding-right5">
                                    <span class="bwmsprite calender-grey-sm-icon"></span>
                                    <span class="article-stats-content">Jun 17, 2016</span>
                                </div>
                                <div class="grid-5 alpha omega">
                                    <span class="bwmsprite author-grey-sm-icon"></span>
                                    <span class="article-stats-content">Vikrant Singh</span>
                                </div>
                                <div class="clear"></div>
                            </div>
                            <p class="margin-top10">
                                What we have here are two obvious players in the affordable and performance centric commuter class and one outsider of sorts The TVS Apache 200 4V ndash one of the obvious...
                            </p>
                        </div>
                        <div class="margin-bottom20">
                            <div class="review-image-wrapper">
                                <a href="/m/expert-reviews/tvs-apache-rtr-200-4v-carb-vs-bajaj-pulsar-as200-vs-honda-hornet-160r-comparison-test-23801.html">
                                    <img alt="TVS Apache RTR 200 4V Carb vs Bajaj Pulsar AS200 vs Honda Hornet 160R: Comparison Test" title="TVS Apache RTR 200 4V Carb vs Bajaj Pulsar AS200 vs Honda Hornet 160R: Comparison Test" src="https://imgd.aeplcdn.com//144x81//bw/ec/23801/TVS-Apache-RTR-200-4V-Action-74532.jpg?wm=2&amp;q=70">
                                </a>
                            </div>
                            <div class="review-heading-wrapper">
                                <a href="/m/expert-reviews/tvs-apache-rtr-200-4v-carb-vs-bajaj-pulsar-as200-vs-honda-hornet-160r-comparison-test-23801.html" title="TVS Apache RTR 200 4V Carb vs Bajaj Pulsar AS200 vs Honda Hornet 160R: Comparison Test" class="font14 target-link">TVS Apache RTR 200 4V Carb vs Bajaj...</a>
                                <div class="grid-7 alpha padding-right5">
                                    <span class="bwmsprite calender-grey-sm-icon"></span>
                                    <span class="article-stats-content">Jun 17, 2016</span>
                                </div>
                                <div class="grid-5 alpha omega">
                                    <span class="bwmsprite author-grey-sm-icon"></span>
                                    <span class="article-stats-content">Vikrant Singh</span>
                                </div>
                                <div class="clear"></div>
                            </div>
                            <p class="margin-top10">
                                What we have here are two obvious players in the affordable and performance centric commuter class and one outsider of sorts The TVS Apache 200 4V ndash one of the obvious...
                            </p>
                        </div>
                        <div class="view-all-btn-container">
                            <a title="" href="" class="bw-ga btn view-all-target-btn">View all<span class="bwmsprite teal-right"></span></a>
                        </div>
                    </div>
                </div>
            </div>
        </section>

        <section>
            <div class="container margin-bottom10">
                <div class="grid-12 alpha omega">
                    <CB:CompareBike ID="ctrlCompareBikes" runat="server"></CB:CompareBike>
                </div>
                <div class="clear"></div>
            </div>
        </section>
        
        <script type="text/javascript" src="<%= staticUrl != "" ? "https://st1.aeplcdn.com" + staticUrl : "" %>/m/src/frameworks.js?<%= staticFileVersion %>"></script>
        <!-- #include file="/includes/footerBW_Mobile.aspx" -->
        <link href="<%= staticUrl != "" ? "https://st2.aeplcdn.com" + staticUrl : "" %>/m/css/bwm-common-btf.css?<%= staticFileVersion %>" rel="stylesheet" type="text/css" />
        <!-- #include file="/includes/footerscript_mobile.aspx" -->
        <script type="text/javascript">
            var compareSource = <%= (int) Bikewale.Entities.Compare.CompareSources.Mobile_CompareBike_UserSelection %> ;  
        </script>
        <script type="text/javascript" src="<%= staticUrl != "" ? "https://st1.aeplcdn.com" + staticUrl : ""%>/m/src/compare/landing.js?<%= staticFileVersion %>"></script>
        <link href='https://fonts.googleapis.com/css?family=Open+Sans:400,600,700' rel='stylesheet' type='text/css' />
    </form>
</body>
</html>
