//Global Variables 
// object for remarketing variables. Add variable here if require for remarketing code
var remarketingVariables = { carMake: "", carModel: "", price: "", cityName: "", sellerType: "", sellerDetailsClick: "", cityId: "", makeId: "", rootId: "" };
var checkBoxSelector = '.list-points li';
var sortSelector = '#sort'; $.countList = 0; $.nbCityData = false; var pageCountForNBCity, NBCITYTOTALCOUNT = 0; var qsTemp; var remainingCount, NBCITYCHANGED = false; $.pageNoNB; var noCarTimer = null; var PAGELOAD = false;
//Knockout- View Model for Listings 
var D_usedSearchPage = function () {
    var d_usedSearchPageSelf = this;
    d_usedSearchPageSelf.globalCity = function () {
        var _self = this;
        _self.showGlobalCityPopUp = function () {
            if (!$.cookie('_CustCityMaster') && !$.getFilterFromQS('city')) {
                $("#cityName").trigger("click");
                $("#globalCityPopUp").focus();
                $('body').addClass('lock-browser-scroll');
            }
        };
    };
    d_usedSearchPageSelf.pgNavigation = function () {
        var _self = this;
        _self.registerEvents = function () {
            bindNextArrowHoverEvent();
            bindPrevArrowHoverEvent();
            bindArrowClickEvent();
        }
        _self.bindNextStockListing = function (nodeSlideShow) {
            var nextListing = 1; var nextStockListCount = 0; nextCityLinkFlag = false;
            var loadNextStockList = getNextStockList(nodeSlideShow.closest('div.stock-list').nextAll('div.stock-list').first());
            if (!nodeSlideShow.closest('li').nextAll('li.listingContent').find('a.slideShow').eq(3).length && !loadNextStockList.length) {
                window.scrollTo(0, document.body.scrollHeight);
            }
            var nextStockList = nodeSlideShow.closest('div.stock-list').nextAll('div.stock-list').eq(nextStockListCount);
            nextListing = _self.getNextListing(nodeSlideShow.closest('li.listingContent'));
            var cityName = nextListing.closest('div.stock-list').find('#nbCitiesTitle').text().split('(')[0].substring(20).trim();

            while (!nextListing.length && nextStockList.length) {
                nextListing = nextStockList.find('li.listingContent img.lazy').first();
                if (nextListing.attr('data-original') == "https://imgd.aeplcdn.com/0x0/cw/design15/nocars-placeholder.jpg")
                    nextListing = _self.getNextListing(nextStockList.find('li.listingContent').first());

                cityName = nextListing.closest('div.stock-list').find('#nbCitiesTitle').text().split('(')[0].substring(20).trim();
                nextStockListCount++;
                nextCityLinkFlag = true;
                nextStockList = nodeSlideShow.closest('div.stock-list').nextAll('div.stock-list').eq(nextStockListCount);
            }
            _self.bindNBCityLink(nextListing, cityName);
        
        }
        _self.bindNBCityLink = function (nextListing, cityName) {

            if (nextListing.length && nextListing.closest('div.stock-list').children('#nbCitiesTitle').length && nextCityLinkFlag) {
                $("#pgNextCityCars").text("Show more Cars in " + cityName);
                $("#pgNextCityCars").attr('rankabs', nextListing.closest('li.listingContent').attr('rankabs'));
                $("#pgNextCityCars").attr('cityname', cityName);
                _self.hidePgNextArrow();
                nextCityLinkFlag = false;
                $('.nextBtn').unbind('mouseenter');
                $('.availNext').unbind('mouseleave');
                $('.nextBtn').mouseenter(function () {
                    $('.nextBtn').fadeOut();
                    $("#pgNextCityCars").animate({ right: '0', opacity: '1' });
                });
                $('#pgNextCityCars').mouseleave(function () {
                    $(this).animate({ right: '-167px', opacity: '0' });
                    $('.nextBtn').fadeIn();
                });
            }
            else if (nextListing.length) {
                _self.bindNextListing(nextListing);
                _self.showPgNextArrow();
            }
            else {
                _self.hidePgNextArrow();
                setTimeout(function () { $('.nextBtn').fadeOut(); }, 100)

            }
        }
        _self.bindPrevStockListing = function (nodeSlideShow) {
            var prevListing; var prevStockListCount = 0;
            prevListing = _self.getPrevListing(nodeSlideShow.closest('li.listingContent'));
            var prevStockList = nodeSlideShow.closest('div.stock-list').prevAll('div.stock-list').eq(prevStockListCount);
            while (!prevListing.length && prevStockList.length) {
                prevListing = prevStockList.find('li.listingContent img.lazy').last();
                if (prevListing.attr('data-original') == "https://imgd.aeplcdn.com/0x0/cw/design15/nocars-placeholder.jpg")
                    prevListing = _self.getPrevListing(prevStockList.find('li.listingContent').last());

                prevStockListCount++;
                prevStockList = nodeSlideShow.closest('div.stock-list').prevAll('div.stock-list').eq(prevStockListCount);
            }
            if (prevListing.length) {
                _self.showPgPrevArrow();
                _self.bindPrevListing(prevListing);
            }
            else
                _self.hidePgPrevArrow();
        }

        _self.getPrevListing = function (nodeLi) {
            var prevListCount = 0; var prevListing = nodeLi.prevAll('li.listingContent').find('a.slideShow img.lazy').eq(prevListCount);
            while (prevListing.length && prevListing.attr('data-original') == "https://imgd.aeplcdn.com/0x0/cw/design15/nocars-placeholder.jpg") {
                prevListCount++;
                prevListing = nodeLi.prevAll('li.listingContent').find('a.slideShow img.lazy').eq(prevListCount);
            }
            return prevListing;
        }

        _self.getNextListing = function (nodeLi) {
            var nextListCount = 0;
            var nextListing = nodeLi.nextAll('li.listingContent').find('a.slideShow img.lazy').eq(nextListCount);
            while (nextListing.length && nextListing.attr('data-original') == "https://imgd.aeplcdn.com/0x0/cw/design15/nocars-placeholder.jpg") {
                nextListCount++;
                nextListing = nodeLi.nextAll('li.listingContent').find('a.slideShow img.lazy').eq(nextListCount);
            }
            return nextListing;
        }

        _self.bindNextListing = function (nextListing) {
            setTimeout(function () {
                $("#pgNextBtn img").attr('src', nextListing.attr('pg-navigation-images'))
                $("#pgNextBtn").attr('rankabs', nextListing.closest('li.listingContent').attr('rankabs'));
                $("#pgNextBtn .pgNextTitle").text(nextListing.closest('div.stock-detail').find('span.spancarname').text());
            }, 400);
        
        }
        _self.bindPrevListing = function (prevListing) {
            setTimeout(function () {
                $("#pgPrevBtn img").attr('src', prevListing.attr('pg-navigation-images'))
                $("#pgPrevBtn").attr('rankabs', prevListing.closest('li.listingContent').attr('rankabs'));
                $("#pgPrevBtn .pgPrevTitle").text(prevListing.closest('div.stock-detail').find('span.spancarname').text());
            }, 400);
        }

        _self.hidePgNextArrow = function () {
            $("#nextBtnArrow").addClass('unbindNavigation');
            // $("#pgNextBtn").hide();
            $('.nextBtn').unbind('mouseenter');
            $('.availNext').unbind('mouseleave');
        }
        _self.showPgNextArrow = function () {
            if ($("#nextBtnArrow").hasClass('unbindNavigation')) {
                $('.nextBtn').unbind('mouseenter');
                $('.availNext').unbind('mouseleave');

                setTimeout(function () { bindNextArrowHoverEvent(); }, 200);

                setTimeout(function () { $('.nextBtn').fadeIn(); }, 100)
                // $("#pgNextBtn").show();
                $("#nextBtnArrow").removeClass('unbindNavigation');


            }
        }
        _self.hidePgPrevArrow = function () {
            $("#prevBtnArrow").addClass('unbindNavigation');
            //$("#pgPrevBtn").hide();
            $('.prevBtn').unbind('hover');
            $('.availPrev').unbind('mouseleave');
            setTimeout(function () { $('.prevBtn').fadeOut(); }, 100);

        }

        _self.showPgPrevArrow = function () {
            if ($("#prevBtnArrow").hasClass('unbindNavigation')) {
                $('.prevBtn').unbind('hover');
                $('.availPrev').unbind('mouseleave');
                setTimeout(function () { bindPrevArrowHoverEvent(); }, 200);

                $('.prevBtn').fadeIn();
                // $("#prevBtnArrow").show();
                // $("#pgPrevBtn").show();
                $('#prevBtnArrow').removeClass('unbindNavigation');

            }
        }

        _self.bindNavigatingArrowsOnPG = function (nodePG, prevPgListingId) {

            var nextListing, prevListing, currentListing; var nodeStockList = nodePG.closest('div.stock-list');
            var prevStockListCount = 0, nextStockListCount = 0;
            $('.availNext').trigger('mouseleave');
            $('.availPrev').trigger('mouseleave');
            _self.bindNextStockListing(nodePG);
            _self.bindPrevStockListing(nodePG);


        }
    };

    d_usedSearchPageSelf.nbCityApiHit = function () {
        var _self = this;
        _self.allIndiaCityID = "9999";
        _self.getNextNbCityWithCount = function () {
           var lastNBCityCount = 0;
            var nbCityInfo = {
                totalNBCityCount: 0,
                nbCitiesId: "",
                isLazyLaod:true
            };
            for (var i = 0; i <= 10; i++) {
                var nextNBCityElem = $("#nearByCityList .selected").removeClass('selected').next();
                var nextNBCityId = nextNBCityElem.attr('cityid');
                lastNBCityCount = parseInt(nextNBCityElem.addClass('selected').find('.city-count').text());
                if ((nextNBCityId == _self.allIndiaCityID && !$('#nearByCityList li').last().is(':visible')) || nextNBCityElem.find('.city-count').text() == "0") {
                    if (nextNBCityId == _self.allIndiaCityID) {
                        nextNBCityElem.prev().addClass('selected');
                         break;
                    }
                }
                else {
                    nbCityInfo.nbCitiesId = nbCityInfo.nbCitiesId + ',' + nextNBCityId;
                  
                    nbCityInfo.totalNBCityCount = nbCityInfo.totalNBCityCount + lastNBCityCount;
                    pageCountForNBCity = Math.ceil(lastNBCityCount / 20);
                }
                if (nbCityInfo.totalNBCityCount > 20 || !$("#nearByCityList .selected").next().length) {
                    break;
                }
            }
            $.pageNoNB = 1;
            nbCityInfo.nbCitiesId = nbCityInfo.nbCitiesId.slice(1);
            if (nbCityInfo.totalNBCityCount > 20)
                nbCityInfo.isLazyLaod = false;

            return nbCityInfo;
        }
    };
    
    d_usedSearchPageSelf.PageLoad = function () {
        d_usp_pgNavigation.registerEvents();
        D_usedSearch.cityPopup.registerEvents();
        D_usedSearch.globalCityArea.registerEvents();
        D_usedSearch.noCars.registerEvents();
        D_usedSearch.similarCars.registerEvents();
        D_usedSearch.budgetSelection.registerEvents();
        D_usedSearch.photoGallery.registerEvents();
        D_usedSearch.Finance.registerEvents();
        D_usedSearch.cityWarning.registerEvents();
        D_usedSearch.Valuation.registerEvents();
		D_usedSearch.windowEventHandler.registerEvents();
    };
};
        
