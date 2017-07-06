﻿<%@ Page Language="C#" Inherits="Bikewale.ServiceCenter.ServiceCenterDetails" AutoEventWireup="false" EnableViewState="false" %>
<%@ Register Src="~/controls/DealerCard.ascx" TagName="DealerCard" TagPrefix="BW" %>
<%@ Register Src="~/controls/ServiceCenterCard.ascx" TagName="ServiceCenterCard" TagPrefix="BW" %>
<%@ Register Src="~/controls/serviceschedule.ascx" TagName="ServiceSchedule" TagPrefix="BW" %>
<%@ Register Src="~/controls/MostPopularBikes_new.ascx" TagName="MostPopularBikesMake" TagPrefix="BW" %>
<%@ Register Src="~/controls/usedBikeModel.ascx" TagName="usedBikeModel" TagPrefix="BW" %>
<!DOCTYPE html>
<html>
<head>
        <%
            if (serviceVM != null && serviceVM.objServiceCenterData != null)
            {
                keywords = String.Format("{0}, {0} {1}, {2} servicing {1}", serviceVM.objServiceCenterData.Name, serviceVM.CityName, serviceVM.MakeName);
                description = String.Format("{0} is an authorised service center of {1}. Get all details related to servicing cost, pick and drop facility and service schedule from {0}", serviceVM.objServiceCenterData.Name, serviceVM.MakeName);
                title = String.Format("{0} {1} | {0} service center in {1} - BikeWale ", serviceVM.objServiceCenterData.Name, serviceVM.objServiceCenterData.CityName);
                string url = Bikewale.Utility.UrlFormatter.GetServiceCenterUrl(makeMaskingName, serviceVM.CityMaskingName, serviceVM.objServiceCenterData.Name, serviceCenterId);
                canonical = string.Format("https://www.bikewale.com{0}", url);
                alternate = string.Format("https://www.bikewale.com/m{0}", url);
                AdId = "1395986297721";
                AdPath = "/1017752/BikeWale_New_";
                isAd970x90Shown = true;
                isAd970x90BottomShown = true;
                isAd300x250Shown = false;
                isAd300x250BTFShown = false;
            }
    %>
    <!-- #include file="/includes/headscript_desktop_min.aspx" -->
    <link rel="stylesheet" type="text/css" href="/css/service/details.css" />
    <script type="text/javascript">
        <!-- #include file="\includes\gacode_desktop.aspx" -->
    </script>
