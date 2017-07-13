﻿<%@ Page Language="C#" AutoEventWireup="false" EnableViewState="false" Inherits="Bikewale.Mobile.Used.BikeDetails" %>

<%@ Register Src="~/m/controls/SimilarUsedBikes.ascx" TagPrefix="BW" TagName="SimilarUsedBikes" %>
<%@ Register Src="~/m/controls/UploadPhotoRequestPopup.ascx" TagPrefix="BW" TagName="UploadPhotoRequestPopup" %>
<%@ Register Src="~/m/controls/UsedBikeLeadCaptureControl.ascx" TagPrefix="BW" TagName="UBLeadCapturePopup" %>
<%@ Register Src="~/m/controls/ServiceCenterCard.ascx" TagName="ServiceCenterCard" TagPrefix="BW" %>
<%@ Register Src="~/m/controls/usedBikeModel.ascx" TagName="usedBikeModel" TagPrefix="BW" %>
<!DOCTYPE html>
<html>
<head>
    <%
        title = pgTitle;
        keywords = pgKeywords;
        description = pgDescription;
        canonical = pgCanonicalUrl;
        OGImage = (firstImage != null) ? Bikewale.Utility.Image.GetPathToShowImages(firstImage.OriginalImagePath, firstImage.HostUrl, Bikewale.Utility.ImageSize._360x202) : string.Empty;
        AdPath = "/1017752/BikeWale_Mobile_UsedBikes_Details_";
        AdId = "1475577296808";
        Ad_320x50 = true;
        Ad_Bot_320x50 = true;
        Ad_300x250 = false;
        TargetedModel = (inquiryDetails != null && inquiryDetails.Model != null) ? inquiryDetails.Model.ModelName : string.Empty;
        TargetedCity = (inquiryDetails != null && inquiryDetails.City != null) ? inquiryDetails.City.CityName : string.Empty;
        ShowSellBikeLink = true;
    %>
    <!-- #include file="/includes/headscript_mobile_min.aspx" -->
    <link rel="stylesheet" type="text/css" href="/m/css/used/details.css" />
    <script type="text/javascript">
        <!-- #include file="\includes\gacode_mobile.aspx" -->
    </script>
