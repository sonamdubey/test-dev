<meta charset="utf-8">
<script language="c#" runat="server">
    private string title = "", description = "", keywords = "", AdId = "", AdPath = "", alternate="",canonical="";
    private ushort feedbackTypeId = 0;
    string staticUrl = System.Configuration.ConfigurationManager.AppSettings["staticUrl"];
    string staticFileVersion = System.Configuration.ConfigurationManager.AppSettings["staticFileVersion"];
</script>
<meta http-equiv="X-UA-Compatible" content="IE=edge">
<title><%=title %></title>
<meta name="description" content="<% =description%>" />
<meta name="viewport" content="width=device-width, initial-scale=1.0, maximum-scale=1.0, minimum-scale=1.0, user-scalable=no" />
<% if(!String.IsNullOrEmpty(keywords)) { %><meta name="keywords" content="<%= keywords %>" /><% } %>
<%if(!String.IsNullOrEmpty(canonical)) { %><link rel="canonical" href="<%=canonical %>" /><% } %>
<link rel="SHORTCUT ICON" href="http://img2.aeplcdn.com/bikewaleimg/images/favicon.png"  type="image/png"/>
<link href="/m/css/bwm-common-style.css?<%= staticFileVersion %>" rel="stylesheet" type="text/css">
<script type="text/javascript" src="<%= staticUrl != "" ? "http://st1.aeplcdn.com" + staticUrl : "" %>/m/src/frameworks.js?<%= staticFileVersion %>"></script>
<!-- for IE to understand the new elements of HTML5 like header, footer, section and so on -->
<!--[if lt IE 9]>
    <script src="/m/src/html5.js"></script>
<![endif]-->
<!-- #include file="\includes\gacode.aspx" -->