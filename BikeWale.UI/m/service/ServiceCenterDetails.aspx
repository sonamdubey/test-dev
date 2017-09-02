<%@ Page Language="C#" AutoEventWireup="false" Inherits="Bikewale.Mobile.Service.ServiceCenterDetails" EnableViewState="false" %>
<%@ Register Src="~/m/controls/DealersCard.ascx" TagName="DealerCard" TagPrefix="BW" %>
<%@ Register Src="~/m/controls/ServiceCenterCard.ascx" TagName="ServiceCenterCard" TagPrefix="BW" %>
<%@ Register Src="~/m/controls/serviceschedule.ascx" TagName="ServiceSchedule" TagPrefix="BW" %>
<%@ Register Src="~/m/controls/MMostPopularBikes.ascx" TagName="PopularBikeMake" TagPrefix="BW" %>
<%@ Register Src="~/m/controls/usedBikeModel.ascx" TagName="usedBikeModel" TagPrefix="BW" %>

<!DOCTYPE html>
<html>
<head>
    <%
        keywords = String.Format("{0}, {0} {1}, {2} servicing {1}", serviceCenteName, serviceCity, makeName);
        description = String.Format("{0} is an authorised service center of {1}. Get all details related to servicing cost, pick and drop facility and service schedule from {0}", serviceCenteName,makeName );
        title = String.Format("{0} {1} | {0} service center in {1} - BikeWale ", serviceCenteName, serviceCity);
        canonical = String.Format("https://www.bikewale.com{0}", Bikewale.Utility.UrlFormatter.GetServiceCenterUrl(makeMaskingName, cityMaskingName, serviceCenteName, serviceCenterId));
        Ad_320x50 = true;
        Ad_Bot_320x50 = true;
    %>
    <!-- #include file="/includes/headscript_mobile_min.aspx" -->
    <link rel="stylesheet" type="text/css" href="/m/css/service/details.css" />
    <script type="text/javascript">
        <!-- #include file="\includes\gacode_mobile.aspx" -->
    </script>
