// JavaScript Document
var invalidBudget = false;
var objNewPrice = new Object();
var objDealer = new Object();
var objUserreview = new Object();
var pageNo = 1;
var pageSize = 9;
var cityIdCookie = -1;
var NewPricemodelId = null;
var NewMakeId = null;
var NewModelId = null;
var NewMakeMaskingName = "";
var NewModelMaskingName = "";

var ulUpcoming = '#upComingCars';
var ulNewLaunches = '#newLaunches';
var topSelling = '#topSelling';

var isAreaMandatory = false;

$(document).ready(function () {
    PrefilCityUpcoming();
    cityIdCookie = $.cookie('_CustCityIdMaster');
    //showCityPopUp();
    $('#locateDealerCarSelect').val('');
    $('#getMonthlyUsage').val('');
    $("#getbudgetprice").val('');
    $('#getFinalPrice').val('');
    $('#userReviewSearch').val('');
    $("#drpCity").prop('disabled', true);
    $("#drpCity").val("Select City");
    $('#btnRecommend').click(function () {
        var state = $(this).attr('state');
        var value = $('#' + state).find('input').val();
        if (value.length > 0) {
            if (state == 'budget') {
                if (validateBudget_RC()) {
                    showUsageTab();
                }
            }
            else if (state == 'usage') {
                if (validateMonthlyUsage()) {
                    showPreferenceTab();
                }
            }
        }
        else {
            ShakeFormView($(".budget-usage-form-container"));
            $('#' + state).find('.form-control').addClass('border-red');
            $('#' + state).find('.error').show();
        }
    });
    $('#budget-tab').click(function () {
        if (!$(this).hasClass('disabled-tab')) {
            $.budgetState();
            $.showCurrentTab('budget');
            $('#budget-tab').addClass('active-tab text-bold');
            $('#btnRecommend').attr('state', 'budget').show();

        }
    });

    $('#usage-tab').click(function () {
        if (!$(this).hasClass('disabled-tab')) {
            showUsageTab();
        } else if (!validateBudget_RC()) {
            $('#budget').find('.form-control').addClass('border-red');
            $('#budget').find('.error').show();
        } else {
            showUsageTab();
        }
    });

    $('#preference-tab').click(function () {
        if (!$(this).hasClass('disabled-tab')) {
            showPreferenceTab();
        } else if (!validateBudget_RC()) {
            $('#budget').find('.form-control').addClass('border-red');
            $('#budget').find('.error').show();
        }
    });

    $.showCurrentTab = function (tabType) {
        $('#budget,#usage,#preference').hide();
        $('#' + tabType).show();
    };

    $.budgetState = function () {
        var container = $('.car-to-buy-tabs ul');
        container.find('li:eq(0)').find('span.buy-icon').attr('class', '').attr('class', 'newcars-sprite buy-icon budget-icon-selected');
        container.find('li:eq(1)').find('span.buy-icon').attr('class', '').attr('class', 'newcars-sprite buy-icon usage-icon-grey');
        container.find('li:eq(2)').find('span.buy-icon').attr('class', '').attr('class', 'newcars-sprite buy-icon preference-icon-grey');
        container.find('li').each(function () {
            $(this).find('div').removeClass('text-bold');
        });
        if ($('#getMonthlyUsage').val() > "0") {
            $('#usage-tab').removeClass('disabled-tab text-bold');
            container.find('li:eq(1)').find('span.buy-icon').attr('class', '').attr('class', 'newcars-sprite buy-icon rc-tick-blue');
        } else if (!$('.errMonthlyUsageIcon').is(':visible')) {
            $('#usage-tab').addClass('disabled-tab');
            $('#preference-tab').addClass('disabled-tab');
        }

        if ($('#getbudgetprice').val() > "0") {
            $('#usage-tab').removeClass('disabled-tab text-bold');
        } else {
            $('#usage-tab').addClass('disabled-tab');
        }
    };

    $.usageState = function () {
        var container = $('.car-to-buy-tabs ul');
        container.find('li:eq(0)').find('span.buy-icon').attr('class', '').attr('class', 'newcars-sprite buy-icon rc-tick-blue');
        container.find('li:eq(1)').find('span.buy-icon').attr('class', '').attr('class', 'newcars-sprite buy-icon usage-icon-selected');
        container.find('li:eq(2)').find('span.buy-icon').attr('class', '').attr('class', 'newcars-sprite buy-icon preference-icon-grey');
        container.find('li').each(function () {
            $(this).find('div').removeClass('text-bold');
        });
        if ($('#getMonthlyUsage').val() > 0) {
            $('#preference-tab').removeClass('disabled-tab');
        } else {
            $('#preference-tab').addClass('disabled-tab');
        }
    };

    $.preferenceState = function () {
        var container = $('.car-to-buy-tabs ul');
        container.find('li:eq(0)').find('span.buy-icon').attr('class', '').attr('class', 'newcars-sprite buy-icon rc-tick-blue');
        container.find('li:eq(1)').find('span.buy-icon').attr('class', '').attr('class', 'newcars-sprite buy-icon rc-tick-blue');
        container.find('li:eq(2)').find('span.buy-icon').attr('class', '').attr('class', 'newcars-sprite buy-icon preference-icon-selected');
        container.find('li').each(function () {
            $(this).find('div.car-buy-part').addClass('active-tab text-bold').removeClass('disabled-tab');
        });
        if ($('#getMonthlyUsage').val() > 0) {
            $('#preference-tab').removeClass('disabled-tab text-bold');
        }
    };

    $("#drpDealerCity").prepend('<option value=-1>Select City</option>');

    $('#divDealerCity').on("click", function () {
        if ($('#locateDealerSearch').val() != "" && $('#drpDealerCity').children('option').length <= 1) {
            $('span.errCarIcon').show();
            $('div.errCar').show();
        }
    });

    $('#finalPriceCarSelect').on('blur', function () {
        $('.PriceCarErrMsg').hide();
        $('.PriceCarErrIcn').hide();
        var userCarSelection = $('#finalPriceCarSelect').val().toUpperCase();
        var isError = true;
        $.map(newCarSuggetionsPrice, function (elementOfArray, indexInArray) {
            if ((elementOfArray.l).toUpperCase() == userCarSelection) {
                isError = false;
            }
        });
        if (!isError) {
            $("#drpCity").prop('disabled', false);
            $('#drpCity').removeClass('btn-disable');
        }
        else {
            $('.PriceCarErrMsg').show();
            $('.PriceCarErrIcon').show();
        }
    });

    //on-road price
    $('#getFinalPrice').cw_autocomplete({
        //isNew: 1,
        isPriceExists: 1,
        resultCount: 10,
        textType: ac_textTypeEnum.model,
        source: ac_Source.generic,
        onClear: function () {
            objNewPrice = new Object();
            ShowDropDownDisabled(drpCity);
            HideErrorOnRoadPrice("#getFinal-price");
            $("#drpCity").val("Select City");
        },
        click: function (event, ui, orgTxt) {
            objNewPrice.Name = ui.item.label;
            objNewPrice.Id = ui.item.id;
            NewPricemodelId = ui.item.id.split(':')[2];
            NewMakeMaskingName = ui.item.id.split('|')[0].split(':')[0];
            NewModelMaskingName = ui.item.id.split('|')[1].split(':')[0];
            $('#getFinalPrice').val(objNewPrice.Name);
            PriceBreakUp.Quotation.prefillGlobalCity($("#drpCity"));
            $("#drpCity").removeClass('btn-disable');
            $("#drpCity").prop('disabled', false);
            HideErrorPQ();
            $.cookie('_PQModelId', NewPricemodelId, { path: '/' });
            $.cookie('_PQPageId', "26", { path: '/' });
            $.cookie('_PQVersionId', "0", { path: '/' });
        },
        afterfetch: function (result, reqTerm) {
            if (result != undefined && result.length <= 0) {
                $('.PriceCarErrIcn').show();
                $('.PriceCarErrMsg').show();
                $('#spnCity').show();
                $('#getFinalPrice').addClass('border-red');
                $('#drpCity').addClass('border-red');
                ShowDropDownDisabled(drpCity);
                $("#drpCity").val("Select City");
            }
        },
        open: function (result) {
            objNewPrice.result = result;
        },
        focusout: function () {
            if ($('li.ui-state-focus a:visible').text() != "") {
                var focusedPQCity = objNewPrice.result[$('li.ui-state-focus').index()];
                $('#getFinalPrice').val(focusedPQCity.label);
                NewPricemodelId = focusedPQCity.id.split(':')[2];
                NewMakeMaskingName = focusedPQCity.id.split('|')[0].split(':')[0];
                NewModelMaskingName = focusedPQCity.id.split('|')[1].split(':')[0];
                PriceBreakUp.Quotation.prefillGlobalCity($("#drpCity"));
                $("#drpCity").removeClass('btn-disable');
                $("#drpCity").prop('disabled', false);
                HideErrorPQ();
                $.cookie('_PQModelId', NewPricemodelId, { path: '/' });
                $.cookie('_PQVersionId', "0", { path: '/' });
            }
        }
    });

    //locate dealers
    $('#locateDealerCarSelect').cw_autocomplete({
        isNew: 1,
        isPriceExists: 1,
        resultCount: 10,
        textType: ac_textTypeEnum.make + ',' + ac_textTypeEnum.model,
        source: ac_Source.generic,
        onClear: function () {
            objDealer = new Object();
            $('#locateDealerCarSelect').attr('makeName', 'invalid');
            ShowDropDownDisabled(drpDealerCity);
        },
        click: function (event, ui, orgTxt) {
            objDealer.Name = formatSpecial(ui.item.label);
            objDealer.Id = ui.item.id;
            var values = ui.item.id.replace('|', ':').split(':');
            NewMakeId = values[1];
            var makeName = values[0].toLowerCase();
            $('#locateDealerCarSelect').attr('makename', makeName);
            $('#locateDealerCarSelect').val(objDealer.Name);
            bindDealerCity(NewMakeId);
            $("#drpDealerCity").prop('disabled', false);
            $('span.errCarIcon').hide();
            $('div.errCar').hide();
        },
        afterfetch: function (result, reqTerm) {
            if (result.length <= 0) {
                $('#locateDealerCarSelect').attr('makeName', 'invalid');
                $('span.errCarIcon').show();
                $('div.errCar').show();
                ShowDropDownDisabled(drpDealerCity);
            } else {
                $("#drpDealerCity").prop('disabled', false);
            }
        },
        open: function (result) {
            objDealer.result = result;
        },
        keyup: function () {
            NewMakeId = null;
            objDealer = new Object();
            objDealer.Name = label;
            objDealer.Id = id;
        },
        focusout: function () {
            if ((objDealer.Name == undefined || objDealer.Name == null || objDealer.Name == '') && objDealer.result != undefined && objDealer.result != null && objDealer.result.length > 0) {
                // if (objDealer.result[0].label.toLowerCase().indexOf($('#locateDealerCarSelect').val().toLowerCase()) != -1) {
                objDealer.Name = formatSpecial(objDealer.result[0].label);
                objDealer.Id = formatSpecial(objDealer.result[0].id);
                $('#locateDealerCarSelect').val(objDealer.result[0].label);
                var values = objDealer.result[0].id.replace('|', ':').split(':');
                NewMakeId = values[1];
                var makeName = values[0];
                $('#locateDealerCarSelect').attr('makename', makeName);
                bindDealerCity(NewMakeId);
                $("#drpDealerCity").prop('disabled', false);
                $('span.errCarIcon').hide();
                $('div.errCar').hide();
                //  }
            }
        }
    });

    //user reviews section
    $('#userReviewSearch').cw_autocomplete({
        width: 458,
        isPriceExists: 1,
        resultCount: 10,
        textType: ac_textTypeEnum.model,
        source: ac_Source.generic,
        onClear: function () {
            objUserreview = new Object();
            $("#btnFindUserreviews").css('cursor', 'pointer');
            $('#userReviewLink').attr('href', '/userreviews/');
        },
        afterfetch: function (result, reqTerm) {
            if (result.length <= 0) {
                $("#btnFindUserreviews").css('cursor', 'default');
                $('#userReviewLink').removeAttr('href');
                $('.newCarErrIcn').show();
                $('.newCarErrMsg').show();
            }
        },
        click: function (event, ui, orgTxt) {
            objUserreview.Name = formatSpecial(ui.item.label);
            objUserreview.Id = ui.item.id;
            var values = ui.item.id.split('|');
            var makeName = values[0].split(':')[0];
            var modelName = values[1].split(':')[0];
            $('#userReviewSearch').val(objUserreview.Name);
            $("#userReviewLink").attr('href', '/' + makeName + '-cars/' + modelName + '/userreviews/');
            $("#btnFindUserreviews").css('cursor', 'pointer');
            $('.newCarErrIcn').hide();
            $('.newCarErrMsg').hide();
        },
        open: function (result) {
            objUserreview.result = result;
        },
        keyup: function () {
            NewPricemodelId = null;
            NewMakeMaskingName = "";
            NewModelMaskingName = "";
            objUserreview = new Object();
            objUserreview.Name = label;
            objUserreview.Id = id;
        },
        focusout: function () {
            if ((objUserreview.Name == undefined || objUserreview.Name == null || objUserreview.Name == '') && objUserreview.result != undefined && objUserreview.result != null && objUserreview.result.length > 0) {
                // if (objUserreview.result[0].label.toLowerCase().indexOf($('#userReviewSearch').val().toLowerCase()) != -1) {
                objUserreview.Name = formatSpecial(objUserreview.result[0].label);
                objUserreview.Id = formatSpecial(objUserreview.result[0].id);
                var values = objUserreview.result[0].id.split('|');
                var makeName = values[0].split(':')[0];
                var modelName = values[1].split(':')[0];
                $('#userReviewSearch').val(objUserreview.result[0].label);                
                $("#userReviewLink").attr('href', '/' + makeName + '-cars/' + modelName + '/userreviews/');
                $("#btnFindUserreviews").css('cursor', 'pointer');
                $('.newCarErrIcn').hide();
                $('.newCarErrMsg').hide();
                //  }
            }
        }
    });

    $('#finalprice').on('click', function () {
        var selectedCity = $('#drpCity');
        var cityId = selectedCity.data("cityId");
        var cityName = selectedCity.data("cityName");
        var areaId = selectedCity.data("areaId");
        var areaName = selectedCity.data("areaName");
        var locationObj;
        if (areaId > 0)
            locationObj = { cityId: cityId, cityName: cityName, areaId: areaId, areaName: areaName };
        else
            locationObj = { cityId: cityId, cityName: cityName };
        if (!isNaN(cityId) && !isNaN(NewPricemodelId) && cityId != "-1" && NewMakeMaskingName != "" && NewModelMaskingName != "") {
            NewCar_Common.setLocation(locationObj);
            NewCar_Common.redirectToModelPage(NewMakeMaskingName, NewModelMaskingName);
        } else {
            ShakeFormView($(".tools-price-dealer-container"));
            if (NewPricemodelId == null || NewMakeMaskingName == "" || NewModelMaskingName == "") {
                $('.PriceCarErrIcn').show();
                $('.PriceCarErrMsg').show();
            }
            if (cityId === undefined || cityId == "-1") {
                $('#getFinal-price .cityErr').show();
                $('#getFinal-price .cityErrMsg').show();
            }
        }
    });

    $('#getbudgetprice').blur(function () {
        var isValid = validateBudget_RC();

        if (!isValid) {
            $('#usage-tab').addClass('disabled-tab');
            $('#preference-tab').addClass('disabled-tab');
        } else {
            $('#usage-tab').removeClass('disabled-tab');
        }
    });

    $('#getMonthlyUsage').blur(function () {
        var isValid = validateMonthlyUsage();
        if (!isValid) {
            $('#preference-tab').addClass('disabled-tab');
            $('.errMonthlyUsageIcon').show();
            $('.errMonthlyUsage').show();
            $('#getMonthlyUsage').addClass('border-red');
        } else {
            $('#preference-tab').removeClass('disabled-tab');
            $('#getMonthlyUsage').removeClass('border-red');
            $('.errMonthlyUsageIcon').addClass('hide');
            $('.errMonthlyUsage').addClass('hide');
        }
    });

    $('.btBind').hover(
    function () {
        $('#purposeDescription span').eq(eval($(this).attr('purpose')) - 1).show();
    },
    function () {
        $('#purposeDescription span').eq(eval($(this).attr('purpose')) - 1).hide();
    }
);

    UNTLazyLoad();
    var nextCount = 0, prevCount = 0, nextPageNo = 1;
    $('#upComingCars .jcarousel-control-prev').click(function () {
        prevCount += 1;
    });

    $('#upComingCars .jcarousel-control-next').click(function () {
        nextCount += 1;
        if (nextCount % 2 == 0 && nextCount > prevCount) {
            nextPageNo = nextPageNo + 1;
            var pageNo = nextPageNo;
            var url = '/CarWidgets/UpcomingCarsHomeScreen/?pageNo=' + pageNo + '&pageSize=' + pageSize;
            
            BindData(url, ulUpcoming, Category.Upcoming);
        }
        $(ulUpcoming).find('img.lazy').trigger("UNT");
    });

    var lauchNextCount = 0, lauchPrevCount = 0, lauchNextPageNo = 1;
    $('#newLaunches .jcarousel-control-prev').click(function () {
        lauchNextCount += 1;
    });

    $('#newLaunches .jcarousel-control-next').click(function () {
        lauchNextCount += 1;
        if (lauchNextCount % 2 == 0 && lauchNextCount > lauchPrevCount) {
            lauchNextPageNo = lauchNextPageNo + 1;
            var pageNo = lauchNextPageNo;
            var url = '/CarWidgets/JustLaunchedCars/?pageNo=' + pageNo + '&pageSize=' + pageSize + '&cityId=' + cityIdCookie;            
            BindData(url, ulNewLaunches, Category.NewLaunches);
        }
        $(ulNewLaunches).find('img.lazy').trigger("UNT");
    });

    var sellNextCount = 0, sellPrevCount = 0, sellNextPageNo = 1;
    $('#topSelling .jcarousel-control-prev').click(function () {
        sellPrevCount += 1;
    });
    initLocationPlugin();
    $('#topSelling .jcarousel-control-next').click(function () {
        sellNextCount += 1;
        if (sellNextCount % 2 == 0 && sellNextCount > sellPrevCount) {
            sellNextPageNo = sellNextPageNo + 1;
            var pageNo = sellNextPageNo;
            var url = '/CarWidgets/PopularCars/?pageNo=' + pageNo + '&pageSize=' + pageSize + '&cityId=' + cityIdCookie;
            BindData(url, topSelling, Category.TopSelling);
        }
        $(topSelling).find('img').trigger("UNT");
    });

    initTopSellingPQInstance();
    initJustLaunchesPQInstance();
});

