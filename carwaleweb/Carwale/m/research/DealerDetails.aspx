<%@ Page Language="C#" ContentType="text/html" Inherits="MobileWeb.Research.DealerDetails" AutoEventWireup="false" Trace="false" %>
<%@ Import Namespace="Carwale.BL.Experiments" %>
<%
    MenuIndex = "1";
    Title = makeName + " Dealer Showrooms in " + cityName + " | " + makeName + " New Car Showrooms in " + cityName + " - CarWale";
    Keywords = makeName + " dealers " + cityName + ", " + makeName + " showrooms " + cityName + ", " + cityName + " car dealers, " + makeName + " dealers, " + cityName + " car showrooms, car dealers, car showrooms, dealerships";
    Description = subHeading + ". Dealer information includes full address, phone numbers, email, pin code etc.";
    Canonical = "https://www.carwale.com/" + Carwale.Utility.Format.FormatURL(makeName) + "-dealer-showrooms/" + Carwale.Utility.Format.FormatText(cityName) + "-" + cityId + "/";
    bool showExperimentalColor = ProductExperiments.IsShowExperimentalColor(CookiesCustomers.AbTest);
%>
<!DOCTYPE html>
<html>
<head>
    <!-- #include file="/m/includes/global/head-script.aspx" -->
    <!-- #include file="/m/includes/new-car-script.aspx" -->
</head>

