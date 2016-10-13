﻿<%@ Page Language="C#" AutoEventWireup="false" Inherits="Bikewale.Used.BikeDetails" EnableViewState="false" %>

<%@ Register Src="/controls/UsedBikeLeadCaptureControl.ascx" TagPrefix="BW" TagName="UBLeadCapturePopup" %>
<%@ Register Src="~/controls/SimilarUsedBikes.ascx" TagPrefix="BW" TagName="SimilarUsedBikes" %>
<%@ Register Src="~/m/controls/UploadPhotoRequestPopup.ascx" TagPrefix="BW" TagName="UploadPhotoRequestPopup" %>
<%@ Register Src="~/controls/OtherUsedBikeByCity.ascx" TagPrefix="BW" TagName="OtherUsedBikes" %>

<!DOCTYPE html>
<html>
<head >
    <%  
        isHeaderFix = false;
        title = pgTitle;
        keywords = pgKeywords;
        description = pgDescription;
        canonical = pgCanonicalUrl;
        ogImage = (firstImage != null) ? Bikewale.Utility.Image.GetPathToShowImages(firstImage.OriginalImagePath, firstImage.HostUrl, Bikewale.Utility.ImageSize._360x202) : string.Empty;
        AdId = "1395992162974";
        AdPath = "/1017752/BikeWale_UsedBikes_Details_";
        isAd970x90BTFShown = false;
        isAd970x90Shown = true;
        alternate = pgAlternateUrl; 
        TargetedModel = (inquiryDetails.Model != null) ? inquiryDetails.Model.ModelName : string.Empty;
        TargetedCity = (inquiryDetails.City != null) ? inquiryDetails.City.CityName : string.Empty;
    %>
    <!-- #include file="/includes/headscript_desktop_min.aspx" -->
    <link type="text/css" href="/css/used/details.css" rel="stylesheet" />

    <script type="text/javascript">
        <!-- #include file="\includes\gacode_desktop.aspx" -->
    </script>
