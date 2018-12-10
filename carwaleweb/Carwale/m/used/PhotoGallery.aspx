<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="PhotoGallery.aspx.cs" Inherits="Carwale.UI.m.used.PhotoGallery" %>
<%@ Import Namespace="Carwale.Entity.Classified.Leads" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml" itemscope itemtype="http://schema.org/WebPage">
<head>
    <!-- #include file="/m/includes/global-scripts.aspx" -->
    <link rel="stylesheet" href="/static/m/css/uc-pg-style.css" type="text/css" >
    <link rel="stylesheet" href="/static/m/css/cwm-common-style.css" type="text/css" >
	<link rel="stylesheet" href="/static/cwfontawesome/cw-font-awesome.css" type="text/css">
    <link rel="stylesheet" href="/static/sass/partials/chat-btn.css" type="text/css">
    <link rel="stylesheet" type="text/css" href="/static/m/css/chat-sidebox.css">
    <script type="text/javascript" src="https://st.aeplcdn.com/v2/m/js/home-carousel.js?20160419032055"></script>
    <!-- Trovit Pixel Code -->
<script type="text/javascript">
    (function (i, s, o, g, r, a, m) {
        i['TrovitAnalyticsObject'] = r; i[r] = i[r] || function () {
            (i[r].q = i[r].q || []).push(arguments)
        }, i[r].l = 1 * new Date(); a = s.createElement(o),
        m = s.getElementsByTagName(o)[0]; a.async = 1; a.src = g; m.parentNode.insertBefore(a, m)
    })(window, document, 'script', 'https://analytics.trovit.com/trovit-analytics.js', 'ta');

    ta('init', 'in', 2, '883b0730c9aff671d3164a84785f6825');
</script>
<style>
    .hideImportant {
        display: none !important;
    }
