<%@ Page Language="C#" AutoEventWireup="false" Inherits="Bikewale.Mobile.Used.Search" %>
<%@ Register TagPrefix="BikeWale" TagName="Pager" Src="/m/controls/LinkPagerControl.ascx" %>
<!DOCTYPE html>
<html>
<head>
        <%
            title = pageTitle;
            description = pageDescription;
            canonical = pageCanonical;
            keywords = pageKeywords;
            EnableOG = true;
            relPrevPageUrl = prevUrl.Replace("/m/", "/");
            relNextPageUrl = nextUrl.Replace("/m/", "/");
            Ad_320x50 = true;
            Ad_Bot_320x50 = true;
        %>

    <!-- #include file="/includes/headscript_mobile_min.aspx" -->
    <style type="text/css">
        #pagination-list a:hover,.model-media-item:hover{text-decoration:none}.accordion-tab .accordion-label,.model-details-label,.selected-filters{text-overflow:ellipsis;white-space:nowrap}#used-bikes-list h2 a{color:#2a2a2a}#used-bikes-list li{margin-bottom:20px}#used-bikes-list li:first-child{padding-top:0}#used-bikes-list li:before{content:'';display:block;border-top:1px solid #e2e2e2;margin-right:20px;margin-left:20px;padding-bottom:20px}#used-bikes-list li:first-child:before{content:none}.model-thumbnail-image{width:100%;max-width:476px;margin:0 auto;height:202px;display:table;position:relative;text-align:center;background-color:#f5f5f5}.model-image-target{width:100%;display:table-cell;vertical-align:middle;line-height:0;background:url(http://imgd4.aeplcdn.com/0x0/bw/static/sprites/m/circleloader.gif) center center no-repeat}.model-details-label,.model-media-item{display:inline-block;vertical-align:middle}.model-thumbnail-image img{max-height:202px}.model-media-details{position:absolute;right:15px;bottom:10px;font-size:12px}.model-media-item{padding:4px 5px;color:#4d5057;background:rgba(255,255,255,.9);border-radius:2px}.model-media-count{position:relative;top:-1px}.gallery-photo-icon{width:16px;height:12px;background-position:0 -486px}.kms-driven-icon,.model-date-icon,.model-loc-icon{width:10px;height:14px;margin-right:5px;vertical-align:middle}.model-date-icon{background-position:-140px -484px}.kms-driven-icon{background-position:-159px -484px}.model-loc-icon{background-position:-177px -484px}.author-grey-sm-icon{height:12px;margin-right:5px;vertical-align:middle}.model-details-label{width:85%;color:#82888b;text-align:left;overflow:hidden}.btn.seller-details-btn{font-size:14px;padding:5px 31px}#sort-filter-wrapper{width:100%;position:fixed;left:0;bottom:0;background:#fff;z-index:2;-moz-box-shadow:0 0 6px #999;-webkit-box-shadow:0 0 6px #999;-o-box-shadow:0 0 6px #999;-ms-box-shadow:0 0 6px #999;box-shadow:0 0 6px #999}.filter-icon,.sort-by-icon{width:18px;height:12px;margin-right:15px}.sort-by-icon{background-position:-49px -510px}.filter-icon{background-position:-49px -528px}#sort-filters-loader{display:none;width:100%;height:100%;position:fixed;top:0;left:0;background:rgba(245,245,245,.95);z-index:4}#pagination-list{margin-right:20px;margin-left:20px;overflow:hidden}#pagination-list li{float:left;margin-right:2px;margin-left:2px}#pagination-list .active,#pagination-list a{color:#82888b;font-size:12px;padding-right:5px;padding-left:5px;border:1px solid #fff;display:block;min-width:25px;text-align:center}#pagination-list li.active{color:#4d5057;font-weight:700;border:1px solid #a2a2a2;border-radius:1px}.pagination-control-next,.pagination-control-prev{position:absolute;top:0;height:20px}.pagination-control-prev{left:0}.pagination-control-next{right:5px}.next-page-icon,.prev-page-icon{width:18px;height:18px}.prev-page-icon{background-position:-164px -391px}.next-page-icon{background-position:-178px -391px}.pagination-control-next.inactive .next-page-icon,.pagination-control-prev.inactive .prev-page-icon{opacity:.2;pointer-events:none}.sort-popup-container{display:none;width:80%;min-height:350px;position:fixed;top:14%;left:0;right:0;background:#fff;z-index:4;margin:0 auto}.sort-popup-container .popup-header{font-size:16px;color:#fff;padding:17px 20px;background:#2a2a2a}.sort-popup-container .popup-body{padding:25px 20px 30px}#sort-by-list li{font-size:14px;color:#82888b;cursor:pointer;margin-top:10px;padding-top:5px;padding-bottom:5px}#sort-by-list li:first-child{margin-top:0}.radio-uncheck{width:14px;height:14px;position:relative;top:2px;margin-right:30px;background-position:-179px -80px}.filter-popup-container,.modal-background{display:none;position:fixed;top:0;right:0;left:0}.modal-background{background:#000;bottom:0;z-index:3;opacity:.6;-ms-filter:"progid:DXImageTransform.Microsoft.Alpha(Opacity=60)";filter:alpha(opacity=60);-moz-opacity:.6;-khtml-opacity:.6}.back-to-top{z-index:3 !important;bottom:55px !important}#sort-by-list li.active{color:#4d5057;font-weight:700}#sort-by-list li.active .radio-uncheck{background-position:-179px -59px}.filter-popup-container{width:100%;bottom:0;background:#fff;z-index:4;overflow-x:hidden;overflow-y:scroll}.ui-corner-top{width:100%;height:50px}.filter-back-arrow{padding:15px 15px 12px;cursor:pointer}.filter-popup-label{font-size:18px;font-weight:700;margin-top:12px}#filter-selection-list{padding:25px 30px 80px}.filter-option-key{font-size:12px;color:#82888b}.filter-option-value{font-size:16px;font-weight:700;padding-top:5px;padding-bottom:5px;border-bottom:1px solid #4d5057;cursor:pointer;position:relative}.selected-filters{width:90%;text-align:left;overflow:hidden}.grey-right-icon{width:7px;height:11px;background-position:-69px -421px;position:absolute;right:0;top:11px}.margin-bottom35{margin-bottom:35px}.ui-widget-content{background:#bababa;height:3px;margin:18px 0 3px;width:96%;border-radius:0}.ui-slider .ui-slider-range{border:none;display:block;font-size:.7em;position:absolute;z-index:1;border-radius:0}.ui-widget-header{background:#ef3030;height:3px}.ui-slider{position:relative;text-align:left}.ui-slider .ui-slider-handle{background:url(http://imgd1.aeplcdn.com/0x0/bw/static/sprites/m/slider-handle.png) 3px 3px no-repeat;cursor:pointer;height:28px;width:28px;display:block;margin-left:-9px;position:absolute;outline:0;top:-13px;z-index:2}#previous-owners-list{overflow:hidden}#previous-owners-list li{margin-left:10px;float:left}#previous-owners-list li:first-child{margin-left:0}#previous-owners-list span{display:block;font-size:16px;padding:8px 14px;border:1px solid #82888b;position:relative;border-radius:1px;cursor:pointer}#previous-owners-list .prev-owner-last-item{padding-right:36px;padding-left:36px}#previous-owners-list span:before{content:" ";position:absolute;top:0;left:0;right:0;bottom:0;border:1px solid #fff}#previous-owners-list .active span,#previous-owners-list .active span:before{border:1px solid #4d5057}#previous-owners-list .active span{font-weight:700}.filter-type-seller{font-size:15px;color:#82888b}.filter-type-seller.checked{font-weight:700;color:#4d5057}.filter-container-footer{width:100%;padding:10px 20px;background:rgba(255,255,255,.7);z-index:2}.btn-size-0{font-size:14px;padding:5px 0}#filter-container.fixed #filter-container-header{position:fixed;top:0;left:0;z-index:2}#filter-container.fixed #filter-selection-list{padding-top:75px}#filter-container.fixed #filter-container-footer,#filter-container.fixed.bikes-footer #filter-bike-container-footer{position:fixed;bottom:0;left:0}#filter-city-container.city-header{padding-top:40px}#filter-city-container.city-header .filter-input-box{position:fixed;top:0;left:0}#filter-city-container{background:#f5f5f5}.filter-input-box{width:100%}#filter-city-list li{border-top:1px solid #ccc;font-size:14px;padding:15px 20px;color:#333;cursor:pointer}.filter-bike-banner{width:100%;height:110px;background:url(http://imgd3.aeplcdn.com/0x0/bw/static/landing-banners/m/bike-popup-banner.png) center bottom repeat-x #fff}#filter-bike-container{display:none;z-index:3}#filter-bike-container .filter-container-footer{z-index:3}.accordion-tab{padding:4px 5px;position:relative;border-top:1px solid #e2e2e2}.accordion-tab .accordion-label-tab{width:85%;padding:11px 40px 11px 0;cursor:pointer}.accordion-tab~.bike-model-list{display:none}.accordion-tab .arrow-down{position:absolute;right:20px;top:23px;-moz-transition:transform .3s;-webkit-transition:transform .3s;-o-transition:transform .3s;-ms-transition:transform .3s;transition:transform .3s}.accordion-tab .accordion-checkbox{padding:15px;cursor:pointer}.accordion-tab .accordion-label{font-size:16px;font-weight:700;color:#000;max-width:70%;overflow:hidden;text-align:left;display:inline-block;vertical-align:middle}.accordion-tab .accordion-count{font-size:12px;color:#82888b;display:inline-block;vertical-align:middle;position:relative;top:2px}.checked-box,.unchecked-box{width:14px;height:14px;display:inline-block}.unchecked-box{background-position:-174px -415px}.checked-box{background-position:-174px -430px}.arrow-down{width:16px;height:11px;background-position:-194px -421px}.accordion-tab.tab-checked .accordion-checkbox .unchecked-box,.bike-model-list li.active .unchecked-box{background-position:-174px -430px}.bike-model-list li{padding:10px 20px;cursor:pointer}.bike-model-list li:first-child{padding-top:0}.bike-model-label{font-size:16px;color:#82888b;padding-left:11px}.accordion-tab.active .arrow-down{-moz-transform:rotateZ(180deg);-webkit-transform:rotateZ(180deg);-o-transform:rotateZ(180deg);-ms-transform:rotateZ(180deg);transform:rotateZ(180deg)}*{-webkit-tap-highlight-color:transparent;-moz-tap-highlight-color:transparent}@media only screen and (max-width:350px){.model-thumbnail-image,.model-thumbnail-image img{height:170px}#previous-owners-list span{padding-right:12px;padding-left:12px}#previous-owners-list .prev-owner-last-item{padding-right:25px;padding-left:25px}#pagination-list a{padding-right:3px;padding-left:3px;font-size:12px;min-width:22px}.sort-popup-container{top:10px}}
    </style>
    <script type="text/javascript">
        <!-- #include file="\includes\gacode_mobile.aspx" -->
    </script>

