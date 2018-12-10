<%@ Page Language="C#" AutoEventWireup="true" %>

<%@ Import Namespace="Carwale.UI.Common" %>
<!doctype html>
<html itemscope itemtype="http://schema.org/WebPage">
<head>
    <!-- #include file="/includes/global/head-script.aspx" -->

    <%
        // Define all the necessary meta-tags info here.
        // To know what are the available parameters,
        // check page, headerCommon.aspx in common folder.

        PageId = 1;
        Title = "Server Error";
        Description = "Server Error in CarWale";
        Keywords = "";
        Revisit = "15";
        DocumentState = "Static";
        Response.StatusCode = 500;
    %>
    <script type="text/javascript">
        $('#askexpertsidebanner').css('display', 'none');
    </script>


    <script runat="server">
        protected void Page_Load(object Sender, EventArgs e) {
            DeviceDetection dd = new DeviceDetection(Request.ServerVariables["HTTP_X_REWRITE_URL"].ToString());
            dd.DetectDevice();
        }
    </script>
</head>
<body class="bg-light-grey header-fixed-inner">
    <form>
        <!-- #include file="/includes/header.aspx" -->
        <section class="container">
            <div class="grid-12">
                <div class="margin-bottom20 padding-bottom60 content-inner-block-10 content-box-shadow">

                    <div class="margin-bottom50">

                        <h1 class="margin-bottom20 padding-bottom10 border-solid-bottom">Server Error</h1>
                        <div class="temp-height">
                            <p class="margin-bottom10">Some Error Occured.</p>
                            <p class="margin-bottom10">
                                We are sorry for this inconvenience. Error cause has been sent to the administrators. 
	                    We will resolve the problem very soon.
                            </p>
                            <p class="margin-bottom10">
                                Please follow this 
	                    <span class="text-link" onclick="history.back()">link</span> to go <span class="text-link" onclick="history.back()">Back</span>.
                            </p>
                        </div>

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







