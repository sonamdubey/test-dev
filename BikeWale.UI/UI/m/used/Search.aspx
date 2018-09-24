<%@ Page Language="C#" AutoEventWireup="false" EnableViewState="false" Inherits="Bikewale.Mobile.Used.Search" %>
<%@ Register TagPrefix="BikeWale" TagName="Pager" Src="/UI/m/controls/LinkPagerControl.ascx" %>
<%@ Register Src="~/UI/m/controls/UsedBikeLeadCaptureControl.ascx" TagPrefix="BW" TagName="UBLeadCapturePopup" %>
<%@ Register Src="~/UI/m/controls/UsedBikesCityCountByBrand.ascx" TagPrefix="BW" TagName="UBCCountByMake" %>
<%@ Register Src="~/UI/m/controls/UsedBikeByModels.ascx" TagPrefix="BW" TagName="UsedBikeByModels" %>
<%@ Register Src="~/UI/m/controls/UsedBikeModelByCity.ascx" TagPrefix="BW" TagName="UsedBikeModelByCity" %>
<%@ Register Src="~/UI/m/controls/UsedBikesCityCountByModel.ascx" TagPrefix="BW" TagName="UBCCountByModel" %>

<!DOCTYPE html>
<html>
<head>
        <%
            title = pageTitle;
            description = pageDescription;
            canonical = pageCanonical.Replace("/m/", "/");
            keywords = pageKeywords;
            EnableOG = true;
            relPrevPageUrl = prevUrl.Replace("/m/", "/");
            relNextPageUrl = nextUrl.Replace("/m/", "/");
            AdPath = "/1017752/BikeWale_Mobile_UsedBikes_Search_Results_";
            AdId = "1475576914369";
            Ad_320x50 = true;
            Ad_Bot_320x50 = true;
            ShowSellBikeLink = true;

        %>

    <!-- #include file="/UI/includes/headscript_mobile_min.aspx" -->
    <link rel="stylesheet" type="text/css" href="/UI/m/css/used/search.css" />
    <script type="text/javascript">
        <!-- #include file="\UI\includes\gacode_mobile.aspx" -->
    </script>