function isCharCode(evt) {
    var charCode = (evt.which) ? evt.which : event.keyCode;
    if (charCode == 13)
        enterKeyPress(evt.target != undefined ? evt.target : window.event.srcElement);
    return true;
}

function enterKeyPress(target) {
    if ($(target).attr('id') == 'getbudgetprice' || $(target).attr('id') == 'getMonthlyUsage') {
        $('#btnRecommend').trigger('click');
    }
}

function validateBudget_RC() {
    var validBudget = formatBudget();
    if (validBudget) {
        $("#getbudgetprice").val(formatNumeric(bd.toString()));
        $("#userBudget").text($("#txtBudget").val());
        $('.inValidBudget').hide();
        $('.errBudgetIcon').hide();
        $('#getbudgetprice').removeClass('border-red');
        return true;
    } else {
        ShakeFormView($(".budget-usage-form-container"));
        $('.inValidBudget').show();
        $('.errBudgetIcon').show();
        $('#getbudgetprice').addClass('border-red');
        return false;
    }
}

function formatBudget() {
    var bdLimit = 150000;
    if ($.isNumeric(($("#getbudgetprice").val().replace(/\,/g, '')))) {
        var tempBD = $("#getbudgetprice").val().replace(/\,/g, '').replace(/^0+/, '');
        bd = 0;
        if (eval(tempBD) <= 100 && eval(tempBD) >= bdLimit / 100000) {
            bd = eval((Math.round(eval(tempBD) * 100) * 1000).toString().split('.')[0]);
            return true;
        }
        else if (eval(tempBD) >= bdLimit) {
            bd = Math.round(eval(tempBD.split('.')[0]));
            return true;
        }

        if (eval(tempBD) < 0) {
            return false;
        }
        return false;
    } else {
        return false;
    }
}

