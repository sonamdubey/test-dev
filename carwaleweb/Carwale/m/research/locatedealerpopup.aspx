<%@ Page Language="C#" AutoEventWireup="false"%>
<%
	Title = "Find New Car Showrooms in India - CarWale";
	Description = "Locate New Car Dealers in your City - CarWale";
    //Canonical = "https://www.carwale.com" + Request.ServerVariables["HTTP_X_REWRITE_URL"].ToString().Replace("/m/", "/");
    Canonical = "https://www.carwale.com/new/locatenewcardealers.aspx";
    MenuIndex = "2";
    IsOldJquery = "false";
    SectionName = "LocateDealer";
%>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
<!-- #include file="../includes/global-scripts.aspx" -->
<link rel="stylesheet" href="/static/m/css/design.css" type="text/css" >
<style>
    .m-defaultAlert-window { background: #000; position: fixed; top: 0px; left: 0px; right: 0px; bottom: 0px; opacity: .5; z-index: -1; -ms-filter: "progid:DXImageTransform.Microsoft.Alpha(Opacity=50)"; filter: alpha(opacity=50); -moz-opacity: 0.5; -khtml-opacity: 0.5; }
    .noFound.content-inner-block-10{padding-top:10px; padding-bottom:10px}
</style>
</head>

<body>
<form id="form1" runat="server">
	<!--Outer div starts here-->
	<div data-role="page" >
    	<!--Main container starts here-->
    	<div id="main-container">
			<!-- #include file="../includes/global-header.aspx" -->
<div class="inner-container light-shadow">
    <div class="margin-top10 new-input-style">

        <div class="makeDiv fixedSearchPopup">
            <div data-role="header" data-theme="b" class="ui-corner-top" data-icon="delete">
                <div class="white-text bold-text padding-all10 text-center">Select Make</div>
            </div>
            <div class="m-loading-popup">
                <span class="m-defaultAlert-window"></span>
                <span class="m-loading-icon"></span>
                <div class="clear"></div>
            </div>
            <div class="search-box cross-box-wrap">
                    <span class="cross-box hide" onclick="clearSearchText(this)">
                        <span class="cwmsprite cross-md-dark-grey"></span>
                    </span>
                    <input id="searchBox" type="text" placeholder="- Type to select Make -" />
                </div>
            <div class="popup_content hide">
                <div data-role="content" data-theme="d" class="ui-corner-bottom ui-content">
                    <ul id="make" data-role="listview" data-bind="template: { name: 'car-make-template', foreach: CarMakes }">
                    </ul>
                    <script type="text/html" id="car-make-template">
                        <li onclick="makeChanged(this);" class="makeLi filter-li" data-bind='attr:{value:makeId,text:makeName}'>
                            <a href="#" class="ui-btn ui-btn-icon-right ui-icon-carat-r"><span data-bind="text:makeName"></span></a>
                        </li>
                    </script>
                </div>
            </div>
        </div>


        <div class="cityDiv fixedSearchPopup hide">
                            <div data-role="header" data-theme="b" class="ui-corner-top" data-icon="delete">
                                <div class="floatleft cw-m-sprite city-back-btn" onclick="hideCitypopup(this)"></div>
                                <div class="white-text bold-text padding-all10 text-center">Select City</div>
                            </div>
                            <div class="m-loading-popup">
                                <span class="m-defaultAlert-window"></span>
                                <span class="m-loading-icon"></span>
                                <div class="clear"></div>
                            </div>
                            <div class="popup_content hide">
                                <div class="main-cities">
                                    <ul id="main-citiesList">
                                         <!--ko 'if':checkCityInArray(1) --> 
                                        <li class="moreCities popcity" city="mumbai" onclick="maincityChanged(this)" value="1">
                                            <div class="city-content popcity">
                                                <div class="city-icons mumbai"></div>
                                                <div class="margin-top5 cityname">Mumbai</div>
                                            </div>
                                        </li>
                                         <!-- /ko -->
                                        <!--ko 'if':checkCityInArray(10) -->
                                        <li class="moreCities popcity" city="new delhi" onclick="maincityChanged(this)" value="10">
                                            <div class="city-content popcity">
                                                <div class="city-icons delhi"></div>
                                                <div class="margin-top5 cityname">New Delhi</div>
                                            </div>
                                        </li>
                                        <!-- /ko -->
                                        <!--ko 'if':checkCityInArray(2) -->
                                        <li class="main-citiesList popcity" city="bangalore" onclick="maincityChanged(this)" value="2">
                                            <div class="city-content popcity">
                                                <div class="city-icons bangalore"></div>
                                                <div class="margin-top5 cityname">Bangalore</div>
                                            </div>
                                        </li>
                                        <!-- /ko -->
                                        <!--ko 'if':checkCityInArray(176) -->
                                        <li class="main-citiesList popcity" city="chennai" onclick="maincityChanged(this)" value="176">
                                            <div class="city-content popcity">
                                                <div class="city-icons chennai"></div>
                                                <div class="margin-top5 cityname">Chennai</div>
                                            </div>
                                        </li>
                                        <!-- /ko -->
                                        <!--ko 'if':checkCityInArray(12) -->
                                        <li class="main-citiesList popcity" city="pune" onclick="maincityChanged(this)" value="12">
                                            <div class="city-content popcity">
                                                <div class="city-icons pune"></div>
                                                <div class="margin-top5 cityname">Pune</div>
                                            </div>
                                        </li>
                                        <!-- /ko -->
                                        <!--ko 'if':checkCityInArray(105) -->
                                        <li class="main-citiesList popcity" city="hyderabad" onclick="maincityChanged(this)" value="105">
                                            <div class="city-content popcity">
                                                <div class="city-icons hyderabad"></div>
                                                <div class="margin-top5 cityname">Hyderabad</div>
                                            </div>
                                        </li>
                                        <!-- /ko -->
                                        <!--ko 'if':checkCityInArray(198) -->
                                        <li class="main-citiesList popcity" city="kolkata" onclick="maincityChanged(this)" value="198">
                                            <div class="city-content popcity">
                                                <div class="city-icons kolkata"></div>
                                                <div class="margin-top5 cityname">Kolkata</div>
                                            </div>
                                        </li>
                                        <!-- /ko -->
                                        <!--ko 'if':checkCityInArray(128) -->
                                        <li class="main-citiesList popcity" city="ahmedabad" onclick="maincityChanged(this)" value="128">
                                            <div class="city-content popcity">
                                                <div class="city-icons ahmedabad"></div>
                                                <div class="margin-top5 cityname">Ahmedabad</div>
                                            </div>
                                        </li>
                                        <!-- /ko -->
                                        <!--ko 'if':checkCityInArray(244) -->
                                        <li class="main-citiesList popcity" city="chandigarh" onclick="maincityChanged(this)" value="244">
                                            <div class="city-content popcity">
                                                <div class="city-icons chandigarh"></div>
                                                <div class="margin-top5 cityname">Chandigarh</div>
                                            </div>
                                        </li>
                                        <!-- /ko -->
                                    </ul>
                                    <div class="clear"></div>
                                </div>
                                <div class="citiesByState">More Cities by State</div>
                                <div class="States-list">
                                    <ul id="StatesList" data-role="listview" data-bind="template:{name:'state-template',foreach:statesAndCities}">
                                    </ul>
            
                                    <script type="text/html" id="state-template">
                                        <li onclick="stateChanged(this)" data-bind='attr:{text:stateName}'>
                                            <span data-bind="text:stateName"></span>
                                            <span class="cw-m-sprite li-nav-arrow floatright"></span>
                                        </li>
                                    </script>
                                </div>
            
                            </div>
            
                        </div>

        <div class="citiesByState-list fixedSearchPopup hide">
                            <div data-role="header" data-theme="b" class="ui-corner-top" data-icon="delete">
                                <div class="floatleft cw-m-sprite city-back-btn" onclick="hideCitypopup(this)"></div>
                                <div class="white-text bold-text padding-all10 text-center">Select City</div>
                            </div>
                            <div class="m-loading-popup">
                                <span class="m-defaultAlert-window"></span>
                                <span class="m-loading-icon"></span>
                                <div class="clear"></div>
                            </div>
                            <div class="search-box cross-box-wrap">
                                    <span class="cross-box hide" onclick="clearSearchText(this)">
                                        <span class="cwmsprite cross-md-dark-grey"></span>
                                    </span>
                                     <input id="searchBoxCity" type="text" placeholder="- Type to select City -" />
                                </div>
                            <div class="popup_content hide">
                                
                                <div data-role="content" data-theme="d" class="ui-corner-bottom ui-content">
                                <ul id="citiesByStatesList" class="selectArea" data-role="listview" data-bind="template:{name:'citiesByStates-template',foreach:citiesByStates}">
                                </ul>
            
                                <script type="text/html" id="citiesByStates-template">
                                    <li onclick="cityChanged(this)" class="filter-li" data-bind='attr:{value:cityId,text:cityName}'>
                                        <span data-bind="text:cityName"></span>
                                    </li>
                                </script>
                               </div>
                            </div>
                        </div>
      </div>
    </div>
   </div>
  </div>     
</form>
</body>
<script type="text/javascript">
    var makeId="";
    var makeName="";
    var cityId="";
    var cityName="";
    var maincitieshtml = $('#main-citiesList').html();
    function makeChanged(make) {
        if(typeof(make) == "undefined"){OpenCityPopup();}
        else {
        makeId = $(make).val();
        makeName = $(make).text().trim();
        dataLayer.push({ event: 'locate_dealer_section', cat: 'make_city_popup', act: 'make_click', lab: $(make).attr('text').toLowerCase() }); 
        OpenCityPopup(make);
        }
    }

    //      /m/new/hyundai-dealers/105-Hyderabad.html
    function cityChanged(cityDiv){
        cityId=$(cityDiv).val();
        cityName=$(cityDiv).text().trim();
        $(".citiesByState-list").find("div.m-loading-popup").show()
        dataLayer.push({ event: 'locate_dealer_section', cat: 'make_city_popup', act: 'nonpopular_city_click', lab: $(cityDiv).attr('text').toLowerCase() }); 
        location.href = "/m/" + makeName.toLowerCase().replace(new RegExp(" ", 'g'), "").replace(new RegExp("-", 'g'), "") + "-dealer-showrooms/" + cityName.toLowerCase().replace(new RegExp(" ", 'g'), "").replace(new RegExp("-", 'g'), "") + "-" + cityId;
    }

    function maincityChanged(mainCityDiv){
        cityId=$(mainCityDiv).val();
        cityName=$(mainCityDiv).find('div .cityname').text();
        $(".cityDiv").find("div.m-loading-popup").show()
        dataLayer.push({ event: 'locate_dealer_section', cat: 'make_city_popup', act: 'popular_city_click', lab: $(mainCityDiv).attr('city') }); 
        location.href = "/m/" + makeName.toLowerCase().replace(new RegExp(" ", 'g'), "").replace(new RegExp("-", 'g'), "") + "-dealer-showrooms/" + cityName.toLowerCase().replace(new RegExp(" ", 'g'), "").replace(new RegExp("-", 'g'), "") + "-" + cityId + "/";
    }
    function hideCitypopup(back) {
        var parent=$(back).parent().parent();
        parent.hide();
        if($(parent).hasClass("citiesByState-list")) {
            dataLayer.push({ event: 'locate_dealer_section', cat: 'make_city_popup', act: 'back_button', lab: 'back_from_cities' });             
            makeChanged();
            return;}
        else if($(parent).hasClass("cityDiv")) {
            dataLayer.push({ event: 'locate_dealer_section', cat: 'make_city_popup', act: 'back_button', lab: 'back_from_states' });                         
            openMakePopup();
            $('#searchBox').val('').trigger('keyup');
            if ($('div.noFound').length > 0) $('div.noFound').remove();
            return;}
    }

    function openMakePopup() {
        var currentPopup = $(".makeDiv");
        showLoadingForDiv(currentPopup, currentPopup.prev());//nothing to hode so passing prev()
        $('#searchBox').val('');
        $.ajax({
            type: 'GET',
            url: '/webapi/CarMakesData/GetCarMakes/?type=new',
            dataType: 'Json',
            success: function (json) {
                selectedModelId = json[0].ModelId;
                var viewModel = {
                    CarMakes: ko.observableArray(json)
                };
                ko.cleanNode(document.getElementById("make"));
                ko.applyBindings(viewModel, document.getElementById("make"));
                hideLoadingForDiv(currentPopup);
                $('#searchBox').cw_fastFilter('#make');
            }
        });
    }

    function OpenCityPopup(selectedMake) {
        if (makeId < 0 || makeId == "") {

            var errorMsg = "Please choose a Make first, so that we can show only the specific city options where it is available."
            $('.m-defaultAlert-window').show();
            $('.m-defaultAlert').show();
            $('.m-defaultAlertContent').text(errorMsg);
        }
        else {
        
            var currentPopup = $(".cityDiv");
            showLoadingForDiv(currentPopup,currentPopup.prev());
            $.ajax({
                type: 'GET',
                url: '/webapi/newcardealers/cities/?makeid=' + makeId,
                dataType: 'Json',
                success: function (json) {
                    var viewModel = {
                        statesAndCities: ko.observableArray(json.Item1)
                    };
                    var mainCitiesViewModel = {
                        statesAndCities: ko.observableArray(json.Item2)
                    };
                    
                    ko.cleanNode(document.getElementById("StatesList"));
                    ko.applyBindings(viewModel, document.getElementById("StatesList"));

                    ko.cleanNode(document.getElementById("main-citiesList"));
                    $('#main-citiesList').html(maincitieshtml);
                    ko.applyBindings(mainCitiesViewModel, document.getElementById("main-citiesList"));

                    hideLoadingForDiv(currentPopup);
                }
            });
        }
    }
    
    function showLoadingForDiv(currentPopup, prevPopup) {
        prevPopup.hide();
        currentPopup.find("div.popup_content").hide();
        currentPopup.find("div.m-loading-popup").show();
        currentPopup.addClass("popup_layer").show().scrollTop(0);
        window.scrollTo(0, 0);
    }

    function hideLoadingForDiv(currentPopup) {
        currentPopup.find("div.popup_content").show();
        currentPopup.find("div.m-loading-popup").hide();
    }

    /* cw_fastFilter for popup search box code starts here */
    jQuery.fn.cw_fastFilter = function (list, options) {
        // Options: input, list, timeout, callback
        options = options || {};
        list = jQuery(list);
        var input = this;
        var lastFilter = '', noFoundLen = 0;
        var noFoundDiv = '<div class="noFound content-inner-block-10 text-red">No search found!</div>';
        var crossBox = '.cross-box-wrap .cross-box';
        var selCrossBox = list.closest('.fixedSearchPopup').find(crossBox);
        var timeout = options.timeout || 100;
        var callback = options.callback || function (total) {
            noFoundLen = list.siblings("div.noFound").length;
            if (input.val() != "") selCrossBox.show();
            else selCrossBox.hide();
            //no search found text
            if (total == 0 && noFoundLen < 1) list.after(noFoundDiv).show();
            else if (total > 0 && noFoundLen > 0) $('div.noFound').remove();
        };

        var keyTimeout;
        var lis = list.children();
        var len = lis.length;
        var oldDisplay = len > 0 ? lis[0].style.display : "block";
        callback(len); // do a one-time callback on initialization to make sure everything's in sync

        input.change(function () {
            // var startTime = new Date().getTime();
            var filter = input.val().toLowerCase();
            var li, innerText;
            var numShown = 0;
            for (var i = 0; i < len; i++) {
                li = lis[i];
                innerText = !options.selector ?
                    (li.textContent || li.innerText || "") :
                    $(li).find(options.selector).text();

                if (innerText.toLowerCase().indexOf(filter) >= 0) {
                    if (li.style.display == "none") {
                        li.style.display = oldDisplay;
                    }
                    numShown++;
                } else {
                    if (li.style.display != "none") {
                        li.style.display = "none";
                    }
                }
            }
            callback(numShown);
            return false;
        }).keydown(function () {
            clearTimeout(keyTimeout);
            keyTimeout = setTimeout(function () {
                if (input.val() === lastFilter) return;
                lastFilter = input.val();
                input.change();
            }, timeout);
        });
        return this; // maintain jQuery chainability
    }
    /* cw_fastFilter for popup search box code ends here */

    function clearSearchText(txtBox) {
        var $this = $(txtBox);
        var $srachTxtBox = $this.closest('.cross-box-wrap').find('input[type="text"]');
        $srachTxtBox.val("").keydown().focus();
        $('div.noFound').remove();
        $this.hide();
    }

    function stateChanged(selectedState) {
        var prevPopup = $(".cityDiv");
        var currentPopup = $('.citiesByState-list');
        dataLayer.push({ event: 'locate_dealer_section', cat: 'make_city_popup', act: 'state_select_from_stateslist', lab: $(selectedState).attr('text').toLowerCase() });
        showLoadingForDiv(currentPopup, prevPopup);

        var cities=ko.dataFor(selectedState).cities;
        var viewModel = {
            citiesByStates: ko.observableArray(cities)
        };
        ko.cleanNode(document.getElementById("citiesByStatesList"));
        ko.applyBindings(viewModel, document.getElementById("citiesByStatesList"));
        hideLoadingForDiv(currentPopup);
        $('#searchBoxCity').val('');
        $('#searchBoxCity').cw_fastFilter('#citiesByStatesList');
    }

    $(document).ready(function(){
        $('#searchBox').val('').trigger('keyup');
        openMakePopup();
        dataLayer.push({ event: 'locate_dealer_section', cat: 'make_city_popup', act: 'initial_open', lab: 'pageload' });
    });

    function checkCityInArray(cityId)
    {
        var array = ko.dataFor($('#main-citiesList')[0]).statesAndCities();
        console.log(array.length);
        var i = 0;
        for (i = 0; i < array.length; i++)
        {
            if (array[i].cityId == cityId) {return true; }
        }
        
        return false;
    }
</script>
    <!-- Google Tag Manager -->
<noscript><iframe src="//www.googletagmanager.com/ns.html?id=GTM-W2Z3ZM"
height="0" width="0" style="display:none;visibility:hidden"></iframe></noscript>
<script>(function (w, d, s, l, i) {
    w[l] = w[l] || []; w[l].push({
        'gtm.start':
        new Date().getTime(), event: 'gtm.js'
    }); var f = d.getElementsByTagName(s)[0],
    j = d.createElement(s), dl = l != 'dataLayer' ? '&l=' + l : ''; j.async = true; j.src =
    '//www.googletagmanager.com/gtm.js?id=' + i + dl; f.parentNode.insertBefore(j, f);
})(window, document, 'script', 'dataLayer', 'GTM-W2Z3ZM');</script>
<!-- End Google Tag Manager -->
</html>
