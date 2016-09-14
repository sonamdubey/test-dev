<%@ Page Language="C#" AutoEventWireup="false" Inherits="Bikewale.Mobile.Used.Search" %>
<%@ Register TagPrefix="BikeWale" TagName="Pager" Src="/m/controls/LinkPagerControl.ascx" %>
<!DOCTYPE html>
<html>
<head>
        <%
            title = pageTitle;
            description = pageDescription;
            canonical = "";
            keywords = pageKeywords;
            EnableOG = true;
        %>

    <!-- #include file="/includes/headscript_mobile.aspx" -->
    <link href="<%= staticUrl != "" ? "http://st2.aeplcdn.com" + staticUrl : "" %>/m/css/used-search-atf.css?<%= staticFileVersion %>" rel="stylesheet" type="text/css" />

</head>
<body>
    <form id="form1" runat="server">
        <!-- #include file="/includes/headBW_Mobile.aspx" -->
       
        <div class="modal-background"></div>

        <section>
            <div class="container bg-white clearfix">
                <h1 class="padding-top15 padding-right20 padding-bottom15 padding-left20 box-shadow"><%=heading %></h1>

                <div class="font14 padding-top10 padding-right20 padding-bottom10 padding-left20">Showing <span class="text-bold"><%=_startIndex %>-<%=_endIndex %></span> of <span class="text-bold"><%=totalListing %></span> bikes</div>

                <div id="sort-filter-wrapper" class="text-center border-solid-bottom">
                    <div id="sort-floating-btn" class="grid-6 padding-top10 padding-bottom10 border-solid-right cur-pointer">
                        <span class="bwmsprite sort-by-icon"></span>
                        <span class="font14 text-bold">Sort by</span>
                    </div>
                    <div id="filter-floating-btn" class="grid-6 padding-top10 padding-bottom10 cur-pointer">
                        <span class="bwmsprite filter-icon"></span>
                        <span class="font14 text-bold">Filter</span>
                    </div>
                    <div class="clear"></div>
                </div>

                <ul id="used-bikes-list">
                    <asp:Repeater ID="rptUsedListings" runat="server">
                        <ItemTemplate>
                            <li>
                                <div class="model-thumbnail-image">
                                    <a href="javascript:void(0)" class="model-image-target">
                                        <img class="lazy" data-original="<%# Bikewale.Utility.Image.GetPathToShowImages(DataBinder.Eval(Container.DataItem, "Photo.OriginalImagePath").ToString(),DataBinder.Eval(Container.DataItem, "Photo.HostUrl").ToString(),Bikewale.Utility.ImageSize._370x208) %>" alt="" title="" border="0" />
                                        <div class="model-media-details <%# Convert.ToUInt16(DataBinder.Eval(Container.DataItem, "TotalPhotos")) > 1? "":"hide" %>">
                                            <div class="model-media-item">
                                                <span class="bwmsprite gallery-photo-icon"></span>
                                                <span class="model-media-count"><%# DataBinder.Eval(Container.DataItem, "TotalPhotos").ToString() %></span>
                                            </div>
                                        </div>
                                    </a>
                                </div>
                                <div class="margin-right20 margin-left20 padding-top10 font14">
                                    <h2 class="margin-bottom10">
                                        <a href="">
                                            <%# DataBinder.Eval(Container.DataItem, "MakeName").ToString() + " " + DataBinder.Eval(Container.DataItem, "ModelName").ToString() + " " + DataBinder.Eval(Container.DataItem, "VersionName").ToString() %>
                                        </a>
                                    </h2>
                                    <div class="grid-6 alpha omega margin-bottom5">
                                        <span class="bwmsprite model-date-icon"></span>
                                        <span class="model-details-label"><%# DataBinder.Eval(Container.DataItem, "ModelYear").ToString() %> model</span>
                                    </div>
                                    <div class="grid-6 omega margin-bottom5">
                                        <span class="bwmsprite kms-driven-icon"></span>
                                        <span class="model-details-label"><%# Bikewale.Utility.Format.FormatNumeric(DataBinder.Eval(Container.DataItem, "KmsDriven").ToString()) %> kms</span>
                                    </div>
                                    <div class="grid-6 alpha omega margin-bottom5">
                                        <span class="bwmsprite author-grey-sm-icon"></span>
                                        <span class="model-details-label"><%# DataBinder.Eval(Container.DataItem, "NoOfOwners").ToString() %> owner</span>
                                    </div>
                                    <div class="grid-6 omega margin-bottom5">
                                        <span class="bwmsprite model-loc-icon"></span>
                                        <span class="model-details-label"><%# DataBinder.Eval(Container.DataItem, "CityName").ToString() %></span>
                                    </div>
                                    <div class="clear"></div>
                                    <p class="margin-bottom15"><span class="bwmsprite inr-md-icon"></span>&nbsp;<span class="font22 text-bold"><%# Bikewale.Utility.Format.FormatNumeric(DataBinder.Eval(Container.DataItem, "AskingPrice").ToString()) %></span></p>
                                    <a href="javascript:void(0)" class="btn btn-orange seller-details-btn" rel="nofollow">Get seller details</a>
                                </div>
                            </li>
                        </ItemTemplate>
                    </asp:Repeater>
                </ul>


                <div class="margin-right10 margin-left10 padding-top15 padding-bottom15 border-solid-top font14">
                    <div class="grid-5 omega text-light-grey">
                        <span class="text-default text-bold"><%=_startIndex %>-<%=_endIndex %></span> of <span class="text-default text-bold"><%=totalListing %></span> bikes
                    </div>
                <BikeWale:Pager ID="ctrlPager" runat="server" />
                </div>
                <div class="margin-right10 margin-left10 padding-top15 padding-bottom15 border-solid-top font14">
                    <div class="grid-5 omega text-light-grey">
                        <span class="text-default text-bold">1-20</span> of <span class="text-default text-bold"><%=totalListing %></span> bikes
                    </div>
                    <div class="grid-7 alpha omega position-rel">
                        <ul id="pagination-list">
                            <li>
                                <a href="">91</a>
                            </li>
                            <li>
                                <a href="">92</a>
                            </li>
                            <li class="active">
                                <a href="">93</a>
                            </li>
                            <li>
                                <a href="">94</a>
                            </li>
                            <li>
                                <a href="">95</a>
                            </li>
                        </ul>
                        <span class="pagination-control-prev inactive">
                            <a href="" class="bwmsprite prev-page-icon"></a>
                        </span>
                        <span class="pagination-control-next">
                            <a href="" class="bwmsprite next-page-icon"></a>
                        </span>
                    </div>
                    <div class="clear"></div>
                </div>
            </div>
        </section>

        <!-- sort popup start -->
        <div id="sort-by-container" class="sort-popup-container">
            <div class="popup-header">Sort</div>
            <div class="popup-body">
                <ul id="sort-by-list" class="margin-bottom25">
                    <li>
                        <span class="bwmsprite radio-uncheck"></span>
                        <span class="sort-list-label">Best match</span>
                    </li>
                    <li class="active">
                        <span class="bwmsprite radio-uncheck"></span>
                        <span class="sort-list-label">Most recent</span>
                    </li>
                    <li>
                        <span class="bwmsprite radio-uncheck"></span>
                        <span class="sort-list-label">Price - Low to High</span>
                    </li>
                    <li>
                        <span class="bwmsprite radio-uncheck"></span>
                        <span class="sort-list-label">Price - High to Low</span>
                    </li>
                    <li>
                        <span class="bwmsprite radio-uncheck"></span>
                        <span class="sort-list-label">Kms - Low to High</span>
                    </li>
                    <li>
                        <span class="bwmsprite radio-uncheck"></span>
                        <span class="sort-list-label">Kms - High to Low</span>
                    </li>
                </ul>
                <div class="grid-6 alpha">
                    <p id="cancel-sort-by" class="btn btn-white btn-full-width btn-size-0">Cancel</p>
                </div>
                <div class="grid-6 omega">
                    <p id="apply-sort-by" class="btn btn-orange btn-full-width btn-size-0">Apply</p>
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
                        <p class="selected-filters"></p>
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
                    <div id="budget-range-slider"></div>
                </div>
                <div class="margin-bottom35">
                    <p class="filter-option-key leftfloat">Kms ridden</p>
                    <p id="kms-amount" class="font14 text-bold rightfloat"></p>
                    <div class="clear"></div>
                    <div id="kms-range-slider"></div>
                </div>
                <div class="margin-bottom35">
                    <p class="filter-option-key leftfloat">Bike age</p>
                    <p id="bike-age-amount" class="font14 text-bold rightfloat"></p>
                    <div class="clear"></div>
                    <div id="bike-age-slider"></div>
                </div>
                <div class="margin-bottom25">
                    <p class="filter-option-key margin-bottom10">Previous owners</p>
                    <ul id="previous-owners-list">
                        <li>
                            <span>1</span>
                        </li>
                        <li>
                            <span>2</span>
                        </li>
                        <li>
                            <span>3</span>
                        </li>
                        <li>
                            <span>4</span>
                        </li>
                        <li>
                            <span class="prev-owner-last-item">4 +</span>
                        </li>
                    </ul>
                </div>
                <div>
                    <p class="filter-option-key margin-bottom10">Seller type</p>
                    <div class="filter-type-seller grid-6 unchecked padding-left25">Individual</div>
                    <div class="filter-type-seller grid-6 unchecked padding-left25">Dealer</div>
                    <div class="clear"></div>
                </div>
            </div>

            <div id="filter-container-footer" class="filter-container-footer">
                <div class="grid-6">
                    <p id="reset-filters" class="btn btn-white btn-full-width btn-size-0">Reset</p>
                </div>
                <div class="grid-6">
                    <p id="apply-filters" class="btn btn-orange btn-full-width btn-size-0">Apply filters</p>
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
                        <li>Ahmedabad</li>
                        <li>Bangalore</li>
                        <li>Chennai</li>
                        <li>Hyderabad</li>
                        <li>Kolkata</li>
                        <li>Mumbai</li>
                        <li>Navi Mumbai</li>
                        <li>New Delhi</li>
                        <li>Pune</li>
                        <li>Thane</li>
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
                    <li>
                        <div class="accordion-tab">
                            <div class="accordion-checkbox leftfloat">
                                <span class="bwmsprite unchecked-box"></span>
                            </div>
                            <div class="accordion-label-tab leftfloat">
                                <span class="accordion-label">Honda</span>
                                <span class="accordion-count"></span>
                                <span class="bwmsprite arrow-down"></span>
                            </div>
                            <div class="clear"></div>
                        </div>
                        <ul class="bike-model-list">
                            <li>
                                <span class="bwmsprite unchecked-box"></span>
                                <span class="bike-model-label">Navi</span>
                            </li>
                            <li>
                                <span class="bwmsprite unchecked-box"></span>
                                <span class="bike-model-label">Activa-i</span>
                            </li>
                            <li>
                                <span class="bwmsprite unchecked-box"></span>
                                <span class="bike-model-label">CD 110 Dream</span>
                            </li>
                            <li>
                                <span class="bwmsprite unchecked-box"></span>
                                <span class="bike-model-label">Dio</span>
                            </li>
                            <li>
                                <span class="bwmsprite unchecked-box"></span>
                                <span class="bike-model-label">Dream Neo</span>
                            </li>
                            <li>
                                <span class="bwmsprite unchecked-box"></span>
                                <span class="bike-model-label">Activa 3G</span>
                            </li>
                            <li>
                                <span class="bwmsprite unchecked-box"></span>
                                <span class="bike-model-label">Dream Yuga</span>
                            </li>
                            <li>
                                <span class="bwmsprite unchecked-box"></span>
                                <span class="bike-model-label">Livo</span>
                            </li>
                            <li>
                                <span class="bwmsprite unchecked-box"></span>
                                <span class="bike-model-label">Activa 125</span>
                            </li>
                            <li>
                                <span class="bwmsprite unchecked-box"></span>
                                <span class="bike-model-label">Aviator</span>
                            </li>
                            <li>
                                <span class="bwmsprite unchecked-box"></span>
                                <span class="bike-model-label">CB Shine</span>
                            </li>
                            <li>
                                <span class="bwmsprite unchecked-box"></span>
                                <span class="bike-model-label">CB Unicorn 150</span>
                            </li>
                        </ul>
                    </li>
                    <li>
                        <div class="accordion-tab">
                            <div class="accordion-checkbox leftfloat">
                                <span class="bwmsprite unchecked-box"></span>
                            </div>
                            <div class="accordion-label-tab leftfloat">
                                <span class="accordion-label">Harley Davidson</span>
                                <span class="accordion-count"></span>
                                <span class="bwmsprite arrow-down"></span>
                            </div>
                            <div class="clear"></div>
                        </div>
                        <ul class="bike-model-list">
                            <li>
                                <span class="bwmsprite unchecked-box"></span>
                                <span class="bike-model-label">Navi</span>
                            </li>
                            <li>
                                <span class="bwmsprite unchecked-box"></span>
                                <span class="bike-model-label">Activa-i</span>
                            </li>
                            <li>
                                <span class="bwmsprite unchecked-box"></span>
                                <span class="bike-model-label">CD 110 Dream</span>
                            </li>
                            <li>
                                <span class="bwmsprite unchecked-box"></span>
                                <span class="bike-model-label">Dio</span>
                            </li>
                            <li>
                                <span class="bwmsprite unchecked-box"></span>
                                <span class="bike-model-label">Dream Neo</span>
                            </li>
                            <li>
                                <span class="bwmsprite unchecked-box"></span>
                                <span class="bike-model-label">Activa 3G</span>
                            </li>
                            <li>
                                <span class="bwmsprite unchecked-box"></span>
                                <span class="bike-model-label">Dream Yuga</span>
                            </li>
                            <li>
                                <span class="bwmsprite unchecked-box"></span>
                                <span class="bike-model-label">Livo</span>
                            </li>
                            <li>
                                <span class="bwmsprite unchecked-box"></span>
                                <span class="bike-model-label">Activa 125</span>
                            </li>
                            <li>
                                <span class="bwmsprite unchecked-box"></span>
                                <span class="bike-model-label">Aviator</span>
                            </li>
                            <li>
                                <span class="bwmsprite unchecked-box"></span>
                                <span class="bike-model-label">CB Shine</span>
                            </li>
                            <li>
                                <span class="bwmsprite unchecked-box"></span>
                                <span class="bike-model-label">CB Unicorn 150</span>
                            </li>
                        </ul>
                    </li>
                    <li>
                        <div class="accordion-tab">
                            <div class="accordion-checkbox leftfloat">
                                <span class="bwmsprite unchecked-box"></span>
                            </div>
                            <div class="accordion-label-tab leftfloat">
                                <span class="accordion-label">MV Agusta</span>
                                <span class="accordion-count"></span>
                                <span class="bwmsprite arrow-down"></span>
                            </div>
                            <div class="clear"></div>
                        </div>
                        <ul class="bike-model-list">
                            <li>
                                <span class="bwmsprite unchecked-box"></span>
                                <span class="bike-model-label">Navi</span>
                            </li>
                            <li>
                                <span class="bwmsprite unchecked-box"></span>
                                <span class="bike-model-label">Activa-i</span>
                            </li>
                            <li>
                                <span class="bwmsprite unchecked-box"></span>
                                <span class="bike-model-label">CD 110 Dream</span>
                            </li>
                            <li>
                                <span class="bwmsprite unchecked-box"></span>
                                <span class="bike-model-label">Dio</span>
                            </li>
                            <li>
                                <span class="bwmsprite unchecked-box"></span>
                                <span class="bike-model-label">Dream Neo</span>
                            </li>
                            <li>
                                <span class="bwmsprite unchecked-box"></span>
                                <span class="bike-model-label">Activa 3G</span>
                            </li>
                            <li>
                                <span class="bwmsprite unchecked-box"></span>
                                <span class="bike-model-label">Dream Yuga</span>
                            </li>
                            <li>
                                <span class="bwmsprite unchecked-box"></span>
                                <span class="bike-model-label">Livo</span>
                            </li>
                            <li>
                                <span class="bwmsprite unchecked-box"></span>
                                <span class="bike-model-label">Activa 125</span>
                            </li>
                            <li>
                                <span class="bwmsprite unchecked-box"></span>
                                <span class="bike-model-label">Aviator</span>
                            </li>
                            <li>
                                <span class="bwmsprite unchecked-box"></span>
                                <span class="bike-model-label">CB Shine</span>
                            </li>
                            <li>
                                <span class="bwmsprite unchecked-box"></span>
                                <span class="bike-model-label">CB Unicorn 150</span>
                            </li>
                        </ul>
                    </li>
                    <li>
                        <div class="accordion-tab">
                            <div class="accordion-checkbox leftfloat">
                                <span class="bwmsprite unchecked-box"></span>
                            </div>
                            <div class="accordion-label-tab leftfloat">
                                <span class="accordion-label">Aprilia</span>
                                <span class="accordion-count"></span>
                                <span class="bwmsprite arrow-down"></span>
                            </div>
                            <div class="clear"></div>
                        </div>
                        <ul class="bike-model-list">
                            <li>
                                <span class="bwmsprite unchecked-box"></span>
                                <span class="bike-model-label">Navi</span>
                            </li>
                            <li>
                                <span class="bwmsprite unchecked-box"></span>
                                <span class="bike-model-label">Activa-i</span>
                            </li>
                            <li>
                                <span class="bwmsprite unchecked-box"></span>
                                <span class="bike-model-label">CD 110 Dream</span>
                            </li>
                            <li>
                                <span class="bwmsprite unchecked-box"></span>
                                <span class="bike-model-label">Dio</span>
                            </li>
                            <li>
                                <span class="bwmsprite unchecked-box"></span>
                                <span class="bike-model-label">Dream Neo</span>
                            </li>
                            <li>
                                <span class="bwmsprite unchecked-box"></span>
                                <span class="bike-model-label">Activa 3G</span>
                            </li>
                            <li>
                                <span class="bwmsprite unchecked-box"></span>
                                <span class="bike-model-label">Dream Yuga</span>
                            </li>
                            <li>
                                <span class="bwmsprite unchecked-box"></span>
                                <span class="bike-model-label">Livo</span>
                            </li>
                            <li>
                                <span class="bwmsprite unchecked-box"></span>
                                <span class="bike-model-label">Activa 125</span>
                            </li>
                            <li>
                                <span class="bwmsprite unchecked-box"></span>
                                <span class="bike-model-label">Aviator</span>
                            </li>
                            <li>
                                <span class="bwmsprite unchecked-box"></span>
                                <span class="bike-model-label">CB Shine</span>
                            </li>
                            <li>
                                <span class="bwmsprite unchecked-box"></span>
                                <span class="bike-model-label">CB Unicorn 150</span>
                            </li>
                        </ul>
                    </li>
                    <li>
                        <div class="accordion-tab">
                            <div class="accordion-checkbox leftfloat">
                                <span class="bwmsprite unchecked-box"></span>
                            </div>
                            <div class="accordion-label-tab leftfloat">
                                <span class="accordion-label">Hero</span>
                                <span class="accordion-count"></span>
                                <span class="bwmsprite arrow-down"></span>
                            </div>
                            <div class="clear"></div>
                        </div>
                        <ul class="bike-model-list">
                            <li>
                                <span class="bwmsprite unchecked-box"></span>
                                <span class="bike-model-label">Navi</span>
                            </li>
                            <li>
                                <span class="bwmsprite unchecked-box"></span>
                                <span class="bike-model-label">Activa-i</span>
                            </li>
                            <li>
                                <span class="bwmsprite unchecked-box"></span>
                                <span class="bike-model-label">CD 110 Dream</span>
                            </li>
                            <li>
                                <span class="bwmsprite unchecked-box"></span>
                                <span class="bike-model-label">Dio</span>
                            </li>
                            <li>
                                <span class="bwmsprite unchecked-box"></span>
                                <span class="bike-model-label">Dream Neo</span>
                            </li>
                            <li>
                                <span class="bwmsprite unchecked-box"></span>
                                <span class="bike-model-label">Activa 3G</span>
                            </li>
                            <li>
                                <span class="bwmsprite unchecked-box"></span>
                                <span class="bike-model-label">Dream Yuga</span>
                            </li>
                            <li>
                                <span class="bwmsprite unchecked-box"></span>
                                <span class="bike-model-label">Livo</span>
                            </li>
                            <li>
                                <span class="bwmsprite unchecked-box"></span>
                                <span class="bike-model-label">Activa 125</span>
                            </li>
                            <li>
                                <span class="bwmsprite unchecked-box"></span>
                                <span class="bike-model-label">Aviator</span>
                            </li>
                            <li>
                                <span class="bwmsprite unchecked-box"></span>
                                <span class="bike-model-label">CB Shine</span>
                            </li>
                            <li>
                                <span class="bwmsprite unchecked-box"></span>
                                <span class="bike-model-label">CB Unicorn 150</span>
                            </li>
                        </ul>
                    </li>
                    <li>
                        <div class="accordion-tab">
                            <div class="accordion-checkbox leftfloat">
                                <span class="bwmsprite unchecked-box"></span>
                            </div>
                            <div class="accordion-label-tab leftfloat">
                                <span class="accordion-label">Honda</span>
                                <span class="accordion-count"></span>
                                <span class="bwmsprite arrow-down"></span>
                            </div>
                            <div class="clear"></div>
                        </div>
                        <ul class="bike-model-list">
                            <li>
                                <span class="bwmsprite unchecked-box"></span>
                                <span class="bike-model-label">Navi</span>
                            </li>
                            <li>
                                <span class="bwmsprite unchecked-box"></span>
                                <span class="bike-model-label">Activa-i</span>
                            </li>
                            <li>
                                <span class="bwmsprite unchecked-box"></span>
                                <span class="bike-model-label">CD 110 Dream</span>
                            </li>
                            <li>
                                <span class="bwmsprite unchecked-box"></span>
                                <span class="bike-model-label">Dio</span>
                            </li>
                            <li>
                                <span class="bwmsprite unchecked-box"></span>
                                <span class="bike-model-label">Dream Neo</span>
                            </li>
                            <li>
                                <span class="bwmsprite unchecked-box"></span>
                                <span class="bike-model-label">Activa 3G</span>
                            </li>
                            <li>
                                <span class="bwmsprite unchecked-box"></span>
                                <span class="bike-model-label">Dream Yuga</span>
                            </li>
                            <li>
                                <span class="bwmsprite unchecked-box"></span>
                                <span class="bike-model-label">Livo</span>
                            </li>
                            <li>
                                <span class="bwmsprite unchecked-box"></span>
                                <span class="bike-model-label">Activa 125</span>
                            </li>
                            <li>
                                <span class="bwmsprite unchecked-box"></span>
                                <span class="bike-model-label">Aviator</span>
                            </li>
                            <li>
                                <span class="bwmsprite unchecked-box"></span>
                                <span class="bike-model-label">CB Shine</span>
                            </li>
                            <li>
                                <span class="bwmsprite unchecked-box"></span>
                                <span class="bike-model-label">CB Unicorn 150</span>
                            </li>
                        </ul>
                    </li>
                    <li>
                        <div class="accordion-tab">
                            <div class="accordion-checkbox leftfloat">
                                <span class="bwmsprite unchecked-box"></span>
                            </div>
                            <div class="accordion-label-tab leftfloat">
                                <span class="accordion-label">Harley Davidson</span>
                                <span class="accordion-count"></span>
                                <span class="bwmsprite arrow-down"></span>
                            </div>
                            <div class="clear"></div>
                        </div>
                        <ul class="bike-model-list">
                            <li>
                                <span class="bwmsprite unchecked-box"></span>
                                <span class="bike-model-label">Navi</span>
                            </li>
                            <li>
                                <span class="bwmsprite unchecked-box"></span>
                                <span class="bike-model-label">Activa-i</span>
                            </li>
                            <li>
                                <span class="bwmsprite unchecked-box"></span>
                                <span class="bike-model-label">CD 110 Dream</span>
                            </li>
                            <li>
                                <span class="bwmsprite unchecked-box"></span>
                                <span class="bike-model-label">Dio</span>
                            </li>
                            <li>
                                <span class="bwmsprite unchecked-box"></span>
                                <span class="bike-model-label">Dream Neo</span>
                            </li>
                            <li>
                                <span class="bwmsprite unchecked-box"></span>
                                <span class="bike-model-label">Activa 3G</span>
                            </li>
                            <li>
                                <span class="bwmsprite unchecked-box"></span>
                                <span class="bike-model-label">Dream Yuga</span>
                            </li>
                            <li>
                                <span class="bwmsprite unchecked-box"></span>
                                <span class="bike-model-label">Livo</span>
                            </li>
                            <li>
                                <span class="bwmsprite unchecked-box"></span>
                                <span class="bike-model-label">Activa 125</span>
                            </li>
                            <li>
                                <span class="bwmsprite unchecked-box"></span>
                                <span class="bike-model-label">Aviator</span>
                            </li>
                            <li>
                                <span class="bwmsprite unchecked-box"></span>
                                <span class="bike-model-label">CB Shine</span>
                            </li>
                            <li>
                                <span class="bwmsprite unchecked-box"></span>
                                <span class="bike-model-label">CB Unicorn 150</span>
                            </li>
                        </ul>
                    </li>
                    <li>
                        <div class="accordion-tab">
                            <div class="accordion-checkbox leftfloat">
                                <span class="bwmsprite unchecked-box"></span>
                            </div>
                            <div class="accordion-label-tab leftfloat">
                                <span class="accordion-label">MV Agusta</span>
                                <span class="accordion-count"></span>
                                <span class="bwmsprite arrow-down"></span>
                            </div>
                            <div class="clear"></div>
                        </div>
                        <ul class="bike-model-list">
                            <li>
                                <span class="bwmsprite unchecked-box"></span>
                                <span class="bike-model-label">Navi</span>
                            </li>
                            <li>
                                <span class="bwmsprite unchecked-box"></span>
                                <span class="bike-model-label">Activa-i</span>
                            </li>
                            <li>
                                <span class="bwmsprite unchecked-box"></span>
                                <span class="bike-model-label">CD 110 Dream</span>
                            </li>
                            <li>
                                <span class="bwmsprite unchecked-box"></span>
                                <span class="bike-model-label">Dio</span>
                            </li>
                            <li>
                                <span class="bwmsprite unchecked-box"></span>
                                <span class="bike-model-label">Dream Neo</span>
                            </li>
                            <li>
                                <span class="bwmsprite unchecked-box"></span>
                                <span class="bike-model-label">Activa 3G</span>
                            </li>
                            <li>
                                <span class="bwmsprite unchecked-box"></span>
                                <span class="bike-model-label">Dream Yuga</span>
                            </li>
                            <li>
                                <span class="bwmsprite unchecked-box"></span>
                                <span class="bike-model-label">Livo</span>
                            </li>
                            <li>
                                <span class="bwmsprite unchecked-box"></span>
                                <span class="bike-model-label">Activa 125</span>
                            </li>
                            <li>
                                <span class="bwmsprite unchecked-box"></span>
                                <span class="bike-model-label">Aviator</span>
                            </li>
                            <li>
                                <span class="bwmsprite unchecked-box"></span>
                                <span class="bike-model-label">CB Shine</span>
                            </li>
                            <li>
                                <span class="bwmsprite unchecked-box"></span>
                                <span class="bike-model-label">CB Unicorn 150</span>
                            </li>
                        </ul>
                    </li>
                    <li>
                        <div class="accordion-tab">
                            <div class="accordion-checkbox leftfloat">
                                <span class="bwmsprite unchecked-box"></span>
                            </div>
                            <div class="accordion-label-tab leftfloat">
                                <span class="accordion-label">Aprilia</span>
                                <span class="accordion-count"></span>
                                <span class="bwmsprite arrow-down"></span>
                            </div>
                            <div class="clear"></div>
                        </div>
                        <ul class="bike-model-list">
                            <li>
                                <span class="bwmsprite unchecked-box"></span>
                                <span class="bike-model-label">Navi</span>
                            </li>
                            <li>
                                <span class="bwmsprite unchecked-box"></span>
                                <span class="bike-model-label">Activa-i</span>
                            </li>
                            <li>
                                <span class="bwmsprite unchecked-box"></span>
                                <span class="bike-model-label">CD 110 Dream</span>
                            </li>
                            <li>
                                <span class="bwmsprite unchecked-box"></span>
                                <span class="bike-model-label">Dio</span>
                            </li>
                            <li>
                                <span class="bwmsprite unchecked-box"></span>
                                <span class="bike-model-label">Dream Neo</span>
                            </li>
                            <li>
                                <span class="bwmsprite unchecked-box"></span>
                                <span class="bike-model-label">Activa 3G</span>
                            </li>
                            <li>
                                <span class="bwmsprite unchecked-box"></span>
                                <span class="bike-model-label">Dream Yuga</span>
                            </li>
                            <li>
                                <span class="bwmsprite unchecked-box"></span>
                                <span class="bike-model-label">Livo</span>
                            </li>
                            <li>
                                <span class="bwmsprite unchecked-box"></span>
                                <span class="bike-model-label">Activa 125</span>
                            </li>
                            <li>
                                <span class="bwmsprite unchecked-box"></span>
                                <span class="bike-model-label">Aviator</span>
                            </li>
                            <li>
                                <span class="bwmsprite unchecked-box"></span>
                                <span class="bike-model-label">CB Shine</span>
                            </li>
                            <li>
                                <span class="bwmsprite unchecked-box"></span>
                                <span class="bike-model-label">CB Unicorn 150</span>
                            </li>
                        </ul>
                    </li>
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
        
        <!-- #include file="/includes/footerBW_Mobile.aspx" -->
        <!-- #include file="/includes/footerscript_Mobile.aspx" -->
        <link href="<%= staticUrl != "" ? "http://st2.aeplcdn.com" + staticUrl : "" %>/m/css/used-search-btf.css?<%= staticFileVersion %>" rel="stylesheet" type="text/css" />
        <script type="text/javascript" src="<%= staticUrl != "" ? "http://st2.aeplcdn.com" + staticUrl : "" %>/m/src/used-search.js?<%= staticFileVersion %>"></script>
    </form>
</body>
</html>
