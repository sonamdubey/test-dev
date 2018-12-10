<%@ Page Language="C#" AutoEventWireup="false" Inherits=" Carwale.UI.Editorial.VideoDefault" Trace="false" ViewStateMode="Disabled" %>
<%@ Register TagPrefix="uc" TagName="VideoCarousel" src="/Controls/VideoCarousel.ascx" %>
<!doctype html>
<html itemscope itemtype="http://schema.org/WebPage">
<head>
<%
	// Define all the necessary meta-tags info here.
	// To know what are the available parameters,
	// check page, headerCommon.aspx in common folder.
	
	PageId 			= 55;
	Title 			= "Car Videos, Expert Video Reviews with Road Test & Car Comparison | Carwale";
	Description 	= "Check latest car videos, Watch Carwale Expert's take on latest Cars - Features, performance, price, fuel economy, handling and more.";
    AdId            = "1396440332273";
    AdPath          = "/1017752/ReviewsNews_";
    canonical       = "https://www.carwale.com/videos/";
%>
 <!-- #include file="/includes/global/head-script.aspx" -->
  <script type='text/javascript'>
      googletag.cmd.push(function () {
          googletag.defineSlot('<%= AdPath %>300x250', [300, 250], 'div-gpt-ad-<%= AdId %>-0').addService(googletag.pubads());
          googletag.defineSlot('<%= AdPath %>970x90', [[220, 90], [728, 90], [950, 90], [960, 90], [970, 66], [970, 90]], 'div-gpt-ad-<%= AdId %>-2').addService(googletag.pubads());
          googletag.pubads().setTargeting("City", "<%= CookiesCustomers.MasterCity.ToString() %>");
          googletag.pubads().setTargeting('UserModelHistory', '<%= CookiesCustomers.UserModelHistory.Replace('~', ',')%>');
          //googletag.pubads().enableSyncRendering();
          googletag.pubads().enableSingleRequest();
          googletag.enableServices();
      });
    </script>
  
<link rel="stylesheet" href="/static/css/video.css" type="text/css" >
<script  language="javascript"  src="/static/src/bt.js"  type="text/javascript"></script>
<style>
    .carousel_wrapper {width:955px;}
    .content-place p {min-height:auto;}
    .list_carousel {height:240px; overflow:hidden;}
    .car-data-list li {min-height:215px; width:200px;}
