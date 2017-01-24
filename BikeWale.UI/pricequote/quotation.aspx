<%@ Page Language="C#" AutoEventWireup="false" Inherits="Bikewale.PriceQuote.Quotation" Trace="false" Debug="false" %>
<%@ Register Src="~/controls/NewAlternativeBikes.ascx" TagName="AlternativeBikes" TagPrefix="BW" %>
<%@ Register Src="~/controls/UpcomingBikes_new.ascx" TagName="UpcomingBikes" TagPrefix="BW" %>
<%@ Register Src="~/controls/LeadCaptureControl.ascx" TagName="LeadCapture" TagPrefix="BW" %>

<%@ Import Namespace="Bikewale.Common" %>
<!doctype html>
<html>
<head>
<%
    title = "Instant Free New Bike Price Quote";
    description = "Bikewale.com New bike free price quote.";
    ShowTargeting = "1";
    TargetedModel = mmv.Model;
    AdId = "1395986297721";
    AdPath = "/1017752/Bikewale_PQ_";

    isAd300x250BTFShown = false;
%>
<!-- #include file="/includes/headscript.aspx" -->
 
<style type="text/css">
    #PQImageVariantContainer img{width:100%}
    .PQDetailsTableTitle{color:#82888b}
    .PQDetailsTableAmount,.PQOnRoadPrice{color:#4d5057}
    .text-dark-black { color:#1a1a1a; }
    .PQOffersUL{margin-left:18px;list-style:disc}
    .PQOffersUL li{padding-bottom:15px}
    .margin-top7 { margin-top:7px; }
    .inr-sm { width:8px; height:12px; background-position:-110px -468px; }
    .pqVariants .variants-dropdown{ width:210px; height: 40px; margin-left: 12px; color: #555; position: relative; cursor: pointer; background: #fff;}
    .variant-selection-tab { width:90%; display: block; overflow: hidden; text-overflow: ellipsis; white-space: nowrap; text-align: left; }
    #upDownArrow.fa-angle-down { transition: all 0.5s ease-in-out 0s; font-size: 20px;}
    .variants-dropdown .fa-angle-down { transition: transform .3s; -moz-transition: transform .3s; -webkit-transition: transform .3s; -o-transition: transform .3s; -ms-transition: transform .3s; }
    .variants-dropdown.open .fa-angle-down { -moz-transform: rotateZ(180deg); -webkit-transform: rotateZ(180deg); -o-transform: rotateZ(180deg); -ms-transform: rotateZ(180deg); transform: rotateZ(180deg); }
    .variants-dropdown-list { display:none; width:210px; overflow:hidden; background: #fff; border: 1px solid #ccc; position: absolute; top:40px; left:82px; z-index: 2;}
    .variants-dropdown-list li { margin-bottom:5px; margin-top:5px; font-size:14px; }
    .variants-dropdown-list li.selected { font-weight:bold; }
    .variants-dropdown-list li input{ width:100%; text-align:left;overflow: hidden; text-overflow: ellipsis; white-space: nowrap;background: #fff;color:rgb(77, 80, 87); padding:2px 0 2px 8px;}
    .variants-dropdown-list li:hover input{ padding:2px 0 2px 8px; cursor:pointer; background:#82888b; color:#fff; }
    .jcarousel-wrapper.alternatives-carousel{width:974px}
    .alternatives-carousel .jcarousel li{height:auto;margin-right:18px}
    .alternatives-carousel .jcarousel li.front{border:none}
    .alternative-section .jcarousel-control-left{left:-24px}
    .alternative-section .jcarousel-control-right{right:-24px}
    .alternative-section .jcarousel-control-left,.alternative-section .jcarousel-control-right{top:50%}
    .newBikes-latest-updates-container .grid-4{padding-left:10px}
    .available-colors{display:inline-block;width:150px;text-align:center;margin-bottom:20px;padding:0 5px;vertical-align:top}
    .available-colors .color-box{width:60px;height:60px;margin:0 auto 15px;border-radius:3px;background:#f00;border:1px solid #ccc}
    .upcoming-brand-bikes-container li.front{border:none}
    .upcoming-brand-bikes-container li .imageWrapper{width:303px;height:174px}
    .upcoming-brand-bikes-container li .imageWrapper a{width:303px;height:174px;display:block;background:url('https://imgd1.aeplcdn.com/0x0/bw/static/sprites/d/loader.gif') no-repeat center center}
    .upcoming-brand-bikes-container li{width:303px;height:auto;margin-right:12px}
    .upcoming-brand-bikes-container li .imageWrapper a img{width:303px;height:174px}
    .upcoming-brand-bikes-container .jcarousel{width:934px;overflow:hidden;left:20px}
    .modelGetDetails h3 { border-bottom:1px solid #ecedee; }
    .modelGetDetails ul { list-style:disc; color:#82888b; margin-left:25px; font-size:14px; }
    .modelGetDetails ul li { padding-top: 12px;padding-right: 12px; width:100% !important; float: left; }
    #leadCapturePopup { display:none; width:450px; min-height:470px; background:#fff; margin:0 auto; position:fixed; top:10%; right:5%; left:5%; z-index:10; padding: 30px 40px; }
    .personal-info-form-container { margin: 10px auto; width: 300px; min-height: 100px; }
    .personal-info-form-container .personal-info-list { margin:0 auto; width:280px; float:left; margin-bottom:20px; border-radius:0; }
    .personal-info-list .errorIcon, .personal-info-list .errorText { display:none; }
    .user-contact-details-icon { width:36px; height:44px; background-position: 0 -391px; }
    .mobile-prefix { position: absolute; padding: 10px 13px 13px; color: #999; }
     #leadCapturePopup .error-icon, #leadCapturePopup .bw-blackbg-tooltip {display:none}
     #modelAlternateBikeContent{ background: #fff; -moz-box-shadow: 0 2px 2px #e2e2e2, 0 1px 1px #f1f1f1; -webkit-box-shadow: 0 2px 2px #e2e2e2, 0 1px 1px #f1f1f1; -o-box-shadow: 0 2px 2px #e2e2e2, 0 1px 1px #f1f1f1; -ms-box-shadow: 0 2px 2px #e2e2e2, 0 1px 1px #f1f1f1; box-shadow: 0 2px 2px #e2e2e2, 0 1px 1px #f1f1f1; border: 1px solid #e2e2e2\9;}
</style>

<script type="text/javascript">

    var pqId = '<%= objQuotation.PriceQuoteId%>';
    var versionId = '<%= objQuotation.VersionId%>';
    var cityId = '<%= cityId%>';      
    var bikeVersionLocation = '';
    var campaignId = "<%= objQuotation.CampaignId%>";
    var manufacturerId = "<%= objQuotation.ManufacturerId%>";
    var versionName = "<%= objQuotation.VersionName%>";
    var myBikeName = "<%=mmv.BikeName%>";
    var clientIP = "<%= clientIP%>";
    var pageUrl = window.location.href;

</script>
</head>
<body class="bg-light-grey header-fixed-inner">
<form runat="server">
    <!-- #include file="/includes/headBW.aspx" -->	
    <section class="bg-light-grey padding-top10">
        <div class="container">
            <div class="grid-12">
                <div class="breadcrumb margin-bottom10"><!-- breadcrumb code starts here -->
                    <ul>
                        <li itemscope="" itemtype="http://data-vocabulary.org/Breadcrumb">
                            <a href="/" itemprop="url">
                                <span itemprop="title">Home</span>
                            </a>
                        </li>
                        <li itemscope="" itemtype="http://data-vocabulary.org/Breadcrumb">
                            <span class="bwsprite fa-angle-right margin-right10"></span>
                            <a href="/new/" itemprop="url">
                                <span itemprop="title">New</span>
                            </a>
                        </li>
                        <li itemscope="" itemtype="http://data-vocabulary.org/Breadcrumb">
                            <span class="bwsprite fa-angle-right margin-right10"></span>
                            <a href="/pricequote/" itemprop="url">
                                <span itemprop="title">On-road price</span>
                            </a>
                        </li>
                        <li>
                            <span class="bwsprite fa-angle-right margin-right10"></span><%= mmv.BikeName %>
                        </li>
                    </ul>
                    <div class="clear"></div>
                </div>
                <h1 class="font26 margin-bottom10">On-road price for <%= mmv.BikeName %> in <%= objQuotation.City %></h1>
                <div class="clear"></div>
            </div>
            <div class="clear"></div>
        </div>
    </section>
    
    <section class="container">
        <div class="grid-12 margin-bottom20" id="dealerPriceQuoteContainer">
            <div class="content-box-shadow padding-top20 padding-bottom20 rounded-corner2">
                <div class="grid-4 padding-right10 padding-left20" id="PQImageVariantContainer">
                    <div class="pqBikeImage margin-bottom15">
                        <img alt=" <%= mmv.BikeName %> Images" src="<%=imgPath%>" title="<%= mmv.BikeName %> Images">
                    </div>
                    <div class="pqVariants <%=(versionList.Count > 1)?"":"hide" %>">
                        <p class="margin-left10 font16 text-light-grey leftfloat margin-top7">Version:</p>
                        <div class="form-control-box position-rel variants-dropdown rounded-corner2 leftfloat">
                            <asp:DropDownList id="ddlVersion" class="form-control" runat="server" AutoPostBack="true"></asp:DropDownList>
                        </div>
                    </div>
                </div>
                <div class="grid-4 padding-right15 padding-left10" id="PQDetailsContainer">
                    <%if (objQuotation != null && objQuotation.ExShowroomPrice > 0)
                      { %>
                    <%--<p class="font16 margin-bottom15">On-road price in 
                        <span><%= (String.IsNullOrEmpty(objQuotation.Area))?objQuotation.City:(objQuotation.Area + ", " + objQuotation.City) %></span>
                    </p>--%>
                    <% } %>
                    <div>
                        <%if (objQuotation != null && objQuotation.ExShowroomPrice > 0)
                          {%>
                            <table class="font14" cellspacing="0" cellpadding="0" width="100%" border="0">
                                <tr>
                                    <td width="200" class="PQDetailsTableTitle padding-bottom15">
                                        Ex-Showroom (<%= objQuotation.City %>)
                                    </td>
                                    <td align="right" class="PQDetailsTableAmount padding-bottom15">
                                        <span class="bwsprite inr-sm"></span>&nbsp;<span id="exShowroomPrice"><%= CommonOpn.FormatNumeric( objQuotation.ExShowroomPrice.ToString() ) %></span>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="PQDetailsTableTitle padding-bottom15">RTO</td>
                                    <td align="right" class="PQDetailsTableAmount padding-bottom15">
                                        <span class="bwsprite inr-sm"></span>&nbsp;<span><%= CommonOpn.FormatNumeric( objQuotation.RTO.ToString() ) %></span>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="PQDetailsTableTitle padding-bottom15">Insurance (Comprehensive)<%--<br />
                                        <div style="position: relative; color: #999; font-size: 11px; margin-top: 1px;">Save up to 60% on insurance - <a onclick="dataLayer.push({ event: 'Bikewale_all', cat: 'BW_PQ', act: 'Insurance_Clicked',lab: '<%= (objQuotation!=null)?(objQuotation.MakeName + "_" + objQuotation.ModelName + "_" + objQuotation.VersionName + "_" + objQuotation.City):string.Empty %>' });" target="_blank" href="/insurance/">PolicyBoss</a>
                                            <span style="margin-left: 8px; vertical-align: super; font-size: 9px;">Ad</span>  
                                        </div>--%>
                                    </td>
                                    <td align="right" class="PQDetailsTableAmount padding-bottom15">
                                        <span class="bwsprite inr-sm"></span>&nbsp;<span><%= CommonOpn.FormatNumeric(  objQuotation.Insurance.ToString()  ) %></span>
                                    </td>
                                </tr>
                                <tr><td colspan="2" class="border-solid-top padding-bottom15" align="right"></tr>
                                <tr>
                                    <td class="PQDetailsTableTitle PQOnRoadPrice padding-bottom15 text-dark-black">On-road price</td>
                                    <td align="right" class="PQDetailsTableAmount font18 padding-bottom15 text-dark-black">
                                        <span class="bwsprite inr-lg"></span>&nbsp;<span><%= CommonOpn.FormatNumeric( objQuotation.OnRoadPrice.ToString()  ) %></span>
                                    </td>
                                </tr>	
                            </table>
                        <%}
                          else
                          { %>
                        <table class="font14" cellspacing="0" cellpadding="0" width="100%" border="0">
                            <tr>
                                <td colspan="2" style="vertical-align:central">
                                    <div id="div_ShowErrorMsg"  class="grey-bg border-light content-block text-highlight margin-top15" style="background:#fef5e6;">Price for this bike is not available in this city.</div>   
                                </td>
                            </tr>
                        </table>
                            <%} %>
                    </div>
                </div>
                <div class="grid-4 padding-right20 padding-left15">                  
                    <!-- #include file="/ads/Ad300x250.aspx" -->
                </div>
                <div class="clear"></div>
            </div>
        </div>
        <div class="clear"></div>
    </section>
    <% if (objQuotation != null && !string.IsNullOrEmpty(objQuotation.ManufacturerAd)){ %>
        <section>
            <%=String.Format(objQuotation.ManufacturerAd) %>
        </section>
    <%} %>
    
        <style type="text/css">
            .phone-black-icon { width:11px; height:15px; position:relative; top:2px; margin-right:4px; background-position:-73px -444px; }
            .offer-benefit-sprite {background: url(https://imgd1.aeplcdn.com/0x0/bw/static/sprites/d/offer-benefit-sprite.png?v1=2Sep2016v1) no-repeat;display: inline-block}#campaign-container .campaign-left-col{width:78%;padding-right:10px}#campaign-container .campaign-right-col{width:21%}.campaign-offer-label{width:75%;font-size:14px;font-weight:bold}.btn-large{padding:8px 56px}#campaign-offer-list li{width:175px;display:inline-block;vertical-align:middle;margin-top:15px;margin-bottom:10px;padding-right:5px}#campaign-offer-list li span{display:inline-block;vertical-align:middle}.campaign-offer-1,.campaign-offer-2,.campaign-offer-3,.campaign-offer-4{width:34px;height:28px;margin-right:5px}.campaign-offer-1{background-position:0 -356px}.campaign-offer-2{background-position:0 -390px}.campaign-offer-3{background-position:0 -425px}.campaign-offer-4{background-position:0 -463px}
        </style>


    <section>
        <div class="container margin-bottom20 <%= (ctrlAlternativeBikes.FetchedRecordsCount > 0) ? "" : "hide" %>">
            <div class="grid-12">
                <BW:AlternativeBikes ID="ctrlAlternativeBikes" runat="server" />
            </div>
            <div class="clear"></div>
        </div>
    </section>

    <section class="margin-bottom20 <%= (ctrlUpcomingBikes.FetchedRecordsCount > 0) ? "" : "hide" %>">
        <!-- Upcoming bikes from brands -->
        <div class="container">
            <div class="grid-12">
                <h2 class="text-bold text-center margin-top20 margin-bottom30 font22">Upcoming bikes from <%= mmv.Make %></h2>
                <div class="content-box-shadow rounded-corner2">
                    <div class="jcarousel-wrapper upcoming-brand-bikes-container margin-top20">
                        <div class="jcarousel">
                            <ul>
                                <BW:UpcomingBikes runat="server" ID="ctrlUpcomingBikes" />
                            </ul>
                        </div>
                        <span class="jcarousel-control-left"><a href="#" class="bwsprite jcarousel-control-prev"></a></span>
                        <span class="jcarousel-control-right"><a href="#" class="bwsprite jcarousel-control-next"></a></span>
                    </div>
                </div>
            </div>
            <div class="clear"></div>
        </div>
    </section>

    <BW:LeadCapture ID="ctrlLeadCapture" runat="server" />

    <!-- #include file="/includes/footerBW.aspx" -->
    <!-- #include file="/includes/footerscript.aspx" -->
    <script type="text/javascript">
    
    var variantsDropdown = $(".variants-dropdown"),
    variantSelectionTab = $(".variant-selection-tab"),
    variantUL = $(".variants-dropdown-list"),
    variantListLI = $(".variants-dropdown-list li");

    $.variantChangeDown = function (variantsDropdown) {
        variantsDropdown.addClass("open");
        variantUL.show();
    };

    $.variantChangeUp = function (variantsDropdown) {
        variantsDropdown.removeClass("open");
        variantUL.slideUp();
    };

    variantsDropdown.click(function (e) {
        if (!variantsDropdown.hasClass("open"))
            $.variantChangeDown(variantsDropdown);
        else
            $.variantChangeUp(variantsDropdown);
    });

    //TODO show the selected variant from dropdown

    $(document).mouseup(function (e) {
        if (!$(".variants-dropdown, .variant-selection-tab, .variant-selection-tab #upDownArrow").is(e.target)) {
            $.variantChangeUp($(".variants-dropdown"));
        }
    });
    </script>
    <script type="text/javascript">
        var bikeVersionLocation = myBikeName + '_' + versionName + '_' + GetGlobalCityArea();
    $(document).ready(function () {
        makeMapName = '<%= mmv.MakeMappingName%>';
        modelMapName = '<%= mmv.ModelMappingName%>';
    });

    $(".leadcapturebtn").click(function (e) {

        ele = $(this);
        var leadOptions = {
            "dealerid": ele.attr('data-item-id'),
            "dealername": ele.attr('data-item-name'),
            "dealerarea": ele.attr('data-item-area'),
            "versionid": versionId,
            "leadsourceid": ele.attr('data-leadsourceid'),
            "pqsourceid": ele.attr('data-pqsourceid'),
            "isleadpopup": ele.attr('data-isleadpopup'),
            "mfgCampid": ele.attr('data-mfgcampid'),
            "pqid": pqId,
            "pageurl": pageUrl,
            "clientip": clientIP,
            "gaobject": {
                cat: 'BW_Quotation_Page_Desktop',
                act: 'Get_Offers_Manufacturer_Clicked',
                lab: bikeVersionLocation
                    }
        };

        dleadvm.setOptions(leadOptions);

    });


</script>
</form>
</body>
</html>