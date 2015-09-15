﻿<%@ Page Language="C#" AutoEventWireup="false" Inherits="Bikewale.Mobile.New.Search" %>
<%@ Register TagPrefix="BW" TagName="MPopupWidget" Src="/m/controls/MPopupWidget.ascx" %>
<!doctype html>
<html>
    <head>
        <%
            title = "New Bikes, Used Bikes, Bike Prices, Reviews & Photos in India";
            keywords = "new bikes, used bikes, buy used bikes, sell your bike, bikes prices, reviews, photos, news, compare bikes, Instant Bike On-Road Price";
            description = "BikeWale - India's favourite bike portal. Find new and used bikes, buy or sell your bikes, compare new bikes prices & values.";
            canonical = "http://www.bikewale.com";
            AdPath = "/1017752/Bikewale_Mobile_Homepage";
            AdId = "1398766000399";
         %>
        <!-- #include file="/includes/headscript_mobile.aspx" -->
    </head>
    <body class="bg-light-grey">
        <!-- #include file="/includes/Navigation_Mobile.aspx" -->
        <link href="<%= staticUrl != "" ? "http://st2.aeplcdn.com" + staticUrl : "" %>/m/css/new/bwm-search.css" rel="stylesheet" type="text/css" />
        <div class="blackOut-window"></div>
        <header>
    	    <div class="header-fixed"> <!-- Fixed Header code starts here -->
        	    <span id="bikecount" class="font18 text-white brand-total"></span>
                <div class="leftfloat">
                    <span class="navbarBtn bwmsprite nav-icon margin-right10"></span>                
                </div>
                <div class="rightfloat">
                    <a class="sort-btn" id="sort-btn">
                        <span class="bwmsprite sort-icon"></span>
                    </a>
                </div>
                <div class="rightfloat">
                    <a class="filter-btn" id="filter-btn">
                        <span class="bwmsprite filter-icon"></span>
                    </a>
                </div>
            
                <div class="clear"></div>
            </div> <!-- ends here -->
    	    <div class="clear"></div>
       
        </header>
    
        <section><!--  Used Search code starts here -->
            <div class="container">
                <div><!--  class="grid-12"-->
            
              		    <div class="hide" id="sort-by-div">
                    	    <div  class="filter-sort-div font14 bg-white">
                                <div sc="1" so="">
                                    <a data-title="sort" class="price-sort position-rel">
                                	    Price<span class="hide" so="0" class="sort-text"></span>
                                    </a>
                                </div>
                                <div sc="" class="border-solid-left">
                                    <a data-title="sort" class="position-rel">
                                	    Popularity 
                                    </a>
                                </div>
                                <div sc="2" class="border-solid-left">
                                    <a data-title="sort" class="position-rel">
                                	    Mileage 
                                    </a>
                                </div>
                            </div>
                        </div>
                    <div class="bike-search">
                        <div id="divSearchResult" data-bind="template: { name: 'listingTemp', foreach: searchResult }" class="search-bike-container">

                        </div>
                        <div style="text-align:center;">
                            <div id="nobike" class="hide">
                                <img src="/images/no_result_m.png" />
                            </div>
                            <div id="loading" class="hide">
                                <img src="/images/search-loading.gif" />
                            </div>
                        </div>
                    </div>
                    <script type="text/html" id="listingTemp">
                        <div class="search-bike-item">
                            <div class="front">
                                <div class="contentWrapper">
                                    <!--<div class="position-abt pos-right10 pos-top10 infoBtn bwmsprite alert-circle-icon"></div>-->
                                    <div class="imageWrapper">
                                        <a data-bind="attr: { href: '/m/' + bikemodel.makeBase.maskingName() + '-bikes/' + bikemodel.maskingName() + '/' }">
                                            <img class="lazy" data-bind="attr: { src: bikemodel.hostUrl() + '/310x174/' + bikemodel.imagePath(), title: bikeName, alt: bikeName }">
                                        </a>
                                    </div>
                                        <div class="bikeDescWrapper">
                                            <div class="bikeTitle">
                                                <h3><a data-bind="attr: { href: '/m/' + bikemodel.makeBase.maskingName() + '-bikes/' + bikemodel.maskingName() + '/', title: bikeName }, text: bikeName"></a></h3>
                                            </div>
                                            <div class="font22 text-grey margin-bottom5">
                                                <span class="fa fa-rupee"></span>
                                                <span class="font24" data-bind="text: price"></span>
                                            </div>
                                            <div class="margin-bottom10 font14 text-light-grey">Ex-showroom, <%= ConfigurationManager.AppSettings["defaultName"] %></div>
                                            <div class="font13 margin-bottom10">
                                                <span data-bind="html: availSpecs"> </span>
                                            </div>
                                            <div class="padding-top5 clear">
                                                <!-- ko if:bikemodel.reviewCount() != 0  -->
                                                    <div class="grid-6 alpha">
                                                        <div class="padding-left5 padding-right5 border-solid-right ">
                                                            <div>
                                                                <span class="margin-bottom10" data-bind="html: AppendCertificationStar(bikemodel.reviewRate())"></span>
                                                            </div>
                                                        </div>
                                                    </div>
                                                <!-- /ko -->
                                                <div class="grid-6 omega">
                                                    <div class="padding-left5 padding-right5">
                                                        <span class="font16 text-light-grey" data-bind="text: ShowReviewCount(bikemodel.reviewCount())"></span>
                                                    </div>
                                                </div>
                                                <div class="clear"></div>
                                                <a data-bind="attr: { modelId: bikemodel.modelId }, click: function () { FillCitiesPopup(bikemodel.modelId()) }" class="btn btn-sm btn-white margin-top10 fillPopupData">Get on road price</a>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            <div class="border-top1 margin-left20 margin-right20 padding-top20 clear"></div>
                        </div>
                    </script>
                </div>
                <div class="clear"></div>
            </div>
    
        </section><!-- Used Search code  Ends here -->
   
            <!--Filters starts here-->
                <div id="filter-div" class="popup_layer hide">
                    <div data-role="header" data-theme="b" class="ui-corner-top" data-icon="delete">
                        <div id="hidePopup" class="filterBackArrow" popupname="filterpopup" onclick="CloseWindow(this)">
                            <!--<span class="bwmsprite back-long-arrow-left-white"></span>-->
                            <span class="fa fa-angle-left"></span>
                        </div>
                        <div class="floatleft cw-m-sprite city-back-btn" id="back-btn"></div>
                        <div class="filterTitle">Filters</div>
                        <div id="btnReset" class="resetrTitle">Reset</div>
                        <div class="clear"></div>
                    </div>
                    <div class="content-inner-block-20 margin-bottom40 clearfix">
                	    <!--Brand section starts here-->
                        <div class="dropdown form-control-box margin-bottom20"> 
                            <div class="form-control">
                              <span class="hida">Brand</span>
                              <div class="multiSel"></div>
                            </div>
                          
                            <div name="bike" class="multiSelect">
                                <ul>
                                    <li class="unchecked" filterId="2"><span>Aprilia</span></li>
                                    <li class="unchecked" filterId="7"><span>Honda</span></li>
                                    <li class="unchecked" filterId="11"><span>Royal Enfield</span></li>
                                    <li class="unchecked" filterId="1"><span>Bajaj</span></li>
                                    <li class="unchecked" filterId="8"><span>Hyosung</span></li>
                                    <li class="unchecked" filterId="12"><span>Suzuki</span></li>
                                    <li class="unchecked" filterId="40"><span>Benelli</span></li>
                                    <li class="unchecked" filterId="34"><span>Indian</span></li>
                                    <li class="unchecked" filterId="22"><span>Triumph</span></li>
                                    <li class="unchecked" filterId="3"><span>BMW</span></li>
                                    <li class="unchecked" filterId="17"><span>Kawasaki</span></li>
                                    <li class="unchecked" filterId="15"><span>TVS</span></li>
                                    <li class="unchecked" filterId="4"><span>Ducati</span></li>
                                    <li class="unchecked" filterId="9"><span>KTM</span></li>
                                    <li class="unchecked" filterId="16"><span>Vespa</span></li>
                                    <li class="unchecked" filterId="5"><span>Harley Davidson</span></li>
                                    <li class="unchecked" filterId="19"><span>LML</span></li>
                                    <li class="unchecked" filterId="13"><span>Yamaha</span></li>
                                    <li class="unchecked" filterId="6"><span>Hero</span></li>
                                    <li class="unchecked" filterId="10"><span>Mahindra</span></li>
                                    <li class="unchecked" filterId="14"><span>Yo</span></li>
                                    <li class="unchecked" filterId="39"><span>Hero Electric</span></li>
                                    <li class="unchecked" filterId="20"><span>Moto Guzzi</span></li>
                            </ul>
                            </div>
                        </div>
                        <!--Brand section starts here-->

                        <!--Budget section starts here-->
                        <div class="margin-bottom20">
                            <h3 class="text-black margin-bottom10">Budget</h3>
                            <div class="slider-box content-box-shadow content-inner-block-10">
                                <div class="leftfloat">
                                    <span id="rangeAmount">0 - Any value</span>
                                </div>
                                <div class="clear"></div>
                                <div name="budget" id="mSlider-range" class="bwm-sliders"></div>
                            </div>
                        </div>
                        <!--Budget section ends here-->
                    
                        <!--Displacement section starts here-->
                        <div class="dropdown form-control-box margin-bottom20"> 
                            <div class="form-control">
                              <span class="hida">Displacement</span>    
                              <div class="multiSel"></div>  
                            </div>
                          
                            <div name="displacement" class="multiSelect">
                                <ul>
                                    <li class="unchecked" filterId="1"><span>Up to 110 cc</span></li>
                                    <li class="unchecked" filterId="2"><span>110-150 cc</span></li>
                                    <li class="unchecked" filterId="3"><span>150-200 cc</span></li>
                                    <li class="unchecked" filterId="4"><span>200-250 cc</span></li>
                                    <li class="unchecked" filterId="5"><span>250-500 cc</span></li>
                                    <li class="unchecked" filterId="6"><span>500 cc and more</span></li>
                                </ul>
                            </div>
                        </div>
                        <!--Displacement section starts here-->
                    
                       <!--ride section starts here-->
                        <div class="dropdown form-control-box margin-bottom20"> 
                            <div class="form-control">
                                <span class="hide">Ride Style</span>
                                <span class="hida">Ride Style</span>    
                                <div class="multiSel"></div>  
                            </div>
                          
                            <div name="ridestyle" class="multiSelect">
                                <ul>
                                    <li class="unchecked" filterId="1"><span>Cruisers</span></li>
                                    <li class="unchecked" filterId="2"><span>Sports</span></li>
                                    <li class="unchecked" filterId="3"><span>Street</span></li>
                                    <li class="unchecked" filterId="5"><span>Scooters</span></li>
                                </ul>
                            </div>
                        </div>
                        <!--ride section starts here-->
                    
                    
                        <!--ride section starts here-->
                        <div class="form-control-box margin-bottom20 clearfix">
                    	    <h3 class="text-black margin-bottom10">Mileage</h3>
                            <div name="mileage" class="grid-12 mileage-box">
                        	    <div class="grid-3 content-inner-block-5">
                            	    <span filterid="1" class="form-control mileage">70+</span>
                                </div>
                                <div class="grid-3 content-inner-block-5">
                            	    <span filterid="2" class="form-control mileage">70-50</span>
                                </div>
                                <div class="grid-3 content-inner-block-5">
                            	    <span filterid="3" class="form-control mileage">50-30</span>
                                </div>
                                <div class="grid-3 content-inner-block-5">
                            	    <span filterid="4" class="form-control mileage">30-0</span>
                                </div>
                            </div>
                        </div>
                        <!--ride section starts here-->
                    
                         <div class="grid-12 alpha omega margin-bottom20 clear">
                        	    <div class="grid-5 alpha">
                            	    <h3>ABS</h3>
                                </div>
                                <div name="AntiBreakingSystem" class="grid-7 omega">
                            	    <span filterid="1" class="form-control grid-6 checkOption">Yes</span>
                            	    <span filterid="2" class="form-control grid-6 checkOption">No</span>                                
                                </div>
                            </div>
                    
                         <div class="grid-12 alpha omega margin-bottom20 clear">
                        	    <div class="grid-5 alpha">
                            	    <h3>Brakes</h3>
                                </div>
                                <div name="braketype" class="grid-7 omega">
                            	    <span filterid="2" class="form-control grid-6 checkOption">Disc</span>
                            	    <span filterid="1" class="form-control grid-6 checkOption">Drum</span>                                
                                </div>
                            </div>
                        
                             <div class="grid-12 alpha omega margin-bottom20 clear">
                        	    <div class="grid-5 alpha">
                            	    <h3>Wheels</h3>
                                </div>
                                <div name="alloywheel" class="grid-7 omega">
                            	    <span filterid="1" class="form-control grid-6 checkOption">Alloy</span>
                            	    <span filterid="2" class="form-control grid-6 checkOption">Spoke</span>                                
                                </div>
                            </div>
                        
                             <div class="grid-12 alpha omega margin-bottom20 clear">
                        	    <div class="grid-5 alpha">
                            	    <h3>Start type</h3>
                                </div>
                                <div name="starttype" class="grid-7 omega">
                            	    <span filterid="1" class="form-control grid-6 checkOption">Electric</span>
                            	    <span filterid="2" class="form-control grid-6 checkOption">Kick</span>                                
                                </div>
                            </div>

			    </div>
            
                <!--Button starts here-->
                <div class="popup-btn-filters hide text-center">
                    <div class="margin-left10 margin-right10">
                        <input type="button" id="btnApplyFilters" class="btn btn-orange btn-full-width" value="Apply Filters" />
                    </div>
                </div>
                <!--Button ends here-->
                <!--Filters ends here-->
                <div class="clear"></div>
            </div>
            <!--Main container ends here-->
        <!-- #include file="/includes/footerBW_Mobile.aspx" -->
        <!-- all other js plugins -->    
        <!-- #include file="/includes/footerscript_Mobile.aspx" -->
        <script src="<%= staticUrl != "" ? "http://st2.aeplcdn.com" + staticUrl : "" %>/m/src/new/search.js?15sept2015" type="text/javascript"></script>
        <BW:MPopupWidget runat="server" ID="MPopupWidget1" />        
    </body>
</html>