</style>
</head>
<body class="bg-white header-fixed-inner special-page special-skin-body no-bg-color">
    <form id="Form1" runat="server">   
    <!-- #include file="/includes/header.aspx" -->
    <section class="container">
            <div class="grid-12">
                <div class="padding-bottom15 text-center">
                    <%= Carwale.UI.HtmlHelpers.Helpers.GetAdBarForAspx(AdId, 0, 90, 0, 0, true, 2) %>
                </div>
            </div>
   </section>
    <div class="clear"></div>
    <section class="bg-light-grey padding-top10 padding-bottom20 no-bg-color">
        <div class="container">
    <div class="grid-12">
        <div><!-- breadcrumb code starts here -->
     	    <ul class="breadcrumb margin-bottom15 special-skin-text">
         	    <li><a href="/">Home</a></li>
                 <li><span class="fa fa-angle-right margin-right10"></span>Car Videos</li>
             </ul>
             <div class="clear"></div>
             <h1 class="font30 text-black special-skin-text">Videos</h1>
            <div class="border-solid-bottom margin-top10 margin-bottom15"></div>
        </div>
    </div>
    <div class="clear"></div>
    <div class="grid-12">
        <div class="content-box-shadow">
	        <div class="content-inner-block-10">
            <uc:VideoCarousel TopCount=-1 PageId=55 runat="server" />
            </div>
        </div>
    </div>
    <div class="column grid-8 margin-top20">
            <div class="content-box-shadow margin-bottom20">
                <div class="content-inner-block-10">
                    <div class="leftfloat">
                    <h3 class="font14">Reviews, Specials, Underground, Launch Alerts & <br />a whole lot more...</h3>
                </div>
                    <div class="rightfloat" style="*width:200px;">
                    <div class="g-ytsubscribe" data-channel="powerdriftofficial" data-layout="full" data-count="default" data-onytevent="onYtEvent"></div>
                    <script src="https://apis.google.com/js/platform.js"></script>
                    <script>
                        function onYtEvent(payload) {
                            if (payload.eventType == 'subscribe') {
                                // Add code to handle subscribe event.
                            } else if (payload.eventType == 'unsubscribe') {
                                // Add code to handle unsubscribe event.
                            }
                            if (window.console) { // for debugging only
                                window.console.log('YT event: ', payload);
                            }
                        }
                    </script>
                </div>
                    <div class="clear"></div>
                </div>
            </div>
            <div class="content-box-shadow">
                    <div class="relative video-tab-wrap">
    	            <!-- data tabs code starts here-->
                    <div class="video-tabs">
        	            <ul>
            	            <li class="active"id="mostpopular">Most Popular</li>
                            <li id="expertreviews">Expert Reviews</li>
                            <li id="interior">Interiors Show</li>
                            <li id="miscel">Miscellaneous</li>
                        </ul>
                        <div class="clear"></div>
                    </div>
                    <!-- data tabs code ends here-->
                    <!-- Most Popular data code starts here-->
                    <div class="car-data-list" id="most-popular">
        	            <ul id="ulmostpopular">
                            <div id="divMostPopular"></div>
        	            </ul>
                        <div class="clear"></div>
                    </div>
                    <!-- Most Popular data code ends here-->
                    <!-- Expert Reviews data code starts here-->
                    <div class="car-data-list hide" id="expert-reviews">
        	            <ul id="ulexpertreviews">
                            <div id="divExpertReviews"></div>
        	            </ul>
                        <div class="clear"></div>
                    </div>
                    <!-- Expert Reviews data code ends here-->
                    <!-- Interiors Show data code starts here-->
                    <div class="car-data-list hide" id="interiors-show">
        	            <ul id="ulinterior">
                            <div id="divInterior"></div>
        	            </ul>
                        <div class="clear"></div>
                    </div>
                    <!-- Interiors Show data code ends here-->
                    <!-- Miscellaneous data code starts here-->
                    <div class="car-data-list hide" id="miscellaneous">
        	            <ul id="ulmiscell">
                            <div id="divMiscell"></div>
        	            </ul>
                        <div class="clear"></div>
                    </div>
                    <!-- Miscellaneous data code ends here-->
                    <div id="loadingmsg" class="hide">
                        <div class="loading-popup">
                            <span class="loading-icon"></span>
                            <p style="font-size: 13px; color: blue;">Please wait, we are fetching the results for you...</p>
                            <div class="clear"></div>
                        </div>
                    </div>
                </div>
            </div>
        
        </div>
    <div class="column grid-4 margin-top20">
           <!-- Ad block code start here -->
                   <%= Carwale.UI.HtmlHelpers.Helpers.GetAdBarForAspx(AdId, 300, 250, 20, 20, false, 0) %>
           <!-- Ad block code start here -->
           <div class="clear"></div>
       </div>
    <div class="clear"></div>
            </div>  
    </section>  
    <div class="clear margin-top-10"></div>
    </form>  
