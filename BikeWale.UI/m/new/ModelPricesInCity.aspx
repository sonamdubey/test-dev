<%@ Page Language="C#" AutoEventWireup="false" CodeBehind="ModelPricesInCity.aspx.cs" Inherits="Bikewale.Mobile.New.ModelPricesInCity" %>

<%@ Register Src="/m/controls/ModelPriceInNearestCities.ascx" TagPrefix="BW" TagName="ModelPriceInNearestCities" %>
<%@ Register Src="/m/controls/DealersCard.ascx" TagName="Dealers" TagPrefix="BW" %>
<%@ Register Src="~/m/controls/AlternativeBikes.ascx" TagPrefix="BW" TagName="AlternateBikes" %>
 <%@ Import Namespace="Bikewale.Common" %>

<!DOCTYPE html>
<html>
<head>
    <%
        title = string.Format("{0} price in {1} - Check On Road Price & Dealer Info. - BikeWale", bikeName, cityName);
        if (firstVersion!= null)
            description = string.Format("{0} price in {1} - Rs. {2} (On road price). Get its detailed on road price in {1}. Check your nearest {0} Dealer in {1}", bikeName, cityName, firstVersion.OnRoadPrice);
        keywords = string.Format("{0} price in {1}, {0} on-road price, {0} bike, buy {0} bike in {1}, new {2} price", bikeName, cityName, modelName);
        canonical = string.Format("http://www.bikewale.com/{0}-bikes/{1}/price-in-{2}/", makeMaskingName, modelMaskingName, cityMaskingName);
        OGImage = modelImage;
     %>

    <!-- #include file="/includes/headscript_mobile.aspx" -->
    <style type="text/css">
        #modelCityPriceDetails {
            width: 100%;
            display: table;
            padding: 15px 10px 10px;
        }

            #modelCityPriceDetails .bike-image, #modelCityPriceDetails h1 {
                display: table-cell;
                vertical-align: middle;
            }

            #modelCityPriceDetails .bike-image {
                width: 80px;
                height: 45px;
            }

            #modelCityPriceDetails h1 {
                position: relative;
                top: -3px;
            }

        .bike-image img {
            width: 100%;
        }

        .text-default {
            color: #4d5057;
        }

        .text-dark-black {
            color: #1a1a1a;
        }

        #versionTabsWrapper {
            width: 100%;
            display: block;
            overflow-x: auto;
        }

        .model-versions-tabs-wrapper {
            width: 100%;
            display: table;
            background: #fff;
            border-bottom: 1px solid #e2e2e2;
        }

            .model-versions-tabs-wrapper li {
                padding: 10px 20px;
                display: table-cell;
                text-align: center;
                white-space: nowrap;
                font-size: 14px;
                color: #82888b;
            }

                .model-versions-tabs-wrapper li.active {
                    border-bottom: 3px solid #ef3f30;
                    font-weight: bold;
                    color: #4d5057;
                }

        .float-button {
            background-color: #f5f5f5;
            padding: 10px;
        }

            .float-button.float-fixed {
                position: fixed;
                bottom: 0;
                z-index: 8;
                left: 0;
                right: 0;
            }

        .select-area-label {
            position: relative;
            top: 2px;
        }

        .vertical-top {
            display: inline-block;
            vertical-align: top;
        }

        .details-left-column {
            width: 73%;
            padding-right: 5px;
        }

        .details-right-column {
            width: 24%;
            text-align: right;
        }

        .content-inner-block-2017 {
            padding: 20px 20px 17px;
        }

        .border-divider {
            border-top: 1px solid #e2e2e2;
        }

        .dealer-in-city-list li {
            padding-top: 15px;
            padding-bottom: 18px;
            border-top: 1px solid #f5f5f5;
        }

            .dealer-in-city-list li:first-child {
                border-top: 0;
            }

        .dealership-loc-icon {
            width: 8px;
            height: 12px;
            background-position: -41px -437px;
            position: relative;
            top: 4px;
        }

        .dealership-address {
            width: 92%;
            color: #82888b;
        }

        .tel-sm-icon {
            width: 17px;
            height: 15px;
            background-position: -86px -323px;
            position: relative;
            top: 2px;
        }

        .mail-grey-icon {
            width: 15px;
            height: 9px;
            background-position: -19px -437px;
            margin-right: 5px;
        }

        .block {
            display: block;
        }

        .blue-right-arrow-icon {
            width: 6px;
            height: 10px;
            background-position: -58px -437px;
            position: relative;
            top: 1px;
            left: 7px;
        }

        .text-truncate {
            width: 100%;
            text-align: left;
            text-overflow: ellipsis;
            white-space: nowrap;
            overflow: hidden;
        }

        .prices-by-cities-list {
            overflow: hidden;
        }

            .prices-by-cities-list li {
                width: 50%;
                float: left;
                padding-right: 10px;
                margin-bottom: 20px;
            }

                .prices-by-cities-list li a, .prices-by-cities-list li .price-in-city-price {
                    display: inline-block;
                    vertical-align: top;
                }

                .prices-by-cities-list li a {
                    width: 50%;
                }

                .prices-by-cities-list li .price-in-city-price {
                    width: 45%;
                    color: #2a2a2a;
                }

        .inr-dark-grey-xsm-icon {
            width: 9px;
            height: 12px;
            background-position: 0px -459px;
            position: relative;
            top: 1px;
        }
    </style>
