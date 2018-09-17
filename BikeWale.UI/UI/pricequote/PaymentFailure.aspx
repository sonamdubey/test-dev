<%@ Page Language="C#" AutoEventWireup="false" Inherits="Bikewale.PriceQuote.PaymentFailure" Trace="false" %>
<!Doctype html>
<html>
<head>
    <% 
        AdId = "1395986297721";
        AdPath = "/1017752/Bikewale_PQ_";
        isAd300x250Shown = false;
        isAd300x250BTFShown = false;
        isAd970x90Shown = false;
        isAd970x90BottomShown = false;
    %>
    <!-- #include file="/UI/includes/headscript.aspx" -->
    <link href="<%= staticUrl  %>/UI/css/bookingfail.css?<%= staticFileVersion %>" rel="stylesheet" type="text/css">
</head>
<body class="bg-light-grey">
    <form runat="server">
        <div class="main-container">
        <!--Header code start here -->
        <div id="header" class="header-fixed">
		<!-- BW Doodle start-->
		<span class="bw-doodle__container">
			<span class="bw-doodle__bell-one"></span>
			<span class="bw-doodle__bell-two"></span>
			<span class="bw-doodle__ganesh-image">
				<span class="bw-doodle__ganesh"></span>
				<span class="bw-doodle__ganesh-bg"></span>
			</span>
		</span>
		<!-- BW Doodle end-->
            <div class="left-float margin-left50">
                <a href="/" class="bwsprite bw-logo"></a>
            </div>
            <div class="clear"></div>
        </div>
            </div>
        <!--Header code end here -->

        <section class="bg-white header-fixed-inner">
    	<div class="container">
            <div class="grid-12">
                <div class="padding-bottom15 text-center">
                    
                </div>
            </div>
            <div class="clear"></div>
        </div>
    </section>
	
    <section class="container margin-top20 margin-bottom20">
        <div class="grid-12">
            <div class="content-box-shadow">
                <div class="booking-fail-alert-container">
                    <div class="booking-fail-alert">
                        <p class="text-bold">
                            <span class="inline-block bwsprite booking-fail-icon margin-right10"></span>
                            <span class="inline-block font18">Oops... We could not complete your transaction</span>                        </p>
                    </div>
                </div>
                <div class="content-inner-block-20 margin-left10 margin-top10 transaction-failure-container margin-bottom20">
                	<p class="font18 text-bold margin-bottom15">Transaction reference number: <%= System.Configuration.ConfigurationManager.AppSettings["OfferUniqueTransaction"] %><%= Carwale.BL.PaymentGateway.PGCookie.PGTransId %></p>
                    <p class="margin-bottom30 font14">
                       Your booking could not be completed due to 
                        payment failure. The amount for this booking hasn’t been deducted by BikeWale. 
                        However, some banks may hold the payment amount  and reverse the funds within for 7-10 
                        working days. Kindly contact the bank for further information. 
                    </p>
                    <asp:Button class="btn btn-orange" id="btnMakePayment" Text="Retry Payment" runat="server" />
                </div>
            </div>
   		</div>
        <div class="clear"></div>
   	</section>

         <script type="text/javascript">
            $(document).ready(function () {
                // GA code
                var cityArea = "<%= objCustomer.objCustomerBase.cityDetails.CityName +'_'+ objCustomer.objCustomerBase.AreaDetails.AreaName %>";
                dataLayer.push({ 'event': 'Bikewale_all', 'cat': 'Booking Page', 'act': 'Payment_Failure', 'lab': '<%= MakeModel.Replace("'","") %>' + cityArea });
              $("#btnMakePayment").click(function () {
                  dataLayer.push({ event: 'product_bw_gtm', cat: 'New Bike Booking - <%= MakeModel.Replace("'","") %>', act: 'Click Button Get_Dealer_Details', lab: 'Clicked on Retry_Payment' });
        });
            });
        </script>
    </form>
    
</body>
</html>