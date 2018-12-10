<%@ Page Language="C#" AutoEventWireup="false" Inherits="Carwale.UI.Editorial.FeaturesView" Trace="false" Debug="false" enableEventValidation="false" EnableViewState="false"%>
<%@ Register TagPrefix="Qr" TagName="QuickResearch" src="/Controls/QuickResearch.ascx" %>
<%@ Register TagPrefix="uc" TagName="TipsAndAdvices" src="/Controls/TipsAndAdvices.ascx" %>
<%@ Register TagPrefix="uc" TagName="SubNavigation" Src="/Controls/subNavigation.ascx" %>
<%@ Import Namespace="Carwale.Utility" %>
<!doctype html>
<html itemscope itemtype="http://schema.org/WebPage">
<head>
<%
	// Define all the necessary meta-tags info here.
	// To know what are the available parameters,
	// check page, headerCommon.aspx in common folder.

    featuresDetail.Description = (System.Text.RegularExpressions.Regex.Replace(featuresDetail.Description, @"<[^>]*>", string.Empty));
	PageId 			= 47;
	Title 			= CarComparePageTitle;
    Description     = featuresDetail.Description;
	Revisit 		= "15"; 
	DocumentState 	= "Static";
    canonical       = canonicalUrl;
    altUrl          = "https://www.carwale.com/m" + Url;
    AdId            = "1396440332273";
    AdPath          = "/1017752/ReviewsNews_";
    
%>
<!-- #include file="/includes/global/head-script.aspx" -->
    <meta name="twitter:card" content="summary_large_image">
<meta name="twitter:site" content="@CarWale">
<meta name="twitter:title" content="<%=featuresDetail.Title %>">
<meta name="twitter:description" content="<%=featuresDetail.Description%>">
<meta name="twitter:creator" content="@CarWale">
<meta name="twitter:image" content="<%=Carwale.Utility.ImageSizes.CreateImageUrl(featuresDetail.HostUrl,Carwale.Utility.ImageSizes._642X361,featuresDetail.OriginalImgUrl) %>">
<meta property="og:title" content="<%=featuresDetail.Title %>" />
<meta property="og:type" content="website" />
<meta property="og:url" content="<%="https://www.carwale.com" + featuresDetail.ArticleUrl %>" />
<meta property="og:image" content="<%=Carwale.Utility.ImageSizes.CreateImageUrl(featuresDetail.HostUrl,Carwale.Utility.ImageSizes._642X361,featuresDetail.OriginalImgUrl) %>" />
<meta property="og:description" content="<%=featuresDetail.Description%>" />
<meta property="og:site_name" content="CarWale" />
<meta property="article:published_time" content="<%=featuresDetail.DisplayDate.ToString("s") %>" />
<meta property="article:section" content="Car News" />
<meta property="article:tag" content="<%=string.Join(",",featuresDetail.TagsList.ToArray() )%>" />
<meta property="fb:admins" content="154881297559" />
<meta property="fb:pages" content="154881297559" />
     <script type='text/javascript'>
         var AdSlots = {};
         googletag.cmd.push(function () {
             googletag.defineSlot('<%= AdPath %>300x250', [[300, 250], [300, 600]], 'div-gpt-ad-<%= AdId %>-0').addService(googletag.pubads());
            googletag.defineSlot('<%= AdPath %>300x250_BTF', [300, 250], 'div-gpt-ad-<%= AdId %>-1').addService(googletag.pubads());
            googletag.defineSlot('<%= AdPath %>970x90', [[220, 90], [728, 90], [950, 90], [960, 90], [970, 66], [970, 90], [960, 60], [970, 60]], 'div-gpt-ad-<%= AdId %>-2').addService(googletag.pubads());
             AdSlots['div-gpt-ad-<%= AdId %>-9'] = googletag.defineSlot('<%= AdPath %>300x600', [[120, 240], [120, 600], [160, 600], [300, 250], [300, 600]], 'div-gpt-ad-<%= AdId %>-9').addService(googletag.pubads());
             googletag.pubads().setTargeting("<%= targetKey %>", "<%= targetValue %>");
             googletag.pubads().setTargeting('UserModelHistory', '<%= CookiesCustomers.UserModelHistory.Replace('~', ',')%>');
             //googletag.pubads().enableSyncRendering();
             googletag.pubads().collapseEmptyDivs();
             googletag.pubads().enableSingleRequest();
             googletag.enableServices();
         });
    </script>
    <script type="application/ld+json">
        {
            "@context":"http://schema.org",
             "@type":"NewsArticle",
             "inLanguage":"English",
             "headline":"<%=featuresDetail.Title %>",
             "description":"<%=featuresDetail.Description %>",
             "mainEntityOfPage":{
             "@type": "WebPage",
            "@id": "<%="https://www.carwale.com" + featuresDetail.ArticleUrl %>"
         },
         "datePublished":"<%=featuresDetail.DisplayDate.ToString("MM/dd/yyyy") %>",
         "dateModified":"<%=featuresDetail.DisplayDate.ToString("MM/dd/yyyy") %>",
         "url":"<%="https://www.carwale.com" + featuresDetail.ArticleUrl %>",
         "publisher":{
         "@type":"Organization",
         "name":"CarWale",
         "url":"https://www.carwale.com",
         "logo":{
         "@type":"ImageObject",
         "url":"https://imgd.aeplcdn.com/0x0/cw/static/sprites/m/cw-logo-v1.png",
         "height":"60",
         "width":"294"
         }
         },
         "author":{
           "@type":"Person",
            "name":"<%=featuresDetail.AuthorName %>"
         },
         "image":{
        "@type":"ImageObject",
        "url":"<%="https://imgd.aeplcdn.com/" + featuresDetail.LargePicUrl %>",
         "height":"361",
         "width":"642"
         }
    }
    </script>