var D_usedSearch = {
    doc: $(document),
    trackingCategory: "UsedSearchPage",
    isFromPageLoad: false,
    latestNonPremiumCarRank: 0,
    latestPremiumDealerCarRank: 0,
    latestPremiumIndividualCarRank: 0,
    enumCityId: {
        MumbaiAll: 3000,
        DelhiNCR: 3001
    },
    windowEventHandler: {
        registerEvents: function(){
            $(document).on('keydown', D_usedSearch.windowEventHandler.keydownEventHandler);
            $(window).on('popstate', D_usedSearch.windowEventHandler.popstateEventHandler);
            if ('scrollRestoration' in history) {
                history.scrollRestoration = 'manual';
            }
        },
        keydownEventHandler: function (e) {
            if (e.keyCode == 27) {
                $('.popup-close-esc-key').each(function () {
                    if ($(this).is(':visible')){
                        $(this).trigger('click');
                    }
                });
            }
        },
        popstateEventHandler: function (e) {
            if ($('#financeIframe').is(':visible')) {
                D_usedSearch.Finance.closeFinanceForm();
            }
            else if($('.valuation-popup-content').is(':visible')) {
                D_usedSearch.Valuation.closePopup();
            }
        }
    },
    photoGallery: {
        registerEvents: function () {
            $(document).on('mouseout', '.pg-prev', function () {
                D_usedSearch.photoGallery.showPrevArrowOnMouseout();
            });
            $(document).on('mouseout', '.pg-next', function () {
                D_usedSearch.photoGallery.showNextArrowOnMouseout();
            });
        },
        bindNextAvailableCar: function () {
            $('div.availNext').animate({ right: '0', opacity: '1' });
        },
        hideNextAvailableCar: function () {
            $('div.availNext').animate({ right: '-167px', opacity: '0' });
        },
        showNextArrowOnMouseout: function(){
            $('.pg-next-image').css('display', 'block');
        },
        showPrevArrowOnMouseout: function () {
            $('.pg-prev-image').css('display', 'block');
        },
        hidePgArrow: function(){
            if ($('.pg-gallery .pg-thumbs li').length <= 1) {
                $('.pg-prev, .pg-next').hide();
            }
        },
        showDeliveryNotice: function (thisElement) {
            var stockCity = thisElement.find('span.cityName').text();
            var lstDeliveryText = thisElement.find('.delivery-text');
            var pgSellerDetailEle = $('#photoGallery');
            var pgDeliveryText = pgSellerDetailEle.find('p.pg-deliverytext');
            var pgCarLocationIcon = pgSellerDetailEle.find('div.pg-car-location-ic');
            pgSellerDetailEle.find('span.pg-carcityname').text(stockCity);
            if (lstDeliveryText.length > 0 && lstDeliveryText.text().trim()) {//premium muticity dealer condition
                $('#photoGallery .pg-delivery').show();
                if (pgDeliveryText.hasClass('hide')) {
                    pgDeliveryText.removeClass('hide');
                }
                pgCarLocationIcon.addClass('hide');
                pgDeliveryText.text(lstDeliveryText.text());
            }
            else {
                if (D_usedSearch.cityWarning.checkPgCityWarning(thisElement)) {
                    pgDeliveryText.addClass('hide');
                    if (pgCarLocationIcon.hasClass('hide')) {
                        pgCarLocationIcon.removeClass('hide');
                    }
                }
            }
        },
    },
    cityPopup: {
        registerEvents: function () {
            $(document).on("mastercitychange", function (event, cityName, cityId) {
                $('#closeLocIcon').show(); // show the location popup's close button and make blackout window clickable.
                $('body').removeClass('no-action-bg');
                if ($.globalCityChange != undefined)
                    $.globalCityChange(cityId, cityName);
            });
        }
    },
    globalCityArea: {
        registerEvents: function () {
            $(document).on('click', 'div.changeCityBlackout, #results li', function () {
                D_usedSearch.globalCityArea.closeGlobalAreaBlackWindow();
            });
            $(document).on('keyup', '#placesQuery', function (e) {
                if (e.keyCode == 13 && $('div.blackOut-window-bt').is(':visible'))
                    D_usedSearch.globalCityArea.closeGlobalAreaBlackWindow();
            });
        },
        changeGlobalAreaFromNoCar: function () {
            $('#placesQuery').focus();
            $('div.blackOut-window-bt').addClass('changeCityBlackout').show();
        },
        closeGlobalAreaBlackWindow: function () {
            $('div.blackOut-window-bt').removeClass('changeCityBlackout').hide();
        }
    },
    globalCity: {
        isGlobalCityPresent: function () {
            // return boolean if cookie for GlobalCity is present
            if ($.cookie('_CustCityIdMaster') != "-1" && $.cookie('_CustCityIdMaster') != null)
                return true;
            else return false;

        },
        getGlobalCityId: function () {
            if ($.cookie('_CustCityIdMaster')) {
                return $.cookie('_CustCityIdMaster');
            }
            else return -1;
        },
        getGlobalCityName: function () {
            if ($.cookie('_CustCityMaster')) {
                return $.cookie('_CustCityMaster');
            }
        }
    },
    noCars: {
        registerEvents: function () {
            $(document).on('click', '#noCarsFound span.changeCitylink', function () {
                Location.globalSearch.openLocHint();
                Location.globalSearch.ipDivHide();
                if (!Location.ms_ie)
                    Location.txtPlacesQuery.focus();
                //D_usedSearch.noCars.changeCityFromNoCarLink();
            });
        },
        changeCityFromNoCarLink: function () {
            D_usedSearch.globalCityArea.changeGlobalAreaFromNoCar();
        },
    },   
    cityWarning: {
        cityWarningDiv: $('div.carsinCity'),
        cityId: "",
        isUserCityChanged: false,
        showCityWarningOnSoldOut: false,
        registerEvents: function () {
            $(document).on('click', 'a.changeCityLink', function () {
                Location.globalSearch.openLocHint();
                Location.globalSearch.ipDivHide();
                if (!Location.ms_ie)
                    Location.txtPlacesQuery.focus();
            });

            D_usedSearch.doc.on('click', '#cityWarningClose', function () {
                D_usedSearch.cityWarning.closeCityWarningBox();
            });

            D_usedSearch.doc.on('click', '.pg_changeCityLink', function () {
                if ($('#photoGallery').is(':visible'))
                    $('#pg-btn-close').trigger('click');
                //D_usedSearch.cityWarning.changeGlobalCity();
                Location.globalSearch.openLocHint();
                Location.globalSearch.ipDivHide();
                if (!Location.ms_ie)
                    Location.txtPlacesQuery.focus();
            });
        },
        isListingsPresent: function () {
            if (!($('.stock-list ul li').length > 0)) {
                D_usedSearch.cityWarning.resetWarning();
                return false;
            }
            return true;
        },
        closeCityWarningBox: function () {
            D_usedSearch.cityWarning.cityWarningDiv.hide();
        },
        changeGlobalCity: function () {
            D_usedSearch.globalCityArea.changeGlobalAreaFromNoCar();
        },
        checkWarningConditions: function () {
            var qsCityId = $.getFilterFromQS('city');
            D_usedSearch.cityWarning.cityId = qsCityId;
            var globalCityId = D_usedSearch.globalCity.getGlobalCityId();
            if (globalCityId != -1 || qsCityId != "") {
                if ($.fromCW) {
                    if ($.cookie('_CustCityUserAction')) {
                        document.cookie = "_CustCityUserAction=" + "; expires=Thu, 01 Jan 1970 00:00:00 UTC; path=/";
                        $.showCityWarning = false;
                    }
                    else {
                        $.showCityWarning = true;
                    }
                }
                else {
                    if (globalCityId != -1 && D_usedSearch.cityWarning.isCitySame(globalCityId, qsCityId)) {
                        $.showCityWarning = false;
                    }
                    else {
                        $.showCityWarning = true;
                    }
                }
            }
            else {
                $.showCityWarning = true;
            }
        },
        toggleCityWarning: function () {
            if ($.showCityWarning) {
                D_usedSearch.cityWarning.cityWarningDiv.show();
                D_usedSearch.cityWarning.setWarningCityName();
            }
            else {
                D_usedSearch.cityWarning.cityWarningDiv.hide();
            }
        },
        isCitySame: function (globalCityId, qsCityId) {
            if (qsCityId == D_usedSearch.enumCityId.MumbaiAll)
                qsCityId = 1;
            else if (qsCityId == D_usedSearch.enumCityId.DelhiNCR)
                qsCityId = 10;
            return qsCityId == globalCityId;
        },
        resetWarningOnCityChange: function (cityId) {
            if (cityId != 0)
                $.showCityWarning = false;
            else {
                D_usedSearch.cityWarning.cityId = '';
                $.showCityWarning = true;
            }
            D_usedSearch.cityWarning.toggleCityWarning();
        },
        resetWarning: function () {
            $.showCityWarning = false;
            D_usedSearch.cityWarning.toggleCityWarning();
        },
        setWarningCityName: function () {
            var cId = D_usedSearch.cityWarning.cityId;

            if (cId) {
                $('p.cityWarningTextAllIndia').addClass('hide');
                $('p.warningCityText').removeClass('hide');
                cityName = $.trim($('#drpCity [value=' + cId + ']').first().text().split("(")[0]);
                $('h4.cityWarningHeading').text("Cars in " + cityName);
                $('span.warningCity').text(cityName);
            }
            else {
                $('h4.cityWarningHeading').text("Cars from all over India");
                $('p.cityWarningTextAllIndia').removeClass('hide');
                $('p.warningCityText').addClass('hide');
            }

        },
        checkPgCityWarning: function (element) {
            if ($.showCityWarning || D_usedSearch.cityWarning.isNearByCity(element) || D_usedSearch.cityWarning.showCityWarningOnSoldOut) {
                $('#photoGallery .pg-delivery').show();
                return true;
            }
            else {
                $('#photoGallery .pg-delivery').hide();
                return false;
            }
        },
        isNearByCity: function (element) {
            if (element.parents('.stock-list').find('#nbCitiesTitle').length > 0) {
                return true;
            }
            return false;
        },
        setCityWarningOnSoldOut: function () {
            if ($.soldOut == "true")
                D_usedSearch.cityWarning.showCityWarningOnSoldOut = true;
            else
                D_usedSearch.cityWarning.showCityWarningOnSoldOut = false;
        }
    },
    similarCars:
    {
        registerEvents: function () {
            $(document).on('click', 'div.toggleCarousel', function () {
                var targetDiv = $(this).parents('li').find('.contact-seller');
                var stocksUrl = targetDiv.attr('stockRecommendationsUrl');
                if (stocksUrl) {
                    $('#newLoading,.blackOut-window-bt').show();
                    $.ajax({
                        context: this,
                        url: stocksUrl,
                        type: 'GET',
                        contentType: 'application/json; charset=utf-8',
                        dataType: 'json',
                        beforeSend: function (xhr) { xhr.setRequestHeader('sourceid', '1'); },
                        success: function (response) {
                            $('#newLoading,.blackOut-window-bt').hide();
                            var stocks = response.Stocks ? response.Stocks : response;
                            if (stocks.length) {
                                D_buyerProcess.recommendedCars.viewSellerBtnVisible(false);
                                RECOMMENDLISTING(stocks);
                                $('#similarCarPopup').show();
                                $('.blackOut-window-bt').show();
                                $('.recommendationList').scrollTop(0);
                                $.sellerDetailsBtnTextChange();
                            }
                            else {
                                $(this).text('No Similar Cars').removeClass('text-link text-bold car-choices').addClass('no-similar-cars');
                            }
                        },
                        error: function (xhr) {
                            $('#newLoading,.blackOut-window-bt').hide();
                            $(this).text('No Similar Cars').removeClass('text-link text-bold car-choices').addClass('no-similar-cars');
                        }
                    });

                }
                else {
                    window.location.href = targetDiv.attr('dealerCarsUrl') || 'javascript:void(0)';
                }
            });            
        },
        showSimilarCarLink: function (countTotalStock, searchUrl) {
            if (countTotalStock > 27 ) {
                $('.toggleCarousel').show();
            }
        }
    },
    utils:
    {
        getFilterFromSearchUrl: function (searchUrl, name) {
            var params = searchUrl.split('&');
            var result = {};
            var propval, filterName, value;
            var isFound = false;
            var paramsLength = params.length;
            for (var i = 0; i < paramsLength; i++) {
                var propval = params[i].split('=');
                filterName = propval[0];
                if (filterName == name) {
                    value = propval[1];
                    isFound = true;
                    break;
                }
            }
            if (isFound && value.length > 0) {
                if (value.indexOf('+') > 0)
                    return value.replace(/\+/g, " ");
                else
                    return value;
            }
            else
                return "";
        },
        getAllParamsWithValuesFromQS : function () {
            var dictionary = {};          
            var tempParams = window.location.hash.slice(1).split('&');

            if (tempParams) {
                for (var i = 0; i < tempParams.length; i++) {
                    var param = tempParams[i].split('=');
                    dictionary[param[0]] = param[1];
                }
            }
            return dictionary;
        },
        addFilterInQS: function (name, value, url) {
            var hash = url.replace('#', '');
            if (hash.length > 0)
                return hash.substring(0, hash.length) + "&" + name + "=" + value;
            else
                return name + "=" + value;
        },
        removeCarRanksFromQS: function (completeQS) {
            completeQS = $.removeFilterFromQSNB("lcr", completeQS);
            completeQS = $.removeFilterFromQSNB("ldr", completeQS);
            completeQS = $.removeFilterFromQSNB("lir", completeQS);
            return completeQS;
        },
        //qs: query string
        //qsParams: Object to which query params are added.
        addQueryParams: function (qs, qsParams) {
            if (typeof qs !== 'undefined' && typeof qsParams !== 'undefined') {
                qs.split('&') //add query parameters in qsParams Object.
                      .forEach(function (queryParam) {
                          var keyValue = queryParam.split('=');
                          if (keyValue.length > 1 && !qsParams.hasOwnProperty(keyValue[0])) {
                              qsParams[keyValue[0]] = keyValue[1];
                          }
                      });
            }
        },

        getQS: function () {
            var qs = window.location.hash;
            var qsParams = {}; //all query paramters will be maintained as key-value pair in this object.
            if (qs) {
                qs = qs.substring(1); // window.location.hash returns qs with '#', to remove it, we are taking substring.
                this.addQueryParams(qs, qsParams);
            }
            qs = window.location.search;
            if (qs) {
                qs = qs.substring(1);
                this.addQueryParams(qs, qsParams);
            }
            qs = '';
            Object.keys(qsParams) // creating qs by enumerating the keys.
                .forEach(function (prop) {
                    qs += prop + '=' + qsParams[prop] + '&';
                });
            qs = qs.slice(0,-1);
            return qs; //to be removed later.
        }
    },
    budgetSelection:
        {
            registerEvents:function()
                {

                $(document).on('click', '#minPriceList,#maxPriceList', function () {
                    $('#btnSetBudget').removeClass('btn-white').addClass('btn-orange');
                });

                $(document).on('click', '#maxPriceList', function () {
                    $('#btnSetBudget').trigger('click');
                });
                }
        },
    Finance: {
        iframeUrl: financeIframeUrl,
        registerEvents: function () {
            $(document).on("click", ".getFinance", function (event) {
                D_usedSearch.Finance.triggerFinanceClick(this, event);
            });
            $(document).on("click", "#iframe-close", function () {
                D_usedSearch.Finance.closeFinanceForm(this);
            });
            $(document).on("click", "#iframeTimeOutClick", function () {
                D_usedSearch.Finance.closeFinanceForm(this);
            });
            $(document).on('click', '.blackOut-window', function () {
                D_usedSearch.Finance.closeFinanceForm();
            });
        },
        triggerFinanceClick: function (currentnode, event) {
            event.stopPropagation();
            $('div.popup-loading-pic').removeClass('hideImportant');
            var node = $(currentnode).parents('li').find('.contact-seller'); //search page listing
            window.history.pushState("cartrade", "", "");
            Common.utils.lockPopup();
            $('#financeIframe').show(function () {
                try {
                    classifiedFinance.getFinance($(currentnode).data("href"), node, $("#iframecontent")).then(function (response) {
                        $('div.popup-loading-pic').addClass('hideImportant');
                        $("div.detail-ui-corner-top").css('display', 'none');
                        $("#iframecontent").show();
                    }).catch(function (errResponse) {
                        $("#iframeTimeOut").show();
                        $('div.popup-loading-pic').removeClass('hideImportant');
                    });
                } catch (err) { }
            });
        },
        closeFinanceForm: function () {
            clearTimeout(classifiedFinance.iframeError);
            $("#iframecontent").empty();
            $("#iframeTimeOut").hide();
            $("#financeIframe").hide();
            Common.utils.unlockPopup();
        }
    },
    Valuation: {
        loadingIconObj: $('.valuation-popup-loading-pic'),
        valuationPopupObj: $('.valuation-popup'),
        popupHeading: $('.valuation-popup__head'),
        valuationOtherDetails: $('.valuation-other-details'),
        similarCars: $('.valuation-popup__similar-cars'),
        rightPriceContent : $('.valuation-popup__right-price'),
        valuationHref: '',
        profileId: '',
        registerEvents: function () {
            $(document).on('click', '.view-market-price', D_usedSearch.Valuation.triggerValuationClick);
            $(document).on('click', '#globalPopupBlackOut, #valuation-popup-close', D_usedSearch.Valuation.triggerClosePopup);
            D_usedSearch.Valuation.sellerDetails.registerEvents();
            D_usedSearch.Valuation.recommendations.registerEvents();
        },
        triggerValuationClick: function (event) {
            event.stopPropagation();
            D_usedSearch.Valuation.valuationHref = $(this).data('href');
            D_usedSearch.Valuation.profileId = $(this).attr('profileid');
            var gsdButton = $(this).parents('li').find('.contact-seller');
            D_usedSearch.Valuation.showValuation(gsdButton);
            D_usedSearch.Valuation.pushToHistory({ title: 'searchValuation', href: D_usedSearch.Valuation.valuationHref });
        },
        pushToHistory: function (state) {
            window.history.pushState(state, '', '');
        },
        showValuation: function (gsdButton) {
            Common.utils.lockPopup();

            var $sourceNode = gsdButton.parents(".stock-detail").find(".top-rated-seller-tag");
            var $destinationNode = $(".form__user-details .top-rated-seller-tag")
            $.processTopSellerTag($sourceNode, $destinationNode);

            $('.blackOut-window-bt').show();
            D_usedSearch.Valuation.valuationPopupObj.show();
            D_usedSearch.Valuation.loadingIconObj.show();
            if ($.cookie("TempCurrentUser") != null) {
                var tmpCurrentUser = $.cookie("TempCurrentUser").split(':');
                $("#rpName").val(tmpCurrentUser[0]);
                $("#rpMobile").val(tmpCurrentUser[1]);
            }
            D_usedSearch.Valuation.rightPriceContent.load(D_usedSearch.Valuation.valuationHref, function (response, status) {
                D_usedSearch.Valuation.rightPriceContent.animate({scrollTop:0});
                D_usedSearch.Valuation.valuationOtherDetails.show();
                D_usedSearch.Valuation.loadingIconObj.hide();
                D_usedSearch.Valuation.trackClick();
                if (status == 'error') {
                    D_usedSearch.Valuation.rightPriceContent.html("Something went wrong. Please try again later");
                }

                var gsdButtonValuation = $('#rpgetsellerDetails');
                if (gsdButton && gsdButtonValuation) {
                    gsdButton.each(function () {
                        $.each(this.attributes, function () {
                            var name = this.name;
                            if (name != 'class' && name != 'id' && name != 'data-bind') {
                                gsdButtonValuation.attr(name, this.value);
                            }
                        });
                    });
                if (typeof m_bp_additonalFn != 'undefined')
                        m_bp_additonalFn.sellerDetailsBtnTextChange();
                D_usedSearch.Valuation.showChatButton(gsdButton,gsdButtonValuation);
                } else {
                    gsdButtonValuation.hide();
                }
            });
        },
        triggerClosePopup: function () {
            if (D_usedSearch.Valuation.valuationPopupObj.is(':visible')) {
                history.back();
            }
        },
        closePopup: function () {
            $('.blackOut-window-bt').hide();
            D_usedSearch.Valuation.valuationPopupObj.hide();
            D_usedSearch.Valuation.rightPriceContent.empty();
            D_usedSearch.Valuation.valuationPopupObj.removeClass('similar-cars-active');
            D_usedSearch.Valuation.valuationPopupObj.removeClass('list--slide');
            D_usedSearch.Valuation.valuationOtherDetails.hide();
            D_usedSearch.Valuation.similarCars.hide();
            D_usedSearch.Valuation.recommendations.slideBtn.hide();
            D_usedSearch.Valuation.sellerDetails.showBuyerForm();
            D_usedSearch.Valuation.recommendations.rprecommendation.empty();
            Common.utils.unlockPopup();
        },
        trackClick: function () {
            var trackingParam = {};
            trackingParam['profileId'] = D_usedSearch.Valuation.profileId;
            trackingParam['caseId'] = $(".right-price-box").attr("caseid");
            var rightPriceTrackingData = cwTracking.prepareLabel(trackingParam);
            cwTracking.trackCustomDataWithQs(D_usedSearch.trackingCategory, 'RightPriceClick', rightPriceTrackingData, D_usedSearch.utils.getQS());
        },
        setHeading: function (message) {
            D_usedSearch.Valuation.popupHeading.text(message);
        },
        sellerDetails: {
            userForm: $('.form__user-details'),
            sellerForm: $('.form__seller-details'),
            errorMessage: $('#rpnot_auth'),
            gettingDetailsBtn: $("#rpgettingDetails"),
            sellerDetailsBtn: $("#rpgetsellerDetails"),
            mobileError: $("#rp_mobileError"),
            emailError: $("#rp_emailError"),

            registerEvents: function () {
                $(document).on('click', "#rpgetsellerDetails", function () {
                    chatProcess.isChatLead = false;
                    if (typeof leadData !== "undefined") {
                        leadData.setCurrGsdButton(this);
                    }
                    D_usedSearch.Valuation.sellerDetails.sellerDetailsBtn.hide();
                    D_usedSearch.Valuation.sellerDetails.gettingDetailsBtn.removeClass('hide');
                    D_usedSearch.Valuation.sellerDetails.submitUserDetails(this);
                    if (typeof tracker !== "undefined") {
                        tracker.trackGsdClick(originId);
                    }
                });
                $(document).on('click', ".back-to-gsd-form", function () { D_usedSearch.Valuation.sellerDetails.showBuyerForm()});
                $(document).on('click', '#rp_chat_btn_container', function () {
                    chatProcess.isChatLead = true;
                    chatUIProcess.loader.show($(this).find('.chat-btn'));
                    D_usedSearch.Valuation.sellerDetails.submitUserDetails(D_usedSearch.Valuation.sellerDetails.sellerDetailsBtn);
                });
            },
            submitUserDetails: function (node) {
                ISRECOMMENDED = false;
                D_buyerProcess.sellerDetails.setVariables($(node));
                D_buyerProcess.sellerDetails.readBuyersData($(node),true);
            },
            bindSellerDetails: function (ds) {
                if (ds) {
                    D_usedSearch.Valuation.sellerDetails.gettingDetailsBtn.addClass('hide');
                    D_usedSearch.Valuation.sellerDetails.userForm.hide();
                    if (ds.dealerShowroomPage) {
                        $('.seller-virtual-link').attr('href', ds.dealerShowroomPage).show();
                    }
                    else {
                        $(".seller-virtual-link").hide();
                    }
                    $('#sellerName').text(ds.name);
                    $('#sellerContact').text(ds.mobile);
                    $('#sellerEmail').text(ds.email);
                    $('#sellerAddress').text(ds.address);
                    $("#sellerContactPerson").text(ds.contactPerson ? ds.contactPerson : "");
                    D_usedSearch.Valuation.sellerDetails.sellerForm.show();
                    $('.bp-SimilarCars').show();
                }
            },
            showServerErrorMessage: function (message) {
                D_usedSearch.Valuation.sellerDetails.userForm.hide();
                D_usedSearch.Valuation.sellerDetails.errorMessage.show().find(".back-to-gsd-form > span").text(message);
            },
            showBuyerForm: function () {
                D_usedSearch.Valuation.sellerDetails.sellerDetailsBtn.show();
                D_usedSearch.Valuation.sellerDetails.gettingDetailsBtn.addClass('hide');
                D_usedSearch.Valuation.sellerDetails.userForm.show();
                D_usedSearch.Valuation.sellerDetails.sellerForm.hide();
                D_usedSearch.Valuation.sellerDetails.errorMessage.hide();
                D_usedSearch.Valuation.sellerDetails.mobileError.hide();
                D_usedSearch.Valuation.sellerDetails.mobileError.next().hide();
                D_usedSearch.Valuation.sellerDetails.emailError.hide();
                D_usedSearch.Valuation.sellerDetails.emailError.next().hide();
                D_usedSearch.Valuation.sellerDetails.gettingDetailsBtn.addClass('hide');
            }
        },
        recommendations: {
            slideBtn: $('#valuationPopupSlideBtn'),
            tooltipLabel: $('#slideBtnTooltipLabel'),
            rprecommendation :$('#rprecommendations'), 
            registerEvents: function () {
                $(document).on('click', '#valuationPopupSlideBtn', D_usedSearch.Valuation.recommendations.toggleList);
            },
            bindRecommendations: function (data) {
                D_buyerProcess.recommendedCars.viewSellerBtnVisible(true);
                D_usedSearch.Valuation.similarCars.show();
                RECOMMENDLISTING(data);
                var recommendedListings = $('#recommendations').html();
                RECOMMENDLISTING([]);
                D_usedSearch.Valuation.recommendations.rprecommendation.html(recommendedListings);
                $('.bp-SimilarCars').hide();
                D_usedSearch.Valuation.recommendations.slideBtn.trigger('click');
            },
            toggleList: function () {
                if (!D_usedSearch.Valuation.valuationPopupObj.hasClass('similar-cars-active')) {
                    D_usedSearch.Valuation.valuationPopupObj.addClass('similar-cars-active');
                    setTimeout(function () {
                        D_usedSearch.Valuation.recommendations.slideBtn.show();
                    }, 500);
                }

                if (!D_usedSearch.Valuation.valuationPopupObj.hasClass('list--slide')) {
                    D_usedSearch.Valuation.valuationPopupObj.addClass('list--slide');
                    D_usedSearch.Valuation.setHeading('People also liked');
                    D_usedSearch.Valuation.recommendations.setTooltipLabel('Right price');
                }
                else {
                    D_usedSearch.Valuation.valuationPopupObj.removeClass('list--slide');
                    D_usedSearch.Valuation.setHeading('Right Price');
                    D_usedSearch.Valuation.recommendations.setTooltipLabel('Recommended cars');
                }
            },

            setTooltipLabel: function (message) {
                D_usedSearch.Valuation.recommendations.tooltipLabel.text(message);
            }
        },
        showChatButton: function (gsdButton,gsdButtonValuation) {
            var $searchChatContainer = gsdButton.parent().siblings('.chat-btn-container'); // chat button container on search page.
            var $rpChatContainer = $('#rp_chat_btn_container'); // chat button container on right price.
            if ($searchChatContainer.length < 1 || $searchChatContainer.hasClass('hide')) { //condition for knockout and repeater. 
                $rpChatContainer.addClass('hide');
                gsdButtonValuation.removeClass('grid-8').addClass('grid-12');
                D_usedSearch.Valuation.sellerDetails.gettingDetailsBtn.removeClass('grid-8').addClass('grid-12');
            }
            else {
                $rpChatContainer.removeClass('hide');
                gsdButtonValuation.removeClass('grid-12').addClass('grid-8');
                D_usedSearch.Valuation.sellerDetails.gettingDetailsBtn.removeClass('grid-12').addClass('grid-8');
            }
        }
    }
}

