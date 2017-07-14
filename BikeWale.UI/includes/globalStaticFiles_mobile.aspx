﻿<script runat="server">	
    string staticUrlPath = System.Configuration.ConfigurationManager.AppSettings["staticUrl"];
    string staticFilesVersion = System.Configuration.ConfigurationManager.AppSettings["staticFileVersion"];
</script>
<link href='https://fonts.googleapis.com/css?family=Open+Sans:400,700' rel='stylesheet' type='text/css'>
<link rel="stylesheet" href="<%=  staticUrlPath%>/m/css/jquery.mobile-1.4.2.min.css?v=2.0" />
<link rel="stylesheet" href="/m/css/style.css?<%= staticFilesVersion%>" />
<link rel="stylesheet"  href="/m/css/design.css?<%= staticFilesVersion%>" />
<link href="/m/css/bwm-common-style.css?<%= staticFileVersion %>" rel="stylesheet" />
<script type="text/javascript" src="<%=  staticUrlPath %>/m/src/jquery-1.10.2.min.js?v=1.0"></script>
<script type="text/javascript" src="<%=  staticUrlPath %>/m/src/index.js?v=1.0"></script>
<script type="text/javascript" src="<%=  staticUrlPath %>/m/src/jquery.mobile-1.4.2.min.js?v=1.0"></script>
<script type="text/javascript" src="<%=  staticUrlPath %>/m/src/m-carousel.js?v=1.0"></script>
<script type="text/javascript" src="<%=  staticUrlPath %>/src/framework/knockout.js?<%= staticFilesVersion%>"></script>
<script type="text/javascript">
    $(function () { $('.m-carousel').carousel(); });
    $(document).ready(function () {
        $.mobile.ajaxEnabled = false;
    });
    bwHostUrl = '<%= ConfigurationManager.AppSettings["bwHostUrlForJs"] %>';
</script>