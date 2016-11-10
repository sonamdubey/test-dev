<%@ Page Language="C#" AutoEventWireup="false" Inherits="Bikewale.Mobile.Service.ServiceCenterDetails" EnableViewState="false" %>
<%@ Register Src="~/m/controls/DealersCard.ascx" TagName="DealerCard" TagPrefix="BW" %>
<%@ Register Src="~/m/controls/LeadCaptureControl.ascx" TagName="LeadCapture" TagPrefix="BW" %>
<!DOCTYPE html>
<html>
<head>
    <%
        keywords = String.Format("{0}, {0} dealer, {0} Showroom, {0} {1}", dealerName, dealerCity);
        description = String.Format("{2} is dealer of {0} bikes in {1}. Get best offers on {0} bikes at {2} showroom", makeName, dealerCity, dealerName);
        title = String.Format("{0} {1} - {0} Showroom in {1} - BikeWale", dealerName, dealerCity);
        canonical =  String.Format("http://www.bikewale.com{0}",Bikewale.Utility.UrlFormatter.GetDealerUrl(makeMaskingName, cityMaskingName, dealerName, (int)dealerId));
    %>
    <!-- #include file="/includes/headscript_mobile_min.aspx" -->
    <link rel="stylesheet" type="text/css" href="/m/css/service/details.css">
    
    <script src="http://maps.googleapis.com/maps/api/js?key=<%= Bikewale.Utility.BWConfiguration.Instance.GoogleMapApiKey %>&libraries=places"></script>
    <script type="text/javascript">
        <!-- #include file="\includes\gacode_mobile.aspx" -->
    </script>
