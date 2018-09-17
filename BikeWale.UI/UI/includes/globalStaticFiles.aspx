<script language="c#" runat="server">	
     string staticUrlPath = Bikewale.Utility.BWConfiguration.Instance.StaticUrl;
     string staticFilesVersion = Bikewale.Utility.BWConfiguration.Instance.StaticFileVersion;
    private bool isTransparentHeader = false;
</script>
<link rel="SHORTCUT ICON" href="<%= staticUrlPath != "" ? "https://img2.aeplcdn.com/bikewaleimg" : "" %>/images/favicon.png?<%= staticFilesVersion%>" type="image/png" />

        <%
            string fontFile = "/UI/css/fonts/OpenSans/open-sans-v15-latin-regular.woff",
            fontUrl = String.Format("{0}{1}?{2}", Bikewale.Utility.BWConfiguration.Instance.StaticUrl, fontFile, Bikewale.Utility.BWConfiguration.Instance.StaticCommonFileVersion);
        %>
        <style>
            @font-face { 
                font-family: 'Open Sans';
                font-style: normal;
                font-weight: 400;
                src: local('Open Sans Regular'), local('OpenSans-Regular'), url('<%= fontUrl%>') format('woff');
             }
        </style>
        
        <%  
            fontFile = "/UI/css/fonts/OpenSans/open-sans-v15-latin-700.woff";
            fontUrl  = String.Format("{0}{1}?{2}", Bikewale.Utility.BWConfiguration.Instance.StaticUrl, fontFile, Bikewale.Utility.BWConfiguration.Instance.StaticCommonFileVersion); 
        %>
        <style>
             @font-face { 
	            font-family: 'Open Sans';
	            font-style: normal;
	            font-weight: 700;
	            src: local('Open Sans Bold'), local('OpenSans-Bold'), url('<% =fontUrl %>') format('woff');
             }
        </style>

<link href="<%= String.IsNullOrEmpty(staticUrlPath) ? "" : staticUrlPath %>/UI/css/style.css?<%= staticFilesVersion%>" rel="stylesheet" type="text/css" />
<link href="<%= String.IsNullOrEmpty(staticUrlPath) ? "" : staticUrlPath %>/UI/css/960.css?<%= staticFilesVersion%>" rel="stylesheet" type="text/css" />
<link href="<%= String.IsNullOrEmpty(staticUrlPath) ? "" : staticUrlPath %>/UI/css/bw-common-style.css?<%= staticFilesVersion%>" rel="stylesheet" type="text/css" />

<script type="text/javascript" src="<%= staticUrlPath %>/UI/src/frameworks.js?<%= staticFilesVersion %>"></script>
<script type="text/javascript" src="<%= staticUrlPath  %>/UI/src/BikeWaleCommon.js?v=1.2"></script>
<script type="text/javascript" src="<%= staticUrlPath  %>/UI/src/common/bt.js?v1.1"></script>
<!--[if IE]><script language="javascript" src="<%= staticUrlPath %>/UI/src/common/excanvas.js?v=1.0"></script><![endif]-->
<!--[if IE 6]>
    <script src="https://stc.carwale.com/ie-png-fix.js?v=1.0"></script>
    <script>
        DD_belatedPNG.fix('.bw-logo a');/* fix png transparency problem with IE6 */
    </script>
<![endif]-->
<!-- for IE to understand the new elements of HTML5 like header, footer, section and so on -->
<!--[if lt IE 9]>
    <script src="/UI/src/html5.js"></script>
<![endif]-->
<script type="text/javascript">
    bwHostUrl = "<%= Bikewale.Utility.BWConfiguration.Instance.BwHostUrlForJs %>";
</script>
