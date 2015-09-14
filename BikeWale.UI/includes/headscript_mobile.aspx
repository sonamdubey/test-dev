﻿<meta charset="utf-8">
<script language="c#" runat="server">
    private string title = "", description = "", keywords = "", AdId = "", AdPath = "", alternate="",canonical="";
    private ushort feedbackTypeId = 0;
    string staticUrl = System.Configuration.ConfigurationManager.AppSettings["staticUrl"];
</script>
<meta http-equiv="X-UA-Compatible" content="IE=edge">
<title><%=title %></title>
<meta name="viewport" content="width=device-width, initial-scale=1.0, maximum-scale=1.0, minimum-scale=1.0, user-scalable=no" />
<meta name="keywords" content="<%= keywords %>" />
<meta name="description" content="<% =description%>" />
<link rel="canonical" href="<%=canonical %>" />
<link rel="SHORTCUT ICON" href="http://img2.aeplcdn.com/v2/icons/bikewale.png?v=1.1" />
<link href="/m/css/bwm-common-style.css?09142015" rel="stylesheet" type="text/css">
<link href="<%= staticUrl != "" ? "http://st1.aeplcdn.com" + staticUrl : "" %>/m/css/home.css?14sept2015" rel="stylesheet" type="text/css">
<script type="text/javascript" src="<%= staticUrl != "" ? "http://st1.aeplcdn.com" + staticUrl : "" %>/m/src/frameworks.js?14sept2015"></script>
<!-- for IE to understand the new elements of HTML5 like header, footer, section and so on -->
<!--[if lt IE 9]>
    <script src="/m/src/html5.js"></script>
<![endif]-->