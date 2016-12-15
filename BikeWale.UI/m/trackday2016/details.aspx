<%@ Page Language="C#" AutoEventWireup="false" Inherits="Bikewale.Mobile.TrackDay.Details" %>

<!DOCTYPE html>
<html>
<head>
    <% switch (articleid)
       {
           case 1:
               title = "BikeWale Track Day 2016 - An Introduction";
               description = "Inaugural BikeWale Track Day gave us the perfect opportunity to find out the limits of a regular bike on a racetrack in Chennai";
               break;
           case 2:
               title = "BikeWale Track Day 2016 - TVS Apache RTR 200 4V";
               description = "BikeWale team put TVS Apache RTR 200 4V to test on a racetrack in Chennai. Check out the details of its performance in areas like on-the-edge handling, late braking performance, and ultimate lap time at MMRT";
               break;
           case 3:
               title = "BikeWale Track Day 2016 - Yamaha YZF-R3";
               description = "BikeWale team put Yamaha YZF-R3 to test on a racetrack in Chennai. Check out the details of its performance in areas like on-the-edge handling, late braking performance, and ultimate lap time at MMRT";
               break;
           case 4:
               title = "BikeWale Track Day 2016 - Benelli TNT 600i ABS";
               description = "BikeWale team put Benelli TNT 600i ABS to test on a racetrack in Chennai. Check out the details of its performance in areas like on-the-edge handling, late braking performance, and ultimate lap time at MMRT";
               break;
           case 5:
               title = "BikeWale Track Day 2016 - Ducati 959 Panigale";
               description = "BikeWale team put Ducati 959 Panigale to test on a racetrack in Chennai. Check out the details of its performance in areas like on-the-edge handling, late braking performance, and ultimate lap time at MMRT";
               break;
           case 6:
               title = "Ten best photos of BikeWale Track Day 2016";
               description = "Check out the 10 best photos of RTR 200, R3, TNT 660i ABS, 959 Panigale on a racetrack";
               break;
           default:
               title = "BikeWale Track Day 2016 - An Introduction";
            description = "Inaugural BikeWale Track Day gave us the perfect opportunity to find out the limits of a regular bike on a racetrack in Chennai";
            break;        
    } %>

    <% 
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
        <% if (!androidApp)
           { %>
        <!-- #include file="/includes/headBW_Mobile.aspx" -->
        <% } %>

        <section>
            <% if (objTrackDay != null)
               { %>
            <div id="track-day-article" class="container box-shadow bg-white section-container padding-bottom1">
                <div class="padding-top20">
                    <div class="text-center">
                        <div class="trackday-logo"></div>
                    </div>
                    <%-- <% switch (articleid)
                       {
                        case 1: %>
                            <!-- #include file="/m/trackday/articles/Introduction.aspx" -->
                            <% break;
                        case 2: 
                        %>
                            <!-- #include file="/m/trackday/articles/TVSApache.aspx" -->
                            <% break;
                        case 3: 
                        %>
                            <!-- #include file="/m/trackday/articles/YamahaYZF.aspx" -->
                            <% break;
                        case 4: 
                        %>
                            <!-- #include file="/m/trackday/articles/BenelliTNT.aspx" -->
                            <% break;
                        case 5: 
                        %>
                            <!-- #include file="/m/trackday/articles/DucatiPanigale.aspx" -->
                            <% break;
                        case 6: 
                        %>
                            <!-- #include file="/m/trackday/articles/TopBikes.aspx" -->
                            <% break;
                        default: 
                        %>
                            <!-- #include file="/m/trackday/articles/Introduction.aspx" -->
                            <% break;
                       } %>--%>

                    <div class="padding-right20 padding-left20">
                        <div class="bg-loader-placeholder">
                            <img class="article-image lazy" data-original="<%= Bikewale.Utility.Image.GetPathToShowImages(objTrackDay.OriginalImgUrl, objTrackDay.HostUrl, Bikewale.Utility.ImageSize._640x348) %>" alt="<%= objTrackDay.Title %>" src="" border="0" />
                        </div>

                        <div class="text-center margin-bottom30">
                            <h1 class="article-heading text-unbold"><%= objTrackDay.Title %></h1>
                            <p class="article-author margin-bottom5"><i><%= objTrackDay.AuthorName %></i></p>
                            <p class="font12 text-light-grey"><%= Bikewale.Utility.FormatDate.GetFormatDate(objTrackDay.DisplayDate.ToString(),"MMM dd, yyyy") %></p>
                        </div>

                         <%= String.IsNullOrEmpty(objTrackDay.Content) ? "" : objTrackDay.Content %>
                        
                    </div>

                </div>
            </div>
            <% } %>
        </section>

        <div id="gallery-close-btn" class="bwmsprite cross-lg-white"></div>
        <div id="gallery-blackOut-window"></div>

        <script type="text/javascript" src="<%= staticUrl != "" ? "https://st1.aeplcdn.com" + staticUrl : "" %>/m/src/frameworks.js?<%= staticFileVersion %>"></script>

        <% if (!androidApp)
           { %>
        <!-- #include file="/includes/footerBW_Mobile.aspx" -->
        <% } %>

        <link href="<%= staticUrl != "" ? "https://st2.aeplcdn.com" + staticUrl : "" %>/m/css/bwm-common-btf.css?<%= staticFileVersion %>" rel="stylesheet" type="text/css" />
        <script type="text/javascript" src="<%= staticUrl != "" ? "https://st2.aeplcdn.com" + staticUrl : "" %>/m/src/common.min.js?<%= staticFileVersion %>"></script>
        <script type="text/javascript" src="<%= staticUrl != "" ? "https://st2.aeplcdn.com" + staticUrl : "" %>/m/trackday/src/track-day.js?<%= staticFileVersion %>"></script>

        <link href='https://fonts.googleapis.com/css?family=Open+Sans:400,700' rel='stylesheet' type='text/css' />

    </form>
</body>
</html>
