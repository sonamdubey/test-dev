<%@ Page Language="C#" AutoEventWireup="false" Inherits="Bikewale.PriceQuote.PaymentFailure" Trace="false" %>
<%
    AdId = "1395986297721";
    AdPath = "/1017752/Bikewale_PriceQuote_";
%>
<!-- #include file="/includes/pgheader.aspx" -->
<link rel="stylesheet" href="/css/bw-pq.css?<%= staticFileVersion %>" type="text/css">
<link rel="stylesheet" href="/css/bw-pq-new.css?<%= staticFileVersion %>" type="text/css">
        <div class="main-container">
	        <div class="container_12">
    	        <div class="grid_8 margin-top10">
        	        <h1 class="margin-bottom5">Sorry! Your Payment Failed</h1>
            	        <div class="inner-content">
                            <p>In case you have been charged, the amount will be refunded to your acount within 7-10 working days based on your bank. Kindly contact the bank for further information.</p>
                            <p class="margin-top10"><span><strong>Transaction reference number: <%= System.Configuration.ConfigurationManager.AppSettings["OfferUniqueTransaction"] %><%= Carwale.BL.PaymentGateway.PGCookie.PGTransId %></strong></span></p>
                        </div>
                        <div class="mid-box margin-top15 margin-bottom20 center-align"><asp:Button class="action-btn text_white" id="btnMakePayment" Text="Try Paying Again" runat="server" /></div>
        	        </div>
            </div>
        </div>
<script type="text/ecmascript">
    $(document).ready(function () {
        // GA code
        var cityArea = GetGlobalCityArea();
        dataLayer.push({ 'event': 'Bikewale_all', 'cat': 'Booking Page', 'act': 'Payment_Failure', 'lab': '<%= MakeModel.Replace("'","") %>' + cityArea });
        $("#btnMakePayment").click(function () {
            dataLayer.push({ event: 'product_bw_gtm', cat: 'New Bike Booking - <%= MakeModel.Replace("'","") %>', act: 'Click Button Get_Dealer_Details', lab: 'Clicked on Retry_Payment' });
        });
    });
</script>
    </div>
</form>
</body>
</html>