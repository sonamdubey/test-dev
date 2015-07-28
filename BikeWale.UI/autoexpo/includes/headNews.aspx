<%@ Import Namespace="Bikewale.Common" %>
<%--<%@ Register TagPrefix="Carwale" TagName="UserFeedback" src="/Controls/UserFeedback.ascx" %>--%>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
<title><%=Title%></title>
<meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
<meta name="keywords" content="<%=Keywords%>" />
<meta name="description" content="<%=Description%>" />
<% if(canonical != "") { %> <link rel="canonical" href="<%=canonical%>" /> <% } %>
<link rel="SHORTCUT ICON" href="http://img2.aeplcdn.com/bikewaleimg/images/favicon.png?v=1.0" />
    <link href="/autoexpo/css/style.css?v=1.0" rel="stylesheet" type="text/css" />
<link href="/autoexpo/css/dropdown-menu.css" rel="stylesheet" type="text/css" />
<link href="/autoexpo/css/colorbox.css" rel="stylesheet" type="text/css" />

<script type="text/javascript" src="http://st2.aeplcdn.com/src/jquery-1.7.2.min.js"></script>
<script language="javascript" src="http://st2.aeplcdn.com/src/common/bt.js" type="text/javascript"></script>
<script type="text/javascript" src="/autoexpo/js/dropdown-menu.min.js"></script>
<script type="text/javascript" src="/autoexpo/js/jquery.colorbox.js"></script>
<script src="http://st2.aeplcdn.com/src/jquery.jcarousel.min.js" type="text/javascript"></script>
<!-- for admanager -->
<script type="text/javascript">
    sas_tmstp = Math.round(Math.random() * 10000000000);
    sas_pageid = '32849/228383'; 	// Page : AutoExpo/homepageandros
    var sas_formatids = '10111,10110,11974';
    sas_target = ''; 		// Targeting
    document.write('<scr' + 'ipt src="http://www4.smartadserver.com/call2/pubjall/' + sas_pageid + '/' + sas_formatids + '/' + sas_tmstp + '/' + escape(sas_target) + '?"></scr' + 'ipt>');
</script>
<!-- END OF TAG FOR admanager -->
<script language="c#" runat="server">	
    private int PageId = 0;
	private string Title = "", Description = "", Keywords = "", Revisit = "", DocumentState = "Static";	
	private string OEM = "", BodyType = "", Segment = "";//for keyword based google Ads 
    private string canonical = "";
</script>
    <script type="text/javascript">
        if (typeof sas_manager != 'undefined') {
            sas_manager.render(11974); // Format : DHTML 1x1
        }
    </script>
    <script type="text/javascript">
        $(document).ready(function (e) {
            $('#dropdown-nav .dropdown-menu').dropdown_menu({
                sub_indicators: true,
                drop_shadows: true,
                close_delay: 300
            });
            $(".tabs-list li a").click(function () {
                $(".tabs-list li a").removeClass("active");
                $(".tabs-list li span").removeClass("tail-bottom");
                $(this).addClass("active");
                $(this).siblings().addClass("tail-bottom");
                $(".tabs-data").hide();
                $(".tabs-data").eq($(this).parent().index()).show();
            });
            $(".map-tabs li a").click(function () {
                $(".map-tabs li a").removeClass("active");
                $(".map-tabs li span").removeClass("tail-top");
                $(this).addClass("active");
                $(this).siblings().addClass("tail-top");
                $(".map-data").hide();
                $(".map-data").eq($(this).parent().index()).show();
            });
            $(".pics").colorbox({ rel: 'nofollow' });
            $(".videos").colorbox({
                iframe: true,
                innerWidth: 640,
                innerHeight: 360
            });

            //$('div.carouselh').jsCarousel({
            //    onthumbnailclick: function (src) { },
            //    autoscroll: false,
            //    circular: true, 
            //    masked: false,
            //    itemstodisplay: 4,
            //    orientation: 'h' 
            //});

            $('.gallery').colorbox({ rel: 'gallery' });            
        });