function validateMonthlyUsage() {
    var monthlyUsage = $('#getMonthlyUsage').val();
    if ($.isNumeric((monthlyUsage.replace(/\,/g, '')))) {
        musage = Math.round(monthlyUsage.replace(/\,/g, '').replace(/^0+/, ''));

        if (musage > 0) {
            return true;
        }
        else {
           
            return false;
        }
    }
    else if (monthlyUsage.length > 0) {
        ShakeFormView($(".budget-usage-form-container"));
        return false;
    }
    else
        ShakeFormView($(".budget-usage-form-container"));
        return false;
}

//function to generate querystring to redirect to recommend-cars page
function generateQS(upr) {
    var pr = getPriorities(upr);
    var queryString = "bd=" + bd + '&mu=' + musage + '&upr=' + upr + '&pr=' + pr;
    window.location = "/recommend-cars?" + queryString;
}

//priority values for preference step in recommend cars section
function getPriorities(value) {
    var priorities = [];
    priorities[1] = ["9,4,1,8,5,3,6,2,7"];
    priorities[2] = ["3,4,2,7,9,8,6,5,1"];
    priorities[3] = ["2,9,8,3,1,5,4,7,6"];
    priorities[4] = ["7,5,1,8,9,6,3,2,4"];
    return priorities[value];
}


