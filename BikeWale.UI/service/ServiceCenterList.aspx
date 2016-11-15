<%@ Page Language="C#" Inherits="Bikewale.Service.ServiceCenterList" AutoEventWireup="false" EnableViewState="false" %>
<%@ Register Src="~/controls/UsedBikeWidget.ascx" TagName="UsedBikes" TagPrefix="BW" %>
<%@ Register Src="~/controls/MostPopularBikes_new.ascx" TagName="MostPopularBikesMake" TagPrefix="BW" %>
<%@ Register Src="~/controls/LeadCaptureControl.ascx" TagName="LeadCapture" TagPrefix="BW" %>
<!DOCTYPE html>

<html>
<head>
    <%      
        keywords = String.Format("{0} showroom {1}, {0} dealers {1}, {1} bike showroom, {1} bike dealers,{1} dealers, {1} bike showroom, bike dealers, bike showroom, dealerships", makeName, cityName);
        description = String.Format("There are {2} {0} dealer showrooms in {1}. Get in touch with {0} showroom for prices, availability, test rides, EMI options and more!", makeName, cityName, totalDealers);
        title = String.Format("{0} Showrooms in {1} | {2} {0} Bike Dealers  - BikeWale", makeName, cityName, totalDealers);
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
    <link href="/min/css/service/listing.css" rel="stylesheet" type="text/css" />
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
                        <h2 class="section-h2-title padding-15-20"><%=totalDealers %> <%=makeName %> service centers in <%=cityName %></h2>
                        <div id="listing-left-column" class="grid-4 alpha omega">
                            <ul id="center-list">
                                <asp:Repeater ID="rptDealers" runat="server">
                                    <ItemTemplate>
                                        <li data-item-type="<%# (DataBinder.Eval(Container.DataItem,"DealerType")) %>" data-item-id="<%# DataBinder.Eval(Container.DataItem,"DealerId") %>" data-item-inquired="false" data-item-number="<%# DataBinder.Eval(Container.DataItem,"MaskingNumber") %>" data-lat="<%# DataBinder.Eval(Container.DataItem,"objArea.Latitude") %>" data-log="<%# DataBinder.Eval(Container.DataItem,"objArea.Longitude") %>" data-address="<%# DataBinder.Eval(Container.DataItem,"Address") %>" 
                                            data-campid="<%# DataBinder.Eval(Container.DataItem,"CampaignId") %>" data-item-url="<%# Bikewale.Utility.UrlFormatter.RemoveSpecialCharUrl(DataBinder.Eval(Container.DataItem,"Name").ToString()) %>" >

                                            <a href="/<%=makeMaskingName %>-dealer-showrooms-in-<%=cityMaskingName %>/<%# DataBinder.Eval(Container.DataItem,"DealerId") %>-<%# Bikewale.Utility.UrlFormatter.RemoveSpecialCharUrl(DataBinder.Eval(Container.DataItem,"Name").ToString()) %>/" title="<%# DataBinder.Eval(Container.DataItem,"Name") %>" class="dealer-card-target font14">
                                                <div class="dealer-card-content">
                                                    <h3 class="dealer-name text-black text-bold margin-bottom5"><%# DataBinder.Eval(Container.DataItem,"Name") %></h3>
                                                    <p class="<%# (String.IsNullOrEmpty(DataBinder.Eval(Container.DataItem,"Address").ToString()))?"hide":"text-light-grey margin-bottom5" %>">
                                                        <span class="bwsprite dealership-loc-icon vertical-top margin-right5"></span>
                                                        <span class="vertical-top details-column"><%# DataBinder.Eval(Container.DataItem,"Address") %></span>
                                                    </p>
                                                    <p class="<%# (String.IsNullOrEmpty(DataBinder.Eval(Container.DataItem,"MaskingNumber").ToString()))?"hide":"" %>">
                                                        <span class="bwsprite phone-black-icon vertical-top margin-right5"></span>
                                                        <span class="vertical-top text-bold text-default details-column"><%# DataBinder.Eval(Container.DataItem,"MaskingNumber") %></span>
                                                    </p>
                                                </div>
                                            </a>
                                            <div class="padding-left20 service-center-lead-content margin-top15">
                                                <button type="button" class="btn btn-white service-btn get-details-btn font14">Get details on phone</button>
                                                <div class="lead-mobile-content">
                                                    <div class="input-box input-number-box form-control-box type-user-details">
                                                        <input type="tel" id="lead-input-<%# DataBinder.Eval(Container.DataItem,"DealerId") %>" class="service-center-lead-mobile" maxlength="10">
                                                        <label for="lead-input-<%# DataBinder.Eval(Container.DataItem,"DealerId") %>">Type in your mobile number</label>
                                                        <span class="input-number-prefix">+91</span>
                                                        <span class="boundary"></span>
                                                        <span class="error-text"></span>
                                                    </div>
                                                    <button type="button" class="btn btn-orange submit-service-center-lead-btn font14">Send</button>
                                                    <div class="clear"></div>
                                                </div>
                                            </div>
                                            <div class="service-center-lead-response">Service Center details successfully sent on your phone.<br />Not Received? <span style="color: #0288d1">Resend</span></div>
                                        </li>
                                    </ItemTemplate>
                                </asp:Repeater>
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

        <section>
            <div class="container section-bottom-margin">
                <div class="grid-12">
                    <div class="content-box-shadow padding-bottom20">
                        <div class="padding-15-20 margin-bottom5">
                            <h2 class="section-h2-title margin-bottom10">Looking to buy a new Bajaj bike in Mumbai?</h2>
                            <p class="font14">Check out authorised Bajaj dealers in Mumbai</p>
                        </div>
                        <ul class="bw-horizontal-cards">
                            <li class="card">
                                <a href="" title="Executive Bajaj - Malad" class="card-target">
                                    <h3 class="text-black text-bold text-truncate margin-bottom5">Executive Bajaj - Malad</h3>
                                    <p class="text-light-grey margin-bottom5">
                                        <span class="bwsprite dealership-loc-icon vertical-top margin-right5"></span>
                                        <span class="vertical-top details-column">11, Link House, Plot No.446, Near Lakozy Toyota, Link Road, Malad (W)</span>
                                    </p>
                                    <p class="text-default">
                                        <span class="bwsprite phone-black-icon vertical-top margin-right5"></span>
                                        <span class="text-bold vertical-top details-column">7620914785</span>
                                    </p>
                                </a>
                            </li>
                            <li class="card">
                                <a href="" title="Kamala Landmarc Motorbikes - Malad" class="card-target">
                                    <h3 class="text-black text-bold text-truncate margin-bottom5">Kamala Landmarc Motorbikes - Malad</h3>
                                    <p class="text-light-grey margin-bottom5">
                                        <span class="bwsprite dealership-loc-icon vertical-top margin-right5"></span>
                                        <span class="vertical-top details-column">11, Link House, Plot No.446, Near Lakozy Toyota, Link Road, Malad (W)</span>
                                    </p>
                                    <p class="text-default">
                                        <span class="bwsprite phone-black-icon vertical-top margin-right5"></span>
                                        <span class="text-bold vertical-top details-column">7620914785</span>
                                    </p>
                                </a>
                            </li>
                            <li class="card">
                                <a href="" title="Executive Bajaj - Malad" class="card-target">
                                    <h3 class="text-black text-bold text-truncate margin-bottom5">Executive Bajaj - Malad</h3>
                                    <p class="text-light-grey margin-bottom5">
                                        <span class="bwsprite dealership-loc-icon vertical-top margin-right5"></span>
                                        <span class="vertical-top details-column">11, Link House, Plot No.446, Near Lakozy Toyota, Link Road, Malad (W)</span>
                                    </p>
                                    <p class="text-default">
                                        <span class="bwsprite phone-black-icon vertical-top margin-right5"></span>
                                        <span class="text-bold vertical-top details-column">7620914785</span>
                                    </p>
                                </a>
                            </li>
                        </ul>
                        <div class="clear"></div>
                        <div class="padding-left20 font14">
                            <a href="">View all Bajaj dealers<span class="bwsprite blue-right-arrow-icon"></span></a>
                        </div>
                    </div>
                </div>
                <div class="clear"></div>
            </div>
        </section>

        <section>
            <div class="container margin-bottom30">
                <div class="grid-12 font12">
                    <p><span class="font14"><strong>Disclaimer</strong>:</span> The above-mentioned information about authorised Honda service centers is furnished to best of our knowledge. The facilities of pick and drop, timings and service schedule related information might vary slightly from service center to service center. Please check with the authorised Make service center before scheduling an appointment.</p>
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
                          <h2 class="font18 padding-18-20">Popular <%=makeName %> bikes in <%=cityName %></h2>
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
        

        <script type="text/javascript" src="<%= staticUrl != "" ? "http://st1.aeplcdn.com" + staticUrl : "" %>/src/frameworks.js?<%=staticFileVersion %>"></script>
        <!-- #include file="/includes/footerBW.aspx" -->
        <BW:LeadCapture ID="ctrlLeadCapture" runat="server" />
        <link href="<%= staticUrl != "" ? "http://st2.aeplcdn.com" + staticUrl : "" %>/css/bw-common-btf.css?<%=staticFileVersion %>" rel="stylesheet" type="text/css" />
        <script type="text/javascript" src="<%= staticUrl != string.Empty ? "http://st2.aeplcdn.com" + staticUrl : string.Empty %>/src/common.min.js?<%= staticFileVersion %>"></script>
        <script type="text/javascript" src="<%= staticUrl != "" ? "http://st2.aeplcdn.com" + staticUrl : "" %>/src/service/listing.js?<%= staticFileVersion %>"></script>
        <link href='https://fonts.googleapis.com/css?family=Open+Sans:400,700' rel='stylesheet' type='text/css' />
        <!--[if lt IE 9]>
            <script src="/src/html5.js"></script>
        <![endif]-->
        <script type="text/javascript">
            $(".leadcapturebtn").click(function (e) {
                ele = $(this);
                var leadOptions = {
                    "dealerid": ele.attr('data-item-id'),
                    "dealername": ele.attr('data-item-name'),
                    "dealerarea": ele.attr('data-item-area'),
                    "campid": ele.attr('data-campid'),
                    "leadsourceid": ele.attr('data-leadsourceid'),
                    "pqsourceid": ele.attr('data-pqsourceid'),
                    "isdealerbikes": true,
                    "pageurl": window.location.href,
                    "isregisterpq": true,
                    "clientip": clientip
                };
                dleadvm.setOptions(leadOptions);
            });
        </script>
    </form>
</body>
</html>
