<%@ Page Language="C#" AutoEventWireup="false" Inherits="Bikewale.New.ListNewBikeDealersByCity_New" Trace="false" Debug="false" %>
<%@ Register TagPrefix="NBL" TagName="NewBikeLaunches" Src="/controls/NewBikeLaunches.ascx" %>
<%@ Register TagPrefix="TIP" TagName="TipsAdvicesMin" Src="/controls/TipsAdvicesMin.ascx" %>
<%@ Register TagPrefix="FM" TagName="ForumsMin" Src="/controls/forumsmin.ascx" %>
<%@ Import Namespace="Bikewale.Common" %>

<!doctype html>
<html>
<head>
    <% 
        description = objMMV.Make + " bike dealers/showrooms in India. Find new bike dealer information for more than 200 cities. Dealer information includes full address, phone numbers, email, pin code etc.";
        keywords = objMMV.Make + " bike dealers, " + objMMV.Make + " bike showrooms," + objMMV.Make + " dealers, " + objMMV.Make + " showrooms, " + objMMV.Make + " dealerships, dealerships, test drive";
        title = objMMV.Make + " Bike Dealers | " + objMMV.Make + " Bike Showrooms in India - BikeWale";
        canonical = "http://www.bikewale.com/new/" + objMMV.MakeMappingName + "-dealers/";
        AdId = "1395986297721";
        AdPath = "/1017752/BikeWale_New_";
    %>
    <!-- #include file="/includes/headscript.aspx" -->
    <style type="text/css">
        .header-fixed { z-index:5 !important; }
        body { overflow-x:hidden; }
        .opacity0 { opacity:0; -ms-filter: "progid:DXImageTransform.Microsoft.Alpha(Opacity=0)"; filter: alpha(opacity=0); -moz-opacity: 0; -khtml-opacity: 0; height:0; }
        .dealer-city-map-wrapper { width:100%;height:580px;display:block; }
        #cityListingSidebar { width:338px; min-height:590px; border:1px solid #f1f1f1; background:#fff; z-index:3; }
        .citySidebarHeading { width:338px; position:fixed; top:51px; background:#fff; z-index:3; }
        .citySidebarHeading h1 { color:#000; }
        #citySidebarList, #filteredCityList { width:100%; min-height:540px; background:#fff; padding:110px 20px 0; z-index:1;}
        #filteredCityList { display:none; }
        #citySidebarList > li, #filteredCityList > li { padding-top:15px; padding-bottom:15px; border-top:1px solid #f1f1f1; }
        #citySidebarList > li:first-child, #filteredCityList > li:first-child { padding-top:10px; border-top:0; }
        #citySidebarList > li h3 {font-size:16px; font-weight:400; cursor:pointer;}
        #citySidebarList > li h3:hover { color:#2a2a2a; }
        .citySidebarHeading span.search-icon-grey {position: absolute;right:10px;top:10px;cursor: pointer;z-index:2; }
        .citySidebarHeading span.fa-spinner {display:none;right:14px;top:12px;background:#fff}
        #cityListingSidebar .form-control { border-radius:0; }
        .citySubList { display:none; }
        .citySubList li { margin-top:15px; padding-left:5px; }
        .citySubList a { font-size:16px; color:#82888b; }
        .citySubList a:hover, #filteredCityList > li a:hover { text-decoration:none; color:#4d5057; }
        .gm-style-iw + div {display: none;}
        .labels {color: #4d5057; background-color: white;font-size:16px;font-weight: bold;text-align: center;width:54px;height:54px;display:block;border-radius:50%;white-space: nowrap;top:-26px;left:-26px;padding-top:12px;border:2px solid #4d5057;}
        .labels:hover { border: 2px solid #ef3f30; }
        .label-hover { border: 2px solid #ef3f30 !important; }
        #filteredCityList > li a { font-size:16px; color:#4d5057; }
        #filteredCityList > li a:hover { color:#2a2a2a; }
    </style>
    <script src="http://maps.googleapis.com/maps/api/js?key=AIzaSyDjG8tpNdQI86DH__-woOokTaknrDQkMC8" type="text/javascript"></script>    
    <script type="text/javascript" src="/src/new/markerwithlabel.js"></script>
</head>
<body class="bg-light-grey padding-top50">
    <form runat="server">
        <!-- #include file="/includes/headBW.aspx" -->
        <section>
            <div class="opacity0 grid-12 padding-right20 padding-left20">
                <div class="breadcrumb">
                    <ul>
                        <li itemscope="" itemtype="http://data-vocabulary.org/Breadcrumb">
                            <a href="/" itemprop="url">
                                <span itemprop="title">Home</span>
                            </a>
                        </li>
                        <li><span class="fa fa-angle-right margin-right10"></span>Dealer Locator</li>
                    </ul>
                    <div class="clear"></div>
                </div>
            </div>
        </section>

        <section>
            <div class="grid-12 alpha omega">
                <div id="cityListingSidebar" class="bg-white position-abt pos-right0">
                    <div class="citySidebarHeading padding-top15 padding-right20 padding-left20">
                        <h1 id="sidebarHeader" class="font16 border-solid-bottom padding-bottom15 margin-bottom10">Bajaj dealers in India</h1>
                        <div class="padding-bottom10 form-control-box">
                            <span class="bwsprite search-icon-grey"></span>
                            <input type="text" class="form-control padding-right40" placeholder="Type to search city" id="getCityInput" />
                            <span class="fa fa-spinner fa-spin position-abt text-black"></span>
                        </div>
                    </div>
                    <ul id="citySidebarList">
                        <li>
                            <h3 data-state-id="1" data-state-lat="16.5000" data-state-long="80.6400">Andhra Pradesh</h3>
                            <ul class="citySubList">
                                <li data-city-id="50">
                                    <a href="#"><span class="city-name">Adilabad</span> <span>(<span class="city-dealer-count">8</span>)</span></a>
                                </li>
                                <li data-city-id="51">
                                    <a href="#"><span class="city-name">Chirala</span> <span>(<span class="city-dealer-count">9</span>)</span></a>
                                </li>
                                <li data-city-id="52">
                                    <a href="#"><span class="city-name">Ponnur</span> <span>(<span class="city-dealer-count">20</span>)</span></a>
                                </li>
                                <li data-city-id="53">
                                    <a href="#"><span class="city-name">Vizianagaram</span> <span>(<span class="city-dealer-count">14</span>)</span></a>
                                </li>
                            </ul>
                        </li>
                        <li>
                            <h3 data-state-id="2" data-state-lat="27.0600" data-state-long="93.3700">Arunachal Pradesh</h3>
                            <ul class="citySubList">
                                <li data-city-id="60">
                                    <a href="#"><span class="city-name">Naharlagun</span> <span>(<span class="city-dealer-count">24</span>)</span></a>
                                </li>
                            </ul>
                        </li>
                        <li>
                            <h3 data-state-id="3" data-state-lat="30.7500" data-state-long="76.7800">Gujarat</h3>
                            <ul class="citySubList">
                                <li data-city-id="70">
                                    <a href="#"><span class="city-name">Ahmedabad</span> <span>(<span class="city-dealer-count">4</span>)</span></a>
                                </li>
                                <li data-city-id="71">
                                    <a href="#"><span class="city-name">Patan</span> <span>(<span class="city-dealer-count">12</span>)</span></a>
                                </li>
                                <li data-city-id="72">
                                    <a href="#"><span class="city-name">Vadodara</span> <span>(<span class="city-dealer-count">18</span>)</span></a>
                                </li>
                                <li data-city-id="73">
                                    <a href="#"><span class="city-name">Kutch</span> <span>(<span class="city-dealer-count">22</span>)</span></a>
                                </li>
                            </ul>
                        </li>
                        <li>
                            <h3 data-state-id="4" data-state-lat="15.4989" data-state-long="73.8278">Goa</h3>
                            <ul class="citySubList">
                                <li data-city-id="80">
                                    <a href="#"><span class="city-name">Ponda</span> <span>(<span class="city-dealer-count">9</span>)</span></a>
                                </li>
                                <li data-city-id="81">
                                    <a href="#"><span class="city-name">Mapusa</span> <span>(<span class="city-dealer-count">4</span>)</span></a>
                                </li>
                            </ul>
                        </li>
                        <li>
                            <h3 data-state-id="5" data-state-lat="18.9600" data-state-long="72.8200">Maharashtra</h3>
                            <ul class="citySubList">
                                <li data-city-id="90">
                                    <a href="#"><span class="city-name">Mumbai</span> <span>(<span class="city-dealer-count">21</span>)</span></a>
                                </li>
                                <li data-city-id="91">
                                    <a href="#"><span class="city-name">Panvel</span> <span>(<span class="city-dealer-count">15</span>)</span></a>
                                </li>
                                <li data-city-id="92">
                                    <a href="#"><span class="city-name">Chiplun</span> <span>(<span class="city-dealer-count">9</span>)</span></a>
                                </li>
                                <li data-city-id="93">
                                    <a href="#"><span class="city-name">Solapur</span> <span>(<span class="city-dealer-count">4</span>)</span></a>
                                </li>
                            </ul>
                        </li>
                    </ul>
                    <ul id="filteredCityList">

                    </ul>
                </div>
            </div>
            <div class="clear"></div>
        </section>

        <section class="bg-white">
            <div class="grid-12 alpha omega">
                <div class="dealer-city-map-wrapper">
                    <div id="dealerCityMapWrapper" style="position: fixed; top: 50px; width: 100%; height: 530px;">
                        <div id="dealersCityMap" style="width: 100%; height: 530px;">
                        </div>
                    </div>
                </div>
            </div>
            <div class="clear"></div>
        </section>

        <!-- #include file="/includes/footerBW.aspx" -->
        <!-- #include file="/includes/footerscript.aspx" -->
        <script type="text/javascript" src="<%= staticUrl != "" ? "http://st2.aeplcdn.com" + staticUrl : "" %>/src/dealersbycity.js?<%= staticFileVersion %>"></script>

        <!--
    <div class="container_12">
        <div class="grid_12">
            <ul class="breadcrumb">
                <li>You are here: </li>
                <li itemscope="" itemtype="http://data-vocabulary.org/Breadcrumb">
                    <a href="/" itemprop="url">
                        <span itemprop="title">Home</span>
                    </a>
                </li>
                <li class="fwd-arrow">&rsaquo;</li>
                <li itemscope="" itemtype="http://data-vocabulary.org/Breadcrumb">
                    <a href="/new/" itemprop="url"><span itemprop="title">New Bikes</span></a>
                </li>
                <li class="fwd-arrow">&rsaquo;</li>
                <li itemscope="" itemtype="http://data-vocabulary.org/Breadcrumb">
                    <a itemprop="url" href="/new/locate-dealers/"><span itemprop="title">New Bike Dealers</span></a>
                </li>
                <li class="fwd-arrow">&rsaquo;</li>
                <li class="current"><strong><%=objMMV.Make%> Dealers/Showrooms</strong></li>
            </ul>
            <div class="clear"></div>
        </div>
        <div class="grid_8  margin-top10">
            
            <h1><%=objMMV.Make%> Dealers/Showrooms in India</h1>
            <div class="margin-top5"><%=objMMV.Make%> sells bikes through <%=DealerCount%> dealerships in <%=StateCount%> cities across India.</div>
            <div id="stateTabs" style="border: 1px solid #fff;" class="margin-top20">
                <asp:repeater id="rptState" runat="server">
			                <itemtemplate>
				                <div class="state-tab" id='dv-<%# DataBinder.Eval(Container.DataItem, "StateId")%>' onclick="javascript:showCity('<%# DataBinder.Eval(Container.DataItem, "StateId")%>', this);">
					                <h3 class="hd3" style="margin:5px; padding:0;"><span class="icon-sheet right-arrow-bg"></span><span><%# DataBinder.Eval(Container.DataItem, "State")%></span></h3>
					                <div id='tbl-<%# DataBinder.Eval(Container.DataItem, "StateId")%>' class="dvShrink" >
						                <asp:DataList ID="dlCity" runat="server" RepeatColumns="3" RepeatDirection="Horizontal" Width="100%" 
							                CellSpacing="10" CellPadding="0" DataSource='<%# BindCities(int.Parse(DataBinder.Eval(Container.DataItem, "StateId").ToString())) %>'>
							                <itemtemplate>
								                <a href="/new/<%=objMMV.MakeMappingName%>-dealers/<%# DataBinder.Eval(Container.DataItem, "CityId")%>-<%# DataBinder.Eval(Container.DataItem, "CityMaskingName").ToString().ToLower() %>.html"><%# DataBinder.Eval(Container.DataItem, "City")%> (<%# DataBinder.Eval(Container.DataItem, "TotalBranches")%>)<br></a>
							                </itemtemplate>
						                    </asp:DataList>
					                </div>
					                <div id="merge-sel" class="unsel-state"></div>											
				                </div>
			                </itemtemplate>			
		                </asp:repeater>
            </div>
            <div class="clear"></div>

            <%--<div class="grid_4 alpha margin-top20">
                        <div class="grey-bg content-block"><TIP:TipsAdvicesMin ID="ctrl_TipsAdvices" runat="server" TopRecords="10"/></div>
                    </div>
                    <div class="grid_4 omega margin-top20 grey-bg">
                        <div class="content-block">
                             <FM:ForumsMin runat="server" ID="ForumsMin" TopRecords="10" />
                        </div>
                    </div><div class="clear"></div>--%>
        </div>
        
        <div class="grid_4">
            
            <div class="margin-top5">
                
            </div>
            <div class="light-grey-bg content-block border-radius5 padding-bottom20 margin-top15">
                <NBL:NewBikeLaunches ID="ctrl_NewBikeLaunches" TopCount="3" runat="server" />
                <div class="clear"></div>
            </div>
        </div>
        
    </div>
    -->

        <!--
    <script language="javascript">
        function showCity(stateId, obj) {
            var selTabObj = $(obj);
            var selectedState = $("#tbl-" + stateId);
            var selectedImg = $("#dv-" + stateId).find("span.icon-sheet");
            var manageArrowImgs = $("#stateTabs span.icon-sheet");

            $("div.expnd-cont").remove();
            $("div.sel-state").toggleClass('sel-state unsel-state');
            $("div.state-tab").css({ "background-color": "#f3f2f2" });

            if (manageArrowImgs.hasClass("down-arrow")) {
                manageArrowImgs.removeClass('down-arrow').addClass('right-arrow-bg');
            }

            selTabObj.find(".hd3").removeClass("mar-bot");

            var index = $("div.state-tab").index(selTabObj);
            var indexLength = $("div.state-tab").length - 1;
            var insertAfter = 3 - (index % 3) - 1;
            var indexDiff = indexLength - index;
            var exndCont = "<div class='expnd-cont'><div class='clear'></div><div class='insert-after'>" + selectedState.html() + "</div></div>";

            if (selectedState.hasClass("dvShrink")) {
                if (insertAfter == 0 || (insertAfter == 2 && index == indexLength)) {
                    $(exndCont).insertAfter(selTabObj);
                } else if (insertAfter == 1 && indexDiff >= 1) {
                    $(exndCont).insertAfter(selTabObj.next());
                } else if (insertAfter == 2 && indexDiff >= 2) {
                    $(exndCont).insertAfter(selTabObj.next().next());
                } else {
                    if (insertAfter == 1 && indexDiff < 1) {
                        $(exndCont).insertAfter(selTabObj);
                    } else if (insertAfter == 2 && indexDiff < 2) {
                        $(exndCont).insertAfter(selTabObj.next());
                    }
                }

                selTabObj.find("#merge-sel").toggleClass('unsel-state sel-state');
                selTabObj.find(".hd3").addClass("mar-bot");
                selTabObj.css({ "background-color": "#fff" });

                selectedImg.removeClass('right-arrow-bg').addClass('down-arrow');
            }
        }
    </script>
    -->
    
    </form>
</body>
</html>