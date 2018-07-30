<%@ Page Language="C#" AutoEventWireup="false" Inherits="Bikewale.Mobile.DealerDetails" EnableViewState="false" %>
<%@ Register Src="~/m/controls/DealersCard.ascx" TagName="DealerCard" TagPrefix="BW" %>
<%@ Register Src="~/m/controls/LeadCaptureControl.ascx" TagName="LeadCapture" TagPrefix="BW" %>
<%@ Register Src="~/m/controls/MMostPopularBikes.ascx" TagName="PopularBikeMake" TagPrefix="BW" %>
<%@ Register Src="~/m/controls/ServiceCenterCard.ascx" TagName="ServiceCenterCard" TagPrefix="BW" %>
<!DOCTYPE html>
<html>
<head>
    <%
        keywords = String.Format("{0}, {0} dealer, {0} Showroom, {0} {1}", dealerName, dealerCity);
        description = String.Format("{2} is an authorized {0} showroom in {1}. Get address, contact details direction, EMI quotes etc. of {2} {0} showroom.", makeName, dealerCity, dealerName);
        title = String.Format("{0} | {0} showroom in {1} - BikeWale", dealerName, dealerCity);
        canonical =  String.Format("https://www.bikewale.com{0}",Bikewale.Utility.UrlFormatter.GetDealerUrl(makeMaskingName, cityMaskingName, dealerName, (int)dealerId));
    %>
    <!-- #include file="/includes/headscript_mobile_min.aspx" -->
    <link rel="stylesheet" type="text/css" href="/m/css/dealer/details.css" />
    <script type="text/javascript">
        <!-- #include file="\includes\gacode_mobile.aspx" -->
    </script>
