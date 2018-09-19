<%@ Page Language="C#" AutoEventWireup="false"  Inherits="Bikewale.Mobile.Used.MakesInCity" EnableViewState="false"%>

<!DOCTYPE html>
<html>
<head>
     <%
         title = pgTitle;
         description = pgDescription;
         canonical = pgCanonical;
         keywords = pgKeywords;
         AdPath = "/1017752/Bikewale_Mobile_Model_";
         AdId = "1444028976556";
         Ad_320x50 = true;
         Ad_Bot_320x50 = true;
         Ad_300x250 = false;
        
    %>
    <!-- #include file="/UI/includes/headscript_mobile_min.aspx" -->
    <style type="text/css">
        @charset "utf-8";.form-control-box .search-icon-grey{position:absolute;right:10px;top:10px;cursor:pointer;z-index:2;background-position:-34px -275px}.filter-active #other-cities-label,.filter-active #popular-city-content{display:none}#used-popular-cities{max-width:680px;margin:0 auto;text-align:center;padding-right:10px;padding-left:10px}#used-popular-cities li{display:inline-block;vertical-align:top}.popular-city-target{width:150px;height:145px;display:block;padding-top:10px;border:1px solid #e2e2e2;text-align:left;color:#4d5057;margin-right:10px;margin-bottom:20px;margin-left:10px;overflow:hidden}#other-cities-list a{display:block;font-size:16px;color:#82888b;padding-top:8px;padding-bottom:8px}#other-cities-list a:first-child,.no-result-found{padding-top:16px}#other-cities-list a:hover{color:#4d5057;text-decoration:none}.no-result-found{font-size:16px;color:#82888b}.city-sprite{background:url(https://imgd.aeplcdn.com/0x0/bw/static/sprites/m/bwm-city-sprite-v2.png?v07Oct2016) no-repeat;display:inline-block}.c1-icon,.c10-icon,.c105-icon,.c12-icon,.c176-icon,.c198-icon,.c2-icon,.c220-icon{height:48px}.c1-icon{width:78px;background-position:0 0}.c12-icon{width:112px;background-position:-84px 0}.c2-icon{width:82px;background-position:-202px 0}.c10-icon{width:38px;background-position:-290px 0}.c176-icon{width:33px;background-position:-334px 0}.c105-icon{width:34px;background-position:-373px 0}.c198-icon{width:110px;background-position:-413px 0}.c220-icon{width:106px;background-position:-529px 0}@media only screen and (max-width:360px){.popular-city-target{margin-right:8px;margin-left:8px}}@media only screen and (max-width:320px){.popular-city-target{width:135px;margin-right:5px;margin-left:5px;margin-bottom:15px}}
    </style>
    <script type="text/javascript">
        <!-- #include file="\UI\includes\gacode_mobile.aspx" -->
    </script>
</head>
<body>
    <form id="form1" runat="server">
        <!-- #include file="/UI/includes/headBW_Mobile.aspx" -->
        <%if(MakeDetails!=null){ %>
        <section>
            <div class="container bg-white clearfix box-shadow padding-bottom15 margin-bottom10">
                <div class="padding-top20 padding-right20 padding-left20">
                    <h1 class="margin-bottom15">Browse used <%=MakeDetails.MakeName %> bikes by cities</h1>
                    <div class="form-control-box">
                        <span class="bwmsprite search-icon-grey"></span>
                        <input type="text" id="cityInput" class="form-control padding-right40" placeholder="Type to select city" />
                    </div>
                </div>

                <div id="popular-city-content" class="margin-top20 margin-bottom15 font14">
                    <p class="text-bold margin-left20 margin-bottom20">Popular cities</p>
                    <ul id="used-popular-cities">
                        <%foreach (Bikewale.Entities.Used.UsedBikeCities objCity in UsedBikeCityCountTopList)
                          {%>
                                        
                             <li>
                                <a href="/m/used/<%=makeMaskingName %>-bikes-in-<%=objCity.CityMaskingName %>/" title="Used <%=MakeDetails.MakeName %> bikes in <%=objCity.CityName %>" class="popular-city-target">
                                    <div class="text-center margin-bottom15 padding-top10">
                                        <span class="city-sprite c<%=objCity.CityId %>-icon"></span>
                                    </div>
                                    <div class="padding-right10 padding-left10">
                                        <p class="text-bold"><%=objCity.CityName %></p>
                                        <p class="text-light-grey"><%=objCity.BikesCount %> Used bikes</p>
                                    </div>
                                </a>
                            </li>
                       
                        <%} %>
                       
                    </ul>
                    <div class="clear"></div>
                    <div class="margin-right20 margin-left20 border-solid-bottom"></div>
                </div>

                <div class="padding-right20 padding-left20">
                    <p id="other-cities-label" class="font14 text-bold">Other cities</p>
                    <ul id="other-cities-list" data-error-message="No city found!">
                        <%foreach (Bikewale.Entities.Used.UsedBikeCities objCity in UsedBikeCityCountList)
                          {%>
                        <li>
                            <a href="/m/used/<%=makeMaskingName %>-bikes-in-<%=objCity.CityMaskingName %>/" title="Used <%=MakeDetails.MakeName %> bikes in <%=objCity.CityName %>"><%=string.Format("{0} ({1})",objCity.CityName ,objCity.BikesCount )%></a>
                        </li>
                       <%} %>
                    </ul>
                    <div id="city-no-result" class="no-result-found"></div>
                </div>

            </div>
        </section>
        <section>
            <div class="breadcrumb">
                <span class="breadcrumb-title">You are here:</span>
                <ul>
                    <li>
                        <a class="breadcrumb-link" href="/m/">
                            <span class="breadcrumb-link__label" itemprop="name">Home</span>
                        </a>
                    </li>
                    <li>
                        <a class="breadcrumb-link" href="/m/used/">
                            <span class="breadcrumb-link__label" itemprop="name">Used Bikes</span>
                        </a>
                    </li>
                     <li>
                        <a class="breadcrumb-link" href="/used/bikes-in-india/">
                            <span class="breadcrumb-link__label" itemprop="name">Search</span>
                        </a>
                    </li>
                    <li>
                        <span class="breadcrumb-link__label">Bikes in City</span>
                    </li>
                </ul>
                <div class="clear"></div>
            </div>
            <div class="clear"></div>
        </section>
        <%} %>

        <script type="text/javascript" src="<%= staticUrl %>/UI/m/src/frameworks.js?<%= staticFileVersion %>"></script>
        <!-- #include file="/UI/includes/footerBW_Mobile.aspx" -->
        <link href="<%= staticUrl  %>/UI/m/css/bwm-common-btf.css?<%= staticFileVersion %>" rel="stylesheet" type="text/css" />
        <!-- #include file="/UI/includes/footerscript_mobile.aspx" -->
        <script type="text/javascript" src="<%= staticUrl  %>/UI/m/src/used/bikes-in-city.js?<%= staticFileVersion%>"></script>
        <!-- #include file="/UI/includes/fontBW_Mobile.aspx" -->
    </form>
</body>
</html>

