<%@ Control Language="C#" AutoEventWireUp="false"%>
<%
    if ((HttpContext.Current.Request.ServerVariables["HTTP_USER_AGENT"] ?? string.Empty).ToString().ToLower().IndexOf("android") > -1 && Request.Cookies["AndroidDownload"] == null && (HttpContext.Current.Request.ServerVariables["HTTP_USER_AGENT"] ?? string.Empty).ToString().ToLower().IndexOf("windows") < 0 && !(Request.Url.ToString().Contains("/m/used/")))
//if(Request.Cookies["AndroidDownload"] == null)
{
%>
<meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <meta name="viewport" content="width=device-width, initial-scale=1.0, maximum-scale=1.0, minimum-scale=1.0, user-scalable=no" />
    <link type="text/css" href="https://st.aeplcdn.com/v2/m/css/Mobile-pop-up.css?20160419032055" rel="stylesheet"/>
<style type="text/css">
	/* Popup css starts here */
	.cwm-app-popup { border:3px solid #218ac3; padding:10px; width:200px; position:fixed; top:50%; left:50%; margin-top: -90px;  margin-left: -112px; background:#fff; font-family:arial; z-index: 1010; }
	.cwm-app-popup h2 { color:#218ac3; font-size:20px; font-weight:normal; margin-bottom:10px; text-align:center !important; }
	.cwm-app-popup-tb { margin-bottom:15px; margin-left:10px;}
	.cwm-app-popup-logo { float:left; margin-right:7%; padding-top:3px;}
	.cwm-app-popup-txt { display:table-cell; }
	.cwm-app-popup-skip { float:left; margin-right:5%; }
	.cwm-app-popup-skip a { display:inline-block; background:#fff; color:#949798; border:1px solid #c0c0c0;
	text-decoration:none; padding:7px 15px; font-size:12px; text-transform: uppercase; border-radius:3px; -moz-border-radius:3px; /* For FF */ -webkit-border-radius:3px; /* For Safari and Chrome */ -o-border-radius:3px; /* For Opera */ -ms-border-radius:3px; /* For IE */ }
	.cwm-app-popup-skip a:hover, .cwm-app-popup-skip a:focus { text-decoration:none; color:#666; background:#f1f0f0; cursor: pointer; }
	.cwm-app-popup-down { float:right;  }
	.cwm-app-popup-down a, .cwm-app-popup-down a:visited { display:inline-block; background:#218ac3; color:#fefefe; border:1px solid #0f6594;
	text-decoration:none; padding:7px 15px;  font-size:12px; text-transform: uppercase; border-radius:3px; -moz-border-radius:3px; /* For FF */ -webkit-border-radius:3px; /* For Safari and Chrome */ -o-border-radius:3px; /* For Opera */ -ms-border-radius:3px; /* For IE */ }
	.cwm-app-popup-down a:hover, .cwm-app-popup-down a:focus { text-decoration:none; color:#fff; background: #0f6695; }
	.cwm-app-popup-overflow {overflow:hidden;}
	/* Popup css ends here */
</style>

<div class="blackout-window"></div>
<div class="cwm-app-popup">
    	<h2>Get CarWale App</h2>
    	<div class="cwm-app-popup-overflow">
        	<div class="cwm-app-popup-tb">
				<div class="cwm-app-popup-logo"><a href="https://play.google.com/store/apps/details?id=com.carwale&referrer=utm_source%3Dmsitepopup"><img src="https://img1.aeplcdn.com/m/images/carwale-mlogo.png" alt="" title="" border="0" /></a></div>
				<div class="cwm-app-popup-txt"><strong>New Cars,<br> Used Cars,<br> On Road price</strong><br> &amp; much more</div>
				<div class="clear"></div>
            </div>
			<div class="clear"></div>
			<div class="cwm-app-popup-overflow" style="margin-bottom:5px; text-align:center;">
				<div class="cwm-app-popup-skip"><a>Skip</a></div>
				<div class="cwm-app-popup-down"><a target="_blank" href="https://play.google.com/store/apps/details?id=com.carwale&referrer=utm_source%3Dmsitepopup">Download App</a></div>
				<div class="clear"></div>
            </div>
			<div class="clear"></div>     
        </div>
        <div class="clear"></div>        
    </div>

<%
 } 
%>

<script>
    $(document).ready(function () {
        $(".cwm-app-popup-skip").click(function () {
            $(".cwm-app-popup").hide();
            $(".blackout-window").hide();
            var expiryTime = 1000 * 60 * 60 * 24 * 30;  //30 days
            //var expiryTime = 60000;  //1 min
            var expires = new Date((new Date()).valueOf() + expiryTime);
            document.cookie = "AndroidDownload=1;expires=" + expires.toUTCString() + ";path=/";
        });
    });
</script>