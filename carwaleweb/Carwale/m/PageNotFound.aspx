<%@ Page Language="C#" ContentType="text/html" AutoEventWireup="false" trace="false" %>
<% Response.StatusCode = 404; %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
<!-- #include file="/m/includes/global/head-script.aspx" -->
<link rel="stylesheet" href="/static/m/css/design.css" type="text/css" >
</head>
<body>
	<!--Outer div starts here-->
    <!-- #include file="/m/includes/header.aspx" -->
	<div data-role="page" >
    	<!--Main container starts here-->
    	<div id="main-container">
		
            <div class="pgsubhead" style="position:relative;top:-3px;">Page Not Found</div>
            <div class="box">
                <div class="new-line5">Requested page couldn't be found on CarWale</div>
                <div class="new-line5"><b>Possible causes for this inconvenience are:</b></div>
                <div class="new-line5">
                    <ul>
                        <li>The requested page might have been removed from the server.</li>
                        <li>The URL might be mis-typed by you.</li>
                        <li>Some maintenance process is going on the server.</li>
                    </ul>
                </div>
                <div class="new-line5">Please try visiting the page again within few minutes. </div>
            </div>
		
        </div>
        <div class="clear"></div>
        <!-- #include file="/m/includes/footer.aspx" -->
        <!-- all other js plugins -->
        <!-- #include file="/m/includes/global/footer-script.aspx" -->
        <!--Main container ends here-->
    </div>
    <!--Outer div ends here-->
    <script>
        function track404() {
            var m = new Date();
            var time = m.toLocaleString();
            Common.utils.trackAction('CWNonInteractive', 'PageNotFound', (typeof (document.referrer) != "undefined" && document.referrer.trim() != "" ? document.referrer : "null"), (location.pathname + location.search + location.hash));
        }

        if (document.attachEvent ? document.readyState === "complete" : document.readyState !== "loading") {
            track404();
        } else {
            document.addEventListener('DOMContentLoaded', track404);
        }
    </script>
</body>
</html>