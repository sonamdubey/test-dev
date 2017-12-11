<%@ Page Language="C#" AutoEventWireup="false" Inherits="Bikewale.Mobile.Service.ServiceCenterInCountry" EnableViewState="false" %>
<%@ Register Src="~/m/controls/BikeCare.ascx" TagName="BikeCare" TagPrefix="BW" %>
<%@ Register Src="~/m/controls/ServiceCentersByBrand.ascx" TagName="OtherServiceCenters" TagPrefix="BW" %>
<%@ Register Src="~/m/controls/usedBikeModel.ascx" TagName="usedBikeModel" TagPrefix="BW" %>
<!DOCTYPE html>
<html>
<head>
     <%
         title = string.Format("Authorised {0}  Service Centers in India | {0} bike servicing  in India -  BikeWale", objMMV.MakeName);
         keywords = string.Format("{0} Servicing centers, {0} service centers, {0} service center contact details, Service Schedule for {0} bikes, bike repair, {0} bike repairing", objMMV.MakeName);
         description = string.Format("There are {1} authorised {0}  service centers in {2} cities in India. Get in touch with your nearest {0} bikes service center to get your bike serviced. Check your service schedules now.", objMMV.MakeName, ServiceCenterList.ServiceCenterCount, ServiceCenterList.CityCount);
         canonical = string.Format("https://www.bikewale.com/service-centers/{0}/", objMMV.MaskingName);
        AdPath = "/1017752/Bikewale_Mobile_Model";
        AdId = "1444028976556";
        Ad_320x50 = true;
        Ad_Bot_320x50 = true;
    %>

    <!-- #include file="/includes/headscript_mobile_min.aspx" -->
    <link rel="stylesheet" type="text/css" href="/m/css/service/location.css" />

    <script type="text/javascript">
        <!-- #include file="\includes\gacode_mobile.aspx" -->
    </script>
