<%@ Page Language="C#" AutoEventWireup="false" Inherits="Bikewale.Mobile.PriceQuote.PaymentFailure" Trace="false" %>

<%
    title = "";
    keywords = "";
    description = "";
    canonical = "";
    AdPath = "/1017752/Bikewale_Mobile_PriceQuote";
    AdId = "1398766000399";
%>
<!-- #include file="/includes/PaymentHeaderMobile.aspx" -->
<link rel="stylesheet"  href="<%= !String.IsNullOrEmpty(staticUrl) ? "http://st2.aeplcdn.com" + staticUrl : "" %>/m/css/bw-new-style.css?<%= staticFileVersion %>" />
    <div class="padding5">
        <h1>Sorry! Your Payment Failed</h1>
        <div class="box1 new-line10">
            <div>In case you have been charged, the amount will be refunded to your acount within 7-10 working days based on your bank. Kindly contact the bank for further information.</div>
            <div class="margin-top-10 f-bold">Transaction reference number:<span><strong> <%= System.Configuration.ConfigurationManager.AppSettings["OfferUniqueTransaction"] %><%= Carwale.BL.PaymentGateway.PGCookie.PGTransId %></strong></span></div>
        </div>
        <asp:Button data-role="none" id="btnTryAgain" runat="server" class="rounded-corner5" Text="Try Paying Again" />
    </div>
</div>
            <!-- inner-section code ends here-->
</div>
</div> 
    <div id="divForPopup" style="display:none;"></div>

<script type="text/ecmascript">
    $(document).ready(function () {
        $("#btnTryAgain").click(function () {
            dataLayer.push({ event: 'product_bw_gtm', cat: 'New Bike Booking - <%= MakeModel.Replace("'","") %>', act: 'Click Button Get_Dealer_Details', lab: 'Clicked on Retry_Payment' });
        });
    });
</script>
</form>
</body>
</html>