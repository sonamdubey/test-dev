<script runat="server">	
    string staticUrlPath = System.Configuration.ConfigurationManager.AppSettings["staticUrl"];
    string staticFilesVersion = System.Configuration.ConfigurationManager.AppSettings["staticFileVersion"];
</script>
<link rel="SHORTCUT ICON" href="<%= staticUrlPath != "" ? "http://img2.aeplcdn.com/bikewaleimg" : "" %>/images/favicon.png?<%= staticFilesVersion%>"  type="image/png"/>
<link href='https://fonts.googleapis.com/css?family=Open+Sans:400,700' rel='stylesheet' type='text/css'>
<link type="text/css" href="<%= staticUrlPath != "" ? "http://st1.aeplcdn.com" + staticUrlPath : "" %>/css/style.css?<%= staticFilesVersion%>" rel="stylesheet"/>
<link type="text/css" href="<%= staticUrlPath != "" ? "http://st1.aeplcdn.com" + staticUrlPath : "" %>/css/960.css?<%= staticFilesVersion%>" rel="stylesheet"/>
<link href="<%= staticUrlPath != "" ? "http://st1.aeplcdn.com" + staticUrlPath : "" %>/css/bw-common-style.css?<%= staticFilesVersion%>" rel="stylesheet" />
<%--<script type="text/javascript" src="http://code.jquery.com/jquery-1.7.2.min.js"></script>--%>
<script type="text/javascript" src="<%= staticUrlPath != "" ? "http://st2.aeplcdn.com" + staticUrlPath : "" %>/src/jquery-1.7.2.min.js?v=1.0"></script>
<script type="text/javascript" src="<%= staticUrlPath != "" ? "http://st2.aeplcdn.com" + staticUrlPath : "" %>/src/BikeWaleCommon.js?v=1.2"></script>
<script type="text/javascript" src="<%= staticUrlPath != "" ? "http://st.aeplcdn.com" + staticUrlPath : "" %>/src/common/bt.js?v1.1"></script>
<script type="text/javascript" src="<%= !String.IsNullOrEmpty(staticUrlPath) ? "http://st2.aeplcdn.com" + staticUrlPath : "" %>/src/framework/knockout.js?<%= staticFilesVersion%>"></script>
<!--[if IE]><script language="javascript" src="<%= staticUrlPath != "" ? "http://st.aeplcdn.com" + staticUrlPath : "" %>/src/common/excanvas.js?v=1.0"></script><![endif]-->
<!--[if IE 6]>
    <script src="http://st.carwale.com/ie-png-fix.js?v=1.0"></script>
    <script>
        DD_belatedPNG.fix('.bw-logo a');/* fix png transparency problem with IE6 */
    </script>
<![endif]-->
<!-- for IE to understand the new elements of HTML5 like header, footer, section and so on -->
<!--[if lt IE 9]>
    <script src="/src/html5.js"></script>
<![endif]-->
<script type="text/javascript">
    bwHostUrl = "<%= ConfigurationManager.AppSettings["bwHostUrlForJs"] %>";
</script>
<script language="c#" runat="server">	
    private bool isTransparentHeader = false;  
</script>