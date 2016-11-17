﻿<%@ Page Language="C#" Inherits="Bikewale.Service.ServiceCenterList" AutoEventWireup="false" EnableViewState="false" %>
<%@ Register Src="~/controls/DealerCard.ascx" TagName="DealerCard" TagPrefix="BW" %>
<!DOCTYPE html>

<html>
<head>
    <%      
        keywords = String.Format("{0} servicing {1}, {0} service center in {1}, {0} Service centers, {0} service schedules, {0} bike repair, repairing, servicing", makeName, cityName);
        description = String.Format("There are {0} {1} service centers in {2}. Get in touch with your nearest {1} service center for service repairing, schedule details, pricing, pick and drop facility. Check the Service schedule for {1} bikes now.", totalServiceCenters, makeName, cityName);
        title = String.Format("{0} service centers in {1} | {0} bike servicing in {1} - BikeWale", makeName, cityName);
        canonical = String.Format("http://www.bikewale.com/{0}-dealer-showrooms-in-{1}/", makeMaskingName, cityMaskingName);
        alternate = String.Format("http://www.bikewale.com/m/{0}-dealer-showrooms-in-{1}/", makeMaskingName, cityMaskingName);
        AdId = "1395986297721";
        AdPath = "/1017752/BikeWale_New_";
        isAd970x90Shown = true;
        isAd970x90BottomShown = true;
        isAd300x250Shown = false;
        isAd300x250BTFShown = false;
        isHeaderFix = false;
    %>
    <!-- #include file="/includes/headscript_desktop_min.aspx" -->
    <link href="/css/service/listing.css" rel="stylesheet" type="text/css" />
    <script src="http://maps.googleapis.com/maps/api/js?key=<%= Bikewale.Utility.BWConfiguration.Instance.GoogleMapApiKey %>&libraries=places"></script>
    <script type="text/javascript">
        <!-- #include file="\includes\gacode_desktop.aspx" -->
        var currentCityName = '<%= cityName %>';
        var pageUrl = '<%= pageUrl %>';
        var clientip = '<%= clientIP %>';
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
                                <a itemprop="url" href="/"><span itemprop="title">Home</span></a>
                            </li>
                            <li itemscope="" itemtype="http://data-vocabulary.org/Breadcrumb">
                                <span class="bwsprite fa-angle-right margin-right10"></span><a itemprop="url" href=""><span itemprop="title">Used Bikes</span></a>
                            </li>
                            <li itemscope="" itemtype="http://data-vocabulary.org/Breadcrumb">
                                <span class="bwsprite fa-angle-right margin-right10"></span><a itemprop="url" href=""><span itemprop="title">Service Center Locator</span></a>
                            </li>
                            <li itemscope="" itemtype="http://data-vocabulary.org/Breadcrumb">
                                <span class="bwsprite fa-angle-right margin-right10"></span><a itemprop="url" href=""><span itemprop="title"><%=makeName%> Bikes Service Centers</span></a>
                            </li>
                            <li class="current"><span class="bwsprite fa-angle-right margin-right10"></span><%=makeName%> Bikes Service Center in <%=cityName %></li>
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
                        <h1 class="section-header"><%=makeName%> service centers in <%=cityName%></h1>
                        <div class="section-inner-padding font14 text-light-grey">
                            <p id="service-main-content">Is your Honda bike due for a service or are you looking to repair your Honda bike? BikeWale brings you the list of all authorised Make service centers in City. Make has 100 authorised service centers in Mumbai. We recommend availing services only from authorised service centers. Authorised Honda service centers abide by the servicing standards of Honda with an assurance of genuine Honda spare parts. </p><p id="service-more-content">BikeWale strongly recommends to use only Make genuine spare parts for your safety and durability of your bike. For more information on pick-up and drop facility, prices and service schedules get in touch with any of the below mentioned authorised make service centers in City. Do check out the maintenance tips and answers to FAQs from BikeWale experts! </p><a href="javascript:void(0)" id="read-more-target" rel="nofollow">Read more</a>
                        </div>
                    </div>
                </div>
                <div class="clear"></div>
            </div>
        </section>

        <section>
            <div class="container section-bottom-margin">
                <div class="grid-12">
                    <div class="content-box-shadow">
                        <h2 class="section-h2-title padding-15-20"><%=totalServiceCenters %> <%=makeName %> service centers in <%=cityName %></h2>
                        <div id="listing-left-column" class="grid-4 alpha omega">
                            <ul id="center-list">
                                    <% foreach (var serviceCenter in serviceCentersList)
                                     { %>
                                
                                        <li data-item-id="<%= serviceCenter.ServiceCenterId %>" data-item-inquired="false" data-lat="<%= serviceCenter.Lattitude %>" data-log="<%= serviceCenter.Longitude %>" data-address="<%= serviceCenter.Address %>" 
                                             data-item-url="<%= Bikewale.Utility.UrlFormatter.GetServiceCenterUrl(makeMaskingName, urlCityMaskingName, serviceCenter.Name, Convert.ToInt32(serviceCenter.ServiceCenterId)) %> " >

                                            <a href="<%= Bikewale.Utility.UrlFormatter.GetServiceCenterUrl(makeMaskingName, urlCityMaskingName, serviceCenter.Name, Convert.ToInt32(serviceCenter.ServiceCenterId)) %>" title="" class="dealer-card-target font14">
                                                <div class="dealer-card-content">
                                                    <h3 class="dealer-name text-black text-bold margin-bottom5"><%= serviceCenter.Name %></h3>
                                                    <% if(!(String.IsNullOrEmpty(serviceCenter.Address))) { %>
                                                    <p class="text-light-grey margin-bottom5">
                                                        <span class="bwsprite dealership-loc-icon vertical-top margin-right5"></span>
                                                        <span class="vertical-top details-column"><%= serviceCenter.Address %></span>
                                                    </p>
                                                    <%} %>
                                                    <% if(!(String.IsNullOrEmpty(serviceCenter.Phone))) { %>
                                                    <p class="">
                                                        <span class="bwsprite phone-black-icon vertical-top margin-right5"></span>
                                                        <span class="vertical-top text-bold text-default details-column"><%= serviceCenter.Phone%></span>
                                                    </p>
                                                    <% } %>
                                                    <button type="button" class="btn btn-white btn-full-width service-btn font14 margin-top15">Get service center details</button>
                                                </div>
                                            </a>

                                        </li>
                                    <% } %>
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

        <section>
            <div class="container section-bottom-margin">
                <div class="grid-12">
                    <div class="content-box-shadow">
                        <h2 class="section-h2-title padding-15-20">Tips from BikeWale experts to keep your bike in good shape!</h2>
                        <ul id="bw-tips-list">
                            <li class="grid-6">
                                <a href="">
                                    <span class="service-sprite care-icon"></span>
                                    <h3 class="bike-tips-label margin-left10 inline-block">Bike Care - Maintenance tips</h3>
                                    <span class="bwsprite right-arrow"></span>
                                </a>
                            </li>
                            <li class="grid-6">
                                <a href="">
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

        <% if(ctrlDealerCard.showWidget) { %>
        <section>
            <div class="container section-bottom-margin">
                <div class="grid-12">
                    <div class="content-box-shadow padding-bottom20">
                        <div class="padding-15-20 margin-bottom5">
                            <h2 class="section-h2-title margin-bottom10">Looking to buy a new <%= makeName %> bike in <%= cityName %>?</h2>
                            <p class="font14">Check out authorised <%= makeName %> dealers in <%= cityName %></p>
                        </div>      
                             <BW:DealerCard runat="server" ID="ctrlDealerCard" />                                            
                    </div>
                </div>
                <div class="clear"></div>
            </div>
        </section>
        <% } %>

        <section>
            <div class="container margin-bottom30">
                <div class="grid-12 font12">
                    <p><span class="font14"><strong>Disclaimer</strong>:</span>The above-mentioned information about authorised <%= makeName %> service centers is furnished to best of our knowledge. The facilities of pick and drop, timings and service schedule related information might vary slightly from service center to service center. Please check with the authorised <%= makeName %> service center before scheduling an appointment.
</p>
                </div>
                <div class="clear"></div>
            </div>
        </section>

        <script type="text/javascript" src="<%= staticUrl != "" ? "http://st1.aeplcdn.com" + staticUrl : "" %>/src/frameworks.js?<%=staticFileVersion %>"></script>
        <!-- #include file="/includes/footerBW.aspx" -->
        <link href="<%= staticUrl != "" ? "http://st2.aeplcdn.com" + staticUrl : "" %>/css/bw-common-btf.css?<%=staticFileVersion %>" rel="stylesheet" type="text/css" />
        <script type="text/javascript" src="<%= staticUrl != string.Empty ? "http://st2.aeplcdn.com" + staticUrl : string.Empty %>/src/common.min.js?<%= staticFileVersion %>"></script>
        <script type="text/javascript" src="<%= staticUrl != "" ? "http://st2.aeplcdn.com" + staticUrl : "" %>/src/service/listing.js?<%= staticFileVersion %>"></script>
        <link href='https://fonts.googleapis.com/css?family=Open+Sans:400,700' rel='stylesheet' type='text/css' />
        <!--[if lt IE 9]>
            <script src="/src/html5.js"></script>
        <![endif]-->
    </form>
</body>
</html>
