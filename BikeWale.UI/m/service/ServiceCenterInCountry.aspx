<%@ Page Language="C#" AutoEventWireup="false" Inherits="Bikewale.Mobile.Service.ServiceCenterInCountry" EnableViewState="false" %>
<%@ Register Src="~/m/controls/MUpcomingBikes.ascx" TagName="MUpcomingBikes" TagPrefix="BW" %>
<%@ Register Src="~/m/controls/MNewLaunchedBikes.ascx" TagName="MNewLaunchedBikes" TagPrefix="BW" %>
<!DOCTYPE html>
<html>
<head>
     <% 
         title = string.Format("Authorised {0}  Service Centers in India | {0} bike servicing  in India -  BikeWale", objMMV.MakeName);
         keywords = string.Format("{0} Servicing centers, {0} service centers, {0} service center contact details, Service Schedule for {0} bikes, bike repair, {0} bike repairing", objMMV.MakeName);
         description = string.Format("There are {1} authorised {0}  service centers in {2} in India. Get in touch with your nearest {0} bikes service center to get your bike serviced. Check your service schedules now.", objMMV.MakeName, ServiceCenterList.ServiceCenterCount, ServiceCenterList.CityCount);
         canonical = string.Format("http://www.bikewale.com/{0}-service-centers-in-india/", objMMV.MaskingName);
        AdPath = "/1017752/Bikewale_Mobile_Model";
        AdId = "1444028976556";
        Ad_320x50 = true;
        Ad_Bot_320x50 = true;
    %>

    <!-- #include file="/includes/headscript_mobile_min.aspx" -->
    <link href="/m/css/service/location.css" rel="stylesheet" type="text/css" />
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
                <div class="card-inner-padding font14 text-light-grey">
                    <p id="service-main-content">There are <%=ServiceCenterList.ServiceCenterCount %> authorised <%=objMMV.MakeName %> service centers in India. BikeWale strongly recommends you to avail services only from authorized <%=objMMV.MakeName %> service centers. These authorised service centers are</p><p id="service-more-content"> spread over <%=ServiceCenterList.CityCount%> cities to service your <%=objMMV.MakeName %> bike and keep your bike moving. Enter the name of your city in the search box provided below to find authorised <%=objMMV.MakeName %> service centers in your city. </p><a href="javascript:void(0)" id="read-more-target" rel="nofollow">... Read more</a>
                </div>
            </div>
        </section>

        <section>
            <div class="container bg-white box-shadow card-bottom-margin">
                <h2 class="padding-15-20 border-solid-bottom"><%=ServiceCenterList.ServiceCenterCount%> <%=objMMV.MakeName %> service centers in <%=ServiceCenterList.CityCount%> cities</h2>
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
                                            <a data-item-id="<%=stcity.Id %>" data-item-name="<%=stcity.CityName %>" data-lat="<%=stcity.Lattitude %>" data-long ="<%=stcity.Longitude %>" data-link="<%=stcity.Link %>" data-ServiceCenterCount="<%=stcity.ServiceCenterCountCity%>" title="<%=makeMaskingName %>-service-center-in-<%=stcity.CityMaskingName %>" href="/m/<%=makeMaskingName %>-service-center-in-<%=stcity.CityMaskingName %>/"><%=stcity.CityName %> (<%=stcity.ServiceCenterCountCity %>)</a>
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

        <section>
            <h2 class="section-heading">Bike Care - Maintenance tips</h2>
            <div class="container bg-white box-shadow card-bottom-margin content-inner-block-20">
                <div class="margin-bottom20 font14">
                    <div class="review-image-wrapper">
                        <a href="" title="">
                            <img class="lazy" data-original="https://imgd1.aeplcdn.com//370x208//bw/ec/24006/Side-75983.jpg?wm=0" alt="New colour scheme for Bajaj Pulsar RS200 in Columbia" src="">
                        </a>
                    </div>
                    <div class="review-heading-wrapper">
                        <a href="" title="" class="target-link">New colour scheme for Bajaj Pulsar RS200</a>
                        <div class="grid-7 alpha padding-right5">
                            <span class="bwmsprite calender-grey-sm-icon"></span>
                            <span class="article-stats-content">Jun 07, 2016</span>
                        </div>
                        <div class="grid-5 alpha omega">
                            <span class="bwmsprite author-grey-sm-icon"></span>
                            <span class="article-stats-content">BikeWale Team</span>
                        </div>
                        <div class="clear"></div>
                    </div>
                    <p class="margin-top10">
                        The Bajaj Pulsar RS200 gets a new white and red colour scheme combo for the Columbian market. The RS200 is available in India in three solid colours namely yellow, red...
                    </p>
                </div>

                <div class="margin-bottom20 font14">
                    <div class="review-image-wrapper">
                        <a href="" title="">
                            <img class="lazy" data-original="https://imgd1.aeplcdn.com//370x208//bw/ec/23838/Bajaj-Avenger-150-Street-Front-threequarter-74921.jpg?wm=0" alt="New colour scheme for Bajaj Pulsar RS200 in Columbia" src="">
                        </a>
                    </div>
                    <div class="review-heading-wrapper">
                        <a href="" title="" class="target-link">New colour scheme for Bajaj Pulsar RS200 in Columbia</a>
                        <div class="grid-7 alpha padding-right5">
                            <span class="bwmsprite calender-grey-sm-icon"></span>
                            <span class="article-stats-content">Jun 07, 2016</span>
                        </div>
                        <div class="grid-5 alpha omega">
                            <span class="bwmsprite author-grey-sm-icon"></span>
                            <span class="article-stats-content">BikeWale Team</span>
                        </div>
                        <div class="clear"></div>
                    </div>
                    <p class="margin-top10">
                        The Bajaj Pulsar RS200 gets a new white and red colour scheme combo for the Columbian market. The RS200 is available in India in three solid colours namely yellow, red...
                    </p>
                </div>

                <div class="margin-bottom20 font14">
                    <div class="review-image-wrapper">
                        <a href="" title="">
                            <img class="lazy" data-original="https://imgd1.aeplcdn.com//370x208//bw/ec/23997/Side-75963.jpg?wm=0" alt="New colour scheme for Bajaj Pulsar RS200 in Columbia" src="">
                        </a>
                    </div>
                    <div class="review-heading-wrapper">
                        <a href="" title="" class="target-link">New colour scheme for Bajaj Pulsar RS200 in Columbia</a>
                        <div class="grid-7 alpha padding-right5">
                            <span class="bwmsprite calender-grey-sm-icon"></span>
                            <span class="article-stats-content">Jun 07, 2016</span>
                        </div>
                        <div class="grid-5 alpha omega">
                            <span class="bwmsprite author-grey-sm-icon"></span>
                            <span class="article-stats-content">BikeWale Team</span>
                        </div>
                        <div class="clear"></div>
                    </div>
                    <p class="margin-top10">
                        The Bajaj Pulsar RS200 gets a new white and red colour scheme combo for the Columbian market. The RS200 is available in India in three solid colours namely yellow, red...
                    </p>
                </div>
                
                <a href="" class="font14">Read all bike maintenance tips<span class="bwmsprite blue-right-arrow-icon"></span></a>
            </div>
        </section>
        <%} %>
        <section>
            <h2 class="section-heading">Bike troubleshooting - FAQs</h2>
            <div class="container bg-white box-shadow card-bottom-margin padding-bottom20">
                <ul class="accordion-list">
                    <li>
                        <div class="accordion-head">
                            <p class="accordion-head-title">I have starting problem with my Vehicle.</p>
                            <span class="bwmsprite arrow-sm-down"></span>
                        </div>                        
                        <div class="accordion-body">
                            <p>The Bajaj Pulsar RS200 gets a new white and red colour scheme combo for the Columbian market. The RS200 is available in India in three solid colours namely yellow, red</p>
                        </div>
                    </li>
                    <li>
                        <div class="accordion-head">
                            <p class="accordion-head-title">The disc brakes are giving a noise when applied</p>
                            <span class="bwmsprite arrow-sm-down"></span>
                        </div>                        
                        <div class="accordion-body">
                            <p>The Bajaj Pulsar RS200 gets a new white and red colour scheme combo for the Columbian market. The RS200 is available in India in three solid colours namely yellow, red</p>
                        </div>
                    </li>
                    <li>
                        <div class="accordion-head">
                            <p class="accordion-head-title">Horn is very feeble</p>
                            <span class="bwmsprite arrow-sm-down"></span>
                        </div>                        
                        <div class="accordion-body">
                            <p>The Bajaj Pulsar RS200 gets a new white and red colour scheme combo for the Columbian market. The RS200 is available in India in three solid colours namely yellow, red</p>
                        </div>
                    </li>
                </ul>

                <div class="padding-left20">
                    <a href="" class="font14">Read all FAQs<span class="bwmsprite blue-right-arrow-icon"></span></a>
                </div>
            </div>
        </section>

        <script type="text/javascript" src="<%= staticUrl != "" ? "http://st1.aeplcdn.com" + staticUrl : "" %>/m/src/frameworks.js?<%= staticFileVersion %>"></script>
        <!-- #include file="/includes/footerBW_Mobile.aspx" -->
        <link href="<%= staticUrl != "" ? "http://st2.aeplcdn.com" + staticUrl : "" %>/m/css/bwm-common-btf.css?<%= staticFileVersion %>" rel="stylesheet" type="text/css" />
        <script type="text/javascript" src="<%= staticUrl != "" ? "http://st2.aeplcdn.com" + staticUrl : "" %>/m/src/common.min.js?<%= staticFileVersion %>"></script>
        <script type="text/javascript" src="<%= staticUrl != "" ? "http://st1.aeplcdn.com" + staticUrl : "" %>/m/src/service/location.js?<%= staticFileVersion %>"></script>
        <link href='https://fonts.googleapis.com/css?family=Open+Sans:400,700' rel='stylesheet' type='text/css' />
    </form>
</body>
</html>
