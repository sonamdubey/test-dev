<script src="<%= staticUrl != "" ? "http://st2.aeplcdn.com" + staticUrl : "" %>/src/lscache.min.js?<%= staticFileVersion%>"></script>

<BW:MPopupWidget runat="server" id="PopupWidget" />

<% if(Ad_Bot_320x50){ %>
<section>            
    <div class="container">
        <div>
            <!-- #include file="/ads/Ad320x50_Bottom_mobile.aspx" -->
        </div>
    </div>
</section>
<% } %>
<footer class="bg-footer padding-top30 padding-bottom15"><!-- Footer section code starts here -->
        <div class="container">
        	<div class="grid-12 text-center">
                <div>
                    <a href="/m/" class="bwmsprite bw-logo"></a>              
                </div>
                <div class="margin-top15">
                    <ul>
                        <li><a href="/m/contactus.aspx" rel="nofollow">Contact Us</a></li>
                        <li><a href="/m/advertisewithus.aspx" rel="nofollow">Advertise with Us</a></li>
                        <li><a href="http://www.carwale.com/m/">CarWale.com</a></li>
                        <%--<li><a href="/m/aboutus.aspx">About Us</a></li>--%>
                        <%--<li><a href="/forums/">Forums</a></li>--%>
                        <%--<li><a href="/m/sitemap.aspx">Sitemap</a></li>--%>
                    </ul>
                    <p class="font13 text-white margin-bottom10">Download Mobile App</p>
                    
                    <div>
                        <a class="bwmsprite google-play-logo" href="https://play.google.com/store/apps/details?id=com.bikewale.app&referrer=utm_source%3DBikeWaleMobileWebsite%26utm_medium%3DFooter%26utm_campaign%3DBikeWale%2520MobileWebsite%2520Footer" target="_blank" class="bwsprite gplay-icon margin-right5" rel="nofollow">
                        </a>
                    </div>
                </div>
                <div class="margin-top15 margin-bottom15">
                	<a href="/?site=desktop" target="_blank" class="text-white">View Desktop Version</a>
                </div>
                
            </div>
            <div class="clear"></div>
        </div>
        <div class="border-solid-top text-white font11 margin-top5 padding-top10 grid-12">
                    <div class="grid-4 alpha text-left opacity50">&copy; BikeWale India</div>
                    <div class="grid-8 omega text-right">
                        <a href="http://www.bikewale.com/visitoragreement.aspx" class="text-white" rel="nofollow">Visitor Agreement </a>&
                        <a href="http://www.bikewale.com/privacypolicy.aspx" class="text-white" rel="nofollow">Privacy Policy</a>
                    </div>
                </div>
            	<div class="clear"></div>
    </footer> <!-- Ends here -->