<%@ Page Language="C#" AutoEventWireup="false" Inherits="Bikewale.New.LocateNewBikeDealers" %>
<%@ Import Namespace="Bikewale.Common" %>
<!doctype html>
<html>
<head>
    <%
        title = "New Bike Dealers in India - Locate Authorized Showrooms - BikeWale";
        keywords = "new bike dealers, new bike showrooms, bike dealers, bike showrooms, showrooms, dealerships, price quote";
        description = "Locate New bike dealers and authorized bike showrooms in India. Find new bike dealer information for more than 200 cities. Authorized company showroom information includes full address, phone numbers, email address, pin code etc.";
        canonical = "http://www.bikewale.com/new/locate-dealers/";
        alternate = "http://www.bikewale.com/m/new/locate-dealers/";
        //AdId = "1395986297721";
        //AdPath = "/1017752/BikeWale_New_";
        isHeaderFix = false;
        isAd970x90Shown = false;
        isTransparentHeader = true;
        PopupWidget.Visible = false;
    %>
    <!-- #include file="/includes/headscript.aspx" -->
    <style type="text/css">
        .dealerlocator-banner { background:#8d8c8a url(http://imgd1.aeplcdn.com/0x0/bw/static/landing-banners/d/booking-landing-banner.jpg) no-repeat left top; height:489px; padding-top: 1px;}.locator-search-container { margin:0 auto; width:600px; }.locator-search-container .form-control { padding:8px; border-radius:0; }.locator-search-brand, .locator-search-city { width:233px; height:40px; float:left;}.locator-search-brand select, .locator-search-city select { width: 233px; height: 38px; color: #555; border: none;}.locator-search-city .chosen-container { border-left:1px solid #ccc !important; }.locator-search-brand .chosen-container, .locator-search-city .chosen-container { border: 0; border-radius: 0;}.locator-search-brand .chosen-container:first-child, .locator-search-city .chosen-container:first-child  { border: 0; border-radius: 2px !important;}.locator-search-btn { width:133px; }.locator-search-btn.btn-lg { padding:7px 20px; padding:8px 20px 7px\9; }.brandlogosprite { background-image: url(http://imgd3.aeplcdn.com/0x0/bw/static/sprites/d/brand-type-sprite.png?21Mar2016v1); background-repeat:no-repeat; display:inline-block; }.brand-type-container li {display:inline-block; *display:inline; *zoom:1; vertical-align:top;width:180px;height:85px;margin:0 5px 30px;text-align:center;font-size:14px;-moz-border-radius: 2px;-webkit-border-radius: 2px;-o-border-radius: 2px;-ms-border-radius: 2px;border-radius: 2px;}.brand-type { width:180px; height:50px; display:block; margin:0 auto; }.brand-type-title { margin-top:10px; display:block; }.brand-type-container a { text-decoration:none; color:#1a1a1a; display: inline-block; }.brand-type-container li:hover span.brand-type-title {font-weight: bold; }.brand-bottom-border {overflow:hidden;}@-moz-document url-prefix() {.locator-search-btn.btn-lg { padding:6px 20px; }}
    </style>
</head>
<body class="bg-white">
    <form runat="server">
        <!-- #include file="/includes/headBW.aspx" -->
        <header class="dealerlocator-banner">
            <div class="container">
                <div class="welcome-box">
                    <h1 class="font30 text-uppercase margin-bottom30">Dealer locator</h1>
                    <p class="font20 margin-bottom50">Locate dealers near you</p>
                    <div class="locator-search-container">
                        <div class="locator-search-brand form-control-box">
                            <select class="form-control chosen-select"></select>
                        </div>
                        <div class="locator-search-city form-control-box">
                            <select class="form-control chosen-select"></select>
                        </div>
                        <input type="button" class="btn btn-orange font16 btn-lg leftfloat locator-search-btn rounded-corner-no-left" value="Search" />
                        <div class="clear"></div>
                    </div>
                </div>
            </div>
        </header>

        <section class="container">
            <div class="grid-12">
                <h2 class="text-bold text-center margin-top40 margin-bottom30 font28">Discover your bike</h2>
                <div class="brand-type-container">
                    <ul class="text-center">
                        <li>
                            <a href="">
                                <span class="brand-type"><span class="brandlogosprite brand-honda"></span></span>
                                <span class="brand-type-title">Honda</span>
                            </a>
                        </li>
                        <li>
                            <a href="">
                                <span class="brand-type"><span class="brandlogosprite brand-bajaj"></span></span>
                                <span class="brand-type-title">Bajaj</span>
                            </a>
                        </li>
                        <li>
                            <a href="">
                                <span class="brand-type"><span class="brandlogosprite brand-hero"></span></span>
                                <span class="brand-type-title">Hero</span>
                            </a>
                        </li>
                        <li>
                            <a href="">
                                <span class="brand-type"><span class="brandlogosprite brand-tvs"></span></span>
                                <span class="brand-type-title">TVS</span>
                            </a>
                        </li>
                        <li>
                            <a href="">
                                <span class="brand-type"><span class="brandlogosprite brand-royalenfield"></span></span>
                                <span class="brand-type-title">Royal Enfield</span>
                            </a>
                        </li>
                        <li>
                            <a href="">
                                <span class="brand-type"><span class="brandlogosprite brand-yamaha"></span></span>
                                <span class="brand-type-title">Yamaha</span>
                            </a>
                        </li>
                        <li>
                            <a href="">
                                <span class="brand-type"><span class="brandlogosprite brand-suzuki"></span></span>
                                <span class="brand-type-title">Suzuki</span>
                            </a>
                        </li>
                        <li>
                            <a href="">
                                <span class="brand-type"><span class="brandlogosprite brand-ktm"></span></span>
                                <span class="brand-type-title">KTM</span>
                            </a>
                        </li>
                        <li>
                            <a href="">
                                <span class="brand-type"><span class="brandlogosprite brand-mahindra"></span></span>
                                <span class="brand-type-title">Mahindra</span>
                            </a>
                        </li>
                        <li>
                            <a href="">
                                <span class="brand-type"><span class="brandlogosprite brand-harleydavidson"></span></span>
                                <span class="brand-type-title">Harley Davidson</span>
                            </a>
                        </li>                     
                    </ul>
                    <div class="brand-bottom-border border-solid-top margin-left20 margin-right20 hide"></div>
                    <ul class="brand-style-moreBtn padding-top25 brandTypeMore hide margin-left5">             
                    <li>
                        <a href="">
                            <span class="brand-type"><span class="brandlogosprite brand-aprilia"></span></span>
                            <span class="brand-type-title">Aprilia</span>
                        </a>
                    </li>               
                    <li>
                        <a href="">
                            <span class="brand-type"><span class="brandlogosprite brand-benelli"></span></span>
                            <span class="brand-type-title">Benelli</span>
                        </a>
                    </li>            
                    <li>
                        <a href="">
                            <span class="brand-type"><span class="brandlogosprite brand-bmw"></span></span>
                            <span class="brand-type-title">BMW</span>
                        </a>
                    </li>            
                    <li>
                        <a href="">
                            <span class="brand-type"><span class="brandlogosprite brand-ducati"></span></span>
                            <span class="brand-type-title">Ducati</span>
                        </a>
                    </li>            
                    <li>
                        <a href="">
                            <span class="brand-type"><span class="brandlogosprite brand-heroelectric"></span></span>
                            <span class="brand-type-title">Hero Electric</span>
                        </a>
                    </li>                
                    <li>
                        <a href="">
                            <span class="brand-type"><span class="brandlogosprite brand-hyosung"></span></span>
                            <span class="brand-type-title">Hyosung</span>
                        </a>
                    </li>                 
                    <li>
                        <a href="">
                            <span class="brand-type"><span class="brandlogosprite brand-indian"></span></span>
                            <span class="brand-type-title">Indian</span>
                        </a>
                    </li>                
                    <li>
                        <a href="">
                            <span class="brand-type"><span class="brandlogosprite brand-kawasaki"></span></span>
                            <span class="brand-type-title">Kawasaki</span>
                        </a>
                    </li>                
                    <li>
                        <a href="">
                            <span class="brand-type"><span class="brandlogosprite brand-lml"></span></span>
                            <span class="brand-type-title">LML</span>
                        </a>
                    </li>                
                    <li>
                        <a href="">
                            <span class="brand-type"><span class="brandlogosprite brand-motoguzzi"></span></span>
                            <span class="brand-type-title">Moto Guzzi</span>
                        </a>
                    </li>                
                    <li>
                        <a href="">
                            <span class="brand-type"><span class="brandlogosprite brand-mv-agusta"></span></span>
                            <span class="brand-type-title">MV Agusta</span>
                        </a>
                    </li>               
                    <li>
                        <a href="">
                            <span class="brand-type"><span class="brandlogosprite brand-triumph"></span></span>
                            <span class="brand-type-title">Triumph</span>
                        </a>
                    </li>                
                    <li>
                        <a href="">
                            <span class="brand-type"><span class="brandlogosprite brand-um"></span></span>
                            <span class="brand-type-title">UM</span>
                        </a>
                    </li>                
                    <li>
                        <a href="">
                            <span class="brand-type"><span class="brandlogosprite brand-vespa"></span></span>
                            <span class="brand-type-title">Vespa</span>
                        </a>
                    </li>                
                    <li>
                        <a href="">
                            <span class="brand-type"><span class="brandlogosprite brand-yo"></span></span>
                            <span class="brand-type-title">Yo</span>
                        </a>
                    </li>                   
                </ul>
                </div>
                <div class="view-brandType text-center padding-bottom30">
                    <a href="javascript:void(0)" id="view-brandType" class="view-more-btn font16">View <span>more</span> brands</a>
                </div>
            </div>
            <div class="clear"></div>
        </section>

        <!-- #include file="/includes/footerBW.aspx" -->
        <!-- #include file="/includes/footerscript.aspx" -->
        <script type="text/javascript">
            $(window).on("scroll", function () {
                if ($(window).scrollTop() > 40)
                    $('#header').removeClass("header-landing").addClass("header-fixed");
                else
                    $('#header').removeClass("header-fixed").addClass("header-landing");
            });
            $("a.view-more-btn").click(function (e) {
                moreBrandList = $(this).parent().parent().find("ul.brand-style-moreBtn"),
                moreText = $(this).find("span"),
                borderDivider = $(".brand-bottom-border");
                moreBrandList.slideToggle();
                moreText.text(moreText.text() === "more" ? "less" : "more");
                borderDivider.slideToggle();
            });
        </script>

        <!--
        <div class="container_12">
            <div class="grid_12">
                <ul class="breadcrumb">
                    <li>You are here: </li>
                    <li  itemscope="" itemtype="http://data-vocabulary.org/Breadcrumb">
                        <a href="/" itemprop="url">
                            <span itemprop="title">Home</span>
                        </a>
                    </li>
                    <li class="fwd-arrow">&rsaquo;</li>
                    <li itemscope="" itemtype="http://data-vocabulary.org/Breadcrumb">
                        <a href="/new/" itemprop="url">
                            <span itemprop="title">New Bikes</span>
                        </a>
                    </li>
                    <li class="fwd-arrow">&rsaquo;</li>
                    <li class="current"><strong>New Bike Dealers / Showrooms in India</strong></li>
                </ul>
                <div class="clear"></div>
            </div>
            <div class="grid_8 margin-top10">
        
                <h1>New Bike Dealers / Showrooms in India</h1>
                <p class="padding-top10">Find new bike dealers and authorized showrooms in India. New bike dealer information for more than 200 cities is available. Click on a bike manufacturer name to get the list of its authorized dealers in India.</p>
                <div class="margin-top15">
                    <h2>Search Dealers by City & Manufacturer</h2>
                    <div class="margin-top10 content-block grey-bg">
                        <span>
                            <asp:dropdownlist id="cmbMake" runat="server" />
                        </span>
                        <span class="margin-left10 margin-right20">
                            <asp:dropdownlist id="cmbCity" enabled="false" runat="server">
					                <asp:ListItem Text="--Select City--" Value="-1" />
				                </asp:dropdownlist>
                        </span>
                        <input type="hidden" id="hdn_drpCity" runat="server" />
                        <span class="action-btn"><a onclick="javascript:locateDealer(this)">Locate Dealers</a></span>
                    </div>
                </div>
                <div class="margin-top15">
                    <h2>Browse Dealers by Manufacturer</h2>
                    <div class="margin-top5">
                        <asp:datalist id="dlShowMakes" runat="server" repeatcolumns="3" width="100%" repeatdirection="Horizontal" cellspacing="2" cellpadding="2">
				            <itemtemplate><a href="/new/<%# DataBinder.Eval(Container.DataItem, "MaskingName").ToString()%>-dealers/">					
						            <%# DataBinder.Eval(Container.DataItem, "BikeMake")%> Dealers</a> <span class="text-grey">(<%# DataBinder.Eval(Container.DataItem, "TotalCount")%>)</span><br>						
					            </itemtemplate>					 
			            </asp:datalist>
                    </div>
                </div>
            </div>
        </div>
        -->
        <script type="text/javascript">
        /*
        $(document).ready(function () {
            if ($("#cmbMake").val().split('_')[0] > 0) {
                FillCity();
            }
        });

        $("#cmbMake").change(function () {
            FillCity();
        });

        function FillCity() {
            var makeId = $("#cmbMake").val().split('_')[0];
            //alert(cityId);
            if (makeId != "0") {
                //alert(cityId);
                $.ajax({
                    type: "POST",
                    url: "/ajaxpro/Bikewale.Ajax.AjaxCommon,Bikewale.ashx",
                    data: '{"makeId":"' + makeId + '"}',
                    beforeSend: function (xhr) { xhr.setRequestHeader("X-AjaxPro-Method", "GetDealersCitiesListByMakeId"); },
                    success: function (response) {
                        var responseJSON = eval('(' + response + ')');
                        var resObj = eval('(' + responseJSON.value + ')');

                        var dependentCmbs = new Array();
                        bindDropDownList(resObj, $("#cmbCity"), "hdn_drpCity", dependentCmbs, "--Select City--");
                    }
                });
            } else {
                $("#cmbCity").val(0).attr("disabled", true);
            }
        }

        function locateDealer(e) {
            var cityId = $("#cmbCity").val().split('_')[0];
            var makeValueArray = $("#cmbMake").val();
            var makeId = makeValueArray.split('_')[0];
            var city = $("#cmbCity option:selected").val().split('_')[1];
            //var make = replaceAll(replaceAll($("#cmbMake option:selected").text(), " ", ""), "-", "");
            var make = makeValueArray.split('_')[1];
            if (Number(cityId) <= 0 && Number(makeId) <= 0) {
                alert("Please select city and make to locate dealers.");    //change the message
                return false;
            }
            else if (Number(cityId) <= 0) {
                alert("Please select city to locate dealers.");
                return false;
            }
            else if (Number(makeId) <= 0) {
                alert("Please select make to locate dealers.");
                return false;
            }
            //alert(Bikewale.Common.UrlRewrite.FormatSpecial(city));
            //FormatSpecial(city);
            //alert(city);
            //alert(city.toLowerCase());
            location.href = "/new/" + make.toLowerCase() + "-dealers/" + cityId + "-" + city + ".html";
        }

        function replaceAll(str, rep, repWith) {
            var occurrence = str.indexOf(rep);

            while (occurrence != -1) {
                str = str.replace(rep, repWith);
                occurrence = str.indexOf(rep);
            }
            return str;
        }
        */
    </script>
    
    </form>
</body>
</html>