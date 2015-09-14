<script runat="server">	
    string staticUrl = System.Configuration.ConfigurationManager.AppSettings["staticUrl"];
</script>
<link rel="stylesheet" href="<%= !String.IsNullOrEmpty(staticUrl) ? "http://st2.aeplcdn.com" + staticUrl : "" %>/m/css/jquery.mobile-1.4.2.min.css?v=2.0" />
<link rel="stylesheet" href="<%= !String.IsNullOrEmpty(staticUrl) ? "http://st2.aeplcdn.com" + staticUrl : "" %>/m/css/style.css?14sept2015" />
<link rel="stylesheet"  href="/m/css/design.css?09142015" />
<script type="text/javascript" src="<%= !String.IsNullOrEmpty(staticUrl) ? "http://st2.aeplcdn.com" + staticUrl : "" %>/m/src/jquery-1.10.2.min.js?v=1.0"></script>
<script type="text/javascript" src="<%= !String.IsNullOrEmpty(staticUrl) ? "http://st2.aeplcdn.com" + staticUrl : "" %>/m/src/index.js?v=1.0"></script>
<script type="text/javascript" src="<%= !String.IsNullOrEmpty(staticUrl) ? "http://st2.aeplcdn.com" + staticUrl : "" %>/m/src/jquery.mobile-1.4.2.min.js?v=1.0"></script>
<script type="text/javascript" src="<%= !String.IsNullOrEmpty(staticUrl) ? "http://st2.aeplcdn.com" + staticUrl : "" %>/m/src/m-carousel.js?v=1.0"></script>
<script type="text/javascript" src="<%= !String.IsNullOrEmpty(staticUrl) ? "http://st2.aeplcdn.com" + staticUrl : "" %>/src/framework/knockout.js?23june2015"></script>
<script type="text/javascript">
    $(function () { $('.m-carousel').carousel(); });
    $(document).ready(function () {
        $.mobile.ajaxEnabled = false;
    });
    bwHostUrl = '<%= ConfigurationManager.AppSettings["bwHostUrl"] %>';
</script>