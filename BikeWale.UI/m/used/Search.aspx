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

    <!-- #include file="/includes/headscript_mobile.aspx" -->
    <link href="<%= staticUrl != "" ? "http://st2.aeplcdn.com" + staticUrl : "" %>/m/css/used-search-atf.css?<%= staticFileVersion %>" rel="stylesheet" type="text/css" />

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
                            <span class="font14 text-bold" data-bind="click : function(d,e){ QueryString();SetPageFilters();}">Filter</span>
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

                                        <div class="grid-6 omega margin-bottom5 <%# DataBinder.Eval(Container.DataItem, "KmsDriven").ToString() == "0"? "hide" : string.Empty %>">
                                            <span class="bwmsprite kms-driven-icon"></span>
                                            <span class="model-details-label"><%# Bikewale.Utility.Format.FormatNumeric(DataBinder.Eval(Container.DataItem, "KmsDriven").ToString()) %> kms</span>
                                        </div>
                                        <div class="grid-6 alpha omega margin-bottom5<%# string.IsNullOrEmpty(Convert.ToString(DataBinder.Eval(Container.DataItem, "NoOfOwners")))? "hide" : string.Empty %>">
                                            <span class="bwmsprite author-grey-sm-icon"></span>
                                            <span class="model-details-label"><%# DataBinder.Eval(Container.DataItem, "NoOfOwners").ToString() %> owner</span>
                                        </div>
                                        <div class="grid-6 omega margin-bottom5 <%# string.IsNullOrEmpty(Convert.ToString(DataBinder.Eval(Container.DataItem, "CityName")))? "hide" : string.Empty %>">
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
                    <ul id="used-bikes-list" style="display:none" data-bind="visible: !OnInit() ,foreach : BikeDetails()">
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
                        <div id="nobike"  data-bind="visible : !OnInit() || noBikes()">
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
                    <div data-bind="visible: !OnInit()">
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
                         <li data-sortorder="4">
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
                        <p  class="font14 text-bold rightfloat" data-bind="visible : KmsDriven() > 0">0 - <span data-bind="    CurrencyText: KmsDriven() + ' Kms'"></span></p>
                        <div class="clear"></div>
                        <div  data-bind="KOSlider: KmsDriven, sliderOptions: {range: 'min',value: 80000,min: 5000,max: 80000,step: 5000}"></div>
                    </div>
                    <div class="margin-bottom35">
                        <p class="filter-option-key leftfloat">Bike age</p>
                        <p  class="font14 text-bold rightfloat" data-bind="visible: BikeAge() > 0" >0 - <span data-bind="text: BikeAge() + ' years'"></span></p>
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
                            <input type="text" class="form-control padding-right40" placeholder="Type to select city" id="popupCityInput" autocomplete="off">
                        </div>
                    
                        <ul id="filter-city-list">
                             <li data-cityid="0" data-bind="click: FilterCity">All India</li>
                        <%foreach(var city in cities) {%>
                        <li data-cityid="<%= city.CityId %>" data-bind="click : FilterCity"><%=city.CityName %></li>
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

        <!-- #include file="/includes/footerBW_Mobile.aspx" -->
        <!-- #include file="/includes/footerscript_Mobile.aspx" -->
        <link href="<%= staticUrl != "" ? "http://st2.aeplcdn.com" + staticUrl : "" %>/m/css/used-search-btf.css?<%= staticFileVersion %>" rel="stylesheet" type="text/css" />
        <script type="text/javascript">
                    var OnInitTotalBikes = <%= totalListing %>; 
                    var selectedCityId = <%= cityId %>; 
        </script>
        <script type="text/javascript" src="<%= staticUrl != "" ? "http://st2.aeplcdn.com" + staticUrl : "" %>/m/src/used-search.js?<%= staticFileVersion %>"></script>
    </form>
</body>
</html>
