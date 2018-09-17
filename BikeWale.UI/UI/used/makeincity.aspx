<%@ Page Language="C#" AutoEventWireup="false"  Inherits="Bikewale.Used.MakeInCity" EnableViewState="false" %>
<!DOCTYPE html>

<html>
<head>
    <%
        title = pgTitle;
        description = pgDescription;
        canonical = pgCanonical;
        keywords = pgKeywords;
        AdId = "1475576036058";
        AdPath = "/1017752/BikeWale_UsedBikes_Search_Results_";
        isAd300x250BTFShown = false;
        isAd300x250Shown = false;
        alternate = pgAlternative;
        isAd970x90BottomShown = true;
        isAd970x90Shown = true;        
    %>

    <!-- #include file="/UI/includes/headscript_desktop_min.aspx" -->
    <style type="text/css">
        @charset "utf-8";#popular-city-list{margin-right:10px;margin-left:10px;padding-bottom:15px;border-bottom:1px solid #e2e2e2}#popular-city-list li{margin:12px;display:inline-block;vertical-align:top}#popular-city-list a{width:292px;height:201px;border:1px solid #f6f6f6;-webkit-box-shadow:0 1px 2px rgba(0,0,0,.2);-moz-box-shadow:0 1px 2px rgba(0,0,0,.2);-ms-box-shadow:0 1px 2px rgba(0,0,0,.2);-o-box-shadow:0 1px 2px rgba(0,0,0,.2);box-shadow:0 1px 2px rgba(0,0,0,.2)}.city-card-target{display:block}.city-card-target:hover{text-decoration:none}#other-city-list li{font-size:16px;margin-bottom:20px}#other-city-list a{color:#82888b}.city-bike-count{color:#a2a2a2}.city-image-preview{width:292px;height:114px;display:block;margin-bottom:15px;text-align:center;padding-top:10px}.city-sprite{background:url(https://imgd.aeplcdn.com/0x0/bw/static/sprites/d/city-sprite.png?24062016) no-repeat;display:inline-block}.c1-icon,.c10-icon,.c105-icon,.c12-icon,.c128-icon,.c176-icon,.c198-icon,.c2-icon,.c220-icon,.c239-icon{height:92px}.c1-icon{width:130px;background-position:0 0}.c12-icon{width:186px;background-position:-140px 0}.c2-icon{width:136px;background-position:-336px 0}.c10-icon{width:70px;background-position:-482px 0}.c176-icon{width:53px;background-position:-562px 0}.c105-icon{width:65px;background-position:-625px 0}.c198-icon{width:182px;background-position:-700px 0}.c220-icon{width:174px;background-position:-892px 0}.c128-icon,.c239-icon{width:0;background-position:0 0}.form-control-box .search-icon-grey{position:absolute;right:10px;top:10px;cursor:pointer;z-index:2}.no-result-found{color:#82888b;font-size:16px;margin-bottom:20px;padding-right:10px;padding-left:10px}@media only screen and (max-width:1024px){#popular-city-list li{margin:6px}}
    </style>
    <script type="text/javascript">
        <!-- #include file="\UI\includes\gacode_desktop.aspx" -->
    </script>
</head>
<body class="bg-light-grey header-fixed-inner">
    <form id="form1" runat="server">
        <!-- #include file="/UI/includes/headBW.aspx" -->
        
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
        <%if(MakeDetails!=null) {%>
        <section>
            <div class="container margin-bottom20">
                <div class="grid-12">
                    <div class="content-box-shadow">
                        <h1 class="content-inner-block-20">Browse used <%=MakeDetails.MakeName %> bikes by cities</h1>
                        <p class="font16 text-default text-bold padding-left20 margin-bottom10">Popular cities</p>
                        <ul id="popular-city-list">
                            <%foreach (var objCity in UsedBikeCityCountTopList)
                                {%>
                                <li>
                                    <a href="/used/<%=makeMaskingName%>-bikes-in-<%=objCity.CityMaskingName %>/" title="Used <%=MakeDetails.MakeName%> bikes in <%=objCity.CityName %>" class="city-card-target">
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
                            <p class="font16 text-default text-bold padding-left10 margin-bottom15">Other cities</p>

                            <div class="grid-12">
                                <div id="filter-input" class="form-control-box margin-bottom15">
                                    <span class="bwsprite search-icon-grey"></span>
                                    <input type="text" class="form-control padding-right40" placeholder="Type to search city" id="getCityInput" />
                                </div>
                            </div>
                            <div class="clear"></div>

                            <ul id="other-city-list" data-error-message="No city found!">
                                <%foreach (var objCity in UsedBikeCityCountList)
                                    {%>
                                    <li class="grid-4">
                                        <a href="/used/<%=makeMaskingName%>-bikes-in-<%=objCity.CityMaskingName %>/" title="Used <%=MakeDetails.MakeName%> bikes in <%=objCity.CityName %>">
                                            <%=string.Format("{0} ({1})",objCity.CityName ,objCity.BikesCount )%>
                                        </a>
                                    </li>
                                <%} %>
                            </ul>
                            <div id="city-no-result" class="no-result-found"></div>
                            <div class="clear"></div>
                        </div>
                    </div>
                </div>
                <div class="clear"></div>
            </div>
        </section>   
        <%} %>    

        <script type="text/javascript" src="<%= staticUrl %>/UI/src/frameworks.js?<%=staticFileVersion %>"></script>
        <!-- #include file="/UI/includes/footerBW.aspx" -->
        <link href="<%= staticUrl %>/UI/css/bw-common-btf.css?<%=staticFileVersion %>" rel="stylesheet" type="text/css" />
        <!-- #include file="/UI/includes/footerscript.aspx" -->
        <!-- #include file="/UI/includes/fontBW.aspx" -->
        <script type="text/javascript">

            /* filter */
            $("#getCityInput").on("keyup", function (event) {
                filter.location($(this), '#other-city-list', '#city-no-result'); // (input field, list to filter, error message container)
            });

            var filter = {

                location: function (filterContent, filterList, noResultContent) {
                    var inputText = $(filterContent).val(),
                        inputTextLength = inputText.length,
                        elementList = $(filterList + ' li'),
                        len = elementList.length,
                        element, i;

                    inputText = inputText.toLowerCase();

                    if (inputText != "") {
                        for (i = 0; i < len; i++) {
                            element = elementList[i];

                            var locationName = $(element).text().toLowerCase().trim();
                            if (/\s/.test(locationName))
                                var splitlocationName = locationName.split(" ")[1];
                            else
                                splitlocationName = "";

                            if ((inputText == locationName.substring(0, inputTextLength)) || inputText == splitlocationName.substring(0, inputTextLength)) {
                                element.style.display = "block";
                            }
                            else {
                                element.style.display = "none";
                            }
                        }

                        var list = $(filterList),
                            visibilityCount = 0;

                        list.each(function () {
                            var visibleElements = $(this).find('li[style*="display: block;"]').length;

                            if (visibleElements != 0) {
                                visibilityCount++;
                            }
                        });

                        if (visibilityCount == 0) {
                            var errorMessage = $(filterList).attr('data-error-message');
                            $(noResultContent).show().text(errorMessage);
                        }
                        else {
                            $(noResultContent).hide();
                        }
                    }
                    else {
                        for (i = 0; i < len; i++) {
                            element = elementList[i];
                            element.style.display = "block";
                        }
                        $(noResultContent).hide();
                    }
                }
            }
                      
        </script>
    </form>
</body>
</html>