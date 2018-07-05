ga_pg_id = '4';
var onRoadcity, clientIp = '', onRoadArea, onRoadMakeModel, selectedMakeModel = { makeModelName: "", modelId: "" };
var onCookieObj = {}, mname = "", viewModelOnRoad;
var selectedMakeName = '', selectedCityName = '', gaLabel = '', selectedAreaName = '';

function findCityById(vm, id) {
    return ko.utils.arrayFirst(vm.bookingCities(), function (child) {
        return child.cityId === id;
    });
}

function FillCitiesOnRoad(modelId) {
    showHideMatchError(onRoadcity, false);
    $.ajax({
        type: "GET",
        url: "/api/PQCityList/?modelId=" + modelId,
        dataType: 'json',
        success: function (response) {
            var cities = response.cities;
            var citySelected = null;
            if (cities) {
                insertCitySeparator(cities);
                nbCheckCookies();
                viewModelOnRoad.bookingCities(cities);
                if (!isNaN(onCookieObj.PQCitySelectedId) && onCookieObj.PQCitySelectedId > 0 && viewModelOnRoad.bookingCities() && selectElementFromArray(viewModelOnRoad.bookingCities(), onCookieObj.PQCitySelectedId)) {
                    viewModelOnRoad.selectedCity(onCookieObj.PQCitySelectedId);
                    viewModelOnRoad.hasAreas(findCityById(viewModelOnRoad, onCookieObj.PQCitySelectedId).hasAreas);
                }
                onRoadcity.find("option[value='0']").prop('disabled', true);
                onRoadcity.trigger('chosen:updated');
                cityChangedOnRoad();
            }
            else {
                viewModelOnRoad.bookingCities([]);
                calcWidth();
            }
        }
    });
}

function cityChangedOnRoad() {
    showHideMatchError(onRoadArea, false);
    if (viewModelOnRoad.selectedCity() != undefined) {
        viewModelOnRoad.hasAreas(findCityById(viewModelOnRoad, viewModelOnRoad.selectedCity()).hasAreas);
        if (viewModelOnRoad.hasAreas() != undefined && viewModelOnRoad.hasAreas()) {
            $.ajax({
                type: "GET",
                url: "/api/PQAreaList/?modelId=" + selectedModel + "&cityId=" + viewModelOnRoad.selectedCity(),
                dataType: 'json',
                beforeSend: function (xhr) { xhr.setRequestHeader("X-AjaxPro-Method", "GetPriceQuoteArea"); },
                success: function (response) {
                    areas = response.areas;
                    if (areas.length) {
                        viewModelOnRoad.bookingAreas(areas);
                        if (!isNaN(onCookieObj.PQAreaSelectedId) && onCookieObj.PQAreaSelectedId > 0 && selectElementFromArray(viewModelOnRoad.bookingAreas(), onCookieObj.PQAreaSelectedId)) {
                            viewModelOnRoad.selectedArea(onCookieObj.PQAreaSelectedId);
                        }
                        calcWidth();
                    }
                    else {
                        viewModelOnRoad.selectedArea(0);
                        viewModelOnRoad.bookingAreas([]);
                        calcWidth();
                    }
                },
                error: function (e) {
                    viewModelOnRoad.selectedArea(0);
                    viewModelOnRoad.bookingAreas([]);
                    calcWidth();
                }
            });
        }
        else {
            viewModelOnRoad.bookingAreas([]);
            calcWidth();
        }
    } else {
        viewModelOnRoad.bookingAreas([]);
        calcWidth();
    }
}

function isValidInfoOnRoad() {
    isValid = true;
    var errMsg = "Missing fields:";
    showHideMatchError(onRoadMakeModel, false);
    showHideMatchError(onRoadcity, false);
    showHideMatchError(onRoadArea, false);
    mname = onRoadMakeModel.val();

    if (selectedModel == 0 || (mname == "" || mname.length < 2 || mname == "Search Make and Model")) {
        showHideMatchError(onRoadMakeModel, true);
        errMsg += "Make/Model,";
        isValid = false;
    }

    if (viewModelOnRoad.selectedCity() == undefined) {
        showHideMatchError(onRoadcity, true);
        errMsg += "City,";
        isValid = false;
    }


    if (viewModelOnRoad.bookingAreas().length > 0 && viewModelOnRoad.selectedArea() == undefined) {
        showHideMatchError(onRoadArea, true);
        errMsg += "Area,";
        isValid = false;
    }

    if (!isValid) {
        errMsg = errMsg.substring(0, errMsg.length - 1);
        gtmCodeAppenderWidget(pageId, "Error in submission", errMsg);
    }

    return isValid;
}

