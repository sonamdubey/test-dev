﻿<%@ Page Language="C#" AutoEventWireup="false" Inherits="Bikewale.Mobile.New.ModelPricesInCity" %>

<%@ Register Src="~/m/controls/ModelPriceInNearestCities.ascx" TagPrefix="BW" TagName="ModelPriceInNearestCities" %>
<%@ Register Src="~/m/controls/DealersCard.ascx" TagName="Dealers" TagPrefix="BW" %>
<%@ Register Src="~/m/controls/NewAlternativeBikes.ascx" TagPrefix="BW" TagName="AlternateBikes" %>
<%@ Register Src="~/m/controls/LeadCaptureControl.ascx" TagName="LeadCapture" TagPrefix="BW" %>
<%@ Register Src="~/m/controls/UsedBikes.ascx" TagName="MostRecentusedBikes" TagPrefix="BW" %>

<%@ Import Namespace="Bikewale.Common" %>

<!DOCTYPE html>
<html>
<head>
    <%
        title = string.Format("{0} price in {1} - Check On Road Price & Dealer Info. - BikeWale", bikeName, cityName);
        
        if (firstVersion != null && !isDiscontinued)
            description = string.Format("{0} price in {1} - Rs. {2} (On road price). Get its detailed on road price in {1}. Check your nearest {0} Dealer in {1}", bikeName, cityName, firstVersion.OnRoadPrice);
        else if (firstVersion != null)
            description = string.Format("{0} price in {1} - Rs. {2} (Ex-Showroom). Get prices for all the versions of and check out the nearest {0} Dealer in {1}", bikeName, cityName, firstVersion.ExShowroomPrice);
            
        keywords = string.Format("{0} price in {1}, {0} on-road price, {0} bike, buy {0} bike in {1}, new {2} price", bikeName, cityName, modelName);
        canonical = string.Format("http://www.bikewale.com/{0}-bikes/{1}/price-in-{2}/", makeMaskingName, modelMaskingName, cityMaskingName);
        OGImage = modelImage;
        AdPath = "/1017752/Bikewale_Mobile_Model";
        AdId = "1444028976556";
        Ad_320x50 = true;
        Ad_Bot_320x50 = true;
    %>

    <!-- #include file="/includes/headscript_mobile.aspx" -->
    <link href="<%= staticUrl != "" ? "http://st2.aeplcdn.com" + staticUrl : "" %>/m/css/new/bwm-modelprice-in-city.css?<%= staticFileVersion %>" rel="stylesheet" type="text/css" />
</head>