var d_usedSearchPage = new D_usedSearchPage(); d_usp_globalcity = new d_usedSearchPage.globalCity(); d_usp_pgNavigation = new d_usedSearchPage.pgNavigation();
d_usp_nbcityapiHit = new d_usedSearchPage.nbCityApiHit();

var listingsViewModel = function (model) {
    ko.mapping.fromJS(model, {}, this);
};

$.fn.removeCurrentFilter = function () {
    return this.click(function () {
        var node = $(this);
        $('div [name=' + node.attr('parentfiltername') + ']').find('li[filterid=' + node.attr('filterid') + ']').trigger('click');
    });
};

$.fn.removeMakeRootFilter = function () {
    return this.click(function () {
        var node = $(this);
        $('#manu-box ul.ul-makes').find('li[carfilterid="' + node.attr('carfilterid') + '"]').trigger('click');
    });
};

$.fn.sortFilter = function () {
    return this.change(function () {
        $.pageNo = 1;
        var node = $(this);
        $.removePageNoParam();
        $.removeKnockouts();
        $.updateFilters(node, 'sort', node.value, 3);
        $(this).blur();//added by Sachin Bharti(10th Aug)
        $("#nearByCityList .active").addClass("selected");
    });
};

$.setSortOnPageLoad = function () {
    var sc = $.getFilterFromQS('sc');
    var so = $.getFilterFromQS('so');
    var value = $('#sort option[sc="' + sc + '"]').filter('option[so="' + so + '"]').val();
    $('#sort').val(value);
};

$.fn.expandCollapse = function () {
    return this.click(function () {
        var node = $(this);
        var nextNode = node.next();
        if (nextNode.hasClass('budgetContainerMain')) {
            if (nextNode.is(':visible')) {
                nextNode.removeClass('overflowVisible').slideUp();
            }
            else {
                nextNode.slideDown(function () {
                    nextNode.addClass('overflowVisible');
                });
            }
        }
        else {
            nextNode.slideToggle();
        }
        node.find('.fa-angle-up').toggleClass('transform');
        node.find('.fa-angle-down').toggleClass('transformy');
    });
};

$.fn.onCheckBoxClick = function () {
    return this.click(function () {
        $("div.getNextPage").hide();
        if (!$(this).hasClass('filter-disable') || $(this).hasClass('filter-enable') || $(this).hasClass('active')) {
            var node = $(this);
            $.updateFilters(node, node.parent().parent().parent().attr('name'), node.attr('filterId'), 2);
        }
    });
}

$.fn.resetAll = function () {
    return this.click(function () {

        $('.list-points li').removeClass('active');
        $.pageNo = 1;
        $.resetAllSliders();
        $.setBudgetRange('', '');
        var city = $.getFilterFromQS('city');
        var so = $.getFilterFromQS('so');
        var sc = $.getFilterFromQS('sc');
        var pc = $.getFilterFromQS('pc');
        var completeQS = '';
        if (so.length > 0 && sc.length > 0)
            completeQS = "sc=" + sc + "&so=" + so;
        if (city.length > 0)
            completeQS = $.addParameterToString('city', city, completeQS);
        if (pc.length > 0)
            completeQS = $.addParameterToString('pc', pc, completeQS);
        $('#makesList li').removeClass('active');
        $('span.removeFilter').empty();
        $.removeKnockouts();
        $.pushState(completeQS);
    });
};

$('h3.sub-values').expandCollapse();

$(checkBoxSelector).onCheckBoxClick();

$(sortSelector).sortFilter();

$('#resetFilters').resetAll();

$.addParameterToString = function (name, value, completeQS) {
    if (value) {
        if (completeQS.length > 0)
            completeQS += "&" + name + "=" + value;
        else
            completeQS = name + "=" + value;
    }
    return completeQS;
};


$.appendToQS = function (temp, name, value) {
    if (temp.length > 0)
        temp += "&" + name + "=" + value;
    else
        temp += name + "=" + value;

    return temp;
};

// scroll body to 0px on click
$.fn.goToTop = function () {
    return this.click(function () {
        $('body,html').animate({
            scrollTop: 125
        }, 1000);
        return false;
    });
};

// scroll body to 0px on select filter
//Added by Sachin Bharti(10th Aug 2015)
$.goTop = function () {
    $('body,html').animate({
        scrollTop: 125
    }, 1000);
};

$.showSoldOutSlug = function () {
    var slug = $("div.soldout-box");
    var qs = window.location.search.slice(1);
    $.soldOut = D_usedSearch.utils.getFilterFromSearchUrl(qs, "issold");
    if ($.soldOut == "true") {
        D_usedSearch.cityWarning.setCityWarningOnSoldOut();
        D_usedSearch.cityWarning.resetWarning();
        slug.show();
        qs = $.removeFilterFromQSNB("issold", qs);
        var currURL = window.location.pathname;
        if (qs) {
            currURL = "?" + qs;
        }
        currURL = currURL + window.location.hash;
        history.replaceState(currURL, "", currURL);
        setTimeout(function () { slug.hide("slow"); }, 15000);
    }
    else
        slug.remove();
};

$.getMumbaiDelhiNames = function (cityId) {
    if (cityId == 1)
        return 'Mumbai City';
    else if (cityId == 3000)
        return 'Mumbai (All)';
};

$.resetPageLoadValues = function () {
    $.soldOut = $.sc = $.filterbyadditional = $.color = $.fuel = $.bodytype = $.owners = $.seller = $.trans = $.kms = $.year = $.budget = $.city = $.car = '';
};

$.setQSParametersInURL = function () {
    var completeQS = '';
    completeQS = $.addParameterToString("car", $.car, completeQS);
    completeQS = $.addParameterToString("city", $.city, completeQS);
    completeQS = $.addParameterToString("pc", $.city, completeQS);
    completeQS = $.addParameterToString("budget", $.budget, completeQS);
    completeQS = $.addParameterToString("year", $.year, completeQS);
    completeQS = $.addParameterToString("kms", $.kms, completeQS);
    completeQS = $.addParameterToString("trans", $.trans, completeQS);
    completeQS = $.addParameterToString("seller", $.seller, completeQS);
    completeQS = $.addParameterToString("owners", $.owners, completeQS);
    completeQS = $.addParameterToString("bodytype", $.bodytype, completeQS);
    completeQS = $.addParameterToString("fuel", $.fuel, completeQS);
    completeQS = $.addParameterToString("color", $.color, completeQS);
    completeQS = $.addParameterToString("filterbyadditional", $.filterbyadditional, completeQS);
    completeQS = $.addParameterToString("sc", $.sc, completeQS);
    completeQS = $.addParameterToString("so", $.so, completeQS);
    completeQS = $.addParameterToString("pn", $.pageNo.toString(), completeQS);
    window.location.hash = completeQS;
};

/*************************************************** Knockout Related Functions Start*************************************************************/
//function to be used for formatting the contact nos
function formatPhoneNumber(phnNum) {
    var phone = String(phnNum).substr(phnNum.length - 10, 10);
    var formatted = phone.substr(0, 3) + ' ' + phone.substr(3, 3) + ' ' + phone.substr(6, 4);
    return formatted;
}

$.bindLazyListings = function (listingData) {
    listingNo = listingNo + 1;
    var koHtml = '<div class="stock-list search-list-container">' +
 '<div class="pg-out-box">' +
                            '<p>Page ' + $.pageNo + '</p>' +
                    '</div>' +
                    '<ul id="listing' + listingNo + '" class="LLko" data-bind="template: { name: \'listingTemp\', foreach: listing }">' +
                    '</ul>' +
                    '<div class="clear"></div>' +
                    '</div>';
    $('#listing' + (listingNo - 1)).parent().after(koHtml);
    ko.applyBindings(new listingsViewModel(listingData), document.getElementById("listing" + listingNo));
};

//Remove all the elements from the DOM
$.removeKnockouts = function () {
    $('#listingsData').remove();
    $('#listing1').empty();
    $(".LLko").each(function () {
        $(this).parent().remove();
    });
    for (var i = 0; i < 10; i++) {
        $('#listing' + i + '').empty();
    }

    //alert(1);
};

//Code to detect the End of the page

$(window).scroll(function () {

    var mainListHt = $('div.main-filter-list').height();
    var previousscroll = 0;
    var flBoxOffset = $('div.main-filter-list').offset().top;
    var btmFloatHt = $('.usedsearch-floating-strip').height();
    var winHeight = $(window).height();
    var footerOffset = $('footer').offset().top
    var opacity = $floatingStrip.css("opacity");

    var winscroll = $(window).scrollTop();
    var listingHeight = $('#cw-loading-box').height();
    var searchResultsHeight = $('#pageListing').height();
    if ( mainListHt < listingHeight) {
        //scroll down
        var posy = mainListHt - flBoxOffset - 135;
        if (winscroll >= posy && winscroll + winHeight <= footerOffset) {
            $('div.main-filter-list').css({'position': 'fixed','overflow': 'visible','bottom': btmFloatHt + 'px'});
            $('#filters').css({'height': 'auto'});
        }
        else if (winscroll >= posy && winscroll + winHeight > footerOffset) {
            $('div.main-filter-list').css({'bottom': 0,'position': 'absolute'});
            $('#filters').css({ 'height': searchResultsHeight });
        }
        else if (!$('.top-nav-nested-panel').is(':visible')) {
            $('div.main-filter-list').css({ 'bottom': 'auto', 'position': 'static' });
            $('#filters').css({'height': 'auto'});
        }
    }
    else {
        $('div.main-filter-list').css({ 'bottom': 'auto', 'position': 'static' });
    }

    if ($(window).scrollTop() > 200)
        $('#back-top').show();
    else
        $('#back-top').hide();

    if ($.fetchMoreResults && !Common.isScrollLocked) {
        if ($(window).scrollTop() >= $(document).height() - $(window).height() - $('footer').height() - $('.showMoreCars').height() - 250) {
            if ($.pageNo < 10 && $('div.getNextPage').is(':visible')) {
                $.getNextPageData();
            }
        }
    }

    if ($(window).scrollTop() >= $(document).height() - $(window).height() - $('footer').height() - $('.showMoreCars').height() - 250 && remainingCount<=0 && $('div.getNextPage').is(':visible') && $.getFilterFromQS("city") != 3000 && $.getFilterFromQS("city") != 3001) {
        if (pageCountForNBCity < 1)
            var nbCityInfo = d_usp_nbcityapiHit.getNextNbCityWithCount();
        else
            $.pageNoNB++;

        if(!isNaN(pageCountForNBCity))
        getNbCityData(nbCityInfo);
    }


});

function getNbCityData(nbCityInfo) {
    var cityId;
    $("div.getNextPage").hide();
    if (!$.pageNoNB)
        $.pageNoNB = 1;
    
    var stockLength=$(".stock-list li").length;
    if (stockLength < 208 && $.pageNoNB == 1 )
    {
        NBCITYCHANGED = true;
        var element = $("#nearByCityList .selected");
        if (nbCityInfo)
             cityId = nbCityInfo.nbCitiesId;
        else
            cityId = element.attr("cityId");

        if (cityId == 9999 && !element.is(':visible')) {
            element = $("#nearByCityList .selected").next();
            $("#nearByCityList li").removeClass("selected");
            element.addClass("selected");
        }
        if (cityId && nbCityInfo.totalNBCityCount) {
            qsTemp = $.removeFilterFromQSNB("city", qsTemp);
            qsTemp = $.removeFilterFromQSNB("pn", qsTemp);
            qsTemp = D_usedSearch.utils.removeCarRanksFromQS(qsTemp);
            qsTemp = $.addParameterToString('city', cityId, qsTemp);
            qsTemp = $.addParameterToString('pn', "1", qsTemp);
            qsTemp = $.addParameterToString("lcr", 0, qsTemp);
            qsTemp = $.addParameterToString("ldr", 0, qsTemp);
            qsTemp = $.addParameterToString("lir", 0, qsTemp);
            $.showLoadingTxt();
          //  $.countList = parseInt($("#nearByCityList .selected span.city-count").text());
                pageCountForNBCity--;
            $.hitAPI(qsTemp, true);

        }
    }
    else if (stockLength < 208 && $.pageNoNB > 1 ) {
      
        //$.pageNoNB++;
        var noOfCities = $.getFilterFromQSNB('city', qsTemp).split(',').length;
        qsTemp = D_usedSearch.utils.removeCarRanksFromQS(qsTemp);
        qsTemp = $.addParameterToString("lcr", D_usedSearch.latestNonPremiumCarRank, qsTemp);
        qsTemp = $.addParameterToString("ldr", D_usedSearch.latestPremiumDealerCarRank, qsTemp);
        qsTemp = $.addParameterToString("lir", D_usedSearch.latestPremiumIndividualCarRank, qsTemp);
        if (noOfCities > 1) {
            var lastNBCity = parseInt($.getFilterFromQSNB('city', qsTemp).split(',').pop());
            qsTemp = $.removeFilterFromQSNB("city", qsTemp);
            qsTemp = $.addParameterToString('city', lastNBCity, qsTemp);
        }

        qsTemp = $.removeFilterFromQSNB("pn", qsTemp);
        
        if (qsTemp.length > 0)
            qsTemp += "&pn=" + $.pageNoNB;
        else
            qsTemp = "pn=" + $.pageNoNB;
        
        pageCountForNBCity--;
        $.showLoadingTxt();
        $.hitAPI(qsTemp, true);
        
    }

}

$.getNextPageData = function () {
    $.fetchMoreResults = false;
    $.pageNo = parseInt($.getFilterFromQS("pn"));
    if (isNaN($.pageNo))
        $.pageNo = 2;
    else
        $.pageNo++;

    $.loadNextPage();
};

$.loadNextPage = function () {
    var completeQS = $.removeFilterFromQS("pn");
    window.location.hash = completeQS;
    $.removeExcludedStocksFromQS();
    completeQS = D_usedSearch.utils.removeCarRanksFromQS(completeQS);
    if (completeQS.length > 0)
        completeQS += "&pn=" + $.pageNo;
    else
        completeQS = "pn=" + $.pageNo;
    completeQS = D_usedSearch.utils.addFilterInQS ("lcr", D_usedSearch.latestNonPremiumCarRank, completeQS);
    completeQS = D_usedSearch.utils.addFilterInQS("ldr", D_usedSearch.latestPremiumDealerCarRank, completeQS);
    completeQS = D_usedSearch.utils.addFilterInQS("lir", D_usedSearch.latestPremiumIndividualCarRank, completeQS);
    if(excludeStocks)
    {
        completeQS += "&excludestocks=" + excludeStocks;
    }
    $.pushState(completeQS);
}

$.fn.showMoreCars = function () {
    return this.click(function () {
        if ($.getFilterFromQS("city") == $("#nearByCityList .selected").attr("cityid") || !$("#nearByCityList .selected").attr("cityid"))
            $.getNextPageData();
        else
            getNbCityData();

        dataLayer.push({ event: 'showMoreCars', cat: 'UsedCarSearch', act: 'ShowMoreCars Clicked' });
    });
};

/*************************************************** Knockout Related Functions End*************************************************************/

/**********************************************Seller Rating info releted logic start**************************************************************/
//this fuction take dealer rating text from source node and if rating text is availabe then on souce node then update rating text on destination node 
$.processTopSellerTag = function ($sourceNode, $destinationNode) {
    if ($sourceNode && $sourceNode.length != 0) {  //if node is not available then $().find() return base node(reference node) so length check is to know node(which we want) is available or not 
        var ratingText = $sourceNode.text();
        $destinationNode.text(ratingText).show();
    }
    else {
        $destinationNode.hide();
    }
}
/**********************************************Seller Rating info releted logic end**************************************************************/

/*************************************************** Slider Implementation Starts *************************************************************/
$.bindAgeSlider = function () {
    $("#model-range").slider({
        range: true,
        min: 0,
        max: 8,
        values: [0, 8],
        step: 1,
        slide: function (event, ui) {
            $.pageNo = 1;
            if (ui.values[1] < 1) {
                return false;
            }
            if (ui.values[0] == 1)
                $("#minYear").html(ui.values[0] + ' year');
            else if (ui.values[0] == 0)
                $("#minYear").html(ui.values[0]);
            else
                $("#minYear").html(ui.values[0] + ' years');

            if (ui.values[1] == 8)
                $("#maxYear").html(ui.values[1] + ' years & above');
            else if (ui.values[1] == 1)
                $("#maxYear").html(ui.values[1] + ' year');
            else
                $("#maxYear").html(ui.values[1] + ' years');
        },

    });
    $("#minYear").html($("#model-range").slider("values", 0));
    $("#maxYear").html($("#model-range").slider("values", 1) + ' years & above');
    $('#model-range').on('slidestop', function () {
        $.getQSForSlider('year');
    });
};

$.bindKMSlider = function () {
    $("#kilometer-range").slider({
        range: true,
        min: 0,
        max: 80000,
        step: 5000,
        values: [0, 80000],
        slide: function (event, ui) {
            $.pageNo = 1;
            if (ui.values[1] < 1) {
                return false;
            }
            if (ui.values[0] == 0)
                $("#minKm").html(ui.values[0]);
            else
                $("#minKm").html(ui.values[0] + ' kms');

            if (ui.values[1] == 80000)
                $("#maxKm").html(ui.values[1] + ' kms & above');
            else
                $("#maxKm").html(ui.values[1] + ' kms');
        }
    });
    $("#minKm").html($("#kilometer-range").slider("values", 0));
    $("#maxKm").html($("#kilometer-range").slider("values", 1) + ' km & above');
    $('#kilometer-range').on('slidestop', function () {
        $.getQSForSlider('kms');
    });
};

//Get KM Start and End Index
$.getKmRange = function () {
    var start = '', end = '';
    start = parseFloat($("#minKm").text()) / 1000;
    if (parseInt($("#maxKm").text()) == 80000)
        end = '';
    else
        end = parseFloat($("#maxKm").text()) / 1000;
    return 'kms=' + start + '-' + end;
};

//Get Year Start and End Index
$.getYearRange = function () {
    var start = '', end = '';
    start = parseFloat($("#minYear").text());
    if (parseInt($("#maxYear").text()) == 8)
        end = '';
    else
        end = parseFloat($("#maxYear").text());
    return 'year=' + start + '-' + end;
};

$.getQSForSlider = function (type) {
    var completeQS = $.removeFilterFromQS(type);
    //history.pushState(null, null, "?" + completeQS);
    window.location.hash = completeQS;
    completeQS = $.removeFilterFromQS('pn');
    completeQS = D_usedSearch.utils.removeCarRanksFromQS(completeQS);
    var qs = type == 'kms' ? $.getKmRange() : $.getYearRange();
    if (completeQS.length > 0)
        completeQS = completeQS + "&" + qs;
    else
        completeQS = qs;
    $.removeKnockouts();
    $.pushState(completeQS);
}