//bind dealer cities based on make selected
function bindDealerCity(selectedMakeId) {
    var dealerCities = [];
    $('#drpDealerCity').empty();
    $('#drpDealerCity').removeAttr('disabled');
    $('#drpDealerCity').removeClass('btn-disable');
    $("#drpDealerCity").prepend('<option value=-1>---Loading---</option>');
    $.ajax({
        type: 'GET',
        url: '/webapi/NewCarDealers/cities/?makeId=' + selectedMakeId,
        dataType: 'Json',
        success: function (json) {
            k = 0;
            for (var i in json.Item1) {
                for (var j in json.Item1[i].cities) {
                    dealerCities[k] = json.Item1[i].cities[j];
                    k++;
                }
            }

            var viewModel = {
                pqDealerCities: ko.observableArray(dealerCities)
            };

            ko.cleanNode(document.getElementById("drpDealerCity"));
            ko.applyBindings(viewModel, document.getElementById("drpDealerCity"));
            $("#drpDealerCity").prepend('<option value=-1 disabled>Select City</option>');
            preselectCityInPopup();
            if (json.Item1.length <= 0) {
                $('.errCarIcon').show();
                $('.errNoDealers').show();
                ShowDropDownDisabled(drpDealerCity);
            } else {
                $('.errCarIcon').hide();
                $('.errNoDealers').hide();
                $('div.errCar').hide();
            }
        }
    });
}

