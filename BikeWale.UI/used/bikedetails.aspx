<%@ Page Language="C#" AutoEventWireup="false" Inherits="Bikewale.Used.BikeDetails" EnableViewState="false" %>
<%@ Register Src="/controls/UsedBikeLeadCaptureControl.ascx" TagPrefix="BW" TagName="UBLeadCapturePopup" %>
<%@ Register Src="~/controls/SimilarUsedBikes.ascx" TagPrefix="BW" TagName="SimilarUsedBikes" %>
<%@ Register Src="~/m/controls/UploadPhotoRequestPopup.ascx" TagPrefix="BW" TagName="UploadPhotoRequestPopup" %>
<%@ Register Src="~/controls/usedBikeModel.ascx" TagName="usedBikeModel" TagPrefix="BW" %>
<%@ Register Src="~/controls/ServiceCenterCard.ascx" TagName="ServiceCenterCard" TagPrefix="BW" %>

<!DOCTYPE html>
<html>
<head>
    <%  
        isHeaderFix = false;
        title = pgTitle;
        keywords = pgKeywords;
        description = pgDescription;
        canonical = pgCanonicalUrl;
        ogImage = (firstImage != null) ? Bikewale.Utility.Image.GetPathToShowImages(firstImage.OriginalImagePath, firstImage.HostUrl, Bikewale.Utility.ImageSize._360x202) : string.Empty;
        AdId = "1501506048521";
        AdPath = "/1017752/BikeWale_UsedBikes_Details_";
        isAd300x250Shown = false;
        isAd970x90BTFShown = false;
        alternate = pgAlternateUrl;
        TargetedModel = (inquiryDetails!=null && inquiryDetails.Model != null) ? inquiryDetails.Model.ModelName : string.Empty;
        TargetedCity = (inquiryDetails != null &&  inquiryDetails.City != null) ? inquiryDetails.City.CityName : string.Empty;
    %>
    <!-- #include file="/includes/headscript_desktop_min.aspx" -->
    <style type="text/css">
        @charset "utf-8";.btn-inv-teal:focus,.btn-inv-teal:hover,.card .card-target:hover{text-decoration:none}.padding-left30{padding-left:30px}.ad-summary-label{position:relative;top:-5px}#ad-summary .model-details-label{color:#4d5057}.inr-md-lg{width:12px;height:17px;background-position:-64px -515px;position:relative;top:1px}.ad-summary-btn.btn{padding:8px 32px}#bike-main-carousel .jcarousel-wrapper{width:385px}#bike-main-carousel .jcarousel{padding-top:0}#bike-main-carousel .jcarousel li{float:left;width:385px;height:220px;margin-right:0;cursor:pointer}#bike-main-carousel .carousel-img-container{display:table;width:385px;height:220px;text-align:center;line-height:0}#bike-main-carousel .carousel-img-container span{display:table-cell;vertical-align:middle;width:100%;height:220px;background:url(https://imgd.aeplcdn.com/0x0/bw/static/sprites/d/loader.gif) center center no-repeat #f5f5f5}#bike-main-carousel .carousel-img-container img{max-width:100%;height:220px}.model-media-details{position:absolute;right:10px;bottom:30px;font-size:12px;cursor:pointer}.model-media-item{display:inline-block;vertical-align:middle;padding:4px 5px;color:#4d5057;background:rgba(255,255,255,.8);border-radius:2px;line-height:0}.btn-inv-teal.btn-sm,.inv-teal-sm{padding:6px 19px}.gallery-photo-icon{width:16px;height:12px;background-position:-213px -207px}.model-media-count{position:relative;top:-1px}#bike-main-carousel .jcarousel-control-next,#bike-main-carousel .jcarousel-control-prev,.photos-next-navigation,.photos-prev-navigation{width:26px;height:48px}#bike-main-carousel .jcarousel-control-left{left:0}#bike-main-carousel .jcarousel-control-right{right:0}#bike-main-carousel .jcarousel-control-prev,.photos-prev-navigation{background-position:-125px -355px}#bike-main-carousel .jcarousel-control-next,.photos-next-navigation{background-position:-150px -355px}#bike-main-carousel .jcarousel-control-prev:hover,.photos-prev-navigation{background-position:-125px -386px}#bike-main-carousel .jcarousel-control-next:hover,.photos-next-navigation:hover{background-position:-150px -386px}#bike-main-carousel .jcarousel-control-prev.inactive,.photos-prev-navigation.inactive{background-position:-125px -325px;cursor:not-allowed}#bike-main-carousel .jcarousel-control-next.inactive,.photos-next-navigation.inactive{background-position:-150px -325px;cursor:not-allowed}#bike-no-image{width:100%;margin:0 auto 20px;height:185px;display:table;position:relative;text-align:center;background-color:#f5f5f5}.model-details-label,.specs-features-list .grid-6 p{text-align:left;overflow:hidden;text-overflow:ellipsis;white-space:nowrap}.no-image-content{display:table-cell;vertical-align:middle}.no-image-icon{width:40px;height:30px;background-position:-97px -543px}.request-media-icon{width:38px;height:31px;background-position:-142px -543px;margin-top:25px}.btn-inv-teal{background:0 0;color:#3799a7;border:1px solid #3799a7}.btn-inv-teal:hover{color:#fff;background:#41b4c4;border-color:#41b4c4}.btn-inv-teal:focus{color:#fff;background:#3799a7}.inv-teal-sm{font-size:14px}.key-value-list p{width:50%;float:left;margin-top:10px}.key-value-list li:first-child p{margin-top:0}.bike-details-key{color:#82888b}.bike-details-value{font-weight:700}#makeOverallTabsWrapper{width:100%;height:45px}.overall-floating-tabs{width:976px;background:#fff;border-bottom:1px solid #e2e2e2}#makeOverallTabs.fixed-tab{position:fixed;top:0;margin:0 auto;z-index:5;border-bottom:0;-moz-box-shadow:0 2px 2px #e2e2e2,0 1px 1px #f1f1f1;-webkit-box-shadow:0 2px 2px #e2e2e2,0 1px 1px #f1f1f1;-o-box-shadow:0 2px 2px #e2e2e2,0 1px 1px #f1f1f1;-ms-box-shadow:0 2px 2px #e2e2e2,0 1px 1px #f1f1f1;box-shadow:0 2px 2px #e2e2e2,0 1px 1px #f1f1f1}.overall-specs-tabs-wrapper{display:table;background:#fff}.overall-specs-tabs-wrapper a{padding:10px 20px;display:table-cell;font-size:14px;color:#82888b}.overall-specs-tabs-wrapper a:hover{text-decoration:none;color:#4d5057}.overall-specs-tabs-wrapper a.active{border-bottom:3px solid #ef3f30;font-weight:700;color:#4d5057}.specs-features-list .grid-6 p{margin-bottom:25px;width:100%}.blue-right-arrow-icon{width:6px;height:10px;background-position:-74px -469px;position:relative;top:1px;left:7px}.used-bikes-carousel .model-jcarousel-image-preview{height:160px;line-height:0;background:#f5f5f5;margin-bottom:10px}.used-bikes-carousel .card-image-block{background-color:#f5f5f5}.author-grey-sm-icon,.kms-driven-icon,.model-date-icon,.model-loc-icon{width:10px;height:12px;margin-right:5px;vertical-align:middle}.model-date-icon{background-position:-65px -543px}.kms-driven-icon{background-position:-65px -563px}.model-loc-icon{background-position:-82px -543px}.used-bikes-carousel .card-desc-block .grid-6{margin-bottom:6px}#makeUsedBikeContent h2,.card{margin-bottom:20px}.model-details-label{width:82%;display:inline-block;vertical-align:middle;color:#82888b}#similarContent .inner-content-carousel .jcarousel li{min-height:365px}.card{width:292px;min-height:140px;border:1px solid #f6f6f6;-webkit-box-shadow:0 1px 2px 0 rgba(0,0,0,.2);-moz-box-shadow:0 1px 2px 0 rgba(0,0,0,.2);-ms-box-shadow:0 1px 2px 0 rgba(0,0,0,.2);-o-box-shadow:0 1px 2px 0 rgba(0,0,0,.2);box-shadow:0 1px 2px 0 rgba(0,0,0,.2);float:left;margin-left:30px}.card:first-child{margin-left:20px}.card .card-target{min-height:140px;display:block;padding:15px 20px 0}.card .text-truncate{width:100%}.details-column{width:92%}.btn-truncate{max-width:230px;text-align:center;text-overflow:ellipsis;white-space:nowrap;overflow:hidden}.dealership-loc-icon{width:9px;height:12px;background-position:-52px -469px;position:relative;top:4px}.dealership-card-details{width:92%}.vertical-top{display:inline-block;vertical-align:top}.phone-black-icon{width:11px;height:15px;position:relative;top:5px;margin-right:3px;background-position:-73px -444px}.blackOut-window-model{display:none;background:#222;position:fixed;top:0;left:0;right:0;bottom:0;z-index:9;opacity:.95;-ms-filter:"progid:DXImageTransform.Microsoft.Alpha(Opacity=50)";filter:alpha(opacity=95);-moz-opacity:.95;-khtml-opacity:.95}.modelgallery-close-btn{z-index:9}.bike-gallery-popup{display:none;width:730px;height:545px;margin:40px auto 0;overflow-y:auto;position:fixed;top:4%;right:5%;left:5%;z-index:10}.modelgallery-close-btn{position:fixed;top:20px;right:20px}.connected-carousels .navigation,.connected-carousels .stage{width:640px;position:relative;margin:0 auto 20px}.connected-carousels .carousel-stage{height:360px}.connected-carousels .carousel{overflow:hidden;position:relative}.connected-carousels .carousel ul{width:20000em;position:relative;list-style:none;margin:0;padding:0}.connected-carousels .carousel-stage li{float:left;width:640px;height:360px}.connected-carousels .stage-slide{width:100%;height:360px;display:table;text-align:center}.stage-slide .stage-image-placeholder{width:640px;display:table-cell;vertical-align:middle}.stage-image-placeholder img{max-width:100%;max-height:360px;background:#fff}.bike-gallery-details{position:absolute;bottom:0;font-size:14px;width:640px;color:#fff;padding:10px 20px 5px;background:linear-gradient(to bottom,rgba(0,0,0,0),rgba(5,0,0,.7))}.carousel-navigation li{width:145px;height:85px;margin-right:10px;float:left;cursor:pointer}.carousel-navigation .navigation-slide{width:100%;height:85px;display:table;text-align:center;line-height:0}.navigation-slide .navigation-image-placeholder{display:table-cell;vertical-align:middle;border:2px solid rgba(34,34,34,.1)}.carousel-navigation li.active .navigation-image-placeholder{border:2px solid #ccc}.navigation-image-placeholder img{max-height:81px}.connected-carousels .navigation:hover .photos-next-navigation,.connected-carousels .navigation:hover .photos-prev-navigation,.connected-carousels .stage:hover .photos-next-stage,.connected-carousels .stage:hover .photos-prev-stage{display:block}.photos-next-stage,.photos-prev-stage{display:none;width:40px;height:80px;text-indent:-9999px;border:1px solid #e2e2e2;background-color:#fff;padding:10px 5px;-moz-border-radius:2px;-webkit-border-radius:2px;-o-border-radius:2px;-ms-border-radius:2px;border-radius:2px}.connected-carousels .stage .next,.connected-carousels .stage .prev{position:absolute;top:40%;z-index:1}.photos-prev-stage{background-position:-30px -73px;left:-20px}.photos-next-stage{background-position:-61px -73px;right:-20px}.photos-prev-stage:hover{background-position:-30px -128px}.photos-next-stage:hover{background-position:-61px -128px}.photos-prev-stage.inactive{background-position:-30px -18px;cursor:not-allowed}.photos-next-stage.inactive{background-position:-61px -18px;cursor:not-allowed}.photos-next-navigation,.photos-prev-navigation{display:none;text-indent:-9999px;border:1px solid #eee;background-color:#fff;padding:10px 5px;-moz-border-radius:2px;-webkit-border-radius:2px;-o-border-radius:2px;-ms-border-radius:2px;border-radius:2px;position:absolute;top:20%;z-index:2}.photos-prev-navigation{left:-15px}.photos-next-navigation{right:-15px}#get-seller-details-popup.bwm-fullscreen-popup,#request-media-popup.bwm-fullscreen-popup{padding:15px 60px;display:none}#get-seller-details-popup.bw-popup,#request-media-popup.bw-popup{width:480px;top:30%}#request-media-popup .popup-inner-container{padding:0}#mobile-verification-section,#request-sent-section,#seller-details-section,#update-mobile-content{display:none}.btn-fixed-width{padding-right:0;padding-left:0;width:205px}.size-small .icon-outer-container{width:92px;height:92px;margin-bottom:10px}.size-small .icon-inner-container{width:84px;height:84px;margin:3px auto}.otp-icon{width:29px;height:29px;background-position:-109px -177px}.edit-blue-icon{position:relative;top:13px;cursor:pointer}.thankyou-icon{width:52px;height:58px;background-position:-165px -436px;margin-top:10px}.margin-bottom35{margin-bottom:35px}.dealer-details-list li{margin-bottom:25px}.dealer-details-list .data-key{font-size:12px;color:#82888b}.dealer-details-list .data-value,.input-box input{font-size:16px;font-weight:700}.text-truncate{width:100%;text-align:left;text-overflow:ellipsis;white-space:nowrap;overflow:hidden}input[type=text]:focus,input[type=number]:focus{outline:0;box-shadow:none}.input-box{height:60px;text-align:left}.input-box input{width:100%;display:block;padding:7px 0;border-bottom:1px solid #82888b;color:#4d5057}.input-box label,.input-number-prefix{font-size:16px;position:absolute;color:#82888b;top:4px}.input-box label{left:0;-webkit-transition:.2s ease all;-moz-transition:.2s ease all;-o-transition:.2s ease all;transition:.2s ease all}.input-number-box input{padding-left:30px}.input-number-prefix{display:none;font-weight:700}.boundary{position:relative;width:100%;display:block}.boundary:after,.boundary:before{content:'';position:absolute;bottom:0;width:0;height:2px;background-color:#41b4c4;-webkit-transition:.2s ease all;-moz-transition:.2s ease all;-o-transition:.2s ease all;transition:.2s ease all}.boundary:before{left:50%}.boundary:after{right:50%}.error-text{display:none;font-size:12px;position:relative;top:4px;left:0;color:#d9534f}.input-box.input-number-box input:focus~.input-number-prefix,.input-box.input-number-box.not-empty .input-number-prefix,.input-box.invalid .error-text{display:inline-block}.input-box input:focus~label,.input-box.not-empty label{top:-16px;font-size:12px}.input-box input:focus~.boundary:after,.input-box input:focus~.boundary:before{width:50%}.input-box.invalid .boundary:after,.input-box.invalid .boundary:before{background-color:#d9534f;width:50%}#ub-ajax-loader{display:none}#modal-window{background:#000;position:fixed;top:0;left:0;right:0;bottom:0;z-index:9;opacity:.5;display:none;-ms-filter:"progid:DXImageTransform.Microsoft.Alpha(Opacity=50)";filter:alpha(opacity=50);-moz-opacity:.5;-khtml-opacity:.5}#spinner-content{width:50px;height:50px;position:fixed;top:45%;left:10%;right:0;margin:0 auto;z-index:999}.bw-spinner{-webkit-animation:rotator 1.4s linear infinite;animation:rotator 1.4s linear infinite}@-webkit-keyframes rotator{0%{-webkit-transform:rotate(0);transform:rotate(0)}100%{-webkit-transform:rotate(270deg);transform:rotate(270deg)}}@keyframes rotator{0%{-webkit-transform:rotate(0);transform:rotate(0)}100%{-webkit-transform:rotate(270deg);transform:rotate(270deg)}}.circle-path{stroke-dasharray:187;stroke-dashoffset:0;-webkit-transform-origin:center;transform-origin:center;-webkit-animation:dash 1.4s ease-in-out infinite,colors 5.6s ease-in-out infinite;animation:dash 1.4s ease-in-out infinite,colors 5.6s ease-in-out infinite}@-webkit-keyframes colors{0%,100%{stroke:#ef3f30}}@keyframes colors{0%,100%{stroke:#ef3f30}}@-webkit-keyframes dash{0%{stroke-dashoffset:187}50%{stroke-dashoffset:46.75;-webkit-transform:rotate(135deg);transform:rotate(135deg)}100%{stroke-dashoffset:187;-webkit-transform:rotate(450deg);transform:rotate(450deg)}}@keyframes dash{0%{stroke-dashoffset:187}50%{stroke-dashoffset:46.75;-webkit-transform:rotate(135deg);transform:rotate(135deg)}100%{stroke-dashoffset:187;-webkit-transform:rotate(450deg);transform:rotate(450deg)}}@media only screen and (max-width:1024px){#bike-main-carousel .jcarousel li{width:365px}#bike-main-carousel .jcarousel-control-left,#bike-main-carousel .jcarousel-control-right{top:35%}}.bike-sold-msg{width:89%;padding:10px 20px;font-size:14px;display:inline-block;vertical-align:middle}.used-sprite{background:url(https://imgd.aeplcdn.com/0x0/bw/static/sprites/d/used-sprite.png?v1=14Oct2016) no-repeat;display:inline-block}.sold-icon{width:87px;height:64px;background-position:0 -209px;vertical-align:middle}
        .specs-features__content p {font-size: 0; margin-top: 26px; width: 100%;}.specs-features-item__content {font-size: 14px;width: 50%;padding-right: 10px;padding-left: 10px;text-align: left;text-overflow: ellipsis;white-space: nowrap;overflow: hidden;display: inline-block;vertical-align: top;}
    </style>

    <script type="text/javascript">
        <!-- #include file="\includes\gacode_desktop.aspx" -->
        var testimonialSlider = 0;
    </script>