</head>
<body>
    <form id="form1" runat="server">
        <!-- #include file="/includes/headBW_Mobile.aspx" -->
        <% if(inquiryDetails!=null) { %>
        <% if(!isBikeSold) { %>
        <section>
            <div class="container bg-white clearfix box-shadow margin-bottom10">
                <h1 class="font16 padding-top15 padding-right20 padding-bottom15 padding-left20"><%= modelYear %>, <%= bikeName %></h1>
                <div id="model-main-image">
                    <%if (inquiryDetails.PhotosCount > 0)
                      { %>
                    <a href="javascript:void(0)" class="model-main-image-wrapper <%= inquiryDetails.PhotosCount > 1 ? "model-gallery-target " : string.Empty %>" rel="nofollow">
                        <img src="<%= (firstImage!=null) ? Bikewale.Utility.Image.GetPathToShowImages(firstImage.OriginalImagePath,firstImage.HostUrl,Bikewale.Utility.ImageSize._360x202) : string.Empty %>" alt="Used <%= String.Format("{0} {1}", modelYear, bikeName) %>" title="Used <%= String.Format("{0} {1}", modelYear, bikeName) %>" />
                        <% if (inquiryDetails.PhotosCount > 1)
                           { %>
                        <div class="model-media-details">
                            <div class="model-media-item">
                                <span class="bwmsprite photos-sm"></span>
                                <span class="model-media-count"><%= inquiryDetails.PhotosCount %></span>
                            </div>
                        </div>
                        <% } %>
                    </a>
                    <% }
                      else
                      { %>
                    <div class=" no-image-content ">
                        <span class="bwmsprite no-image-icon"></span>
                        <p class="font12 text-bold text-light-grey margin-top5 margin-bottom15">Seller has not uploaded any images</p>
                        <% if (!isPhotoRequestDone)
                           { %>
                        <a href="javascript:void(0)" id="request-media-btn" class="btn btn-inv-teal btn-sm font14 text-bold" rel="nofollow">Request images</a>
                        <% } %>
                    </div>
                    <% } %>
                </div>
                <% if (inquiryDetails.MinDetails != null)
                   { %>

                <div class="margin-right20 margin-left20 padding-top15 padding-bottom15 border-bottom-light font14">
                    <% if (!string.IsNullOrEmpty(modelYear))
                       { %>
                    <div class="grid-6 alpha omega margin-bottom5">
                        <span class="bwmsprite model-date-icon"></span>
                        <span class="model-details-label"><%= modelYear %> model</span>
                    </div>
                    <% } %>
                    <% if (inquiryDetails.MinDetails.KmsDriven > 0)
                       { %>
                    <div class="grid-6 alpha omega margin-bottom5">
                        <span class="bwmsprite kms-driven-icon"></span>
                        <span class="model-details-label"><%= Bikewale.Utility.Format.FormatPrice(inquiryDetails.MinDetails.KmsDriven.ToString()) %> kms</span>
                    </div>
                    <% } %>
                    <% if (!string.IsNullOrEmpty(inquiryDetails.MinDetails.OwnerType))
                       { %>
                    <div class="grid-6 alpha omega margin-bottom5">
                        <span class="bwmsprite author-grey-sm-icon"></span>
                        <span class="model-details-label"><%= Bikewale.Utility.Format.AddNumberOrdinal(Convert.ToUInt16(inquiryDetails.MinDetails.OwnerType),4) %> owner</span>
                    </div>
                    <% } %>
                    <% if (inquiryDetails.City != null && !string.IsNullOrEmpty(inquiryDetails.City.CityName))
                       { %>
                    <div class="grid-6 alpha omega margin-bottom5">
                        <span class="bwmsprite model-loc-icon"></span>
                        <span class="model-details-label"><%= inquiryDetails.City.CityName %></span>
                    </div>
                    <%} %>
                    <div class="clear"></div>
                    <p><span class="bwmsprite inr-md-icon"></span>&nbsp;<span class="font22 text-bold"><%= Bikewale.Utility.Format.FormatPrice(inquiryDetails.MinDetails.AskingPrice.ToString()) %></span></p>
                </div>

                <div class="grid-12 float-button float-fixed clearfix">
                    <div class="grid-12 alpha omega">
                        <a id="get-seller-button" class="btn btn-orange btn-full-width rightfloat used-bike-lead" data-profile-id="<%= profileId %>" data-ga-cat="Used_Bike_Detail" data-ga-act="Get_Seller_Details_Clicked" data-ga-lab="<%= profileId %>" href="javascript:void(0);" rel="nofollow">Get seller details</a>
                    </div>
                </div>
                <% } %>

                <% if (inquiryDetails.OtherDetails != null)
                   { %>
                <div class="margin-right20 margin-left20 padding-top15 padding-bottom20 font14">
                    <p class="text-bold margin-bottom15">Bike details</p>
                    <ul class="specs-features-list">
                        <li>
                            <p class="specs-features-label">Profile ID</p>
                            <p class="specs-features-value"><%= profileId %></p>
                        </li>
                        <li>
                            <p class="specs-features-label">Date updated</p>
                            <p class="specs-features-value"><%= Bikewale.Utility.FormatDate.GetDDMMYYYY(inquiryDetails.OtherDetails.LastUpdatedOn.ToString()) %></p>
                        </li>
                        <li>
                            <p class="specs-features-label">Seller</p>
                            <p class="specs-features-value"><%= inquiryDetails.OtherDetails.Seller.Equals("s",StringComparison.CurrentCultureIgnoreCase) ? "Individual" : "Dealer" %></p>
                        </li>
                        <li>
                            <p class="specs-features-label">Manufacturing year</p>
                            <p class="specs-features-value"><%= Bikewale.Utility.FormatDate.GetFormatDate(inquiryDetails.MinDetails.ModelYear.ToString(),"MMMM yyyy") %></p>
                        </li>
                        <% if (!string.IsNullOrEmpty(inquiryDetails.OtherDetails.Color.ColorName))
                           { %>
                        <li>
                            <p class="specs-features-label">Colour</p>
                            <p class="specs-features-value"><%= inquiryDetails.OtherDetails.Color.ColorName %></p>
                        </li>
                        <%} %>
                        <li>
                            <p class="specs-features-label">Bike registered at</p>
                            <p class="specs-features-value"><%= inquiryDetails.OtherDetails.RegisteredAt %></p>
                        </li>
                         <% if (!String.IsNullOrEmpty(inquiryDetails.OtherDetails.Insurance)){ %>
                        <li>
                            <p class="specs-features-label">Insurance</p>
                            <p class="specs-features-value"><%= inquiryDetails.OtherDetails.Insurance %></p>
                        </li>
                        <%} %>
                        <% if (!String.IsNullOrEmpty(inquiryDetails.OtherDetails.RegistrationNo)){ %>
                        <li>
                            <p class="specs-features-label">Registration no.</p>
                            <p class="specs-features-value"><%= inquiryDetails.OtherDetails.RegistrationNo %></p>
                        </li>
                        <%} %>
                    </ul>
                    <div class="clear"></div>
                    <% if (!string.IsNullOrEmpty(inquiryDetails.OtherDetails.Description))
                       { %>
                    <div class="margin-bottom15 padding-top15 border-bottom-light"></div>
                    <p class="text-bold margin-bottom15">Ad description</p>
                    <p class="text-light-grey"><%= inquiryDetails.OtherDetails.Description %></p>
                    <% } %>
                </div>
                <% } %>
            </div>
        </section>
        <% } else { %>
        <section>
                <div class="container bg-white clearfix box-shadow margin-bottom10">
                    <div class="font14 padding-top15 padding-right20 padding-bottom15 padding-left20">
                    <span class="used-sprite sold-icon"></span>
                       
                        <div class="bike-sold-msg text-grey "> <h1><%= bikeName %></h1> The <%= bikeName %> bike you are looking for has been sold. You might want to consider other used bikes shown below.</div>
                </div> </div>
        </section>
        <% } %>

        <script type="text/javascript" src="<%= staticUrl %>/m/src/frameworks.js?<%= staticFileVersion %>"></script>
                <% if ((inquiryDetails.SpecsFeatures != null && !isBikeSold) || ctrlusedBikeModel.FetchCount > 0 || ctrlSimilarUsedBikes.FetchedRecordsCount > 0)
           { %>
        <section>
            <div id="model-bottom-card-wrapper" class="container bg-white clearfix box-shadow margin-bottom30">
                <div id="model-overall-specs-wrapper">
                    <div id="overall-specs-tab" class="overall-specs-tabs-container">
                        <ul class="overall-specs-tabs-wrapper">
                            <% if (inquiryDetails.SpecsFeatures != null && !isBikeSold)
                               { %>
                            <li data-tabs="#modelSpecs" class="active">Specifications</li>
                            <li data-tabs="#modelFeatures">Features</li>
                            <% } %>
                            <% if (ctrlSimilarUsedBikes.FetchedRecordsCount > 0)
                               { %>
                            <li class="<%= (inquiryDetails.SpecsFeatures!=null)?string.Empty:"active" %>" data-tabs="#modelSimilar">Similar bikes</li>
                            <% } %>
                            <% if (ctrlusedBikeModel.FetchCount > 0)
                               { %>
                            <li data-tabs="#modelOtherBikes">Other bikes</li>
                            <% } %>
                        </ul>
                    </div>
                </div>

                <% if (inquiryDetails.SpecsFeatures != null && !isBikeSold)
                   { %>
                <div id="modelSpecs" class="bw-model-tabs-data margin-right20 margin-left20 padding-top15 padding-bottom20 font14 border-solid-bottom">
                    <h2 class="margin-bottom20">Specification summary</h2>
                    <ul class="specs-features-list">
                        <li>
                            <p class="specs-features-label">Displacement</p>
                            <p class="specs-features-value"><%= Bikewale.Utility.FormatMinSpecs.ShowAvailable(inquiryDetails.SpecsFeatures.Displacement,"cc") %></p>
                        </li>
                        <li>
                            <p class="specs-features-label">Max Power</p>
                            <p class="specs-features-value"><%= Bikewale.Utility.FormatMinSpecs.ShowAvailable(inquiryDetails.SpecsFeatures.MaxPower, "bhp",inquiryDetails.SpecsFeatures.MaxPowerRPM, "rpm") %></p>
                        </li>
                        <li>
                            <p class="specs-features-label">Maximum Torque</p>
                            <p class="specs-features-value"><%= Bikewale.Utility.FormatMinSpecs.ShowAvailable(inquiryDetails.SpecsFeatures.MaximumTorque, "Nm",inquiryDetails.SpecsFeatures.MaximumTorqueRPM, "rpm") %></p>
                        </li>
                        <li>
                            <p class="specs-features-label">No. of gears</p>
                            <p class="specs-features-value"><%= inquiryDetails.SpecsFeatures.NoOfGears %></p>
                        </li>
                        <li>
                            <p class="specs-features-label">Mileage</p>
                            <p class="specs-features-value"><%= Bikewale.Utility.FormatMinSpecs.ShowAvailable(inquiryDetails.SpecsFeatures.FuelEfficiencyOverall, "kmpl") %></p>
                        </li>
                        <li>
                            <p class="specs-features-label">Brake Type</p>
                            <p class="specs-features-value"><%= Bikewale.Utility.FormatMinSpecs.ShowAvailable(inquiryDetails.SpecsFeatures.BrakeType) %></p>
                        </li>
                    </ul>
                    <div class="clear"></div>

                    <div class="margin-top15">
                        <a href="/m<%= moreBikeSpecsUrl %>" title="<%= string.Format("{0} Specifications",bikeName) %>">View full specifications<span class="bwmsprite blue-right-arrow-icon"></span></a>
                    </div>
                </div>

                <div id="modelFeatures" class="bw-model-tabs-data margin-right20 margin-left20 padding-top20 padding-bottom20 font14 border-solid-bottom">
                    <h2 class="margin-bottom20">Features summary</h2>
                    <ul class="specs-features-list">
                        <li>
                            <p class="specs-features-label">Speedometer</p>
                            <p class="specs-features-value"><%= Bikewale.Utility.FormatMinSpecs.ShowAvailable(inquiryDetails.SpecsFeatures.Speedometer) %></p>
                        </li>
                        <li>
                            <p class="specs-features-label">Fuel Guage</p>
                            <p class="specs-features-value"><%= Bikewale.Utility.FormatMinSpecs.ShowAvailable(inquiryDetails.SpecsFeatures.FuelGauge) %></p>
                        </li>
                        <li>
                            <p class="specs-features-label">Tachometer Type</p>
                            <p class="specs-features-value"><%= Bikewale.Utility.FormatMinSpecs.ShowAvailable(inquiryDetails.SpecsFeatures.TachometerType) %></p>
                        </li>
                        <li>
                            <p class="specs-features-label">Digital Fuel Guage</p>
                            <p class="specs-features-value"><%= Bikewale.Utility.FormatMinSpecs.ShowAvailable(inquiryDetails.SpecsFeatures.DigitalFuelGauge) %></p>
                        </li>
                        <li>
                            <p class="specs-features-label">Tripmeter</p>
                            <p class="specs-features-value"><%= Bikewale.Utility.FormatMinSpecs.ShowAvailable(inquiryDetails.SpecsFeatures.Tripmeter) %></p>
                        </li>
                        <li>
                            <p class="specs-features-label">Electric Start</p>
                            <p class="specs-features-value"><%= Bikewale.Utility.FormatMinSpecs.ShowAvailable(inquiryDetails.SpecsFeatures.ElectricStart) %></p>
                        </li>
                    </ul>
                    <div class="clear"></div>
                    <div class="margin-top15">
                        <a href="/m<%= moreBikeFeaturesUrl %>" title="<%= string.Format("{0} Features",bikeName) %>">View full features<span class="bwmsprite blue-right-arrow-icon"></span></a>
                    </div>
                </div>
                <% } %>
                <!-- Similar used bikes starts -->
                <BW:SimilarUsedBikes ID="ctrlSimilarUsedBikes" runat="server" />
                <!-- Similar used bikes ends -->
                <!-- Other used bikes starts -->
                  <% if (ctrlusedBikeModel.FetchCount>0)
                       { %>
                 <div id="modelOtherBikes" class="bw-model-tabs-data padding-top15 active"><BW:usedBikeModel runat="server" ID="ctrlusedBikeModel" /></div>
                    
                        
                    <% } %> 
                <!-- Other used bikes ends -->
            </div>
            <div id="modelSpecsFooter"></div>
        </section>
        <% } %>
        <% if (inquiryDetails.PhotosCount > 1)
           { %>
        <!-- gallery start -->
        <div id="model-gallery-container">
            <p class="font16 text-white gallery-header"><%=modelYear %>, <%= bikeName %> Images</p>
            <div class="gallery-close-btn position-abt pos-top15 pos-right15 bwmsprite cross-md-white cur-pointer"></div>

            <div id="bike-gallery-popup">
                <div class="font14 text-white margin-bottom15">
                    <%--<span class="leftfloat media-title"></span>--%>
                    <span class="rightfloat gallery-count"></span>
                    <div class="clear"></div>
                </div>
                <div class="connected-carousels-photos">
                    <div class="stage-photos">
                        <div class="swiper-container noSwiper carousel-photos carousel-stage-photos">
                            <div class="swiper-wrapper">
                                <asp:Repeater ID="rptUsedBikePhotos" runat="server">
                                    <ItemTemplate>
                                        <div class="swiper-slide">
                                            <img class="swiper-lazy" data-src="<%# Bikewale.Utility.Image.GetPathToShowImages(DataBinder.Eval(Container.DataItem, "OriginalImagePath").ToString(),DataBinder.Eval(Container.DataItem, "HostUrl").ToString(),Bikewale.Utility.ImageSize._360x202) %>" alt="<%= bikeName %>" title="<%= bikeName %>" />
                                            <span class="swiper-lazy-preloader"></span>
                                        </div>

                                    </ItemTemplate>
                                </asp:Repeater>
                            </div>
                            <div class="bwmsprite swiper-button-next"></div>
                            <div class="bwmsprite swiper-button-prev"></div>
                        </div>
                    </div>

                    <div class="navigation-photos">
                        <div class="swiper-container noSwiper carousel-navigation-photos">
                            <div class="swiper-wrapper">
                                <asp:Repeater ID="rptUsedBikeNavPhotos" runat="server">
                                    <ItemTemplate>
                                        <div class="swiper-slide">
                                            <div class="navigation-image-wrapper">
                                                <img class="swiper-lazy" data-src="<%# Bikewale.Utility.Image.GetPathToShowImages(DataBinder.Eval(Container.DataItem, "OriginalImagePath").ToString(),DataBinder.Eval(Container.DataItem, "HostUrl").ToString(),Bikewale.Utility.ImageSize._110x61) %>" alt="<%= bikeName %>" title="<%= bikeName %>" />
                                                <span class="swiper-lazy-preloader"></span>
                                            </div>
                                        </div>
                                    </ItemTemplate>
                                </asp:Repeater>
                            </div>
                            <div class="bwmsprite swiper-button-next hide"></div>
                            <div class="bwmsprite swiper-button-prev hide"></div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
        <!-- gallery end -->
        <% } %>

        <%if (inquiryDetails.PhotosCount == 0)
          { %>

        <BW:UploadPhotoRequestPopup runat="server" ID="widgetUploadPhotoRequest"></BW:UploadPhotoRequestPopup>
        <%} %>
        <BW:UBLeadCapturePopup runat="server" ID="ctrlUBLeadCapturePopup"></BW:UBLeadCapturePopup>

        <% } %>
        
         <% if (ctrlServiceCenterCard.showWidget)
                   { %>
        <div class="container bg-white box-shadow padding-top15 margin-bottom10">
                    <BW:ServiceCenterCard runat="server" ID="ctrlServiceCenterCard" />
            </div>    
            <%}%>

        <!-- #include file="/includes/footerBW_Mobile.aspx" -->
        <link href="<%= staticUrl  %>/m/css/bwm-common-btf.css?<%= staticFileVersion %>" rel="stylesheet" type="text/css" />
        <!-- #include file="/includes/footerscript_mobile.aspx" -->
        <script type="text/javascript" src="<%= staticUrl  %>/m/src/used-details.js?<%= staticFileVersion%>"></script>        
        <!-- #include file="/includes/fontBW_Mobile.aspx" -->
    </form>
</body>
</html>
