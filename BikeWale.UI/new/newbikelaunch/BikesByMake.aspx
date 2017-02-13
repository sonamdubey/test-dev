<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="BikesByMake.aspx.cs" Inherits="Bikewale.New.NewLaunchBikes.BikesByMake" %>

<!DOCTYPE html>
<html>
<head>
    <title>New bike launches by make</title>
    <%
        isHeaderFix = false;    
    %>
    <!-- #include file="/includes/headscript_desktop_min.aspx" -->
    <link rel="stylesheet" type="text/css" href="/css/new-launch/new-launch.css" />
    <script type="text/javascript">
        <!-- #include file="\includes\gacode_desktop.aspx" -->
    </script>
</head>
<body>
    <form id="form1" runat="server">
        <!-- #include file="/includes/headBW.aspx" -->
        <section>
            <div class="container padding-top10">
                <div class="grid-12">
                    <div class="breadcrumb margin-bottom15">
                        <ul>
                            <li itemscope="" itemtype="http://data-vocabulary.org/Breadcrumb">
                                <a href="/" itemprop="url">
                                    <span itemprop="title">Home</span>
                                </a>
                            </li>
                            <li itemscope="" itemtype="http://data-vocabulary.org/Breadcrumb">
                                <span class="bwsprite fa-angle-right margin-right10"></span>
                                 <a href="" itemprop="url">
                                    <span itemprop="title">New bike launches</span>
                                </a>
                            </li>
                            <li itemscope="" itemtype="http://data-vocabulary.org/Breadcrumb">
                                <span class="bwsprite fa-angle-right margin-right10"></span>
                                <span itemprop="title">New Bajaj Bike Launches</span>
                            </li>
                        </ul>
                        <div class="clear"></div>
                    </div>
                </div>
                <div class="clear"></div>
            </div>
        </section>

        <section>
            <div class="container section-bottom-margin">
                <div class="grid-12">
                    <div class="bg-white">
                        <h1 class="section-header">New Bajaj Bike Launches</h1>
                        <p class="section-inner-padding font14 text-light-grey">India is one of the largest two-wheeler market in the world. The Indian market of two-wheelers has some noteworthy diversity. In order to thrive in this massive competition, manufacturers try to keep up with fast changing trends. Every year there are plenty of bike models that hit the market. BikeWale brings you an exhaustive list of newly launched bikes in India. Explore the list of latest bikes.</p>
                    </div>
                </div>
                <div class="clear"></div>
            </div>
        </section>


        <script type="text/javascript" src="<%= staticUrl != "" ? "https://st1.aeplcdn.com" + staticUrl : "" %>/src/frameworks.js?<%=staticFileVersion %>"></script>
        <!-- #include file="/includes/footerBW.aspx" -->
        <link href="<%= staticUrl != "" ? "https://st2.aeplcdn.com" + staticUrl : "" %>/css/bw-common-btf.css?<%=staticFileVersion %>" rel="stylesheet" type="text/css" />
        
        <!-- #include file="/includes/footerscript.aspx" -->
        <script type="text/javascript" src="<%= staticUrl != "" ? "https://st1.aeplcdn.com" + staticUrl : "" %>/src/new-launch.js?<%=staticFileVersion %>"></script>
        <link href='https://fonts.googleapis.com/css?family=Open+Sans:400,600,700' rel='stylesheet' type='text/css' />
    </form>
</body>
</html>