<link rel="stylesheet" href="/static/css/slideshow.css" type="text/css" >
<link rel="stylesheet" href="/static/css/rt.css" type="text/css" >
<link rel="stylesheet" href="/static/css/articles.css" type="text/css" >
<!-- Below is files for color box -->
<script  type="text/javascript"  src="/static/src/jquery.colorbox.js" ></script>
<link rel="stylesheet" href="/static/css/colorbox.css" type="text/css" >
<script   src="/static/js/contenttracking.js"  type="text/javascript"></script>
<script src="https://apis.google.com/js/platform.js" async defer></script>
<script type="text/javascript" src="/static/js/sticky-ad-news.js" defer></script>
<script>
    function googlePlusCallBack() {
        Common.utils.trackAction('CWInteractive', 'share', 'GooglePlus', $(this)[0].Yf.Il);
    }
</script>
<style>
.fl{float:left;padding:5px;border:solid 1px #CCC}
.fr{float:right;padding:5px;border:solid 1px #CCC}
.imgtext{background-color:#f2f2f2;padding:5px;font-size:11px}
.news-sprite{background:url(https://img.carwale.com/images/news-sprite.png) no-repeat;display:inline-block;position:relative}
.v-views-icon{background-position:0 -70px;top:6px;width:16px;height:15px;margin-right:5px}
.p{margin-top:20px!important}
</style>
</head>
<body class="bg-white header-fixed-inner special-page special-skin-body no-bg-color">
<form runat="server">
    <!-- #include file="/includes/header.aspx" -->
    <section class="container">
    	<div class="grid-12">
            <div class="padding-bottom15 text-center ad-slot">
            <%= Carwale.UI.HtmlHelpers.Helpers.GetAdBarForAspx(AdId, 0, 90, 0, 0, true, 2) %>
            </div>
        </div>
    </section>
    <div class="clear"></div>
    <section class="bg-light-grey padding-bottom20 padding-top10 no-bg-color overall-container">
        <div class="container">
            <div class="grid-12">
                    <div class="breadcrumb margin-bottom15"> <!-- breadcrumb code starts here -->
                	    <ul class="special-skin-text" itemscope itemtype="http://schema.org/BreadcrumbList">
                    	    <li itemprop="itemListElement" itemscope itemtype="http://schema.org/ListItem"><a href="/" itemprop="item" title="Carwale"><span itemprop="name">Home</span></a><meta itemprop="position" content="1" /></li>
                            <li itemprop="itemListElement" itemscope itemtype="http://schema.org/ListItem"><span class="fa fa-angle-right margin-right10"></span><a title="Car Research" href="/reviews-news/" itemprop="item"><span itemprop="name">Reviews & News</span></a><meta itemprop="position" content="2" /></li>
                            <li itemprop="itemListElement" itemscope itemtype="http://schema.org/ListItem"><span class="fa fa-angle-right margin-right10"></span><a title="Comparison Test" href="/features/" itemprop="item"><span itemprop="name">Special Reports</span></a><meta itemprop="position" content="3" /></li>
                            <li itemprop="itemListElement" itemscope itemtype="http://schema.org/ListItem"><span class="fa fa-angle-right margin-right10"></span><span itemprop="name"><%= ArticleTitle%></span><meta itemprop="position" content="4" /></li>
                        </ul>
                        <div class="clear"></div>
                    </div>
                      <h1 class="font30 text-black special-skin-text"><%= ArticleTitle%></h1>
                    <div class="border-solid-bottom margin-top10"></div>
                </div>
            <div class="clear"></div>             
            <div class="grid-8 scroll">
                <div class="content-box-shadow content-inner-block-10 content-details margin-top15" id="<%= BasicId%>" categoryid="6" categoryname="feature">
		       
                    <div class="leftfloat">
                        <%= displayDate.ToString("MMMM dd, yyyy, hh:mm tt")%> IST by <% if(!string.IsNullOrEmpty(authorMaskingName)){ %><a href="/authors/<%= authorMaskingName %>"><%= authorName %></a><%} else { %><%= authorName %><%} %> </div>        

		            <div class="rightfloat"><span class="cw-sprite view-icon margin-right5 news-sprite v-views-icon"></span><%= views != "" ?views: "0" %> Views</div> 
                    <div class="clear"></div>
                    <ul class="social" style="margin-top:10px">			
			            <li style="float:left; width:90px; height:20px;"><iframe src="https://www.facebook.com/plugins/like.php?layout=button_count&show_faces=false&action=like&font&colorscheme=light&width=100&height=25&href=https://www.carwale.com<%= Url%>" scrolling="no" frameborder="0" style="border:none; overflow:hidden; width:100px; height:25px;" allowTransparency="true"></iframe></li>
                        <li style="float:left; width:90px; height:20px;"><iframe allowtransparency="true" frameborder="0" scrolling="no" src='https://platform.twitter.com/widgets/tweet_button.html?text=<%= ArticleTitle  %>&via=CarWale&url=https://www.carwale.com<%= Url%>&counturl=https://www.carwale.com<%= Url%>' style="width:110px; height:40px;"></iframe></li>
                        <li style="float:left; width:90px; height:20px;">
                            <div class="g_plus">
                                <!-- Place this tag where you want the +1 button to render -->
                                <g:plusone size="medium" href='https://www.carwale.com<%= Url%>' callback="googlePlusCallBack" count="true"></g:plusone>
                                <!-- Place this tag after the last plusone tag -->
                                <script type="text/javascript">
                                    (function () {
                                        var po = document.createElement('script'); po.type = 'text/javascript'; po.async = true;
                                        po.src = 'https://apis.google.com/js/plusone.js';
                                        var s = document.getElementsByTagName('script')[0]; s.parentNode.insertBefore(po, s);
                                    })();
                                </script>
                            </div>
                        </li>
		            </ul>
                    <div class="clear"></div>
        	        <!--Below code for expert Review Subnavigation starts here-->
                    <div class="inline-block10 margin-top10">
                    <uc:SubNavigation ID="SubNavigation" runat="server" IsOverviewPage="true" />    
                </div>
                    <!--Below code for expert Review Subnavigation ends here-->     
                    <div id="expert-review">
                        <asp:Repeater ID="rptFeatures" runat="server">
					    <itemtemplate>
                            <div id='<%# Format.RemoveSpecialCharacters( DataBinder.Eval( Container.DataItem, "PageName" ).ToString()) %>' class="btm-border">
                                <h2> <%# DataBinder.Eval( Container.DataItem, "PageName" ).ToString() %> </h2>
                                    <div class="text-area-right content-font">
                                         <%# DataBinder.Eval( Container.DataItem, "Content" ).ToString() %>
                                
                                        <%--<div id="divOtherInfo" runat="server"></div>  --%>
                                    </div>
                            </div>
                         </itemtemplate>
                    </asp:Repeater>
                        <%--for photos code starts here--%>
                        <% if (ShowGallery) {%>
                            <div id="photos">
                        <h2>Photos</h2>
                            <div>
                                <asp:DataList ID="dlstPhoto" runat="server" RepeatDirection="Horizontal" RepeatColumns="3" ItemStyle-VerticalAlign="top">
				                    <itemtemplate>
					                    <a rel="slidePhoto" target="_blank" href="<%# Carwale.Utility.ImageSizes.CreateImageUrl(Convert.ToString(DataBinder.Eval(Container.DataItem,"HostUrl")),Carwale.Utility.ImageSizes._725X408,Convert.ToString(DataBinder.Eval(Container.DataItem,"OriginalImgPath")))%>" title="<b><%# DataBinder.Eval( Container.DataItem, "Caption" ).ToString() %></b>" />
						                    <img alt="<%# DataBinder.Eval( Container.DataItem, "MakeBase.MakeName" ).ToString() + " " + DataBinder.Eval( Container.DataItem, "ModelBase.ModelName" ).ToString() + " " + DataBinder.Eval( Container.DataItem, "ImageCategory" ).ToString() %>" border="0" style="margin:0px 45px 10px 0px;cursor:pointer;" src="https://imgd.aeplcdn.com/0x0/statics/grey.gif" class="lazy" data-original="<%# Carwale.Utility.ImageSizes.CreateImageUrl(Convert.ToString(DataBinder.Eval(Container.DataItem,"HostUrl")),Carwale.Utility.ImageSizes._144X81,Convert.ToString(DataBinder.Eval(Container.DataItem,"OriginalImgPath")))%>" title="Click to view larger photo" />
					                    </a>
				                    </itemtemplate>
			                    </asp:DataList> 
                            </div>
                    </div>
                        <%} %>
                    </div>
                </div>
                <a data-next rel="noindex" href="/relatedArticle/details/?basicId=<%=latestRelatedArticleIds.ElementAt(0)%>"></a>
            </div>
           <div class="grid-4">
                <div class="content-box-shadow content-inner-block-5 margin-bottom20 ad-slot margin-top15">
          	        <%= Carwale.UI.HtmlHelpers.Helpers.GetAdBarForAspx(AdId, 300, 250, 0, 0, false, 0) %>
                </div>
                <div class="content-box-shadow content-inner-block-10 margin-bottom20 margin-top15">
                    <h2>Quick Research</h2>
                    <Qr:QuickResearch id="qrQuickResearch" runat="server" />
                </div>

               <% if (carRightWidget != null && carRightWidget.PopularModels != null && carRightWidget.UpcomingCars != null && carRightWidget.PopularModels.Count > 0 && carRightWidget.UpcomingCars.Count > 0) { %>
                  <div class="content-box-shadow margin-bottom20">
                      <% Carwale.UI.NewCars.RecommendCars.RazorPartialBridge.RenderPartial("~/Views/Shared/FRQ/_PopularCarWidget.cshtml", carRightWidget); %>
                  </div>
               <%} %>
               <div class="content-box-shadow content-inner-block-5 margin-bottom20 ad-slot margin-top20">
                <%= Carwale.UI.HtmlHelpers.Helpers.GetAdBarForAspx(AdId, 300, 250, 0, 0, false, 1) %>
               </div>
                <div class="content-box-shadow content-inner-block-10">
                    <uc:TipsAndAdvices id="ucTipsAndAdvices" runat="server" />
                </div>
               <aside>
                    <div class="ad-container ad-slot">
                        <div id="floating-ad" style="min-width: 312px; display: block !important;" class="content-box-shadow content-inner-block-5 margin-bottom20 ad-slot margin-top20">
                            <%= Carwale.UI.HtmlHelpers.Helpers.GetAdBarForAspx(AdId, 300, 600, 0, 0, false, 9) %>
                        </div>
                    </div>
               </aside>
            </div>
            <div class="clear"></div>
        </div>
    </section>
    </form>
    <!-- #include file="/includes/footer.aspx" -->
    <!-- #include file="/includes/global/footer-script.aspx" -->
    <script type="text/javascript" src="/static/src/jquery.dfp.min.js" defer></script>
    <script   src="/static/js/jquery-jscroll.js" > </script>
    <script type="text/javascript">
            var articleBasicid = '<%= BasicId%>';
        var relatedArticleIds = <%= Newtonsoft.Json.JsonConvert.SerializeObject(latestRelatedArticleIds) %>;
        var hostUrl = '<%= "https://" + Carwale.Utility.CWConfiguration.HostUrl %>';
            var relatedArticleCount = 1;
            var scrollTitleDict = [];
            var articlesParams = [];
            var titleIndex = 1;
            var changeNextOffsetTop = false;
            var INT_MAX =2147483647;
            var nextScrollChangeValue = INT_MAX;
            var nextScrollChangeIndex = 0;
            var triggerTracking = true;
            Common.showCityPopup = false;
            doNotShowAskTheExpert = false;

            $(document).ready(function () {
                ContentTracking.tracking.setUpTracking(1, 'Feature', '.content-details');
                $("a[rel='slidePhoto']").colorbox();
                initialize();
                $('img.lazy').lazyload();
                $(window).scroll(function() {
                    var scrollTop = $(this).scrollTop();
                    if(changeNextOffsetTop == true && nextScrollChangeValue<scrollTop){
                        if(nextScrollChangeValue == $('.jscroll-added')[nextScrollChangeIndex-1].offsetTop)
                        changeNextOffsetTop = false;                   
                        scrollTitleDict[nextScrollChangeIndex].key = $('.jscroll-added')[nextScrollChangeIndex-1].offsetTop;
                        nextScrollChangeValue = scrollTitleDict[nextScrollChangeIndex].key;
                    }
                    if (scrollTitleDict.length > 1) {
                        Common.utils.setUrlTitleOnScroll(scrollTop, scrollTitleDict, articlesParams);
                    }
                });
            });

            var scrollCallback = function () {
                var count;
                var pageUrl;
                if(triggerTracking){
                    pageUrl= $('[data-page-url]:last').data('page-url');
                    Common.utils.firePageView(pageUrl);
                }
                if(relatedArticleIds.length >= relatedArticleCount) { 
                    ++titleIndex;
                    var relatedArticleList = document.querySelectorAll(".jscroll-added");
                    var elementTop =  $(this)[0].offsetTop;
                    changeNextOffsetTop=true;
                    if (relatedArticleCount > 1) {
                        scrollTitleDict.push({
                            key: elementTop-$(window).height(),
                            value: titleIndex - 1
                        });
                    } else {
                        scrollTitleDict.push({      
                            key: elementTop,
                            value: titleIndex - 1
                        });
                    }
                    nextScrollChangeIndex = titleIndex-1;
                    nextScrollChangeValue = scrollTitleDict[titleIndex-1].key;
                    articlesParams.push(new Object({
                        Title: relatedArticleList[titleIndex-2].firstElementChild.innerText,
                        Url: pageUrl
                    }));
                    if (relatedArticleIds.length > relatedArticleCount) {
                        var relatedArticleUrl = '/relatedArticle/details/?basicId='+relatedArticleIds[relatedArticleCount++];
                        $('a[data-next]:last').attr('href', relatedArticleUrl);
                        var gplusButton = 'gplus-' + relatedArticleIds[relatedArticleCount-1];
                        gapi.plusone.render(gplusButton, { 'href':hostUrl + $('#'+gplusButton).data('articleurl'), 'callback': 'googlePlusCallBack', 'count': 'true' });
                        count = relatedArticleCount;
                        googletag.pubads().refresh([AdSlots['div-gpt-ad-1396440332273-9']]);
                        Common.utils.trackAction('CWNonInteractive', 'NetworkAdTest', 'News details page_TW_d', 'Ad loaded_' + (relatedArticleCount));
                    }
                    else {
                        $('a[data-next]:last').attr('href', "/m/emptyResult/");
                        triggerTracking = false;
                        count = relatedArticleCount + 1;
                        googletag.pubads().refresh([AdSlots['div-gpt-ad-1396440332273-9']]);
                        Common.utils.trackAction('CWNonInteractive', 'NetworkAdTest', 'News details page_TW_d', 'Ad loaded_' + (relatedArticleCount + 1));
                    }
                }
                $('img.lazy').lazyload();                    
            }
            var initialize = function() {
                scrollTitleDict.push({
                    key: 0,
                    value: 0
                });

                articlesParams.push(new Object({
                    Title: document.title,
                    Url: window.location.pathname,
                }));
            }

            $('.scroll').jscroll({
                autoTrigger: true,
                autoTriggerUntil: 2,
                contentPercent: 75,
                callback: scrollCallback,
                loadingHtml: "<div class='text-center margin-top5'><img src='https://imgd.aeplcdn.com/0x0/cw/static/circle-loader.gif'/></div>",
                container: '.content-details'
            });
        </script>
</body>
</html>
