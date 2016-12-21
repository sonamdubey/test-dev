﻿<script language="c#" runat="server">	
    private string staticUrl = System.Configuration.ConfigurationManager.AppSettings["staticUrl"];
    private string staticFileVersion = System.Configuration.ConfigurationManager.AppSettings["staticFileVersion"];
    private string title = string.Empty, description = string.Empty, keywords = string.Empty, relPrevPageUrl = string.Empty, relNextPageUrl = string.Empty, AdId = string.Empty, AdPath = string.Empty, alternate = string.Empty, ShowTargeting = string.Empty, TargetedModel = string.Empty, TargetedSeries = string.Empty, TargetedMake = string.Empty, TargetedModels = string.Empty, canonical = string.Empty, TargetedCity = string.Empty
        , fbTitle = string.Empty, fbImage = string.Empty, ogImage = string.Empty;
    private ushort feedbackTypeId = 0;
    private bool isHeaderFix = true,
        isAd970x90Shown = true,
        isAd970x90BTFShown = false,
        isAd970x90BottomShown = true,
        isAd976x400FirstShown = false,
        isAd976x400SecondShown = false,
        isAd976x204 = false,
        isAd300x250BTFShown=true,
        isAd300x250Shown=true,
        isTransparentHeader = false,
        enableOG = true;
</script>

<title><%= title %></title>
<meta name="description" content="<%= description %>" />
<meta charset="utf-8"/>
<meta http-equiv="X-UA-Compatible" content="IE=edge"/>
<meta http-equiv="Content-Type" content="text/html; charset=utf-8" />    
<meta name="google-site-verification" content="fG4Dxtv_jDDSh1jFelfDaqJcyDHn7_TCJH3mbvq6xW8" />
<% if(!String.IsNullOrEmpty(keywords)) { %><meta name="keywords" content="<%= keywords %>" /><% } %>
<%if(!String.IsNullOrEmpty(alternate)) { %><meta name="alternate" content="<%= alternate %>" /><% } %>
<%if(!String.IsNullOrEmpty(canonical)) { %>
<link rel="canonical" href="<%=canonical %>" /> 
<% } %>
 <%if (!String.IsNullOrEmpty(relPrevPageUrl))
   { %>
<link rel="prev" href="<%= relPrevPageUrl %>" /><% } %>
 <%if(!String.IsNullOrEmpty(relNextPageUrl)){ %>
<link rel="next" href="<%= relNextPageUrl %>" /><% }%>
<%if(enableOG) { %>
<meta property="og:title" content="<%= title %>" />
<meta property="og:type" content="website" />
<meta property="og:description" content="<%= description %>" />
<%if(!String.IsNullOrEmpty(canonical)) { %><meta property="og:url" content="<%=canonical %>" /> <% } %>
<meta property="og:image" content = "<%= string.IsNullOrEmpty(ogImage) ? Bikewale.Utility.BWConfiguration.Instance.BikeWaleLogo : ogImage %>" />
<% } %>

<link rel="SHORTCUT ICON" href="https://imgd1.aeplcdn.com/0x0/bw/static/sprites/d/favicon.png"  type="image/png"/>