<!-- #include file="/includes/footer.aspx"-->
<!-- #include file="/includes/global/footer-script.aspx"-->
<script   src="/static/src/jquery.jcarousel.min.js"  type="text/javascript"></script>
<script type="text/javascript">
    Common.showCityPopup = false;
    doNotShowAskTheExpert = false;
    var makeIdnew = 0, modelIdnew = 0;
    var categoryId = 1;
    var ulId = "ulmostpopular", basicIds = "";
    var topcount = 9;
    var divId = "divMostPopular";
    var hashparams = "catId=" + categoryId + "&mId=" + makeIdnew + "&moId=" + modelIdnew;
    $("img.lazy").lazyload({
        effect: "fadeIn",
        failure_limit: 15
    });

    lazyLoadVideoImages = function () {
        $("#featuredVideos img[src='https://imgd.aeplcdn.com/0x0/statics/grey.gif']:lt(4)").each(function () {
            $(this).attr("src", $(this).attr("data-original"));
        });
    }
    $(document).ready(function () {
        lazyLoadVideoImages();
        $('#loadingmsg').show();

        $(".video-tabs li").click(function () {
            $(".video-tabs li").removeClass("active");
            $(this).addClass("active");
            $(".car-data-list").hide();
            $(".car-data-list").eq($(this).index()).show();
        });

        $("#featuredVideos").jcarousel({ scroll: 4, initCallback: initCallbackFV, buttonNextHTML: null, buttonPrevHTML: null });
        bindCarouselEvents();

        bindCatCars(hashparams);
        //$("#QuickPriceWidget_btnProceed").attr('value', 'View On Road Price Instantly');
    });

    //$("#search").click(function () {
    //    makeIdnew = $("#drpVidMakes").val()!=-1?$("#drpVidMakes").val():0;
    //    modelIdnew = $("#drpVidModels").val() != -1 ? $("#drpVidModels").val() : 0;
    //    bindCatCars();
    //});

    $("#mostpopular").click(function () {
        ulId = "ulmostpopular";
        divId = "divMostPopular";
        categoryId = 1;
        hashparams = "catId=" + categoryId + "&mId=" + makeIdnew + "&moId=" + modelIdnew;
        bindCatCars(hashparams);
    });

    $("#expertreviews").click(function () {
        ulId = "ulexpertreviews";
        categoryId = 2;
        divId = "divExpertReviews";
        hashparams = "catId=" + categoryId + "&mId=" + makeIdnew + "&moId=" + modelIdnew;
        bindCatCars(hashparams);
    });

    $("#interior").click(function () {
        ulId = "ulinterior";
        categoryId = 3;
        divId = "divInterior";
        hashparams = "catId=" + categoryId + "&mId=" + makeIdnew + "&moId=" + modelIdnew;
        bindCatCars(hashparams);
    });

    $("#miscel").click(function () {
        ulId = "ulmiscell";
        categoryId = 4;
        divId = "divMiscell";
        hashparams = "catId=" + categoryId + "&mId=" + makeIdnew + "&moId=" + modelIdnew;
        bindCatCars(hashparams);
    });

    function bindCatCars(params) {
        //alert("I am over here");
        $('#loadingmsg').show();
        $("#" + divId).load("/videos/videocategories.aspx?" + params, function () {
            $('#loadingmsg').hide();
            $("img.lazy").lazyload({
                effect: "fadeIn",
                failure_limit: 15
            });
            $('body,html').scroll();
        });

        //$("#" + divId).load("/videos/videocategories.aspx?" + params, function () {
        //    alert("length is:" + !$('#tbl_res').exist());
        //    if (!$('#tbl_res').exist()) {
        //        $("#" + divId).html("<span>Videos Coming Soon...</span>");
        //    }
        //});
    }


    $(".dgNavDivTop a").live('click', function (e) {
        e.preventDefault();
        var navi_lnk = this.href;
        var qs = navi_lnk.split("?")[1];
        bindCatCars(qs);
    });

    function initCallbackFV(carousel) {
        $('#featuredVideos_next').bind('click', function () {
            return false;
        });

        $('#featuredVideos_prev').bind('click', function () {
            return false;
        });
    };

    function bindCarouselEvents() {
        var ucHome_prev = $('#featuredVideos_prev');
        var ucHome_next = $('#featuredVideos_next');
        var carouselUC = $('#featuredVideos').data('jcarousel');
        if (carouselUC.size() > 4) {
            ucHome_prev.click(function () {
                carouselUC.prev();
                if (carouselUC.first == 1)
                    ucHome_prev.addClass("disabled");
                ucHome_next.removeClass("disabled");
            });
            ucHome_next.click(function () {
                lazyLoadVideoImages();
                carouselButtonBehaviour(carouselUC, ucHome_prev, ucHome_next);
                carouselUC.next();
            });
        }
        else {
            ucHome_next.addClass("disabled");
        }
    }

    function carouselButtonBehaviour(carousel, prevButton, nextButton) {
        carousel.next();
        prevButton.removeClass("disabled");
        if (carousel.last == carousel.size())
            nextButton.addClass("disabled");
    }

    function addCommas(nStr) {
        nStr += '';
        x = nStr.split('.');
        x1 = x[0];
        x2 = x.length > 1 ? '.' + x[1] : '';
        var rgx = /(\d+)(\d{3})/;
        while (rgx.test(x1)) {
            x1 = x1.replace(rgx, '$1' + ',' + '$2');
        }
        return x1 + x2;
    }


    function urlFormat(url) {
        reg = /[^/\-0-9a-zA-Z\s]*/g; // everything except a-z, 0-9, / and - 
        url = url.replace(reg, '');
        var formattedUrl = url.toLowerCase().replace(/ /g, "").replace(/-/g, "").replace("/", "");
        return formattedUrl;
    }



    function formatSubCat(subCat) {
        return subCat.toLowerCase().replace(" ", "-");
    }
</script>
</body>
</html>