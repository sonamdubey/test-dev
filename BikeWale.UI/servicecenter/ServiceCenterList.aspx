<%@ Page Language="C#" Inherits="Bikewale.Service.ServiceCenterList" AutoEventWireup="false" EnableViewState="false" %>

<%@ Register Src="~/controls/DealerCard.ascx" TagName="DealerCard" TagPrefix="BW" %>
<%@ Register Src="~/controls/UsedBikeWidget.ascx" TagName="UsedBikes" TagPrefix="BW" %>
<%@ Register Src="~/controls/MostPopularBikes_new.ascx" TagName="MostPopularBikesMake" TagPrefix="BW" %>
<%@ Register Src="~/controls/BrandCityPopUp.ascx" TagName="BrandCity" TagPrefix="BW" %>

<!DOCTYPE html>

<html>
<head>
    <%      
        keywords = String.Format("{0} servicing {1}, {0} service center in {1}, {0} Service centers, {0} service schedules, {0} bike repair, repairing, servicing", makeName, cityName);
        description = String.Format("There are {0} {1} service centers in {2}. Get in touch with your nearest {1} service center for service repairing, schedule details, pricing, pick and drop facility. Check the Service schedule for {1} bikes now.", totalServiceCenters, makeName, cityName);
        title = String.Format("{0} service centers in {1} | {0} bike servicing in {1} - BikeWale", makeName, cityName);
        canonical = String.Format("https://www.bikewale.com/{0}-service-center-in-{1}/", makeMaskingName, urlCityMaskingName);
        alternate = String.Format("https://www.bikewale.com/m/{0}-service-center-in-{1}/", makeMaskingName, urlCityMaskingName);
        AdId = "1395986297721";
        AdPath = "/1017752/BikeWale_New_";
        isAd970x90Shown = true;
        isAd970x90BottomShown = true;
        isAd300x250Shown = false;
        isAd300x250BTFShown = false;
        isHeaderFix = false;
    %>
    <!-- #include file="/includes/headscript_desktop_min.aspx" -->
    <link rel="stylesheet" type="text/css" href="/css/service/listing.css" />
    <script src="https://maps.googleapis.com/maps/api/js?key=<%= Bikewale.Utility.BWConfiguration.Instance.GoogleMapApiKey %>&libraries=places"></script>
    <script type="text/javascript">
        <!-- #include file="\includes\gacode_desktop.aspx" --> 
    </script>

