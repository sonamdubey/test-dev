<%
    IsShowAd = false;
    ShowBottomAd = false;
%>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml" itemscope itemtype="http://schema.org/WebPage">
<head>
    <title>Certification Terms & Conditions - CarWale</title>
    <!-- #include file="/m/includes/global/head-script.aspx" -->
    <style>
        .list-bullet {
            list-style: disc;
            margin-left: 15px
        }
    </style>
</head>
<body>
    <% if (Request.QueryString["isapp"] != "true") { %>
    <!-- #include file="/m/includes/header.aspx" -->
    <% } %>
    <!--Outer div starts here-->
        <!--Main container starts here-->
        <div id="main-container">
            <h1 class="content-inner-block-10 text-center">Certification Terms and Conditions</h1>
            <div class="inner-container light-shadow content-box-shadow content-inner-block-10 rounded-corner2 margin-bottom10">
                This report is based on the condition of the vehicle on the date of certification and provides an approximate estimate of the condition on that date.
                    <ul class="list-bullet">
                        <li> MXC Solutions India Private Limited (“CarTrade.com”) has relied on the odometer reading for KMs driven as seen at the time of inspection and is not responsible for verifying its genuineness</li>
                        <li>Except where indicated the parts were in working condition - this report does not take into account the condition of the parts which are not visible or not inspected</li>
                        <li>The report is in no way a commitment to the condition of the vehicle</li>
                        <li>All reports are subjective and CarTrade.com and its associates cannot be held liable in any way due to the use of this report</li>
                        <li>We recommend prospective buyers to get the vehicle inspected through their own means before purchase</li>
                    </ul>
            </div>
        <!--Main container ends here-->
        </div>
    <!--Outer div ends here-->
    <% if (Request.QueryString["isapp"] != "true") { %>
    <!-- #include file="/m/includes/footer.aspx" -->
    <!-- #include file="/m/includes/global/footer-script.aspx" -->
    <% } %>
</body>
</html>
