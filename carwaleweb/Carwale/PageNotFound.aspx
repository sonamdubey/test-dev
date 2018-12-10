<%@ Page Language="C#" AutoEventWireup="true" %>

<%@ Import Namespace="WURFL" %>
<%@ Import Namespace="WURFL.Config" %>
<%@ Import Namespace="System.Web.Caching" %>
<%@ Import Namespace="System.Net" %>
<!doctype html>
<html>
<head>
    

    <%
        // Define all the necessary meta-tags info here.
        // To know what are the available parameters,
        // check page, headerCommon.aspx in common folder.

        PageId = 1;
        Title = "404 Page Not Found";
        Description = "404 Page Not Found at CarWale";
        Keywords = "";
        Revisit = "15";
        DocumentState = "Static";
        Response.StatusCode = 404;
    %>
    <!-- #include file="/includes/global/head-script.aspx" -->
    <style>
       ul.normal li { margin: 5px 0 0 15px; padding: 0; list-style-image: url(https://img.carwale.com/images/bullet.gif); font-weight: normal; }
    </style>
    <script runat="server">
        protected void Page_Load(object Sender, EventArgs e)
        {
            if (Carwale.UI.ClientBL.DeviceDetectionManager.IsMobile(new HttpContextWrapper(HttpContext.Current)))
            {
                Server.Transfer("/m/pagenotfound.aspx");
            }
        }
    </script>
</head>
<body class="bg-light-grey header-fixed-inner">
    <form>
        <!-- #include file="/includes/header.aspx" -->
        <section class="container">
            <div class="grid-12">
                <div class="content-box-shadow content-inner-block-10 margin-bottom20 padding-bottom60">
                    <h1 class="border-solid-bottom padding-bottom10">Page Not Found</h1>
                        
                        <div class="margin-top20 temp-height" id="main">
                            <p class="margin-bottom10">Requested page couldn't be found on <a href="/" title="Visit CarWale home page">CarWale</a></p>
                            <p class="margin-bottom10">
                                Possible causes for this inconvenience are:
			                <ul class="normal margin-bottom10">
                                <li>The requested page might have been removed from the server.</li>
                                <li>The URL might be mis-typed by you.</li>
                                <li>Some maintenance process is going on the server.</li>
                            </ul>
                                Please try visiting the page again within few minutes. 
                            </p>
                            <p class="margin-bottom10">
                                Please follow this 
			                <span class="text-link" onclick="window.history.back()">link</span> to go <span  class="text-link" onclick="window.history.back()">Back</span>.
                            </p>
                        </div>
                  </div>
            </div>
            <div class="clear"></div>
        </section>
        <div class="clear"></div>

        <!-- #include file="/includes/footer.aspx" -->
        <!-- all other js plugins -->
        <!-- #include file="/includes/global/footer-script.aspx" -->
    </form>
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





