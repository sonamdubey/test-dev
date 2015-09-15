<script runat="server">	
    string staticUrl = System.Configuration.ConfigurationManager.AppSettings["staticUrl"];
    string staticFileVersion = System.Configuration.ConfigurationManager.AppSettings["staticFileVersion"];
</script>
<link rel="stylesheet" href="<%= !String.IsNullOrEmpty(staticUrl) ? "http://st2.aeplcdn.com" + staticUrl : "" %>/m/css/jquery.mobile-1.4.2.min.css?v=2.0" />
<link rel="stylesheet" href="<%= !String.IsNullOrEmpty(staticUrl) ? "http://st2.aeplcdn.com" + staticUrl : "" %>/m/css/style.css?<%= staticFileVersion%>" />
<link rel="stylesheet"  href="<%= !String.IsNullOrEmpty(staticUrl) ? "http://st2.aeplcdn.com" + staticUrl : "" %>/m/css/design.css?<%= staticFileVersion%>" />
<script type="text/javascript" src="<%= !String.IsNullOrEmpty(staticUrl) ? "http://st2.aeplcdn.com" + staticUrl : "" %>/m/src/jquery-1.10.2.min.js?v=1.0"></script>
<script type="text/javascript" src="<%= !String.IsNullOrEmpty(staticUrl) ? "http://st2.aeplcdn.com" + staticUrl : "" %>/m/src/index.js?v=1.0"></script>
<script type="text/javascript" src="<%= !String.IsNullOrEmpty(staticUrl) ? "http://st2.aeplcdn.com" + staticUrl : "" %>/m/src/jquery.mobile-1.4.2.min.js?v=1.0"></script>
<script type="text/javascript" src="<%= !String.IsNullOrEmpty(staticUrl) ? "http://st2.aeplcdn.com" + staticUrl : "" %>/m/src/m-carousel.js?v=1.0"></script>
<script type="text/javascript" src="<%= !String.IsNullOrEmpty(staticUrl) ? "http://st2.aeplcdn.com" + staticUrl : "" %>/src/framework/knockout.js?<%= staticFileVersion%>"></script>
<script type="text/javascript">
    $(function () { $('.m-carousel').carousel(); });
    $(document).ready(function () {
        $.mobile.ajaxEnabled = false;
    });
    bwHostUrl = '<%= ConfigurationManager.AppSettings["bwHostUrl"] %>';
</script>