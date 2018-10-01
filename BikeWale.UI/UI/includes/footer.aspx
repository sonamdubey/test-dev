<% if(isAd970x90Shown){ %>
<section>
        <!-- #include file="/UI/ads/Ad970x90_Bottom.aspx" -->
</section>
<% } %>
        <!--footer code start here -->
        <div class="container_12 padding-bottom20">
        	<div class="grid_8">            	           	
                <BM:BikeMakes ID="ctrl_BikMakes" runat="server" HeaderText="Bike Manufacturers" NoOfColumns="4"/>                
            </div>
            <div class="grid_4">
            	<h2>Related Links</h2>
                <div class="grid_2 alpha omega padding-top10">
                	<ul class="footer-link">                    	
                        <li><a href="/visitoragreement.aspx">Visitor Agreement</a></li>
                        <li><a href="/privacypolicy.aspx">Privacy Policy</a></li>
                        <li><a href="/sitemap.aspx">Sitemap</a></li>
                        <li><a href="https://www.carwale.com" target="_blank" rel="noopener">CarWale.com</a></li>
                    </ul>
                </div>
                <div class="grid_2 padding-top10">
                	<ul class="footer-link">
                        <li><a href="/advertisewithus.aspx">Advertise with us</a></li>
                        <li><a href="/contactus.aspx">Contact Us</a></li>
                        <li><a href="/aboutus.aspx">About Us</a></li>                    	                            
                    </ul>
                </div>
                <div class="grid-3 footer-company-section-three">
                    <p class="font18 text-white margin-bottom30">Download Mobile App</p>
                    <div class="margin-bottom15">
                        <a href="https://play.google.com/store/apps/details?id=com.bikewale.app&referrer=utm_source%3DDesktopsite%26utm_medium%3DFooter%26utm_campaign%3DBikeWale%2520Desktopsite%2520Footer" target="_blank" rel="noopener" class="gplay-icon margin-right5" title="Bikewale App on Google Play">Bikewale App on Google Play</a>
                    </div>
                </div>
            </div>
            <div class="grid_12 center-align margin-top20">
            	&copy;&nbsp;<%=DateTime.Now.Year.ToString()%>.&nbsp; BikeWale.com. All Rights Reserved | <span class="grey-text"><a href="/privacypolicy.aspx">Privacy Policy</a></span> | <span class="grey-text"><a href="/termsconditions.aspx">Terms & Condition</a></span>
            </div>
        </div>
        <!--footer code end here -->               
    </div>
</body>
</html>