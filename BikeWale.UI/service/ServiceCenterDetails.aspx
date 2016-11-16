<%@ Page Language="C#" Inherits="Bikewale.Service.ServiceCenterDetails" AutoEventWireup="false" EnableViewState="false" %>
<%@ Register Src="~/controls/DealerCard.ascx" TagName="DealerCard" TagPrefix="BW" %>
<%@ Register Src="~/controls/LeadCaptureControl.ascx" TagName="LeadCapture" TagPrefix="BW" %>
<!DOCTYPE html>
<html>
<head>
        <%
        keywords = String.Format("{0}, {0} dealer, {0} Showroom, {0} {1}", dealerName, cityName);
        description = String.Format("{2} is dealer of {0} bikes in {1}. Get best offers on {0} bikes at {2} showroom", makeName, cityName,dealerName);
        title = String.Format("{0} {1} - {0} Showroom in {1} - BikeWale", dealerName, cityName);
        canonical = String.Format("http://www.bikewale.com/{0}-dealer-showrooms-in-{1}/{2}-{3}/", makeMaskingName, cityMaskingName,dealerId, dealerMaskingName);
        alternate = String.Format("http://www.bikewale.com/m/{0}-dealer-showrooms-in-{1}/{2}-{3}/", makeMaskingName, cityMaskingName,dealerId, dealerMaskingName);
        AdId = "1395986297721";
        AdPath = "/1017752/BikeWale_New_";
        isAd970x90Shown = true;
        isAd970x90BottomShown = true;
        isAd300x250Shown = false;
        isAd300x250BTFShown = false;
    %>
    <!-- #include file="/includes/headscript_desktop_min.aspx" -->
    <link href="/css/service/details.css" rel="stylesheet" type="text/css" />
    <script src="http://maps.googleapis.com/maps/api/js?key=<%= Bikewale.Utility.BWConfiguration.Instance.GoogleMapApiKey %>&libraries=places"></script>
    <script type="text/javascript">
        <!-- #include file="\includes\gacode_desktop.aspx" -->
    </script>