<body class="bg-light-grey m-special-skin-body m-no-bg-color <%= (showExperimentalColor ? "btn-abtest" : "")%>">
    <!-- #include file="/m/includes/header.aspx" -->
    <!-- dealer locate Code starts here-->
    <section class="container">
            <div class="cw-m-dealer-locater-page margin-bottom20 grid-12">
                    <h1 class="pgsubhead margin-top10 m-special-skin-text">
                        <%=makeName %> Dealer Showrooms in <%=cityName %>
                    </h1>
                    <span class="font13 text-dark-grey special-skin-text  m-special-skin-text"><%=subHeading %></span>
                <ul>
                <!--- Premium Dealers-->
                <asp:Repeater ID="rptDealerPremium" runat="server">
                    <ItemTemplate>
                       <div class="hide"><%# dealerDetails = ((Carwale.Entity.Dealers.NewCarDealer)Container.DataItem)  %></div>
                        <li class="content-inner-block-5 rounded-corner2 margin-top10 premiumdealer content-box-shadow" dealername="<%#dealerDetails.Name %>">
                            <div class='cw-m-dlp-img-div <%#string.IsNullOrWhiteSpace(dealerDetails.ShowroomImage) ? "hide":"" %>'>
                                <a href="/m/new/<%=Carwale.Utility.Format.FormatURL(makeName)%>-dealers-showroom/<%=cityId %>-<%=Carwale.Utility.Format.FormatText(cityName) %>/<%#MobileWeb.Common.CommonOpn.FormatSpecial(dealerDetails.Name) %>-<%#dealerDetails.DealerId>0? dealerDetails.DealerId:0%>/" class="linktab" dealername="<%#dealerDetails.Name %>">
                                    <img src="<%#string.IsNullOrWhiteSpace(dealerDetails.ShowroomImage)? "https://img.carwale.com/adgallery/no-img-big.png\" height=\"160px\"":dealerDetails.ShowroomImage%>" alt="<%#dealerDetails.Name %>" title="<%#dealerDetails.Name %>" width="100%">
                                </a>
                            </div>
                            <div>
                                <h2>
                                    <a href="/m/new/<%=Carwale.Utility.Format.FormatURL(makeName)%>-dealers-showroom/<%=cityId %>-<%=Carwale.Utility.Format.FormatText(cityName) %>/<%#MobileWeb.Common.CommonOpn.FormatSpecial(dealerDetails.Name) %>-<%#dealerDetails.DealerId>0? dealerDetails.DealerId:0%>/"
                                        class="text-black padding-bottom10 linktab text-bold" dealername="<%#dealerDetails.Name %>"><%#dealerDetails.Name %></a>
                                </h2>
                            </div>
                            <div class="font14 padding-bottom5 cw-m-dl-border-bottom">
                                <a class="text-grey" href="/m/new/<%=Carwale.Utility.Format.FormatURL(makeName)%>-dealers-showroom/<%=cityId %>-<%=Carwale.Utility.Format.FormatText(cityName) %>/<%#MobileWeb.Common.CommonOpn.FormatSpecial(dealerDetails.Name) %>-<%#dealerDetails.DealerId>0? dealerDetails.DealerId:0%>/"><%#dealerDetails.Address + ", " + dealerDetails.CityName + ", " + dealerDetails.State + " " + dealerDetails.PinCode%>
                            </div>
                            <div class="premcall cw-m-dlp-button-group-list padding-top5" dealername="<%#dealerDetails.Name %>">
                                <a href="/m/new/<%=Carwale.Utility.Format.FormatURL(makeName)%>-dealers-showroom/<%=cityId %>-<%=Carwale.Utility.Format.FormatText(cityName) %>/<%#MobileWeb.Common.CommonOpn.FormatSpecial(dealerDetails.Name) %>-<%#dealerDetails.DealerId>0? dealerDetails.DealerId:0%>/"
                                    class="btn btn-orange btn-xs navigator" dealername="<%#dealerDetails.Name %>">Get Buying Assistance</a>
                            <%# !string.IsNullOrWhiteSpace(dealerDetails.MobileNo) ? "<a href='tel:" + (string.IsNullOrWhiteSpace(dealerDetails.MobileNo)
                             ? (string.IsNullOrWhiteSpace(dealerDetails.LandlineNo) ? "'" : dealerDetails.LandlineNo) + "'" : dealerDetails.MobileNo) + "'" + 
                             "class='btn btn-green btn-sm call-sm-btn' data-role='click impression' data-event='CWNonInteractive' data-action='Dealer-Locator-Premium-Listing-Slug' data-cat='Call-Slug-Behaviour' data-label='"
                             + makeName + "," + cityName +","+ dealerDetails.Name + "," +dealerDetails.DealerId + ","+ (string.IsNullOrWhiteSpace(dealerDetails.MobileNo)
                             ? (string.IsNullOrWhiteSpace(dealerDetails.LandlineNo)? "":dealerDetails.LandlineNo) : dealerDetails.MobileNo) + 
                             "' data-cwtccat='DealerListingPage' data-cwtcact='MaskingNumberClick' data-cwtclbl='" +
                             string.Format("cityid={0}|makieid={1}|dealerid={2}|campaignid={3}|campaignshown=1",dealerDetails.CityId ,dealerDetails.MakeId ,dealerDetails.DealerId ,dealerDetails.CampaignId ) +
                             "' ><span class='fa fa-phone padding-right5'></span>Call</a>" : "" %>
                            </div>
                        </li>
                    </ItemTemplate>
                </asp:Repeater>
                <!--- End of premium-->

                <!--Non Premium Listing-->
                <asp:Repeater ID="rptDealer" runat="server">
                    <ItemTemplate>
                        <div class="hide"><%# dealerDetails = ((Carwale.Entity.Dealers.NewCarDealer)Container.DataItem)  %></div>
                        <li class="dlp-single-btn">
                            <div class="nonpremiumdealer content-inner-block-5 rounded-corner2 margin-top10 content-box-shadow">
                                <div>
                                    <h2 class="text-bold">
                                        <%#dealerDetails.Name %>
                                    </h2>
                                </div>
                                <div class="font14 padding-bottom5 cw-m-dl-border-bottom"><%#dealerDetails.Address +", "+ dealerDetails.CityName + ", " + dealerDetails.State + " " + dealerDetails.PinCode%></div>
                                <div class="nonpremcall cw-m-dlp-button-group-list padding-top5 <%#string.IsNullOrWhiteSpace(dealerDetails.MobileNo)&&string.IsNullOrWhiteSpace(dealerDetails.LandlineNo)?"hide":"" %>" dealername="<%#dealerDetails.Name %>">
                                    <a href="tel:<%#GetContactNumber(dealerDetails)%>"
                                        class="text-black" data-cwtccat='DealerListingPage' data-cwtcact='DealerNumberClick' data-cwtclbl="<%# string.Format("cityid={0}|makieid={1}|dealerid={2}|campaignid=0|campaignshown=0",dealerDetails.CityId ,dealerDetails.MakeId ,dealerDetails.DealerId ) %>">
                                        <span class="fa fa-phone padding-right5"></span><%#GetContactNumber(dealerDetails)%></a>
                                </div>
                            </div>
                        </li>
                    </ItemTemplate>
                </asp:Repeater>
                <!--End of Non Premium Listing-->
                </ul>
            </div>
        <div class="clear"></div>
    </section>
    <section class="grid-12">
        <h3 class="text-black m-special-skin-text"><%=makeName%> cars in <%=cityName%></h3>
        <div class="swiper-container ld-car-alternatives margin-top10">
            <ul class="swiper-wrapper">
                <asp:Repeater ID="rptModel" runat="server">
                    <ItemTemplate>
                        <li class="swiper-slide content-box-shadow">
                            <div class="contentWrapper">
                                <div class="imageWrapper">
                                    <a href="/<%=MobileWeb.Common.CommonOpn.FormatSpecial(makeName) %>-cars/<%#((Carwale.Entity.CarData.CarModelSummary)Container.DataItem).MaskingName%>/"
                                        class="<%=(isEligibleForOrp ? "model-city-link" : "")%>"
                                        <%#((isEligibleForOrp) ? ("data-cityid=\"" + cityId + "\" data-cityname=\"" + cityName + "\"" + " data-redirect-url=\"" + "/"+ MobileWeb.Common.CommonOpn.FormatSpecial(makeName) +"-cars/"+ ((Carwale.Entity.CarData.CarModelSummary)Container.DataItem).MaskingName +"/\"") : "")%>>
                                        <img class="lazy" src="<%#(Carwale.Utility.ImageSizes.CreateImageUrl(((Carwale.Entity.CarData.CarModelSummary)Container.DataItem).HostUrl, Carwale.Utility.ImageSizes._272X153, ((Carwale.Entity.CarData.CarModelSummary)Container.DataItem).OriginalImage))%>" 
                                            alt="<%#((Carwale.Entity.CarData.CarModelSummary)Container.DataItem).ModelName%>" title="<%#((Carwale.Entity.CarData.CarModelSummary)Container.DataItem).ModelName%>">
                                    </a>
                                </div>
                                <div class="carDescWrapper">
                                    <h3 class="<%=isEligibleForOrp ? "" : "text-black carTitle"%>">
                                        <a href="/<%=MobileWeb.Common.CommonOpn.FormatSpecial(makeName) %>-cars/<%#((Carwale.Entity.CarData.CarModelSummary)Container.DataItem).MaskingName%>/"
                                            class="<%=isEligibleForOrp ? "model-city-link" : ""%>"
                                            <%#((isEligibleForOrp) ? ("data-cityid=\"" + cityId + "\" data-cityname=\"" + cityName + "\"" + " data-redirect-url=\"" + "/"+ MobileWeb.Common.CommonOpn.FormatSpecial(makeName) +"-cars/"+ ((Carwale.Entity.CarData.CarModelSummary)Container.DataItem).MaskingName +"/\"") : "")%>>
                                            <%=makeName%> <%#((Carwale.Entity.CarData.CarModelSummary)Container.DataItem).ModelName%>
                                        </a></h3>
                                    <div class="margin-top15">
                                        <div class="margin-bottom5">
                                            <span class="font24 text-black margin-right5">₹ <%# Carwale.UI.Common.FormatPrice.GetFormattedPriceV2(((Carwale.Entity.CarData.CarModelSummary)Container.DataItem).CarPriceOverview.Price == (int)Carwale.Entity.PriceBucket.NoUserCity ? "" : ((Carwale.Entity.CarData.CarModelSummary)Container.DataItem).CarPriceOverview.Price.ToString())%></span>
                                            <span class="font12"><%#(((Carwale.Entity.CarData.CarModelSummary)Container.DataItem).CarPriceOverview.PriceVersionCount > (int)Carwale.Entity.PriceBucket.HaveUserCity ? "onwards" : "")%></span>
                                            <div class='font13 <%#(((Carwale.Entity.CarData.CarModelSummary)Container.DataItem).CarPriceOverview.PriceStatus == (int)Carwale.Entity.PriceBucket.PriceNotAvailable ? "oliveText" : ((Carwale.Entity.CarData.CarModelSummary)Container.DataItem).CarPriceOverview.PriceStatus == (int)Carwale.Entity.PriceBucket.CarNotSold ? "redText" : "")%>'>
                                                <%#((Carwale.Entity.CarData.CarModelSummary)Container.DataItem).CarPriceOverview.PriceLabel %> <%#(((Carwale.Entity.CarData.CarModelSummary)Container.DataItem).CarPriceOverview.PriceStatus == (int)Carwale.Entity.PriceBucket.HaveUserCity ? ", " + ((Carwale.Entity.CarData.CarModelSummary)Container.DataItem).CarPriceOverview.City.CityName : "")%>
                                                <span><%#(((Carwale.Entity.CarData.CarModelSummary)Container.DataItem).CarPriceOverview.PriceStatus > (int)Carwale.Entity.PriceBucket.HaveUserCity ? "<div class='inline-block'><span class='average-info-tooltip class-ad-tooltip info-icon deals-car-sprite inline-block'></span><p class='average-info-content hide'>" + ((Carwale.Entity.CarData.CarModelSummary)Container.DataItem).CarPriceOverview.ReasonText + "</p></div>" : "")%></span>
                                            </div>
                                        </div>
                                        <%if (!isEligibleForOrp) { %>
                                            <span class="text-link font12 margin-top5" cityid="<%=cityId %>" cityname="<%=cityName %>" modelid="<%#((Carwale.Entity.CarData.CarModelSummary)Container.DataItem).ModelId%>" onclick="redirectOrOpenPopup(this,114);"><%#((Carwale.Entity.CarData.CarModelSummary)Container.DataItem).CarPriceOverview.Price > 0 ? "Check On-Road Price" : "" %></span>
                                        <%} %>
                                    </div>
                                </div>
                            </div>
                        </li>
                    </ItemTemplate>
                </asp:Repeater>
            </ul>
            <div class="clear"></div>
            <!-- Add Pagination -->
            <div class="swiper-pagination swiper-pagination-white hide"></div>
            <!-- Navigation -->
            <div class="swiper-button-next swiper-button-white"></div>
            <div class="swiper-button-prev swiper-button-white"></div>
        </div>
    </section>
    <div class="grid-12 font13 margin-bottom15 text-dark-grey m-special-skin-text">Every car model may not be available at each of the <%=makeName %> dealers in <%=cityName %></div>
    <div class="clear">
    </div>

    <!-- #include file="/m/includes/footer.aspx" -->
    <!-- #include file="/m/includes/global/footer-script.aspx" -->

    <script>
        var cityName = '<%=cityName %>';
        var cityId = '<%=cityId %>';
        Common.showCityPopup = false;
        Common.USERIP = "<%= Carwale.Utility.UserTracker.GetUserIp()%>";

        $(document).ready(function () {
            globalCityByUserAction(cityId, cityName, '', "DealerLocatorCitySet");
        });
        function globalCityByUserAction(cityId, cityName, zoneId, action) {
            var label = cityName + "," + Common.USERIP;
            var globalCity = $.cookie("_CustCityIdMaster");
            if (Number($.cookie("_CustCityIdMaster")) <= 0 && Number(zoneId) <= 0) { //if not a pq zone
                setCookie(cityName, cityId);
                Common.utils.trackAction("CWNonInteractive", "MSite_UserAction_GlobalCitySet", action, label);
            }

            if (Number($.cookie("_CustZoneIdMaster")) == 0 && Number(zoneId) > 0) { //if its a pq zone
                if (Number($.cookie("_CustCityIdMaster")) <= 0) {
                    if (cityId == utils.city.MUMBAI) {
                        setCookie("Mumbai", cityId);
                        Common.utils.trackAction("CWNonInteractive", "MSite_UserAction_GlobalCitySet", action, label);
                    }
                    else if (cityId == utils.city.DELHI) {
                        setCookie("Delhi", cityId);
                        Common.utils.trackAction("CWNonInteractive", "MSite_UserAction_GlobalCitySet", action, label);
                    }
                    else if (cityId == utils.city.BANGLORE) {
                        setCookie("Banglore", cityId);
                        Common.utils.trackAction("CWNonInteractive", "MSite_UserAction_GlobalCitySet", action, label);
                    }
                    if (cityId == Number($.cookie("_CustCityIdMaster"))) {
                        utils.setZoneMasterCookie(zoneId, cityName);
                    }
                } else {
                    if (cityId == Number($.cookie("_CustCityIdMaster"))) {
                        utils.setZoneMasterCookie(zoneId, cityName);
                        Common.utils.trackAction("CWNonInteractive", "MSite_UserAction_GlobalCitySet", "PQOnlyZoneSet", label);
                    }
                }
            }
            var areaText = $.cookie("_CustCityMaster");
            $('#global-place').text(areaText);
        }
        function navigate(lng, lat) {
            window.open('http://maps.google.com?daddr=' + lat + ',' + lng + '&amp;ll=');
        }

        $('.premcall,.nonpremcall').click(function () {
            var dealer = $(this).attr('dealername').toLowerCase() + "-<%=makeName.ToLower()%>-<%=cityName.ToLower()%>";
            var action = "call_premium_dealer";
            if ($(this).hasClass('premcall')) action = "call_premium_dealer";
            else action = "call_non_premium_dealer";

            dataLayer.push({ event: 'locate_dealer_section', cat: 'dealer_listing_msite', act: action, lab: dealer });
        });
            $('.navigator').click(function () {
                var dealer = $(this).parent().attr('dealername');
                dataLayer.push({ event: 'locate_dealer_section', cat: 'dealer_listing_msite', act: 'get_directions_button', lab: dealer });
            });
            $('.linktab').click(function () {
                var dealer = $(this).attr('dealername').toLowerCase();
                var action = 'dealer_showroom_link';
                if ($(this).has('img').length > 0) action = action + "_img";
                else if ($(this).text() == "More..") action = action + "_morebutton";
                else action += "_dealertitle"
                dataLayer.push({ event: 'locate_dealer_section', cat: 'dealer_listing_msite', act: action, lab: dealer });
            });
    </script>
</body>
</html>
