<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default_v2.aspx.cs" Inherits="Bikewale.Used.Default_v2" %>

<!DOCTYPE html>

<html>
<head>
    <%
        isHeaderFix = false;
        isTransparentHeader = true; 
    %>

    <title>Used Bikes in India</title>

    <!-- #include file="/includes/headscript_desktop_min.aspx" -->

    <link type="text/css" href="/css/used/landing.css" rel="stylesheet" />

    <script type="text/javascript">
        <!-- #include file="\includes\gacode_desktop.aspx" -->
    </script>
</head>
<body class="bg-light-grey">
    <form id="form1" runat="server">
        <!-- #include file="/includes/headBW.aspx" -->
        <header class="used-landing-banner">
            <div id="used-landing-box" class="container">
                <div class="welcome-box">
                    <h1 class="font30 text-uppercase margin-bottom30">Used bikes</h1>
                    <h2 class="font20 text-unbold text-white margin-bottom50">View wide range of used bikes</h2>
                </div>
            </div>
        </header>

        <section>
            <div class="container section-container">
                <div class="grid-12">
                    <div class="content-box-shadow negative-50 text-center padding-25-30">
                        <h2 class="section-header">Search used bikes</h2>
                        <div class="usedbikes-search-container">
                            <div id="search-form-city" class="form-control-box">
                                <select class="form-control chosen-select">
                                    <option value="0" >Select a city</option>
                                    <option>Ahmedabad</option>
                                    <option>Bangalore</option>
                                    <option>Chennai</option>
                                    <option>Hyderabad</option>
                                    <option>Kolkata</option>
                                    <option>Mumbai</option>
                                    <option>New Delhi</option>
                                    <option>Pune</option>
                                </select>
                            </div>
                            <div id="search-form-budget" class="form-control-box">
                                <div id="min-max-budget-box" class="form-selection-box">
                                    <span id="budget-default-label">Select budget</span>
                                    <span id="min-amount"></span>
                                    <span id="max-amount"></span>
                                    <span id="upDownArrow" class="fa fa-angle-down position-abt pos-top18 pos-right20"></span>
                                    <div class="clear"></div>
                                </div>
                                <div id="budget-list-box">
                                    <div id="user-budget-input" class="bg-light-grey text-light-grey">
                                        <div id="min-input-label" class="input-label-box border-solid-right">Min</div><div id="max-input-label" class="input-label-box">Max</div>
                                    </div>
                                    <ul id="min-budget-list" class="text-left"></ul>
                                    <ul id="max-budget-list" class="text-right"></ul>
                                </div>
                            </div>
                            <div class="clear"></div>
                            <input type="button" id="" class="btn btn-orange btn-lg search-bikes-btn margin-bottom5" value="Search" />
                        </div>
                    </div>
                </div>
                <div class="clear"></div>
            </div>
        </section>




        <script type="text/javascript" src="<%= staticUrl != "" ? "http://st1.aeplcdn.com" + staticUrl : "" %>/src/frameworks.js?<%=staticFileVersion %>"></script>

        <!-- #include file="/includes/footerBW.aspx" -->
        <link href="<%= staticUrl != "" ? "http://st2.aeplcdn.com" + staticUrl : "" %>/css/bw-common-btf.css?<%=staticFileVersion %>" rel="stylesheet" type="text/css" />
        <script type="text/javascript" src="<%= staticUrl != string.Empty ? "http://st2.aeplcdn.com" + staticUrl : string.Empty %>/src/common.min.js?<%= staticFileVersion %>"></script>
        <script type="text/javascript" src="<%= staticUrl != string.Empty ? "http://st2.aeplcdn.com" + staticUrl : string.Empty %>/src/used-landing.js?<%= staticFileVersion %>"></script>
        <link href='https://fonts.googleapis.com/css?family=Open+Sans:400,700' rel='stylesheet' type='text/css' />
        <!--[if lt IE 9]>
            <script src="/src/html5.js"></script>
        <![endif]-->
    </form>
</body>
</html>
