<!DOCTYPE html>
<html>
<head>
    <meta http-equiv="Content-Type" content="text/html; charset=iso-8859-1" />
    <link href="http://fonts.googleapis.com/icon?family=Material+Icons" rel="stylesheet">
    <link href="/css/materialize.css" rel="stylesheet" />
    <link rel="stylesheet" href="/css/common.css?03102016" type="text/css" />
    <link rel="stylesheet" href="/css/chosen.min.css?03102016" />
    <script src="/src/framework.js?03102016" type="text/javascript"></script>
    <script src="/src/AjaxFunctions.js?03102016" type="text/javascript"></script>
    <script type="text/javascript" src="/src/common/common.js?03102016"></script>
    <title>BikeWale Operations</title>
    <style type="text/css">
        .chosen-container {padding: 8px;}
    </style>
</head>
<body>
    <form runat="server">
	    <div>
            <nav>
                <div class="nav-wrapper teal">
                    <ul class="side-nav" id="mobile-demo">
                        <li>
                            <div class="teal">
                                <a href="/default.aspx"><span class="white-text side-nav-title">BikeWale Operations</span></a>
                            </div>
                        </li>
                        <li>
                            <ul class="collapsible teal lighten-2" data-collapsible="accordion">
                                <li>
                                    <div class="collapsible-header">Contents</div>
                                    <div class="collapsible-body teal-text">
                                        <ul>
                                            <li class="collapsible-body-title teal lighten-4">Bike Data</li>
                                            <li>
	                                            <a href="/content/bikemakes.aspx">Bike Makes</a>
                                            </li>
                                            <li>
	                                            <a href="/content/bikemodels.aspx">Bike Models</a>
                                            </li>
                                            <li>
	                                            <a href="/content/bikeversions.aspx">Bike Versions</a>
                                            </li>
                                            <li>
	                                            <a href="/content/NewBikeModelColors_New.aspx">New Bike Model Colors</a>
                                            </li>
                                            <li>
	                                            <a href="/content/ModelColorWiseImage.aspx">Update Model Images By Color</a>
                                            </li>
                                            <li>
	                                            <a href="/content/expectedlanuches.aspx">Expected Bike Launches</a>
                                            </li>
                                            <li>
	                                            <a href="/content/bikecomparisonlist.aspx">Bike Comparison List</a>
                                            </li>
                                            <li>
	                                            <a href="/content/bikeunitssold.aspx">Bulk Unit Sold Upload</a>
                                            </li>

                                            <li class="collapsible-body-title teal lighten-4">Manage Prices</li>
                                            <li>
	                                            <a href="/content/showroomprices.aspx">Showroom Prices</a>
                                            </li>
                                            <li>
	                                            <a href="/content/bulkpriceupload.aspx">Bulk Price Upload</a>
                                            </li>
                                            <li>
	                                            <a href="/content/pricemonitoring.aspx">Price Monitoring</a>
                                            </li>

                                            <li class="collapsible-body-title teal lighten-4">Bikewale Dealers</li>
                                            <li>
                                                <a href="/content/adddealers.aspx">Add delears</a>
                                            </li>
                                            <li>
                                                <a href="/content/editdealers.aspx">Edit delears</a>
                                            </li>

                                            <li class="collapsible-body-title teal lighten-4">User Reviews</li>
                                            <li>
                                                <a href="/content/manageuserreviews.aspx">Manage Reviews</a>
                                            </li>

                                            <li class="collapsible-body-title teal lighten-4">Mobile Applications</li>
                                            <li>
                                                <a href="/MobileApp/AppVersioning.aspx">Manage App Versions</a>
                                            </li>
                                        </ul>
                                    </div>

                                </li>
                                <li>
                                    <div class="collapsible-header">Classified</div>
                                    <div class="collapsible-body teal-text">
                                        <ul>
                                            <li>
	                                            <a href="/classified/verifycustomerlisting.aspx">Manage Listings</a>
                                            </li>
                                            <li>
	                                            <a href="/classified/verifyeditedlisting.aspx">Verify Edited Listings</a>
                                            </li>
                                        </ul>
                                    </div>
                                </li>
                                <li>
                                    <div class="collapsible-header">Dealers</div>
                                    <div class="collapsible-body teal-text">
                                        <ul>
                                            <li>
	                                            <a href="/newbikebooking/default.aspx">Manage Dealers</a>
                                            </li>
                                            <li>
	                                            <a href="/campaign/SearchDealerCampaigns.aspx">Search Dealer Campaigns</a>
                                            </li>
                                            <li>
	                                            <a href="/manufacturecampaign/SearchManufacturerCampaign.aspx">Manufacturer's Campaigns</a>
                                            </li>
                                            <li>
	                                            <a href="/campaign/ManageDealerPriceCategories.aspx">Add Price Categories</a>
                                            </li>
                                        </ul>
                                    </div>
                                </li>
                            </ul>
                        </li>
                    </ul>
                    <ul class="left">
                        <li><a href="#" data-activates="mobile-demo" class="buttons"><i class="material-icons">menu</i></a></li>
                        <li class="nav-title">PageTitle</li>
                    </ul>

                    <ul class="right">
                        <li class="nav-logged-in-user">Welcome <%= BikeWaleOpr.Common.CurrentUser.UserName %></li>
                        <li><a class="white-text" href="/common/logout.aspx?logout=logout"><i class="material-icons right">exit_to_app</i>Logout</a></li>
                    </ul>

                </div>
            </nav>

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

