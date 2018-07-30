<%@ Page Language="C#" AutoEventWireup="false" Inherits="Bikewale.New.ListNewBikeDealersByCity" Trace="false" Debug="false" %>

<%@ Register TagPrefix="NBL" TagName="NewBikeLaunches" Src="/controls/NewBikeLaunches.ascx" %>
<%@ Register TagPrefix="TIP" TagName="TipsAdvicesMin" Src="/controls/TipsAdvicesMin.ascx" %>
<%@ Register TagPrefix="FM" TagName="ForumsMin" Src="/controls/forumsmin.ascx" %>
<%@ Import Namespace="Bikewale.Common" %>
<% 
    description = objMMV.Make + " bike dealers/showrooms in India. Find new bike dealer information for more than 200 cities. Dealer information includes full address, phone numbers, email, pin code etc.";
    keywords = objMMV.Make + " bike dealers, " + objMMV.Make + " bike showrooms," + objMMV.Make + " dealers, " + objMMV.Make + " showrooms, " + objMMV.Make + " dealerships, dealerships, test drive";
    title = objMMV.Make + " Bike Dealers | " + objMMV.Make + " Bike Showrooms in India - BikeWale";
    canonical = "https://www.bikewale.com/new/" + objMMV.MakeMappingName + "-dealers/";
    AdId = "1395986297721";
    AdPath = "/1017752/BikeWale_New_";
%>
<!-- #include file="/includes/headNew.aspx" -->

<style type="text/css">
    .dvShrink {display: none;}
    .dvDisplay {display: block;padding-left: 10px;}
    .blue-imgblock {padding: 5px;border: 1px solid #DBDBDB;cursor: pointer;}
    .insert-after {width: 613px;background-color: #fff;border: 1px solid #DBDBDB;}
    .sel-state {background-color: #fff;z-index: 2;position: relative;height: 3px;width: 209px;top: 2px;font-size: 0;}
    .unsel-state {padding: 0;margin: 0;}
    .state-tab {float: left;width: 201px;padding: 0;margin-top: 3px;margin-right: 3px;border: 1px solid #DBDBDB;background-color: #f3f2f2;margin-bottom: 0;}
    .state-tab .hd3 {cursor: pointer;}
    .mar-bot {margin-bottom: 5px;}
    .expnd-cont {margin: 0;}
</style>
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
                <a href="/new-bikes-in-india/" itemprop="url"><span itemprop="title">New Bikes</span></a>
            </li>
            <li class="fwd-arrow">&rsaquo;</li>
            <li itemscope="" itemtype="http://data-vocabulary.org/Breadcrumb">
                <a itemprop="url" href="/dealer-showrooms/"><span itemprop="title">New Bike Dealers</span></a>
            </li>
            <li class="fwd-arrow">&rsaquo;</li>
            <li class="current"><strong><%=objMMV.Make%> Bikes Dealers/Showrooms</strong></li>
        </ul>
        <div class="clear"></div>
    </div>
    <div class="grid_8  margin-top10">
        <!--    Left Container starts here -->
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
								            <a href="/dealer-showrooms/<%=objMMV.MakeMappingName%>/<%# DataBinder.Eval(Container.DataItem, "CityMaskingName").ToString().ToLower() %>/"><%# DataBinder.Eval(Container.DataItem, "City")%> (<%# DataBinder.Eval(Container.DataItem, "TotalBranches")%>)<br></a>
							            </itemtemplate>
						                </asp:DataList>
					            </div>
					            <div id="merge-sel" class="unsel-state"></div>											
				            </div>
			            </itemtemplate>			
		            </asp:repeater>
        </div>
        <div class="clear"></div>
    </div>
    <!--    Left Container ends here -->
    <div class="grid_4">
        <!--    Right Container starts here -->
        <div class="margin-top5">
            <!-- BikeWale_NewBike/BikeWale_NewBike_HP_300x250 -->
            <!-- #include file="/ads/Ad300x250.aspx" -->
        </div>
        <div class="light-grey-bg content-block border-radius5 padding-bottom20 margin-top15">
            <NBL:NewBikeLaunches ID="ctrl_NewBikeLaunches" TopCount="3" runat="server" />
            <div class="clear"></div>
        </div>
    </div>
    <!--    Right Container ends here -->
</div>

<script type="text/javascript">
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
<!-- #include file="/includes/footerInner.aspx" -->