</head>
<body class="bg-light-grey">
    <form runat="server">
        <!-- #include file="/includes/headBW.aspx" -->
        <section>
            <div class="container padding-top10">
                <div class="grid-12">
                    <div class="breadcrumb margin-bottom15">
                        <!-- breadcrumb code starts here -->
                        <ul>
                            <li itemscope="" itemtype="http://data-vocabulary.org/Breadcrumb">
                                <a itemprop="url" href="/" title="Home"><span itemprop="title">Home</span></a>
                            </li>
                            <li itemscope="" itemtype="http://data-vocabulary.org/Breadcrumb">
                                <span class="bwsprite fa-angle-right margin-right10"></span><a itemprop="url" href="/bike-service-center/" title="Service Center Locator"><span itemprop="title">Service Center Locator</span></a>
                            </li>
                            <li itemscope="" itemtype="http://data-vocabulary.org/Breadcrumb">
                                <span class="bwsprite fa-angle-right margin-right10"></span><a itemprop="url" href="<%= String.Format("/{0}-service-center-in-india/", makeMaskingName) %>" title="<%= String.Format("{0} Bikes Service Centers", makeName) %>"><span itemprop="title"><%=makeName%> Bikes Service Centers</span></a>
                            </li>
                            <li class="current"><span class="bwsprite fa-angle-right margin-right10"></span><%=makeName%> Bikes Service Center In <%=cityName %></li>
                        </ul>
                        <div class="clear"></div>
                    </div>
                </div>
                <div class="clear"></div>
            </div>
        </section>

        <section>
            <div class="container section-bottom-margin">
                <div class="grid-12">
                    <div class="bg-white">
                        <h1 class="section-header"><%=makeName%> service center<%=(totalServiceCenters > 1)?"s":""%> in <%=cityName%></h1>
                        <div class="section-inner-padding font14 text-light-grey">
                            <p id="service-main-content">Is your <%= makeName %> bike due for a service or are you looking to repair your <%= makeName %> bike? BikeWale brings you the list of all authorised  <%= makeName %> service centers in <%= cityName %>. <%= makeName %> has <%= totalServiceCenters %> authorised service center<% if (totalServiceCenters > 1)
                                                                                                                                                                                                                                                                                                                                               { %>s<%}%> in <%= cityName %>. We recommend availing services only from authorised service centers. Authorised <%= makeName %> service centers abide by the servicing standards of <%= makeName %> with an assurance of genuine <%= makeName %> spare parts..</p>
                            <p id="service-more-content">BikeWale strongly recommends to use only <%= makeName %> genuine spare parts for your safety and durability of your bike. For more information on pick-up and drop facility, prices and service schedules get in touch with any of the below mentioned authorised <%= makeName %> service centers in <%= cityName %>. Do check out the maintenance tips and answers to FAQs from BikeWale experts! </p>
                            <a href="javascript:void(0)" id="read-more-target" rel="nofollow">Read more</a>
                        </div>
                    </div>
                </div>
                <div class="clear"></div>
            </div>
        </section>

        <% if (totalServiceCenters > 0)
           { %>
        <section>
            <div class="container section-bottom-margin">
                <div class="grid-12">
                    <div class="content-box-shadow">
                        <h2 class="section-h2-title padding-15-20"><%=totalServiceCenters %> <%=makeName %> service center<% if (totalServiceCenters > 1)
                                                                                                                             { %>s<%}%> in <%=cityName %>
                            <a href="Javascript:void(0)" id="brandSelect"><span class="margin-left10 bwsprite edit-blue"></span>change</a>
                        </h2>
                        <div id="listing-left-column" class="grid-4 alpha omega">
                            <ul id="center-list">
                                <% if (serviceCentersList != null)
                                   {
                                       foreach (var serviceCenter in serviceCentersList)
                                       { %>

                                <li data-item-id="<%= serviceCenter.ServiceCenterId %>" data-item-inquired="false" data-lat="<%= serviceCenter.Lattitude %>" data-log="<%= serviceCenter.Longitude %>" data-item-number="<%= serviceCenter.Phone%>" data-address="<%= serviceCenter.Address %>"
                                    data-item-url="<%= Bikewale.Utility.UrlFormatter.GetServiceCenterUrl(makeMaskingName, urlCityMaskingName, serviceCenter.Name, Convert.ToUInt32(serviceCenter.ServiceCenterId)) %> ">

                                    <a href="<%= Bikewale.Utility.UrlFormatter.GetServiceCenterUrl(makeMaskingName, urlCityMaskingName, serviceCenter.Name, Convert.ToUInt32(serviceCenter.ServiceCenterId)) %>" title="<%= serviceCenter.Name %>" class="dealer-card-target font14">
                                        <div class="dealer-card-content">
                                            <h3 class="dealer-name text-black text-bold margin-bottom5"><%= serviceCenter.Name %></h3>
                                            <% if (!(String.IsNullOrEmpty(serviceCenter.Address)))
                                               { %>
                                            <p class="text-light-grey margin-bottom5">
                                                <span class="bwsprite dealership-loc-icon vertical-top margin-right5"></span>
                                                <span class="vertical-top details-column"><%= serviceCenter.Address %></span>
                                            </p>
                                            <%} %>
                                            <% if (!(String.IsNullOrEmpty(serviceCenter.Phone)))
                                               { %>
                                            <p>
                                                <span class="bwsprite phone-black-icon vertical-top margin-right5"></span>
                                                <span class="vertical-top text-bold text-default details-column"><%= serviceCenter.Phone%></span>
                                            </p>
                                            <% } %>
                                        </div>
                                    </a>
                                    <div class="service-center-lead-content margin-top15 padding-left20 padding-right20">
                                        <% if (!(String.IsNullOrEmpty(serviceCenter.Address)))
                                           { %>
                                        <button type="button" class="btn btn-white service-btn get-details-btn font14">Get details on phone</button>
                                        <%} %>
                                        <div class="lead-mobile-content">
                                            <div class="input-box input-number-box form-control-box type-user-details">
                                                <input type="tel" id="lead-input-<%= serviceCenter.ServiceCenterId %>" class="service-center-lead-mobile" maxlength="10">
                                                <label for="lead-input-<%= serviceCenter.ServiceCenterId %>">Type in your mobile number</label>
                                                <span class="input-number-prefix">+91-</span>
                                                <span class="boundary"></span>
                                                <span class="error-text"></span>
                                            </div>
                                            <button type="button" class="btn btn-orange submit-service-center-lead-btn font14" data-id="<%= serviceCenter.ServiceCenterId %>">Send details</button>
                                            <div class="clear"></div>
                                        </div>

                                        <div class="service-center-lead-response font12 text-light-grey">
                                            <span class="service-sprite response-icon"></span>
                                            <p class="response-text inline-block"></p>
                                        </div>
                                    </div>
                                </li>
                                <% }
                                       } %>
                            </ul>
                        </div>
                        <div id="listing-right-column" class="grid-8 alpha omega">
                            <div class="dealer-map-wrapper">
                                <div id="dealerMapWrapper" style="width: 661px; height: 530px;">
                                    <div id="dealersMap" style="width: 661px; height: 530px;"></div>
                                </div>
                            </div>
                            <div class="clear"></div>
                        </div>
                        <div class="clear"></div>
                    </div>
                </div>
                <div class="clear"></div>
            </div>
            <div id="listing-footer"></div>
        </section>
        <%} %>

        <section>
            <div class="container section-bottom-margin">
                <div class="grid-12">
                    <div class="content-box-shadow">
                        <h2 class="section-h2-title padding-15-20">Tips from BikeWale experts to keep your bike in good shape!</h2>
                        <ul id="bw-tips-list">
                            <li class="grid-6">
                                <a href="/bike-care/" target="_blank" title="Bike Care - Maintenance tips">
                                    <span class="service-sprite care-icon"></span>
                                    <h3 class="bike-tips-label margin-left10 inline-block">Bike Care - Maintenance tips</h3>
                                    <span class="bwsprite right-arrow"></span>
                                </a>
                            </li>
                            <li class="grid-6">
                                <a href="/bike-troubleshooting/" target="_blank" title="Bike troubleshooting - FAQs">
                                    <span class="service-sprite faq-icon"></span>
                                    <h3 class="bike-tips-label margin-left10 inline-block">Bike troubleshooting - FAQs</h3>
                                    <span class="bwsprite right-arrow"></span>
                                </a>
                            </li>
                        </ul>
                        <div class="clear"></div>
                    </div>
                </div>
                <div class="clear"></div>
            </div>
        </section>
           <% if(ctrlPopoularBikeMake.FetchedRecordsCount > 0 || ctrlRecentUsedBikes.FetchedRecordsCount >0){ %>
        <section>
            <div class="container">
                <div class="grid-12">
                    <div class="content-box-shadow">
                        <% if (ctrlPopoularBikeMake.FetchedRecordsCount > 0)
                           { %>
                          <h2 class="section-h2-title padding-15-20">Popular <%=makeName %> bikes in <%=cityName %></h2>
                        <BW:MostPopularBikesMake runat="server" ID="ctrlPopoularBikeMake" />
                        <%} %>
                        <div class="margin-left10 margin-right10 border-solid-bottom"></div>
                        <!-- Used bikes widget -->
                        <% if (ctrlRecentUsedBikes.FetchedRecordsCount > 0)
                           { %>
                        <BW:UsedBikes runat="server" ID="ctrlRecentUsedBikes" />
                        <%} %>

                        <div class="clear"></div>
                    </div>
                </div>
                <div class="clear"></div>
            </div>
        </section>
          <% } %>
        <section>
            <div class="container section-bottom-margin">
                <div class="grid-12">
                    <div class="content-box-shadow">
                        <div class="padding-top15 padding-right20 padding-left20">
                            <h2 class="section-h2-title margin-bottom10">Looking to buy a new <%= makeName %> bike in <%= cityName %>?</h2>
                            <p class="font14">Check out authorised <%= makeName %> dealers in <%= cityName %></p>
                        </div>
                        <BW:DealerCard runat="server" ID="ctrlDealerCard" />
                    </div>
                </div>
                <div class="clear"></div>
            </div>
        </section>
   

        <section>
            <div class="container margin-bottom30">
                <div class="grid-12 font12">
                    <p>
                        <span class="font14"><strong>Disclaimer</strong>:</span>The above-mentioned information about authorised <%= makeName %> service centers is furnished to best of our knowledge. The facilities of pick and drop, timings and service schedule related information might vary slightly from service center to service center. Please check with the authorised <%= makeName %> service center before scheduling an appointment.
                    </p>
                </div>
                <div class="clear"></div>
            </div>
        </section>
        <script type="text/javascript">
            var currentCityName = '<%= cityName %>';
            var pageUrl = '<%= pageUrl %>';
            var clientip = '<%= clientIP %>';
           </script>
        <script type="text/javascript" src="<%= staticUrl != "" ? "https://st1.aeplcdn.com" + staticUrl : "" %>/src/frameworks.js?<%=staticFileVersion %>"></script>
        <!-- #include file="/includes/footerBW.aspx" -->
        <BW:BrandCity ID="ctrlBrandCity" runat="server" />
        <link href="<%= staticUrl != "" ? "https://st2.aeplcdn.com" + staticUrl : "" %>/css/bw-common-btf.css?<%=staticFileVersion %>" rel="stylesheet" type="text/css" />
        <script type="text/javascript" src="<%= staticUrl != string.Empty ? "https://st2.aeplcdn.com" + staticUrl : string.Empty %>/src/common.min.js?<%= staticFileVersion %>"></script>
        <script type="text/javascript" src="<%= staticUrl != "" ? "https://st2.aeplcdn.com" + staticUrl : "" %>/src/service/listing.js?<%= staticFileVersion %>"></script>
        <link href='https://fonts.googleapis.com/css?family=Open+Sans:400,700' rel='stylesheet' type='text/css' />
        <!--[if lt IE 9]>
            <script src="/src/html5.js"></script>
        <![endif]-->
    </form>
</body>
</html>
