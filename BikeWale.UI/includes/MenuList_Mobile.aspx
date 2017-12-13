<div data-role="panel" id="nav-panel" data-position="left" data-display="overlay" data-theme="a">
    <ul id="menuList">
        <li><a menu="1" href="/m/">Home</a></li>
        <li><a menu="2" href="/m/new-bikes-in-india/">New Bikes</a></li>
        <li><a menu="11" href="/m/comparebikes/">Compare Bikes</a></li>
        <!--<li><a href="#">Used BIkes</a></li>
        <li><a href="#">Compare Bikes</a></li>-->
        <li><a menu="13" href="/m/bikebooking/">Book Your Bike</a></li>
        <li><a menu="3" href="/m/pricequote/">On-Road Price</a></li>
        <li><a menu="4" href="/m/upcoming-bikes/">Upcoming Bikes</a></li>
        <li><a menu="5" href="/m/new-bike-launches/">Recent Launches</a></li>
        <li><a menu="6" href="/m/news/">News</a></li>
        <li><a menu="7" href="/m/expert-reviews/">Expert Reviews</a> </li>
        <li><a menu="8" href="/m/features/">Features</a> </li>
        <li><a menu="9" href="/m/user-reviews/">User Reviews</a></li>
        <li><a menu="10" href="/m/dealer-showrooms/">Locate Dealers</a> </li>
        <li><a menu="12" href="/m/pricequote/rsaofferclaim.aspx">Claim Your Offer</a></li>
        <!--<li><a href="#">Sell Bikes</a></li>
        <li><a href="#">News</a></li>
        <li><a href="#">Forums</a></li>-->
    </ul>
</div>

<script type="text/javascript">
    var globalmenuId = '<%= menu%>';
    //alert(globalmenu);
    
    $('#menuList li a').each(function(){
        //alert($(this).attr('menu'));
        $(this).removeClass('active');
        if (globalmenuId == $(this).attr('menu'))
        {
            $(this).addClass('active');
        }
    });
    
</script>