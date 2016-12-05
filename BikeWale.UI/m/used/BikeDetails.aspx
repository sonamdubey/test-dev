<%@ Page Language="C#" AutoEventWireup="false" EnableViewState="false" Inherits="Bikewale.Mobile.Used.BikeDetails" %>

<%@ Register Src="~/m/controls/SimilarUsedBikes.ascx" TagPrefix="BW" TagName="SimilarUsedBikes" %>
<%@ Register Src="~/m/controls/UsedOtherBikeByCity.ascx" TagPrefix="BW" TagName="OtherUsedBikes" %>
<%@ Register Src="~/m/controls/UploadPhotoRequestPopup.ascx" TagPrefix="BW" TagName="UploadPhotoRequestPopup" %>
<%@ Register Src="~/m/controls/UsedBikeLeadCaptureControl.ascx" TagPrefix="BW" TagName="UBLeadCapturePopup" %>
<%@ Register Src="~/m/controls/ServiceCenterCard.ascx" TagName="ServiceCenterCard" TagPrefix="BW" %>
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
    
    %>
    <!-- #include file="/includes/headscript_mobile_min.aspx" -->
    <style type="text/css">
        @charset "utf-8";.btn-inv-teal:hover,.model-media-item:hover{text-decoration:none}#model-main-image{width:100%;max-width:476px;margin:0 auto;height:200px;display:table;position:relative;text-align:center;background-color:#f5f5f5}.model-main-image-wrapper{width:100%;display:table-cell;vertical-align:middle;line-height:0}#model-main-image img{max-height:200px}.model-media-details{position:absolute;right:15px;bottom:10px;font-size:12px}.model-media-item{display:inline-block;vertical-align:middle;padding:4px 5px;color:#4d5057;background:rgba(255,255,255,.9);border-radius:2px}.model-media-count{position:relative;top:-1px}.gallery-photo-icon{width:16px;height:12px;background-position:0 -486px}.border-bottom-light{border-bottom:1px solid #eee}.btn-inv-teal{background:0 0;color:#3799a7;border:1px solid #3799a7}.btn-inv-teal:hover{background:#41b4c4;color:#fff;border:1px solid #41b4c4}.btn-inv-teal:focus{background:#37939f;border:1px solid #37939f}.btn-inv-teal.btn-sm{padding:6px 19px}.no-image-content{display:table-cell;vertical-align:middle}.kms-driven-icon,.model-date-icon,.model-loc-icon{width:10px;height:14px;margin-right:5px;vertical-align:middle}.model-date-icon{background-position:-140px -484px}.kms-driven-icon{background-position:-159px -484px}.model-loc-icon{background-position:-177px -484px}.author-grey-sm-icon{height:12px;margin-right:5px;vertical-align:middle}.model-details-label{width:85%;display:inline-block;vertical-align:middle;text-align:left;text-overflow:ellipsis;white-space:nowrap;overflow:hidden}.float-button{background-color:#f5f5f5}.float-button .btn{padding:8px 0;font-size:18px}.float-button.float-fixed{position:fixed;bottom:0;z-index:8;left:0;right:0;background:rgba(245,245,245,.9)}#model-overall-specs-wrapper{height:44px}.overall-specs-tabs-container{width:100%;display:block;background:#fff;overflow-x:auto;z-index:2;-webkit-overflow-scrolling:touch}.specs-features-list li p,.text-truncate{text-overflow:ellipsis;white-space:nowrap;overflow:hidden}.overall-specs-tabs-container.fixed-tab-nav{position:fixed;top:0;left:0}.overall-specs-tabs-wrapper{width:100%;display:table;border-bottom:1px solid #e2e2e2}.overall-specs-tabs-wrapper li{display:table-cell;width:1%;padding:10px 15px;text-align:center;white-space:nowrap;font-size:14px;color:#82888b;cursor:pointer}.overall-specs-tabs-container::-webkit-scrollbar{width:0;height:0}.overall-specs-tabs-container::-webkit-scrollbar-thumb,.overall-specs-tabs-container::-webkit-scrollbar-track{display:none}.overall-specs-tabs-wrapper li.active{border-bottom:3px solid #ef3f30;color:#4d5057;font-weight:700}.specs-features-list li{width:100%;margin-top:20px;float:left}.specs-features-list li:first-child{margin-top:0}.specs-features-list li p{float:left;padding-right:5px;text-align:left}.specs-features-list .specs-features-label{width:45%;color:#82888b}.specs-features-list .specs-features-value{width:55%;font-weight:700}.blue-right-arrow-icon{width:6px;height:10px;background-position:-58px -437px;position:relative;top:1px;left:7px}.block{display:block}#other-bike-swiper .swiper-slide,#similar-bike-swiper .swiper-slide{width:200px;min-height:190px;background:#fff}#other-bike-swiper .swiper-slide h3,#similar-bike-swiper .swiper-slide h3{margin-bottom:7px}.swiper-wrapper .swiper-slide:first-child{margin-left:10px}.model-swiper-image-preview{width:100%;height:auto;padding:5px 5px 0;display:table;text-align:center}.model-swiper-image-preview a{width:180px;height:101px;position:relative;display:table-cell;vertical-align:middle;background:#f5f5f5;line-height:0}.model-swiper-details{padding:5px 10px 10px}.swiper-shadow{-webkit-box-shadow:0 1px 4px rgba(0,0,0,.2);-moz-box-shadow:0 1px 4px rgba(0,0,0,.2);-ms-box-shadow:0 1px 4px rgba(0,0,0,.2);box-shadow:0 1px 4px rgba(0,0,0,.2);-webkit-border-radius:2px;-moz-border-radius:2px;-ms-border-radius:2px;border-radius:2px;border:1px solid #e2e2e2\9}#other-bike-swiper .model-details-label,#similar-bike-swiper .model-details-label{width:80%;color:#82888b}#other-bike-swiper .btn-xs,#similar-bike-swiper .btn-xs{padding:6px 0}.swiper-slide .model-swiper-image-preview img{height:101px}.author-grey-icon-xs,.kms-driven-icon-xs,.model-date-icon-xs,.model-loc-icon-xs{width:9px;height:10px;margin-right:3px;vertical-align:middle}.model-date-icon-xs{background-position:-140px -502px}.kms-driven-icon-xs{background-position:-156px -502px}.model-loc-icon-xs{background-position:-173px -502px}.author-grey-icon-xs{background-position:-196px -486px}#get-seller-details-popup.bwm-fullscreen-popup,#request-media-popup.bwm-fullscreen-popup{padding:30px 30px 100px;display:none}#mobile-verification-section,#request-sent-section,#seller-details-section,#update-mobile-content{display:none}.btn-fixed-width{padding-right:0;padding-left:0;width:205px}#submit-request-sent-btn.btn{padding:8px 67px}.user-contact-details-icon{width:29px;height:30px;background-position:-109px -216px}.otp-icon{width:29px;height:29px;background-position:-109px -177px}.edit-blue-icon{width:25px;height:25px;background-position:-110px -120px;position:relative;top:13px;cursor:pointer}.request-media-icon{width:32px;height:25px;background-position:-136px -428px;position:relative;top:2px}.thankyou-icon{width:24px;height:28px;background-position:-138px -394px}.no-image-icon{width:40px;height:30px;background-position:0 -510px}.margin-bottom35{margin-bottom:35px}.dealer-details-list li{margin-bottom:25px}.dealer-details-list .data-key{font-size:12px;color:#82888b}.dealer-details-list .data-value{font-size:16px;font-weight:700}.text-truncate{width:100%;text-align:left}input[type=text]:focus,input[type=number]:focus{outline:0;box-shadow:none}.input-box{height:60px;text-align:left}.input-box input{width:100%;display:block;padding:7px 0;border-bottom:1px solid #82888b;font-weight:700;color:#4d5057}.input-box label{position:absolute;top:4px;left:0;font-size:16px;color:#82888b;-webkit-transition:.2s ease all;-moz-transition:.2s ease all;-o-transition:.2s ease all;transition:.2s ease all}.input-number-box input{padding-left:25px}.input-number-prefix{display:none;position:absolute;top:7px;font-weight:700;color:#82888b}.boundary{position:relative;width:100%;display:block}.boundary:after,.boundary:before{content:'';position:absolute;bottom:0;width:0;height:2px;background-color:#41b4c4;-webkit-transition:.2s ease all;-moz-transition:.2s ease all;-o-transition:.2s ease all;transition:.2s ease all}.boundary:before{left:50%}.boundary:after{right:50%}.error-text{display:none;font-size:12px;position:relative;top:4px;left:0;color:#d9534f}.input-box.input-number-box input:focus~.input-number-prefix,.input-box.input-number-box.not-empty .input-number-prefix,.input-box.invalid .error-text{display:inline-block}.input-box input:focus~label,.input-box.not-empty label{top:-14px;font-size:12px}.input-box input:focus~.boundary:after,.input-box input:focus~.boundary:before{width:50%}.input-box.invalid .boundary:after,.input-box.invalid .boundary:before{background-color:#d9534f;width:50%}.size-small .icon-outer-container{width:72px;height:72px}.size-small .icon-inner-container{width:64px;height:64px;margin:3px auto}.padding-15-20{padding:15px 20px}.details-column{width:92%}.bw-horizontal-swiper.card-container .swiper-slide{width:308px}.bw-horizontal-swiper .swiper-card{width:308px;font-size:14px;min-height:140px}.bw-horizontal-swiper .swiper-card a{display:block;padding:15px 10px 10px}@media only screen and (max-width:320px){.bw-horizontal-swiper .swiper-card,.bw-horizontal-swiper.card-container .swiper-slide{width:275px}.location-details{font-size:13px}}#model-gallery-container{display:none;width:100%;height:100%;position:fixed;top:0;left:0;overflow-y:scroll;background:rgba(0,0,0,.9);z-index:8}#model-gallery-container .gallery-header{font-size:16px;color:#fff;padding-right:30px;padding-bottom:5px;position:fixed;top:15px;left:20px;font-weight:700}#bike-gallery-popup{width:100%;max-width:476px;height:auto;margin:0 auto;position:fixed;top:25%;left:0;right:0}.media-title{width:75%;padding-left:10px;padding-right:10px}.gallery-count{width:25%;text-align:right;padding-right:10px}.connected-carousels-photos .stage-photos{width:100%;margin:0 auto 15px;position:relative}.connected-carousels-photos .navigation-photos{width:100%;max-width:476px;position:relative;margin:0 auto}.connected-carousels-photos .carousel-photos{min-height:200px;overflow:hidden;position:relative}.connected-carousels-photos .carousel-photos .swiper-slide{width:100%;margin:0;max-height:268px;font-size:0;position:relative;height:auto;text-align:center}.connected-carousels-photos .carousel-photos .swiper-slide img{height:200px}.connected-carousels-photos .carousel-navigation-photos{height:auto;width:100%;background:0 0;padding-left:0;margin-bottom:20px}.navigation-photos .carousel-navigation-photos .swiper-slide{display:block;position:relative;width:107px;height:52px;overflow:hidden;padding-left:15px}.navigation-photos .carousel-navigation-photos .swiper-slide:after{content:'';position:absolute;top:0;left:0;z-index:2;width:100%;height:50px;display:block;background:#2a2a2a;opacity:.3}.navigation-photos .swiper-slide.swiper-slide-active:after{content:none}.navigation-photos .swiper-slide .navigation-image-wrapper{width:90px;height:50px;border:1px solid #2a2a2a;text-align:center}.navigation-photos .swiper-slide.swiper-slide-active .navigation-image-wrapper{border:1px solid #f5f5f5}.navigation-photos .swiper-slide .navigation-image-wrapper img{height:50px}.connected-carousels-photos .photos-next-stage,.connected-carousels-photos .photos-prev-stage{display:block;position:absolute;top:40%;color:#fff}.connected-carousels-photos .photos-prev-stage{left:0}.connected-carousels-photos .photos-next-stage{right:0}.connected-carousels-photos .next,.connected-carousels-photos .prev{position:absolute;top:40%;z-index:1;width:26px;height:43px;text-indent:-9999px;border:1px solid #e6e6e6;background-color:#fff;padding:5px;-moz-border-radius:0;-webkit-border-radius:0;-o-border-radius:0;-ms-border-radius:0;border-radius:0}.connected-carousels-photos .photos-prev-navigation,.connected-carousels-photos .photos-prev-stage{background-position:-153px -207px}.connected-carousels-photos .photos-next-navigation,.connected-carousels-photos .photos-next-stage{background-position:-177px -238px}.connected-carousels-photos .photos-prev-navigation:hover,.connected-carousels-photos .photos-prev-stage:hover{background-position:-153px -238px}.connected-carousels-photos .photos-next-navigation:hover,.connected-carousels-photos .photos-next-stage:hover{background-position:-177px -238px}.connected-carousels-photos .photos-next-navigation.inactive,.connected-carousels-photos .photos-next-stage.inactive,.connected-carousels-photos .photos-prev-navigation.inactive,.connected-carousels-photos .photos-prev-stage.inactive{display:block}.connected-carousels-photos .photos-prev-navigation.inactive,.connected-carousels-photos .photos-prev-stage.inactive{background-position:-153px -176px;cursor:not-allowed}.connected-carousels-photos .photos-next-navigation.inactive,.connected-carousels-photos .photos-next-stage.inactive{background-position:-177px -176px;cursor:not-allowed}.connected-carousels-photos .photos-prev-navigation{left:0;top:3%}.connected-carousels-photos .photos-next-navigation{right:0;top:3%}.swiper-button-next{right:5px}.swiper-button-prev{left:5px}.swiper-button-next,.swiper-button-prev{border:0}@media only screen and (min-width:361px){.connected-carousels-photos .carousel-photos .swiper-slide img{height:268px}#bike-gallery-popup{top:20%}}@media screen and (max-width:350px){#bike-gallery-popup .stage-photos{margin-bottom:10px}#model-main-image,#model-main-image img{height:170px}}.used-sprite{background:url(http://imgd2.aeplcdn.com/0x0/bw/static/sprites/m/used-sprite.png?v1=14Oct2016) no-repeat;display:inline-block}.sold-icon{width:62px;height:46px;background-position:0 -133px;vertical-align:middle}.bike-sold-msg{display:inline-block;vertical-align:middle;width:75%;padding:10px 20px}
    </style>
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
                        <img src="<%= (firstImage!=null) ? Bikewale.Utility.Image.GetPathToShowImages(firstImage.OriginalImagePath,firstImage.HostUrl,Bikewale.Utility.ImageSize._360x202) : string.Empty %>" alt="Used <%= modelYear %> <%= bikeName %>" title="Used <%= modelYear %> <%= bikeName %>" />
                        <% if (inquiryDetails.PhotosCount > 1)
                           { %>
                        <div class="model-media-details">
                            <div class="model-media-item">
                                <span class="bwmsprite gallery-photo-icon"></span>
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
                        <p class="font12 text-bold text-light-grey margin-top5 margin-bottom15">Seller has not uploaded any photos</p>
                        <% if (!isPhotoRequestDone)
                           { %>
                        <a href="javascript:void(0)" id="request-media-btn" class="btn btn-inv-teal btn-sm font14 text-bold" rel="nofollow">Request photos</a>
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
                            <p class="specs-features-value"><%= Bikewale.Utility.FormatDate.GetFormatDate(inquiryDetails.MinDetails.ModelYear.ToString(),"MMM yyyy") %></p>
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
                        <div class="bike-sold-msg text-grey ">The <%= bikeName %> bike you are looking for has been sold. You might want to consider other used bikes shown below.</div>
                </div> </div>
        </section>
        <% } %>

        <script type="text/javascript" src="<%= staticUrl != "" ? "http://st1.aeplcdn.com" + staticUrl : "" %>/m/src/frameworks.js?<%= staticFileVersion %>"></script>
                <% if ((inquiryDetails.SpecsFeatures != null && !isBikeSold) || ctrlOtherUsedBikes.FetchedRecordsCount > 0 || ctrlSimilarUsedBikes.FetchedRecordsCount > 0)
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
                            <% if (ctrlOtherUsedBikes.FetchedRecordsCount > 0)
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
                <BW:OtherUsedBikes ID="ctrlOtherUsedBikes" runat="server" />
                <!-- Other used bikes ends -->
            </div>
            <div id="modelSpecsFooter"></div>
        </section>
        <% } %>
        <% if (inquiryDetails.PhotosCount > 1)
           { %>
        <!-- gallery start -->
        <div id="model-gallery-container">
            <p class="font16 text-white gallery-header"><%=modelYear %>, <%= bikeName %> Photos</p>
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
                    <BW:ServiceCenterCard runat="server" ID="ctrlServiceCenterCard" />
                <% }  %>

        <!-- #include file="/includes/footerBW_Mobile.aspx" -->
        <!--[if lt IE 9]>
            <script src="<%= staticUrl != "" ? "http://st2.aeplcdn.com" + staticUrl : "" %>/m/src/html5.js"></script>
        <![endif]-->
        <link href="<%= staticUrl != "" ? "http://st2.aeplcdn.com" + staticUrl : "" %>/m/css/bwm-common-btf.css?<%= staticFileVersion %>" rel="stylesheet" type="text/css" />
        <script type="text/javascript" src="<%= staticUrl != "" ? "http://st2.aeplcdn.com" + staticUrl : "" %>/m/src/common.min.js?<%= staticFileVersion %>"></script>
        <script type="text/javascript" src="<%= staticUrl != "" ? "http://st2.aeplcdn.com" + staticUrl : "" %>/m/src/used-details.js?<%= staticFileVersion%>"></script>
        
        <link href='https://fonts.googleapis.com/css?family=Open+Sans:400,700' rel='stylesheet' type='text/css' />
    </form>
</body>
</html>