function getPriceQuoteOnRoad() {
    if($('#jsvalues').length > 0){
        clientIp = $('#jsvalues').data('clientip');
    }
    var cityId = viewModelOnRoad.selectedCity(), areaId = viewModelOnRoad.selectedArea() ? viewModelOnRoad.selectedArea() : 0;
    if (isValidInfoOnRoad()) {
        if (cityId != onCookieObj.PQCitySelectedId || areaId > 0)
            setLocationCookie($('#ddlCitiesOnRoad option:selected'), $('#ddlAreaOnRoad option:selected'));
        var obj = {
            'CityId': viewModelOnRoad.selectedCity(),
            'AreaId': viewModelOnRoad.selectedArea(),
            'ModelId': selectedModel,
            'ClientIP': clientIp,
            'SourceType': '1',
            'VersionId': 0,
            'pQLeadId': 4,
            'deviceId': getCookie('BWC')
        };

        $.ajax({
            type: 'POST',
            url: "/api/v2/PriceQuote/",
            data: obj,
            dataType: 'json',
            beforeSend: function (xhr) {
                xhr.setRequestHeader('utma', getCookie('__utma'));
                xhr.setRequestHeader('utmz', getCookie('_bwutmz'));
            },
            success: function (json) {
                var jsonObj = json;

                selectedCityName = $("#ddlCitiesOnRoad option:selected").text();

                if (areaId > 0)
                    selectedAreaName = $("#ddlAreaOnRoad option:selected").text();

                if (selectedMakeName != "" && selectedCityName != "") {
                    gaLabel = selectedMakeName + ',' + selectedCityName;

                    if (selectedAreaName != '')
                        gaLabel += ',' + selectedAreaName;
                }

                cookieValue = "CityId=" + viewModelOnRoad.selectedCity() + "&AreaId=" + (!isNaN(viewModelOnRoad.selectedArea()) ? viewModelOnRoad.selectedArea() : 0) + "&PQId=" + jsonObj.quoteId + "&VersionId=" + jsonObj.versionId + "&DealerId=" + jsonObj.dealerId;

                if (jsonObj != undefined && jsonObj.quoteId != "") {

                    if (jsonObj.dealerId > 0) {
                        gtmCodeAppender(pageId, 'Dealer_PriceQuote_Success_Submit', gaLabel);
                    }
                    else {
                        gtmCodeAppender(pageId, 'BW_PriceQuote_Success_Submit', gaLabel);
                    }

                    window.location = "/pricequote/dealer/?MPQ=" + Base64.encode(cookieValue);

                } else {
                    gtmCodeAppenderWidget(pageId, 'BW_PriceQuote_Error_Submit', gaLabel);
                    $("#errMsgOnRoad").text("Oops. We do not seem to have pricing for given details.").show();
                }
            },
            error: function (e) {
                gtmCodeAppenderWidget(pageId, 'BW_PriceQuote_Error_Submit', gaLabel);
                $("#errMsg").text("Oops. Some error occured. Please try again.").show();
            }
        });
    } else {
        gtmCodeAppenderWidget(pageId, 'BW_PriceQuote_Error_Submit', gaLabel);
        $("#errMsgOnRoad").text("Please select all the details").show();
    }
}

function gtmCodeAppenderWidget(pageId, action, label) {
    if (pageId != null) {
        switch (pageId) {
            case "1":
                category = 'CheckPQ_Make';
                break;
            case "2":
                category = "CheckPQ_Series";
                action = "CheckPQ_Series_" + action;
                break;
            case "3":
                category = "CheckPQ_Model";
                break;
            case "4":
                category = "New_Bikes_Page";
                break;
        }
        if (label) {
            dataLayer.push({ 'event': 'Bikewale_all', 'cat': category, 'act': action, 'lab': label });
        }
        else {
            dataLayer.push({ 'event': 'Bikewale_all', 'cat': category, 'act': action });
        }
    }
}

