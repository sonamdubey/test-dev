<script runat="server">	
    string staticUrl = System.Configuration.ConfigurationManager.AppSettings["staticUrl"];
</script>
<link rel="SHORTCUT ICON" href="<%= staticUrl != "" ? "http://img2.aeplcdn.com/bikewaleimg" : "" %>/images/favicon.png"  type="image/png"/>
<link type="text/css" href="<%= staticUrl != "" ? "http://st1.aeplcdn.com" + staticUrl : "" %>/css/style.css?30july2015" rel="stylesheet"/>
<link type="text/css" href="<%= staticUrl != "" ? "http://st1.aeplcdn.com" + staticUrl : "" %>/css/960.css" rel="stylesheet"/>
<script type="text/javascript" src="http://st.carwale.com/jquery-1.7.2.min.js?v=1.0" ></script>
<script type="text/javascript" src="<%= staticUrl != "" ? "http://st2.aeplcdn.com" + staticUrl : "" %>/src/BikeWaleCommon.js?v=1.2"></script>
<script type="text/javascript" src="<%= staticUrl != "" ? "http://st.aeplcdn.com" + staticUrl : "" %>/src/common/bt.js?v1.1"></script>
<script type="text/javascript" src="<%= !String.IsNullOrEmpty(staticUrl) ? "http://st2.aeplcdn.com" + staticUrl : "" %>/src/framework/knockout.js?11june2015"></script>
<!--[if IE]><script language="javascript" src="<%= staticUrl != "" ? "http://st.aeplcdn.com" + staticUrl : "" %>/src/common/excanvas.js?v=1.0"></script><![endif]-->
<!--[if IE 6]>
    <script src="http://st.carwale.com/ie-png-fix.js?v=1.0"></script>
    <script>
        DD_belatedPNG.fix('.bw-logo a');/* fix png transparency problem with IE6 */
    </script>
<![endif]-->