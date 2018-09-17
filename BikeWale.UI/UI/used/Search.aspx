<%@ Page Language="C#" AutoEventWireup="false"  Inherits="Bikewale.Used.Search" EnableViewState="false" %>
<%@ Register TagPrefix="BikeWale" TagName="Pager" Src="~/UI/m/controls/LinkPagerControl.ascx" %>
<%@ Register Src="/UI/controls/UsedBikeLeadCaptureControl.ascx" TagPrefix="BW" TagName="UBLeadCapturePopup" %>
<%@ Register Src="/UI/controls/UsedBikesCityCountByBrand.ascx" TagPrefix="BW" TagName="UBCCountByMake" %>
<%@ Register Src="/UI/controls/UsedBikeByModels.ascx" TagPrefix="BW" TagName="UsedBikeByModels" %>
<%@ Register Src="/UI/controls/UsedBikeModelByCity.ascx" TagPrefix="BW" TagName="UsedBikeModelByCity" %>
<%@ Register Src="/UI/controls/UsedBikesCityCountByModel.ascx" TagPrefix="BW" TagName="UBCCountByModel" %>

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
    <!-- #include file="/UI/includes/headscript_desktop_min.aspx" -->
    <link rel="stylesheet" type="text/css" href="/UI/css/used/search.css" />
    <script type="text/javascript">
        <!-- #include file="\UI\includes\gacode_desktop.aspx" -->
    </script>