</head>
<body>
    <form id="form1" runat="server">
        <!-- #include file="/UI/includes/headBW_Mobile.aspx" -->
         <div class="modal-background"></div>
         <div id="usedBikesSection">
            <section>
                <div class="container bg-white clearfix">
                    <h1 class="padding-top15 padding-right20 padding-bottom15 padding-left20 box-shadow"><%= heading %></h1> 
                    <%if (ctrlUsedBikeByModels.FetchCount > 0 || ctrlUsedBikeModelByCity.FetchCount > 0 || ctrlUsedBikesCityCountByMake.FetchedCount > 0 || ctrlUsedBikesCityCountByModel.FetchedCount > 0)
                        { %>
                    <div id="city-model-used-carousel" >                        
                        <h2 class="carousel-heading font14 text-default padding-left20 margin-bottom10">Refine your search further!</h2>
                        <span id="close-city-model-carousel" class="bwmsprite cross-md-dark-grey cur-pointer"></span>
                        <%if (ctrlUsedBikeByModels.FetchCount > 0)
                            { %>
                                  <BW:UsedBikeByModels ID="ctrlUsedBikeByModels" runat="server" />
                        <% }
                            else if (ctrlUsedBikesCityCountByMake.FetchedCount > 0)
                            { %>                        
                        <BW:UBCCountByMake runat="server" ID="ctrlUsedBikesCityCountByMake"></BW:UBCCountByMake>  
                          
                       <%}
                           else if (ctrlUsedBikeModelByCity.FetchCount > 0)
                           {%>
                                  <BW:UsedBikeModelByCity ID="ctrlUsedBikeModelByCity" runat="server" />
                        <% }
                            else if (ctrlUsedBikesCityCountByModel.FetchedCount > 0)
                            { %>    
                        <BW:UBCCountByModel runat="server" ID="ctrlUsedBikesCityCountByModel"></BW:UBCCountByModel> 
                        <%}%>
                      
                    </div>
                    <%} %>
                    <div class="font14 padding-top10 padding-right20 padding-bottom10 padding-left20" style="display:none" data-bind="visible: !OnInit() && TotalBikes() > 0">Showing <span class="text-bold"><span data-bind="    CurrencyText: (Pagination().pageNumber() - 1) * Pagination().pageSize() + 1"></span>-<span data-bind="    CurrencyText: Math.min(TotalBikes(), Pagination().pageNumber() * Pagination().pageSize())""></span> of <span class="text-bold" data-bind="    CurrencyText: TotalBikes()"></span> bikes</div>
                <% if (totalListing > 0)
                    { %>
                    <div data-bind="visible: OnInit()" class="font14 padding-top10 padding-right20 padding-bottom10 padding-left20">Showing <span class="text-bold"><%=_startIndex %>-<%=_endIndex %></span> of <span class="text-bold"><%= Bikewale.Utility.Format.FormatPrice(totalListing.ToString()) %></span> bikes</div>
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

                        <%if (usedBikesList != null)
                            {
                                foreach (var bike in usedBikesList)
                                {
                                    string curBikeName = string.Format("{0} {1} {2}", bike.MakeName, bike.ModelName, bike.VersionName);  %>
                                <li >
                                    <div class="model-thumbnail-image">
                                    <a href="<%= string.Format("/m/used/bikes-in-{0}/{1}-{2}-{3}/", bike.CityMaskingName, bike.MakeMaskingName, bike.ModelMaskingName, bike.ProfileId) %>" class="model-image-target">
                                        <%  if (!(String.IsNullOrEmpty(bike.Photo.OriginalImagePath) || String.IsNullOrEmpty(bike.Photo.HostUrl)))
                                            { %>    
                                        <img class="lazy" data-original="<%= Bikewale.Utility.Image.GetPathToShowImages(bike.Photo.OriginalImagePath, bike.Photo.HostUrl, Bikewale.Utility.ImageSize._310x174) %>" 
                                                 alt="<%= curBikeName %>" title="<%= curBikeName %>" border="0" src="" />
                                        <% }
                                            else
                                            { %>
                                        <div class="bg-light-grey">
                                        <span class="bwmsprite no-image-icon margin-bottom15"></span>
                                        <p class="font12 text-bold text-light-grey">Image not available</p>
                                    </div>
                                        <% } %>
                                         <% if (bike.TotalPhotos > 0)
                                             { %>
                                            <div class="model-media-details ">
                                                <div class="model-media-item">
                                                    <span class="bwmsprite photos-sm"></span>
                                                    <span class="model-media-count" ><%= bike.TotalPhotos %></span>
                                                </div>
                                            </div>
                                         <% } %>
                                        </a>
                                    </div>
                                    <div class="margin-right20 margin-left20 padding-top10 font14">
                                        <h2 class="margin-bottom5 text-truncate">
                                        <a href="<%= string.Format("/m/used/bikes-in-{0}/{1}-{2}-{3}/", bike.CityMaskingName, bike.MakeMaskingName, bike.ModelMaskingName, bike.ProfileId) %>" title="<%= string.Format("{0}, {1}", bike.ModelYear, curBikeName) %>">
                                                <%= string.Format("{0}, {1}", !string.IsNullOrEmpty(bike.ModelYear) ? bike.ModelYear : "", curBikeName) %>
                                            </a>
                                        </h2>
                                        <div class="margin-bottom5">
                                            <span class="font12 text-x-light">Updated on: <%= bike.LastUpdated.ToString("dd MMMM yyyy") %></span>
                                        </div>
                                        <%if (!string.IsNullOrEmpty(bike.ModelYear))
                                            { %>
                                        <div class="grid-6 alpha omega margin-bottom5 ">
                                            <span class="bwmsprite model-date-icon"></span>
                                            <span class="model-details-label"><%= bike.ModelYear %> model</span>
                                        </div>
                                         <% } %>
                                             <%if (bike.KmsDriven > 0)
                                                 { %>
                                        <div class="grid-6 alpha omega margin-bottom5 ">
                                            <span class="bwmsprite kms-driven-icon"></span>
                                            <span class="model-details-label"><%= Bikewale.Utility.Format.FormatPrice(bike.KmsDriven.ToString()) %> kms</span>
                                        </div>
                                        <% } %>
                                             <%if (bike.NoOfOwners > 0)
                                                 { %>
                                        <div class="grid-6 alpha omega margin-bottom5 ">
                                            <span class="bwmsprite author-grey-sm-icon"></span>
                                            <span class="model-details-label"><%= Bikewale.Utility.Format.AddNumberOrdinal(bike.NoOfOwners) %> owner</span>
                                        </div>
                                        <% } %>
                                             <%if (!string.IsNullOrEmpty(bike.CityName))
                                                 { %>
                                        <div class="grid-6 alpha omega margin-bottom5">
                                            <span class="bwmsprite model-loc-icon"></span>
                                            <span class="model-details-label"><%= bike.CityName %></span>
                                        </div>
                                         <% } %>
                                        <div class="clear"></div>
                                        <p class="margin-bottom15"><span class="bwmsprite inr-md-icon"></span>&nbsp;<span class="font22 text-bold"><%= Bikewale.Utility.Format.FormatPrice(bike.AskingPrice.ToString()) %></span></p>
                                    <a href="javascript:void(0)" class="btn btn-orange seller-details-btn used-bike-lead" data-ga-cat="Used_Bike_Listing" data-ga-act="Get_Seller_Details_Clicked" data-profile-id="<%= bike.ProfileId  %>" data-ga-lab="<%= bike.ProfileId %>" rel="nofollow">Get seller details</a>
                                    </div>
                                </li>
                             <% }
                            }  %>
                    </ul>

                    <ul id="used-bikes-list" style="display:none" data-bind="visible: !OnInit() && !noBikes() ,foreach : BikeDetails()">
                        <li>
                            <div class="model-thumbnail-image">
                                <a data-bind=" attr: { 'href': '/m/used/bikes-in-' + cityMasking + '/' + makeMasking + '-' + modelMasking + '-' + profileId + '/' }" class="model-image-target">
                                    <!-- ko if: photo.originalImagePath != '' -->
                                    <img data-bind="attr: { alt: bikeName, title: bikeName, src: '' }, lazyload: photo.hostUrl + '/370x208/' + photo.originalImagePath" src="" alt="" title="" border="0" />
                                    <!-- /ko -->
                                    <!-- ko if: photo.originalImagePath == '' -->
                                    <div class="bg-light-grey">
                                        <span class="bwmsprite no-image-icon margin-bottom15"></span>
                                        <p class="font12 text-bold text-light-grey">Image not available</p>
                                    </div>
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
                                <h2 class="margin-bottom5 text-truncate"><a data-bind="text: (modelYear > 0?modelYear+', ':'') + bikeName, attr: { 'href': '/m/used/bikes-in-' + cityMasking + '/' + makeMasking + '-' + modelMasking + '-' + profileId + '/' }"></a></h2>
                                <div class="margin-bottom5">
                                    <span class="font12 text-x-light" data-bind="text: 'Updated on: ' + strLastUpdated"></span>
                                </div>
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
                                <a href="javascript:void(0)" class="btn btn-orange seller-details-btn used-bike-lead" rel="nofollow" data-ga-cat="Used_Bike_Listing" data-ga-act="Get_Seller_Details_Clicked" data-bind="attr: { 'data-ga-lab': profileId, 'data-profile-id': profileId }">Get seller details</a>
                            </div>
                        </li>
                    </ul>
                    <div style="text-align: center;">
                        <div id="nobike"  data-bind="visible : noBikes()">
                            <img src="https://imgd.aeplcdn.com/0x0/bw/static/design15/no-result-m.png" alt="No match found">
                        </div>
                    </div>                     
                    <div class="margin-right10 margin-left10 padding-top15 padding-bottom15 border-solid-top font14">
                        <div class="grid-5 omega text-light-grey" data-bind="visible: TotalBikes() > 0">
                    <div class="font14" data-bind="visible: !OnInit() && TotalBikes() > 0"><span class="text-bold" data-bind="    CurrencyText: (Pagination().pageNumber() - 1) * Pagination().pageSize() + 1"></span>-<span class="text-bold" data-bind="    CurrencyText: Math.min(TotalBikes(), Pagination().pageNumber() * Pagination().pageSize())"></span> of <span class="text-bold" data-bind="    CurrencyText: TotalBikes()"></span> bikes</div>
                    <% if (totalListing > 0)
                        { %>
                            <div data-bind="visible: OnInit()" class="font14"><span class="text-bold"><%=_startIndex %>-<%=_endIndex %></span> of <span class="text-bold"><%=Bikewale.Utility.Format.FormatPrice(totalListing.ToString()) %></span>  bikes</div>
                    <% } %>
                    </div>
                        <div data-bind="visible: OnInit()" class="init-pagination">
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

             <script type="text/javascript" src="<%= staticUrl  %>/UI/m/src/frameworks.js?<%= staticFileVersion %>"></script>

            <div id="sort-filters-loader">
                <div class="cover-popup-loader"></div>
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
                             <li data-cityid="0" data-bind="click: FilterCity" data-citymasking="india">All India</li>
                            <% if (citiesList != null)
                                { %>
                            <%foreach (var city in citiesList)
                                {%>
                            <li data-cityid="<%= city.CityId %>" data-citymasking="<%= city.CityMaskingName %>" data-bind="click : $root.FilterCity"><%=city.CityName %></li>
                            <%}
                                } %>
                          
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
                    <% if (makeModelsList != null)
                        {
                            foreach (var make in makeModelsList)
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
                            <%if (make.Models != null)
                                {
                                    foreach (var model in make.Models)
                                    {%>
                                <li>
                                <span data-modelid="<%=model.ModelId %>"  class="bwmsprite unchecked-box"></span>
                                <span class="bike-model-label"><%=model.ModelName %></span>
                                </li>
                            <%}
                                } %>
                            </ul>
                        </li>
                    <%}
                        } %>
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

        <section>
            <div class="breadcrumb">
                <span class="breadcrumb-title">You are here:</span>
                <ul>

                     <li itemscope="" itemtype="http://data-vocabulary.org/Breadcrumb">
                         <a class="breadcrumb-link"  href="/m/" itemprop="url">
                                    <span class="breadcrumb-link__label" itemprop="name" title="Home">Home</span>
                         </a>
                     </li>
                    <li itemtype="http://data-vocabulary.org/Breadcrumb">
                                    
                        <a class="breadcrumb-link"  href="/m/used/" itemprop="url"><span class="breadcrumb-link__label" itemprop="name" title="Used Bikes">Used Bikes</span>
                        </a>
                    </li>
                    <% if (makeId > 0)
                        { %>
                    <li itemtype="http://data-vocabulary.org/Breadcrumb">
                        <a class="breadcrumb-link"  href="/m/used/bikes-in-<%= objCity!=null ? objCity.CityMaskingName : "india" %>/" itemprop="url"><span class="breadcrumb-link__label" itemprop="name" title="<%= cityName %>"><%= cityName %></span></a>
                    </li> 
                        <% if (objMake != null && modelId > 0)
                            { %>
                            <li itemtype="http://data-vocabulary.org/Breadcrumb">
                        <a class="breadcrumb-link"  href="/m/used/<%= objMake.MaskingName %>-bikes-in-<%= objCity!=null ? objCity.CityMaskingName : "india" %>/" itemprop="url" title="<%= string.Format("{0} Bikes",objMake.MakeName) %>"><span class="breadcrumb-link__label" itemprop="name"><%= string.Format("Used {0} Bikes",objMake.MakeName) %></span></a>
                    </li> 
                    <% }
                        } %>
                    <% if (!string.IsNullOrEmpty(heading))
                        { %>
                    <li>
                        <span><%= heading %></span>
                    </li>
                    <%} %>
                </ul>
                <div class="clear"></div>
            </div>
            <div class="clear"></div>
        </section>

        <div class="back-to-top" id="back-to-top"></div>
        <BW:UBLeadCapturePopup runat="server" ID="ctrlUBLeadCapturePopup"></BW:UBLeadCapturePopup>
        <script type="text/javascript">
            var bodyHeight = $('body').height();
        </script>
        <!-- #include file="/UI/includes/footerBW_Mobile.aspx" -->        
        <link href="<%= staticUrl  %>/UI/m/css/bwm-common-btf.css?<%= staticFileVersion %>" rel="stylesheet" type="text/css" />
        <!-- #include file="/UI/includes/footerscript_mobile.aspx" -->
        <script type="text/javascript">
            var OnInitTotalBikes = <%= totalListing %>; 
            var pageQS = "<%= currentQueryString %>";
            var selectedCityId = <%= cityId %>;selectedMakeId = "<%= makeId %>",selectedModelId = "<%= modelId %>";
            var usedPageIdentifier="<%=PageIdentifier%>";
            var selectedCityId = "<%=cityId%>";
            var gaObj = { 'id': '<%= (int)Bikewale.Entities.Pages.GAPages.Used_Bike_Listing%>', 'name': '<%= Bikewale.Entities.Pages.GAPages.Used_Bike_Listing%>' };
        </script>
        <script type="text/javascript" src="<%= staticUrl %>/UI/m/src/used-search.js?<%= staticFileVersion%>"></script>        
        <!-- #include file="/UI/includes/fontBW_Mobile.aspx" -->
    </form>
</body>
</html>
