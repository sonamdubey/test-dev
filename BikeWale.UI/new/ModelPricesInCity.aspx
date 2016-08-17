<%@ Page Language="C#" AutoEventWireup="false" Inherits="Bikewale.New.ModelPricesInCity" EnableViewState="false" %>

<%@ Register Src="/controls/ModelPriceInNearestCities.ascx" TagPrefix="BW" TagName="ModelPriceInNearestCities" %>
<%@ Register Src="~/controls/NewAlternativeBikes.ascx" TagName="AlternativeBikes" TagPrefix="BW" %>
<%@ Register Src="~/controls/DealerCard.ascx" TagName="Dealers" TagPrefix="BW" %>
<%@ Register Src="~/controls/LeadCaptureControl.ascx" TagName="LeadCapture" TagPrefix="BW" %>
<%@ Import Namespace="Bikewale.Common" %>
<!doctype html>
<html>
<head>

    <%
        title = string.Format("{0} price in {1} - Check On Road Price & Dealer Info. - BikeWale", bikeName, cityName);
        if (firstVersion != null && !isDiscontinued)
            description = string.Format("{0} price in {1} - Rs. {2} (On road price). Get its detailed on road price in {1}. Check your nearest {0} Dealer in {1}", bikeName, cityName, firstVersion.OnRoadPrice);
        else if(firstVersion != null)
            description = string.Format("{0} price in {1} - Rs. {2} (Ex-Showroom). Get prices for all the versions of and check out the nearest {0} Dealer in {1}", bikeName, cityName, firstVersion.ExShowroomPrice);
        
        keywords = string.Format("{0} price in {1}, {0} on-road price, {0} bike, buy {0} bike in {1}, new {2} price", bikeName, cityName, modelName);
        canonical = string.Format("http://www.bikewale.com/{0}-bikes/{1}/price-in-{2}/", makeMaskingName, modelMaskingName, cityMaskingName);
        alternate = string.Format("http://www.bikewale.com/m/{0}-bikes/{1}/price-in-{2}/", makeMaskingName, modelMaskingName, cityMaskingName);
        ogImage = modelImage;
        AdId = "1442913773076";
        AdPath = "/1017752/Bikewale_NewBike_";
        isAd970x90Shown = true;
        isAd970x90BTFShown = false;
        isAd970x90BottomShown = true;
        isAd300x250Shown = false;
        isAd300x250BTFShown = false;
                   
    %>
    <!-- #include file="/includes/headscript.aspx" -->
    <link href="<%= staticUrl != "" ? "http://st2.aeplcdn.com" + staticUrl : "" %>/css/new/modelprice-in-city.css?<%= staticFileVersion %>" rel="stylesheet" type="text/css" />
