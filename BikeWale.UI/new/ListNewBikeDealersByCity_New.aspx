<%@ Page Language="C#" AutoEventWireup="false" Inherits="Bikewale.New.ListNewBikeDealersByCity_New" EnableViewState="false" Trace="false" Debug="false" %>
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
    <link href="<%= staticUrl != "" ? "http://st2.aeplcdn.com" + staticUrl : "" %>/css/dealersbylocation.css?<%= staticFileVersion%>" rel="stylesheet" type="text/css">
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
                        <li><span class="bwsprite fa fa-angle-right margin-right10"></span>Dealer Locator</li>
                    </ul>
                    <div class="clear"></div>
                </div>
            </div>
        </section>

        <section>
            <div class="grid-12 alpha omega">
                <div id="listingSidebar" class="bg-white position-abt pos-right0">
                    <div id="listingSidebarHeading" class="padding-top15 padding-right20 padding-left20">
                        <div class="margin-bottom20">
                            <h1 id="sidebarHeader" class="font16 margin-bottom10">Royal Enfield dealers in Maharashtra</h1>
                            <h2 class="text-unbold font14 text-xt-light-grey border-solid-bottom padding-bottom15">25 dealers across 40 cities in Maharashtra</h2>
                        </div>
                        <div class="form-control-box">
                            <span class="bwsprite search-icon-grey"></span>
                            <input type="text" class="form-control padding-right40" placeholder="Type to search city" id="getCityInput" />
                            <span class="fa fa-spinner fa-spin position-abt text-black"></span>
                            <span class="bwsprite error-icon errorIcon"></span>
                            <div class="bw-blackbg-tooltip errorText"></div>
                        </div>
                        <div class="padding-top10 padding-bottom10">
                            <a href="#" class="inline-block"><span class="bwsprite back-icon"></span></a>
                            <span class="font16 text-black inline-block">Maharashtra</span>
                        </div>
                    </div>
                    <ul id="listingSidebarList" class="city-sidebar-list">
                        <li>
                            <a href="http://webserver:9011/tvs-bikes/wego/" data-item-id="90">Mumbai (21)</a>
                        </li>
                        <li>
                            <a href="#" data-item-id="91">Panvel (15)</a>
                        </li>
                        <li>
                            <a href="#" data-item-id="92">Chiplun (9)</a>
                        </li>
                        <li>
                            <a href="#" data-item-id="93">Solapur (4)</a>
                        </li>
                    </ul>
                </div>
            </div>
            <div class="clear"></div>
        </section>

        <section class="bg-white">
            <div class="grid-12 alpha omega">
                <div class="dealer-map-wrapper">
                    <div id="dealersMapWrapper" style="position: fixed; top: 50px; width: 100%; height: 530px;">
                        <div id="dealersMap" style="width: 100%; height: 530px;">
                        </div>
                    </div>
                </div>
            </div>
            <div class="clear"></div>
        </section>

        <!-- #include file="/includes/footerBW.aspx" -->
        <!-- #include file="/includes/footerscript.aspx" -->
        <script type="text/javascript">
            var dealersByCity = true;
        </script>
        <script type="text/javascript" src="<%= staticUrl != "" ? "http://st2.aeplcdn.com" + staticUrl : "" %>/src/dealersbylocation.js?<%= staticFileVersion %>"></script>
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