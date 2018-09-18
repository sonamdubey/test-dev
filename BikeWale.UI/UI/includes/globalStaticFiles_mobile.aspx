<script runat="server">	
    string staticUrlPath = System.Configuration.ConfigurationManager.AppSettings["staticUrl"];
    string staticFilesVersion = System.Configuration.ConfigurationManager.AppSettings["staticFileVersion"];
    string fontFile="";
    string fontUrl = "";
</script>

<%     fontFile  = "/UI/css/fonts/OpenSans/open-sans-v15-latin-regular.woff";
       fontUrl  = String.Format("{0}{1}?{2}", Bikewale.Utility.BWConfiguration.Instance.StaticUrl, fontFile, Bikewale.Utility.BWConfiguration.Instance.StaticCommonFileVersion);
%>
<style>
    @font-face { 
        font-family: 'Open Sans';
        font-style: normal;
        font-weight: 400;
        src: local('Open Sans Regular'), local('OpenSans-Regular'), url('<%=fontUrl%>') format('woff');
    }

</style>
<%     fontFile = "/UI/css/fonts/OpenSans/open-sans-v15-latin-700.woff"; %>
<%       fontUrl  = String.Format("{0}{1}?{2}", Bikewale.Utility.BWConfiguration.Instance.StaticUrl, fontFile, Bikewale.Utility.BWConfiguration.Instance.StaticCommonFileVersion); %>
<style>
     @@font-face { 
	    font-family: 'Open Sans';
	    font-style: normal;
	    font-weight: 700;
	    src: local('Open Sans Bold'), local('OpenSans-Bold'), url('<% =fontUrl %>') format('woff');
     }
</style>

<link rel="stylesheet" href="<%=  staticUrlPath%>/UI/m/css/jquery.mobile-1.4.2.min.css?v=2.0" />
<link rel="stylesheet" href="/UI/m/css/style.css?<%= staticFilesVersion%>" />
<link rel="stylesheet"  href="/UI/m/css/design.css?<%= staticFilesVersion%>" />
<link href="/UI/m/css/bwm-common-style.css?<%= staticFileVersion %>" rel="stylesheet" />
<link rel='stylesheet' type='text/css' href='/UI/m/css/bwm-doodle.css'/>
<script type="text/javascript" src="<%=  staticUrlPath %>/UI/m/src/jquery-1.10.2.min.js?v=1.0"></script>
<script type="text/javascript" src="<%=  staticUrlPath %>/UI/m/src/index.js?v=1.0"></script>
<script type="text/javascript" src="<%=  staticUrlPath %>/UI/m/src/jquery.mobile-1.4.2.min.js?v=1.0"></script>
<script type="text/javascript" src="<%=  staticUrlPath %>/UI/m/src/m-carousel.js?v=1.0"></script>
<script type="text/javascript" src="<%=  staticUrlPath %>/UI/src/framework/knockout.js?<%= staticFilesVersion%>"></script>
<script type="text/javascript">
    $(function () { $('.m-carousel').carousel(); });
    $(document).ready(function () {
        $.mobile.ajaxEnabled = false;
    });
    bwHostUrl = '<%= ConfigurationManager.AppSettings["bwHostUrlForJs"] %>';
</script>