</head>
<body class="bg-light-grey">
    <form runat="server">
        <!-- #include file="/includes/headBW_Mobile.aspx" -->
        <% if (objServiceCenterData != null)
        { %>
             <section>
            <div class="container bg-white card-bottom-margin">
                <h1 class="card-header"><%= objServiceCenterData.Name %>, <%= objServiceCenterData.CityName %></h1>
                <div class="card-inner-padding font14 text-light-grey">
                    <h2 class="font14 margin-bottom15"><%= string.Format("Authorized {0} service center", makeName) %></h2>

                    <%if (!string.IsNullOrEmpty(objServiceCenterData.Address))
                     { %>
                    <p class="margin-bottom10">
                        <span class="bwmsprite dealership-loc-icon vertical-top"></span>
                        <span class="vertical-top details-column text-light-grey"><%= objServiceCenterData.Address %></span>
                    </p>
                    <% } %>

                    <% if (!(String.IsNullOrEmpty(objServiceCenterData.Mobile)) || !(String.IsNullOrEmpty(objServiceCenterData.Phone)))
                    { %>
                    <div class="margin-bottom10">
                        <% if(!objServiceCenterData.Phone.Contains(",")) { %>
                        <a href="tel:<%=objServiceCenterData.Phone %>" class="text-default text-bold maskingNumber">
                            <span class="bwmsprite tel-sm-grey-icon vertical-top"></span>
                            <span class="vertical-top text-bold details-column"><%=objServiceCenterData.Phone %></span>
                        </a>
                        <% } else { %>
                        <a href="javascript:void(0)" class="text-default text-bold maskingNumber contact-service-btn" data-service-name="<%= objServiceCenterData.Name %>" data-service-number="<%= objServiceCenterData.Phone %>" rel="nofollow">
                            <span class="bwmsprite tel-sm-grey-icon vertical-top"></span>
                            <span class="vertical-top text-bold details-column"><%=objServiceCenterData.Phone %></span>
                        </a>
                        <% } %>
                    </div>
                    <% } %>

                    <% if (!string.IsNullOrEmpty(objServiceCenterData.Email))
                    { %>
                    <div class="margin-bottom10">
                        <a href="mailto:<%= objServiceCenterData.Email %>" class="text-light-grey">
                            <span class="bwmsprite mail-grey-icon vertical-top"></span>
                            <span class="vertical-top details-column text-light-grey"><%= objServiceCenterData.Email %></span>
                        </a>
                    </div>
                    <% } %>

                    <% if (serviceLat > 0 && serviceLong > 0)
                       { %>
                    <div class="border-solid-bottom margin-bottom15 padding-top10"></div>

                    <h2 class="font14 text-default margin-bottom15">Get commute distance and time:</h2>
                    <div class="form-control-box margin-bottom15">
                        <input id="locationSearch" type="text" class="form-control padding-right40" placeholder="Type in your location" />
                        <span id="getUserLocation" class="crosshair-icon position-abt"></span>
                    </div>
                    <div class="location-details margin-bottom15">
                        Distance: <span id="commuteDistance" class="margin-right10"></span>
                        Time: <span id="commuteDuration"></span>
                    </div>
                    <div id="commuteResults"></div>
                    <a id="linkMap" href="http://maps.google.com/maps?z=12&t=m&q=loc:<%= serviceLat %>,<%= serviceLong %>" target="_blank" rel="noopener"><span class="bwmsprite get-direction-icon margin-right5"></span>Get directions</a>
                    <% } %>
                </div>
            </div>
        </section>
       <% } %>

        <div class="modal-background"></div>
        <div id="contact-service-popup" class="modal-popup-container">
            <div class="popup-header"></div>
            <div class="popup-body">
                <p class="body-label">Select one of the phone numbers to talk to service center executive</p>
                <ul class="popup-list margin-bottom20"></ul>
                <div class="grid-6 alpha">
                    <p class="btn btn-white btn-full-width btn-size-0 cancel-popup-btn">Cancel</p>
                </div>
                <div class="grid-6 omega">
                    <a href="" id="call-service-btn" class="btn btn-orange btn-full-width btn-size-0">Call</a>
                </div>
                <div class="clear"></div>
            </div>
        </div>

        <section>
            <div class="container bg-white box-shadow padding-top15">
            <% if (ctrlServiceCenterCard.showWidget && ctrlServiceCenterCard.centerData!=null && ctrlServiceCenterCard.centerData.Count>1)
                   { %>
                    <BW:ServiceCenterCard runat="server" ID="ctrlServiceCenterCard" />
                <% }  %>
            </div>
        </section>
        <script type="text/javascript" src="<%= staticUrl  %>/m/src/frameworks.js?<%= staticFileVersion %>"></script>

        <section>
            <!-- schedule widget start -->
            <BW:ServiceSchedule runat="server" ID="ctrlServiceSchedule" />
        </section>

        <section>
            <div class="container bg-white box-shadow card-bottom-margin">
                <h2 class="padding-top15 padding-right20 padding-left20">Tips from BikeWale experts to keep your bike in good shape!</h2>
                <ul id="bw-tips-list">
                    <li>
                        <a href="/m/bike-care/" target="_blank" rel="noopener" title="Bike Care - Maintenance tips" >
                            <span class="service-sprite care-icon"></span>
                            <h3 class="text-unbold margin-left10 vertical-middle">Bike Care - Maintenance tips</h3>
                            <span class="bwmsprite right-arrow"></span>
                        </a>
                    </li>
                    <li>
                        <a href="/m/bike-troubleshooting/" target="_blank" rel="noopener" title="Bike troubleshooting - FAQs">
                            <span class="service-sprite faq-icon"></span>
                            <h3 class="text-unbold margin-left10 vertical-middle">Bike troubleshooting - FAQs</h3>
                            <span class="bwmsprite right-arrow"></span>
                        </a>
                    </li>
                </ul>
            </div>
        </section>
            <% if (ctrlPopoularBikeMake.FetchedRecordsCount > 0 || ctrlusedBikeModel.FetchCount > 0)
               {%>
        <section>
            <div class="container bg-white box-shadow card-bottom-margin">
                <% if (ctrlPopoularBikeMake.FetchedRecordsCount > 0)
                 {%> 

                 <BW:PopularBikeMake runat="server" ID="ctrlPopoularBikeMake" />
                <%} %>

                <div class="padding-top10 text-center">
                    <!-- #include file="/ads/Ad300x250_mobile.aspx" -->
                </div>
                <div class="margin-right10 margin-left10 border-solid-bottom"></div>

                           <% if (ctrlusedBikeModel.FetchCount>0)
                       { %>
                 <div class="padding-top15">
                    <BW:usedBikeModel runat="server" ID="ctrlusedBikeModel" />
                  </div>      
                    <% } %> 
            </div>            
        </section>
        <% } %>
        <% if (ctrlDealerCard.showWidget) { %>
        <section>
            <div class="container bg-white box-shadow card-bottom-margin">
                <BW:DealerCard runat="server" ID="ctrlDealerCard" />
            </div>
        </section>
         <% }  %>

        <section>
            <div class="container margin-bottom20 font12 padding-top5 padding-right20 padding-left20">
                <span class="font14"><strong>Disclaimer:</strong></span> The above-mentioned information about authorised <%= makeName %> service centers is furnished to best of our knowledge. The facilities of pick and drop, timings and service schedule related information might vary slightly from service center to service center. Please check with the authorised <%= makeName %> service center before scheduling an appointment.
            </div>
        </section>

        <!-- #include file="/includes/footerBW_Mobile.aspx" -->
        <link href="<%= staticUrl  %>/m/css/bwm-common-btf.css?<%= staticFileVersion %>" rel="stylesheet" type="text/css" />
        <link href="<%= staticUrl  %>/css/chosen.min.css?<%= staticFileVersion %>" type="text/css" rel="stylesheet" />
        <!-- #include file="/includes/footerscript_mobile.aspx" -->
        <script type="text/javascript" src="<%= staticUrl  %>/m/src/chosen-jquery-min-mobile.js?<%= staticFileVersion %>"></script>
        <script src="https://maps.googleapis.com/maps/api/js?key=<%= Bikewale.Utility.BWConfiguration.Instance.GoogleMapApiKey %>&libraries=places&callback=initializeMap" async defer></script>
        <script type="text/javascript">
            var versionId, dealerId = "<%= dealerId %>", cityId = "<%= cityId %>", clientIP = "<%= clientIP%>",campaignId = "<%= campaignId %>";
             var serviceLat = "<%= serviceLat %>", serviceLong = "<%= serviceLong%>";
             var bodHt, footerHt, scrollPosition, leadSourceId;
             var googleMapAPIKey = "<%= Bikewale.Utility.BWConfiguration.Instance.GoogleMapApiKey%>";
            var pageUrl = window.location.href;
        </script>
        <script type="text/javascript" src="<%= staticUrl  %>/m/src/service/details.js?<%= staticFileVersion %>"></script>
        <!-- #include file="/includes/fontBW_Mobile.aspx" -->
    </form>
</body>
</html>
