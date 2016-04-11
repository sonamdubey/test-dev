<%@ Page Language="C#" AutoEventWireup="false" Inherits="Bikewale.Mobile.New.ListNewBikeDealersByCity" %>
<%@ Register TagPrefix="BW" TagName="MPopupWidget" Src="/m/controls/MPopupWidget.ascx" %>
<!DOCTYPE html>
<html>
<head>
    <!-- #include file="/includes/headscript_mobile.aspx" -->
    <style type="text/css">
        #cityHeader{background:#313131;color:#fff;width:100%;height:48px;overflow:hidden;z-index:2;}.city-back-btn {padding:12px 15px;cursor:pointer;}.fa-arrow-back{width:12px;height:20px;background-position:-63px -162px;}.city-header-text { width:80%; text-align:left; text-overflow:ellipsis; white-space:nowrap; overflow:hidden; }.padding-top48 { padding-top:48px; }.box-shadow { background:#fff; -webkit-box-shadow:1px 1px 1px #e2e2e2; -moz-box-shadow:1px 1px 1px #e2e2e2; box-shadow:1px 1px 1px #e2e2e2; }.text-pure-black { color:#000; }
        #citySearchHeader { background:#fff; width:100%; }                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                             
        .city-input-search-box span.search-icon-grey {position: absolute;right:10px;top:10px;cursor: pointer;z-index:2; background-position:-34px -275px; }
        .city-input-search-box .fa-spinner {display:none;right:14px;top:12px;z-index:3;}
        #defaultStateList, #filteredCityList { padding:0 20px; }
        #filteredCityList { display:none; }
        #defaultStateList h2 { color:#4d5057; }
        #defaultStateList > li, #filteredCityList > li { padding-top:15px; padding-bottom:15px; border-top:1px solid #f1f1f1; }
        #defaultStateList > li:first-child, #filteredCityList > li:first-child { padding-top:5px; border-top:0; }
        #defaultStateList > li h2 {font-size:16px; font-weight:400; cursor:pointer;}
        #defaultStateList > li h2:hover { color:#2a2a2a; }
        .citySubList { display:none; }
        .citySubList li { margin-top:15px; padding-left:5px; }
        .citySubList a { font-size:16px; color:#82888b; }
        .citySubList a:hover, #filteredCityList > li a:hover { text-decoration:none; color:#4d5057; }
        .pq-fixed {position: fixed;top: 0;left: 0px;padding-top:15px;padding-right:25px;padding-left:25px;z-index: 9;}
        .padding-top125 { padding-top:125px; }
        #filteredCityList > li a { font-size:16px; color:#4d5057; }
        #filteredCityList > li a:hover { color:#2a2a2a; }
    </style>
</head>
<body class="bg-light-grey">
    <form runat="server">
        <header id="cityHeader">
            <div class="leftfloat city-back-btn">
                <a href="javascript:void(0)"><span class="bwmsprite fa-arrow-back"></span></a>
            </div>
            <div class="city-header-text leftfloat margin-top10 font18">Select city</div>
            <div class="clear"></div>
        </header>

        <section class="container margin-top10 margin-bottom30">
            <div class="grid-12">
                <div id="inputAndListWrapper" class="box-shadow padding-top15">
                    <div id="citySearchHeader" class="padding-right20 padding-left20">
                        <h1 class="font16 text-pure-black border-solid-bottom padding-bottom15 margin-bottom10">Bajaj dealers in India</h1>
                        <div class="city-input-search-box form-control-box padding-bottom10">
                            <span class="bwmsprite search-icon-grey"></span>
                            <input type="text" class="form-control padding-right40" placeholder="Type to select city" id="getCityInput" />
                            <span class="fa fa-spinner fa-spin position-abt text-black"></span>
                        </div>
                    </div>
                    <div id="stateCityListWrapper">
                        <ul id="defaultStateList">
                             <li>
                                <h2 class="font16 text-unbold">Andhra Pradesh</h2>
                                <ul class="citySubList">
                                    <li>
                                        <a href="#"><span class="city-name">Adilabad</span> <span>(<span>8</span>)</span></a>
                                    </li>
                                    <li>
                                        <a href="#"><span class="city-name">Chirala</span> <span>(<span>9</span>)</span></a>
                                    </li>
                                    <li>
                                        <a href="#"><span class="city-name">Ponnur</span> <span>(<span>20</span>)</span></a>
                                    </li>
                                    <li>
                                        <a href="#"><span class="city-name">Vizianagaram</span> <span>(<span>14</span>)</span></a>
                                    </li>
                                </ul>
                            </li>
                            <li>
                                <h2>Arunachal Pradesh</h2>
                                <ul class="citySubList">
                                    <li>
                                        <a href="#"><span class="city-name">Naharlagun</span> <span>(<span>24</span>)</span></a>
                                    </li>
                                </ul>
                            </li>
                            <li>
                                <h2>Gujarat</h2>
                                <ul class="citySubList">
                                    <li>
                                        <a href="#"><span class="city-name">Ahmedabad</span> <span>(<span>4</span>)</span></a>
                                    </li>
                                    <li>
                                        <a href="#"><span class="city-name">Patan</span> <span>(<span>12</span>)</span></a>
                                    </li>
                                    <li>
                                        <a href="#"><span class="city-name">Vadodara</span> <span>(<span>18</span>)</span></a>
                                    </li>
                                    <li>
                                        <a href="#"><span class="city-name">Kutch</span> <span>(<span>22</span>)</span></a>
                                    </li>
                                </ul>
                            </li>
                            <li>
                                <h2>Goa</h2>
                                <ul class="citySubList">
                                    <li>
                                        <a href="#"><span class="city-name">Ponda</span> <span>(<span>9</span>)</span></a>
                                    </li>
                                    <li>
                                        <a href="#"><span class="city-name">Mapusa</span> <span>(<span>4</span>)</span></a>
                                    </li>
                                </ul>
                            </li>
                            <li>
                                <h2>Maharashtra</h2>
                                <ul class="citySubList">
                                    <li>
                                        <a href="#"><span class="city-name">Mumbai</span> <span>(<span>21</span>)</span></a>
                                    </li>
                                    <li>
                                        <a href="#"><span class="city-name">Panvel</span> <span>(<span>15</span>)</span></a>
                                    </li>
                                    <li>
                                        <a href="#"><span class="city-name">Chiplun</span> <span>(<span>9</span>)</span></a>
                                    </li>
                                    <li>
                                        <a href="#"><span class="city-name">Solapur</span> <span>(<span>4</span>)</span></a>
                                    </li>
                                </ul>
                            </li>
                        </ul>
                        <ul id="filteredCityList"></ul>
                        <div id="citySearchRemoveHeader"></div>
                    </div>
                </div>
            </div>
            <div class="clear"></div>
        </section>
        
        <!-- #include file="/includes/footerBW_Mobile.aspx" -->
        <!-- #include file="/includes/footerscript_Mobile.aspx" -->
        <script type="text/javascript">
            $('#defaultStateList').on('click', 'h2', function () {
                var clickedState = $(this),
                    clickedStateParent = clickedState.parent('li');
                if (!clickedState.hasClass('active-state')) {
                    $('#defaultStateList h2').removeClass('active-state');
                    $('#defaultStateList ul.citySubList').hide();
                    clickedState.addClass('active-state');
                    $('html, body').animate({ scrollTop: clickedStateParent.offset().top - 100 });
                    clickedState.next('ul.citySubList').slideDown();
                }
                else if (clickedState.hasClass('active-state')) {
                    $('#defaultStateList ul.citySubList').hide();
                    clickedState.removeClass('active-state');
                }
            });

            $(document).ready(function () {
                var citySearchHeader = $('#citySearchHeader'),
                    stateCityListWrapper = $('#stateCityListWrapper'),
                    citySearchRemoveHeader = $('#citySearchRemoveHeader'),
                    $window = $(window);
                $window.scroll(function () {
                    if (!citySearchHeader.hasClass('pq-fixed')) {
                        if ($window.scrollTop() > citySearchHeader.offset().top && $window.scrollTop() < citySearchRemoveHeader.offset().top - 114) { //subtract 40px (pq header height)
                            citySearchHeader.addClass('pq-fixed');
                            stateCityListWrapper.addClass('padding-top125');
                        }
                    }
                    else if (citySearchHeader.hasClass('pq-fixed')) {
                        if ($window.scrollTop() < stateCityListWrapper.offset().top || $window.scrollTop() > citySearchRemoveHeader.offset().top - 114) { //subtract 40px (pq header height)
                            citySearchHeader.removeClass('pq-fixed');
                            stateCityListWrapper.removeClass('padding-top125');
                        }
                    }
                });

            });

            var userCityInput, userCityInputLength, element, elementName;
            var filteredCityList = $('#filteredCityList');

            $('#getCityInput').focus();

            $('#getCityInput').on('keyup', function () {
                userCityInput = $(this).val(),
                userCityInput = userCityInput.toLowerCase(),
                userCityInputLength = userCityInput.length;
                if (userCityInput != "") {
                    $('#defaultStateList').hide();
                    filteredCityList.empty();
                    $('#filteredCityList').show();
                    if ($('#defaultStateList').offset().top > $(window).scrollTop()) {
                        $('body, html').animate({ 'scrollTop': '50px' });
                    }
                    $('#defaultStateList .citySubList li').find('span.city-name').each(function () {
                        element = $(this);
                        elementName = element.text().toLowerCase().trim();
                        elementTargetLink = $(this).parent('a').attr('href');
                        if (/\s/.test(elementName))
                            var splitlocationName = elementName.split(" ")[1];
                        else
                            splitlocationName = "";

                        if ((userCityInput == elementName.substring(0, userCityInputLength)) || userCityInput == splitlocationName.substring(0, userCityInputLength)) {
                            filteredCityList.append('<li><a href=' + elementTargetLink + '><span class="city-name">' + element.text() + '</span> <span>(10)</span></a></li>');
                        }
                        else {

                        }
                    });
                }
                else {
                    $('#filteredCityList').hide();
                    $('#defaultStateList').show();

                }
            });

        </script>
    </form>
</body>
</html>
