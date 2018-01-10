<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="BikeDetails_v2.aspx.cs" Inherits="Bikewale.Used.BikeDetails_v2" %>

<!DOCTYPE html>
<html>
<head runat="server">
    <%  
        title = "Used bike details";
        isHeaderFix = false;
    %>
    
    <!-- #include file="/includes/headscript_desktop_min.aspx" -->
    <link type="text/css" href="/css/used/details.css" rel="stylesheet" />

    <script type="text/javascript">
        <!-- #include file="\includes\gacode_desktop.aspx" -->
    </script>
</head>
<body>
    <form id="form1" runat="server">
        <!-- #include file="/includes/headBW.aspx" -->
        <section class="bg-light-grey padding-top10" id="breadcrumb">
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
                                <a href="/" itemprop="url">
                                    <span itemprop="title">Used Bikes</span>
                                </a></li>
                            <li><span class="bwsprite fa-angle-right margin-right10"></span>
                                <span>Royal Enfield Classic Desert Storm</span>
                            </li>
                        </ul>
                        <div class="clear"></div>
                    </div>
                </div>
                <div class="clear"></div>
            </div>
        </section>

        <section>
            <div class="container">
                <div class="grid-12 margin-bottom20">
                    <div class="content-box-shadow content-inner-block-20">
                        <h1 class="margin-bottom20">2013, Royal Enfield Classic Desert Storm</h1>
                        <div id="bike-main-carousel" class="grid-5 alpha">
                            <div class="jcarousel-wrapper">
                                <div class="jcarousel">
                                    <ul>
                                        <li>
                                            <div class="carousel-img-container">
                                                <span>
                                                    <img src="https://imgd.aeplcdn.com/476x268//staging/bw/used/S42661/42661_20160721050709814.jpg" title="Bajaj Pulsar RS200 Tail lamp" alt="Bajaj Pulsar RS200 Tail lamp" border="0">
                                                </span>
                                            </div>
                                        </li>
                                        <li>
                                            <div class="carousel-img-container">
                                                <span>
                                                    <img src="https://imgd.aeplcdn.com/476x268//staging/bw/used/S42670/42670_20160722042837543.jpg" title="Bajaj Pulsar RS200 Tail lamp" alt="Bajaj Pulsar RS200 Tail lamp" border="0">
                                                </span>
                                            </div>
                                        </li>
                                        <li>
                                            <div class="carousel-img-container">
                                                <span>
                                                    <img src="https://imgd.aeplcdn.com//476x268//bw/used/S42602/42602_20160613114137617.jpg" title="Bajaj Pulsar RS200 Tail lamp" alt="Bajaj Pulsar RS200 Tail lamp" border="0">
                                                </span>
                                            </div>
                                        </li>
                                        <li>
                                            <div class="carousel-img-container">
                                                <span>
                                                    <img src="https://imgd.aeplcdn.com//476x268//bw/used/S42598/42598_20160613081003622.jpg" title="Bajaj Pulsar RS200 Tail lamp" alt="Bajaj Pulsar RS200 Tail lamp" border="0">
                                                </span>
                                            </div>
                                        </li>
                                    </ul>
                                </div>
                                <div class="model-media-details">
                                    <div class="model-media-item">
                                        <span class="bwsprite gallery-photo-icon"></span>
                                        <span class="model-media-count">4</span>
                                    </div>
                                </div>
                                <span class="jcarousel-control-left">
                                    <a href="#" class="bwsprite jcarousel-control-prev inactive" rel="nofollow" data-jcarouselcontrol="true"></a>
                                </span>
                                <span class="jcarousel-control-right">
                                    <a href="#" class="bwsprite jcarousel-control-next" rel="nofollow" data-jcarouselcontrol="true"></a>
                                </span>
                            </div>
                        </div>
                        <div id="ad-summary" class="grid-7 padding-left30 omega font14">
                            <h2 class="text-default ad-summary-label margin-bottom10">Ad summary</h2>
                            <div class="grid-4 alpha margin-bottom10"> 
                                <span class="bwsprite model-date-icon"></span>
                                <span class="model-details-label">2011 model</span>
                            </div>
                                                
                            <div class="grid-4 alpha omega margin-bottom10">
                                <span class="bwsprite kms-driven-icon"></span>
                                <span class="model-details-label">18,000 kms</span>
                            </div>
                            <div class="clear"></div>
                                                
                            <div class="grid-4 alpha margin-bottom10">
                                <span class="bwsprite author-grey-sm-icon"></span>
                                <span class="model-details-label">1st Owner</span>
                            </div>
                                                
                            <div class="grid-4 alpha omega margin-bottom10">
                                <span class="bwsprite model-loc-icon"></span>
                                <span class="model-details-label">Kayamkulam</span>
                            </div>                                                
                            <div class="clear"></div>

                            <div class="margin-top5 margin-bottom10">
                                <span class="bwsprite inr-md-lg"></span>
                                <span class="font24 text-bold">1,22,000</span>
                            </div>

                            <a href="javascript:void(0)" class="btn btn-orange ad-summary-btn" rel="nofollow">Get seller details</a>
                        </div>
                        <div class="clear"></div>

                        <div class="margin-bottom20 border-solid-bottom"></div>

                        <h2 class="text-default margin-bottom15">Bike details</h2>
                        <div class="grid-6 alpha border-solid-right margin-bottom20">
                            <ul class="key-value-list font14">
                                <li>
                                    <p class="bike-details-key">Profile ID</p>
                                    <p class="bike-details-value">1348762</p>
                                </li>
                                <li>
                                    <p class="bike-details-key">Date updated</p>
                                    <p class="bike-details-value">02 Nov 2015</p>
                                </li>
                                <li>
                                    <p class="bike-details-key">Seller</p>
                                    <p class="bike-details-value">Individual</p>
                                </li>
                                <li>
                                    <p class="bike-details-key">Registration year</p>
                                    <p class="bike-details-value">Aug 2013</p>
                                </li>
                            </ul>
                        </div>
                        <div class="grid-6 padding-left40 omega margin-bottom20">
                            <ul class="key-value-list font14">
                                <li>
                                    <p class="bike-details-key">Colour</p>
                                    <p class="bike-details-value">Yellow</p>
                                </li>
                                <li>
                                    <p class="bike-details-key">Bike registered at</p>
                                    <p class="bike-details-value">Thane</p>
                                </li>
                                <li>
                                    <p class="bike-details-key">Insurance</p>
                                    <p class="bike-details-value">Comprehensive</p>
                                </li>
                                <li>
                                    <p class="bike-details-key">Registration no.</p>
                                    <p class="bike-details-value">MH-02-BN-9011</p>
                                </li>
                            </ul>
                        </div>
                        <div class="clear"></div>

                        <div class="margin-bottom20 border-solid-bottom"></div>

                        <h2 class="text-default margin-bottom15">Ad description</h2>
                        <p class="font14 text-light-grey">This used Honda Dio comes in a shade of inviting grey. The age of this bike is roughly 3 years. The original registration certificate is present</p>
                    </div>
                </div>
                <div class="clear"></div>
            </div>
        </section>

        <section>
            <div class="container">
                <div id="makeTabsContentWrapper" class="grid-12 margin-bottom20">
                    <div class="content-box-shadow">
                        <div id="makeOverallTabsWrapper">
                            <div id="makeOverallTabs" class="overall-floating-tabs">
                                <div class="overall-specs-tabs-wrapper">
                                    <a href="#specsContent" rel="nofollow">Specs & Features</a>
                                    <a href="#similarContent" rel="nofollow">Similar bikes</a>
                                    <a href="#usedContent" rel="nofollow">Used Bajaj bikes</a>
                                </div>
                            </div>
                        </div>
                        <div id="specsContent" class="bw-model-tabs-data specs-features-list font14">
                            <h2 class="content-inner-block-20">Specifications summary</h2>
                            <div class="grid-4 omega">
                                <div class="grid-6 text-light-grey">
                                    <p>Displacement</p>
                                    <p>Max Power</p>
                                    <p>Maximum Torque</p>
                                    <p>No. of gears</p>
                                </div>
                                <div class="grid-6 omega text-bold">
                                    <p>199.50 cc</p>
                                    <p title="24.50 bhp @ 9,750 rpm">24.50 bhp @ 9,750 rpm</p>
                                    <p title="18.60 Nm @ 8,000 rpm">18.60 Nm @ 8,000 rpm</p>
                                    <p>6</p>
                                </div>
                                <div class="clear"></div>
                            </div>
                            <div class="grid-4 omega">
                                <div class="grid-6 text-light-grey">
                                    <p>Brake Type</p>
                                    <p>Front Disc</p>
                                    <p>Rear Disc</p>
                                    <p>Front Disc/Drum Size</p>
                                </div>
                                <div class="grid-6 omega text-bold">
                                    <p>Disc</p>
                                    <p>Yes</p>
                                    <p>Yes</p>
                                    <p>300 mm</p>
                                </div>
                                <div class="clear"></div>
                            </div>
                            <div class="grid-4">
                                <div class="grid-6 text-light-grey">
                                    <p>Alloy Wheels</p>
                                    <p>Front Suspension</p>
                                    <p>Rear Tyre</p>
                                    <p>Tubeless Tyres</p>
                                </div>
                                <div class="grid-6 alpha text-bold">
                                    <p>Yes</p>
                                    <p title="Telescopic Front Fork with Antifriction Bush">Telescopic Front Fork with Antifriction Bush</p>
                                    <p title="130/70 – 17, 62 P Tubeless">130/70 – 17, 62 P Tubeless</p>
                                    <p>Yes</p>
                                </div>
                                <div class="clear"></div>
                            </div>
                            <div class="clear"></div>
                            <div class="padding-left20 margin-bottom10">
                                <a href="javascript:void(0)" class="" title="Royal Enfield Classic Desert Storm specifications" rel="nofollow">View full specifications <span class="bwsprite blue-right-arrow-icon"></span></a>
                            </div>

                            <div class="grid-8 alpha margin-bottom25">
                                <h2 class="content-inner-block-20">Features summary</h2>
                                <div class="grid-6 omega">
                                    <div class="grid-6 text-light-grey">
                                        <p>Brake Type</p>
                                        <p>Front Disc</p>
                                        <p>Rear Disc</p>
                                        <p>Front Disc/Drum Size</p>
                                    </div>
                                    <div class="grid-6 omega text-bold">
                                        <p>Disc</p>
                                        <p>Yes</p>
                                        <p>Yes</p>
                                        <p>300 mm</p>
                                    </div>
                                    <div class="clear"></div>
                                </div>
                                <div class="grid-6 padding-left15">
                                    <div class="grid-6 omega text-light-grey">
                                        <p>Alloy Wheels</p>
                                        <p>Front Suspension</p>
                                        <p>Rear Tyre</p>
                                        <p>Tubeless Tyres</p>
                                    </div>
                                    <div class="grid-6 padding-left20 omega text-bold">
                                        <p>Yes</p>
                                        <p title="Telescopic Front Fork with Antifriction Bush">Telescopic Front Fork with Antifriction Bush</p>
                                        <p title="130/70 – 17, 62 P Tubeless">130/70 – 17, 62 P Tubeless</p>
                                        <p>Yes</p>
                                    </div>
                                    <div class="clear"></div>
                                </div>
                                <div class="clear"></div>
                                <div class="padding-left20">
                                    <a href="javascript:void(0)" class="" title="Royal Enfield Classic Desert Storm features" rel="nofollow">View all features <span class="bwsprite blue-right-arrow-icon"></span></a>
                                </div>
                            </div>
                            <div class="grid-4 text-center alpha margin-bottom25">
                                <!-- ad -->
                            </div>
                            <div class="clear"></div>
                            <div class="margin-right10 margin-left10 border-solid-bottom"></div>
                        </div>

                        <div id="similarContent" class="bw-model-tabs-data padding-top20 font14">
                            <h2 class="padding-left20 margin-bottom15">Similar used Royal Enfield Classic Desert Storm bikes</h2>
                            <div class="jcarousel-wrapper inner-content-carousel used-bikes-carousel">
                                <div class="jcarousel">
                                    <ul>
                                        <li>
                                            <a href="" title="Bajaj Discover 150S" class="jcarousel-card">
                                                <div class="model-jcarousel-image-preview">
                                                    <span class="card-image-block">
                                                        <img class="lazy" data-original="https://imgd.aeplcdn.com/310x174//staging/bw/used/S42661/42661_20160721050709814.jpg" alt="Bajaj Discover 150S" border="0">
                                                    </span>
                                                </div>
                                                <div class="card-desc-block">
                                                    <h3 class="bikeTitle">Bajaj Discover 150S</h3>
                                                    <div class="grid-6 alpha"> 
                                                        <span class="bwsprite model-date-icon"></span>
                                                        <span class="model-details-label">2011 model</span>
                                                    </div>
                                                
                                                    <div class="grid-6 alpha omega">
                                                        <span class="bwsprite kms-driven-icon"></span>
                                                        <span class="model-details-label">18,000 kms</span>
                                                    </div>
                                                
                                                    <div class="grid-6 alpha">
                                                        <span class="bwsprite author-grey-sm-icon"></span>
                                                        <span class="model-details-label">1st Owner</span>
                                                    </div>
                                                
                                                    <div class="grid-6 alpha omega">
                                                        <span class="bwsprite model-loc-icon"></span>
                                                        <span class="model-details-label">Kayamkulam</span>
                                                    </div>                                                
                                                    <div class="clear"></div>

                                                    <span class="bwsprite inr-lg"></span>&nbsp;<span class="font18 text-default text-bold">55,635</span>
                                                </div>
                                            </a>
                                            <div class="margin-left20 margin-bottom20">
                                                <a href="javascript:void(0)" class="btn btn-sm btn-grey font14" rel="nofollow">Get seller details</a>
                                            </div>
                                        </li>
                                        <li>
                                            <a href="" title="Suzuki GS150R Standard" class="jcarousel-card">
                                                <div class="model-jcarousel-image-preview">
                                                    <span class="card-image-block">
                                                        <img class="lazy" data-original="https://imgd.aeplcdn.com//310x174//bw/used/S42598/42598_20160613081003622.jpg" alt="Suzuki GS150R Standard" border="0">
                                                    </span>
                                                </div>
                                                <div class="card-desc-block">
                                                    <h3 class="bikeTitle">Suzuki GS150R Standard</h3>
                                                    <div class="grid-6 alpha"> 
                                                        <span class="bwsprite model-date-icon"></span>
                                                        <span class="model-details-label">2011 model</span>
                                                    </div>
                                                
                                                    <div class="grid-6 alpha omega">
                                                        <span class="bwsprite kms-driven-icon"></span>
                                                        <span class="model-details-label">18,000 kms</span>
                                                    </div>
                                                
                                                    <div class="grid-6 alpha">
                                                        <span class="bwsprite author-grey-sm-icon"></span>
                                                        <span class="model-details-label">1st Owner</span>
                                                    </div>
                                                
                                                    <div class="grid-6 alpha omega">
                                                        <span class="bwsprite model-loc-icon"></span>
                                                        <span class="model-details-label">Kayamkulam</span>
                                                    </div>                                                
                                                    <div class="clear"></div>

                                                    <span class="bwsprite inr-lg"></span>&nbsp;<span class="font18 text-default text-bold">35,235</span>
                                                </div>
                                            </a>
                                            <div class="margin-left20 margin-bottom20">
                                                <a href="javascript:void(0)" class="btn btn-sm btn-grey font14" rel="nofollow">Get seller details</a>
                                            </div>
                                        </li>
                                        <li>
                                            <a href="" title="Bajaj Pulsar 150 DTS-i Standard" class="jcarousel-card">
                                                <div class="model-jcarousel-image-preview">
                                                    <span class="card-image-block">
                                                        <img class="lazy" data-original="https://imgd.aeplcdn.com//310x174//bw/used/S42590/42590_20160613051732997.jpg" alt="Bajaj Pulsar 150 DTS-i Standard" border="0">
                                                    </span>
                                                </div>
                                                <div class="card-desc-block">
                                                    <h3 class="bikeTitle">Bajaj Pulsar 150 DTS-i Standard</h3>
                                                    <div class="grid-6 alpha"> 
                                                        <span class="bwsprite model-date-icon"></span>
                                                        <span class="model-details-label">2011 model</span>
                                                    </div>
                                                
                                                    <div class="grid-6 alpha omega">
                                                        <span class="bwsprite kms-driven-icon"></span>
                                                        <span class="model-details-label">18,000 kms</span>
                                                    </div>
                                                
                                                    <div class="grid-6 alpha">
                                                        <span class="bwsprite author-grey-sm-icon"></span>
                                                        <span class="model-details-label">1st Owner</span>
                                                    </div>
                                                
                                                    <div class="grid-6 alpha omega">
                                                        <span class="bwsprite model-loc-icon"></span>
                                                        <span class="model-details-label">Kayamkulam</span>
                                                    </div>                                                
                                                    <div class="clear"></div>

                                                    <span class="bwsprite inr-lg"></span>&nbsp;<span class="font18 text-default text-bold">34,000</span>
                                                </div>
                                            </a>
                                            <div class="margin-left20 margin-bottom20">
                                                <a href="javascript:void(0)" class="btn btn-sm btn-grey font14" rel="nofollow">Get seller details</a>
                                            </div>
                                        </li>
                                        <li>
                                            <a href="" title="KTM Duke 200 Standard" class="jcarousel-card">
                                                <div class="model-jcarousel-image-preview">
                                                    <span class="card-image-block">
                                                        <img class="lazy" data-original="https://imgd.aeplcdn.com//310x174//bw/used/S42584/42584_20160613043659076.jpg" alt="KTM Duke 200 Standard" border="0">
                                                    </span>
                                                </div>
                                                <div class="card-desc-block">
                                                    <h3 class="bikeTitle">KTM Duke 200 Standard</h3>
                                                    <div class="grid-6 alpha"> 
                                                        <span class="bwsprite model-date-icon"></span>
                                                        <span class="model-details-label">2011 model</span>
                                                    </div>
                                                
                                                    <div class="grid-6 alpha omega">
                                                        <span class="bwsprite kms-driven-icon"></span>
                                                        <span class="model-details-label">18,000 kms</span>
                                                    </div>
                                                
                                                    <div class="grid-6 alpha">
                                                        <span class="bwsprite author-grey-sm-icon"></span>
                                                        <span class="model-details-label">1st Owner</span>
                                                    </div>
                                                
                                                    <div class="grid-6 alpha omega">
                                                        <span class="bwsprite model-loc-icon"></span>
                                                        <span class="model-details-label">Kayamkulam</span>
                                                    </div>                                                
                                                    <div class="clear"></div>

                                                    <span class="bwsprite inr-lg"></span>&nbsp;<span class="font18 text-default text-bold">1,45,000</span>
                                                </div>
                                            </a>
                                            <div class="margin-left20 margin-bottom20">
                                                <a href="javascript:void(0)" class="btn btn-sm btn-grey font14" rel="nofollow">Get seller details</a>
                                            </div>
                                        </li>
                                    </ul>
                                </div>
                                <span class="jcarousel-control-left"><a href="#" class="bwsprite jcarousel-control-prev inactive" rel="nofollow"></a></span>
                                <span class="jcarousel-control-right"><a href="#" class="bwsprite jcarousel-control-next" rel="nofollow"></a></span>
                            </div>
                            <div class="padding-left20 margin-top15 padding-bottom20">
                                <a href="javascript:void(0)" class="" title="" rel="nofollow">Royal Enfield Classic Desert Storm bikes in Mumbai <span class="bwsprite blue-right-arrow-icon"></span></a>
                            </div>
                            <div class="margin-right10 margin-left10 border-solid-bottom"></div>
                        </div>

                        <div id="usedContent" class="bw-model-tabs-data padding-top20 font14">
                            <h2 class="padding-left20 margin-bottom15">Used Bajaj bikes for sale</h2>
                            <div class="jcarousel-wrapper inner-content-carousel used-bikes-carousel">
                                <div class="jcarousel">
                                    <ul>
                                        <li>
                                            <a href="" title="Bajaj Discover 150S" class="jcarousel-card">
                                                <div class="model-jcarousel-image-preview">
                                                    <span class="card-image-block">
                                                        <img class="lazy" data-original="https://imgd.aeplcdn.com/310x174//staging/bw/used/S42661/42661_20160721050709814.jpg" alt="Bajaj Discover 150S" border="0">
                                                    </span>
                                                </div>
                                                <div class="card-desc-block">
                                                    <h3 class="bikeTitle">Bajaj Discover 150S</h3>
                                                    <div class="grid-6 alpha"> 
                                                        <span class="bwsprite model-date-icon"></span>
                                                        <span class="model-details-label">2011 model</span>
                                                    </div>
                                                
                                                    <div class="grid-6 alpha omega">
                                                        <span class="bwsprite kms-driven-icon"></span>
                                                        <span class="model-details-label">18,000 kms</span>
                                                    </div>
                                                
                                                    <div class="grid-6 alpha">
                                                        <span class="bwsprite author-grey-sm-icon"></span>
                                                        <span class="model-details-label">1st Owner</span>
                                                    </div>
                                                
                                                    <div class="grid-6 alpha omega">
                                                        <span class="bwsprite model-loc-icon"></span>
                                                        <span class="model-details-label">Kayamkulam</span>
                                                    </div>                                                
                                                    <div class="clear"></div>

                                                    <span class="bwsprite inr-lg"></span>&nbsp;<span class="font18 text-default text-bold">55,635</span>
                                                </div>
                                            </a>
                                            <div class="margin-left20 margin-bottom20">
                                                <a href="javascript:void(0)" class="btn btn-sm btn-grey font14" rel="nofollow">Get seller details</a>
                                            </div>
                                        </li>
                                        <li>
                                            <a href="" title="Suzuki GS150R Standard" class="jcarousel-card">
                                                <div class="model-jcarousel-image-preview">
                                                    <span class="card-image-block">
                                                        <img class="lazy" data-original="https://imgd.aeplcdn.com//310x174//bw/used/S42598/42598_20160613081003622.jpg" alt="Suzuki GS150R Standard" border="0">
                                                    </span>
                                                </div>
                                                <div class="card-desc-block">
                                                    <h3 class="bikeTitle">Suzuki GS150R Standard</h3>
                                                    <div class="grid-6 alpha"> 
                                                        <span class="bwsprite model-date-icon"></span>
                                                        <span class="model-details-label">2011 model</span>
                                                    </div>
                                                
                                                    <div class="grid-6 alpha omega">
                                                        <span class="bwsprite kms-driven-icon"></span>
                                                        <span class="model-details-label">18,000 kms</span>
                                                    </div>
                                                
                                                    <div class="grid-6 alpha">
                                                        <span class="bwsprite author-grey-sm-icon"></span>
                                                        <span class="model-details-label">1st Owner</span>
                                                    </div>
                                                
                                                    <div class="grid-6 alpha omega">
                                                        <span class="bwsprite model-loc-icon"></span>
                                                        <span class="model-details-label">Kayamkulam</span>
                                                    </div>                                                
                                                    <div class="clear"></div>

                                                    <span class="bwsprite inr-lg"></span>&nbsp;<span class="font18 text-default text-bold">35,235</span>
                                                </div>
                                            </a>
                                            <div class="margin-left20 margin-bottom20">
                                                <a href="javascript:void(0)" class="btn btn-sm btn-grey font14" rel="nofollow">Get seller details</a>
                                            </div>
                                        </li>
                                        <li>
                                            <a href="" title="Bajaj Pulsar 150 DTS-i Standard" class="jcarousel-card">
                                                <div class="model-jcarousel-image-preview">
                                                    <span class="card-image-block">
                                                        <img class="lazy" data-original="https://imgd.aeplcdn.com//310x174//bw/used/S42590/42590_20160613051732997.jpg" alt="Bajaj Pulsar 150 DTS-i Standard" border="0">
                                                    </span>
                                                </div>
                                                <div class="card-desc-block">
                                                    <h3 class="bikeTitle">Bajaj Pulsar 150 DTS-i Standard</h3>
                                                    <div class="grid-6 alpha"> 
                                                        <span class="bwsprite model-date-icon"></span>
                                                        <span class="model-details-label">2011 model</span>
                                                    </div>
                                                
                                                    <div class="grid-6 alpha omega">
                                                        <span class="bwsprite kms-driven-icon"></span>
                                                        <span class="model-details-label">18,000 kms</span>
                                                    </div>
                                                
                                                    <div class="grid-6 alpha">
                                                        <span class="bwsprite author-grey-sm-icon"></span>
                                                        <span class="model-details-label">1st Owner</span>
                                                    </div>
                                                
                                                    <div class="grid-6 alpha omega">
                                                        <span class="bwsprite model-loc-icon"></span>
                                                        <span class="model-details-label">Kayamkulam</span>
                                                    </div>                                                
                                                    <div class="clear"></div>

                                                    <span class="bwsprite inr-lg"></span>&nbsp;<span class="font18 text-default text-bold">34,000</span>
                                                </div>
                                            </a>
                                            <div class="margin-left20 margin-bottom20">
                                                <a href="javascript:void(0)" class="btn btn-sm btn-grey font14" rel="nofollow">Get seller details</a>
                                            </div>
                                        </li>
                                        <li>
                                            <a href="" title="KTM Duke 200 Standard" class="jcarousel-card">
                                                <div class="model-jcarousel-image-preview">
                                                    <span class="card-image-block">
                                                        <img class="lazy" data-original="https://imgd.aeplcdn.com//310x174//bw/used/S42584/42584_20160613043659076.jpg" alt="KTM Duke 200 Standard" border="0">
                                                    </span>
                                                </div>
                                                <div class="card-desc-block">
                                                    <h3 class="bikeTitle">KTM Duke 200 Standard</h3>
                                                    <div class="grid-6 alpha"> 
                                                        <span class="bwsprite model-date-icon"></span>
                                                        <span class="model-details-label">2011 model</span>
                                                    </div>
                                                
                                                    <div class="grid-6 alpha omega">
                                                        <span class="bwsprite kms-driven-icon"></span>
                                                        <span class="model-details-label">18,000 kms</span>
                                                    </div>
                                                
                                                    <div class="grid-6 alpha">
                                                        <span class="bwsprite author-grey-sm-icon"></span>
                                                        <span class="model-details-label">1st Owner</span>
                                                    </div>
                                                
                                                    <div class="grid-6 alpha omega">
                                                        <span class="bwsprite model-loc-icon"></span>
                                                        <span class="model-details-label">Kayamkulam</span>
                                                    </div>                                                
                                                    <div class="clear"></div>

                                                    <span class="bwsprite inr-lg"></span>&nbsp;<span class="font18 text-default text-bold">1,45,000</span>
                                                </div>
                                            </a>
                                            <div class="margin-left20 margin-bottom20">
                                                <a href="javascript:void(0)" class="btn btn-sm btn-grey font14" rel="nofollow">Get seller details</a>
                                            </div>
                                        </li>
                                    </ul>
                                </div>
                                <span class="jcarousel-control-left"><a href="#" class="bwsprite jcarousel-control-prev inactive" rel="nofollow"></a></span>
                                <span class="jcarousel-control-right"><a href="#" class="bwsprite jcarousel-control-next" rel="nofollow"></a></span>
                            </div>
                            <div class="padding-left20 margin-top15 padding-bottom20">
                                <a href="javascript:void(0)" class="" title="" rel="nofollow">View all used bikes in Mumbai <span class="bwsprite blue-right-arrow-icon"></span></a>
                            </div>
                        </div>

                        <div id="overallMakeDetailsFooter"></div>
                    </div>
                </div>
                <div class="clear"></div>
            </div>
        </section>

        <!-- gallery -->
        <section>
            <div class="blackOut-window-model"></div>
            <div class="bike-gallery-popup" id="bike-gallery-popup">
                <div class="modelgallery-close-btn bwsprite cross-lg-white cur-pointer"></div>
                <div class="bike-gallery-heading">
                    <p class="font18 text-bold margin-left30 text-white margin-bottom20">Royal Enfield Classic Desert Storm</p>

                    <div class="connected-carousels">
                        <div class="stage">
                            <div class="carousel carousel-stage">
                                <ul>
                                    <li>
                                        <div class="stage-slide">
                                            <div class="stage-image-placeholder">
                                                <img class="lazy" data-original="https://imgd.aeplcdn.com/642x361//staging/bw/used/S42661/42661_20160721050709814.jpg" alt="" src="https://imgd.aeplcdn.com/0x0/bw/static/sprites/d/loader.gif">
                                            </div>
                                        </div>
                                    </li>
                                    <li>
                                        <div class="stage-slide">
                                            <div class="stage-image-placeholder">
                                                <img class="lazy" data-original="https://imgd.aeplcdn.com/642x361//staging/bw/used/S42670/42670_20160722042837543.jpg" alt="" src="https://imgd.aeplcdn.com/0x0/bw/static/sprites/d/loader.gif">
                                            </div>
                                        </div>
                                    </li>
                                    <li>
                                        <div class="stage-slide">
                                            <div class="stage-image-placeholder">
                                                <img class="lazy" data-original="https://imgd.aeplcdn.com//642x361//bw/used/S42602/42602_20160613114137617.jpg" alt="" src="https://imgd.aeplcdn.com/0x0/bw/static/sprites/d/loader.gif">
                                            </div>
                                        </div>
                                    </li>
                                    <li>
                                        <div class="stage-slide">
                                            <div class="stage-image-placeholder">
                                                <img class="lazy" data-original="https://imgd.aeplcdn.com//642x361//bw/used/S42598/42598_20160613081003622.jpg" alt="" src="https://imgd.aeplcdn.com/0x0/bw/static/sprites/d/loader.gif">
                                            </div>
                                        </div>
                                    </li>
                                </ul>
                            </div>
                            <div class="bike-gallery-details">
                                <span class="rightfloat bike-gallery-count"></span>
                            </div>
                            <a href="#" class="prev photos-prev-stage bwsprite" rel="nofollow"></a>
                            <a href="#" class="next photos-next-stage bwsprite" rel="nofollow"></a>
                        </div>

                        <div class="navigation">
                            <a href="#" class="prev photos-prev-navigation bwsprite" rel="nofollow"></a>
                            <a href="#" class="next photos-next-navigation bwsprite" rel="nofollow"></a>
                            <div class="carousel carousel-navigation">
                                <ul>
                                    <li>
                                        <div class="navigation-slide">
                                            <div class="navigation-image-placeholder">
                                                <img class="lazy" data-original="https://imgd.aeplcdn.com/144x81//staging/bw/used/S42661/42661_20160721050709814.jpg" alt="" src="https://imgd.aeplcdn.com/0x0/bw/static/sprites/d/loader.gif">
                                            </div>
                                        </div>
                                    </li>
                                    <li>
                                        <div class="navigation-slide">
                                            <div class="navigation-image-placeholder">
                                                <img class="lazy" data-original="https://imgd.aeplcdn.com/144x81//staging/bw/used/S42670/42670_20160722042837543.jpg" alt="" src="https://imgd.aeplcdn.com/0x0/bw/static/sprites/d/loader.gif">
                                            </div>
                                        </div>
                                    </li>
                                    <li>
                                        <div class="navigation-slide">
                                            <div class="navigation-image-placeholder">
                                                <img class="lazy" data-original="https://imgd.aeplcdn.com//144x81//bw/used/S42602/42602_20160613114137617.jpg" alt="" src="https://imgd.aeplcdn.com/0x0/bw/static/sprites/d/loader.gif">
                                            </div>
                                        </div>
                                    </li>
                                    <li>
                                        <div class="navigation-slide">
                                            <div class="navigation-image-placeholder">
                                                <img class="lazy" data-original="https://imgd.aeplcdn.com//144x81//bw/used/S42598/42598_20160613081003622.jpg" alt="" src="https://imgd.aeplcdn.com/0x0/bw/static/sprites/d/loader.gif">
                                            </div>
                                        </div>
                                    </li>
                                </ul>
                            </div>
                        </div>
                    </div>

                </div>
            </div>
        </section>
        
        <script type="text/javascript" src="<%= staticUrl  %>/src/frameworks.js?<%=staticFileVersion %>"></script>

        <!-- #include file="/includes/footerBW.aspx" -->
        <link href="<%= staticUrl %>/css/bw-common-btf.css?<%=staticFileVersion %>" rel="stylesheet" type="text/css" />
        <script type="text/javascript" src="<%= staticUrl  %>/src/common.min.js?<%= staticFileVersion %>"></script>
        <script type="text/javascript" src="<%= staticUrl  %>/src/used-details.js?<%= staticFileVersion %>"></script>
        <script type="text/javascript" >
            var gaObj = { 'id': '<%= (int)Bikewale.Entities.Pages.GAPages.Used_Bike_Details%>', 'name': '<%= Bikewale.Entities.Pages.GAPages.Used_Bike_Details%>' };
            </script>
        <link href='https://fonts.googleapis.com/css?family=Open+Sans:400,700' rel='stylesheet' type='text/css' />
             
        <!--[if lt IE 9]>
            <script src="/src/html5.js"></script>
        <![endif]-->
    </form>
</body>
</html>
