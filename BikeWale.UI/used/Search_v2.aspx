<%@ Page Language="C#" AutoEventWireup="false" CodeBehind="Search_v2.aspx.cs" Inherits="Bikewale.Used.Search_v2" %>

<!DOCTYPE html>
<html>
<head>
    <%
        isHeaderFix = false;
    %>

    <title>Used search</title>
    <!-- #include file="/includes/headscript_desktop_min.aspx" -->

    <link href="<%= staticUrl != "" ? "http://st2.aeplcdn.com" + staticUrl : "" %>/css/used-search.css?<%= staticFileVersion %>" rel="stylesheet" type="text/css" />

    <script type="text/javascript" src="<%= staticUrl != "" ? "http://st1.aeplcdn.com" + staticUrl : "" %>/src/frameworks.js?<%=staticFileVersion %>"></script>
    <script type="text/javascript">
        <!-- #include file="\includes\gacode_desktop.aspx" -->
    </script>
</head>
<body class="bg-light-grey">
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
                            <li><span class="bwsprite fa-angle-right margin-right10"></span>
                              <span>Used Bikes</span>
                            </li>
                        </ul>
                        <div class="clear"></div>
                    </div>
                </div>
                <div class="clear"></div>
            </div>
        </section>

        <section class="container margin-bottom20">
            <div class="grid-12">
                <div class="content-box-shadow">
                    <div class="content-box-shadow padding-14-20">
                        <div class="grid-8 alpha">
                            <h1 class="font24 text-x-black">Used bike search</h1>
                        </div>
                        <div class="grid-4 omega">

                        </div>
                        <div class="clear"></div>
                    </div>
                    <div id="search-listing-content">
                        <div class="grid-4 alpha font14 position-rel">
                            <div id="filter-column" class="border-solid-right">
                                <div id="filters-head">
                                    <p class="font18 text-bold text-x-black leftfloat">Filters</p>
                                    <p id="reset-filters" class="btn btn-white font14 rightfloat">Reset</p>
                                    <div class="clear"></div>
                                </div>

                                <div class="filter-block">
                                    <p class="filter-label">City</p>
                                    <div class="clear"></div>
                                </div>

                                <div class="filter-block">
                                    <p class="filter-label">Bike</p>
                                    <p id="clear-all-bike" class="font12 rightfloat">Clear all</p>
                                    <div class="clear"></div>
                                </div>

                                <div class="filter-block">
                                    <p class="filter-label">Budget</p>
                                    <p id="budget-amount" class="font14 text-bold rightfloat">Rs. 40,000 - Rs. 20,00,000</p>
                                    <div class="clear"></div>
                                </div>

                                <div class="filter-block">
                                    <p class="filter-label">Kms ridden</p>
                                    <p id="kms-amount" class="font14 text-bold rightfloat">0 - 50,000 kms</p>
                                    <div class="clear"></div>
                                </div>

                                <div class="filter-block">
                                    <p class="filter-label">Bike age</p>
                                    <p id="bike-age-amount" class="font14 text-bold rightfloat">0 - 5 years</p>
                                    <div class="clear"></div>
                                </div>

                                <div class="filter-block">
                                    <p class="filter-label margin-bottom10">Previous owners</p>
                                    <div class="clear"></div>
                                    <ul id="previous-owners-list">
                                        <li>
                                            <span>1</span>
                                        </li>
                                        <li>
                                            <span>2</span>
                                        </li>
                                        <li>
                                            <span>3</span>
                                        </li>
                                        <li>
                                            <span>4</span>
                                        </li>
                                        <li>
                                            <span class="prev-owner-last-item">4 +</span>
                                        </li>
                                    </ul>
                                </div>

                                <div class="filter-block">
                                    <p class="filter-label margin-bottom10">Seller type</p>
                                    <div class="clear"></div>
                                    <div class="filter-type-seller unchecked padding-left25">Individual</div>
                                    <div class="filter-type-seller unchecked padding-left25">Dealer</div>
                                </div>

                                <div id="filters-footer"></div>
                            </div>
                        </div>
                        <div class="grid-8 padding-right20">
                            <div class="margin-top15 font12 padding-bottom5 border-solid-bottom">
                                <ul id="selected-filters">
                                    <li>
                                        Honda Cb Shine
                                    </li>
                                    <li>
                                        Honda Cb Shine
                                    </li>
                                    <li>
                                        Honda Cb Shine
                                    </li>
                                    <li>
                                        Honda Cb Shine
                                    </li>
                                    <li>
                                        Honda Cb Shine
                                    </li>
                                    <li>
                                        Honda Cb Shine
                                    </li>
                                    <li>
                                        Honda Cb Shine
                                    </li>
                                    <li>
                                        Honda Cb Shine
                                    </li>
                                </ul>
                                <div class="clear"></div>
                            </div>
                            <div class="padding-top15 padding-bottom15 text-light-grey font14 border-solid-bottom">
                                <p>Showing <span class="text-default text-bold">1-20</span> of <span class="text-default text-bold">200</span> bikes</p>
                            </div>
                            <ul id="used-bikes-list">
                                <li>
                                    <div class="model-thumbnail-image">
                                        <a href="" title="Honda CB Unicorn GP E">
                                            <img class="lazy" data-original="http://imgd1.aeplcdn.com//310x174//bw/used/S42378/42378_20160608023658807.jpeg" alt="Honda CB Unicorn GP E" src="" />
                                            <div class="model-media-details">
                                                <div class="model-media-item">
                                                    <span class="bwsprite gallery-photo-icon"></span>
                                                    <span class="model-media-count">55</span>
                                                </div>
                                            </div>
                                        </a>
                                    </div>
                                    <div class="model-details-content font14">
                                        <h2 class="margin-bottom10"><a href="" class="text-truncate text-black" title="Honda CB Unicorn GP E">Honda CB Unicorn GP E</a></h2>
                                        <div class="grid-6 alpha">
                                            <span class="bwsprite model-date-icon"></span>
                                            <span class="model-details-label">2013 model</span>
                                        </div>
                                        <div class="grid-6 omega">
                                            <span class="bwsprite kms-driven-icon"></span>
                                            <span class="model-details-label">1,45,000 kms</span>
                                        </div>
                                        <div class="grid-6 alpha">
                                            <span class="bwsprite author-grey-sm-icon"></span>
                                            <span class="model-details-label">2nd owner</span>
                                        </div>
                                        <div class="grid-6 omega">
                                            <span class="bwsprite model-loc-icon"></span>
                                            <span class="model-details-label">Mumbai</span>
                                        </div>
                                        <div class="clear"></div>
                                        <p class="margin-bottom15"><span class="bwsprite inr-md-lg"></span>&nbsp;<span class="font22 text-bold">1,22,000</span></p>
                                        <a href="javascript:void(0)" class="btn btn-orange seller-details-btn" rel="nofollow">Get seller details</a>
                                    </div>
                                    <div class="clear"></div>
                                </li>
                                <li>
                                    <div class="model-thumbnail-image">
                                        <a href="" title="Honda Dream Yuga Electric Start/Alloy">
                                            <img class="lazy" data-original="http://imgd1.aeplcdn.com//310x174//bw/used/S42393/42393_20160608065915421.jpg" alt="Honda Dream Yuga Electric Start/Alloy" src="" />
                                            <div class="model-media-details">
                                                <div class="model-media-item">
                                                    <span class="bwsprite gallery-photo-icon"></span>
                                                    <span class="model-media-count">4</span>
                                                </div>
                                            </div>
                                        </a>
                                    </div>
                                    <div class="model-details-content font14">
                                        <h2 class="margin-bottom10"><a href="" class="text-truncate text-black" title="Honda Dream Yuga Electric Start/Alloy">Honda Dream Yuga Electric Start/Alloy</a></h2>
                                        <div class="grid-6 alpha">
                                            <span class="bwsprite model-date-icon"></span>
                                            <span class="model-details-label">2013 model</span>
                                        </div>
                                        <div class="grid-6 omega">
                                            <span class="bwsprite kms-driven-icon"></span>
                                            <span class="model-details-label">1,45,000 kms</span>
                                        </div>
                                        <div class="grid-6 alpha">
                                            <span class="bwsprite author-grey-sm-icon"></span>
                                            <span class="model-details-label">2nd owner</span>
                                        </div>
                                        <div class="grid-6 omega">
                                            <span class="bwsprite model-loc-icon"></span>
                                            <span class="model-details-label">Mumbai</span>
                                        </div>
                                        <div class="clear"></div>
                                        <p class="margin-bottom15"><span class="bwsprite inr-md-lg"></span>&nbsp;<span class="font22 text-bold">1,22,000</span></p>
                                        <a href="javascript:void(0)" class="btn btn-orange seller-details-btn" rel="nofollow">Get seller details</a>
                                    </div>
                                    <div class="clear"></div>
                                </li>
                                <li>
                                    <div class="model-thumbnail-image">
                                        <a href="" title="Honda CB Unicorn GP E">
                                            <img class="lazy" data-original="http://imgd1.aeplcdn.com//310x174//bw/used/S42378/42378_20160608023658807.jpeg" alt="Honda CB Unicorn GP E" src="" />
                                            <div class="model-media-details">
                                                <div class="model-media-item">
                                                    <span class="bwsprite gallery-photo-icon"></span>
                                                    <span class="model-media-count">55</span>
                                                </div>
                                            </div>
                                        </a>
                                    </div>
                                    <div class="model-details-content font14">
                                        <h2 class="margin-bottom10"><a href="" class="text-truncate text-black" title="Harley Davidson Softail Classic Heritage">Harley Davidson Softail Classic Heritage</a></h2>
                                        <div class="grid-6 alpha">
                                            <span class="bwsprite model-date-icon"></span>
                                            <span class="model-details-label">2013 model</span>
                                        </div>
                                        <div class="grid-6 omega">
                                            <span class="bwsprite kms-driven-icon"></span>
                                            <span class="model-details-label">1,45,000 kms</span>
                                        </div>
                                        <div class="grid-6 alpha">
                                            <span class="bwsprite author-grey-sm-icon"></span>
                                            <span class="model-details-label">2nd owner</span>
                                        </div>
                                        <div class="grid-6 omega">
                                            <span class="bwsprite model-loc-icon"></span>
                                            <span class="model-details-label">Mumbai</span>
                                        </div>
                                        <div class="clear"></div>
                                        <p class="margin-bottom15"><span class="bwsprite inr-md-lg"></span>&nbsp;<span class="font22 text-bold">1,22,000</span></p>
                                        <a href="javascript:void(0)" class="btn btn-orange seller-details-btn" rel="nofollow">Get seller details</a>
                                    </div>
                                    <div class="clear"></div>
                                </li>
                                <li>
                                    <div class="model-thumbnail-image">
                                        <a href="" title="Honda CB Unicorn GP E">
                                            <img class="lazy" data-original="http://imgd1.aeplcdn.com//310x174//bw/used/S42378/42378_20160608023658807.jpeg" alt="Honda CB Unicorn GP E" src="" />
                                            <div class="model-media-details">
                                                <div class="model-media-item">
                                                    <span class="bwsprite gallery-photo-icon"></span>
                                                    <span class="model-media-count">55</span>
                                                </div>
                                            </div>
                                        </a>
                                    </div>
                                    <div class="model-details-content font14">
                                        <h2 class="margin-bottom10"><a href="" class="text-truncate text-black" title="Honda CB Unicorn GP E">Honda CB Unicorn GP E</a></h2>
                                        <div class="grid-6 alpha">
                                            <span class="bwsprite model-date-icon"></span>
                                            <span class="model-details-label">2013 model</span>
                                        </div>
                                        <div class="grid-6 omega">
                                            <span class="bwsprite kms-driven-icon"></span>
                                            <span class="model-details-label">1,45,000 kms</span>
                                        </div>
                                        <div class="grid-6 alpha">
                                            <span class="bwsprite author-grey-sm-icon"></span>
                                            <span class="model-details-label">2nd owner</span>
                                        </div>
                                        <div class="grid-6 omega">
                                            <span class="bwsprite model-loc-icon"></span>
                                            <span class="model-details-label">Mumbai</span>
                                        </div>
                                        <div class="clear"></div>
                                        <p class="margin-bottom15"><span class="bwsprite inr-md-lg"></span>&nbsp;<span class="font22 text-bold">1,22,000</span></p>
                                        <a href="javascript:void(0)" class="btn btn-orange seller-details-btn" rel="nofollow">Get seller details</a>
                                    </div>
                                    <div class="clear"></div>
                                </li>
                                <li>
                                    <div class="model-thumbnail-image">
                                        <a href="" title="Honda Dream Yuga Electric Start/Alloy">
                                            <img class="lazy" data-original="http://imgd1.aeplcdn.com//310x174//bw/used/S42393/42393_20160608065915421.jpg" alt="Honda Dream Yuga Electric Start/Alloy" src="" />
                                            <div class="model-media-details">
                                                <div class="model-media-item">
                                                    <span class="bwsprite gallery-photo-icon"></span>
                                                    <span class="model-media-count">4</span>
                                                </div>
                                            </div>
                                        </a>
                                    </div>
                                    <div class="model-details-content font14">
                                        <h2 class="margin-bottom10"><a href="" class="text-truncate text-black" title="Honda Dream Yuga Electric Start/Alloy">Honda Dream Yuga Electric Start/Alloy</a></h2>
                                        <div class="grid-6 alpha">
                                            <span class="bwsprite model-date-icon"></span>
                                            <span class="model-details-label">2013 model</span>
                                        </div>
                                        <div class="grid-6 omega">
                                            <span class="bwsprite kms-driven-icon"></span>
                                            <span class="model-details-label">1,45,000 kms</span>
                                        </div>
                                        <div class="grid-6 alpha">
                                            <span class="bwsprite author-grey-sm-icon"></span>
                                            <span class="model-details-label">2nd owner</span>
                                        </div>
                                        <div class="grid-6 omega">
                                            <span class="bwsprite model-loc-icon"></span>
                                            <span class="model-details-label">Mumbai</span>
                                        </div>
                                        <div class="clear"></div>
                                        <p class="margin-bottom15"><span class="bwsprite inr-md-lg"></span>&nbsp;<span class="font22 text-bold">1,22,000</span></p>
                                        <a href="javascript:void(0)" class="btn btn-orange seller-details-btn" rel="nofollow">Get seller details</a>
                                    </div>
                                    <div class="clear"></div>
                                </li>
                                <li>
                                    <div class="model-thumbnail-image">
                                        <a href="" title="Honda CB Unicorn GP E">
                                            <img class="lazy" data-original="http://imgd1.aeplcdn.com//310x174//bw/used/S42378/42378_20160608023658807.jpeg" alt="Honda CB Unicorn GP E" src="" />
                                            <div class="model-media-details">
                                                <div class="model-media-item">
                                                    <span class="bwsprite gallery-photo-icon"></span>
                                                    <span class="model-media-count">55</span>
                                                </div>
                                            </div>
                                        </a>
                                    </div>
                                    <div class="model-details-content font14">
                                        <h2 class="margin-bottom10"><a href="" class="text-truncate text-black" title="Harley Davidson Softail Classic Heritage">Harley Davidson Softail Classic Heritage</a></h2>
                                        <div class="grid-6 alpha">
                                            <span class="bwsprite model-date-icon"></span>
                                            <span class="model-details-label">2013 model</span>
                                        </div>
                                        <div class="grid-6 omega">
                                            <span class="bwsprite kms-driven-icon"></span>
                                            <span class="model-details-label">1,45,000 kms</span>
                                        </div>
                                        <div class="grid-6 alpha">
                                            <span class="bwsprite author-grey-sm-icon"></span>
                                            <span class="model-details-label">2nd owner</span>
                                        </div>
                                        <div class="grid-6 omega">
                                            <span class="bwsprite model-loc-icon"></span>
                                            <span class="model-details-label">Mumbai</span>
                                        </div>
                                        <div class="clear"></div>
                                        <p class="margin-bottom15"><span class="bwsprite inr-md-lg"></span>&nbsp;<span class="font22 text-bold">1,22,000</span></p>
                                        <a href="javascript:void(0)" class="btn btn-orange seller-details-btn" rel="nofollow">Get seller details</a>
                                    </div>
                                    <div class="clear"></div>
                                </li>
                            </ul>
                            
                            <div id="search-listing-footer" class="font14">
                                <div class="grid-5 alpha omega text-light-grey">
                                    <p>Showing <span class="text-default text-bold">1-20</span> of <span class="text-default text-bold">200</span> bikes</p>
                                </div>
                                <div id="pagination-list-content" class="grid-7 alpha omega position-rel">
                                    <ul id="pagination-list">
                                        <li>
                                            <a href="">1</a>
                                        </li>
                                        <li>
                                            <a href="">2</a>
                                        </li>
                                        <li class="active">
                                            <a href="">3</a>
                                        </li>
                                        <li>
                                            <a href="">4</a>
                                        </li>
                                        <li>
                                            <a href="">5</a>
                                        </li>
                                    </ul>
                                    <a href="" class="pagination-control-prev inactive">
                                        <span class="bwsprite prev-page-icon"></span>
                                    </a>
                                    <a href="" class="pagination-control-next">
                                        <span class="bwsprite next-page-icon"></span>
                                    </a>
                                </div>
                                <div class="clear"></div>
                            </div>
                        </div>
                        <div class="clear"></div>
                    </div>
                </div>
            </div>
            <div class="clear"></div>
        </section>


        <!-- #include file="/includes/footerBW.aspx" -->
        <!--[if lt IE 9]>
            <script src="/src/html5.js"></script>
        <![endif]-->
        <link href="<%= staticUrl != "" ? "http://st2.aeplcdn.com" + staticUrl : "" %>/css/bw-common-btf.css?<%=staticFileVersion %>" rel="stylesheet" type="text/css" />
        <script type="text/javascript" src="<%= staticUrl != string.Empty ? "http://st2.aeplcdn.com" + staticUrl : string.Empty %>/src/common.min.js?<%= staticFileVersion %>"></script>
        <script type="text/javascript" src="<%= staticUrl != string.Empty ? "http://st2.aeplcdn.com" + staticUrl : string.Empty %>/src/used-search.js?<%= staticFileVersion %>"></script>
        <link href='https://fonts.googleapis.com/css?family=Open+Sans:400,700' rel='stylesheet' type='text/css' />

    </form>
</body>
</html>
