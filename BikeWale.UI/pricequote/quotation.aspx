<%@ Page Language="C#" AutoEventWireup="false" Inherits="Bikewale.PriceQuote.Quotation" Trace="false" Debug="false" %>
<%@ Register TagPrefix="uc" TagName="UserReviewsMin" Src="~/controls/UserReviewsMin.ascx" %>
<%@ Register TagPrefix="uc" TagName="UpcomingBikes" Src="~/controls/UpcomingBikesMin.ascx" %>
<%@ Register TagPrefix="LD" TagName="LocateDealer" Src="/controls/locatedealer.ascx" %>
<%@ Register TagPrefix="SB" TagName="SimilarBike" Src="~/controls/SimilarBikes.ascx" %>
<%@ Import Namespace="Bikewale.Common" %>
<%
    title = "Instant Free New Bike Price Quote";
    description = "Bikewale.com New bike free price quote.";
    ShowTargeting = "1";
    TargetedModel = mmv.Model;
    AdId = "1395986297721";
    AdPath = "/1017752/Bikewale_PriceQuote_";
%>
<!-- #include file="/includes/headNew.aspx" -->
<script type="text/javascript" src="/src/pq/price_quote.js?v=1.1"></script>
<%--<link rel="stylesheet"  href="<%= !String.IsNullOrEmpty(staticUrl) ? "http://st2.aeplcdn.com" + staticUrl : "" %>/css/bw-pq.css" />--%>
<link href="/css/bw-pq.css?<%= staticFileVersion %>" rel="stylesheet" />

 <%@ Register TagPrefix="PW" TagName="PopupWidget" Src="/controls/PopupWidget.ascx" %>
<PW:PopupWidget runat="server" ID="PopupWidget" />

<div class="main-container">
	<div class="container_12">
        <div class="grid_12">
            <ul class="breadcrumb">
                <li>You are here: </li>
                <li><a class="blue" href="/">Home</a></li>
                <li class="fwd-arrow">&rsaquo;</li>
                <li><a class="blue" href="/new/">New</a></li>
                <li class="fwd-arrow">&rsaquo;</li>
                <li><a class="blue" href="/pricequote/">On-Road Price Quote</a></li>
                <li class="fwd-arrow">&rsaquo;</li>
                <li class="current"><strong><%= mmv.BikeName %></strong></li>
            </ul><div class="clear"></div>
        </div>   
    	<div class="grid_8 margin-top10">
        	<h1 class="margin-bottom5">On Road Price Quote - <%= mmv.BikeName %></h1>
            <div class="padding5 margin-bottom10 <%=(versionList.Count > 1)?"":"hide" %>">
                Variants: <asp:DropDownList id="ddlVersion" runat="server" AutoPostBack="true"></asp:DropDownList>
            </div>
            <div id="get-pq-new" class="inner-content">
            	<div id="div_ShowPQ">
                    <table cellspacing="0" cellpadding="0" width="100%" border="0" class="tbl-default margin-top10">
                        <tr>
                            <td style="width:100px;vertical-align:top;border-right:1px solid #E5E5E5;">
                                 <div class="show-pq-pic">
                                    <img alt=" <%= mmv.BikeName %> Photos" src="<%=imgPath%>" title="<%= mmv.BikeName %> Photos">
                                 </div>
                            </td>
                            <%if (objQuotation != null && objQuotation.ExShowroomPrice > 0)
                              {%>
                            <td valign="top" style="padding-left:20px;">
                                <table>
                                    <%--<h2><%= mmv.BikeName %></h2>--%>
                                    <tr>
                                        <td width="370">
                                             Ex-Showroom (<%= objQuotation.City %>)
                                        </td>
                                        <td width="100" class="numeri-cell" align="right"><span id="exShowroomPrice"><%= CommonOpn.FormatNumeric( objQuotation.ExShowroomPrice.ToString() ) %></span></td>
                                    </tr>
                                    <tr>
                                        <td>RTO</td>
                                        <td width="100" class="numeri-cell" align="right"><%= CommonOpn.FormatNumeric( objQuotation.RTO.ToString() ) %></td>
                                    </tr>
                                    <tr>
                                        <td>Insurance (Comprehensive)</td>
                                        <td width="100" class="numeri-cell" align="right"><%= CommonOpn.FormatNumeric(  objQuotation.Insurance.ToString()  ) %></td>
                                    </tr>
                                    <tr><td colspan="2"><div class="dotted-hr"></div><td></tr>
                                    <tr>
                                        <td class="price2">Total On Road Price</td>
                                        <td width="100" class="price2 numeri-cell" align="right"><span class="WebRupee">Rs.</span><%= CommonOpn.FormatNumeric( objQuotation.OnRoadPrice.ToString()  ) %></td>
                                    </tr>
                                    <%--<tr>
                                        <td colspan="3">Bike available with Zero Dep insurance for <span class="WebRupee">Rs.</span>1,33,000</td>
                                    </tr>	--%>	
                              </table>
                            </td>
                            <%}else{ %>
                                <td style="vertical-align:central">
                                    <div id="div_ShowErrorMsg"  class="grey-bg border-light content-block text-highlight margin-top15" style="background:#fef5e6;">Price for this bike is not available in this city.</div>   
                                </td>
                             <%} %>
                        </tr>
                    </table>
            	</div>
            </div>
            <div class="grid_4 alpha omega">
                <SB:SimilarBike ID="ctrl_similarBikes" TopCount="2" runat="server" />
            </div>
            <div class="grid_4 omega ">
                <uc:UpcomingBikes ID="ucUpcoming" TopRecords="2" runat="server" />
            </div>
        </div>

       <%-- <div id="div_ShowErrorMsg" runat="server" class="grey-bg border-light content-block text-highlight margin-top15"></div>--%>
        
        <div class="grid_4">
            <div class="margin-top15 margin-bottom20">
                <!-- BikeWale_NewBike/BikeWale_NewBike_HP_300x250 -->
                <!-- #include file="/ads/Ad300x250.aspx" -->
            </div>
            <div class="inner-content">
                <LD:LocateDealer ID="ucLocateDealer" runat="server" />
            </div>
        </div>
    </div>
</div>
    
<script type="text/javascript">
    $(document).ready(function () {
        makeMapName = '<%= mmv.MakeMappingName%>';
        modelMapName = '<%= mmv.ModelMappingName%>';
        $("#version_" + '<%= versionId%>').html("<b>this bike</b>");
    });

    function compareSelected()
    {        
        var compareURL = "/comparebikes/";
        var objSelCompare = $("#tblAllVersions :checkbox:checked");
        var i = 1;

        if (objSelCompare.length < 2) {
            alert("Please select atleast two versions to compare");
        } else if (objSelCompare.length > 4) {
            alert("You can select upto 4 versions for comparison");
        } else {
            var compareQS = "?";            
            objSelCompare.each(function () {
                if (i == 1) {
                    compareURL += makeMapName + "-" + modelMapName;
                    compareQS += "bike" + i + "=" + $(this).val();
                } else {
                    compareURL += "-vs-" + makeMapName + "-" + modelMapName;
                    compareQS += "&bike" + i + "=" + $(this).val();
                }
                i++;
            });
            compareURL += "/" + compareQS;

            window.open(compareURL, "", "", "");
        }
    }
</script>
<!-- #include file="/includes/footerInner.aspx" -->
