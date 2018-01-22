<%@ Register Src="~/m/controls/LogInOutControl.ascx" TagPrefix="BW" TagName="Login" %>

<nav id="nav">
	<a href="https://www.bikewale.com/autoexpo2018/" title="AutoExpo 2018 - BikeWale" class="auto-expo-nav bw-ga" data-cat="Other" data-act="AutoExpo_2018_Link Clicked" data-lab="Navigation_Drawer_Link">
		<div class="auto-expo-nav__title">
			<p class="auto-expo-nav__title--sub">Explore</p>
			<p class="auto-expo-nav__title--main">Auto Expo 2018</p>
		</div>
		<div class="auto-expo-nav__logo"></div>
	</a>
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
                <li><a href="/m/dealer-showrooms/">Locate Dealer</a></li>
                <li><a href="/m/service-centers/">Locate Service Center</a></li>
                <li><a href="/m/upcoming-bikes/">Upcoming Bikes</a></li>
                <li><a href="/m/new-bike-launches/">New Launches</a></li>
                <li><a href="/m/bikebooking/">Book Your Bike</a></li>
            </ul>
        </li>
        <li>
            <a href="javascript:void(0)" rel="nofollow">
                <span class="scooter-icon"></span>
                <span class="navbarTitle">New Scooters</span>
                <span class="nav-drop bwmsprite fa-angle-down"></span>
            </a>
            <ul class="nestedUL">
                <li><a href="/m/honda-scooters/">Honda Scooters</a></li>
                <li><a href="/m/hero-scooters/">Hero Scooters</a></li>
                <li><a href="/m/tvs-scooters/">TVS Scooters</a></li>
                <li><a href="/m/yamaha-scooters/">Yamaha Scooters</a></li>
                <li><a href="/m/scooters/">All Scooters</a></li>
            </ul>
        </li>
        <li>
            <a href="javascript:void(0)" rel="nofollow">
                <span class="bwmsprite usedBikes-icon"></span>
                <span class="navbarTitle">Buy & Sell Used Bikes</span>
                <span class="nav-drop bwmsprite fa-angle-down"></span>
            </a>
            <ul class="nestedUL">
                <li><a href="/m/used/">Find Used Bikes</a></li>
                <li><a href="/m/used/bikes-in-india/">All Used Bikes</a></li>
                <li><a href="/m/used/sell/">Sell Your Bike</a></li>
            </ul>
        </li>
        <li>
            <a href="/m/reviews/">
                <span class="reviews-icon"></span>
                <span class="navbarTitle">Reviews</span>
            </a>
        </li>
        <li>
            <a href="javascript:void(0)" rel="nofollow">
                <span class="bwmsprite news-icon"></span>
                <span class="navbarTitle">News, Videos &amp; Tips</span>
                <span class="nav-drop bwmsprite fa-angle-down"></span>
            </a>
            <ul class="nestedUL">
                <li><a href="/m/news/">News</a></li>
                <li><a href="/m/expert-reviews/">Expert Reviews</a></li>
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
        <BW:Login ID="ctrlLogin" runat="server" />
    </ul>

    <div id="nav-app-content">
        <p class="font12 text-bold inline-block">India’s #1 Bike Research Destination</p>
        <a href="https://play.google.com/store/apps/details?id=com.bikewale.app&referrer=utm_source%3DMobilesite%26utm_medium%3DDrawer%26utm_campaign=BikeWale%2520MobilesiteDrawer" target="_blank" rel="noopener nofollow" class="btn btn-orange nav-app-install-btn font12 text-bold inline-block">Install</a>
    </div>
</nav>
<!-- nav code ends here -->
