<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Finance.aspx.cs" Inherits="Carwale.UI.m.used.Finance" %>
<% 
   Title = "Loan Approval";
   IsShowAd = false;
   ShowBottomAd = false;
%>
<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml" itemscope itemtype="http://schema.org/WebPage">
    <!-- #include file="/m/includes/global/head-script.aspx" -->
<body> 
    <!-- #include file="/m/includes/header.aspx" -->
    <script>
        try {
            var ctFinanceCWVisitedURL = "<%= ctFinanceCWVisitedURL %>";
            var ios = getQsParameterByName("ios");
            if (!ios) {
                window.location.replace(ctFinanceCWVisitedURL);
            }
        } catch (err) { console.log(err.message) };

        function getQsParameterByName(name) {
            var params = window.location.search.slice(1).split('&');
            if (params)
            {
                for (var i = params.length - 1; i >= 0; i--) {
                    var tempParam = params[i].split('=');
                    if (tempParam[0] == name)
                        return tempParam[1];
                }
            }
         }
    </script>
         <div id="iframe-content">
                <iframe src="<%= iframeUrl %>" width="100%"></iframe>
        </div>
        <!-- #include file="/m/includes/footer.aspx" -->
        <!-- #include file="/m/includes/global/footer-script.aspx" -->

    <script type="text/javascript" src="/static/js/classified_finance.js"></script>
    <script type="text/javascript" src="/static/js/promise-polyfill.min.js"></script>
    <script>
        classifiedFinance.addEventListenerForIframe();
        classifiedFinance.targetDestination = '<%= iframeUrl%>';
    </script>
</body>
</html>
