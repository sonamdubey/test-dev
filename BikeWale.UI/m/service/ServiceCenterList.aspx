<%@ Page Language="C#" AutoEventWireup="false" Inherits="Bikewale.Mobile.Service.ServiceCenterList" EnableViewState="false" %>
<%@ Register Src="~/m/controls/LeadCaptureControl.ascx" TagName="LeadCapture" TagPrefix="BW" %>
<%@ Register Src="~/m/controls/DealersCard.ascx" TagName="DealerCard" TagPrefix="BW" %>
<!DOCTYPE html>
<html>
<head>

    <%
        keywords = String.Format("{0} servicing {1}, {0} service center in {1}, {0} Service centers, {0} service schedules, {0} bike repair, repairing, servicing ", makeName, cityName);
        description = String.Format("There are {0} {1} service centers in {2}. Get in touch with your nearest {1} service center for service repairing, schedule details, pricing, pick and drop facility. Check the Service schedule for {1} bikes now.",totalServiceCenters,  makeName, cityName);
        title = String.Format("{0} service centers in {1} | {0} bike servicing in {1} - BikeWale ", makeName, cityName);
        canonical = String.Format("http://www.bikewale.com/{0}-service-center-in-{1}/", makeMaskingName, urlCityMaskingName);
        Ad_320x50 = true;
        Ad_Bot_320x50 = true;
    %>

    <!-- #include file="/includes/headscript_mobile_min.aspx" -->
    <link rel="stylesheet" type="text/css" href="/m/css/service/listing.css">

    <script type="text/javascript">
        <!-- #include file="\includes\gacode_mobile.aspx" -->

        var makeName = "<%=makeName%>";
        var makeMaskingName = "<%=makeMaskingName%>";
        var makeId = "<%=makeId%>";
        var cityName = "<%=cityName%>";
        var cityId = "<%= cityId%>";
        var cityMaskingName = "<%= cityMaskingName%>";
        var clientIP = "<%= clientIP %>";
        var pageUrl = "<%= pageUrl%>";
        var pqSrcId = "<%= Convert.ToUInt16(Bikewale.Entities.PriceQuote.PQSourceEnum.Mobile_DealerLocator_Listing) %>";
        var pageSrcId = eval("<%= Bikewale.Utility.BWConfiguration.Instance.SourceId %>");
        var googleMapAPIKey = "<%= Bikewale.Utility.BWConfiguration.Instance.GoogleMapApiKey %>";
    </script>
