<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <%
        title = "New Bikes, Used Bikes, Bike Prices, Reviews & Photos in India";
        keywords = "new bikes, used bikes, buy used bikes, sell your bike, bikes prices, reviews, photos, news, compare bikes, Instant Bike On-Road Price";
        description = "BikeWale - India's favourite bike portal. Find new and used bikes, buy or sell your bikes, compare new bikes prices & values.";
        AdPath = "/1017752/BikeWale_HomePage_";
        AdId = "1395985604192";
        alternate = "http://www.bikewale.com/m/";
        canonical = "http://www.bikewale.com/";        
    %>
    <!-- #include file="/includes/headscript.aspx" -->
</head>
<body class="bg-light-grey">
<form runat="server">    
    <!-- #include file="/includes/headBW.aspx" -->    
    
    <section class="header-fixed-inner">
    	<div class="container">
        	<div class="grid-12">
                <div class="breadcrumb margin-bottom15">
                    <!-- breadcrumb code starts here -->
                    <ul>
                        <li><a href="/">Home</a></li>
                        <li><span class="fa fa-angle-right margin-right10"></span>Search Result</li>
                    </ul>
                    <div class="clear"></div>
                </div>
                <%--<div class="border-solid-bottom margin-top10 margin-bottom15"></div>--%>
            </div>
            <div class="clear"></div>
        </div>
    </section>

    <section>
        <div class="container margin-bottom20">
            <div class="grid-12">
                <%--<div class="search-box-container margin-bottom20">
                    <p class="leftfloat margin-top10 font14 margin-right20">Search</p>
                    <div class="leftfloat form-control-box margin-right20">
                        <input type="text" class="form-control" />
                    </div>
                    <input type="submit" value="search" class="btn btn-grey leftfloat"/>
                    <div class="clear"></div>
                </div>--%>
                <div id="search-result-wrapper">
                    <div id="cse-search-results" class="content-box-shadow content-inner-block-10 rounded-corner2 search-result-container">

                    </div>
                </div>
            </div>
            <div class="clear"></div>
        </div>
    </section>
    <script type="text/javascript">
        var googleSearchIframeName = "cse-search-results";
        //var googleSearchFormName = "cse-search-box";
        var googleSearchFrameWidth = 955;
        var googleSearchDomain = "www.google.com";
        var googleSearchPath = "/cse";
    </script>
    <script type="text/javascript" src="http://www.google.com/afsonline/show_afs_search.js"></script>
    <!-- #include file="/includes/footerBW.aspx" -->
    <!-- #include file="/includes/footerscript.aspx" -->
    <style type="text/css">
        .search-box-container { width:540px; }
        .search-box-container .form-control-box { width:300px; }
        .search-result-container { min-height:600px; }
    </style>
    </form>
</body>
</html>