<body class="bg-light-grey">
    <form runat="server">
        <!-- #include file="/includes/headBW_Mobile.aspx" -->

        <section class="bg-white box-shadow padding-top10 margin-bottom10">
              <%if(isDiscontinued) { %> <div class="discont-text-label font14 text-white text-center">Discontinued</div>     <% } %>
            <div id="modelCityPriceDetails">
                <div class="bike-image">
                    <img src="<%=modelImage %>" title="<%= title %>" alt="<%= title %>" />
                </div>
                <h1 class="text-dark-black font18"><%=bikeName %><br />
                    price in <%=cityName %></h1>
            </div>
            <p class="font14 text-light-grey padding-right20 padding-left20 margin-bottom10">
                <%= pageDescription %>
            </p>

            <div>
                <div id="versionTabsWrapper">
                    <ul id='versions' class="model-versions-tabs-wrapper">
                        <asp:Repeater ID="rpVersioNames" runat="server">
                            <ItemTemplate>
                                <li class="<%# (Convert.ToUInt32(DataBinder.Eval(Container.DataItem, "VersionId")) != versionId)?string.Empty:"active" %>" id="<%# DataBinder.Eval(Container.DataItem, "VersionId").ToString() %>" <%if(versionCount==1) { %>style="display:inline-block; width:auto;" <% } %> ><%# DataBinder.Eval(Container.DataItem, "VersionName").ToString() %></li>
                            </ItemTemplate>
                        </asp:Repeater>
                    </ul>
                </div>
                <asp:Repeater ID="rprVersionPrices" runat="server">
                    <ItemTemplate>
                        <% if (!isDiscontinued)
                           { %>
                        <div class="content-inner-block-20 font14 priceTable <%# (Convert.ToUInt32(DataBinder.Eval(Container.DataItem, "VersionId")) != versionId)?"hide":string.Empty %>" id="<%# DataBinder.Eval(Container.DataItem, "VersionId").ToString() %>">
                            <table cellspacing="0" cellpadding="0" width="100%" border="0">
                                <tr>
                                    <td width="65%" class="padding-bottom15 text-light-grey">Ex-showroom</td>
                                    <td width="45%" align="right" class="padding-bottom15"><span class="bwmsprite inr-xxsm-icon"></span><span class="text-bold"><%# CommonOpn.FormatPrice(DataBinder.Eval(Container.DataItem,"ExShowroomPrice").ToString()) %></span></td>
                                </tr>
                                <tr>
                                    <td class="padding-bottom15 text-light-grey">RTO</td>
                                    <td align="right" class="padding-bottom15"><span class="bwmsprite inr-xxsm-icon"></span><span class="text-bold"><%#CommonOpn.FormatPrice(DataBinder.Eval(Container.DataItem,"RTO").ToString()) %></span></td>
                                </tr>
                                <tr>
                                    <td class="padding-bottom15 text-light-grey">Insurance (comprehensive)</td>
                                    <td align="right" class="padding-bottom15"><span class="bwmsprite inr-xxsm-icon"></span><span class="text-bold"><%#CommonOpn.FormatPrice(DataBinder.Eval(Container.DataItem,"Insurance").ToString()) %></span></td>
                                </tr>
                                <tr>
                                    <td colspan="2" class="padding-bottom15 border-divider"></td>
                                </tr>
                                <tr>
                                    <td class="text-bold"><%= modelName %> On-road price in <%= cityName %></td>
                                    <td align="right"><span class="bwmsprite inr-xxsm-icon"></span><span class="text-bold"><%#CommonOpn.FormatPrice(DataBinder.Eval(Container.DataItem,"OnRoadPrice").ToString()) %></span></td>
                                </tr>
                            </table>
                        </div>
                        <%}
                           else
                           { %>
                        <div class="content-inner-block-20 margin-top5 font14 priceTable <%# (Convert.ToUInt32(DataBinder.Eval(Container.DataItem, "VersionId")) != versionId)?"hide":string.Empty %>" id="<%# DataBinder.Eval(Container.DataItem, "VersionId").ToString() %>">
                            <p class="text-x-grey margin-bottom15"><%=bikeName %> is now discontinued in India.</p>
                            <div class="version-details-row">
                                <p class="text-default text-bold">
                                    <span class="margin-right5">Last known Ex-showroom price</span>
                                    <span class="bwmsprite inr-xxsm-icon"></span>
                                    <span class="text-bold"><%# CommonOpn.FormatPrice(DataBinder.Eval(Container.DataItem,"ExShowroomPrice").ToString()) %></span>
                                </p>
                            </div>
                        </div>
                        <% } %>
                    </ItemTemplate>
                </asp:Repeater>

                 <% if (ctrlDealers.showWidget) { %>
                <div class="margin-right20 margin-left20 border-divider"></div> 
                    <BW:Dealers runat="server" ID="ctrlDealers" />
                <% }  %>

                <BW:ModelPriceInNearestCities ID="ctrlTopCityPrices" runat="server" />


                <% if (isAreaAvailable && !isDiscontinued)
                   { %>
                <div class="grid-12 float-button float-fixed">
                    <p class="grid-6 font13 select-area-label text-light-grey">Please select area to get accurate on-road price</p>
                    <p class="grid-6 alpha">
                        <a href="javascript:void(0)" data-pqsourceid="<%= (int) Bikewale.Entities.PriceQuote.PQSourceEnum.Mobile_PriceInCity_SelectArea %>" data-preselcity="<%=cityId %>" data-modelid="<%=modelId %>" c="Price_in_City_Page" a="Select_Area_Clicked" l="<%= string.Format("{0}_{1}_{2}", makeName, modelName, versionName)%>" class="bw-ga btn btn-xs btn-full-width font16 btn-orange getquotation changeCity" rel="nofollow">Select your area</a>
                    </p>
                </div>
                <%} %>

                <div class="clear"></div>

            </div>
        </section>


        <section class="<%= (ctrlAlternateBikes.FetchedRecordsCount > 0) ? string.Empty : "hide" %>">
            <BW:AlternateBikes ID="ctrlAlternateBikes" runat="server" />
        </section>

        <% if (ctrlRecentUsedBikes.fetchedCount > 0) { %>
           <section>
               <div class="box-shadow bg-white margin-bottom10">
                    <BW:MostRecentUsedBikes runat="server" ID="ctrlRecentUsedBikes" />
                </div>
           </section>
        <%} %>
        <span class="font13 text-light-grey padding-right20 padding-left20 margin-bottom10"><strong>Disclaimer</strong>:</span>
    <p class="font12 text-light-grey padding-right20 padding-left20 margin-bottom10"> 
        BikeWale takes utmost care in gathering precise and accurate information about <%=makeName %> <%=modelName %> 
        price in <%= cityName %>
        However, this information is only indicative and may not reflect the final price you may pay. For more information please read <a href="/termsconditions.aspx" target="_blank">terms and conditions</a>,<a href="/visitoragreement.aspx" target="_blank"> visitor agreement </a> and  <a href="/privacypolicy.aspx" target="_blank">privacy policy</a>.
    </p>
        

        <BW:LeadCapture ID="ctrlLeadCapture" runat="server" />

        <!-- #include file="/includes/footerBW_Mobile.aspx" -->
        <!-- #include file="/includes/footerscript_Mobile.aspx" -->
        <script type="text/javascript">
            ga_pg_id = "16";
            var bikenamever = '<%= string.Format("{0}_{1}_{2}", makeName, modelName,versionName)%>';
            var bikeNameLocation = '<%=string.Format("{0}_{1}_{2}",makeName,modelName,cityName)%>';
            var modelId = "<%= modelId %>";
            var clientIP = "<%= clientIP%>";
            var pageUrl = window.location.href; 
            var areaName = '<%=areaName%>';
            $(".leadcapturebtn").click(function(e){
                ele = $(this);
                
                var leadOptions = {
                    "dealerid" : ele.attr('data-item-id'),
                    "dealername" : ele.attr('data-item-name'),
                    "dealerarea"  : ele.attr('data-item-area'),
                    "versionid" : $("#versions li.active").attr("id") ,
                    "leadsourceid" : ele.attr('data-leadsourceid'),
                    "pqsourceid" : ele.attr('data-pqsourceid'),
                    "pageurl" : pageUrl,
                    "clientip" : clientIP,
                    "isregisterpq": true,
                    "gaobject": {
                        cat: ele.attr('data-ga-cat'),
                        act: ele.attr('data-ga-act'),
                        lab: bikeNameLocation.concat(areaName == "" ? "" : "_" + areaName)
                    }
                };

                dleadvm.setOptions(leadOptions);

            });


            $(document).ready(function () {
                var floatButton = $('.float-button'),
                    footer = $('footer');

                var tabsLength = $('.model-versions-tabs-wrapper li').length - 1;
                if (tabsLength < 1) {
                    $('.model-versions-tabs-wrapper li').css({'display': 'inline-block', 'width': 'auto'});
                }

                $(window).scroll(function () {
                    try
                    {
                        if (floatButton != null) {
                            if (floatButton.offset().top < footer.offset().top - 50)
                                floatButton.addClass('float-fixed').show();
                            if (floatButton.offset().top > footer.offset().top - 50)
                                floatButton.removeClass('float-fixed').hide();
                        }
                    }
                    catch(e)
                    {

                    }
                });

                $('.model-versions-tabs-wrapper li').on('click', function () {
                    $('.model-versions-tabs-wrapper li').removeClass('active');
                    $(this).addClass('active');
                    showTab($(this).attr('id'));
                });

                function showTab(version) {
                    $('.model-versions-tabs-wrapper a').removeClass('active');
                    $('.model-versions-tabs-wrapper a[id="' + version + '"]').addClass('active');
                    $('.priceTable').hide();
                    $('.priceTable[id="' + version + '"]').show();
                }

            });
        </script>
    </form>

</body>
</html>
