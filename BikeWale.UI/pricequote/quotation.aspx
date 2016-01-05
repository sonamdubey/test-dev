<%@ Page Language="C#" AutoEventWireup="false" Inherits="Bikewale.PriceQuote.Quotation" Trace="false" Debug="false" %>
<%@ Register TagPrefix="uc" TagName="UserReviewsMin" Src="~/controls/UserReviewsMin.ascx" %>
<%@ Register TagPrefix="LD" TagName="LocateDealer" Src="/controls/locatedealer.ascx" %>
<%@ Register Src="~/controls/AlternativeBikes.ascx" TagName="AlternativeBikes" TagPrefix="BW" %>
<%@ Register Src="~/controls/UpcomingBikes_new.ascx" TagName="UpcomingBikes" TagPrefix="BW" %>
<%@ Register TagPrefix="PW" TagName="PopupWidget" Src="/controls/PopupWidget.ascx" %>
<%@ Import Namespace="Bikewale.Common" %>
<!doctype html>
<html>
<head>
<%
    title = "Instant Free New Bike Price Quote";
    description = "Bikewale.com New bike free price quote.";
    ShowTargeting = "1";
    TargetedModel = mmv.Model;
    AdId = "1395986297721";
    AdPath = "/1017752/Bikewale_PQ_";
%>
<!-- #include file="/includes/headscript.aspx" -->

<style type="text/css">
    #PQImageVariantContainer img {
        width: 100%;
    }

    .PQDetailsTableTitle {
        color: #82888b;
    }

    .PQDetailsTableAmount, .PQOnRoadPrice {
        color: #4d5057;
    }

    .PQOffersUL {
        margin-left: 18px;
        list-style: disc;
    }

        .PQOffersUL li {
            padding-bottom: 15px;
        }

    .pqVariants .form-control-box {
        width: 92%;
    }

    .form-control-box select.form-control {
        color: #4d5057;
    }

    .jcarousel-wrapper.alternatives-carousel {
        width: 974px;
    }

    .alternatives-carousel .jcarousel li {
        height: auto;
        margin-right: 18px;
    }

        .alternatives-carousel .jcarousel li.front {
            border: none;
        }

    .alternative-section .jcarousel-control-left {
        left: -24px;
    }

    .alternative-section .jcarousel-control-right {
        right: -24px;
    }

    .alternative-section .jcarousel-control-left, .alternative-section .jcarousel-control-right {
        top: 50%;
    }

    .newBikes-latest-updates-container .grid-4 {
        padding-left: 10px;
    }

    .available-colors {
        display: inline-block;
        width: 150px;
        text-align: center;
        margin-bottom: 20px;
        padding: 0 5px;
        vertical-align: top;
    }

        .available-colors .color-box {
            width: 60px;
            height: 60px;
            margin: 0 auto 15px;
            border-radius: 3px;
            background: #f00;
            border: 1px solid #ccc;
        }

    .upcoming-brand-bikes-container li.front {
        border: none;
    }

    .upcoming-brand-bikes-container li .imageWrapper {
        width: 303px;
        height: 174px;
    }

        .upcoming-brand-bikes-container li .imageWrapper a {
            width: 303px;
            height: 174px;
            display: block;
            background: url('http://img.aeplcdn.com/bikewaleimg/images/loader.gif') no-repeat center center;
        }

    .upcoming-brand-bikes-container li {
        width: 303px;
        height: auto;
        margin-right: 12px;
    }

        .upcoming-brand-bikes-container li .imageWrapper a img {
            width: 303px;
            height: 174px;
        }

    .upcoming-brand-bikes-container .jcarousel {
        width: 934px;
        overflow: hidden;
        left: 20px;
    }
