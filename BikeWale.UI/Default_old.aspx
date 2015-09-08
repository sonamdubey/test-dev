<%@ Page Language="C#" Trace="false" Inherits="Bikewale.Default_old" EnableEventValidation="false" Async="true"  %>
<%--<%@ Register TagPrefix="FM" TagName="ForumsMin" Src="/controls/forumsmin.ascx" %>
<%@ Register TagPrefix="TA" TagName="TipsAdvicesMin" Src="/controls/TipsAdvicesMin.ascx" %>
<%@ Register TagPrefix="CM" TagName="ComparisonMin" Src="/controls/comparisonmin.ascx" %>--%>
<%@ Register TagPrefix="FB" TagName="FeaturedBike" Src="/controls/featuredbike.ascx" %>
<%@ Register TagPrefix="UB" TagName="UpcomingBikesMin" Src="/controls/UpcomingBikesMin.ascx" %>
<%@ Register TagPrefix="UL" TagName="TopUsedListedBike" Src="/controls/TopUsedListedBike.ascx"%>
<%@ Register TagPrefix="BP" TagName="InstantBikePrice" Src="/controls/instantbikeprice.ascx" %>
<%@ Register TagPrefix="LD" TagName="LocateDealer" Src="/controls/locatedealer.ascx" %>
<%@ Register TagPrefix="CE" TagName="CalculateEMIMin" Src="/controls/CalculateEMIMin.ascx" %>
<%@ Register TagPrefix="BB" TagName="BrowseBikes" Src="/controls/browsebikes.ascx" %>
<%@ Register TagPrefix="B" TagName="HomePageBanner" Src="/controls/homepagebanner.ascx" %>
<%@ Register TagPrefix="RT" TagName="RoadTest" Src="/controls/RoadTest.ascx" %>
<%@ Register TagPrefix="LB" TagName="LaunchedBikes" Src="/controls/RecentLaunchedBikesMin.ascx" %>
<%@ Register TagPrefix="PW" TagName="PopupWidget" Src="/controls/PopupWidget.ascx" %>
<%
    title 			= "New Bikes, Used Bikes, Bike Prices, Reviews & Photos in India";
    keywords		= "new bikes, used bikes, buy used bikes, sell your bike, bikes prices, reviews, photos, news, compare bikes, Instant Bike On-Road Price";
	description 	= "BikeWale - India's favourite bike portal. Find new and used bikes, buy or sell your bikes, compare new bikes prices & values.";  
    AdPath="/1017752/BikeWale_HomePage_";
    AdId="1395985604192";
    alternate   ="http://www.bikewale.com/m/";
%>
<!-- #include file="/includes/headhome.aspx" -->
<PW:PopupWidget runat="server" ID="PopupWidget" />

<!--BW Gallery code start here -->
<style type="text/javascript">
    /* Targeted IDs*/
    #featured-bike img, #upcoming-bike img, #road-test img{ width: 196px;}
