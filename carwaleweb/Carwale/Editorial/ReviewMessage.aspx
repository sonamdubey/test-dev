<%@ Page Trace="false" Language="C#" %>

<%@ Import Namespace="Carwale.UI.Common" %>
<!doctype html>
<html>
<head>
    <!-- #include file="/includes/global/head-script.aspx" -->
    <%
        // Define all the necessary meta-tags info here.
        // To know what are the available parameters,
        // check page, headerCommon.aspx in common folder.

        PageId = 43;
        Title = "Inquiry Submission Confirmation";
        Description = "Inquiry Submission Confirmation at CarWale.com";
        Keywords = "";
        Revisit = "15";
        DocumentState = "Static";
    %>
    <script type='text/javascript'>
        googletag.cmd.push(function () {
            googletag.defineSlot('<%= AdPath %>300x250', [300, 250], 'div-gpt-ad-<%= AdId %>-0').addService(googletag.pubads());
        googletag.defineSlot('<%= AdPath %>300x250_BTF', [300, 250], 'div-gpt-ad-<%= AdId %>-1').addService(googletag.pubads());
        googletag.defineSlot('<%= AdPath %>970x90', [[220, 90], [728, 90], [950, 90], [960, 90], [970, 66], [970, 90]], 'div-gpt-ad-<%= AdId %>-2').addService(googletag.pubads());
            googletag.pubads().setTargeting("City", "<%= CookiesCustomers.MasterCity.ToString() %>");
            googletag.pubads().setTargeting('UserModelHistory', '<%= CookiesCustomers.UserModelHistory.Replace('~', ',')%>');
        //googletag.pubads().enableSyncRendering();
        googletag.pubads().collapseEmptyDivs();
        googletag.pubads().enableSingleRequest();
        googletag.enableServices();
    });
    </script>
    <style>
        p {
            font-size: 13px;
            color: #333333;
        }
    </style>
</head>
<body class="bg-white header-fixed-inner special-page special-skin-body no-bg-color">
    <form runat="server">
        <!-- #include file="/includes/header.aspx" -->
        <section class="container">
            <div class="grid-12">
                <div class="padding-bottom10 padding-top10 text-center">
                    <%= Carwale.UI.HtmlHelpers.Helpers.GetAdBarForAspx(AdId, 0, 90, 0, 0, true, 2) %>
                </div>
            </div>
        </section>
        <div class="clear"></div>
        <section class="bg-light-grey padding-top10 padding-bottom20 no-bg-color">
            <div class="container">
                <div class="grid-12">
                    <div class="breadcrumb">
                        <!-- breadcrumb code starts here -->
                        <ul class="special-skin-text">
                            <li><a href="/">Home</a></li>
                            <li><span class="fa fa-angle-right margin-right10"></span><a href="/new/">New Cars</a></li>
                        </ul>
                        <div class="clear"></div>
                    </div>
                    <h1 class="font30 text-black special-skin-text">Confirmation</h1>
                    <div class="border-solid-bottom margin-top10 margin-bottom15"></div>
                </div>
                <div class="clear"></div>
                <div class="grid-8">
                    <div class="content-box-shadow content-inner-block-10">
                        <div id="left_container_onethird">
                            <table width="100%" border="0">
                                <tr>
                                    <td valign="top">
                                        <!-- For the ad only ends here-->
                                        <p>
                                            <strong>Your review has been submitted successfully.</strong><br>
                                            Your review will help other car buyers at Carwale.com in making their 
						purchasing decisions. 
                                        </p>

                                        <p>
                                            <a href='<%= Request.QueryString["url"]%>'><strong>Go Back</strong></a>
                                        </p>
                                    </td>
                                </tr>
                            </table>
                        </div>
                    </div>
                </div>
                <div class="grid-4">
                    <div class="addbox">
                            <%= Carwale.UI.HtmlHelpers.Helpers.GetAdBarForAspx(AdId, 300, 250, 20, 20, false, 0) %>
                            <%= Carwale.UI.HtmlHelpers.Helpers.GetAdBarForAspx(AdId, 300, 250, 0, 20, false, 1) %>
                        </div>
                </div>
                <div class="clear"></div>
            </div>
        </section>
        <div class="clear"></div>
        <img height="0" width="0" src='https://www.s2d6.com/x/?x=a&amp;h=17260&amp;o=<%= "UR" + CurrentUser.Id %>' alt="" />
        <img height="0" width="0" src='https://www.s2d6.com/x/?x=r&amp;h=17260&amp;o=<%= "UR" + CurrentUser.Id %>&amp;g=441212112514&amp;s=0&amp;q=0' alt="" />

        <!-- #include file="/includes/footer.aspx" -->
        <!-- all other js plugins -->
        <!-- #include file="/includes/global/footer-script.aspx" -->
    </form>
</body>
</html>





