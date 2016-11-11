<%@ Page Language="C#" AutoEventWireup="false" Inherits="Bikewale.Mobile.Service.ServiceCenterDetails" EnableViewState="false" %>
<%@ Register Src="~/m/controls/DealersCard.ascx" TagName="DealerCard" TagPrefix="BW" %>
<%@ Register Src="~/m/controls/ServiceCenterCard.ascx" TagName="ServiceCenterCard" TagPrefix="BW" %>
<%@ Register Src="~/m/controls/serviceschedule.ascx" TagName="ServiceSchedule" TagPrefix="BW" %>
<%@ Register Src="~/m/controls/LeadCaptureControl.ascx" TagName="LeadCapture" TagPrefix="BW" %>

<!DOCTYPE html>
<html>
<head>
    <%
        keywords = String.Format("{0}, {0} {1}, {2} servicing {1}", serviceCenteName, serviceCity, makeName);
        description = String.Format("{0} is an authorised service center of {1}. Get all details related to servicing cost, pick and drop facility and service schedule from {0}", serviceCenteName,makeName );
        title = String.Format("{0} {1} | {0} service center in {1} - BikeWale ", serviceCenteName, serviceCity);
        canonical = String.Format("http://www.bikewale.com{0}", Bikewale.Utility.UrlFormatter.GetServiceCenterUrl(makeMaskingName, cityMaskingName, serviceCenteName, (int)dealerId));
    %>
    <!-- #include file="/includes/headscript_mobile_min.aspx" -->
    <link href="/m/css/service/details.css" rel="stylesheet" type="text/css" />
    <script src="http://maps.googleapis.com/maps/api/js?key=<%= Bikewale.Utility.BWConfiguration.Instance.GoogleMapApiKey %>&libraries=places"></script>
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
                <h1 class="card-header"><%= objServiceCenterData.Name %></h1>
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
                        <a href="" class="text-default text-bold maskingNumber">
                            <span class="bwmsprite tel-sm-grey-icon vertical-top"></span>
                            <span class="vertical-top text-bold details-column">
                                <% if (!(String.IsNullOrEmpty(objServiceCenterData.Mobile)))
                                   { %>
                                            <%= objServiceCenterData.Mobile.Trim()%><% }
                        if (!(String.IsNullOrEmpty(objServiceCenterData.Mobile)) && !(String.IsNullOrEmpty(objServiceCenterData.Phone)))
                        {%>, <%}
                        if (!(String.IsNullOrEmpty(objServiceCenterData.Phone)))
                        { %>
                                            <%= objServiceCenterData.Phone.Trim() %>
                                            <% } %>
                            </span>
                        </a>
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
                        <span id="getUserLocation" class="crosshair-icon position-abt pos-right10 pos-top10"></span>
                    </div>
                    <div class="location-details margin-bottom15">
                        Distance: <span id="commuteDistance" class="margin-right10"></span>
                        Time: <span id="commuteDuration"></span>
                    </div>
                    <div id="commuteResults"></div>
                    <a id="anchorGetDir" href="http://maps.google.com/maps?z=12&t=m&q=loc:<%= serviceLat %>,<%= serviceLong %>" target="_blank"><span class="bwmsprite get-direction-icon margin-right5"></span>Get directions</a>
                    <% } %>
                </div>
            </div>
        </section>
       <% } %>
        <section>
            <% if (ctrlServiceCenterCard.showWidget)
                   { %>
                    <BW:ServiceCenterCard runat="server" ID="ctrlServiceCenterCard" />
                <% }  %>
        </section>
        <script type="text/javascript" src="<%= staticUrl != "" ? "http://st1.aeplcdn.com" + staticUrl : "" %>/m/src/frameworks.js?<%= staticFileVersion %>"></script>        
        
        <section>
            <!-- schedule widget start -->
            <BW:ServiceSchedule runat="server" ID="ctrlServiceSchedule" />
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

        <% if (ctrlDealerCard.showWidget) { %>
        <section>
            <div class="container bg-white box-shadow card-bottom-margin padding-bottom20 padding-top15">
                <div class="padding-right20 padding-left20 margin-bottom15">
                    <h2 class="margin-bottom5">Looking to buy a new <%= makeName %> bike in <%=serviceCity %>?</h2>
                    <p>Check out authorised <%= makeName %> dealers in <%=serviceCity %></p>
                    <BW:DealerCard runat="server" ID="ctrlDealerCard" />  
                </div>
            </div>            
        </section>
         <% }  %>

        <section>
            <div class="container margin-bottom20 font12 padding-top5 padding-right20 padding-left20">
                <span class="font14"><strong>Disclaimer:</strong></span> The above mentioned information about <%=makeName %> dealership showrooms in <%=serviceCity %> is furnished to the best of our knowledge. 
                    All <%=makeName %> bike models and colour options may not be available at each of the <%=makeName %> dealers. 
                    We recommend that you call and check with your nearest <%=makeName %> dealer before scheduling a showroom visit.
            </div>
        </section>

         <BW:LeadCapture ID="ctrlLeadCapture" runat="server" />
        <!-- #include file="/includes/footerBW_Mobile.aspx" -->
        <link href="<%= staticUrl != "" ? "http://st2.aeplcdn.com" + staticUrl : "" %>/m/css/bwm-common-btf.css?<%= staticFileVersion %>" rel="stylesheet" type="text/css" />
        <link href="<%= staticUrl != "" ? "http://st1.aeplcdn.com" + staticUrl : "" %>/css/chosen.min.css?<%= staticFileVersion %>" type="text/css" rel="stylesheet" />
        <script type="text/javascript" src="<%= staticUrl != "" ? "http://st2.aeplcdn.com" + staticUrl : "" %>/m/src/common.min.js?<%= staticFileVersion %>"></script>
        <script type="text/javascript" src="<%= staticUrl != "" ? "http://st1.aeplcdn.com" + staticUrl : "" %>/m/src/chosen-jquery-min-mobile.js?<%= staticFileVersion %>"></script>
        <script type="text/javascript">
            var versionId, dealerId = "<%= dealerId %>", cityId = "<%= cityId %>", clientIP = "<%= clientIP%>",campaignId = "<%= campaignId %>";                                              
             var serviceLat = "<%= serviceLat %>", serviceLong = "<%= serviceLong%>";
             var bodHt, footerHt, scrollPosition, leadSourceId;                         
             var googleMapAPIKey = "<%= Bikewale.Utility.BWConfiguration.Instance.GoogleMapApiKey%>";
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