</head>
<body class="bg-light-grey padding-top50">
    <form id="Form1" runat="server">
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
                                <span class="bwsprite fa-angle-right margin-right10"></span><a itemprop="url" href=""><span itemprop="title"><%=makeName%> Service Centers</span></a>
                            </li>
                             <li itemscope="" itemtype="http://data-vocabulary.org/Breadcrumb">
                                <span class="bwsprite fa-angle-right margin-right10"></span><a itemprop="url" href=""><span itemprop="title"><%=makeName%> Service Center in <%=cityName%></span></a>
                            </li>
                            <li class="current"><span class="bwsprite fa-angle-right margin-right10"></span><%=dealerName %></li>
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
                        <h1 class="section-header"><%=dealerName %></h1>
                        <div class="section-inner-padding">
                            <div class="grid-7 alpha omega font14">
                                <%if(dealerObj!=null){ %>
                                
                                <h2 class="font14 margin-bottom10">Authorized <%=makeName %> service center in <%=cityName %></h2>
                            
                                <% if (!string.IsNullOrEmpty(address))
                                   {%>
                                <div class="margin-bottom10">
                                    <span class="bwsprite dealership-loc-icon vertical-top"></span>
                                    <span class="vertical-top text-light-grey details-column"><%=address %></span>
                                </div>
                                <%} %>
                                <% if (!string.IsNullOrEmpty(maskingNumber))
                                   {%>
                                <div class="margin-bottom10">
                                    <span class="bwsprite phone-black-icon vertical-top"></span>
                                    <span class="vertical-top text-bold details-column"><%=maskingNumber %></span>
                                </div>
                                <%} %>
                                <% if (!string.IsNullOrEmpty(eMail))
                                   {%>
                                <div class="margin-bottom10">
                                    <span class="bwsprite mail-grey-icon vertical-top"></span>
                                    <a href="mailto:<%=eMail %>" target="_blank" class="vertical-top text-light-grey" rel="nofollow">
                                        <span class="dealership-card-details"><%=eMail %></span>
                                    </a>
                                </div>
                                <%} %>
                                <% if (!string.IsNullOrEmpty(workingHours))
                                   {%>
                                <div class="margin-bottom10">
                                    <span class="bwsprite clock-icon vertical-top"></span>
                                    <span class="vertical-top text-light-grey dealership-card-details">Working hours: <%=workingHours %></span>
                                </div>
                                <%} %>
                                <%} %>
                                <% if (dealerObj != null && dealerObj.Area != null && dealerObj.Area.Latitude > 0 && dealerObj.Area.Longitude >0)
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
                                <% if (dealerObj != null && dealerObj.Area != null && dealerObj.Area.Latitude > 0 && dealerObj.Area.Longitude >0)
                                   { %>
                                <div id="dealer-map" style="width:378px;height:254px;border:1px solid #eee;"></div>
                                <div id="get-direction-button" title="Get directions">
                                    <a href="https://maps.google.com/?saddr=&amp;daddr=<%=dealerObj.Area.Latitude %>,<%=dealerObj.Area.Longitude %>" target="_blank">
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

        <section>
            <div class="container section-bottom-margin">
                <div class="grid-12">
                    <div class="content-box-shadow padding-bottom20">
                        <h2 class="section-h2-title padding-15-20-20">Other Bajaj service centers in Mumbai</h2>
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
                            <a href="">View all Bajaj service centers<span class="bwsprite blue-right-arrow-icon"></span></a>
                        </div>
                    </div>
                </div>
                <div class="clear"></div>
            </div>
        </section>

        <section>
            <div id="service-scheduler" class="container section-bottom-margin">
                <div class="grid-12">
                    <div class="content-box-shadow padding-15-20-20">
                        <h2 class="section-h2-title margin-bottom10">Is your bajaj bike due for a service?</h2>
                        <p class="font14 margin-bottom25">Get you Bajaj bike serviced with time period given below.</p>
                        <div id="scheduler-left-column" class="grid-4 alpha">
                            <div class="select-box">
                                <p class="font12 text-xt-light-grey">Model</p>
                                <select class="chosen-select" data-placeholder="Select model">
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
                            <img id="service-model-image" src="http://imgd1.aeplcdn.com//310x174//bw/models/honda-cb-shine-kick/drum/spokes-111.jpg?20151209184344" alt="Honda CB Shine" title="Honda CB Shine" />
                        </div>
                        <div class="grid-8 omega">
                            <table width="100%" border="0" cellspacing="0" cellpadding="0">
                                <thead>
                                    <tr>
                                        <th align="left" width="20%">Service no.</th>
                                        <th align="left" width="40%">Validity from the date of purchase</th>
                                        <th align="left" width="40%">Validity from the date of previous service</th>
                                    </tr>
                                </thead>
                                <tbody>
                                    <tr>
							            <td>1</td>
                                        <td>0-600 kms</td>
							            <td>60 days</td>
						            </tr>
                                    <tr>
							            <td>2</td>
                                        <td>500-1000 kms</td>
							            <td>100 days</td>
						            </tr>
                                    <tr>
							            <td>3</td>
                                        <td>1000-5000 kms</td>
							            <td>250 days</td>
						            </tr>
                                    <tr>
							            <td>4</td>
                                        <td>5000-10000 kms</td>
							            <td>500 days</td>
						            </tr>
                                </tbody>
                            </table>
                            <!-- no days -->
                            <table width="100%" border="0" cellspacing="0" cellpadding="0" class="hide">
                                <thead>
                                    <tr>
                                        <th align="left" width="30%">Service no.</th>
                                        <th align="left" width="70%">Validity from the date of previous service</th>
                                    </tr>
                                </thead>
                                <tbody>
                                    <tr>
							            <td>1</td>
                                        <td>0-600 kms</td>
						            </tr>
                                    <tr>
							            <td>2</td>
                                        <td>500-1000 kms</td>
						            </tr>
                                    <tr>
							            <td>3</td>
                                        <td>1000-5000 kms</td>
						            </tr>
                                    <tr>
							            <td>4</td>
                                        <td>5000-10000 kms</td>
						            </tr>
                                </tbody>
                            </table>
                            <!-- no kms -->
                            <table width="100%" border="0" cellspacing="0" cellpadding="0" class="hide">
                                <thead>
                                    <tr>
                                        <th align="left" width="30%">Service no.</th>
                                        <th align="left" width="70%">Validity from the date of previous service</th>
                                    </tr>
                                </thead>
                                <tbody>
                                    <tr>
							            <td>1</td>
							            <td>60 days</td>
						            </tr>
                                    <tr>
							            <td>2</td>
                                        <td>100 days</td>
						            </tr>
                                    <tr>
							            <td>3</td>
                                        <td>250 days</td>
						            </tr>
                                    <tr>
							            <td>4</td>
                                        <td>500 days</td>
						            </tr>
                                </tbody>
                            </table>
                            <!-- no service -->
                            <div id="service-not-avaiable" class="hide">
                                <span class="service-sprite calender-lg"></span>
                                <p class="font14 text-light-grey">Sorry! The service schedule for Bajaj Pulsar is not available.<br />Please check out the service schedule for other Bajaj bikes.</p>
                            </div>
                        </div>
                        <div class="clear"></div>
                    </div>
                </div>
                <div class="clear"></div>
            </div>
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
                        <div class="padding-15-20-20">
                            <h2 class="section-h2-title margin-bottom10">Looking to buy a new Bajaj bike in Mumbai?</h2>
                            <p class="font14">Check out authorised Bajaj dealers in Mumbai</p>
                        </div>
                        <ul class="bw-horizontal-cards">
                            <li class="card">
                                <a href="" title="Executive Bajaj - Malad" class="card-target">
                                    <p class="text-black text-bold text-truncate margin-bottom5">Executive Bajaj - Malad</p>
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
                                    <p class="text-black text-bold text-truncate margin-bottom5">Kamala Landmarc Motorbikes - Malad</p>
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
                                    <p class="text-black text-bold text-truncate margin-bottom5">Executive Bajaj - Malad</p>
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
                        <a href="" class="margin-left20">View all Bajaj dealers<span class="bwsprite blue-right-arrow-icon"></span></a>
                    </div>
                </div>
                <div class="clear"></div>
            </div>
        </section>

        <section>
            <div class="container section-bottom-margin">
                <div class="grid-12 font12">
                    <p><span class="font14"><strong>Disclaimer</strong>:</span> The above-mentioned information about authorised Honda service centers is furnished to best of our knowledge. The facilities of pick and drop, timings and service schedule related information might vary slightly from service center to service center. Please check with the authorised Make service center before scheduling an appointment.</p>
                </div>
                <div class="clear"></div>
            </div>
        </section>

       <%if (dealerObj != null && dealerObj.CampaignId > 0 && (dealerObj.DealerType == (int)(Bikewale.Entities.PriceQuote.DealerPackageTypes.Premium) || dealerObj.DealerType == (int)(Bikewale.Entities.PriceQuote.DealerPackageTypes.Deluxe)))
         { %>
        <section id="dealerAssistance">
            <div class="container margin-bottom20" id="leadForm">
                <div class="grid-12">
                    <div id="buyingAssistance" class="content-box-shadow content-inner-block-20">
                        <h2 class="font18 margin-bottom20">Complete buying assistance from <%=dealerName %></h2>
                        <p class="font14 text-light-grey margin-bottom25">Get in touch with <%=dealerName %> for best offers, test rides, EMI options, exchange benefits and much more...</p>
                        <div class="input-box form-control-box type-user-details margin-right20">
                            <input type="text" id="assistGetName" data-bind="textInput: fullName"/>
                            <label for="assistGetName">Name</label>
                            <span class="boundary"></span>
                            <span class="error-text"></span>
                        </div>
                        <div class="input-box form-control-box type-user-details margin-right20">
                            <input type="email" id="assistGetEmail" data-bind="textInput: emailId"/>
                            <label for="assistGetEmail">Email</label>
                            <span class="boundary"></span>
                            <span class="error-text"></span>
                        </div>
                        <div class="input-box input-number-box form-control-box type-user-details">
                            <input type="tel" id="assistGetMobile" maxlength="10" data-bind="textInput: mobileNo" />
                            <label for="assistGetMobile">Mobile number</label>
                            <span class="input-number-prefix">+91</span>
                            <span class="boundary"></span>
                            <span class="error-text"></span>
                        </div>
                        <div class="type-dropdown margin-bottom5">
                            <p class="font12 text-light-grey">Bike</p>
                            <div class="dropdown-select-wrapper">
                                <select id="getLeadBike" class="dropdown-select form-control chosen-select">
                                    <option value="">Select a bike</option>
                                    <%foreach(var model in dealerDetails.Models){ %>
                                    <option  value="<%=model.objVersion.VersionId %>"><%=model.BikeName %></option>
                                    <%} %>
                                </select>
                                <span class="boundary"></span>
                                <span class="error-text"></span>
                            </div>
                        </div>
                        <div class="type-sumit-button">
<input type="button" data-isregisterpq="true" data-item-name="<%=dealerObj.Name %>" data-item-area="<%=dealerObj.Area%>" data-leadsourceid="14" class="btn btn-orange margin-bottom5 " data-isleadpopup="false" data-pqsourceid="<%= (int) Bikewale.Entities.PriceQuote.PQSourceEnum.Desktop_dealer_details_Get_offers %>" 
    data-item-id="<%= dealerId %>" data-bind="click: function (d, e) { validateBikeData(); validateUserLeadDetails(); HiddenSubmitLead(d, e) }" value="Get offers" />
                  </div>
                    </div>
                    <div id="dealer-assist-msg" class="hide">
                                <p class="leftfloat font14">Thank you for your interest. <span data-bind="text: dealerName()"></span>&nbsp;will get in touch shortly</p>
                                <span id="assistance-response-close-btn" class="rightfloat bwsprite cross-lg-lgt-grey cur-pointer"></span>
                                <div class="clear"></div>
                            </div>
                </div>
                <div class="clear"></div>
            </div>
        </section>
        <section>
            <div class="container margin-bottom20">
                <div class="grid-12">
                    <div class="content-box-shadow padding-left5 padding-right5">
                        <h2 class="font18 margin-bottom20 padding-top20 padding-left15">Models available at <%=dealerName%></h2>
                        <ul id="modelsAvailable">
                            <%foreach(var model in dealerDetails.Models){ %>
                             <li>
                                <a  href='<%= Bikewale.Utility.UrlFormatter.BikePageUrl(model.MakeMaskingName , model.objModel.MaskingName) %>'>
                                    <div class="image-block">
                                        <div>
                                            <img class="lazy" data-original="<%= Bikewale.Utility.Image.GetPathToShowImages(model.OriginalImagePath, model.HostURL, Bikewale.Utility.ImageSize._476x268)%>" alt="" title="<%=model.objMake.MakeName %> <%=model.objModel.ModelName %>" src="" />
                                        </div>
                                    </div>
                                    <div class="details-block">
                                        <h3 class="text-black text-truncate margin-bottom10" title="<%=model.objMake.MakeName %> <%=model.objModel.ModelName %>"><%=model.objMake.MakeName %> <%=model.objModel.ModelName %></h3>
                                        <div class="font14 text-xt-light-grey margin-bottom5">
                                        <%= Bikewale.Utility.FormatMinSpecs.GetMinSpecs(model.Specs.Displacement.ToString(),model.Specs.FuelEfficiencyOverall.ToString(),model.Specs.MaxPower.ToString(),model.Specs.KerbWeight.ToString())%>
                                      </div>
                                        <div class="text-default">
                                            <span class="bwsprite inr-md-lg"></span>
                                            <span class="font22 text-bold"><%=Bikewale.Common.CommonOpn.FormatPrice(model.VersionPrice.ToString()) %></span><span class="font14 text-light-grey">&nbsp;onwards</span>
                                        </div>
                                    </div>
                                </a>
                            </li>
                            <%} %>
                          </ul>
                    </div>
                 </div>
                <div class="clear"></div>
            </div>
        </section>
        <%} %>
        <% if(ctrlDealerCard.showWidget){ %>
        <section>
            <div class="container margin-bottom20">
                <div class="grid-12">
                    <div class="content-box-shadow">
                          <BW:DealerCard runat="server" ID="ctrlDealerCard" />
                    </div>
                </div>
                <div class="clear"></div>
            </div>
        </section>
        <% } %>      
        <script type="text/javascript" src="<%= staticUrl != "" ? "http://st1.aeplcdn.com" + staticUrl : "" %>/src/frameworks.js?<%=staticFileVersion %>"></script>
        <!-- #include file="/includes/footerBW.aspx" -->
        <script type="text/javascript">
            var versionId = "";
            $("#getLeadBike").change(function () {
                var val = $(this).val();
                if (val && val != "" && val != "0") {
                    versionId = val;
                }
            });

            var dealerLat = '<%= dealerObj!= null && dealerObj.Area!= null ? dealerObj.Area.Latitude.ToString() : string.Empty %>';
            var dealerLong = '<%= dealerObj!= null && dealerObj.Area!= null ? dealerObj.Area.Longitude.ToString() : string.Empty %>';
            var $ddlModels = $("#assistGetModel");
            var pqId = null;
            var currentCityName = '<%=cityName%>';
            var bikeCityId = '<%=cityId%>';
            var googleMapAPIKey = "<%= Bikewale.Utility.BWConfiguration.Instance.GoogleMapApiKey %>";
            var pageUrl = window.location.href;
            var clientIP = '<%= clientIP %>';
            var leadSourceId, pqSourceId;
            var pageSrcId = eval("<%= Bikewale.Utility.BWConfiguration.Instance.SourceId %>");
            var googleMapAPIKey = "<%= Bikewale.Utility.BWConfiguration.Instance.GoogleMapApiKey %>";
            var makeName = "<%= makeName%>";

            $(document).on("change", $ddlModels, function () {
                hideError($ddlModels);
            });

            /* form validation */
            var validate = {
                setError: function (element, message) {
                    var elementLength = element.val().length;
                    errorTag = element.siblings('span.error-text');

                    errorTag.show().text(message);
                    if (!elementLength) {
                        element.closest('.input-box').removeClass('not-empty').addClass('invalid');
                    }
                    else {
                        element.closest('.input-box').addClass('not-empty invalid');
                    }
                },

                hideError: function (element) {
                    element.closest('.input-box').removeClass('invalid').addClass('not-empty');
                    element.siblings('span.error-text').text('');
                },

                onFocus: function (inputField) {
                    if (inputField.closest('.input-box').hasClass('invalid')) {
                        validate.hideError(inputField);
                    }
                },

                onBlur: function (inputField) {
                    var inputLength = inputField.val().length;
                    if (!inputLength) {
                        inputField.closest('.input-box').removeClass('not-empty');
                    }
                    else {
                        inputField.closest('.input-box').addClass('not-empty');
                    }
                },

                dropdown: {
                    setError: function (element, message) {
                        var dropdownWrapper = element.closest('.dropdown-select-wrapper'),
                            errorTag = dropdownWrapper.find('.error-text');

                        dropdownWrapper.addClass('invalid');
                        errorTag.show().text(message);
                    },

                    hideError: function (element) {
                        var dropdownWrapper = element.closest('.dropdown-select-wrapper'),
                            errorTag = dropdownWrapper.find('.error-text');

                        dropdownWrapper.removeClass('invalid');
                        errorTag.text('');
                    }
                }
            }

        </script>
        <BW:LeadCapture ID="ctrlLeadCapture" runat="server" />
        
        <link href="<%= staticUrl != "" ? "http://st2.aeplcdn.com" + staticUrl : "" %>/css/bw-common-btf.css?<%=staticFileVersion %>" rel="stylesheet" type="text/css" />
        <script type="text/javascript" src="<%= staticUrl != string.Empty ? "http://st2.aeplcdn.com" + staticUrl : string.Empty %>/src/common.min.js?<%= staticFileVersion %>"></script>
        <script type="text/javascript" src="<%= staticUrl != "" ? "http://st2.aeplcdn.com" + staticUrl : "" %>/src/service/details.js?<%= staticFileVersion %>"></script>
        <link href='https://fonts.googleapis.com/css?family=Open+Sans:400,700' rel='stylesheet' type='text/css' />

        <!--[if lt IE 9]>
            <script src="<%= staticUrl != "" ? "http://st2.aeplcdn.com" + staticUrl : "" %>/src/html5.js"></script>
        <![endif]-->
    </form>
</body>
</html>