function preselectCityInPopup() {
    var cityId = $.cookie('_CustCityIdMaster');
    $('#drpDealerCity').val(-1);
    if (cityId != "" && cityId != null && cityId != "-1" && cityId != "-2") {
        $('#drpDealerCity option[value=' + cityId + ']').attr('selected', 'selected');
        $('span.errCarIcon').hide();
        $('div.errCar').hide();
    } else {
        $('#drpDealerCity').val(-1);
    }
}

function locateDealers(e) {
    var cityId = $('#drpDealerCity').val();
    var cityName = $('#drpDealerCity option:selected').text();
    var makeName = $('#locateDealerCarSelect').attr('makename');
    if ($('#drpDealerCity').children('option').length > 1) {
        if (!($('div.locate-dealer-carSelect').find('.errNoDealers').is(":visible") || $('div.locate-dealer-carSelect').find('.errCar').is(":visible"))) {
            if (cityId != "-1" && cityId != null) {
                location.href = "/" + makeName.replace(' ', '').toLowerCase() + '-dealer-showrooms/' + cityName.replace(" ", "").toLowerCase() + '-' + cityId
            } else if (typeof (makeName) != "undefined" && makeName != "invalid") {
                location.href = '/new/' + makeName.replace(' ', '').toLowerCase() + '-dealers/';
            }
            else if (makeName != "invalid") {
                location.href = "/new/locatenewcardealers.aspx";
            }
        }
    } else {
        if (!($('div.locate-dealer-carSelect').find('.errNoDealers').is(":visible") || $('div.locate-dealer-carSelect').find('.errCar').is(":visible"))) {
            if (cityId != "-1" && cityId != null) {
                location.href = "/" + makeName.replace(' ', '').toLowerCase() + '-dealer-showrooms/' + cityName.replace(" ", "").toLowerCase() + '-' + cityId
            } else if (typeof (makeName) != "undefined" && makeName != "invalid") {
                location.href = '/new/' + makeName.replace(' ', '').toLowerCase() + '-dealers/';
            }
            else if (makeName != "invalid") {
                location.href = "/new/locatenewcardealers.aspx";
            }
        }
    }
}

