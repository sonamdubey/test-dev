<%@ Page Language="C#" AutoEventWireup="false" EnableViewState="false"  Inherits="Bikewale.Mobile.Used.BikesInCity" %>

<!DOCTYPE html>
<html>
<head>
     <%
         title = "Browse used bikes by cities";
         description = "Browse used bike by cities in India";
         canonical = "http://www.bikewale.com/used/browse-bikes-by-cities/";
         keywords = "city wise used bikes listing,used bikes for sale, second hand bikes, buy used bike";
         AdPath = "/1017752/Bikewale_Mobile_Model";
         AdId = "1444028976556";
         Ad_320x50 = true;
         Ad_Bot_320x50 = true;
         Ad_300x250 = false;
        
    %>
    <!-- #include file="/includes/headscript_mobile_min.aspx" -->
    <style type="text/css">
        @charset "utf-8";.form-control-box .search-icon-grey{position:absolute;right:10px;top:10px;cursor:pointer;z-index:2;background-position:-34px -275px}.filter-active #other-cities-label,.filter-active #popular-city-content{display:none}#used-popular-cities{max-width:680px;margin:0 auto;text-align:center;padding-right:10px;padding-left:10px}#used-popular-cities li{display:inline-block;vertical-align:top}.popular-city-target{width:150px;height:145px;display:block;padding-top:10px;border:1px solid #e2e2e2;text-align:left;color:#4d5057;margin-right:10px;margin-bottom:20px;margin-left:10px;overflow:hidden}#other-cities-list a{display:block;font-size:16px;color:#82888b;padding-top:8px;padding-bottom:8px}#other-cities-list a:first-child,.noResult{padding-top:16px}#other-cities-list a:hover{color:#4d5057;text-decoration:none}.noResult{font-size:16px;color:#82888b}.city-sprite{background:url(http://imgd2.aeplcdn.com/0x0/bw/static/sprites/m/bwm-city-sprite.png) no-repeat;display:inline-block}.ahmedabad-icon,.bangalore-icon,.chandigarh-icon,.chennai-icon,.delhi-icon,.hyderabad-icon,.kolkata-icon,.lucknow-icon,.mumbai-icon,.pune-icon{height:70px}.mumbai-icon{width:98px;background-position:0 0}.pune-icon{width:140px;background-position:-108px 0}.bangalore-icon{width:102px;background-position:-258px 0}.delhi-icon{width:54px;background-position:-370px 0}.chennai-icon{width:41px;background-position:-434px 0}.hyderabad-icon{width:49px;background-position:-485px 0}.kolkata-icon{width:138px;background-position:-544px 0}.lucknow-icon{width:132px;background-position:-692px 0}.ahmedabad-icon,.chandigarh-icon{width:0;background-position:0 0}@media only screen and (max-width:360px){.popular-city-target{margin-right:8px;margin-left:8px}}@media only screen and (max-width:320px){.popular-city-target{width:135px;margin-right:5px;margin-left:5px;margin-bottom:15px}}
    </style>
    <script type="text/javascript">
        <!-- #include file="\includes\gacode_mobile.aspx" -->
    </script>
</head>
<body>
    <form id="form1" runat="server">
        <!-- #include file="/includes/headBW_Mobile.aspx" -->
        
        <section>
            <div class="container bg-white clearfix box-shadow padding-bottom15 margin-bottom10">
                <div class="padding-top20 padding-right20 padding-left20">
                    <h1 class="margin-bottom15">Find used bikes in India</h1>
                    <div class="form-control-box">
                        <span class="bwmsprite search-icon-grey"></span>
                        <input type="text" id="cityInput" class="form-control padding-right40" placeholder="Type to select city" />
                    </div>
                </div>

                <div id="popular-city-content" class="margin-top20 margin-bottom15 font14">
                    <p class="text-bold margin-left20 margin-bottom20">Popular cities</p>
                    <ul id="used-popular-cities">
                        <%foreach ( Bikewale.Entities.Used.UsedBikeCities objCity in  objBikeCityCountTop) {%>
                                        
                             <li>
                                <a href="/m/used/bikes-in-<%=objCity.CityMaskingName %>/" title="Used bikes in <%=objCity.CityName %>" class="popular-city-target">
                                    <div class="text-center margin-bottom5">
                                        <span class="city-sprite <%=objCity.CityMaskingName %>-icon"></span>
                                    </div>
                                    <div class="padding-right10 padding-left10">
                                        <p class="text-bold"><%=objCity.CityName %></p>
                                        <p class="text-light-grey"><%=objCity.bikesCount %> Used bikes</p>
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
                    <ul id="other-cities-list">
                        <%foreach (Bikewale.Entities.Used.UsedBikeCities objCity in objBikeCityCount)
                          {%>
                        <li>
                            <a href="/m/used/bikes-in-<%=objCity.CityMaskingName %>/" title="Used bikes in <%=objCity.CityName %>"><%=objCity.CityName %> (<%=objCity.bikesCount %>)</a>
                        </li>                       <%} %>
                    </ul>
                </div>

            </div>
        </section>

        <script type="text/javascript" src="<%= staticUrl != "" ? "http://st1.aeplcdn.com" + staticUrl : "" %>/m/src/frameworks.js?<%= staticFileVersion %>"></script>
        <!-- #include file="/includes/footerBW_Mobile.aspx" -->
        <link href="<%= staticUrl != "" ? "http://st2.aeplcdn.com" + staticUrl : "" %>/m/css/bwm-common-btf.css?<%= staticFileVersion %>" rel="stylesheet" type="text/css" />
        <script type="text/javascript" src="<%= staticUrl != "" ? "http://st2.aeplcdn.com" + staticUrl : "" %>/m/src/common.min.js?<%= staticFileVersion %>"></script>
        <script type="text/javascript" src="<%= staticUrl != "" ? "http://st2.aeplcdn.com" + staticUrl : "" %>/m/src/used/bikes-in-city.js?<%= staticFileVersion%>"></script>
        <link href='https://fonts.googleapis.com/css?family=Open+Sans:400,700' rel='stylesheet' type='text/css' />
    </form>
</body>
</html>
