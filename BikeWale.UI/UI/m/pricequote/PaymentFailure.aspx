<%@ Page Language="C#" AutoEventWireup="false" Inherits="Bikewale.Mobile.PriceQuote.PaymentFailure" Trace="false" %>
<!doctype html>
<html>
<head>
    <%
        title = "";
        keywords = "";
        description = "";
        canonical = "";
        AdPath = "/1017752/Bikewale_Mobile_PriceQuote";
        AdId = "1398766000399";   
    %>
    <!-- #include file="/includes/headscript_mobile.aspx" -->
     <link href="<%= staticUrl  %>/m/css/bwm-booking.css?<%= staticFileVersion %>" rel="stylesheet" type="text/css" />
</head>
<body class="bg-light-grey">
    <form runat="server">
            <div class="main-container">
        <!-- Header code starts here--> 
            <header>

    	        <div class="header-fixed"> <!-- Fixed Header code starts here -->
        	        <a href="/m/" class="bwmsprite bw-logo bw-lg-fixed-position" style="left:10px;"></a>
                </div> <!-- ends here -->
    	        <div class="clear"></div>        
            </header>
            <!-- Header code ends here-->  
        <section class="container clearfix">
            <div class="grid-12 padding-top10 padding-bottom10">
                <div class="fail-msg content-inner-block-10 content-box-shadow position-rel">
                    <p class="position-rel"><span class="fail-icon margin-right10 position-abt"></span><span class="font16 text-bold padding-left45"><span class="inline-block booking-sprite booking-fail-icon margin-right10"></span> Oops... We could not complete your transaction</span></p>
                </div>

                <div class="content-inner-block-10 content-box-shadow">
                    <p class="font14 margin-top15 margin-bottom15 text-bold">Transaction reference number: <%= System.Configuration.ConfigurationManager.AppSettings["OfferUniqueTransaction"] %><%= Carwale.BL.PaymentGateway.PGCookie.PGTransId %></p>
                    <p class="text-medium-grey font14">Your booking could not be completed due to 
                        payment failure. The amount for this booking hasn’t been deducted by BikeWale. 
                        However, some banks may hold the payment amount  and reverse the funds within for 7-10 
                        working days. Kindly contact the bank for further information. 
                    </p>

                    <p class="margin-top15 margin-bottom10">
                         <asp:Button data-role="none" id="btnTryAgain" runat="server" class="btn btn-md btn-orange" Text="Retry Payment" />
                    </p>
                </div>

                <div class="clear"></div>
            </div>
        </section>

        <script type="text/javascript">
            $(document).ready(function () {
                // GA code
                var cityArea = "<%= objCustomer.objCustomerBase.cityDetails.CityName +'_'+ objCustomer.objCustomerBase.AreaDetails.AreaName %>";
                dataLayer.push({ 'event': 'Bikewale_all', 'cat': 'Booking Page', 'act': 'Payment_Failure', 'lab': '<%= MakeModel.Replace("'","") %>' + cityArea });

        $("#btnTryAgain").click(function () {
            dataLayer.push({ event: 'product_bw_gtm', cat: 'New Bike Booking - <%= MakeModel.Replace("'","") %>', act: 'Click Button Get_Dealer_Details', lab: 'Clicked on Retry_Payment' });
        });
            });


    </script>
</form>
</body>
</html>
    