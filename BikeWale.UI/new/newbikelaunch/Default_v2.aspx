﻿<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default_v2.aspx.cs" Inherits="Bikewale.New.NewLaunchBikes.Default_v2" %>

<!DOCTYPE html>
<html>
<head>
    <%
        isAd970x90Shown = false;
        isHeaderFix = false;
        isTransparentHeader = true;    
    %>
    <!-- #include file="/includes/headscript_desktop_min.aspx" -->
    <link rel="stylesheet" type="text/css" href="/css/new-launch/new-launch.css" />
    <script type="text/javascript">
        <!-- #include file="\includes\gacode_desktop.aspx" -->
    </script>
</head>
<body>
    <form id="form1" runat="server">
        <!-- #include file="/includes/headBW.aspx" -->

        <header class="new-launch-banner">
            <div id="new-launch-center-box" class="container">
                <div class="welcome-box">
                    <h1 class="font30 text-uppercase margin-bottom30">New bike launches</h1>
                    <h2 class="font20 text-unbold text-white margin-bottom50">Latest bikes in India</h2>
                </div>
            </div>
        </header>

        <section>
            <div class="container">
                <div class="grid-12">
                    <div class="content-box-shadow negative-50 padding-15-20 font14 text-light-grey collapsible-content">
                        <p class="main-content">India is one of the largest two-wheeler market in the world. The Indian market of two-wheelers has some noteworthy diversity. In order to thrive in this massive competition, manufacturers try to keep up with fast changing trends. Every year there are plenty of bike models that hit the market. BikeWale brings you an exhaustive list of newly launched bikes in India. Explore the list of latest bikes.</p>
                    </div>
                </div>
                <div class="clear"></div>
            </div>
        </section>

        <section>
            <div class="container section-bottom-margin">
                <div class="grid-12">
                    <h2 class="section-heading">New bike launches in India</h2>
                    <div class="content-box-shadow">
                        <div class="padding-right20 padding-left20">
                            <div class="padding-top15 padding-bottom15 border-solid-bottom">
                                <h3 class="font18 text-black grid-4 alpha">lorem ipsum</h3>
                                <div class="grid-8 omega text-right new-launch-filter">
                                    <p class="font14 text-light-grey inline-block margin-right15">Filter by:</p>
                                    <div class="select-box inline-block margin-right15">
                                        <p class="select-label">All brands</p>
                                        <select class="chosen-select" data-placeholder="Select brand">
                                            <option></option>
                                            <option value="1">Bajaj</option>
                                            <option value="11">Benelli</option>
                                            <option value="12">Ducati</option>
                                            <option value="13">Harley Davidson</option>
                                            <option value="14">Hero Electric</option>
                                            <option value="1">Bajaj</option>
                                            <option value="11">Benelli</option>
                                            <option value="12">Ducati</option>
                                            <option value="13">Harley Davidson</option>
                                            <option value="14">Hero Electric</option>
                                        </select>
                                    </div>

                                    <div class="select-box select-box-no-input inline-block right-align-box">
                                        <p class="select-label">All years</p>
                                        <select class="chosen-select" data-title="Select year">
                                            <option></option>
                                            <option value="1">2017</option>
                                            <option value="2">2016</option>
                                            <option value="3">2015</option>
                                            <option value="4">2014</option>
                                        </select>
                                    </div>
                                </div>
                                <div class="clear"></div>
                            </div>
                        </div>

                        <ul class="new-launches-list">
                            <li class="list-item">
                                <a href="" title="Honda CB Hornet 160R" class="block">
                                    <div class="model-jcarousel-image-preview">
                                        <img class="lazy" data-original="https://imgd2.aeplcdn.com//476x268//bw/models/honda-cb-hornet-160r.jpg?20151012195209" alt="Honda CB Hornet 160R" src="">
                                    </div>
                                    <div class="card-desc-block">
                                        <h3 class="bikeTitle">Honda CB Hornet 160R</h3>
                                        <ul class="key-specs-list margin-bottom10 font12">
                                            <li>209.85 cc</li><li>65 kmpl</li><li>10.85 bhp</li><li>209 kgs</li>
                                        </ul>
                                        <div class="grid-6 alpha padding-right20">
                                            <p class="key-size-13 text-truncate margin-bottom5">Ex-showroom, Mumbai</p>
                                            <span class="bwsprite inr-lg"></span> <span class="value-size-18">12,489</span>
                                        </div>
                                        <div class="grid-6 padding-left20 omega border-light-left">
                                            <p class="key-size-13 text-truncate margin-bottom5">Launched on</p>
                                            <span class="value-size-16">10 Mar 2017</span>
                                        </div>
                                        <div class="clear"></div>
                                    </div>
                                </a>
                                <div class="margin-bottom10 margin-left20">
                                    <a href="" class="btn btn-white btn-180-32">Check on-road price</a>
                                </div>
                            </li>
                            <li class="list-item">
                                <a href="" title="Discover 150F" class="block">
                                    <div class="model-jcarousel-image-preview">
                                        <img class="lazy" data-original="http://imgd1.aeplcdn.com//310x174//bw/models/bajaj-discover-150f.jpg?20152309104236" alt="Discover 150F" src="">
                                    </div>
                                    <div class="card-desc-block">
                                        <h3 class="bikeTitle">Discover 150F</h3>
                                        <ul class="key-specs-list margin-bottom10 font12">
                                            <li>209.85 cc</li><li>65 kmpl</li><li>10.85 bhp</li><li>209 kgs</li>
                                        </ul>
                                        <div class="grid-6 alpha padding-right20">
                                            <p class="key-size-13 text-truncate margin-bottom5">Ex-showroom, Mumbai</p>
                                            <span class="bwsprite inr-lg"></span> <span class="value-size-18">12,489</span>
                                        </div>
                                        <div class="grid-6 padding-left20 omega border-light-left">
                                            <p class="key-size-13 text-truncate margin-bottom5">Launched on</p>
                                            <span class="value-size-16">10 Mar 2017</span>
                                        </div>
                                        <div class="clear"></div>
                                    </div>
                                </a>
                                <div class="margin-bottom10 margin-left20">
                                    <a href="" class="btn btn-white btn-180-32">Check on-road price</a>
                                </div>
                            </li>
                            <li class="list-item">
                                <a href="" title="Bajaj V15" class="block">
                                    <div class="model-jcarousel-image-preview">
                                        <img class="lazy" data-original="http://imgd1.aeplcdn.com//310x174//bw/models/bajaj-v15.jpg?20160102144026" alt="Bajaj V15" src="">
                                    </div>
                                    <div class="card-desc-block">
                                        <h3 class="bikeTitle">Bajaj V15</h3>
                                        <ul class="key-specs-list margin-bottom10 font12">
                                            <li>209.85 cc</li><li>65 kmpl</li><li>10.85 bhp</li><li>209 kgs</li>
                                        </ul>
                                        <div class="grid-6 alpha padding-right20">
                                            <p class="key-size-13 text-truncate margin-bottom5">Ex-showroom, Mumbai</p>
                                            <span class="bwsprite inr-lg"></span> <span class="value-size-18">12,489</span>
                                        </div>
                                        <div class="grid-6 padding-left20 omega border-light-left">
                                            <p class="key-size-13 text-truncate margin-bottom5">Launched on</p>
                                            <span class="value-size-16">10 Mar 2017</span>
                                        </div>
                                        <div class="clear"></div>
                                    </div>
                                </a>
                                <div class="margin-bottom10 margin-left20">
                                    <a href="" class="btn btn-white btn-180-32">Check on-road price</a>
                                </div>
                            </li>
                            <li class="list-item">
                                <a href="" title="Bajaj Pulsar 180 DTS-i" class="block">
                                    <div class="model-jcarousel-image-preview">
                                        <img class="lazy" data-original="http://imgd1.aeplcdn.com//310x174//bw/models/bajaj-pulsar-180-dts--i-standard-272.jpg?20151209174449" alt="Bajaj Pulsar 180 DTS-i" src="">
                                    </div>
                                    <div class="card-desc-block">
                                        <h3 class="bikeTitle">Bajaj Pulsar 180 DTS-i</h3>
                                        <ul class="key-specs-list margin-bottom10 font12">
                                            <li>209.85 cc</li><li>65 kmpl</li><li>10.85 bhp</li><li>209 kgs</li>
                                        </ul>
                                        <div class="grid-6 alpha padding-right20">
                                            <p class="key-size-13 text-truncate margin-bottom5">Ex-showroom, Mumbai</p>
                                            <span class="bwsprite inr-lg"></span> <span class="value-size-18">12,489</span>
                                        </div>
                                        <div class="grid-6 padding-left20 omega border-light-left">
                                            <p class="key-size-13 text-truncate margin-bottom5">Launched on</p>
                                            <span class="value-size-16">10 Mar 2017</span>
                                        </div>
                                        <div class="clear"></div>
                                    </div>
                                </a>
                                <div class="margin-bottom10 margin-left20">
                                    <a href="" class="btn btn-white btn-180-32">Check on-road price</a>
                                </div>
                            </li>
                            <li class="list-item">
                                <a href="" title="Honda CB Hornet 160R" class="block">
                                    <div class="model-jcarousel-image-preview">
                                        <img class="lazy" data-original="https://imgd2.aeplcdn.com//476x268//bw/models/honda-cb-hornet-160r.jpg?20151012195209" alt="Honda CB Hornet 160R" src="">
                                    </div>
                                    <div class="card-desc-block">
                                        <h3 class="bikeTitle">Honda CB Hornet 160R</h3>
                                        <ul class="key-specs-list margin-bottom10 font12">
                                            <li>209.85 cc</li><li>65 kmpl</li><li>10.85 bhp</li><li>209 kgs</li>
                                        </ul>
                                        <div class="grid-6 alpha padding-right20">
                                            <p class="key-size-13 text-truncate margin-bottom5">Ex-showroom, Mumbai</p>
                                            <span class="bwsprite inr-lg"></span> <span class="value-size-18">12,489</span>
                                        </div>
                                        <div class="grid-6 padding-left20 omega border-light-left">
                                            <p class="key-size-13 text-truncate margin-bottom5">Launched on</p>
                                            <span class="value-size-16">10 Mar 2017</span>
                                        </div>
                                        <div class="clear"></div>
                                    </div>
                                </a>
                                <div class="margin-bottom10 margin-left20">
                                    <a href="" class="btn btn-white btn-180-32">Check on-road price</a>
                                </div>
                            </li>
                            <li class="list-item">
                                <a href="" title="Discover 150F" class="block">
                                    <div class="model-jcarousel-image-preview">
                                        <img class="lazy" data-original="http://imgd1.aeplcdn.com//310x174//bw/models/bajaj-discover-150f.jpg?20152309104236" alt="Discover 150F" src="">
                                    </div>
                                    <div class="card-desc-block">
                                        <h3 class="bikeTitle">Discover 150F</h3>
                                        <ul class="key-specs-list margin-bottom10 font12">
                                            <li>209.85 cc</li><li>65 kmpl</li><li>10.85 bhp</li><li>209 kgs</li>
                                        </ul>
                                        <div class="grid-6 alpha padding-right20">
                                            <p class="key-size-13 text-truncate margin-bottom5">Ex-showroom, Mumbai</p>
                                            <span class="bwsprite inr-lg"></span> <span class="value-size-18">12,489</span>
                                        </div>
                                        <div class="grid-6 padding-left20 omega border-light-left">
                                            <p class="key-size-13 text-truncate margin-bottom5">Launched on</p>
                                            <span class="value-size-16">10 Mar 2017</span>
                                        </div>
                                        <div class="clear"></div>
                                    </div>
                                </a>
                                <div class="margin-bottom10 margin-left20">
                                    <a href="" class="btn btn-white btn-180-32">Check on-road price</a>
                                </div>
                            </li>
                            <li class="list-item">
                                <a href="" title="Bajaj V15" class="block">
                                    <div class="model-jcarousel-image-preview">
                                        <img class="lazy" data-original="http://imgd1.aeplcdn.com//310x174//bw/models/bajaj-v15.jpg?20160102144026" alt="Bajaj V15" src="">
                                    </div>
                                    <div class="card-desc-block">
                                        <h3 class="bikeTitle">Bajaj V15</h3>
                                        <ul class="key-specs-list margin-bottom10 font12">
                                            <li>209.85 cc</li><li>65 kmpl</li><li>10.85 bhp</li><li>209 kgs</li>
                                        </ul>
                                        <div class="grid-6 alpha padding-right20">
                                            <p class="key-size-13 text-truncate margin-bottom5">Ex-showroom, Mumbai</p>
                                            <span class="bwsprite inr-lg"></span> <span class="value-size-18">12,489</span>
                                        </div>
                                        <div class="grid-6 padding-left20 omega border-light-left">
                                            <p class="key-size-13 text-truncate margin-bottom5">Launched on</p>
                                            <span class="value-size-16">10 Mar 2017</span>
                                        </div>
                                        <div class="clear"></div>
                                    </div>
                                </a>
                                <div class="margin-bottom10 margin-left20">
                                    <a href="" class="btn btn-white btn-180-32">Check on-road price</a>
                                </div>
                            </li>
                            <li class="list-item">
                                <a href="" title="Bajaj Pulsar 180 DTS-i" class="block">
                                    <div class="model-jcarousel-image-preview">
                                        <img class="lazy" data-original="http://imgd1.aeplcdn.com//310x174//bw/models/bajaj-pulsar-180-dts--i-standard-272.jpg?20151209174449" alt="Bajaj Pulsar 180 DTS-i" src="">
                                    </div>
                                    <div class="card-desc-block">
                                        <h3 class="bikeTitle">Bajaj Pulsar 180 DTS-i</h3>
                                        <ul class="key-specs-list margin-bottom10 font12">
                                            <li>209.85 cc</li><li>65 kmpl</li><li>10.85 bhp</li><li>209 kgs</li>
                                        </ul>
                                        <div class="grid-6 alpha padding-right20">
                                            <p class="key-size-13 text-truncate margin-bottom5">Ex-showroom, Mumbai</p>
                                            <span class="bwsprite inr-lg"></span> <span class="value-size-18">12,489</span>
                                        </div>
                                        <div class="grid-6 padding-left20 omega border-light-left">
                                            <p class="key-size-13 text-truncate margin-bottom5">Launched on</p>
                                            <span class="value-size-16">10 Mar 2017</span>
                                        </div>
                                        <div class="clear"></div>
                                    </div>
                                </a>
                                <div class="margin-bottom10 margin-left20">
                                    <a href="" class="btn btn-white btn-180-32">Check on-road price</a>
                                </div>
                            </li>
                        </ul>

                        <div class="padding-right20 padding-bottom10 padding-left20">
                            <div class="footer-pagination font14 border-solid-top padding-top10">
                                <div class="grid-5 alpha omega text-light-grey">
                                    <p>Showing <span class="text-default text-bold">1-20</span> of <span class="text-default text-bold">200</span> bikes</p>
                                </div>
                                <div id="pagination-list-content" class="pagination-list-content grid-7 alpha omega position-rel rightfloat">
                                    <ul id="pagination-list" class="pagination-list">
                                        <li class="active">1</li>
                                        <li>
                                            <a href="">2</a>
                                        </li>
                                        <li>
                                            <a href="">3</a>
                                        </li>
                                        <li>
                                            <a href="">4</a>
                                        </li>
                                        <li>
                                            <a href="">5</a>
                                        </li>
                                    </ul>
                                    <span class="pagination-control-prev inactive">
                                        <a href="" class="bwmsprite bwsprite prev-page-icon"></a>
                                    </span>
                                    <span class="pagination-control-next">
                                        <a href="" class="bwmsprite bwsprite next-page-icon"></a>
                                    </span>
                                </div>
                                <div class="clear"></div>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="clear"></div>
            </div>
        </section>

        <section>
            <div class="container section-bottom-margin">
                <h2 class="section-heading">Explore new launches by brands</h2>
                <div class="grid-12">
                    <div class="content-box-shadow padding-top20 padding-bottom25 text-center collapsible-brand-content">
                        <div id="brand-type-container" class="brand-type-container">
                            <ul>
                                <li>
                                    <a href="" title="">
                                        <span class="brand-type">
                                            <span class="lazy brandlogosprite brand-7"></span>
                                        </span>
                                        <span class="brand-type-title">Honda</span>
                                    </a>
                                </li>
                                <li>
                                    <a href="" title="">
                                        <span class="brand-type">
                                            <span class="lazy brandlogosprite brand-1"></span>
                                        </span>
                                        <span class="brand-type-title">Bajaj</span>
                                    </a>
                                </li>
                                <li>
                                    <a href="" title="">
                                        <span class="brand-type">
                                            <span class="lazy brandlogosprite brand-6"></span>
                                        </span>
                                        <span class="brand-type-title">Hero</span>
                                    </a>
                                </li>
                                <li>
                                    <a href="" title="">
                                        <span class="brand-type">
                                            <span class="lazy brandlogosprite brand-11"></span>
                                        </span>
                                        <span class="brand-type-title">Royal Enfield</span>
                                    </a>
                                </li>
                                <li>
                                    <a href="" title="">
                                        <span class="brand-type">
                                            <span class="lazy brandlogosprite brand-13"></span>
                                        </span>
                                        <span class="brand-type-title">Yamaha</span>
                                    </a>
                                </li>
                                <li>
                                    <a href="" title="">
                                        <span class="brand-type">
                                            <span class="lazy brandlogosprite brand-12"></span>
                                        </span>
                                        <span class="brand-type-title">Suzuki</span>
                                    </a>
                                </li>
                                <li>
                                    <a href="" title="">
                                        <span class="brand-type">
                                            <span class="lazy brandlogosprite brand-10"></span>
                                        </span>
                                        <span class="brand-type-title">Mahindra</span>
                                    </a>
                                </li>
                                <li>
                                    <a href="" title="">
                                        <span class="brand-type">
                                            <span class="lazy brandlogosprite brand-5"></span>
                                        </span>
                                        <span class="brand-type-title">Harley Davidson</span>
                                    </a>
                                </li>
                                <li>
                                    <a href="" title="">
                                        <span class="brand-type">
                                            <span class="lazy brandlogosprite brand-40"></span>
                                        </span>
                                        <span class="brand-type-title">Benelli</span>
                                    </a>
                                </li>
                                <li>
                                    <a href="" title="">
                                        <span class="brand-type">
                                            <span class="lazy brandlogosprite brand-4"></span>
                                        </span>
                                        <span class="brand-type-title">Ducati</span>
                                    </a>
                                </li>
                            </ul>
                            <ul class="brand-style-moreBtn padding-top25 brandTypeMore hide border-solid-top">
                                <li>
                                    <a href="" title="">
                                        <span class="brand-type">
                                            <span class="lazy brandlogosprite brand-7"></span>
                                        </span>
                                        <span class="brand-type-title">Honda</span>
                                    </a>
                                </li>
                                <li>
                                    <a href="" title="">
                                        <span class="brand-type">
                                            <span class="lazy brandlogosprite brand-1"></span>
                                        </span>
                                        <span class="brand-type-title">Bajaj</span>
                                    </a>
                                </li>
                                <li>
                                    <a href="" title="">
                                        <span class="brand-type">
                                            <span class="lazy brandlogosprite brand-6"></span>
                                        </span>
                                        <span class="brand-type-title">Hero</span>
                                    </a>
                                </li>
                                <li>
                                    <a href="" title="">
                                        <span class="brand-type">
                                            <span class="lazy brandlogosprite brand-11"></span>
                                        </span>
                                        <span class="brand-type-title">Royal Enfield</span>
                                    </a>
                                </li>
                                <li>
                                    <a href="" title="">
                                        <span class="brand-type">
                                            <span class="lazy brandlogosprite brand-13"></span>
                                        </span>
                                        <span class="brand-type-title">Yamaha</span>
                                    </a>
                                </li>
                                <li>
                                    <a href="" title="">
                                        <span class="brand-type">
                                            <span class="lazy brandlogosprite brand-12"></span>
                                        </span>
                                        <span class="brand-type-title">Suzuki</span>
                                    </a>
                                </li>
                                <li>
                                    <a href="" title="">
                                        <span class="brand-type">
                                            <span class="lazy brandlogosprite brand-10"></span>
                                        </span>
                                        <span class="brand-type-title">Mahindra</span>
                                    </a>
                                </li>
                                <li>
                                    <a href="" title="">
                                        <span class="brand-type">
                                            <span class="lazy brandlogosprite brand-5"></span>
                                        </span>
                                        <span class="brand-type-title">Harley Davidson</span>
                                    </a>
                                </li>
                                <li>
                                    <a href="" title="">
                                        <span class="brand-type">
                                            <span class="lazy brandlogosprite brand-40"></span>
                                        </span>
                                        <span class="brand-type-title">Benelli</span>
                                    </a>
                                </li>
                                <li>
                                    <a href="" title="">
                                        <span class="brand-type">
                                            <span class="lazy brandlogosprite brand-4"></span>
                                        </span>
                                        <span class="brand-type-title">Ducati</span>
                                    </a>
                                </li>
                            </ul>
                        </div>
                        <div>
                            <a href="javascript:void(0)" class="view-brandType view-more-btn font16" rel="nofollow">View more brands</a>
                        </div>
                    </div>
                </div>
                <div class="clear"></div>
            </div>
        </section>

        <section>
            <div class="container section-bottom-margin">
                <h2 class="section-heading">Explore year-wise bike launches</h2>
                <div class="grid-12">
                    <div class="content-box-shadow padding-top20 padding-bottom20">
                        <div class="jcarousel-wrapper inner-content-carousel year-type-carousel">
                            <div class="jcarousel">
                                <ul>
                                    <li>
                                        <a href="" title="Bikes launched in 2017" class="jcarousel-card">
                                            <div class="card-image-block">
                                                <span>2017</span>
                                            </div>
                                            <div class="card-desc-block">
                                                <h3 class="margin-bottom5">Bikes launched in 2017</h3>
                                                <p class="font12 text-light-grey text-truncate">Bajaj Dominar 400, Royal Enfield Classic 350, Honda CB Hornet 160R</p>
                                            </div>
                                        </a>
                                    </li>
                                    <li>
                                        <a href="" title="Bikes launched in 2016" class="jcarousel-card">
                                            <div class="card-image-block">
                                                <span>2016</span>
                                            </div>
                                            <div class="card-desc-block">
                                                <h3 class="margin-bottom5">Bikes launched in 2016</h3>
                                                <p class="font12 text-light-grey text-truncate">Bajaj Dominar 400, Royal Enfield Classic 350, Honda CB Hornet 160R</p>
                                            </div>
                                        </a>
                                    </li>
                                    <li>
                                        <a href="" title="Bikes launched in 2015" class="jcarousel-card">
                                            <div class="card-image-block">
                                                <span>2015</span>
                                            </div>
                                            <div class="card-desc-block">
                                                <h3 class="margin-bottom5">Bikes launched in 2015</h3>
                                                <p class="font12 text-light-grey text-truncate">Bajaj Dominar 400, Royal Enfield Classic 350, Honda CB Hornet 160R</p>
                                            </div>
                                        </a>
                                    </li>
                                    <li>
                                        <a href="" title="Bikes launched in 2014" class="jcarousel-card">
                                            <div class="card-image-block">
                                                <span>2014</span>
                                            </div>
                                            <div class="card-desc-block">
                                                <h3 class="margin-bottom5">Bikes launched in 2014</h3>
                                                <p class="font12 text-light-grey text-truncate">Bajaj Dominar 400, Royal Enfield Classic 350, Honda CB Hornet 160R</p>
                                            </div>
                                        </a>
                                    </li>
                                </ul>
                            </div>
                            <span class="jcarousel-control-left"><a href="#" class="bwsprite jcarousel-control-prev" rel="nofollow"></a></span>
                            <span class="jcarousel-control-right"><a href="#" class="bwsprite jcarousel-control-next" rel="nofollow"></a></span>
                        </div>
                    </div>
                </div>
                <div class="clear"></div>
            </div>
        </section>

        <section>
            <div class="container section-bottom-margin">
                <h2 class="section-heading">Upcoming bikes in India</h2>
                <div class="grid-12">
                    <div class="content-box-shadow padding-top20 padding-bottom20">
                        <div class="jcarousel-wrapper inner-content-carousel">
                            <div class="jcarousel">
                                <ul>
                                    <li>
                                        <a href="" title="TVS XL 100" class="jcarousel-card">
                                            <div class="model-jcarousel-image-preview">
                                                <span class="card-image-block">
                                                    <img class="lazy" data-original="http://imgd3.aeplcdn.com///310x174//bw/upcoming/tvs-xl100-704.jpg?20162803121410" alt="TVS XL 100" src="" border="0">
                                                </span>
                                            </div>
                                            <div class="card-desc-block">
                                                <h3 class="bikeTitle">
                                                    <span>TVS XL 100</span>
                                                </h3>
                                                <p class="font14 text-light-grey margin-bottom5">Expected launch</p>
                                                <p class="font16 text-default text-bold margin-bottom15">Jan 2016</p>
                                                <p class="font14 text-light-grey margin-bottom5">Expected price</p>
                                                <span class="bwsprite inr-lg"></span>
                                                <span class="font18 text-default text-bold">28,000 onwards</span>
                                            </div>
                                        </a>
                                    </li>
                                    <li>
                                        <a href="" title="Kawasaki Ninja ZX-14R [2016]" class="jcarousel-card">
                                            <div class="model-jcarousel-image-preview">
                                                <span class="card-image-block">
                                                    <img class="lazy" data-original="http://imgd2.aeplcdn.com///310x174//bw/upcoming/kawasaki-ninjazx-14r[2016]-703.jpg?20162402145840" alt="Kawasaki Ninja ZX-14R [2016]" src="" border="0">
                                                </span>
                                            </div>
                                            <div class="card-desc-block">
                                                <h3 class="bikeTitle">
                                                    <span>Kawasaki Ninja ZX-14R [2016]</span>
                                                </h3>
                                                <p class="font14 text-light-grey margin-bottom5">Expected launch</p>
                                                <p class="font16 text-default text-bold margin-bottom15">Feb 2016</p>
                                                <p class="font14 text-light-grey margin-bottom5">Expected price</p>
                                                <span class="bwsprite inr-lg"></span>
                                                <span class="font18 text-default text-bold">17,50,000 onwards</span>
                                            </div>
                                        </a>
                                    </li>
                                    <li>
                                        <a href="" title="Suzuki Hayate EP" class="jcarousel-card">
                                            <div class="model-jcarousel-image-preview">
                                                <span class="card-image-block">
                                                    <img class="lazy" data-original="http://imgd3.aeplcdn.com///310x174//bw/upcoming/suzuki-hayateep-705.jpg?20160104150238" alt="Suzuki Hayate EP" src="" border="0">
                                                </span>
                                            </div>
                                            <div class="card-desc-block">
                                                <h3 class="bikeTitle">
                                                    <span>Suzuki Hayate EP</span>
                                                </h3>
                                                <p class="font14 text-light-grey margin-bottom5">Expected launch</p>
                                                <p class="font16 text-default text-bold margin-bottom15">Apr 2016</p>
                                                <p class="font14 text-light-grey margin-bottom5">Expected price</p>
                                                <span class="bwsprite inr-lg"></span>
                                                <span class="font18 text-default text-bold">60,000 onwards</span>
                                            </div>
                                        </a>
                                    </li>
                                    <li>
                                        <a href="" title="TVS XL 100" class="jcarousel-card">
                                            <div class="model-jcarousel-image-preview">
                                                <span class="card-image-block">
                                                    <img class="lazy" data-original="http://imgd3.aeplcdn.com///310x174//bw/upcoming/tvs-xl100-704.jpg?20162803121410" alt="TVS XL 100" src="" border="0">
                                                </span>
                                            </div>
                                            <div class="card-desc-block">
                                                <h3 class="bikeTitle">
                                                    <span>TVS XL 100</span>
                                                </h3>
                                                <p class="font14 text-light-grey margin-bottom5">Expected launch</p>
                                                <p class="font16 text-default text-bold margin-bottom15">Jan 2016</p>
                                                <p class="font14 text-light-grey margin-bottom5">Expected price</p>
                                                <span class="bwsprite inr-lg"></span>
                                                <span class="font18 text-default text-bold">28,000 onwards</span>
                                            </div>
                                        </a>
                                    </li>
                                    <li>
                                        <a href="" title="Kawasaki Ninja ZX-14R [2016]" class="jcarousel-card">
                                            <div class="model-jcarousel-image-preview">
                                                <span class="card-image-block">
                                                    <img class="lazy" data-original="http://imgd2.aeplcdn.com///310x174//bw/upcoming/kawasaki-ninjazx-14r[2016]-703.jpg?20162402145840" alt="Kawasaki Ninja ZX-14R [2016]" src="" border="0">
                                                </span>
                                            </div>
                                            <div class="card-desc-block">
                                                <h3 class="bikeTitle">
                                                    <span>Kawasaki Ninja ZX-14R [2016]</span>
                                                </h3>
                                                <p class="font14 text-light-grey margin-bottom5">Expected launch</p>
                                                <p class="font16 text-default text-bold margin-bottom15">Feb 2016</p>
                                                <p class="font14 text-light-grey margin-bottom5">Expected price</p>
                                                <span class="bwsprite inr-lg"></span>
                                                <span class="font18 text-default text-bold">17,50,000 onwards</span>
                                            </div>
                                        </a>
                                    </li>
                                    <li>
                                        <a href="" title="Suzuki Hayate EP" class="jcarousel-card">
                                            <div class="model-jcarousel-image-preview">
                                                <span class="card-image-block">
                                                    <img class="lazy" data-original="http://imgd3.aeplcdn.com///310x174//bw/upcoming/suzuki-hayateep-705.jpg?20160104150238" alt="Suzuki Hayate EP" src="" border="0">
                                                </span>
                                            </div>
                                            <div class="card-desc-block">
                                                <h3 class="bikeTitle">
                                                    <span>Suzuki Hayate EP</span>
                                                </h3>
                                                <p class="font14 text-light-grey margin-bottom5">Expected launch</p>
                                                <p class="font16 text-default text-bold margin-bottom15">Apr 2016</p>
                                                <p class="font14 text-light-grey margin-bottom5">Expected price</p>
                                                <span class="bwsprite inr-lg"></span>
                                                <span class="font18 text-default text-bold">60,000 onwards</span>
                                            </div>
                                        </a>
                                    </li>
                                </ul>
                            </div>
                            <span class="jcarousel-control-left"><a href="#" class="bwsprite jcarousel-control-prev" rel="nofollow"></a></span>
                            <span class="jcarousel-control-right"><a href="#" class="bwsprite jcarousel-control-next" rel="nofollow"></a></span>
                        </div>
                    </div>
                </div>
                <div class="clear"></div>
            </div>
        </section>

        <script type="text/javascript" src="<%= staticUrl != "" ? "https://st1.aeplcdn.com" + staticUrl : "" %>/src/frameworks.js?<%=staticFileVersion %>"></script>
        <!-- #include file="/includes/footerBW.aspx" -->
        <link href="<%= staticUrl != "" ? "https://st2.aeplcdn.com" + staticUrl : "" %>/css/bw-common-btf.css?<%=staticFileVersion %>" rel="stylesheet" type="text/css" />
        
        <!-- #include file="/includes/footerscript.aspx" -->
        <script type="text/javascript" src="<%= staticUrl != "" ? "https://st1.aeplcdn.com" + staticUrl : "" %>/src/new-launch.js?<%=staticFileVersion %>"></script>
        <script type="text/javascript">
            $(window).on("scroll", function () {
                if ($(window).scrollTop() > 40)
                    $('#header').removeClass("header-landing").addClass("header-fixed");
                else
                    $('#header').removeClass("header-fixed").addClass("header-landing");
            });
        </script>
        <link href='https://fonts.googleapis.com/css?family=Open+Sans:400,600,700' rel='stylesheet' type='text/css' />
    </form>
</body>
</html>
