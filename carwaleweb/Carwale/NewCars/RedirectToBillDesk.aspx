<%@ Page Language="C#" Inherits="Carwale.UI.NewCars.RedirectToBillDesk"  AutoEventWireUp="false" trace="false"%>
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
     <div style="text-align:center;margin-top:10%">
         <h2>Please wait while we redirect to Gateway...</h2>
     </div>
    <form name="formBillDesk" id="formBillDesk" action="<%=paymentGatewayUrl %>" method="post">
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