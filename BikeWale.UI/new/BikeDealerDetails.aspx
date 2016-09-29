<%@ Page Language="C#" Inherits="Bikewale.New.BikeDealerDetails" AutoEventWireup="false" EnableViewState="false" %>
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
    <style type="text/css">
        .dropdown-menu,.input-box input{border-bottom:1px solid #82888b}.padding-14-20{padding:14px 20px}.padding-top50{padding-top:50px}.featured-tag{width:74px;text-align:center;line-height:20px;background:#3799a7;z-index:1;font-weight:400;font-size:12px;color:#fff;border-radius:2px}.vertical-top{display:inline-block;vertical-align:top}.dealership-card-details{width:92%}#get-direction-button{width:50px;height:50px;background:#f9f9f9;position:absolute;right:20px;bottom:20px;-webkit-border-radius:50%;-moz-border-radius:50%;-o-border-radius:50%;border-radius:50%;text-align:center;cursor:pointer;-webkit-box-shadow:1px 5px 15px #aaa;-moz-box-shadow:1px 5px 15px #aaa;-o-box-shadow:1px 5px 15px #aaa;box-shadow:1px 5px 15px #aaa}#get-direction-button:hover{-webkit-box-shadow:1px 5px 15px #999;-moz-box-shadow:1px 5px 15px #999;-o-box-shadow:1px 5px 15px #999;box-shadow:1px 5px 15px #999}#commute-distance-form .form-control{width:225px;padding-right:35px}.location-details{display:none}#submitAssistanceFormBtn.btn{padding:8px 56px}input{font-family:'Open Sans',sans-serif,Arial}input[type=text]:focus,input[type=email]:focus,input[type=tel]:focus{outline:0;box-shadow:none}.input-box{height:60px;text-align:left;display:inline-block;vertical-align:top}.input-box input{width:100%;display:block;padding:6px 0;font-size:16px;font-weight:700;color:#4d5057}.input-box label,.input-number-prefix{font-size:16px;position:absolute;color:#82888b}.input-box label{top:5px;left:0;pointer-events:none;-webkit-transition:.2s ease all;-moz-transition:.2s ease all;-o-transition:.2s ease all;transition:.2s ease all}.input-number-box input{padding-left:30px}.input-number-prefix{display:none;top:6px;font-weight:700}.boundary{position:relative;width:100%;display:block}.boundary:after,.boundary:before{content:'';position:absolute;bottom:0;width:0;height:2px;background-color:#41b4c4;-webkit-transition:.2s ease all;-moz-transition:.2s ease all;-o-transition:.2s ease all;transition:.2s ease all}.boundary:before{left:50%}.boundary:after{right:50%}.error-text{display:none;font-size:12px;position:relative;top:0;left:0;color:#d9534f}.dropdown-select-wrapper.invalid .error-text,.input-box.input-number-box input:focus~.input-number-prefix,.input-box.input-number-box.not-empty .input-number-prefix,.input-box.invalid .error-text{display:inline-block}.input-box input:focus~label,.input-box.not-empty label{top:-14px;font-size:12px}.input-box input:focus~.boundary:after,.input-box input:focus~.boundary:before{width:50%}.dropdown-select-wrapper.invalid .boundary:after,.dropdown-select-wrapper.invalid .boundary:before,.input-box.invalid .boundary:after,.input-box.invalid .boundary:before{background-color:#d9534f;width:50%}.type-user-details.form-control-box{width:292px}.type-dropdown{width:462px}.type-dropdown,.type-sumit-button{display:inline-block;vertical-align:bottom;height:60px}.type-sumit-button{margin-left:35px}.dropdown-select{display:none}.dropdown-menu{width:100%;min-width:125px;font-size:16px;position:relative;display:inline-block;vertical-align:middle;color:#4d5057}.dropdown-menu .dropdown-label,.dropdown-menu .dropdown-selected-item{width:100%;font-weight:700;background:url(http://imgd1.aeplcdn.com/0x0/bw/static/sprites/m/dropdown-icon.png) 100% no-repeat;white-space:nowrap;overflow:hidden;text-overflow:ellipsis}.dropdown-menu .dropdown-selected-item{background-position:98%;border-bottom:1px solid #e2e2e2;padding:8px 30px 8px 10px}.dropdown-menu .dropdown-label{cursor:pointer;display:inline-block;z-index:0}.dropdown-menu .dropdown-list-wrapper{display:none;width:100%;overflow:hidden;position:absolute;top:3px;left:0;background:#fff;z-index:1;-webkit-border-radius:2px;-moz-border-radius:2px;-o-border-radius:2px;-ms-border-radius:2px;border-radius:2px;border:1px solid #e2e2e2\9;-webkit-box-shadow:-3px 3px 15px 1px #ddd;-moz-box-shadow:-3px 3px 15px 1px #ddd;-ms-box-shadow:-3px 3px 15px 1px #ddd;-o-box-shadow:-3px 3px 15px 1px #ddd;box-shadow:-3px 3px 15px 1px #ddd}#modelsAvailable li,.dropdown-menu.dropdown-active .dropdown-list-wrapper{display:inline-block}.dropdown-menu .dropdown-menu-list{padding-top:10px;padding-bottom:10px}.dropdown-menu .dropdown-menu-list li{padding:5px 10px;cursor:pointer;white-space:nowrap}.dropdown-menu .dropdown-menu-list li[data-option-value=""]:hover{background:0 0;cursor:default}.dropdown-menu .dropdown-with-select li:hover{background:#eee}#modelsAvailable li{width:290px;min-height:260px;margin:0 15px 25px 14px;vertical-align:top}.image-block{width:290px;height:163px;line-height:0}.image-block div{background:url(http://imgd4.aeplcdn.com/0x0/bw/static/sprites/d/loader.gif) center center no-repeat}.image-block img{max-width:100%;height:163px}.details-block{padding:12px 20px 10px}.text-truncate{width:100%;text-align:left;text-overflow:ellipsis;white-space:nowrap;overflow:hidden}.dealership-loc-icon,.phone-black-icon{width:10px;position:relative;margin-right:5px}.dealership-loc-icon{height:12px;background-position:-52px -469px;top:4px}.phone-black-icon{height:10px;background-position:-73px -444px;top:5px}.star-white{width:8px;height:8px;background-position:-222px -107px;margin-right:4px}.clock-icon,.mail-grey-icon{width:12px;margin-right:5px;position:relative}.mail-grey-icon{height:10px;background-position:-92px -446px;top:5px}.clock-icon{height:12px;background-position:-213px -224px;top:4px}.get-direction-icon{width:20px;height:20px;background-position:-112px -441px;margin-top:15px}.crosshair-icon{width:20px;height:20px;background:url(http://imgd1.aeplcdn.com/0x0/bw/static/sprites/d/detect-location-icon.png) no-repeat}.inr-md-lg{width:12px;height:17px;background-position:-64px -515px;position:relative;top:1px}.gm-style-cc{display:none}@media only screen and (max-width:1024px){.location-details{font-size:13px}#dealer-map{width:365px!important}#modelsAvailable li{margin:0 8px 25px}.type-user-details.form-control-box{width:275px}}
    </style>
    <script src="http://maps.googleapis.com/maps/api/js?key=<%= Bikewale.Utility.BWConfiguration.Instance.GoogleMapApiKey %>&libraries=places"></script>
    <script type="text/javascript">
        <!-- #include file="\includes\gacode_desktop.aspx" -->
           </script>
</head>
<body class="bg-light-grey padding-top50">
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
                                <span class="bwsprite fa-angle-right margin-right10"></span><a itemprop="url" href="/new/"><span itemprop="title">New Bikes</span></a>
                            </li>
                            <li itemscope="" itemtype="http://data-vocabulary.org/Breadcrumb">
                                <span class="bwsprite fa-angle-right margin-right10"></span><a itemprop="url" href="/dealer-showroom-locator/"><span itemprop="title">Dealer Showroom Locator</span></a>
                            </li>
                            <li itemscope="" itemtype="http://data-vocabulary.org/Breadcrumb">
                                <span class="bwsprite fa-angle-right margin-right10"></span><a itemprop="url" href="/<%=makeMaskingName %>-dealer-showrooms-in-india/"><span itemprop="title"><%=makeName%> Dealer showrooms</span></a>
                            </li>
                             <li itemscope="" itemtype="http://data-vocabulary.org/Breadcrumb">
                                <span class="bwsprite fa-angle-right margin-right10"></span><a itemprop="url" href="/<%=makeMaskingName %>-dealer-showrooms-in-<%=cityMaskingName %>/"><span itemprop="title"><%=makeName%> Dealer Showroom in <%=cityName%></span></a>
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
            <div class="container margin-bottom20" id="dealerInfo">
                <div class="grid-12">
                    <div class="content-box-shadow">
                        <div class="content-box-shadow padding-14-20">
                            <h1><%=dealerName %></h1>
                        </div>
                        <div class="content-inner-block-20">
                            <div class="grid-7 alpha omega font14">
                                <%if(dealerObj!=null){ %>
                                
                                <div class="margin-bottom10">
                                    <span class="featured-tag inline-block margin-right10"><span class="bwsprite star-white"></span>Featured</span>
                                    <h2 class="font14 text-black text-bold inline-block">Authorized <%=makeName %> dealer in <%=cityName %></h2>
                                </div>
                            
                                <% if (!string.IsNullOrEmpty(address))
                                   {%>
                                <div class="margin-bottom10">
                                    <span class="bwsprite dealership-loc-icon vertical-top"></span>
                                    <span class="vertical-top text-light-grey dealership-card-details"><%=address %></span>
                                </div>
                                <%} %>
                                <% if (!string.IsNullOrEmpty(maskingNumber))
                                   {%>
                                <div class="margin-bottom10">
                                    <span class="bwsprite phone-black-icon vertical-top"></span>
                                    <span class="vertical-top text-bold dealership-card-details"><%=maskingNumber %></span>
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
                                <div id="commute-distance-form" class="margin-top20">
                                    <p class="text-bold margin-bottom15">Get commute distance and time:</p>
                                    <div class="leftfloat form-control-box margin-right15">
                                        <input id="locationSearch" type="text" class="form-control" placeholder="Enter your location" />
                                        <span id="getUserLocation" class="crosshair-icon font12 position-abt pos-right10 pos-top10 cur-pointer"></span>
                                    </div>
                                    <div class="location-details padding-top10 padding-bottom10 leftfloat">
                                        Distance: <span id="commuteDistance" class="margin-right10"></span>
                                        Time: <span id="commuteDuration"></span>
                                    </div>
                                    <div class="clear"></div>
                                    <div id="commuteResults"></div>
                                </div>
                            </div>
                            <div class="grid-5 omega position-rel">
                                <div id="dealer-map" style="width:378px;height:254px;border:1px solid #eee;"></div>
                                <div id="get-direction-button" title="Get directions">
                                    <span class="bwsprite get-direction-icon"></span>
                                </div>
                            </div>
                            <div class="clear"></div>
                        </div>
                    </div>
                </div>
                <div class="clear"></div>
            </div>
        </section>
       <%if (dealerObj!=null && dealerObj.CampaignId > 0)
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

<input type="button" data-isregisterpq="true" data-item-name="<%=dealerObj.Name %>" data-item-area="<%=dealerObj.Area%>" data-leadsourceid="15" class="btn btn-orange margin-bottom5 " data-isleadpopup="false" data-pqsourceid="<%= (int) Bikewale.Entities.PriceQuote.PQSourceEnum.Desktop_DealerLocator_SubmitButton %>" 
    data-item-id="<%= dealerId %>" data-bind="click: function (d, e) { validateBikeData(); HiddenSubmitLead(d, e) }" value="Get offers" />

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
                                    <div>
                                        <span class="bwsprite inr-md-lg"></span>
                                        <span class="font22 text-bold"><%=Bikewale.Common.CommonOpn.FormatPrice(model.VersionPrice.ToString()) %></span><span class="font14 text-light-grey">&nbsp;onwards</span>
                                    </div>
                                </div>
                            </li>
                            <%} %>
                          </ul>
                    </div>
                 </div>
                <div class="clear"></div>
            </div>
            
        </section>
        <%} %>

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
                
        <script type="text/javascript" src="<%= staticUrl != "" ? "http://st1.aeplcdn.com" + staticUrl : "" %>/src/frameworks.js?<%=staticFileVersion %>"></script>
        <!-- #include file="/includes/footerBW.aspx" -->
        <script type="text/javascript">
            var versionId = "";
            $("#getLeadBike").change(function () {
                var val = $(this).val();
                if( val && val!="" && val!="0" )
                {
                    versionId = val;
                }
            });
 
            var dealerLat = '<%=dealerObj.Area.Latitude%>';
            var dealerLng = '<%=dealerObj.Area.Longitude%>';
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
        <script type="text/javascript" src="<%= staticUrl != "" ? "http://st2.aeplcdn.com" + staticUrl : "" %>/src/dealer/details.js?<%= staticFileVersion %>"></script>
        <link href='https://fonts.googleapis.com/css?family=Open+Sans:400,700' rel='stylesheet' type='text/css' />

        <!--[if lt IE 9]>
            <script src="<%= staticUrl != "" ? "http://st2.aeplcdn.com" + staticUrl : "" %>/src/html5.js"></script>
        <![endif]-->
    </form>
</body>
</html>
