<%@ Page Language="C#" AutoEventWireup="false" CodeBehind="Search_v2.aspx.cs" Inherits="Bikewale.Used.Search_v2" %>

<!DOCTYPE html>
<html>
<head>
    <%
        title = "Used Search";
        isHeaderFix = false;
    %>
    <!-- #include file="/includes/headscript_desktop_min.aspx" -->
    <style type="text/css">
        @charset "utf-8";.accordion-tab .category-label,.model-details-label,.text-truncate{text-overflow:ellipsis;white-space:nowrap;overflow:hidden}.pagination-control-next.inactive,.pagination-control-prev.inactive,.sort-label{pointer-events:none}.padding-14-20{padding:14px 20px}.text-x-black{color:#1a1a1a}#listing-left-column{height:1230px;z-index:1}#listing-right-column{margin-left:323px}.sort-div,.sort-selection-div{background:#fff;border:1px solid #e2e2e2}.sort-div{width:220px;height:36px;padding:7px;position:relative;cursor:pointer;float:right}.sort-by-title{width:200px}.sort-select-btn.text-truncate{color:#82888b;width:90%}.sort-selection-div{width:220px;position:absolute;z-index:2;top:45px;right:0}.sort-list-items ul::after,.sort-list-items ul::before{border-left:10px solid transparent;border-right:10px solid transparent;content:"";left:50%;position:absolute;z-index:1}.sort-selection-div ul li{padding:5px 8px;transition:background .1s linear}.sort-label{color:#82888b}.sort-selection-div ul li:hover{cursor:pointer;background:#eee}.sort-selection-div ul li.selected{font-weight:700}.sort-list-items ul::before{border-bottom:10px solid #e2e2e2;top:-11px}.sort-list-items ul::after{border-bottom:10px solid #fff;top:-10px}#upDownArrow.fa-angle-down{transition:all .5s ease-in-out 0s;font-size:20px}.sort-div .fa-angle-down{transition:transform .3s;-moz-transition:transform .3s;-webkit-transition:transform .3s;-o-transition:transform .3s;-ms-transition:transform .3s}.sort-div.open .fa-angle-down{-moz-transform:rotateZ(180deg);-webkit-transform:rotateZ(180deg);-o-transform:rotateZ(180deg);-ms-transform:rotateZ(180deg);transform:rotateZ(180deg)}.sort-by-text p{height:35px;line-height:35px}#selected-filters li p{float:left;margin-right:15px;margin-bottom:8px;cursor:pointer}#used-bikes-list li{padding-top:20px;padding-bottom:20px;border-bottom:1px solid #e2e2e2}.model-thumbnail-image{width:300px;height:169px;display:table;text-align:center;background:#f5f5f5;float:left;position:relative;overflow:hidden}.model-thumbnail-image a{display:table-cell;vertical-align:middle;line-height:0;background:url(http://imgd2.aeplcdn.com/0x0/bw/static/sprites/d/loader.gif) center center no-repeat}.model-thumbnail-image img{max-width:300px;width:auto;height:169px}.model-details-content{width:318px;padding-left:20px;float:left}.model-details-content .grid-6{margin-bottom:6px}.text-truncate{display:block;text-align:left}.model-details-content .grid-6.omega{padding-left:30px}.model-details-label{width:84%;display:inline-block;vertical-align:middle;color:#82888b;text-align:left}.btn.seller-details-btn{font-size:14px;padding:5px 31px}#filters-head{padding:17px 20px}#filters-head .text-bold{position:relative;top:2px}#reset-filters.btn{padding:4px 21px}.filter-block{padding:17px 20px 20px;border-top:1px solid #f5f5f5;background:#fff}.filter-block:first-child{border-top:0}.filter-block .filter-label{color:#82888b;float:left}.filter-position-default{top:0!important}.position-fix{position:fixed}#filter-type-city{min-height:91px}#filter-type-city .chosen-container{padding:0 0 5px;border:0;border-bottom:1px solid #4d5057;border-radius:0}#filter-type-city .chosen-container-single .chosen-single{font-size:16px;font-weight:700;color:#4d5057;background:0 0}#filter-type-city .chosen-container-single .chosen-search input[type=text]{border:0;padding:6px 10px;font-size:16px}#filter-type-city .chosen-container-single .chosen-search{padding:0;border-bottom:1px solid #41b4c4}#filter-type-city .chosen-container .chosen-results{margin:0;padding:0;font-size:14px}#filter-type-city .chosen-container .chosen-drop{top:2px;border:1px solid #e2e2e2;box-shadow:0 2px 3px rgba(0,0,0,.15)}#filter-type-city .chosen-container-single .chosen-drop{border-radius:2px}#filter-type-city .chosen-container .chosen-results li{padding:8px 10px 7px;transition:background .1s linear}#filter-type-bike.filter-block{padding-right:5px}#filter-bike-list{width:100%;height:520px;overflow-y:scroll}#filter-bike-list li{padding-right:12px}.accordion-tab{position:relative;border-top:1px solid #e2e2e2}#filter-bike-list li:first-child .accordion-tab{border-top:0}.accordion-tab .accordion-label-tab{width:90%;padding:16px 30px 16px 0;cursor:pointer}.accordion-tab~.bike-model-list-content{display:none}.accordion-tab .arrow-down{width:16px;height:10px;background-position:-205px -517px;position:absolute;right:0;top:22px;-moz-transition:transform .3s;-webkit-transition:transform .3s;-o-transition:transform .3s;-ms-transition:transform .3s;transition:transform .3s}.accordion-tab.tab-checked .accordion-checkbox .unchecked-box,.bike-model-list li.active .unchecked-box{background-position:-216px -135px}.accordion-tab .accordion-checkbox{position:relative;top:19px;cursor:pointer}.accordion-tab .category-label{font-size:16px;font-weight:700;max-width:70%;text-align:left;display:inline-block;vertical-align:middle}.accordion-tab .accordion-count{font-size:12px;color:#82888b;display:inline-block;vertical-align:middle;position:relative;top:2px}.bike-model-list li{font-size:14px;color:#82888b;padding:10px 0;cursor:pointer}.bike-model-list .unchecked-box{position:relative;top:2px;margin-right:6px}.bike-model-list li.active{font-weight:700;color:#4d5057}#filter-bike-list .form-control{border-radius:0}#filter-bike-list .search-icon{position:absolute;right:10px;top:12px;cursor:pointer;z-index:2}#filter-type-bike.active-clear #reset-bikes-filter{display:block}#reset-bikes-filter{display:none;color:#a2a2a2;cursor:pointer;position:relative;top:2px}#reset-bikes-filter:hover{color:#4d5057}.accordion-tab.active .arrow-down{-moz-transform:rotateZ(180deg);-webkit-transform:rotateZ(180deg);-o-transform:rotateZ(180deg);-ms-transform:rotateZ(180deg);transform:rotateZ(180deg)}#filter-bike-list::-webkit-scrollbar,#filter-type-city .chosen-container .chosen-results::-webkit-scrollbar{width:5px}#filter-bike-list::-webkit-scrollbar-track,#filter-type-city .chosen-container .chosen-results::-webkit-scrollbar-track{background:#fff}#filter-bike-list::-webkit-scrollbar-thumb,#filter-type-city .chosen-container .chosen-results::-webkit-scrollbar-thumb{background:#ddd}#filter-bike-list::-webkit-scrollbar-thumb:hover,#filter-type-city .chosen-container .chosen-results::-webkit-scrollbar-thumb:hover{background:#bbb}.ui-widget-content{background:#bababa;height:3px;margin:18px 0 3px;width:96%;border-radius:0;cursor:pointer}.ui-slider .ui-slider-range{border:none;display:block;font-size:.7em;position:absolute;z-index:1;border-radius:0}.ui-widget-header{background:#ef3030;height:3px}.ui-slider{position:relative;text-align:left}.ui-slider .ui-slider-handle{background:url(http://imgd1.aeplcdn.com/0x0/bw/static/sprites/m/slider-handle.png) 3px 3px no-repeat;cursor:pointer;height:28px;width:28px;display:block;margin-left:-9px;position:absolute;outline:0;top:-13px;z-index:2}#previous-owners-list{overflow:hidden}#previous-owners-list li{margin-left:10px;float:left}#previous-owners-list li:first-child{margin-left:0}#previous-owners-list span{display:block;font-size:16px;padding:8px 14px;border:1px solid #a2a2a2;position:relative;border-radius:2px;cursor:pointer}#previous-owners-list .last-item{padding-right:26px;padding-left:26px}#previous-owners-list span:before{content:" ";position:absolute;top:0;left:0;right:0;bottom:0;border:1px solid #fff}#previous-owners-list .active span,#previous-owners-list .active span:before{border:1px solid #4d5057}#previous-owners-list .active span{font-weight:700}#filter-type-seller.filter-block{padding-bottom:10px}#seller-type-list li{font-size:16px;color:#82888b;cursor:pointer}#seller-type-list li:first-child{margin-bottom:15px}#seller-type-list li.checked{font-weight:700;color:#4d5057}#seller-type-list li.checked .unchecked-box{background-position:-216px -135px}.model-media-details{position:absolute;right:10px;bottom:10px;font-size:12px}.model-media-item{display:inline-block;vertical-align:middle;padding:4px 5px;color:#4d5057;background:rgba(255,255,255,.8);border-radius:2px}.model-media-item:hover{text-decoration:none}.model-media-count{position:relative;top:-1px}.gallery-photo-icon{width:16px;height:12px;background-position:-213px -207px}.inr-md-lg{width:12px;height:17px;background-position:-64px -515px;position:relative;top:1px}.author-grey-sm-icon,.kms-driven-icon,.model-date-icon,.model-loc-icon{width:10px;height:12px;margin-right:5px;vertical-align:middle}.model-date-icon{background-position:-65px -543px}.kms-driven-icon{background-position:-65px -563px}.model-loc-icon{background-position:-82px -543px}.cross-icon{width:8px;height:8px;background-position:-82px -565px;margin-left:6px}.unchecked-box{width:14px;height:14px;background-position:-216px -120px;margin-right:10px}.search-icon{width:17px;height:17px;background-position:-213px -160px}.next-page-icon,.prev-page-icon{width:8px;height:12px;position:relative;left:10px;top:5px}.prev-page-icon{background-position:-213px -187px}.next-page-icon{background-position:-222px -187px}.inactive .next-page-icon.inactive,.inactive .prev-page-icon{opacity:.2}#search-listing-footer{padding-top:12px;padding-bottom:12px}#search-listing-footer .grid-5{position:relative;top:4px}#pagination-list-content{min-width:185px;width:auto;max-width:355px;float:right}#pagination-list{margin-right:35px;margin-left:35px;overflow:hidden}#pagination-list li{float:left;margin-right:5px;margin-left:5px}#pagination-list a{color:#82888b;font-size:12px;padding:5px;border:1px solid #fff;display:block;min-width:30px;text-align:center}#pagination-list a:hover{color:#4d5057;text-decoration:none}#pagination-list li.active a{color:#4d5057;font-weight:700;border:1px solid #a2a2a2;-webkit-border-radius:1px;-moz-border-radius:1px;-ms-border-radius:1px;border-radius:1px}.pagination-control-next,.pagination-control-prev{position:absolute;top:0;width:30px;height:29px}.pagination-control-prev{left:0}.pagination-control-next{right:0}@media only screen and (max-width:1024px){#listing-right-column{margin-left:313px}.model-details-content{width:293px}.model-details-label{width:82%}#previous-owners-list .last-item{padding-right:20px;padding-left:20px}}
    </style>
    <script type="text/javascript">
        <!-- #include file="\includes\gacode_desktop.aspx" -->
    </script>
</head>
<body class="bg-light-grey">
    <form id="form1" runat="server">
        <!-- #include file="/includes/headBW.aspx" -->
        <section class="bg-light-grey padding-top10" id="breadcrumb">
            <div class="container">
                <div class="grid-12">
                    <div class="breadcrumb margin-bottom15">
                        <!-- breadcrumb code starts here -->
                        <ul>
                            <li itemscope="" itemtype="http://data-vocabulary.org/Breadcrumb"><a href="/" itemprop="url">
                                <span itemprop="title">Home</span></a>
                            </li>
                            <li><span class="bwsprite fa-angle-right margin-right10"></span>
                              <span>Used Bikes</span>
                            </li>
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
                                <h1 class="font24 text-x-black">Used bike search</h1>
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
                                        <li class="sort-label">Sort by</li>
                                        <li>Relevance</li>
                                        <li>Price: Low to High</li>
                                        <li>Price: High to Low</li>
                                        <li>Year: Latest to Oldest</li>
                                        <li>Kms: Low to High</li>
                                        <li>Last Updated: Latest to Oldest</li>
                                    </ul>
                                </div>
                            </div>
                            <div class="clear"></div>
                        </div>
                        <div id="search-listing-content" class="position-rel">
                            <div id="listing-right-column" class="grid-8 padding-right20 rightfloat">
                                <div class="margin-top15 font12 padding-bottom5 border-solid-bottom">
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
                                <div class="padding-top15 padding-bottom15 text-light-grey font14 border-solid-bottom">
                                    <p>Showing <span class="text-default text-bold">1-20</span> of <span class="text-default text-bold">200</span> bikes</p>
                                </div>
                                <ul id="used-bikes-list">
                                    <li>
                                        <div class="model-thumbnail-image">
                                            <a href="" title="Honda CB Unicorn GP E">
                                                <img class="lazy" data-original="http://imgd1.aeplcdn.com//310x174//bw/used/S42378/42378_20160608023658807.jpeg" alt="Honda CB Unicorn GP E" src="" />
                                                <div class="model-media-details">
                                                    <div class="model-media-item">
                                                        <span class="bwsprite gallery-photo-icon"></span>
                                                        <span class="model-media-count">55</span>
                                                    </div>
                                                </div>
                                            </a>
                                        </div>
                                        <div class="model-details-content font14">
                                            <h2 class="margin-bottom10"><a href="" class="text-truncate text-black" title="Honda CB Unicorn GP E">Honda CB Unicorn GP E</a></h2>
                                            <div class="grid-6 alpha">
                                                <span class="bwsprite model-date-icon"></span>
                                                <span class="model-details-label">2013 model</span>
                                            </div>
                                            <div class="grid-6 omega">
                                                <span class="bwsprite kms-driven-icon"></span>
                                                <span class="model-details-label">1,45,000 kms</span>
                                            </div>
                                            <div class="grid-6 alpha">
                                                <span class="bwsprite author-grey-sm-icon"></span>
                                                <span class="model-details-label">2nd owner</span>
                                            </div>
                                            <div class="grid-6 omega">
                                                <span class="bwsprite model-loc-icon"></span>
                                                <span class="model-details-label">Mumbai</span>
                                            </div>
                                            <div class="clear"></div>
                                            <p class="margin-bottom15"><span class="bwsprite inr-md-lg"></span>&nbsp;<span class="font22 text-bold">1,22,000</span></p>
                                            <a href="javascript:void(0)" class="btn btn-white seller-details-btn" rel="nofollow">Get seller details</a>
                                        </div>
                                        <div class="clear"></div>
                                    </li>
                                    <li>
                                        <div class="model-thumbnail-image">
                                            <a href="" title="Honda Dream Yuga Electric Start/Alloy">
                                                <img class="lazy" data-original="http://imgd1.aeplcdn.com//310x174//bw/used/S42393/42393_20160608065915421.jpg" alt="Honda Dream Yuga Electric Start/Alloy" src="" />
                                                <div class="model-media-details">
                                                    <div class="model-media-item">
                                                        <span class="bwsprite gallery-photo-icon"></span>
                                                        <span class="model-media-count">4</span>
                                                    </div>
                                                </div>
                                            </a>
                                        </div>
                                        <div class="model-details-content font14">
                                            <h2 class="margin-bottom10"><a href="" class="text-truncate text-black" title="Honda Dream Yuga Electric Start/Alloy">Honda Dream Yuga Electric Start/Alloy</a></h2>
                                            <div class="grid-6 alpha">
                                                <span class="bwsprite model-date-icon"></span>
                                                <span class="model-details-label">2013 model</span>
                                            </div>
                                            <div class="grid-6 omega">
                                                <span class="bwsprite kms-driven-icon"></span>
                                                <span class="model-details-label">1,45,000 kms</span>
                                            </div>
                                            <div class="grid-6 alpha">
                                                <span class="bwsprite author-grey-sm-icon"></span>
                                                <span class="model-details-label">2nd owner</span>
                                            </div>
                                            <div class="grid-6 omega">
                                                <span class="bwsprite model-loc-icon"></span>
                                                <span class="model-details-label">Mumbai</span>
                                            </div>
                                            <div class="clear"></div>
                                            <p class="margin-bottom15"><span class="bwsprite inr-md-lg"></span>&nbsp;<span class="font22 text-bold">1,22,000</span></p>
                                            <a href="javascript:void(0)" class="btn btn-white seller-details-btn" rel="nofollow">Get seller details</a>
                                        </div>
                                        <div class="clear"></div>
                                    </li>
                                    <li>
                                        <div class="model-thumbnail-image">
                                            <a href="" title="Honda CB Unicorn GP E">
                                                <img class="lazy" data-original="http://imgd1.aeplcdn.com//310x174//bw/used/S42378/42378_20160608023658807.jpeg" alt="Honda CB Unicorn GP E" src="" />
                                                <div class="model-media-details">
                                                    <div class="model-media-item">
                                                        <span class="bwsprite gallery-photo-icon"></span>
                                                        <span class="model-media-count">55</span>
                                                    </div>
                                                </div>
                                            </a>
                                        </div>
                                        <div class="model-details-content font14">
                                            <h2 class="margin-bottom10"><a href="" class="text-truncate text-black" title="Harley Davidson Softail Classic Heritage">Harley Davidson Softail Classic Heritage</a></h2>
                                            <div class="grid-6 alpha">
                                                <span class="bwsprite model-date-icon"></span>
                                                <span class="model-details-label">2013 model</span>
                                            </div>
                                            <div class="grid-6 omega">
                                                <span class="bwsprite kms-driven-icon"></span>
                                                <span class="model-details-label">1,45,000 kms</span>
                                            </div>
                                            <div class="grid-6 alpha">
                                                <span class="bwsprite author-grey-sm-icon"></span>
                                                <span class="model-details-label">2nd owner</span>
                                            </div>
                                            <div class="grid-6 omega">
                                                <span class="bwsprite model-loc-icon"></span>
                                                <span class="model-details-label">Mumbai</span>
                                            </div>
                                            <div class="clear"></div>
                                            <p class="margin-bottom15"><span class="bwsprite inr-md-lg"></span>&nbsp;<span class="font22 text-bold">1,22,000</span></p>
                                            <a href="javascript:void(0)" class="btn btn-white seller-details-btn" rel="nofollow">Get seller details</a>
                                        </div>
                                        <div class="clear"></div>
                                    </li>
                                    <li>
                                        <div class="model-thumbnail-image">
                                            <a href="" title="Honda CB Unicorn GP E">
                                                <img class="lazy" data-original="http://imgd1.aeplcdn.com//310x174//bw/used/S42378/42378_20160608023658807.jpeg" alt="Honda CB Unicorn GP E" src="" />
                                                <div class="model-media-details">
                                                    <div class="model-media-item">
                                                        <span class="bwsprite gallery-photo-icon"></span>
                                                        <span class="model-media-count">55</span>
                                                    </div>
                                                </div>
                                            </a>
                                        </div>
                                        <div class="model-details-content font14">
                                            <h2 class="margin-bottom10"><a href="" class="text-truncate text-black" title="Honda CB Unicorn GP E">Honda CB Unicorn GP E</a></h2>
                                            <div class="grid-6 alpha">
                                                <span class="bwsprite model-date-icon"></span>
                                                <span class="model-details-label">2013 model</span>
                                            </div>
                                            <div class="grid-6 omega">
                                                <span class="bwsprite kms-driven-icon"></span>
                                                <span class="model-details-label">1,45,000 kms</span>
                                            </div>
                                            <div class="grid-6 alpha">
                                                <span class="bwsprite author-grey-sm-icon"></span>
                                                <span class="model-details-label">2nd owner</span>
                                            </div>
                                            <div class="grid-6 omega">
                                                <span class="bwsprite model-loc-icon"></span>
                                                <span class="model-details-label">Mumbai</span>
                                            </div>
                                            <div class="clear"></div>
                                            <p class="margin-bottom15"><span class="bwsprite inr-md-lg"></span>&nbsp;<span class="font22 text-bold">1,22,000</span></p>
                                            <a href="javascript:void(0)" class="btn btn-white seller-details-btn" rel="nofollow">Get seller details</a>
                                        </div>
                                        <div class="clear"></div>
                                    </li>
                                    <li>
                                        <div class="model-thumbnail-image">
                                            <a href="" title="Honda Dream Yuga Electric Start/Alloy">
                                                <img class="lazy" data-original="http://imgd1.aeplcdn.com//310x174//bw/used/S42393/42393_20160608065915421.jpg" alt="Honda Dream Yuga Electric Start/Alloy" src="" />
                                                <div class="model-media-details">
                                                    <div class="model-media-item">
                                                        <span class="bwsprite gallery-photo-icon"></span>
                                                        <span class="model-media-count">4</span>
                                                    </div>
                                                </div>
                                            </a>
                                        </div>
                                        <div class="model-details-content font14">
                                            <h2 class="margin-bottom10"><a href="" class="text-truncate text-black" title="Honda Dream Yuga Electric Start/Alloy">Honda Dream Yuga Electric Start/Alloy</a></h2>
                                            <div class="grid-6 alpha">
                                                <span class="bwsprite model-date-icon"></span>
                                                <span class="model-details-label">2013 model</span>
                                            </div>
                                            <div class="grid-6 omega">
                                                <span class="bwsprite kms-driven-icon"></span>
                                                <span class="model-details-label">1,45,000 kms</span>
                                            </div>
                                            <div class="grid-6 alpha">
                                                <span class="bwsprite author-grey-sm-icon"></span>
                                                <span class="model-details-label">2nd owner</span>
                                            </div>
                                            <div class="grid-6 omega">
                                                <span class="bwsprite model-loc-icon"></span>
                                                <span class="model-details-label">Mumbai</span>
                                            </div>
                                            <div class="clear"></div>
                                            <p class="margin-bottom15"><span class="bwsprite inr-md-lg"></span>&nbsp;<span class="font22 text-bold">1,22,000</span></p>
                                            <a href="javascript:void(0)" class="btn btn-white seller-details-btn" rel="nofollow">Get seller details</a>
                                        </div>
                                        <div class="clear"></div>
                                    </li>
                                    <li>
                                        <div class="model-thumbnail-image">
                                            <a href="" title="Honda CB Unicorn GP E">
                                                <img class="lazy" data-original="http://imgd1.aeplcdn.com//310x174//bw/used/S42378/42378_20160608023658807.jpeg" alt="Honda CB Unicorn GP E" src="" />
                                                <div class="model-media-details">
                                                    <div class="model-media-item">
                                                        <span class="bwsprite gallery-photo-icon"></span>
                                                        <span class="model-media-count">55</span>
                                                    </div>
                                                </div>
                                            </a>
                                        </div>
                                        <div class="model-details-content font14">
                                            <h2 class="margin-bottom10"><a href="" class="text-truncate text-black" title="Harley Davidson Softail Classic Heritage">Harley Davidson Softail Classic Heritage</a></h2>
                                            <div class="grid-6 alpha">
                                                <span class="bwsprite model-date-icon"></span>
                                                <span class="model-details-label">2013 model</span>
                                            </div>
                                            <div class="grid-6 omega">
                                                <span class="bwsprite kms-driven-icon"></span>
                                                <span class="model-details-label">1,45,000 kms</span>
                                            </div>
                                            <div class="grid-6 alpha">
                                                <span class="bwsprite author-grey-sm-icon"></span>
                                                <span class="model-details-label">2nd owner</span>
                                            </div>
                                            <div class="grid-6 omega">
                                                <span class="bwsprite model-loc-icon"></span>
                                                <span class="model-details-label">Mumbai</span>
                                            </div>
                                            <div class="clear"></div>
                                            <p class="margin-bottom15"><span class="bwsprite inr-md-lg"></span>&nbsp;<span class="font22 text-bold">1,22,000</span></p>
                                            <a href="javascript:void(0)" class="btn btn-white seller-details-btn" rel="nofollow">Get seller details</a>
                                        </div>
                                        <div class="clear"></div>
                                    </li>
                                    <li>
                                        <div class="model-thumbnail-image">
                                            <a href="" title="Honda CB Unicorn GP E">
                                                <img class="lazy" data-original="http://imgd1.aeplcdn.com//310x174//bw/used/S42378/42378_20160608023658807.jpeg" alt="Honda CB Unicorn GP E" src="" />
                                                <div class="model-media-details">
                                                    <div class="model-media-item">
                                                        <span class="bwsprite gallery-photo-icon"></span>
                                                        <span class="model-media-count">55</span>
                                                    </div>
                                                </div>
                                            </a>
                                        </div>
                                        <div class="model-details-content font14">
                                            <h2 class="margin-bottom10"><a href="" class="text-truncate text-black" title="Honda CB Unicorn GP E">Honda CB Unicorn GP E</a></h2>
                                            <div class="grid-6 alpha">
                                                <span class="bwsprite model-date-icon"></span>
                                                <span class="model-details-label">2013 model</span>
                                            </div>
                                            <div class="grid-6 omega">
                                                <span class="bwsprite kms-driven-icon"></span>
                                                <span class="model-details-label">1,45,000 kms</span>
                                            </div>
                                            <div class="grid-6 alpha">
                                                <span class="bwsprite author-grey-sm-icon"></span>
                                                <span class="model-details-label">2nd owner</span>
                                            </div>
                                            <div class="grid-6 omega">
                                                <span class="bwsprite model-loc-icon"></span>
                                                <span class="model-details-label">Mumbai</span>
                                            </div>
                                            <div class="clear"></div>
                                            <p class="margin-bottom15"><span class="bwsprite inr-md-lg"></span>&nbsp;<span class="font22 text-bold">1,22,000</span></p>
                                            <a href="javascript:void(0)" class="btn btn-white seller-details-btn" rel="nofollow">Get seller details</a>
                                        </div>
                                        <div class="clear"></div>
                                    </li>
                                    <li>
                                        <div class="model-thumbnail-image">
                                            <a href="" title="Honda Dream Yuga Electric Start/Alloy">
                                                <img class="lazy" data-original="http://imgd1.aeplcdn.com//310x174//bw/used/S42393/42393_20160608065915421.jpg" alt="Honda Dream Yuga Electric Start/Alloy" src="" />
                                                <div class="model-media-details">
                                                    <div class="model-media-item">
                                                        <span class="bwsprite gallery-photo-icon"></span>
                                                        <span class="model-media-count">4</span>
                                                    </div>
                                                </div>
                                            </a>
                                        </div>
                                        <div class="model-details-content font14">
                                            <h2 class="margin-bottom10"><a href="" class="text-truncate text-black" title="Honda Dream Yuga Electric Start/Alloy">Honda Dream Yuga Electric Start/Alloy</a></h2>
                                            <div class="grid-6 alpha">
                                                <span class="bwsprite model-date-icon"></span>
                                                <span class="model-details-label">2013 model</span>
                                            </div>
                                            <div class="grid-6 omega">
                                                <span class="bwsprite kms-driven-icon"></span>
                                                <span class="model-details-label">1,45,000 kms</span>
                                            </div>
                                            <div class="grid-6 alpha">
                                                <span class="bwsprite author-grey-sm-icon"></span>
                                                <span class="model-details-label">2nd owner</span>
                                            </div>
                                            <div class="grid-6 omega">
                                                <span class="bwsprite model-loc-icon"></span>
                                                <span class="model-details-label">Mumbai</span>
                                            </div>
                                            <div class="clear"></div>
                                            <p class="margin-bottom15"><span class="bwsprite inr-md-lg"></span>&nbsp;<span class="font22 text-bold">1,22,000</span></p>
                                            <a href="javascript:void(0)" class="btn btn-white seller-details-btn" rel="nofollow">Get seller details</a>
                                        </div>
                                        <div class="clear"></div>
                                    </li>
                                    <li>
                                        <div class="model-thumbnail-image">
                                            <a href="" title="Honda CB Unicorn GP E">
                                                <img class="lazy" data-original="http://imgd1.aeplcdn.com//310x174//bw/used/S42378/42378_20160608023658807.jpeg" alt="Honda CB Unicorn GP E" src="" />
                                                <div class="model-media-details">
                                                    <div class="model-media-item">
                                                        <span class="bwsprite gallery-photo-icon"></span>
                                                        <span class="model-media-count">55</span>
                                                    </div>
                                                </div>
                                            </a>
                                        </div>
                                        <div class="model-details-content font14">
                                            <h2 class="margin-bottom10"><a href="" class="text-truncate text-black" title="Harley Davidson Softail Classic Heritage">Harley Davidson Softail Classic Heritage</a></h2>
                                            <div class="grid-6 alpha">
                                                <span class="bwsprite model-date-icon"></span>
                                                <span class="model-details-label">2013 model</span>
                                            </div>
                                            <div class="grid-6 omega">
                                                <span class="bwsprite kms-driven-icon"></span>
                                                <span class="model-details-label">1,45,000 kms</span>
                                            </div>
                                            <div class="grid-6 alpha">
                                                <span class="bwsprite author-grey-sm-icon"></span>
                                                <span class="model-details-label">2nd owner</span>
                                            </div>
                                            <div class="grid-6 omega">
                                                <span class="bwsprite model-loc-icon"></span>
                                                <span class="model-details-label">Mumbai</span>
                                            </div>
                                            <div class="clear"></div>
                                            <p class="margin-bottom15"><span class="bwsprite inr-md-lg"></span>&nbsp;<span class="font22 text-bold">1,22,000</span></p>
                                            <a href="javascript:void(0)" class="btn btn-white seller-details-btn" rel="nofollow">Get seller details</a>
                                        </div>
                                        <div class="clear"></div>
                                    </li>
                                    <li>
                                        <div class="model-thumbnail-image">
                                            <a href="" title="Honda CB Unicorn GP E">
                                                <img class="lazy" data-original="http://imgd1.aeplcdn.com//310x174//bw/used/S42378/42378_20160608023658807.jpeg" alt="Honda CB Unicorn GP E" src="" />
                                                <div class="model-media-details">
                                                    <div class="model-media-item">
                                                        <span class="bwsprite gallery-photo-icon"></span>
                                                        <span class="model-media-count">55</span>
                                                    </div>
                                                </div>
                                            </a>
                                        </div>
                                        <div class="model-details-content font14">
                                            <h2 class="margin-bottom10"><a href="" class="text-truncate text-black" title="Honda CB Unicorn GP E">Honda CB Unicorn GP E</a></h2>
                                            <div class="grid-6 alpha">
                                                <span class="bwsprite model-date-icon"></span>
                                                <span class="model-details-label">2013 model</span>
                                            </div>
                                            <div class="grid-6 omega">
                                                <span class="bwsprite kms-driven-icon"></span>
                                                <span class="model-details-label">1,45,000 kms</span>
                                            </div>
                                            <div class="grid-6 alpha">
                                                <span class="bwsprite author-grey-sm-icon"></span>
                                                <span class="model-details-label">2nd owner</span>
                                            </div>
                                            <div class="grid-6 omega">
                                                <span class="bwsprite model-loc-icon"></span>
                                                <span class="model-details-label">Mumbai</span>
                                            </div>
                                            <div class="clear"></div>
                                            <p class="margin-bottom15"><span class="bwsprite inr-md-lg"></span>&nbsp;<span class="font22 text-bold">1,22,000</span></p>
                                            <a href="javascript:void(0)" class="btn btn-white seller-details-btn" rel="nofollow">Get seller details</a>
                                        </div>
                                        <div class="clear"></div>
                                    </li>
                                </ul>
                            
                                <div id="search-listing-footer" class="font14">
                                    <div class="grid-5 alpha omega text-light-grey">
                                        <p>Showing <span class="text-default text-bold">1-20</span> of <span class="text-default text-bold">200</span> bikes</p>
                                    </div>
                                    <div id="pagination-list-content" class="grid-7 alpha omega position-rel">
                                        <ul id="pagination-list">
                                            <li><a href="">1</a></li>
                                            <li><a href="">2</a></li>
                                            <li class="active"><a href="">3</a></li>
                                            <li><a href="">4</a></li>
                                            <li><a href="">5</a></li>
                                        </ul>
                                        <a href="" class="pagination-control-prev inactive">
                                            <span class="bwsprite prev-page-icon"></span>
                                        </a>
                                        <a href="" class="pagination-control-next">
                                            <span class="bwsprite next-page-icon"></span>
                                        </a>
                                    </div>
                                    <div class="clear"></div>
                                </div>
                            </div>
                            <div id="listing-left-column" class="grid-4 alpha font14 position-abt">
                                <div id="filter-sidebar" class="border-solid-right">
                                    <div id="filters-head">
                                        <p class="font18 text-bold text-x-black leftfloat">Filters</p>
                                        <p id="reset-filters" class="btn btn-white font14 rightfloat">Reset</p>
                                        <div class="clear"></div>
                                    </div>

                                    <div id="filter-type-city" class="filter-block">
                                        <p class="filter-label margin-bottom5">City</p>
                                        <div class="clear"></div>
                                        <select class="city-chosen-select hide">
                                            <option>Mumbai</option>
                                            <option>New Delhi</option>
                                            <option>Chennai</option>
                                            <option>Banglore</option>
                                        </select>
                                    </div>

                                    <div id="filter-type-bike" class="filter-block">
                                        <p class="filter-label">Bike</p>
                                        <p id="reset-bikes-filter" class="font12 padding-right20 rightfloat">Clear all</p>
                                        <div class="clear"></div>
                                        <ul id="filter-bike-list">
                                            <li>
                                                <div id="mk-7" class="accordion-tab">
                                                    <div class="accordion-checkbox leftfloat">
                                                        <span class="bwsprite unchecked-box"></span>
                                                    </div>
                                                    <div class="accordion-label-tab leftfloat">
                                                        <span class="category-label">Honda</span>
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
                                                    <li id="md-1">
                                                        <span class="bwsprite unchecked-box"></span>
                                                        <span class="category-label">Navi</span>
                                                    </li>
                                                    <li id="md-2">
                                                        <span class="bwsprite unchecked-box"></span>
                                                        <span class="category-label">Activa-i</span>
                                                    </li>
                                                    <li id="md-3">
                                                        <span class="bwsprite unchecked-box"></span>
                                                        <span class="category-label">CD 110 Dream</span>
                                                    </li>
                                                    <li id="md-4">
                                                        <span class="bwsprite unchecked-box"></span>
                                                        <span class="category-label">Dio</span>
                                                    </li>
                                                    <li id="md-5">
                                                        <span class="bwsprite unchecked-box"></span>
                                                        <span class="category-label">Dream Neo</span>
                                                    </li>
                                                    <li id="md-6">
                                                        <span class="bwsprite unchecked-box"></span>
                                                        <span class="category-label">Activa 3G</span>
                                                    </li>
                                                </ul>
                                                </div>
                                            </li>
                                            <li>
                                                <div id="mk-8" class="accordion-tab">
                                                    <div class="accordion-checkbox leftfloat">
                                                        <span class="bwsprite unchecked-box"></span>
                                                    </div>
                                                    <div class="accordion-label-tab leftfloat">
                                                        <span class="category-label">Bajaj</span>
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
                                                        <li id="md-11">
                                                            <span class="bwsprite unchecked-box"></span>
                                                            <span class="category-label">Navi</span>
                                                        </li>
                                                        <li id="md-12">
                                                            <span class="bwsprite unchecked-box"></span>
                                                            <span class="category-label">Activa-i</span>
                                                        </li>
                                                        <li id="md-13">
                                                            <span class="bwsprite unchecked-box"></span>
                                                            <span class="category-label">CD 110 Dream</span>
                                                        </li>
                                                        <li id="md-14">
                                                            <span class="bwsprite unchecked-box"></span>
                                                            <span class="category-label">Dio</span>
                                                        </li>
                                                        <li id="md-15">
                                                            <span class="bwsprite unchecked-box"></span>
                                                            <span class="category-label">Dream Neo</span>
                                                        </li>
                                                        <li id="md-16">
                                                            <span class="bwsprite unchecked-box"></span>
                                                            <span class="category-label">Activa 3G</span>
                                                        </li>
                                                        <li id="md-17">
                                                            <span class="bwsprite unchecked-box"></span>
                                                            <span class="category-label">Dream Yuga</span>
                                                        </li>
                                                        <li id="md-18">
                                                            <span class="bwsprite unchecked-box"></span>
                                                            <span class="category-label">Yuga</span>
                                                        </li>
                                                    </ul>
                                                </div>
                                            </li>
                                        </ul>
                                    </div>

                                    <div class="filter-block">
                                        <p class="filter-label">Budget</p>
                                        <p id="budget-amount" class="font14 text-bold rightfloat"></p>
                                        <div class="clear"></div>
                                        <div id="budget-range-slider"></div>
                                    </div>

                                    <div class="filter-block">
                                        <p class="filter-label">Kms ridden</p>
                                        <p id="kms-amount" class="font14 text-bold rightfloat"></p>
                                        <div class="clear"></div>
                                        <div id="kms-range-slider"></div>
                                    </div>

                                    <div class="filter-block">
                                        <p class="filter-label">Bike age</p>
                                        <p id="bike-age-amount" class="font14 text-bold rightfloat"></p>
                                        <div class="clear"></div>
                                        <div id="bike-age-slider"></div>
                                    </div>

                                    <div class="filter-block">
                                        <p class="filter-label margin-bottom10">Previous owners</p>
                                        <div class="clear"></div>
                                        <ul id="previous-owners-list">
                                            <li id="own-1"><span>1</span></li>
                                            <li id="own-2"><span>2</span></li>
                                            <li id="own-3"><span>3</span></li>
                                            <li id="own-4"><span>4</span></li>
                                            <li id="own-5"><span class="last-item">4+</span></li>
                                        </ul>
                                    </div>

                                    <div id="filter-type-seller" class="filter-block">
                                        <p class="filter-label margin-bottom15">Seller type</p>
                                        <div class="clear"></div>
                                        <ul id="seller-type-list">
                                            <li id="sl-1"><span class="bwsprite unchecked-box"></span><span class="category-label">Individual</span></li>
                                            <li id="sl-2"><span class="bwsprite unchecked-box"></span><span class="category-label">Dealer</span></li>
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

        <script type="text/javascript" src="<%= staticUrl != "" ? "http://st1.aeplcdn.com" + staticUrl : "" %>/src/frameworks.js?<%=staticFileVersion %>"></script>
        <!-- #include file="/includes/footerBW.aspx" -->
        <link href="<%= staticUrl != "" ? "http://st2.aeplcdn.com" + staticUrl : "" %>/css/bw-common-btf.css?<%=staticFileVersion %>" rel="stylesheet" type="text/css" />
        <script type="text/javascript" src="<%= staticUrl != string.Empty ? "http://st2.aeplcdn.com" + staticUrl : string.Empty %>/src/common.min.js?<%= staticFileVersion %>"></script>
        <script type="text/javascript" src="<%= staticUrl != string.Empty ? "http://st2.aeplcdn.com" + staticUrl : string.Empty %>/src/used-search.js?<%= staticFileVersion %>"></script>
        <link href='https://fonts.googleapis.com/css?family=Open+Sans:400,700' rel='stylesheet' type='text/css' />
        <!--[if lt IE 9]>
            <script src="/src/html5.js"></script>
        <![endif]-->

    </form>
</body>
</html>