$.resetAllSliders = function () {
    $.resetYearSlider();
    $.resetKilometerSlider();
};

//Reset Year Slider
$.resetYearSlider = function () {
    var values = $.setSliderRange($('#model-range'));
    if (values["minValue"] == 1)
        $("#minYear").html(values["minValue"] + " year");
    else if (values["minValue"] == 0)
        $("#minYear").html(values["minValue"]);
    else
        $("#minYear").html(values["minValue"] + " years");

    if (values["maxValue"] == 8)
        $("#maxYear").html(values["maxValue"] + " years & above");
    else if (values["maxValue"] == 1)
        $("#maxYear").html(values["maxValue"] + " year");
    else
        $("#maxYear").html(values["maxValue"] + " years");

};

//Reset Kilometer Slider
$.resetKilometerSlider = function () {
    var values = $.setSliderRange($('#kilometer-range'));

    if (values["minValue"] == 0)
        $("#minKm").html(values["minValue"]);
    else
        $("#minKm").html(values["minValue"] + " kms");

    if (values["maxValue"] == 80000)
        $("#maxKm").html(values["maxValue"] + " kms & above");
    else
        $("#maxKm").html(values["maxValue"] + " kms");
};

//Reset Range of Slider to Default
$.setSliderRange = function (element) {
    var min = element.slider("option", "min");
    var max = element.slider("option", "max");
    element.slider("values", 0, min);
    element.slider("values", 1, max);
    var values = {
        minValue: min,
        maxValue: max
    };
    return values;
};

$.setYearSliderOnLoad = function () {
    if ($.year.length > 0) {
        var values = $.year.split('-');
        var start = '', end = '';
        if (!isNaN(values[0]))
            start = values[0];
        if (!isNaN(values[1]))
            end = values[1];
        if (start == 1)
            $("#minYear").text(start + ' year');
        else if (start == 0)
            $("#minYear").text(start);
        else
            $("#minYear").text(start + ' years');

        if (end == 1)
            $("#maxYear").text(end + ' year');
        else if (end != '')
            $("#maxYear").text(end + ' years');
        else
            $("#maxYear").text('8 years & above');

        $.setSliderRangeQS($("#model-range"), start, end);
    }
};

$.setKmSliderOnLoad = function () {
    if ($.kms.length > 0) {
        var values = $.kms.split('-');
        var start = '', end = '';
        if (!isNaN(values[0]))
            start = values[0] * 1000;
        if (!isNaN(values[1]))
            end = values[1] * 1000;
        if (start == 0)
            $("#minKm").text(start);
        else
            $("#minKm").text(start + ' kms');

        if (end != '') {
            end = parseInt(end);
            $("#maxKm").text(end + ' kms');
        }
        else
            $("#maxKm").text('80000 kms & above');
        $.setSliderRangeQS($("#kilometer-range"), start, end);
    }
};

$.setSliderRangeQS = function (element, start, end) {
    element.slider("values", 0, start);
    if (end != '')
        element.slider("values", 1, end);
};
/*************************************************** Slider Implementation Ends *************************************************************/
/*************************************************** Budget Functionality Starts ************************************************************/
$.setBudgetOnLoad = function () {
    if ($.budget.length > 0) {
        var values = $.budget.split('-');
        var start = '', end = '';
        if (!isNaN(values[0]))
            start = values[0];
        if (!isNaN(values[1]))
            end = values[1];
        $.setBudgetRange(start, end);
    }
};

////Set Budget Start and End Index (Query String)
$.setBudgetRange = function (start, end) {
    if (start == "" && end == "") {
        $("#budgetBtn").text("Choose budget");
        $('#minInput').val("");
        $('#maxInput').val("");
    }
    else
        $("#budgetBtn").text(start + "L - " + end + "L");
    $('#minInput').val(start);
    $('#maxInput').val(end);
};

$.fn.setBudget = function () {
    return this.click(function () {
        $('#btnSetBudget').removeClass('btn-orange').addClass('btn-white');
        var element = $(this);
        var minBudget = 0, maxBudget;
        minBudget = $('#minInput').val();
        maxBudget = $('#maxInput').val();
        minBudget = (minBudget == "" || minBudget == "0") ? 0 : parseFloat(minBudget);
        maxBudget = parseFloat(maxBudget) == 0 ? 1 : parseFloat(maxBudget);
        if (isNaN(maxBudget))
            maxBudget = "";
        if (maxBudget < minBudget && maxBudget != "") {
            $('#maxInput').next().show();
            return false;
        }
        var tempQS = $.removeFilterFromQS('budget');
        window.location.hash = tempQS;
        tempQS = $.removeFilterFromQS('pn');
        tempQS = D_usedSearch.utils.removeCarRanksFromQS(tempQS);
        if (maxBudget == undefined)
            maxBudget = '';
        tempQS = $.appendToQS(tempQS, "budget", minBudget + "-" + maxBudget);
        $.pageNo = 1;
        $.removeKnockouts();
        $.removeSortParam();
        window.location.hash = tempQS;
        $.pushState(tempQS);
        element.removeClass('us-go-gray').removeClass('us-go-green').addClass('us-go-white');
    });
};

