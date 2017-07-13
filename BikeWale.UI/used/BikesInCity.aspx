<%@ Page Language="C#" AutoEventWireup="false" EnableViewState="false" Inherits="Bikewale.Used.BikesInCity" %>
<!DOCTYPE html>

<html>
<head>
    <%
        title = "Browse used bikes by cities";
        description = "Browse used bike by cities in India";
        canonical = "https://www.bikewale.com/used/browse-bikes-by-cities/";
        keywords = "city wise used bikes listing,used bikes for sale, second hand bikes, buy used bike";
        AdId = "1475576036058";
        AdPath = "/1017752/BikeWale_UsedBikes_Search_Results_";
        isAd300x250BTFShown = false;
        isAd300x250Shown = false; 
        alternate = "https://www.bikewale.com/m/used/browse-bikes-by-cities/";
        isAd970x90BottomShown = true;
        isAd970x90Shown = true;        
    %>

    <!-- #include file="/includes/headscript_desktop_min.aspx" -->
    <style type="text/css">
        @charset "utf-8";#popular-city-list{margin-right:10px;margin-left:10px;padding-bottom:15px;border-bottom:1px solid #e2e2e2}#popular-city-list li{margin:12px;display:inline-block;vertical-align:top}#popular-city-list a{width:292px;height:201px;border:1px solid #f6f6f6;-webkit-box-shadow:0 1px 2px rgba(0,0,0,.2);-moz-box-shadow:0 1px 2px rgba(0,0,0,.2);-ms-box-shadow:0 1px 2px rgba(0,0,0,.2);-o-box-shadow:0 1px 2px rgba(0,0,0,.2);box-shadow:0 1px 2px rgba(0,0,0,.2)}.city-card-target{display:block}.city-card-target:hover{text-decoration:none}#other-city-list li{font-size:16px;margin-bottom:20px}#other-city-list a{color:#82888b}.city-bike-count{color:#a2a2a2}.city-image-preview{width:292px;height:114px;display:block;margin-bottom:15px;text-align:center;padding-top:10px}.city-sprite{background:url(https://imgd.aeplcdn.com/0x0/bw/static/sprites/d/city-sprite.png?24062016) no-repeat;display:inline-block}.c128-icon,.c2-icon,.c239-icon,.c176-icon,.c10-icon,.c105-icon,.c198-icon,.c220-icon,.c1-icon,.c12-icon{height:92px}.c1-icon{width:130px;background-position:0 0}.c12-icon{width:186px;background-position:-140px 0}.c2-icon{width:136px;background-position:-336px 0}.c10-icon{width:70px;background-position:-482px 0}.c176-icon{width:53px;background-position:-562px 0}.c105-icon{width:65px;background-position:-625px 0}.c198-icon{width:182px;background-position:-700px 0}.c220-icon{width:174px;background-position:-892px 0}.c128-icon,.c239-icon{width:0;background-position:0 0}@media only screen and (max-width:1024px){#popular-city-list li{margin:6px}}
    </style>
    <script type="text/javascript">
        <!-- #include file="\includes\gacode_desktop.aspx" -->
    </script>
</head>
<body class="bg-light-grey header-fixed-inner">
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
                            <li itemtype="http://data-vocabulary.org/Breadcrumb"><span class="bwsprite fa-angle-right margin-right10"></span>
                              <a href="/used/" itemprop="url"><span>Used Bikes</span></a>
                            </li>
                            <li itemtype="http://data-vocabulary.org/Breadcrumb"><span class="bwsprite fa-angle-right margin-right10"></span>
                              <a href="/used/bikes-in-india/" itemprop="url"><span>Search</span></a>
                            </li>
                            <li><span class="bwsprite fa-angle-right margin-right10"></span>
                              <span>Bikes in City</span>
                            </li>
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
                        <h1 class="content-inner-block-20">Browse used bike by cities</h1>
                        <p class="font16 text-default text-bold padding-left20 margin-bottom10">Popular cities</p>
                        <ul id="popular-city-list">
                       <%foreach (var objCity in  objBikeCityCountTop) {%>
                            <li>
                                <a href="/used/bikes-in-<%=objCity.CityMaskingName %>/" title="Used bikes in <%=objCity.CityName %>" class="city-card-target">
                                    <div class="city-image-preview">
                                        <span class="city-sprite c<%=objCity.CityId %>-icon"></span>
                                    </div>
                                    <div class="font14 padding-left20 padding-right20">
                                        <p class="text-default text-bold margin-bottom5"><%=objCity.CityName %></p>
                                        <p class="text-light-grey"><%=objCity.BikesCount %> Used bikes</p>
                                    </div>
                                </a>
                            </li>
                       <%} %> 
                        </ul>
                        <div class="padding-top20 padding-right10 padding-bottom10 padding-left10">
                            <p class="font16 text-default text-bold padding-left10 margin-bottom20">Other cities</p>
                            <ul id="other-city-list">
                                <%foreach (var objCity in objBikeCityCount)
                                    {%>
                                    <li class="grid-4">
                                        <a href="/used/bikes-in-<%=objCity.CityMaskingName %>/" title="Used bikes in <%=objCity.CityName %>">
                                            <%=string.Format("{0} ({1})",objCity.CityName ,objCity.BikesCount )%>
                                        </a>
                                    </li>
                                <%} %>
                            </ul>
                            <div class="clear"></div>
                        </div>
                    </div>
                </div>
                <div class="clear"></div>
            </div>
        </section>       

        <script type="text/javascript" src="<%= staticUrl  %>/src/frameworks.js?<%=staticFileVersion %>"></script>
        <!-- #include file="/includes/footerBW.aspx" -->
        <link href="<%= staticUrl  %>/css/bw-common-btf.css?<%=staticFileVersion %>" rel="stylesheet" type="text/css" />
        <!-- #include file="/includes/footerscript.aspx" -->
        <!-- #include file="/includes/fontBW.aspx" -->
    </form>
</body>
</html>