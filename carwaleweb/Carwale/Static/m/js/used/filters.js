var filters = function () {

    var $document = $(document);
    var $filterDiv = $('#filter-div');
    var $btnApplyfilter = $('#btnApplyFilter');
    var _isCityApiHit = false;
    var _carLabelArray = [];
    var _selectedMakeName;
    var _selectedMake;
    var _isFilterFetched = false;
    var _filterParams = ["city", "budget", "year", "kms", "car", "filterbyadditional", "fuel", "owners",
                         "bodytype", "trans", "area", "latitude", "longitude", "shouldfetchnearbycars"];
    var isFilterPopupOpened = false;  //variable to indicate whether filter popup is open or not

    var _showPopUp = function ($element) {
        $element.addClass("popup_layer").show().scrollTop(0);
        $element.find("ul.ui-content").show();
        $btnApplyfilter.hide();
    };
    var getCarLabelArray = function () {
        return _carLabelArray;
    };
    var setCarLabelArray = function (value) {
        _carLabelArray = value;
    };
    function applyFiltersBindings() {
        $document.on("click", "#btnFilter", _filterScreenOpen);
        $document.on("click", "#filterBack", _filterScreenClose);
        $document.on("click", "#btnApplyFilter", _applyFiltersBtnClick._btnApplyFilterClick);
        $document.on("click", "#citySelection", _citySelection._btncitySelectionClick);
        $document.on("click", "#btnCityBack", _citySelection._cityBackButton);
        $document.on("click", "#manufacturerSelection", _makeSelection._manufacturerSelection);
        $document.on("click", "#btnMakeBack", _makeSelection._makeBack);
        $document.on("click", "ul.makeSelection li", _makeSelection._makeSelectionListClick);
        $document.on("click", "#modelListing li a", _makeSelection._modelClick);
        $document.on("click", "#btnModelBack", _makeSelection._btnModelBack);
        $document.on("click", "#chkAllModels", _makeSelection._selectAllRoots);
        $document.on("click", "#btnSubmit", _makeSelection._btnSubmitClick);
        $document.on("click", "#carLabel span.close-box", _makeSelection._deleteCar);
        $document.on("click", "#additional li,#sellerType li", _sliderSelection._checkBoxClick);
        $document.on("click", ".icon-list__item", _sliderSelection._filterIconClick);
        if (typeof globalLocation !== "undefined") {
            globalLocation.onLocationStorePropertyChange("isActive", _citySelection._cityChanged);
        }
        $(window).on('popstate', _popStateHandler);
    };

    var registerClearFilter = function () {
        document.getElementById('clearFilter')
            .addEventListener('click', function () {
                filterValues.clearFilters();
            });
    };

    var _popStateHandler = function () {
        //these conditions prevent this popstate handler to execute, as their respective js files have popstate which handles everything
        if ($('#sort-div').is(':visible') || $('#globalcity-popup').is(':visible') ||
            $('#buyerForm').is(':visible') || $('#mck-message-cell').is(':visible')) {
            return false;
        }
        if ($('#ucModelListDiv').is(':visible')) {
            $('#ucModelListDiv').hide('slide', { direction: 'right' }, 500);
            $('#btnSubmit').hide('slide', { direction: 'right' }, 500);
        }
        else if ($('#makeList').is(':visible') || $('#cityList').is(':visible')) {
            $('#cityList').hide('slide', { direction: 'right' }, 500);
            $('#makeList').hide('slide', { direction: 'right' }, 500);
            $btnApplyfilter.show();
        }
        else {
            if ($filterDiv.is(':visible')) {
                $filterDiv.hide('slide', { direction: 'right' }, 500);
                $btnApplyfilter.hide('slide', { direction: 'right' }, 500);
                filters.isFilterPopupOpened = false;
                Common.utils.unlockPopup();
            }
            else if ($('#financeIframe').is(':visible')) {
                $('#financeIframe').hide('slide', { direction: 'right' }, 500);
            }
            else if ($('#recommendedCarsPopup').is(':visible')) {
                $("#showSimilarCar").hide('slide', { direction: 'right' }, 500);
            }
            else if ($('.valuation-result-section').is(':visible')) {
                $("#getSellerDetailsBtn").hide();
                $('.valuation-result-section').hide('slide', { direction: 'right' }, 500);
                Common.utils.unlockPopup();
            }
            else {
                var currQS = window.location.href;
                search.listing.getData(currQS, false, { isFilter: true });
                window.scrollTo(0, 0);
                cwTracking.trackCustomData('PageViews', '', 'NA', true);
            }
        }
    };
    var _setCarLabelOnFilterClick = function () {
        var carIds = commonUtilities.getFilterFromQS("car", window.location.search);
        if (!carIds)
            carIds = "";
        var idArray = carIds.split(' ');
        var rootidArr = [];
        for (var item in _carLabelArray) {
            rootidArr[item] = _carLabelArray[item].rootId;
        }
        if (rootidArr.length != idArray.length || !(rootidArr.sort().toString() === idArray.sort().toString())) {
            filterValues.setCarLabelsOnLoad();
        }
    }
    var _filterScreenOpen = function () {
        filters.isFilterPopupOpened = true;
        if (!_isFilterFetched) {
            _isFilterFetched = true;
            Common.utils.showLoading();
            $filterDiv.load("/search/getfilterdata/", function () {
                accordion.registerEvents();
                filters.registerClearFilter();
                Common.utils.hideLoading();
                $btnApplyfilter = $('#btnApplyFilter');
                $('#more-filters-ul').show();
                $('#btnMoreFilter').text("");
                filterValues.setCarLabelsOnLoad();
                slider.bindSliders();
                filterValues.setAllFilterValues(true);
                commonUtilities.addToHistory("filters");
                $filterDiv.show('slide', { direction: 'right' }, 500);
                $btnApplyfilter.show('slide', { direction: 'right' }, 500);
            });
        }
        else {
            _setCarLabelOnFilterClick();
            filterValues.setAllFilterValues(false);
            commonUtilities.addToHistory("filters");
            $filterDiv.show('slide', { direction: 'right' }, 500);
            $btnApplyfilter.show('slide', { direction: 'right' }, 500);
        }

        Common.utils.lockPopup();
    };

    var _filterScreenClose = function () {
        history.back();
        Common.utils.unlockPopup();
    };

    var _applyFiltersBtnClick = {
        _btnApplyFilterClick: function () {
            var removeFilterSet = {};
            filters.isFilterPopupOpened = false;
            _filterParams.forEach(function (param) {
                removeFilterSet[param] = true;
            });
            sort.sortParams.forEach(function (param) {
                removeFilterSet[param] = true;
            });
            var queryString = window.location.search.slice(1);
            var currQS = commonUtilities.removeFilterFromQS(removeFilterSet, queryString);
            currQS = filterValues.getQSFilterValues(currQS);
            var apiUrl = "/m/search/getsearchresults/" + currQS;
            search.listing.getData(apiUrl, false, { isFilter: true });
            _applyFiltersBtnClick._closeFilterPopup();
            window.scrollTo(0, 0);
            window.history.replaceState(currQS, "", "/m/used/cars-for-sale/" + currQS);
            cwTracking.trackCustomData('PageViews', '', 'NA', true);
            var cityObj = locationValue.getCityValue();
            var areaObj = locationValue.getAreaValue();
            var latlongObj = locationValue.getLatLongValue();
            var sessionStorageKeyObj = {}
            sessionStorageKeyObj[locationStorage.locationSessionStorageKeys.cityId] = cityObj.cityId;
            sessionStorageKeyObj[locationStorage.locationSessionStorageKeys.cityName] = cityObj.cityName;
            sessionStorageKeyObj[locationStorage.locationSessionStorageKeys.areaId] = areaObj.areaId;
            sessionStorageKeyObj[locationStorage.locationSessionStorageKeys.areaName] = areaObj.areaName;
            sessionStorageKeyObj[locationStorage.locationSessionStorageKeys.longitude] = latlongObj.longitude;
            sessionStorageKeyObj[locationStorage.locationSessionStorageKeys.latitude] = latlongObj.latitude;
            locationStorage.setLocationSessionStorage(sessionStorageKeyObj);
        },
        _closeFilterPopup: function () {
            $filterDiv.hide('slide', { direction: 'right' }, 500);
            $btnApplyfilter.hide('slide', { direction: 'right' }, 500);
            Common.utils.unlockPopup();
        }
    };

    var _citySelection = {
        _btncitySelectionClick: function () {
            Common.utils.trackAction("CWInteractive", "MSite_FilterPage", "Tap_City_Filter", "Filter Page");
            if (typeof LOCATION_EVENTS !== 'undefined') {
                LOCATION_EVENTS.openPopup([2]);
            }
        },
        _btnCitySelection: function () {
            _showPopUp($('#cityList'));
            commonUtilities.addToHistory("city");
        },
        _cityChanged: function (params) {
            if (params.newValue === false &&
                filters.isFilterPopupOpened &&
                !commonUtilities.areObjectsEqual(params.newStore.location, params.oldStore.location, locationStorage.locationIngnoredKeyForEquality)
            ) {
                var location = locationStorage.getLocationObjFromStore(params.newStore);
                _citySelection._setCityValue(location);
            }
        },
        _setCityValue: function (location) {
            var $element = $("#citySelection");
            $element.attr("data-cityid", location.cityId);
            $element.attr("data-areaid", location.areaId);
            $element.attr("data-cityname", location.cityName);
            $element.attr("data-areaname", location.areaName);
            filterValues.accordionTextSetter.setCityAccordionHead(location, $element);
        },
        _cityBackButton: function () {
            history.back();
        }
    };

    var _makeSelection = {

        _manufacturerSelection: function () {
            _showPopUp($('#makeList'));
            commonUtilities.addToHistory("manufacturer");

        },
        _makeBack: function () {
            history.back();
            $btnApplyfilter.show();
        },
        _makeSelectionListClick: function () {
            _selectedMake = $(this).val().toString();
            _selectedMakeName = $(this).text();
            commonUtilities.apiRequestHandler('/webapi/carrootsdata/getrootsbymakeid/?make=' + _selectedMake, 'GET', {}, null, _makeSelection._makeListApiHandler);
        },
        _makeListApiHandler: function (json) {
            Common.utils.hideLoading();
            _makeSelection._bindModelsListing(json, _selectedMake);
            _makeSelection._openRootList();
            _makeSelection._setModelChkboxes();
        },
        _bindModelsListing: function (json, selectedMake) {
            $("#modelListing").empty();
            var $modelListing = $("#modelListing");
            if (json) {
                var itemHtml = "";
                for (var item in json) {
                    itemHtml += "<li><a rootId=" + selectedMake + "." + json[item].RootId + ">" + json[item].Name + "</a></li>";
                }
                $modelListing.html(itemHtml);
            }
        },
        _openRootList: function () {
            _showPopUp($("#ucModelListDiv"));
            $("#btnSubmit").show();
            window.history.pushState({ currentState: "model" }, "", "");
        },
        _setModelChkboxes: function () {
            var allModelsObj = { allModelsFlag: false };
            var $chkAllModels = $("#chkAllModels");
            $chkAllModels.removeClass("active");
            var $modelListSelection = $("#modelListing a");
            _makeSelection._setCheckOnMake(allModelsObj, $chkAllModels, $modelListSelection)
            _makeSelection._setCheckOnRoot(allModelsObj);
        },
        _setCheckOnMake: function (allModelsObj, $chkAllModels, $modelListSelection) {
            var carLabelArrayLength = _carLabelArray.length;
            for (var j = 0; j < carLabelArrayLength; j++) {
                if (_selectedMake == _carLabelArray[j].rootId) {
                    allModelsObj.allModelsFlag = true;
                    $modelListSelection.addClass("active");
                    $chkAllModels.addClass("active");
                }
            }
        },
        _setCheckOnRoot: function (allModelsObj) {
            var carLabelArrayLength = _carLabelArray.length;
            for (var j = 0; j < carLabelArrayLength && allModelsObj.allModelsFlag == false; j++) {
                $("#modelListing a[rootId='" + _carLabelArray[j].rootId + "']").each(function () {
                    $(this).addClass("active");
                });
            }
        },
        _modelClick: function () {
            var $element = $(this);
            var $chkAllModels = $("#chkAllModels");
            if ($element.hasClass("active")) {
                $element.removeClass("active");
                if ($chkAllModels.hasClass("active"))
                    $chkAllModels.removeClass("active");
            }
            else {
                $element.addClass("active");
                var allModelsFlag = true;
                $("#modelListing a").each(function () {
                    if (!$(this).hasClass("active")) {
                        allModelsFlag = false;
                        return false;
                    }
                });
                if (allModelsFlag)
                    $chkAllModels.addClass("active");
            }
        },
        _btnModelBack: function () {
            $("#chkAllModels").removeClass("active");
            history.back();
        },
        _selectAllRoots: function () {
            var $element = $("#modelListing");
            var $checkAllModels = $("#chkAllModels");
            if ($checkAllModels.hasClass("active")) {
                $element.each(function () {
                    $(this).find("a").removeClass("active");
                });
                $checkAllModels.removeClass("active");
            }
            else {
                $element.each(function () {
                    $(this).find("a").addClass("active");
                });
                $checkAllModels.addClass("active");
            }

        },
        _btnSubmitClick: function () {
            var tempModelArray = [], carNameIds = _carLabelArray;
            _makeSelection._bindModelsLabel(_makeSelection._getSelectedModels(tempModelArray, carNameIds));
            _makeSelection._goBack();
            setTimeout(function () {
                if ($('#makeList').is(':visible'))
                    $('#makeList').hide();
            }, 10);
            $btnApplyfilter.show();
            filterValues.accordionTextSetter.setCarAccordionHead(_carLabelArray);
        },
        _bindModelsLabel: function (tempModelArray) {
            _carLabelArray = jQuery.grep(_carLabelArray, function (value) {
                return value.rootId.split(".")[0] != _selectedMake;
            });
            var finalObj = $.merge(_carLabelArray, tempModelArray);
            _carLabelArray = finalObj;
            $(".selected-car").css("display", "");
            $('#carLabel').empty();
            var carLabelHtml = "";
            for (var item in _carLabelArray) {
                carLabelHtml += "<li class='selected-car-list__item'><div class='selected-car-item__name'><span class='makeid hide' rootId='" + _carLabelArray[item].rootId + "'></span><span class='selected-car__make-name carname nextmake'>" + _carLabelArray[item].rootName + "</span><span class='close-box'></span></div></li>";
            }
            $('#carLabel').html(carLabelHtml);
        },
        _getSelectedModels: function (tempModelArray, carNameIds) {
            if (!$("#chkAllModels").hasClass("active")) {
                tempModelArray = _makeSelection._addSelectedModels(carNameIds);
            }
            else {
                _carLabelArray = jQuery.grep(_carLabelArray, function (value) {
                    return (value.rootId.split(".")[0] != _selectedMake);
                });
                tempModelArray = _makeSelection._getSelectedMake();
            }
            return tempModelArray;
        },
        _goBack: function () {
            $("#chkAllModels").removeClass("active");
            $('#ucModelListDiv').hide('slide', { direction: 'right' }, 500);
            $("#btnSubmit").hide();
            history.go(-2);
        },
        _addSelectedModels: function (carNameIds) {
            var temp = [], tmpObj;
            $("#modelListing a.active").each(function () {
                tmpObj = {
                    'rootId': $(this).attr("RootId"),
                    'rootName': (_selectedMakeName + " " + $(this).text())
                };
                temp.push(tmpObj);
            });
            return temp;
        },
        _getSelectedMake: function () {
            var temp = [];
            var tmp = {
                'rootId': _selectedMake,
                'rootName': _selectedMakeName
            };
            var isFound = commonUtilities.findArrayElement(_carLabelArray, "rootId", _selectedMake);
            if (isFound == false)
                temp.push(tmp);
            return temp;
        },
        _deleteCar: function () {
            var $element = $(this);
            _carLabelArray = commonUtilities.findAndRemove(_carLabelArray, "rootId", $element.siblings(".makeid").attr("rootId"));
            $element.closest('.selected-car-list__item').remove();
            filterValues.accordionTextSetter.setCarAccordionHead(_carLabelArray);
        }
    };
    var _sliderSelection = {
        _checkBoxClick: function () {
            var className = 'active';
            var element = $(this);
            element.toggleClass('active');
            var obj = {
                htmlElement: this,
                className: className
            };
            filterValues.accordionTextSetter.setCheckBoxAccordionHead(obj);
        },
        _filterIconClick: function () {
            var className = 'active-icon';
            $(this).toggleClass(className);
            var obj = {
                htmlElement: this,
                className: className
            };
            filterValues.accordionTextSetter.setCheckBoxAccordionHead(obj);
        }
    };

    return {
        applyFiltersBindings: applyFiltersBindings,
        getCarLabelArray: getCarLabelArray,
        setCarLabelArray: setCarLabelArray,
        isFilterPopupOpened: isFilterPopupOpened,
        registerClearFilter: registerClearFilter
    };
}();

$(document).ready(function () {
    filters.applyFiltersBindings();
});