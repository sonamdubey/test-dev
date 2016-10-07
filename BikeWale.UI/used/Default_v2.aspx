<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default_v2.aspx.cs" Inherits="Bikewale.Used.Default_v2" %>

<!DOCTYPE html>

<html>
<head>
    <%
        isHeaderFix = false;
        isAd970x90Shown = false;
        isTransparentHeader = true; 
    %>

    <title>Used Bikes in India</title>

    <!-- #include file="/includes/headscript_desktop_min.aspx" -->

    <link type="text/css" href="/css/used/landing.css" rel="stylesheet" />

    <script type="text/javascript">
        <!-- #include file="\includes\gacode_desktop.aspx" -->
    </script>
</head>
<body class="bg-light-grey">
    <form id="form1" runat="server">
        <!-- #include file="/includes/headBW.aspx" -->
        <header class="used-landing-banner">
            <div id="used-landing-box" class="container">
                <div class="welcome-box">
                    <h1 class="font30 text-uppercase margin-bottom30">Used bikes</h1>
                    <h2 class="font20 text-unbold text-white">View wide range of used bikes</h2>
                </div>
            </div>
        </header>

        <section>
            <div class="container section-container">
                <div class="grid-12">
                    <div class="content-box-shadow negative-50 text-center padding-25-30">
                        <h2 class="section-header">Search used bikes</h2>
                        <div class="usedbikes-search-container">
                            <div id="search-form-city" class="form-control-box">
                                <select class="form-control chosen-select">
                                    <option value="0" >Select a city</option>
                                    <option>Ahmedabad</option>
                                    <option>Bangalore</option>
                                    <option>Chennai</option>
                                    <option>Hyderabad</option>
                                    <option>Kolkata</option>
                                    <option>Mumbai</option>
                                    <option>New Delhi</option>
                                    <option>Pune</option>
                                </select>
                            </div>
                            <div id="search-form-budget" class="form-control-box">
                                <div id="min-max-budget-box" class="form-selection-box">
                                    <span id="budget-default-label">Select budget</span>
                                    <span id="min-amount"></span>
                                    <span id="max-amount"></span>
                                    <span id="upDownArrow" class="fa fa-angle-down position-abt pos-top18 pos-right20"></span>
                                    <div class="clear"></div>
                                </div>
                                <div id="budget-list-box">
                                    <div id="user-budget-input" class="bg-light-grey text-light-grey">
                                        <div id="min-input-label" class="input-label-box border-solid-right">Min</div><div id="max-input-label" class="input-label-box">Max</div>
                                    </div>
                                    <ul id="min-budget-list" class="text-left"></ul>
                                    <ul id="max-budget-list" class="text-right"></ul>
                                </div>
                            </div>
                            <div class="clear"></div>
                            <input type="button" id="" class="btn btn-orange btn-lg search-bikes-btn margin-bottom20" value="Search" />
                            <div>
                                <a href="javascript:void(0)" id="profile-id-popup-target" class="font14 text-underline" rel="nofollow">Search by Profile ID</a>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="clear"></div>
            </div>
        </section>

        <section>
            <div class="container section-container">
                <div class="grid-12">
                    <h2 class="section-header">Best way to sell your bike</h2>
                    <div class="content-box-shadow text-center padding-top25 padding-bottom25 padding-right20 padding-left20 font14">
                        <div class="margin-bottom20">
                            <div class="grid-3">
                                <span class="used-sprite free-cost"></span>
                                <p class="text-bold margin-bottom5">Free of cost</p>
                                <p>You can upload your bike ad absolutely free</p>
                            </div>
                            <div class="grid-3">
                                <span class="used-sprite buyer"></span>
                                <p class="text-bold margin-bottom5">Genuine buyers </p>
                                <p>We let only verified buyers to get in touch with you</p>
                            </div>
                            <div class="grid-3">
                                <span class="used-sprite listing-time"></span>
                                <p class="text-bold margin-bottom5">Unlimited listing duration</p>
                                <p>Your bike listing will stay active until it is sold</p>
                            </div>
                            <div class="grid-3">
                                <span class="used-sprite contact-buyer"></span>
                                <p class="text-bold margin-bottom5">Get buyer details</p>
                                <p>We will send you the details of buyers thorugh SMS and mail</p>
                            </div>
                            <div class="clear"></div>
                        </div>
                        <a href="" id="sell-btn" class="btn btn-teal">Sell</a>
                    </div>
                </div>
                <div class="clear"></div>
            </div>
        </section>
        
        <section>
            <div class="container section-container">
                <div class="grid-12">
                    <h2 class="section-header">Search used bikes by brands</h2>
                    <div class="content-box-shadow padding-top20">
                        <div class="brand-type-container">
                            <ul class="text-center">
                                <li>
                                    <a href="">
                                        <span class="brand-type">
                                            <span class="brandlogosprite brand-7"></span>
                                        </span>
                                        <span class="brand-type-title">Honda</span>
                                    </a>
                                </li>                                    
                                <li>
                                    <a href="">
                                        <span class="brand-type">
                                            <span class="brandlogosprite brand-1"></span>
                                        </span>
                                        <span class="brand-type-title">Bajaj</span>
                                    </a>
                                </li>                                    
                                <li>
                                    <a href="">
                                        <span class="brand-type">
                                            <span class="brandlogosprite brand-6"></span>
                                        </span>
                                        <span class="brand-type-title">Hero</span>
                                    </a>
                                </li>                                    
                                <li>
                                    <a href="">
                                        <span class="brand-type">
                                            <span class="brandlogosprite brand-15"></span>
                                        </span>
                                        <span class="brand-type-title">TVS</span>
                                    </a>
                                </li>                                    
                                <li>
                                    <a href="">
                                        <span class="brand-type">
                                            <span class="brandlogosprite brand-11"></span>
                                        </span>
                                        <span class="brand-type-title">Royal Enfield</span>
                                    </a>
                                </li>                                    
                                <li>
                                    <a href="">
                                        <span class="brand-type">
                                            <span class="brandlogosprite brand-13"></span>
                                        </span>
                                        <span class="brand-type-title">Yamaha</span>
                                    </a>
                                </li>                                    
                                <li>
                                    <a href="">
                                        <span class="brand-type">
                                            <span class="brandlogosprite brand-12"></span>
                                        </span>
                                        <span class="brand-type-title">Suzuki</span>
                                    </a>
                                </li>                                    
                                <li>
                                    <a href="">
                                        <span class="brand-type">
                                            <span class="brandlogosprite brand-9"></span>
                                        </span>
                                        <span class="brand-type-title">KTM</span>
                                    </a>
                                </li>                                    
                                <li>
                                    <a href="">
                                        <span class="brand-type">
                                            <span class="brandlogosprite brand-10"></span>
                                        </span>
                                        <span class="brand-type-title">Mahindra</span>
                                    </a>
                                </li>                                    
                                <li>
                                    <a href="">
                                        <span class="brand-type">
                                            <span class="brandlogosprite brand-5"></span>
                                        </span>
                                        <span class="brand-type-title">Harley Davidson</span>
                                    </a>
                                </li>                                    
                            </ul>
                            <div class="brand-bottom-border margin-right20 margin-left20 border-solid-top hide"></div>
                            <ul class="brand-style-moreBtn padding-top25 brandTypeMore hide margin-left5">
                                <li>
                                    <a href="">
                                        <span class="brand-type">
                                            <span class="brandlogosprite brand-7"></span>
                                        </span>
                                        <span class="brand-type-title">Honda</span>
                                    </a>
                                </li>                                    
                                <li>
                                    <a href="">
                                        <span class="brand-type">
                                            <span class="brandlogosprite brand-1"></span>
                                        </span>
                                        <span class="brand-type-title">Bajaj</span>
                                    </a>
                                </li>                                    
                                <li>
                                    <a href="">
                                        <span class="brand-type">
                                            <span class="brandlogosprite brand-6"></span>
                                        </span>
                                        <span class="brand-type-title">Hero</span>
                                    </a>
                                </li>                                    
                                <li>
                                    <a href="">
                                        <span class="brand-type">
                                            <span class="brandlogosprite brand-15"></span>
                                        </span>
                                        <span class="brand-type-title">TVS</span>
                                    </a>
                                </li>                                    
                                <li>
                                    <a href="">
                                        <span class="brand-type">
                                            <span class="brandlogosprite brand-11"></span>
                                        </span>
                                        <span class="brand-type-title">Royal Enfield</span>
                                    </a>
                                </li>                                    
                                <li>
                                    <a href="">
                                        <span class="brand-type">
                                            <span class="brandlogosprite brand-13"></span>
                                        </span>
                                        <span class="brand-type-title">Yamaha</span>
                                    </a>
                                </li>                                    
                                <li>
                                    <a href="">
                                        <span class="brand-type">
                                            <span class="brandlogosprite brand-12"></span>
                                        </span>
                                        <span class="brand-type-title">Suzuki</span>
                                    </a>
                                </li>                                    
                                <li>
                                    <a href="">
                                        <span class="brand-type">
                                            <span class="brandlogosprite brand-9"></span>
                                        </span>
                                        <span class="brand-type-title">KTM</span>
                                    </a>
                                </li>                                    
                                <li>
                                    <a href="">
                                        <span class="brand-type">
                                            <span class="brandlogosprite brand-10"></span>
                                        </span>
                                        <span class="brand-type-title">Mahindra</span>
                                    </a>
                                </li>                                    
                                <li>
                                    <a href="">
                                        <span class="brand-type">
                                            <span class="brandlogosprite brand-5"></span>
                                        </span>
                                        <span class="brand-type-title">Harley Davidson</span>
                                    </a>
                                </li>
                            </ul>
                        </div>
                        <div class="view-brandType text-center padding-bottom25">
                            <a href="javascript:void(0)" id="view-brandType" class="view-more-btn font16" rel="nofollow">View <span>more</span> brands</a>
                        </div>
                    </div>
                </div>
                <div class="clear"></div>
            </div>
        </section>

        <section>
            <div class="container section-container">
                <div class="grid-12">
                    <h2 class="section-header">Search used bikes by cities</h2>
                    <div class="content-box-shadow">
                        <div class="text-center margin-top15 padding-bottom20">
                            <a href="" class="btn btn-inv-teal inv-teal-sm">View all cities<span class="bwsprite teal-next"></span></a>
                        </div>
                    </div>
                </div>
                <div class="clear"></div>
            </div>
        </section>

        <section>
            <div class="container section-container">
                <div class="grid-12">
                    <h2 class="section-header">Recently uploaded used bikes</h2>
                    <div id="recent-uploads" class="content-box-shadow padding-top20 padding-bottom20">
                        <div class="jcarousel-wrapper inner-content-carousel used-carousel">
                            <div class="jcarousel">
                                <ul>
                                    <li>
                                        <a href="" title="Royal Enfield Classic Desert Storm" class="jcarousel-card">
                                            <div class="model-jcarousel-image-preview">
                                                <div class="card-image-block position-rel">
                                                    <img class="lazy" data-original="http://imgd2.aeplcdn.com//642x361//bw/used/S42613/42613_20160614120104660.jpg" alt="Royal Enfield Classic Desert Storm" border="0">
                                                    <div class="model-media-details">
                                                        <div class="model-media-item">
                                                            <span class="bwsprite gallery-photo-icon"></span>
                                                            <span class="model-media-count">1</span>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="card-desc-block">
                                                <h3 class="bikeTitle">Royal Enfield Classic Desert Storm</h3>
                                                <div class="grid-6 alpha omega margin-bottom5"> 
                                                    <span class="bwsprite model-date-icon"></span>
                                                    <span class="model-details-label">2008 model</span>
                                                </div>
                                                
                                                <div class="grid-6 alpha omega margin-bottom5">
                                                    <span class="bwsprite kms-driven-icon"></span>
                                                    <span class="model-details-label">26,840 kms</span>
                                                </div>
                                                
                                                <div class="grid-6 alpha omega margin-bottom5">
                                                    <span class="bwsprite author-grey-sm-icon"></span>
                                                    <span class="model-details-label">1st Owner</span>
                                                </div>
                                                
                                                <div class="grid-6 alpha omega margin-bottom5">
                                                    <span class="bwsprite model-loc-icon"></span>
                                                    <span class="model-details-label">Bangalore</span>
                                                </div>                                                
                                                <div class="clear"></div>
                                                <div class="margin-top5">
                                                    <span class="bwsprite inr-lg"></span>
                                                    <span class="font18 text-default text-bold">15,00,000</span>
                                                </div>
                                            </div>
                                        </a>
                                    </li>

                                    <li>
                                        <a href="" title="Royal Enfield Classic Desert Storm" class="jcarousel-card">
                                            <div class="model-jcarousel-image-preview">
                                                <div class="card-image-block position-rel">
                                                    <img class="lazy" data-original="http://imgd2.aeplcdn.com//642x361//bw/used/S42598/42598_20160613081003622.jpg" alt="Royal Enfield Classic Desert Storm" border="0">
                                                    <div class="model-media-details">
                                                        <div class="model-media-item">
                                                            <span class="bwsprite gallery-photo-icon"></span>
                                                            <span class="model-media-count">1</span>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="card-desc-block">
                                                <h3 class="bikeTitle">Royal Enfield Classic Desert Storm</h3>
                                                <div class="grid-6 alpha omega margin-bottom5"> 
                                                    <span class="bwsprite model-date-icon"></span>
                                                    <span class="model-details-label">2008 model</span>
                                                </div>
                                                
                                                <div class="grid-6 alpha omega margin-bottom5">
                                                    <span class="bwsprite kms-driven-icon"></span>
                                                    <span class="model-details-label">26,840 kms</span>
                                                </div>
                                                
                                                <div class="grid-6 alpha omega margin-bottom5">
                                                    <span class="bwsprite model-loc-icon"></span>
                                                    <span class="model-details-label">Bangalore</span>
                                                </div>                                                
                                                <div class="clear"></div>
                                                <div class="margin-top5">
                                                    <span class="bwsprite inr-lg"></span>
                                                    <span class="font18 text-default text-bold">15,00,000</span>
                                                </div>
                                            </div>
                                        </a>
                                    </li>

                                    <li>
                                        <a href="" title="Royal Enfield Classic Desert Storm" class="jcarousel-card">
                                            <div class="model-jcarousel-image-preview">
                                                <div class="card-image-block position-rel">
                                                    <img class="lazy" data-original="http://imgd2.aeplcdn.com//642x361//bw/used/S42602/42602_20160613114137617.jpg" alt="Royal Enfield Classic Desert Storm" border="0">
                                                    <div class="model-media-details">
                                                        <div class="model-media-item">
                                                            <span class="bwsprite gallery-photo-icon"></span>
                                                            <span class="model-media-count">1</span>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="card-desc-block">
                                                <h3 class="bikeTitle">Royal Enfield Classic Desert Storm</h3>
                                                <div class="grid-6 alpha omega margin-bottom5"> 
                                                    <span class="bwsprite model-date-icon"></span>
                                                    <span class="model-details-label">2008 model</span>
                                                </div>
                                                
                                                <div class="grid-6 alpha omega margin-bottom5">
                                                    <span class="bwsprite kms-driven-icon"></span>
                                                    <span class="model-details-label">26,840 kms</span>
                                                </div>
                                                
                                                <div class="grid-6 alpha omega margin-bottom5">
                                                    <span class="bwsprite author-grey-sm-icon"></span>
                                                    <span class="model-details-label">1st Owner</span>
                                                </div>
                                                
                                                <div class="grid-6 alpha omega margin-bottom5">
                                                    <span class="bwsprite model-loc-icon"></span>
                                                    <span class="model-details-label">Bangalore</span>
                                                </div>                                                
                                                <div class="clear"></div>
                                                <div class="margin-top5">
                                                    <span class="bwsprite inr-lg"></span>
                                                    <span class="font18 text-default text-bold">15,00,000</span>
                                                </div>
                                            </div>
                                        </a>
                                    </li>

                                    <li>
                                        <a href="" title="Royal Enfield Classic Desert Storm" class="jcarousel-card">
                                            <div class="model-jcarousel-image-preview">
                                                <div class="card-image-block position-rel">
                                                    <img class="lazy" data-original="http://imgd8.aeplcdn.com/642x361//staging/bw/used/S42667/42667_20160722024912678.png" alt="Royal Enfield Classic Desert Storm" border="0">
                                                    <div class="model-media-details">
                                                        <div class="model-media-item">
                                                            <span class="bwsprite gallery-photo-icon"></span>
                                                            <span class="model-media-count">1</span>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="card-desc-block">
                                                <h3 class="bikeTitle">Royal Enfield Classic Desert Storm</h3>
                                                <div class="grid-6 alpha omega margin-bottom5"> 
                                                    <span class="bwsprite model-date-icon"></span>
                                                    <span class="model-details-label">2008 model</span>
                                                </div>
                                                
                                                <div class="grid-6 alpha omega margin-bottom5">
                                                    <span class="bwsprite author-grey-sm-icon"></span>
                                                    <span class="model-details-label">1st Owner</span>
                                                </div>
                                                
                                                <div class="grid-6 alpha omega margin-bottom5">
                                                    <span class="bwsprite model-loc-icon"></span>
                                                    <span class="model-details-label">Bangalore</span>
                                                </div>                                                
                                                <div class="clear"></div>
                                                <div class="margin-top5">
                                                    <span class="bwsprite inr-lg"></span>
                                                    <span class="font18 text-default text-bold">15,00,000</span>
                                                </div>
                                            </div>
                                        </a>
                                    </li>

                                    <li>
                                        <a href="" title="Royal Enfield Classic Desert Storm" class="jcarousel-card">
                                            <div class="model-jcarousel-image-preview">
                                                <div class="card-image-block position-rel">
                                                    <img class="lazy" data-original="http://imgd2.aeplcdn.com//642x361//bw/used/S42613/42613_20160614120104660.jpg" alt="Royal Enfield Classic Desert Storm" border="0">
                                                    <div class="model-media-details">
                                                        <div class="model-media-item">
                                                            <span class="bwsprite gallery-photo-icon"></span>
                                                            <span class="model-media-count">1</span>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="card-desc-block">
                                                <h3 class="bikeTitle">Royal Enfield Classic Desert Storm</h3>
                                                <div class="grid-6 alpha omega margin-bottom5"> 
                                                    <span class="bwsprite model-date-icon"></span>
                                                    <span class="model-details-label">2008 model</span>
                                                </div>
                                                
                                                <div class="grid-6 alpha omega margin-bottom5">
                                                    <span class="bwsprite kms-driven-icon"></span>
                                                    <span class="model-details-label">26,840 kms</span>
                                                </div>
                                                
                                                <div class="grid-6 alpha omega margin-bottom5">
                                                    <span class="bwsprite author-grey-sm-icon"></span>
                                                    <span class="model-details-label">1st Owner</span>
                                                </div>
                                                
                                                <div class="grid-6 alpha omega margin-bottom5">
                                                    <span class="bwsprite model-loc-icon"></span>
                                                    <span class="model-details-label">Bangalore</span>
                                                </div>                                                
                                                <div class="clear"></div>
                                                <div class="margin-top5">
                                                    <span class="bwsprite inr-lg"></span>
                                                    <span class="font18 text-default text-bold">15,00,000</span>
                                                </div>
                                            </div>
                                        </a>
                                    </li>
                                </ul>
                            </div>
                            <span class="jcarousel-control-left"><a href="#" class="bwsprite jcarousel-control-prev" rel="nofollow"></a></span>
                            <span class="jcarousel-control-right"><a href="#" class="bwsprite jcarousel-control-next" rel="nofollow"></a></span>
                        </div>

                        <div class="text-center margin-top15">
                            <a href="" class="btn btn-inv-teal inv-teal-sm">View all cities<span class="bwsprite teal-next"></span></a>
                        </div>
                    </div>
                </div>
                <div class="clear"></div>
            </div>
        </section>

        <!-- profile-id -->
        <div id="profile-id-popup" class="bw-popup text-center size-small">
            <div class="bwsprite cross-lg-lgt-grey close-btn position-abt pos-top10 pos-right10 cur-pointer"></div>
            <div class="icon-outer-container rounded-corner50">
                <div class="icon-inner-container rounded-corner50">
                    <span class="used-sprite profile-id-icon margin-top25"></span>
                </div>
            </div>
            <p class="font18 text-bold margin-top10 margin-bottom10">Search by Profile ID</p>
            <p class="font14 text-light-grey margin-bottom30">If you like a certain listing you can search it by its Profile ID. The unique Profile ID is mentioned in the Ad details.</p>
            <div class="input-box form-control-box margin-bottom15">
                <input type="text" id="listingProfileId">
                <label for="listingProfileId">Profile ID</label>
                <span class="boundary"></span>
                <span class="error-text"></span>
            </div>
            <a class="btn btn-orange btn-fixed-width" id="search-profile-id-btn">Search</a>
        </div>

        <script type="text/javascript" src="<%= staticUrl != "" ? "http://st1.aeplcdn.com" + staticUrl : "" %>/src/frameworks.js?<%=staticFileVersion %>"></script>

        <!-- #include file="/includes/footerBW.aspx" -->
        <link href="<%= staticUrl != "" ? "http://st2.aeplcdn.com" + staticUrl : "" %>/css/bw-common-btf.css?<%=staticFileVersion %>" rel="stylesheet" type="text/css" />
        <script type="text/javascript" src="<%= staticUrl != string.Empty ? "http://st2.aeplcdn.com" + staticUrl : string.Empty %>/src/common.min.js?<%= staticFileVersion %>"></script>
        <script type="text/javascript" src="<%= staticUrl != string.Empty ? "http://st2.aeplcdn.com" + staticUrl : string.Empty %>/src/used-landing.js?<%= staticFileVersion %>"></script>
        <link href='https://fonts.googleapis.com/css?family=Open+Sans:400,700' rel='stylesheet' type='text/css' />
        <!--[if lt IE 9]>
            <script src="/src/html5.js"></script>
        <![endif]-->
    </form>
</body>
</html>
