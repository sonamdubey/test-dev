<%@ Page Language="C#" AutoEventWireup="false" Inherits="Bikewale.Mobile.Used.Search" %>

<!DOCTYPE html>
<html>
<head>
    <title>Used search</title>
    <!-- #include file="/includes/headscript_mobile.aspx" -->
    <link href="<%= staticUrl != "" ? "http://st2.aeplcdn.com" + staticUrl : "" %>/m/css/used-search.css?<%= staticFileVersion %>" rel="stylesheet" type="text/css" />
</head>
<body>
    <form id="form1" runat="server">
        <!-- #include file="/includes/headBW_Mobile.aspx" -->
        <section>
            <div class="container bg-white clearfix">
                <h1 class="padding-top15 padding-right20 padding-bottom15 padding-left20 box-shadow">Used Royal Enfield Bullet bikes</h1>

                <div class="font14 padding-top10 padding-right20 padding-bottom10 padding-left20">Showing <span class="text-bold">1-20</span> of <span class="text-bold">200</span> bikes</div>

                <div id="sort-filter-wrapper" class="text-center border-solid-bottom">
                    <div class="grid-6 padding-top10 padding-bottom10 border-solid-right cur-pointer">
                        <span class="bwmsprite sort-by-icon"></span>
                        <span class="font14 text-bold">Sort by</span>
                    </div>
                    <div class="grid-6 padding-top10 padding-bottom10 cur-pointer">
                        <span class="bwmsprite filter-icon"></span>
                        <span class="font14 text-bold">Filter</span>
                    </div>
                    <div class="clear"></div>
                </div>

                <ul>
                    <li>
                        
                    </li>
                </ul>
            </div>
        </section>
        <!-- #include file="/includes/footerBW_Mobile.aspx" -->
        <!-- #include file="/includes/footerscript_Mobile.aspx" -->
        <script type="text/javascript" src="<%= staticUrl != "" ? "http://st2.aeplcdn.com" + staticUrl : "" %>/m/src/used-search.js?<%= staticFileVersion %>"></script>
    </form>
</body>
</html>
