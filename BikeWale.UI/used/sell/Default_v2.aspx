﻿<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default_v2.aspx.cs" Inherits="Bikewale.Used.Sell.Default_v2" %>

<!DOCTYPE html>

<html>
<head>
    <title>Sell Bike</title>

    <!-- #include file="/includes/headscript_desktop_min.aspx" -->

    <link href="/css/sell-bike.css" rel="stylesheet" type="text/css" />

    <script type="text/javascript">
        <!-- #include file="\includes\gacode_desktop.aspx" -->
    </script>
</head>
<body class="bg-light-grey header-fixed-inner">
    <form id="form1" runat="server">
        <!-- #include file="/includes/headBW.aspx" -->

        <section class="bg-light-grey padding-top10" id="breadcrumb">
            <div class="container">
                <div class="grid-12">
                    <div class="breadcrumb margin-bottom15">
                        <!-- breadcrumb code starts here -->
                        <ul>
                            <li itemscope="" itemtype="http://data-vocabulary.org/Breadcrumb"><a href="/" itemprop="url">
                                <span itemprop="title">Home</span></a>
                            </li>
                            <li itemtype="http://data-vocabulary.org/Breadcrumb"><span class="bwsprite fa-angle-right margin-right10"></span>
                              <a href="/used/" itemprop="url"><span>Used Bikes</span></a>
                            </li>
                            <li itemtype="http://data-vocabulary.org/Breadcrumb"><span class="bwsprite fa-angle-right margin-right10"></span>
                              <span>Sell Your Bike</span>
                            </li>                            
                        </ul>
                        <div class="clear"></div>
                    </div>
                </div>
                <div class="clear"></div>
            </div>
        </section>

        <section>
            <div class="container margin-bottom20">
                <div class="grid-12">
                    <div class="content-box-shadow">
                        <div class="content-box-shadow padding-14-20">
                            <h1 class="font24 text-x-black">Sell your bike</h1>
                        </div>
                        <div id="sell-bike-content">
                            <div id="sell-bike-left-col" class="grid-7 panel-group">
                                <div class="panel">
                                    <div class="panel-head">
                                        <span class="sell-bike-sprite step-1-active"></span>
                                        <span class="panel-title">Bike details</span>
                                    </div>
                                    <div class="panel-body">
                                        <div class="panel-row">
                                            <div class="grid-4 alpha select-box">
                                                <p class="chosen-select-label margin-bottom5">Brand<sup>*</sup></p>
                                                <select class="chosen-select">
                                                    <option value="0">Select Make</option>
                                                    <option value="10">Honda</option>
                                                    <option value="11">Bajaj</option>
                                                    <option value="12">Hero</option>
                                                    <option value="13">TVS</option>
                                                    <option value="14">Royal Enfield</option>
                                                    <option value="15">Harley Davidson</option>
                                                    <option value="16">KTM</option>
                                                    <option value="17">Aprilia</option>
                                                    <option value="18">Benelli</option>
                                                    <option value="19">Yamaha</option>
                                                </select>
                                                <span class="boundary"></span>
                                                <span class="error-text"></span>
                                            </div>

                                            <div class="grid-4 select-box">
                                                <p class="chosen-select-label margin-bottom5">Model<sup>*</sup></p>
                                                <select class="chosen-select">
                                                    <option value="0">Select Model</option>
                                                    <option value="50">125 Scooter</option>
                                                    <option value="51">Activa</option>
                                                    <option value="52">CB Hornet 160R</option>
                                                    <option value="53">CB Shine</option>
                                                    <option value="54">Avenger 220 Cruise</option>
                                                    <option value="55">Avenger 220 Street</option>
                                                </select>
                                                <span class="boundary"></span>
                                                <span class="error-text"></span>
                                            </div>

                                            <div class="grid-4 omega select-box">
                                                <p class="chosen-select-label margin-bottom5">Version<sup>*</sup></p>
                                                <select class="chosen-select">
                                                    <option value="0">Select Version</option>
                                                    <option value="80">Kick/Drum/Spokes</option>
                                                    <option value="81">Electric Start/Drum/Alloy</option>
                                                    <option value="82">CBS</option>
                                                </select>
                                                <span class="boundary"></span>
                                                <span class="error-text"></span>
                                            </div>

                                            <div class="clear"></div>
                                        </div>

                                        <div class="panel-row margin-top20">
                                            <input type="button" class="btn btn-orange" value="Save and Continue" />
                                        </div>
                                    </div>
                                    <div class="clear"></div>
                                </div>
                                <%--<div class="panel">
                                    <div class="panel-head">
                                        <span class="sell-bike-sprite step-2-inactive"></span>
                                        <span class="panel-title">Personal details</span>
                                    </div>
                                </div>
                                <div class="panel">
                                    <div class="panel-head">
                                        <span class="sell-bike-sprite step-3-inactive"></span>
                                        <span class="panel-title">More details</span>
                                    </div>
                                </div>--%>
                            </div>
                            <div id="sell-bike-right-col" class="grid-5">

                            </div>
                            <div class="clear"></div>
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
        <script type="text/javascript" src="/src/knockout.validation.js"></script>
        <script type="text/javascript" src="/src/sell-bike.js"></script>
        <link href='https://fonts.googleapis.com/css?family=Open+Sans:400,700' rel='stylesheet' type='text/css' />
        <!--[if lt IE 9]>
            <script src="/src/html5.js"></script>
        <![endif]-->

    </form>
</body>
</html>