</head>
<body class="bg-light-grey">
    <form runat="server">
        <!-- #include file="/includes/headBW_Mobile.aspx" -->

        <section>
            <div class="container bg-white card-bottom-margin">
                <h1 class="card-header"><%=makeName%> service center<% if(totalServiceCenters > 0 ) { %>s<% } %> in <%=cityName%></h1>
                <div class="card-inner-padding font14 text-light-grey">
                    <p id="service-main-content">Is your <%= makeName %> bike due for a service or are you looking to repair your <%= makeName %> bike? BikeWale brings you the list of all authorised <%= makeName %> service centers in <%= cityName %>.<% if(totalServiceCenters > 0 ) { %> <%= makeName %> has <%= totalServiceCenters %> authorised</p><p id="service-more-content"> service center<% if(totalServiceCenters > 1 ) { %>s<%} %> in <%= cityName %>. <%} %> We recommend availing services only from authorised service centers.<br />Authorised <%= makeName %> service centers abide by the servicing standards of Honda with an assurance of genuine <%= makeName %> spare parts. BikeWale strongly recommends to use only <%= makeName %> genuine spare parts for your safety and durability of your bike. For more information on pick-up and drop facility, prices and service schedules get in touch with any of the below mentioned authorised <%= makeName %> service centers in City. Do check out the maintenance tips and answers to FAQs from BikeWale experts! </p><a href="javascript:void(0)" id="read-more-target" rel="nofollow">... Read more</a>
                </div>
            </div>
        </section>

        <% if(totalServiceCenters > 0) { %>
        <section>
            <div class="container bg-white box-shadow card-bottom-margin">
                <h2 class="padding-15-20 border-solid-bottom"><%=totalServiceCenters %> <%=makeName%> service center<% if(totalServiceCenters > 1 ) { %>s<%} %> in <%=cityName%></h2>
                <ul id="center-list">
                    <% foreach (var serviceCenter in serviceCentersList)
                       { %>
                            <li>
                                <a href="/m<%= Bikewale.Utility.UrlFormatter.GetServiceCenterUrl(makeMaskingName, urlCityMaskingName, serviceCenter.Name, serviceCenter.ServiceCenterId) %> "  title="<%= serviceCenter.Name %> | <%= makeName %> | <%= cityName %>" class="center-list-item">
                                    <h3 class="text-truncate margin-bottom5 text-black">
                                        <%= serviceCenter.Name %>
                                    </h3>
                                    <% if(!(String.IsNullOrEmpty(serviceCenter.Address))) { %>
                                    <p class = "margin-bottom5" >
                                        <span class="bwmsprite dealership-loc-icon vertical-top"></span>
                                        <span class="vertical-top details-column text-light-grey"> <%= serviceCenter.Address %></span>
                                    </p>
                                </a>
                                    <% } %>
                                <% if(!serviceCenter.Phone.Contains(",")) { %>
                                    <% if(!(String.IsNullOrEmpty(serviceCenter.Phone))) {  %>
                                    <span class="vertical-top bwmsprite tel-sm-grey-icon"></span>
                                    <a href="tel:<%= serviceCenter.Phone.Trim()%>" class="vertical-top details-column text-bold text-default"><%= serviceCenter.Phone.Trim() %></a>
                                <% } %>
                                <% } else { %>
                            <a href="javascript:void(0)" class="text-default text-bold maskingNumber contact-service-btn" data-service-name="<%= serviceCenter.Name %>" data-service-number="<%= serviceCenter.Phone %>" rel="nofollow">
                                <span class="bwmsprite tel-sm-grey-icon vertical-top"></span>
                                <span class="vertical-top text-bold details-column"><%=serviceCenter.Phone %></span>
                            </a>
                        <% } %>
                                <% if(!(String.IsNullOrEmpty(serviceCenter.Phone))) { %>
                                <div class="margin-top15">
                                    <% if(!serviceCenter.Phone.Contains(",")) { %>
                                    <a href="tel:<%= serviceCenter.Phone.Trim()%>" class="btn btn-inv-green service-btn"><span class="bwmsprite tel-green-icon margin-right10"></span>Call service centre</a>
                                <% } else { %>
                                    <button type="button" class="btn btn-inv-green service-btn contact-service-btn" data-service-name="<%= serviceCenter.Name %>" data-service-number="<%= serviceCenter.Phone.Trim()%>"><span class="bwmsprite tel-green-icon margin-right10"></span>Call service centre</button>
                                    <% } %>
                                </div>
                                <% } %>
                            </li>
                    <% } %>
                </ul>
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
            <div class="container bg-white box-shadow card-bottom-margin">
                <h2 class="padding-top15 padding-right20 padding-left20">Tips from BikeWale experts to keep your bike in good shape!</h2>
                <ul id="bw-tips-list">
                    <li>
                        <a href="/m/bike-care/" target="_blank" title="Bike Care - Maintenance tips">
                            <span class="service-sprite care-icon"></span>
                            <h3 class="text-unbold margin-left10 vertical-middle">Bike Care - Maintenance tips</h3>
                            <span class="bwmsprite right-arrow"></span>
                        </a>
                    </li>
                    <li>
                        <a href="/m/bike-troubleshooting/" target="_blank" title="Bike troubleshooting - FAQs">
                            <span class="service-sprite faq-icon"></span>
                            <h3 class="text-unbold margin-left10 vertical-middle">Bike troubleshooting - FAQs</h3>
                            <span class="bwmsprite right-arrow"></span>
                        </a>
                    </li>
                </ul>
            </div>
        </section>


        <% if (ctrlDealerCard.showWidget) { %>
        <section>
            <div class="container bg-white box-shadow card-bottom-margin">
                <div class="margin-right20 margin-left20 padding-top15">
                    <h2 class="margin-bottom5">Looking to buy a new <%= makeName %> bike in <%=cityName %>?</h2>
                    <p>Check out authorised <%= makeName %> dealers in <%=cityName %></p>
                </div>
                <BW:DealerCard runat="server" ID="ctrlDealerCard" />
            </div>            
        </section>
         <% }  %>

        <section>
            <div class="container margin-bottom20 font12 padding-top5 padding-right20 padding-left20">
                <span class="font14"><strong>Disclaimer:</strong></span> The above-mentioned information about authorised <%=makeName %> service centers is furnished to best of our knowledge. The facilities of pick and drop, timings and service schedule related information might vary slightly from service center to service center. Please check with the authorised <%=makeName %> service center before scheduling an appointment.

            </div>
        </section>

        <script type="text/javascript" src="<%= staticUrl != "" ? "http://st1.aeplcdn.com" + staticUrl : "" %>/m/src/frameworks.js?<%= staticFileVersion %>"></script>
        <!-- #include file="/includes/footerBW_Mobile.aspx" -->
        <BW:LeadCapture ID="ctrlLeadCapture" runat="server" />
        <link href="<%= staticUrl != "" ? "http://st2.aeplcdn.com" + staticUrl : "" %>/m/css/bwm-common-btf.css?<%= staticFileVersion %>" rel="stylesheet" type="text/css" />
        <script type="text/javascript" src="<%= staticUrl != "" ? "http://st2.aeplcdn.com" + staticUrl : "" %>/m/src/common.min.js?<%= staticFileVersion %>"></script>
        <script type="text/javascript" src="<%= staticUrl != "" ? "http://st2.aeplcdn.com" + staticUrl : "" %>/m/src/service/listing.js?<%= staticFileVersion %>"></script>
        <link href='https://fonts.googleapis.com/css?family=Open+Sans:400,700' rel='stylesheet' type='text/css' />

    </form>
</body>
</html>
