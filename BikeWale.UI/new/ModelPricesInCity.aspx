<%@ Page Language="C#" AutoEventWireup="false" CodeBehind="ModelPricesInCity.aspx.cs" Inherits="Bikewale.New.ModelPricesInCity" %>

<!doctype html>
<html>
<head>
    <!-- #include file="/includes/headscript.aspx" -->
    <style type="text/css">
        .model-versions-tabs-wrapper { display:table; background:#fff; }.model-versions-tabs-wrapper a { padding:10px 20px; display:table-cell; font-size:14px; color:#82888b; }.model-versions-tabs-wrapper a:hover { text-decoration:none; color:#4d5057; }.model-versions-tabs-wrapper a.active { border-bottom:3px solid #ef3f30; font-weight:bold; color:#4d5057; }.border-divider { border-top:1px solid #e2e2e2; }.model-version-image-content { width:292px; overflow:hidden; }.model-version-image-content img { width:100%; }#versionPriceInCityWrapper .selectAreaToGetList li { margin-top:13px; }.bullet-point { padding-left:13px; background:url('data:image/png;base64,iVBORw0KGgoAAAANSUhEUgAAAAYAAAAGCAYAAADgzO9IAAAAGXRFWHRTb2Z0d2FyZQBBZG9iZSBJbWFnZVJlYWR5ccllPAAAAyZpVFh0WE1MOmNvbS5hZG9iZS54bXAAAAAAADw/eHBhY2tldCBiZWdpbj0i77u/IiBpZD0iVzVNME1wQ2VoaUh6cmVTek5UY3prYzlkIj8+IDx4OnhtcG1ldGEgeG1sbnM6eD0iYWRvYmU6bnM6bWV0YS8iIHg6eG1wdGs9IkFkb2JlIFhNUCBDb3JlIDUuNi1jMDY3IDc5LjE1Nzc0NywgMjAxNS8wMy8zMC0yMzo0MDo0MiAgICAgICAgIj4gPHJkZjpSREYgeG1sbnM6cmRmPSJodHRwOi8vd3d3LnczLm9yZy8xOTk5LzAyLzIyLXJkZi1zeW50YXgtbnMjIj4gPHJkZjpEZXNjcmlwdGlvbiByZGY6YWJvdXQ9IiIgeG1sbnM6eG1wPSJodHRwOi8vbnMuYWRvYmUuY29tL3hhcC8xLjAvIiB4bWxuczp4bXBNTT0iaHR0cDovL25zLmFkb2JlLmNvbS94YXAvMS4wL21tLyIgeG1sbnM6c3RSZWY9Imh0dHA6Ly9ucy5hZG9iZS5jb20veGFwLzEuMC9zVHlwZS9SZXNvdXJjZVJlZiMiIHhtcDpDcmVhdG9yVG9vbD0iQWRvYmUgUGhvdG9zaG9wIENDIDIwMTUgKFdpbmRvd3MpIiB4bXBNTTpJbnN0YW5jZUlEPSJ4bXAuaWlkOkE0NDhENDQ2MTY5MTExRTZBRTE3QzMxMDE4N0IwNTUyIiB4bXBNTTpEb2N1bWVudElEPSJ4bXAuZGlkOkE0NDhENDQ3MTY5MTExRTZBRTE3QzMxMDE4N0IwNTUyIj4gPHhtcE1NOkRlcml2ZWRGcm9tIHN0UmVmOmluc3RhbmNlSUQ9InhtcC5paWQ6QTQ0OEQ0NDQxNjkxMTFFNkFFMTdDMzEwMTg3QjA1NTIiIHN0UmVmOmRvY3VtZW50SUQ9InhtcC5kaWQ6QTQ0OEQ0NDUxNjkxMTFFNkFFMTdDMzEwMTg3QjA1NTIiLz4gPC9yZGY6RGVzY3JpcHRpb24+IDwvcmRmOlJERj4gPC94OnhtcG1ldGE+IDw/eHBhY2tldCBlbmQ9InIiPz6QHJtYAAAARElEQVR42mJcsX4zGwMDQzcQxwAxIxAvBuJSFiDRBcR5DAgAYn9nAhKxDJgglYkBB2CCmokO5oDsKINaCjMSpLAWIMAAxGMKcqcmhHwAAAAASUVORK5CYII=') no-repeat 0% 50%; }.btn-xxlg { padding: 8px 62px; }.text-x-black { color:#1a1a1a; }.dealer-details-item a:hover { text-decoration:none; }.phone-black-icon { width:11px; height:15px; position:relative; top:3px; margin-right:6px; background-position:-73px -444px; }.mail-grey-icon { width:12px; height:10px; margin-right:6px; background-position:-92px -446px; }#modelPriceInNearbyCities li { width:200px; display:inline-block; vertical-align:top; margin-right:30px; margin-bottom:10px; }#modelPriceInNearbyCities li a { width:135px; overflow:hidden; text-overflow:ellipsis; white-space:nowrap; padding-right:5px; display:inline-block; vertical-align:top; }#modelPriceInNearbyCities .nearby-city-price { width:60px; color:#2a2a2a; text-align:right; display:inline-block; vertical-align:top; }.blue-right-arrow-icon { width:6px; height:10px; background-position:-74px -469px; position:relative; top:1px; left:7px; }.dealership-loc-icon { width:8px; height:12px; background-position:-53px -469px; position:relative;top:4px; }.dealership-address { width:92%; }.vertical-top { display:inline-block;vertical-align:top; }
    </style>
</head>
<body class="bg-light-grey header-fixed-inner">
    <form runat="server">
        <!-- #include file="/includes/headBW.aspx" -->
        <section class="bg-light-grey padding-top10">
            <div class="container">
                <div class="grid-12">
                    <div class="breadcrumb margin-bottom15">
                        <ul>
                            <li itemscope="" itemtype="http://data-vocabulary.org/Breadcrumb"><a href="/" itemprop="url">
                                <span itemprop="title">Home</span></a>
                            </li>
                            <li itemscope="" itemtype="http://data-vocabulary.org/Breadcrumb">
                                <span class="fa fa-angle-right margin-right10"></span>
                                <span>On-road price</span>
                            </li>
                        </ul>
                        <div class="clear"></div>
                    </div>
                    <h1 class="font22 text-default margin-bottom20">Bajaj Pulsar RS200 price in Pune</h1>
                </div>
                <div class="clear"></div>
            </div>
        </section>

        <section id="versionPriceInCityWrapper" class="container margin-bottom25">
            <div class="grid-12 font14">
                <div class="content-box-shadow">
                    <p class="padding-top20 padding-right20 padding-bottom5 padding-left20 text-light-grey">Bajaj Pulsar On-road price in Pune - <span class="fa fa-rupee"></span>&nbsp;1,25,657  onwards. This bike comes in 4 versions.<br />Click on any version name to know on-road price in this city:</p>
                    <div class="model-versions-tabs-wrapper">
                        <a href="javascript:void(0)" class="active">Electric Start/Drum/Alloy</a>
                        <a href="javascript:void(0)">Electric Start/Disc/Alloy</a>
                        <a href="javascript:void(0)">Standard</a>
                        <a href="javascript:void(0)">ABS</a>
                    </div>
                    <div class="border-divider"></div>

                    <div id="modelVersionDetailsWrapper" class="text-light-grey padding-bottom20">
                        <div class="grid-4 padding-top10">
                            <div class="model-version-image-content">
                                <img src="http://imgd1.aeplcdn.com//310x174//bw/models/tvs-wego-drum-165.jpg?20151209224944" title="" alt="" />
                            </div>
                        </div>
                        <div class="grid-4 padding-top15">
                            <table cellspacing="0" cellpadding="0" width="100%" border="0">
                                <tr>
                                    <td width="200" class="padding-bottom15">Ex-showroom</td>
                                    <td align="right" class="padding-bottom15 text-default"><span class="fa fa-rupee"></span>&nbsp;1,25,657</td>
                                </tr>
                                <tr>
                                    <td class="padding-bottom15">RTO</td>
                                    <td align="right" class="padding-bottom15 text-default"><span class="fa fa-rupee"></span>&nbsp;3,000</td>
                                </tr>
                                <tr>
                                    <td class="padding-bottom15">Insurance</td>
                                    <td align="right" class="padding-bottom15 text-default"><span class="fa fa-rupee"></span>&nbsp;1,500</td>
                                </tr>
                                <tr>
                                    <td class="padding-bottom15">Accessories</td>
                                    <td align="right" class="padding-bottom15 text-default"><span class="fa fa-rupee"></span>&nbsp;1,500</td>
                                </tr>
                                <tr>
                                    <td colspan="2" class="padding-bottom15 border-divider"></td>
                                </tr>
                                <tr>
                                    <td class="text-bold text-default">On-road price in Pune</td>
                                    <td align="right" class="font16 text-bold text-default"><span class="fa fa-rupee"></span>&nbsp;1,25,657</td>
                                </tr>
                            </table>
                        </div>
                        <div class="grid-4 padding-top15 padding-left30">
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
                            <a href="javascript:void(0)" class="btn btn-orange btn-xxlg font14">Select your area</a>
                        </div>
                        <div class="clear"></div>
                    </div>
                    <div class="margin-right20 margin-left20 border-divider"></div>

                    <div id="dealersInCityWrapper" class="padding-top20 padding-bottom20">
                        <h2 class="font14 text-bold text-x-black padding-right20 padding-left20">Bajaj dealers in Mumbai</h2>
                        <div class="grid-12 padding-top15">
                            <ul>
                                <li class="dealer-details-item grid-4 margin-bottom25">
                                    <h3 class="font14"><a href="" class="text-default">Kamala Landmarc Motorbikes</a></h3>
                                    <div class="margin-top10">
                                        <p class="text-light-grey margin-bottom5">
                                            <span class="bwsprite dealership-loc-icon vertical-top margin-right5"></span>
                                            <span class="vertical-top dealership-address">Vishwaroop IT Park, Sector 30, Navi Mumbai, Maharashtra, 400067</span>
                                        </p>
                                        <p class="margin-bottom5"><span class="text-bold"><span class="bwsprite phone-black-icon"></span><span>9876543210</span></span></p>
                                        <p class="margin-bottom15"><a href="mailto:bikewale@motors.com" class="text-light-grey"><span class="bwsprite mail-grey-icon"></span><span>bikewale@motors.com</span></a></p>
                                        <a href="" class="btn btn-grey btn-md font14">Get offers from dealer</a>
                                    </div>
                                    <div class="clear"></div>
                                </li>
                                <li class="dealer-details-item grid-4 margin-bottom25">
                                    <h3 class="font14"><a href="" class="text-default">Kamala Landmarc Motorbikes</a></h3>
                                    <div class="margin-top10">
                                        <p class="text-light-grey margin-bottom5">
                                            <span class="bwsprite dealership-loc-icon vertical-top margin-right5"></span>
                                            <span class="vertical-top dealership-address">Vishwaroop IT Park, Sector 30, Navi Mumbai, Maharashtra, 400067</span>
                                        </p>
                                        <p class="margin-bottom5"><span class="text-bold"><span class="bwsprite phone-black-icon"></span><span>9876543210</span></span></p>
                                        <p class="margin-bottom15"><a href="mailto:bikewale@motors.com" class="text-light-grey"><span class="bwsprite mail-grey-icon"></span><span>bikewale@motors.com</span></a></p>
                                        <a href="" class="btn btn-grey btn-md font14">Get offers from dealer</a>
                                    </div>
                                    <div class="clear"></div>
                                </li>
                                <li class="dealer-details-item grid-4 margin-bottom25">
                                    <h3 class="font14"><a href="" class="text-default">Kamala Landmarc Motorbikes</a></h3>
                                    <div class="margin-top10">
                                        <p class="text-light-grey margin-bottom5">
                                            <span class="bwsprite dealership-loc-icon vertical-top margin-right5"></span>
                                            <span class="vertical-top dealership-address">Vishwaroop IT Park, Sector 30, Navi Mumbai, Maharashtra, 400067</span>
                                        </p>
                                        <p class="margin-bottom5"><span class="text-bold"><span class="bwsprite phone-black-icon"></span><span>9876543210</span></span></p>
                                        <p class="margin-bottom15"><a href="mailto:bikewale@motors.com" class="text-light-grey"><span class="bwsprite mail-grey-icon"></span><span>bikewale@motors.com</span></a></p>
                                        <a href="" class="btn btn-grey btn-md font14">Get offers from dealer</a>
                                    </div>
                                    <div class="clear"></div>
                                </li>
                            </ul>
                        </div>
                        <div class="clear"></div>
                        <a href="" class="margin-left20">View all dealers<span class="bwsprite blue-right-arrow-icon"></span></a>
                    </div>
                    <div class="margin-right20 margin-left20 border-divider"></div>

                    <div id="modelPriceInNearbyCities" class="content-inner-block-20">
                        <h2 class="font14 text-bold text-x-black margin-bottom15">Bajaj Pulsar RS200 price in nearby cities <span class="text-light-grey text-unbold">(Ex-showroom)</span></h2>
                        <ul>
                            <li>
                                <a href="" title="Mumbai">Mumbai</a>
                                <span class="nearby-city-price"><span class="fa fa-rupee"></span>&nbsp;1.67 L</span>
                            </li>
                            <li>
                                <a href="" title="Navi Mumbai">Navi Mumbai</a>
                                <span class="nearby-city-price"><span class="fa fa-rupee"></span>&nbsp;1.32 L</span>
                            </li>
                            <li>
                                <a href="" title="Thane">Thane</a>
                                <span class="nearby-city-price"><span class="fa fa-rupee"></span>&nbsp;1.04 L</span>
                            </li>
                            <li>
                                <a href="" title="Kalyan">Kalyan</a>
                                <span class="nearby-city-price"><span class="fa fa-rupee"></span>&nbsp;87 K</span>
                            </li>
                            <li>
                                <a href="" title="Kalyan">Kalyan</a>
                                <span class="nearby-city-price"><span class="fa fa-rupee"></span>&nbsp;77 K</span>
                            </li>
                        </ul>
                        <div class="clear"></div>
                    </div>
                </div>
            </div>
            <div class="clear"></div>
        </section>

        <section class="container margin-bottom30">
            <div class="grid-12">
                <h2 class="font18 text-bold text-x-black margin-bottom15 padding-left20">Bajaj Pulsar RS200 Alternate bikes</h2>
            </div>
            <div class="clear"></div>
        </section>

        <!-- #include file="/includes/footerBW.aspx" -->
        <!-- #include file="/includes/footerscript.aspx" -->
        <script type="text/javascript">
            $('.model-versions-tabs-wrapper a').on('click', function () {
                $('.model-versions-tabs-wrapper a').removeClass('active');
                $(this).addClass('active');
            });
        </script>

    </form>
</body>
</html>