</head>
<body class="bg-light-grey">
    <form runat="server">
        <!-- #include file="/includes/headBW_Mobile.aspx" -->

        <section class="bg-white box-shadow margin-bottom25">
            <div id="modelCityPriceDetails">
                <div class="bike-image">
                    <img src="<%=modelImage %>" title="<%= title %>" alt="<%= title %>" />
                </div>
                <h1 class="text-dark-black font18">Bajaj Pulsar<br />
                    price in Pune</h1>
            </div>
            <p class="font14 text-light-grey padding-right20 padding-left20 margin-bottom10">
                <%=bikeName %> On-road price in <%=cityName %>&nbsp; <span class="bwmsprite inr-xxsm-icon"></span>
                <% if (firstVersion != null)
                     { %>&nbsp;<%=CommonOpn.FormatPrice(firstVersion.OnRoadPrice.ToString()) %> <% } %>  onwards. 
                       <% if (versionCount > 1)
                          { %> This bike comes in <%=versionCount %> versions.<br />
                <% } %>Click on any version name to know on-road price in this city:
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
                                <p class="details-left-column text-bold vertical-top">On-road price in <%=TargetedCity %></p>
                                <p class="details-right-column vertical-top"><span class="bwmsprite inr-xxsm-icon"></span><span class="text-bold">&nbsp;<%#CommonOpn.FormatPrice(DataBinder.Eval(Container.DataItem,"OnRoadPrice").ToString()) %></span></p>
                            </div>
                        </div>
                    </ItemTemplate>
                </asp:Repeater>
                
                 <BW:Dealers ID="ctrlDealers" runat="server" />

                <BW:ModelPriceInNearestCities ID="ctrlTopCityPrices" runat="server" />

                <div class="grid-12 float-button float-fixed">
                    <p class="grid-6 font13 select-area-label text-light-grey">Please select area to get accurate on-road price</p>
                    <p class="grid-6 alpha">
                        <a class="btn btn-sm btn-full-width font18 btn-orange" href="javascript:void(0)">Select area</a>
                    </p>
                </div>

             <%--   <div class="grid-12 float-button float-fixed">
                            <% if(isAreaAvailable){ %>
                            <p class="text-black">Please select your area to get:</p>
                            <ul class="selectAreaToGetList margin-bottom20">
                                <li class="bullet-point">
                                    <p>Nearest dealership details</p>
                                </li>
                                <li class="bullet-point">
                                    <p>Exclusive offers</p>
                                </li>
                                <li class="bullet-point">
                                    <p>Complete buying assistance</p>
                                </li>
                            </ul>
                            <a href="javascript:void(0)" pqSourceId="<%= (int) Bikewale.Entities.PriceQuote.PQSourceEnum.Desktop_PriceInCity_SelectAreas %>" selCityId ="<%=cityId %>" ismodel="true" modelid="<%=modelId %>" class="btn btn-orange btn-xxlg font14 fillPopupData changeCity">Select your area</a>
                            <%} else { %>
                            <script type='text/javascript' src='https://www.googletagservices.com/tag/js/gpt.js'>
                              googletag.pubads().definePassback('/1017752/Bikewale_PQ_300x250', [300, 250]).display();
                            </script>
                            <% } %>
                        </div>--%>

                <div class="clear"></div>

            </div>
        </section>

         <section class="<%= (ctrlAlternateBikes.FetchedRecordsCount > 0) ? "" : "hide" %>">
            <div class="container margin-bottom10">
                <div class="grid-12">
                    <h2 class="margin-top30px margin-bottom20 text-center padding-top20"><%= bikeName %> Alternate Bikes </h2>

                    <div class="swiper-container discover-bike-carousel alternatives-carousel padding-bottom60">
                        <div class="swiper-wrapper">
                            <BW:AlternateBikes ID="ctrlAlternateBikes" runat="server" />
                        </div>
                        <!-- Add Pagination -->
                        <div class="swiper-pagination"></div>
                        <!-- Navigation -->
                        <div class="bwmsprite swiper-button-next hide"></div>
                        <div class="bwmsprite swiper-button-prev hide"></div>
                    </div>

                </div>
                <div class="clear"></div>
            </div>
        </section>

        <!-- #include file="/includes/footerBW_Mobile.aspx" -->
        <!-- #include file="/includes/footerscript_Mobile.aspx" -->
        <script type="text/javascript">
            $(document).ready(function () {
                var floatButton = $('.float-button'),
                    footer = $('footer');

                $(window).scroll(function () {
                    if (floatButton.offset().top < footer.offset().top - 50)
                        floatButton.addClass('float-fixed').show();
                    if (floatButton.offset().top > footer.offset().top - 50)
                        floatButton.removeClass('float-fixed').hide();
                });

                $('.model-versions-tabs-wrapper li').on('click', function () {
                    $('.model-versions-tabs-wrapper li').removeClass('active');
                    $(this).addClass('active');
                });

            });
        </script>
    </form>
</body>
</html>