</head>
<body class="bg-light-grey">
    <form runat="server">
        <!-- #include file="/includes/headBW_Mobile.aspx" -->
        
        <section class="container bg-white margin-bottom10">
            <div class="bg-white box-shadow">
                <% if(dealerArea.Length > 0) { %>
                <h1 class="box-shadow padding-15-20"><%=string.Format("{0},{1}, {2}",dealerDetails.Name,dealerArea, dealerCity)%></h1>
                <% } else { %>
                <h1 class="box-shadow padding-15-20"><%=string.Format("{0},{1}",dealerDetails.Name,dealerCity)%></h1>
                <% } %>
                <div class="dealer-details position-rel pos-top-3 content-inner-block-20 font14">
                    <%if (dealerDetails.DealerType == (int)(Bikewale.Entities.PriceQuote.DealerPackageTypes.Premium) || dealerDetails.DealerType == (int)(Bikewale.Entities.PriceQuote.DealerPackageTypes.Deluxe))
                    { %>
                    <div class="margin-bottom20">
                        <span class="featured-tag inline-block margin-right10"><span class="bwmsprite star-white"></span>Featured</span>
                        <h2 class="font14 text-bold inline-block"><%= string.Format("Authorized {0} dealer in {1}", makeName, dealerCity) %></h2>
                    </div>
                    <%} else { %>
                    <div class="margin-bottom20">
                        <h2 class="font14 text-bold inline-block"><%= string.Format("Authorized {0} dealer in {1}", makeName, dealerCity) %></h2>
                    </div>
                    <% } %>
                    <%if (!string.IsNullOrEmpty(dealerDetails.Address))
                     { %>
                    <p class="margin-bottom10">
                        <span class="bwmsprite dealership-loc-icon vertical-top"></span>
                        <span class="vertical-top dealership-details text-light-grey"><%=String.IsNullOrEmpty(dealerDetails.Pincode)?string.Format("{0}, {1}",dealerDetails.Address,dealerCity):string.Format("{0}, {1}, {2}",dealerDetails.Address,dealerDetails.Pincode,dealerCity) %></span>
                    </p>
                    <% } %>
                    <% if (!string.IsNullOrEmpty(dealerDetails.MaskingNumber))
                    { %>
                    <div class="margin-bottom10">
                        <a href="tel:<%= dealerDetails.MaskingNumber %>" class="text-default text-bold maskingNumber">
                            <span class="bwmsprite tel-sm-grey-icon vertical-top"></span>
                            <span class="vertical-top text-bold dealership-details"><%= dealerDetails.MaskingNumber %></span>
                        </a>
                    </div>
                    <% } %>
                    <% if (!string.IsNullOrEmpty(dealerDetails.EMail))
                    { %>
                    <div class="margin-bottom10">
                        <a href="mailto:<%= dealerDetails.EMail %>" class="text-light-grey">
                            <span class="bwmsprite mail-grey-icon vertical-top"></span>
                            <span class="vertical-top dealership-details text-light-grey"><%= dealerDetails.EMail %></span>
                        </a>
                    </div>
                    <% } %>
                    <% if (!string.IsNullOrEmpty(dealerDetails.WorkingHours))
                    { %>
                    <div class="margin-bottom10">
                        <span class="bwmsprite clock-icon vertical-top"></span>
                        <span class="vertical-top dealership-details text-light-grey">Working hours: <%= dealerDetails.WorkingHours %></span>
                    </div>
                    <%} %>
                    <% if(dealerLat> 0 && dealerLong>0) { %>
                    <div class="border-solid-bottom margin-bottom15 padding-top10"></div>
                    
                    <h2 class="font14 text-default margin-bottom15">Get commute distance and time:</h2>
                    <div class="form-control-box margin-bottom15">
                        <input id="locationSearch" type="text" class="form-control padding-right40" placeholder="Enter your location" />
                        <span id="getUserLocation" class="crosshair-icon position-abt"></span>
                    </div>
                    <div class="location-details margin-bottom15">
                        Distance: <span id="commuteDistance" class="margin-right10"></span>
                        Time: <span id="commuteDuration"></span>
                    </div>
                    <div id="commuteResults"></div>
                    <a id="anchorGetDir" href="http://maps.google.com/maps?z=12&t=m&q=loc:<%= dealerLat %>,<%= dealerLong %>" target="_blank" rel="noopener"><span class="bwmsprite get-direction-icon margin-right5"></span>Get directions</a>
                    <% } %>
                </div>

                <% if (campaignId > 0 && dealerDetails.DealerType != (int)(Bikewale.Entities.PriceQuote.DealerPackageTypes.Standard))
                   { %>
                <div class="grid-12 float-button clearfix float-fixed">
                    <% if (!string.IsNullOrEmpty(maskingNumber))
                       { %>
                    <div class="grid-6 alpha padding-right5">
                        <a data-leadsourceid="15" class=" btn btn-orange btn-full-width rightfloat leadcapturebtn" href="javascript:void(0);"><%= ctaSmallText %></a>
                    </div>
                    <div class="grid-6 omega padding-left5">
                        <a id="calldealer" class="btn btn-green btn-full-width rightfloat" href="tel:<%= dealerDetails.MaskingNumber %>">
                            <span class="bwmsprite tel-white-icon margin-right5"></span>Call dealer</a>
                    </div>
                    <% } else 
                      { %>
                    <div class="grid-12 alpha padding-right5">
                        <a data-leadsourceid="15" class=" btn btn-orange btn-full-width rightfloat leadcapturebtn" href="javascript:void(0);"><%= ctaSmallText %></a>
                    </div>
                    <% } %>
                    <div class="clear"></div>
                </div>
                <div class="clear"></div>
            </div>

            <% } %>
        </section>

        <%if (dealerBikesCount > 0 && campaignId > 0 && (dealerDetails.DealerType == (int)(Bikewale.Entities.PriceQuote.DealerPackageTypes.Premium) || dealerDetails.DealerType == (int)(Bikewale.Entities.PriceQuote.DealerPackageTypes.Deluxe)))
              
          { %>
        <section class="container bg-white margin-bottom10">
            <div class="box-shadow padding-top15 padding-right20 padding-bottom5 padding-left20">
                <h2 class="font18 margin-bottom15">Bikes available at <%= dealerName %></h2>
                <ul id="model-available-list">
					<% foreach(var model in dealerModels) { %>
                            <li>
                                <a class="modelurl" href='/m<%= Bikewale.Utility.UrlFormatter.BikePageUrl(model.objMake.MaskingName, model.objModel.MaskingName) %>'>
                                    <div class="image-block">
                                        <div class="image-content">
                                            <img class="lazy"
                                                data-original="<%= Bikewale.Utility.Image.GetPathToShowImages(model.OriginalImagePath, model.HostURL, Bikewale.Utility.ImageSize._310x174) %>"
                                                alt="<%= model.BikeName %>" src="https://imgd.aeplcdn.com/0x0/bw/static/sprites/m/circleloader.gif">
                                        </div>
                                    </div>

                                    <div class="details-block">
                                        <h3 class="font16 margin-bottom10 text-black text-truncate"><%= model.BikeName %></h3>
                                        <div class="font14 text-x-light margin-bottom5">
                                            <%= Bikewale.Utility.FormatMinSpecs.GetMinSpecsSpanElements(model.MinSpecsList) %>
                                        </div>
                                        <div class="text-default">
                                            <span class="font14 text-light-grey"> On-road Price, <%= cityName %></span>
                                            </div>
                                        <div class="text-default">
                                            <span class="bwmsprite inr-sm-icon"></span>
                                            <span class="font18 text-bold"><%= Bikewale.Utility.Format.FormatPrice(Convert.ToString(model.VersionPrice)) %></span>
                                        </div>
                                        <% if(pqAreaId > 0){ %>    
                                        <div class="text-default">
                                            <a rel="nofollow" class="btn btn-white btn-sm-1 margin-top5 inline-block dealerDetails" href="javascript:void(0)" data-versionid="<%= model.objVersion.VersionId %>" data-pqsourceid="<%= (int) Bikewale.Entities.PriceQuote.PQSourceEnum.Mobile_DealerLocator_Detail_AvailableModels %>" data-modelid="<%= model.objModel.ModelId %>">Show detailed price</a>
                                        </div>              
                                        <%} %>                  
                                    </div>                                
                                </a>
                            </li>
						<% } %>
                </ul>
            </div>
        </section>
        <%} %>
        <script type="text/javascript" src="<%= staticUrl  %>/m/src/frameworks.js?<%= staticFileVersion %>"></script>
        <section class="container bg-white margin-bottom10">
            <div class="box-shadow">
                <!-- dealer card -->
                <% if (ctrlDealerCard.showWidget) { %>
                    <BW:DealerCard runat="server" ID="ctrlDealerCard" />
                <% }  %>
            </div>
        </section>   
             <% if (ctrlPopoularBikeMake.FetchedRecordsCount > 0 ) {%>
        <section>
                 <div class="container bg-white box-shadow margin-bottom15">
                     <BW:PopularBikeMake runat="server" ID="ctrlPopoularBikeMake" />
                </div>
                        
        </section>
        <% } %>

         <BW:LeadCapture ID="ctrlLeadCapture" runat="server" />

         <% if (ctrlServiceCenterCard.showWidget)
            { %>
            <div class="container bg-white box-shadow padding-top15 margin-bottom10">
                <BW:ServiceCenterCard runat="server" ID="ctrlServiceCenterCard" />
            </div>
         <% }  %>

         <section>
            <div class="container margin-bottom30">
                <div class="grid-12 font12">
                    <span class="font14"><strong>Disclaimer:</strong></span> The above mentioned information about <%=makeName %> dealership showrooms in <%=cityName %> is furnished to the best of our knowledge. 
                        All <%=makeName %> bike models and colour options may not be available at each of the <%=makeName %> dealers. 
                        We recommend that you call and check with your nearest <%=makeName %> dealer before scheduling a showroom visit.
                </div>
                <div class="clear"></div>
            </div>
        </section>

        <!-- #include file="/includes/footerBW_Mobile.aspx" -->
        <link href="<%= staticUrl  %>/m/css/bwm-common-btf.css?<%= staticFileVersion %>" rel="stylesheet" type="text/css" />
        <!-- #include file="/includes/footerscript_mobile.aspx" -->
        <script type="text/javascript">
             var versionId, dealerId = "<%= dealerId %>", cityId = "<%= cityId %>", clientIP = "<%= Bikewale.Common.CommonOpn.GetClientIP()%>",campaignId = "<%= campaignId %>";                                              
             var dealerLat = "<%= dealerLat %>", dealerLong = "<%= dealerLong%>";
             var bodHt, footerHt, scrollPosition, leadSourceId;                         
             var googleMapAPIKey = "<%= Bikewale.Utility.BWConfiguration.Instance.GoogleMapApiKey%>";
             var pageUrl = window.location.href;
            <% if (Bikewale.Utility.GlobalCityArea.GetGlobalCityArea().CityId == 0 && pqCityId > 0 && !String.IsNullOrEmpty(cityName))
               { %>
            $(document).ready(function () {
                SetCookieInDays("location", "<%= String.Format("{0}_{1}",pqCityId,cityName) %>", 365);
            });
            <%}%>
            $(window).scroll(function () {
                bodHt = $('body').height();
                footerHt = $('footer').height();
                scrollPosition = $(this).scrollTop();
                if ($('.float-button').hasClass('float-fixed')) {
                    if (scrollPosition + $(window).height() > (bodHt - footerHt))
                        $('.float-button').removeClass('float-fixed').hide();
                }
                if (scrollPosition + $(window).height() < (bodHt - footerHt))
                    $('.float-button').addClass('float-fixed').show();
            });
                     
           $(".leadcapturebtn").click(function(e){
               ele = $(this);
               var leadOptions = {
                   "dealerid" : dealerId,                    
                   "leadsourceid" : ele.attr('data-leadsourceid'),
                   "pqsourceid" : ele.attr('data-pqsourceid'),
                   "pageurl" : pageUrl,
                   "clientip" : clientIP,
                   "isregisterpq": true,
                   "isdealerbikes": true,
                   "campid": campaignId
               };
               dleadvm.setOptions(leadOptions);
           });
          
           $(".dealerDetails").click(function (e) {
               ele = $(this);
               checkCookies();
               $('#priceQuoteWidget,#popupContent,.blackOut-window').show();
               $('#popupWrapper').addClass('loader-active');
               $('#popupWrapper,#popupContent').show();
               var options = {
                   "modelId": ele.data('modelid'),
                   "versionid" :ele.data('versionid') ,
                   "cityId": "<%= pqCityId %>",
                   "areaId": "<%= pqAreaId %>",
                   "city": "<%= cityName %>",
                   "area": "<%= pqAreaName %>",
                   "pqsourceid": ele.data('pqsourceid'),
                   "pagesrcid": ele.data('pqsourceid'),
                   "dealerid": dealerId
               };
               vmquotation.setOptions(options);
           });

        </script>
        <script src="https://maps.googleapis.com/maps/api/js?key=<%= Bikewale.Utility.BWConfiguration.Instance.GoogleMapApiKey %>&libraries=places&callback=initializeMap" async defer></script>
        <script type="text/javascript" src="<%= staticUrl  %>/m/src/dealer/details.js?<%= staticFileVersion %>"></script>
        <!-- #include file="/includes/fontBW_Mobile.aspx" -->
    </form>
</body>
</html>
