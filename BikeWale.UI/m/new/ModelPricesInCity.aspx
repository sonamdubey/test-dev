<%@ Page Language="C#" AutoEventWireup="false" CodeBehind="ModelPricesInCity.aspx.cs" Inherits="Bikewale.Mobile.New.ModelPricesInCity" %>

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

        #modelAlternateBikeContent .swiper-slide {
            width: 186px;
            min-height: 252px;
            border: 1px solid #e2e2e2;
            background: #fff;
        }

            #modelAlternateBikeContent .swiper-slide h3 {
                margin-bottom: 7px;
            }

        .model-swiper-image-preview {
            width: 100%;
            height: 103px;
        }

            .model-swiper-image-preview a {
                display: block;
            }

        .model-swiper-details {
            padding: 10px 18px 20px;
        }

        .btn-x-sm {
            padding: 8px 14px;
            color: #4d5057;
        }

        #modelForSaleContent li {
            margin-top: 20px;
        }

            #modelForSaleContent li:first-child {
                margin-top: 0;
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
                <h1 class="text-dark-black font18"><%=bikeName %><br />
                    price in <%=cityName %></h1>
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


                <% if (isAreaAvailable)
                   { %>
                <div class="grid-12 float-button float-fixed">
                    <p class="grid-6 font13 select-area-label text-light-grey">Please select area to get accurate on-road price</p>
                    <p class="grid-6 alpha">
                        <a href="javascript:void(0)" pqsourceid="<%= (int) Bikewale.Entities.PriceQuote.PQSourceEnum.Desktop_PriceInCity_SelectAreas %>" selcityid="<%=cityId %>" modelid="<%=modelId %>" class="btn btn-sm btn-full-width font18 btn-orange fillPopupData changeCity">Select your area</a>
                    </p>
                </div>
                <%} %>

                <div class="clear"></div>

            </div>
        </section>

        <section class="<%= (ctrlAlternateBikes.FetchedRecordsCount > 0) ? "" : "hide" %>">
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
