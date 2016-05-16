﻿<BW:PopupWidget runat="server" id="PopupWidget" />
<%@ Register Src="~/controls/LoginControlNew.ascx" TagPrefix="BW" TagName="Login" %>
<% if(isAd970x90Shown){ %>
<section>
        <!-- #include file="/ads/Ad970x90_Bottom.aspx" -->
</section>
<% } %>
<footer class="bg-footer padding-top40 padding-bottom20" id="bg-footer"><!-- Footer section code starts here -->
    <div class="container">
        <div class="grid-4">
            <a href="/" class="bwsprite bw-logo"></a>
            <p class="font18 text-white margin-top50 margin-bottom20">Join us on</p>
            <div class="footer-social-icons">
                    <a href="https://www.facebook.com/Bikewale.Official" target="_blank" title="Facebook" class="margin-right30" rel="nofollow">
                    <span class="fa fa-facebook"></span>
                </a>
                    <a href="https://twitter.com/bikewale" target="_blank" title="Twitter" class="margin-right30" rel="nofollow">
                    <span class="fa fa-twitter"></span>
                </a>
                    <a href="https://plus.google.com/115751055341108541383/posts" target="_blank" title="Google+" rel="nofollow">
                    <span class="fa fa-google-plus"></span>
                </a>
            </div>
        </div>
        <div class="grid-2 footer-company-section-one">
            <p class="font18 text-white margin-bottom20">Company</p>
            <ul>
                <li><a href="/sitemap.aspx">Sitemap</a></li>
                <li><a href="http://www.carwale.com">CarWale.com</a></li>
                <li><a href="/contactus.aspx" rel="nofollow">Contact Us</a></li>
            </ul>
        </div>
        <div class="grid-3 footer-company-section-two">
            <ul class="margin-top45">
                <li><a href="/advertisewithus.aspx" rel="nofollow">Advertise with us</a></li>
                <li><a href="/aboutus.aspx" rel="nofollow">About Us</a></li>
                <li><a href="http://www.bikewale.com/m/">View Mobile Version</a></li>
            </ul>
        </div>
        <div class="grid-3 footer-company-section-three">
            <p class="font18 text-white margin-bottom30">Download Mobile App</p>
            <div class="margin-bottom15">
                <a href="https://play.google.com/store/apps/details?id=com.bikewale.app&referrer=utm_source%3DDesktopsite%26utm_medium%3DFooter%26utm_campaign%3DBikeWale%2520Desktopsite%2520Footer" target="_blank" class="bwsprite gplay-icon margin-right5" rel="nofollow"></a>
               </div>
        </div>
        <div class="clear"></div>
        <div class="border-solid-top text-white margin-top25 padding-top25">
            <div class="grid-6 alpha font16">&copy; BikeWale India</div>
            <div class="grid-6 omega text-right font13">
                <a href="/visitoragreement.aspx" class="text-white" rel="nofollow">Visitor Agreement </a>&
                <a href="/privacypolicy.aspx" class="text-white" rel="nofollow">Privacy Policy</a>
            </div>
        </div>
        <div class="clear"></div>
    </div>
</footer><!-- Ends here -->
<BW:Login ID="ctrlLogin" runat="server" />
<script>
    if ($(window).width() < 996 && $(window).width() > 790)
        $("#bg-footer .grid-6").addClass("padding-left30 padding-right30");
</script>