//Add comma to budget
$.addCommas = function (nStr) {
    var formatted = "";
    var breakPoint = 3;
    var numberToFormat = nStr.toString().replace(/\,/g, '');
    for (i = numberToFormat.length - 1; i >= 0; i--) {
        formatted = numberToFormat[i].toString() + formatted;
        if ((numberToFormat.length - i) == breakPoint && numberToFormat.length > breakPoint) {
            formatted = "," + formatted;
            breakPoint += 2;
        }
    }

    return formatted;
};
/*************************************************** Budget Functionality Ends ************************************************************/
/*********************************************** FeedBack Functionality Starts************************************************************/
//Added By Sachin Bharti(7th Aug 2015)
$.bindFeedBack = function () {
    //Feedback Widget starts here..	
    var animate1 = "fast";
    var value = 5;
    $('#feedBackSlider').slider({
        animate: animate1,
        orientation: "horizontal",
        range: "min",
        min: 0,
        max: 10,
        value: value,
        create: function (event, ui) {
            $(this).find(".ui-slider-handle").html("<span class='slider-handle-value'>" + value + "</span>");
            feedbkRating = value;
        },
        slide: function (event, ui) {
            $(this).find(".ui-slider-handle span").text(ui.value);
            if (ui.value >= 0 && ui.value <= 6)
                $(this).find(".ui-slider-range").css('background-color', 'red');
            if (ui.value > 6 && ui.value <= 8)
                $(this).find(".ui-slider-range").css('background-color', 'rgb(255, 133, 0)');
            if (ui.value >= 9)
                $(this).find(".ui-slider-range").css('background-color', 'rgb(45, 173, 45)');
            feedbkRating = ui.value;
        },
        change: function (event, ui) {
            $(this).find(".ui-slider-handle span").text(ui.value);
            feedbkRating = ui.value;
        },
        stop: function (event, ui) {
            feedbkRating = ui.value;
        }
    });

    $('.feedbackBtn').click(function (e) {
        if ($('.feedback-form').is(':hidden')) {
            $('.feedback-form').slideDown();
            $('div.shortlist-popup').removeClass('overflowVisible').slideUp();
        }
    });

    $('.feedback-form .cross-md-dark-grey, .shortlist-close').click(function (e) {
        $('div.shortlist-popup').removeClass('overflowVisible')
        $('.feedback-form, .shortlist-popup').slideUp();
    });

    //Added By Sachin Bharti(7th Aug 2015)
    var isDisabled = false;
    $("#btnFeedback").click(function () {
        var txtfeed = $.trim($("#txtComments").val());
        var txtFeedFor = feedbkRating;
        var url = window.location.href;
        txtfeed = txtfeed.replace(/"/g, "'");
        var textPlace = "What do you like about and what can we improve on?";
        if (textPlace == txtfeed) {
            txtfeed = "";
        }

        if (!isDisabled) {
            $("#processInline").show();
            $.ajax({
                type: 'POST',
                url: '/api/feedback/',
                contentType: "application/json",
                data: '{"Feedback":"' + txtfeed + '","FeedbackRating":"' + txtFeedFor + '","Source":"' + url + '","CarInfo":"' + '","SourceID":"' + '22' + '","ReportId":"-1"}',
                success: function (response) {
                    $("#processInline").hide();
                    $("#divFeedback").html("Thanks for giving your valuable feedback.");
                },
                failure: function (response) {
                    alert("Error occured while saving your feedback. Please try again after sometime.");
                }

            });
        }

        isDisabled = true;
        $('#btnFeedback').css('cursor', 'default');
        $("input[type=submit]").attr("disabled", "disabled");

        setTimeout(function () {
            isDisabled = false;
            $("#btnFeedback").css('cursor', 'pointer');
            $("input[type=submit]").attr("disabled", "false");
        }, 10000);

    });
};
/*********************************************** FeedBack Functionality Ends************************************************************/
/*********************************************** PhotoGallery Functionality Starts*******************************************************/
//Get all images for particular profileId And Open ColorBox for ImageGallery
$.bindPhotoGallery = function () {

    $.bindSlideShow();
    $('#pg-btn-close').bindSlideShowClose();
    $.photoGalleryIconClick();
};

$.bindCertificateLogoHTML = function (thisElement) {
    var dynamicHTML = '';
    var certificateLogo = thisElement.find('a.certLogo');
    if (certificateLogo.length != 0) {
        var imgLogoUrl = certificateLogo.find("img").attr("src");
        var logoType = certificateLogo.find("img").attr('logotype');
        if (logoType != '' && logoType != null && logoType != undefined) {
            if (logoType == 'Certification') {
                dynamicHTML = '<img border="0" src="' + imgLogoUrl + '"></img>';
                dynamicHTML += '<div class="margin-top10 margin-bottom10 font13">Checked on 217 points. Includes 1 year comprehensive CarWale Guarantee. <a id = "absureMoreDetails" target="_blank" href = "' + thisElement.find('#linkToDetails').attr('href') + '#warranty">Know More.</a></div>';
                dynamicHTML += '<div class="grey-botom margin-bottom10"></div>';
                dynamicHTML += '<div class="pg-guarantee-border margin-bottom10"></div>';
            }
        }
        else {
            dynamicHTML += '<div><strong>CarWale</strong></div><div><strong>Inspected</strong></div><div class="margin-top10 margin-bottom10 font13">Checked on 217 points. <a id = "absureMoreDetails" target="_blank" href = "' + thisElement.find('#linkToDetails').attr('href') + '#carcondition">Know More.</a></div><div class="grey-botom margin-bottom10"></div>';
        }
        $('#pg_seller_details').find('.certificationDetails').html(dynamicHTML);
        return true;
    }
    else {
        $('#pg_seller_details').find('.certificationDetails').html('');
        return false;
    }
}


$.bindSlideShow = function () {

    $('a.slideShow').unbind('click');
    $('a.slideShow').click(function (e, nodePG) {
        var prevCityListing;

        var prevPgListingId = $("a.slideShow[profileid='" + $('#pg_seller_details').attr('profileid') + "']");
        if (nodePG == undefined)
            nodePG = $(this);

        if ($(this).nextAll('span.shortlist').hasClass('shortlistIcon-active'))
            $("#descriptions").nextAll('span.shortlistphotogallery').addClass('shortlistIcon-active');
        else
            $("#descriptions").nextAll('span.shortlistphotogallery').removeClass('shortlistIcon-active');



        var currElement = nodePG;
        setFocusOnTxt();
        e.preventDefault();
        if (currElement.parent().hasClass('img-placer')) {
            var photoCount = currElement.parent().parent().parent().next().next().next().find('span.cw-cam-icon').next().text();
            if (photoCount.length > 0)
                $('.gallery-tabs-container').show();
            else {
                $('.gallery-tabs-container').hide();
            }
        }

        var $sourceNode = currElement.parents(".stock-detail").find('.top-rated-seller-tag'); 
        var $destinationNode = $("#pg_contactSellerForm .top-rated-seller-tag");
        $.processTopSellerTag($sourceNode, $destinationNode);
        
        $.showLoading();
        var profileId = currElement.attr('profileId');
        remarketingVariables.carMake = currElement.attr('makeName');
        remarketingVariables.carModel = currElement.attr('modelName');
        remarketingVariables.price = currElement.attr('price');
        remarketingVariables.cityName = currElement.attr('cityName');
        remarketingVariables.sellerType = currElement.attr('seller');
        remarketingVariables.sellerDetailsClick = "2" //Photo Gallery Get Seller Details
        var absureCertified = false;
        var maskingNo = $.trim(currElement.parents('div.stock-detail').find('p.get-seller-tel span.dealerMaskingNumber').text());
        var tempStr = '';
        var thisElement = currElement.parents('div.stock-detail');
        var stockList = currElement.parents('li');
        var node = stockList.find('.contact-seller');
        //$('#pg-more-details1').attr('href', currElement.parents('div.stock-detail').find('#linkToDetails').attr('href') + "?rk=" + currElement.parents('li').attr('rankAbs') + "&isP=" + (currElement.parents('li').attr('isPremium') == undefined ? false : currElement.parents('li').attr('isPremium')));
        $('#pg-more-details1').attr('href', currElement.parents('div.stock-detail').find('#linkToDetails').attr('href'));
        $('#pg-more-details1').attr('profileId', currElement.parents('div.stock-detail').find('#linkToDetails').attr('profileid'));
        $('#pg-more-details1').attr('rank', stockList.attr('rank'));
        $('#pg-more-details1').attr('rankAbs', stockList.attr('rankAbs'));
        $('#pg-more-details1').attr('isPremium', stockList.attr('isPremium'));
        if (currElement.parents('li').attr('isPremium') == undefined) {
            $('#pg-more-details1').attr('isPremium', false);
        }
        var pgGetDetails = $('#pg_get_details');
        var $stockListChatContainer = stockList.find('.chat-btn-container');
        var $pgChatContainer = $('#pg_chat_btn_container');
        if ($stockListChatContainer.length < 1 || $stockListChatContainer.hasClass('hide')) { // for knockout and repeater.
            $pgChatContainer.addClass('hide');
            pgGetDetails.removeClass('grid-8').addClass('grid-12');
        }
        else {
            $pgChatContainer.removeClass('hide');
            pgGetDetails.removeClass('grid-12').addClass('grid-8');
        }
        pgGetDetails.attr('profileId', currElement.parents('div.stock-detail').find('#linkToDetails').attr('profileid'));
        pgGetDetails.attr('rank', stockList.attr('rank'));
        pgGetDetails.attr('rankAbs', stockList.attr('rankAbs'));
        pgGetDetails.attr('isPremium', stockList.attr('isPremium'));
        if (currElement.parents('li').attr('isPremium') == undefined) {
            pgGetDetails.attr('isPremium', false);
        }
        pgGetDetails.attr('cityid', node.attr('cityid'));
        pgGetDetails.attr('dc', node.attr('dc'));
        pgGetDetails.attr('rootid', node.attr('rootid'));
        pgGetDetails.attr('bodystyleid', node.attr('bodystyleid'));
        pgGetDetails.attr('versionsubsegmentid', node.attr('versionsubsegmentid'));
        pgGetDetails.attr('pricenumeric', node.attr('pricenumeric'));
        pgGetDetails.attr('kmnumeric', node.attr('kmnumeric'));
        pgGetDetails.attr('stockrecommendationsurl', node.attr('stockrecommendationsurl'));
        pgGetDetails[0].dataset.slotId = node[0].dataset['slotId'];
        pgGetDetails[0].dataset.ctePackageId = node[0].dataset['ctePackageId'];
        
        pgGetDetails.attr('makeId', node.attr('makeId'));
        tempStr += '<li title="' + thisElement.find('span.slkms').text() + ' km">' +
                           '<span class="uc-gallery-sprite km-icon"></span> ' +
                           '<span class="point-txt">' + thisElement.find('span.slkms').text() + '</span>' +
                           '</li>';
        tempStr += '<li title="' + thisElement.find('span.slcolor').text() + '">' +
                            '<span class="uc-gallery-sprite silver-icon"></span> ' +
                            '<span class="point-txt">' + thisElement.find('span.slcolor').text() + '</span>' +
                            '</li>';
        tempStr += '<li title="' + thisElement.find('span.slowners').text() + '">' +
                            '<span class="uc-gallery-sprite owner-icon"></span> ' +
                            '<span class="point-txt">' + thisElement.find('span.slowners').text() + '</span>' +
                            '</li>';
        tempStr += '<li title="' + thisElement.find('span.slfuel').text() + '">' +
                            '<span class="uc-gallery-sprite petrol-icon"></span> ' +
                            '<span class="point-txt">' + thisElement.find('span.slfuel').text() + '</span>' +
                            '</li>';
        $('#pg-car-features ul').html(tempStr);
        var sellerName = currElement.parents('.stock-detail').find('a.contact-seller').attr('sellername');
        $('#pg-maskingNo-container').find('.uc-pg-contact-name-box').attr('title', sellerName);
        $('#pg-maskingNo-container').find('.uc-pg-contact-name-box').text(sellerName);
        //Delivery text in Photo Gallery
        D_usedSearch.photoGallery.showDeliveryNotice(thisElement);

        if (maskingNo != "" && maskingNo != undefined && maskingNo != null) {
            $('#pg-mskNo').text(maskingNo);
            $('#pg-maskingNo-container').show();
        }
        else {
            $('#pg-maskingNo-container').hide();
        }

        $('#pg-car-name').text(currElement.parents('li').find('.spancarname').text());
        $('#pg-car-price').text(currElement.attr('price'));
        $('#photoGallery #pg_seller_details').attr('ProfileId', profileId);
        var pgSellerDetails = $('#pg_seller_details');
        pgSellerDetails.attr('rank', stockList.attr('rank'));
        pgSellerDetails.attr('rankAbs', stockList.attr('rankAbs'));
        pgSellerDetails.attr('isPremium', stockList.attr('isPremium'));
        if (currElement.parents('li').attr('isPremium') == undefined) {
            pgSellerDetails.attr('isPremium', false);
        }

        $('#photoGallery').attr('SellerType', stockList.find('span[seller]').text());
        var isDealer = profileId.charAt(0);
        if (isDealer == "D")
            isDealer = "true";
        else if (isDealer == "S")
            isDealer = "false";
        $.getImagesUrls(profileId.replace("D", "").replace("S", ""), isDealer, absureCertified);
        //$("#pgNextCityCars").hide();
        setTimeout(function () { }, 600);
        d_usp_pgNavigation.bindNavigatingArrowsOnPG(nodePG, prevPgListingId);

    });
};

$.fn.bindSlideShowClose = function () {
    return this.click(function () {
        $('#pgNextCityCars').animate({ right: '-167', opacity: '0' });
        $('.nextBtn').unbind('mouseenter');
        $("#nextBtnArrow").addClass('unbindNavigation');
        $('#pgNextCityCars').unbind('mouseleave');

        var node = $("li.listingContent[rankabs='" + $('#pg_seller_details').attr('rankabs') + "']").first();
        if (node.length) {
            $('html, body').animate({
                scrollTop: node.offset().top - 230
            }, 1);
        }
        $("#pgNextCityCars").attr('cityname', 'NbCity');

        $('#videoContainer').find('iframe').attr('src', '');
        $('#photoGallery, #pg-btn-close').hide();
        $('.blackOut-window-bt').hide();
        D_usedSearch.photoGallery.hideNextAvailableCar();
        $('#photoGallery').removeClass('galleryVisible');
        $('#pg-txtCwiCode').text('');
        $('html, body').css({
            'overflow': 'auto',
            'height': 'auto'
        });
        $('#galleryHolder .pg-image').html('');
        $('#galleryList .pg-thumb-list').html('');
        $('.pg-tollFree').removeClass('error');
        $("#pg-not_auth").hide();
        $("#pg-txtCwiCodeError").hide();
        $("#pg-txtValidCwiCodeError").hide();
        $("#pg-txtCwiCode").removeClass("red-border");
        $("#pg-txtCwiCode").val("");
        $("#pg_txtName, #pg_txtMobile , #pg_emailTick, #pg_txtValidMobileError ,#pg_txtEmail,#pg_txtValidEmailError").removeClass("red-border");
        $("#pg_txtNameError,#pg_txtMobileError,#pg_txtEmailError,#pg_txtValidEmailError,#pg_txtValidMobileError").hide();
        $('#txtCaptchaCode, #pg-txtCaptchaCode').val("");
        $('.captcha').hide();
        //$('#captchaCode, #pg-captchaCode').hide();
    });

};

$.fn.closePhotoGallery = function () {
    return this.click(function () {
        if ($('#photoGallery').is(':visible'))
            $('#pg-btn-close').trigger('click');
    });
};

//Get all images for particular profileId And Open ColorBox for ImageGallery
$.getImagesUrls = function (inquiryId, isDealer, absureCertified) {
    $.ajax({
        type: "GET",
        url: "/webapi/Classified/GetProfileImageDetails/?inquiryId=" + inquiryId + "&isDealer=" + isDealer,
        dataType: 'text',
        success: function (response) {
            var jsonparse = $.parseJSON(response);
            var imgThumbJson = jsonparse.ImgThumbURLs;
            var imgFullJson = jsonparse.ImgFullURLs;
            var images = '', imageUl = '';
            var imgWidth = 0;
            for (var i = 0; i < imgThumbJson.length; i++) {
                images += '<li>';
                if (i == 0) {
                    images += '<a href="' + imgFullJson[i] + '" original-href="' + imgFullJson[i] + '" class="pg-thumb' + i + ' pg-active">' +
                                  '<img src="' + imgThumbJson[i] + '" border="0" alt="' + $('#pg-car-name').text() + "-" + i + '" title="' + $('#pg-car-name').text() + "-" + i + '" desc="" artid="" imgcnt="30" arttitle="" arturl="">';
                }
                else {
                    images += '<a href="' + imgFullJson[i] + '" original-href="' + imgFullJson[i] + '" class="pg-thumb' + i + '">' +
                                  '<img src="' + imgThumbJson[i] + '" border="0" alt="' + $('#pg-car-name').text() + "-" + i + '" title="' + $('#pg-car-name').text() + "-" + i + '" desc="' + $('#pg-car-name').text() + "-" + i + '" artid="" imgcnt="30" arttitle="" arturl="" style="opacity: 0.7;">';
                }
                images += '</a>' +
                      '</li>';
                imgWidth += 143;
            }

            imageUl += '<div class="pg-back"><a></a></div>' +
                                    '<div class="pg-thumbs">' +
                                        '<ul class="pg-thumb-list">';

            images += '</ul>' +
                         '</div>' +
                      '<div class="pg-forward"><a></a></div>';
            images = imageUl + images;
            $('#galleryList').html(images).trigger('updated'); //.promise().done(function () {
            if (jsonparse.YoutubeURL) {
                videoUrl = "https://www.youtube.com/embed/" + jsonparse.YoutubeURL;
                $('#videoContainer').find('iframe').attr('src', videoUrl);
                $('.gallery-tabs-top').show();
                $('#videoTab').trigger('click');
            }
            else {
                $('#videoContainer').find('iframe').attr('src', '');
                $('.gallery-tabs-top').hide();
                $('#photoTab').trigger('click');
            }
            $.hideLoading();
            $('.blackOut-window-bt').show();
            $('#photoGallery, #pg-btn-close').fadeIn('500');
            if (!$('#photoGallery').hasClass('galleryVisible')) {
                $('#photoGallery').addClass('galleryVisible');
            }
            if ($.cookie("TempCurrentUser") != null) {
                var tmpCurrentUser = $.cookie("TempCurrentUser").split(':');
                $("#pg_txtName").val(tmpCurrentUser[0]);
                $("#pg_txtMobile").val(tmpCurrentUser[1]);
            }
            var StartIndex = 0;
            var LiIndex = 0;
            $('#galleryList li').each(function () {
                if ($(this).children('a').attr('href').indexOf(ImageName) > 0)
                    StartIndex = LiIndex;
                LiIndex++
            });
            var galleries = $('.pg-gallery').adGallery({
                start_at_index: StartIndex,
                loader_image: "https://imgd.aeplcdn.com/0x0/adgallery/loader.gif",
                callbacks: { // CallBack function after the image is loaded and is visible
                    afterImageVisible: function () {
                        $('#artTitle').html(this.images[this.current_index].artTitle);
                        $('#artTitle').attr('href', this.images[this.current_index].artUrl);
                        var gallery_info = this.gallery_info.html();
                        if (this.images[this.current_index].imgCnt == 0) {
                            this.gallery_info.hide();
                            $('div.pg-forward').add('div.pg-back').add('div.pg-next').add('div.pg-prev').hide();
                        }
                    }
                },
                slideshow: {
                    enable: true,
                    autostart: true,
                    speed: 1000
                }
            });
            D_usedSearch.photoGallery.hidePgArrow();
            InitGallery(galleries); // Initialize the gallery
            LoadGallery(MakeId, ModelId); // Load the Similar and other models Gallery
            $('#pg_contactSellerForm').show();
            $('#pg-mobile-verification , #pg-seller-info').hide();
            $('#pg_txtName').focus();
        }
    });
};

$.photoGalleryIconClick = function () {
    $('.photoIcon, .image-container__gallery-icon').unbind('click');
    $('.photoIcon, .image-container__gallery-icon').click(function () {
        $(this).parents('.stock-detail').find('a.slideShow').trigger('click');
        $('#photoTab').trigger('click');
    });
    $('.videoIcon').unbind('click');
    $('.videoIcon').click(function () {
        $(this).parents('.stock-detail').find('a.slideShow').trigger('click');
        $('#videoTab').trigger('click');
    });
    $('#photoTab').unbind('click');
    $('#photoTab').click(function () {
        $('#gallery').css({ 'visibility': 'visible', 'height': 'auto', 'overflow': 'inherit' }).show();
        $('#videoContainer').hide();
        $('#videoContainer').find('iframe').attr('src', '');
        $('#gallery').show();
        $('#videoTab').removeClass('bold');
        $('#photoTab').addClass('bold');
        $('#pg_txtName').focus();
    });
    $('#videoTab').unbind('click');
    $('#videoTab').click(function () {
        $('#gallery').css({ 'visibility': 'hidden', 'height': '0px', 'overflow': 'hidden' });
        $('#videoContainer').find('iframe').attr('src', videoUrl);
        $('#videoContainer').show();
        $('#photoTab').removeClass('bold');
        $('#videoTab').addClass('bold');
        $('pg_txtName').focus();
    });
};

/*********************************************** PhotoGallery Functionality Ends*********************************************************/
/*********************************************** City Pop Up Functionality Starts********************************************************/
$.showCityPopup = function () {
    setTimeout(function () {
        if ($.checkCookie("UsedCarsVisitedCookie", "Yes", 24)) {
            return false;
        } else {
            $('#drpCity2').html($('#drpCity').html());
            $('#drpCity2').change(function () {
                $.city = $(this).val();
                if (parseInt($.city) > 0) {
                    $.pageNo = 1;
                    $.changeHeadingText();
                    $('#drpCity').val($.city);
                    var tempQS = $.trim($.getAllParamsFromQS());
                    tempQS = $.appendToQS(tempQS, "city", $.city)
                    $.pushState(tempQS);
                }
                $('#drpCity2').val($.city);
                $("#findCityContent").prev().hide();
                $("#findCityContent").hidesetCookie
            });
            if ($.city == '') {
                $("#findCityContent").prev().show();
                $("#findCityContent").show();
            }
        }
    }, 0);
};

$.getCookie = function (visited) {
    var Isvisited = visited + "=";
    var ca = document.cookie.split(';');
    for (var i = 0; i < ca.length; i++) {
        var c = ca[i];
        while (c.charAt(0) == ' ') c = c.substring(1);
        if (c.indexOf(Isvisited) != -1)
            return c.substring(Isvisited.length, c.length);
    }
    return "";
};

$.checkCookie = function (cookieName, cookieValue, duration) {
    var visited = true;
    var visit = $.getCookie(cookieName);
    if (visit == "") {
        visited = false;
        $.setCookie(cookieName, cookieValue, duration);
    }
    return visited;
}


$.fn.popularCityClicks = function () {
    return this.click(function () {
        $.city = $(this).attr('cityId');
        if ($.city > 0) {
            $.pageNo = 1;
            $.changeHeadingText();
            $('#drpCity').val($.city);
            var tempQS = $.trim($.getAllParamsFromQS());
            tempQS = $.appendToQS(tempQS, "city", $.city)
            $.pushState(tempQS);
        }
        $('#drpCity2').val($.city);
        var element = $("#findCityContent");
        element.prev().hide();
        element.hide();
        return true;
    });
}

$.fn.closeCityPopup = function () {
    return this.click(function () {
        var element = $("#findCityContent");
        element.prev().hide();
        element.hide();
    });
};

// Set cookie for restricting the pop up session for 24 hours
$.setCookie = function (cookieName, cookieValue, exhours) {
    var d = new Date();
    d.setTime(d.getTime() + (exhours * 60 * 60 * 1000));
    document.cookie = cookieName + " = " + cookieValue + "; expires=" + d.toUTCString() + "; path=/";
};

/*********************************************** City Pop Up Functionality Ends********************************************************/
$.insertInSearchProperties = function () {
    var cityId = $('#drpCity option:selected').val();
    var _cityName = $.trim($('#drpCity option:selected').text().split('(')[0]);
    var _carFilter = $.getFilterFromQS('car').split(' ')[0];
    var leaderBoard, ad1, ad2, ad3 = "";
    var regexModel = /^[0-9]+\./;
    var isModelSelected = false;
    leaderBoard = "<div class=\"adunit border-none\" data-adunit=\"UsedCarSearch_970x90\" data-dimensions=\"970x90\" data-targeting='{\"City\":\"" + _cityName + "\"}'></div>";

    if ($.pageNo == '')
        $.pageNo = 1;
    
    var listingsKnockout = $("#listing" + $.pageNo + " .listingContent");
    var listingsRepeater = $("#listingsData .listingContent");
    var listing = listingsKnockout.length > 0 ? listingsKnockout : listingsRepeater;

    var _makeName = _carFilter && !regexModel.test(_carFilter) ? $('#makesList li[carfilterid*="' + _carFilter + '"] span[class="filterText"]').text() : "";
    var _modelName = _carFilter && regexModel.test(_carFilter) ? $('#makesList li[carfilterid*="' + _carFilter + '"] span[class="model-txt"]').text() : "";

    var targetJSON = "{\"City\":\"" + _cityName + "\",\"UsedMake\":\"" + _makeName + "\",\"UsedModel\":\"" + _modelName + "\"}";

    var targetBudget = $.getFilterFromQS('budget');
    var budgetRange = $.findTargetingBudget(targetBudget);
    var leaderBoardTargetJSON = "{\"City\":\"" + _cityName + "\",\"UsedMake\":\"" + _makeName + "\",\"UsedModel\":\"" + _modelName + "\",\"BudgetRange\":\"" + budgetRange + "\"}";

    leaderBoard = "<div class=\"adunit border-none\" data-adunit=\"UsedCarSearch_970x90\" data-dimensions=\"970x90\" data-targeting='" + leaderBoardTargetJSON + "'></div>";
    ad1 = "<li class=\"no-padding border-none ad-no-hover-carousel\" style='width:100%;'><div style='text-align: center;' class=\"adunit sponsored content-inner-block padding-top5 padding-bottom5\" data-adunit=\"UsedCar_InSearch_728x90_Top\" data-dimensions=\"728x90\" data-targeting='" + targetJSON + "'></div></li>";
    ad2 = "<li class=\"no-padding border-none ad-no-hover-carousel\" style='width:100%;'><div style='text-align: center;' class=\"adunit sponsored content-inner-block padding-top5 padding-bottom5\" data-adunit=\"UsedCar_InSearch_728x90_Middle\" data-dimensions=\"728x90\" data-targeting='" + targetJSON + "'></div></li>";
    ad3 = "<li class=\"no-padding border-none ad-no-hover-carousel\" style='width:100%;'><div style='text-align: center;' class=\"adunit sponsored content-inner-block padding-top5 padding-bottom5\" data-adunit=\"UsedCar_InSearch_728x90_Bottom\" data-dimensions=\"728x90\" data-targeting='" + targetJSON + "'></div></li>";
    $("#Leader_Board").html(leaderBoard);

    $(listing).eq(7).after(ad1);
    $(listing).eq(13).after(ad2);
    $(listing).eq(17).after(ad3);

    $.dfp({
        dfpID: '1017752',
        enableSingleRequest: false,
        afterEachAdLoaded: function (adunit) {
            var parentAdUnit = $(adunit).parent();
            if (!$(adunit).hasClass('display-block')) parentAdUnit.addClass("hide");
        }
    });

};

$.findTargetingBudget = function (budget) {
    var min = budget.split('-')[0];
    var max = budget.split('-')[1];
    min = min != "" && min != undefined ? Math.floor(min / 10) >= 4 ? "above40" : (Math.floor(min / 10)) * 10 : "any";
    max = (max != "" && max != undefined && max != 0) ? Math.floor((max / 10) + 1) > 4 ? (max==40 ? "40" :"40,above40") : ((max % 10 == 0) ? (Math.floor(max / 10)) * 10 : (Math.floor((max / 10) + 1)) * 10) : "any" ;
    max = min != "above40" && min != "any" ? ("-" + max) : "";
    var range = min + max;
    range = min == 0 && max == "-any" ? "any" : range;
    return range;
};

//name of the filter is name param / value of the filter is value param / type of the filter is type param / values excepted by type param 1 - city 2 - checkbox 3 - sort
$.updateFilters = function (node, name, value, type) {

    $.pageNo = 1;
    if (type == 1) {
        $.applyCityFilter(name, value);
    }
    else if (type == 2) {

        $.applyCheckBoxFilter(name, value, node);
    }
    else if (type == 3) {
        $.applySortFilter(node);
    }
    $.goTop();//Added by Sachin Bharti(10th of Aug)
};

$.applyCheckBoxFilter = function (name, value, node) {
    $.removePageNoParam();
    $.removeKnockouts();
    $.removeExcludedStocksFromQS();
    var checked = 'active';
    var completeQS = '';
    var curValue = $.getFilterFromQS(name).replace(/ /g, '+');
    if (node.hasClass(checked)) {
        node.removeClass(checked);
        completeQS = $.updateCheckBoxFilterInQS(name, value, curValue, false);
    } else {
        node.addClass(checked);
        if (curValue.length > 0) {
            completeQS = $.updateCheckBoxFilterInQS(name, value, curValue, true);
        }
        else {
            completeQS = $.addFilterInQS(name, value);
        }
    }
    $('#' + name).empty();
    $.pageNo = 1;
    $.pushState(completeQS);
};

$.applySortFilter = function (node) {
    var completeQS = $.removeFilterFromQS('so');
    window.location.hash = completeQS;
    $.removeExcludedStocksFromQS();
    completeQS = $.removeFilterFromQS('sc');
    window.location.hash = completeQS;
    var so = node.find('option:selected').attr('so');
    var sc = node.find('option:selected').attr('sc');
    if (completeQS.length > 0) {
        if (so.length > 0 && sc.length > 0)
            completeQS = completeQS + "&so=" + so + "&sc=" + sc;
    }
    else {
        if (so.length > 0 && sc.length > 0)
            completeQS = "so=" + so + "&sc=" + sc;
    }
    $.pageNo = 1;
    $.pushState(completeQS);
};

$.applyCityFilter = function (name, value) {
    $.removePageNoParam();
    $.removeKnockouts();
    $.removeExcludedStocksFromQS();
    var curValue = $.getFilterFromQS(name);
    var qs = '';
    var completeQs = $.removeFilterFromQS(name);
    window.location.hash = completeQs;
    completeQs = $.removeFilterFromQS('pc');
    if (completeQs.length > 0)
        qs = completeQs + "&" + name + "=" + value;
    else
        qs = name + "=" + value;
    qs = $.addParameterToString('pc', value, qs);
    $.pageNo = 1;
    $.pushState(qs);
};

$.getFilterFromQS = function (name) {
    var hash = window.location.hash.replace('#', '');
    var params = hash.split('&');
    var result = {};
    var propval, filterName, value;
    var isFound = false;
    var paramsLength = params.length;
    for (var i = 0; i < paramsLength; i++) {
        var propval = params[i].split('=');
        filterName = propval[0];
        if (filterName == name) {
            value = propval[1];
            isFound = true;
            break;
        }
    }
    if (isFound && value.length > 0) {
        if (value.indexOf('+') > 0)
            return value.replace(/\+/g, " ");
        else
            return value;
    }
    else
        return "";
};

$.getFilterFromQSNB = function (name, qsTemp) {
    var params = qsTemp.split('&');
    var result = {};
    var propval, filterName, value;
    var isFound = false;
    var paramsLength = params.length;
    for (var i = 0; i < paramsLength; i++) {
        var propval = params[i].split('=');
        filterName = propval[0];
        if (filterName == name) {
            value = propval[1];
            isFound = true;
            break;
        }
    }
    if (isFound && value.length > 0) {
        if (value.indexOf('+') > 0)
            return value.replace(/\+/g, " ");
        else
            return value;
    }
    else
        return "";
};

$.getAllParamsFromQS = function () {
    var completeQS = window.location.hash.replace('#', '');
    var tempParams = completeQS.substring(0, completeQS.length).split('&');
    var params = [];
    if (tempParams.length > 0) {
        for (var i = 0; i < tempParams.length; i++) {
            params.push(tempParams[i].split('=')[0]);
        }
    }
    return params;
};

$.addFilterInQS = function (name, value) {
    var hash = window.location.hash.replace('#', '');
    if (hash.length > 0)
        return hash.substring(0, hash.length) + "&" + name + "=" + value;
    else
        return name + "=" + value;
}

$.updateCheckBoxFilterInQS = function (name, value, curValue, toAdd) {
    var completeQS = '';
    if (toAdd == true)
        completeQS = $.addValueToCheckBoxFilterInQS(name, value, curValue);
    else
        completeQS = $.removeValueFromCheckBoxInQS(name, value, curValue);

    return completeQS;
};

$.removeExcludedStocksFromQS = function () {
    excludeStocks = undefined;
    window.location.hash = $.removeFilterFromQS('excludestocks');
};

$.addValueToCheckBoxFilterInQS = function (name, value, curValue) {
    var completeQS = $.removeFilterFromQS(name);
    var values = curValue.split('+');
    var temp = name + "=" + value;
    for (var i = 0; i < values.length; i++)
        temp = temp + "+" + values[i];
    if (completeQS.length > 0)
        completeQS = completeQS + "&" + temp;
    else
        completeQS = temp;
    return completeQS;
};

$.removeValueFromCheckBoxInQS = function (name, value, curValue) {
    var completeQS = $.removeFilterFromQS(name);
    var values = curValue.split('+');
    var temp = '';
    for (var i = 0; i < values.length; i++) {
        if (values[i] != value) {
            if (temp.length > 0)
                temp = temp + "+" + values[i];
            else
                temp = values[i];
        }
    }
    var param = '';
    if (temp.length > 0)
        param = name + "=" + temp;

    if (completeQS.length > 0 && param.length > 0)
        completeQS = completeQS + "&" + param;
    else if (param.length > 0)
        completeQS = param;

    return completeQS;
};

$.pushState = function (qs) {
    window.location.hash = qs;
    $.showLoadingTxt();
    $.appendSelectedFilters();
    if (qs.indexOf('#') > -1)
        qs = qs.replace('#', '');
    $("#nearByCityList li").removeClass("selected");
    $("#nearByCityList .active").addClass("selected");
    $.hitAPI(qs, false);
};

$.removeSortParam = function () {
    if ($.getFilterFromQS('so').length > 0 && $.getFilterFromQS('sc').length > 0) {
        var completeQS = $.removeFilterFromQS('so');
        window.location.hash = completeQS;
        completeQS = $.removeFilterFromQS('sc');
        window.location.hash = completeQS;
    }
};

$.removePageNoParam = function () {
    if ($.getFilterFromQS('pn').length > 0) {
        var completeQS = $.removeFilterFromQS('pn');
        completeQS = D_usedSearch.utils.removeCarRanksFromQS(completeQS);
        $.pageNo = 1;
        window.location.hash = completeQS;
    }
};

$.removeFilterFromQS = function (name) {
    var url = window.location.hash.replace('#', '');
    if (url.length > 0) {
        var prefix = name + '=';
        var pars = url.split(/[&;]/g);
        for (var i = pars.length; i-- > 0;) {
            if (pars[i].indexOf(prefix) > -1) {
                pars.splice(i, 1);
            }
        }
        url = pars.join('&');
        return url;
    }
    else
        return "";
};
$.removeFilterFromQSNB = function (name, url) {

    if (url.length > 0) {
        var prefix = name + '=';
        var pars = url.split(/[&;]/g);
        for (var i = pars.length; i-- > 0;) {
            if (pars[i].indexOf(prefix) > -1) {
                pars.splice(i, 1);
            }
        }
        url = pars.join('&');
        return url;
    }
    else
        return "";
};

$.appendSelectedFilters = function () {
    var params = $.getAllParamsFromQS();
    var showReset = false;
    for (var i = 0; i < params.length; i++) {
        if (params[i].length > 0 && params[i] != 'budget' && params[i] != 'city' && params[i] != 'year' && params[i] != 'kms' && params[i] != 'pn' && params[i] != 'issold' && params[i] != 'car') {
            var node = $('div[name=' + params[i] + ']');
            var filterNode = $('#' + params[i]).empty();
            var values = $.getFilterFromQS(params[i]).replace(/ /g, '+').split('+');
            for (var j = 0; j < values.length; j++) {
                if (values[j].length > 0 && params.length > 0) {
                    var filter = node.find('li[filterid=' + values[j] + ']');
                    var html = "<span parentfiltername='" + params[i] + "' filterid='" + values[j] + "' class='close-icon filter-parameters'> " + filter.find('.filterText').text() + " <span class='cwsprite cross-sm-lgt-grey'></span></span>&nbsp;";
                    filterNode.append(html);
                    showReset = true;
                }
            }
        }
        else if (params[i] == 'car') {
            var node = $('#manu-box ul.ul-makes');
            var filterNode = $('#car').empty();
            var values = $.getFilterFromQS(params[i]).replace(/ /g, '+').split('+');
            for (var j = 0; j < values.length; j++) {
                if (values[j].length > 0 && params.length > 0) {
                    var filter = node.find('li[carfilterid="' + values[j] + '"]');
                    var name = '';
                    if (filter.find('.filterText').length > 0)
                        name = filter.find('.filterText').text();
                    else
                        name = filter.find('.model-txt').text();
                    //Added by Sachin Bharti(8th Aug 2015)
                    if (name != "") {
                        var html = "<span parentfiltername='" + params[i] + "' carfilterid='" + values[j] + "' class='makeModel filter-parameters'> " + name + " <span class='cwsprite cross-sm-lgt-grey makeModel'></span></span>&nbsp;";
                        filterNode.append(html);
                        showReset = true;
                    }

                }
            }
        }
    }
    if (showReset)
        $('div.sel-filter-box').show();
    else
        $('div.sel-filter-box').hide();
    $('.close-icon').removeCurrentFilter();
    $('.makeModel').removeMakeRootFilter();
};

$.selectFiltersPresentInQS = function () {
    //need not do for city as it can be selected from code behind
    var params = $.getAllParamsFromQS();
    for (var i = 0; i < params.length; i++) {
        if (params[i].length > 0 && params[i] != 'car' && params[i] != 'budget' && params[i] != 'year' && params[i] != 'kms') {
            var node = $('div[name=' + params[i] + ']');
            var values = $.getFilterFromQS(params[i]).replace(/ /g, '+').split('+');
            for (var j = 0; j < values.length; j++) {
                node.find('li[filterid=' + values[j] + ']').addClass('active');
            }
        }
        else if (params[i] == 'budget')
            $.budget = $.getFilterFromQS(params[i]);
        else if (params[i] == 'year')
            $.year = $.getFilterFromQS(params[i]);
        else if (params[i] == 'kms')
            $.kms = $.getFilterFromQS(params[i]);
    }
};

$.hideLoading = function () {
    $('.blackOut-window, #newLoading').hide();
};

$.showLoading = function () {
    $('.blackOut-window, #newLoading').show();
};
$.hideLoadingTxt = function () {
    $('#loadingTxt').hide();
};

$.showLoadingTxt = function () {
    $('#loadingTxt').show();
};

$.hitAPI = function (searchUrl, nbCity) {
    $.ajax({
        context: this,
        type: 'GET',
        headers: { "sourceid": "1" },
        url: '/webapi/classified/stockfilters/?' + searchUrl,
        dataType: 'text',
        success: function (json) {
            var response = $.parseJSON(json);
            D_usedSearch.latestNonPremiumCarRank = response.LastNonFeaturedSlotRank;
            D_usedSearch.latestPremiumDealerCarRank = response.LastDealerFeaturedSlotRank;
            D_usedSearch.latestPremiumIndividualCarRank = response.LastIndividualFeaturedSlotRank;
            excludeStocks = response.ExcludeStocks;
            if (nbCity == true) {
                var nbCityTotalCount = response.FiltersData.stockcount.TotalStockCount;
                //FIX: Remove the response.ResultsData since API changed and we will no loger get it. done to avoid error during release
                var listingJson = JSON.parse('{"listing":' + JSON.stringify(response.ResultsData || response.ResultData) + '}');
                NBCITYTOTALCOUNT = listingJson.listing.length;
                var currentNBCityResults = new Array();
                var listingObj = { listing: "" };
                listingObj.listing = currentNBCityResults;
                for (var i = 0; i < NBCITYTOTALCOUNT; i++) {
                    listingObj.listing.push(listingJson.listing[i])
                    if (listingJson.listing[i].NBCityStripId != null)
                    {
                        if (listingObj.listing.length > 0) {
                            $.bindLazyListings(listingObj);
                            $(".stock-list").last().find(".pg-out-box").text("Page " + $.pageNoNB);
                        }
                        $.nbCityData = true;
                        $('div.getNextPage').show();
                        initGetSellerDetails();
                        $.bindPhotoGallery();
                        $("#nbCitiesTitle").show();
                        $.sellerDetailsBtnTextChange();
                        var currentNBCityCount = parseInt($('#nearByCityList li[cityid="' + listingJson.listing[i].NBCityStripId + '"]').find('.city-count').text());
                        if (NBCITYCHANGED == true && currentNBCityCount > 0 && !$(".stock-list").last().find("#nbCitiesTitle").length) {
                            $(".stock-list").last().find(".pg-out-box").before('<h3 id="nbCitiesTitle"  class="more-cars-headingColor margin-top10">More Used Cars from <span class="margin-left10 font14 text-unbold text-light-grey">(25 Cars)</span></h3>')
                            var cityName = $('#nearByCityList li[cityid="' + listingJson.listing[i].NBCityStripId + '"]').text().split('(')[0];
                            if (listingJson.listing[i].NBCityStripId == 9999)
                                cityName = "All Over India";
                            if (currentNBCityCount > 1)
                                $(".stock-list").last().find('#nbCitiesTitle').text("More Used Cars from " + cityName + ' (' + currentNBCityCount + ' Cars)');
                            else
                                $(".stock-list").last().find('#nbCitiesTitle').text("More Used Cars from " + cityName + ' (' + currentNBCityCount + ' Car)');
                        }
                       
                        if (currentNBCityCount > 0) {
                            $.pageNo++;
                            $.lazyLoadImages("listing" + $.pageNo);
                        }
                        listingObj.listing = [];
                    }
                }
                NBCITYCHANGED = false;
                $.hideLoadingTxt();
            }

            else {
                $.pageNoNB = 1;
                pageCountForNBCity = 0;
                if ($("#noCarsFound").is(':visible'))
                    $("#noCarsFound").hide();
                $(".stock-list ul.LLko").each(function () {
                    if ($(this).length < 1)
                        $(this).parent().remove();
                });
                qsTemp = window.location.hash.replace('#', '');
                $.jsonData = response.FiltersData;
                $.bindCounts(response.FiltersData);
                $.bindMakeRootCounts(response.FiltersData.Makes);
                $.setCarOnLoad();
                var nbCityTotalCount = response.FiltersData.stockcount.TotalStockCount;
                //FIX: Remove the response.ResultsData since API changed and we will no loger get it. done to avoid error during release
                var listingJson = JSON.parse('{"listing":' + JSON.stringify(response.ResultsData || response.ResultData) + '}');
                dataLayer.push({ event: 'Page#' + $.pageNo, cat: 'UsedCarSearch_Pageviews', act: 'Page#' + $.pageNo });
                if (!isNaN($.pageNo) && $.pageNo == 1) {
                    $.indexNonFeatured = 1;
                    $.indexFeatured = 1;
                    $.indexAbsolute = 1;
                    $.bindListings(listingJson);
                }
                else {
                    $.bindLazyListings(listingJson);
                }
                $.sellerDetailsBtnTextChange();
                $.bindPhotoGallery();
                initGetSellerDetails();
                if (typeof classifiedShortlistCars != 'undefined')
                    classifiedShortlistCars.slButtonToggleForLL();

                if (load) {
                    $.callMethodsForLoad();
                    load = false;
                }
                if (isCityChange) {
                    $.changeHeadingText();
                    if ($('#drpCity option:selected').val() > 0) {
                        $.getNearByCityArr(response.NearByCitiesWithCount);
                        $.bindNearByCity();
                        isCityChange = false;
                    }
                    else
                        $('#nearByCityList').empty();
                }
                $.bindTotalRecordCount(nbCityTotalCount);
                $.checkSuperLuxuryCondition();
                $.refreshNearByCityCount();
                $.bindNoRecordshowMoreCars(nbCityTotalCount);
                if (PAGELOAD)
                    PAGELOAD = false;
                
                $.hideLoadingTxt();
                //$('img.lazy').lazyload({ load: function () {alert(1) }});
                $.lazyLoadImages("listing" + $.pageNo);
                $('#tags').trigger('keyup');//Added by Sachin Bharti(11th Aug 2015)
                $('div.getNextPage').show();

                var nbcFlag = checkNearByCityCount();
                if (nbCityTotalCount == 0 && $(".stock-list li").length == 0 && $("#nearByCityList .selected").attr("cityid") != 3000 && $("#nearByCityList .selected").attr("cityid") != 3001 && $("#nearByCityList .selected").attr("cityid") != 9999 && $("#nearByCityList .selected").attr("cityid") != 0 && !nbcFlag) {
                    var cityName = $("#nearByCityList li.active").text().split('(')[0];
                    $("#noCarsFoundCityName").text('');
                    $("#noCarsFoundCityName").append(cityName);
                    $("#noCarsFound").show();
                    D_usedSearch.cityWarning.resetWarning();
                    if (noCarTimer != null)
                        clearTimeout(noCarTimer);
                    noCarTimer = setTimeout(function () { $("#noCarsFound").hide("slow"); }, 10000);
                }
                if ($.soldOut != "true" && D_usedSearch.cityWarning.isListingsPresent()) {
                    if (D_usedSearch.isFromPageLoad && !D_usedSearch.cityWarning.isUserCityChanged) {
                        D_usedSearch.cityWarning.checkWarningConditions();
                        D_usedSearch.cityWarning.toggleCityWarning();
                    }
                    else {
                        D_usedSearch.cityWarning.isUserCityChanged = false;
                    }
                }
            }
            $.insertInSearchProperties();
            var selectedCityId = parseInt($('#nearByCityList').find('li.selected').attr('cityid'));
            D_usedSearch.similarCars.showSimilarCarLink(nbCityTotalCount, searchUrl);
            if (remainingCount <= 0 && nbCity == false && selectedCityId != 3000 && selectedCityId != 3001) {
                var nbCityInfo = d_usp_nbcityapiHit.getNextNbCityWithCount();
                if (nbCityInfo.nbCitiesId.length)
                    getNbCityData(nbCityInfo);
            }
            
            //  To align filters on scroll
            var mainListHt = $('div.main-filter-list').height();
            var flBoxOffset = $('div.main-filter-list').offset().top;
            var btmFloatHt = $('div.usedsearch-floating-strip').height();
            var winHeight = $(window).height();
            var footerOffset = $('footer').offset().top
            var winscroll = $(window).scrollTop();
            var posy = mainListHt - flBoxOffset - 135;
            var filterHeight = $('#filters').height();
            var listingHeight = $('#cw-loading-box').height();
            if (winscroll >= posy && winscroll + winHeight <= footerOffset) {
                $('div.main-filter-list').css({ 'position': 'fixed', 'overflow': 'visible', 'bottom': btmFloatHt + 'px' });
            }
            if (listingHeight < filterHeight) {
                $('#filters').css({ 'height': 'auto' });
                //$('html,body').animate({scrollTop:0});
            }
            // to select short listed cars after filter is applied
            if (typeof classifiedShortlistCars != 'undefined')
                classifiedShortlistCars.shortlistCarsOnFilterSelection();
        }
    });
};

$.lazyLoadImages = function (listingId) {
    $('.stock-list ul#' + listingId + ' img.lazy').lazyload({
        load: function () {
            var x = $(this)[0];
            var currentListing = $(this).parents('li');
            var listingsCount = $.jsonData ? $.jsonData.stockcount.TotalStockCount : $.totalCount;
            searchTracking.triggerTracking(currentListing[0], listingsCount, 'SearchListing', true, D_usedSearch.utils.getQS()); // true => if you want to track query string patameters
        }
    });
};

$.callMethodsForLoad = function () {
    $.selectFiltersPresentInQS();
    $.setCarOnLoad();
    $.appendSelectedFilters();
    $.setBudgetOnLoad();
    $.setYearSliderOnLoad();
    $.setKmSliderOnLoad();
    $.setSortOnPageLoad();
    var cityId = $.getFilterFromQS('city');
    if (cityId.length > 0) {
        $('#drpCity').val(cityId);
    }
    $('#nearByCityList,#listingsData').show();

};

$.checkSuperLuxuryCondition = function () {
    var count = 0;
    $('#manu-box ul.ul-makes ul.rootUl').find('li[issl="true"]').each(function () {
        if ($(this).hasClass('active'))
            count++;
    });
    var budget = $("#budgetBtn").html()
    if (budget != "" && budget != "Choose budget") {
        var min = $("#budgetBtn").html().split('-')[0];
        var max = $("#budgetBtn").html().split('-')[1];
        var minBudget = parseFloat(min.split('L')[0].trim()) * 100000;
        var maxBudget = parseFloat(max.split('L')[0].trim()) * 100000;
    }
    var city = $.getNameCountForCity($('#nearByCityList li[cityid="9999"]').text());
    if (count > 0 || maxBudget >= 5000000 || $('#drpCity option:selected').val() == 9999) {
        if ($('#nearByCityList li').length == 1) {
            var text = $.getNameCountForCity($('#drpCity option:selected').text());
            var liHtml = $.getCityLi(text[0], text[1], $('#drpCity option:selected').val());
            $('#nearByCityList li:last').before(liHtml);
            $('#nearByCityList li').removeClass('active');
            $('#nearByCityList li:first').addClass('active');
        }
        if (city[1] > 0) {
            $('#nearByCityList li[cityid="9999"]').show();
            isAllIndia = true;
        }
    }
    else {
        if ($('#maxInput').val().length == 0 && $('#minInput').val().length != 0 && city[1] > 0) {
            isAllIndia = true;
            $('#nearByCityList li[cityid="9999"]').show();
        }
        else
            $('#nearByCityList li[cityid="9999"]').hide();
    }

    if (isAllIndia)
        $('#nearByCityList li[cityid="9999"]').show();
    if ($('#nearByCityList li').length == 2 && $('#nearByCityList li:first').attr('cityId') == $('#drpCity option:selected').val()) {
        if (!$('#nearByCityList li[cityId="9999"]').is(':visible'))
            $('#nearByCityList li:first').hide();
        if ($('#nearByCityList li[cityId="9999"]').is(':visible') && !$('#nearByCityList li[cityId="' + $('#drpCity option:selected').val() + '"]').is(':visible'))
            $('#nearByCityList li[cityId="' + $('#drpCity option:selected').val() + '"]').show();
    }

    if ($('#nearByCityList li[cityId="9999"]').is(':visible') && !$('#nearByCityList li[cityId="' + $('#drpCity option:selected').val() + '"]').is(':visible'))
        $('#nearByCityList li[cityId="' + $('#drpCity option:selected').val() + '"]').show();
};

$.AllIndiaStateOnLoad = function () {
    if (isAllIndia)
        $('#nearByCityList li[cityId="9999"]').show();
    else
        $('#nearByCityList li[cityId="9999"]').hide();
};

$.bindAllIndiaTab = function () {
    var nearBy = $('#nearByCityList');
    if ($('#drpCity option:selected').val() > 0) {
        var text;
        if (nearBy.find('li').length == 0) {
            text = $.getNameCountForCity($('#drpCity option:selected').text());
            nearBy.append($.getCityLi(text[0], text[1], $('#drpCity option:selected').val()));
        }
        text = $.getNameCountForCity($('#drpCity option[value="9999"]').text());
        nearBy.append($.getCityLi(text[0], text[1], 9999));

    }
};

$.bindNoRecordshowMoreCars = function (count) {
    if (parseInt(count) == 0)
        $.setCityCountZero();
    remainingCount = parseInt(count) - D_usedSearch.latestNonPremiumCarRank;
    if (remainingCount > 0) {
        $.fetchMoreResults = true;
        $('#noRecordsFound').hide();

    }
    else if (remainingCount < 1 && $('#drpCity option:selected').val() > 0) {
        $.fetchMoreResults = false;

        $.changeResponseText();
        $('#noRecordsFound').show();
    }

    if ($.pageNo > 9 && $('#drpCity option:selected').val() > 0) {
        $.fetchMoreResults = false;

        $.changeResponseText();
        $('#noRecordsFound').show();
    }
    if (remainingCount < 1) {
        if ($('#drpCity option:selected').val() <= 0) {
            $('#noRecordsFound').show();
            $('#alert_content').hide();
        }
        else
            $('#alert_content').show();
        $.fetchMoreResults = false;
        $.changeResponseText();
    }
};

//Added By Sachin Bharti(8th Aug 2015)
$.changeResponseText = function () {
    $('#res_msg').find("p:first").text('To find more cars...');
};

$.setCityCountZero = function () {
    if ($('#drpCity option:selected').val() > 0) {
        var selectedCity = $('#drpCity option:selected');
        var name = $.getNameCountForCity($.trim(selectedCity.text()))[0];
        selectedCity.text(name + " (0)");
    }
};

/*------------------------------------------------------------------------------------------------------------------------------------------------------------------------*/
/*-------------------------------------------------------------After Page Load Filter selection functionality ends -------------------------------------------------------*/
/*------------------------------------------------------------------------------------------------------------------------------------------------------------------------*/

/*------------------------------------------------------------------------------------------------------------------------------------------------------------------------*/
/*----------------------------------------------------------------------- Bind Data after API Hit starts -----------------------------------------------------------------*/
/*------------------------------------------------------------------------------------------------------------------------------------------------------------------------*/
$.bindCounts = function (json, city) {
    for (var parentFilter in json) {
        var parentFilterName = JSON.stringify(parentFilter);
        parentFilterName = parentFilterName.replace(/\"/g, '');
        if (parentFilterName !== 'city' && parentFilterName !== 'make' && parentFilterName !== 'root' && parentFilterName !== 'model') {
            $.bindCheckBoxCounts(parentFilterName, parentFilter, json);
        }
        else if (parentFilterName === 'city') {
            var cityId = city || $.getFilterFromQS('city');
            usedFilterCities.bindCityCounts(json[parentFilterName], cityId);
        }
    }
};

/*----------------------------------------------------------------------- Near By City Functionality starts -----------------------------------------------------------------*/

$.getNearByCityArr = function (json) {
    if (json != undefined) {
        var city, cityObj;
        var text = '', cityName = '', cityCount = '', drpCity = $('#drpCity');
        var arr = [];
        for (var i = 0 ; i < json.length ; i++) {
            text = $.trim(drpCity.find('option[value="' + json[i].CityId + '"]:first').text());
            var city = $.getNameCountForCity(text);
            cityObj = {
                id: json[i].CityId,
                name: city[0],
                count: json[i].CityCount
            };
            if (cityObj.count > 0)
                arr.push(cityObj);
        }
        arr.sort(sortByCount);
        $.nearByCity = arr;
    }
};

$.getNameCountForCity = function (text) {
    var city = text.split('(');
    var cityName = $.trim(city[0]);
    var cityCount = 0;
    city[1] = $.trim(city[1]);
    if (city[1] != undefined && city[1].length > 0)
        cityCount = parseInt($.trim(city[1].split(')')[0]));
    return [cityName, cityCount];
};

$.bindNearByCity = function () {
    var arr = $.nearByCity;
    var drpCity = $('#drpCity');
    var selectedCityId = drpCity.find('option:selected').val();
    var nearUl = $('#nearByCityList').empty();
    var text = '';
    var city;
    //Mumbai City Case
    if (selectedCityId == "3000") {
        text = drpCity.find("option[value='3000']:first").text();
        city = $.getNameCountForCity(text);
        if (city[1] > 0)
            nearUl.append($.getCityLi("Mumbai (All)", city[1], "3000"));//mumbai all
        text = drpCity.find("option[value='1']:first").text();
        city = $.getNameCountForCity(text);
        if (city[1] > 0)
            nearUl.append($.getCityLi("Mumbai City", city[1], "1"));//mumbai city
        for (var i = 0; i < arr.length && i < 10; i++) {
            nearUl.append($.getCityLi(arr[i].name, arr[i].count, arr[i].id));
        }

    }//new delhi City Case
    else if (selectedCityId == "3001") {
        text = drpCity.find("option[value='3001']:first").text();
        city = $.getNameCountForCity(text);
        if (city[1] > 0)
            nearUl.append($.getCityLi(city[0], city[1], "3001"));
        text = drpCity.find("option[value='10']:first").text();
        city = $.getNameCountForCity(text);
        if (city[1] > 0)
            nearUl.append($.getCityLi(city[0], city[1], "10"));
        for (var i = 0; i < arr.length && i < 10; i++) {
            nearUl.append($.getCityLi(arr[i].name, arr[i].count, arr[i].id));
        }
    }
    else {
        if (arr.length > 0) {
            text = drpCity.find("option:selected").text();
            value = drpCity.find("option:selected").val();
            city = $.getNameCountForCity(text);
            nearUl.append($.getCityLi(city[0], city[1], value));
            for (var i = 0; i < arr.length && i < 10; i++) {
                nearUl.append($.getCityLi(arr[i].name, arr[i].count, arr[i].id));
            }
        }
    }
    $.bindAllIndiaTab();
    var nbCityCount = $("#nearByCityList li").length;
    for (var i = 5; i < nbCityCount && nbCityCount > 4; i++) {
        $("#nearByCityList li:nth-child(" + i + ")").hide();
    }
    nearUl.find('li:first').addClass('active');
    $('#nearByCityList li').selectNearByCity();
    $("#nearByCityList .active").addClass("selected");


};

$.getCityLi = function (name, count, id) {
    return "<li cityId='" + id + "'>" + name + " ( <span class='city-count'>" + count + "</span> )" + "</li>";
};

function sortByCount(a, b) {
    var aCount = a.count;
    var bCount = b.count;
    return ((aCount > bCount) ? -1 : ((aCount < bCount) ? 1 : 0));
}

$.fn.selectNearByCity = function () {
    return this.click(function () {
       
        $("#noCarsFound").hide();
        $.removeKnockouts();
        var completeQS = $.removeFilterFromQS('city');
        if ($.getFilterFromQS('pn').length > 0) {
            completeQS = $.removeFilterFromQS('pn');
            $.pageNo = 1;
            window.location.hash = completeQS;
        }
        var selectedCity = $('#drpCity option:selected').val();
        $('#drpCity').val($(this).attr('cityId'));
        completeQS = $.removeFilterFromQS('city');
        completeQS = $.addParameterToString('city', $(this).attr('cityId'), completeQS);
        $.pageNo = 1;
        $.pushState(completeQS);
        $(this).parent().find('li').removeClass('active');
        $(this).addClass('active');
        $("#nearByCityList li").removeClass("selected");
        $("#nearByCityList li.active").addClass("selected");
        D_usedSearch.cityWarning.isUserCityChanged = true;
        D_usedSearch.cityWarning.showCityWarningOnSoldOut = false;
        D_usedSearch.cityWarning.resetWarning();
        $.resetSetAlert();
    });
};

$.refreshNearByCityCount = function () {
    var ul = $('#nearByCityList');
    if (ul.find('li').length > 0) {
        var drpCity = $('#drpCity');
        ul.find('li').each(function () {
            var text = drpCity.find('option[value="' + $(this).attr('cityId') + '"]').text();
            var city = $.getNameCountForCity(text);
            $(this).find('span.city-count').text(city[1]);
        });
        //Tab line animation
        if ($(document).find(".cw-tabs.cw-tabs-flex").length > 0)
            tabAnimation.tabHrInit();
    }
    if (!(ul.find('li').length > 0) || !(ul.find('li').is(':visible'))) {
        if (ul.parent().find('hr').length > 0)
            ul.parent().find('hr').remove();
    }
};

$.hideNearByCityWithZero = function () {
    var text, city, cityObj;
    $('#nearByCityList li').each(function () {
        text = $.trim($(this).text());
        city = $.getNameCountForCity(text);
        cityObj = {
            name: city[0],
            count: city[1]
        };
        if (cityObj.count == 0)
            $(this).hide();
    });
    $('#nearByCityList li:first').show();
};

/*----------------------------------------------------------------------- Near By City Functionality ends -----------------------------------------------------------------*/

$.bindTotalRecordCount = function (count) {
    $('#totalCount').text($.addCommas(count.toString().replace(/\./g, '')));
};

$.bindCheckBoxCounts = function (parentFilterName, parentFilter, json) {
    var filter = $('.filters div[name="' + parentFilterName + '"]');
    $('#' + filter.attr('id') + ' li').addClass('filter-disable').addClass('opacity50').find('.count-box').text('0');
    for (childFilters in json[parentFilter]) {
        if (json[parentFilter][childFilters] > 0)
            $(filter).find('li[name="' + childFilters + '"]').show().removeClass('filter-disable').removeClass('opacity50').find('.count-box').text(json[parentFilter][childFilters]);
    }
    $.reOrderFilters(parentFilterName);
};

var usedFilterCities = (function () {
    var _excludeFromNormalCityListIds = [3000, 3001, 9999];  //int because value coming from server is in int
    var _$drpCity = document.getElementById("drpCity");
    var _citiesSeperatorHtml = '<option value="-2" disabled>------------</option>';
    var _getPopularCitiesObj = function () {
        return {
            "3001": { cityId: 3001, isAvailable: false, cityName: "Delhi NCR", rank: 1 },
            "3000": { cityId: 3000, isAvailable: false, cityName: "Mumbai", rank: 2 },
            "40": { cityId: 40, isAvailable: false, cityName: "Thane", rank: 3 },
            "10": { cityId: 10, isAvailable: false, cityName: "New Delhi", rank: 4 },
            "2": { cityId: 2, isAvailable: false, cityName: "Bangalore", rank: 5 },
            "176": { cityId: 176, isAvailable: false, cityName: "Chennai", rank: 6 },
            "198": { cityId: 198, isAvailable: false, cityName: "Kolkata", rank: 7 },
            "128": { cityId: 128, isAvailable: false, cityName: "Ahmedabad", rank: 8 },
            "12": { cityId: 12, isAvailable: false, cityName: "Pune", rank: 9 },
            "105": { cityId: 105, isAvailable: false, cityName: "Hyderabad", rank: 10 }
        };
    };
    var _getHtmlForPopularCites = function (popularCitiesObj) {
        var html = '<option value="0">--Select City--</option>';  //first Option is All India in city drop down
        html += _citiesSeperatorHtml;                 //grouping popular cities in drop down
        var keysOfPopularCitiesObj = Object.keys(popularCitiesObj).sort(function (item1, item2) {
            return popularCitiesObj[item1].rank - popularCitiesObj[item2].rank;
        });
        keysOfPopularCitiesObj.forEach(function (key) {
            var cityObj = popularCitiesObj[key];
            if (cityObj.isAvailable) {
                html += '<option value="' + cityObj.cityId + '">' + cityObj.cityName + ' (' + cityObj.cityCount + ')</option>';
            }
        });
        html += _citiesSeperatorHtml;
        return html;
    };
    var bindCityCounts = function (cityJson, currCity) {
        var html = "";
        var popularCitiesObj = _getPopularCitiesObj();
        cityJson.forEach(function (cityObj) {
            if (cityObj.CityId === 1) {                 //for cityid=1 changing cityname from Mumbai to Mumbai City as Mumbai and Mumbai City is different for used cars 
                cityObj.CityName = "Mumbai City";
            }
            if (_excludeFromNormalCityListIds.indexOf(cityObj.CityId) < 0) {
                html += '<option value="' + cityObj.CityId + '">' + cityObj.CityName + ' (' + cityObj.CityCount + ')</option>';
            }
            if (popularCitiesObj[cityObj.CityId]) {
                popularCitiesObj[cityObj.CityId].isAvailable = true;
                popularCitiesObj[cityObj.CityId].cityCount = cityObj.CityCount;
            }
        });
        html = _getHtmlForPopularCites(popularCitiesObj) + html;
        _$drpCity.innerHTML = html;

        if (currCity) {
            var $selectedOption = _$drpCity.querySelector('option[value="' + currCity + '"]');
            if ($selectedOption) {
                $selectedOption.setAttribute("selected", "selected");
            }
        }
    };

    return {
        bindCityCounts: bindCityCounts,
    };
})();

$.bindMakeRootCounts = function (json) {
    $('#manu-box ul.ul-makes li').hide();
    $('#manu-box ul.ul-makes li div.list-points-models').each(function () {
        $(this).remove();
    });
    var makeList = $('#manu-box ul.ul-makes');
    makeList.find('li').find('span.count-box').text('0');
    var liHtml = '';
    var rootHtml = '';
    for (makeFilter in json) {
        var makeNode = makeList.find('li[carFilterId="' + json[makeFilter]["MakeId"] + '"]');
        makeNode.find('span.count-box').text(json[makeFilter]["MakeCount"]);
        rootHtml = '';
        for (childFilter in json[makeFilter]["RootList"]) {
            rootHtml += '<li class="us-sprite rootLi" carFilterId="' + json[makeFilter]["MakeId"] + "." + json[makeFilter]["RootList"][childFilter]["RootId"] + '" issl="' + json[makeFilter]["RootList"][childFilter]["isSuperLuxury"] + '">' +
            '<span class="model-txt">' + json[makeFilter]["RootList"][childFilter]["RootName"] + '</span>' +
            '<span class="text-grey-count">&nbsp; (<span class="count-box">' + json[makeFilter]["RootList"][childFilter]["RootCount"] + '</span>)</span></li>';
        }
        if (rootHtml != '') {
            makeNode.find('span.count-box').find('div.list-points-models').remove();
            makeNode.find('span.count-box').parent().after("<div class='list-points-models'><ul class='rootUl'>" + rootHtml + "</ul></div>");
            makeNode.find('div.list-points-models li').each(function () {
                var count = parseInt($(this).find('span.count-box').text());
                if (count == 0)
                    $(this).hide();
            });
        }
        if (parseInt(makeNode.find('span.count-box:first').text()) > 0)
            makeNode.show();
    }
    $('li.makeLi').unbind('click');
    $('li.makeLi').makeClick();
    $('li.rootLi').unbind('click');
    $('li.rootLi').rootClick();
    $.showSelectedMakes();
};

$.showSelectedMakes = function () {
    var values = $.getFilterFromQS("car").replace(/ /g, '+').split('+');
    var makeList = $('#manu-box ul.ul-makes');
    var makeId = '';
    for (var i = 0; i < values.length ; i++) {
        if (values[i].indexOf('.') > 0) {
            makeId = values[i].split('.')[0];
            makeList.find('li[carfilterid="' + makeId + '"]').find('li[carfilterid="' + values[i] + '"]').show();
        }
        else
            makeId = values[i];
        makeList.find('li[carfilterid="' + makeId + '"]').show();
    }
};

$.setCarOnLoad = function () {
    var node = $('#manu-box ul.ul-makes');
    var values = $.getFilterFromQS("car").replace(/ /g, '+').split('+');
    for (var j = 0; j < values.length; j++) {
        if (values[j].indexOf('.')) {
            var makeId = values[j].split('.')[0];
            if (!node.find('li[carfilterid="' + makeId + '"]').hasClass('active'))
                node.find('li[carfilterid="' + makeId + '"]').addClass('active');
        }
        node.find('li[carfilterid="' + values[j] + '"]').addClass('active');
    }
};

$.fn.makeClick = function () {
    return this.click(function () {
        $("#noCarsFound").hide("slow");
        var node = $(this);
        var eventAction = node.find('.filterText').text();
        if (node.hasClass('active')) {
            var curValue = '';
            var completeQS = '';
            var filter = $('#car');
            node.find('li.rootLi.active').each(function () {
                $(this).removeClass('active');
                filter.find('span[carfilterid="' + $(this).attr('carfilterid') + '"]').remove();
                curValue = $.getFilterFromQS("car").replace(/ /g, '+');
                completeQS = $.removeValueFromCheckBoxInQS("car", $(this).attr('carfilterid'), curValue);
                window.location.hash = completeQS;
            });
            filter.find('span[carfilterid="' + $(this).attr('carfilterid') + '"]').remove();
        }
        $.applyCheckBoxFilter("car", node.attr('carfilterid'), node);
        dataLayer.push({ event: 'Make_Model_City', cat: 'UsedCarSearch_Retargeting', act: eventAction });
    });
};

$.fn.rootClick = function () {
    return this.click(function (e) {
        e.stopPropagation();
        var node = $(this);
        var eventAction = node.find('.model-txt').text();
        var carId = node.attr('carfilterid');
        var makeId = carId.split('.')[0];
        var curValue = $.getFilterFromQS("car").replace(/ /g, '+');
        var completeQS = $.removeValueFromCheckBoxInQS("car", makeId, curValue);

        var rootCount = node.parent().find('li.rootLi.active').length;
        if (rootCount == 1 && node.hasClass('active'))
            completeQS = $.addValueToCheckBoxFilterInQS("car", makeId, curValue)
        window.location.hash = completeQS;
        $.applyCheckBoxFilter("car", node.attr('carfilterid'), node);
        dataLayer.push({ event: 'Make_Model_City', cat: 'UsedCarSearch_Retargeting', act: eventAction });
    });
};

$.updateCityCountInDropDown = function (drpCity, id, value) {
    var option = drpCity.find('option[value="' + id + '"]');
    option.each(function () {
        var array = $(this).text().split('(');
        if (array[0] != '' && array[0] != undefined) {
            $(this).text(array[0] + ' (' + value + ')').show().prop("disabled", false);
        }
    });
};

$.reOrderFilters = function (parentFilterName) {
    var filter = $('.filters div[name="' + parentFilterName + '"]');
    var visibleFilter = filter.find('li:not(.filter-disable)');
    var hiddenFilter = filter.find('li.filter-disable');

    filter.find('ul').empty().append(visibleFilter).append(hiddenFilter);
    filter.find('li').onCheckBoxClick();
};

$.bindListings = function (json) {
    $('#listingsData').remove();
    var element = document.getElementById('listing' + loadPageNo);
    ko.cleanNode(element);
    ko.applyBindings(new listingsViewModel(json), element);
    listingNo = loadPageNo;
};

//Change Heading as per the city dropdown
$.changeHeadingText = function () {
    var title = 'Search Used Cars';
    if ($('#drpCity option:selected').val() > 0) {
        var cityName = $('#drpCity option:selected').text().split('(');
        title = 'Used Cars in ' + cityName[0];
    }
    $('h1').html(title + "<span class='font16 text-light-grey text-unbold special-skin-text'>&nbsp;<span id='totalCount'></span>&nbsp;<span>Cars</span></span>");

};
/*------------------------------------------------------------------------------------------------------------------------------------------------------------------------*/
/*----------------------------------------------------------------------- Bind Data after API Hit ends -----------------------------------------------------------------*/
/*------------------------------------------------------------------------------------------------------------------------------------------------------------------------*/
$.changeMumbaiName = function () {
    var text = $('#drpCiy option[value="1"]').text();
    var city = $.getNameCountForCity(text);
    $('#drpCity option[value="1"]').text("Mumbai City (" + city[1] + ")");
};

function setFocusOnTxt() {
    $('input[type="text"]').on("focus", function () {
        $('.uc-error-circle,.pguc-error-circle,.cw-blackbg-tooltip,.pgcw-blackbg-tooltip,.error-icon').hide();
    });
}

$.cwLtrscCookie = function () {
    //CW Track Code
    var url = unescape(window.location);
    var landingURL = url;
    var imgCreation = new Image();
    var hashIndex = url.indexOf("#");

    url = url.substr(url.indexOf("?") + 1, hashIndex == -1 ? url.length : url.indexOf("#") - (url.indexOf("?") + 1));
    landingURL = landingURL.substr(0, landingURL.indexOf("?"));

    var searchAttributes = url.split('&');

    for (var no = 0; no < searchAttributes.length; no++) {
        var cutSrc = searchAttributes[no].substr(searchAttributes[no].indexOf("ltsrc"), searchAttributes[no].indexOf("="))
        if (cutSrc == 'ltsrc') {
            var qryString = searchAttributes[no].substr(searchAttributes[no].indexOf("ltsrc") + 6, searchAttributes[no].length)
            imgCreation.src = "/lts/ts.aspx?c=" + qryString + "&refUrl=" + landingURL;
        }
    }
    ltsrc = $.getCookie("CWLTS").split(':')[0];
    //End of CW Track code	
}

// function to changed the text of Get Seller Details Button to View Seller Details once the user verifies his credentials
$.sellerDetailsBtnTextChange = function () {
    if (ISSELLERDETAILVIEWED || $.cookie('TempCurrentUser') != null) {
        $("span.gsdTxt").hide();
        $("span.oneClickDetails").show();
        $('p.get-seller-tel').addClass('hide');
        $('span.preVerification').removeClass('preVerification');
        $('span.or-line').removeClass('hide');
        $('span.view-seller-tel').removeClass('hide');
        $('p.seller-note').addClass('hide');
    }
}

$.getRank = function (isPremium) {		
    if (isPremium() == true) {		
            return $.indexFeatured++;		
        }		
    else {		
        return $.indexNonFeatured++;		
    }		
 }		
 		
$.getRankAbsolute = function () {		
    return $.indexAbsolute++;		
}

$.removeUnwantedQS = function () {
    var i, parms, length, value, url = "";
    params = window.location.hash.replace('#', '').split('&');
    length = params.length;
    if (length > 0) {
        for (i = 0; i < length; i++) {
            if (params[i].indexOf('=') >= 0) {
                if (i != length - 1)
                    url += params[i] + '&';
                else
                    url += params[i];
            }
        }
    }
    window.location.hash = url;
    return url;
}

function getNextListing(nodeLi) {
    var nextListCount = 0;
    // var nextListing = nodeLi.nextAll('li.listingContent').find('a.slideShow img.lazy').eq(nextListCount);
    while (nodeLi.length && nodeLi.attr('data-original') == "https://imgd.aeplcdn.com/0x0/cw/design15/nocars-placeholder.jpg") {
        nodeLi = nodeLi.closest('li.listingContent').nextAll('li.listingContent').eq(nextListCount).find('a.slideShow img.lazy');
        nextListCount++;
    }
    return nodeLi;
}

function getNextStockList(StockList) {

    var nextListing = 1; var nextStockListCount = 0;
    var nextStockList = StockList.nextAll('div.stock-list').eq(nextStockListCount);
    nextListing = getNextListing(StockList.find('li.listingContent').find('a.slideShow img.lazy').first());

    while (!nextListing.length && nextStockList.length) {
        nextStockList = StockList.nextAll('div.stock-list').eq(nextStockListCount);
        nextListing = getNextListing(nextStockList.find('li.listingContent').find('a.slideShow img.lazy').first());
        nextStockListCount++;
    }
    return nextListing;

}


$(document).ready(function () {
    ko.applyBindings({ viewSellerBtnVisible: D_buyerProcess.recommendedCars.viewSellerBtnVisible }, document.getElementById("txtRecommendationHeading"));

    if (!navigator.userAgent.match(/Version\/[\d\.]+.*Safari/) && JSON.parse($.cookie('UCCWWL')) && JSON.parse($.cookie('UCCWWL'))[0] && !JSON.parse($.cookie('UCCWWL'))[0].fuel)
    {
        var date = new Date();
        var myCookieValue = $.cookie('myCookie');
        $.cookie('UCCWWL', myCookieValue, { expires: date, secure: true, path: '/' });
    }
    if ($.cookie('usedSearchViewType')) {
        document.cookie = "usedSearchViewType=" + "; expires=Thu, 01 Jan 1970 00:00:00 UTC;domain=.carwale.com;path=/used"
    }

    

    $(document).on('click', '#pgNextBtn,#pgPrevBtn', function () {
        $('#pg-captcha').hide();
        $('#pg-not_auth').hide();
        var temp = $(this);
        var nodePG;
        if (temp.attr('id') == 'pgNextBtn') {

            nodePG = $("li.listingContent[rankabs='" + $('#pgNextBtn').attr('rankabs') + "']").find('a.slideShow').first();

        }
        else
            nodePG = $("li.listingContent[rankabs='" + $('#pgPrevBtn').attr('rankabs') + "']").find('a.slideShow').first();

        nodePG.trigger('click', [nodePG]);
    });
    $(document).on('click', '#pgNextCityCars', function () {

        var firstImageListingOfNbCity = $("li.listingContent[rankabs='" + $('#pgNextCityCars').attr('rankabs') + "']").find('a.slideShow').first()
        $('#pgNextCityCars').animate({ right: '-167', opacity: '0' });
        $('.nextBtn').unbind('mouseenter');
        $('#pgNextCityCars').unbind('mouseleave');
        firstImageListingOfNbCity.trigger('click', [firstImageListingOfNbCity]);
        if ($('#pg-captcha').is(':visible'))
            $('#pg-captcha').hide();
        if ($('#pg-not_auth').is(':visible'))
            $('#pg-not_auth').hide();
    });
    d_usedSearchPage.PageLoad();
    if (typeof Location != 'undefined' && !$.getFilterFromQS('city') && !$.getFilterFromQS('pc') && ($.appliedIpDetectedCityId != -1 || !$.city)) {
        if(commonUtilities.getFilterFromQS('utm_source') === 'facebook') { // if the user came from facebook, hide the location popup's
            $('body').addClass('no-action-bg');        // close button and make the blockoutwindow non-clickable
            $('#closeLocIcon').hide();
        }
        Location.globalSearch.showGlobalCityPopup();
    }
    $.coachmarkcookie();
    setFocusOnTxt();
    if ($.totalCount == 0)
        $.setCityCountZero();
    $.changeMumbaiName();
    $('#askexpertsidebanner').remove();
    $('#btnSetBudget').setBudget();
    $('.close-icon-md').closeCityPopup();
    $.showCityPopup();
    $('#popularCities li').popularCityClicks();
    $('.blackOut-window-bt').closePhotoGallery();

    /*------------------------------------------------------------------------------------------------------------------------------------------------------------------------*/
    /*-------------------------------------------------------------After Page Load Filter selection functionality starts -----------------------------------------------------*/
    /*------------------------------------------------------------------------------------------------------------------------------------------------------------------------*/
    //$('img.lazy').lazyload({ load: alert(1) });
    $.lazyLoadImages("listingsData");
    $.bindAgeSlider();
    $.bindKMSlider();
    dataLayer.push({ event: 'Page#' + $.pageNo, cat: 'UsedCarSearch_Pageviews', act: 'Page#' + $.pageNo });
    $('#drpCity').change(function () {
        var node = $(this);
        isCityChange = true;
        isAllIndia = false;
        $.removeKnockouts();
        if (node.val() > 0) {
            if (node.val() == 1)
                node.val(3000);
            pcity = node.val();
            $.updateFilters(node, 'city', node.val(), 1);
        }
        else {
            $.pageNo = 1;
            pcity = node.val();
            var completeQS = $.removeFilterFromQS('city');
            window.location.hash = completeQS;
            completeQS = $.removeFilterFromQS('pc');
            $.removePageNoParam();
            $.pushState(completeQS);
        }
        $(this).blur();//Added by Sachin Bharti(10th of Aug)
        $.resetSetAlert();
        D_usedSearch.cityWarning.isUserCityChanged = true;
        D_usedSearch.cityWarning.showCityWarningOnSoldOut = false;
        D_usedSearch.cityWarning.resetWarningOnCityChange(node.val());
        dataLayer.push({ event: 'Make_Model_City', cat: 'UsedCarSearch_Retargeting', act: $('#drpCity [value=' + node.val() + ']').text().split(' ')[0] });
    });

    $("#btnSoldOutClose").click(function () {
        $(".soldout-box").hide("slow");
    });
    $("#btnNoCarsClose").click(function () {
        $("#noCarsFound").hide("slow");
    });
    $('#tags').on('keyup', function (e) {
        // cache all the tag elements in an array
        var tagElems = $('#makesList').children();
        // hide all tags
        $(tagElems).hide();

        for (var i = 0; i < tagElems.length; i++) { // loop through all tagElements
            var tag = $(tagElems).eq(i);

            if (($(tag).text().toLowerCase().trim()).indexOf($(this).val().toLowerCase()) === 0) {
                // if element's text value starts with the hint show that tag element
                if (parseInt($(tag).find('.count-box').text()) > 0 || $(tag).hasClass('active'))
                    $(tag).show();
            }
        }
    });
    initGetSellerDetails();
    $(document).mouseup(function (e) {
        var fbForm = $(".feedback-form");
        var fbBtn = $(".feedbackBtn");
        if (!fbForm.is(e.target)
            && fbForm.has(e.target).length === 0) {
            fbForm.slideUp();
            fbBtn.show();
        }
    });

    var hash = $.removeFilterFromQS('pn');
    hash = D_usedSearch.utils.removeCarRanksFromQS(hash)
    window.location.hash = hash;
    hash = window.location.hash.replace('#', '');

    if (hash.length > 0) {
        hash = $.removeUnwantedQS();
        pcity = $.getFilterFromQS('pc');
        if (pcity.length == 0)
            pcity = $.getFilterFromQS('city');
        if (pcity.length == 0) {
            pcity = $.city;
        }
        if (pcity.length > 0) {
            hash = $.removeFilterFromQS('city');
            hash = $.addParameterToString('city', pcity, hash);
            window.location.hash = hash;
            hash = $.removeFilterFromQS('pc');
            hash = $.addParameterToString('pc', pcity, hash);
        }
        var car = $.getFilterFromQS('car');
        if (car.length == 0) {
            car = $.car;
            hash = $.addParameterToString('car', car, hash);
        }
        hash = $.addParameterToString('pn', "1", hash);
        window.location.hash = hash;
        $.pageNo = 1;
        D_usedSearch.isFromPageLoad = true;
        if ($('#drpCity option:selected').attr('value') == $.getFilterFromQS("city"))
            PAGELOAD = true;
        $.pushState(hash);
    }
    else {
        $.setQSParametersInURL();
        $.bindCounts($.jsonData, $.city);
        $.bindMakeRootCounts($.jsonData.Makes);
        $.getNearByCityArr($.nearByCity);
        if ($('#drpCity option:selected').val() > 0)
            $.bindNearByCity();
        isCityChange = false;
        $.checkSuperLuxuryCondition();
        $.selectFiltersPresentInQS();
        $.setCarOnLoad();
        $.appendSelectedFilters();
        $.setBudgetOnLoad();
        $.setYearSliderOnLoad();
        $.setKmSliderOnLoad();
        $.bindNoRecordshowMoreCars($.totalCount);
        $('#nearByCityList,#listingsData').show();
        $.showSoldOutSlug();
        var nbcFlag = checkNearByCityCount();
        if ($(".stock-list li").length == 0 && !nbcFlag) {
            var cityName = $("#nearByCityList li.active").text().split('(')[0];
            $("#noCarsFoundCityName").text('');
            $("#noCarsFoundCityName").append(cityName);
            $("#noCarsFound").show();
            D_usedSearch.cityWarning.resetWarning();
            if (noCarTimer != null)
                clearTimeout(noCarTimer);
            noCarTimer = setTimeout(function () { $("#noCarsFound").hide("slow"); }, 10000);
        }
        if ($.jsonData)
            $.bindTotalRecordCount($.jsonData.stockcount.TotalStockCount);
        if ($.soldOut != "true" && D_usedSearch.cityWarning.isListingsPresent()) {
            D_usedSearch.cityWarning.checkWarningConditions();
            D_usedSearch.cityWarning.toggleCityWarning();
        }
    }
    $.bindPhotoGallery();
    $.photoGalleryIconClick();
    $.bindFeedBack();
    $.insertInSearchProperties();
    $.resetSetAlert = function () {
        $('#alertEmail').val('');
        $('#alert_content').show();
        $('#alert_status').hide();
    };
    $.cwLtrscCookie();

    $('#back-top a').goToTop();


    $.sellerDetailsBtnTextChange();
    
    qsTemp = window.location.hash.replace('#', '');

    if ($.getFilterFromQS("city") != 3000 && $.getFilterFromQS("city") != 3001 && $.jsonData.stockcount.TotalStockCount < 15 && hash.length==0) {
        var nbCityInfo = d_usp_nbcityapiHit.getNextNbCityWithCount();
        if (nbCityInfo.nbCitiesId.length)
            getNbCityData(nbCityInfo);
    }
    $("div.getNextPage").show();
  

    D_usedSearch.similarCars.showSimilarCarLink($.jsonData.stockcount.TotalStockCount, window.location.hash.replace('#', ''));

    if ($.cookie('TempCurrentUser') && D_buyerProcess.openVerificationPopup.isPopupEligible())
        D_buyerProcess.openVerificationPopup.slidePopup();

    $(document).on('click', '.common-certification-program-slug', function (e) {
        Common.utils.callTracking($(this));
        e.stopPropagation();
    });

    if (typeof cwUsedTracking !== 'undefined') {
        cwUsedTracking.setEventCategory(cwUsedTracking.eventCategory.UsedSearchPage);
    }
});

$.globalCityChange = function (globalCityId, globalCityName) {
    $.pageNo = 1;
    var cityFilter = $("#drpCity");
    $("div.getNextPage").hide();
    if (cityFilter.find("option[value=" + globalCityId + "]").length > 0)
        cityFilter.val(globalCityId).change();
    else {
        cityFilter.append('<option value="' + globalCityId + '" style="display: block;">' + globalCityName + ' (0)</option>');
        cityFilter.val(globalCityId).change();
    }
}


var filterTypeEnum = { ListingType: 1, Fuel: 3, Body: 4, SellerType: 5, Owner: 6, Color: 8 };


$.getSelectedCarMakeStr = function () {
    var ret = "";
    $.each($('#makesList').find('li.makeLi.active span.filterText'), function (index, value) {
        ret = $.trim(ret + $(value).text()) + ",";
    });
    return ret.substring(0, ret.length - 1);
}

$.getSelectedCarRootStr = function () {
    var ret = "";
    $.each($('#makesList').find('li.makeLi.active').find('li.rootLi.active span.model-txt'), function (index, value) {
        ret = $.trim(ret + $(value).text()) + ",";
    });
    return ret.substring(0, ret.length - 1);
}

$.getSelectedFilterStr = function (type) {
    var ret = "";
    $.each($(".filter-set#" + type).find('li.active span.filterText'), function (index, value) {
        ret = $.trim(ret + $(value).text()) + ",";
    });
    return ret.substring(0, ret.length - 1);
}


$.addDetailsQSParams = function (url, obj, slotId) {
    var node = $(obj).parents('li');
    return url + "?slotId=" + slotId + "&rk=" + node.attr('rankAbs') + "&isP=" + (node.attr('isPremium') == undefined ? false : node.attr('isPremium')) + $.getDeliveryCityAttr(node);
}

$.getDeliveryCityAttr = function (node) {
    return node.attr('deliverycity') != "0" ? "&dc=" + node.attr('deliverycity') : "";
}

$.coachmarkcookie = function () {

    if (isCookieExists('UsedCarsCoachmark1')) {
        var cookie = $.cookie('UsedCarsCoachmark1');
        if (cookie.indexOf('search') == -1) {
            SetCookieInDays('UsedCarsCoachmark1', $.cookie('UsedCarsCoachmark1') + "search", 30);
            $.showCoachMark();
        }
    }
    else {
        SetCookieInDays('UsedCarsCoachmark1', "search" + '|', 30);
        $.showCoachMark();
    }

};
function checkNearByCityCount() {
    var nbcFlag = true;
    var selectedCityIndex = false;
    var nbCity = $("#nearByCityList");
    $("#nearByCityList li").each(function (index, value) {
        if ($(this).hasClass('selected'))
            selectedCityIndex = true;
        if (selectedCityIndex && $(this).find('span').text() > 0 && $(this).attr('cityid') != 9999) {
            nbcFlag = false;
            return nbcFlag;
        }
        else if ($(this).attr('cityid') == 9999 && $(this).is(':visible') && $(this).find('span').text() > 0) {
            nbcFlag = false;
            return nbcFlag;
        }
    })


    return nbcFlag;
};

$.showCoachMark = function () {
    var filterCoachmark = $('div .filters-coachmark');
    var sortCoachmark = $('div .sort-coachmark');
    $(window).load(function () {
        filterCoachmark.delay(1000).fadeIn(1000);
    });
    $('#filterCoachmark').click(function () {
        filterCoachmark.hide();
        if ($('#totalCount').text() == 0) {
            $('#sortCoachmark').text('Got it');
        }
        sortCoachmark.show();
    });
    $('#sortCoachmark').click(function () {
        sortCoachmark.hide();
        $('div .stock-list ul li').first().find("div.coachmark.shortlist-coachmark").removeClass('hide').show();
    });
    $(document).on('click', '#shortlistCoachmark', function () {
        var firstList = $('div .stock-list ul li');
        firstList.first().find("div .shortlist-coachmark").hide();
        firstList.first().find("div .get-details-coachmark").show();
    });
    $(document).on('click', '#detailsCoachmark, .nomoreTips', function () {
        $('div .coachmark').hide();
    });
};
// Photogallery prev next car

function bindArrowClickEvent() {
    $('.availNext').click(function () {
        $(this).animate({ right: '550px', top: '35%', opacity: '0' });
        //  $('.nextBtn').fadeIn();

    });
    $('.availPrev').click(function () {
        $(this).animate({ left: '550px', top: '35%', opacity: '0' });
        //  $('.prevBtn').fadeIn();
    });
}

function bindNextArrowHoverEvent() {
    $('.nextBtn').mouseenter(function () {
        $('.availPrev').animate({ left: '-167px', opacity: '0' });
        $('.availNext').animate({ right: '0', opacity: '1' });

    });
    $('.availNext').mouseleave(function () {
        $('.availNext').animate({ right: '-167px', opacity: '0' });
    });


};
function bindPrevArrowHoverEvent() {

    $('.prevBtn').mouseenter(function () {
        $('.availNext').animate({ right: '-167px', opacity: '0' });
        $('.availPrev').animate({ left: '0', opacity: '1' });

    });
    $('.availPrev').mouseleave(function () {
        $('.availPrev').animate({ left: '-167px', opacity: '0' });
    });

};

$('.email-update-field').click(function (e) {
	var checkbox = $(this).find('span.cw-used-sprite');

	checkbox.toggleClass('uc-checked');
	$(this).closest('.form__user-details').find('.optional-email').toggle();
	
	if (checkbox.hasClass('uc-checked')) {
		$('#txtEmail').focus();
	}
	else {
		$('#txtMobile').focus();
	}
});
