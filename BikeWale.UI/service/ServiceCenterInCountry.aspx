<%@ Page Language="C#" AutoEventWireup="false" Inherits="Bikewale.Service.ServiceCenterInCountry" EnableViewState="false" Trace="false" Debug="false" %>
<%@ Import Namespace="Bikewale.Common" %>

<!doctype html>
<html>
<head>
    <%  title = string.Format("Authorised {0}  Service Centers in India | {0} bike servicing  in India -  BikeWale", objMMV.MakeName);
        keywords = string.Format("{0} Servicing centers, {0} service centers, {0} service center contact details, Service Schedule for {0} bikes, bike repair, {0} bike repairing", objMMV.MakeName);
        description = string.Format("There are {1} authorised {0}  service centers in {2} cities in India. Get in touch with your nearest {0} bikes service center to get your bike serviced. Check your service schedules now.", objMMV.MakeName, ServiceCenterList.ServiceCenterCount, ServiceCenterList.CityCount);
        canonical = string.Format("http://www.bikewale.com/{0}-service-centers-in-india/", objMMV.MaskingName);
        alternate = string.Format("http://www.bikewale.com/m/{0}-service-centers-in-india/", objMMV.MaskingName);
        isAd970x90Shown = false;
        isAd300x250Shown = false;
        isAd300x250BTFShown = false;
        isAd970x90BottomShown = false;
        isHeaderFix = false;
    %>
    <!-- #include file="/includes/headscript_desktop_min.aspx" -->
    <link href="/css/service/location.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript">
        <!-- #include file="\includes\gacode_desktop.aspx" -->
    </script>
    <script src="http://maps.googleapis.com/maps/api/js?key=<%= Bikewale.Utility.BWConfiguration.Instance.GoogleMapApiKey %>&libraries=places"></script>
    <script type="text/javascript" src="<%= staticUrl != "" ? "http://st2.aeplcdn.com" + staticUrl : "" %>/src/new/markerwithlabel.js"></script>