</head>
<body>
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
                            <li itemscope="" itemtype="http://data-vocabulary.org/Breadcrumb">
                                <span class="bwsprite fa-angle-right margin-right10"></span>
                                <a href="/used/" itemprop="url">
                                    <span itemprop="title">Used Bikes</span>
                                </a>
                            </li>
                            <% if (inquiryDetails!=null && inquiryDetails.City!=null ) { %>
                            <li itemscope="" itemtype="http://data-vocabulary.org/Breadcrumb">
                                <span class="bwsprite fa-angle-right margin-right10"></span>
                                <a href="/used/bikes-in-<%= inquiryDetails.City.CityMaskingName %>/" itemprop="url">
                                    <span itemprop="title"><%= inquiryDetails.City.CityName %></span>
                                </a>
                            </li>
                            <li itemscope="" itemtype="http://data-vocabulary.org/Breadcrumb">
                                <span class="bwsprite fa-angle-right margin-right10"></span>
                                <a href="<%= string.Format("/used/{0}-{1}-bikes-in-{2}/", inquiryDetails.Make.MaskingName, inquiryDetails.Model.MaskingName, inquiryDetails.City.CityMaskingName) %>" itemprop="url">
                                    <span itemprop="title"><%= inquiryDetails.Model.ModelName %></span>
                                </a>
                            </li>   
                            <% } %>
                            <li><span class="bwsprite fa-angle-right margin-right10"></span>
                                <span><%= string.Format("{0} {1} in {2}",modelYear,bikeName,(inquiryDetails!=null && inquiryDetails.City!=null ) ? inquiryDetails.City.CityName:string.Empty) %></span>
                            </li>                           
                        </ul>
                        <div class="clear"></div>
                    </div>
                </div>
                <div class="clear"></div>
            </div>
        </section>

        <section>
            <div class="container">
                <div class="grid-12 margin-bottom20">
                    <div class="content-box-shadow content-inner-block-20">
                        <h1 class="margin-bottom20"><%=modelYear %>, <%= bikeName %></h1>
                        <div id="bike-main-carousel" class="grid-5 alpha">
                    <%if (inquiryDetails.PhotosCount > 0)
                      { %>
                            <div class="jcarousel-wrapper">
                                <div class="jcarousel">
                                    <ul>
                                        <% for(int i=0;i<inquiryDetails.PhotosCount;i++) { var photo = inquiryDetails.Photo[i];  %>
                                        <li>
                                            <div class="carousel-img-container">
                                                <span>
                                                    <% if(i<4){ %>
                                                    <img src="<%= Bikewale.Utility.Image.GetPathToShowImages(photo.OriginalImagePath,photo.HostUrl,Bikewale.Utility.ImageSize._642x361) %>" title="<%= bikeName %>" alt="<%= bikeName %>" border="0">
                                                    <% } else { %>
                                                    <img class="lazy" data-original="<%= Bikewale.Utility.Image.GetPathToShowImages(photo.OriginalImagePath,photo.HostUrl,Bikewale.Utility.ImageSize._642x361) %>" border="0" alt="<%= bikeName %>" title="<%= bikeName %>" src="http://imgd1.aeplcdn.com/0x0/bw/static/sprites/d/loader.gif">
                                                    <% } %>
                                                </span>
                                            </div>
                                        </li>
                                        <% } %>                                         
                                    </ul>
                                </div>
                                 <% if(inquiryDetails.PhotosCount > 1){ %>
                                <div class="model-media-details">
                                    <div class="model-media-item">
                                        <span class="bwsprite gallery-photo-icon"></span>
                                        <span class="model-media-count"><%= inquiryDetails.PhotosCount %></span>
                                    </div>
                                </div>
                                 <span class="jcarousel-control-left">
                                    <a href="#" class="bwsprite jcarousel-control-prev inactive" rel="nofollow" ></a>
                                </span>
                                <span class="jcarousel-control-right">
                                    <a href="#" class="bwsprite jcarousel-control-next" rel="nofollow" ></a>
                                </span>
                                 <% } %>
                               
                            </div>

                        <% } else { %>
                            <div id="bike-no-image">
                                <div class=" no-image-content ">
                                    <span class="bwsprite no-image-icon"></span>
                                    <p class="font12 text-bold text-light-grey margin-top5 margin-bottom15">Seller has not uploaded any photos</p>
                        
                                    <a href="javascript:void(0)" id="request-media-btn" class="btn btn-inv-teal btn-sm font14 text-bold" rel="nofollow">Request photos</a>
                                </div>
                            </div>
                           <% } %>
                        </div>
                    <% if (inquiryDetails.MinDetails != null) { %> 
                        <div id="ad-summary" class="grid-7 padding-left30 omega font14">
                            <h2 class="text-default ad-summary-label margin-bottom10">Ad summary</h2>
                               <% if (!string.IsNullOrEmpty(modelYear))
                       { %>
                            <div class="grid-4 alpha margin-bottom10">
                                <span class="bwsprite model-date-icon"></span>
                                <span class="model-details-label">2011 model</span>
                            </div>
                                <% } %>
                    <% if (inquiryDetails.MinDetails.KmsDriven > 0)
                       { %>
                            <div class="grid-4 alpha omega margin-bottom10">
                                <span class="bwsprite kms-driven-icon"></span>
                                <span class="model-details-label">18,000 kms</span>
                            </div>
                            <div class="clear"></div>
                             <% } %>
                    <% if (!string.IsNullOrEmpty(inquiryDetails.MinDetails.OwnerType))
                       { %>
                            <div class="grid-4 alpha margin-bottom10">
                                <span class="bwsprite author-grey-sm-icon"></span>
                                <span class="model-details-label">1st Owner</span>
                            </div>
                            <% } %>
                    <% if (!string.IsNullOrEmpty(inquiryDetails.MinDetails.RegisteredAt))
                       { %>
                            <div class="grid-4 alpha omega margin-bottom10">
                                <span class="bwsprite model-loc-icon"></span>
                                <span class="model-details-label">Kayamkulam</span>
                            </div>
                            <div class="clear"></div>
                            <% } %>
                            <div class="margin-top5 margin-bottom10">
                                <span class="bwsprite inr-md-lg"></span>
                                <span class="font24 text-bold"><%= Bikewale.Utility.Format.FormatPrice(inquiryDetails.MinDetails.AskingPrice.ToString()) %></span>
                            </div>
                            <a href="javascript:void(0)" class="btn btn-orange ad-summary-btn font14 used-bike-lead" rel="nofollow" data-profile-id="<%= profileId %>" data-ga-cat="Used_Bike_Detail" data-ga-act="Get_Seller_Details_Clicked" data-ga-lab="<%= profileId %>">Get seller details</a>
                        </div>
                    <% } %>
                        <div class="clear"></div>

                        <div class="margin-bottom20 border-solid-bottom"></div>
                    <% if (inquiryDetails.OtherDetails != null)
                   { %>
                        <h2 class="text-default margin-bottom15">Bike details</h2>
                        <div class="grid-6 alpha border-solid-right margin-bottom20">
                            <ul class="key-value-list font14">
                                <li>
                                    <p class="bike-details-key">Profile ID</p>
                                    <p class="bike-details-value">S<%= inquiryDetails.OtherDetails.Id %></p>
                                </li>
                                <li>
                                    <p class="bike-details-key">Date updated</p>
                                    <p class="bike-details-value"><%= Bikewale.Utility.FormatDate.GetDDMMYYYY(inquiryDetails.OtherDetails.LastUpdatedOn.ToString()) %></p>
                                </li>
                                <li>
                                    <p class="bike-details-key">Seller</p>
                                    <p class="bike-details-value"><%= inquiryDetails.OtherDetails.Seller %></p>
                                </li>
                                <li>
                                    <p class="bike-details-key">Registration year</p>
                                    <p class="bike-details-value"><%= Bikewale.Utility.FormatDate.GetFormatDate(inquiryDetails.MinDetails.ModelYear.ToString(),"MMM yyyy") %></p>
                                </li>
                            </ul>
                        </div>
                        <div class="grid-6 padding-left40 omega margin-bottom20">
                            <ul class="key-value-list font14">
                                <% if (!string.IsNullOrEmpty(inquiryDetails.OtherDetails.Color.ColorName))
                           { %>
                                <li>
                                    <p class="bike-details-key">Colour</p>
                                    <p class="bike-details-value"><%= inquiryDetails.OtherDetails.Color.ColorName %></p>
                                </li>
                                     <%} %>
                                <li>
                                    <p class="bike-details-key">Bike registered at</p>
                                    <p class="bike-details-value"><%= inquiryDetails.OtherDetails.RegisteredAt %></p>
                                </li>
                                <li>
                                    <p class="bike-details-key">Insurance</p>
                                    <p class="bike-details-value"><%= inquiryDetails.OtherDetails.Insurance %></p>
                                </li>
                                <li>
                                    <p class="bike-details-key">Registration no.</p>
                                    <p class="bike-details-value"><%= inquiryDetails.OtherDetails.RegistrationNo %></p>
                                </li>
                            </ul>
                        </div>
                        <div class="clear"></div>

                         <% if (!string.IsNullOrEmpty(inquiryDetails.OtherDetails.Description))
                       { %>

                        <div class="margin-bottom20 border-solid-bottom"></div>

                        <h2 class="text-default margin-bottom15">Ad description</h2>
                        <p class="font14 text-light-grey"><%= inquiryDetails.OtherDetails.Description %></p>
                    <%  } } %>
                    </div>
                </div>
                <div class="clear"></div>
            </div>
        </section>

        <section>
            <div class="container">
                <div id="makeTabsContentWrapper" class="grid-12 margin-bottom20">
                    <div class="content-box-shadow">
                        <div id="makeOverallTabsWrapper">
                            <div id="makeOverallTabs" class="overall-floating-tabs">
                                <div class="overall-specs-tabs-wrapper">
                                    <% if (inquiryDetails.SpecsFeatures != null)
                                       { %>
                                    <a href="#specsContent" rel="nofollow">Specs & Features</a>
                                    <% } %>
                                    <% if (ctrlSimilarUsedBikes.FetchedRecordsCount > 0)
                                       { %>
                                    <a href="#similarContent" rel="nofollow">Similar bikes</a>
                                    <% } %>
                                    <% if (ctrlOtherUsedBikes.FetchedRecordsCount > 0)
                                       { %>
                                    <a href="#usedContent" rel="nofollow">Used Bajaj bikes</a>
                                </div>
                                <% } %>
                            </div>
                        </div>
                                        <% if (inquiryDetails.SpecsFeatures != null)
                   { %>
                        <div id="specsContent" class="bw-model-tabs-data specs-features-list font14">
                            <h2 class="content-inner-block-20">Specifications summary</h2>
                            <div class="grid-4 omega">
                                <div class="grid-6 text-light-grey">
                                    <p>Displacement</p>
                                    <p>Max Power</p>
                                    <p>Maximum Torque</p>
                                    <p>No. of gears</p>
                                </div>
                                <div class="grid-6 omega text-bold">
                                    <p><%= Bikewale.Utility.FormatMinSpecs.ShowAvailable(inquiryDetails.SpecsFeatures.Displacement,"cc") %></p>
                                    <p title="<%= Bikewale.Utility.FormatMinSpecs.ShowAvailable(inquiryDetails.SpecsFeatures.MaxPower, "bhp",inquiryDetails.SpecsFeatures.MaxPowerRPM, "rpm") %>"><%= Bikewale.Utility.FormatMinSpecs.ShowAvailable(inquiryDetails.SpecsFeatures.MaxPower, "bhp",inquiryDetails.SpecsFeatures.MaxPowerRPM, "rpm") %></p>
                                    <p title="<%= Bikewale.Utility.FormatMinSpecs.ShowAvailable(inquiryDetails.SpecsFeatures.MaximumTorque, "Nm",inquiryDetails.SpecsFeatures.MaximumTorqueRPM, "rpm") %>"><%= Bikewale.Utility.FormatMinSpecs.ShowAvailable(inquiryDetails.SpecsFeatures.MaximumTorque, "Nm",inquiryDetails.SpecsFeatures.MaximumTorqueRPM, "rpm") %></p>
                                    <p><%= inquiryDetails.SpecsFeatures.NoOfGears %></p>
                                </div>
                                <div class="clear"></div>
                            </div>
                            <div class="grid-4 omega">
                                <div class="grid-6 text-light-grey">
                                   <p>Mileage</p>
                                    <p>Brake Type</p>
                                    <p>Front Disc</p>
                                    <p>Rear Disc</p>
                                </div>
                                <div class="grid-6 omega text-bold">
                                   <p><%=  Bikewale.Utility.FormatMinSpecs.ShowAvailable(inquiryDetails.SpecsFeatures.FuelEfficiencyOverall,"kmpl") %></p>
                                    <p><%= Bikewale.Utility.FormatMinSpecs.ShowAvailable(inquiryDetails.SpecsFeatures.BrakeType) %></p>
                                    <p><%= Bikewale.Utility.FormatMinSpecs.ShowAvailable(inquiryDetails.SpecsFeatures.FrontDisc) %></p>
                                    <p><%= Bikewale.Utility.FormatMinSpecs.ShowAvailable(inquiryDetails.SpecsFeatures.RearDisc) %></p>
                                
                                </div>
                                <div class="clear"></div>
                            </div>
                            <div class="grid-4">
                                <div class="grid-6 text-light-grey">
                                    <p>Alloy Wheels</p>
                                    <p>Kerb Weight</p>
                                    <p>Top Speed</p>
                                    <p>Fuel Tank Capacity</p>
                                </div>
                                <div class="grid-6 alpha text-bold">
                                    <p><%= Bikewale.Utility.FormatMinSpecs.ShowAvailable(inquiryDetails.SpecsFeatures.AlloyWheels) %></p>
                                    <p><%= Bikewale.Utility.FormatMinSpecs.ShowAvailable(inquiryDetails.SpecsFeatures.KerbWeight,"kg") %></p>
                                    <p><%= Bikewale.Utility.FormatMinSpecs.ShowAvailable(inquiryDetails.SpecsFeatures.TopSpeed, "kmph") %></p>
                                    <p><%= Bikewale.Utility.FormatMinSpecs.ShowAvailable(inquiryDetails.SpecsFeatures.FuelTankCapacity, "litres") %></p>
                                
                                </div>
                                <div class="clear"></div>
                            </div>
                            <div class="clear"></div>
                            <div class="padding-left20 margin-bottom10">
                                <a href="<%= moreBikeSpecsUrl %>" title="<%= string.Format("{0} Specifications",bikeName) %>">View full specifications<span class="bwsprite blue-right-arrow-icon"></span></a>
                            </div>

                            <div class="grid-8 alpha margin-bottom25">
                                <h2 class="content-inner-block-20">Features summary</h2>
                                <div class="grid-6 omega">
                                    <div class="grid-6 text-light-grey">
                                       <p>Speedometer</p>
                                        <p>Fuel Guage</p>
                                        <p>Tachometer Type</p>
                                    </div>
                                    <div class="grid-6 omega text-bold">
                                        <p><%= Bikewale.Utility.FormatMinSpecs.ShowAvailable(inquiryDetails.SpecsFeatures.Speedometer) %></p>
                                        <p><%= Bikewale.Utility.FormatMinSpecs.ShowAvailable(inquiryDetails.SpecsFeatures.FuelGauge) %></p>
                                        <p><%= Bikewale.Utility.FormatMinSpecs.ShowAvailable(inquiryDetails.SpecsFeatures.DigitalFuelGauge) %></p>                                    
                                    </div>
                                    <div class="clear"></div>
                                </div>
                                <div class="grid-6 padding-left15">
                                    <div class="grid-6 omega text-light-grey">
                                      <p>Digital Fuel Guage</p>
                                        <p>Tripmeter</p>
                                        <p>Electric Start</p>
                                    </div>
                                    <div class="grid-6 padding-left20 omega text-bold">
                                        <p><%= Bikewale.Utility.FormatMinSpecs.ShowAvailable(inquiryDetails.SpecsFeatures.DigitalFuelGauge) %></p>
                                        <p><%= Bikewale.Utility.FormatMinSpecs.ShowAvailable(inquiryDetails.SpecsFeatures.Tripmeter) %></p>
                                        <p><%= Bikewale.Utility.FormatMinSpecs.ShowAvailable(inquiryDetails.SpecsFeatures.ElectricStart) %></p>                                    
                                    </div>
                                    <div class="clear"></div>
                                </div>
                                <div class="clear"></div>
                                <div class="padding-left20">
                                    <a href="<%= moreBikeFeaturesUrl %>" title="<%= string.Format("{0} Features",bikeName) %>">View all features <span class="bwsprite blue-right-arrow-icon"></span></a>
                                </div>
                            </div>
                            <div class="grid-4 text-center alpha margin-bottom25">
                               <!-- #include file="/ads/Ad300x250BTF.aspx" -->
                            </div>
                            <div class="clear"></div>
                            <div class="margin-right10 margin-left10 border-solid-bottom"></div>
                        </div>
                            <% } %>
                        <BW:SimilarUsedBikes runat="server" ID="ctrlSimilarUsedBikes"></BW:SimilarUsedBikes>
                        <!-- other used bikes starts -->
                        <BW:OtherUsedBikes ID="ctrlOtherUsedBikes" runat="server" />
                        <!-- other used bikes ends -->

                        <div id="overallMakeDetailsFooter"></div>
                    </div>
                </div>
                <div class="clear"></div>
            </div>
        </section>

        <% if (inquiryDetails.PhotosCount > 1)
           { %>
        <!-- gallery -->
        <section>
            <div class="blackOut-window-model"></div>
            <div class="bike-gallery-popup" id="bike-gallery-popup">
                <div class="modelgallery-close-btn bwsprite cross-lg-white cur-pointer"></div>
                <div class="bike-gallery-heading">
                    <p class="font18 text-bold margin-left30 text-white margin-bottom20"><%=modelYear %>, <%= bikeName %> Photos</p>

                    <div class="connected-carousels">
                        <div class="stage">
                            <div class="carousel carousel-stage">
                                <ul>
                                    <% foreach (var photo in inquiryDetails.Photo)
                                       { %>
                                    <li>
                                        <div class="stage-slide">
                                            <div class="stage-image-placeholder">
                                                <img class="lazy" data-original="<%= Bikewale.Utility.Image.GetPathToShowImages(photo.OriginalImagePath,photo.HostUrl,Bikewale.Utility.ImageSize._642x361) %>" alt="<%= bikeName %>" title="<%= bikeName %>" src="http://imgd1.aeplcdn.com/0x0/bw/static/sprites/d/loader.gif">
                                            </div>
                                        </div>
                                    </li>
                                    <% } %>
                                </ul>
                            </div>
                            <div class="bike-gallery-details">
                                <span class="rightfloat bike-gallery-count"></span>
                            </div>
                            <a href="#" class="prev photos-prev-stage bwsprite" rel="nofollow"></a>
                            <a href="#" class="next photos-next-stage bwsprite" rel="nofollow"></a>
                        </div>

                        <div class="navigation">
                            <a href="#" class="prev photos-prev-navigation bwsprite" rel="nofollow"></a>
                            <a href="#" class="next photos-next-navigation bwsprite" rel="nofollow"></a>
                            <div class="carousel carousel-navigation">
                                <ul>
                                    <% foreach (var photo in inquiryDetails.Photo)
                                       { %>
                                    <li>
                                        <div class="navigation-slide">
                                            <div class="navigation-image-placeholder">
                                                <img class="lazy" data-original="<%= Bikewale.Utility.Image.GetPathToShowImages(photo.OriginalImagePath,photo.HostUrl,Bikewale.Utility.ImageSize._642x361) %>" alt="<%= bikeName %>" src="http://imgd1.aeplcdn.com/0x0/bw/static/sprites/d/loader.gif">
                                            </div>
                                        </div>
                                    </li>
                                    <% } %>
                                </ul>
                            </div>
                        </div>
                    </div>

                </div>
            </div>
        </section>
        <% } %>

        <script type="text/javascript" src="<%= staticUrl != "" ? "http://st1.aeplcdn.com" + staticUrl : "" %>/src/frameworks.js?<%=staticFileVersion %>"></script>

        <%if (inquiryDetails.PhotosCount == 0)
          { %>
        <BW:UploadPhotoRequestPopup runat="server" ID="widgetUploadPhotoRequest"></BW:UploadPhotoRequestPopup>
        <%} %>

        <BW:UBLeadCapturePopup runat="server" ID="ctrlUBLeadCapturePopup"></BW:UBLeadCapturePopup>
        <!-- #include file="/includes/footerBW.aspx" -->
        <link href="<%= staticUrl != "" ? "http://st2.aeplcdn.com" + staticUrl : "" %>/css/bw-common-btf.css?<%=staticFileVersion %>" rel="stylesheet" type="text/css" />
        <script type="text/javascript" src="<%= staticUrl != string.Empty ? "http://st2.aeplcdn.com" + staticUrl : string.Empty %>/src/common.min.js?<%= staticFileVersion %>"></script>
        <script type="text/javascript" src="<%= staticUrl != string.Empty ? "http://st2.aeplcdn.com" + staticUrl : string.Empty %>/src/used-details.js?<%= staticFileVersion %>"></script>
        <link href='https://fonts.googleapis.com/css?family=Open+Sans:400,700' rel='stylesheet' type='text/css' />
        <!--[if lt IE 9]>
            <script src="/src/html5.js"></script>
        <![endif]-->
    </form>
</body>
</html>
