<script language="c#" runat="server">
        string staticUrl = System.Configuration.ConfigurationManager.AppSettings["staticUrl"];
        string stagingPath = System.Configuration.ConfigurationManager.AppSettings["stagingPath"];
        string adPath = System.Configuration.ConfigurationManager.AppSettings["adPath"];
</script>
<link rel="SHORTCUT ICON" href="<%= Carwale.Utility.CWConfiguration._imgHostUrl %>0x0/cw/design15/carwale.png?static1fix" />
<link rel="stylesheet" href="/static/css/style-common.css" type="text/css" >
<script  type="text/javascript"  src="/static/src/jquery-1.7.2.min.js" ></script>

<script type="text/javascript">
    var FACEBOOKAPPID = '<%=System.Configuration.ConfigurationManager.AppSettings["FacebookAppId"]%>';
    var CLIENTID = '<%=System.Configuration.ConfigurationManager.AppSettings["GoogleClientId"]%>';
    var SCOPE = '<%=System.Configuration.ConfigurationManager.AppSettings["GoogleProjectScope"]%>';
    var REDIRECT = '<%=System.Configuration.ConfigurationManager.AppSettings["GoogleRedirectURL"]%>';
</script>
<script  type="text/javascript"  src="/static/src/cw-common.js" ></script>

<script  type="text/javascript"  src="/static/src/autocomplete.js" ></script>
<script  type="text/javascript"  src="/static/src/usercitypopup.js" ></script>
<link rel="stylesheet" href="/static/css/get-city-popup.css" type="text/css" >
<link rel="stylesheet" href="/static/css/pq-popup.css" type="text/css" >
<script  type="text/javascript"  src="/static/src/pq-common.js" ></script>
<script  type="text/javascript"  src="/static/src/graybox.js" ></script>
<%--PRIMARY NAVIGATION TABS TRACKING--%>
<script type="text/javascript">
    $(document).ready(function () {
        $('ul.primary-navbar-list li[class!="sept-prim"]').each(function (i, item) {
            var action = $.trim($(item).text().toLowerCase()) == "" ? "home" : $.trim($(item).text().toLowerCase()).replace("&", "").split(" ").join("_") + "_click"
            $(this).click(function () { dataLayer.push({ event: 'top_nav_section', cat: 'primary_nav_click', act: action }); })
        });
        $(".primary-navbar .cw-logo").click(function () { dataLayer.push({ event: 'top_nav_section', cat: 'primary_nav_click', act: "cw_logo_click" }) });
    });
</script>