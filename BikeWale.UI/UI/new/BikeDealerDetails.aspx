<%@ Page Language="C#" Inherits="Bikewale.New.BikeDealerDetails" AutoEventWireup="false" EnableViewState="false" %>

<%@ Register Src="~/controls/DealerCard.ascx" TagName="DealerCard" TagPrefix="BW" %>
<%@ Register Src="~/controls/LeadCaptureControl.ascx" TagName="LeadCapture" TagPrefix="BW" %>
<%@ Register Src="~/controls/MostPopularBikes_new.ascx" TagName="MostPopularBikesMake" TagPrefix="BW" %>
<%@ Register Src="~/controls/ServiceCenterCard.ascx" TagName="ServiceCenterCard" TagPrefix="BW" %>
<!DOCTYPE html>
<html>
<head>
    <%
        keywords = String.Format("{0}, {0} dealer, {0} Showroom, {0} {1}", dealerName, cityName);
        description = String.Format("{2} is an authorized {0} showroom in {1}. Get address, contact details direction, EMI quotes etc. of {2} {0} showroom.", makeName, cityName, dealerName);
        title = String.Format("{0} | {0} showroom in {1} - BikeWale", dealerName, cityName);
        canonical = String.Format("https://www.bikewale.com/dealer-showrooms/{0}/{1}/{2}-{3}/", makeMaskingName, cityMaskingName, dealerMaskingName, dealerId);
        alternate = String.Format("https://www.bikewale.com/m/dealer-showrooms/{0}/{1}/{2}-{3}/", makeMaskingName, cityMaskingName, dealerMaskingName, dealerId);
        AdId = "1395986297721";
        AdPath = "/1017752/BikeWale_New_";
        isAd970x90Shown = true;
        isAd970x90BottomShown = true;
        isAd300x250Shown = false;
        isAd300x250BTFShown = false;
    %>
    <!-- #include file="/includes/headscript_desktop_min.aspx" -->
    <link rel="stylesheet" type="text/css" href="/css/dealer/details.css" />
    <script type="text/javascript">
        <!-- #include file="\includes\gacode_desktop.aspx" -->
    </script>
