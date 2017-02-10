<%@ Page Language="C#" AutoEventWireup="false" Inherits="Bikewale.Mobile.New.CompareBike" EnableViewState="false" %>

<%@ Register TagPrefix="CB" TagName="CompareBike" Src="/m/controls/CompareBikeMin.ascx" %>
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

        <section id="compare-bike-landing" style="display: none" data-bind="visible: true">
            <div class="container box-shadow bg-white card-bottom-margin bw-tabs-panel">
                <h1 class="box-shadow padding-15-20 margin-bottom3 text-bold">Compare Bikes</h1>
                <div class="comparison-main-card">
                    <div class="bike-details-block ">
                        <!-- ko if : !bike1() -->
                        <div class="compare-box-placeholder" data-bind="click: function (d, e) { openBikeSelection(bike1) }">
                            <div class="bike-icon-wrapper">
                                <span class="grey-bike"></span>
                                <p class="font14 text-light-grey">Tap to select bike 1</p>
                            </div>
                        </div>
                        <!-- /ko -->
                        <!-- ko if : bike1() -->
                        <span class="close-selected-bike position-abt pos-right5 bwmsprite cross-sm-dark-grey" data-bind="click: function () { bike1(null); }"></span>
                        <a data-bind="attr: { 'title': bike1().bikeName, 'href': bike1().bikeUrl }" href="javscript:void(0)" title="" class="block margin-top10">
                            <span class="font12 text-light-grey text-truncate" data-bind="text: bike1().make.name"></span>
                            <h2 class="font14 text-truncate margin-bottom5" data-bind="text: bike1().model.modelName"></h2>
                            <img class="bike-image-block" data-bind="attr: { 'alt': bike1().bikeName, 'src': bike1().hostUrl + '/110x61/' + bike1().originalImagePath }" src="" alt="">
                        </a>
                        <p class="label-text">Version:</p>
                        <p class="padding-bottom10 text-bold dropdown-selected-item option-count-one dropdown-width text-truncate" data-bind="text: bike1().version.versionName"></p>
                        <p class="text-truncate label-text">Ex-showroom, Mumbai</p>
                        <p class="margin-bottom10">
                            <span class="bwmsprite inr-xsm-icon margin-right5" data-bind="visible: bike1().price != '0'"></span><span class="font16 text-bold" data-bind="    text: bike1().price != '0' ? bike1().price : 'Price not available'"></span>
                        </p>
                        <!-- /ko -->
                    </div>
                    <div class="bike-details-block ">
                        <!-- ko if : !bike2() -->

                        <div class="compare-box-placeholder" data-bind="click: function (d, e) { openBikeSelection(bike2); }">
                            <div class="bike-icon-wrapper">
                                <span class="grey-bike"></span>
                                <p class="font14 text-light-grey">Tap to select bike 2</p>
                            </div>
                        </div>
                        <!-- /ko -->
                        <!-- ko if : bike2() -->
                        <span class="close-selected-bike position-abt pos-right5 bwmsprite cross-sm-dark-grey" data-bind="click: function () { bike2(null); }"></span>
                        <a data-bind="attr: { 'title': bike2().bikeName, 'href': bike2().bikeUrl }" href="javscript:void(0)" title="" class="block margin-top10">
                            <span class="font12 text-light-grey text-truncate" data-bind="text: bike2().make.name"></span>
                            <h2 class="font14 text-truncate margin-bottom5" data-bind="text: bike2().model.modelName"></h2>
                            <img class="bike-image-block" data-bind="attr: { 'alt': bike2().bikeName, 'src': bike2().hostUrl + '/110x61/' + bike2().originalImagePath }" src="" alt="">
                        </a>
                        <p class="label-text">Version:</p>
                        <p class="padding-bottom10 text-bold dropdown-selected-item option-count-one dropdown-width text-truncate" data-bind="text: bike2().version.versionName"></p>
                        <p class="text-truncate label-text">Ex-showroom, Mumbai</p>
                        <p class="margin-bottom10">
                            <span class="bwmsprite inr-xsm-icon margin-right5" data-bind="visible: bike2().price != '0'"></span><span class="font16 text-bold" data-bind="    text: bike2().price != '0' ? bike2().price : 'Price not available'"></span>
                        </p>
                        <!-- /ko -->
                    </div>
                    <div class="padding-bottom15 text-center" data-bind="visible: bike1() && bike2()">
                        <a href="javascript:void(0)" data-bind="attr: { 'href': compareLink }" class="btn btn-white btn-size-1" rel="nofollow">Compare Now</a>
                    </div>

                    <div class="clear"></div>
                </div>
            </div>

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
            <div class="container margin-bottom20">
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
        <script type="text/javascript" src="<%= staticUrl != "" ? "https://st1.aeplcdn.com" + staticUrl : ""%>/m/src/compare/landing.js?<%= staticFileVersion %>"></script>
        <link href='https://fonts.googleapis.com/css?family=Open+Sans:400,600,700' rel='stylesheet' type='text/css' />
    </form>
</body>
</html>