function nbCheckCookies() {
    c = document.cookie.split('; ');
    for (i = c.length - 1; i >= 0; i--) {
        C = c[i].split('=');
        if (C[0] == "location") {
            var cData = (String(C[1])).split('_');
            onCookieObj.PQCitySelectedId = parseInt(cData[0]) || 0;
            onCookieObj.PQCitySelectedName = cData[1] || "";
            onCookieObj.PQAreaSelectedId = parseInt(cData[2]) || 0;
            onCookieObj.PQAreaSelectedName = cData[3] || "";

        }
    }
}

function calcWidth() {
    if (viewModelOnRoad.bookingAreas().length > 0)
        $(ele).width(161);
    else $(ele).width(322);
}


docReady(function () {

    $('#globalSearch').parent().show();

    onRoadcity = $('#ddlCitiesOnRoad'); onRoadArea = $('#ddlAreaOnRoad'); onRoadMakeModel = $('#finalPriceBikeSelect');

    viewModelOnRoad = {
        selectedCity: ko.observable(),
        bookingCities: ko.observableArray([]),
        selectedArea: ko.observable(),
        bookingAreas: ko.observableArray([]),
        hasAreas: ko.observable()
    };

    $.fn.hint = bwHint;
    $.fn.bw_autocomplete = bwAutoComplete;

    $("#finalPriceBikeSelect").bw_autocomplete({
        width: 250,
        source: 2,
        recordCount: 10,
        onClear: function () {
            objBikes = new Object();
        },
        click: function (event, ui, orgTxt) {
            var model = null;
            if (ui.item.payload.modelId > 0) {
                model = new Object();
                model.maskingName = ui.item.payload.modelMaskingName;
                model.id = ui.item.payload.modelId;
                selectedMakeName = ui.item.label;
                pageId = '4';
                gtmCodeAppenderWidget(pageId, "Get_On_Road_Price_Click", selectedMakeName);
                $("#errMsgOnRoad").empty();
                selectedModel = model.id;
                FillCitiesOnRoad(selectedModel);

            }
        },
        open: function (result) {
            objBikes.result = result;
        },
        focusout: function () {
            if ($('li.ui-state-focus a:visible').text() != "") {
                focusedMakeModel = new Object();
                focusedMakeModel = objBikes.result[$('li.ui-state-focus').index()];
                selectedMakeModel.makeModelName = onRoadMakeModel.val();
            }
        },
        afterfetch: function (result, searchtext) {
            return false;
        }
    });

    ko.applyBindings(viewModelOnRoad, $("#OnRoadContent")[0]);



    ele = $('#OnRoadContent .final-price-citySelect')[0];

    ko.bindingHandlers.chosen = {
        init: function (element, valueAccessor, allBindings, viewModel, bindingContext) {
            var $element = $(element);
            var options = ko.unwrap(valueAccessor());
            if (typeof options === 'object')
                $element.chosen(options);

            ['options', 'selectedOptions', 'value'].forEach(function (propName) {
                if (allBindings.has(propName)) {
                    var prop = allBindings.get(propName);
                    if (ko.isObservable(prop)) {
                        prop.subscribe(function () {
                            $element.trigger('chosen:updated');
                        });
                    }
                }
            });
        }
    };
    var viewModel = new ViewModelLD();
    ko.applyBindings(viewModel, $('#NBLocateDealer')[0]);
    viewModel.SelectedMake(0);
    viewModel.SelectedCity(0);

    function ViewModelLD() {
        var self = this;
        self.Makes = ko.observableArray([]);
        self.Cities = ko.observableArray([]);
        self.SelectedMake = ko.observable();
        self.SelectedCity = ko.observable();
        self.cityId = ko.observable();
        self.UpdateCity = function () { FillCity(self); };
        self.btnLocateDealer_click = function () { handleLocateDealer(self); };
        $.ajax({
            type: "GET",
            url: "/api/DealerMakes/",
            dataType: 'json',
            success: function (response) {
                var makes = response.makes;
                if (makes) {
                    self.Makes(ko.toJS(makes));
                }
            }
        });
    }

    function findMakeById(vm, id) {
        return ko.utils.arrayFirst(vm.Makes(), function (child) {
            return child.makeId === id;
        });
    }

    function FillCity(vm) {
        if (vm.SelectedMake()) {
            $.getJSON("/api/DealerCity/?makeId=" + vm.SelectedMake(), vm.Cities);
        }
        else {
            vm.Cities([]);
        }
    }

    function handleLocateDealer(vm) {
        if (vm.SelectedMake() && vm.SelectedCity()) {
            toggleErrorMsg($('#cmbMake'), false);
            toggleErrorMsg($('#cmbCity'), false);
            location.href = "/dealer-showrooms/" + findMakeById(vm, vm.SelectedMake()).maskingName + "/" + vm.SelectedCity().split('_')[1] + "/";
        }
        else {
            if ($('#cmbMake').val() == undefined || $('#cmbMake').val() == 0)
                toggleErrorMsg($('#cmbMake'), true, "Please select a bike");
            else
                toggleErrorMsg($('#cmbMake'), false);
            if ($('#cmbCity').val() == undefined || $('#cmbCity').val() == 0)
                toggleErrorMsg($('#cmbCity'), true, "Please select a city");
            else
                toggleErrorMsg($('#cmbCity'), false);
        }
    }

    function replaceAll(str, rep, repWith) {
        var occurrence = str.indexOf(rep);

        while (occurrence != -1) {
            str = str.replace(rep, repWith);
            occurrence = str.indexOf(rep);
        }
        return str;
    }

    $loanAmount = $('#txtLoanAmount');
    $rateOfInterest = $('#txtRateOfInterest');

    $("#btnCalcEmi").click(function () {

        toggleErrorMsg($rateOfInterest, false);
        toggleErrorMsg($loanAmount, false);
        var re = /^[0-9]*$/;
        var reRateOfInterest = /^([0-9]{1,2}){1}(\.[0-9]{1,2})?$/;
        var loanAmt = $loanAmount.val();
        var rateOfInterestVal = $rateOfInterest.val();
        var isValid = true;

        if (isValid && !reRateOfInterest.test(rateOfInterestVal) && !(parseFloat(rateOfInterestVal) < 30)) {
            toggleErrorMsg($rateOfInterest, true, "Please enter valid rate of interest");
            isValid = false;
        }

        if (isValid && loanAmt == "" || loanAmt == "Enter loan amount") {
            toggleErrorMsg($loanAmount, true, "Please enter valid loan amount");
            isValid = false;
        }
        else if (isValid && loanAmt != "" && re.test(loanAmt) == false) {
            toggleErrorMsg($loanAmount, true, "Please provide numeric data only for loan amount");
            isValid = false;
        }
        else if (isValid && parseInt(loanAmt, 10) < 5000) {
            toggleErrorMsg($loanAmount, true, "Please enter loan amount atleast 5000 or greater");
            isValid = false;
        }

        if (isValid && !reRateOfInterest.test(rateOfInterestVal) && isNaN(rateOfInterestVal) && !(parseFloat(rateOfInterestVal) > 30) && parseFloat(rateOfInterestVal) <= 0) {
            toggleErrorMsg($rateOfInterest, true, "Please enter valid rate of interest");
            isValid = false;
        }

        if (isValid) {
            path = "/finance/emicalculator.aspx?la=" + loanAmt + "&rt=" + rateOfInterestVal;
            window.location = path;
        }
        else {
            return isValid;
        }

    });

    $('.comparison-type-carousel').jcarousel();

    $('.comparison-type-carousel .jcarousel-control-prev')
        .on('jcarouselcontrol:active', function () {
            $(this).removeClass('inactive');
        })
        .on('jcarouselcontrol:inactive', function () {
            $(this).addClass('inactive');
        })
        .jcarouselControl({
            target: '-=2'
        });

    $('.comparison-type-carousel .jcarousel-control-next')
        .on('jcarouselcontrol:active', function () {
            $(this).removeClass('inactive');
        })
        .on('jcarouselcontrol:inactive', function () {
            $(this).addClass('inactive');
        })
        .jcarouselControl({
            target: '+=2'
        });
});