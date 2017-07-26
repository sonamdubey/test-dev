﻿<script language="c#" runat="server">	
     string staticUrlPath = Bikewale.Utility.BWConfiguration.Instance.StaticUrl;
     string staticFilesVersion = Bikewale.Utility.BWConfiguration.Instance.StaticFileVersion;
    private bool isTransparentHeader = false;
</script>
<link rel="SHORTCUT ICON" href="<%= staticUrlPath != "" ? "https://img2.aeplcdn.com/bikewaleimg" : "" %>/images/favicon.png?<%= staticFilesVersion%>" type="image/png" />
<link href='https://fonts.googleapis.com/css?family=Open+Sans:400,700' rel='stylesheet' type='text/css'>

<link href="<%= String.IsNullOrEmpty(staticUrlPath) ? "" : staticUrlPath %>/css/style.css?<%= staticFilesVersion%>" rel="stylesheet" type="text/css" />
<link href="<%= String.IsNullOrEmpty(staticUrlPath) ? "" : staticUrlPath %>/css/960.css?<%= staticFilesVersion%>" rel="stylesheet" type="text/css" />
<link href="<%= String.IsNullOrEmpty(staticUrlPath) ? "" : staticUrlPath %>/css/bw-common-style.css?<%= staticFilesVersion%>" rel="stylesheet" type="text/css" />

<script type="text/javascript" src="<%= staticUrlPath %>/src/frameworks.js?<%= staticFilesVersion %>"></script>
<script type="text/javascript" src="<%= staticUrlPath  %>/src/BikeWaleCommon.js?v=1.2"></script>
<script type="text/javascript" src="<%= staticUrlPath  %>/src/common/bt.js?v1.1"></script>
<!--[if IE]><script language="javascript" src="<%= staticUrlPath %>/src/common/excanvas.js?v=1.0"></script><![endif]-->
<!--[if IE 6]>
    <script src="https://stc.carwale.com/ie-png-fix.js?v=1.0"></script>
    <script>
        DD_belatedPNG.fix('.bw-logo a');/* fix png transparency problem with IE6 */
    </script>
<![endif]-->
<!-- for IE to understand the new elements of HTML5 like header, footer, section and so on -->
<!--[if lt IE 9]>
    <script src="/src/html5.js"></script>
<![endif]-->
<script type="text/javascript">
    bwHostUrl = "<%= Bikewale.Utility.BWConfiguration.Instance.BwHostUrlForJs %>";
</script>