</head>
<body class="bg-light-grey">
    <form runat="server">
        <!-- #include file="/includes/headBW.aspx" -->
        <section>
            <div class="container padding-top10">
                <div class="grid-12">
                    <div class="breadcrumb margin-bottom15">
                        <ul>
                            <li itemscope="" itemtype="http://data-vocabulary.org/Breadcrumb">
                                <a href="/" itemprop="url">
                                    <span itemprop="title">Home</span>
                                </a>
                            </li>
                            <li itemscope="" itemtype="http://data-vocabulary.org/Breadcrumb">
                                <span class="bwsprite fa-angle-right margin-right10"></span>
                                 <a href="/bike-service-center/" itemprop="url">
                                    <span itemprop="title">Service Center Locator</span>
                                </a>
                            </li>
                            <li itemscope="" itemtype="http://data-vocabulary.org/Breadcrumb">
                                <span class="bwsprite fa-angle-right margin-right10"></span>
                                <span itemprop="title"><%=objMMV.MakeName %> Bikes Service Centers</span>
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
                    <div class="bg-white">
                        <h1 class="section-header"><%=objMMV.MakeName %> service centers in India</h1>
                        <p class="section-inner-padding font14 text-light-grey">There are <%=ServiceCenterList.ServiceCenterCount %> authorised <%=objMMV.MakeName %> service centers in India. BikeWale strongly recommends you to avail services only from authorized <%=objMMV.MakeName %> service centers. These authorised service centers are</p><p id="service-more-content"> spread over <%=ServiceCenterList.CityCount%> cities to service your <%=objMMV.MakeName %> bike and keep your bike moving. Enter the name of your city in the search box provided below to find authorised <%=objMMV.MakeName %> service centers in your city.</p>
                    </div>
                </div>
                <div class="clear"></div>
            </div>
        </section>

        <section>
            <div class="container section-bottom-margin">
                <div class="grid-12">
                    <div class="content-box-shadow">
                        <h2 class="section-h2-title padding-15-20"><%=ServiceCenterList.ServiceCenterCount%> <%=objMMV.MakeName %> service centers in <%=ServiceCenterList.CityCount%> cities</h2>
                        <div id="listing-left-column" class="grid-4">
                            <div id="filter-input" class="form-control-box">
                                <span class="bwsprite search-icon-grey"></span>
                                <input type="text" class="form-control padding-right40" placeholder="Type to search city" id="getCityInput" />
                            </div>
                            <ul id="location-list">
                                         <% foreach (var st in ServiceCenterList.ServiceCenterDetailsList)
                       { %>
                                    <li  class="item-state">
                                        <p data-item-id="<%=st.Id %>" data-item-name="<%=st.Name %>" data-lat="<%=st.Lat %>" data-long ="<%=st.Long %>" data-dealercount="<%=st.DealerCountState%>" class="type-state cur-pointer" data-item-id="<%=st.Id %>"><%=st.Name %></p>
                                        <ul class="location-list-city">
                                                                              <% foreach (var stcity in st.Cities)
                       { %>
                                                <li>
                                                    <a data-item-id="<%=stcity.CityId %>" data-item-name="<%=stcity.CityName %>" data-lat="<%=stcity.Lattitude %>" data-long ="<%=stcity.Longitude %>" data-link="<%=stcity.Link %>" data-dealercount="<%=stcity.DealersCount%>" title=" <%=objMMV.MakeName%> dealer showrooms in <%=stcity.CityName %>" href="/<%=makeMaskingName %>-dealer-showrooms-in-<%=stcity.CityMaskingName %>/"><%=stcity.CityName %> (<%=stcity.DealersCount %>)</a>
                                                </li>
                                            <%}%>
                                        </ul>                                   
                                    </li>
                                <%}%>                                
                            </ul>
                            <div id="no-result"></div>
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
                <h2 class="section-heading">Bike Care - Maintenance tips</h2>
                <div class="grid-12">
                    <div class="content-box-shadow content-inner-block-20">
                        <ul class="article-list">
                            <li>
                                <div class="grid-4 alpha">
                                    <div class="model-preview-image-container">
                                        <a href="" title="TVS Apache RTR 200 4V Carb vs Bajaj Pulsar AS200 vs Honda Hornet 160R: Comparison Test">
                                            <img class="lazy" data-original="https://imgd1.aeplcdn.com//310x174//bw/ec/23488/Side-73148.jpg?wm=0" alt="TVS Apache RTR 200 4V Carb vs Bajaj Pulsar AS200 vs Honda Hornet 160R: Comparison Test" src="">
                                        </a>
                                    </div>
                                </div>
                                <div class="grid-8 padding-left5 omega">
                                    <a href="" class="article-target-link">TVS Apache RTR 200 4V Carb vs Bajaj Pulsar AS200 vs Honda Hornet 160R: Comparison Test</a>
                                    <div class="article-stats-left-grid margin-bottom10">
                                        <span class="bwsprite calender-grey-sm-icon"></span>
                                        <span class="article-stats-content">Nov 15, 2016</span>
                                    </div>
                                    <div class="article-stats-right-grid margin-bottom10">
                                        <span class="bwsprite author-grey-sm-icon"></span>
                                        <span class="article-stats-content">Sagar Bhanushali</span>
                                    </div>
                                    <p class="font14 line-height17">Jagan Kumar from TVS and RACR’s Rajini Krishnan shared wins in the Indian Motorcycle Racing Championship’s (IMRC) Super Sport category at the Madras Motor Race Track. Ami Van Poederooijen won overall in the combined Pro Stock and Super Sport category...</p>
                                </div>
                                <div class="clear"></div>
                            </li>

                            <li>
                                <div class="grid-4 alpha">
                                    <div class="model-preview-image-container">
                                        <a href="" title="TVS Apache RTR 200 4V Carb vs Bajaj Pulsar AS200 vs Honda Hornet 160R: Comparison Test">
                                            <img class="lazy" data-original="https://imgd1.aeplcdn.com//310x174//bw/ec/21085/Honda-CB-Shine-Side-61910.jpg?wm=0&t=181939320&t=181939320" alt="TVS Apache RTR 200 4V Carb vs Bajaj Pulsar AS200 vs Honda Hornet 160R: Comparison Test" src="">
                                        </a>
                                    </div>
                                </div>
                                <div class="grid-8 padding-left5 omega">
                                    <a href="" class="article-target-link">Bajaj Avenger 220 Cruise vs Royal Enfield Thunderbird 350 : Comparison Test</a>
                                    <div class="article-stats-left-grid margin-bottom10">
                                        <span class="bwsprite calender-grey-sm-icon"></span>
                                        <span class="article-stats-content">Nov 15, 2016</span>
                                    </div>
                                    <div class="article-stats-right-grid margin-bottom10">
                                        <span class="bwsprite author-grey-sm-icon"></span>
                                        <span class="article-stats-content">Sagar Bhanushali</span>
                                    </div>
                                    <p class="font14 line-height17">Jagan Kumar from TVS and RACR’s Rajini Krishnan shared wins in the Indian Motorcycle Racing Championship’s (IMRC) Super Sport category...</p>
                                </div>
                                <div class="clear"></div>
                            </li>

                            <li>
                                <div class="grid-4 alpha">
                                    <div class="model-preview-image-container">
                                        <a href="" title="TVS Apache RTR 200 4V Carb vs Bajaj Pulsar AS200 vs Honda Hornet 160R: Comparison Test">
                                            <img class="lazy" data-original="https://imgd1.aeplcdn.com//310x174//bw/ec/23488/Side-73148.jpg?wm=0" alt="TVS Apache RTR 200 4V Carb vs Bajaj Pulsar AS200 vs Honda Hornet 160R: Comparison Test" src="">
                                        </a>
                                    </div>
                                </div>
                                <div class="grid-8 padding-left5 omega">
                                    <a href="" class="article-target-link">TVS Apache RTR 200 4V Carb vs Bajaj Pulsar AS200 vs Honda Hornet 160R: Comparison Test</a>
                                    <div class="article-stats-left-grid margin-bottom10">
                                        <span class="bwsprite calender-grey-sm-icon"></span>
                                        <span class="article-stats-content">Nov 15, 2016</span>
                                    </div>
                                    <div class="article-stats-right-grid margin-bottom10">
                                        <span class="bwsprite author-grey-sm-icon"></span>
                                        <span class="article-stats-content">Sagar Bhanushali</span>
                                    </div>
                                    <p class="font14 line-height17">Indian Motorcycle Racing Championship’s (IMRC) Super Sport category at the Madras Motor Race Track. Ami Van Poederooijen won overall in the combined Pro Stock and Super Sport category...</p>
                                </div>
                                <div class="clear"></div>
                            </li>
                        </ul>

                        <a href="" class="font14">Read all bike maintenance tips<span class="bwsprite blue-right-arrow-icon"></span></a>
                    </div>
                </div>
                <div class="clear"></div>
            </div>
        </section>

        <section>
            <div class="container section-bottom-margin">
                <h2 class="section-heading">Bike troubleshooting - FAQs</h2>
                <div class="grid-12">
                    <div class="content-box-shadow padding-bottom20">
                        <ul class="accordion-list">
                            <li>
                                <div class="accordion-head">
                                    <p class="accordion-head-title">What to do if you have a puncture?</p>
                                    <span class="bwsprite arrow-sm-down"></span>
                                </div>                        
                                <div class="accordion-body">
                                    <p>There's two kinds of tyres available to the public, the tube type and the tubeless type. If you've got spoked rims, you've got a tyre with a tube. The puncture will leave you without air suddenly, and the best you can do is flag down a passing cab or rickshaw, go to the puncture repair shop and get the person to the motorcycle. Repeat the trip to the repair shop (this time with the wheel) and back again to the motorcycle. If that isn't possible, put the bike in first, engage the clutch and walk the motorcycle to the shop. Remember that you risk damaging your tyre this way.<br /><br />Tubeless tyres are a lot easier to deal with. The very thing that punctures the tyre also seals the hole, so the leak is far slower. You can ride the motorcycle, but be very careful. Ride it with too little air and you risk damaging the tyre beyond repair. If the nearest puncture repair shop isn't equipped with a tubeless puncture repair kit, make them fill a lot of air in the tyre and ride on. If you can't find a puncture repair shop, even a bicycle pump can help you fill air in the tyre.</p>
                                </div>
                            </li>
                            <li>
                                <div class="accordion-head">
                                    <p class="accordion-head-title">What to do if your battery is weak and you have no kick start lever?</p>
                                    <span class="bwsprite arrow-sm-down"></span>
                                </div>                        
                                <div class="accordion-body">
                                    <p>We've all been there at some point, and there's the obvious - jumper cables. Remember to take a jump from a battery that has a higher rating than yours, else you run the risk of two motorcycles that won’t start after your attempts. The next obvious thing to do is to either remove the battery, get it charged and reinstall it, or replace it with a fully charged one. There are a few other things you can do if jumper cables aren’t available, or you don’t know how to remove your battery. Note: if it is a large motorcycle (say over 400cc) do not attempt anything you read beyond this. If, however, you have a small motorcycle with a carburettor, here's exactly what you need to do: stick it in second, pull the clutch in, push the bike and release the clutch. As soon as it catches, pull the clutch in. If you have a helping hand, it is far safer to have one person sit on the motorcycle while the other pushes. Another trick that you can use for small motorcycles is putting them on the main stand – the same rules apply. Stick it in second, leave the ignition on and just give the rear wheel torque by pulling it in the correct direction. If you give it a hard enough tug, the bike should start.<br /><br />There is one other condition under which a push-start will not achieve any results at all: if you have fuel injection on your motorcycle, turn the key over to the ‘on’ position and put your ear near the fuel tank. If you hear a noise, however weak, it means that the fuel pump is still working enough to send fuel to the engine, and you have a chance of the bike starting. Pull the fuses to the headlamp to keep it from taking any more juice away from the fuel pump and try the push start. If you turn your key to ‘on’ and hear nothing at all, then don’t bother trying, your motorcycle won’t start no matter how much you push it.<br /><br />Revving the motorcycle to the redline will not make the battery charge faster – anything beyond 3000 rpm is a waste of fuel, so go for a 20-30 minute cruise to make sure the battery gets charged enough to crank the engine should you stall for any reason.</p>
                                </div>
                            </li>
                            <li>
                                <div class="accordion-head">
                                    <p class="accordion-head-title">What to do if your clutch cable breaks?</p>
                                    <span class="bwsprite arrow-sm-down"></span>
                                </div>                        
                                <div class="accordion-body">
                                    <p>If you have a scooter, obviously this isn’t a problem. However, this can be quite a big issue if you’ve got something that needs gears to be shifted manually. The best thing to do is to stick it in neutral and either push the motorcycle along or have someone tow you. If this isn’t possible, though, technology and a little bit of looking ahead can help you get to help. There’s something called ‘synchromesh’ that gearboxes have today, and that means that you can actually change gears without using the clutch lever. It will take a little bit of practice, though, especially while downshifting. Upshifts will be a lot smoother. The biggest problem will be coming to a halt and taking off from a halt. For this, the obvious solution will be to not do it at all, so you can either wait for a time when there won’t be traffic or use a route with little to no traffic or stop signals. If it cannot be avoided, though, you’ll have to slow down as much you can in first gear, and then try to put it into neutral while using the brakes to come to a complete halt. Starting it will be very tricky, because it will be almost impossible to get it going with just enough throttle to remain in control of the motorcycle. If you have a main stand, you can try putting it on the main stand, putting it in gear and then doing a running start with it. Remember – these are very risky manoeuvres, so please do not try them unless there is an emergency and you cannot afford to wait at all.</p>
                                </div>
                            </li>
                        </ul>

                        <a href="" class="font14 margin-left20">Read all FAQs<span class="bwsprite blue-right-arrow-icon"></span></a>
                    </div>
                </div>
                <div class="clear"></div>
            </div>
        </section>
        
        <script type="text/javascript" src="<%= staticUrl != "" ? "http://st1.aeplcdn.com" + staticUrl : "" %>/src/frameworks.js?<%=staticFileVersion %>"></script>
        <!-- #include file="/includes/footerBW.aspx" -->
        <link href="<%= staticUrl != "" ? "http://st2.aeplcdn.com" + staticUrl : "" %>/css/bw-common-btf.css?<%=staticFileVersion %>" rel="stylesheet" type="text/css" />
        <script type="text/javascript" src="<%= staticUrl != string.Empty ? "http://st2.aeplcdn.com" + staticUrl : string.Empty %>/src/common.min.js?<%= staticFileVersion %>"></script>
        <script type="text/javascript">
            var dealersByCity = true;
          <%--  var cityArr = JSON.parse('<%= cityArr %>');--%>
       <%--     var stateLat = '<%= (dealerCity != null && dealerCity.dealerStates != null) ? dealerCity.dealerStates.StateLatitude : string.Empty %>';
            var stateLong = '<%= (dealerCity != null && dealerCity.dealerStates != null) ? dealerCity.dealerStates.StateLongitude : string.Empty %>';--%>
        </script>
        <script type="text/javascript" src="<%= staticUrl != "" ? "http://st2.aeplcdn.com" + staticUrl : "" %>/src/service/location.js?<%= staticFileVersion %>"></script>
        <link href='https://fonts.googleapis.com/css?family=Open+Sans:400,700' rel='stylesheet' type='text/css' />
        <!--[if lt IE 9]>
            <script src="/src/html5.js"></script>
        <![endif]-->
    </form>
</body>
</html>