</head>
<body class="bg-light-grey header-fixed-inner">
    <form id="Form1" runat="server">
        <!-- #include file="/includes/headBW.aspx" -->
        <% if (serviceVM != null && serviceVM.objServiceCenterData != null)
          { %>
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
                                    <span class="bwsprite fa-angle-right margin-right10"></span><a itemprop="url" href="/bike-service-center/" title="Service Center Locator"><span itemprop="title">Service Center Locator</span></a>
                                </li>
                                <li itemscope="" itemtype="http://data-vocabulary.org/Breadcrumb">
                                    <span class="bwsprite fa-angle-right margin-right10"></span><a itemprop="url" href="/<%=makeMaskingName %>-service-center-in-india/" title="<%=serviceVM.MakeName%> Service Centers"><span itemprop="title"><%=serviceVM.MakeName%> Service Centers</span></a>
                                </li>
                                 <li itemscope="" itemtype="http://data-vocabulary.org/Breadcrumb">
                                    <span class="bwsprite fa-angle-right margin-right10"></span><a itemprop="url" href="/<%=makeMaskingName %>-service-center-in-<%=serviceVM.CityMaskingName %>/" title="<%=serviceVM.MakeName%> Service Center in <%=serviceVM.objServiceCenterData.CityName%>"><span itemprop="title"><%=serviceVM.MakeName%> Service Center in <%=serviceVM.objServiceCenterData.CityName%></span></a>
                                </li>
                                <li class="current"><span class="bwsprite fa-angle-right margin-right10"></span><%=serviceVM.objServiceCenterData.Name %></li>
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
                            <h1 class="section-header"><%=serviceVM.objServiceCenterData.Name %>, <%= serviceVM.CityName %></h1>
                            <div class="section-inner-padding">
                                <div class="grid-7 alpha omega font14">
                                    <% if (serviceVM.objServiceCenterData != null)
                                      { %>
                                    <h2 class="font14 margin-bottom10">Authorized <%= serviceVM.MakeName %> service center in <%=serviceVM.CityName %></h2>
                                    <% if (!string.IsNullOrEmpty(serviceVM.objServiceCenterData.Address))
                                       {%>
                                    <div class="margin-bottom10">
                                        <span class="bwsprite dealership-loc-icon vertical-top"></span>
                                        <span class="vertical-top text-light-grey details-column"><%=serviceVM.objServiceCenterData.Address %></span>
                                    </div>
                                    <%} %>
                                    <% if (!string.IsNullOrEmpty(serviceVM.objServiceCenterData.Phone))
                                       {%>
                                    <div class="margin-bottom10">
                                        <span class="bwsprite phone-black-icon vertical-top"></span>
                                        <span class="vertical-top text-bold details-column"><%=serviceVM.objServiceCenterData.Phone %></span>
                                    </div>
                                    <%} %>
                                    <% if (!string.IsNullOrEmpty(serviceVM.objServiceCenterData.Email))
                                       {%>
                                    <div class="margin-bottom10">
                                        <span class="bwsprite mail-grey-icon vertical-top"></span>
                                        <a href="mailto:<%=serviceVM.objServiceCenterData.Email %>" target="_blank" class="vertical-top text-light-grey" rel="nofollow">
                                            <span class="dealership-card-details"><%=serviceVM.objServiceCenterData.Email %></span>
                                        </a>
                                    </div>
                                    <%} %>
                                    <%} %>
                                    <% if (serviceVM.objServiceCenterData != null && serviceVM.objServiceCenterData.Lattitude > 0 && serviceVM.objServiceCenterData.Longitude > 0)
                                       { %>
                                    <div id="commute-distance-form" class="margin-top20">
                                        <p class="text-bold margin-bottom15">Get commute distance and time:</p>
                                        <div class="leftfloat form-control-box margin-right15">
                                            <input id="locationSearch" type="text" class="form-control" placeholder="Type in your location" />
                                            <span id="getUserLocation" class="crosshair-icon"></span>
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
                                    <% if (serviceVM.objServiceCenterData != null && serviceVM.objServiceCenterData.Lattitude > 0 && serviceVM.objServiceCenterData.Longitude > 0)
                                       { %>
                                    <div id="dealer-map" style="width:378px;height:254px;border:1px solid #eee;"></div>
                                    <div id="get-direction-button" title="Get directions">
                                        <a id="linkMap" href="https://maps.google.com/?saddr=&amp;daddr=<%=serviceVM.objServiceCenterData.Lattitude %>,<%=serviceVM.objServiceCenterData.Longitude %>" target="_blank">
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
         <% if(ctrlServiceCenterCard.showWidget && ctrlServiceCenterCard.centerData!=null && ctrlServiceCenterCard.centerData.Count>1){ %>
            <section>
                <div class="container section-bottom-margin">
                   <div class="grid-12">
                    <div class="content-box-shadow padding-bottom20">
                      <BW:ServiceCenterCard runat="server" ID="ctrlServiceCenterCard" />
                   </div>
                 </div>
                <div class="clear"></div>
               </div>
            </section>
        <% } %>
        <script type="text/javascript" src="<%= staticUrl %>/src/frameworks.js?<%=staticFileVersion %>"></script>
            <section>
                <BW:ServiceSchedule runat="server" ID="ctrlServiceSchedule" />
            </section>
        
            <section>
                <div class="container section-bottom-margin">
                    <div class="grid-12">
                        <div class="content-box-shadow">
                            <h2 class="section-h2-title padding-15-20">Tips from BikeWale experts to keep your bike in good shape!</h2>
                            <ul id="bw-tips-list">
                                <li class="grid-6">
                                    <a href="/bike-care/" title="Bike Care - Maintenance tips" target="_blank">
                                        <span class="service-sprite care-icon"></span>
                                        <h3 class="bike-tips-label margin-left10 inline-block">Bike Care - Maintenance tips</h3>
                                        <span class="bwsprite right-arrow"></span>
                                    </a>
                                </li>
                                <li class="grid-6">
                                    <a href="/bike-troubleshooting/" title="Bike troubleshooting - FAQs" target="_blank">
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
             <% if (ctrlPopoularBikeMake.FetchedRecordsCount > 0 || ctrlusedBikeModel.FetchCount > 0)
                { %>
        <section>
            <div class="container">
                <div class="grid-12">
                    <div class="content-box-shadow">
                        <% if (ctrlPopoularBikeMake.FetchedRecordsCount > 0)
                           { %>
                         <div class="carousel-heading-content padding-top20">
                            <div class="swiper-heading-left-grid inline-block">
                                <h2>Popular <%=serviceVM.MakeName %> bikes in <%= serviceVM.CityName %></h2>
                            </div><div class="swiper-heading-right-grid inline-block text-right">
                                <a href="/<%= makeMaskingName %>-bikes/" title="<%= serviceVM.MakeName %> Bikes" class="btn view-all-target-btn">View all</a>
                            </div>
                            <div class="clear"></div>
                        </div>
                        <BW:MostPopularBikesMake runat="server" ID="ctrlPopoularBikeMake" />
                        <%} %>
                        <div class="margin-left10 margin-right10 border-solid-bottom"></div>
                        <!-- Used bikes widget -->
                             <% if (ctrlusedBikeModel.FetchCount>0)
                       { %>
                 
                    <BW:usedBikeModel runat="server" ID="ctrlusedBikeModel" />
                        
                    <% } %>  
                         <div class="margin-left10 margin-right10 border-solid-bottom"></div>
                        <div class="clear"></div>
                    </div>
                </div>
                
                <div class="clear"></div>
            </div>
        </section>
        <% } %>
                <% if (ctrlDealerCard.showWidget)
                   { %>
        <section>
            <div class="container section-bottom-margin">
                <div class="grid-12">
                    <div class="content-box-shadow">
                        <BW:DealerCard runat="server" ID="ctrlDealerCard" />
                    </div>
                </div>
                <div class="clear"></div>
            </div>
        </section>
        <% } %>


                <div class="container section-bottom-margin">
                    <div class="grid-12 font12">
                        <p><span class="font14"><strong>Disclaimer</strong>:</span> The above-mentioned information about authorised <%= serviceVM.MakeName %> service centers is furnished to best of our knowledge. The facilities of pick and drop, timings and service schedule related information might vary slightly from service center to service center. Please check with the authorised <%= serviceVM.MakeName %> service center before scheduling an appointment.</p>
                    </div>
                    <div class="clear"></div>
                </div>
            </section>   
        <% } %>
        <!-- #include file="/includes/footerBW.aspx" -->
        <script type="text/javascript">
            var serviceLat = '<%= serviceVM!= null && serviceVM.objServiceCenterData.Lattitude> 0? serviceVM.objServiceCenterData.Lattitude.ToString() : string.Empty %>';
            var serviceLong = '<%= serviceVM!= null && serviceVM.objServiceCenterData.Longitude> 0? serviceVM.objServiceCenterData.Longitude.ToString() : string.Empty %>';
            var currentCityName = '<%=cityName%>';
            var googleMapAPIKey = "<%= Bikewale.Utility.BWConfiguration.Instance.GoogleMapApiKey %>";
            var pageSrcId = eval("<%= Bikewale.Utility.BWConfiguration.Instance.SourceId %>");
        </script>
        <link href="<%= staticUrl != "" ? "https://st2.aeplcdn.com" + staticUrl : "" %>/css/bw-common-btf.css?<%=staticFileVersion %>" rel="stylesheet" type="text/css" />
        <!-- #include file="/includes/footerscript.aspx" -->
        <script type="text/javascript" src="<%= staticUrl != "" ? "https://st2.aeplcdn.com" + staticUrl : "" %>/src/service/details.js?<%= staticFileVersion %>"></script>
        <script src="https://maps.googleapis.com/maps/api/js?key=<%= Bikewale.Utility.BWConfiguration.Instance.GoogleMapApiKey %>&libraries=places&callback=initializeMap""></script>
        <!-- #include file="/includes/fontBW.aspx" -->
    </form>
</body>
</html>
