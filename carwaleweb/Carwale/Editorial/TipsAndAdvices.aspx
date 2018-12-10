<%@ Page Language="C#" Inherits="Carwale.UI.Editorial.TipsAndAdvices" AutoEventWireup="false" Debug="false" Trace="false" ViewStateMode="Disabled" %>

<%@ Import Namespace="Carwale.UI.Common" %>

<%@ Register TagPrefix="uc" TagName="TipsAndAdviceSummary" Src="/Controls/TipsAndAdviceSummary.ascx" %>
<%@ Register TagPrefix="uc" TagName="TipsAndAdvices" Src="/Controls/TipsAndAdvices.ascx" %>
<%@ Register Src="/Controls/TipsAndAdvices_New.ascx" TagPrefix="uc" TagName="TipsAndAdvices_New" %>

<!doctype html>
<html itemscope itemtype="http://schema.org/WebPage">
<head>
    <style type="text/css">
        /*Tips and Advices*/
        .tips {
            list-style: none;
            float: left;
            margin-top: 10px;
        }

        .tips-content {
            width: 300px;
            margin-right: 10px;
            float: left;
        }

        .subcat-sprite {
            background: url("https://imgd.aeplcdn.com/0x0/cw/static/sprites/tipsadvices.png") repeat-x scroll 0 0 transparent;
        }

        .nc-purchase, .uc-purchase, .insurance, .loan, .driving, .car-care, .tyres, .safety {
            background-position: -20px -11px;
            display: inline-block !important;
            height: 30px;
            width: 34px;
        }

        .uc-purchase {
            background-position: -72px -11px;
        }

        .insurance, .loan, .driving {
            background-position: -121px -11px;
            width: 28px;
        }

        .loan {
            background-position: -165px -11px;
        }

        .driving {
            background-position: -209px -11px;
        }

        .car-care {
            background-position: -298px -11px;
        }

        .tyres {
            background-position: -253px -11px;
        }

        .safety {
            background-position: -348px -11px;
        }

        .car-synopsis ul {
            list-style: disc;
            margin-left: 25px;
        }

        .car-synopsis li {
            line-height: 20px;
            margin-bottom: 10px;
        }

        .top-navbar-highlight {
            padding: 8px;
            background-color: #ef402f;
            float: left;
            margin-left: 10px;
        }

            .top-navbar-highlight a {
                color: #fff !important;
            }
    </style>

    <%
        // Define all the necessary meta-tags info here.
        // To know what are the available parameters,
        // check page, headerCommon.aspx in common folder.

        PageId = 44;
        Title = "Car Tips, Advice, How-To's and Do It Yourself";
        Description = " Tips, advice, how-to's and DIYs for car driving, ownership and maintenance. Know what to do and what not to around everyday car ownership and driving.";

        Revisit = "15";
        DocumentState = "Static";
        canonical = "https://www.carwale.com/tipsadvice/";
        altUrl = "https://www.carwale.com/m/tipsadvice/";
        AdId = "1396440332273";
        AdPath = "/1017752/ReviewsNews_";
        targetKey = "Accessories";
        targetValue = "TyreGuide";
    %>
    <!-- #include file="/includes/global/head-script.aspx" -->
    <script type='text/javascript'>
        googletag.cmd.push(function () {
            googletag.defineSlot('<%= AdPath %>300x250', [300, 250], 'div-gpt-ad-<%= AdId %>-0').addService(googletag.pubads());
            googletag.defineSlot('<%= AdPath %>970x90', [[220, 90], [728, 90], [950, 90], [960, 90], [970, 66], [970, 90]], 'div-gpt-ad-<%= AdId %>-2').addService(googletag.pubads());
            googletag.pubads().setTargeting("<%= targetKey %>", "<%= targetValue %>");
            googletag.pubads().setTargeting("City", "<%= CookiesCustomers.MasterCity.ToString() %>");
            googletag.pubads().setTargeting('UserModelHistory', '<%= CookiesCustomers.UserModelHistory.Replace('~', ',')%>');
            //googletag.pubads().enableSyncRendering();
            googletag.pubads().collapseEmptyDivs();
            googletag.pubads().enableSingleRequest();
            googletag.enableServices();
        });
    </script>
   
</head>
<body class="bg-white header-fixed-inner special-page special-skin-body no-bg-color">
    <form runat="server">
        <!-- #include file="/includes/header.aspx" -->
        <section class="container">
            <div class="grid-12">
                <div class="padding-bottom15 text-center">
                    <%= Carwale.UI.HtmlHelpers.Helpers.GetAdBarForAspx(AdId, 0, 90, 0, 0, true, 2) %>
                </div>
            </div>
        </section>
        <div class="clear"></div>
        <section class="bg-light-grey padding-top10 padding-bottom20 no-bg-color">
            <div class="container">
                <div class="grid-12">
                    <div class="breadcrumb margin-bottom15">
                        <!-- breadcrumb code starts here -->
                        <ul class="special-skin-text">
                            <li><a href="/">Home</a></li>
                            <li><span class="fa fa-angle-right margin-right10"></span><a href="/reviews-news/">Reviews & News</a></li>
                            <li><span class="fa fa-angle-right margin-right10"></span>Car Tips & Advice</li>
                            <div class="clear"></div>
                        </ul>
                        <div class="clear"></div>
                    </div>
                </div>
                <div class="clear"></div>
                <div class="grid-12">
                    <h1 class="font30 text-left text-black special-skin-text">Tips and Advice</h1>
                    <div class="border-solid-bottom margin-top10 margin-bottom15"></div>
                </div>
            </div>
            <div class="container">
                <div class="grid-8">
                    <div class="content-box-shadow content-inner-block-10">
                        <div>
                            <uc:TipsAndAdviceSummary ID="ucTipsAndAdviceSummary" runat="server"/>
                        </div>
                    </div>
                </div>
                <div class="grid-4">                 
                        <!-- Ad block code start here -->
                        <%= Carwale.UI.HtmlHelpers.Helpers.GetAdBarForAspx(AdId, 300, 250, 20, 20, false, 0) %>
                        <!-- Ad block code end here -->
                    <div class="content-box-shadow content-inner-block-10">
                        <uc:TipsAndAdvices_New runat="server" ID="TipsAndAdvices_New" />                 
                    </div>
               </div>
            </div>
            <div class="clear"></div>
        </section>
        <div class="clear"></div>
        <!-- #include file="/includes/footer.aspx" -->
        <!-- #include file="/includes/global/footer-script.aspx" -->
<script type="text/javascript">
    Common.showCityPopup = false;
    doNotShowAskTheExpert = false;
</script>   
 </form>
</body>
</html>