</head>
<body class="bg-light-grey">
    <form runat="server">
        <!-- #include file="/includes/headBW_Mobile.aspx" -->
         <% if(Ad_320x50){ %>
            <section>
                <div>
                    <!-- #include file="/ads/Ad320x50_mobile.aspx" -->
                </div>
            </section>
        <% } %>
        <%if(objMMV!=null){ %>
        <section>
            <div class="container bg-white card-bottom-margin">
                <h1 class="card-header"><%=objMMV.MakeName %> service centers in India</h1>
                <div class="card-inner-padding font14 text-light-grey collapsible-content">
                    <p class="main-content">There are <%=ServiceCenterList.ServiceCenterCount %> authorised <%=objMMV.MakeName %> service centers in India. BikeWale strongly recommends you to avail services only from authorized <%=objMMV.MakeName %> service centers. These authorised service centers are</p>
                    <p class="more-content"> spread over <%=ServiceCenterList.CityCount%> cities to service your <%=objMMV.MakeName %> bike and keep your bike moving. Enter the name of your city in the search box provided below to find authorised <%=objMMV.MakeName %> service centers in your city. </p><a href="javascript:void(0)" class="read-more-target" rel="nofollow">...Read more</a>
                </div>
            </div>
        </section>

        <section>
            <div class="container bg-white box-shadow card-bottom-margin">
                <h2 class="padding-15-20 border-solid-bottom"><%= String.Format("{0} {1}", ServiceCenterList.ServiceCenterCount,objMMV.MakeName) %> service centers in <%=ServiceCenterList.CityCount%> cities</h2>
                <div class="content-inner-block-20">
                    <div class="form-control-box">
                        <span class="bwmsprite search-icon-grey"></span>
                        <input type="text" class="form-control padding-right40" placeholder="Type to select city" id="getCityInput" />
                    </div>
                    <ul id="location-list">
                                  <% foreach (var st in ServiceCenterList.ServiceCenterDetailsList)
                       { %>
                                <li  class="item-state">
                                    <a data-item-id="<%=st.Id %>" data-item-name="<%=st.Name %>" data-lat="<%=st.Lat %>" data-long ="<%=st.Long %>" data-ServiceCenterCount="<%=st.ServiceCenterCountState%>"  href="javascript:void(0)" rel="nofollow" class="type-state" data-item-id="<%=st.Id %>"><%=st.Name %></a>
                                                 <ul class="location-list-city">
                                                     <% foreach (var stcity in st.Cities)
                       { %>

                                        <li>
                                            <a data-item-id="<%=stcity.Id %>" data-item-name="<%=stcity.CityName %>" data-lat="<%=stcity.Lattitude %>" data-long ="<%=stcity.Longitude %>" data-link="<%=stcity.Link %>" data-ServiceCenterCount="<%=stcity.ServiceCenterCountCity%>" title="<%=makeMaskingName %> service center in <%=stcity.CityMaskingName %>" href="/m/service-centers/<%=makeMaskingName %>/<%=stcity.CityMaskingName %>/"><%=stcity.CityName %> (<%=stcity.ServiceCenterCountCity %>)</a>
                                        </li>
                                      <%}%>
                                    </ul>

                                </li>
                              <%}%>

                            </ul>
                    <div id="no-result"></div>
                </div>
            </div>
        </section>
        <%if(ctrlOtherServiceCenters.FetchedRecordsCount>0){ %>
        <section>
        <BW:OtherServiceCenters runat="server" ID="ctrlOtherServiceCenters" />
            </section>
       <%} %>
          <div class="margin-top20 margin-right20 margin-left20 border-solid-bottom"></div>
        <% if (ctrlusedBikeModel.FetchCount > 0)
           { %>
        <div class="container bg-white box-shadow padding-top15">
   <BW:usedBikeModel runat="server" ID="ctrlusedBikeModel" />  
            </div>
        
            <% } %>
      <%if(ctrlBikeCare.FetchedRecordsCount>0) {%>
        <section>
            <BW:BikeCare runat="server" ID="ctrlBikeCare" />
        </section>
        <%} %>
        <section>
            <h2 class="section-heading">Bike Troubleshooting - FAQs</h2>
            <div class="container bg-white box-shadow card-bottom-margin padding-bottom20">
                <ul class="accordion-list">
                    <li>
                        <div class="accordion-head">
                            <p class="accordion-head-title">What to do if you have a puncture?</p>
                            <span class="bwmsprite arrow-sm-down"></span>
                        </div>
                        <div class="accordion-body">
                            <p>There's two kinds of tyres available to the public, the tube type and the tubeless type. If you've got spoked rims, you've got a tyre with a tube. The puncture will leave you without air suddenly, and the best you can do is flag down a passing cab or rickshaw, go to the puncture repair shop and get the person to the motorcycle. Repeat the trip to the repair shop (this time with the wheel) and back again to the motorcycle. If that isn't possible, put the bike in first, engage the clutch and walk the motorcycle to the shop. Remember that you risk damaging your tyre this way.<br /><br />Tubeless tyres are a lot easier to deal with. The very thing that punctures the tyre also seals the hole, so the leak is far slower. You can ride the motorcycle, but be very careful. Ride it with too little air and you risk damaging the tyre beyond repair. If the nearest puncture repair shop isn't equipped with a tubeless puncture repair kit, make them fill a lot of air in the tyre and ride on. If you can't find a puncture repair shop, even a bicycle pump can help you fill air in the tyre.</p>
                        </div>
                    </li>
                    <li>
                        <div class="accordion-head">
                            <p class="accordion-head-title">What to do if your battery is weak and you have no kick start lever?</p>
                            <span class="bwmsprite arrow-sm-down"></span>
                        </div>
                        <div class="accordion-body">
                            <p>We've all been there at some point, and there's the obvious - jumper cables.<br /><br />Remember to take a jump from a battery that has a higher rating than yours, else you run the risk of two motorcycles that won't start after your attempts. The next obvious thing to do is to either remove the battery, get it charged and reinstall it, or replace it with a fully charged one. There are a few other things you can do if jumper cables aren't available, or you don't know how to remove your battery.<br /><br />Note: if it is a large motorcycle (say over 400cc) do not attempt anything you read beyond this. If, however, you have a small motorcycle with a carburettor, here's exactly what you need to do: stick it in second, pull the clutch in, push the bike and release the clutch. As soon as it catches, pull the clutch in. If you have a helping hand, it is far safer to have one person sit on the motorcycle while the other pushes. Another trick that you can use for small motorcycles is putting them on the main stand - the same rules apply. Stick it in second, leave the ignition on and just give the rear wheel torque by pulling it in the correct direction. If you give it a hard enough tug, the bike should start.<br /><br />There is one other condition under which a push-start will not achieve any results at all: if you have fuel injection on your motorcycle, turn the key over to the 'on' position and put your ear near the fuel tank. If you hear a noise, however weak, it means that the fuel pump is still working enough to send fuel to the engine, and you have a chance of the bike starting. Pull the fuses to the headlamp to keep it from taking any more juice away from the fuel pump and try the push start. If you turn your key to 'on' and hear nothing at all, then don't bother trying, your motorcycle won't start no matter how much you push it.<br /><br />Revving the motorcycle to the redline will not make the battery charge faster - anything beyond 3000 rpm is a waste of fuel, so go for a 20-30 minute cruise to make sure the battery gets charged enough to crank the engine should you stall for any reason.</p>
                        </div>
                    </li>
                    <li>
                        <div class="accordion-head">
                            <p class="accordion-head-title">What to do if your clutch cable breaks?</p>
                            <span class="bwmsprite arrow-sm-down"></span>
                        </div>
                        <div class="accordion-body">
                            <p>If you have a scooter, obviously this isn't a problem. However, this can be quite a big issue if you've got something that needs gears to be shifted manually.<br /><br />The best thing to do is to stick it in neutral and either push the motorcycle along or have someone tow you. If this isn't possible, though, technology and a little bit of looking ahead can help you get to help. There's something called 'synchromesh' that gearboxes have today, and that means that you can actually change gears without using the clutch lever. It will take a little bit of practice, though, especially while downshifting. Upshifts will be a lot smoother. The biggest problem will be coming to a halt and taking off from a halt. For this, the obvious solution will be to not do it at all, so you can either wait for a time when there won't be traffic or use a route with little to no traffic or stop signals.<br /><br />If it cannot be avoided, though, you'll have to slow down as much you can in first gear, and then try to put it into neutral while using the brakes to come to a complete halt. Starting it will be very tricky, because it will be almost impossible to get it going with just enough throttle to remain in control of the motorcycle. If you have a main stand, you can try putting it on the main stand, putting it in gear and then doing a running start with it.<br /><br />Remember - these are very risky manoeuvres, so please do not try them unless there is an emergency and you cannot afford to wait at all.</p>
                        </div>
                    </li>
                </ul>

                <div class="padding-left20">
                    <a href="/m/bike-troubleshooting/" target="_blank" rel="noopener" class="font14">Read all FAQs<span class="bwmsprite blue-right-arrow-icon"></span></a>
                </div>
            </div>
        </section>
        <%} %>
        <script type="text/javascript" src="<%= staticUrl  %>/m/src/frameworks.js?<%= staticFileVersion %>"></script>
        <!-- #include file="/includes/footerBW_Mobile.aspx" -->
        <link href="<%= staticUrl  %>/m/css/bwm-common-btf.css?<%= staticFileVersion %>" rel="stylesheet" type="text/css" />
        <!-- #include file="/includes/footerscript_mobile.aspx" -->
        <script type="text/javascript" src="<%= staticUrl  %>/m/src/service/location.js?<%= staticFileVersion %>"></script>
        <!-- #include file="/includes/fontBW_Mobile.aspx" -->
    </form>
</body>
</html>