</style>
    <div class="gallery-container">
        <div class="container_12"> 
            <div class="block-spacing">
                <div class="grid_8">
                    <!-- Browse code start here -->
                    <div class="light-grey-bg content-block border-radius5">
                        <BB:BrowseBikes runat="server" ID="BrowseBikes" VersionRequired="false"/>                   
                        <div class="clear"></div>
                    </div>                        
                    <!-- Browse code end here -->
                    <div class="margin-top20">
                        <ul class="gallery-tabs">
                            <li id="lifeatured-data" class="active-tab">Featured</li>
                            <li id="linews-data">News</li>
                            <li id="liroad-test-data">Road Tests</li>                            
                        </ul>
                        <div class="gallery-border"></div>
                    </div>
                
                    <div class="light-grey-bg content-block margin-top10" style="height:270px;">                    
                        <div id="featured-data">
                            <!-- BikeWale_FeaturedBike_600x270 -->
                            <%--<div id='div-gpt-ad-<%= AdId %>-2' style='width:600px; height:270px;'>
                            <script type='text/javascript'>
                                googletag.cmd.push(function () { googletag.display('div-gpt-ad-<%= AdId %>-2'); });
                            </script>
                            </div>--%>
                            <B:HomePageBanner runat="server" ID="FeaturedBanner"/>
                        </div>
                        <div id="news-data" class="hide">
                            <B:HomePageBanner runat="server" ID="NewsBanner" Category="1"/>
                        </div>
                        <div id="road-test-data" class="hide">
                            <B:HomePageBanner runat="server" ID="RoadTestBanner" Category="8"/>
                        </div>
                    </div>
                </div>
                <div class="grid_4">
                    <div class="light-grey-bg content-block border-radius5 padding-bottom20">
                        <BP:InstantBikePrice runat="server" ID="InstantBikePrice" />
                    </div>
                    <div class="light-grey-bg content-block border-radius5 margin-top10 padding-bottom20">
                        <LD:LocateDealer runat="server" id="LocateDealer" />
                    </div>
                    <div class="light-grey-bg content-block border-radius5 margin-top10 padding-bottom20">
                        <CE:CalculateEMIMin runat="server" ID="CalculateEMIMin" />
                    </div>                        
                </div>
			    <div class="clear"></div>
            </div>
        </div>
    </div>
    <!--BW Gallery code end here -->
        
    <!--Content code start here -->
    <div class="container_12 margin-top10">
        <div class="grid_4">
            <div class="v-liner">
                <h2>Buy Used Bikes</h2>            
                <div class="margin-top5 margin-left5 left-float">                           
                    <asp:DropDownList id="ddlUsedCities" runat="server" CssClass="brand"></asp:DropDownList>                                                     
                </div>
                <div class="action-btn right-float margin-top5"><a id="btnSearchUsedGo">Find</a></div>
                <script type="text/javascript">
                    $("#btnSearchUsedGo").click(function () {
                        if ($("#ddlUsedCities").val() == "-1") {
                            alert("Please select city");
                            return false;
                        }
                        else {
                           var city= $("#ddlUsedCities").val();
                            //window.location = "/used/search.aspx?#city=" + city + "&dist=50";
                           var city = $("#ddlUsedCities option:selected").val().split('_')[1];
                           window.location = "/used/bikes-in-" + city + '/';
                        }
                    });
                </script>
                <div class="clear"></div>
                <div class="dotted-line margin-top10"></div>
                <UL:TopUsedListedBike runat="server" ID="TopUsedListedBike" TopRecords="7" /> 
            </div>
        </div>
        <div class="grid_4">
            <div class="margin-right10">
                <h2>Sell Your Bike Now</h2>
                <p class="black-text">Sell your bike easy and fast!</p>
                <div class="dotted-line margin-top5"></div>
                <div id="sybh-list" class="padding-top10">                      
                    <div><a class="person" style="text-decoration:none; cursor:default;" title="BikeWale team works with you to get you best price for your bike">Get Expert Help</a></div>
                    <div class="sep"></div>                        
                    <div><a class="timer" style="text-decoration:none; cursor:default;" title="Your bike is listed for sale until it is sold">Unlimited Time Period</a></div>
                    <div class="sep"></div>
                    <div><a class="watch" style="text-decoration:none; cursor:default;" title="BikeWale is committed to give your bike maximum exposure">Maximum Visibility</a></div>
                    <div class="sep"></div>
                    <div><a class="award" style="text-decoration:none; cursor:default;" title="Buyers' mobile numbers are verified before they are sent to you">Genuine Buyers</a></div>
                    <div class="sep"></div>              
                </div>
                <div class="action-btn margin-top10 center-align"><a href="/used/sell/">Sell My Bike Now</a></div>
                <div class="shadow-white"></div>
            </div>
        </div>
        <div class="grid_4">
            <div><!-- BikeWale_HP/BikeWale_HP_300x250 -->
                <!-- #include file="/ads/Ad300x250.aspx" -->
            </div>
        </div>
    </div>
    <!--Content code end here -->
        
    <!--2 Content code start here -->
    <div class="featured-bike-container"></div>
    <div class="container_12 margin-top10">
        <div class="grid_12 padding-top10 featured-bike-tabs">
            <ul class="featured-bike-tabs padding-top10">
                <li id="lifeatured-bike" class="fbike-active-tab">Featured Bikes</li>
                <li id="liupcoming-bike">Upcoming New Bikes</li>   
                <li id="liroad-test">Road Tests</li> 
                <li id="lilaunched-bikes">New Launches</li>           
            </ul>
        </div>      
        <div class="padding-top10" id="featured-bike">              
            <FB:FeaturedBike runat="server" ID="FeaturedBike" TopRecords="4" ControlWidth="grid_3" ImageWidth="196px;"/>
        </div>
        <div class="padding-top10 hide" id="upcoming-bike">      
            <UB:UpcomingBikesMin runat="server" ID="UpcomingBikesMin" TopRecords="4" ControlWidth="grid_3" ImageWidth="196px;"/>        
        </div>  
        <div class="padding-top10 hide" id="road-test">   
            <RT:RoadTest id="ucRoadTestMin" runat="server" TopRecords="4" ControlWidth="grid_3" ImageWidth="196px;"></RT:RoadTest>
        </div>
        <div class="padding-top10 hide" id="launched-bikes">
            <LB:LaunchedBikes  ID="launchedBike" runat="server"/>
        </div>
        <div class="clear"></div>    
    </div>
    <!--2 Content code end here -->
        
    <!--3 Content code start here -->
    <%--<div class="articles-container margin-top15 margin-bottom15 hide">
        <div class="container_12 hide">
            <div class="grid_4 grey-bg">
                <div class="content-block">
                    <CM:ComparisonMin runat="server" ID="ComparisonMin" TopRecords="6" />
                </div>
            </div>
            <div class="grid_4 white-bg">
                <div class="content-block">
                    <TA:TipsAdvicesMin runat="server" ID="TipsAdvicesMin" />
                </div>
            </div>
            <div class="grid_4 grey-bg">
                <div class="content-block">
                     <FM:ForumsMin runat="server" ID="ForumsMin" TopRecords="6" />
                </div>
            </div>
        </div>
        <div class="clear"></div>
    </div>    --%>