<style type="text/css">
    @charset "utf-8";body,html{-webkit-overflow-scrolling:touch}h1,h2{color:#2a2a2a;font-weight:600}body,h3{color:#4d5057}.form-control,article,aside,details,figcaption,figure,footer,header,hgroup,main,menu,nav,section,summary{display:block}.btn,.form-control,html{line-height:1.42857143}.btn,.btn-truncate{text-align:center;white-space:nowrap}.btn,.global-search{vertical-align:middle}.welcome-box h1,h1,h2{font-weight:600}html{height:100%;-webkit-text-size-adjust:100%;-ms-text-size-adjust:100%}body{font-size:13px;font-family:'Open Sans',sans-serif,Arial;background:#f4f3f3;font-style:normal;min-width:960px}blockquote,body,code,dd,div,dl,dt,fieldset,form,h1,h2,h3,h4,h5,h6,input,label,legend,li,ol,p,pre,textarea,ul{margin:0;padding:0}ol,ul{list-style:none}a img{border:none}a{color:#0288d1;text-decoration:none}a:hover{text-decoration:underline}.btn,.btn-orange:hover,.btn-teal:hover,.jcarousel-pagination a{text-decoration:none}#globalCity-input input[type=text],*,.global-search input[type=text],:after,:before{-webkit-box-sizing:border-box;-moz-box-sizing:border-box;box-sizing:border-box}h1{font-size:22px}h2{font-size:16px}h3{font-size:14px}h4{font-size:16px}h5{font-size:14px}h6{font-size:12px}.container{width:996px;margin:0 auto}.grid-1,.grid-10,.grid-11,.grid-12,.grid-2,.grid-3,.grid-4,.grid-5,.grid-6,.grid-7,.grid-8,.grid-9{float:left;position:relative;min-height:1px;padding-right:10px;padding-left:10px}.grid-12{width:100%}.grid-11{width:91.66666667%}.grid-10{width:83.33333333%}.grid-9{width:75%}.grid-8{width:66.66666667%}.grid-7{width:58.33333333%}.grid-6{width:50%}.grid-5{width:41.66666667%}.grid-4{width:33.33333333%}.grid-3{width:25%}.grid-2{width:16.66666667%}.grid-1{width:8.33333333%}.alpha{padding-left:0}.omega{padding-right:0}.clear{clear:both}button,input,select{border:none;outline:0}button{margin:0}button::-moz-focus-inner,button::-webkit-focus-inner,input::-moz-focus-inner,input::-webkit-focus-inner{border:0;padding:0;outline:0}.ui-autocomplete-input,input::-webkit-input-placeholder{color:#a8afb3;font-size:13px}.ui-autocomplete-input,input:-moz-placeholder{color:#a8afb3;font-size:13px;opacity:1}.ui-autocomplete-input,input::-moz-placeholder{color:#a8afb3;font-size:13px;opacity:1}.ui-autocomplete-input,input:-ms-input-placeholder{color:#a8afb3;font-size:13px}.ui-autocomplete-input:focus{color:#555}.form-control-box{position:relative}.form-control{width:100%;padding:10px;background-color:#fff;background-image:none;border:1px solid #ccc;border-radius:2px;-webkit-box-shadow:inset 0 1px 1px rgba(0,0,0,.075);box-shadow:inset 0 1px 1px rgba(0,0,0,.075);-webkit-transition:border-color ease-in-out .15s,-webkit-box-shadow ease-in-out .15s;-o-transition:border-color ease-in-out .15s,box-shadow ease-in-out .15s;transition:border-color ease-in-out .15s,box-shadow ease-in-out .15s}.form-control:focus{border-color:#66afe9;outline:0;-webkit-box-shadow:inset 0 1px 1px rgba(0,0,0,.075),0 0 8px rgba(102,175,233,.6);box-shadow:inset 0 1px 1px rgba(0,0,0,.075),0 0 8px rgba(102,175,233,.6)}select.form-control{-moz-appearance:none;-webkit-appearance:none;background:url(https://imgd1.aeplcdn.com/0x0/bw/static/design15/old-images/d/dropArrowBg.png?v1=03Mar2016) 96% 50% no-repeat #fff;padding-right:30px;background:#fff\9;padding-right:5px\9}.input-xs{padding:5px 10px}.input-sm{padding:8px 10px}.input-md{padding:10px}.input-lg{padding:12px 10px}.input-xlg{padding:14px 10px}.btn{display:inline-block;padding:8px 42px;font-size:16px;border:1px solid transparent;border-radius:2px;outline:0;-ms-touch-action:manipulation;touch-action:manipulation;cursor:pointer;-webkit-user-select:none;-moz-user-select:none;-ms-user-select:none;user-select:none;background-image:none;-webkit-border-fit:border}.btn-orange{background:#f04031;color:#fff}.btn-orange:hover{background:#f85649}.btn-orange:focus{background:#df3828}.btn-teal{background:#41b4c4;color:#fff;border:1px solid #41b4c4}.btn-teal:hover{background:#58bdcb;border:1px solid #58bdcb}.btn-teal:focus{background:#37939f;border:1px solid #37939f}.btn-grey,.btn-white{background:#fff;color:#ef402f;border:1px solid #f04130}.btn-grey:hover,.btn-white:hover{background:#f04031;color:#fff;text-decoration:none}.btn-grey:focus,.btn-white:focus{background:#df3828;color:#fff;border:1px solid #f04130}.btn-disable{cursor:not-allowed;opacity:.5;-ms-filter:"progid:DXImageTransform.Microsoft.Alpha(Opacity=50)";filter:alpha(opacity=50);-moz-opacity:.5;-khtml-opacity:.5}.btn-whiteFixedRed{background:#f04031;color:#fff;border:1px solid #f04130}.btn-xxs{padding:5px 26px}.btn-xs{padding:8px}.btn-sm{padding:8px 25px}.btn-md{padding:8px 30px}.btn-lg{padding:10px 20px 8px}.btn-xlg{padding:12px 52px}.btn-full-width{display:block;width:100%}.btn-truncate{max-width:230px;text-overflow:ellipsis;overflow:hidden}.lock-browser-scroll{position:fixed;overflow-y:scroll;width:100%}.global-search{width:420px;position:relative;display:inline-block;margin-right:30px}.global-search span.search-icon-grey{position:absolute;right:10px;top:5px;cursor:pointer}#globalCity-input input[type=text],.global-search input[type=text]{width:100%;padding:7px 5px;border:none;outline:0}.gl-default-stage,.login-box{padding:4px 10px;cursor:pointer}#errNewBikeSearch.ui-autocomplete{width:470px;position:absolute;top:40px;left:3px;color:#4d5057;text-align:left}.global-location,.login-box{color:#fff;vertical-align:middle}.bike-preview,.imageWrapper,.welcome-box{text-align:center}.error-tooltip-siblings{display:none}.back,.cardWrapper,.front,.global-location,.login-box{display:inline-block}.global-location{position:relative;margin-right:20px}.gl-default-stage{border:1px solid #acaba7}.gl-default-stage span.cross-md-lgt-grey{position:relative;top:2px;margin-left:20px}.login-box{margin-right:10px}.login-box:hover{color:#ccc}.loginCloseBtn.cross-md-dark-grey{background-position:-62px -223px}.loginCloseBtn.cross-md-dark-grey:hover{background-position:-62px -246px}.welcome-box{margin-top:120px;color:#fff}.welcome-box h1{font-size:34px;color:#fff;margin-bottom:10px;text-transform:uppercase}.text-bold,a.bikeTitle{font-weight:700}.bike-search-container{margin:0 auto;width:585px}.bike-search{width:470px;float:left;background:#fff;border:1px solid #ccc;border-right:0;border-radius:2px 0 0 2px;height:40px}.new-bike-search input[type=text]{width:100%;padding:11px 10px}.used-budget-box{width:206px;float:left;color:#a8afb3;position:relative}.findBtn{float:left}.findBtn .btn-md{padding:8px 20px;border-radius:0 2px 2px 0}#nav,.loginPopUpWrapper{background:#fff;width:325px;position:fixed;top:0;right:0;bottom:0;left:-325px;overflow-y:hidden;z-index:10}.jcarousel-wrapper{position:relative;width:976px}.jcarousel{position:relative;overflow:hidden;padding:10px 0 20px}.jcarousel ul{width:20000em;position:relative}.jcarousel li{float:left;width:312px;height:330px;margin-right:20px;background:#fff}.used-bike-carousel{padding-bottom:20px}.used-bike-carousel li{height:320px}.compareCarsHomePage li{width:976px}.jcarousel-pagination a{display:inline-block;height:14px;width:14px;line-height:10px;background:#eeeff0;border:1px solid #ebebea;border-radius:15px;text-indent:-9999px;margin:5px 10px;cursor:pointer}.jcarousel-pagination a:hover{background:#82888b}.jcarousel-pagination a.active{background:#82888b;cursor:default;border:1px solid #ccc}.cardWrapper{width:100%;height:100%;overflow:hidden}.back,.front{height:290px;background:#fff;overflow:hidden;border-radius:3px;border:1px solid #ccc;-webkit-backface-visibility:hidden;-moz-backface-visibility:hidden;-ms-backface-visibility:hidden;backface-visibility:hidden}.contentWrapper,.formWrapper{position:relative;overflow:hidden;background:#fff}.header-fixed,.header-landing{position:fixed;top:0;right:0;left:0}.imageWrapper{width:310px;height:174px;overflow:hidden;display:table}.imageWrapper a{display:table-cell;vertical-align:middle;background:url(https://imgd4.aeplcdn.com/0x0/bw/static/sprites/d/loader.gif) center center no-repeat}.bikeTitle a,a.bikeTitle{color:#2a2a2a;font-size:16px;display:block}.imageWrapper a img{max-width:100%;height:auto}.used-bike-carousel .imageWrapper a img{width:310px}.bikeDescWrapper{padding:15px;background:#fff}.bikeTitle a:hover,a.bikeTitle:hover{text-decoration:underline}a.bikeTitle{margin-bottom:10px}.breadcrumb li,.bw-circle-icon,.bwsprite,.ui-autocomplete{display:inline-block}.formContent{padding:30px 20px 20px}.header-fixed,.header-landing,.header-not-fixed{padding:10px 20px;z-index:3}.breadcrumb li{margin-right:5px}.header-fixed{background:rgba(51,51,51,.8);background:#333\9}.header-landing{background:0 0!important}.inr-lg,.nav-icon{position:relative}.header-fixed-with-bg,.header-not-fixed{background:rgba(51,51,51,.8);background:#333\9}.header-fixed-inner{padding-top:55px}.bwsprite{background:url(https://imgd3.aeplcdn.com/0x0/bw/static/sprites/d/bwsprite-v2.png?v1=19Dec2016) no-repeat}.bw-circle-icon{background:url(https://imgd3.aeplcdn.com/0x0/bw/static/sprites/d/bw-circle-icon.png) no-repeat}.nav-icon{background-position:0 0;width:24px;height:21px;cursor:pointer;top:-1px}.nav-icon:hover{background-position:0 -29px}.bw-logo{background-position:-34px 0;width:93px;height:27px}.search-icon-grey,.search-icon-white{width:18px;height:18px}.search-icon-grey{background-position:-34px -275px}.search-icon-white{background-position:-64px -278px}.inr-sm-dark{width:8px;height:12px;background-position:-128px -468px}.inr-md,.inr-md-light{width:8px;height:13px}.inr-md{background-position:-53px -490px}.inr-md-light{background-position:-129px -490px}.inr-lg,.inr-lg-thin{width:10px;height:14px}.inr-lg{background-position:-110px -491px;top:1px}.inr-lg-thin{background-position:-71px -491px}.inr-xl{width:12px;height:17px;background-position:-40px -515px}.ui-helper-hidden-accessible{position:absolute;left:-9999px}.ui-autocomplete{z-index:10;margin:-1px 0 0 -3px;padding:0;width:auto;background:#aaa;border-radius:0 0 2px 2px;border:1px solid #ccc;border-top:0;overflow:visible}.ui-autocomplete.source:hover{background:#ddd}.ui-menu .ui-menu-item{padding:10px;background:#fff;font-size:14px;cursor:pointer}.ui-menu .ui-menu-item.ui-state-focus,.ui-menu .ui-menu-item:hover{background:#eee}.ui-menu .ui-menu-item a{color:#333}.ui-menu .ui-menu-item a.target-popup-link{display:none;float:right;color:#0288d1;font-size:13px}.ui-menu .ui-menu-item.ui-state-focus a.target-popup-link,.ui-menu .ui-menu-item:hover a.target-popup-link{display:block}.ui-menu .ui-menu-item span.upcoming-link{float:right;color:#4d5057;font-size:13px}.leftfloat{float:left}.rightfloat{float:right}.text-left{text-align:left}.text-right{text-align:right}.text-center{text-align:center}.text-justify{text-align:justify}.text-wrap{white-space:normal}.text-nowrap{white-space:nowrap}.text-lowercase{text-transform:lowercase}.text-uppercase{text-transform:uppercase}.text-capitalize{text-transform:capitalize}.text-unbold{font-weight:400}.text-italic{font-style:italic}.text-default{color:#4d5057}.text-xt-light-grey{color:#a8afb3}.text-light-grey{color:#82888b}.text-medium-grey{color:#999}.text-grey{color:#666}.text-dark-grey{color:#333}.text-black{color:#2a2a2a}.text-red{color:#ef3f30}.text-white{color:#fff}.text-link{cursor:pointer;color:#0288d1}.text-orange{color:#f04031}.text-orange-light{color:#F88379}.font9{font-size:9px}.font10{font-size:10px}.font11{font-size:11px}.font12{font-size:12px}.font14{font-size:14px}.font16{font-size:16px}.font18{font-size:18px}.font20{font-size:20px}.font22{font-size:22px}.font24{font-size:24px}.font25{font-size:25px}.font26{font-size:26px}.font27{font-size:27px}.font28{font-size:28px}.font29{font-size:29px}.font30{font-size:30px}.font32{font-size:32px}.line-height{line-height:1.8}.content-inner-block-5{padding:5px}.content-inner-block-10{padding:10px}.content-inner-block-15{padding:15px}.content-inner-block-20{padding:20px}.content-box-shadow{background:#fff;-moz-box-shadow:0 2px 2px #e2e2e2,0 1px 1px #f1f1f1;-webkit-box-shadow:0 2px 2px #e2e2e2,0 1px 1px #f1f1f1;-o-box-shadow:0 2px 2px #e2e2e2,0 1px 1px #f1f1f1;-ms-box-shadow:0 2px 2px #e2e2e2,0 1px 1px #f1f1f1;box-shadow:0 2px 2px #e2e2e2,0 1px 1px #f1f1f1;border:1px solid #e2e2e2\9}.inline-block{display:inline-block;vertical-align:middle}.block{display:block}.ul-arrow li{margin-top:10px}.bg-white{background:#FFF}.bg-light-grey{background:#f5f5f5}.bg-light-grey-with-alpha{background:rgba(242,242,243,.9)}.bg-grey{background:#e1e1e1}.bg-dark-grey{background:#333}.hide{display:none}.show{display:block}.position-rel{position:relative}.position-abt{position:absolute}.margin-left5{margin-left:5px}.margin-left10{margin-left:10px}.margin-left15{margin-left:15px}.margin-left20{margin-left:20px}.margin-top5{margin-top:5px}.margin-top10{margin-top:10px}.margin-top12{margin-top:12px}.margin-top15{margin-top:15px}.margin-top20{margin-top:20px}.margin-right5{margin-right:5px}.margin-right10{margin-right:10px}.margin-right15{margin-right:15px}.margin-right20{margin-right:20px}.margin-bottom5{margin-bottom:5px}.margin-bottom10{margin-bottom:10px}.margin-bottom15{margin-bottom:15px}.margin-bottom20{margin-bottom:20px}.padding-left5{padding-left:5px}.padding-left10{padding-left:10px}.padding-left15{padding-left:15px}.padding-left20{padding-left:20px}.padding-top5{padding-top:5px}.padding-top10{padding-top:10px}.padding-top15{padding-top:15px}.padding-top20{padding-top:20px}.padding-top25{padding-top:25px}.padding-right5{padding-right:5px}.padding-right10{padding-right:10px}.padding-right15{padding-right:15px}.padding-right20{padding-right:20px}.padding-bottom5{padding-bottom:5px}.padding-bottom10{padding-bottom:10px}.padding-bottom15{padding-bottom:15px}.padding-bottom20{padding-bottom:20px}
</style>

<noscript><iframe src="//www.googletagmanager.com/ns.html?id=GTM-5CSHD6"
height="0" width="0" style="display:none;visibility:hidden"></iframe></noscript>

<% Bikewale.Utility.BWCookies.SetBWUtmz(); %>