</head>
<body class="bg-light-grey padding-top50">
    <form id="Form1" runat="server">
        <!-- #include file="/includes/headBW.aspx" -->
        <section>
            <div class="container padding-top10">
                <div class="grid-12">
                    <div class="breadcrumb margin-bottom15">
                        <!-- breadcrumb code starts here -->
                        <ul>
                            <li itemscope="" itemtype="http://data-vocabulary.org/Breadcrumb">
                                <a itemprop="url" href="/"><span itemprop="title">Home</span></a>
                            </li>
                            <li itemscope="" itemtype="http://data-vocabulary.org/Breadcrumb">
                                <span class="bwsprite fa-angle-right margin-right10"></span><a itemprop="url" href="/dealer-showrooms/"><span itemprop="title">Showroom Locator</span></a>
                            </li>
                            <li itemscope="" itemtype="http://data-vocabulary.org/Breadcrumb">
                                <span class="bwsprite fa-angle-right margin-right10"></span><a itemprop="url" href="/dealer-showrooms/<%=makeMaskingName %>/"><span itemprop="title"><%=makeName%> Showroom</span></a>
                            </li>
                            <li itemscope="" itemtype="http://data-vocabulary.org/Breadcrumb">
                                <span class="bwsprite fa-angle-right margin-right10"></span><a itemprop="url" href="/dealer-showrooms/<%=makeMaskingName %>/<%=cityMaskingName %>/"><span itemprop="title"><%=makeName%> Showroom in <%=cityName%></span></a>
                            </li>
                            <li class="current"><span class="bwsprite fa-angle-right margin-right10"></span><%=dealerName %></li>
                        </ul>
                        <div class="clear"></div>
                    </div>
                </div>
                <div class="clear"></div>
            </div>
        </section>
        <section>
            <div class="container margin-bottom20" id="dealerInfo">
                <div class="grid-12">
                    <div class="content-box-shadow">
                        <div class="content-box-shadow padding-14-20">
                            <h1><%= !String.IsNullOrEmpty(areaName) ? string.Format("{0}, {2}, {1}",dealerName,cityName, areaName) : string.Format("{0}, {1}",dealerName,cityName) %></h1>
                        </div>
                        <div class="content-inner-block-20">
                            <div class="grid-7 alpha omega font14">
                                <%if (dealerObj != null)
                                  { %>

                                <div class="margin-bottom10">
                                    <%if (dealerObj.DealerType == (int)(Bikewale.Entities.PriceQuote.DealerPackageTypes.Premium) || dealerObj.DealerType == (int)(Bikewale.Entities.PriceQuote.DealerPackageTypes.Deluxe))
                                      { %>
                                    <span class="featured-tag inline-block margin-right10"><span class="bwsprite star-white"></span>Featured</span>
                                    <%} %>
                                    <h2 class="font14 text-black text-bold inline-block">Authorized <%=makeName %> dealer in <%=cityName %></h2>
                                </div>

                                <% if (!string.IsNullOrEmpty(address))
                                   {%>
                                <div class="margin-bottom10">
                                    <span class="bwsprite dealership-loc-icon vertical-top"></span>
                                    <span class="vertical-top text-light-grey dealership-card-details"><%=String.IsNullOrEmpty(pincode)?string.Format("{0}, {1}",address,cityName):string.Format("{0}, {1}, {2}",address,pincode,cityName)%></span>
                                </div>
                                <%} %>
                                <% if (!string.IsNullOrEmpty(maskingNumber))
                                   {%>
                                <div class="margin-bottom10">
                                    <span class="bwsprite phone-black-icon vertical-top"></span>
                                    <span class="vertical-top text-bold dealership-card-details"><%=maskingNumber %></span>
                                </div>
                                <%} %>
                                <% if (!string.IsNullOrEmpty(eMail))
                                   {%>
                                <div class="margin-bottom10">
                                    <span class="bwsprite mail-grey-icon vertical-top"></span>
                                    <a href="mailto:<%=eMail %>" target="_blank" rel="noopener" class="vertical-top text-light-grey" rel="nofollow">
                                        <span class="dealership-card-details"><%=eMail %></span>
                                    </a>
                                </div>
                                <%} %>
                                <% if (!string.IsNullOrEmpty(workingHours))
                                   {%>
                                <div class="margin-bottom10">
                                    <span class="bwsprite clock-icon vertical-top"></span>
                                    <span class="vertical-top text-light-grey dealership-card-details">Working hours: <%=workingHours %></span>
                                </div>
                                <%} %>
                                <%} %>
                                <% if (dealerObj != null && dealerObj.Area != null && dealerObj.Area.Latitude > 0 && dealerObj.Area.Longitude > 0)
                                   { %>
                                <div id="commute-distance-form" class="margin-top20">
                                    <p class="text-bold margin-bottom15">Get commute distance and time:</p>
                                    <div class="leftfloat form-control-box margin-right15">
                                        <input id="locationSearch" type="text" class="form-control" placeholder="Enter your location" />
                                        <span id="getUserLocation" class="crosshair-icon font12 position-abt pos-right10 pos-top10 cur-pointer"></span>
                                    </div>
                                    <div class="location-details padding-top10 padding-bottom10 leftfloat">
                                        Distance: <span id="commuteDistance" class="margin-right10"></span>
                                        Time: <span id="commuteDuration"></span>
                                    </div>
                                    <div class="clear"></div>
                                    <div id="commuteResults"></div>
                                </div>
                                <% } %>
                            </div>
                            <div class="grid-5 omega position-rel">
                                <% if (dealerObj != null && dealerObj.Area != null && dealerObj.Area.Latitude > 0 && dealerObj.Area.Longitude > 0)
                                   { %>
                                <div id="dealer-map" style="width: 378px; height: 254px; border: 1px solid #eee;"></div>
                                <div id="get-direction-button" title="Get directions">
                                    <a href="https://maps.google.com/?saddr=&amp;daddr=<%=dealerObj.Area.Latitude %>,<%=dealerObj.Area.Longitude %>" target="_blank" rel="noopener">
                                        <span class="bwsprite get-direction-icon"></span>
                                    </a>
                                </div>
                                <% } %>
                            </div>
                            <div class="clear"></div>
                        </div>
                    </div>
                </div>
                <div class="clear"></div>
            </div>
        </section>
        <%if (dealerObj != null && dealerObj.CampaignId > 0 && (dealerObj.DealerType == (int)(Bikewale.Entities.PriceQuote.DealerPackageTypes.Premium) || dealerObj.DealerType == (int)(Bikewale.Entities.PriceQuote.DealerPackageTypes.Deluxe)))
          { %>
        <section id="dealerAssistance">
            <div class="container margin-bottom20" id="leadForm">
                <div class="grid-12">
                    <div id="buyingAssistance" class="content-box-shadow content-inner-block-20">
                        <h2 class="font18 margin-bottom20">Complete buying assistance from <%=dealerName %></h2>
                        <p class="font14 text-light-grey margin-bottom25">Get in touch with <%=dealerName %> for best offers, test rides, EMI options, exchange benefits and much more...</p>
                        <div class="input-box form-control-box type-user-details margin-right20">
                            <input type="text" id="assistGetName" data-bind="textInput: fullName" />
                            <label for="assistGetName">Name</label>
                            <span class="boundary"></span>
                            <span class="error-text"></span>
                        </div>
                        <div class="input-box form-control-box type-user-details margin-right20">
                            <input type="email" id="assistGetEmail" data-bind="textInput: emailId" />
                            <label for="assistGetEmail">Email</label>
                            <span class="boundary"></span>
                            <span class="error-text"></span>
                        </div>
                        <div class="input-box input-number-box form-control-box type-user-details">
                            <input type="tel" id="assistGetMobile" maxlength="10" data-bind="textInput: mobileNo" />
                            <label for="assistGetMobile">Mobile number</label>
                            <span class="input-number-prefix">+91</span>
                            <span class="boundary"></span>
                            <span class="error-text"></span>
                        </div>
                        <div class="type-dropdown">
                            <p class="font12 text-light-grey">Bike</p>
                            <div class="dropdown-select-wrapper">
                                <select id="getLeadBike" class="dropdown-select form-control chosen-select">
                                    <option value="">Select a bike</option>
                                    <%foreach (var model in dealerDetails.Models)
                                      { %>
                                    <option value="<%=model.objVersion.VersionId %>"><%=model.BikeName %></option>
                                    <%} %>
                                </select>
                                <span class="boundary"></span>
                                <span class="error-text"></span>
                            </div>
                        </div>
                        <div class="type-sumit-button">
                            <input type="button" id="dealer-assist-btn" data-isregisterpq="true" data-item-name="<%=dealerObj.Name %>" data-item-area="<%=dealerObj.Area%>" data-leadsourceid="14" class="btn btn-orange margin-bottom5 " data-isleadpopup="false" data-pqsourceid="<%= (int) Bikewale.Entities.PriceQuote.PQSourceEnum.Desktop_dealer_details_Get_offers %>"
                                data-item-id="<%= dealerId %>" data-bind="click: function (d, e) { validateBikeData(); validateUserLeadDetails(); HiddenSubmitLead(d, e) }" value="<%= ctaSmallText %>" />
                        </div>
                        <p>By proceeding ahead, you agree to BikeWale <a title="Visitor agreement" href="/visitoragreement.aspx" target="_blank" rel="noopener">visitor agreement</a> and <a title="Privacy policy" href="/privacypolicy.aspx" target="_blank" rel="noopener">privacy policy</a>.</p>
                    </div>
                    <div id="dealer-assist-msg" class="hide">
                        <p class="leftfloat font14">Thank you for your interest. <span data-bind="text: dealerName()"></span>&nbsp;will get in touch shortly</p>
                        <span id="assistance-response-close-btn" class="rightfloat bwsprite cross-lg-lgt-grey cur-pointer"></span>
                        <div class="clear"></div>
                    </div>
                </div>
                <div class="clear"></div>
            </div>
        </section>
        <section>
            <div class="container margin-bottom20">
                <div class="grid-12">
                    <div class="content-box-shadow padding-left5 padding-right5 inner-content-card font14">
                        <h2 class="font18 margin-bottom20 padding-top20 padding-left15">Bikes available at <%=dealerName%></h2>
                        <ul>
                            <%foreach (var model in dealerDetails.Models)
                              { %>
                            <li>
                                <a href="<%= Bikewale.Utility.UrlFormatter.BikePageUrl(model.MakeMaskingName , model.objModel.MaskingName) %>" title="<%= String.Format("{0} {1}", model.objMake.MakeName,model.objModel.ModelName) %>">
                                    <div class="model-jcarousel-image-preview">
                                        <img class="lazy" data-original="<%= Bikewale.Utility.Image.GetPathToShowImages(model.OriginalImagePath, model.HostURL, Bikewale.Utility.ImageSize._476x268)%>" alt="<%=String.Format("{0} {1}", model.objMake.MakeName,model.objModel.ModelName) %>" src="" />
                                    </div>
                                    <div class="card-desc-block">
                                        <h3 class="bikeTitle" title="<%=String.Format("{0} {1}", model.objMake.MakeName,model.objModel.ModelName) %>"><%=String.Format("{0} {1}", model.objMake.MakeName,model.objModel.ModelName) %></h3>
                                        <div class="text-xt-light-grey margin-bottom10">
                                            <%= Bikewale.Utility.FormatMinSpecs.GetMinSpecs(model.Specs.Displacement.ToString(),model.Specs.FuelEfficiencyOverall.ToString(),model.Specs.MaxPower.ToString(),model.Specs.KerbWeight.ToString())%>
                                        </div>
                                        <p class="text-light-grey margin-bottom5">On-road price, <%= cityName %></p>
                                        <div>
                                            <span class="bwsprite inr-lg"></span>
                                            <span class="font18 text-default text-bold"><%=Bikewale.Common.CommonOpn.FormatPrice(model.VersionPrice.ToString()) %></span>
                                        </div>
                                    </div>
                                    <div class="clear"></div>
                                </a>
                                <% if(areaId > 0){  %>
                                <div class="margin-left20 margin-bottom20">
                                    <a href="javascript:void(0)" data-pqsourceid="<%= (int) Bikewale.Entities.PriceQuote.PQSourceEnum.Desktop_DealerLocator_Detail_AvailableModels %>" data-modelid="<%= model.objModel.ModelId %>" data-versionid="<%= model.objVersion.VersionId %>" data-areaid="<%= pqAreaId %>" data-cityid="<%= pqCityId %>" data-areaname="<%= pqAreaName %>" data-cityname="<%= cityName %>" class="btn btn-sm btn-grey font14 dealerDetails" rel="nofollow">Show detailed price</a>
                                </div>
                                <%} %>
                            </li>
                            <%} %>
                        </ul>
                    </div>
                </div>
                <div class="clear"></div>
            </div>
        </section>
        <%} %>
        <% if (ctrlDealerCard.showWidget)
           { %>
        <section>
            <div class="container margin-bottom20">
                <div class="grid-12">
                    <div class="content-box-shadow">
                        <BW:DealerCard runat="server" ID="ctrlDealerCard" />
                    </div>
                </div>
                <div class="clear"></div>
            </div>
        </section>
        <% } %>
        <% if (ctrlPopoularBikeMake.FetchedRecordsCount > 0)
           { %>
        <section>
            <div class="container margin-bottom20">
                <div class="grid-12">
                    <div class="content-box-shadow">
                        <div class="bw-model-tabs-data padding-top20 padding-bottom20 border-solid-bottom font14">
                            <div class="carousel-heading-content">
                                <div class="swiper-heading-left-grid inline-block">
                                    <h2>Popular <%=makeName %> bikes in <%=cityName %></h2>
                                </div><div class="swiper-heading-right-grid inline-block text-right">
                                    <a href="/<%= makeMaskingName %>-bikes/" title="<%= makeName %> Bikes" class="btn view-all-target-btn">View all</a>
                                </div>
                                <div class="clear"></div>
                            </div>
                            <BW:MostPopularBikesMake runat="server" ID="ctrlPopoularBikeMake" />
                        </div>
                    </div>
                </div>
                <div class="clear"></div>
            </div>
        </section>
        <%} %>
        <% if (ctrlServiceCenterCard.showWidget)
           { %>
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
        <section>
            <div class="container margin-top10 margin-bottom30">
                <div class="grid-12 font12">
                    <span class="font14"><strong>Disclaimer</strong>:</span> The above mentioned information about <%=makeName%> dealership showrooms in <%=cityName%> is furnished to the best of our knowledge. 
                        All <%=makeName%> bike models and colour options may not be available at each of the <%=makeName%> dealers. 
                        We recommend that you call and check with your nearest <%=makeName%> dealer before scheduling a showroom visit.
                </div>
                <div class="clear"></div>
            </div>
        </section>        
        <script type="text/javascript" src="<%= staticUrl %>/src/frameworks.js?<%=staticFileVersion %>"></script>
        <!-- #include file="/includes/footerBW.aspx" -->
        <script type="text/javascript">
            var versionId = "";
            var dealerid = <%= dealerId %>;
            <% if (Bikewale.Utility.GlobalCityArea.GetGlobalCityArea().CityId == 0 && pqCityId > 0 && !String.IsNullOrEmpty(cityName))
               { %>                   
            $(document).ready(function () {
                SetCookieInDays("location","<%= String.Format("{0}_{1}",pqCityId,cityName) %>",365);
            });
            <%}%>
            $("#getLeadBike").change(function () {
                var val = $(this).val();
                if (val && val != "" && val != "0") {
                    versionId = val;
                }
            });

            var dealerLat = '<%= dealerObj!= null && dealerObj.Area!= null ? dealerObj.Area.Latitude.ToString() : string.Empty %>';
            var dealerLong = '<%= dealerObj!= null && dealerObj.Area!= null ? dealerObj.Area.Longitude.ToString() : string.Empty %>';
            var $ddlModels = $("#assistGetModel");
            var pqId = null;
            var currentCityName = '<%=cityName%>';
            var bikeCityId = '<%=cityId%>';
            var googleMapAPIKey = "<%= Bikewale.Utility.BWConfiguration.Instance.GoogleMapApiKey %>";
            var pageUrl = window.location.href;
            var clientIP = '<%= clientIP %>';
            var leadSourceId, pqSourceId;
            var pageSrcId = eval("<%= Bikewale.Utility.BWConfiguration.Instance.SourceId %>");
            var googleMapAPIKey = "<%= Bikewale.Utility.BWConfiguration.Instance.GoogleMapApiKey %>";
            var makeName = "<%= makeName%>";            
            $(document).on("change", $ddlModels, function () {
                hideError($ddlModels);
            });

        </script>
        <BW:LeadCapture ID="ctrlLeadCapture" runat="server" />

        <link href="<%= staticUrl  %>/css/bw-common-btf.css?<%=staticFileVersion %>" rel="stylesheet" type="text/css" />
        <!-- #include file="/includes/footerscript.aspx" -->
        <script type="text/javascript" src="<%= staticUrl  %>/src/dealer/details.js?<%= staticFileVersion %>"></script>
        <script src="https://maps.googleapis.com/maps/api/js?key=<%= Bikewale.Utility.BWConfiguration.Instance.GoogleMapApiKey %>&libraries=places&callback=initializeMap"></script>
        <!-- #include file="/includes/fontBW.aspx" -->
    </form>
</body>
</html>
