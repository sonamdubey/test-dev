<!DOCTYPE html>
<html>
<head>
    <meta http-equiv="Content-Type" content="text/html; charset=iso-8859-1" />
    <link rel="stylesheet" href="/css/common.css?03102016" type="text/css" />
    <link rel="stylesheet" href="/css/chosen.min.css?03102016" />
    <script src="https://stb.aeplcdn.com/bikewale/min/src/frameworks.js?09Jan2017v1" type="text/javascript"></script>
    <script src="/src/AjaxFunctions.js?03102016" type="text/javascript"></script>
    <title>BikeWale Operations</title>
    <style type="text/css">
        .chosen-container {padding: 8px;}
    </style>
</head>
<body>
    <form runat="server">
	    <div>
           
            <div class="nav-wrapper">
                <ul class="side-nav" id="nav-drawer">
                    <li>
                        <div class="teal">
                            <a href="/"><span class="side-nav-title">BikeWale Operations</span></a>
                        </div>
                    </li>
                    <li>
                        <ul class="collapsible">
                            <li>
                                <div class="collapsible-header">Contents</div>
                                <div class="collapsible-body">
                                    <ul>
                                        <li class="collapsible-body-title">Bike Data</li>
                                        <li><a href="/makes/">Makes</a></li>
                                        <li><a href="/series/">Series</a></li>
                                        <li><a href="/content/bikemodels.aspx">Models</a></li>
                                        <li><a href="/content/bikeversions.aspx">Versions</a></li>
                                        <li><a href="/pageMetas/">Configure Page Metas</a></li>
                                        <li><a href="/content/NewBikeModelColors_New.aspx">Model Colors</a></li>
                                        <li><a href="/content/ModelColorWiseImage.aspx">Model Images By Color</a></li>
                                        <li><a href="/content/expectedlanuches.aspx">Upcoming Bikes</a></li>                                       
                                        <li><a href="/content/bikeunitssold.aspx">Bulk Units Sold</a></li>
                                        <li><a href="/Models/UsedModelImageUpload">Used Model Image Upload</a></li>
                                        <li class="collapsible-body-title teal lighten-4">Manage Prices</li>
                                        <li><a href="/content/showroomprices.aspx">Pricing</a></li>
                                        <li><a href="/prices/bulkupload/">Bulk Pricing</a></li>
                                        <li><a href="/content/pricemonitoring/">Price Monitoring</a></li>
                                        <li class="collapsible-body-title teal lighten-4">Bikewale Dealers</li>
                                        <li><a href="/content/adddealers.aspx">Add delears</a></li>
                                        <li><a href="/content/editdealers.aspx">Edit delears</a></li>
                                        <li class="collapsible-body-title teal lighten-4">User Reviews</li>                                        
                                        <li><a href="/userreviews/">Manage Reviews</a></li>
                                        <li><a href="/userreviews/manageratings/">Manage Ratings</a></li>
                                        <li class="collapsible-body-title teal lighten-4">Mobile Applications</li>
                                        <li><a href="/MobileApp/AppVersioning.aspx">App Versions</a></li>
                                    </ul>
                                </div>
                            </li>
                            <li>
                                <div class="collapsible-header">Classified</div>
                                <div class="collapsible-body teal-text">
                                    <ul>
                                        <li><a href="/classified/verifycustomerlisting.aspx">Manage Listings</a></li>
                                        <li><a href="/classified/verifyeditedlisting.aspx">Verify Edited Listings</a></li>
                                    </ul>
                                </div>
                            </li>
                            <li>
                                <div class="collapsible-header">Dealers</div>
                                <div class="collapsible-body teal-text">
                                    <ul>
                                        <li><a href="/dealers/operations/">Manage Dealers</a></li>
                                        <li><a href="/campaign/SearchDealerCampaigns.aspx">Dealer Campaigns</a></li>                                        
                                        <li><a href="/campaign/ManageDealerPriceCategories.aspx">Dealer Price Categories</a></li>
                                         <li><a href="/servicecenter/search/">Manage Service Centers</a></li>

                                    </ul>
                                </div>
                            </li>
                            <li>
                                <div class="collapsible-header">Enterprise Sales</div>
                                <div class="collapsible-body teal-text">
                                    <ul>                                        
                                        <li><a href="/manufacturercampaign/search/index/">Manufacturer Campaigns</a></li>
                                        <li><a href="/banner/bannerslist/">Home Page Banner Configuration</a></li>
                                        <li><a href="/content/bikecomparisonlist.aspx">Featured Comparisons</a></li>
                                        <li><a href="/adslots/">Manage Ad Slots</a></li>
                                    </ul>
                                </div>
                            </li>
                        </ul>
                    </li>
                </ul>
                <ul class="floatLeft">
                    <li><a href="javascript:void(0)" id="nav-btn" rel="nofollow"><span class="bwsprite nav-icon"></span></a></li>
                    <li class="nav-title">BikeWale Operations</li>
                </ul>
                <ul class="floatRight">
                    <li class="nav-logged-in-user">Welcome <%= Bikewale.Utility.OprUser.UserName %></li>
                    <li><a class="white-text" href="/common/logout.aspx?logout=logout">Logout</a></li>
                </ul>

            </div>

		<%--<div class="header">				
			<div class="logo floatLeft">
				<h1><a href="" title="Centralized Internet Services"><span class="dark">BikeWale</span>Operations</a></h1>
			</div>					
			
            <div class="floatRight">
                <span class="font13 text-bold margin-right20 verical-middle">
				    Welcome <%= BikeWaleOpr.Common.CurrentUser.UserName %>
                </span>
                <a href="/common/logout.aspx?logout=logout" class="btn btn-default verical-middle">Logout</a>
            </div>
            <div class="clear"></div>
            <div class=""></div>			
		</div>
			
		<div class="bar" style="width:100%;">
			<ul>
				<li><a href="/content/default.aspx" accesskey="p">Contents</a></li>
                <li><a href="/classified/default.aspx" accesskey="e">Classified</a></li>
                <li><a href="/newbikebooking/default.aspx" accesskey="e">Manage Dealers</a></li>                    
            </ul>
		</div>--%>
	    <div class='toast' style='display:none'></div>

