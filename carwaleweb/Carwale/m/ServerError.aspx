<%@ Page Language="C#" ContentType="text/html" AutoEventWireup="false" trace="false" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<% Response.StatusCode = 500; %>
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
            <div class="pgsubhead" style="position:relative;top:-3px;">Server Error</div>
            <div class="box">
                <div class="new-line5">Some error occured</div>
                <div class="new-line5">We are sorry for this inconvenience. Error cause has been sent to the administrators. We will resolve the problem very soon.</div>
            </div>
        </div>
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
                Common.utils.trackAction('CWNonInteractive', 'ServerError', (typeof (document.referrer) != "undefined" && document.referrer.trim() != "" ? document.referrer : "null"), (location.pathname + location.search + location.hash));
            }

        if (document.attachEvent ? document.readyState === "complete" : document.readyState !== "loading") {
            track404();
        } else {
            document.addEventListener('DOMContentLoaded', track404);
        }
    </script>
</body>
</html>