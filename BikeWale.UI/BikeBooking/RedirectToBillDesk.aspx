<%@ Page Language="C#" Inherits="Bikewale.BikeBooking.RedirectToBillDesk"  AutoEventWireUp="false" trace="false"%>
<html>
<head id="Head1" runat="server">

    <title>Payment</title>

    <style type="text/css">
        #pay
        {
            height: 29px;
            width: 66px;
        }
    </style>
</head>
 <body>
    <form name="formBillDesk" id="formBillDesk" action="https://pgi.billdesk.com/pgidsk/PGIMerchantPayment" method="post">
        <input type="hidden" name="msg" value='<%=msg%>'/>
       <%-- <input type="submit" id ="pay" name ="pay" onclick="submitData();" runat ="server" value="pay"></input>--%>
     </form>
 </body>
</html> 
<script type="text/javascript">

    if (<%= submit %> == 1) {
        document.formBillDesk.submit();
    }
    
</script>