</style>
</head>
<body class="bg-light-grey header-fixed-inner">
<form runat="server">
    <!-- #include file="/includes/headBW.aspx" -->	
    <section class="bg-light-grey padding-top10">
        <div class="container">
            <div class="grid-12">
                <div class="breadcrumb margin-bottom15"><!-- breadcrumb code starts here -->
                    <ul>
                        <li><a href="/">Home</a></li>
                        <li><span class="fa fa-angle-right margin-right10"></span><a href="/new/">New</a></li>
                        <li><span class="fa fa-angle-right margin-right10"></span><a href="/pricequote/">On-Road Price Quote</a></li>
                        <li><span class="fa fa-angle-right margin-right10"></span><%= mmv.BikeName %></li>
                    </ul>
                    <div class="clear"></div>
                </div>
                <h1 class="font30 text-black margin-top10 margin-bottom10">On-road price quote</h1>
                <div class="clear"></div>
            </div>
            <div class="clear"></div>
        </div>
    </section>
    
    <section class="container">
        <div class="grid-12 margin-bottom20" id="dealerPriceQuoteContainer">
            <div class="content-box-shadow content-inner-block-20 rounded-corner2">
                <div class="grid-3 alpha" id="PQImageVariantContainer">
        	        <div class="pqBikeImage margin-bottom20 margin-top5">
                        <img alt=" <%= mmv.BikeName %> Photos" src="<%=imgPath%>" title="<%= mmv.BikeName %> Photos">
                    </div>
                    <div class="pqVariants <%=(versionList.Count > 1)?"":"hide" %>">
                        <p class="font14 margin-bottom5">Versions</p>
                        <div class="form-control-box">
                            <asp:DropDownList id="ddlVersion" class="form-control" runat="server" AutoPostBack="true"></asp:DropDownList>
                        </div>
                    </div>
                </div>
                <div class="grid-5 padding-right20" id="PQDetailsContainer">
                    <p class="font20 text-bold margin-bottom20"><%= mmv.BikeName %></p>
                    <%if (objQuotation != null && objQuotation.ExShowroomPrice > 0)
                      { %>
                    <p class="font16 margin-bottom15">On-road price in 
                        <span><%= (String.IsNullOrEmpty(objQuotation.Area))?objQuotation.City:(objQuotation.Area + ", " + objQuotation.City) %></span>
                    </p>
                    <% } %>
                    <div>
                        <%if (objQuotation != null && objQuotation.ExShowroomPrice > 0)
                          {%>
                            <table class="font14" cellspacing="0" cellpadding="0" width="100%" border="0">
                                <tr>
                                    <td width:"245" class="PQDetailsTableTitle padding-bottom10">
                                        Ex-Showroom (<%= objQuotation.City %>)
                                    </td>
                                    <td align="right" class="PQDetailsTableAmount text-bold padding-bottom10">
                                        <span class="fa fa-rupee margin-right5"></span><span id="exShowroomPrice"><%= CommonOpn.FormatNumeric( objQuotation.ExShowroomPrice.ToString() ) %></span>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="PQDetailsTableTitle padding-bottom10">RTO</td>
                                    <td align="right" class="PQDetailsTableAmount text-bold padding-bottom10">
                                        <span class="fa fa-rupee margin-right5"></span><span><%= CommonOpn.FormatNumeric( objQuotation.RTO.ToString() ) %></span>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="PQDetailsTableTitle padding-bottom10">Insurance (Comprehensive)<br />
                                        <div style="position: relative; color: #999; font-size: 11px; margin-top: 1px;">Save up to 60% on insurance - <a onclick="dataLayer.push({ event: 'Bikewale_all', cat: 'BW_PQ', act: 'Insurance_Clicked',lab: '<%= (objQuotation!=null)?(objQuotation.MakeName + "_" + objQuotation.ModelName + "_" + objQuotation.VersionName + "_" + objQuotation.City):string.Empty %>' });" target="_blank" href="/insurance/">PolicyBoss</a>
                                            <span style="margin-left: 8px; vertical-align: super; font-size: 9px;">Ad</span>  
                                        </div>
                                    </td>
                                    <td align="right" class="PQDetailsTableAmount text-bold padding-bottom10">
                                        <span class="fa fa-rupee margin-right5"></span><span><%= CommonOpn.FormatNumeric(  objQuotation.Insurance.ToString()  ) %></span>
                                    </td>
                                </tr>
                                <tr><td colspan="2" class="border-solid-top padding-bottom10" align="right"></tr>
                                <tr>
                                    <td class="PQDetailsTableTitle font18 text-bold PQOnRoadPrice padding-bottom10">Total On Road Price</td>
                                    <td align="right" class="PQDetailsTableAmount padding-bottom10 font20 text-bold">
                                        <span class="fa fa-rupee margin-right5"></span><span><%= CommonOpn.FormatNumeric( objQuotation.OnRoadPrice.ToString()  ) %></span>
                                    </td>
                                </tr>	
                            </table>
                        <%}
                          else
                          { %>
                        <table class="font14" cellspacing="0" cellpadding="0" width="100%" border="0">
                            <tr>
                                <td colspan="2" style="vertical-align:central">
                                    <div id="div_ShowErrorMsg"  class="grey-bg border-light content-block text-highlight margin-top15" style="background:#fef5e6;">Price for this bike is not available in this city.</div>   
                                </td>
                            </tr>
                        </table>
                            <%} %>
                    </div>
                </div>
                <div class="grid-4 omega padding-left20 border-solid-left">
                    <LD:LocateDealer ID="ucLocateDealer" runat="server" />
                </div>
                <div class="clear"></div>
            </div>
        </div>
        <div class="clear"></div>
    </section>

    <section class="margin-bottom20 <%= hasAlternateBikes ? "" : "hide" %>">
        <div class="container">
        <div class="grid-12 alternative-section" id="alternative-bikes-section">
            <h2 class="text-bold text-center margin-top20 margin-bottom30"><%= mmv.Make + " " + mmv.Model %> alternatives</h2>
            <div class="content-box-shadow">
                <div class="jcarousel-wrapper alternatives-carousel margin-top20">
                    <div class="jcarousel">
                        <ul>
                            <BW:AlternativeBikes ID="ctrlAlternativeBikes" runat="server" />
                        </ul>
                    </div>
                    <span class="jcarousel-control-left"><a href="#" class="bwsprite jcarousel-control-prev"></a></span>
                    <span class="jcarousel-control-right"><a href="#" class="bwsprite jcarousel-control-next"></a></span>
                </div>
            </div>
        </div>
        <div class="clear"></div>
        </div>
    </section>

    <section class="margin-bottom20 <%= hasUpcomingBikes ? "" : "hide" %>">
        <!-- Upcoming bikes from brands -->
        <div class="container">
            <div class="grid-12">
                <h2 class="text-bold text-center margin-top20 margin-bottom30">Upcoming bikes from <%= mmv.Make %></h2>
                <div class="content-box-shadow rounded-corner2">
                    <div class="jcarousel-wrapper upcoming-brand-bikes-container margin-top20">
                        <div class="jcarousel">
                            <ul>
                                <BW:UpcomingBikes runat="server" ID="ctrlUpcomingBikes" />
                            </ul>
                        </div>
                        <span class="jcarousel-control-left"><a href="#" class="bwsprite jcarousel-control-prev"></a></span>
                        <span class="jcarousel-control-right"><a href="#" class="bwsprite jcarousel-control-next"></a></span>
                    </div>
                </div>
            </div>
            <div class="clear"></div>
        </div>
    </section>
<PW:PopupWidget runat="server" ID="PopupWidget" />
<!-- #include file="/includes/footerBW.aspx" -->
<!-- #include file="/includes/footerscript.aspx" -->
<script type="text/javascript">
    $(document).ready(function () {

        makeMapName = '<%= mmv.MakeMappingName%>';
        modelMapName = '<%= mmv.ModelMappingName%>';
        <%--$("#version_" + '<%= versionId%>').html("<b>this bike</b>");
        dataLayer.push({ 'event': 'Bikewale_all', 'cat': 'Make_Page', 'act': 'Get_On_Road_Price_Click', 'lab': selectedModel });--%>
    });

</script>
</form>
</body>
</html>