</head>
<body class="bg-light-grey header-fixed-inner">
    <form runat="server">
        <!-- #include file="/includes/headBW.aspx" -->
        <section class="bg-light-grey padding-top10">
            <div class="container">
                <div class="grid-12">
                    <div class="breadcrumb margin-bottom15">
                        <!-- breadcrumb code starts here -->
                        <ul>
                            <li itemscope="" itemtype="http://data-vocabulary.org/Breadcrumb"><a href="/" itemprop="url">
                                <span itemprop="title">Home</span></a>
                            </li>
                            <li itemscope="" itemtype="http://data-vocabulary.org/Breadcrumb">
                                <span class="bwsprite fa-angle-right margin-right10"></span>
                                <a href="/<%= makeMaskingName %>-bikes/" itemprop="url">
                                    <span itemprop="title"><%=makeName %> Bikes</span>
                                </a></li>
                            <li><span class="bwsprite fa-angle-right margin-right10"></span>
                                <a href="/<%= makeMaskingName %>-bikes/<%= modelMaskingName %>/" itemprop="url">
                                    <span><%=makeName %> <%= modelName %></span>
                                </a>
                            </li>
                            <li><span class="bwsprite fa-angle-right margin-right10"></span>
                                <span>Price in <%=cityName %></span>
                            </li>
                        </ul>
                        <div class="clear"></div>
                    </div>
                    <h1 class="font22 text-default margin-bottom20"><%=bikeName %> price in <%=cityName %></h1>
                </div>
                <div class="clear"></div>
            </div>
        </section>

        <section id="versionPriceInCityWrapper" class="container margin-bottom25">
            <div class="grid-12 font14">
                <div class="content-box-shadow">
                    <p class="padding-top20 padding-right20 padding-bottom5 padding-left20 text-light-grey">
                        <%=bikeName %> <% if(!isDiscontinued) { %> on-road <% } else { %> ex-showroom <% } %> price in <%=cityName %>&nbsp;<span class="bwsprite inr-sm-grey"></span><% if (firstVersion != null && !isDiscontinued)
                        { %>&nbsp;<%=CommonOpn.FormatPrice(firstVersion.OnRoadPrice.ToString()) %> <% } else if (firstVersion != null) %>  <%=CommonOpn.FormatPrice(firstVersion.ExShowroomPrice.ToString())   %> onwards. 
                       <% if (versionCount > 1)
                          { %> This bike comes in <%=versionCount %> versions.<br />
                        Click on any version name to know <% if(!isDiscontinued) { %> on-road <% } %> price in <%= cityName %>:
                    <% } %></p>
                    <div id='versions' class="model-versions-tabs-wrapper">
                        <asp:Repeater ID="rpVersioNames" runat="server">
                            <ItemTemplate>
                                <a class="<%# (Convert.ToUInt32(DataBinder.Eval(Container.DataItem, "VersionId")) != versionId)?string.Empty:"active" %>" id="<%# DataBinder.Eval(Container.DataItem, "VersionId").ToString() %>" href="javascript:void(0)"><%# DataBinder.Eval(Container.DataItem, "VersionName").ToString() %></a>
                            </ItemTemplate>
                        </asp:Repeater>
                    </div>
                    <div class="border-divider"></div>

                    <div id="modelVersionDetailsWrapper" class="text-light-grey padding-bottom20">
                        <div class="grid-4 padding-top10">
                            <div class="model-version-image-content">
                                <%if(isDiscontinued) { %><span class="discontinued-text-label font16 position-abt text-center">Discontinued</span> <% } %>
                                <img src="<%=modelImage %>" title="<%= title %>" alt="<%= title %>" />
                            </div>
                        </div>
                        <div class="grid-4 padding-top15">
                            <asp:Repeater ID="rprVersionPrices" runat="server">
                                <ItemTemplate>
                                    <% if (!isDiscontinued)
                                       { %>
                                    <div class="priceTable <%# (Convert.ToUInt32(DataBinder.Eval(Container.DataItem, "VersionId")) != versionId)?"hide":string.Empty %>" id="<%# DataBinder.Eval(Container.DataItem, "VersionId").ToString() %>">
                                        <table cellspacing="0" cellpadding="0" width="100%" border="0">
                                            <tr>
                                                <td width="200" class="padding-bottom15">Ex-showroom</td>
                                                <td align="right" class="padding-bottom15 text-default"><span class="bwsprite inr-sm"></span>
                                                    <%# CommonOpn.FormatPrice(DataBinder.Eval(Container.DataItem,"ExShowroomPrice").ToString()) %>                                                     
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="padding-bottom15">RTO</td>
                                                <td align="right" class="padding-bottom15 text-default"><span class="bwsprite inr-sm"></span>
                                                    <%#CommonOpn.FormatPrice(DataBinder.Eval(Container.DataItem,"RTO").ToString()) %>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="padding-bottom15">Insurance</td>
                                                <td align="right" class="padding-bottom15 text-default"><span class="bwsprite inr-sm"></span>
                                                    <%#CommonOpn.FormatPrice(DataBinder.Eval(Container.DataItem,"Insurance").ToString()) %>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td colspan="2" class="padding-bottom15 border-divider"></td>
                                            </tr>
                                            <tr>
                                                <td class="text-bold text-default">On-road price in <%= cityName %></td>
                                                <td align="right" class="font16 text-bold text-default"><span class="bwsprite inr-lg"></span>
                                                    <%#CommonOpn.FormatPrice(DataBinder.Eval(Container.DataItem,"OnRoadPrice").ToString()) %>
                                                </td>
                                            </tr>
                                        </table>
                                    </div>
                                    <%}
                                       else
                                       { %>
                                            <div class="priceTable <%# (Convert.ToUInt32(DataBinder.Eval(Container.DataItem, "VersionId")) != versionId)?"hide":string.Empty %>" id="<%# DataBinder.Eval(Container.DataItem, "VersionId").ToString() %>">
                                                <p class="text-x-grey margin-bottom15"><%=bikeName %> is now discontinued in India.</p>
                                                <div class="padding-bottom15 text-default text-bold">
                                                    <span class="margin-right5">Last known Ex-showroom price</span>
                                                    <span class="bwsprite inr-sm"></span>
                                                    <%# CommonOpn.FormatPrice(DataBinder.Eval(Container.DataItem,"ExShowroomPrice").ToString()) %>                                                     
                                                </div> 
                                            </div>
                                    <% } %>
                                </ItemTemplate>
                            </asp:Repeater>
                        </div>

                        <div class="grid-4 padding-top15 padding-left15">
                            <% if (isAreaAvailable && !isDiscontinued)
                               { %>
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
                            <a href="javascript:void(0)" pqsourceid="<%= (int) Bikewale.Entities.PriceQuote.PQSourceEnum.Desktop_PriceInCity_SelectAreas %>" selcityid="<%=cityId %>" ismodel="true" modelid="<%=modelId %>" class="btn btn-orange btn-xxlg font14 fillPopupData changeCity" rel="nofollow">Select your area</a>
                            <%}
                               else
                               { %>
                            <script type='text/javascript' src='https://www.googletagservices.com/tag/js/gpt.js'>
                              googletag.pubads().definePassback('/1017752/Bikewale_PQ_300x250', [300, 250]).display();
                            </script>
                            <% } %>
                        </div>
                        <div class="clear"></div>
                    </div>

                    <div class="margin-left10 margin-right10 border-solid-bottom"></div>

                    <BW:Dealers ID="ctrlDealers" runat="server" />

                    <BW:ModelPriceInNearestCities ID="ctrlTopCityPrices" runat="server" />

                </div>
            </div>
            <div class="clear"></div>
        </section>

        <section class="container margin-bottom30">
            <div class="grid-12">
                <div class="content-box-shadow padding-bottom20">
                    <% if (ctrlAlternativeBikes.FetchedRecordsCount > 0)
                       { %>
                    <!-- Alternative reviews ends -->
                    <BW:AlternativeBikes ID="ctrlAlternativeBikes" runat="server" />
                    <!-- Alternative reviews ends -->
                    <% } %>
                </div>
            </div>
            <div class="clear"></div>
        </section>

        <BW:LeadCapture ID="ctrlLeadCapture" runat="server" />

        <!-- #include file="/includes/footerBW.aspx" -->
        <!-- #include file="/includes/footerscript.aspx" -->
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
                    "versionid" : $("#versions a.active").attr("id") ,
                    "leadsourceid" : ele.attr('data-leadsourceid'),
                    "pqsourceid" : ele.attr('data-pqsourceid'),
                    "pageurl" : pageUrl,
                    "clientip" : clientIP,
                    "isregisterpq" : true
                };

                dleadvm.setOptions(leadOptions);

            });

            $('.model-versions-tabs-wrapper a').on('click', function () {
                var verid = $(this).attr('id');
                showTab(verid);
            });

            function showTab(version) {
                $('.model-versions-tabs-wrapper a').removeClass('active');
                $('.model-versions-tabs-wrapper a[id="' + version + '"]').addClass('active');
                $('.priceTable').hide();
                $('.priceTable[id="' + version + '"]').show();
            }           

        </script>
    </form>
</body>
</html>
