
<link rel="SHORTCUT ICON" href="<%= staticUrl != "" ? "https://img2.aeplcdn.com/bikewaleimg" : "" %>/images/favicon.png"  type="image/png"/>
<link href='https://fonts.googleapis.com/css?family=Open+Sans:400,700' rel='stylesheet' type='text/css'>
<link type="text/css" href="<%= staticUrl %>/css/style.css?<%= staticFileVersion%>" rel="stylesheet"/>
<link type="text/css" href="<%= staticUrl %>/css/960.css" rel="stylesheet"/>
<link type="text/css" href="<%= staticUrl %>/css/bw-common-style.css?<%= staticFileVersion%>" rel="stylesheet" />

<script type="text/javascript" src="<%= staticUrl  %>/src/frameworks.js?<%=staticFileVersion %>"></script>
<script type="text/javascript" src="<%= staticUrl  %>/src/BikeWaleCommon.js?v=1.2"></script>
<script type="text/javascript" src="<%= staticUrl %>/src/common/bt.js?v1.1"></script>
<!--[if IE]><script language="javascript" src="<%= staticUrl  %>/src/common/excanvas.js?v=1.0"></script><![endif]-->
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
    bwHostUrl = "<%= ConfigurationManager.AppSettings["bwHostUrlForJs"] %>";
</script>
<script language="c#" runat="server">	
    private bool isTransparentHeader = false;  
</script>