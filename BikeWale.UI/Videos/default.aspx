<%@ Page Language="C#" Inherits="Bikewale.Videos.Default" AutoEventWireup="false" %>
<%@ Register TagPrefix="BikeWale" TagName="video" Src="/controls/VideoCarousel.ascx" %>
<%   
    AdId = "1395986297721";
    AdPath = "/1017752/BikeWale_New_";
%>
<!-- #include file="/includes/headNew.aspx" -->
<link href="../css/video.css" rel="stylesheet" />

<div class="container_12">
    <form id="form1" runat="server">
        <div class="grid_12">
            <ul class="breadcrumb">
                <li>You are here: </li>
                <li><a href="/">Home</a></li>
                <li class="fwd-arrow">&rsaquo;</li>
                <li class="current"><strong>Bike Videos</strong></li>
            </ul><div class="clear"></div>
        </div>
        <BikeWale:video id="videos" runat="server"/>
        <!-- Left container starts here -->
        <div class="grid_8  alpha  margin-top10 column">
            <div class="content-block-white">
            <div class="relative">
    	        <!-- data tabs code starts here-->
                <div class="video-tabs">
        	        <ul>
            	        <li class="active" id="mostpopular"  style="font-size : 14px;">Most Popular</li>
                        <li id="expertreviews"  style="font-size : 14px;">Expert Reviews</li>
                        <li id="interior"  style="font-size : 14px;">Interiors Show</li>
                        <li id="miscel"  style="font-size : 14px;">Miscellaneous</li>
                    </ul>
                    <div class="clear"></div>
                </div>
                <!-- data tabs code ends here-->
                <!-- Most Popular data code starts here-->
                <div class="bike-data-list" id="most-popular">
        	        <ul id="ulmostpopular">
                        <div id="divMostPopular"></div>
        	        </ul>
                    <div class="clear"></div>
                </div>
                <!-- Most Popular data code ends here-->
                <!-- Expert Reviews data code starts here-->
                <div class="bike-data-list hide" id="expert-reviews">
        	        <ul id="ulexpertreviews">
                        <div id="divExpertReviews"></div>
        	        </ul>
                    <div class="clear"></div>
                </div>
                <!-- Expert Reviews data code ends here-->
                <!-- Interiors Show data code starts here-->
                <div class="bike-data-list hide" id="interiors-show">
        	        <ul id="ulinterior">
                        <div id="divInterior"></div>
        	        </ul>
                    <div class="clear"></div>
                </div>
                <!-- Interiors Show data code ends here-->
                <!-- Miscellaneous data code starts here-->
                <div class="bike-data-list hide" id="miscellaneous">
        	        <ul id="ulmiscell">
                        <div id="divMiscell"></div>
        	        </ul>
                    <div class="clear"></div>
                </div>
                <!-- Miscellaneous data code ends here-->
                <div id="loadingmsg" class="hide">
                    <div class="loading-popup">
                        <span class="loading-icon"></span>
                        <p style="font-size: 13px;">Please wait, we are fetching the results for you...</p>
                        <div class="clear"></div>
                    </div>
                </div>

            </div>
        </div>
        </div><!-- Left Container ends here -->
   
        <!-- Right Container ends here -->
        <div class="grid_4 column">
             <!-- #include file="/ads/Ad300x250BTF.aspx" -->
        </div><!-- Right Container ends here -->
    </form>
</div>
<script type="text/javascript">

    var divId = "divMostPopular";
    $(document).ready(function () {

        $('#loadingmsg').show();

        $(".video-tabs li").click(function () {
            $(".video-tabs li").removeClass("active");
            $(this).addClass("active");
            $(".bike-data-list").hide();
            $(".bike-data-list").eq($(this).index()).show();
            $(".bike-data-list").eq($(this).index()).removeClass('hide');
        });

        $("#featuredVideos").jcarousel({ scroll: 4, initCallback: initCallbackUC, buttonNextHTML: null, buttonPrevHTML: null });
        bindCarouselEvents();
        bindCatBikes();
    });

    $("#mostpopular").click(function () {
        ulId = "ulmostpopular";
        divId = "divMostPopular";
        //categoryId = 1;
        //hashparams = "catId=" + categoryId + "&mId=" + makeIdnew + "&moId=" + modelIdnew;
        bindCatBikes();
    });

    $("#expertreviews").click(function () {
    
        //alert("inside review");
        ulId = "ulexpertreviews";
        categoryId = 2;
        divId = "divExpertReviews";
        //alert(divId);
        //hashparams = "catId=" + categoryId + "&mId=" + makeIdnew + "&moId=" + modelIdnew;
       bindCatBikes();
    });

    $("#interior").click(function () {
        ulId = "ulinterior";
        categoryId = 3;
        divId = "divInterior";
       // hashparams = "catId=" + categoryId + "&mId=" + makeIdnew + "&moId=" + modelIdnew;
        bindCatBikes();
    });

    $("#miscel").click(function () {
        ulId = "ulmiscell";
        categoryId = 4;
        divId = "divMiscell";
      //  hashparams = "catId=" + categoryId + "&mId=" + makeIdnew + "&moId=" + modelIdnew;
       bindCatBikes();
    });

    function initCallbackUC(carousel) {
        $('#ucHome_next').click(function () {
            return false;
        });

        $('#ucHome_prev').click(function () {
            return false;
        });
    };

    function bindCatBikes() {
        $('#loadingmsg').show();
        $('#loadingmsg').removeClass('hide');
        $("#" + divId).load("/Videos/videocategories.aspx", function () {
            $('#loadingmsg').hide();
            $('#loadingmsg').addClass('hide');
        });
    }

    function bindCarouselEvents() {
        var ucHome_prev = $('#ucHome_prev');
        var ucHome_next = $('#ucHome_next');
        var carouselUC = $('#featuredVideos').data('jcarousel');
        if (carouselUC.size() > 4) {
            ucHome_prev.click(function () {
                carouselUC.prev();
                if (carouselUC.first == 1)
                    ucHome_prev.addClass("disabled");
                ucHome_next.removeClass("disabled");
            });
            ucHome_next.click(function () {
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
</script>
<!-- #include file="/includes/footerInner.aspx" -->

