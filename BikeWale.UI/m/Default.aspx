<%@ Page Language="C#" AutoEventWireup="false" Inherits="Bikewale.Mobile.MobileDefault" Async="true" Trace="false" %>
<%@ Import Namespace="Bikewale.Common" %>
<%
    title = "New Bikes, Used Bikes, Bike Prices, Reviews & Photos in India";
    keywords = "new bikes, used bikes, buy used bikes, sell your bike, bikes prices, reviews, photos, news, compare bikes, Instant Bike On-Road Price";
    description = "BikeWale - India's favourite bike portal. Find new and used bikes, buy or sell your bikes, compare new bikes prices & values.";
    canonical = "http://www.bikewale.com";
    AdPath = "/1017752/Bikewale_Mobile_Homepage";
    AdId = "1398766000399";
    menu = "1";
    Ad_HP_Banner_400x310 = "";
%>
<!-- #include file="/includes/headermobile_home.aspx" -->

    <%--<form id="form1" runat="server">--%>
    <!-- M Carousel code start here-->
        <% if (String.IsNullOrEmpty(Ad_HP_Banner_400x310))
           { %>
        <div class="m-carousel m-fluid m-carousel-photos">            
            <div class="m-carousel-inner">
                <asp:Repeater Id="rptFeaturedArticles" runat="server">
                    <itemtemplate>
                        <div class="m-item">
                            <%--<img src="<%# ImagingFunctions.GetPathToShowImages(DataBinder.Eval(Container.DataItem, "LargePicUrl").ToString(), DataBinder.Eval(Container.DataItem, "HostUrl").ToString()) %>" title="<%# DataBinder.Eval(Container.DataItem, "Title").ToString() %>" alt="<%# DataBinder.Eval(Container.DataItem, "Title").ToString() %>" />--%>
                            <img src="<%# Bikewale.Utility.Image.GetPathToShowImages(DataBinder.Eval(Container.DataItem, "OriginalImgUrl").ToString(), DataBinder.Eval(Container.DataItem, "HostUrl").ToString(),Bikewale.Utility.ImageSize._476x268) %>" title="<%# DataBinder.Eval(Container.DataItem, "Title").ToString() %>" alt="<%# DataBinder.Eval(Container.DataItem, "Title").ToString() %>" />
                            <div class="title-text">
                                <a href="<%# GetFeaturedArticlesLink(DataBinder.Eval(Container.DataItem, "ArticleUrl").ToString(), Convert.ToInt32(DataBinder.Eval(Container.DataItem, "BasicId")), Convert.ToUInt16(DataBinder.Eval(Container.DataItem, "CategoryId"))) %>">
                                    <%# DataBinder.Eval(Container.DataItem, "Title").ToString() %>
                                </a>
                            </div>
                        </div>
                    </itemtemplate>
                </asp:Repeater>
            </div>                    
            <div class="m-carousel-controls m-carousel-hud">
                <a class="m-carousel-prev" href="#" data-slide="prev">Previous</a>
                <a class="m-carousel-next" href="#" data-slide="next">Next</a>
            </div>
            <div class="m-carousel-controls m-carousel-bulleted">
                <a href="#" data-slide="1">1</a>
                <a href="#" data-slide="2">2</a>
                <a href="#" data-slide="3">3</a>
                <a href="#" data-slide="4">4</a>
            </div>
        </div>
        <% } else { %>
        <div>
            <!-- BikeWale_Mobile_FeaturedBike_400x310 -->
            <script src="http://www.googletagservices.com/tag/js/gpt.js">
               googletag.pubads().definePassback(
                  '/1017752/BikeWale_Mobile_Featured_Bike_400x310', [400, 310])
                  .setClickUrl("%%CLICK_URL_UNESC%%")
                  .display();
            </script>
        </div>
        <% } %>
        <!-- M Carousel code end here-->
        <!-- Primary nav code starts here-->
        <div id="nav">
            <ul>
                <!--<li><a href="#">Home <span class="bw-sprite right-arrow"></span></a></li>-->
                <li class="link-shadow"><a href="/m/new/">New Bikes <span class="bw-sprite right-arrow"></span></a></li>
                <li class="link-shadow"><a href="/m/comparebikes/">Compare Bikes <span class="bw-sprite right-arrow"></span></a></li>
                <!--<li><a href="#">Used BIkes <span class="bw-sprite right-arrow"></span></a></li>
                <li><a href="#">Compare Bikes <span class="bw-sprite right-arrow"></span></a></li>-->
                <li class="link-shadow"><a href="/m/bikebooking/">Book Your Bike <span class="bw-sprite right-arrow"></span></a></li>
                <li class="link-shadow"><a href="/m/pricequote/">On-Road Price <span class="bw-sprite right-arrow"></span></a></li>
                <li class="link-shadow"><a href="/m/upcoming-bikes/">Upcoming Bikes <span class="bw-sprite right-arrow"></span></a></li>
                <li class="link-shadow"><a href="/m/new-bikes-launches/">Recent Launches <span class="bw-sprite right-arrow"></span></a></li>
                <li class="link-shadow"><a href="/m/news/">News <span class="bw-sprite right-arrow"></span></a></li>
                <li class="link-shadow"><a href="/m/road-tests/">Road Tests <span class="bw-sprite right-arrow"></span></a></li>
                <li class="link-shadow"><a href="/m/features/">Features <span class="bw-sprite right-arrow"></span></a></li>
                  <li class="link-shadow"><a href="/m/user-reviews/">User Reviews<span class="bw-sprite right-arrow"></span></a></li>
                <li class="link-shadow"><a href="/m/new/locate-dealers/">Locate Dealers <span class="bw-sprite right-arrow"></span></a></li>
                <!--<li><a href="#">Sell Bikes <span class="bw-sprite right-arrow"></span></a></li>                      
                <li><a href="#">Forums <span class="bw-sprite right-arrow"></span></a></li>-->
            </ul>
        </div>
        <!-- Primary nav code ends here-->
    <%--</form>--%>
<!-- #include file="/includes/footermobile_home.aspx" -->
