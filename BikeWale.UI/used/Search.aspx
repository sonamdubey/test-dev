<%@ Page Language="C#" AutoEventWireup="false"  Inherits="Bikewale.Used.Search" EnableViewState="false" %>
<%@ Register TagPrefix="BikeWale" TagName="Pager" Src="~/m/controls/LinkPagerControl.ascx" %>
<%@ Register Src="/controls/UsedBikeLeadCaptureControl.ascx" TagPrefix="BW" TagName="UBLeadCapturePopup" %>

<!DOCTYPE html>
<html>
<head>
    <%
        title = pageTitle;
        description = pageDescription;
        alternate = alternateUrl;
        keywords = pageKeywords;
        canonical = pageCanonical;
        relPrevPageUrl = prevUrl;
        relNextPageUrl = nextUrl;
        AdId = "1395992162974";
        AdPath = "/1017752/BikeWale_UsedBikes_Search_Results_";
        isAd300x250BTFShown = false;
        isAd300x250Shown=false;         

    %>
    <!-- #include file="/includes/headscript_desktop_min.aspx" -->
    <style type="text/css">
        @charset "utf-8";.pagination-control-next.inactive,.pagination-control-prev.inactive,.sort-label{pointer-events:none}.accordion-tab .category-label,.model-details-label,.text-truncate{text-overflow:ellipsis;white-space:nowrap}.padding-14-20{padding:14px 20px}.text-x-black{color:#1a1a1a}#listing-left-column{height:1230px;z-index:1}#listing-right-column{margin-left:323px;min-height:1250px}.sort-div,.sort-selection-div{background:#fff;border:1px solid #e2e2e2}.sort-div{width:220px;height:36px;padding:7px;position:relative;cursor:pointer;float:right}.sort-by-title{width:200px}.sort-select-btn.text-truncate{color:#82888b;width:90%}.sort-selection-div{width:220px;position:absolute;z-index:2;top:45px;right:0}.sort-list-items ul::after,.sort-list-items ul::before{border-left:10px solid transparent;border-right:10px solid transparent;content:"";left:50%;position:absolute;z-index:1}.sort-selection-div ul li{padding:5px 8px;transition:background .1s linear}.sort-label{color:#82888b}.sort-selection-div ul li:hover{cursor:pointer;background:#eee}.sort-selection-div ul li.selected{font-weight:700}.sort-list-items ul::before{border-bottom:10px solid #e2e2e2;top:-11px}.sort-list-items ul::after{border-bottom:10px solid #fff;top:-10px}#upDownArrow.fa-angle-down{transition:all .5s ease-in-out 0s;font-size:20px}.sort-div .fa-angle-down{transition:transform .3s;-moz-transition:transform .3s;-webkit-transition:transform .3s;-o-transition:transform .3s;-ms-transition:transform .3s}.sort-div.open .fa-angle-down{-moz-transform:rotateZ(180deg);-webkit-transform:rotateZ(180deg);-o-transform:rotateZ(180deg);-ms-transform:rotateZ(180deg);transform:rotateZ(180deg)}.sort-by-text p{height:35px;line-height:35px}#selected-filters li p{float:left;margin-right:15px;margin-bottom:8px;cursor:pointer}#used-bikes-list li{padding-top:20px;padding-bottom:20px;border-bottom:1px solid #e2e2e2}.model-thumbnail-image{width:300px;height:169px;display:table;text-align:center;background:#f5f5f5;float:left;position:relative;overflow:hidden}.model-thumbnail-image a{display:table-cell;vertical-align:middle;line-height:0;background:url(https://imgd2.aeplcdn.com/0x0/bw/static/sprites/d/loader.gif) center center no-repeat}.model-thumbnail-image img{max-width:300px;width:auto;height:169px}.model-details-content{width:318px;padding-left:20px;float:left;position:relative;top:-5px}.model-details-content .grid-6{margin-bottom:6px}.text-truncate{display:block}.model-details-label{width:84%;display:inline-block;vertical-align:middle;color:#82888b;text-align:left;overflow:hidden}.btn.seller-details-btn{font-size:14px;padding:5px 31px}#filters-head{padding:17px 20px}#filters-head .text-bold{position:relative;top:2px}#reset-filters.btn{padding:4px 21px}.filter-block{padding:17px 20px 20px;border-top:1px solid #f5f5f5;background:#fff}.filter-block:first-child{border-top:0}.filter-block .filter-label{color:#82888b;float:left}.filter-position-default{top:0!important}.position-fix{position:fixed}#filter-type-city{min-height:91px}#filter-type-city .chosen-container{padding:0 0 5px;border:0;border-bottom:1px solid #4d5057;border-radius:0}#filter-type-city .chosen-container-single .chosen-single{font-size:16px;font-weight:700;color:#4d5057;background:0 0}#filter-type-city .chosen-container-single .chosen-search input[type=text]{border:0;padding:6px 10px;font-size:16px}#filter-type-city .chosen-container-single .chosen-search{padding:0;border-bottom:1px solid #41b4c4}#filter-type-city .chosen-container .chosen-results{margin:0;padding:0;font-size:14px}#filter-type-city .chosen-container .chosen-drop{top:2px;border:1px solid #e2e2e2;box-shadow:0 2px 3px rgba(0,0,0,.15)}#filter-type-city .chosen-container-single .chosen-drop{border-radius:2px}#filter-type-city .chosen-container .chosen-results li{padding:8px 10px 7px;transition:background .1s linear}#filter-type-bike.filter-block{padding-right:5px}#filter-bike-list{width:100%;height:520px;overflow-y:scroll}#pagination-list,#previous-owners-list,.text-truncate{overflow:hidden}#filter-bike-list li{padding-right:12px}.accordion-tab{position:relative;border-top:1px solid #e2e2e2}#filter-bike-list li:first-child .accordion-tab{border-top:0}.accordion-tab .accordion-label-tab{width:90%;padding:16px 30px 16px 0;cursor:pointer}.accordion-tab~.bike-model-list-content{display:none}.accordion-tab .arrow-down{width:16px;height:10px;background-position:-205px -517px;position:absolute;right:0;top:22px;-moz-transition:transform .3s;-webkit-transition:transform .3s;-o-transition:transform .3s;-ms-transition:transform .3s;transition:transform .3s}.accordion-tab.tab-checked .accordion-checkbox .unchecked-box,.bike-model-list li.active .unchecked-box{background-position:-216px -135px}.accordion-tab .accordion-checkbox{position:relative;top:19px;cursor:pointer}.accordion-tab .category-label{font-size:16px;font-weight:700;max-width:70%;overflow:hidden;text-align:left;display:inline-block;vertical-align:middle}.accordion-tab .accordion-count{font-size:12px;color:#82888b;display:inline-block;vertical-align:middle;position:relative;top:2px}.bike-model-list li{font-size:14px;color:#82888b;padding:10px 0;cursor:pointer}.bike-model-list .unchecked-box{position:relative;top:2px;margin-right:6px}.bike-model-list li.active{font-weight:700;color:#4d5057}#filter-bike-list .form-control{border-radius:0}#filter-bike-list .search-icon{position:absolute;right:10px;top:12px;cursor:pointer;z-index:2}#filter-type-bike.active-clear #reset-bikes-filter{display:block}#reset-bikes-filter{display:none;color:#a2a2a2;cursor:pointer;position:relative;top:2px}#reset-bikes-filter:hover{color:#4d5057}.accordion-tab.active .arrow-down{-moz-transform:rotateZ(180deg);-webkit-transform:rotateZ(180deg);-o-transform:rotateZ(180deg);-ms-transform:rotateZ(180deg);transform:rotateZ(180deg)}#filter-bike-list::-webkit-scrollbar,#filter-type-city .chosen-container .chosen-results::-webkit-scrollbar{width:5px}#filter-bike-list::-webkit-scrollbar-track,#filter-type-city .chosen-container .chosen-results::-webkit-scrollbar-track{background:#fff}#filter-bike-list::-webkit-scrollbar-thumb,#filter-type-city .chosen-container .chosen-results::-webkit-scrollbar-thumb{background:#ddd}#filter-bike-list::-webkit-scrollbar-thumb:hover,#filter-type-city .chosen-container .chosen-results::-webkit-scrollbar-thumb:hover{background:#bbb}#filter-sidebar .ui-widget-content{background:#bababa;height:3px;margin:18px 0 3px;width:96%;border-radius:0;cursor:pointer}.ui-slider .ui-slider-range{border:none;display:block;font-size:.7em;position:absolute;z-index:1;border-radius:0}.ui-widget-header{background:#ef3030;height:3px}.ui-slider{position:relative;text-align:left}.ui-slider .ui-slider-handle{background:url(https://imgd1.aeplcdn.com/0x0/bw/static/sprites/m/slider-handle.png) 3px 3px no-repeat;cursor:pointer;height:28px;width:28px;display:block;margin-left:-9px;position:absolute;outline:0;top:-13px;z-index:2}#previous-owners-list li{margin-left:10px;float:left}#previous-owners-list li:first-child{margin-left:0}#previous-owners-list span{display:block;font-size:16px;padding:8px 14px;border:1px solid #a2a2a2;position:relative;border-radius:2px;cursor:pointer}#previous-owners-list .last-item{padding-right:26px;padding-left:26px}#previous-owners-list span:before{content:" ";position:absolute;top:0;left:0;right:0;bottom:0;border:1px solid #fff}#previous-owners-list .active span,#previous-owners-list .active span:before{border:1px solid #4d5057}#previous-owners-list .active span{font-weight:700}#filter-type-seller.filter-block{padding-bottom:10px}#seller-type-list li{font-size:16px;color:#82888b;cursor:pointer}#seller-type-list li:first-child{margin-bottom:15px}#seller-type-list li.checked{font-weight:700;color:#4d5057}#seller-type-list li.checked .unchecked-box{background-position:-216px -135px}.model-media-details{position:absolute;right:10px;bottom:10px;font-size:12px}#loader-bg-window,#modal-window{bottom:0;right:0;position:fixed;top:0}.model-media-item{display:inline-block;vertical-align:middle;padding:4px 5px;color:#4d5057;background:rgba(255,255,255,.8);border-radius:2px}.model-media-item:hover{text-decoration:none}.model-media-count{position:relative;top:-1px}.gallery-photo-icon{width:16px;height:12px;background-position:-213px -207px}.inr-md-lg{width:12px;height:17px;background-position:-64px -515px;position:relative;top:1px}.author-grey-sm-icon,.kms-driven-icon,.model-date-icon,.model-loc-icon{width:10px;height:12px;margin-right:5px;vertical-align:middle}.model-date-icon{background-position:-65px -543px}.kms-driven-icon{background-position:-65px -563px}.model-loc-icon{background-position:-82px -543px}.cross-icon{width:8px;height:8px;background-position:-82px -565px;margin-left:6px}.unchecked-box{width:14px;height:14px;background-position:-216px -120px;margin-right:10px}.search-icon{width:17px;height:17px;background-position:-213px -160px}.next-page-icon,.prev-page-icon{width:8px;height:12px;position:relative;left:10px;top:5px}.prev-page-icon{background-position:-213px -187px}.next-page-icon{background-position:-222px -187px}.inactive .next-page-icon.inactive,.inactive .prev-page-icon{opacity:.2}#search-listing-footer{padding-top:12px;padding-bottom:12px}#search-listing-footer .grid-5{position:relative;top:4px}#pagination-list-content{min-width:185px;width:auto;max-width:355px;float:right}#pagination-list{margin-right:35px;margin-left:35px}#pagination-list li{float:left;margin-right:5px;margin-left:5px;color:#82888b;font-size:12px;padding:5px;border:1px solid #fff;display:block;min-width:30px;text-align:center}#pagination-list a{color:#82888b}#pagination-list a:hover{color:#4d5057;text-decoration:none}#pagination-list .active{color:#4d5057;font-weight:700;border:1px solid #a2a2a2;-webkit-border-radius:1px;-moz-border-radius:1px;-ms-border-radius:1px;border-radius:1px}.pagination-control-next,.pagination-control-prev{position:absolute;top:0;width:30px;height:29px}.pagination-control-prev{left:0}.pagination-control-next{right:0}#get-seller-details-popup.bwm-fullscreen-popup{padding:30px 60px 50px;display:none}#get-seller-details-popup.bw-popup{width:480px;top:33%}#mobile-verification-section,#seller-details-section,#update-mobile-content{display:none}.btn-fixed-width{padding-right:0;padding-left:0;width:205px}.size-small .icon-outer-container{width:92px;height:92px}.size-small .icon-inner-container{width:84px;height:84px;margin:3px auto}.otp-icon{width:29px;height:29px;background-position:-109px -177px}.edit-blue-icon{position:relative;top:13px;cursor:pointer}.margin-bottom35{margin-bottom:35px}.dealer-details-list li{margin-bottom:25px}.dealer-details-list .data-key{font-size:12px;color:#82888b}.dealer-details-list .data-value,.input-box input{font-size:16px;font-weight:700}.text-truncate{width:100%;text-align:left}input[type=text]:focus,input[type=number]:focus{outline:0;box-shadow:none}.input-box{height:60px;text-align:left}.input-box input{width:100%;display:block;padding:7px 0;border-bottom:1px solid #82888b;color:#4d5057}.input-box label,.input-number-prefix{font-size:16px;position:absolute;color:#82888b;top:4px}.input-box label{left:0;-webkit-transition:.2s ease all;-moz-transition:.2s ease all;-o-transition:.2s ease all;transition:.2s ease all}.input-number-box input{padding-left:30px}.input-number-prefix{display:none;font-weight:700}.boundary{position:relative;width:100%;display:block}.boundary:after,.boundary:before{content:'';position:absolute;bottom:0;width:0;height:2px;background-color:#41b4c4;-webkit-transition:.2s ease all;-moz-transition:.2s ease all;-o-transition:.2s ease all;transition:.2s ease all}.boundary:before{left:50%}.boundary:after{right:50%}.error-text{display:none;font-size:12px;position:relative;top:4px;left:0;color:#d9534f}.input-box.input-number-box input:focus~.input-number-prefix,.input-box.input-number-box.not-empty .input-number-prefix,.input-box.invalid .error-text{display:inline-block}.input-box input:focus~label,.input-box.not-empty label{top:-16px;font-size:12px}.input-box input:focus~.boundary:after,.input-box input:focus~.boundary:before{width:50%}.input-box.invalid .boundary:after,.input-box.invalid .boundary:before{background-color:#d9534f;width:50%}#ub-ajax-loader{display:none}#modal-window{background:#000;left:0;z-index:9;opacity:.5;display:none;-ms-filter:"progid:DXImageTransform.Microsoft.Alpha(Opacity=50)";filter:alpha(opacity=50);-moz-opacity:.5;-khtml-opacity:.5}#spinner-content{width:50px;height:50px;position:fixed;top:45%;left:4%;right:0;margin:0 auto;z-index:999}.bw-spinner{-webkit-animation:rotator 1.4s linear infinite;animation:rotator 1.4s linear infinite}@-webkit-keyframes rotator{0%{-webkit-transform:rotate(0);transform:rotate(0)}100%{-webkit-transform:rotate(270deg);transform:rotate(270deg)}}@keyframes rotator{0%{-webkit-transform:rotate(0);transform:rotate(0)}100%{-webkit-transform:rotate(270deg);transform:rotate(270deg)}}.circle-path{stroke-dasharray:187;stroke-dashoffset:0;-webkit-transform-origin:center;transform-origin:center;-webkit-animation:dash 1.4s ease-in-out infinite,colors 5.6s ease-in-out infinite;animation:dash 1.4s ease-in-out infinite,colors 5.6s ease-in-out infinite}@-webkit-keyframes colors{0%,100%{stroke:#ef3f30}}@keyframes colors{0%,100%{stroke:#ef3f30}}@-webkit-keyframes dash{0%{stroke-dashoffset:187}50%{stroke-dashoffset:46.75;-webkit-transform:rotate(135deg);transform:rotate(135deg)}100%{stroke-dashoffset:187;-webkit-transform:rotate(450deg);transform:rotate(450deg)}}@keyframes dash{0%{stroke-dashoffset:187}50%{stroke-dashoffset:46.75;-webkit-transform:rotate(135deg);transform:rotate(135deg)}100%{stroke-dashoffset:187;-webkit-transform:rotate(450deg);transform:rotate(450deg)}}.model-details-content .text-truncate{width:100%!important}#search-listing-content{top:2px;min-height:1250px}#loader-bg-window{background:#f5f5f5;left:0;z-index:5;opacity:0;display:none;-ms-filter:"progid:DXImageTransform.Microsoft.Alpha(Opacity=0)";filter:alpha(opacity=0);-moz-opacity:0;-khtml-opacity:0}#loader-right-column{position:absolute;left:0;top:0;background:#f5f5f5;width:100%;min-height:1250px;height:100%;z-index:2;opacity:.8;display:none;-ms-filter:"progid:DXImageTransform.Microsoft.Alpha(Opacity=80)";filter:alpha(opacity=80);-moz-opacity:.8;-khtml-opacity:.8}.loader-active #spinner-content{left:25%}#get-seller-details-popup{min-height:550px}@media only screen and (max-width:1024px){#listing-right-column{margin-left:313px}.model-details-content{width:293px}.model-details-label{width:82%}#previous-owners-list .last-item{padding-right:20px;padding-left:20px}}
    </style>
    <script type="text/javascript">
        <!-- #include file="\includes\gacode_desktop.aspx" -->
    </script>
</head>
<body class="bg-light-grey header-fixed-inner">
    <form id="form1" runat="server">
        <!-- #include file="/includes/headBW.aspx" -->

        <div id="usedBikesSection" >

        <section class="bg-light-grey padding-top10" id="breadcrumb">
            <div class="container">
                <div class="grid-12">
                    <div class="breadcrumb margin-bottom15">
                        <!-- breadcrumb code starts here -->
                        <ul>
                            <li itemscope="" itemtype="http://data-vocabulary.org/Breadcrumb"><a href="/" itemprop="url">
                                <span itemprop="title">Home</span></a>
                            </li>
                            <li itemtype="http://data-vocabulary.org/Breadcrumb"><span class="bwsprite fa-angle-right margin-right10"></span>
                              <a href="/used/" itemprop="url"><span>Used Bikes</span></a>
                            </li>
                            <% if (makeId > 0)
                               { %>
                            <li itemtype="http://data-vocabulary.org/Breadcrumb"><span class="bwsprite fa-angle-right margin-right10"></span>
                              <a href="/used/bikes-in-<%= objCity!=null ? objCity.CityMaskingName : "india" %>/" itemprop="url"><span><%= cityName %></span></a>
                            </li> 
                             <% if(objMake!=null && modelId > 0) { %>
                                   <li itemtype="http://data-vocabulary.org/Breadcrumb"><span class="bwsprite fa-angle-right margin-right10"></span>
                              <a href="/used/<%= objMake.MaskingName %>-bikes-in-<%= objCity!=null ? objCity.CityMaskingName : "india" %>/" itemprop="url"><span><%= string.Format("{0} Bikes",objMake.MakeName) %></span></a>
                            </li> 
                            <% } } %>
                            <% if (!string.IsNullOrEmpty(heading))
                               { %>
                            <li><span class="bwsprite fa-angle-right margin-right10"></span>
                              <span><%= heading %></span>
                            </li>
                            <%} %>
                        </ul>
                        <div class="clear"></div>
                    </div>
                </div>
                <div class="clear"></div>
            </div>
        </section>

        <section>
            <div class="container margin-bottom20">
                <div class="grid-12">
                    <div class="content-box-shadow">
                        <div class="content-box-shadow padding-14-20">
                            <div class="grid-9 alpha">
                                <h1 class="font24 text-x-black"><%= heading %></h1>
                            </div>
                            <div id="sort-by-content" class="grid-3 omega">
                                <div class="sort-div rounded-corner2">
                                    <div class="sort-by-title" id="sort-by-container">
                                        <span class="leftfloat sort-select-btn text-truncate">Sort by</span>
                                        <span class="clear"></span>
                                    </div>
                                    <span id="upDownArrow" class="rightfloat fa fa-angle-down position-abt pos-top15 pos-right10"></span>
                                </div>
                                <div class="sort-selection-div sort-list-items hide">
                                    <ul id="sort-listing">
                                        <li data-sortorder="1" data-bind="click : ApplySort">Most recent</li>
                                        <li data-sortorder="2" data-bind="click : ApplySort">Price - Low to High</li>
                                        <li data-sortorder="3" data-bind="click : ApplySort">Price - High to Low</li>
                                        <li data-sortorder="4" data-bind="click : ApplySort">Kms - Low to High</li>
                                        <li data-sortorder="5" data-bind="click : ApplySort">Kms - High to Low</li>
                                    </ul>
                                </div>
                            </div>
                            <div class="clear"></div>
                        </div>
                        <div id="search-listing-content" class="position-rel bg-white">
                            <div id="listing-right-column" class="grid-8 padding-right20 rightfloat">
                                <div id="loader-right-column"></div>
                                <div class="margin-top15 font12 padding-bottom5 border-solid-bottom" data-bind="visible: PreviousQS() != ''">
                                    <ul id="selected-filters">
                                        <li id="bike"></li>
                                        <li class="type-slider" data-id="budget-amount"></li>
                                        <li class="type-slider" data-id="kms-amount"></li>
                                        <li class="type-slider" data-id="bike-age-amount"></li>
                                        <li id="owners"></li>
                                        <li id="seller"></li>
                                    </ul>
                                    <div class="clear"></div>
                                </div>
                                <div id="listing-start-point"></div>
                                <div class="padding-top15 padding-bottom15 text-light-grey font14 border-solid-bottom" data-bind="visible: TotalBikes() > 0">
                                    <p style="display:none" data-bind="visible: !OnInit() && TotalBikes() > 0">Showing <span class="text-default text-bold"><span data-bind="    CurrencyText: (Pagination().pageNumber() - 1) * Pagination().pageSize() + 1"><%=_startIndex %></span>-<span data-bind="    CurrencyText: Math.min(TotalBikes(), Pagination().pageNumber() * Pagination().pageSize())""><%=_endIndex %></span></span> of <span class="text-default text-bold" data-bind="    CurrencyText: TotalBikes()"><%= Bikewale.Utility.Format.FormatPrice(totalListing.ToString()) %></span> bikes</p>
                                    <% if(totalListing > 0){ %>
                                        <p data-bind="visible: OnInit()">Showing <span class="text-default text-bold"><%=_startIndex %>-<%=_endIndex %></span> of <span class="text-default text-bold" data-bind="    CurrencyText: TotalBikes()"><%= Bikewale.Utility.Format.FormatPrice(totalListing.ToString()) %></span> bikes</p>
                                    <% } %>
                                </div>
                                <%--<div data-bind="controlsDescendantBindings: true">--%>
                                    <% if (usedBikesList!=null && totalListing > 0) { %>
                                    <ul id="used-bikes-list" data-bind="visible: OnInit() && !noBikes()">
                                        <% foreach(var bike in usedBikesList) { 
                                               string curBikeName = string.Format("{0} {1} {2}",bike.MakeName,bike.ModelName,bike.VersionName);
                                                   %>
                                        <li>

                                            <div class="model-thumbnail-image">
                                                <a href="<%= string.Format("/used/bikes-in-{0}/{1}-{2}-{3}/",bike.CityMaskingName,bike.MakeMaskingName,bike.ModelMaskingName,bike.ProfileId) %>" title="<%= curBikeName %>">
                                                    <img class="lazy" data-original="<%= Bikewale.Utility.Image.GetPathToShowImages(bike.Photo.OriginalImagePath,bike.Photo.HostUrl,Bikewale.Utility.ImageSize._310x174) %>" alt="<%= curBikeName %>" title="<%= curBikeName %>" src="" />
                                                    <% if(bike.TotalPhotos > 0) { %>
                                                    <div class="model-media-details">
                                                        <div class="model-media-item">
                                                            <span class="bwsprite gallery-photo-icon"></span>
                                                            <span class="model-media-count"><%= bike.TotalPhotos %></span>
                                                        </div>
                                                    </div>
                                                    <% } %>
                                                </a>
                                            </div>
                                            <div class="model-details-content font14">
                                                <h2><a href="<%= string.Format("/used/bikes-in-{0}/{1}-{2}-{3}/",bike.CityMaskingName,bike.MakeMaskingName,bike.ModelMaskingName,bike.ProfileId) %>" class="text-truncate text-black" title="<%= curBikeName %>"><%= curBikeName %></a></h2>
                                                <div class="margin-bottom5">
                                                    <span class="font12 text-xt-light-grey">Updated on: <%= bike.LastUpdated.ToString("dd MMM yy") %></span>
                                                </div>
                                                <%if(!string.IsNullOrEmpty(bike.ModelYear)) { %>
                                                 <div class="grid-6 alpha"> 
                                                    <span class="bwsprite model-date-icon"></span>
                                                    <span class="model-details-label" ><%= bike.ModelYear %> model</span>
                                                </div>
                                                <% } %>
                                                 <%if(bike.KmsDriven > 0) { %>
                                                <div class="grid-6 alpha omega" >
                                                    <span class="bwsprite kms-driven-icon"></span>
                                                    <span class="model-details-label"><%= Bikewale.Utility.Format.FormatPrice(bike.KmsDriven.ToString()) %> kms</span>
                                                </div>
                                                <% } %>
                                                 <%if(bike.NoOfOwners > 0) { %>
                                                <div class="grid-6 alpha">
                                                    <span class="bwsprite author-grey-sm-icon"></span>
                                                    <span class="model-details-label" ><%= Bikewale.Utility.Format.AddNumberOrdinal(bike.NoOfOwners) %></> Owner</span>
                                                </div>
                                                <% } %>
                                                 <%if(!string.IsNullOrEmpty(bike.CityName)) { %>
                                                <div class="grid-6 alpha omega">
                                                    <span class="bwsprite model-loc-icon"></span>
                                                    <span class="model-details-label"><%= bike.CityName %></span>
                                                </div>
                                                <% } %>
                                                <div class="clear"></div>
                                                <p class="margin-bottom10"><span class="bwsprite inr-md-lg"></span>&nbsp;<span class="font22 text-bold" ><%= Bikewale.Utility.Format.FormatPrice(bike.AskingPrice.ToString()) %></span></p>
                                                <a href="javascript:void(0)" class="btn btn-white seller-details-btn used-bike-lead" data-ga-cat="Used_Bike_Listing" data-ga-act="Get_Seller_Details_Clicked" data-profile-id="<%= bike.ProfileId  %>" data-ga-lab="<%= bike.ProfileId %>" rel="nofollow">Get seller details</a>
                                            </div>
                                            <div class="clear"></div>
                                        </li>
                                        <% } %>    
                                    </ul>
                                    <% } %>

                                      <ul id="used-bikes-list" style="display:none" data-bind="visible: !OnInit() && !noBikes() ,foreach : BikeDetails()">
                                        <li>

                                            <div class="model-thumbnail-image">
                                                <a href="" data-bind="attr: { 'href': '/used/bikes-in-' + cityMasking + '/' + makeMasking + '-' + modelMasking + '-' + profileId + '/' },title:bikeName" title="">
                                                    <!-- ko if : $index() < 3 -->
                                                    <img data-bind="attr: { alt: bikeName, title: bikeName, src: (photo.originalImagePath != '') ? (photo.hostUrl + '/370x208/' + photo.originalImagePath) : 'https://imgd3.aeplcdn.com/174x98/bikewaleimg/images/noimage.png'} " alt="" title="" border="0" />
                                                    <!-- /ko --><!-- ko if : $index() > 2 -->
                                                    <img data-bind="attr: { alt: bikeName, title: bikeName}, lazyload: ((photo.originalImagePath != '') ? (photo.hostUrl + '/370x208/' + photo.originalImagePath) : 'https://imgd3.aeplcdn.com/174x98/bikewaleimg/images/noimage.png'), " alt="" title="" border="0" />
                                                    <!-- /ko -->
                                                    <div class="model-media-details" data-bind="visible: totalPhotos > 0">
                                                        <div class="model-media-item">
                                                            <span class="bwsprite gallery-photo-icon"></span>
                                                            <span class="model-media-count" data-bind="text: totalPhotos"></span>
                                                        </div>
                                                    </div>

                                                </a>
                                            </div>
                                            <div class="model-details-content font14">
                                                <h2><a href="" data-bind="text: bikeName, attr: { 'href': '/used/bikes-in-' + cityMasking + '/' + makeMasking + '-' + modelMasking + '-' + profileId + '/' }" class="text-truncate text-black" ></a></h2>
                                                <div class="margin-bottom5">
                                                    <span class="font12 text-xt-light-grey" data-bind="text: 'Updated on: ' + strLastUpdated"></span>
                                                </div>
                                                 <div class="grid-6 alpha" data-bind="visible: modelYear > 0"> 
                                                    <span class="bwsprite model-date-icon"></span>
                                                    <span class="model-details-label" data-bind="text: modelYear + ' model'"></span>
                                                </div>
                                                <div class="grid-6 alpha omega" data-bind="visible: kmsDriven > 0">
                                                    <span class="bwsprite kms-driven-icon"></span>
                                                    <span class="model-details-label"><span data-bind="CurrencyText: kmsDriven"></span> kms</span>
                                                </div>

                                                <div class="grid-6 alpha" data-bind="visible: noOfOwners != null">
                                                    <span class="bwsprite author-grey-sm-icon"></span>
                                                    <span class="model-details-label" ><span data-bind="NumberOrdinal: noOfOwners"></span> Owner</span>
                                                </div>

                                                <div class="grid-6 alpha omega" data-bind="visible: city != ''">
                                                    <span class="bwsprite model-loc-icon"></span>
                                                    <span class="model-details-label" data-bind="text: city"></span>
                                                </div>

                                                <div class="clear"></div>
                                                <p class="margin-bottom10" data-bind="visible: askingPrice == 0" ><span class="bwsprite inr-md-lg"></span>&nbsp;<span class="font22 text-bold" >N/A</span></p>
                                                <p class="margin-bottom10" data-bind="visible: askingPrice > 0" ><span class="bwsprite inr-md-lg"></span>&nbsp;<span class="font22 text-bold" data-bind="CurrencyText: askingPrice"></span></p>
                                                <a href="javascript:void(0)" class="btn btn-white seller-details-btn used-bike-lead" rel="nofollow" data-ga-cat="Used_Bike_Listing" data-ga-act="Get_Seller_Details_Clicked" data-bind="attr: { 'data-ga-lab': profileId, 'data-profile-id': profileId }">Get seller details</a>
                                            </div>
                                            <div class="clear"></div>
                                        </li>  
                                    </ul>

                                    <div style="text-align: center;">
                                        <div id="nobike" style="display: none;"  data-bind="visible : noBikes()">
                                            <img src="/images/no_result_m.png">
                                        </div>
                                    </div>  
                               <%-- </div>--%>

                                <div id="search-listing-footer" class="font14">
                                    <div class="grid-5 alpha omega text-light-grey">
                                        <p style="display:none" data-bind="visible: !OnInit() && TotalBikes() > 0">Showing <span class="text-default text-bold"><span data-bind="    CurrencyText: (Pagination().pageNumber() - 1) * Pagination().pageSize() + 1"><%=_startIndex %></span>-<span data-bind="    CurrencyText: Math.min(TotalBikes(), Pagination().pageNumber() * Pagination().pageSize())""><%=_endIndex %></span></span> of <span class="text-default text-bold" data-bind="    CurrencyText: TotalBikes()"><%= Bikewale.Utility.Format.FormatPrice(totalListing.ToString()) %></span> bikes</p>
                                        <% if(totalListing > 0){ %>
                                            <p data-bind="visible: OnInit()">Showing <span class="text-default text-bold"><%=_startIndex %>-<%=_endIndex %></span> of <span class="text-default text-bold"><%= Bikewale.Utility.Format.FormatPrice(totalListing.ToString()) %></span> bikes</p>
                                        <% } %>
                                    </div>
                                   
                                    <div data-bind="visible: OnInit() && Pagination().totalPages() > 1">
                                        <BikeWale:Pager ID="ctrlPager" runat="server" />
                                    </div>
                                <div data-bind="visible: !OnInit() && Pagination().paginated() > 0 && Pagination().totalPages() > 1">
                                    <div id="pagination-list-content" class="grid-7 alpha omega position-rel">
                                        <ul id="pagination-list" data-bind="html: PagesListHtml"></ul>
                                        <span class="pagination-control-prev" data-bind="html: PrevPageHtml, css: Pagination().hasPrevious() ? 'active' : 'inactive' "></span>
                                        <span class="pagination-control-next" data-bind="html: NextPageHtml, css: Pagination().hasNext() ? 'active' : 'inactive'"></span>
                                    </div>
                                </div>

                                    <div class="clear"></div>
                                </div>
                            </div>
                            <div id="listing-left-column" class="grid-4 alpha font14 position-abt">
                                <div id="filter-sidebar" class="border-solid-right">
                                    <div id="filters-head">
                                        <p class="font18 text-bold text-x-black leftfloat">Filters</p>
                                        <p id="reset-filters" class="btn btn-white font14 rightfloat" data-bind="click : ResetFilters">Reset</p>
                                        <div class="clear"></div>
                                    </div>

                                    <div id="filter-type-city" class="filter-block">
                                        <p class="filter-label margin-bottom5">City</p>
                                        <div class="clear"></div>
                                        <select class="city-chosen-select hide" data-bind="chosen:{},event: { change: FilterCity }">
                                             <option data-cityid="0" >All India</option>
                                             <% if (citiesList != null)
                                                { %>
                                            <% foreach(var city in citiesList) { %>
                                                  <option data-cityid="<%= city.CityId %>" value="<%= city.CityId %>" ><%=city.CityName %></option>
                                            <% } %>
                                            <%} %>
                                        </select>
                                    </div>

                                    <div id="filter-type-bike" class="filter-block">
                                        <p class="filter-label">Bike</p>
                                        <p  class="font12 padding-right20 rightfloat cur-pointer" data-bind="click : ResetBikeFilters">Clear all</p>
                                        <div class="clear"></div>
                                        <ul id="filter-bike-list">
                                            <% if (makeModelsList != null)
                                                { %>
                                            <%foreach (var make in makeModelsList) {%>
                                            <li >
                                                <div id="mk-<%=make.Make.MakeId %>" class="accordion-tab">
                                                    <div class="accordion-checkbox leftfloat" data-bind="click : ApplyMakeFilter">
                                                        <span data-makeid="<%=make.Make.MakeId %>" class="bwsprite unchecked-box"></span>
                                                    </div>
                                                    <div class="accordion-label-tab leftfloat">
                                                        <span class="category-label"><%=make.Make.MakeName %></span>
                                                        <span class="accordion-count"></span>
                                                        <span class="bwsprite arrow-down"></span>
                                                    </div>
                                                    <div class="clear"></div>
                                                </div>
                                                <div class="bike-model-list-content">
                                                    <div class="form-control-box margin-bottom5">
                                                        <span class="bwsprite search-icon"></span>
                                                        <input type="text" class="getModelInput form-control padding-right40" placeholder="Type to search brand or model">
                                                    </div>
                                                    <ul class="bike-model-list">
                                                        <%foreach (var model in make.Models)  {%>
                                                            <li id="md-<%=model.ModelId %>" data-bind="click : ApplyModelFilter">
                                                                <span data-modelid="<%=model.ModelId %>" class="bwsprite unchecked-box"></span>
                                                                <span class="category-label"><%=model.ModelName %></span>
                                                            </li>
                                                       <% } %>                                                     
                                                </ul>
                                                </div>
                                            </li>
                                            <% } } %>
                                        </ul>
                                    </div>

                                    <div class="filter-block">
                                        <p class="filter-label">Budget</p>
                                        <p id="budget-amount" class="font14 text-bold rightfloat"></p>
                                        <div class="clear"></div>
                                        <div  data-bind="KOSlider: BudgetValues, sliderOptions: { range: true, values: [0, 7], min: 0, max: 7, step: 1 }"></div>
                                    </div>

                                    <div class="filter-block">
                                        <p class="filter-label">Kms ridden</p>
                                        <p id="kms-amount" class="font14 text-bold rightfloat" data-bind="visible: KmsDriven() > 0">0 - <span data-bind="    CurrencyText: KmsDriven()"></span><span data-bind="    text: KmsDriven() == 200000 ? '+ Kms' : ' Kms' "></span></p>
                                        <div class="clear"></div>
                                         <div  data-bind="KOSlider: KmsDriven, sliderOptions: { range: 'min', value: 80000, min: 5000, max: 200000, step: 5000 }"></div>
                                    </div>

                                    <div class="filter-block">
                                        <p class="filter-label">Bike age</p>
                                        <p id="bike-age-amount" class="font14 text-bold rightfloat" data-bind="visible: BikeAge() > 0" >0 - <span data-bind="    text: BikeAge()"></span><span data-bind="    text: BikeAge() == 8 ? '+ years' : ' years' "></span></p>
                                        <div class="clear"></div>
                                        <div data-bind="KOSlider: BikeAge, sliderOptions: { range: 'min', value: 8, min: 1, max: 8, step: 1 }"></div>
                                    </div>

                                    <div class="filter-block">
                                        <p class="filter-label margin-bottom10">Previous owners</p>
                                        <div class="clear"></div>
                                        <ul id="previous-owners-list" >
                                            <li data-ownerid="1" id="own-1" data-bind="click : FilterOwners"><span>1</span></li>
                                            <li data-ownerid="2" id="own-2" data-bind="click : FilterOwners"><span>2</span></li>
                                            <li data-ownerid="3" id="own-3" data-bind="click : FilterOwners"><span>3</span></li>
                                            <li data-ownerid="4" id="own-4" data-bind="click : FilterOwners"><span>4</span></li>
                                            <li data-ownerid="5" id="own-5" data-bind="click : FilterOwners"><span class="last-item">4+</span></li>
                                        </ul>
                                    </div>

                                    <div id="filter-type-seller" class="filter-block">
                                        <p class="filter-label margin-bottom15">Seller type</p>
                                        <div class="clear"></div>
                                        <ul id="seller-type-list" >
                                            <li  data-sellerid="2" data-bind="click : FilterSellers"><span class="bwsprite unchecked-box"></span><span class="category-label">Individual</span></li>
                                            <li  data-sellerid="1" data-bind="click : FilterSellers"><span class="bwsprite unchecked-box"></span><span class="category-label">Dealer</span></li>
                                        </ul>
                                    </div>
                                </div>
                            </div>
                            <div class="clear"></div>
                        </div>
                    </div>
                </div>
                <div class="clear"></div>
            </div>
        </section>
        
        </div>

        <div id="loader-bg-window"></div>

        <script type="text/javascript" src="<%= staticUrl != "" ? "https://st1.aeplcdn.com" + staticUrl : "" %>/src/frameworks.js?<%=staticFileVersion %>"></script>
        <BW:UBLeadCapturePopup runat="server" ID="ctrlUBLeadCapturePopup"></BW:UBLeadCapturePopup>
        <!-- #include file="/includes/footerBW.aspx" -->
        <link href="<%= staticUrl != "" ? "https://st2.aeplcdn.com" + staticUrl : "" %>/css/bw-common-btf.css?<%=staticFileVersion %>" rel="stylesheet" type="text/css" />
        <script type="text/javascript" src="<%= staticUrl != string.Empty ? "https://st2.aeplcdn.com" + staticUrl : string.Empty %>/src/common.min.js?<%= staticFileVersion %>"></script>
         <script type="text/javascript">
             var OnInitTotalBikes = <%= totalListing %>; 
             var pageQS = "<%= currentQueryString %>";
             var selectedCityId = <%= cityId %>;selectedMakeId = "<%= makeId %>",selectedModelId = "<%= modelId %>";
        </script>
        <script type="text/javascript" src="<%= staticUrl != string.Empty ? "https://st2.aeplcdn.com" + staticUrl : string.Empty %>/src/used-search.js?<%= staticFileVersion %>"></script>
        <link href='https://fonts.googleapis.com/css?family=Open+Sans:400,700' rel='stylesheet' type='text/css' />
        <!--[if lt IE 9]>
            <script src="/src/html5.js"></script>
        <![endif]-->

    </form>
</body>
</html>
