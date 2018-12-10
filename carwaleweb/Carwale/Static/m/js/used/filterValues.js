var filterValues = (function () {
    var _isApplyFilterClicked = false;
    var _filterTypeIds = ['#additional', '#fuel', '#ownership', '#bodyType', '#transmission'];
    function _getLatLongValue() {
        var latlong = locationValue.getLatLongValue();
        return (latlong.latitude > -90 && latlong.latitude < 90) && (latlong.longitude > -180 && latlong.longitude < 180) ?
            "latitude=" + latlong.latitude + "&longitude=" + latlong.longitude + "&" :
            "";
    }
    function _getCityValue() {
        var cityId = locationValue.getCityValue().cityId;
        return cityId > 0 ? "city=" + cityId + "&" : "";
    }

    function _getAreaValue() {
        var areaId = locationValue.getAreaValue().areaId;
        return areaId > 0 ? "area=" + areaId + "&" : "";
    }

    function _getCarLabelValues() {
        var carIds = "";
        $("#carLabel").find("span.makeid").each(function () {
            carIds = ($(this).attr("rootId") + "+" + carIds);
        });
        return (carIds) ? "car=" + carIds.slice(0, -1) + "&" : "";
    };
    function _getBudgetValues() {
        var budget = $("#budgetValues").text();
        if (budget != undefined && budget != "")
            return "budget=" + budget + "&";
        else
            return "budget=" + "0-" + "&";
    };
    function _getAgeValues() {
        var age = $("#ageValues").text();
        return (age) ? "year=" + age + "&" : "year=" + "0-" + "&";
    };
    function _getKilometerValues() {
        var kms = $("#kmsValues").text();
        return (kms) ? "kms=" + kms + "&" : "kms=" + "0-" + "&";
    };

    function _getIconFilterValues($filterType, filterText) {
        var element = _getSelectedIconValues($filterType);
        return (element) ? filterText + "=" + element + "&" : "";
    };

    function _getSelectedIconValues($filterType) {
        var filterIds = "";
        $filterType.find('.active-icon').each(function () {
            filterIds = ($(this).attr("filterId") + "+" + filterIds);
        });
        return (filterIds) ? filterIds.slice(0, -1) : "";
    };

    function _getCheckBoxFilterValues($filterType, filterText) {
        var element = _getSelectedCheckboxValues($filterType);
        return (element) ? filterText + "=" + element + "&" : "";
    };

    function _getSelectedCheckboxValues($filterType) {
        var filterIds = "";
        $filterType.find('li.active').each(function () {
            filterIds = ($(this).attr("filterid") + "+" + filterIds);
        })
        return (filterIds) ? filterIds.slice(0, -1) : "";
    };

    function _getShouldFetchNearByCarValue() {
        var qs = getShouldFetchNearByCarQS();
        if (qs.length > 0) {
            qs += '&';
        }
        return qs;
    }

    function getShouldFetchNearByCarQS() {
        // will only append in QS if the user has unchecked the cars near area/you checkbox.
        var $carsNearAreaCheckbox = $('.area-near-cars input[type=checkbox]');
        var isCheckBoxChecked = $carsNearAreaCheckbox.length < 1 || $carsNearAreaCheckbox.is(':checked');
        return !isCheckBoxChecked ? ('shouldfetchnearbycars=' + isCheckBoxChecked) : '';
    }

    function _getSortValue() {
        var so, sc, $activeSortFilter;
        $activeSortFilter = $("#sortFilter li.active");
        sc = $activeSortFilter.attr("sc");
        so = $activeSortFilter.attr("so");
        so = so || -1;
        sc = sc || -1;
        return "so=" + so + "&" + "sc=" + sc;
    };

    function setAllFilterValues(isFilterApiCalled) {
        _resetAllFilters();
        var currentQueryString = location.search.replace('?', '');
        currentQueryString = currentQueryString.split("&");
        var queryStringLength = currentQueryString.length;
        if (!search.getIsPageLoad()) {
            for (var i = 0; i < queryStringLength; i++) {
                var filter = currentQueryString[i].split("=")[0];
                switch (filter) {
                    case "budget":
                        _setSliderFilter($("#mSlider-budget"), currentQueryString[i], $("#budgetValues"), "#budgetText", "lakh", slider.budgetRange.start, slider.budgetRange.end);
                        break;
                    case "year":
                        _setSliderFilter($("#mSlider-age"), currentQueryString[i], $("#ageValues"), "#ageText", "year", slider.ageRange.start, slider.ageRange.end);
                        break;
                    case "kms":
                        _setSliderFilter($("#mSlider-km"), currentQueryString[i], $("#kmsValues"), "#kmText", "km", slider.kmsRange.start, slider.kmsRange.end);
                        break;
                    case "fuel":
                        _setIconFilter("#fuel", currentQueryString[i], isFilterApiCalled);
                        break;
                    case "owners":
                        _setIconFilter("#ownership", currentQueryString[i], isFilterApiCalled);
                        break;
                    case "bodytype":
                        _setIconFilter("#bodyType", currentQueryString[i], isFilterApiCalled);
                        break;
                    case "trans":
                        _setIconFilter("#transmission", currentQueryString[i], isFilterApiCalled);
                        break;
                    case "city":
                        _setCityValue();
                        break;
                    case "car":
                        _bindCarLabel();
                        break;
                    case "filterbyadditional":
                        _setCheckboxFilter("#additional", currentQueryString[i], isFilterApiCalled);
                        break;
                    case "seller":
                        _setCheckboxFilter("#sellerType", currentQueryString[i], isFilterApiCalled);
                        break;
                }
            }
        }

    };

    function _resetAllFilters() {
        _resetFiltersExceptCity();
        _setCityValue();
    };

    function _resetFiltersExceptCity() {
        slider.computeSliderText("#budgetText", "lakh", 0, 20, slider.budgetRange.start, slider.budgetRange.end);
        slider.computeSliderText("#ageText", "year", 0, 8, slider.ageRange.start, slider.ageRange.end);
        slider.computeSliderText("#kmText", "km", 0, 80000, slider.kmsRange.start, slider.kmsRange.end);
        _resetFiltersCheckBoxes();
        setSliderRangeQS($('#mSlider-km'), slider.kmsRange.start, slider.kmsRange.end);
        setSliderRangeQS($('#mSlider-age'), slider.ageRange.start, slider.ageRange.end);
        setSliderRangeQS($('#mSlider-budget'), slider.kmsRange.start, slider.kmsRange.end);
    };

    function _resetFiltersCheckBoxes() {
        _filterTypeIds.forEach(function (typeId) {
            var activeClass = typeId === '#additional' ? 'active': 'active-icon';
            _clearCheckbox(typeId, activeClass);
        });
    };

    function _clearCheckbox(filterTypeId, activeClass) {
        var selector = filterTypeId + ' li';
        document.querySelectorAll(selector)
            .forEach(function (item) {
                if (item.classList.contains(activeClass)) {
                    item.click();
                }
            });
    }

    function setCarLabelsOnLoad() {
        var pgLoadCars = commonUtilities.getFilterFromQS('car', window.location.search);
        if (!pgLoadCars)
            pgLoadCars = "";
        else
            pgLoadCars = pgLoadCars.trim();
        var carIdsArray = pgLoadCars.split(" ");
        var roots = "";
        var makes = "";
        var len = carIdsArray.length;
        for (var i = 0 ; i < len ; i++) {
            if (carIdsArray[i].indexOf('.') != -1)
                roots = roots + "," + carIdsArray[i].split('.')[1];
            else
                makes += "," + carIdsArray[i];
        }
        makes = makes.substr(1);
        roots = roots.substr(1);
        var carArray = [];
        $.when(
            makes ? $.ajax({
                url: ("/api/v2/makes/?ids=" + makes + "&fields=makename,makeid"),
                success: function (Json) {
                    for (var i = 0 ; i < Json.length ; i++) {
                        var tmp = {
                            'rootId': Json[i].makeId.toString(),
                            'rootName': Json[i].makeName
                        };
                        if (tmp.rootId)
                            carArray.push(tmp);
                    }
                }
            }) : null,
            roots ? $.ajax({
                url: ("/api/roots/?ids=" + roots),
                success: function (Json) {
                    for (var i = 0 ; i < Json.length; i++) {
                        var tmp = {
                            'rootId': Json[i].MakeId + "." + Json[i].RootId,
                            'rootName': Json[i].MakeName + " " + Json[i].Name
                        };
                        if (tmp.rootId)
                            carArray.push(tmp);
                    }
                },
                error: function (msg) {
                    console.log(msg);
                }
            }) : null
    ).then(function () {
        filters.setCarLabelArray(carArray);
        _bindCarLabel();
    });

    };

    function _applyFilterClickChanged(data) {
        _isApplyFilterClicked = data;
    };

    function isApplyFilterClicked() {
        return _isApplyFilterClicked;
    }

    function _bindCarLabel() {
        $(".selected-car").removeClass("hide");
        var $carLabel = $("#carLabel");
        $carLabel.empty();
        var carLabelHtml = "";
        var tmpArray = filters.getCarLabelArray();
        for (var item in tmpArray) {
            carLabelHtml += "<li class='selected-car-list__item'><div class='selected-car-item__name'><span class='makeid hide' rootId='" + tmpArray[item].rootId + "'></span><span class='selected-car__make-name carname nextmake'>" + tmpArray[item].rootName + "</span><span class='close-box'></span></div></li>";
        }
        $carLabel.html(carLabelHtml);
        accordionTextSetter.setCarAccordionHead(tmpArray);
    }

    function setSliderRangeQS($element, start, end) {
        $element.slider("values", 0, start);
        if (end != '')
            $element.slider("values", 1, end);
    };

    function _setIconFilter(element, currentQueryString, isFilterApiCalled) {
        var count = 0, selectedElementsArray, $targetElement;
        var accordionHeadText = isFilterApiCalled ? _getTextofAccordionHead(element) : "";
        if (currentQueryString) {
            count = currentQueryString.split("=")[1].split("+").length;
            selectedElementsArray = currentQueryString.split("=")[1].split("+");
        }
        for (var i = 0; i < count; i++) {
            $targetElement = $(element + " li[filterid='" + selectedElementsArray[i] + "']");
            $targetElement.addClass("active-icon");
            accordionHeadText += ", " + $targetElement.find(".list-item__name").text();
        }
        var obj = {
            htmlElement: element,
            text: accordionHeadText.substr(2)
        };
        accordionTextSetter.setCheckBoxAccordionHead(obj);
    }

    function _getTextofAccordionHead(element) {
        var text = "";
        var $accordionHead = $(element).closest('.accordion-list__item').find('.accordion-head__selected-item');
        if ($accordionHead.length > 0) {
            text = $accordionHead.text();
            if (text) {
                text = ", " + text;
            }
        }
        return text;
    }

    function _setCheckboxFilter(element, currentQueryString, isFilterApiCalled) {
        var count, attr, $targetElement;
        var accordionHeadText = isFilterApiCalled ? _getTextofAccordionHead(element) : "";
        if (element && currentQueryString) {
            attr = currentQueryString.split("=")[1].split("+");
            count = attr.length;
            for (var i = 0; i < count; i++) {
                $targetElement = $(element + " li[filterid='" + attr[i] + "']");
                $targetElement.addClass("active");
                accordionHeadText += ", " + $targetElement.data("item-name");
            }
            var obj = {
                htmlElement: element,
                text: accordionHeadText.substr(2)
            };
            accordionTextSetter.setCheckBoxAccordionHead(obj);
        }
    }

    function _setSliderFilter(element, currentQueryString, elementSpan, sliderTextId, sliderType, start, end) {
        var sliderValues = currentQueryString.split("=")[1];
        var maxValue = sliderValues.split("-")[1];
        var minValue = sliderValues.split("-")[0];
        if (element.selector == '#mSlider-km') {
            minValue = minValue * 1000;
            if (maxValue > 0)
                maxValue = maxValue * 1000;
        }
        elementSpan.text(sliderValues);
        setSliderRangeQS(element, minValue, maxValue);
        if (typeof slider != 'undefined' && slider)
            slider.computeSliderTextforLPage(sliderTextId, sliderType, minValue, maxValue, start, end);
    };

    function _setCityValue() {
        var $element = $("#citySelection");
        var locationObj = locationStorage.getLocationSessionStorage();
        if (locationObj) {
            $element.attr("data-cityid", locationObj[locationStorage.locationSessionStorageKeys.cityId]);
            $element.attr("data-cityname", locationObj[locationStorage.locationSessionStorageKeys.cityName]);
            $element.attr("data-areaid", locationObj[locationStorage.locationSessionStorageKeys.areaId]);
            $element.attr("data-areaname", locationObj[locationStorage.locationSessionStorageKeys.areaName]);
            accordionTextSetter.setCityAccordionHead(locationObj, $element);
        }
    }

    function getCityName(cityId) {
        var cityName = $("#popupCityList li[cityid='" + cityId + "']").first().text();
        return cityName ? cityName : $("#listingTitle").attr("cityName");
    }

    function getCityName(cityId) {
        var cityName = $("#popupCityList li[cityid='" + cityId + "']").first().text();
        return cityName ? cityName : $("#listingTitle").attr("cityName");
    }

    function getQSFilterValues(currQs) {
        if (currQs) {
            currQs += "&";
        }
        var currentQueryString = "?" + currQs;
        currentQueryString += _getCityValue();
        currentQueryString += _getAreaValue();
        currentQueryString += _getLatLongValue();
        currentQueryString += _getBudgetValues();
        currentQueryString += _getAgeValues();
        currentQueryString += _getKilometerValues();
        currentQueryString += _getCarLabelValues();
        currentQueryString += _getCheckBoxFilterValues($("#additional"), "filterbyadditional");
        currentQueryString += _getCheckBoxFilterValues($("#sellerType"), "seller");
        currentQueryString += _getIconFilterValues($("#fuel"), "fuel");
        currentQueryString += _getIconFilterValues($("#ownership"), "owners");
        currentQueryString += _getIconFilterValues($("#bodyType"), "bodytype");
        currentQueryString += _getIconFilterValues($("#transmission"), "trans");
        currentQueryString += _getShouldFetchNearByCarValue();
        currentQueryString += _getSortValue();
        return currentQueryString;
    };
    if (typeof events != 'undefined' && events) {
        events.subscribe("changeFilterCity", _setCityValue);
        events.subscribe("applyFilterclicked", _applyFilterClickChanged);
    }

    var accordionTextSetter = (function () {

        var setCarAccordionHead = function (carLabels) {
            var targetElement = $("#manufacturerAccordionHead");
            if (targetElement.length > 0) {
                var headText = carLabels.map(function (obj) {
                    return obj.rootName;
                }).join(", ");
                targetElement.text(headText);
            }
        }

        var setCityAccordionHead = function (locationObj, element) {
            $element = element || $("#citySelection");
            var headText = "";
            if (locationObj.cityId > 0) {
                $element.find("#cityLabel").text(locationObj.cityName).addClass("city-field--selected-state");
                headText = locationObj.cityName;
                if (locationObj.areaId > 0) {
                    $element.find("#areaLabel").text(locationObj.areaName).addClass("city-field--selected-state").removeClass("hide");
                    headText += ", " + locationObj.areaName;
                }
                else {
                    $element.find("#areaLabel").addClass("hide");
                }
            }
            else {
                $element.find("#areaLabel").addClass("hide");
            }
            var targetElement = $("#cityAccordionHead");
            if (targetElement.length > 0) {
                targetElement.text(headText);
            }
        }

        var setCheckBoxAccordionHead = function (obj) {
            var element = $(obj.htmlElement) || $(this);
            var targetElement = element.closest('.accordion-list__item').find('.accordion-head__selected-item');
            if (targetElement.length > 0) {
                var text = typeof obj.text === 'undefined' ? _getTextForCheckBox(obj.className, targetElement, element) : obj.text;
                targetElement.text(text);
            }
        }

        var _getTextForCheckBox = function (className, targetElement, element) {
            var text = targetElement.text();
            var selectedText = element.data('item-name');
            if (typeof selectedText !== 'undefined' && element.hasClass(className)) {
                text = (text) ? text + ", " + selectedText : selectedText;
            }
            else {
                var regex = new RegExp("\\b" + selectedText + "(?:, |$)", "ig"); //"regex = /\babc(?:, |$)/ig" in case of selectedElement.text() is 'abc'
                text = text.replace(regex, "").replace(/, $/, "");
            }
            return text;
        }

        return {
            setCarAccordionHead: setCarAccordionHead,
            setCityAccordionHead: setCityAccordionHead,
            setCheckBoxAccordionHead: setCheckBoxAccordionHead
        }
    })();

    var clearFilters = function () {
        _resetFiltersExceptCity();
        document.querySelectorAll('#carLabel span.close-box')
            .forEach(function (closeBtn) {
                closeBtn.click();
            });
    }

    return {
        setSliderRangeQS: setSliderRangeQS,
        setAllFilterValues: setAllFilterValues,
        setCarLabelsOnLoad: setCarLabelsOnLoad,
        getQSFilterValues: getQSFilterValues,
        accordionTextSetter: accordionTextSetter,
        isApplyFilterClicked: isApplyFilterClicked,
        getCityName: getCityName,
        getShouldFetchNearByCarQS: getShouldFetchNearByCarQS,
        clearFilters : clearFilters,
    };
})();