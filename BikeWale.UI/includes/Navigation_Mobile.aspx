<%@ Register Src="~/m/controls/LogInOutControl.ascx" TagPrefix="BW" TagName="Login" %>

<nav id="nav">
    <%--<% if(Bikewale.Common.CurrentUser.UserId > 0) { %>
    <div id="user-banner-content">
        <div class="user-details-content">
            <div class="image-content">
            </div>
            <div class="details-content text-white">
                <p class="font18 text-bold"><%=Bikewale.Common.CurrentUser.Name%></p>
                <p class="font12"><%=Bikewale.Common.CurrentUser.Email %></p>
            </div>
        </div>
    </div>
    <% } %>--%>

    <!-- nav code starts here -->
    <ul class="navUL padding-top10">
        <li>
            <a href="/m/">
                <span class="bwmsprite home-icon"></span>
                <span class="navbarTitle">Home</span>
            </a>
        </li>
        <li>
            <a href="javascript:void(0)" rel="nofollow">
                <span class="bwmsprite newBikes-icon"></span>
                <span class="navbarTitle">New Bikes</span>
                <span class="nav-drop bwmsprite fa-angle-down"></span>
            </a>
            <ul class="nestedUL">
                <li><a href="/m/new-bikes-in-india/">Find New Bikes</a></li>
                <li><a href="/m/comparebikes/">Compare Bikes</a></li>
                <li><a href="/m/pricequote/">Check On-Road Price</a></li>
                <li><a href="/m/dealer-showroom-locator/">Locate Dealer</a></li>
                <li><a href="/m/bike-service-center/">Locate Service Center</a></li>
                <li><a href="/m/upcoming-bikes/">Upcoming Bikes</a></li>
                <li><a href="/m/new-bike-launches/">New Launches</a></li>
                <li><a href="/m/bikebooking/">Book Your Bike</a></li>
            </ul>
        </li>
        <li>
            <a href="javascript:void(0)" rel="nofollow">
                <span class="bwmsprite usedBikes-icon"></span>
                <span class="navbarTitle">Used Bikes</span>
                <span class="nav-drop bwmsprite fa-angle-down"></span>
            </a>
            <ul class="nestedUL">
                <li><a href="/m/used/">Find Used Bikes</a></li>
                <li><a href="/m/used/bikes-in-india/">All Used Bikes</a></li>
            </ul>
        </li>
        <li>
            <a href="/m/used/sell/">
                <span class="bwmsprite sellBikes-icon"></span>
                <span class="navbarTitle">Sell Your Bike</span>
            </a>
        </li>
        <li>
            <a href="javascript:void(0)" rel="nofollow">
                <span class="bwmsprite reviews-icon"></span>
                <span class="navbarTitle">Reviews, News & Videos</span>
                <span class="nav-drop bwmsprite fa-angle-down"></span>
            </a>
            <ul class="nestedUL">
                <li><a href="/m/news/">News</a></li>
                <li><a href="/m/expert-reviews/">Expert Reviews</a></li>
                <li><a href="/m/user-reviews/">User Reviews</a></li>
                <li><a href="/m/features/">Features</a></li>
                <li><a href="/m/bike-care/">Bike Care</a></li>
                <li><a href="/m/bike-videos/">Videos</a></li>
                
            </ul>
        </li>
        <li>
            <a href="/m/trackday2016/">
                <span class="bwmsprite track-day"></span>
                <span class="navbarTitle">Track Day 2016</span>
            </a>
        </li>
        <%--<li>
            <a href="javascript:void(0)">
                <span class="bwmsprite newBikes-icon"></span>
                <span class="navbarTitle">Bike Booking</span>
                <span class="nav-drop bwmsprite fa-angle-down"></span>
            </a>
            <ul class="nestedUL">
                <li><a href="/m/bikebooking/">Book Your Bike</a></li>
                <li><a href="/m/pricequote/rsaofferclaim.aspx">Claim Your Offer</a></li>
                <li><a href="/m/bikebooking/cancellation.aspx">Cancel Your Booking</a></li>
            </ul>
        </li>
        <li>
            <a href="/m/insurance/">
                <span class="bwmsprite insurance-icon"></span>
                <span class="navbarTitle">Insurance</span>
            </a>
        </li>--%>
        <BW:Login ID="ctrlLogin" runat="server" />
    </ul>

    <div id="nav-app-content">
        <p class="font12 text-bold inline-block">India’s #1 Bike Research Destination</p><a href="https://play.google.com/store/apps/details?id=com.bikewale.app&referrer=utm_source%3DMobilesite%26utm_medium%3DDrawer%26utm_campaign=BikeWale%2520MobilesiteDrawer" target="_blank" class="btn btn-orange nav-app-install-btn font12 text-bold inline-block" rel="nofollow">Install</a>
    </div>
</nav>
<!-- nav code ends here -->