</head>
<body>
    <form id="form1" runat="server">
        <!-- #include file="/includes/headBW.aspx" -->
        <% if(inquiryDetails!=null) { %>
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
                            <% if (inquiryDetails != null && inquiryDetails.City != null)
                               { %>
                            <li itemscope="" itemtype="http://data-vocabulary.org/Breadcrumb">
                                <span class="bwsprite fa-angle-right margin-right10"></span>
                                <a href="/used/bikes-in-<%= inquiryDetails.City.CityMaskingName %>/" itemprop="url">
                                    <span itemprop="title"><%= inquiryDetails.City.CityName %></span>
                                </a>
                            </li>
                            <li itemscope="" itemtype="http://data-vocabulary.org/Breadcrumb">
                                <span class="bwsprite fa-angle-right margin-right10"></span>
                                <a href="<%= string.Format("/used/{0}-bikes-in-{1}/", inquiryDetails.Make.MaskingName, inquiryDetails.City.CityMaskingName) %>" itemprop="url">
                                    <span itemprop="title">Used <%= inquiryDetails.Make.MakeName %> Bikes</span>
                                </a>
                            </li>

                            <li itemscope="" itemtype="http://data-vocabulary.org/Breadcrumb">
                                <span class="bwsprite fa-angle-right margin-right10"></span>
                                <a href="<%= string.Format("/used/{0}-{1}-bikes-in-{2}/", inquiryDetails.Make.MaskingName, inquiryDetails.Model.MaskingName, inquiryDetails.City.CityMaskingName) %>" itemprop="url">
                                    <span itemprop="title">Used <%= inquiryDetails.Model.ModelName %></span>
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

        <% if (!isBikeSold)
           { %>
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
                                        <% for (int i = 0; i < inquiryDetails.PhotosCount; i++)
                                           {
                                               var photo = inquiryDetails.Photo[i];  %>
                                        <li>
                                            <div class="carousel-img-container">
                                                <span>
                                                    <% if (i < 4)
                                                       { %>
                                                    <img src="<%= Bikewale.Utility.Image.GetPathToShowImages(photo.OriginalImagePath,photo.HostUrl,Bikewale.Utility.ImageSize._642x361) %>" title="<%= bikeName %>" alt="<%= bikeName %>" border="0">
                                                    <% }
                                                       else
                                                       { %>
                                                    <img class="lazy" data-original="<%= Bikewale.Utility.Image.GetPathToShowImages(photo.OriginalImagePath,photo.HostUrl,Bikewale.Utility.ImageSize._642x361) %>" border="0" alt="<%= bikeName %>" title="<%= bikeName %>" src="">
                                                    <% } %>
                                                </span>
                                            </div>
                                        </li>
                                        <% } %>
                                    </ul>
                                </div>
                                <% if (inquiryDetails.PhotosCount > 1)
                                   { %>
                                <div class="model-media-details">
                                    <div class="model-media-item">
                                        <span class="bwsprite gallery-photo-icon"></span>
                                        <span class="model-media-count"><%= inquiryDetails.PhotosCount %></span>
                                    </div>
                                </div>
                                <span class="jcarousel-control-left">
                                    <a href="#" class="bwsprite jcarousel-control-prev inactive" rel="nofollow"></a>
                                </span>
                                <span class="jcarousel-control-right">
                                    <a href="#" class="bwsprite jcarousel-control-next" rel="nofollow"></a>
                                </span>
                                <% } %>
                            </div>

                            <% }
                              else
                              { %>
                            <div id="bike-no-image">
                                <div class=" no-image-content ">
                                    <span class="bwsprite no-image-icon"></span>
                                    <p class="font12 text-bold text-light-grey margin-top5 margin-bottom15">Seller has not uploaded any images</p>
                                    <% if (!isPhotoRequestDone)
                                       { %>
                                    <a href="javascript:void(0)" id="request-media-btn" class="btn btn-inv-teal btn-sm font14 text-bold" rel="nofollow">Request images</a>
                                    <% } %>
                                </div>
                            </div>
                            <% } %>
                        </div>
                        <% if (inquiryDetails.MinDetails != null)
                           { %>
                        <div id="ad-summary" class="grid-7 padding-left30 omega font14">
                            <h2 class="text-default ad-summary-label margin-bottom10">Ad summary</h2>
                            <% if (!string.IsNullOrEmpty(modelYear))
                               { %>
                            <div class="grid-4 alpha margin-bottom10">
                                <span class="bwsprite model-date-icon"></span>
                                <span class="model-details-label"><%= modelYear %> model</span>
                            </div>
                            <% } %>
                            <% if (inquiryDetails.MinDetails.KmsDriven > 0)
                               { %>
                            <div class="grid-4 alpha omega margin-bottom10">
                                <span class="bwsprite kms-driven-icon"></span>
                                <span class="model-details-label"><%= Bikewale.Utility.Format.FormatPrice(inquiryDetails.MinDetails.KmsDriven.ToString()) %> kms</span>
                            </div>
                            <div class="clear"></div>
                            <% } %>
                            <% if (!string.IsNullOrEmpty(inquiryDetails.MinDetails.OwnerType))
                               { %>
                            <div class="grid-4 alpha margin-bottom10">
                                <span class="bwsprite author-grey-sm-icon"></span>
                                <span class="model-details-label"><%= Bikewale.Utility.Format.AddNumberOrdinal(Convert.ToUInt16(inquiryDetails.MinDetails.OwnerType),4) %> owner</span>
                            </div>
                            <% } %>
                            <% if (inquiryDetails.City != null && !string.IsNullOrEmpty(inquiryDetails.City.CityName))
                               { %>
                            <div class="grid-4 alpha omega margin-bottom10">
                                <span class="bwsprite model-loc-icon"></span>
                                <span class="model-details-label"><%= inquiryDetails.City.CityName %></span>
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
                                    <p class="bike-details-value"><%= profileId %></p>
                                </li>
                                <li>
                                    <p class="bike-details-key">Date updated</p>
                                    <p class="bike-details-value"><%= Bikewale.Utility.FormatDate.GetDDMMYYYY(inquiryDetails.OtherDetails.LastUpdatedOn.ToString()) %></p>
                                </li>
                                <li>
                                    <p class="bike-details-key">Seller</p>
                                    <p class="bike-details-value"><%= inquiryDetails.OtherDetails.Seller.Equals("s",StringComparison.CurrentCultureIgnoreCase) ? "Individual" : "Dealer" %></p>
                                </li>
                                <li>
                                    <p class="bike-details-key">Registration year</p>
                                    <p class="bike-details-value"><%= Bikewale.Utility.FormatDate.GetFormatDate(inquiryDetails.MinDetails.ModelYear.ToString(),"MMMM yyyy") %></p>
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
                                <% if (!String.IsNullOrEmpty(inquiryDetails.OtherDetails.Insurance)) {%>
                                <li>
                                    <p class="bike-details-key">Insurance</p>
                                    <p class="bike-details-value"><%= inquiryDetails.OtherDetails.Insurance %></p>
                                </li>
                                <%} %>
                                <% if (!String.IsNullOrEmpty(inquiryDetails.OtherDetails.RegistrationNo)){ %>
                                <li>
                                    <p class="bike-details-key">Registration no.</p>
                                    <p class="bike-details-value"><%= inquiryDetails.OtherDetails.RegistrationNo %></p>
                                </li>
                                <%} %>
                            </ul>
                        </div>
                        <div class="clear"></div>

                        <% if (!string.IsNullOrEmpty(inquiryDetails.OtherDetails.Description))
                           { %>

                        <div class="margin-bottom20 border-solid-bottom"></div>

                        <h2 class="text-default margin-bottom15">Ad description</h2>
                        <p class="font14 text-light-grey"><%= inquiryDetails.OtherDetails.Description %></p>
                        <%  }
                   } %>
                    </div>
                </div>
                <div class="clear"></div>
            </div>
        </section>
        <% }
           else
           { %>
        <section>
            <div class="container">
                <div class="grid-12 margin-bottom20">
                    <div class="content-box-shadow content-inner-block-20">
                        <span class="used-sprite sold-icon"></span>
                       
                        <div class="bike-sold-msg text-grey"> <h1><%= bikeName %></h1>
                            The <%= bikeName %> bike you are looking for has been sold. You might want to consider other used bikes shown below.</div>
                    </div>
                </div>
                <div class="clear"></div>
            </div>
        </section>
        <% } %>

        <% if ((inquiryDetails.versionMinSpecs != null && !isBikeSold) || ctrlusedBikeModel.FetchCount>0|| ctrlSimilarUsedBikes.FetchedRecordsCount > 0)
           { %>
        <section>
            <div class="container">
                <div id="makeTabsContentWrapper" class="grid-12 margin-bottom20">
                    <div class="content-box-shadow">
                        <div id="makeOverallTabsWrapper">
                            <div id="makeOverallTabs" class="overall-floating-tabs">
                                <div class="overall-specs-tabs-wrapper">
                                    <% if (inquiryDetails.versionMinSpecs != null && !isBikeSold)
                                       { %>
                                    <a href="#specsContent" rel="nofollow">Specs & Features</a>
                                    <% } %>
                                    <% if (ctrlSimilarUsedBikes.FetchedRecordsCount > 0)
                                       { %>
                                    <a href="#similarContent" rel="nofollow">Similar bikes</a>
                                    <% } %>
                                    <% if (ctrlusedBikeModel.FetchCount > 0)
                                       { %>
                                    <a href="#usedContent" rel="nofollow">Other used bikes</a>
                                    <% } %>
                                </div>
                            </div>
                        </div>
                        <% if (inquiryDetails.versionMinSpecs != null && !isBikeSold)
                           { %>
                        <div id="specsContent" class="bw-model-tabs-data specs-features-list font14">
                            <h2 class="content-inner-block-20">Specifications summary</h2>
                             <% 
                                 var specsList = inquiryDetails.versionMinSpecs;
                                 var index = 0;
                                 var listLength = specsList.Count();
                             %>
                            <%for (int x = 0; x < 3 && index < listLength; x++)
                            {%>
                            <div class="grid-4 omega specs-features__content margin-bottom20">
                                <% for (int i = 0; i < 4 && index < listLength; i++)
                                     {
                                 %>
                                <p>
                                    <span class="specs-features-item__content text-light-grey"><%=specsList[index].Name%> </span>
                                    <span class="specs-features-item__content text-bold"><%=Bikewale.Utility.FormatMinSpecs.ShowAvailable(specsList[index].Value, specsList[index].UnitType, specsList[index].DataType)%> </span>
                                </p>
                                <%index++; %>
                                 <%} %>
                            </div>
                            <%} %>
                            <div class="clear"></div>
                            <div class="padding-left20 margin-bottom10">
                                <a href="<%= moreBikeSpecsUrl %>" title="<%= string.Format("{0} Specifications",bikeName) %>">View full specifications<span class="bwsprite blue-right-arrow-icon"></span></a>
                            </div>

                            <div class="grid-8 alpha margin-bottom25">
                                <h2 class="content-inner-block-20">Features summary</h2>
                                 <%for (int x = 0; x < 2 && index < listLength; x++)
                                     {%>
                                    <div class="grid-6 omega specs-features__content margin-bottom20">
                                    <% for (int i = 0; i < 3 && index < listLength; i++)
                                    {
                                     %>
                                    <p>
                                      <span class="specs-features-item__content text-light-grey"><%=specsList[index].Name%> </span>
                                      <span class="specs-features-item__content text-bold"><%=Bikewale.Utility.FormatMinSpecs.ShowAvailable(specsList[index].Value, specsList[index].UnitType, specsList[index].DataType)%> </span>  
                                    </p>
                                     <%index++; %>
                                     <%} %>
                                    </div>
                                 <%} %>
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
                           <% if (ctrlusedBikeModel.FetchCount>0)
                       { %>
                 <div id="usedContent" class="bw-model-tabs-data font14 active"><BW:usedBikeModel runat="server" ID="ctrlusedBikeModel" /></div>
                     
                        
                    <% } %> 

                        <div id="overallMakeDetailsFooter"></div>
                    </div>
                </div>
                <div class="clear"></div>
            </div>
        </section>
        <% } %>

        <% if (inquiryDetails != null && inquiryDetails.PhotosCount > 1 && !isBikeSold)
           { %>
        <!-- gallery -->
        <section>
            <div class="blackOut-window-model"></div>
            <div class="bike-gallery-popup" id="bike-gallery-popup">
                <div class="modelgallery-close-btn bwsprite cross-lg-white cur-pointer"></div>
                <div class="bike-gallery-heading">
                    <p class="font18 text-bold margin-left30 text-white margin-bottom20"><%=modelYear %>, <%= bikeName %> Images</p>

                    <div class="connected-carousels">
                        <div class="stage">
                            <div class="carousel carousel-stage">
                                <ul>
                                    <% foreach (var photo in inquiryDetails.Photo)
                                       { %>
                                    <li>
                                        <div class="stage-slide">
                                            <div class="stage-image-placeholder">
                                                <img class="lazy" data-original="<%= Bikewale.Utility.Image.GetPathToShowImages(photo.OriginalImagePath,photo.HostUrl,Bikewale.Utility.ImageSize._642x361) %>" alt="<%= bikeName %>" title="<%= bikeName %>" src="">
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
                                                <img class="lazy" data-original="<%= Bikewale.Utility.Image.GetPathToShowImages(photo.OriginalImagePath,photo.HostUrl,Bikewale.Utility.ImageSize._642x361) %>" alt="<%= bikeName %>" src="">
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

        <script type="text/javascript" src="<%= staticUrl  %>/src/frameworks.js?<%=staticFileVersion %>"></script>

        <%if (inquiryDetails != null && inquiryDetails.PhotosCount == 0 && !isBikeSold)
          { %>
        <BW:UploadPhotoRequestPopup runat="server" ID="widgetUploadPhotoRequest"></BW:UploadPhotoRequestPopup>
        <%} %>

        <BW:UBLeadCapturePopup runat="server" ID="ctrlUBLeadCapturePopup"></BW:UBLeadCapturePopup>
        <% } %>
        <% if(ctrlServiceCenterCard.showWidget){ %>
        <section>
            <div class="container margin-bottom20">
                <div class="grid-12">
                    <div class="content-box-shadow">
                        <BW:ServiceCenterCard runat="server" ID="ctrlServiceCenterCard" />
                    </div>
                </div>
                <div class="clear"></div>
            </div>
        </section>
        <% } %>
        <!-- #include file="/includes/footerBW.aspx" -->
        <link href="<%= staticUrl  %>/css/bw-common-btf.css?<%=staticFileVersion %>" rel="stylesheet" type="text/css" />
        <!-- #include file="/includes/footerscript.aspx" -->
        <script type="text/javascript" src="<%= staticUrl%>/src/used-details.js?<%= staticFileVersion%>">
        </script>
        <script type="text/javascript" >
            var gaObj = { 'id': '<%= (int)Bikewale.Entities.Pages.GAPages.Used_Bike_Details%>', 'name': '<%= Bikewale.Entities.Pages.GAPages.Used_Bike_Details%>' };
            </script>

        <!-- #include file="/includes/fontBW.aspx" -->
    </form>
</body>
</html>
