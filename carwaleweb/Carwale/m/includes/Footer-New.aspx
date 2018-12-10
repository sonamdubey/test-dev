<script language="c#" runat="server">
    public string google_dynx_itemid = "", google_dynx_itemid2 = "", google_dynx_pagetype = "", google_dynx_totalvalue = "";
</script>
        <div class="inner-container padding10 dark-shadow" id="more-tab">
            <span class="cw-m-sprite more floatleft"></span>
            <span class="gray">More</span>
            <span id="f-nav-icon" class="cw-m-sprite more-arrow floatright"></span>
        </div>
        <div id="more-nav" class="hide">
            <ul>
                <li>
                    <span><a href="/m/new/" >New Cars</a></span>
                    <span class="cw-m-sprite more-nav-arrow floatright"></span>
                </li>
                <li>
                    <span><a href="/m/used/" >Used Cars</a></span>
                    <span class="cw-m-sprite more-nav-arrow floatright"></span>
                </li>
                <li>
                    <span><a href="/m/comparecars/" >Compare Cars</a></span>
                    <span class="cw-m-sprite more-nav-arrow floatright"></span>
                </li>
                <li>
                    <span><a href="/quotation/landing/" >On-Road Price</a></span>
                    <span class="cw-m-sprite more-nav-arrow floatright"></span>
                </li>
                <li>
                    <span><a href="/m/research/locatedealerpopup.aspx" onclick="dataLayer.push({ event: 'locate_dealer_section', cat: 'bottom_more', act: 'locate_dealer_link'})" >Locate Dealer</a></span>
                    <span class="cw-m-sprite more-nav-arrow floatright"></span>
                </li>
                <li>
                    <span><a href="/m/upcoming-cars/" >Upcoming Cars</a></span>
                    <span class="cw-m-sprite more-nav-arrow floatright"></span>
                </li>
                <li>
                    <span><a href="/m/offers/" >Offers </a></span>
                    <span class="cw-m-sprite more-nav-arrow floatright"></span>
                </li>
                <li>
                    <span><a href="/used/sell/" >Sell Car</a></span>
                    <span class="cw-m-sprite more-nav-arrow floatright"></span>
                </li>
                <li>
                    <span><a href="/m/news/" >News</a></span>
                    <span class="cw-m-sprite more-nav-arrow floatright"></span>
                </li>
                <li>
                    <span><a href="/m/pitstop/" >PitStop</a></span>
                    <span class="cw-m-sprite more-nav-arrow floatright"></span>
                </li>
                <li>
                    <span><a href="/m/forums/" >Forums</a></span>
                    <span class="cw-m-sprite more-nav-arrow floatright"></span>
                </li>
                 <li>
                    <span><a href="/m/insurance/" >Insurance</a></span>
                    <span class="cw-m-sprite more-nav-arrow floatright"></span>
                </li>
                 <li>
                    <span><a href="/m/social/" >Join the conversation on our Social Hub</a></span>
                    <span class="cw-m-sprite more-nav-arrow floatright"></span>
                </li>
            </ul>
        </div>
        <!--More tab ends here-->
        <%--<%if(ShowBottomAd) {%>--%>
        <div class="banner-div">
            <div id="divBottomAdBar" class="ad-div">
            <!-- /1017752/CarWale_Mobile_ROS_300x250 -->
            <div id='div-gpt-ad-1435297201507-0' style='height:250px; width:300px;'>
            <script type='text/javascript'>
            googletag.cmd.push(function() { googletag.display('div-gpt-ad-1435297201507-0'); });
            </script>
            </div>
            </div>
        </div>
    <%--<%} %>--%>
        <!--Footer starts here-->
        <div id="footerend" class="footer">
            <div class="floatleft">
            	<a href="/m/" >
                	<span class="cw-m-sprite home"></span>
                	<span class="gray">Home</span>
                </a>
            </div>
            <div class="floatleft">
            	<a href='/m/Feedback.aspx?returnUrl=<%=HttpUtility.UrlEncode(Request.ServerVariables["HTTP_X_REWRITE_URL"])%>'>
                	<span class="cw-m-sprite feedback"></span>
                	<span class="gray">Feedback</span>
                </a>
            </div>
            <div class="floatleft">
                <a href="https://www.carwale.com/default.aspx?site=desktop" class="white-text">View Desktop Site</a>
            </div>
        </div>
        <!--Footer ends here-->
    
        <!--Loader starts here-->
        <div class="hide" id="m-blackOut-window"></div>
        <div class="hide" id="m-newLoading">
            <div class="m-loading-popup">
                <span class="m-loading-icon"></span>
                <div class="clear"></div>
            </div>
        </div>
        <!-- Loader ends here -->
