<%@ Page Language="C#" AutoEventWireup="false" Inherits="Carwale.Used.UCAUnsubscribe" Trace="false" Debug="false" %>

<!doctype html>
<html>
<head>
    <%
        // Define all the necessary meta-tags info here.
        // To know what are the available parameters,
        // check page, headerCommon.aspx in common folder.

        PageId = 1;
        Title = "Unsubscribe Used Car Alerts";
        Description = "Unsubscribe Used Car Alerts";
        Keywords = "";
        Revisit = "15";
        DocumentState = "Static";
    %>
    <!-- #include file="/includes/global/head-script.aspx" -->
    <script type='text/javascript'>
        googletag.cmd.push(function () {
            googletag.defineSlot('<%= AdPath %>970x90', [[220, 90], [728, 90], [950, 90], [960, 90], [970, 66], [970, 90]], 'div-gpt-ad-<%= AdId %>-2').addService(googletag.pubads());

        //googletag.pubads().enableSyncRendering();
        googletag.pubads().collapseEmptyDivs();
        googletag.pubads().enableSingleRequest();
        googletag.enableServices();
    });
    </script>
    <!-- Header ends here -->
</head>
<body class="header-fixed-inner">
    <form runat="server">
        <!-- #include file="/includes/header.aspx" -->
        <input type="hidden" id="hdnIsPageFromCache" runat="server" />
        <section class="container">
            <div class="grid-12">
                <div class="padding-bottom15 text-center">
                    <%= Carwale.UI.HtmlHelpers.Helpers.GetAdBarForAspx(AdId, 0, 90, 0, 0, true, 2) %>
                </div>
            </div>
        </section>
        <div class="clear"></div>
        <section class="bg-light-grey padding-top10 padding-bottom20 no-bg-color">
            <div class="container content-box-shadow content-inner-block-10">
                <h1 class="font30 text-black special-skin-text">Unsubscribe - Used Car Alert</h1>
                <div class="border-solid-bottom margin-top10 margin-bottom15"></div>
                <div class="grid-12">
                    <div class="RedBarMid">
                        <div id="dReq" runat="server">
                            <p>
                                The email address <b><%=CustomerEmail %></b> will no longer receive this used car alert. Please confirm:&nbsp;<input type="button" runat="server" id="btnConfirm" value="Unsubscribe" class="btn btn-orange btn-xs" />
                            </p>
                        </div>
                        <div id="dMes" runat="server" visible="false">
                            <p>The used car alert has been removed successfully. It may take up to 48 hours to take effect though.</p>
                            <p>
                                We hope that CarWale was successful in helping you find the used car you were looking for. 
                        However, if you are still searching for one, you may create another alert <a href="/used/cars-for-sale/<%= CustomerCity > 0 ? "?city=" + CustomerCity : ""%>">here</a>.
                            </p>
                        </div>
                        <input type="hidden" id="hdnUCAId" runat="server" />
                        <input type="hidden" id="hdnUCAEmail" runat="server" />
                    </div>
                </div>
                <div class="clear"></div>
            </div>
        </section>
        <div class="clear"></div>
        <!-- #include file="/includes/footer.aspx" -->
        <!-- all other js plugins -->
        <!-- #include file="/includes/global/footer-script.aspx" -->
    </form>
</body>
</html>
