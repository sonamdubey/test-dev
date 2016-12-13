<%@ Page Language="C#" AutoEventWireup="false" Inherits="Bikewale.Mobile.TrackDay.Details" %>
<!DOCTYPE html>
<html>
<head>
    <% 
        //title       = "Bike News - Latest Indian Bike News & Views - BikeWale";
        //description = "Latest news updates on Indian bikes industry, expert views and interviews exclusively on BikeWale.";
        //keywords    = "news, bike news, auto news, latest bike news, indian bike news, bike news of india"; 
        //canonical   = "https://www.bikewale.com/news/";
        //AdPath = "/1017752/Bikewale_Mobile_NewBikes";
        //AdId = "1398766302464";
        //Ad_320x50 = true;
        //Ad_Bot_320x50 = true;
    %>
   
    <!-- #include file="/includes/headscript_mobile_min.aspx" -->

    <link rel="stylesheet" type="text/css" href="<%= staticUrl != "" ? "https://st2.aeplcdn.com" + staticUrl : "" %>/m/trackday/css/track-day.css?<%= staticFileVersion %>" />
    <script type="text/javascript">
        <!-- #include file="\includes\gacode_mobile.aspx" -->
    </script>
</head>
<body class="bg-light-grey">
    <form runat="server">
        <% if(!androidApp) { %>
        <!-- #include file="/includes/headBW_Mobile.aspx" -->
        <% } %>
        
        <section>
            <div id="track-day-article" class="container box-shadow bg-white section-container padding-15-20">
                <!-- #include file="/m/trackday/articles/Introduction.aspx" -->
            </div>
        </section>
        
        <script type="text/javascript" src="<%= staticUrl != "" ? "https://st1.aeplcdn.com" + staticUrl : "" %>/m/src/frameworks.js?<%= staticFileVersion %>"></script>

        <% if(!androidApp) { %>
        <!-- #include file="/includes/footerBW_Mobile.aspx" -->
        <% } %>

        <link href="<%= staticUrl != "" ? "https://st2.aeplcdn.com" + staticUrl : "" %>/m/css/bwm-common-btf.css?<%= staticFileVersion %>" rel="stylesheet" type="text/css" />
        <script type="text/javascript" src="<%= staticUrl != "" ? "https://st2.aeplcdn.com" + staticUrl : "" %>/m/src/common.min.js?<%= staticFileVersion %>"></script>
        <link href='https://fonts.googleapis.com/css?family=Open+Sans:400,700' rel='stylesheet' type='text/css' />
    
    </form>
</body>
</html>