</head>
<body class="bg-light-grey">
    <form runat="server">
        <!-- #include file="/includes/headBW_Mobile.aspx" -->
        
        <section>
            <div class="container bg-white card-bottom-margin">
                <h1 class="card-header"><%= dealerDetails.Name %></h1>
                <div class="card-inner-padding font14 text-light-grey">
                    <h2 class="font14 margin-bottom15"><%= string.Format("Authorized {0} service center", makeName) %></h2>

                    <%if (!string.IsNullOrEmpty(dealerDetails.Address))
                     { %>
                    <p class="margin-bottom10">
                        <span class="bwmsprite dealership-loc-icon vertical-top"></span>
                        <span class="vertical-top details-column text-light-grey"><%= dealerDetails.Address %></span>
                    </p>
                    <% } %>

                    <% if (!string.IsNullOrEmpty(dealerDetails.MaskingNumber))
                    { %>
                    <div class="margin-bottom10">
                        <a href="tel:<%= dealerDetails.MaskingNumber %>" class="text-default text-bold maskingNumber">
                            <span class="bwmsprite tel-sm-grey-icon vertical-top"></span>
                            <span class="vertical-top text-bold details-column"><%= dealerDetails.MaskingNumber %></span>
                        </a>
                    </div>
                    <% } %>

                    <% if (!string.IsNullOrEmpty(dealerDetails.EMail))
                    { %>
                    <div class="margin-bottom10">
                        <a href="mailto:<%= dealerDetails.EMail %>" class="text-light-grey">
                            <span class="bwmsprite mail-grey-icon vertical-top"></span>
                            <span class="vertical-top details-column text-light-grey"><%= dealerDetails.EMail %></span>
                        </a>
                    </div>
                    <% } %>

                    <% if (!string.IsNullOrEmpty(dealerDetails.WorkingHours))
                    { %>
                    <div class="margin-bottom10">
                        <span class="bwmsprite clock-icon vertical-top"></span>
                        <span class="vertical-top details-column text-light-grey">Working hours: <%= dealerDetails.WorkingHours %></span>
                    </div>
                    <%} %>

                    <% if(dealerLat> 0 && dealerLong>0) { %>
                    <div class="border-solid-bottom margin-bottom15 padding-top10"></div>
                    
                    <h2 class="font14 text-default margin-bottom15">Get commute distance and time:</h2>
                    <div class="form-control-box margin-bottom15">
                        <input id="locationSearch" type="text" class="form-control padding-right40" placeholder="Type in your location" />
                        <span id="getUserLocation" class="crosshair-icon position-abt pos-right10 pos-top10"></span>
                    </div>
                    <div class="location-details margin-bottom15">
                        Distance: <span id="commuteDistance" class="margin-right10"></span>
                        Time: <span id="commuteDuration"></span>
                    </div>
                    <div id="commuteResults"></div>
                    <a id="anchorGetDir" href="http://maps.google.com/maps?z=12&t=m&q=loc:<%= dealerLat %>,<%= dealerLong %>" target="_blank"><span class="bwmsprite get-direction-icon margin-right5"></span>Get directions</a>
                    <% } %>
                </div>
            </div>
        </section>

        <section>
            <div class="container bg-white box-shadow card-bottom-margin padding-bottom20">
                <h2 class="padding-15-20">Other Bajaj servcie centers in Mumbai</h2>
                <div class="bw-horizontal-swiper swiper-container card-container margin-bottom15">
                    <div class="swiper-wrapper">
                        <div class="swiper-slide">
                            <div class="swiper-card">
                                <a href="" title="">
                                    <div class="target-link margin-bottom5 text-truncate font14">Executive Bajaj, Ghatkopar</div>        
                                    <p class="margin-bottom5 text-light-grey">
                                        <span class="bwmsprite dealership-loc-icon vertical-top"></span>
                                        <span class="vertical-top details-column">Paragon Plaza, Phoenix MarketCity, Unit Nos 24/25, Next to Maruti Showroom, LBS Road, Ghatkopar (W)</span>
                                    </p>
                                    <p class="text-truncate">
                                        <span class="bwmsprite tel-sm-grey-icon pos-top0 margin-right5"></span>
                                        <span class="text-bold text-default">02132-5544763</span>
                                    </p>
                                </a>
                            </div>
                        </div>

                        <div class="swiper-slide">
                            <div class="swiper-card">
                                <a href="" title="">
                                    <p class="target-link margin-bottom5 text-truncate font14">Executive Bajaj, Ghatkopar</p>        
                                    <p class="margin-bottom5 text-light-grey">
                                        <span class="bwmsprite dealership-loc-icon vertical-top"></span>
                                        <span class="vertical-top details-column">Paragon Plaza, Phoenix MarketCity, Unit Nos 24/25, Next to Maruti Showroom, LBS Road, Ghatkopar (W)</span>
                                    </p>
                                    <p class="text-truncate">
                                        <span class="bwmsprite tel-sm-grey-icon pos-top0 margin-right5"></span>
                                        <span class="text-bold text-default">02132-5544763</span>
                                    </p>
                                </a>
                            </div>
                        </div>

                        <div class="swiper-slide">
                            <div class="swiper-card">
                                <a href="" title="">
                                    <p class="target-link margin-bottom5 text-truncate font14">Executive Bajaj, Ghatkopar</p>        
                                    <p class="margin-bottom5 text-light-grey">
                                        <span class="bwmsprite dealership-loc-icon vertical-top"></span>
                                        <span class="vertical-top details-column">Paragon Plaza, Phoenix MarketCity, Unit Nos 24/25, Next to Maruti Showroom, LBS Road, Ghatkopar (W)</span>
                                    </p>
                                    <p class="text-truncate">
                                        <span class="bwmsprite tel-sm-grey-icon pos-top0 margin-right5"></span>
                                        <span class="text-bold text-default">02132-5544763</span>
                                    </p>
                                </a>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="padding-right20 padding-left20 font14">
                    <a href="" title="">View all Bajaj service centers <span class="bwmsprite blue-right-arrow-icon"></span></a>
                </div>
            </div>
        </section>

        <section>
            <div id="service-schedular" class="container bg-white box-shadow card-bottom-margin padding-15-20">
                <h2 class="margin-bottom5">Is your bajaj bike due for a service?</h2>
                <p class="margin-bottom15">Get your Bajaj bike serviced with time period given below.</p>
                <div class="select-box margin-bottom20">
                    <p class="font12 text-light-grey">Model</p>
                    <select class="chosen-select">
                        <option value="1">Pulsar RS200</option>
                        <option value="2">CB Shine</option>
                        <option value="3">CB ShineSP</option>
                        <option value="4">CB Unicorn 150</option>
                        <option value="5">Pulsar RS200</option>
                        <option value="6">CB Shine</option>
                        <option value="7">CB ShineSP</option>
                        <option value="8">CB Unicorn 150</option>
                    </select>
                </div>
                <table width="100%" border="0" cellspacing="0" cellpadding="0">
                    <thead>
                        <tr>
                            <th align="left" width="20%" class="font12 text-unbold text-x-light padding-bottom5 border-solid-bottom">Service<br />no.</th>
                            <th align="left" width="40%" class="font12 text-unbold text-x-light padding-bottom5 border-solid-bottom">Validity from the<br />date of purchase</th>
                            <th align="left" width="40%" class="font12 text-unbold text-x-light padding-bottom5 border-solid-bottom">Validity from the date of previous service</th>
                        </tr>
                    </thead>
                    <tbody>
                        <tr>
							<td width="20%" class="padding-bottom10 padding-top10 padding-right10 font14 divider-top" valign="top">1</td>
                            <td width="40%" class="padding-bottom10 padding-top10 padding-right10 divider-top" valign="top">0-600 kms</td>
							<td width="40%" class="padding-bottom10 padding-top10 divider-top" valign="top">60 days</td>
						</tr>
                        <tr>
							<td width="20%" class="padding-bottom10 padding-top10 padding-right10 font14 divider-top" valign="top">2</td>
                            <td width="40%" class="padding-bottom10 padding-top10 padding-right10 divider-top" valign="top">500-1000 kms</td>
							<td width="40%" class="padding-bottom10 padding-top10 divider-top" valign="top">100 days</td>
						</tr>
                        <tr>
							<td width="20%" class="padding-bottom10 padding-top10 padding-right10 font14 divider-top" valign="top">3</td>
                            <td width="40%" class="padding-bottom10 padding-top10 padding-right10 divider-top" valign="top">1000-5000 kms</td>
							<td width="40%" class="padding-bottom10 padding-top10 divider-top" valign="top">250 days</td>
						</tr>
                        <tr>
							<td width="20%" class="padding-bottom10 padding-top10 padding-right10 font14 divider-top" valign="top">4</td>
                            <td width="40%" class="padding-bottom10 padding-top10 padding-right10 divider-top" valign="top">5000-10000 kms</td>
							<td width="40%" class="padding-bottom10 padding-top10 divider-top" valign="top">500 days</td>
						</tr>
                    </tbody>
                </table>
                <!-- no days -->
                <table width="100%" border="0" cellspacing="0" cellpadding="0" class="hide">
                    <thead>
                        <tr>
                            <th align="left" width="30%" class="font12 text-unbold text-x-light padding-bottom5 border-solid-bottom">Service<br />no.</th>
                            <th align="left" width="70%" class="font12 text-unbold text-x-light padding-bottom5 border-solid-bottom">Validity from the date<br />of purchase</th>
                        </tr>
                    </thead>
                    <tbody>
                        <tr>
							<td width="30%" class="padding-bottom10 padding-top10 padding-right10 font14 divider-top" valign="top">1</td>
							<td width="70%" class="padding-bottom10 padding-top10 divider-top" valign="top">0-600 kms</td>
						</tr>
                        <tr>
							<td width="30%" class="padding-bottom10 padding-top10 padding-right10 font14 divider-top" valign="top">2</td>
							<td width="70%" class="padding-bottom10 padding-top10 divider-top" valign="top">500-1000 kms</td>
						</tr>
                        <tr>
							<td width="30%" class="padding-bottom10 padding-top10 padding-right10 font14 divider-top" valign="top">3</td>
							<td width="70%" class="padding-bottom10 padding-top10 divider-top" valign="top">1000-5000 kms</td>
						</tr>
                        <tr>
							<td width="30%" class="padding-bottom10 padding-top10 padding-right10 font14 divider-top" valign="top">4</td>
							<td width="70%" class="padding-bottom10 padding-top10 divider-top" valign="top">5000-10000 kms</td>
						</tr>
                    </tbody>
                </table>
                <!-- no days -->
                <!-- no kms -->
                <table width="100%" border="0" cellspacing="0" cellpadding="0" class="hide">
                    <thead>
                        <tr>
                            <th align="left" width="30%" class="font12 text-unbold text-x-light padding-bottom5 border-solid-bottom">Service<br />no.</th>
                            <th align="left" width="70%" class="font12 text-unbold text-x-light padding-bottom5 border-solid-bottom">Validity from the date of previous service</th>
                        </tr>
                    </thead>
                    <tbody>
                        <tr>
							<td width="30%" class="padding-bottom10 padding-top10 padding-right10 font14 divider-top" valign="top">1</td>
							<td width="70%" class="padding-bottom10 padding-top10 divider-top" valign="top">60 days</td>
						</tr>
                        <tr>
							<td width="30%" class="padding-bottom10 padding-top10 padding-right10 font14 divider-top" valign="top">2</td>
							<td width="70%" class="padding-bottom10 padding-top10 divider-top" valign="top">100 days</td>
						</tr>
                        <tr>
							<td width="30%" class="padding-bottom10 padding-top10 padding-right10 font14 divider-top" valign="top">3</td>
							<td width="70%" class="padding-bottom10 padding-top10 divider-top" valign="top">250 days</td>
						</tr>
                        <tr>
							<td width="30%" class="padding-bottom10 padding-top10 padding-right10 font14 divider-top" valign="top">4</td>
							<td width="70%" class="padding-bottom10 padding-top10 divider-top" valign="top">500 days</td>
						</tr>
                    </tbody>
                </table>
                <!-- no kms -->
                <!-- no service -->
                <div id="service-not-avaiable" class="hide">
                    <span class="service-sprite calender-lg"></span>
                    <p class="font14 text-light-grey">Sorry! The service schedule for<br />Bajaj Pulsar is not available.<br />Please check out the service schedule for other Bajaj bikes.</p>
                </div>
                <!-- no service -->
            </div>
        </section>

        <section>
            <div class="container bg-white box-shadow card-bottom-margin">
                <h2 class="padding-top15 padding-right20 padding-left20">Tips from BikeWale experts to keep your bike in good shape!</h2>
                <ul id="bw-tips-list">
                    <li>
                        <a href="">
                            <span class="service-sprite care-icon"></span>
                            <h3 class="text-unbold margin-left10 vertical-middle">Bike Care - Maintenance tips</h3>
                            <span class="bwmsprite right-arrow"></span>
                        </a>
                    </li>
                    <li>
                        <a href="">
                            <span class="service-sprite faq-icon"></span>
                            <h3 class="text-unbold margin-left10 vertical-middle">Bike troubleshooting - FAQs</h3>
                            <span class="bwmsprite right-arrow"></span>
                        </a>
                    </li>
                </ul>
            </div>
        </section>

        <section>
            <div class="container bg-white box-shadow card-bottom-margin padding-bottom20 padding-top15">
                <div class="padding-right20 padding-left20 margin-bottom15">
                    <h2 class="margin-bottom5">Looking to buy a new Bajaj bike in Mumbai?</h2>
                    <p>Check out authorised Bajaj dealers in Mumbai</p>
                </div>
                <div class="bw-horizontal-swiper swiper-container card-container margin-bottom15">
                    <div class="swiper-wrapper">
                        <div class="swiper-slide">
                            <div class="swiper-card">
                                <a href="" title="">
                                    <div class="target-link margin-bottom5 text-truncate font14">Executive Bajaj, Ghatkopar</div>        
                                    <p class="margin-bottom5 text-light-grey">
                                        <span class="bwmsprite dealership-loc-icon vertical-top"></span>
                                        <span class="vertical-top details-column">Paragon Plaza, Phoenix MarketCity, Unit Nos 24/25, Next to Maruti Showroom, LBS Road, Ghatkopar (W)</span>
                                    </p>
                                    <p class="text-truncate">
                                        <span class="bwmsprite tel-sm-grey-icon pos-top0 margin-right5"></span>
                                        <span class="text-bold text-default">02132-5544763</span>
                                    </p>
                                </a>
                            </div>
                        </div>

                        <div class="swiper-slide">
                            <div class="swiper-card">
                                <a href="" title="">
                                    <p class="target-link margin-bottom5 text-truncate font14">Executive Bajaj, Ghatkopar</p>        
                                    <p class="margin-bottom5 text-light-grey">
                                        <span class="bwmsprite dealership-loc-icon vertical-top"></span>
                                        <span class="vertical-top details-column">Paragon Plaza, Phoenix MarketCity, Unit Nos 24/25, Next to Maruti Showroom, LBS Road, Ghatkopar (W)</span>
                                    </p>
                                    <p class="text-truncate">
                                        <span class="bwmsprite tel-sm-grey-icon pos-top0 margin-right5"></span>
                                        <span class="text-bold text-default">02132-5544763</span>
                                    </p>
                                </a>
                            </div>
                        </div>

                        <div class="swiper-slide">
                            <div class="swiper-card">
                                <a href="" title="">
                                    <p class="target-link margin-bottom5 text-truncate font14">Executive Bajaj, Ghatkopar</p>        
                                    <p class="margin-bottom5 text-light-grey">
                                        <span class="bwmsprite dealership-loc-icon vertical-top"></span>
                                        <span class="vertical-top details-column">Paragon Plaza, Phoenix MarketCity, Unit Nos 24/25, Next to Maruti Showroom, LBS Road, Ghatkopar (W)</span>
                                    </p>
                                    <p class="text-truncate">
                                        <span class="bwmsprite tel-sm-grey-icon pos-top0 margin-right5"></span>
                                        <span class="text-bold text-default">02132-5544763</span>
                                    </p>
                                </a>
                            </div>
                        </div>
                    </div>
                </div>

                <div class="padding-right20 padding-left20 font14">
                    <a href="" title="">View all Bajaj dealers <span class="bwmsprite blue-right-arrow-icon"></span></a>
                </div>
            </div>
        </section>

        <section>
            <div class="container margin-bottom20 font12 padding-top5 padding-right20 padding-left20">
                <span class="font14"><strong>Disclaimer:</strong></span> The above mentioned information about <%=makeName %> dealership showrooms in <%=cityName %> is furnished to the best of our knowledge. 
                    All <%=makeName %> bike models and colour options may not be available at each of the <%=makeName %> dealers. 
                    We recommend that you call and check with your nearest <%=makeName %> dealer before scheduling a showroom visit.
            </div>
        </section>

        <section class="container bg-white margin-bottom10">
            <div class="bg-white box-shadow">
                
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
                    
                </div>
                
            </div>
        </section>

        <%if (dealerBikesCount > 0 && campaignId > 0 && (dealerDetails.DealerType == (int)(Bikewale.Entities.PriceQuote.DealerPackageTypes.Premium) || dealerDetails.DealerType == (int)(Bikewale.Entities.PriceQuote.DealerPackageTypes.Deluxe)))
              
          { %>
        <section class="container bg-white margin-bottom10">
            <div class="box-shadow padding-top15 padding-right20 padding-bottom5 padding-left20">
                <h2 class="font18 margin-bottom15">Models available at <%= dealerName %></h2>
                <ul id="model-available-list">
                    <asp:Repeater ID="rptModels" runat="server">
                        <ItemTemplate>
                            <li>
                                <a class="modelurl" href='/m<%# Bikewale.Utility.UrlFormatter.BikePageUrl(Convert.ToString(DataBinder.Eval(Container.DataItem,"objMake.MaskingName")),Convert.ToString(DataBinder.Eval(Container.DataItem,"objModel.MaskingName"))) %>'>
                                    <div class="image-block">
                                        <div class="image-content">
                                            <img class="lazy"
                                                data-original="<%# Bikewale.Utility.Image.GetPathToShowImages(DataBinder.Eval(Container.DataItem, "OriginalImagePath").ToString(),DataBinder.Eval(Container.DataItem, "HostUrl").ToString(),Bikewale.Utility.ImageSize._310x174) %>"
                                                alt="<%# DataBinder.Eval(Container.DataItem, "BikeName") %>" src="http://imgd3.aeplcdn.com/0x0/bw/static/sprites/m/circleloader.gif">
                                        </div>
                                    </div>

                                    <div class="details-block">
                                        <h3 class="font16 margin-bottom10 text-black text-truncate"><%# DataBinder.Eval(Container.DataItem, "BikeName") %></h3>
                                        <div class="font14 text-x-light margin-bottom10">
                                            <%# Bikewale.Utility.FormatMinSpecs.GetMinSpecs(Convert.ToString(DataBinder.Eval(Container.DataItem, "Specs.Displacement")),Convert.ToString(DataBinder.Eval(Container.DataItem, "Specs.FuelEfficiencyOverall")),Convert.ToString(DataBinder.Eval(Container.DataItem, "Specs.MaxPower"))) %>
                                        </div>
                                        <div class="text-default">
                                            <span class="bwmsprite inr-sm-icon"></span>
                                            <span class="font18 text-bold"><%# Bikewale.Utility.Format.FormatPrice(Convert.ToString(DataBinder.Eval(Container.DataItem, "VersionPrice"))) %></span>
                                            <span class="font14 text-light-grey"> onwards</span>
                                        </div>                                    
                                    </div>                                
                                </a>
                            </li>
                        </ItemTemplate>
                    </asp:Repeater>
                </ul>
            </div>
        </section>
        <%} %>
        <script type="text/javascript" src="<%= staticUrl != "" ? "http://st1.aeplcdn.com" + staticUrl : "" %>/m/src/frameworks.js?<%= staticFileVersion %>"></script>
        <section class="container bg-white margin-bottom10">
            <div class="box-shadow">
                <!-- dealer card -->
                <% if (ctrlDealerCard.showWidget) { %>
                    <BW:DealerCard runat="server" ID="ctrlDealerCard" />
                <% }  %>
            </div>
        </section>   

         <BW:LeadCapture ID="ctrlLeadCapture" runat="server" />

        <!-- #include file="/includes/footerBW_Mobile.aspx" -->
        <link href="<%= staticUrl != "" ? "http://st2.aeplcdn.com" + staticUrl : "" %>/m/css/bwm-common-btf.css?<%= staticFileVersion %>" rel="stylesheet" type="text/css" />
        <link href="<%= staticUrl != "" ? "http://st1.aeplcdn.com" + staticUrl : "" %>/css/chosen.min.css?<%= staticFileVersion %>" type="text/css" rel="stylesheet" />
        <script type="text/javascript" src="<%= staticUrl != "" ? "http://st2.aeplcdn.com" + staticUrl : "" %>/m/src/common.min.js?<%= staticFileVersion %>"></script>
        <script type="text/javascript" src="<%= staticUrl != "" ? "http://st1.aeplcdn.com" + staticUrl : "" %>/m/src/chosen-jquery-min-mobile.js?<%= staticFileVersion %>"></script>
        <script type="text/javascript">
            var versionId, dealerId = "<%= dealerId %>", cityId = "<%= cityId %>", clientIP = "<%= Bikewale.Common.CommonOpn.GetClientIP()%>",campaignId = "<%= campaignId %>";                                              
             var dealerLat = "<%= dealerLat %>", dealerLong = "<%= dealerLong%>";
             var bodHt, footerHt, scrollPosition, leadSourceId;                         
             var googleMapAPIKey = "<%= Bikewale.Utility.BWConfiguration.Instance.GoogleMapApiKey%>";
            var cityArea = "<%= dealerCity + "_" + dealerArea%>";
            var pageUrl = window.location.href;

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
          

        </script>
        <script type="text/javascript" src="<%= staticUrl != "" ? "http://st2.aeplcdn.com" + staticUrl : "" %>/m/src/service/details.js?<%= staticFileVersion %>"></script>
        <link href='https://fonts.googleapis.com/css?family=Open+Sans:400,700' rel='stylesheet' type='text/css' />
    </form>
</body>
</html>
