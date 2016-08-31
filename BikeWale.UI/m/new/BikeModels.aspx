<%@ Page Language="C#" AutoEventWireup="false" Inherits="Bikewale.Mobile.New.NewBikeModels" EnableViewState="false" Trace="false" %>
<%@ Register Src="/m/controls/NewNewsWidget.ascx" TagName="News" TagPrefix="BW" %>
<%@ Register Src="/m/controls/NewExpertReviewsWidget.ascx" TagName="ExpertReviews" TagPrefix="BW" %>
<%@ Register Src="/m/controls/NewVideosWidget.ascx" TagName="Videos" TagPrefix="BW" %>
<%@ Register Src="~/m/controls/NewAlternativeBikes.ascx" TagPrefix="BW" TagName="AlternateBikes" %>
<%@ Register Src="/m/controls/NewUserReviewList.ascx" TagPrefix="BW" TagName="UserReviews" %>
<%@ Register Src="~/m/controls/MPriceInTopCities.ascx" TagPrefix="BW" TagName="TopCityPrice" %>
<!DOCTYPE html>
<html>
<head>
    <%
        description = String.Format("{0} Price in India - Rs. {1}. Find {2} Reviews, Specs, Features, Mileage, On Road Price. See {0} Colours, Images at Bikewale.", bikeName, Bikewale.Utility.Format.FormatPriceLong(price.ToString()), bikeModelName);
        title = String.Format("{0} Price, Reviews, Spec, Photos, Mileage | Bikewale", bikeName);
        canonical = String.Format("http://www.bikewale.com/{0}-bikes/{1}/", modelPage.ModelDetails.MakeBase.MaskingName, modelPage.ModelDetails.MaskingName);
        AdPath = "/1017752/Bikewale_Mobile_Model";
        AdId = "1444028976556";
        Ad_320x50 = true;
        Ad_Bot_320x50 = true;
        Ad_300x250 = true;
        TargetedModel = bikeModelName;
        TargetedCity = cityName;
        keywords = string.Format("{0}, {0} Bike, bike, {0} Price, {0} Reviews, {0} Photos, {0} Mileage", bikeName);
        EnableOG = true;
        OGImage = modelImage;
    %>
    <!-- #include file="/includes/headscript_mobile_min.aspx" -->
    <style type="text/css">
        @charset "utf-8";.single-version-label,.text-truncate{text-overflow:ellipsis;overflow:hidden;white-space:nowrap}.btn-grey:hover,.model-media-item:hover,a.btn:hover{text-decoration:none}.reviews-rating{width:45%}.container img{max-width:100%}.rating-wrap img{width:18px}.line-Ht22{line-height:22px}.line-Ht18{line-height:18px}.btn-grey{background:#fff;color:#f04031;border:1px solid #f04031}.btn-grey:focus,.btn-grey:hover{background:#f04031;color:#fff;border:1px solid #f04031}.text-blue{color:#0288d1}.text-darker-black{color:#1a1a1a}.grid-3.version-label-text{width:60px}.grid-9.variantDropDown{width:70%;max-width:320px}.border-light-top{border-top:1px solid #f1f1f1}.tel-grey-icon{position:relative;top:2px}#model-image-wrapper .model-main-image{background-color:#fff;max-width:476px;margin:0 auto;min-height:180px;max-height:268px;position:relative;text-align:center}#model-image-wrapper .model-media-details{position:absolute;right:15px;bottom:10px;font-size:12px}.model-media-item{display:inline-block;vertical-align:middle;padding:5px 5px 3px;color:#82888b;background:rgba(240,240,240,.8);border-radius:2px}.model-media-count{position:relative;top:-1px}.gallery-photo-icon,.gallery-video-icon{height:12px}.gallery-photo-icon{width:16px;background-position:0 -486px}.gallery-video-icon{width:12px;background-position:-26px -486px}.text-xt-light-grey{color:#a8afb3}.block{display:block;width:100%}.border-light-right{border-right:1px solid #e2e2e2}.offers-sprite{background:url(http://imgd3.aeplcdn.com/0x0/bw/static/sprites/m/offers-sprite.png?v19Aug2016) no-repeat;display:inline-block}.loc-change-blue-icon{width:16px;height:16px;background-position:-195px -395px;position:relative;top:2px}.loc-change-blue-icon:hover{background-position:-84px -278px}.upcoming-text-label{background:#4c4c4c;z-index:2;padding:2px 25px 4px 20px;line-height:22px;left:-10px}.upcoming-text-label:after{content:"";background:url(http://imgd3.aeplcdn.com/0x0/bw/static/sprites/m/upcoming-ribbon.png?v=4Mar2016) right top no-repeat;width:12px;position:absolute;height:28px;top:0;padding:0 0 0 35px}.float-button{background-color:#f5f5f5}.float-button.float-fixed{position:fixed;bottom:0;z-index:8;left:0;right:0;background:rgba(245,245,245,.9)}.call-icon,.contact-icon,.disclaimer-icon,.offers-icon{width:14px;height:16px}.disclaimer-icon{background-position:-135px -308px}.offers-icon{background-position:-135px -280px}.contact-icon{background-position:-107px -280px}.call-icon{background-position:-107px -135px}.inr-dark-grey-xsm-icon{width:9px;height:12px;background-position:0 -459px;position:relative;top:1px}.inr-dark-md-icon{width:11px;height:14px;background-position:-19px -459px}.specs-capacity-icon,.specs-maxpower-icon,.specs-mileage-icon,.specs-weight-icon{width:30px}.specs-capacity-icon{height:20px;background-position:-46px 0}.specs-mileage-icon{height:24px;background-position:-46px -30px}.specs-maxpower-icon{height:30px;background-position:-46px -62px}.specs-weight-icon{height:30px;background-position:-46px -102px}.margin-bottom13{margin-bottom:13px}.text-truncate{width:100%;text-align:left}.elevated-shadow{position:relative;top:1px;-moz-box-shadow:0 0 7px #ccc;-webkit-box-shadow:0 0 7px #ccc;-o-box-shadow:0 0 7px #ccc;-ms-box-shadow:0 0 7px #ccc;box-shadow:0 0 7px #ccc}.dropdown-select{display:none}.dropdown-menu{width:100%;height:20px;min-width:125px;font-size:14px;position:relative;display:inline-block;vertical-align:middle;color:#4d5057}.single-version-label{padding:5px 30px 5px 9px;font-weight:700;width:100%}.dropdown-menu .dropdown-label,.dropdown-menu .dropdown-selected-item{width:100%;font-weight:700;background:url(http://imgd1.aeplcdn.com/0x0/bw/static/sprites/m/dropdown-icon.png) 94% no-repeat;white-space:nowrap;overflow:hidden;text-overflow:ellipsis}.dropdown-menu .dropdown-label{padding-right:30px;padding-left:9px;cursor:pointer;display:inline-block;z-index:0}.dropdown-menu .dropdown-selected-item{border-bottom:1px solid #e2e2e2;padding:5px 30px 5px 9px}.dropdown-menu .dropdown-list-wrapper{display:none;min-width:170px;overflow:hidden;position:absolute;top:3px;left:0;background:#fff;z-index:1;-webkit-border-radius:2px;-moz-border-radius:2px;-o-border-radius:2px;-ms-border-radius:2px;border-radius:2px;border:1px solid #e2e2e2\9;-webkit-box-shadow:-3px 3px 15px 1px #ddd;-moz-box-shadow:-3px 3px 15px 1px #ddd;-ms-box-shadow:-3px 3px 15px 1px #ddd;-o-box-shadow:-3px 3px 15px 1px #ddd;box-shadow:-3px 3px 15px 1px #ddd}#dealer-offers-list,.dropdown-menu.dropdown-active .dropdown-list-wrapper,.vertical-top{display:inline-block}.dropdown-menu .dropdown-menu-list{padding-top:10px;padding-bottom:10px}.dropdown-menu .dropdown-menu-list li{padding:5px 10px;cursor:pointer;white-space:nowrap}.dropdown-menu .dropdown-with-select li:hover{background:#eee}.selected-location-label.text-truncate{width:84%}.padding-10-20{padding:10px 20px}#model-dealer-card{padding-top:15px;margin-right:20px;margin-left:20px;border-top:1px solid #eee}#model-dealer-card .offers-content-label{width:64px;font-size:14px;display:inline-block;vertical-align:middle;position:relative;top:-5px}#dealer-offers-list{width:78%;vertical-align:middle}#dealer-offers-list li{width:76px;height:70px;text-align:center;margin-bottom:10px;margin-left:10px;padding-top:7px;padding-bottom:7px;float:left;cursor:pointer;font-size:11px;border:1px solid #f5f5f5;-webkit-border-radius:2px;-moz-border-radius:2px;-o-border-radius:2px;-ms-border-radius:2px;border-radius:2px;border:1px solid #e2e2e2\9;-webkit-box-shadow:1px 1px 1px #ddd;-moz-box-shadow:1px 1px 1px #ddd;-ms-box-shadow:1px 1px 1px #ddd;-o-box-shadow:1px 1px 1px #ddd;box-shadow:1px 1px 1px #ddd}#dealer-offers-list li:first-child{margin-left:0}#dealer-offers-list .more-offers-card{background-color:#f9f9f9}.vertical-top{vertical-align:top}.inr-grey-xxxsm-icon{width:7px;height:10px;background-position:-94px -136px}.btn-sm-0{padding:5px 21px;font-size:14px}.btn-teal{background:#41b4c4;color:#fff;border:1px solid #41b4c4}.float-button .btn{padding:8px 0;font-size:18px}.btn-green{background:#1b9618;color:#fff;border:1px solid #1b9618}.btn-green:focus{background:#008b08;border:1px solid #008b08}.line-height-1-7{line-height:1.7}.booknow-label{width:64%;padding-left:10px;position:relative;top:-3px}.callback-btn.btn-sm-0{padding:5px 28px}.callback-label{width:46%;padding-left:10px;position:relative;top:-3px}.nearby-partner-left-col{width:45%;max-width:260px;margin-right:5px}.nearby-partner-label{width:73%}#dealer-offers-popup.bwm-fullscreen-popup,#more-dealers-popup.bwm-fullscreen-popup{padding:30px 25px;display:none}.margin-top12{margin-top:12px}.offers-sm-box-icon{width:16px;height:16px;background-position:-183px -460px;position:relative;top:1px}.tel-sm-icon{width:15px;height:12px;background-position:-88px -324px;position:relative;top:2px}.offers-box-icon{width:34px;height:37px;background-position:0 0}.emi-calc-icon{width:34px;height:35px;background-position:0 -47px}.dealership-icon-wrapper{height:22px}.dealership-icon{width:29px;height:22px;background-position:0 -356px}.margin-bottom7{margin-bottom:7px}.offerIcon_1_sm,.offerIcon_2_sm,.offerIcon_3_sm,.offerIcon_4_sm,.offerIcon_5_sm,.offerIcon_6_sm,.offerIcon_7_sm{width:30px;height:29px}.offerIcon_1_sm{background-position:0 -93px}.offerIcon_2_sm{background-position:0 -132px}.offerIcon_3_sm{background-position:0 -166px}.offerIcon_4_sm{background-position:0 -207px}.offerIcon_5_sm{background-position:0 -246px}.offerIcon_6_sm{background-position:0 -283px}.offerIcon_7_sm{background-position:0 -317px}#dealer-offers-popup .btn{padding:8px 19px}.dealer-offers-list .dealer-offer-image,.dealer-offers-list .dealer-offer-label{display:inline-block;vertical-align:middle}.dealer-offers-list .dealer-offer-label{width:85%;font-size:14px;text-align:left;padding-left:10px}.dealer-offers-list li{padding-top:20px}.dealer-offers-list li:first-child{padding-top:0}.fullwidth{width:100%;text-align:left;background-color:transparent;cursor:pointer}.bw-horz-tabs{width:372px;border-bottom:1px solid #ccc;border-left:1px solid #ccc;border-top:1px solid #ccc}.bw-horz-tabs li{padding:25px 0 25px 50px;background:#f5f5f5;border-top:1px solid #ccc;font-size:16px;cursor:pointer;border-left:3px solid transparent}.bw-horz-tabs li.active{background:#fff;border-left:3px solid #ef3f30}.bw-horz-tabs li:first-child{border-top:none}.bw-horz-tabs-data{width:100%}.bw-horz-tabs-data .bw-tabs-data li{float:left;margin-bottom:20px;width:100%}.bw-horz-tabs-data .bw-tabs-data li div{float:left;width:60%;padding-right:15px}.bw-horz-tabs-data .bw-tabs-data li div:last-child{width:40%;padding-left:15px;padding-right:0}.overall-specs-tabs-container{width:100%;display:block;background:#fff;overflow-x:auto;z-index:2;-webkit-overflow-scrolling:touch}.overall-specs-tabs-container.fixed-tab-nav{position:fixed;top:0;left:0}.overall-specs-tabs-wrapper{width:100%;display:table;border-bottom:1px solid #e2e2e2}.overall-specs-tabs-wrapper li{display:table-cell;width:1%;padding:10px 15px;text-align:center;white-space:nowrap;font-size:14px;color:#82888b;cursor:pointer}.overall-specs-tabs-container::-webkit-scrollbar{width:0;height:0}.overall-specs-tabs-container::-webkit-scrollbar-thumb,.overall-specs-tabs-container::-webkit-scrollbar-track{display:none}.overall-specs-tabs-wrapper li.active{border-bottom:3px solid #ef3f30;color:#4d5057;font-weight:700}
    </style>
    <script type="text/javascript">
        <!-- #include file="\includes\gacode_mobile.aspx" -->

        var dealerId = '<%= dealerId%>';
        var pqId = '<%= pqId%>';
        var versionId = '<%= versionId%>';
        var cityId = '<%= cityId%>';
        var clientIP = "<%= clientIP%>";
        var pageUrl = "www.bikewale.com/quotation/dealerpricequote.aspx?versionId=" + versionId + "&cityId=" + cityId;
        var areaId = '<%= areaId%>';
        var bikeVersionLocation = '';
        var bikeVersion = '';
        var isBikeWalePq = "<%= isBikeWalePQ%>";
        var isDealerPriceAvailable = "<%= pqOnRoad != null ? pqOnRoad.IsDealerPriceAvailable : false%>";
        var campaignId = "<%= campaignId%>";
        var manufacturerId = "<%= manufacturerId%>";
        var isDealerPQ = "<%= isDealerPQ %>";
        var versionCount = "<%=versionCount  %>";
    </script>
    
</head>
<body itemscope itemtype="http://schema.org/Product">
     <% if (modelPage != null && modelPage.ModelDesc != null)
                                       { %>
    <meta itemprop="description" itemtype="https://schema.org/description" content= "<%= modelPage.ModelDesc.SmallDescription %>" />
    <% } %>
    <meta itemprop="image" content="<%= modelImage %>" />
    <meta itemprop="manufacturer" name="manufacturer" content="<%= modelPage.ModelDetails.MakeBase.MakeName %>">  
    <meta itemprop="model" content="<%= modelPage.ModelDetails.ModelName %>"/>
    <form id="form1" runat="server">
        <%--<div id="top-progress-bar">
            <div class="progress-bar"></div>
        </div>--%>
        <!-- #include file="/includes/headBW_Mobile.aspx" -->
        <section>
            <div class="container bg-white clearfix">
                <span itemprop="name" class="hide"><%= bikeName %></span>
                <div class="<%= !modelPage.ModelDetails.New ? "padding-top20 position-rel" : ""%>">
                    <% if (modelPage.ModelDetails.New)
                       { %><h1 class="padding-top10 padding-left20 padding-right20"><%= bikeName %></h1>
                    <% } %>
                    <% if (modelPage.ModelDetails.Futuristic)
                       { %>
                    <div class="upcoming-text-label font16 position-abt pos-top10 text-white text-center">Upcoming</div>
                    <div class="bikeTitle">
                        <h1 class="padding-top30 padding-left20 padding-right20"><%= bikeName %></h1>
                    </div>
                    <% } %>
                    <% if(!modelPage.ModelDetails.New && !modelPage.ModelDetails.Futuristic)
                       { %>
                    <div class="upcoming-text-label font16 position-abt pos-top10 text-white text-center">Discontinued</div>
                    <div class="bikeTitle">
                        <h1 class="padding-top30 padding-left20 padding-right20"><%= bikeName %></h1>
                    </div>
                    <% } %>

                    <% if (modelPage.ModelDetails.New || !modelPage.ModelDetails.New)
                       { %>
                    <div class="padding-left20 padding-right10 margin-top5 margin-bottom10">
                        <p class=" <%= modelPage.ModelDetails.ReviewCount > 0 ? "" : "hide"  %> leftfloat margin-right10 rating-wrap">
                            <%= Bikewale.Utility.ReviewsRating.GetRateImage(Convert.ToDouble((modelPage.ModelDetails == null || modelPage.ModelDetails.ReviewRate == null) ? 0 : modelPage.ModelDetails.ReviewRate )) %>
                        </p>
                        <p class="<%= modelPage.ModelDetails.ReviewCount > 0 ? "hide" : ""  %> leftfloat margin-right10 rating-wrap">
                            Not rated yet
                        </p>
                        <% if (modelPage.ModelDetails.ReviewCount > 0)

                                       { %>
                        <span itemprop="aggregateRating" itemscope="" itemtype="http://schema.org/AggregateRating">
                        <meta itemprop="ratingValue" content="<%=modelPage.ModelDetails.ReviewRate %>">
                        <meta itemprop="worstRating" content="1">
                        <meta itemprop="bestRating" content="5">
                            <a href="/m/<%=modelPage.ModelDetails.MakeBase.MaskingName %>-bikes/<%= modelPage.ModelDetails.MaskingName %>/user-reviews/" class="<%= modelPage.ModelDetails.ReviewCount > 0 ? "" : "hide"  %> border-solid-left leftfloat margin-right10 padding-left10 line-Ht22">
                                <span itemprop="reviewCount"><%= modelPage.ModelDetails.ReviewCount %>
                                </span>Reviews
                                  <% } %>
                            </a>
                        </span>
                        <div class="clear"></div>
                    </div>
                    <% } %>
                    <div id="model-image-wrapper">
                        <div class="model-main-image">
                           
                            <a href="/m/<%=modelPage.ModelDetails.MakeBase.MaskingName %>-bikes/<%= modelPage.ModelDetails.MaskingName %>/photos/" ><img src="<%=modelImage %>" alt="<%= bikeName %> images" title="<%= bikeName %> model image" /></a>
                            <div class="model-media-details">
                                <% if (modelPage.ModelDetails.PhotosCount>0)
                                { %>
                                <a href="/m/<%=modelPage.ModelDetails.MakeBase.MaskingName %>-bikes/<%= modelPage.ModelDetails.MaskingName %>/photos" class="model-media-item">
                                    <span class="bwmsprite gallery-photo-icon"></span>
                                    <span class="model-media-count"><%= modelPage.ModelDetails.PhotosCount %></span>
                                </a>
                                <% } %>
                               <% if (modelPage.ModelDetails.VideosCount>0)
                                { %>
                                <a href="/m/<%=modelPage.ModelDetails.MakeBase.MaskingName %>-bikes/<%= modelPage.ModelDetails.MaskingName %>/photos/#videos" class="model-media-item">
                                     <span class="bwmsprite gallery-video-icon"></span>
                                    <span class="model-media-count"><%=modelPage.ModelDetails.VideosCount%></span>
                                </a>
                               <% } %>
                            </div>
                           
                        </div>
                    <% if (modelPage.ModelDetails.Futuristic)
                       { %>
                             <div class="bikeDescWrapper">
                          <% if (modelPage.UpcomingBike != null) {%>
                         <div class="margin-bottom10 font14 text-light-grey">Expected price</div>
                         <div>
                             <span class="font24 text-bold"><span class="bwmsprite inr-lg-icon"></span>
                                 <%= Bikewale.Utility.Format.FormatNumeric(Convert.ToString(modelPage.UpcomingBike.EstimatedPriceMin)) %> - <%= Bikewale.Utility.Format.FormatNumeric(Convert.ToString(modelPage.UpcomingBike.EstimatedPriceMax)) %></span>
                         </div>
                         <div class="border-solid-bottom margin-top15 margin-bottom15"></div>
                         <div class="margin-bottom10 font14 text-light-grey">Expected launch date</div>
                         <div class="font18 text-grey margin-bottom5">
                             <span class="text-bold"><%= modelPage.UpcomingBike.ExpectedLaunchDate %></span>
                         </div>
                        <% } %>
                        <p class="font14 text-grey"><%= bikeName %> is not launched in India yet. Information on this page is tentative.</p>
                    </div>
                    <% } %>
                </div>
                
                <!-- previous top model card -->
            <% if (!isDiscontinued)
               {
                   if (toShowOnRoadPriceButton)
                   {   %>
            <div class="grid-12 float-button float-fixed clearfix padding-bottom10">

                <a id="btnGetOnRoadPrice" href="javascript:void(0)" data-reload="true"  data-persistent="true" data-modelid="<%=modelId %>" style="width: 100%" class="btn btn-orange margin-top10 getquotation">Check on-road price</a>
                <% }
                   else
                   {   %>
                <div class="grid-12 float-button float-fixed clearfix">
                    
                    <% if (modelPage.ModelDetails.New && viewModel != null && !isBikeWalePQ )
                        {   
                        %>
                        <% if ( viewModel.IsPremiumDealer)
                            { 
                        %>  <div class="grid-<%= String.IsNullOrEmpty(viewModel.MaskingNumber)? "12":"6" %> alpha omega padding-right5">
                                <a id="getAssistance" leadSourceId="19" class="btn btn-orange btn-full-width rightfloat" href="javascript:void(0);">Get offers</a>
                            </div>
                            <%  if(!string.IsNullOrEmpty(viewModel.MaskingNumber))
                                { %>
                                <div class="grid-6 alpha omega padding-left5">
                                    <a id="calldealer" class="btn btn-green btn-full-width rightfloat" href="tel:+91<%= String.IsNullOrEmpty(viewModel.MaskingNumber)? viewModel.MobileNo: viewModel.MaskingNumber %>"><span class="bwmsprite tel-white-icon margin-right5"></span>Call dealer</a>
                                </div>
                            <% } %>
                        <% }                             
                        }  
                    %>
                </div>
                <%
                }
               }
               %>
            </div>
        </section>      

        <section>
            <asp:HiddenField ID="hdnVariant" Value="0" runat="server" />
            <div class="container bg-white clearfix elevated-shadow">
                <!-- new bikes section -->
                <% if (!isDiscontinued)
                   { %>
                <div class="grid-12 padding-top5 padding-bottom5 border-solid-bottom">
                    <div class="grid-6 alpha border-solid-right">
                        <p class="font12 text-light-grey padding-left10">Version:</p>
                        <p id="defversion" class="single-version-label font14 margin-left5"><%=variantText %></p>
                        <% if (versionCount > 1)
                           { %>
                            <div class="dropdown-select-wrapper">
                                <asp:DropDownList CssClass="dropdown-select" ID="ddlNewVersionList" runat="server" />
                            </div>
                        <% } %>
                    </div>
                    <div class="grid-6 padding-left20">
                        <p class="font12 text-light-grey">Location:</p>
                        <div class="font14 text-bold">
                            <a href="javascript:void(0)" data-reload="true" data-persistent="true" data-modelid='<%= modelId %>' class="getquotation changeCity" rel="nofollow">
                                <span class="selected-location-label inline-block text-default text-truncate"><%= location %></span>                            
                                <span class="bwmsprite loc-change-blue-icon"></span>
                            </a>
                        </div>
                    </div>
                    <div class="clear"></div>
                </div>
                <div class="clear"></div>
                <% }
                   else if(isDiscontinued && !modelPage.ModelDetails.Futuristic) { %>
                       <div class="bike-price-container padding-10-20">
                            <div class="bike-price-container ">
                                <span class="font14 text-grey"><%= bikeName %> is now discontinued in India.</span>
                            </div>
                        </div>
                 <% } 
                   if (!modelPage.ModelDetails.Futuristic)
                   { %>
                        <div class="padding-10-20">
                            <p class="font12 text-light-grey"><%=priceText %> price in <%= location %></p>
                            <p>
                                <span class="bwmsprite inr-md-icon"></span>
                                <span class="font22 text-bold"><%= Bikewale.Utility.Format.FormatPrice(price.ToString()) %>&nbsp;</span>
                                <%if (isOnRoadPrice && price > 0)
                                  {%>
                                <a href="/m/pricequote/dealerpricequote.aspx?MPQ=<%= detailedPriceLink %>" class="font16 text-bold viewBreakupText" rel="nofollow" >View detailed price</a>
                                <% } %>
                            </p>
                        </div>
                <% } %>
                <%
                    if (viewModel != null && !isBikeWalePQ)
                    { 
                %>
                         <div id="model-dealer-card">
                    <% if(viewModel.IsPremiumDealer){ %>
                    <div class="dealer-details margin-bottom10">
                        <div class="dealership-icon-wrapper inline-block margin-right5">
                            <span class="offers-sprite dealership-icon"></span>
                        </div>
                        <div class="inline-block">
                            <h2 id="dealername" class="text-default"><%=viewModel.Organization %></h2>
                            <p class="font14 text-light-grey"><%=viewModel.AreaName %></p>
                        </div>
                    </div>
                    <% } %>
                    <% if (viewModel.Offers != null && viewModel.OfferCount > 0)
                       { %>
                    <div class="dealer-offers-content margin-bottom10">
                        <p class="offers-content-label">Offers available:</p>
                        <ul id="dealer-offers-list">
                            <asp:Repeater ID="rptNewOffers" runat="server">
                                <ItemTemplate>
                                    <li>
                                        <p class="margin-bottom5">
                                            <span class="offers-sprite offerIcon_<%# Convert.ToString(DataBinder.Eval(Container.DataItem, "OfferCategoryId"))%>_sm"></span>
                                        </p>
                                        <p>
                                            <%# Convert.ToString(DataBinder.Eval(Container.DataItem, "offerType")) %>
                                        </p>
                                    </li>
                                </ItemTemplate>
                                <FooterTemplate>
                                    <% if (moreOffersCount > 3)
                                       {%>
                                    <li class="more-offers-card">
                                        <p class="font18 text-bold text-light-grey margin-top5 margin-bottom7">+<%= moreOffersCount %> </p>
                                        <p>more offers</p>
                                    </li>
                                    <% } %>
                                </FooterTemplate>
                            </asp:Repeater>
                        </ul>
                    </div>
                    <%} %>
                    <% if (viewModel.IsPremiumDealer)
                       {
                           if (isBookingAvailable)
                           { %>
                    <div class="margin-bottom20">
                        <div class="vertical-top">
                            <a rel="nofollow" href="/m/pricequote/bookingsummary_new.aspx?MPQ=<%= mpqQueryString %>" class="btn btn-teal btn-sm-0">Book now</a>
                        </div>
                        <p class="booknow-label font11 line-height-1-7 text-xx-light vertical-top">
                            Pay <span class="bwmsprite inr-grey-xxxsm-icon"></span><%= Bikewale.Utility.Format.FormatPrice(bookingAmt.ToString()) %> to book online and <br />balance of <span class="bwmsprite inr-grey-xxxsm-icon"></span><%= Bikewale.Utility.Format.FormatPrice((price - bookingAmt).ToString()) %> at dealership
                        </p>
                    </div>
                    <% }
                                     else
                        { %>
                    <div class="margin-bottom20">
                        <div class="vertical-top">
                            <a id="requestcallback" c="Model_Page" a="Request_Callback_Details_Clicked" v="bikeVersionLocation" leadSourceId="30" href="javascript:void(0)" class="btn btn-white callback-btn btn-sm-0 bw-ga">Request callback</a>
                        </div>
                        <p class="callback-label font11 line-height-1-7 text-xx-light vertical-top">Get EMI options, test rides other services from dealer</p>
                    </div>
                    <% 
                    }
                       }
                       else
                       { %>
                    <div class="margin-bottom20">
                        <div class="inline-block nearby-partner-left-col">
                            <div class="dealership-icon-wrapper inline-block margin-right5">
                                <span class="offers-sprite dealership-icon"></span>
                            </div>
                            <div class="inline-block nearby-partner-label">
                                <p class="font14">One partner dealer near you</p>
                            </div>
                        </div>
                        <a id="viewprimarydealer" c="Model_Page" a="View_Dealer_Details_Clicked" v="bikeVersionLocation"  href="javascript:void(0)" class="btn btn-orange btn-sm-0 bw-ga">View dealer details</a>
                    </div>
                    <% } %>
                </div>
                 <% } %>
                <% if (viewModel!= null && viewModel.SecondaryDealerCount > 0)
                   { %>
                    <div class="content-inner-block-20 border-solid-top font16">
                        <a href="javascript:void(0)" rel="nofollow" id="more-dealers-target">Prices from <%=viewModel.SecondaryDealerCount %> more partner dealers<span class="bwmsprite blue-right-arrow-icon"></span></a>
                    </div>
                <% } %>
            </div>
        </section>

        <script type="text/javascript" src="<%= staticUrl != "" ? "http://st1.aeplcdn.com" + staticUrl : "" %>/m/src/frameworks.js?<%= staticFileVersion %>"></script>

        <section>
            <div id="modelSpecsTabsContentWrapper" class="container bg-white clearfix box-shadow margin-top30 margin-bottom30">
                <div id="modelOverallSpecsTopContent">
                    <div id="overallSpecsTab" class="overall-specs-tabs-container">
                        <ul class="overall-specs-tabs-wrapper">
                            <% if ((modelPage.ModelDesc != null && !string.IsNullOrEmpty(modelPage.ModelDesc.SmallDescription)) || modelPage.ModelVersionSpecs != null)
                           { %>
                            <li data-tabs="#modelSummaryContent">About</li>
                            <% } %>
                            <% if (modelPage.ModelVersions != null && modelPage.ModelVersions.Count > 0)
                            { %>
                            <li data-tabs="#modelPricesContent">Prices</li>
                            <%} %>
                            <% if(modelPage.ModelVersionSpecs!= null){ %>
                            <li data-tabs="#modelSpecsFeaturesContent">Specs & Features</li>
                            <% } %>
                            <%if (modelPage.ModelColors != null && modelPage.ModelColors.Count() > 0)
                            { %>
                            <li data-tabs="#modelColoursContent">Colours</li>
                            <%} %>
                            <% if (ctrlExpertReviews.FetchedRecordsCount > 0 || ctrlUserReviews.FetchedRecordsCount > 0)
                             { %>
                            <li data-tabs="#modelReviewsContent">Reviews</li>
                              <%} %>
                            <% if (ctrlVideos.FetchedRecordsCount > 0)
                                { %>
                                <li data-tabs="#modelVideosContent">Videos</li>
                            <%} %>
                             <% if (ctrlNews.FetchedRecordsCount > 0)
                             { %>
                                <li data-tabs="#makeNewsContent">News</li>
                            <%} %>
                             <% if (ctrlAlternativeBikes.FetchedRecordsCount > 0)
                              { %>
                                 <li data-tabs="#modelAlternateBikeContent">Alternatives</li>
                            <%} %>
                        </ul>
                    </div>
                </div>

                <%if (modelPage.ModelDesc != null && !string.IsNullOrEmpty(modelPage.ModelDesc.SmallDescription) || (modelPage.ModelVersionSpecs != null))
                  { %>
                <div id="modelSummaryContent" class="bw-model-tabs-data margin-right20 margin-left20 padding-top15 padding-bottom15 border-solid-bottom">
                    <%if (modelPage.ModelDesc != null && !string.IsNullOrEmpty(modelPage.ModelDesc.SmallDescription))
                      { %>
                    <h2>About <%=bikeName %></h2>
                    <h3>Preview</h3>
                    <p class="font14 text-light-grey line-height17">
                        <span class="model-preview-main-content">
                            <%= modelPage.ModelDesc.SmallDescription %>   
                        </span>
                        <span class="model-preview-more-content">
                            <%= modelPage.ModelDesc.FullDescription %>
                        </span>

                        <%if (!string.IsNullOrEmpty(modelPage.ModelDesc.SmallDescription))
                          { %>
                        <a href="javascript:void(0)" class="read-more-model-preview font14" rel="nofollow">Read more</a>
                        <% } %>
                    </p>
                    <% } %>
                    <% if (modelPage.ModelVersionSpecs != null)
                       { %>
                    <h3 class="margin-top15">Specification summary</h3>
                    <div class="text-center">
                        <div class="summary-overview-box">
                            <div class="odd btmAftBorder">
                                <span class="inline-block offers-sprite specs-capacity-icon margin-right10" title="<%=bikeName %> Engine Capacity"></span>
                                <div class="inline-block">
                                    <p class="font18 text-bold margin-bottom5">
                                        <%= Bikewale.Utility.FormatMinSpecs.ShowAvailable(modelPage.ModelVersionSpecs.Displacement) %>
                                        <span class='<%= Bikewale.Utility.FormatMinSpecs.ShowAvailable(modelPage.ModelVersionSpecs.Displacement).Equals("--") ? "hide":"" %>'>cc</span>
                                    </p>
                                    <p class="font16 text-light-grey">Capacity</p>
                                </div>
                            </div>
                            <div class="even btmAftBorder">
                                <span class="inline-block offers-sprite specs-mileage-icon margin-right10" title="<%=bikeName %> Mileage"></span>
                                <div class="inline-block">
                                    <p class="font18 text-bold margin-bottom5">
                                        <%= Bikewale.Utility.FormatMinSpecs.ShowAvailable(modelPage.ModelVersionSpecs.FuelEfficiencyOverall) %>
                                        <span class='<%= Bikewale.Utility.FormatMinSpecs.ShowAvailable(modelPage.ModelVersionSpecs.FuelEfficiencyOverall).Equals("--") ? "hide":"" %>'>kmpl</span>
                                    </p>
                                    <p class="font16 text-light-grey">Mileage</p>
                                </div>
                            </div>
                            <div class="odd">
                                <span class="inline-block offers-sprite specs-maxpower-icon margin-right10" title="<%=bikeName %> Max Power"></span>
                                <div class="inline-block">
                                    <p class="font18 text-bold margin-bottom5">
                                        <%= Bikewale.Utility.FormatMinSpecs.ShowAvailable(modelPage.ModelVersionSpecs.MaxPower) %>
                                        <span class='<%= Bikewale.Utility.FormatMinSpecs.ShowAvailable(modelPage.ModelVersionSpecs.MaxPower).Equals("--") ? "hide":"" %>'>bhp</span>
                                    </p>
                                    <p class="font16 text-light-grey">Max power</p>
                                </div>
                            </div>
                            <div class="even">
                                <span class="inline-block offers-sprite specs-weight-icon margin-right10" title="<%=bikeName %> Kerb Weight"></span>
                                <div class="inline-block">
                                    <p class="font18 text-bold margin-bottom5">
                                        <%= Bikewale.Utility.FormatMinSpecs.ShowAvailable(modelPage.ModelVersionSpecs.KerbWeight) %>
                                        <span class='<%= Bikewale.Utility.FormatMinSpecs.ShowAvailable(modelPage.ModelVersionSpecs.KerbWeight).Equals("--") ? "hide":"" %>'>kg</span>
                                    </p>
                                    <p class="font16 text-light-grey">Weight</p>
                                </div>
                            </div>
                        </div>
                    </div>
                    <% } %>
                </div>
                <% } %>

                <div id="modelPricesContent" class="bw-model-tabs-data">
                    <% if (modelPage.ModelVersions != null && modelPage.ModelVersions.Count > 0)
                       { %>
                    <h2 class="padding-top15 padding-right20 padding-left20"><%= bikeName %> Prices</h2>
                    <!-- varient code starts here -->
                    <h3 class="padding-right20 padding-left20">Prices by versions</h3>

                    <table width="100%" border="0" cellspacing="0" cellpadding="0" class="padding-right20 padding-left20 margin-bottom5">
                        <thead>
                            <tr>
                                <th align="left" width="65%" class="font12 text-unbold text-x-light padding-bottom5 border-solid-bottom">Version</th>
                                <th align="left" width="35%" class="font12 text-unbold text-x-light padding-bottom5 border-solid-bottom">Price</th>
                            </tr>
                        </thead>
                        <tbody>
                            <asp:Repeater ID="rptVarients" runat="server" OnItemDataBound="rptVarients_ItemDataBound2">
                                <ItemTemplate>
                                    <tr>
							            <td width="65%" class="padding-bottom10 padding-top10 padding-right10 font14 divider-bottom" valign="top"><%# DataBinder.Eval(Container.DataItem, "VersionName") %></td>
							            <td width="35%" class="padding-bottom10 padding-top10 divider-bottom" valign="top">
                                            <span class="bwmsprite inr-dark-md-icon"></span>
                                            <span id="<%# "priced_" + Convert.ToString(DataBinder.Eval(Container.DataItem, "VersionId")) %>" class="font16 text-bold">
                                                <asp:Label Text='<%# Bikewale.Utility.Format.FormatPrice(Convert.ToString(DataBinder.Eval(Container.DataItem, "Price"))) %>' ID="txtComment" runat="server"></asp:Label>
                                            </span>
							            </td>
						            </tr>
                                    <asp:HiddenField ID="hdnVariant" runat="server" Value='<%#Eval("VersionId") %>' />                           
                                </ItemTemplate>
                            </asp:Repeater>
                            <tr>
						        <td colspan="2" class="padding-top5 font12 text-x-light">Above mentioned prices are <%=priceText %>, <%=location %></td>
                            
					        </tr>
                        </tbody>
                    </table>
                    <!-- varient code ends here -->
                    <% } %>
                   <BW:TopCityPrice ID="ctrlTopCityPrices" runat="server" />
                   <div class="margin-right20 margin-left20 border-solid-bottom"></div>
                </div>

                <% if(modelPage.ModelVersionSpecs != null){ %>
                <div id="modelSpecsFeaturesContent" class="bw-model-tabs-data font14">
                    <h2 class="padding-top15 padding-right20 padding-left20"><%=modelPage.ModelDetails.ModelName%> Specifications & Features</h2>
                    <h3 class="padding-right20 padding-left20 model-specs-header">Specifications</h3>

                    <ul id="model-specs-list">
                        <li>
                            <div class="model-accordion-tab active">
                                <span class="offers-sprite engine-sm-icon margin-right10"></span>
                                <span>Engine & transmission</span>
                                <span class="bwmsprite fa-angle-down"></span>
                            </div>
                            <ul class="specs-features-list">
                                <li>
                                    <div class="specs-features-label">Displacement</div>
                                    <div class="text-bold"><%= Bikewale.Utility.FormatMinSpecs.ShowAvailable(modelPage.ModelVersionSpecs.Displacement,"cc") %></div>
                                </li>
                                <li>
                                    <div class="specs-features-label">Cylinders</div>
                                    <div class="text-bold"><%= Bikewale.Utility.FormatMinSpecs.ShowAvailable(modelPage.ModelVersionSpecs.Cylinders) %></div>
                                </li>
                                <li>
                                    <div class="specs-features-label">Max Power</div>
                                    <div class="text-bold"><%= Bikewale.Utility.FormatMinSpecs.ShowAvailable(modelPage.ModelVersionSpecs.MaxPower, "bhp", modelPage.ModelVersionSpecs.MaxPowerRPM, "rpm") %></div>
                                </li>
                                <li>
                                    <div class="specs-features-label">Maximum Torque</div>
                                    <div class="text-bold"><%= Bikewale.Utility.FormatMinSpecs.ShowAvailable(modelPage.ModelVersionSpecs.MaximumTorque, "Nm", modelPage.ModelVersionSpecs.MaximumTorqueRPM, "rpm") %></div>
                                </li>
                                <li>
                                    <div class="specs-features-label">Bore</div>
                                    <div class="text-bold"><%= Bikewale.Utility.FormatMinSpecs.ShowAvailable(modelPage.ModelVersionSpecs.Bore, "mm") %></div>
                                </li>
                                <li>
                                    <div class="specs-features-label">Stroke</div>
                                    <div class="text-bold"><%= Bikewale.Utility.FormatMinSpecs.ShowAvailable(modelPage.ModelVersionSpecs.Stroke, "mm") %></div>
                                </li>
                                <li>
                                    <div class="specs-features-label">Valves Per Cylinder</div>
                                    <div class="text-bold"><%= Bikewale.Utility.FormatMinSpecs.ShowAvailable(modelPage.ModelVersionSpecs.ValvesPerCylinder) %></div>
                                </li>
                                <li>
                                    <div class="specs-features-label">Fuel Delivery System</div>
                                    <div class="text-bold"><%= Bikewale.Utility.FormatMinSpecs.ShowAvailable(modelPage.ModelVersionSpecs.FuelDeliverySystem) %></div>
                                </li>
                            </ul>
                        </li>
                        <li>
                            <div class="model-accordion-tab brakes-accordion-tab">
                                <span class="offers-sprite brakes-sm-icon margin-right10"></span>
                                <span>Brakes, wheels & suspension</span>
                                <span class="bwmsprite fa-angle-down"></span>
                            </div>
                            <ul class="specs-features-list">
                                <li>
                                    <div class="specs-features-label">Brake Type</div>
                                    <div class="text-bold"><%= Bikewale.Utility.FormatMinSpecs.ShowAvailable(modelPage.ModelVersionSpecs.BrakeType) %></div>
                                </li>
                                <li>
                                    <div class="specs-features-label">Front Disc</div>
                                    <div class="text-bold"><%= Bikewale.Utility.FormatMinSpecs.ShowAvailable(modelPage.ModelVersionSpecs.FrontDisc) %></div>
                                </li>
                                <li>
                                    <div class="specs-features-label">Front Disc/Drum Size</div>
                                    <div class="text-bold"><%= Bikewale.Utility.FormatMinSpecs.ShowAvailable(modelPage.ModelVersionSpecs.FrontDisc_DrumSize, "mm") %></div>
                                </li>
                                <li>
                                    <div class="specs-features-label">Rear Disc</div>
                                    <div class="text-bold"><%= Bikewale.Utility.FormatMinSpecs.ShowAvailable(modelPage.ModelVersionSpecs.RearDisc) %></div>
                                </li>
                                <li>
                                    <div class="specs-features-label">Rear Disc/Drum Size</div>
                                    <div class="text-bold"><%= Bikewale.Utility.FormatMinSpecs.ShowAvailable(modelPage.ModelVersionSpecs.RearDisc_DrumSize, "mm") %></div>
                                </li>
                                <li>
                                    <div class="specs-features-label">Calliper Type</div>
                                    <div class="text-bold"><%= Bikewale.Utility.FormatMinSpecs.ShowAvailable(modelPage.ModelVersionSpecs.CalliperType) %></div>
                                </li>
                                <li>
                                    <div class="specs-features-label">Wheel Size</div>
                                    <div class="text-bold"><%= Bikewale.Utility.FormatMinSpecs.ShowAvailable(modelPage.ModelVersionSpecs.WheelSize, "inches") %></div>
                                </li>
                            </ul>
                        </li>
                        <li>
                            <div class="model-accordion-tab">
                                <span class="offers-sprite dimension-sm-icon margin-right10"></span>
                                <span>Dimensions & chasis</span>
                                <span class="bwmsprite fa-angle-down"></span>
                            </div>
                            <ul class="specs-features-list">
                                <li>
                                    <div class="specs-features-label">Kerb Weight</div>
                                    <div class="text-bold"><%= Bikewale.Utility.FormatMinSpecs.ShowAvailable(modelPage.ModelVersionSpecs.KerbWeight, "kg") %></div>
                                </li>
                                <li>
                                    <div class="specs-features-label">Overall Length</div>
                                    <div class="text-bold"><%= Bikewale.Utility.FormatMinSpecs.ShowAvailable(modelPage.ModelVersionSpecs.OverallLength, "mm") %></div>
                                </li>
                                <li>
                                    <div class="specs-features-label">Overall Width</div>
                                    <div class="text-bold"><%= Bikewale.Utility.FormatMinSpecs.ShowAvailable(modelPage.ModelVersionSpecs.OverallWidth, "mm") %></div>
                                </li>
                                <li>
                                    <div class="specs-features-label">Overall Height</div>
                                    <div class="text-bold"><%= Bikewale.Utility.FormatMinSpecs.ShowAvailable(modelPage.ModelVersionSpecs.OverallHeight, "mm") %></div>
                                </li>
                            </ul>
                        </li>
                        <li>
                            <div class="model-accordion-tab">
                                <span class="offers-sprite fuel-sm-icon margin-right10"></span>
                                <span>Fuel effeciency & performance</span>
                                <span class="bwmsprite fa-angle-down"></span>
                            </div>
                            <ul class="specs-features-list">
                                <li>
                                    <div class="specs-features-label">Fuel Tank Capacity</div>
                                    <div class="text-bold"><%= Bikewale.Utility.FormatMinSpecs.ShowAvailable(modelPage.ModelVersionSpecs.FuelTankCapacity, "litres") %></div>
                                </li>
                                <li>
                                    <div class="specs-features-label">Reserve Fuel Capacity</div>
                                    <div class="text-bold"><%= Bikewale.Utility.FormatMinSpecs.ShowAvailable(modelPage.ModelVersionSpecs.ReserveFuelCapacity, "litres") %></div>
                                </li>
                                <li>
                                    <div class="specs-features-label">Fuel Efficiency Overall</div>
                                    <div class="text-bold"><%= Bikewale.Utility.FormatMinSpecs.ShowAvailable(modelPage.ModelVersionSpecs.FuelEfficiencyOverall, "kmpl") %></div>
                                </li>
                                <li>
                                    <div class="specs-features-label">Fuel Efficiency Range</div>
                                    <div class="text-bold"><%= Bikewale.Utility.FormatMinSpecs.ShowAvailable(modelPage.ModelVersionSpecs.FuelEfficiencyRange, "km") %></div>
                                </li>
                                <li>
                                    <div class="specs-features-label">Top Speed</div>
                                    <div class="text-bold"><%= Bikewale.Utility.FormatMinSpecs.ShowAvailable(modelPage.ModelVersionSpecs.TopSpeed, "kmph") %></div>
                                </li>
                            </ul>
                        </li>
                    </ul>

                    <h3 id="model-features-heading" class="margin-top20 padding-right20 padding-left20">Features</h3>

                    <ul class="specs-features-list model-features-list" id="model-main-features-list">
                        <li>
                            <div class="specs-features-label">Speedometer</div>
                            <div class="text-bold"><%= Bikewale.Utility.FormatMinSpecs.ShowAvailable(modelPage.ModelVersionSpecs.Speedometer) %></div>
                        </li>
                        <li>
                            <div class="specs-features-label">Fuel Guage</div>
                            <div class="text-bold"><%= Bikewale.Utility.FormatMinSpecs.ShowAvailable(modelPage.ModelVersionSpecs.FuelGauge) %></div>
                        </li>
                        <li>
                            <div class="specs-features-label">Tachometer Type</div>
                            <div class="text-bold"><%= Bikewale.Utility.FormatMinSpecs.ShowAvailable(modelPage.ModelVersionSpecs.Tachometer) %></div>
                        </li>
                        <li>
                            <div class="specs-features-label">Digital Fuel Guage</div>
                            <div class="text-bold"><%= Bikewale.Utility.FormatMinSpecs.ShowAvailable(modelPage.ModelVersionSpecs.DigitalFuelGauge) %></div>
                        </li>
                        <li>
                            <div class="specs-features-label">Tripmeter</div>
                            <div class="text-bold"><%= Bikewale.Utility.FormatMinSpecs.ShowAvailable(modelPage.ModelVersionSpecs.Tripmeter) %></div>
                        </li>
                        <li>
                            <div class="specs-features-label">Electric Start</div>
                            <div class="text-bold"><%= Bikewale.Utility.FormatMinSpecs.ShowAvailable(modelPage.ModelVersionSpecs.ElectricStart) %></div>
                        </li>
                    </ul>
                    <ul id="model-more-features-list" class="specs-features-list model-features-list">
                        <li>
                            <div class="specs-features-label">Tachometer</div>
                            <div class="text-bold"><%= Bikewale.Utility.FormatMinSpecs.ShowAvailable(modelPage.ModelVersionSpecs.Tachometer) %></div>
                        </li>
                        <li>
                            <div class="specs-features-label">Shift Light</div>
                            <div class="text-bold"><%= Bikewale.Utility.FormatMinSpecs.ShowAvailable(modelPage.ModelVersionSpecs.ShiftLight) %></div>
                        </li>
                        <li>
                            <div class="specs-features-label">No. of Tripmeters</div>
                            <div class="text-bold"><%= Bikewale.Utility.FormatMinSpecs.ShowAvailable(modelPage.ModelVersionSpecs.NoOfTripmeters) %></div>
                        </li>
                        <li>
                            <div class="specs-features-label">Tripmeter Type</div>
                            <div class="text-bold"><%= Bikewale.Utility.FormatMinSpecs.ShowAvailable(modelPage.ModelVersionSpecs.TripmeterType) %></div>
                        </li>
                        <li>
                            <div class="specs-features-label">Low Fuel Indicator</div>
                            <div class="text-bold"><%= Bikewale.Utility.FormatMinSpecs.ShowAvailable(modelPage.ModelVersionSpecs.LowFuelIndicator) %></div>
                        </li>
                        <li>
                            <div class="specs-features-label">Low Oil Indicator</div>
                            <div class="text-bold"><%= Bikewale.Utility.FormatMinSpecs.ShowAvailable(modelPage.ModelVersionSpecs.LowOilIndicator) %></div>
                        </li>
                        <li>
                            <div class="specs-features-label">Low Battery Indicator</div>
                            <div class="text-bold"><%= Bikewale.Utility.FormatMinSpecs.ShowAvailable(modelPage.ModelVersionSpecs.LowBatteryIndicator) %></div>
                        </li>
                        <li>
                            <div class="specs-features-label">Pillion Seat</div>
                            <div class="text-bold"><%= Bikewale.Utility.FormatMinSpecs.ShowAvailable(modelPage.ModelVersionSpecs.PillionSeat) %></div>
                        </li>
                        <li>
                            <div class="specs-features-label">Pillion Footrest</div>
                            <div class="text-bold"><%= Bikewale.Utility.FormatMinSpecs.ShowAvailable(modelPage.ModelVersionSpecs.PillionFootrest) %></div>
                        </li>
                    </ul>
                      <div class="padding-top10 padding-right20 padding-left20">		
                         <a href="javascript:void(0)"  class="view-features-link bw-ga" c="Model_Page" a="View_all_features_link_cliked" v="myBikeName" title="<%=bikeName %> Features" rel="nofollow">View all features</a>
                    </div>
                    <div class="margin-right20 margin-left20 border-solid-bottom padding-bottom20"></div>
                </div>
                <% } %>

                <%if (modelPage.ModelColors != null && modelPage.ModelColors.Count() > 0)
                { %>   
                <!-- colours code starts here -->    
                <div id="modelColoursContent" class="bw-model-tabs-data font14">
                    <h2 class="padding-top15 padding-right20 padding-left20"><%=bikeName %> Colours</h2>
                    <ul id="modelColorsList" class="padding-top5 padding-right20 padding-left20">
                    <asp:Repeater ID="rptColors" runat="server">
                            <ItemTemplate>                        
                                <li>
                                    <div class="color-box <%# (((IList)(DataBinder.Eval(Container.DataItem, "HexCodes"))).Count == 1 )?"color-count-one": (((IList)(DataBinder.Eval(Container.DataItem, "HexCodes"))).Count >= 3 )?"color-count-three":"color-count-two" %> inline-block">
                                        <asp:Repeater runat="server" DataSource='<%# DataBinder.Eval(Container.DataItem, "HexCodes") %>'>
                                            <ItemTemplate>
                                                    <span <%# String.Format("style='background-color: #{0}'",Convert.ToString(Container.DataItem)) %>></span>
                                            </ItemTemplate>
                                        </asp:Repeater>
                                    </div>
                                    <p class="font16 inline-block"><%# Convert.ToString(DataBinder.Eval(Container.DataItem, "ColorName")) %></p>
                                </li>
                            </ItemTemplate>
                    </asp:Repeater>
                    </ul>
                </div>
                <%} %>
                        <!-- colours code ends here -->
                <%if (Ad_300x250)
                   { %>
                <section>
                    <!-- #include file="/ads/Ad300x250_mobile.aspx" -->
                </section>
                <% } %>

                <% if (ctrlExpertReviews.FetchedRecordsCount > 0 || ctrlUserReviews.FetchedRecordsCount > 0)
                    { %>
                <div id="modelReviewsContent" class="bw-model-tabs-data margin-right20 margin-left20 padding-top20 padding-bottom20 border-solid-bottom font14">
                <h2><%=bikeName %> Reviews</h2>                      
                       
                    <% if (ctrlExpertReviews.FetchedRecordsCount > 0)
                        { %>
                    <BW:ExpertReviews runat="server" ID="ctrlExpertReviews" />
                    <% } %>
                    <%if (ctrlUserReviews.FetchedRecordsCount > 0)
                            { %>

                        <BW:UserReviews runat="server" ID="ctrlUserReviews" />

                    <% } %>
                        
                </div>
                <% } %>
                <%if (ctrlVideos.FetchedRecordsCount > 0)
                    { %>
                    <div id="modelVideosContent" class="bw-model-tabs-data margin-right20 margin-left20 padding-top15 padding-bottom20 border-solid-bottom font14">
                        <h2><%= bikeModelName %> Videos</h2>
                            <BW:Videos runat="server" ID="ctrlVideos" />
                    </div>
                <% } %>

                <%if (ctrlNews.FetchedRecordsCount > 0)
                 { %>
                 <BW:News runat="server" ID="ctrlNews" />
                <% } %>      

                <% if (ctrlAlternativeBikes.FetchedRecordsCount > 0)
                   { %>
                    <BW:AlternateBikes ID="ctrlAlternativeBikes" runat="server" />           
                <%} %>
                <div id="modelSpecsFooter"></div>
            </div>
        </section>
      
        <% 
            if (ctrlUserReviews.FetchedRecordsCount > 0)
            {
                reviewTabsCnt++;
                isUserReviewZero = false;
                isUserReviewActive = true;

            }
            if (ctrlExpertReviews.FetchedRecordsCount > 0)
            {
                reviewTabsCnt++;
                isExpertReviewZero = false;
                if (!isUserReviewActive)
                {
                    isExpertReviewActive = true;
                }
            }
            if (ctrlNews.FetchedRecordsCount > 0)
            {
                reviewTabsCnt++;
                isNewsZero = false;
                if (!isUserReviewActive && !isExpertReviewActive)
                {
                    isNewsActive = true;
                }
            }
            if (ctrlVideos.FetchedRecordsCount > 0)
            {
                reviewTabsCnt++;
                isVideoZero = false;
                if (!isUserReviewActive && !isExpertReviewActive && !isNewsActive)
                {
                    isVideoActive = true;
                }
            }
        %>       

     
        <!-- Terms and condition Popup Ends -->

        <!-- Lead Capture pop up start  -->
        <div id="leadCapturePopup" class="bw-popup bwm-fullscreen-popup contact-details hide">
            <div class="popup-inner-container text-center">
                <div class="bwmsprite close-btn leadCapture-close-btn rightfloat"></div>
                <div id="contactDetailsPopup">
                    <!-- Contact details Popup starts here -->
                    <p class="font18 margin-bottom5">Provide contact details</p>
                    <p class="text-light-grey margin-bottom5">Dealership will get back to you with offers</p>

                    <div class="personal-info-form-container margin-top10">
                        <div class="form-control-box">
                            <input type="text" class="form-control get-first-name" placeholder="Your name" id="getFullName" data-bind="value: fullName" />
                            <span class="bwmsprite error-icon "></span>
                            <div class="bw-blackbg-tooltip errorText"></div>
                        </div>
                        <div class="form-control-box margin-top20">
                            <input type="text" class="form-control get-email-id" placeholder="Email address" id="getEmailID" data-bind="value: emailId" />
                            <span class="bwmsprite error-icon"></span>
                            <div class="bw-blackbg-tooltip errorText"></div>
                        </div>
                        <div class="form-control-box margin-top20">
                            <p class="mobile-prefix">+91</p>
                            <input type="text" class="form-control get-mobile-no" maxlength="10" placeholder="Mobile no." id="getMobile" data-bind="value: mobileNo" />
                            <span class="bwmsprite error-icon"></span>
                            <div class="bw-blackbg-tooltip errorText"></div>
                        </div>
                        <div class="clear"></div>
                        <a class="btn btn-full-width btn-orange margin-top20" id="user-details-submit-btn" data-bind="event: { click: submitLead }">Submit</a>
                    </div>                    
                </div>
                <!-- Contact details Popup ends here -->
                 <!-- thank you message starts here -->
                <div id="notify-response" class="hide margin-top10 content-inner-block-20 text-center">
                        <p class="font18 text-bold margin-bottom20">Thank you <span class="notify-leadUser"></span></p>
                        <%if(viewModel != null){ %>
                            <p class="font16 margin-bottom40"><%=viewModel.Organization %>, <%=viewModel.AreaName %> will get in touch with you soon</p>
                        <%} %>
                        <input type="button" id="notifyOkayBtn" class="btn btn-orange" value="Okay" />
                </div>
                <!-- thank you message ends here -->
                <div id="otpPopup">
                    <p class="font18 margin-bottom5">Verify your mobile number</p>
                    <p class="text-light-grey margin-bottom5">We have sent OTP on your mobile. Please enter that OTP in the box provided below:</p>
                    <div>
                        <div class="lead-mobile-box lead-otp-box-container margin-bottom10 font22">
                            <span class="bwmsprite tel-grey-icon"></span>
                            <span class="text-light-grey font24">+91</span>
                            <span class="lead-mobile font24">9876543210</span>
                            <span class="bwmsprite edit-blue-icon edit-mobile-btn"></span>
                        </div>
                        <div class="otp-box lead-otp-box-container">
                            <div class="form-control-box margin-bottom10">
                                <input type="text" class="form-control" placeholder="Enter your OTP" id="getOTP" maxlength="5" data-bind="value: otpCode" />
                                <span class="bwmsprite error-icon errorIcon"></span>
                                <div class="bw-blackbg-tooltip errorText"></div>
                            </div>
                            <a class="margin-left10 blue resend-otp-btn margin-top10" id="resendCwiCode" data-bind="visible: (NoOfAttempts() < 2), click: function () { regenerateOTP() }">Resend OTP</a>
                            <p class="margin-left10 margin-top10 otp-notify-text text-light-grey font12" data-bind="visible: (NoOfAttempts() >= 2)">
                                OTP has been already sent to your mobile
                            </p>
                            <a class="btn btn-full-width btn-orange margin-top20" id="otp-submit-btn">Confirm</a>
                        </div>
                        <div class="update-mobile-box">
                            <div class="form-control-box text-left">
                                <p class="mobile-prefix">+91</p>
                                <input type="text" class="form-control padding-left40" placeholder="Mobile no." maxlength="10" id="getUpdatedMobile" data-bind="value: mobileNo" />
                                <span class="bwmsprite error-icon errorIcon"></span>
                                <div class="bw-blackbg-tooltip errorText"></div>
                            </div>
                            <input type="button" class="btn btn-orange margin-top20" value="Send OTP" id="generateNewOTP" data-bind="event: { click: submitLead }" />
                        </div>
                    </div>

                </div>
                <!-- OTP Popup ends here -->
            </div>
        </div>
        <!-- Lead Capture pop up end  -->
        <!-- Terms and condition Popup start -->
        <div class="termsPopUpContainer content-inner-block-20 hide" id="termsPopUpContainer">
            <div class="fixed-close-btn-wrapper">
                <div id="termsPopUpCloseBtn" class="termsPopUpCloseBtn bwmsprite fixed-close-btn cross-lg-lgt-grey cur-pointer"></div>
            </div>
            <h3>Terms and Conditions</h3>
            <div class="hide" style="vertical-align: middle; text-align: center;" id="termspinner">
                <img src="http://imgd2.aeplcdn.com/0x0/bw/static/sprites/d/loader.gif" />
            </div>
            <div id="terms" class="breakup-text-container padding-bottom10 font14">
            </div>
            <div id='orig-terms' class="hide">
            </div>
        </div>
        <!-- Terms and condition Popup end -->

        <div id="dealer-offers-popup" class="bwm-fullscreen-popup">
            <div class="offers-popup-close-btn position-abt pos-top15 pos-right15 bwmsprite cross-lg-lgt-grey cur-pointer"></div>
            <div class="icon-outer-container rounded-corner50percent margin-bottom10">
                <div class="icon-inner-container rounded-corner50percent text-center">
                    <span class="offers-sprite offers-box-icon margin-top12"></span>
                </div>
            </div>
            <p class="font16 text-bold text-center margin-bottom20">Offers from this dealer</p>
            <ul class="dealer-offers-list margin-bottom25">
                <li>
                    <asp:Repeater ID="rptOffers" runat="server">
                        <ItemTemplate>
                            <li>
                                <span class="dealer-offer-image offers-sprite offerIcon_<%# Convert.ToString(DataBinder.Eval(Container.DataItem, "OfferCategoryId"))%>_sm"></span>
                                <span class="dealer-offer-label"><%# Convert.ToString(DataBinder.Eval(Container.DataItem, "offerText")) %>
                                    <span class="tnc font9 <%# Convert.ToBoolean(DataBinder.Eval(Container.DataItem, "IsOfferTerms"))? string.Empty: "hide" %>" id="<%# Convert.ToString(DataBinder.Eval(Container.DataItem, "OfferId")) %>">View terms</span>
                                </span>
                                
                            </li>
                        </ItemTemplate>
                    </asp:Repeater>
                </li>
            </ul>
            <div class="text-center">
                <a id="getofferspopup" leadSourceId="31"  href="javascript:void(0);" class="btn btn-orange text-bold">Get offers from dealer</a>
            </div>
        </div>
        <% if(viewModel != null){ %>
        <div id="more-dealers-popup" class="bwm-fullscreen-popup">
            <div class="dealers-popup-close-btn position-abt pos-top15 pos-right15 bwmsprite cross-lg-lgt-grey cur-pointer"></div>
            <div class="icon-outer-container rounded-corner50percent margin-bottom10">
				<div class="icon-inner-container rounded-corner50percent text-center">
					<span class="offers-sprite offers-box-icon margin-top12"></span>
				</div>
			</div>
            <p class="font16 text-bold text-center margin-bottom15"><%=viewModel.SecondaryDealerCount %> partner dealers near you</p>
            <ul>
                <asp:Repeater ID="rptSecondaryDealers" runat="server">
                    <ItemTemplate>
                        <li class="secondary-dealer-card">
                            <a href="javascript:void(0)" onclick="secondarydealer_Click(<%# Convert.ToString(DataBinder.Eval(Container.DataItem, "DealerId")) %>)">
						        <div>
							        <span class="grid-9 alpha omega font14 text-default text-bold"><%# Convert.ToString(DataBinder.Eval(Container.DataItem, "Name")) %></span>
							        <span class="grid-3 omega text-light-grey text-right"><%# Convert.ToString(DataBinder.Eval(Container.DataItem, "Distance")) %> kms</span>
							        <div class="clear"></div>
							        <span class="font12 text-light-grey"><%# Convert.ToString(DataBinder.Eval(Container.DataItem, "Area")) %></span>
							        <div class="margin-top15">
                                        <div class="grid-4 alpha omega">
                                            <p class="font12 text-light-grey margin-bottom5">On-road price</p>
                                            <span class="bwmsprite inr-xsm-icon"></span>&nbsp;<span class="font16 text-default text-bold"><%# Bikewale.Utility.Format.FormatPrice(DataBinder.Eval(Container.DataItem, "SelectedVersionPrice").ToString()) %></span>
                                        </div>
                                        <div class="border-solid-left grid-8 padding-top10 padding-left20 omega <%# Convert.ToBoolean(DataBinder.Eval(Container.DataItem,"IsPremiumDealer")) && (Convert.ToUInt16(DataBinder.Eval(Container.DataItem,"OfferCount"))> 0)?  string.Empty : "hide" %>">
                                            <span class="bwmsprite offers-sm-box-icon"></span>
                                            <span class="font14 text-default text-bold"><%# Convert.ToString(DataBinder.Eval(Container.DataItem, "OfferCount")) %></span>
                                            <span class="font12 text-light-grey">Offers available</span>
                                        </div>
								        <div class="clear"></div>
							        </div>
						        </div>
					        </a>
                        </li>
                    </ItemTemplate>
                </asp:Repeater>
            </ul>
        </div>
        <% } %>
        <!-- #include file="/includes/footerBW_Mobile.aspx" -->
        <!--[if lt IE 9]>
            <script src="<%= staticUrl != "" ? "http://st2.aeplcdn.com" + staticUrl : "" %>/m/src/html5.js"></script>
        <![endif]-->
        <link href="<%= staticUrl != "" ? "http://st2.aeplcdn.com" + staticUrl : "" %>/m/css/bwm-common-btf.css?<%= staticFileVersion %>" rel="stylesheet" type="text/css" />
        <link href="<%= staticUrl != "" ? "http://st2.aeplcdn.com" + staticUrl : "" %>/m/css/bwm-model-btf.css?<%= staticFileVersion %>" rel="stylesheet" type="text/css" />
        <script type="text/javascript" src="<%= staticUrl != "" ? "http://st2.aeplcdn.com" + staticUrl : "" %>/m/src/common.min.js?<%= staticFileVersion %>"></script>
        <script type="text/javascript" src="<%= staticUrl != "" ? "http://st2.aeplcdn.com" + staticUrl : "" %>/m/src/bwm-model.js?<%= staticFileVersion %>"></script>
        <link href='https://fonts.googleapis.com/css?family=Open+Sans:400,700' rel='stylesheet' type='text/css' />
        <script type="text/javascript">
            var leadSourceId;
            vmModelId = '<%= modelId%>';
            clientIP = '<%= clientIP%>';
            cityId = '<%= cityId%>';
            isUsed = '<%= !modelPage.ModelDetails.New %>';
            var pageUrl = "<%= canonical %>";
            var myBikeName = "<%= this.bikeName %>";
            var versionName = "<%= variantText %>"
            ga_pg_id = '2';
            if (bikeVersionLocation == '') {
                bikeVersionLocation = getBikeVersionLocation();
            }
            if (bikeVersion == '') {
                bikeVersion = getBikeVersion();
            }
            var getCityArea = GetGlobalCityArea();
            $(document).ready(function (e) {
                $(".leadcapturesubmit").on('click', function () {
                    openLeadPopup($(this));
                });
                $("#templist input").on("click", function () {
                    if ($(this).attr('data-option-value') == $('#hdnVariant').val()) {
                        return false;
                    }
                    $('.dropdown-select-wrapper #defaultVariant').text($(this).val());
                    $('#hdnVariant').val($(this).attr('data-option-value'));
                    dataLayer.push({ 'event': 'Bikewale_all', 'cat': 'Model_Page', 'act': 'Version_Change', 'lab': bikeVersionLocation });
                });

                if ($('#getMoreDetailsBtn').length > 0) {
                    dataLayer.push({ 'event': 'Bikewale_all', 'cat': 'Model_Page', 'act': 'Get_More_Details_Shown', 'lab': myBikeName + '_' + getBikeVersion() + '_' + getCityArea });
                }
                if ($('#btnGetOnRoadPrice').length > 0) {
                    dataLayer.push({ 'event': 'Bikewale_noninteraction', 'cat': 'Model_Page', 'act': 'Get_On_Road_Price_Button_Shown', 'lab': myBikeName + '_' + getBikeVersion() + '_' + getCityArea });
                }
                if ($("#getAssistance").length > 0) {
                    dataLayer.push({ "event": "Bikewale_noninteraction", "cat": "Model_Page", "act": "Get_Offers_Shown", "lab": myBikeName + "_" + getBikeVersion() + '_' + getCityArea });
                }

            });
           
            function secondarydealer_Click(dealerID) {
                var rediurl = "CityId=" + cityId + "&AreaId=" + areaId + "&PQId=" + pqId + "&VersionId=" + versionId + "&DealerId=" + dealerID + "&IsDealerAvailable=true";
                window.location.href = "/m/pricequote/dealerpricequote.aspx?MPQ=" + Base64.encode(rediurl);
            }
            $("#viewprimarydealer, #dealername").on("click", function () {
                var rediurl = "CityId=" + cityId + "&AreaId=" + areaId + "&PQId=" + pqId + "&VersionId=" + versionId + "&DealerId=" + dealerId + "&IsDealerAvailable=true";
                window.location.href = "/m/pricequote/dealerpricequote.aspx?MPQ=" + Base64.encode(rediurl);
            });
            
        </script>
    </form>
</body>
</html>