<script type="text/javascript">
    $(document).ready(function () {
        $("a.person,a.timer,a.watch,a.award").bt({ contentSelector: "$(this).attr('title')", positions: ['top', 'bottom', 'left'], fill: '#ffffee', strokeWidth: 1, strokeStyle: '#666666', width: '300px', cssClass: 'f-small', spikeLength: 7 });

        $("#lifeatured-data,#linews-data,#liroad-test-data").click(function () {
            $("#liroad-test-data,#linews-data,#lifeatured-data").removeClass("active-tab");
            $(this).addClass("active-tab");
            $("#news-data,#road-test-data,#featured-data").addClass("hide");
            var bannerdiv = $(this).attr('id').substring(2, $(this).attr('id').length);
            $("#" + bannerdiv).removeClass("hide");
            $("#" + bannerdiv).children().children().children("ul.bike-img-list").children('li').addClass("hide");
            $("#" + bannerdiv).children().children().children("ul.bike-img-list").children('li').first().removeClass("hide");
        });
       
        $("#lifeatured-bike,#liupcoming-bike, #liroad-test , #lilaunched-bikes").click(function () {
            $("#liupcoming-bike,#lifeatured-bike,#liroad-test, #lilaunched-bikes").removeClass("fbike-active-tab");
            $(this).addClass("fbike-active-tab");           
            $("#featured-bike,#upcoming-bike,#road-test,#launched-bikes").addClass("hide");
            $("#" + $(this).attr('id').substring(2, $(this).attr('id').length)).removeClass("hide");
        });        
    });

    function SelectBannerImage(obj) {       
        var BasicId = $(obj).attr("id").substring(13, $(obj).attr("id").length);
        $('li[name*="banner-"]').addClass("hide");
        $("#banner-full-" + BasicId).removeClass("hide");
    }
</script>
<!--3 Content code end here -->
 <!-- #include file="/includes/footerInner.aspx" -->       