//redirecting to search page based on type & parameters
//eg: type as budget 
function redirectToSearchPage(params, type) {
    var values = [];
    values = params.split(',');
    var queryString = '#';
    $.each(values, function (i, value) {
        queryString += type + '=' + values[i] + '&';
    });
    queryString = queryString.slice(0, -1);

    location.href = '/new/search.aspx' + queryString;
}

function formatNumeric(numStr) {
    var formatted = "";
    var breakPoint = 3;
    if (numStr.length > 3) {
        for (var i = numStr.length - 1; i >= 0; i--) {
            formatted = numStr.charAt(i) + formatted;

            if ((numStr.length - i) == breakPoint && numStr.length > breakPoint) {
                formatted = "," + formatted;
                breakPoint += 2;
            }
        }
        return formatted;
    }
    return numStr;
}

function HideErrorPQ() {
    $('.PriceCarErrIcn').hide();
    $('.PriceCarErrMsg').hide();
    $('#spnCity').hide();
    $('#getFinalPrice').removeClass('border-red');
    $('#drpCity').removeClass('border-red');
}

function bindModelCity(selectedModelId, drpId, callback) {
    $('#' + drpId).empty();
    $('.cityErrIcon').hide();
    $('.cityErrMsg').hide();
    $.ajax({
        type: 'GET',
        url: '/webapi/GeoCity/GetPQCitiesByModelId/?modelid=' + selectedModelId,
        dataType: 'Json',
        success: function (json) {
            var viewModel = {
                pqCities: ko.observableArray(json)
            };
            ko.cleanNode(document.getElementById(drpId));
            ko.applyBindings(viewModel, document.getElementById(drpId));
            ModelCar.PQ.bindZones('', '#' + drpId);
            $("#" + drpId).prepend('<option value=-1>Select City</option>');
            $("#" + drpId + " option[value=" + -1 + "]").attr('disabled', 'disabled');
            $("#" + drpId + " option[value=" + -2 + "]").attr('disabled', 'disabled');
            $('#' + drpId).val("-1");
            $('#' + drpId).removeClass('btn-disable');
            if (typeof (callback) == "function") callback();
        }
    });
}

