<%@ Import Namespace="Carwale.UI.NewCars.RecommendCars" %>
<script language="c#" runat="server">
    public bool ShowBottomAd = true;
</script>
    <%if(ShowBottomAd) {%>
        
            <div id="divBottomAdBar" class="ad-div">
                <!-- /1017752/CarWale_Mobile_ROS_300x250 -->
                <div id='div-gpt-ad-1435297201507-0' class="text-center margin-top10 margin-bottom20">
                    <script type='text/javascript'>
                        googletag.cmd.push(function () { googletag.display('div-gpt-ad-1435297201507-0'); });
                    </script>
                </div>
            </div>
       
    <%;} %>
    <noscript id="pqcity_popup">
        <script type="text/html" src="/static/m/ui/pqcitypopup.html"></script>
    </noscript>
<script>
        var abTestKey = "<%= System.Configuration.ConfigurationManager.AppSettings["AbTestVersion"] %>";
        var abTestKeyMaxValue = "<%= System.Configuration.ConfigurationManager.AppSettings["AbTestKeyMaxValue"] %>";
        var abTestKeyMinValue = "<%= System.Configuration.ConfigurationManager.AppSettings["AbTestKeyMinValue"] %>";
        var askingAreaCityId = [<%= System.Configuration.ConfigurationManager.AppSettings["AskingAreaCityIds"] %>];        
 </script>
<footer class="bg-footer padding-top30 padding-bottom15">
        <!-- Footer section code starts here -->
        <div class="container">
            <div class="grid-12 text-center">
                 <div class="footer-logo-section border-solid-bottom margin-bottom5">
                    <div class="inline-block margin-right15">
                        <a href="/m/" class="cw-logo-svg"></a>
                        <p class="text-medium-grey margin-left15 font11 ask-expert-text">ask the experts</p>
                    </div>
                    <div class="inline-block margin-right15">
                        <a href="https://m.cartrade.com/" target="_blank" class="ct-logo-svg"></a>
                    </div>
                    <div class="inline-block">
                        <a href="https://www.bikewale.com/m/" target="_blank" class="bw-logo-svg"></a>
                    </div>
                </div>
                <div>
                     <ul>
                        <li><a href="/m/forums/">Forums</a></li>
                        <li><a href="/m/social/">Social Hub</a> </li>
                        <!--<li><a href="/aboutus.aspx">About Us</a></li>
                        <li><a href="/career.aspx">Careers</a></li>
                        <li><a href="/sitemap.aspx">Sitemap</a></li>-->
                    </ul>
                </div>

                <div class="app-download-menu padding-left15 padding-top5">
                    <div class="margin-bottom10 text-white">Download Mobile App</div>
                    <div class="app-link-container">
                    <a target="_blank" href="https://itunes.apple.com/in/app/carwale/id910137745?mt=8"><img src="https://imgd.aeplcdn.com/0x0/cw/static/icons/app-store-img.png" alt="Download App" title="Download App"></a>
                    <a target="_blank" href="https://play.google.com/store/apps/details?id=com.carwale&referrer=utm_source%3DCarWaleMsite%26utm_medium%3DFooter%26utm_campaign=CarWale%2520MobilesiteFooter"><img src="https://imgd.aeplcdn.com/0x0/cw/static/icons/google-store-img.png" alt="Download App" title="Download App"></a>
                </div>
                </div>
                <div class="border-solid-top text-white font11 margin-top5 padding-top10">
                    <div class="grid-4 alpha text-left">&copy; CarWale India</div>
                    <div class="grid-8 omega text-right">
                        <a href="/visitoragreement.aspx" class="text-white">Visitor Agreement </a>&
                        <a href="/privacypolicy.aspx" class="text-white">Privacy Policy</a>
                    </div>
                </div>
                <div class="clear"></div>
            </div>
            <div class="clear"></div>
        </div>
    <noscript id="advantage_popup">
	<script type="text/html" src="/static/m/ui/advantagecitypopup.html"></script>
	</noscript>
    <% RazorPartialBridge.RenderPartial("~/Views/Shared/_GoogleAdRemarketingConversionScript.cshtml"); %>  
    </footer>
       