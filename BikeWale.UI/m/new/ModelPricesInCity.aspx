<%@ Page Language="C#" AutoEventWireup="false"   Inherits="Bikewale.Mobile.New.ModelPricesInCity" %>

<%@ Register Src="~/m/controls/ModelPriceInNearestCities.ascx" TagPrefix="BW" TagName="ModelPriceInNearestCities" %>
<%@ Register Src="~/m/controls/DealersCard.ascx" TagName="Dealers" TagPrefix="BW" %>
<%@ Register Src="~/m/controls/NewAlternativeBikes.ascx" TagPrefix="BW" TagName="AlternateBikes" %>
<%@ Register Src="~/m/controls/LeadCaptureControl.ascx" TagName="LeadCapture" TagPrefix="BW" %>

<%@ Import Namespace="Bikewale.Common" %>

<!DOCTYPE html>
<html>
<head>
    <%
        title = string.Format("{0} price in {1} - Check On Road Price & Dealer Info. - BikeWale", bikeName, cityName);
        if (firstVersion != null)
            description = string.Format("{0} price in {1} - Rs. {2} (On road price). Get its detailed on road price in {1}. Check your nearest {0} Dealer in {1}", bikeName, cityName, firstVersion.OnRoadPrice);
        keywords = string.Format("{0} price in {1}, {0} on-road price, {0} bike, buy {0} bike in {1}, new {2} price", bikeName, cityName, modelName);
        canonical = string.Format("http://www.bikewale.com/{0}-bikes/{1}/price-in-{2}/", makeMaskingName, modelMaskingName, cityMaskingName);
        OGImage = modelImage;
        AdPath = "/1017752/Bikewale_Mobile_Model";
        AdId = "1017752";
        Ad_320x50 = true;
        Ad_Bot_320x50 = true;
    %>

    <!-- #include file="/includes/headscript_mobile.aspx" -->
 <link href="<%= staticUrl != "" ? "http://st2.aeplcdn.com" + staticUrl : "" %>/m/css/new/bwm-modelprice-in-city.css?<%= staticFileVersion %>" rel="stylesheet" type="text/css" />
