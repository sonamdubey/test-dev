<%@ Page Language="C#" Inherits="Bikewale.New.BrowseNewBikeDealerDetails_v2" AutoEventWireup="false" EnableViewState="false" %>

<!DOCTYPE html>
<html>
<head>
    <%
       
        keywords = String.Format("{0} dealers city, {0} showrooms {1}, {1} bike dealers, {0} dealers, {1} bike showrooms, bike dealers, bike showrooms, dealerships", makeName, cityName);
        description = String.Format("{0} bike dealers/showrooms in {1}. Find {0} bike dealer information for more than 200 cities. Dealer information includes full address, phone numbers, email, pin code etc", makeName, cityName);
        title = String.Format("{0} Dealers in {1} city | {0} New bike Showrooms in {1} - BikeWale", makeName, cityName);
        canonical = String.Format("http://www.bikewale.com/{0}-bikes/dealers-in-{1}/", makeMaskingName, cityMaskingName);
        alternate = String.Format("http://www.bikewale.com/m/{0}-bikes/dealers-in-{1}/", makeMaskingName, cityMaskingName);
        AdId = "1395986297721";
        AdPath = "/1017752/BikeWale_New_";
        isAd970x90Shown = false;
        isAd300x250Shown = false;
        isAd300x250BTFShown = false;
        isAd970x90BottomShown = false;
    %>
    <!-- #include file="/includes/headscript.aspx" -->
    <link href="<%= staticUrl != "" ? "http://st2.aeplcdn.com" + staticUrl : "" %>/css/dealer/listing.css?<%= staticFileVersion %>" rel="stylesheet" type="text/css" />
    <script src="http://maps.googleapis.com/maps/api/js?key=<%= Bikewale.Utility.BWConfiguration.Instance.GoogleMapApiKey %>&libraries=places"></script>
    <style>
        .popup-btn-progress-wrapper{width:138px;margin:0 auto}.popup-otp-progress-wrapper{width:180px;margin:0 auto}.popup-btn-progress-wrapper .btn,.popup-otp-progress-wrapper .btn{width:100%}.progress-bar{width:0;height:4px;background:#16A085;bottom:0;left:0;border-radius:2px}.btn-loader{background-color:#822821}.btnSpinner{right:22px;top:10px;z-index:9;background:#fff}#BWloader{text-align:center;position:relative;font-size:16px;margin-bottom:20px;bottom:0;width:100%}
        #getUserLocation {position:absolute;cursor:pointer;font-size:9px;}
        .thankyou-icon {width:52px; height:58px; background-position: -165px -436px; }
    </style>
</head>
<body class="bg-light-grey padding-top50">
    <form runat="server">
        <!-- #include file="/includes/headBW.aspx" -->
        <section>
            <div class="container padding-top10">
                <div class="grid-12">
                    <div class="breadcrumb margin-bottom15">
                        <!-- breadcrumb code starts here -->
                        <ul>
                            <li itemscope="" itemtype="http://data-vocabulary.org/Breadcrumb">
                                <a itemprop="url" href="/"><span itemprop="title">Home</span></a>
                            </li>
                            <li itemscope="" itemtype="http://data-vocabulary.org/Breadcrumb">
                                <span class="bwsprite fa-angle-right margin-right10"></span><a itemprop="url" href="/new/"><span itemprop="title">New Bikes</span></a>
                            </li>
                            <li itemscope="" itemtype="http://data-vocabulary.org/Breadcrumb">
                                <span class="bwsprite fa-angle-right margin-right10"></span><a itemprop="url" href="/new/locate-dealers/"><span itemprop="title">New Bike Dealer</span></a>
                            </li>
                            <li itemscope="" itemtype="http://data-vocabulary.org/Breadcrumb">
                                <span class="bwsprite fa-angle-right margin-right10"></span><a itemprop="url" href="/new/<%=makeMaskingName %>-dealers/"><span itemprop="title"><%=makeName%> Bikes Dealers</span></a>
                            </li>
                            <li class="current"><span class="bwsprite fa-angle-right margin-right10"></span><%=makeName%> Bikes Dealers in Mumbai</li>
                        </ul>
                        <div class="clear"></div>
                    </div>
                </div>
                <div class="clear"></div>
            </div>
        </section>

        <section>
            <div class="container margin-bottom20">
                <div class="grid-12">
                    <div class="content-box-shadow">
                        <div class="content-box-shadow padding-14-20">
                            <h1>Bajaj dealers in Mumbai</h1>
                        </div>
                        <div class="font14 text-light-grey content-inner-block-20">
                            Honda has 10 authorized dealers in Mumbai. Apart from the authorized dealerships, 
                            Honda bikes are also available at unauthorized showrooms and broker outlets. 
                            BikeWale recommends buying bikes only from authorized Honda dealer outlets in Mumbai. 
                            For information on test rides, price, offers, etc. you may get in touch with any of 
                            the below mentioned authorized Honda dealers in Mumbai.
                        </div>
                    </div>
                </div>
                <div class="clear"></div>
            </div>
        </section>
        

        <!-- #include file="/includes/footerBW.aspx" -->
        <!-- #include file="/includes/footerscript.aspx" -->

        <script type="text/javascript">

        </script>

        <script type="text/javascript" src="<%= staticUrl != "" ? "http://st2.aeplcdn.com" + staticUrl : "" %>/src/dealer/listing.js?<%= staticFileVersion %>"></script>
    </form>
</body>
</html>
