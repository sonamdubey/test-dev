<%@ Page Language="C#" AutoEventWireup="false" Inherits="Bikewale.Used.Default" EnableViewState="false"%>
<%@ Register Src="~/UI/controls/usedBikeModel.ascx" TagName="usedBikeModel" TagPrefix="BW" %>
<%@ Register Src="~/UI/controls/usedBikeInCities.ascx" TagName="usedBikeInCities" TagPrefix="BW" %>
<!DOCTYPE html>

<html>
<head>
    <%
        title = "Used Bikes in India | Buy and Sell Used Bikes- BikeWale";
        description = "BikeWale is the India's largest portal for selling and buying used bikes with more than 10000+ listings. Buy and Sell your second-hand bikes online for FREE!";
        keywords = "Used bikes, used bike, used bikes for sale, second hand bikes, buy used bike";
        canonical = "https://www.bikewale.com/used/";
        alternate = "https://www.bikewale.com/m/used/";
        isHeaderFix = false;
        isAd970x90Shown = false;
        isTransparentHeader = true; 
    %>
    <!-- #include file="/UI/includes/headscript_desktop_min.aspx" -->

    <style type="text/css">
        @charset "utf-8";#used-landing-box .welcome-box{margin-top:85px}.used-landing-banner{background:url(https://imgd1.aeplcdn.com/0x0/bw/static/landing-banners/d/used-landing-banner.jpg?v1=06Oct2016) center no-repeat #53442a;height:268px;padding-top:1px}.section-container{margin-bottom:25px}h2.section-header{font-size:18px;text-align:center;margin-bottom:20px}.padding-25-30{padding:25px 30px}.negative-50{margin-top:-50px}.usedbikes-search-container{margin:0 auto;width:570px}#search-form-city{width:334px}#search-form-budget{width:233px}#search-form-budget,#search-form-city{height:44px;float:left}#search-form-city .chosen-container{border:1px solid #e2e2e2;border-radius:0;padding:12px 20px}.form-selection-box{width:100%;padding:12px 20px;border:1px solid #e2e2e2;border-left:0;position:relative;text-align:left;cursor:pointer;color:#666}#budget-list-box::after,#budget-list-box::before{border-left:10px solid transparent;border-right:10px solid transparent;content:"";left:46.5%;z-index:1}.pos-top18{top:18px}#budget-list-box{display:none;position:absolute;top:43px;left:-1px;z-index:1;width:234px;background:#fff;border:1px solid #e2e2e2;border-radius:0 0 2px 2px}.input-label-box{display:inline-block;margin:5px 0;padding:3px 8px;width:49%;font-size:14px;color:#82888b}#min-input-label.input-label-box{width:51%}#search-form-budget #min-input-label,#search-form-budget.max-active #max-input-label{color:#4d5057}#search-form-budget.max-active #min-input-label{color:#82888b}#max-budget-list{display:none}#search-form-budget li{display:block;padding:5px 20px;color:#82888b;cursor:pointer;background:0 0}#search-form-budget li:hover{background:#f5f5f5}#budget-list-box::before{border-bottom:10px solid #e2e2e2;position:absolute;top:-11px}#budget-list-box::after{border-bottom:10px solid #f5f5f5;position:absolute;top:-10px}#min-max-budget-box.open .fa-angle-down{-moz-transform:rotateZ(180deg);-webkit-transform:rotateZ(180deg);-o-transform:rotateZ(180deg);-ms-transform:rotateZ(180deg);transform:rotateZ(180deg)}#upDownArrow.fa-angle-down{transition:all .5s ease-in-out 0s;font-size:20px}#min-max-budget-box.open+#budget-list-box{display:block}.search-bikes-btn.btn-lg{margin-top:35px;padding:8px 64px}.text-underline{text-decoration:underline}.btn-inv-teal:focus,.btn-inv-teal:hover,.city-card-target:hover{text-decoration:none}.used-sprite{background:url(https://imgd2.aeplcdn.com/0x0/bw/static/sprites/d/used-sprite.png?v1=06Oct2016) no-repeat;display:inline-block}.buyer,.contact-buyer,.free-cost,.listing-time{width:36px;height:36px;margin-bottom:18px}.free-cost{background-position:0 0}.buyer{background-position:0 -41px}.listing-time{background-position:0 -82px}.contact-buyer{background-position:0 -123px}.profile-id-icon{width:36px;height:36px;background-position:0 -164px}#sell-btn.btn{padding:8px 77px}#profile-id-popup.bw-popup{display:none;width:480px;top:33%;padding:30px 50px;min-height:420px}.size-small .icon-outer-container{width:92px;height:92px}.size-small .icon-inner-container{width:84px;height:84px;margin:3px auto}input[type=text]:focus,input[type=number]:focus{outline:0;box-shadow:none}.input-box{height:60px;text-align:left}.input-box input{font-size:16px;width:100%;display:block;padding:7px 0;border-bottom:1px solid #82888b;font-weight:700;color:#4d5057}.input-box label,.input-number-prefix{font-size:16px;position:absolute;color:#82888b;top:4px}.input-box label{left:0;-webkit-transition:.2s ease all;-moz-transition:.2s ease all;-o-transition:.2s ease all;transition:.2s ease all}.input-number-box input{padding-left:30px}.input-number-prefix{display:none;font-weight:700}.boundary{position:relative;width:100%;display:block}.boundary:after,.boundary:before{content:'';position:absolute;bottom:0;width:0;height:2px;background-color:#41b4c4;-webkit-transition:.2s ease all;-moz-transition:.2s ease all;-o-transition:.2s ease all;transition:.2s ease all}.boundary:before{left:50%}.boundary:after{right:50%}.error-text{display:none;font-size:12px;position:relative;top:4px;left:0;color:#d9534f}.input-box.input-number-box input:focus~.input-number-prefix,.input-box.input-number-box.not-empty .input-number-prefix,.input-box.invalid .error-text{display:inline-block}.input-box input:focus~label,.input-box.not-empty label{top:-16px;font-size:12px}.input-box input:focus~.boundary:after,.input-box input:focus~.boundary:before{width:50%}.input-box.invalid .boundary:after,.input-box.invalid .boundary:before{background-color:#d9534f;width:50%}.brand-type-container li{display:inline-block;vertical-align:top;width:180px;height:85px;margin:0 5px 30px;text-align:center;font-size:18px;-moz-border-radius:2px;-webkit-border-radius:2px;-o-border-radius:2px;-ms-border-radius:2px;border-radius:2px}.model-details-label,.model-media-item{vertical-align:middle;display:inline-block}.brand-1,.brand-10,.brand-11,.brand-12,.brand-13,.brand-14,.brand-15,.brand-16,.brand-17,.brand-18,.brand-19,.brand-2,.brand-20,.brand-22,.brand-23,.brand-24,.brand-3,.brand-34,.brand-37,.brand-38,.brand-39,.brand-4,.brand-40,.brand-41,.brand-42,.brand-5,.brand-6,.brand-7,.brand-71,.brand-8,.brand-80,.brand-81,.brand-9,.brand-72,.brand-73,.brand-23,.brand-type,.brand-74,.brand-75,.brand-76,.brand-77,.brand-79,.brand-82{height:50px}.brand-type{width:180px;display:block;margin:0 auto}.brand-type-title{margin-top:10px;display:block}.brand-type-container a{text-decoration:none;color:#82888b;display:inline-block}.brand-type-container li:hover span.brand-type-title{color:#4d5057;font-weight:700}.brand-bottom-border{overflow:hidden}.brandlogosprite{background:url('https://imgd.aeplcdn.com/0x0/bw/static/sprites/d/brand-type-sprite.png?19Nov20181542599237756') no-repeat;display:inline-block}.brand-2{width:87px;background-position:0 0}.brand-7{width:56px;background-position:-96px 0}.brand-1{width:88px;background-position:-162px 0}.brand-8{width:100px;background-position:-260px 0}.brand-12{width:67px;background-position:-370px 0}.brand-40{width:125px;background-position:-447px 0}.brand-34{width:122px;background-position:-582px 0}.brand-22{width:121px;background-position:-714px 0}.brand-3{width:44px;background-position:-845px 0}.brand-17{width:86px;background-position:-899px 0}.brand-15{width:118px;background-position:-995px 0}.brand-4{width:43px;background-position:-1123px 0}.brand-9{width:99px;background-position:-1176px 0}.brand-16{width:117px;background-position:-1285px 0}.brand-5{width:59px;background-position:-1412px 0}.brand-19{width:122px;background-position:-1481px 0}.brand-13{width:122px;background-position:-1613px 0}.brand-6{width:63px;background-position:-1745px 0}.brand-10{width:102px;background-position:-1818px 0}.brand-14{width:127px;background-position:-1930px 0}.brand-39{width:89px;background-position:-2067px 0}.brand-20{width:82px;background-position:-2166px 0}.brand-11{width:121px;background-position:-2258px 0}.brand-41{width:63px;background-position:-2389px 0}.brand-42{width:64px;background-position:-2461px 0}.brand-80{width:44px;background-position:-4055px 0}.brand-81{width:66px;background-position:-4109px 0}.brand-71{width:39px;background-position:-2616px 0}.brand-37{width:119px;background-position:-2665px 0}.brand-18{width:63px;background-position:-2794px 0}.brand-23{width:125px;background-position: -3026px 0;}.brand-72{width: 122px;background-position:-3924px 0;}.brand-73{width: 60px;background-position:-3217px 0;}.brand-74{width: 110px;background-position:-3281px 0;}.brand-75{width: 122px;background-position:-3400px 0;}.brand-76{width:124px;background-position:-3530px 0;}.brand-77{width:122px;background-position:-3663px 0;}.brand-79{width:120px;background-position:-3795px 0;}.brand-82{width:70px;background-position:-4184px 0;}.city-card-target{display:block}.city-image-preview{width:292px;height:114px;display:block;margin-bottom:15px;text-align:center;padding-top:10px}#city-jcarousel .inner-content-carousel .jcarousel-control-left,#city-jcarousel .inner-content-carousel .jcarousel-control-right{top:32%}.city-sprite{background:url(https://imgd1.aeplcdn.com/0x0/bw/static/sprites/d/city-sprite.png?24062016) no-repeat;display:inline-block}.ahmedabad-icon,.c1-icon,.c10-icon,.c105-icon,.c12-icon,.c176-icon,.c2-icon,.chandigarh-icon,.kolkata-icon,.lucknow-icon{height:92px}.c1-icon{width:130px;background-position:0 0}.c12-icon{width:186px;background-position:-140px 0}.c2-icon{width:136px;background-position:-336px 0}.c10-icon{width:70px;background-position:-482px 0}.c176-icon{width:53px;background-position:-562px 0}.c105-icon{width:65px;background-position:-625px 0}.kolkata-icon{width:182px;background-position:-700px 0}.lucknow-icon{width:174px;background-position:-892px 0}.ahmedabad-icon,.chandigarh-icon{width:0;background-position:0 0}.btn-inv-teal{background:0 0;color:#3799a7;border:1px solid #3799a7}.btn-inv-teal:hover{color:#fff;background:#41b4c4;border-color:#41b4c4}.btn-inv-teal:focus{color:#fff;background:#3799a7}.inv-teal-sm{font-size:14px;padding:6px 19px}.teal-next{width:6px;height:10px;background-position:-82px -469px;margin-left:6px}#recent-uploads li{height:305px}.used-carousel .card-image-block,.used-carousel .model-jcarousel-image-preview{height:160px;background-color:#f5f5f5;line-height:0}.used-carousel .card-desc-block{padding-top:15px}.model-media-details{position:absolute;right:10px;bottom:10px;font-size:12px}.model-media-item{padding:4px 5px;color:#4d5057;background:rgba(255,255,255,.8);border-radius:2px}.gallery-photo-icon{width:16px;height:12px;background-position:-213px -207px}.model-media-count{position:relative;top:-1px}.model-details-label{width:84%;color:#82888b;text-align:left;overflow:hidden;text-overflow:ellipsis;white-space:nowrap}.author-grey-sm-icon,.kms-driven-icon,.model-date-icon,.model-loc-icon{width:10px;height:12px;margin-right:5px;vertical-align:middle}.model-date-icon{background-position:-65px -543px}.kms-driven-icon{background-position:-65px -563px}.author-grey-sm-icon{height:10px;margin-right:4px}.model-loc-icon{background-position:-82px -543px}@media only screen and (max-width:1024px){.brand-type,.brand-type-container li{width:170px}}
    </style>

    <script type="text/javascript">
        <!-- #include file="\UI\includes\gacode_desktop.aspx" -->
    </script>