</head>
<body class="bg-light-grey">
    <form runat="server">
        <!-- #include file="/includes/headBW_Mobile.aspx" -->

        <section class="bg-white box-shadow margin-bottom25">
            <div id="modelCityPriceDetails">
                <div class="bike-image">
                    <img src="<%=modelImage %>" title="<%= title %>" alt="<%= title %>" />
                </div>
                <h1 class="text-dark-black font18"><%=bikeName %><br />
                    price in <%=cityName %></h1>
            </div>
            <p class="font14 text-light-grey padding-right20 padding-left20 margin-bottom10">
                <%=bikeName %> On-road price in <%=cityName %>&nbsp;<span class="bwmsprite inr-xxsm-icon"></span>
                <% if (firstVersion != null)
                   { %><%=CommonOpn.FormatPrice(firstVersion.OnRoadPrice.ToString()) %> <% } %>  onwards. 
                       <% if (versionCount > 1)
                          { %> This bike comes in <%=versionCount %> versions.<br />
                <% } %>Click on any version name to know on-road price in <%= cityName %>:
            </p>

            <div>
                <div id="versionTabsWrapper">
                    <ul id='versions' class="model-versions-tabs-wrapper">
                        <asp:Repeater ID="rpVersioNames" runat="server">
                            <ItemTemplate>
                                <li class="<%# (Convert.ToUInt32(DataBinder.Eval(Container.DataItem, "VersionId")) != versionId)?string.Empty:"active" %>" id="<%# DataBinder.Eval(Container.DataItem, "VersionId").ToString() %>"><%# DataBinder.Eval(Container.DataItem, "VersionName").ToString() %></li>
                            </ItemTemplate>
                        </asp:Repeater>
                    </ul>
                </div>
                <asp:Repeater ID="rprVersionPrices" runat="server">
                    <ItemTemplate>
                        <div <%--id="versionOnRoadPriceDetails" --%>class="content-inner-block-20 margin-top5 font14 priceTable <%# (Convert.ToUInt32(DataBinder.Eval(Container.DataItem, "VersionId")) != versionId)?"hide":string.Empty %>" id="<%# DataBinder.Eval(Container.DataItem, "VersionId").ToString() %>">
                            <div class="version-details-row margin-bottom15">
                                <p class="details-left-column text-light-grey vertical-top">Ex-showroom</p>
                                <p class="details-right-column vertical-top"><span class="bwmsprite inr-xxsm-icon"></span><span class="text-bold">&nbsp;<%# CommonOpn.FormatPrice(DataBinder.Eval(Container.DataItem,"ExShowroomPrice").ToString()) %></span></p>
                            </div>
                            <div class="version-details-row margin-bottom15">
                                <p class="details-left-column text-light-grey vertical-top">RTO</p>
                                <p class="details-right-column vertical-top"><span class="bwmsprite inr-xxsm-icon"></span><span class="text-bold">&nbsp;<%#CommonOpn.FormatPrice(DataBinder.Eval(Container.DataItem,"RTO").ToString()) %></span></p>
                            </div>
                            <div class="version-details-row margin-bottom15">
                                <p class="details-left-column text-light-grey vertical-top">Insurance (comprehensive)</p>
                                <p class="details-right-column vertical-top"><span class="bwmsprite inr-xxsm-icon"></span><span class="text-bold">&nbsp; <%#CommonOpn.FormatPrice(DataBinder.Eval(Container.DataItem,"Insurance").ToString()) %></span></p>
                            </div>
                            <div class="border-divider margin-bottom15"></div>
                            <div class="version-details-row">
                                <p class="details-left-column text-bold vertical-top">On-road price in <%= cityName %></p>
                                <p class="details-right-column vertical-top"><span class="bwmsprite inr-xxsm-icon"></span><span class="text-bold">&nbsp;<%#CommonOpn.FormatPrice(DataBinder.Eval(Container.DataItem,"OnRoadPrice").ToString()) %></span></p>
                            </div>
                        </div>
                    </ItemTemplate>
                </asp:Repeater>

                <BW:Dealers ID="ctrlDealers" runat="server" />

                <BW:ModelPriceInNearestCities ID="ctrlTopCityPrices" runat="server" />


                <% if (isAreaAvailable)
                   { %>
                <div class="grid-12 float-button float-fixed">
                    <p class="grid-6 font13 select-area-label text-light-grey">Please select area to get accurate on-road price</p>
                    <p class="grid-6 alpha">
                        <a href="javascript:void(0)" pqsourceid="<%= (int) Bikewale.Entities.PriceQuote.PQSourceEnum.Mobile_PriceInCity_SelectAreas %>" selcityid="<%=cityId %>" modelid="<%=modelId %>" class="btn btn-xs btn-full-width font16 btn-orange fillPopupData changeCity">Select your area</a>
                    </p>
                </div>
                <%} %>

                <div class="clear"></div>

            </div>
        </section>

        <section class="<%= (ctrlAlternateBikes.FetchedRecordsCount > 0) ? string.Empty : "hide" %>">
             <BW:AlternateBikes ID="ctrlAlternateBikes" runat="server" />
        </section>

        <BW:LeadCapture ID="ctrlLeadCapture" runat="server" />

        <!-- #include file="/includes/footerBW_Mobile.aspx" -->
        <!-- #include file="/includes/footerscript_Mobile.aspx" -->
        <script type="text/javascript">
            
            var modelId = <%= modelId %>;
            var clientIP = "<%= clientIP%>";
            var pageUrl = window.location.href; 

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
                    "isregisterpq" : true
                };

                customerViewModel.setOptions(leadOptions);

            });


            $(document).ready(function () {
                var floatButton = $('.float-button'),
                    footer = $('footer');

                var tabsLength = $('.model-versions-tabs-wrapper li').length - 1;
                if (tabsLength < 3) {
                    $('.model-versions-tabs-wrapper li').css({'display': 'inline-block', 'width': 'auto'});
                }

                $(window).scroll(function () {
                    if (floatButton.offset().top < footer.offset().top - 50)
                        floatButton.addClass('float-fixed').show();
                    if (floatButton.offset().top > footer.offset().top - 50)
                        floatButton.removeClass('float-fixed').hide();
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