<script type="text/javascript">
    $(document).ready(function(){    
        $("#more-tab").click(function () {
            if ($("#more-nav").hasClass('hide')) {
                $("#more-nav").slideDown().removeClass("hide");
                $("html, body").animate({ scrollTop: $("#more-nav").offset().top }, 1000);
                $("#more-tab").find("#f-nav-icon").removeClass("more-arrow").addClass("more-arrow-down");
            }
            else{
                $("#more-nav").slideUp().addClass("hide");
                $("html, body").animate({ scrollTop: $("#more-tab").offset().top }, 500);
                $("#more-tab").find("#f-nav-icon").removeClass("more-arrow-down").addClass("more-arrow");
            }
        });
    });

</script>
<!-- CW track Code -->
<script>
var cwurl = unescape(window.location), landingURL = cwurl, imgCreation = new Image, hashIndex = cwurl.indexOf("#"); cwurl = cwurl.substr(cwurl.indexOf("?") + 1, -1 == hashIndex ? cwurl.length : cwurl.indexOf("#") - (cwurl.indexOf("?") + 1)), landingURL = landingURL.substr(0, landingURL.indexOf("?")); for (var searchAttributes = cwurl.split("&"), no = 0; no < searchAttributes.length; no++) { var cutSrc = searchAttributes[no].substr(searchAttributes[no].indexOf("ltsrc"), searchAttributes[no].indexOf("=")); if ("ltsrc" == cutSrc) { var qryString = searchAttributes[no].substr(searchAttributes[no].indexOf("ltsrc") + 6, searchAttributes[no].length); imgCreation.src = "/lts/ts.aspx?c=" + qryString + "&refUrl=" + landingURL } }
</script>
<!-- End CW track Code -->
<!-- Google Tag Manager -->
<noscript><iframe src="//www.googletagmanager.com/ns.html?id=GTM-W2Z3ZM"
height="0" width="0" style="display:none;visibility:hidden"></iframe></noscript>
<script>(function (w, d, s, l, i) {
    w[l] = w[l] || []; w[l].push({
        'gtm.start':
        new Date().getTime(), event: 'gtm.js'
    }); var f = d.getElementsByTagName(s)[0],
    j = d.createElement(s), dl = l != 'dataLayer' ? '&l=' + l : ''; j.async = true; j.src =
    '//www.googletagmanager.com/gtm.js?id=' + i + dl; f.parentNode.insertBefore(j, f);
})(window, document, 'script', 'dataLayer', 'GTM-W2Z3ZM');</script>
<!-- End Google Tag Manager -->

<!-- Google Code for Remarketing Tag -->
<!--------------------------------------------------
Remarketing tags may not be associated with personally identifiable information or placed on pages related to sensitive categories. See more information and instructions on how to setup the tag on: http://google.com/ads/remarketingsetup
--------------------------------------------------->
<script type="text/javascript">
    var google_tag_params = {
        dynx_itemid: '<%= google_dynx_itemid %>',
        dynx_CarName: '<%= google_dynx_itemid2 %>',
        dynx_pagetype: '<%= google_dynx_pagetype %>s',
        dynx_totalvalue: '<%= google_dynx_totalvalue %>',
    };
</script>
<script>
    dataLayer.push({
        'event': 'remarketingTriggered',
        'google_tag_params': window.google_tag_params
    });
</script>
<script type="text/javascript">
    /* <![CDATA[ */
    var google_conversion_id = 999894061;
    var google_custom_params = window.google_tag_params;
    var google_remarketing_only = true;
    var google_conversion_format = 3;
    /* ]]> */
</script>
<script type="text/javascript" src="//www.googleadservices.com/pagead/conversion.js"></script>
<script type="text/javascript" src="//www.googleadservices.com/pagead/conversion_async.js"></script>
<noscript>
<div style="display:inline;">
<img height="1" width="1" style="border-style:none;" alt="" src="//googleads.g.doubleclick.net/pagead/viewthroughconversion/999894061/?value=0&amp;guid=ON&amp;script=0"/>
</div>
</noscript>       