</script>
<!--[if IE 6]>
<script src="/js/ie-png-fix.js"></script>
<script>
  DD_belatedPNG.fix('.icons_bg,.my-cw-icons');/* fix png transparency problem with IE6 */
</script>
<![endif]-->
<meta name="google-site-verification" content="sDaPYXZMVDkLVrZ8i92XCHrACCXDgDV7mj9aimDuBbc" />
</head>
<body id="ros">
    <!-- #include file="/includes/gacode.aspx" -->
    <noscript>
    <a href="http://www4.smartadserver.com/call/pubjumpi/32849/228383/11974/S/[timestamp]/?" target="_blank">
    <img src="http://www4.smartadserver.com/call/pubi/32849/228383/11974/S/[timestamp]/?" border="0" alt="" /></a>
    </noscript>
    <div class="container">
    	<!-- Header code starts here -->
        <div class="header">
        	<div class="cw-logo-posi">
            	<a href="/" target="_blank">
                	<img src="http://img.carwale.com/bikewaleimg/images/bw-logo.png" border="0" alt="BikeWale" title="BikeWale" />
               	</a>
            </div>
            <div class="ae-logo-posi">
            	<a>
                	<img src="/autoexpo/images/auto-expo-logo.png" border="0" alt="AutoExpo 2014" title="AutoExpo 2014" />
                </a>
           	</div>
            <div class="clear"></div>
        </div>
        <!-- Header code ends here -->
        <!-- nav code starts here -->
        <div class="nav">
        	<div id="dropdown-nav">
                <ul class="dropdown-menu dropdown-menu-skin">
                    <li><a <%= PageId == 1 ? "class='active'" : "" %> href="/autoexpo/2014/">Home</a></li>
                    <li><a <%= PageId == 2 ? "class='active'" : "" %> href="/autoexpo/2014/gallery.aspx">Photos</a></li>
                    <li><a <%= PageId == 3 ? "class='active'" : "" %> href="/autoexpo/2014/videos.aspx">Videos</a></li>
                    <li><a <%= PageId == 4 ? "class='active'" : "" %> href="/autoexpo/2014/exhibitorlist.aspx">Exhibitors</a></li>
                    <li>
                    	<a <%= PageId == 5 ? "class='active'" : "" %> href="#">Brands</a>
                    	<ul>
                            <li><a href="/autoexpo/2014/brand.aspx?mid=1">Bajaj</a></li>
                            <li><a href="/autoexpo/2014/brand.aspx?mid=8">Hyosung</a></li>
                            <li><a href="/autoexpo/2014/brand.aspx?mid=5">Harley Davidson</a></li>
                            <li><a href="/autoexpo/2014/brand.aspx?mid=6">Hero</a></li>
                            <li><a href="/autoexpo/2014/brand.aspx?mid=7">Honda</a></li>
                            <li><a href="/autoexpo/2014/brand.aspx?mid=13">Yamaha</a></li>
                            <li><a href="/autoexpo/2014/brand.aspx?mid=10">Mahindra</a></li>
                            <li><a href="/autoexpo/2014/brand.aspx?mid=12">Suzuki</a></li>
                            <li><a href="/autoexpo/2014/brand.aspx?mid=22">Triumph</a></li>
                            <li><a href="/autoexpo/2014/brand.aspx?mid=15">TVS</a></li>
                        </ul>
                    </li>
                    <!--<li><a <%= PageId == 6 ? "class='active'" : "" %> href="#">Specials</a></li>-->
                    <li><a <%= PageId == 7 ? "class='active'" : "" %> href="/autoexpo/2014/showtimings.aspx">Schedule</a></li>
                    <li><a <%= PageId == 9 ? "class='active'" : "" %> href="http://in.bookmyshow.com/concerts/auto-expo-tickets/" target="_blank">Book Tickets</a></li> 
                    <li><a href="http://autoexpo.carwale.com/" target="_blank">Cars</a> </li>                   
                </ul>
            </div>
           <%-- <div class="search-box">
            	<input type="text" name="search" />
                <a class="ae-sprite search-icon"></a>
            </div>--%>
            <div class="clear"></div>
        </div>
        <!-- nav code ends here -->
        <div class="clear"></div>