</style>
<!-- End Trovit Pixel Code -->
</head>
<body>
    <!-- Outer div starts here -->
    <div data-role="page">
        <!-- Main container starts here -->
        <div id="main-container">
            <div class="uc-pg-container">
                <div class="uc-pg-header">
                    <a href="javascript:window.close()">
                        <span class="nc-pg-sprite bk-arrow floatleft"></span>
                        <div class="clear"></div>
                    </a>
                    <div class="clear"></div>
                </div>
                <div class="uc-pg-body">
                    <!-- carousel starts here -->
                    <div id="m-uc-gallery" class="m-carousel m-fluid m-carousel-photos">
                        <div id="imgGalleryCarousel" class="m-carousel-inner autoHeight">
                            <asp:Repeater ID="rptPhotos" runat="server">
                                <ItemTemplate>
                                    <div class="m-item">
                                        <img src="<%# Container.DataItem.ToString() %>">
                                    </div>
                                </ItemTemplate>
                            </asp:Repeater>
                        </div>
                        <div class="m-carousel-controls m-carousel-hud">
                            <a data-slide="prev" href="#" class="m-carousel-prev ui-link">Previous</a>
                            <a data-slide="next" href="#" class="m-carousel-next ui-link">Next</a>
                        </div>
                    </div>
                </div>
                <!--carousel ends here-->
                <div class="detail-btn-container grid-12">
                    <%if(isTopRated) {%>
                    <div class="grid-12 block text-right">
                         <span class="top-rated-seller-tag margin-top0 font12 margin-left20">Top Rated Seller</span>
                         <div class="seller-rating__toast">
                            <span class="seller-rating-toast__close"></span>
                            <p>This seller has consistently been rated well by his customers</p>
                         </div>
                     </div>
                    <%} %>
                    <div class="grid-4 padding-left0 padding-right20 chat-btn-container">
                        <button class="btn-xs chat-btn">
                            <span id="chatIcon" class="chat-btn--whatsapp-icon" data-chat-lead-type="<%= LeadType.WhatsAppLead.ToString("D")%>"></span>
                            <span class="chat-btn__text">Chat</span>
                        </button>
                    </div>
                    <!-- get seller btn code starts here -->
                        <a href="" id="getsellerDetails" data-action-page="photo-gallery" profileId ="<%=profileId%>" cityname="<%=cityName%>" rootid="<%=rootId %>" dc="<%= dc%>" class="detailsBtn  btn-orange buyerprocessBtn getSellerDetails override pg-detailbtn text-center grid-8 " data-cte-package-id="<%=ctePackageId %>" oid="11" popupurl="<%=similarCarsUrl%>">
                            <span class="text-bold getSimilarCarSellerDetails font18">Get Seller Details</span>
                            <span class="oneClickDetails hideImportant">
                            <span class="font18">1-Click</span>
                            <span class="font18">View Details</span>
                            </span>
                        </a>
                        <span class="applozic-launcher" data-mck-id data-mck-name></span>
                </div>
            </div>
            <div class="uc-pg-footer">
                <div class="uc-pg-text">
                    <span class="nc-pg-sprite six-dots-icon"></span>
                    <span>Gallery</span>
                </div>
                <div class="uc-pg-thumb tab-panel hide">
                    <div class="uc-pg-tabs">
                        <ul>
                            <li class="active" data-id="photos">Photos</li>
                        </ul>
                    </div>
                    <div class="uc-pg-tabs-data" id="photos">
                        <ul>
                            <asp:Repeater ID="rptPhotoTabs" runat="server">
                                <ItemTemplate>
                                    <li class="thumb">
                                        <a href="<%# Container.DataItem.ToString() %>" class="swipebox" title="">
                                            <img src="<%# Container.DataItem.ToString() %>"" alt="Maruti-Suzuki-Alto-800" title="Maruti-Suzuki-Alto-800" />
                                        </a>
                                    </li>
                                </ItemTemplate>
                            </asp:Repeater>
                        </ul>
                        <div class="clear"></div>
                    </div>
                </div>
            </div>
        </div>
        <!-- Request a call back popUp code starts here -->
        <div class="m-defaultAlert-window hide"></div>
        <div class="m-defaultAlert-window-new hide"></div>
        
        <!-- Request a call back popUp code ends here -->
    </div>
    <div class="cw-tabs">
        <ul>
            <li></li>
        </ul>
    </div>
    <div id="sellerDetailsDiv" style="display:none">
   <img class="recommendloadIcon centerAlign" src="https://imgd.aeplcdn.com/0x0/statics/loader.gif" width="16" height="16">
    <ul class="sellerDetails margin-bottom10 content-inner-block-10 text-white">
        <li>
            <span class="buyerprocess-sprite seller-name"></span>
            <p class="leftfloat"><span class="seller-Person"></span><br><span class="seller-Name font13 text-light-grey"> Loading...</span></p>
            <div class="clear"></div>
        </li>
        <li>
            <span class="buyerprocess-sprite seller-masking-no"></span>
            <p class="leftfloat"><span class="seller-Contact">Loading...</span></p>
            <div class="clear"></div>
        </li>
        <li>
            <span class="buyerprocess-sprite seller-email"></span>
            <p class="leftfloat"><span class="seller-Email">Loading...</span></p>
            <div class="clear"></div>
        </li>
        <li>
            <span class="buyerprocess-sprite seller-address"></span>
            <p class="leftfloat"><span class="seller-Address">Loading...</span></p>
            <div class="clear"></div>
            <p><span class="font10 text-light-grey margin-left10">Your contact details have been shared with the seller</span></p>
        </li>
    </ul>

    </div>
    <!-- #include file="/m/used/Buyerprocess.aspx" -->
    <!--Main container ends here-->
    <!--Outer div ends here-->
    <input id="placesQuery" style="display:none;"></input>
    <input id="placeValue" style="display:none;"></input>    
    <script type="text/javascript" src="/static/Src/commonUtilities.js" defer></script>
    <script type="text/javascript" src="/static/js/used/cwUsedTracking.js" defer></script>
    <script type="text/javascript" src="/static/Src/otp-verification.js"  defer></script>
    <script type="text/javascript" src="/static/js/used/chatProcess.js" defer></script>
    <script  type="text/javascript"  src="/static/m/js/used/buyerprocess.js"  defer></script>


    <!-- #include file="/m/includes/global/footer-script.aspx" -->
    <script type="text/javascript">
        var currentImgIndex = 0;
        function windowResize() {
            var resizeTimer;
            $(window).resize(function () {
                isWindowResize = true;
                clearTimeout(resizeTimer);
                resizeTimer = setTimeout(function () {
                    callActionBox();
                    resizeCarousel();
                    $('div.m-carousel').carousel('move', currentImgIndex);
                }, 200);
            });
        }
        function bindAfterSlide() {
            $('div.m-carousel').on('afterSlide', function (event, prevIndex, currentIndex) {
                currentImgIndex = currentIndex;
            });
        }
        bindAfterSlide();

        $(document).ready(function (e) {
            m_bp_additonalFn.sellerDetailsBtnTextChange();            
            if (navigator.userAgent.match(/(iPod|iPhone|iPad)/)) {
                $(".callBtn").attr('target', '_blank');
            }
            // m-carousel code
            $('.m-carousel').carousel();
            $(window).resize(function () {
                callActionBox();
                var width, height, sliderCss
                width = window.innerWidth ? window.innerWidth : $(window).width();
                height = window.innerHeight ? window.innerHeight : $(window).height();
                sliderCss = {
                    width: width,
                    height: height
                };
                $("#m-uc-gallery").css(sliderCss);

                bindAfterSlide();
                windowResize()
            });
            // Set width and height on document ready
            var ncWidth = $(window).width();
            var ncHeight = $(window).height();
            var sliderCss = {
                width: ncWidth,
                height: ncHeight
            };
            $("#m-uc-gallery").css(sliderCss);

            // below code for opening gallery data on click on gallery text
            $(".uc-pg-text").click(function () {
                $(".m-defaultAlert-window-new").show();
                $(".uc-pg-thumb").slideDown().removeClass("hide");
                $("html, body").animate({ scrollTop: $(".uc-pg-thumb").offset().top }, 1000);
                $(".uc-pg-text").hide();
            });
            // below code for hiding gallery data on click on img
            $(".uc-pg-tabs-data li").click(function (e) {
                $(".m-defaultAlert-window-new").hide();
                e.preventDefault();
                $(".uc-pg-thumb").slideUp().removeClass("hide");
                $("html, body").animate({ scrollTop: $(".uc-pg-thumb").offset().top }, 1000);
                $(".uc-pg-text").show();
            });
            // below code for nc-pg-tabs
            $(".uc-pg-tabs li").click(function () {
                var panel = $(this).closest('.tab-panel');
                $(".uc-pg-tabs li").removeClass('active');
                $(this).addClass('active');

                var panelId = $(this).attr('data-id');
                panel.find('.uc-pg-tabs-data').hide();
                $('#' + panelId).show();
            });
            // request a call btn code
            $("#reqCallBtn").click(function () {
                $(".m-defaultAlert-window,.requestCallBack").show();
                $("div.uc-pg-form-area").show();
                $("#reqCallTitle").show();
                $("#reqCallThankYou").hide();
                $(".uc-pg-thank-you").hide();
                $("#formSubmit").show();
                $("#formDone").hide();
            });
            // below code for closing 'm-defaultAlert-window' and 'requestCallBack'
            $(".nc-pg-close-icon,#formDone").click(function () {
                $("#reqCallTitle").show();
                $("#reqCallThankYou").hide();
                $(".m-defaultAlert-window,.requestCallBack").hide();
            });
            // below code for to show 'nc-pg-thank-you' and 'formDone' btn
            $("#formSubmit").click(function () {
                $("#formSubmit,.uc-pg-form-area").hide();
                $("#reqCallTitle").hide();
                $("#reqCallThankYou").show();
                $(".uc-pg-thank-you").show();
                $("#formDone").show();
            });
            $("div.m-defaultAlert-window-new").click(function () {
                $("div.uc-pg-thumb").slideUp().removeClass("hide");
                $(this).hide();
                $("div.uc-pg-text").show();
                $('div.m-carousel').carousel();
            });
            if (typeof cwUsedTracking !== 'undefined') {
                cwUsedTracking.setEventCategory(cwUsedTracking.eventCategory.UsedPhotoGallery);
            }

        });
        $('li.thumb').click(function () {
            var pos = $(this).index();
            var curPos = $('.m-active').index();
            var count = 0;
            var btnType;
            if (pos > curPos) {
                count = pos - curPos;
                btnType = "next";
            }
            else {
                count = curPos - pos;
                btnType = "prev";
            }
            while (count != 0) {
                $('div.m-item').removeClass('m-active');
                $('a[data-slide="' + btnType + '"]').trigger('click');
                count--;
            }
            $('div.m-item').eq(pos).addClass('m-active');
        });
        // callActionBox function
        function callActionBox() {
            var winHeight = $(window).height();
            if (winHeight > 470) {
                $(".call-action-box, .pg-detailbtn").show();
            } else {
                $(".call-action-box, .pg-detailbtn").hide();
            }
        }

        //after window resize set carousel height & width
        function resizeCarousel() {
            var width, height, sliderCss
            width = window.innerWidth ? window.innerWidth : $(window).width();
            height = window.innerHeight ? window.innerHeight : $(window).height();
            sliderCss = {
                width: width,
                height: height
            };
            $("#m-uc-gallery").css(sliderCss);
        }
    </script>
     <% Carwale.UI.NewCars.RecommendCars.RazorPartialBridge.RenderPartial("~/Views/Shared/_OutbrainScriptView.cshtml"); %>
    <% Carwale.UI.NewCars.RecommendCars.RazorPartialBridge.RenderPartial("~/Views/Shared/_FacebookScriptView.cshtml"); %>
    
    <div id="chatPopup"></div>
    
    <div id="cw_loading_icon" class="speedometer__container hide">
        <div class="speedometer-loader__content">
            <div class="speedometer-loader"></div><div class="speedometer-needle-circle">
            </div>
        </div>
    </div>

    <script type="text/javascript" src="/Static/js/chatApplozic/jquery.min.js" defer></script>
    <script type="text/javascript" src="/Static/js/chatApplozic/applozic.plugins.min.js" defer></script>
    <script type="text/javascript" src="/Static/js/chatApplozic/applozic.widget.min.js" defer></script>
    <script type="text/javascript" src="https://maps.google.com/maps/api/js?key=AIzaSyDKfWHzu9X7Z2hByeW4RRFJrD9SizOzZt4&libraries=places" defer></script>
    <script type="text/javascript" src="/Static/js/chatApplozic/locationpicker.jquery.min.js" defer></script>
    <script type="text/javascript" src="/Static/js/chatApplozic/applozic.chat.js" defer></script>
    <script type="text/javascript" src="/Static/js/chatApplozic/applozic.sidebox.js" defer></script>
</body>
</html>
