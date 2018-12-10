<%
    new DeviceDetection(Request.ServerVariables["HTTP_X_REWRITE_URL"]).DetectDevice();
     %>
<!doctype html>
<html>
<head>
    <!-- #include file="/includes/global/head-script.aspx" -->
</head>
<body class="header-fixed-inner">
    <form runat="server">
        <!-- #include file="/includes/header.aspx" -->
        <section class="margin-top30 margin-bottom30"><!--  thankyou page code starts here -->
        <div class="container">
       		<div class="grid-12">
            	<div class="content-box-shadow content-inner-block-20">
                	<p class="font30 text-black margin-top40 margin-left30 special-skin-text">Thank you for your valuable feedback.<br />Your input is important to us.</p>
                	<div class="border-solid-top margin-top20 margin-bottom20"></div>
                    <div class="margin-left30 font20">
                    	<p class="font20">Browse for more information on:</p>
                        <p class="margin-top30"><a target="_blank" href="https://www.carwale.com/new/">New cars</a></p>
                        <p class="margin-top30"><a target="_blank" href="https://www.carwale.com/used/">Used cars</a></p>
                        <p class="margin-top30"><a target="_blank" href="https://www.carwale.com/new/prices.aspx">Free on-road price</a></p>
                        <p class="margin-top30"><a target="_blank" href="https://www.carwale.com/upcoming-cars/">Upcoming cars</a></p>
                    </div>
                </div>
            </div>
        	<div class="clear"></div>
        </div>
    </section>
    <div class="clear"></div>
    </form>
    <div class="clear"></div>
    <!-- #include file="/includes/footer.aspx" -->
    <!-- #include file="/includes/global/footer-script.aspx" -->
</body>
</html>
