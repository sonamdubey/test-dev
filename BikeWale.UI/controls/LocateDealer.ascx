<%@ Control Language="C#" AutoEventWireup="false" Inherits="Bikewale.Controls.LocateDealer" %>
<%@ Import Namespace="Bikewale.Common" %>
<p class="font20 text-bold margin-bottom10">Locate Dealer</p>
<p class="font14 margin-bottom15">Find a new bike dealer & authorized showroom</p>
<div class="form-control-box margin-bottom20">   
    <asp:DropDownList id="ddlMake" runat="server" CssClass="form-control brand"></asp:DropDownList>  
</div>
<div class="margin-top10"><a id="btnGo" class="action-btn btn-lg" runat="server" >Go</a></div>                            
<div class="clear"></div>

<script type="text/javascript">
    $("#<%=btnGo.ClientID.ToString() %>").click(function () {       
        if ($("#<%=ddlMake.ClientID.ToString() %>").val() == "0") {
            alert("Please select make");
            
        }
        else {
            var makevalue = ($("[id$='ddlMake']").val());
            makevalue = makevalue.split("_")[1];
            window.location.href = "/dealer-showrooms/" + makevalue + "/";

        }
        return false;
    });
</script>