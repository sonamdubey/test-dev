
<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="UsedSearch.aspx.cs" Inherits="Carwale.UI.Used.Search" Trace="false"  EnableViewState="false" EnableEventValidation="false" %>
<%@ Import Namespace="System.Data" %>
<%@ Import Namespace="Carwale.Utility" %>
<%@ Import Namespace="Carwale.UI.NewCars.RecommendCars" %>
<%@ Import Namespace="Carwale.Entity.Enum" %>
<%@ Import Namespace="Carwale.Utility.Classified" %>
<!doctype html>
<html itemscope itemtype="http://schema.org/WebPage">
<head>
   
    <%
        PageId = 22;
        Title = title;
        Description = pageDescription;
        Revisit = "10";
        DocumentState = "Static";
        AdId = "1397545875283";
        AdPath = "/1017752/UsedCarSearch_";
        canonical = canonicalUS;
        altUrl = altURL;
        ampUrl = ampURL;
        prevPageUrl = prev;
        nextPageUrl = next;
        Keywords = pageKeywords;
    %>
    <!-- #include file="/includes/global/head-script.aspx" -->
    <style>
        .hideImportant {display: none !important;}
        .text-grey-count {color:#aaa;}
        .shortlistCarthumb img{width:100%;}
        .border-none {border:none!important;}
        .main-filter-list{width: 229px; z-index:0;}
    </style>
    <!--[if IE]> <style> .stock-list li .btn-orange { font-weight: normal !important; } </style> <![endif]-->
     <script type="text/javascript">
         var carTradeCertificationId = '<%= ConfigurationManager.AppSettings["CartradeCertificationId"].ToString()%>';
         var financeIframeUrl = '<%= ConfigurationManager.AppSettings["CTFinanceDesktop"].ToString()%>';
         var excludeStocks = '<%= excludeStocks%>';
    </script>

</head>
<body class="bg-white header-fixed-inner special-skin-body no-bg-color model-special-page usedlist-special-page">
    <div id="financeIframe" class="iframe-popup-content hide">
        <span class="similarcar-close position-abt pos-top10 pos-right10 cur-pointer popup-close-esc-key" id="iframe-close">
            <span class="cwsprite cross-lg-white"></span>
        </span>
         <div class="text-white" style="background: #333;padding: 18px">
        </div>
        <div class="clear"></div>
        <div class="search-loading-pic popup-loading-pic hideImportant" style="overflow: hidden; display: block;">
            <img src="https://imgd.aeplcdn.com/0x0/statics/loader.gif" width="16" height="16" alt="" title="">
        </div>       
        <div id="iframecontent" class="iframe-content"></div>
        <div id="iframeTimeOut" data-overlay-theme="a" data-theme="c" data-dismissible="false" class="text-center timeout-popup rounded-corner2 content-box-shadow hide">
            <span class="error padding-left10 padding-right10 block">We could not fetch loan details right now. Please try again later.</span>
            <a href="javascript:void(0)" data-role="button" class="btn btn-xs btn-white btn-full-width margin-top15" id="iframeTimeOutClick">OK</a>
        </div>
    </div>
    <div class="valuation-popup hide">
		<div class="box-shadow popup-placeholder">
			<span class="cwsprite valuation-popup__close popup-close-esc-key" id="valuation-popup-close"></span>
			<h3 class="valuation-popup__head">Right Price</h3>
			<span class="valuation-popup-loading-pic"></span>

			<div class="valuation-popup-content">
			<div class="valuation-popup__right-price"></div>
                <div class="valuation-other-details hide">
                    <div class="valuation-popup__form">
                        <div class="form__user-details">
							<p class="form__heading">Fill in your details</p>
							<div class="form-control-box form-field__name">
								<input type="text" id="rpName" class="form-control padding-left40" placeholder="Name" maxlength="50">
								<span class="cw-used-sprite uc-uname"></span>
								<span class="cw-blackbg-tooltip"></span>
								<span class="cwsprite error-icon"></span>
							</div>
                            <div class="form-control-box form-field__mobile">
                                <span class="form-sprite-16 mobile-icon-16"></span>
                                <span class="form-field__mobile-prefix">+91</span>
                                <input type="tel" id="rpMobile" class="form-control" placeholder="Mobile number" maxlength="10">
                                <span id="rp_mobileError" class="cw-blackbg-tooltip"></span>
                                <span class="cwsprite error-icon"></span>
                            </div>
                            <div class="form-control-box form-field__email optional-email hide">
                                <span class="cw-used-sprite uc-email"></span>
                                <input type="text" id="txtEmail" class="form-control" placeholder="Email (optional)">
                                <span id="rp_emailError" class="cw-blackbg-tooltip"></span>
                                <span class="cwsprite error-icon"></span>
                            </div>
                            <div class="form-control-box email-update-field clearfloat">
                                <span id="emailTick" class="cw-used-sprite uc-unchecked leftfloat"></span>
                                <p class="email-update__text">Get updates on email</p>
                            </div>
                            <div style="margin-bottom:20px;" class="position-rel dealer-rating-container inline-block"><span class="top-rated-seller-tag">Top Rated Seller</span><span class="dealer-rating-tooltip dealer-rating-tooltip--left">This seller has consistently been rated well by his customers</span></div>
                            <div class="btn-container">
                                <div id="rp_chat_btn_container" class="grid-4 chat-btn-container">
                                    <span class="btn-xs chat-btn">
                                        <span class="js-threedot-loader">
                                            <span class="chat-icon"></span>
                                            <span class="chat-btn__text">Chat</span>
                                        </span>
                                        <% RazorPartialBridge.RenderPartial( "~/Views/Shared/_ThreeDotLoader.cshtml" ); %>
                                    </span>
                                </div>
                                <a href="javascript:void(0)" id="rpgetsellerDetails" class="btn btn-orange btn-xs margin-bottom10 seller-btn-container right-price-btn"><span class="gsdTxt">Get Seller Details</span><span class="oneClickDetails hide font18">1-Click <span class="font11">View Details</span></span></a>
                                <a href="javascript:void(0)" id="rpgettingDetails" class="btn btn-xs btn-orange text-uppercase margin-bottom10 right-price-btn hide" style="">Getting Details <span>.</span><span>.</span><span>.</span></a>       
                            </div>
                            <div class="clear"></div>
                            <p style="text-align:left; font-size:11px; margin-top:10px">By submitting this form you agree to our <a href="/visitoragreement.aspx" target="_blank">terms and conditions</a></p>
                        </div>
                        <div class="content-inner-block-10 alert hide" id="rpnot_auth">
                            <div class="back-to-gsd-form">
                                <span>Information you provided was invalid. Please provide valid information.</span>
                                <p style="color: #0288d1; cursor: pointer">Change your number</p>
                            </div>
                        </div>
                        <div class="form__seller-details hide">
                            <p class="form__heading">Seller details</p>
                            <ul class="form__details-list">
                                <li>
                                    <span class="form-sprite-16 dealer-icon-16 list-item__icon-col"></span>
                                    <div class="list-item__details-col">
                                        <p class="font16 text-bold seller-details-left-name-field" id="sellerName"></p><div class="position-rel dealer-rating-container inline-block seller-detail__top-rated"><span class="top-rated-seller-tag top4">Top Rated Seller</span><span class="dealer-rating-tooltip dealer-rating-tooltip--left">This seller has consistently been rated well by his customers</span></div>
                                        <p class="font12" id="sellerContactPerson"></p>

                                        <a href="#" class="font14 seller-virtual-link margin-top5" target="_blank">Check other cars from this seller</a>

                                    </div>
                                </li>
                                <li>
                                    <span class="form-sprite-16 mobile-icon-16 list-item__icon-col"></span>
                                    <p class="list-item__details-col" id="sellerContact"></p>
                                </li>
                                <li>
                                    <span class="form-sprite-16 email-icon-16 list-item__icon-col"></span>
                                    <p class="list-item__details-col" id="sellerEmail"></p>
                                </li>
                                <li>
                                    <span class="form-sprite-16 loc-icon-16 list-item__icon-col"></span>
                                    <p class="list-item__details-col" id="sellerAddress"></p>
                                </li>
                            </ul>
                            <p class="font12 margin-bottom15">Your contact details have been shared with the seller.</p>
                            <a href="#" target="_blank" class="font14 text-link bp-SimilarCars hide">View similar cars >></a>
                        </div>
                    </div>

                    <div class="valuation-popup__similar-cars hide">
                        <ul class="recommendationList margin-top10" id="rprecommendations">
                        </ul>
                    </div>
                    <span id="valuationPopupSlideBtn" class="similar-cars__slide-btn">
                        <span class="arrow-sprite-9 left-icon-9"></span>
                    </span>

                    <span class="similar-cars__slide-tooltip coachmark">
                        <span class="coachmark-arrow-top"></span>
                        <span id="slideBtnTooltipLabel">Recommended cars</span>
                    </span>
                </div>
            </div>
        </div>
    </div>
    <form id="Form1" runat="server">
    <!-- #include file="/includes/header.aspx" -->
        <section class="container">
            <div class="grid-12">
                <div class="inline-block position-rel">
                    <div class="position-abt pos-right0 pos-top-minus-15 font11 text-medium-grey">Ad</div>
                    <div class="padding-bottom5 text-center" id="Leader_Board">
                    </div>
                </div>
            </div>
            <div class="clear"></div>
        </section>
        <div class="clear"></div>
    <!-- New used search page code starts here-->
        <section class="bg-light-grey padding-top10 padding-bottom20 no-bg-color">
            <div class="container">
                <div class="grid-12">
                    <div class="breadcrumb margin-bottom5">
                        <!-- breadcrumb code starts here -->
                        <ul id="breadCrumb" class="breadcrumb leftfloat special-skin-text" runat="server"></ul>
                        <div class="clear"></div>
                    </div>
                    <!-- breadcrumb code ends here -->
                    <h1 class="font30 text-black special-skin-text"><%= pageTitle %><span class="font16 text-light-grey text-unbold special-skin-text">&nbsp;<span id="totalCount"><%=filtersCount %></span>&nbsp;<span>Cars</span></span></h1>
                    <div class="border-solid-bottom margin-top5 margin-bottom10"></div>
                </div>
                <div class="clear"></div>
                <div id="filters" class="grid-3 page-heading position-rel">
                    <!-- Search filter code starts here -->
                    <div class="content-box-shadow filters main-filter-list">
                        <div class="font14">
                            <div id="filterStart"></div>
                            <!-- floating filter area code starts here-->
                            <div id="floating-box" style="position: relative;">
                                <!-- mainPart code ends here-->
                                <div id="mainPart">
                                    <div class="content-inner-block-10 bg-light-grey">
                                        <p class="font20 leftfloat text-bold">Filter</p>
                                        <p class="text-link rightfloat margin-top5" id="resetFilters"><span class="fa fa-repeat text-light-grey"></span>&nbsp;Reset</p>
                                        <div class="clear"></div>
                                    </div>

                                    <!--Select City Start-->
                                    <div class="filter-set content-inner-block-10 position-rel" name="city">
                                        <div class="coachmark filters-coachmark hide">
                                            <span class="coachmark-arrow-left"></span>
                                            <p class="font16 text-bold">Filters</p>
                                            <p class="font16 inline-block">Refine your search using these Filters</p>
                                            <p class="inline-block margin-left10"><a id="filterCoachmark" class="btn btn-green btn-green-sm">Next</a></p>
                                            <p class="margin-top5"><a class="nomoreTips cur-pointer">Don't show anymore tips</a></p>
                                        </div>
                                        <h3 class="sub-values">Select City<span class="fa fa-angle-up"></span></h3>
                                        <div class="padding-top10">
                                            <div class="form-control-box">
                                                <span class="select-box fa fa-angle-down"></span>
                                                <asp:DropDownList runat="server" ID="drpCity" CssClass="form-control "></asp:DropDownList>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="border-solid-top"></div>
                                    <!--Select City End-->
                                    <!--Budget code starts here -->
                                    <div class="filter-set content-inner-block-10" name="budget">
                                        <h3 class="sub-values">Budget
                                <span class="fa fa-angle-up"></span>
                                        </h3>
                                        <div class="slider-box padding-top10 budgetContainerMain">
                                            <div class="used-budget-box">
                                                <div id="minMaxContainer" class="border-solid">
                                                    <span class="leftfloat" id="budgetBtn">Choose budget</span>
                                                    <span id="upDownArrow" class="rightfloat fa fa-angle-down"></span>
                                                    <div class="clear"></div>
                                                </div>
                                                <div id="budgetListContainer" class="hide">
                                                    <div id="userBudgetInput">
                                                        <input type="text" class="priceBox" id="minInput" placeholder="Min" maxlength="5">
                                                        <input type="text" class="priceBox" id="maxInput" placeholder="Max" maxlength="5">
                                                        <div class="cw-blackbg-tooltip cw-blackbg-tooltip-search-error text-center hide">Max budget should be greater than Min budget.</div>
                                                    </div>
                                                    <ul id="minPriceList" class="text-left">
                                                        <li data-min-price="Any">Any</li>
                                                        <li data-min-price="1">1 Lakh</li>
                                                        <li data-min-price="3">3 Lakh</li>
                                                        <li data-min-price="4">4 Lakh</li>
                                                        <li data-min-price="6">6 Lakh</li>
                                                        <li data-min-price="8">8 Lakh</li>
                                                        <li data-min-price="12">12 Lakh</li>
                                                        <li data-min-price="20">20 Lakh</li>
                                                    </ul>
                                                    <ul id="maxPriceList" class="text-right  hide">
                                                        <li data-max-price="12">12 Lakh</li>
                                                        <li data-max-price="15">15 Lakh</li>
                                                        <li data-max-price="20">20 Lakh</li>
                                                        <li data-max-price="30">30 Lakh</li>
                                                        <li data-max-price="40">40 Lakh</li>
                                                        <li data-max-price="50">50 Lakh</li>
                                                        <li data-max-price="60">60 Lakh</li>
                                                        <li data-max-price="Any">Any</li>
                                                    </ul>
                                                </div>
                                            </div>
                                            <div id="btnSetBudget" class="go-btn-box btn btn-xs btn-white text-uppercase">Go</div>
                                            <div class="clear"></div>
                                        </div>
                                    </div>
                                    <div class="border-solid-top"></div>
                                    <!--Budget code ends here -->

                                    <!--Filter By Start-->
                                    <div class="filter-set content-inner-block-10 position-rel" id="1" name="filterbyadditional" select="false">
                                        <h3 class="sub-values">Only Show Cars With
                                <span class="fa fa-angle-up"></span>
                                        </h3>
                                        <div class="list-points margin-top20">
                                            <ul>
                                                <%-- To be uncommented when enough franchise stocks have been added.
                                                    <li class="us-sprite" filterid="3" name="FranchiseeDealer"><span class="filterText">Franchisee Dealer</span>
                                                    <span class="text-grey-count">&nbsp;(<span class="count-box"></span>)</span>
                                                    </li>
                                                --%>
                                                <li class="us-sprite" filterid="2" name="CarsWithPhotos"><span class="filterText">Cars with Photos</span>
                                                    <span class="text-grey-count">&nbsp;(<span class="count-box"></span>)</span>
                                                </li>
                                                <li class="us-sprite" filterid="1" name="CarTradeCertifiedCars"><span class="filterText">Certified Cars</span>
                                                    <span class="text-grey-count">&nbsp;(<span class="count-box"></span>)</span>
                                                </li>
                                            </ul>
                                        </div>
                                    </div>
                                    <div class="border-solid-top"></div>
                                    <!--Filter By End-->
                                    <!--Manufacturer code starts here -->
                                    <div class="manufacture-box content-inner-block-10">
                                        <h3 class="sub-values">Manufacturer
                                    <span class="fa fa-angle-up"></span>
                                        </h3>
                                        <div class="extra-fields padding-top10">
                                            <div class="custom-search-box position-rel">
                                                <input class="form-control" type="text" name="search" id="tags" placeholder="Select Manufacturer" />
                                                <div class="position-abt pos-top5 pos-right10 font20 text-light-grey"><span class="fa fa-search"></span></div>
                                            </div>
                                            <div id="manu-box">
                                                <div class="filter-set margin-top20">
                                                    <div class="list-points-makes ul-makes manufacture-list">
                                                        <ul class="ul-makes" id="makesList">
                                                            <asp:Repeater ID="rptMakes" runat="server">
                                                                <ItemTemplate>
                                                                    <li class="us-sprite makeLi" carfilterid="<%# DataBinder.Eval(Container.DataItem,"MakeId") %>">
                                                                        <span class="filterText"><%# DataBinder.Eval(Container.DataItem,"MakeName") %></span>
                                                                        <span class="text-grey-count">&nbsp;(<span class="count-box">300</span>)</span>
                                                                    </li>
                                                                </ItemTemplate>
                                                            </asp:Repeater>
                                                        </ul>
                                                    </div>
                                                </div>
                                                <div class="clear"></div>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="border-solid-top"></div>
                                    <!--Manufacturer code ends here -->
                                    <!--Color Start-->
                                    <div class="filter-set content-inner-block-10 " id="8" name="color" select="false">
                                        <h3 class="sub-values">Colours<span class="fa fa-angle-down"></span></h3>
                                        <div class="list-points padding-top10 hide">
                                            <ul class="colorsList">
                                                <li class="us-sprite" filterid="12" name="White">
                                                    <span class="filterText">White
                                            <span class="white-bx"></span>
                                                    </span>
                                                    <span class="text-grey-count">&nbsp;(<span class="count-box">300</span>)</span>
                                                </li>
                                                <li class="us-sprite" filterid="2" name="Black">
                                                    <span class="filterText">Black
                                            <span class="black-bx"></span>
                                                    </span>
                                                    <span class="text-grey-count">&nbsp;(<span class="count-box">300</span>)</span>
                                                </li>
                                                <li class="us-sprite" filterid="11" name="Silver">
                                                    <span class="filterText">Silver
                                            <span class="silver-bx"></span>
                                                    </span>
                                                    <span class="text-grey-count">&nbsp;(<span class="count-box">300</span>)</span>
                                                </li>
                                                <li class="us-sprite" filterid="10" name="Red">
                                                    <span class="filterText">Red
                                            <span class="red-bx"></span>
                                                    </span>
                                                    <span class="text-grey-count">&nbsp;(<span class="count-box">300</span>)</span>
                                                </li>
                                                <li class="us-sprite" filterid="3" name="Blue">
                                                    <span class="filterText">Blue
                                            <span class="blue-bx"></span>
                                                    </span>
                                                    <span class="text-grey-count">&nbsp;(<span class="count-box">300</span>)</span>
                                                </li>
                                                <li class="us-sprite" filterid="7" name="Grey">
                                                    <span class="filterText">Grey
                                            <span class="grey-bx"></span>
                                                    </span>
                                                    <span class="text-grey-count">&nbsp;(<span class="count-box">300</span>)</span>
                                                </li>
                                                <li class="us-sprite" filterid="1" name="Beige">
                                                    <span class="filterText">Beige
                                            <span class="beige-bx"></span>
                                                    </span>
                                                    <span class="text-grey-count">&nbsp;(<span class="count-box">300</span>)</span>
                                                </li>
                                                <li class="us-sprite" filterid="4" name="Brown">
                                                    <span class="filterText">Brown
                                            <span class="brown-bx"></span>
                                                    </span>
                                                    <span class="text-grey-count">&nbsp;(<span class="count-box">300</span>)</span>
                                                </li>
                                                <li class="us-sprite" filterid="5" name="GoldNYellow">
                                                    <span class="filterText">Gold / Yellow
                                            <span class="gold-yellow-bx"></span>
                                                    </span>
                                                    <span class="text-grey-count">&nbsp;(<span class="count-box">300</span>)</span>
                                                </li>
                                                <li class="us-sprite" filterid="6" name="Green">
                                                    <span class="filterText">Green
                                            <span class="green-bx"></span>
                                                    </span>
                                                    <span class="text-grey-count">&nbsp;(<span class="count-box">300</span>)</span>
                                                </li>
                                                <li class="us-sprite" filterid="9" name="Purple">
                                                    <span class="filterText">Purple
                                            <span class="purple-bx"></span>
                                                    </span>
                                                    <span class="text-grey-count">&nbsp;(<span class="count-box">300</span>)</span>
                                                </li>
                                                <li class="us-sprite" filterid="8" name="Maroon">
                                                    <span class="filterText">Maroon
                                            <span class="maroon-bx"></span>
                                                    </span>
                                                    <span class="text-grey-count">&nbsp;(<span class="count-box">300</span>)</span>
                                                </li>
                                                <li class="us-sprite" filterid="13" name="Others">
                                                    <span class="filterText">Others</span>
                                                    <span class="text-grey-count">&nbsp;(<span class="count-box">300</span>)</span>
                                                </li>
                                            </ul>
                                        </div>
                                    </div>
                                    <div class="border-solid-top"></div>
                                    <!--Color End-->
                                    <!--Fuel Type code starts here -->
                                    <div class="filter-set content-inner-block-10" id="3" name="fuel" select="false">
                                        <h3 class="sub-values">Fuel Type
                                    <span class="fa fa-angle-down"></span>
                                        </h3>
                                        <div class="list-points padding-top10 hide">
                                            <ul>
                                                <li class="us-sprite" filterid="1" name="Petrol"><span class="filterText">Petrol</span>
                                                    <span class="text-grey-count">&nbsp;(<span class="count-box">300</span>)</span>
                                                </li>
                                                <li class="us-sprite" filterid="3" name="CNG"><span class="filterText">CNG</span>
                                                    <span class="text-grey-count">&nbsp;(<span class="count-box">300</span>)</span>
                                                </li>
                                                <li class="us-sprite" filterid="4" name="LPG"><span class="filterText">LPG</span>
                                                    <span class="text-grey-count">&nbsp;(<span class="count-box">300</span>)</span>
                                                </li>
                                                <li class="us-sprite" filterid="2" name="Diesel"><span class="filterText">Diesel</span>
                                                    <span class="text-grey-count">&nbsp;(<span class="count-box">300</span>)</span>
                                                </li>
                                                <li class="us-sprite" filterid="5" name="Electric"><span class="filterText">Electric</span>
                                                    <span class="text-grey-count">&nbsp;(<span class="count-box">300</span>)</span>
                                                </li>
                                                <li style="display: none;" class="us-sprite" filterid="6" name="Hybrid"><span class="filterText">Hybrid</span>
                                                    <span class="text-grey-count">&nbsp;(<span class="count-box">300</span>)</span>s
                                                </li>
                                            </ul>
                                        </div>
                                        <div class="clear"></div>
                                    </div>
                                    <div class="border-solid-top"></div>
                                    <!--Fuel Type code ends here -->
                                    <!--Model Year code starts here -->
                                    <div class="filter-set content-inner-block-10 " name="year">
                                        <h3 class="sub-values">Car Age
                                    <span class="fa fa-angle-down"></span>
                                        </h3>
                                        <div class="slider-box padding-top10 hide">
                                            <div class="leftfloat margin-left5">
                                                <span id="minYear" class="bold"></span>
                                            </div>
                                            <div class="rightfloat">
                                                <span id="maxYear" class="bold"></span>
                                            </div>
                                            <div class="clear"></div>
                                            <div id="model-range"></div>
                                        </div>
                                    </div>
                                    <div class="border-solid-top"></div>
                                    <!--Model Year code ends here -->
                                    <!--Kilometer code starts here -->
                                    <div class="filter-set content-inner-block-10" name="km">
                                        <h3 class="sub-values">Kilometer <span class="fa fa-angle-down"></span></h3>
                                        <div class="slider-box padding-top10 hide">
                                            <div class="leftfloat margin-left5">
                                                <span id="minKm" class="bold"></span>
                                            </div>
                                            <div class="rightfloat">
                                                <span id="maxKm" class="bold"></span>
                                            </div>
                                            <div class="clear"></div>
                                            <div id="kilometer-range"></div>
                                        </div>
                                    </div>
                                    <!--Kilometer code ends here -->
                                </div>
                                <div class="border-solid-top"></div>
                                <!--mainPart code ends here -->
                                <div class="clear"></div>
                                <!--Body Type code starts here -->
                                <div class="filter-set content-inner-block-10 bodyPart" id="4" name="bodytype" select="false">
                                    <h3 class="sub-values">Body Type
                                    <span class="fa fa-angle-down"></span>
                                    </h3>
                                    <div class="list-points padding-top10 hide">
                                        <ul>
                                            <li class="us-sprite" filterid="3" name="Hatchback"><span class="filterText">Hatchback</span>
                                                <span class="text-grey-count">&nbsp;(<span class="count-box">300</span>)</span>
                                            </li>
                                            <li class="us-sprite" filterid="2" name="Coupe"><span class="filterText">Coupe</span>
                                                <span class="text-grey-count">&nbsp;(<span class="count-box">300</span>)</span>
                                            </li>
                                            <li class="us-sprite" filterid="6" name="Suv"><span class="filterText">SUV/MUV</span>
                                                <span class="text-grey-count">&nbsp;(<span class="count-box">300</span>)</span>
                                            </li>
                                            <li class="us-sprite" filterid="4" name="Minivan"><span class="filterText">Minivan/Van</span>
                                                <span class="text-grey-count">&nbsp;(<span class="count-box">300</span>)</span>
                                            </li>
                                            <li class="us-sprite" filterid="7" name="Truck"><span class="filterText">Truck</span>
                                                <span class="text-grey-count">&nbsp;(<span class="count-box">300</span>)</span>
                                            </li>
                                            <li class="us-sprite" filterid="8" name="StationWagon"><span class="filterText">Station Wagon</span>
                                                <span class="text-grey-count">&nbsp;(<span class="count-box">300</span>)</span>
                                            </li>
                                            <li class="us-sprite" filterid="1" name="Sedan"><span class="filterText">Sedan</span>
                                                <span class="text-grey-count">&nbsp;(<span class="count-box">300</span>)</span>
                                            </li>
                                            <li class="us-sprite" filterid="5" name="Convertible"><span class="filterText">Convertible</span>
                                                <span class="text-grey-count">&nbsp;(<span class="count-box">300</span>)</span>
                                            </li>
                                        </ul>
                                    </div>
                                </div>
                                <div class="border-solid-top"></div>
                                <!--Body Type code ends here -->
                                <!--Owners code starts here -->
                                <div class="filter-set content-inner-block-10 " id="6" name="owners" select="false">
                                    <h3 class="sub-values">Owners
                                    <span class="fa fa-angle-down"></span>
                                    </h3>
                                    <div class="list-points padding-top10 hide">
                                        <ul>
                                            <li class="us-sprite" filterid="1" name="First"><span class="filterText">First Owner</span>
                                                <span class="text-grey-count">&nbsp;(<span class="count-box">300</span>)</span>
                                            </li>
                                            <li class="us-sprite" filterid="2" name="Second"><span class="filterText">Second Owner</span>
                                                <span class="text-grey-count">&nbsp;(<span class="count-box">300</span>)</span>
                                            </li>
                                            <li class="us-sprite" filterid="3" name="ThirdAndAbove"><span class="filterText">Third or More Owners</span>
                                                <span class="text-grey-count">&nbsp;(<span class="count-box">300</span>)</span>
                                            </li>
                                            <li class="us-sprite" filterid="6" name="Unregistered"><span class="filterText">Unregistered Car</span>
                                                <span class="text-grey-count">&nbsp;(<span class="count-box">300</span>)</span>
                                            </li>
                                        </ul>
                                    </div>
                                </div>
                                <div class="border-solid-top"></div>
                                <!--Owners code ends here -->
                                <!--Seller Type code starts here -->
                                <div class="filter-set content-inner-block-10 hide" id="5" name="seller" select="false">
                                    <h3 class="sub-values">Seller Type
                                    <span class="fa fa-angle-down"></span>
                                    </h3>
                                    <div class="list-points padding-top10 hide">
                                        <ul>
                                            <li class="us-sprite" filterid="2" name="Individual"><span class="filterText">Individual</span>
                                                <span class="text-grey-count">&nbsp;(<span class="count-box">300</span>)</span>
                                            </li>
                                            <li class="us-sprite" filterid="1" name="Dealer"><span class="filterText">Dealer</span>
                                                <span class="text-grey-count">&nbsp;(<span class="count-box">300</span>)</span>
                                            </li>
                                        </ul>
                                    </div>
                                </div>
                                <div class="border-solid-top"></div>
                                <!--Seller Type code ends here -->
                                <!--Transmission code starts here -->
                                <div class="filter-set content-inner-block-10" id="7" name="trans" select="false">
                                    <h3 class="sub-values">Transmission
                                    <span class="fa fa-angle-down"></span>
                                    </h3>
                                    <div class="list-points padding-top10 hide">
                                        <ul>
                                            <li class="us-sprite" filterid="2" name="Manual"><span class="filterText">Manual</span>
                                                <span class="text-grey-count">&nbsp;(<span class="count-box">300</span>)</span>
                                            </li>
                                            <li class="us-sprite" filterid="1" name="Automatic"><span class="filterText">Automatic</span>
                                                <span class="text-grey-count">&nbsp;(<span class="count-box">300</span>)</span>
                                            </li>
                                        </ul>
                                    </div>
                                </div>
                                <div class="border-solid-top"></div>
                                <!--Transmission code ends here -->
                                <!-- Feedback code starts here -->
                                <%--<div class="feedback-box hide">
                                <div class="clear"></div>
                                <div class="feedback-hdr" id="fback-click">
                                    <span class="cw-sprite cw-small-logo leftfloat"></span>
                                    <span class="feedbk-title">Help us improve</span>
                                    <span class="us-sprite close-icon-md padding-bottom10 rightfloat" id="btnFeedbackClose"></span>
                                    <div class="clear"></div>
                                </div>
                                <div id="fback-content">
                                    <textarea id="txtComments" textmode="MultiLine" rows="4" placeholder="What do you like about and what can we improve on?"></textarea>
                                    <p class="margin-bottom10 light-grey-text">How likely are you to suggest this page to your friends?</p>
                                    <div class="margin-bottom10">
                                        <div class="leftfloat dark-text">
                                            Not likely<br />
                                            0
                                        </div>
                                        <div class="rightfloat dark-text align-right">
                                            Very likely<br />
                                            10
                                        </div>
                                        <div class="clear"></div>
                                    </div>
                                    <div id="feedBackSlider"></div>
                                    <div class="align-center margin-top15 margin-bottom20">
                                        <input type="button" value="Submit" id="btnFeedback" class="red-btn" />
                                    </div>
                                    <img id="processInline" class="hide" src="https://imgd.aeplcdn.com/0x0/statics/loader.gif" border="0" alt="processing..." />
                                    <p><strong id="divFeedback"></strong></p>
                                </div>
                            </div>--%>
                                <!-- Feedback code ends here -->
                            </div>
                            <div class="clear"></div>
                        </div>
                        <div class="clear"></div>
                    </div>
                    <!-- Search filter code ends here -->
                </div>
                <div id="pageListing" class="grid-9 alpha">
                    <div class="content-box-shadow" id="dataDiv">
                        <!--Selected Filters and Reset Filter Section Start-->
                        <div class="border-solid-bottom">
                            <div class="content-inner-block-10 font13 grid-7 leftfloat">
                                <div id="selectedFilters">
                                    <span id="car" class="removeFilter"></span>
                                    <span id="filterbyadditional" class="removeFilter"></span>
                                    <span id="color" class="removeFilter"></span>
                                    <span id="fuel" class="removeFilter"></span>
                                    <span id="bodytype" class="removeFilter"></span>
                                    <span id="owners" class="removeFilter"></span>
                                    <span id="seller" class="removeFilter"></span>
                                    <span id="trans" class="removeFilter"></span>
                                </div>
                            </div>
                            <div class="grid-2 leftfloat text-right font16 text-light-grey margin-top5 margin-bottom5">
                             <%--<span class="fa fa-th-list listViewBtn text-link" title="List View"></span>
                            <span class="fa fa-th-large gridViewBtn" title="Grid View"></span>--%>
                            </div>
                            <div class="leftfloat padding-right10 grid-3 alpha position-rel">
                                <div class="coachmark sort-coachmark hide">
                                    <span class="coachmark-arrow-top"></span>
                                    <p class="font16 text-bold">Sort</p>
                                    <p class="inline-block font16">Use this to find cars with lowest<br />Price, KM, Year etc.</p>
                                    <p class="inline-block margin-left10"><a id="sortCoachmark" class="btn btn-green btn-green-sm">Next</a></p>
                                    <p><a class="nomoreTips cur-pointer">Don't show anymore tips</a></p>
                                </div>
                                <div class="form-control-box">
                                    <span class="select-box fa fa-angle-down"></span>
                                    <select id="sort" class="form-control input-xs margin-top5 margin-bottom5 sorting-list">
                                        <option value="5" sc="" so="" class="active">Best Match</option>
                                        <option value="1" sc="2" so="0">Price: Low to High<span class="us-sprite sort-icon"></span></option>
                                        <option value="0" sc="2" so="1">Price: High to Low<span class="us-sprite sort-icon"></span></option>
                                        <option value="4" sc="0" so="1">Year: Newest to Oldest</option>
                                        <option value="2" sc="3" so="0">KM: Low to High</option>
                                        <option value="3" sc="6" so="1">Recently Updated</option>
                                    </select>
                                </div>
                            </div>
                            <div class="clear"></div>
                        </div>
                        <!--Selected Filters and Reset Filter Section End-->
                        <div class="cw-tabs cw-tabs-flex nearByCity hideImportant">
                            <ul id="nearByCityList" class="hide">
                                <%--<li class="active" data-tabs="locationOne">Navi Mumbai (987)</li>
                            <li data-tabs="locationTwo">Thane (645)</li>
                            <li data-tabs="locationThree">Mumbai (687)</li>
                            <li data-tabs="locationFour">Mumbai All (1022)</li>--%>
                            </ul>
                        </div>

                        <!-- Listing Content Start-->
                        <div>
                            <div class="clear"></div>
                            <div id="cw-loading-box" class="position-rel">
                                <!-- Car Sold Out code starts here-->
                                <div class="soldout-box margin-top5 position-rel hide" runat="server">
                                    <a id="btnSoldOutClose" class="cur-pointer cwsprite cross-md-dark-grey position-abt pos-top10 pos-right10" title="Close"></a>
                                    <div class="soldout-content content-inner-block-10 text-center">
                                        <p class="font14">Oops... the car you are looking for has been sold out!</p>
                                    </div>
                                </div>
                                   <div id="noCarsFound" class="soldout-content position-rel hide content-inner-block-10 text-center">
                                        <a id="btnNoCarsClose" class="cur-pointer cwsprite cross-md-dark-grey position-abt pos-top5 pos-right5" title="Close"></a>
                                        <p class="font14 margin-bottom5 margin-top5">Oops! Cars are not available matching your search criteria in <strong id="noCarsFoundCityName"></strong>(<span class="font14 text-link changeCitylink">Click here</span> to change city)</p>
                                    </div>
                                <!-- Car Sold Out code ends here-->
                                <!-- cars in city code starts here -->
                                <div class="padding-left10 padding-right10 gridView">
                                 <div class="content-inner-block-10 border-solid-bottom position-rel carsinCity hide">
                                     <span id="cityWarningClose" class="cwsprite cross-md-lgt-grey position-abt pos-top10 pos-right10 cur-pointer"></span>
                                     <div class="leftfloat">
                                         <span class="cwsprite car-location-ic"></span>
                                     </div>
                                     <div class="leftfloat padding-left10">
                                         <h4 class="text-black margin-top5 margin-bottom5 cityWarningHeading">Cars in Delhi</h4>
                                         <!-- text for cars in City -->
                                         <p class="font14 warningCityText">You are viewing cars in <span class="warningCity">Delhi</span>. If it is not your city, <a href="javascript:void(0);" class="changeCityLink">click here</a> to change it.</p>
                                         <!-- text for cars in India -->
                                         <p class="font14 hide cityWarningTextAllIndia">You are viewing cars from all over India. To view cars from your city, <a href="javascript:void(0);" class="changeCityLink">click here.</a></p>
                                     </div>
                                     <div class="clear"></div>
                                 </div>
                                    <div class="stock-list search-list-container">
                                        <ul id="listingsData" class="hide">
                                            <asp:Repeater runat="server" ID="rptStockListings">
                                                <ItemTemplate>
                                                    <%# IsStockFranchise = Convert.ToInt32(DataBinder.Eval(Container.DataItem,"CwBasePackageId")) == (int)CwBasePackageId.Franchise%>
                                                    <%# IsOriginalImageAvailable = !string.IsNullOrEmpty(DataBinder.Eval(Container.DataItem,"OriginalImgPath").ToString()) && !string.IsNullOrEmpty(DataBinder.Eval(Container.DataItem,"HostUrl").ToString()) %>
                                                    <%# UrlWithSlotAndRank = $"{DataBinder.Eval(Container.DataItem,"Url")}?slotId={DataBinder.Eval(Container.DataItem,"SlotId")}&rk={Container.ItemIndex+1}" %>
                                                    <li isPremium=<%# Convert.ToBoolean(DataBinder.Eval(Container.DataItem, "IsPremium")).ToString().ToLower() %> 
                                                        rank=<%#GetRank(Convert.ToBoolean(DataBinder.Eval(Container.DataItem,"IsPremium"))) %> 
                                                        rankAbs=<%#GetRankAbsolute() %> 
                                                        class="listing-adv listingContent card-list-container <%# IsStockFranchise ? "card-list-container--franchisee" : string.Empty%> <%# Convert.ToInt32(DataBinder.Eval(Container.DataItem,"PhotoCount")) > 0 ? "cur-pointer" : ""%>" 
                                                        profileid="<%# DataBinder.Eval(Container.DataItem,"ProfileId") %>"
                                                        <%# DataBinder.Eval(Container.DataItem,"DealerRatingText") != null && !string.IsNullOrWhiteSpace(DataBinder.Eval(Container.DataItem,"DealerRatingText").ToString()) ? "isTopRatedSeller=true" : ""%>
                                                        <%# Convert.ToInt32(DataBinder.Eval(Container.DataItem,"CertificationId")) > 0 ? "isCertified=true" : ""%>
                                                        data-slot-id="<%# DataBinder.Eval(Container.DataItem, "SlotId")%>"
                                                        data-cte-package-id="<%# DataBinder.Eval(Container.DataItem, "CtePackageId")%>"
                                                        >
                                                        <div class="stock-detail">
                                                            <div class="image-container">
                                                                <div class="coachmark shortlist-coachmark hide">
                                                                    <span class="coachmark-arrow-top"></span>
                                                                    <p class="coachmark__heading">Shortlist</p>
                                                                    <p class="coachmark__subheading">Click on <span class="cw-used-sprite shortlistIcon-black"></span> to save the cars you like.</p>
                                                                    <p class="coachmark__btn"><a id="shortlistCoachmark" class="btn btn-green btn-green-sm">Next</a></p>
                                                                    <p><a class="nomoreTips coachmark__link">Don't show anymore tips</a></p>
                                                                 </div>
                                                                <div class="image__wrapper">
                                                                    <div class="image-wrapper__container">
                                                                        <div class="img-placer image-container__item">
                                                                            <span class="featured__tag <%# Convert.ToBoolean(DataBinder.Eval(Container.DataItem,"IsPremium")) == true ? "" : "hideImportant" %>">Featured</span>
                                                                                <div class="no-image-container <%# !IsOriginalImageAvailable ? string.Empty : "hide" %>">
                                                                                    <img class="no-image-container__image" src="https://imgd.aeplcdn.com/0x0/cw/static/used/no-car-images.svg" alt="No Cars" title="No Cars" />
                                                                                    <p class="no-image-container__text">No Image</p>
                                                                                </div>
                                                                                <a class="<%# Convert.ToInt32(DataBinder.Eval(Container.DataItem,"PhotoCount")) > 0 ? "slideShow car-item__image-link" : "" %>" profileid="<%# DataBinder.Eval(Container.DataItem,"ProfileId") %>" makeName="<%# DataBinder.Eval(Container.DataItem,"MakeName")%>" modelName="<%# DataBinder.Eval(Container.DataItem,"ModelName")%>" price="<%# DataBinder.Eval(Container.DataItem,"Price")%>" cityName="<%# DataBinder.Eval(Container.DataItem,"cityName")%>" seller="<%#DataBinder.Eval(Container.DataItem,"Seller")%>" data-role="click-tracking" data-cat ="UsedCarSearch" data-event="CWNonInteractive" data-action ="OnPhotoClick">
                                                                                    <img border="0" src="https://imgd.aeplcdn.com/0x0/statics/grey.gif" data-original='<%# ImageSizes.CreateImageUrl(DataBinder.Eval(Container.DataItem,"HostUrl").ToString(), ImageSizes._300x225, DataBinder.Eval(Container.DataItem,"OriginalImgPath").ToString())  %>' pg-navigation-images='<%# ImageSizes.CreateImageUrl(DataBinder.Eval(Container.DataItem,"HostUrl").ToString(), ImageSizes._174X98, DataBinder.Eval(Container.DataItem,"OriginalImgPath").ToString())%>'  class="lazy car-item__image <%# IsOriginalImageAvailable ? string.Empty : "hide" %>" profileid="<%# DataBinder.Eval(Container.DataItem,"ProfileId") %>"/>
                                                                                </a>
                                                                            <span class="<%#Convert.ToInt32(DataBinder.Eval(Container.DataItem,"PhotoCount")) > 0? "image-container__gallery-icon slideShow":"" %> "></span>
                                                                       
                                                                            <span class="shortlist-icon--inactive shortlist"></span>
                                                                        </div>
                                                                    </div>
                                                                </div>
                                                                <a class="certified__tag <%# DataBinder.Eval(Container.DataItem,"CertificationId") != null && Convert.ToInt32(DataBinder.Eval(Container.DataItem,"CertificationId")) == carTradeCertificationId ? string.Empty : "hide" %> " href="<%# UrlWithSlotAndRank %>&isP=<%# Convert.ToBoolean(DataBinder.Eval(Container.DataItem, "IsPremium")).ToString().ToLower()+ (Convert.ToInt32(DataBinder.Eval(Container.DataItem, "DeliveryCity"))!= 0?"&dc="+DataBinder.Eval(Container.DataItem, "DeliveryCity").ToString():"" )%>#certification" target="_blank" data-role="click-tracking" data-event="CWNonInteractive" data-action ="CT_Certification_Clicked" data-cat ="UsedCarSearch">
                                                                    <span class="certified-tag__img"></span>
                                                                    certified
                                                                </a>
                                                                <div class=" <%# ((certProgId = Convert.ToInt32(DataBinder.Eval(Container.DataItem,"CertProgId"))) != 0 || IsStockFranchise) ? "dealers-logo__container" : "hide" %>">
                                                                    <span class="dealers-logo__tag" data-event="CWInteractive"  data-cat="UsedCarSearch" data-action="Certification_Program_Click" data-label="<%# certProgId.ToString() %>"><img class="dealer-logo__img" src="<%# DataBinder.Eval(Container.DataItem,"CertProgLogoUrl")?.ToString() %>" style="max-height:34px; max-width:70px;"/>  </span>
                                                                </div>      
                                                                <div class="clear"></div>
                                                            </div>
                                                            <div class="card-detail-block">
                                                                <div class="table-div">
                                                                    <div class="card-detail-block__data">
                                                                        <a href="<%# UrlWithSlotAndRank %>&isP=<%# Convert.ToBoolean(DataBinder.Eval(Container.DataItem, "IsPremium")).ToString().ToLower() + (Convert.ToInt32(DataBinder.Eval(Container.DataItem, "DeliveryCity"))!= 0?"&dc="+DataBinder.Eval(Container.DataItem, "DeliveryCity").ToString():"")%>" target="_blank" profileid="<%# DataBinder.Eval(Container.DataItem,"ProfileId") %>" data-enum="DetailView" class="block">
                                                                        <table itemscope itemtype="http://schema.org/Cars" class="card-detail-block__data-table">
		                                                                    <tr>
			                                                                    <td>
				                                                                    <h2 itemprop="carName" class="card-detail-block__title" data-role="click-tracking" data-event="CWNonInteractive" data-action ="ListingClick_Title" data-cat ="UsedCarSearch"><a href="<%# UrlWithSlotAndRank %>&isP=<%# Convert.ToBoolean(DataBinder.Eval(Container.DataItem, "IsPremium")).ToString().ToLower() + (Convert.ToInt32(DataBinder.Eval(Container.DataItem, "DeliveryCity"))!= 0?"&dc="+DataBinder.Eval(Container.DataItem, "DeliveryCity").ToString():"") %>" title="<%# DataBinder.Eval(Container.DataItem,"CarName") %>" target="_blank" id="linkToDetails" profileid="<%# DataBinder.Eval(Container.DataItem,"ProfileId") %>" data-enum="DetailView"><span class="spancarname card-detail-block__title-text"><%# DataBinder.Eval(Container.DataItem,"CarName") %></span></a></h2>
			                                                                    </td>
		                                                                    </tr>
		                                                                    <tr>
			                                                                    <td>
				                                                                    <div class="gridViewPrice car-unit-data">
					                                                                    <a href="<%# UrlWithSlotAndRank %>&isP=<%# Convert.ToBoolean(DataBinder.Eval(Container.DataItem, "IsPremium")).ToString().ToLower()+ (Convert.ToInt32(DataBinder.Eval(Container.DataItem, "DeliveryCity"))!= 0?"&dc="+DataBinder.Eval(Container.DataItem, "DeliveryCity").ToString():"" )%>" target="_blank"  class="car-unit-data__amount" data-role="click-tracking" data-event="CWNonInteractive" data-action ="ListingClick" data-cat ="UsedCarSearch">
						                                                                    <span itemprop="price" class="rupee-lac slprice">₹ <%# DataBinder.Eval(Container.DataItem,"Price") %></span>
					                                                                    </a>
                                                                                        <a href="javascript:void(0);" data-href="<%# DataBinder.Eval(Container.DataItem, "ValuationUrl") %>" profileid="<%# DataBinder.Eval(Container.DataItem,"ProfileId") %>" class="view-market-price emi-unit-data__link">Check Right Price</a>
                                                                                  
				                                                                    </div>
			                                                                    </td>
		                                                                    </tr>
		                                                                    <tr class="otherDetails card-detail__vehicle-information"> 
				                                                                <td>
                                                                                    <table class="card-detail__vehicle-data">
                                                                                        <tr data-role="click-tracking" data-event="CWNonInteractive" data-action ="ListingClick" data-cat ="UsedCarSearch">
                                                                                            <td><span itemprop="mileage" class="slkms vehicle-data__item"><%# DataBinder.Eval(Container.DataItem,"Km") %>&nbsp;km</span></td>
						                                                                    <td><span itemprop="fuelType" title="<%# DataBinder.Eval(Container.DataItem,"Fuel") + (!string.IsNullOrWhiteSpace(Eval("AdditionalFuel").ToString()) ? " + 1" : string.Empty ) %>" class="slfuel vehicle-data__item">
                                                                                                <%# DataBinder.Eval(Container.DataItem,"Fuel") + (!string.IsNullOrWhiteSpace(Eval("AdditionalFuel").ToString()) ? " + 1" : string.Empty )%>
                                                                                                </span>
                                                                                            </td>
						                                                                    <td><span class="slYear vehicle-data__item"><%# DataBinder.Eval(Container.DataItem,"MakeYear") %></span></td>
						                                                                </tr>
                                                                                    </table>
                                                                                </td>
		                                                                    </tr>
                                                                            <tr>
                                                                                <td class="card-detail-block__info">
                                                                                    <p data-role="click-tracking" data-event="CWNonInteractive" data-action ="ListingClick" data-cat ="UsedCarSearch">
						                                                                <span class="city" itemscope itemtype="https://schema.org/city">
                                                                                            <span itemprop="areaName" class="<%# string.IsNullOrEmpty(DataBinder.Eval(Container.DataItem,"AreaName").ToString()) ? "hideImportant" : "city-info__area-name" %>"><%# DataBinder.Eval(Container.DataItem,"AreaName") %>,</span>
						                                                                    <span itemprop="cityName" class="cityName"> <%# DataBinder.Eval(Container.DataItem,"CityName") %></span>
					                                                                    </span>
                                                                                    </p>
                                                                                </td>
                                                                            </tr>
	                                                                    </table>
                                                                        </a>
                                                                    </div>
                                                                    <div class="clear"></div>
                                                                    <div class="listViewScreen1 seller-btn-detail">
                                                                        <div class="coachmark get-details-coachmark hide">
                                                                            <span class="coachmark-arrow-top"></span>
                                                                            <p class="font16 text-bold">Get Seller Details</p>
                                                                            <p class="inline-block font16">Get address and mobile no. of the seller</p>
                                                                            <p class="inline-block margin-left10"><a id="detailsCoachmark" class="btn btn-green btn-green-sm">Got it</a></p>
                                                                        </div>
                                                                        <div class="position-rel btn-container">
                                                                       
                                                                            <span class="inline-block grid-12 alpha omega preVerification"><a class="grid-12 redirect-rt btn btn-orange btn-xs gridBtn contact-seller seller-btn-container" id="pg-more-details-<%# DataBinder.Eval(Container.DataItem,"ProfileId") %>" 
                                                                                 profileid="<%# DataBinder.Eval(Container.DataItem,"ProfileId") %>" 
                                                                                 makeName="<%# DataBinder.Eval(Container.DataItem,"MakeName")%>" 
                                                                                 modelName="<%# DataBinder.Eval(Container.DataItem,"ModelName")%>" 
                                                                                 price="<%# DataBinder.Eval(Container.DataItem,"Price")%>"
                                                                                 cityName="<%# DataBinder.Eval(Container.DataItem,"cityName")%>"
                                                                                 seller="<%#DataBinder.Eval(Container.DataItem,"Seller")%>" 
                                                                                 cityId="<%#DataBinder.Eval(Container.DataItem,"CityId")%>" 
                                                                                 makeId="<%#DataBinder.Eval(Container.DataItem,"MakeId")%>" 
                                                                                 rootId="<%#DataBinder.Eval(Container.DataItem,"RootId")%>" 
                                                                                 bodyStyleId="<%#DataBinder.Eval(Container.DataItem,"BodyStyleId")%>" 
                                                                                 versionsubsegmentID="<%#DataBinder.Eval(Container.DataItem,"VersionSubSegmentID")%>" 
                                                                                 priceNumeric="<%#DataBinder.Eval(Container.DataItem,"PriceNumeric") %>"
                                                                                 kmNumeric="<%#DataBinder.Eval(Container.DataItem,"KmNumeric") %>"
                                                                                 modelid="<%#DataBinder.Eval(Container.DataItem,"ModelId") %>"
                                                                                 makemonth="<%#DataBinder.Eval(Container.DataItem,"MakeMonth") %>"
                                                                                 owner="<%#DataBinder.Eval(Container.DataItem,"OwnerTypeId") %>"
                                                                                 makeyear="<%#DataBinder.Eval(Container.DataItem,"MakeYear") %>"
                                                                                 data-slot-id="<%#DataBinder.Eval(Container.DataItem,"SlotId") %>"
                                                                                 data-cte-package-id="<%#DataBinder.Eval(Container.DataItem,"CtePackageId") %>"
                                                                            <%#string.IsNullOrEmpty(DataBinder.Eval(Container.DataItem,"StockRecommendationsUrl")?.ToString()) ? "" : "stockRecommendationsUrl="+ DataBinder.Eval(Container.DataItem, "StockRecommendationsUrl") %>
                                                                            <%#string.IsNullOrEmpty(DataBinder.Eval(Container.DataItem,"DealerCarsUrl")?.ToString()) ? "" : "dealerCarsUrl="+ DataBinder.Eval(Container.DataItem, "DealerCarsUrl") %>
                                                                            data-enum="BtnSellerView">
                                                                                <span class="gsdTxt">Get Seller Details</span>
                                                                                <span class="oneClickDetails hide font18">1-Click <span class="font15">View Details</span>
                                                                                </span></a></span>
                                                                        </div>
                                                                        <div class="clear"></div>
                                                                    </div>
                                                                </div>



                                                                <div class="clear"></div>

                                                                <div class="available-pics leftfloat">
                                                                    <a class="<%# Convert.ToInt32(DataBinder.Eval(Container.DataItem,"PhotoCount")) > 0 ? "photoIcon" : "hideImportant" %> leftfloat">
                                                                        <span class="cw-used-sprite cw-cam-icon leftfloat"></span>
                                                                        <span class="margin-left5 leftfloat"><%# DataBinder.Eval(Container.DataItem,"PhotoCount") %></span>
                                                                        <div class="clear"></div>
                                                                    </a>
                                                                    <a class="<%# Convert.ToInt32(DataBinder.Eval(Container.DataItem,"VideoCount")) > 0 && Convert.ToBoolean(DataBinder.Eval(Container.DataItem, "IsPremiumPackage"))? "videoIcon" : "hideImportant" %> leftfloat">
                                                                        <span class="cw-used-sprite cw-video-icon leftfloat"></span>
                                                                        <span class="margin-left5 leftfloat"><%# DataBinder.Eval(Container.DataItem,"VideoCount") %></span>
                                                                        <div class="clear"></div>
                                                                    </a>
                                                                    <div class="clear"></div>
                                                                </div>
                                                             <div class="toggleCarousel car-choices car-detail-block__similar-car"  data-role="click-tracking" data-event="CWNonInteractive" <%# !string.IsNullOrEmpty(DataBinder.Eval(Container.DataItem,"DealerCarsUrl")?.ToString()) ? "data-action ='MoreCars_Click'" : "data-action ='SimilarCars_Click'" %> data-label="<%# DataBinder.Eval(Container.DataItem,"ProfileId")%>" data-cat ="UsedCarSearch" ><%# !string.IsNullOrEmpty(DataBinder.Eval(Container.DataItem,"DealerCarsUrl")?.ToString()) ? "More Cars of this Dealer" : "Similar Cars" %></div>                                                            
                                                                 <p class="delivery-text <%# !String.IsNullOrEmpty(DataBinder.Eval(Container.DataItem, "DeliveryText").ToString()) ? "" : "hide" %>"><%# !String.IsNullOrEmpty(DataBinder.Eval(Container.DataItem, "DeliveryText").ToString())?DataBinder.Eval(Container.DataItem, "DeliveryText").ToString() : ""%></p>
                                                                <div class="clear"></div>
                                                            </div>
                                                        </div>
                                                    </li>
                                                </ItemTemplate>
                                            </asp:Repeater>
                                        </ul>
                                        <div class="clear"></div>
                                    </div>
                                    <!------------------------------------------------------------------------------------------------------------------------------------>
                                    <!-- Knockout for the listing data -->
                                    <script type="text/html" id="listingTemp">
                                        <li data-bind='attr: {deliverycity : DeliveryCity(),rank : $.getRank(IsPremium), isPremium : IsPremium(),rankAbs : $.getRankAbsolute(), class: PhotoCount() == 0 ? "listing-adv listingContent card-list-container" : "listing-adv listingContent cur-pointer card-list-container" ,profileid:ProfileId, isTopRatedSeller:DealerRatingText() ? true : null, isCertified: CertificationId() ? true : null , "data-slot-id": SlotId(), "data-cte-package-id": CtePackageId() }, css: {"card-list-container--franchisee" : CwBasePackageId() == <%=(int)CwBasePackageId.Franchise%>}'>
                                            <div class="stock-detail">
                                                <div class="image-container">
                                                    <div class="coachmark shortlist-coachmark hide">
                                                        <span class="coachmark-arrow-top"></span>
                                                        <p class="coachmark__heading">Shortlist</p>
                                                        <p class="coachmark__subheading">Click on <span class="cw-used-sprite shortlistIcon-black"></span> to save the cars you like.</p>
                                                        <p class="coachmark__btn"><a id="shortlistCoachmark" class="btn btn-green btn-green-sm">Next</a></p>
                                                        <p><a class="nomoreTips coachmark__link">Don't show anymore tips</a></p>
                                                    </div>
                                                    <div class="image__wrapper">
                                                        <div class="image-wrapper__container">
                                                        <div class="img-placer image-container__item">
                                                            <!-- ko if: IsPremium() -->
                                                            <span class="featured__tag">featured</span>
                                                            <!-- /ko -->
                                                            <!-- ko if: PhotoCount() > 0 -->
                                                            <a class="slideShow car-item__image-link" data-bind="attr: { profileId: ProfileId, cityName: CityName, cityId:CityId, makeName: MakeName, modelName: ModelName,price: Price,seller:Seller}" data-role="click-tracking" data-cat ="UsedCarSearch" data-event="CWNonInteractive" data-action ="OnPhotoClick">
                                                            <!-- ko if: OriginalImgPath() && HostUrl()  -->
                                                               <img class="lazy car-item__image" src="https://imgd.aeplcdn.com/0x0/statics/grey.gif" data-bind="attr: {profileId: ProfileId, alt: CarName, title: CarName, 'data-original': '<%= Carwale.Utility.CWConfiguration._imgHostUrl %>' + '300X225' + OriginalImgPath(), 'pg-navigation-images': '<%= Carwale.Utility.CWConfiguration._imgHostUrl %>' + '174X98' + OriginalImgPath()  }" />
                                                               <%--<img class="lazy car-item__image" src="https://imgd.aeplcdn.com/0x0/statics/grey.gif" data-bind="attr: {profileId: ProfileId, alt: CarName, title: CarName, 'data-original': OriginalImgPath() == '' || OriginalImgPath() == null || HostUrl() == '' || HostUrl() == null ? '<%= Carwale.Utility.CWConfiguration._imgHostUrl %>0x0/cw/design15/nocars-placeholder2.jpg' : '<%= Carwale.Utility.CWConfiguration._imgHostUrl %>' + '300X225' + OriginalImgPath(),'pg-navigation-images': OriginalImgPath() == '' || OriginalImgPath() == null || HostUrl() == '' || HostUrl() == null ? '<%= Carwale.Utility.CWConfiguration._imgHostUrl %>0x0/cw/design15/nocars-placeholder2.jpg' :'<%= Carwale.Utility.CWConfiguration._imgHostUrl %>' + '174X98' + OriginalImgPath()  }" />--%>
                                                            <!-- /ko -->
                                                            </a>
                                                            <!-- /ko -->
                                                            <!-- ko if: PhotoCount() == 0 -->
                                                                <div class="no-image-container">
                                                                    <img class="no-image-container__image" src="https://imgd.aeplcdn.com/0x0/cw/static/used/no-car-images.svg" alt="No Cars" title="No Cars" />
                                                                    <p class="no-image-container__text">No Image</p>
                                                                </div>
                                                            <!-- /ko -->
                                                            <span data-bind="attr:{class: (PhotoCount() > 0 ? 'image-container__gallery-icon slideShow':'') }"></span>
                                                            <span class="shortlist-icon--inactive shortlist"></span>
                                                        </div>
                                                    </div>
                                                </div>
                                                <!-- ko if: CertificationId() > 0 && CertificationId() == carTradeCertificationId && CertificationScore() != null -->
                                                <a data-bind="attr: { href: $.addDetailsQSParams(Url(), $element, SlotId()) + '#certification', profileId: ProfileId }" target="_blank" data-role="click-tracking" data-event="CWNonInteractive" data-action ="CT_Certification_Clicked" data-cat ="UsedCarSearch" class="certified__tag">
                                                    <span class="certified-tag__img"></span>
                                                    Certified
                                                </a>
                                                <!-- /ko -->
                                                <!-- ko if: CertProgLogoUrl() != null && CertProgLogoUrl() != '' -->
                                                <div class="dealers-logo__container">
                                                    <span  data-bind="attr: { class: 'dealers-logo__tag', 'data-label': CertProgId() }" data-event="CWInteractive"  data-cat="UsedCarSearch" data-action="Certification_Program_Click"> <img class="dealer-logo__img" data-bind="attr: { src: CertProgLogoUrl() }" style="max-height:34px; max-width:70px;"/> </span>
                                                </div>
                                                <!-- /ko -->
                                            </div>
                                            <div class="clear"></div>
                                                <div class="card-detail-block">
                                                    <div class="table-div">
                                                        <div class="card-detail-block__data">
                                                            <a data-bind="attr: { href: $.addDetailsQSParams(Url(), $element, SlotId()), profileId: ProfileId }" target="_blank">
                                                                <table itemscope itemtype="http://schema.org/Cars" class="grid-12 omega alpha card-detail-block__data-table">
		                                                            <tr>
			                                                            <td>
				                                                            <h2 itemprop="carName" class="card-detail-block__title" data-role="click-tracking" data-event="CWNonInteractive" data-action ="ListingClick_Title" data-cat ="UsedCarSearch"><a data-bind="attr: { href: $.addDetailsQSParams(Url(), $element, SlotId()), title: CarName, profileId: ProfileId }" target="_blank" id="linkToDetails"><span class="spancarname card-detail-block__title-text" data-bind="    text: CarName"></span></a></h2>
			                                                            </td>
		                                                            </tr>
		                                                            <tr>
			                                                            <td>
				                                                            <div class="gridViewPrice car-unit-data">
					                                                            <a data-bind="attr: { href: $.addDetailsQSParams(Url(), $element, SlotId()), profileId: ProfileId }" target="_blank"  class="car-unit-data__amount" data-role="click-tracking" data-event="CWNonInteractive" data-action ="ListingClick" data-cat ="UsedCarSearch">
                                                                                    <span itemprop="price" class="rupee-lac slprice">₹ <span data-bind="text: Price"></span></span>
                                                                                </a>
                                                                                <a href="javascript:void(0);" data-bind="attr: {'data-href': ValuationUrl, profileid: ProfileId}" class="view-market-price emi-unit-data__link">Check Right Price</a>
                                                                               
                                                                        
				                                                            </div>
			                                                            </td>
		                                                            </tr>
		                                                            <tr class="otherDetails card-detail__vehicle-information">
			                                                            <td>
                                                                            <table class="card-detail__vehicle-data">
                                                                                <tr data-role="click-tracking" data-event="CWNonInteractive" data-action ="ListingClick" data-cat ="UsedCarSearch">
					                                                                <td data-bind="attr: {title: Km() + 'km'}">
                                                                                        <span itemprop="mileage" class="slkms vehicle-data__item" data-bind="    text: Km() + ' km'"></span>
					                                                                </td>
					                                                                <td  class="slfuel">
						                                                                <!-- ko if: !AdditionalFuel() -->
						                                                                <span itemprop="fuelType" data-bind="attr: { title: Fuel() }"><span class="fuel vehicle-data__item" data-bind="    text: Fuel"></span></span>
						                                                                <!-- /ko -->
						                                                                <!-- ko if: AdditionalFuel() -->
						                                                                <span itemprop="additionalFuelType" data-bind="attr: { title: Fuel() + ' + 1' }">
							                                                                <span class="fuel vehicle-data__item" data-bind="text: Fuel() + ' + 1'"></span>
						                                                                </span>
						                                                                <!-- /ko -->
					                                                                </td>
                                                                                    <td itemprop="manufacturedYear" class="slYear vehicle-data__item" data-bind="text: MakeYear"></td>
                                                                                </tr>
                                                                            </table>
			                                                            </td>
		                                                            </tr>
                                                                    <tr>
                                                                        <td class="card-detail-block__info">
                                                                            <p data-role="click-tracking" data-event="CWNonInteractive" data-action ="ListingClick" data-cat ="UsedCarSearch">
                                                                                <span class="city" itemscope="" itemtype="https://schema.org/city">
                                                                                    <span class="city-info__area-name" itemprop="areaName" data-bind=" text: AreaName() == '' ? '' : AreaName() + ', '"></span>
                                                                                    <span itemprop="cityName" class="cityName" data-bind="    text: CityName()"></span>
                                                                                </span>
                                                                            </p>
                                                                        </td>
                                                                    </tr>
	                                                            </table>
                                                            </a>
                                                            <div class="clear"></div>
                                                        </div>
                                                        
                                                        <div class="listViewScreen1 seller-btn-detail">
                                                            <div class="coachmark get-details-coachmark hide">
                                                                <span class="coachmark-arrow-top"></span>
                                                                <p class="font16 text-bold">Get Seller Details</p>
                                                                <p class="inline-block font16">Get address and mobile no. of the seller</p>
                                                                <p class="inline-block margin-left10"><a id="detailsCoachmark" class="btn btn-green btn-green-sm">Got it</a></p>
                                                            </div>
                                                            <div class="position-rel dealer-rating-container btn-container">
                                                                 <span class="inline-block preVerification grid-12 alpha omega">                                                                    
                                                                     <a class="contact-seller gridBtn redirect-rt btn btn-orange btn-xs seller-btn-container grid-12" id="pg-more-details" data-bind=" attr: { profileId: ProfileId, makeName: MakeName, modelName: ModelName, price: Price, cityName: CityName, cityId: CityId, makeId: MakeId, rootId: RootId, seller: Seller, dc: (DeliveryCity() > '0') ? DeliveryCity() : null, bodyStyleId: BodyStyleId, versionsubsegmentID: VersionSubSegmentID, priceNumeric: PriceNumeric, kmNumeric: KmNumeric, modelid: ModelId, owner: OwnerTypeId, makemonth: MakeMonth, makeYear: MakeYear, stockRecommendationsUrl: StockRecommendationsUrl() ? StockRecommendationsUrl : null, dealerCarsUrl: DealerCarsUrl() ? DealerCarsUrl : null, 'data-slot-id': SlotId, 'data-cte-package-id': CtePackageId}">
                                                                         <span class="gsdTxt">Get Seller Details</span>
                                                                         <span class="oneClickDetails hide font18">1-Click <span class="font15">View Details</span></span>
                                                                     </a>
                                                                 </span>                                                            
                                                            </div>
                                                            <div class="clear"></div>
                                                        </div>
                                                    </div>
                                                    <div class="clear"></div>
                                                    <!-- ko ifnot: DealerCarsUrl() != null && DealerCarsUrl !='' -->
                                                    <div class="toggleCarousel car-choices car-detail-block__similar-car"  data-role="click-tracking" data-event="CWNonInteractive" data-action ="SimilarCars_Click" data-cat ="UsedCarSearch" data-bind="text: 'Similar Cars', attr: {'data-label': ProfileId}" ></div>
                                                    <!-- /ko -->
                                                    <!-- ko if: DealerCarsUrl() != null && DealerCarsUrl !='' -->
                                                    <div class="toggleCarousel car-choices car-detail-block__similar-car"  data-role="click-tracking" data-event="CWNonInteractive" data-action ="MoreCars_Click" data-cat ="UsedCarSearch" data-bind="text: 'More Cars of this Dealer', attr: {'data-label': ProfileId}" ></div>
                                                    <!-- /ko -->
                                                    <!-- ko if: DeliveryText() != null && DeliveryText() != '' -->
                                                        <p class="delivery-text" data-bind="text:DeliveryText()"></p>
                                                    <!-- /ko -->
                                                    <div class="clear"></div>
                                                </div>
                                            </div>
                                        </li>
                                    
                                    </script>
                                    <div class="stock-list search-list-container">
                                        <ul id="listing<%= HttpUtility.JavaScriptStringEncode(filters.pn) %>" class="ko-listing" data-bind="template: { name: 'listingTemp', foreach: listing }">
                                        </ul>
                                        <div class="clear"></div>
                                        <!-- Page base data display code starts here -->
                                        <div class="pg-out-box hide">
                                            <p>Page 2</p>
                                        </div>
                                        <!-- Page base data display code ends here -->
                                    </div>
                                    <!-- Knockout for the listing data ends here -->
                                    <!------------------------------------------------------------------------------------------------------------------------------------>
                                    <div class="getNextPage"></div>
                                    <!-- Loading box code starts here-->
                                    <div class="blackOut-window-bt hide"></div>
                                    <div id="newLoading" class="hide">
                                        <div class="loader-big"></div>
                                        <div  class="loading-popup">
                                            <span class="loading-icon"></span>
                                            <div class="clear"></div>
                                        </div>
                                    </div>
                                    <!-- Loading box code ends here-->
                                </div>
                            </div>
                            <!--New photo gallery added by kirtan-->                           
                            <div id="photoGallery" class="uc-gallery-container hide">
                                 <span id="pg-btn-close" style="top:5px; right:5px; z-index:9;" class="uc-gallery-sprite close-ic-xlg cur-pointer pos-top10 pos-right10 popup-close-esc-key"></span>
                                <!-- photogallery prev next btn starts here -->
                                <div id="prevBtnArrow" class="prevBtn  pos-left0">
                                    <a class="text-center" href="javascript:void(0);" id="pgPrevBtn"><span>Prev Car</span></a>
                                </div>
                                <div  id="nextBtnArrow" class="nextBtn pos-right0">
                                    <a class="text-center" href="javascript:void(0);" id="pgNextBtn"><span>Next Car</span></a>
                                </div>
                                <!-- available cars on next prev hover -->
                                <div class="availNext available-cars hide" data-role="click-tracking"  data-cat ="UsedCarSearch" data-event="CWNonInteractive" data-action ="PhotoGallery_Next">
                                    <div  class="carThumb">
                                        <img  src="https://img.aeplcdn.com/used/no-cars.jpg">
                                    </div>
                                    <p  class="pgNextTitle font12 text-default">No Cars Available</p>
                                </div>
                                <p id="pgNextCityCars"  class="text-link"></p>
                                <div  class="availPrev available-cars hide" data-role="click-tracking"  data-cat ="UsedCarSearch" data-event="CWNonInteractive" data-action ="PhotoGallery_Prev">
                                    <div  class="carThumb">
                                        <img src="https://img.aeplcdn.com/used/no-cars.jpg">
                                    </div>
                                    <p  class="pgPrevTitle font12 text-default">No Cars Available</p>
                                </div>
                                <div>
                                    <div class="margin-top10 pg-header-details font16">
                                        <h3 id="pg-car-name" class="leftfloat margin-right10 padding-right10 border-right-white font16 text-unbold">Toyota Corolla Altis G Diesel</h3>
                                        <div class="leftfloat">₹ <strong id="pg-car-price" class="text-unbold">1,50,35,000</strong></div>
                                        <div class="certificationDetails rightfloat">
                                            <div class="pg-delivery">
                                                <div class="inline-block carlocationTxt break-word">
                                                    <p class="margin-bottom5">Car location: <span class="pg-carcityname"></span></p>
                                                    <p class="pg-deliverytext">Not your city? <a href="javascript: void(0);" class="pg_changeCityLink">Search cars in your city</a></p>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="clear"></div>
                                    </div>
                                </div>
                                <div class="gallery-tabs-container">
                                    <div class="gallery-tabs-top">
                                        <div class="leftfloat gallery-tabs bold" id="videoTab">Videos <span>|</span> </div>
                                        <div class="leftfloat gallery-tabs" id="photoTab">Photos</div>
                                        <div class="clear"></div>
                                    </div>
                                </div>
                                <!-- Ask to seller Gallery code starts here -->
                                <div class="ask-gallery margin-bottom10">
                                    <div id="videoContainer">
                                        <iframe width="891" height="501" src="" frameborder="0" allowfullscreen="allowfullscreen"></iframe>
                                    </div>
                                    <div id="gallery" class="pg-gallery margin-top10 " style="visibility: hidden;">
                                        <div id="galleryHolder">
                                            <div id="descriptions">
                                                <div class="pg-controls hideImportant"></div>
                                            </div>
                                             <span class="cur-pointer cw-used-sprite shortlistIcon position-abt pos-top10 pos-right20 shortlistphotogallery shortListBtnZindex"></span>
                                            <div class="pg-image-wrapper">
                                                
                                                <div class="pg-image"></div>
                                                <img class="pg-loader" src="https://img.aeplcdn.com/adgallery/loader.gif" style="display: none;">
                                                <div class="pg-next">
                                                    <div class="pg-next-image"></div>
                                                </div>
                                                <div class="pg-prev">
                                                    <div class="pg-prev-image"></div>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="clear"></div>
                                        <div id="galleryList" class="pg-nav">
                                        </div>
                                        <div class="clear"></div>
                                        <div id="pg-car-features" class="uc-pg-points hide">
                                            <ul>
                                            </ul>
                                            <div class="clear"></div>
                                        </div>
                                        <div class="rightfloat more-detail-link">
                                            <a id="pg-more-details1" href="#" target="_blank" data-role="click-tracking" data-event="CWNonInteractive" data-action ="PhotoGallery_ViewMoreDetailsClick" data-cat ="UsedCarSearch" class="text-white padding-bottom5 border-solid-bottom" data-enum="DetailView">View More Details</a>
                                        </div>
                                        <div class="clear"></div>
                                    </div>
                                    <div class="clear"></div>
                                </div>
                                <!-- Ask to seller Gallery code ends here -->
                                <!-- Ask to seller form code starts here -->
                                <div id="pg_seller_details" class="ask-form-container padding-top15">
                                    <div id="pg-loadingImg" class="process-inline hide">&nbsp;&nbsp;&nbsp;&nbsp;Loading...</div>
                                    <div id="pg_contactSellerForm" class="step-1">
                                        <!-- step-1 code starts here -->
                                        <div id="pg-maskingNo-container">
                                            <p class="font14 margin-bottom10 margin-top5">Contact the Seller </p>
                                            <div class="uc-pg-call-box">
                                                <span class="fa fa-phone font20 margin-right10 leftfloat"></span>
                                                <strong id="pg-mskNo" class="leftfloat">880 011 3275</strong>
                                                <div class="clear"></div>
                                            </div>
                                            <div class="uc-pg-contact-name-box hide" title=""></div>
                                            <div class="clear"></div>
                                            <div class="align-center">
                                                <div class="or-text">
                                                    <div>
                                                        <span>OR</span>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="dotted-h"></div>
                                        </div>
                                        <p class="font14 margin-top10 margin-bottom10">Get Seller Details</p>
                                        <div class="uc-pg-fields position-rel">
											<input id="pg_txtName" type="text" name="Name" class="uc-pg-name-txt" placeholder="Enter Name" value="" maxlength="50"/>
											<span class="cw-used-sprite uc-uname"></span>
                                            <span id="pg_txtNameError" class="cw-blackbg-tooltip hide"></span>
                                            <span class="cwsprite error-icon hide"></span>
                                        </div>
                                        <div class="uc-pg-fields pos-relative mobile-ug-field">
                                            <span class="uc-pg-plus-num">+91</span>
											<input id="pg_txtMobile" class="uc-pg-mobile-txt" type="text" maxlength="10" name="mobile" placeholder="Enter 10 Digit Number" />
											<span class="cw-used-sprite uc-mobile"></span>
                                            <span id="pg_txtMobileError" class="cw-blackbg-tooltip hide"></span>
                                            <span class="cwsprite error-icon hide"></span>
                                        </div>
                                        <div class="uc-pg-fields email-ug-field">
                                            <input name="emailTick" type="checkbox" id="pg_emailTick">
                                            <label for="pg_emailTick">Get updates on Email</label>
                                        </div>
                                        <div id="pg_email_field" class="uc-pg-fields hide position-rel">
                                            <input id="pg_txtEmail" name="Email" type="text" placeholder="Email">
                                            <span id="pg_txtEmailError" class="cw-blackbg-tooltip hide"></span>
                                            <span class="cwsprite error-icon hide"></span>
                                        </div>
                                        <div style="margin: 20px 0;"  class="dealer-rating-container position-rel">
                                            <span class="top-rated-seller-tag">Top Rated Seller</span>
                                            <span class="dealer-rating-tooltip dealer-rating-tooltip--left">This seller has consistently been rated well by his customers</span>
                                        </div>
                                        <div class="leftfloat margin-top5 grid-12 alpha omega">
                                            <div id="pg_chat_btn_container" class="grid-4 chat-btn-container">
                                                <span class="btn-xs chat-btn">
                                                    <span class="js-threedot-loader">
                                                        <span class="chat-icon"></span>
                                                        <span class="chat-btn__text">Chat</span>
                                                    </span>
                                                    <% RazorPartialBridge.RenderPartial( "~/Views/Shared/_ThreeDotLoader.cshtml" ); %>
                                                </span>
                                            </div>
                                            <button type="button" id="pg_get_details" class="btn btn-xs btn-orange grid-8 seller-btn-container" value="Get Seller Details" data-enum="BtnSellerView">
                                                <span class="gsdTxt">Get Seller Details</span>
                                                <span class="oneClickDetails hide font18">1-Click <span class="font15">View Details</span></span>
                                            </button>										</div>
										<div class="clear"></div>
                                        <p style="text-align:left; font-size:11px; margin-top:20px">By submitting this form you agree to our <a href="/visitoragreement.aspx" target="_blank">terms and conditions</a></p>
                                    </div>
                                    <!-- step-1 code ends here -->
                                    <div id="pg-mobile-verification" class="step-2 hide">
                                        <!-- step-2 code starts here -->
                                        <a id="pg-numberChange" class="rightfloat cur-pointer">Back</a>
                                        <div class="clear"></div>
                                        <div class="margin-bottom5 margin-top15 font18 text-bold">Mobile Verification <span class="font12 light-grey-text unbold">one time</span></div>
                                        <div class="border-bottom"></div>
                                        <div id="mobile-verification">
                                            <p class="margin-bttom20 font14 margin-bottom15">Please enter the 5-digit verification code sent to you via SMS.</p>
                                            <!-- mobile-verification code starts here -->
                                            <p class="margin-bottom20 font14"></p>

                                            <%--<p class="error margin-bottom20 font14 hide" id="pg-txtCwiCodeError">Please enter 5-digit verification code to proceed</p>
                                            <p id="pg-txtValidCwiCodeError" class="error margin-bottom20 font14 hide">Invalid code! </p>--%>

                                            <div>
                                                <div class="uc-pg-fields leftfloat verify-code">
                                                    <div class="form-control-box">
                                                        <input type="text" tabindex="1" id="pg-txtCwiCode" maxlength="5" class="text form-control"  placeholder="Enter 5 digit code" />
                                                        <span id="pg-txtCwiCodeError" class="cw-blackbg-tooltip hide"></span>
                                                        <span class="cwsprite error-icon hide"></span>
                                                    </div>
                                                   <%-- <input id="pg-txtCwiCode" class="uc-pg-5digit-txt" type="text" placeholder="Enter 5 digit code" maxlength="5" />
                                                     <span id="pg-txtCwiCodeError" class="cw-blackbg-tooltip hide"></span>
                                                     <span class="cwsprite error-icon hide"></span>--%>

                                                </div>
                                                <div class="leftfloat margin-top5 margin-left5">
                                                    <div id="pg-process_img" class="process-inline hide" style="display: block;"></div>
                                                </div>
                                                <div class="clear"></div>
                                                <div id="pg-btnVerifyCode" class="leftfloat">
                                                    <a class="btn btn-orange rounded-corner5 text-uppercase">Verify</a>
                                                </div>
                                                <div class="clear"></div>
                                            </div>
                                            <div>
                                                <p class="leftfloat hide"><a>Send SMS</a> verification code again</p>
                                                <br />
                                            </div>
                                        </div>
                                        <!-- mobile-verification code ends here -->
                                        <div id="pg-zipcode-verification" class="margin-top15 hide">
                                            <!-- zipcode-verification code starts here -->
                                            <div class="margin-bottom20">
                                                <div id="pg-tollFreeBox" class="hide" style="display: block;">
                                                    <div id="pg-enableZipdial" class="margin-top20" style="">
                                                        <span class="font20 margin-right5"><span class="fa fa-phone"></span></span>
                                                        <span id="pg-zipDial" class="font20 text-bold"><span class="zipdial-txt"></span></span>
                                                        <%--<span><b>(Toll Free)</b></span>--%>
                                                        <div class="clear"></div>
                                                        <div id="pg-zipNote" class="uc-pgZipNote margin-top10">
                                                            <ul>
                                                                <li class="pg-tollFree">Give missed call on this <span class="text-bold">TOLL FREE</span> no.</li>
                                                                <li>Click "View Seller Details" after the call is disconnected.</li>
                                                            </ul>
                                                        </div>
                                                        <div id="pg-zipdial-verify" class="leftfloat margin-top10">
                                                            <input type="button" class="btn btn-orange text-uppercase" value="View Seller Details"></input>
                                                            <div class="position-rel dealer-rating-container inline-block">
                                                                <span class="top-rated-seller-tag">Top Rated Seller</span>
                                                                <span class="dealer-rating-tooltip dealer-rating-tooltip--left">This seller has consistently been rated well by his customers</span>
                                                            </div>
                                                        </div>
                                                        <div id="pg-zipdial-loading" class="process-inline hide"></div>
                                                        <div class="clear"></div>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                        <!-- zipcode-verification code ends here -->
                                        <div class="align-center">
                                            <div class="or-text">
                                                <div>
                                                    <span>
                                                        OR
                                                    </span>
                                                </div>
                                            </div>
                                            
                                        </div>
                                        <div class="dotted-h"></div>
                                        <div id="pg-toggleOption" class="optionToggle text-link">Click here to verify by missed call</div>
                                    </div>
                                    <!-- step-2 code ends here -->
                                    <div id="pg-seller-info" class="step-3 word-break hide">
                                        <!-- step-3 code starts here -->
                                        <h2 class="font20 margin-bottom10">Thank you!</h2>
                                        <p class="margin-bottom15">Your details have been shared with the seller</p>
                                        <p class="margin-bottom10"><strong>Contact Seller at:</strong></p>
                                        <p class="margin-bottom10"><strong>Name:</strong></p>
                                        <p>
                                            <span id="pg-contact_person" class="seller-details-right-name-field"></span>
                                            <span class="dealer-rating-container position-rel inline-block">
                                                <span class="top-rated-seller-tag">Top Rated Seller</span>
                                                <span class="dealer-rating-tooltip dealer-rating-tooltip--left">This seller has consistently been rated well by his customers</span>
                                             </span>
                                        </p>
                                        <p id="pg-seller_name"></p>
                                        <a href="#" class="font14 seller-virtual-link margin-top5">Check other cars from this seller</a>
                                        <p class="margin-bottom10 margin-top15"><strong>Email:</strong></p>
                                        <p class="margin-bottom15"><a id="pg-seller_email" href="mailto:Ganesh@brokerdalal.com, mailto:Ganesh@brokerdalal.com">Ganesh@brokerdalal.com</a></p>
                                        <p class="margin-bottom10"><strong>Address:</strong></p>
                                        <p id="pg-seller_address" class="margin-bottom10 min-line-height">
                                            Site No. D-267,Ttc Industrial Area,
                                            <br />
                                            Midc, Turbhe, Navi Mumbai.,
                                            <br />
                                            Maharashtra, 400705
                                        </p>
                                        <p class="margin-bottom10"><strong>Mobile:</strong></p>
                                        <p id="pg-seller_mobile" class="min-line-height">
                                            +91-9898765435,<br />
                                            +91-9876545890
                                        </p>
                                       <div class="margin-top15 margin-bottom10">
                                            <a class="font14 text-link pg-similarCars" target="_blank" onclick="dataLayer.push({ event: 'ViewSimilarCarsClick', cat: 'UsedCarSearch', act: 'ViewSimilarCarsClick_SearchPhoto'});">View similar cars >></a></div>
                                        </div>
                                    <!-- step-3 code ends here -->
                                    <!-- Buyer Process captcha starts here -->
                                    <div id="pg-captcha" class="captcha margin-top30 hide">
                                        <h3 class="margin-bottom20 text-black">Security check</h3>
                                        <iframe id="pg-captchaCode" src="" frameborder="0" scrolling="no" width="200" height="55"></iframe>
                                        <div>
	                                        <p class="margin-bottom10">Enter the code shown above</p>
                                            <div class="position-rel captcha-inputbox">
                                                <input id="pg-txtCaptchaCode" class="form-control" />
                                                <span id="pg-lblCaptcha" class="hide cw-blackbg-tooltip"></span>
                                                <span class="hide cwsprite error-icon"></span>
                                            </div>
                                            <p>(If you can't read it: <a class="text-link" onclick="javascript:regenerateCode(this,2)">Regenerate Code</a>)</p>
	                                        <input id="pg-btnVerifyCaptcha" type="button" class="pg-btnVerifyCaptcha btn btn-orange margin-top10" value="Verify" />
                                        </div>
                                    </div>
                                    <!-- Buyer Process captcha ends here -->
                                    <div id="pg-not_auth" class="alert hide" style="margin-top: 25px;">
                                        <div class="back-to-gsd-form">
                                            <span>Error message</span>
                                            <p style="color: #0288d1; cursor: pointer">Change your number</p>
                                        </div>
                                    </div>

                                     </div>
                            </div>
                            <!-- Ask to seller form code ends here -->
                            <div class="clear"></div>
                            </div>
                            <!--New photo gallery end -->
                            <div class="clear"></div>
                            <!-- Listing Content End-->
                            <p id="loadingTxt" class="font16 text-center margin-bottom10 margin-top10 hide">Loading...</p>
                            <!--No records found start-->
                            <div id="noRecordsFound" class="margin-top20 margin-bottom20 content-inner-block-20 hide">
                                <div id="res_msg" class="alert2 ucAlert border-solid rounded-corner5">
                                    <div class="content-inner-block-10 bg-light-grey content-inner-block-10 border-solid-bottom">
                                        <p class="margin-bottom5 font20">Oops! No Cars found matching your criteria.</p>
                                        <p class="font16">Try broadening your search criteria</p>
                                        <span class="us-sprite noResult-icon"></span>
                                    </div>
                                    <div class="content-inner-block-10">
                                        <div id="alert_content" class="<%= cityId == 0 ? "hide" : "" %>">
                                            <!-- class changed from hide to show on 19/3/2012 by Ashish G. Kamble -->
                                            <p class="font20">Set Email alert</p>
                                            <div><span id="span1" class="font16">Get updates when more car is available for sale</span></div>
                                            <div id="alert_crit" class="margin-bottom20">
                                                <div class="clear clear-margin"></div>

                                            </div>
                                            <div class="us-popup">
                                                <div class="leftfloat margin-right20">
                                                    <input type="text" id="alertEmail" class="set-width180 form-control" placeholder="Enter your email" />
                                                </div>
                                                <div class="leftfloat margin-left20">
                                                    <div class="form-control-box">
                                                        <span class="select-box fa fa-angle-down"></span>
                                                        <select id="selAlertFrq" class="form-control">
                                                            <option value="2">Daily</option>
                                                            <option value="3">Twice In A Week</option>
                                                        </select>
                                                    </div>
                                                    &nbsp;
                                                </div>
                                                <div class="leftfloat margin-left30">
                                                    <input id="setAlert" type="button" class="buttons btn btn-orange text-uppercase" value="Set Alert" onclick="setBuyerAlerts(this);" />
                                                    <span class="process-inline hide" style="display: none;"></span>
                                                </div>
                                                <div class="clear"></div>
                                            </div>
                                            <p class="font12 text-light-grey">We will not share your Email address with anyone</p>
                                        </div>
                                        <div id="alert_status"></div>
                                    </div>
                                </div>
                            </div>
                            <!--No records found end-->
                        </div>
                        <div class="clear"></div>
                        <div class="showMoreCars hideImportant text-center margin-top20 margin-bottom20">
                        <div class="margin-bottom15 hide">
                            <div class="process-inline"></div>
                            <div>Loading...</div>
                        </div>
                     <p class="text-center"><a class="btn btn-orange text-uppercase .more-cars-link hideImportant">Show More Cars</a></p>  
                    </div>
                    </div>
                    <div class="clear"></div>
                    <!-- Page base data display code starts here -->
                    
                </div>
                <div class="clear"></div>
                
            <div id="imageGallery" class="hide"></div>
            <!-- Loading box code starts here-->
            <div id="findCity" class="hide" style="z-index: 9999;"></div>
            <div id="findCityContent" class="hideImportant">
                <div class="city-popup" style="z-index: 99999;">
                    <div class="city-content">
                        <div class="city-content-close">
                            <span class="us-sprite close-icon-md"></span>
                        </div>

                        <h2>Before you get started....</h2>
                        <p class="margin-bottom15 margin-top5">Tell us your city to help us find the right cars for you.</p>
                        <div id="showcity">
                            <h3 class="margin-bottom10">Select City</h3>
                            <select runat="server" id="drpCity2" style="width: 205px;">
                                <option value="-1">--Select City--</option>
                            </select>
                            <div id="popularCities" class="margin-top10">
                                <ul>
                                    <li cityid="3000"><a>Mumbai</a></li>
                                    <li cityid="3001"><a>New Delhi</a></li>
                                    <li cityid="2"><a>Bangalore</a></li>
                                    <li cityid="176"><a>Chennai</a></li>
                                    <li cityid="198"><a>Kolkata</a></li>
                                    <li cityid="128"><a>Ahmedabad</a></li>
                                    <li cityid="12"><a>Pune</a></li>
                                    <li cityid="105"><a>Hyderabad</a></li>
                                    <li cityid="177"><a>Coimbatore</a></li>
                                </ul>
                                <div class="clear"></div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
            <!-- Goto top code start here -->
            <p id="back-top" class="hide cur-pointer"><a href="javascript:void(0);"><span></span></a></p>
            <!-- Goto top code end here -->
        </section>
        <!-- floating strip code starts here -->
        <div class="usedsearch-floating-strip float">
            <ul class="rightfloat inner-div">
                <li class="action-box shortlistBtn" data-role="click-tracking" data-cat ="UsedCarSearch" data-event="CWNonInteractive" data-action ="ShortList&CompareWindow_Click"><span class="cw-used-sprite shortlist-icon--active inline-block margin-right5"></span>& Compare<div class="short-list-tab-cnt-main">
                    <div class="short-list-tab-counter search-count">0</div>
                    <span class="cw-used-sprite tooltip-arrow"></span>
                </div>
                </li>
                <li class="action-box feedbackBtn"><span class="fa fa-edit inline-block margin-right5"></span>Feedback</li>
                <div class="clear"></div>
            </ul>
            <div class="clear"></div>
            <!-- feedback popup starts here -->
            <div class="feedback-form hide">
                <span class="loginCloseBtn cwsprite cross-md-dark-grey position-abt pos-top10 pos-right10 cur-pointer"></span>
                <p class="font20 text-bold">Help us Improve</p>
                <p class="margin-top10">
                    <textarea class="form-control" id="txtComments" placeholder="What do you like about us and what can we improve upon? "></textarea>
                </p>
                <p class="margin-top10 margin-bottom10 font14">How likely are you to suggest this page to your friends?</p>
                <div class="margin-bottom10">
                    <div class="leftfloat font14">
                        Not likely<br />
                        0
                    </div>
                    <div class="rightfloat text-right font14">
                        Very likely<br />
                        10
                    </div>
                    <div class="clear"></div>
                </div>
                <div id="feedBackSlider"></div>
                <p class="margin-top20">
                    <input class="btn btn-orange text-uppercase" type="button" id="btnFeedback" value="SUBMIT">
                    <img id="processInline" class="hide" src="https://imgd.aeplcdn.com/0x0/statics/loader.gif" border="0" alt="processing..." />
                    <p class="margin-top10"><strong id="divFeedback"></strong></p>
                </p>
            </div>
            <div class="shortlist-popup hide">
                <span class="cur-pointer position-abt cwsprite cross-lg-dark-grey pos-top15 pos-right15 shortlist-close"></span>
                <div class="wishlist-data">
                    <p class="font18 margin-bottom20">Click on <span class="shortlist-icon--active"> </span> to shortlist and compare</p>
                    <ul id="sortSlCar">
                        
                    </ul>
                    <div class="clear"></div>
                </div>
            </div>
            <div class="usedpopup-close"></div>
        </div> 
                   
        <!-- floating strip code starts end -->
        <span class="chat-btn global-chat-icon global-chat-btn chat-launcher-icon applozic-launcher mck-button-launcher hide"><span class="chat-text">My Chats</span>
            <span class="chat-icon"><span id="chat-count" class="chat-count"></span></span>
        </span> 
    <!-- Loading box code ends here-->
    <!-- New used search page code ends here-->
    <!-- #include file=/used/Buyerprocess.aspx -->
    <!-- #include file="/includes/footer.aspx" -->
    <!-- #include file="/includes/global/footer-script.aspx" -->

    <% RazorPartialBridge.RenderPartial( "~/Views/Used/ErrorPopup.cshtml" ); %>

    <%-- For Triggering Applozic Chat --%>
    <span id="chat-btn-search" class="applozic-launcher" data-mck-id data-mck-name></span>
      
    <link rel="stylesheet" href="/static/css/jquery.ad-gallery.css" type="text/css" >
    <link rel="stylesheet" href="/static/sass/m/css/loader.css" type="text/css">
    <link rel="stylesheet" href="/Static/sass/partials/chat-btn.css" type="text/css">
    <link rel="stylesheet" href="/static/css/cw-usedsearch.css" type="text/css" >
    <link rel="stylesheet" href="/static/css/uc-gallery-style.css" type="text/css" >
    <link rel="stylesheet" type="text/css" href="/static/m/css/chat-sidebox.css">
    <link rel="stylesheet" href="/Static/sass/used/card-list.css" type="text/css">
    <link rel="stylesheet" href="/Static/sass/used/desktop-card-list.css" type="text/css">

        <!-- Trovit Pixel Code -->
        <script type="text/javascript">
            (function(i,s,o,g,r,a,m){i['TrovitAnalyticsObject']=r;i[r]=i[r]||function(){
                (i[r].q=i[r].q||[]).push(arguments)},i[r].l=1*new Date();a=s.createElement(o),
                m=s.getElementsByTagName(o)[0];a.async=1;a.src=g;m.parentNode.insertBefore(a,m)
            })(window,document,'script','https://analytics.trovit.com/trovit-analytics.js','ta');

            ta('init', 'in', 2, '883b0730c9aff671d3164a84785f6825');          
        </script>
        <!-- End Trovit Pixel Code -->

    <script  type="text/javascript"  src="/static/src/jquery.obj.min.js" defer></script>
    <script  type="text/javascript"  src="/static/src/jquery.ad-gallery.js" defer></script>
    <script type="text/javascript" src="/static/Src/commonUtilities.js" defer></script>
    <script type="text/javascript" src="/static/js/used/cwUsedTracking.js" defer></script>

    <script  type="text/javascript"  src="/static/src/image-gallery.js" defer></script>
    <script  type="text/javascript"  src="/static/src/classified_search.js" ></script>
    <script  type="text/javascript" defer src="/static/js/classified_finance.js" ></script>
    <script  type="text/javascript" defer src="/static/js/promise-polyfill.min.js" ></script>
    <script  type="text/javascript" defer src="/static/src/classified-shortlistcars.js"></script>

    <script type="text/javascript" src="/static/js/used/chatProcess.js" defer></script>
    <script  type="text/javascript"  src="/Static/src/BuyerProcess.js" defer></script>
    <script type="text/javascript" src="/Static/js/chatApplozic/jquery.min.js" defer></script>
    <script type="text/javascript" src="/Static/js/chatApplozic/applozic.plugins.min.js" defer></script>
    <script type="text/javascript" src="/Static/js/chatApplozic/applozic.widget.min.js" defer></script>
    <script type="text/javascript" src="https://maps.google.com/maps/api/js?key=AIzaSyDKfWHzu9X7Z2hByeW4RRFJrD9SizOzZt4&libraries=places" defer></script>
    <script type="text/javascript" src="/Static/js/chatApplozic/locationpicker.jquery.min.js" defer></script>
    <script type="text/javascript" src="/Static/js/chatApplozic/applozic.chat.js" defer></script>
    <script type="text/javascript" src="/Static/js/chatApplozic/applozic.sidebox.js" defer></script>

    <script type="text/javascript">
        $.car = '<%= !string.IsNullOrEmpty(filters.car) ? HttpUtility.JavaScriptStringEncode(filters.car.Replace(" ","+")) : "" %>';
        $.city = '<%= !string.IsNullOrEmpty(filters.city) ? HttpUtility.JavaScriptStringEncode(filters.city.Replace(" ","+")) : "" %>';
        $.budget = '<%= !string.IsNullOrEmpty(filters.budget) ? HttpUtility.JavaScriptStringEncode(filters.budget.Replace(" ","+")) : "" %>';
        $.year = '<%= !string.IsNullOrEmpty(filters.year) ? HttpUtility.JavaScriptStringEncode(filters.year.Replace(" ","+")) : "" %>';
        $.kms = '<%= !string.IsNullOrEmpty(filters.kms) ? HttpUtility.JavaScriptStringEncode(filters.kms.Replace(" ","+")) : "" %>';
        $.trans = '<%= !string.IsNullOrEmpty(filters.trans) ? HttpUtility.JavaScriptStringEncode(filters.trans.Replace(" ","+")) : "" %>';
        $.seller = '<%= !string.IsNullOrEmpty(filters.seller) ? HttpUtility.JavaScriptStringEncode(filters.seller.Replace(" ","+").Replace(",","+")) : "" %>';
        $.owners = '<%= !string.IsNullOrEmpty(filters.owners) ? HttpUtility.JavaScriptStringEncode(filters.owners.Replace(" ","+")) : "" %>';
        $.bodytype = '<%= !string.IsNullOrEmpty(filters.bodytype) ? HttpUtility.JavaScriptStringEncode(filters.bodytype.Replace(" ","+")) : "" %>';
        $.fuel = '<%= !string.IsNullOrEmpty(filters.fuel) ? HttpUtility.JavaScriptStringEncode(filters.fuel.Replace(" ","+")) : "" %>';
        $.color = '<%= !string.IsNullOrEmpty(filters.color) ? HttpUtility.JavaScriptStringEncode(filters.color.Replace(" ","+")) : "" %>';
        $.filterbyadditional = '<%= !string.IsNullOrEmpty(filters.filterbyadditional) ? HttpUtility.JavaScriptStringEncode(filters.filterbyadditional.Replace(" ","+")) : "" %>';
        $.sc = '<%= HttpUtility.JavaScriptStringEncode(filters.sc) %>';
        $.so = '<%= HttpUtility.JavaScriptStringEncode(filters.so) %>';
        $.jsonData = eval(<%= jsonData %>);
        $.pageNo = <%= HttpUtility.JavaScriptStringEncode(filters.pn) %>;
        $.totalCount = <%= totalCount %>;
        $.appliedIpDetectedCityId = <%= appliedIpDetectedCityId %>;
        $.fetchMoreResults = false;
        $.pageSize = 20;
        $.soldOut = '<%= soldOut %>';
        $.ImageName = '';
        $.nearByCity = <%=nearByCity%>;
        $.listingsTrackingPlatform = '<%=ConfigurationManager.AppSettings["ListingsTrackingPlatform"].ToString()%>';
        $.indexFeatured = '<%=indexFeatured+1%>';
        $.indexNonFeatured ='<%=indexNonFeatured+1%>';
        $.indexAbsolute = '<%=indexAbsolute+1%>';
        $.showCityWarning = ('<%=showCityWarning%>').toLowerCase().trim() == 'true' ? true:false;
        $.fromCW = ('<%=fromCW%>').toLowerCase().trim() == 'true' ? true : false;
        D_usedSearch.latestNonPremiumCarRank = <%= latestNonPremiumRank%>;
        D_usedSearch.latestPremiumDealerCarRank = <%= latestPremiumDealerRank%>;
        D_usedSearch.latestPremiumIndividualCarRank = <%= latestPremiumIndividualRank%>;
        var listingsTrackingCategoryEnum = {
            Impression: 1,
            DetailView: 2,
            PhotoView : 3,
            BtnSellerView:4,
            Response:5,
            ResponsePg: 15,
            ResponseSearchRm: 16,
            ResponsePgRm: 17
        }
        var countIncrement = 0;
        var videoUrl = "";
        var isCityChange = true;
        var isLuxury = false;
        var load = true;
        var pcity = $.city;
        var loadPageNo = $.pageNo;
        var listingNo = loadPageNo;
        var ImageName = '';
        var MainCategory = '';
        var MakeId = '';
        var ModelId = '';
        var MakeName = '';
        var ModelName = '';
        var isAllIndia = false;    
        
        // shortlist box position on footer
        $(window).on('scroll', function() {
            scrollPosition = $(this).scrollTop();
            if (scrollPosition + $(window).height() >= $('.bg-footer').offset().top) {
                $('.usedsearch-floating-strip').addClass('nofloat');
            }
            else{
                $('.usedsearch-floating-strip').removeClass('nofloat');
            }
        });
        // Filter position fixed while scroll
        var $floatingStrip = $(".usedsearch-floating-strip");
        var opacity = $floatingStrip.css("opacity");
        var scrollStopped;
        var fadeInCallback = function () {
            if (typeof scrollStopped != 'undefined') {
                clearInterval(scrollStopped);
            }
            scrollStopped = setTimeout(function () {
                $floatingStrip.animate({ opacity: 1 }, "slow");
            }, 2000);
        };
        $(window).scroll(function () {
            if($('.shortlist-popup').is(':hidden') && $('.feedback-form').is(':hidden')){    
                if (!$floatingStrip.is(":animated") && opacity == 1) {
                    $floatingStrip.animate({ opacity: 0 }, "slow", fadeInCallback);
                } else {
                    fadeInCallback.call(this);
                }
            }
        });
        function regenerateCode(thisElement, captchaFlag){
            if(captchaFlag == 1){
                $(thisElement).closest('#captcha').find('#captchaCode').attr("src", "/Common/CaptchaImage/JpegImage.aspx");
                $(thisElement).closest('#captcha').find('#txtCaptchaCode').val("");
                $(thisElement).closest('#captcha').find('#lblCaptcha').hide();
                $(thisElement).closest('#captcha').find('#lblCaptcha').next().hide();
            }
            else if(captchaFlag == 2){
                $(thisElement).closest('#pg-captcha').find('#pg-captchaCode').attr("src", "/Common/CaptchaImage/JpegImage.aspx");
                $(thisElement).closest('#pg-captcha').find('#pg-txtCaptchaCode').val("");
                $(thisElement).closest('#pg-captcha').find('#pg-lblCaptcha').hide();
                $(thisElement).closest('#pg-captcha').find('#pg-lblCaptcha').next().hide();
            }
        }
        doNotShowAskTheExpert=false;
        var videoSlider = 1;
        $('#sortSlCar').sortable();
    </script>
</form>
    <div id="chatPopup" ></div>
</body>
</html>