</head>
<body class="bg-light-grey page-type-landing">
    <form id="form1" runat="server">
        <!-- #include file="/UI/includes/headBW.aspx" -->
        <header class="used-landing-banner">
            <div id="used-landing-box" class="container">
                <div class="welcome-box">
                    <h1 class="font30 text-uppercase margin-bottom30">Used bikes</h1>
                    <h2 class="font20 text-unbold text-white">View wide range of used bikes</h2>
                </div>
            </div>
        </header>     
        <section>
            <div class="container section-container">
                   <div id="search-used-bikes" class="grid-12">
                    <div class="content-box-shadow negative-50 text-center padding-25-30">
                        <h2 class="section-header">Search used bikes</h2>
                        <div class="usedbikes-search-container">
                            <div id="search-form-city" class="form-control-box">
                                 <select class="form-control chosen-select" id="drpCities">
                                    <option >Select a city</option>
                                    <% foreach(var city in viewModel.Cities){ %> 
                                    <option id="selectedCity" value="<%=city.CityId%>" data-item-id="<%=city.CityId%>" data-citymaskingname="<%=city.CityMaskingName%>"><%=city.CityName %></option>
                                 <%} %>
                                </select>
                            </div>
                            <div id="search-form-budget" class="form-control-box">
                                <div id="min-max-budget-box" class="form-selection-box">
                                    <span id="budget-default-label">Select budget</span>
                                    <span id="min-amount"></span>
                                    <span id="max-amount"></span>
                                    <span id="upDownArrow" class="fa fa-angle-down position-abt pos-top18 pos-right20"></span>
                                    <div class="clear"></div>
                                </div>
                                <div id="budget-list-box">
                                    <div id="user-budget-input" class="bg-light-grey text-light-grey">
                                        <div id="min-input-label" class="input-label-box border-solid-right">Min</div><div id="max-input-label" class="input-label-box">Max</div>
                                    </div>
                                    <ul id="min-budget-list" class="text-left"></ul>
                                    <ul id="max-budget-list" class="text-right"></ul>
                                </div>
                            </div>
                            <div class="clear"></div>
                            <a data-bind="attr: { href: redirectUrl }"  id="searchCityBudget" class="btn btn-orange btn-lg search-bikes-btn margin-bottom20" >Search</a>
                            <div>
                                <a href="javascript:void(0)" id="profile-id-popup-target" class="font14 text-underline" rel="nofollow">Search by Profile ID</a>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="clear"></div>
            </div>
        </section>

        <section>
            <div class="container section-container">
                <div class="grid-12">
                    <h2 class="section-header">Best way to sell your bike</h2>
                    <div class="content-box-shadow text-center padding-top25 padding-bottom25 padding-right20 padding-left20 font14">
                        <div class="margin-bottom20">
                            <div class="grid-3">
                                <span class="used-sprite free-cost"></span>
                                <p class="text-bold margin-bottom5">Free of cost</p>
                                <p>You can upload your bike ad absolutely free</p>
                            </div>
                            <div class="grid-3">
                                <span class="used-sprite buyer"></span>
                                <p class="text-bold margin-bottom5">Genuine buyers </p>
                                <p>We let only verified buyers to get in touch with you</p>
                            </div>
                            <div class="grid-3">
                                <span class="used-sprite listing-time"></span>
                                <p class="text-bold margin-bottom5">Unlimited listing duration</p>
                                <p>Your bike listing will stay active until it is sold</p>
                            </div>
                            <div class="grid-3">
                                <span class="used-sprite contact-buyer"></span>
                                <p class="text-bold margin-bottom5">Get buyer details</p>
                                <p>We will send you the details of buyers thorugh SMS and mail</p>
                            </div>
                            <div class="clear"></div>
                        </div>
                        <a href="/used/sell/" title="Sell your bike" id="sell-btn" class="btn btn-teal">Sell</a>
                    </div>
                </div>
                <div class="clear"></div>
            </div>
        </section>
     
           <% if( viewModel.TopMakeList!= null && viewModel.OtherMakeList!= null){  %>
        <section>
            <div class="container section-container">
                <div class="grid-12">
                    <h2 class="section-header">Search used bikes by brands</h2>
                    <div class="content-box-shadow padding-top20 collapsible-brand-content">
                        <div id="brand-type-container" class="brand-type-container">
                            <ul class="text-center">
                                  <% foreach(var bike in viewModel.TopMakeList){ %>  
                                <li>
                                    <a href="<%= bike.Link %>" title="Used <%=bike.MakeName %> bikes">
                                        <span class="brand-type">
                                            <span class="brandlogosprite brand-<%=bike.MakeId %>"></span>
                                        </span>
                                        <span class="brand-type-title"><%=bike.MakeName %></span>
                                    </a>
                                </li>                                    
                                  <% } %>
                            </ul>
                            <ul class="brand-style-moreBtn padding-top25 brandTypeMore hide margin-left5">
                                <% foreach(var bike in viewModel.OtherMakeList){ %> 
                                <li>
                                     <a href="<%= bike.Link %>" title="Used <%=bike.MakeName %> bikes">
                                        <span class="brand-type">
                                            <span class="brandlogosprite brand-<%=bike.MakeId %>"></span>
                                        </span>
                                        <span class="brand-type-title"><%=bike.MakeName %></span>
                                    </a>
                                </li>                                    
                                <%} %>
                            </ul>
                        </div>
                        <div class="view-all-btn-container padding-bottom25">
                            <a href="javascript:void(0)" class="view-brandType btn view-all-target-btn rotate-arrow" rel="nofollow"><span class="btn-label">View more brands</span><span class="bwsprite teal-right"></span></a>
                        </div>
                    </div>
                </div>
                <div class="clear"></div>
            </div>
        </section>
        <%} %>
       
    <%if (ctrlusedBikeInCities.objCitiesWithCount != null && ctrlusedBikeInCities.objCitiesWithCount.Count()>0){ %>
          
                      <section>
            <div class="container text-center section-container">
                <h2 class="section-header">Search used bikes by cities</h2>
                <div class="grid-12">
                    <BW:usedBikeInCities runat="server" ID="ctrlusedBikeInCities" />  
              </div>
                 <div class="clear"></div>
                </div>
                          
                          </section>
                    <%} %>
                     
                <% if (ctrlusedBikeModel.FetchCount>0)
                       { %>
         <section>
            <div class="container section-container">
                <h2 class="section-header">Popular used bikes</h2>
                 <div class="grid-12 content-box-shadow padding-bottom15 padding-top20">
                    <BW:usedBikeModel runat="server" ID="ctrlusedBikeModel" />
           </div>   
                  <div class="clear"></div>  
              </div>   
         
              </section>   
                    <% } %> 
  

        <!-- profile-id -->
        <div id="profile-id-popup" class="bw-popup text-center size-small">
            <div class="bwsprite cross-lg-lgt-grey close-btn position-abt pos-top10 pos-right10 cur-pointer"></div>
            <div class="icon-outer-container rounded-corner50">
                <div class="icon-inner-container rounded-corner50">
                    <span class="used-sprite profile-id-icon margin-top25"></span>
                </div>
            </div>
            <p class="font18 text-bold margin-top10 margin-bottom10">Search by Profile ID</p>
            <p class="font14 text-light-grey margin-bottom30">If you like a certain listing you can search it by its Profile ID. The unique Profile ID is mentioned in the Ad details.</p>
            <div class="input-box form-control-box margin-bottom15">
                <input type="text" id="listingProfileId">
                <label for="listingProfileId">Profile ID</label>
                <span class="boundary"></span>
                <span class="error-text"></span>
            </div>
            <a class="btn btn-orange btn-fixed-width" id="search-profile-id-btn">Search</a>
        </div>

        <script type="text/javascript" src="<%= staticUrl  %>/UI/src/frameworks.js?<%=staticFileVersion %>"></script>

        <!-- #include file="/UI/includes/footerBW.aspx" -->
        <link href="<%= staticUrl  %>/UI/css/bw-common-btf.css?<%=staticFileVersion %>" rel="stylesheet" type="text/css" />
        <!-- #include file="/UI/includes/footerscript.aspx" -->
        <script type="text/javascript" src="<%= staticUrl  %>/UI/src/used-landing.js?<%= staticFileVersion %>"></script>
        <script type="text/javascript" >  
            var CustomerId = "<%=currentUser%>";
            var gaObj = { 'id': '<%= (int)Bikewale.Entities.Pages.GAPages.Used_Bike_Landing%>', 'name': '<%= Bikewale.Entities.Pages.GAPages.Used_Bike_Landing%>' };
            </script>

        <!-- #include file="/UI/includes/fontBW.aspx" -->
    </form>
</body>
</html>
