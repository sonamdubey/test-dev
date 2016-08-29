<BW:PopupWidget runat="server" id="PopupWidget" />
<%@ Register Src="~/controls/LoginControlNew.ascx" TagPrefix="BW" TagName="Login" %>
<% if(isAd970x90Shown){ %>
<section>
        <!-- #include file="/ads/Ad970x90_Bottom.aspx" -->
</section>
<% } %>
<div class="blackOut-window"></div>
<!-- #include file="/includes/Navigation.aspx" -->
<div class="globalcity-popup rounded-corner2 hide" id="globalcity-popup"><!-- global city pop up code starts here -->
    <div class="globalcity-popup-data text-center">
        <div class="globalcity-close-btn position-abt pos-top10 pos-right10 bwsprite cross-lg-lgt-grey cur-pointer"></div>
        <div class="icon-outer-container rounded-corner50 margin-bottom20">
            <div class="icon-inner-container rounded-corner50">
                <span class="bwsprite cityPopup-icon margin-top15"></span>
            </div>
        </div>
        <p class="font20 margin-bottom15">Please tell us your city</p>
        <p class="text-light-grey margin-bottom15">This allows us to provide relevant content for you.</p>
        <div class="form-control-box globalcity-input-box">
            <div class="margin-bottom20">
                <span class="position-abt pos-right15 pos-top15 cwmsprite cross-sm-dark-grey cur-pointer"></span>
                <input type="text" class="form-control padding-right30" name="globalCityPopUp" placeholder="Type to select city" id="globalCityPopUp">
                <span class="fa fa-spinner fa-spin position-abt pos-right10 pos-top15 text-black" style="display:none"></span>
                <span class="bwsprite error-icon hide"></span>
                <div class="bw-blackbg-tooltip hide">Please enter your city</div>
            </div>
        </div>
        <div>
            <a id="btnGlobalCityPopup" class="btn btn-orange font18">Confirm city</a>
        </div>
    </div>
    <div class="clear"></div>
</div>
<footer class="bg-footer padding-top40 padding-bottom20">
    <!-- Footer section code starts here -->
    <div class="container">
        <div class="grid-4">
            <a href="/" class="bwsprite bw-logo"></a>
            <p class="font18 text-white margin-top50 margin-bottom20">Join us on</p>
            <div class="footer-social-icons">
                <a href="https://www.facebook.com/Bikewale.Official" target="_blank" title="Facebook" class="margin-right30 fa-fb" rel="nofollow">
                    <span class="bwsprite fa-facebook"></span>
                </a>
                <a href="https://twitter.com/bikewale" target="_blank" title="Twitter" class="margin-right30 fa-tw" rel="nofollow">
                    <span class="bwsprite fa-twitter"></span>
                </a>
                <a href="https://plus.google.com/115751055341108541383/posts" target="_blank" class="fa-gp" title="Google+" rel="nofollow">
                    <span class="bwsprite fa-google-plus"></span>
                </a>
            </div>
        </div>
        <div class="grid-2">
            <p class="font18 text-white margin-bottom20">Company</p>
            <ul>
                <li><a href="/sitemap.aspx">Sitemap</a></li>
                <li><a target="_blank" href="http://www.carwale.com">CarWale.com</a></li>
                <li><a href="/contactus.aspx">Contact Us</a></li>
            </ul>
        </div>
        <div class="grid-3">
            <ul class="margin-top45">
                <li><a href="/advertisewithus.aspx">Advertise with us</a></li>
                <li><a href="/aboutus.aspx">About Us</a></li>
                <li><a href="http://www.bikewale.com/m/">View Mobile Version</a></li>
            </ul>
        </div>
        <div class="grid-3">
            	<p class="font18 text-white margin-bottom30">Download Mobile App</p>
                <div class="margin-bottom15">
                            <a href="https://play.google.com/store/apps/details?id=com.bikewale.app&referrer=utm_source%3DDesktopsite%26utm_medium%3DFooter%26utm_campaign%3DBikeWale%2520Desktopsite%2520Footer" target="_blank" class="bwsprite gplay-icon margin-right5" rel="nofollow">
			                </a>
                </div>
        </div>
        <div class="clear"></div>
        <div class="border-solid-top text-white margin-top25 padding-top25">
            <div class="grid-6 alpha font16">&copy; BikeWale India</div>
            <div class="grid-6 omega text-right font13">
                <a href="/visitoragreement.aspx" class="text-white" rel="nofollow">Visitor Agreement </a>&
                <a href="/privacypolicy.aspx" class="text-white" rel="nofollow">Privacy Policy</a>
            </div>
            <div class="clear"></div>
        </div>
        <div class="clear"></div>
    </div>
</footer>
<!-- Ends here -->
<BW:Login ID="ctrlLogin" runat="server" />

<!-- #include file="/includes/footerscript.aspx" -->
</form>
</body>
</html>