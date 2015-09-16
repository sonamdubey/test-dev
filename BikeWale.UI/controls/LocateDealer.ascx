<%@ Control Language="C#" AutoEventWireup="false" Inherits="Bikewale.Controls.LocateDealer" %>
<%@ Import Namespace="Bikewale.Common" %>
<h1>Locate Dealer</h1>
<p>Find a new bike dealer & authorized showroom</p>
<div class="left-float margin-top10 margin-right10 padding-bottom20">   
    <asp:DropDownList id="ddlMake" runat="server" CssClass="brand"></asp:DropDownList>  
</div>
<div class="margin-top10"><a id="btnGo" class="action-btn" runat="server" >Go</a></div>                            
<div class="clear"></div>

<script type="text/javascript">
    $("#<%=btnGo.ClientID.ToString() %>").click(function () {       
        if ($("#<%=ddlMake.ClientID.ToString() %>").val() ==  "0") {
           alert("Please select make");
            return false;
        }        
    });
</script>