function checkForCity(cityId, drpCity, divToBind) {
    if (drpCity == 'drpPqCity') {
        if ($('#' + divToBind).find("#" + drpCity + " option[value='" + cityId + "']").length > 0)
            return true;
        else
            return false;
    }
    else {
        if ($("#" + drpCity + " option[value='" + cityId + "']").length > 0)
            return true;
        else
            return false;
    }
}

//Push CRM Lead
function PushCRMLead(name, email, mobile, city, cityName, leadType, carName, makeId, modelId, versionId, event) {
    $.ajax({
        type: "POST",
        url: "/ajaxpro/CarwaleAjax.AjaxResearch,Carwale.ashx",
        data: '{"carName":"' + carName + '", "custName":"' + name + '", "email":"' + email + '", "mobile":"' + mobile + '", "selectedCityId":"' + city + '", "versionId":"' + versionId + '", "modelId":"' + modelId + '", "makeId":"' + makeId + '", "leadtype":"' + leadType + '", "cityName":"' + cityName + '"}',
        beforeSend: function (xhr) { xhr.setRequestHeader("X-AjaxPro-Method", "PushCRM"); },
        success: function (response) {
            var responseObj = eval('(' + response + ')');
            if (responseObj.value) {
                $(event).parents("li").find('.formContent').addClass('hide');
                $(event).parents("li").find('.thankYouForm').removeClass('hide');
                trackTNU("upcomingCars", 'Successful-submit', cityName + "-" + modelId);
            }
            else {
                trackTNU("upcomingCars", 'Unsuccessful-submit', cityName + "-" + modelId);
            }
        }
    });
}

function showUsageTab() {
    $.usageState();
    $.showCurrentTab('usage');
    $('#usage-tab').addClass('active-tab text-bold');
    $('#budget-tab').addClass('active-tab').removeClass('disabled-tab text-bold');
    $('#btnRecommend').attr('state', 'usage').show();
    $('#getMonthlyUsage').focus();
    $('#budget').find('.form-control').removeClass('border-red');
    $('#budget').find('.error').hide();
}

