<%@ Page Language="C#" AutoEventWireup="false" Inherits="Bikewale.New.ModelPricesInCity" EnableViewState="false" %>
<%@ Register Src="/controls/ModelPriceInNearestCities.ascx" TagPrefix="BW" TagName="ModelPriceInNearestCities" %>
<%@ Register Src="~/controls/NewAlternativeBikes.ascx" TagName="AlternativeBikes" TagPrefix="BW" %>
<%@ Register Src="~/controls/DealerCard.ascx" TagName="Dealers" TagPrefix="BW" %>
<%@ Register Src="~/controls/LeadCaptureControl.ascx"  TagName="LeadCapture" TagPrefix="BW" %>
<%@ Import Namespace="Bikewale.Common" %>
<!doctype html>
<html>
<head>
    
    <%
        title = string.Format("{0} price in {1} - Check On Road Price & Dealer Info. - BikeWale", bikeName, cityName);
        if (firstVersion!= null)
            description = string.Format("{0} price in {1} - Rs. {2} (On road price). Get its detailed on road price in {1}. Check your nearest {0} Dealer in {1}", bikeName, cityName, firstVersion.OnRoadPrice);
        keywords = string.Format("{0} price in {1}, {0} on-road price, {0} bike, buy {0} bike in {1}, new {2} price", bikeName, cityName, modelName);
        canonical = string.Format("http://www.bikewale.com/{0}-bikes/{1}/price-in-{2}/", makeMaskingName, modelMaskingName, cityMaskingName);
        alternate = string.Format("http://www.bikewale.com/m/{0}-bikes/{1}/price-in-{2}/", makeMaskingName, modelMaskingName, cityMaskingName);
        ogImage = modelImage;
     %>
    <!-- #include file="/includes/headscript.aspx" -->
    <style type="text/css">
        .model-versions-tabs-wrapper { display:table; background:#fff; }.model-versions-tabs-wrapper a { padding:10px 20px; display:table-cell; font-size:14px; color:#82888b; }.model-versions-tabs-wrapper a:hover { text-decoration:none; color:#4d5057; }.model-versions-tabs-wrapper a.active { border-bottom:3px solid #ef3f30; font-weight:bold; color:#4d5057; }.border-divider { border-top:1px solid #e2e2e2; }.model-version-image-content { width:292px; overflow:hidden; }.model-version-image-content img { width:100%; }#versionPriceInCityWrapper .selectAreaToGetList li { margin-top:13px; }.bullet-point { padding-left:13px; background:url('data:image/png;base64,iVBORw0KGgoAAAANSUhEUgAAAAYAAAAGCAYAAADgzO9IAAAAGXRFWHRTb2Z0d2FyZQBBZG9iZSBJbWFnZVJlYWR5ccllPAAAAyZpVFh0WE1MOmNvbS5hZG9iZS54bXAAAAAAADw/eHBhY2tldCBiZWdpbj0i77u/IiBpZD0iVzVNME1wQ2VoaUh6cmVTek5UY3prYzlkIj8+IDx4OnhtcG1ldGEgeG1sbnM6eD0iYWRvYmU6bnM6bWV0YS8iIHg6eG1wdGs9IkFkb2JlIFhNUCBDb3JlIDUuNi1jMDY3IDc5LjE1Nzc0NywgMjAxNS8wMy8zMC0yMzo0MDo0MiAgICAgICAgIj4gPHJkZjpSREYgeG1sbnM6cmRmPSJodHRwOi8vd3d3LnczLm9yZy8xOTk5LzAyLzIyLXJkZi1zeW50YXgtbnMjIj4gPHJkZjpEZXNjcmlwdGlvbiByZGY6YWJvdXQ9IiIgeG1sbnM6eG1wPSJodHRwOi8vbnMuYWRvYmUuY29tL3hhcC8xLjAvIiB4bWxuczp4bXBNTT0iaHR0cDovL25zLmFkb2JlLmNvbS94YXAvMS4wL21tLyIgeG1sbnM6c3RSZWY9Imh0dHA6Ly9ucy5hZG9iZS5jb20veGFwLzEuMC9zVHlwZS9SZXNvdXJjZVJlZiMiIHhtcDpDcmVhdG9yVG9vbD0iQWRvYmUgUGhvdG9zaG9wIENDIDIwMTUgKFdpbmRvd3MpIiB4bXBNTTpJbnN0YW5jZUlEPSJ4bXAuaWlkOkE0NDhENDQ2MTY5MTExRTZBRTE3QzMxMDE4N0IwNTUyIiB4bXBNTTpEb2N1bWVudElEPSJ4bXAuZGlkOkE0NDhENDQ3MTY5MTExRTZBRTE3QzMxMDE4N0IwNTUyIj4gPHhtcE1NOkRlcml2ZWRGcm9tIHN0UmVmOmluc3RhbmNlSUQ9InhtcC5paWQ6QTQ0OEQ0NDQxNjkxMTFFNkFFMTdDMzEwMTg3QjA1NTIiIHN0UmVmOmRvY3VtZW50SUQ9InhtcC5kaWQ6QTQ0OEQ0NDUxNjkxMTFFNkFFMTdDMzEwMTg3QjA1NTIiLz4gPC9yZGY6RGVzY3JpcHRpb24+IDwvcmRmOlJERj4gPC94OnhtcG1ldGE+IDw/eHBhY2tldCBlbmQ9InIiPz6QHJtYAAAARElEQVR42mJcsX4zGwMDQzcQxwAxIxAvBuJSFiDRBcR5DAgAYn9nAhKxDJgglYkBB2CCmokO5oDsKINaCjMSpLAWIMAAxGMKcqcmhHwAAAAASUVORK5CYII=') no-repeat 0% 50%; }.btn-xxlg { padding: 8px 62px; }.text-x-black { color:#1a1a1a; }.dealer-details-item a:hover { text-decoration:none; }.phone-black-icon { width:11px; height:15px; position:relative; top:3px; margin-right:6px; background-position:-73px -444px; }.mail-grey-icon { width:12px; height:10px; margin-right:6px; background-position:-92px -446px; }#modelPriceInNearbyCities li { width:200px; display:inline-block; vertical-align:top; margin-right:30px; margin-bottom:10px; }#modelPriceInNearbyCities li a { width:135px; overflow:hidden; text-overflow:ellipsis; white-space:nowrap; padding-right:5px; display:inline-block; vertical-align:top; }#modelPriceInNearbyCities .nearby-city-price { width:60px; color:#2a2a2a; text-align:right; display:inline-block; vertical-align:top; }.blue-right-arrow-icon { width:6px; height:10px; background-position:-74px -469px; position:relative; top:1px; left:7px; }.dealership-loc-icon { width:8px; height:12px; background-position:-53px -469px; position:relative;top:4px; }.dealership-address { width:92%; }.vertical-top { display:inline-block;vertical-align:top; }
        .inr-sm, .inr-sm-grey, .inr-sm-dark { width:8px; height:12px; }.inr-sm {background-position:-110px -468px; }.inr-sm-grey{background-position: -92px -468px;}.inr-sm-dark{background-position: -128px -468px;}.inr-lg{ width:10px; height:14px; }.inr-lg { background-position:-110px -490px; }
    </style>
</head>
<script type="text/javascript">
    var clientIP = "<%= clientIP%>";
    </script>
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
                                    <span itemprop="title"><%=makeName %></span>
                                </a></li>
                            <li><span class="bwsprite fa-angle-right margin-right10"></span>
                                <a href="/<%= makeMaskingName %>-bikes/<%= modelMaskingName %>/" itemprop="url">
                                <span><%= modelName %></span>
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
                    <p class="padding-top20 padding-right20 padding-bottom5 padding-left20 text-light-grey"><%=bikeName %> On-road price in <%=cityName %>&nbsp;<span class="bwsprite inr-sm-grey"></span><% if(firstVersion!= null){ %>&nbsp;<%=CommonOpn.FormatPrice(firstVersion.OnRoadPrice.ToString()) %> <% } %>  onwards. 
                       <% if(versionCount > 1){ %> This bike comes in <%=versionCount %> versions.<br /> <% } %>Click on any version name to know on-road price in this city:</p>
                    <div id='versions' class="model-versions-tabs-wrapper">
                        <asp:Repeater ID="rpVersioNames" runat="server">
                            <ItemTemplate>
                                <a id="<%# DataBinder.Eval(Container.DataItem, "VersionId").ToString() %>" href="javascript:void(0)"><%# DataBinder.Eval(Container.DataItem, "VersionName").ToString() %></a>
                            </ItemTemplate>
                        </asp:Repeater>
                    </div>
                    <div class="border-divider"></div>

                    <div id="modelVersionDetailsWrapper" class="text-light-grey padding-bottom20">
                        <div class="grid-4 padding-top10">
                            <div class="model-version-image-content">
                                <img src="<%=modelImage %>" title="<%= title %>" alt="<%= title %>" />
                            </div>
                        </div>
                        <div class="grid-4 padding-top15">
                            <asp:Repeater ID="rprVersionPrices" runat="server">
                                <ItemTemplate>
                                    <div class="priceTable hide" id="<%# DataBinder.Eval(Container.DataItem, "VersionId").ToString() %>">
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
                                                <td class="text-bold text-default">On-road price in <%=TargetedCity %></td>
                                                <td align="right" class="font16 text-bold text-default"><span class="bwsprite inr-lg"></span>
                                                    <%#CommonOpn.FormatPrice(DataBinder.Eval(Container.DataItem,"OnRoadPrice").ToString()) %>
                                                </td>
                                            </tr>
                                        </table>
                                    </div>
                                </ItemTemplate>
                            </asp:Repeater>
                        </div>

                        <div class="grid-4 padding-top15 padding-left30">
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
                        </div>
                        <div class="clear"></div>
                    </div> 

                    <BW:Dealers ID="ctrlDealers" runat="server" />

                    <BW:ModelPriceInNearestCities ID="ctrlTopCityPrices" runat="server" />

                </div>
            </div>
            <div class="clear"></div>
        </section>

        <section class="container margin-bottom30">
            <div class="grid-12">
                <div class="content-box-shadow padding-bottom20">
                    <% if (ctrlAlternativeBikes.FetchedRecordsCount > 0) { %>
                    <!-- Alternative reviews ends -->
                    <BW:AlternativeBikes ID="ctrlAlternativeBikes" runat="server" />
                    <!-- Alternative reviews ends -->
                    <% } %> 
                </div>
            </div>
            <div class="clear"></div>
        </section>

          <BW:LeadCapture ID="ctrlLeadCapture"  runat="server" />

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

                customerViewModel.setOptions(leadOptions);

            });

            $('.model-versions-tabs-wrapper a').on('click', function () {
                var verid = $(this).attr('id');
                showTab(verid);
            });
            $(document).ready(function () {
                $('#versions a').first().addClass('active');
                $('.priceTable').first().show();
                AddCommaToNumber();
            });
            function showTab(version) {
                $('.model-versions-tabs-wrapper a').removeClass('active');
                $('.model-versions-tabs-wrapper a[id="' + version + '"]').addClass('active');
                $('.priceTable').hide();
                $('.priceTable[id="' + version + '"]').show();
            }

            function AddCommaToNumber() {
                $('.comma').each(function () {
                    var number = parseInt($(this).html());
                    $(this).html(numberWithCommas(number));
                });
            }
            // Works only with Int
            function numberWithCommas(x) {
                return x.toString().substring(0, x.toString().length - 3).replace(/\B(?=(\d{2})+(?!\d))/g, ",") + "," + x.toString().substring(x.toString().length - 3);
            }
        </script>
    </form>
</body>
</html>
