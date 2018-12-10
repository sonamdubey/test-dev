<%@ Control Language="C#" AutoEventWireup="false" CodeBehind="CallSlugNumber.ascx.cs" Inherits="MobileWeb.Controls.CallSlugNumber" %>
<style>
    .fa-phone { color: rgb(255,255,255); }
    .m-call-slug{padding: 10px;}
    .center-div{display:table; margin:0 auto}
    
</style>


<div id="callslug" class="m-call-slug container text-center" style="display:block;" onclick='dataLayer.push({event:"CallSlugTracking",cat:"call-slug-click",act:"<%=pagename %>",lab:"<%GetTrackingLabel();%>"})'>
    <div class="<%=ToDisplay == true ? "center-div":""%>">
    <div id="divPrice" class="btn btn-orange btn-sm linkButtonBigBlueNew <%=ToDisplay == true ? "leftfloat":"btn-full-width"%>" " modelid="<%=ModelId %>" versionid="<%=versionId %>" onclick="redirectOrOpenPopup(this,'69')">On-Road Price</div>
    <a id="emiCallSlug" class="container <%=ToDisplay == true ? "":"hide"%>" href="tel:<%=string.IsNullOrEmpty(callSlugNumber) ? "" : callSlugNumber.Replace(" ","")%>">
        <div class="btn btn-green rightfloat margin-left10" style="padding:8px 10px;">
            <span class="fa fa-phone"></span>
            <span id="callslugnumber" class="margin-left5 text-bold">Call Dealer</span>
        </div>
    </a>
    </div>
</div>

<%--<a class="container" href="tel:<%=string.IsNullOrEmpty(callSlugNumber) ? "" : callSlugNumber.Replace(" ","")%>">
<div id="callslug" class="m-call-slug container text-center" style="display:none;" onclick='dataLayer.push({event:"CallSlugTracking",cat:"call-slug-click",act:"<%=Pagename %>",lab:"<%=trackingLabel.ToLower() %>"})'>
            <div class="btn btn-green margin-bottom5">
                <span class="fa fa-phone"></span>
                <span id="callslugnumber" class="margin-left5 text-bold"><%=callSlugNumber%></span>
            </div>
            <div class="font12 m-call-sptxt">
                <span><%= OEMDealerFound ? "Need help in your car buying?" : "Contact your authorised dealer" %></span>
            </div>
        </div>
</a>--%>
<script>
var callSlugDisplay=<%=ToDisplay.ToString().ToLower()%>;
    $(document).ready(function () {
        var OnRoadPriceBtn = $('#divPrice').is(":visible");
        var CallDealerBtn = $('#callslugnumber').is(":visible");
        FloatingSlugTracking(OnRoadPriceBtn, CallDealerBtn);

        $('#divPrice').click(function () {
            FloatingSlugTracking(OnRoadPriceBtn, CallDealerBtn, true);
        });
        $('#callslugnumber').click(function () {
            FloatingSlugTracking('', '', '', true);
        });
    });
    

</script>