</head>
<body>
    <form id="form1" runat="server">
        <!-- #include file="/includes/headBW_Mobile.aspx" -->
       <asp:HiddenField runat="server" ID="hdnHash" Value="" />
         <div class="modal-background"></div>
         <div id="usedBikesSection">
            <section>
                <div class="container bg-white clearfix">
                    <h1 class="padding-top15 padding-right20 padding-bottom15 padding-left20 box-shadow"><%= heading %></h1>
                <div class="font14 padding-top10 padding-right20 padding-bottom10 padding-left20" style="display:none" data-bind="visible: !OnInit() && TotalBikes() > 0">Showing <span class="text-bold"><span data-bind="    CurrencyText: (Pagination().pageNumber() - 1) * Pagination().pageSize() + 1"></span>-<span data-bind="CurrencyText: Math.min(TotalBikes(), Pagination().pageNumber() * Pagination().pageSize())""></span> of <span class="text-bold" data-bind="CurrencyText: TotalBikes()"></span> bikes</div>
                <% if(totalListing > 0){ %>
                    <div data-bind="visible: OnInit()" class="font14 padding-top10 padding-right20 padding-bottom10 padding-left20">Showing <span class="text-bold"><%=_startIndex %>-<%=_endIndex %></span> of <span class="text-bold"><%= Bikewale.Utility.Format.FormatPrice(strTotal) %></span> bikes</div>
                    <% } %>
                    <div id="sort-filter-wrapper" class="text-center border-solid-bottom">
                        <div id="sort-floating-btn" class="grid-6 padding-top10 padding-bottom10 border-solid-right cur-pointer">
                            <span class="bwmsprite sort-by-icon"></span>
                            <span class="font14 text-bold">Sort by</span>
                        </div>
                        <div id="filter-floating-btn" class="grid-6 padding-top10 padding-bottom10 cur-pointer">
                            <span class="bwmsprite filter-icon"></span>
                            <span class="font14 text-bold" id="filterStart" data-bind="click : function(d,e){ QueryString();SetPageFilters();}">Filter</span>
                        </div>
                        <div class="clear"></div>
                    </div>

                    <ul id="used-bikes-list" data-bind="visible: OnInit() && !noBikes()">
                        <asp:Repeater ID="rptUsedListings" runat="server">
                            <ItemTemplate>
                                <li >
                                    <div class="model-thumbnail-image">
                                    <a href="/m/used/bikes-in-<%# DataBinder.Eval(Container.DataItem, "CityMaskingName").ToString() %>/<%# DataBinder.Eval(Container.DataItem, "MakeMaskingName").ToString() %>-<%# DataBinder.Eval(Container.DataItem, "ModelMaskingName").ToString() %>-<%# DataBinder.Eval(Container.DataItem, "ProfileId").ToString() %>/" class="model-image-target">
                                            <img class="lazy" data-original="<%# Bikewale.Utility.Image.GetPathToShowImages(DataBinder.Eval(Container.DataItem, "Photo.OriginalImagePath").ToString(),DataBinder.Eval(Container.DataItem, "Photo.HostUrl").ToString(),Bikewale.Utility.ImageSize._370x208) %>" 
                                                 alt="" title="" border="0" />
                                            <div class="model-media-details <%# Convert.ToUInt16(DataBinder.Eval(Container.DataItem, "TotalPhotos")) > 0? "":"hide" %>">
                                                <div class="model-media-item">
                                                    <span class="bwmsprite gallery-photo-icon"></span>
                                                    <span class="model-media-count" ><%# DataBinder.Eval(Container.DataItem, "TotalPhotos").ToString() %></span>
                                                </div>
                                            </div>
                                        </a>
                                    </div>
                                    <div class="margin-right20 margin-left20 padding-top10 font14">
                                        <h2 class="margin-bottom10">
                                        <a href="/m/used/bikes-in-<%# DataBinder.Eval(Container.DataItem, "CityMaskingName").ToString() %>/<%# DataBinder.Eval(Container.DataItem, "MakeMaskingName").ToString() %>-<%# DataBinder.Eval(Container.DataItem, "ModelMaskingName").ToString() %>-<%# DataBinder.Eval(Container.DataItem, "ProfileId").ToString() %>/">
                                                <%# DataBinder.Eval(Container.DataItem, "MakeName").ToString() + " " + DataBinder.Eval(Container.DataItem, "ModelName").ToString() + " " + DataBinder.Eval(Container.DataItem, "VersionName").ToString() %>
                                            </a>
                                        </h2>
                                        <div class="grid-6 alpha omega margin-bottom5 <%# DataBinder.Eval(Container.DataItem, "ModelYear").ToString() == "0"? "hide" : string.Empty %>">
                                            <span class="bwmsprite model-date-icon"></span>
                                            <span class="model-details-label"><%# DataBinder.Eval(Container.DataItem, "ModelYear").ToString() %> model</span>
                                        </div>
                                        <div class="grid-6 alpha omega margin-bottom5 <%# DataBinder.Eval(Container.DataItem, "KmsDriven").ToString() == "0"? "hide" : string.Empty %>">
                                            <span class="bwmsprite kms-driven-icon"></span>
                                            <span class="model-details-label"><%# Bikewale.Utility.Format.FormatNumeric(DataBinder.Eval(Container.DataItem, "KmsDriven").ToString()) %> kms</span>
                                        </div>
                                        <div class="grid-6 alpha omega margin-bottom5<%# string.IsNullOrEmpty(Convert.ToString(DataBinder.Eval(Container.DataItem, "NoOfOwners")))? "hide" : string.Empty %>">
                                            <span class="bwmsprite author-grey-sm-icon"></span>
                                            <span class="model-details-label"><%# DataBinder.Eval(Container.DataItem, "NoOfOwners").ToString() %> owner</span>
                                        </div>
                                        <div class="grid-6 alpha omega margin-bottom5">
                                            <span class="bwmsprite model-loc-icon"></span>
                                            <span class="model-details-label"><%# DataBinder.Eval(Container.DataItem, "CityName").ToString() %></span>
                                        </div>
                                        <div class="clear"></div>
                                        <p class="margin-bottom15"><span class="bwmsprite inr-md-icon"></span>&nbsp;<span class="font22 text-bold"><%# Bikewale.Utility.Format.FormatPrice(DataBinder.Eval(Container.DataItem, "AskingPrice").ToString()) %></span></p>
                                    <%--<a href="javascript:void(0)" class="btn btn-orange seller-details-btn" rel="nofollow">Get seller details</a>--%>
                                    </div>
                                </li>
                            </ItemTemplate>
                        </asp:Repeater> 
                    </ul>

                    <ul id="used-bikes-list" style="display:none" data-bind="visible: !OnInit() && !noBikes() ,foreach : BikeDetails()">
                        <li>
                            <div class="model-thumbnail-image">
                                <a data-bind=" attr: { 'href': '/m/used/bikes-in-' + cityMasking + '/' + makeMasking + '-' + modelMasking + '-' + profileId + '/' }" class="model-image-target">
                                    <!-- ko if : $index() < 3 -->
                                    <img data-bind="attr: { alt: bikeName, title: bikeName, src: (photo.originalImagePath != '') ? (photo.hostUrl + '/370x208/' + photo.originalImagePath) : 'http://imgd3.aeplcdn.com/174x98/bikewaleimg/images/noimage.png'} " alt="" title="" border="0" />
                                    <!-- /ko --><!-- ko if : $index() > 2 -->
                                    <img data-bind="attr: { alt: bikeName, title: bikeName, src: 'http://imgd3.aeplcdn.com/0x0/bw/static/sprites/m/circleloader.gif' }, lazyload: ((photo.originalImagePath != '') ? (photo.hostUrl + '/370x208/' + photo.originalImagePath) : 'http://imgd3.aeplcdn.com/174x98/bikewaleimg/images/noimage.png'), " alt="" title="" border="0" />
                                    <!-- /ko -->
                                    <div class="model-media-details">
                                        <div class="model-media-item" data-bind="visible: totalPhotos > 0">
                                            <span class="bwmsprite gallery-photo-icon"></span>
                                            <span class="model-media-count" data-bind="text: totalPhotos"></span>
                                        </div>
                                    </div>
                                </a>
                            </div>
                            <div class="margin-right20 margin-left20 padding-top10 font14">
                                <h2 class="margin-bottom10"><a data-bind="text: bikeName, attr: { 'href': '/m/used/bikes-in-' + cityMasking + '/' + makeMasking + '-' + modelMasking + '-' + profileId + '/' }"></a></h2>
                                <div class="grid-6 alpha omega margin-bottom5" data-bind="visible : modelYear > 0">
                                    <span class="bwmsprite model-date-icon"></span>
                                    <span class="model-details-label" data-bind="text: modelYear + ' model'"></span>
                                </div>
                                <div class="grid-6 alpha omega margin-bottom5" data-bind="visible: kmsDriven > 0">
                                    <span class="bwmsprite kms-driven-icon"></span>
                                    <span class="model-details-label" ><span data-bind="CurrencyText: kmsDriven"></span> kms</span>
                                </div>
                                <div class="grid-6 alpha omega margin-bottom5" data-bind="visible: noOfOwners!=null">
                                    <span class="bwmsprite author-grey-sm-icon"></span>
                                    <span class="model-details-label"><span data-bind="NumberOrdinal: noOfOwners"></span> Owner</span>
                                </div>
                                <div class="grid-6 alpha omega margin-bottom5" data-bind="visible: city!=''">
                                    <span class="bwmsprite model-loc-icon"></span>
                                    <span class="model-details-label" data-bind="text: city"></span>
                                </div>
                                <div class="clear"></div>
                                <p class="margin-bottom15" data-bind="visible : askingPrice == 0" ><span class="bwmsprite inr-md-icon"></span>&nbsp;<span class="font22 text-bold" >N/A</span></p>
                                <p class="margin-bottom15" data-bind="visible : askingPrice > 0" ><span class="bwmsprite inr-md-icon"></span>&nbsp;<span class="font22 text-bold" data-bind="    CurrencyText:askingPrice"></span></p>
                                <%--<a href="javascript:void(0)" class="btn btn-orange seller-details-btn" rel="nofollow">Get seller details</a>--%>
                            </div>
                        </li>
                    </ul>
                    <div style="text-align: center;">
                        <div id="nobike"  data-bind="visible : noBikes()">
                            <img src="/images/no_result_m.png">
                        </div>
                    </div>                     
                    <div class="margin-right10 margin-left10 padding-top15 padding-bottom15 border-solid-top font14">
                        <div class="grid-5 omega text-light-grey" data-bind="visible: TotalBikes() > 0">
                    <div class="font14 " style="display:none" data-bind="visible: !OnInit() && TotalBikes() > 0"><span class="text-bold" data-bind="    CurrencyText: (Pagination().pageNumber() - 1) * Pagination().pageSize() + 1"></span>-<span class="text-bold" data-bind="    CurrencyText: Math.min(TotalBikes(), Pagination().pageNumber() * Pagination().pageSize())"></span> of <span class="text-bold" data-bind="    CurrencyText: TotalBikes()"></span> bikes</div>
                    <% if(totalListing >0){ %>
                            <div data-bind="visible: OnInit()" class="font14 "><span class="text-bold"><%=_startIndex %>-<%=_endIndex %></span> of <span class="text-bold"><%=Bikewale.Utility.Format.FormatPrice(strTotal) %></span>  bikes</div>
                            <% } %>
                    </div>
                        <div data-bind="visible: OnInit()">
                            <BikeWale:Pager ID="ctrlPager" runat="server" />
                        </div>
                    <div data-bind="visible: !OnInit() && Pagination().paginated() > 0">
                        <div class="grid-7 alpha omega position-rel">
                            <ul id="pagination-list" data-bind="html: PagesListHtml"></ul>
                            <span class="pagination-control-prev" data-bind="html: PrevPageHtml, css: Pagination().hasPrevious() ? 'active' : 'inactive' "></span>
                            <span class="pagination-control-next" data-bind="html: NextPageHtml, css: Pagination().hasNext() ? 'active' : 'inactive'"></span>
                        </div>
                    </div>
                    <div class="clear"></div>
                </div>
                </div>
            </section>

            <div id="sort-filters-loader">
                <div id="popup-loader"></div>
            </div>

                <div id="sortNFilters" >
            <!-- sort popup start -->
            <div id="sort-by-container" class="sort-popup-container">
                <div class="popup-header">Sort</div>
                <div class="popup-body">
                    <ul id="sort-by-list" class="margin-bottom25" >
                        <li data-sortorder="1">
                            <span class="bwmsprite radio-uncheck"></span>
                            <span class="sort-list-label" >Most recent</span>
                        </li>
                         <li data-sortorder="2">
                            <span class="bwmsprite radio-uncheck"></span>
                            <span class="sort-list-label" >Price - Low to High</span>
                        </li>  
                         <li data-sortorder="3">
                            <span class="bwmsprite radio-uncheck"></span>
                            <span class="sort-list-label" >Price - High to Low</span>
                        </li>  
                         <li data-sortorder="4">
                            <span class="bwmsprite radio-uncheck"></span>
                            <span class="sort-list-label" >Kms - Low to High</span>
                        </li> 
                         <li data-sortorder="5">
                            <span class="bwmsprite radio-uncheck"></span>
                            <span class="sort-list-label" >Kms - High to Low</span>
                        </li>                          
                    </ul>
                    <div class="grid-6 alpha">
                        <p id="cancel-sort-by" class="btn btn-white btn-full-width btn-size-0">Cancel</p>
                    </div>
                    <div class="grid-6 omega">
                        <p id="apply-sort-by" class="btn btn-orange btn-full-width btn-size-0" data-bind="click:applySort">Apply</p>
                    </div>
                    <div class="clear"></div>
                </div>
            </div>
            <!-- sort popup end -->

            <!-- filter popup start -->
            <div id="filter-container" class="filter-popup-container">
                <div id="filter-container-header" class="ui-corner-top">
                    <div id="close-filter" class="filter-back-arrow leftfloat">
                        <span class="bwmsprite fa-angle-left"></span>
                    </div>
                    <div class="filter-popup-label leftfloat">Filters</div>
                    <div class="clear"></div>
                </div>
                <div id="filter-selection-list">
                    <div id="filter-type-city" class="margin-bottom25">
                        <p class="filter-option-key">City</p>
                        <div class="filter-option-value">
                            <p class="selected-filters" data-bind="text : SelectedCity().name"></p>
                            <span class="bwmsprite grey-right-icon"></span>
                        </div>
                    </div>
                    <div id="filter-type-bike" class="margin-bottom25">
                        <p class="filter-option-key">Bike</p>
                        <div class="filter-option-value">
                            <p class="selected-filters"></p>
                            <span class="bwmsprite grey-right-icon"></span>
                        </div>
                    </div>
                    <div class="margin-bottom35">
                        <p class="filter-option-key leftfloat">Budget</p>
                        <p id="budget-amount" class="font14 text-bold rightfloat"></p>
                        <div class="clear"></div>
                        <div  data-bind="KOSlider: BudgetValues, sliderOptions: { range: true, values: [0, 7], min: 0, max: 7, step: 1 }"></div>
                    </div>
                    <div class="margin-bottom35">
                        <p class="filter-option-key leftfloat">Kms ridden</p>
                        <p  class="font14 text-bold rightfloat" data-bind="visible : KmsDriven() > 0">0 - <span data-bind="    CurrencyText: KmsDriven()"></span><span data-bind="    text : KmsDriven() == 200000 ? '+ Kms':' Kms' "></span></p>
                        <div class="clear"></div>
                        <div  data-bind="KOSlider: KmsDriven, sliderOptions: {range: 'min',value: 80000,min: 5000,max: 200000,step: 5000}"></div>
                    </div>
                    <div class="margin-bottom35">
                        <p class="filter-option-key leftfloat">Bike age</p>
                        <p  class="font14 text-bold rightfloat" data-bind="visible: BikeAge() > 0" >0 - <span data-bind="    text: BikeAge() "></span><span data-bind="    text : BikeAge() == 8 ? '+ years':' years' "></span></p>
                        <div class="clear"></div>
                        <div id="bike-age-slider" data-bind="KOSlider: BikeAge, sliderOptions: { range: 'min', value: 8, min: 1, max: 8, step: 1 }"></div>
                    </div>
                    <div class="margin-bottom25">
                        <p class="filter-option-key margin-bottom10">Previous owners</p>
                        <ul id="previous-owners-list">
                            <li data-ownerid="1">
                                <span>1</span>
                            </li>
                            <li  data-ownerid="2">
                                <span>2</span>
                            </li>
                            <li  data-ownerid="3">
                                <span>3</span>
                            </li>
                            <li  data-ownerid="4">
                                <span>4</span>
                            </li>
                            <li  data-ownerid="5">
                                <span class="prev-owner-last-item">4 +</span>
                            </li>
                        </ul>
                    </div>
                    <div id="sellerTypes">
                        <p class="filter-option-key margin-bottom10">Seller type</p>
                        <div class="filter-type-seller grid-6 unchecked padding-left25" data-sellerid="2">Individual</div>
                        <div class="filter-type-seller grid-6 unchecked padding-left25" data-sellerid="1">Dealer</div>
                        <div class="clear"></div>
                    </div>
                </div>

                <div id="filter-container-footer" class="filter-container-footer">
                    <div class="grid-6">
                        <p id="reset-filters" class="btn btn-white btn-full-width btn-size-0" data-bind="click: function (d, e) { SetDefaultFilters(); IsReset(true);}" >Reset</p>
                    </div>
                    <div class="grid-6">
                        <p id="apply-filters" class="btn btn-orange btn-full-width btn-size-0" data-bind="click: ApplyFilters">Apply filters</p>
                    </div>
                    <div class="clear"></div>
                </div>

                <!-- city popup start -->
                <div id="filter-city-container" class="filter-popup-container bwm-city-area-box">
                    <div class="form-control-box text-left">
                        <div class="filter-input-box">
                            <span id="close-city-filter" class="back-arrow-box">
                                <span class="bwmsprite back-long-arrow-left"></span>
                            </span>
                            <input type="text" class="form-control padding-right40" placeholder="Type to select city" id="popupCityInput" data-bind="textInput: Cities().cityFilter" autocomplete="off">
                        </div>
                    
                        <ul id="filter-city-list" >
                             <li data-cityid="0" data-bind="click: FilterCity">All India</li>
                          
                            <%foreach(var city in cities) {%>
                            <li data-cityid="<%= city.CityId %>" data-bind="click : $root.FilterCity"><%=city.CityName %></li>
                            <%} %>
                          
                        </ul>                    

                        <div class="margin-top30 font24 text-center margin-top60 "></div>
                    </div>
                </div>
                <!-- city popup end -->

                <!-- bike popup start -->
                <div id="filter-bike-container" class="filter-popup-container">
                    <div class="ui-corner-top">
                        <div id="close-bike-filter" class="filter-back-arrow leftfloat">
                            <span class="bwmsprite fa-angle-left"></span>
                        </div>
                        <div class="filter-popup-label leftfloat">Select bikes</div>
                        <div class="clear"></div>
                    </div>
                    <div class="filter-bike-banner"></div>
                    <ul id="filter-bike-list">
                    <%foreach (var make in makeModels)
                      {%>
                        <li>
                            <div class="accordion-tab">

                                <div class="accordion-checkbox leftfloat">
                                <span data-makeid="<%=make.Make.MakeId %>" class="bwmsprite unchecked-box"></span>
                                </div>

                            <div class="accordion-label-tab leftfloat">
                                <span class="accordion-label"><%=make.Make.MakeName %></span>
                                    <span class="accordion-count"></span>
                                    <span class="bwmsprite arrow-down"></span>
                                </div>
                                <div class="clear"></div>
                            </div>
                            <ul class="bike-model-list">
                            <%foreach (var model in make.Models)
                              {%>
                                <li>
                                <span data-modelid="<%=model.ModelId %>"  class="bwmsprite unchecked-box"></span>
                                <span class="bike-model-label"><%=model.ModelName %></span>
                                </li>
                            <%} %>
                            </ul>
                        </li>
                    <%} %>
                    </ul>
                    <div id="filter-bike-container-footer" class="filter-container-footer">
                        <div class="grid-6 alpha">
                            <p id="reset-bikes-filter" class="btn btn-white btn-full-width btn-size-0">Reset</p>
                        </div>
                        <div class="grid-6 omega">
                            <p id="set-bikes-filter" class="btn btn-orange btn-full-width btn-size-0">Done</p>
                        </div>
                        <div class="clear"></div>
                    </div>
                </div>
                <!-- bike popup start -->

            </div>
            <!-- filter popup end -->
            </div>
        </div> 
        <div class="back-to-top" id="back-to-top"></div>
        <script type="text/javascript" src="<%= staticUrl != "" ? "http://st1.aeplcdn.com" + staticUrl : "" %>/m/src/frameworks.js?<%= staticFileVersion %>"></script>
        <!-- #include file="/includes/footerBW_Mobile.aspx" -->        
        <link href="<%= staticUrl != "" ? "http://st2.aeplcdn.com" + staticUrl : "" %>/m/css/bwm-common-btf.css?<%= staticFileVersion %>" rel="stylesheet" type="text/css" />
        <script type="text/javascript" src="<%= staticUrl != "" ? "http://st2.aeplcdn.com" + staticUrl : "" %>/m/src/common.min.js?<%= staticFileVersion %>"></script>
        <link href='https://fonts.googleapis.com/css?family=Open+Sans:400,700' rel='stylesheet' type='text/css' />
        <script type="text/javascript">
            var OnInitTotalBikes = <%= totalListing %>; 
            var selectedCityId = <%= cityId %>;selectedMakeId = "<%= makeId %>",selectedModelId = "<%= modelId %>";
        </script>
        <script type="text/javascript" src="<%= staticUrl != "" ? "http://st2.aeplcdn.com" + staticUrl : "" %>/m/src/used-search.js?<%= staticFileVersion %>"></script>
    </form>
</body>
</html>