</head>
<body class="bg-light-grey header-fixed-inner">
    <form id="form1" runat="server">
        <!-- #include file="/UI/includes/headBW.aspx" -->

        <div id="usedBikesSection">

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
                                 <% if (objMake != null && modelId > 0)
                                    { %>
                                       <li itemtype="http://data-vocabulary.org/Breadcrumb"><span class="bwsprite fa-angle-right margin-right10"></span>
                                  <a href="/used/<%= objMake.MaskingName %>-bikes-in-<%= objCity!=null ? objCity.CityMaskingName : "india" %>/" itemprop="url"><span><%= string.Format("Used {0} Bikes",objMake.MakeName) %></span></a>
                                </li> 
                                <% }
                                   } %>
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
                                <div class="alpha">
                                    <div class="grid-9">
                                        <h1 class="font24 text-x-black"><%= heading %></h1>
                                    </div>
                                    <div class="grid-3">
                                        <a target="_blank" rel="noopener" class="btn btn-teal assistance-submit-btn rightfloat" href="/used/sell/" title="Sell your bike">Sell your bike</a>
                                    </div>
                                </div>
                                <div class="clear"></div>
                            </div>
                            <% if (ctrlUsedBikeByModels.FetchCount > 0 || ctrlUsedBikeModelByCity.FetchCount > 0 || ctrlUsedBikesCityCountByMake.FetchedCount > 0 || ctrlUsedBikesCityCountByModel.FetchedCount > 0)
                               { %>                                                           
                            <div id="city-model-used-carousel" >                               
                                <h2 class="font14 text-default padding-left15 margin-bottom20">Refine your search further!</h2>
                                <span id="close-city-model-carousel" class="bwsprite cross-md-dark-grey cur-pointer"></span>  
                                <%if (ctrlUsedBikeByModels.FetchCount > 0)
                                  { %>   
                                        <BW:UsedBikeByModels ID="ctrlUsedBikeByModels" runat="server" />
                                 <%}
                                  else if (ctrlUsedBikesCityCountByMake.FetchedCount > 0)
                                  { %>
                                <BW:UBCCountByMake runat="server" ID="ctrlUsedBikesCityCountByMake"></BW:UBCCountByMake>        
                            <% }
                                  else if (ctrlUsedBikesCityCountByModel.FetchedCount > 0)
                                  { %>    
                                    <BW:UBCCountByModel runat="server" ID="ctrlUsedBikesCityCountByModel"></BW:UBCCountByModel> 
                                <%} %>
                                <%else if (ctrlUsedBikeModelByCity.FetchCount > 0)
                                  {%>
                                        <BW:UsedBikeModelByCity ID="ctrlUsedBikeModelByCity" runat="server" />
                               <%}%>                                  
                            </div>
                            <% } %>  
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
                                        <p class="grid-7 padding-top5" style="display:none" data-bind="visible: !OnInit() && TotalBikes() > 0">Showing <span class="text-default text-bold"><span data-bind="    CurrencyText: (Pagination().pageNumber() - 1) * Pagination().pageSize() + 1"><%=_startIndex %></span>-<span data-bind="    CurrencyText: Math.min(TotalBikes(), Pagination().pageNumber() * Pagination().pageSize())""><%=_endIndex %></span></span> of <span class="text-default text-bold" data-bind="    CurrencyText: TotalBikes()"><%= Bikewale.Utility.Format.FormatPrice(totalListing.ToString()) %></span> bikes</p>
                                        <% if (totalListing > 0)
                                           { %>
                                            <p class="grid-7 padding-top5" data-bind="visible: OnInit()">Showing <span class="text-default text-bold"><%=_startIndex %>-<%=_endIndex %></span> of <span class="text-default text-bold" data-bind="    CurrencyText: TotalBikes()"><%= Bikewale.Utility.Format.FormatPrice(totalListing.ToString()) %></span> bikes</p>

                                        <% } %>

                                         <div id="sort-by-content" class="grid-5 omega rightfloat">
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
                                        <% if (usedBikesList != null && totalListing > 0)
                                           { %>
                                        <ul id="used-bikes-list" data-bind="visible: OnInit() && !noBikes()">
                                            <% foreach (var bike in usedBikesList)
                                               {
                                                   string curBikeName = string.Format("{0} {1} {2}", bike.MakeName, bike.ModelName, bike.VersionName);
                                                       %>
                                            <li>

                                                <div class="model-thumbnail-image">
                                                    <a href="<%= string.Format("/used/bikes-in-{0}/{1}-{2}-{3}/",bike.CityMaskingName,bike.MakeMaskingName,bike.ModelMaskingName,bike.ProfileId) %>" title="<%= curBikeName %>">
                                                        <% if (!(String.IsNullOrEmpty(bike.Photo.OriginalImagePath) || String.IsNullOrEmpty(bike.Photo.HostUrl)))
                                                           { %>
                                                        <img class="lazy" data-original="<%= Bikewale.Utility.Image.GetPathToShowImages(bike.Photo.OriginalImagePath,bike.Photo.HostUrl,Bikewale.Utility.ImageSize._310x174) %>" alt="<%= curBikeName %>" title="<%= curBikeName %>" src="" />
                                                        <%}
                                                           else
                                                           { %>
                                                        <div class="bg-light-grey">
                                                            <span class="bwsprite no-image-icon margin-bottom15"></span>
                                                            <p class="font12 text-bold text-light-grey">Image not available</p>
                                                        </div>
                                                        <%} %>
                                                        <% if (bike.TotalPhotos > 0)
                                                           { %>
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
                                                    <h2><a href="<%= string.Format("/used/bikes-in-{0}/{1}-{2}-{3}/",bike.CityMaskingName,bike.MakeMaskingName,bike.ModelMaskingName,bike.ProfileId) %>" class="text-truncate text-black" title="<%= string.Format("{0}, {1}",bike.ModelYear,curBikeName) %>"><%= string.Format("{0}, {1}",bike.ModelYear,curBikeName) %></a></h2>
                                                    <div class="margin-bottom5">
                                                        <span class="font12 text-xt-light-grey">Updated on: <%= bike.LastUpdated.ToString("dd MMMM yyyy") %></span>
                                                    </div>
                                                    <%if (!string.IsNullOrEmpty(bike.ModelYear))
                                                      { %>
                                                     <div class="grid-6 alpha"> 
                                                        <span class="bwsprite model-date-icon"></span>
                                                        <span class="model-details-label" ><%= bike.ModelYear %> model</span>
                                                    </div>
                                                    <% } %>
                                                     <%if (bike.KmsDriven > 0)
                                                       { %>
                                                    <div class="grid-6 alpha omega" >
                                                        <span class="bwsprite kms-driven-icon"></span>
                                                        <span class="model-details-label"><%= Bikewale.Utility.Format.FormatPrice(bike.KmsDriven.ToString()) %> kms</span>
                                                    </div>
                                                    <% } %>
                                                     <%if (bike.NoOfOwners > 0)
                                                       { %>
                                                    <div class="grid-6 alpha">
                                                        <span class="bwsprite author-grey-sm-icon"></span>
                                                        <span class="model-details-label" ><%= Bikewale.Utility.Format.AddNumberOrdinal(bike.NoOfOwners) %></> Owner</span>
                                                    </div>
                                                    <% } %>
                                                     <%if (!string.IsNullOrEmpty(bike.CityName))
                                                       { %>
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
                                                    <a href="" data-bind="attr: { 'href': '/used/bikes-in-' + cityMasking + '/' + makeMasking + '-' + modelMasking + '-' + profileId + '/', title: bikeName }" title="">
                                                        <!-- ko if: photo.originalImagePath != '' -->
                                                        <img data-bind="attr: { alt: bikeName }, lazyload: photo.hostUrl + '/370x208/' + photo.originalImagePath" src="" alt="" border="0" />
                                                        <!-- /ko -->
                                                        <!-- ko if: photo.originalImagePath == '' -->
                                                        <div class="bg-light-grey">
                                                            <span class="bwsprite no-image-icon margin-bottom15"></span>
                                                            <p class="font12 text-bold text-light-grey">Image not available</p>
                                                        </div>
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
                                                    <h2><a href="" data-bind="text: (modelYear > 0?modelYear+', ':'') + bikeName, attr: { 'href': '/used/bikes-in-' + cityMasking + '/' + makeMasking + '-' + modelMasking + '-' + profileId + '/' }" class="text-truncate text-black" ></a></h2>
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
                                                    <p class="margin-bottom10" data-bind="visible: askingPrice > 0" ><span class="bwsprite inr-md-lg"></span>&nbsp;<span class="font22 text-bold" data-bind="    CurrencyText: askingPrice"></span></p>
                                                    <a href="javascript:void(0)" class="btn btn-white seller-details-btn used-bike-lead" rel="nofollow" data-ga-cat="Used_Bike_Listing" data-ga-act="Get_Seller_Details_Clicked" data-bind="attr: { 'data-ga-lab': profileId, 'data-profile-id': profileId }">Get seller details</a>
                                                </div>
                                                <div class="clear"></div>
                                            </li>  
                                        </ul>

                                        <div style="text-align: center;">
                                            <div id="nobike" style="display: none;"  data-bind="visible : noBikes()">
                                                <img src="https://imgd.aeplcdn.com/0x0/bw/static/design15/no-result-m.png" alt="No match found">
                                            </div>
                                        </div>  
                                   <%-- </div>--%>

                                    <div id="search-listing-footer" class="font14">
                                        <div class="grid-5 alpha omega text-light-grey">
                                            <p style="display:none" data-bind="visible: !OnInit() && TotalBikes() > 0">Showing <span class="text-default text-bold"><span data-bind="    CurrencyText: (Pagination().pageNumber() - 1) * Pagination().pageSize() + 1"><%=_startIndex %></span>-<span data-bind="    CurrencyText: Math.min(TotalBikes(), Pagination().pageNumber() * Pagination().pageSize())""><%=_endIndex %></span></span> of <span class="text-default text-bold" data-bind="    CurrencyText: TotalBikes()"><%= Bikewale.Utility.Format.FormatPrice(totalListing.ToString()) %></span> bikes</p>
                                            <% if (totalListing > 0)
                                               { %>
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
                                            <select id="ddlCity" class="city-chosen-select hide" data-bind="chosen:{width: '100%'},event: { change: FilterCity }">
                                                 <option data-cityid="0" data-citymasking="india" value="0">All India</option>
                                                 <% if (citiesList != null)
                                                    { %>
                                                <% foreach (var city in citiesList)
                                                   { %>
                                                      <option data-cityid="<%= city.CityId %>" data-citymasking="<%= city.CityMaskingName %>" value="<%= city.CityId %>" ><%=city.CityName %></option>
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
                                                <%foreach (var make in makeModelsList)
                                                  {%>
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
                                                            <%foreach (var model in make.Models)
                                                              {%>
                                                                <li id="md-<%=model.ModelId %>" data-bind="click : ApplyModelFilter">
                                                                    <span data-modelid="<%=model.ModelId %>" class="bwsprite unchecked-box"></span>
                                                                    <span class="category-label"><%=model.ModelName %></span>
                                                                </li>
                                                           <% } %>                                                     
                                                    </ul>
                                                    </div>
                                                </li>
                                                <% }
                                                   } %>
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

        <script type="text/javascript" src="<%= staticUrl  %>/UI/src/frameworks.js?<%=staticFileVersion %>"></script>
        <BW:UBLeadCapturePopup runat="server" ID="ctrlUBLeadCapturePopup"></BW:UBLeadCapturePopup>
        <!-- #include file="/UI/includes/footerBW.aspx" -->
        <link href="<%= staticUrl  %>/UI/css/bw-common-btf.css?<%=staticFileVersion %>" rel="stylesheet" type="text/css" />
        <!-- #include file="/UI/includes/footerscript.aspx" -->
         <script type="text/javascript">
             var OnInitTotalBikes = <%= totalListing %>; 
             var pageQS = "<%= currentQueryString %>" ;
             var selectedCityId = <%= cityId %>;selectedMakeId = "<%= makeId %>",selectedModelId = "<%= modelId %>";
             var usedPageIdentifier="<%=PageIdentifier%>";
             var gaObj = { 'id': '<%= (int)Bikewale.Entities.Pages.GAPages.Used_Bike_Listing%>', 'name': '<%= Bikewale.Entities.Pages.GAPages.Used_Bike_Listing%>' };

        </script>
        <script type="text/javascript" src="<%= staticUrl %>/UI/src/used-search.js?<%= staticFileVersion%>"></script>
        <!-- #include file="/UI/includes/fontBW.aspx" -->
    </form>
</body>
</html>
