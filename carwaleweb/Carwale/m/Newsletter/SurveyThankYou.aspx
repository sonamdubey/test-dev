<%@ Import Namespace="Carwale.BL.Experiments" %>
<%@ Import Namespace="Carwale.UI.Common" %>

<!DOCTYPE html>
<html>
<% 
    bool showExperimentalColor = ProductExperiments.IsShowExperimentalColor(CookiesCustomers.AbTest);
%>
<head>
    <!-- #include file="/m/includes/global/head-script.aspx" -->
</head>
<body class="bg-light-grey <%= (showExperimentalColor ? "btn-abtest" : "")%>">
    <!-- #include file="/m/includes/header.aspx" -->
    <section><!--  thankyou page code starts here -->
        <div class="container">
        	<div class="grid-12">
                <h2 class="text-center margin-top50 margin-bottom30">Thank you for your valuable feedback. Your input is important to us.</h2>
                <div class="content-box-shadow content-inner-block-10 font16 margin-bottom20">
                    <p class="font18">Browse for more information on:</p>
                    <p class="margin-top20"><a target="_blank" href="https://www.carwale.com/m/new/">New cars</a></p>
                    <p class="margin-top20"><a target="_blank" href="https://www.carwale.com/m/used/">Used cars</a></p>
                    <p class="margin-top20"><a target="_blank" href="https://www.carwale.com/quotation/landing/">Free on-road price</a></p>
                    <p class="margin-top20"><a target="_blank" href="https://www.carwale.com/m/upcoming-cars/">Upcoming cars</a></p>
                </div>
            </div>
            <div class="clear"></div>
        </div>
    </section>
    <div class="clear"></div>
    <!-- #include file="/m/includes/footer.aspx" -->
    <!-- #include file="/m/includes/global/footer-script.aspx" -->
</body>
</html>