function showPreferenceTab() {
    $.preferenceState();
    $.showCurrentTab('preference');
    $('#preference-tab').attr('state', 'preference');
    $('#usage-tab').removeClass('text-bold');
    $('#budget-tab').removeClass('text-bold');
    $('#preference-tab').addClass('text-bold');
    $('#btnRecommend').hide();
    $('#usage').find('.form-control').removeClass('border-red');
    $('#usage').find('.error').hide();
}

function pushInquiry(event) {

    var modelId = $(event).attr("modelid");
    var nameEle = $(event).parent().find("input[name='userName']");
    var emailEle = $(event).parent().find("input[name='userEmail']");
    var mobileEle = $(event).parent().find("input[name='userMobile']");
    var err = ValidateContactDetails($.trim(nameEle.val()), $.trim(emailEle.val()), $.trim(mobileEle.val()));
    var isFormValid = true;

    if (err[0] == "") {
        nameEle.siblings().addClass('hide');
        nameEle.removeClass('border-red');
    }
    else {
        ShakeFormView($(".userNameInput"));
        nameEle.siblings().removeClass('hide');
        nameEle.addClass('border-red');
        var errSpan = nameEle.siblings()[1];
        $(errSpan).text(err[0]);
        isFormValid = false;
    }

    if (err[1] == "") {
        emailEle.siblings().addClass('hide');
        emailEle.removeClass('border-red');
    }
    else {
        ShakeFormView($(".userEmailInput"));
        emailEle.siblings().removeClass('hide');
        emailEle.addClass('border-red');
        var errSpan = emailEle.siblings()[1];
        $(errSpan).text(err[1]);
        isFormValid = false;
    }

    if (err[2] == "") {
        mobileEle.siblings('.cw-blackbg-tooltip, .error-icon').addClass('hide');
        mobileEle.removeClass('border-red');
    }
    else {
        ShakeFormView($(".userMobileInput"));
        mobileEle.siblings('.cw-blackbg-tooltip, .error-icon').removeClass('hide');//border-red
        mobileEle.addClass('border-red');
        var errSpan = mobileEle.siblings()[2];
        $(errSpan).text(err[2]);
        isFormValid = false;
    }
    if (!ValidateCityUpcoming(modelId)) {
        ShakeFormView($(".userCityInput"));
        isFormValid = false;
    }

    if (isFormValid) {
        PushCRMLead($.trim(nameEle.val()), $.trim(emailEle.val()), $.trim(mobileEle.val()), upcomingLeadCity.id, '', 4, '', '', modelId, '', event);
    }
    else {
        trackTNU('upcomingCars', 'Unsuccessful-validation-submit', modelId);
    }
} 

function showHideDrpError(element, error) {
    if (error) {
        $(element[0]).removeClass('hide');
        $(element[1]).removeClass('hide');
    }
    else {
        $(element[0]).addClass('hide');
        $(element[1]).addClass('hide');
    }
}

function ShowDropDownDisabled(dropDownId) {
    $(dropDownId).empty();
    $(dropDownId).prepend('<option value=-1>Select City</option>');
    $(dropDownId).prop('disabled', true);
    $(dropDownId).addClass('btn-disable');
}

function HideErrorOnRoadPrice(selector) {
    $(selector + ' .PriceCarErrMsg').hide();
    $(selector + ' .PriceCarErrIcn').hide();
    $(selector + ' .cityErr').hide();
    $(selector + ' .cityErrMsg').hide();
}

//TNU
function trackTNU(category, action, label) {
    dataLayer.push({ event: 'TopSelling-Upcoming-New', cat: category, act: action, lab: label });
}

function initLocationPlugin() {
    var div = $('#drpCity');
    new LocationSearch((div), {
        showCityPopup: true,
        isAreaOptional: true,
        callback: function (locationObj) {
            NewCar_Common.redirectToModelPage(NewMakeMaskingName, NewModelMaskingName);
        },
        validationFunction: function () {
           return PriceBreakUp.Quotation.getGlobalLocation();
        }
    });
}

$(document).on('click', "#newLaunches .closeBtn,#topSelling .closeBtn, #upComingCars .closeBtn", function () {
    $(this).parents("li").flip(false);
});
$(document).on("mastercitychange", function (event, cityName, cityId) {
    location.reload();
});
