var DealerId;
var CampaignId;
var ListClickResponse = [];
var EMPTYARRAY = [];
var NCD_RESPONSE;

if ($('#mycarousel')) {
    _target = 2
}

function callToMapApi(makeId) {
    $.ajax({
        type: 'GET',
        url: '/webapi/NewCarDealers/showrooms/?makeId=' + makeId + '&cityId=-1&stateId=-1&allStates=1',
        dataType: 'Json',
        success: function (response) {
            try {
                plotGoogleMap(EMPTYARRAY, "mapDiv", false);
                NCD_RESPONSE = response;
            }
            catch (err) {
                console.log('DealerByMake ' + err.message);
            }
        }
    });
}

$('.dealer-city-names li a').on('click', function (event) {
    event.stopPropagation();
});

$('.manufacturer-dealer-city li').on('click', function () {
    if ($(this).find('.dealer-city-names').is(":visible")) {
        plotGoogleMap(EMPTYARRAY, "mapDiv", false);
    }
    else {
        ListClickResponse = [];
        var CurrentId = this.id;
        $.each(NCD_RESPONSE.newCarDealers, function (key, value) {
            if (value.stateId == CurrentId)
                if (isLatLongValid(value.latitude, value.longitude)) {
                    ListClickResponse.push(value);
                }
        });
        if (PageId == 22) {
            if (ListClickResponse.length > 0) {
                plotGoogleMap(ListClickResponse, "mapDiv", true);
            }
            else {
                plotGoogleMap(EMPTYARRAY, "mapDiv", false);
            }
        }
    }
});


$(document).ready(function () {
    bindMakes();

    $('#drpDealerCity').chosen({ max_selected_options: 1 });
    if (PageId == 81) {
        plotGoogleMap(MapDataList, 'divMap', true)
    }
    $('#drpDealerCity').chosen({ max_selected_options: 1 });

    /* Chosen Select box code ends here */

    /* Dealer Pop Up code starts here */
    $(".dealer-req-btn").live('click', function () {
        $("#dealer-request-popup").find('#encryptedResponse').val('');
        $(".blackOut-window").show();
        $("#dealer-request-popup").show();
        DealerId = $(this).closest('.dealer-list-deatils').attr('dealerid');
        CampaignId = $(this).closest('.dealer-list-deatils').attr('campaignid');
        bindModels(MakeId, DealerId);
    });
    $(".blackOut-window").live('click', function () {
        closePopUp();

    });
    /* Dealer Pop Up code ends here */


    /* Dealer Manufacturer Accordian code starts here */
    var bodyHt = $('body').height();
    $('.manufacturer-dealer-city li').click(function () {

        if (!$(this).find('.dealer-city-names').is(":visible")) {
            $('.dealer-city-names').slideUp(0);
            $(this).find('.dealer-city-names').slideDown(0);
            $(this).siblings('li').removeClass('active-dealer-city');
            $(this).addClass('active-dealer-city');
        }
        else {
            $(this).find('.dealer-city-names').slideUp(0);
            $('.active-dealer-city').removeClass('active-dealer-city');

        }
        setTimeout(function () {
            bodyHt = $('body').height();
            $(window).trigger('scroll');
        }, 10);
    });
    /* Dealer Manufacturer Accordian code ends here */

    /* Window Scroll code starts here */
    if (PageId == 22 || PageId == 81) {
        $(window).on('scroll', function (e) {
            e.preventDefault();
            scrollPosition = $(this).scrollTop();
            leftScroll = $('.left-scroll-content').offset().top;
            if (scrollPosition + $(window).height() > bodyHt - leftScroll) {
                $('.dealer-map').removeClass('floating-div');
                $('.dealer-map').addClass('floating-div-middle');
            }
            else if (scrollPosition > leftScroll && scrollPosition < $('footer').offset().top) {
                if (!Common.isScrollLocked) {
                    $('.dealer-map').addClass('floating-div');
                    $('.dealer-map').removeClass('floating-div-middle');
                }
            }
            else {
                $('.dealer-map').removeClass('floating-div');
                $('.dealer-map').removeClass('floating-div-middle');
            }
        });
    }
    /* Window Scroll code ends here */
});


$('.dealer-list-deatils').mouseover(function () {
    showBasicDealerDetails($(this).attr('dealerid'));
});

$('.dealer-list-deatils').mouseout(function () {
    hideInfoWindow();
});

function SubmitLead(clickedButton) {
    // function for submitting Lead
    ShakeFormView($(".contactdetails .form-control-box"));
    var dealerLeadObject = new Object();
    var div = $(clickedButton).closest('#dealer-request-popup');
    dealerLeadObject.Name = $.trim($(div).find("#custName").val());
    dealerLeadObject.Email = $.trim($(div).find("#custEmailOptional").val());
    dealerLeadObject.Mobile = $.trim($(div).find("#custMobile").val());
    dealerLeadObject.MakeId = MakeId;
    dealerLeadObject.modelName = $('.chosen-choices').find('.search-choice').text();
    dealerLeadObject.VersionId = 0;
    dealerLeadObject.InquirySourceId = 127;
    dealerLeadObject.LeadClickSource = 134;
    dealerLeadObject.DealerId = CampaignId;
    dealerLeadObject.CityId = CityId;
    dealerLeadObject.EncryptedPQDealerAdLeadId = $(div).find('#encryptedResponse').val();
    if (clickedButton.context == undefined)
    {
        dealerLeadObject.cwtccat = "DealerListingPage";
        dealerLeadObject.cwtcact = "RequestAssitanceButtonLeadSubmit";
        dealerLeadObject.cwtclbl = 'make:' + MakeName + '|model:' + $("#drpModel option:[selected]").text() + '|city:' + CityName;
    }
    if (IsValidate(div)) {
        $(div).find('.tyHeading').empty();
        setCookies(div);
        dealerLeadObject.modelId = $('#drpModel').val()[0];
        SendNewCarRequestDealer(dealerLeadObject, div);
    }
}

function bindModels(makeId, dealerId) {
    $.ajax({
        type: 'GET',
        url: '/api/NewCarDealers/GetDealerModels/?' + (dealerId == undefined ? '' : ('&dealerId=' + dealerId)),// remove if condition
        dataType: 'Json',
        success: function (json) {
           json = $.grep(json, function (v) {
               return v.makeId === makeId;
            });
            var viewModel = {
                pqCarModels: ko.observableArray(json)
            };
            ko.cleanNode(document.getElementById("drpModel"));
            $('#divModel .chosen-container').remove();
            ko.applyBindings(viewModel, document.getElementById("drpModel"));

            $('#drpModel').chosen({ max_selected_options: 1 });

        }
    });
}

function bindMakes() {
    var dealerMakes = [];
    $.ajax({
        type: 'GET',
        url: '/webapi/NewCarDealers/ncdmakecount/?type=""',
        dataType: 'Json',
        success: function (json) {
            dealerMakes = json;

            var viewModel = {
                pqDealerMakes: ko.observableArray(dealerMakes)
            };

            ko.cleanNode(document.getElementById("drpDealerMakes"));
            ko.applyBindings(viewModel, document.getElementById("drpDealerMakes"));
            $("#drpDealerMakes").chosen({ max_selected_options: 1 });
            registerEvents();
            if (typeof MakeId !== 'undefined') {
                prefillMakeInDropdown(MakeId);
                bindDealerCity($('#drpDealerMakes').val(), false);
            }
        }
    });
}

function registerEvents() {
    $('.search-field input').keyup(function (e) {

        if (e.keyCode == 8) { // if backspace is pressed
            $(this).closest('.chosen-container').addClass('chosen-with-drop');
            $('.chosen-container').children('.result-selected').addClass('active-result').removeClass('result-selected');

            $('.chosen-with-drop').children('li').on("click", function () {
                $('.chosen-with-drop').removeClass('chosen-with-drop');
            });
            $(this).trigger('click');
            disableCityDropdown(this);
        }
    });

    $('#drpDealerMakes').on('change', function () {
        $('.errCar').hide();
        if ($('#drpDealerMakes').val() != null) {
            bindDealerCity($('#drpDealerMakes').val(), true);
        }
    });
}

//bind dealer cities based on make selected
function bindDealerCity(selectedMakeId, openCityDrpdown) {
    $('#drpDealerCity').empty();
    var dealerCities = [];

    $.ajax({
        type: 'GET',
        url: '/webapi/NewCarDealers/cities/?makeId=' + selectedMakeId,
        dataType: 'Json',
        success: function (json) {
            if (json.Item1.length > 0) {
                k = 0;
                for (var i in json.Item1) {
                    for (var j in json.Item1[i].cities) {
                        dealerCities[k] = json.Item1[i].cities[j];
                        k++;
                    }
                }

                dealerCities.sort(function (a, b) {
                    if (a.cityName == b.cityName)
                        return 0;
                    if (a.cityName < b.cityName)
                        return -1;
                    else
                        return 1;
                });

                $('#drpDealerCity').prop("disabled", false);
                $.each(dealerCities, function (key, value) {
                    $('#drpDealerCity').append('<option value="' + value.cityId + '">' + value.cityName + '</option>');
                });

                prefillCity();

                if (openCityDrpdown)
                    openDealerCityDrp();

                $('#drpDealerCity').trigger("chosen:updated");
            }
        }
    });
}

function IsValidate(currentDiv) {
    var retVal = true;
    if (!validateCustName(currentDiv))
        retVal = false;

    if (!validateCustMobile(currentDiv))
        retVal = false;

    if (!validateModel(currentDiv))
        retVal = false;

    if (!validateCustEmail(currentDiv))
        retVal = false;

    return retVal;
}

$('#btnDone').click(function () {
    var custEmail = $.trim($('#custEmailOptional').val());
    var currentDiv = $(this).parent().parent().parent();
    if (custEmail.length > 0) {

        SubmitLead(currentDiv);
        $('#dealer-request-popup').find('.contactdetails').show();
        $('#dealer-request-popup').find('.thank-you-msg').hide();
        $('#dealer-request-popup').hide();
        $(".blackOut-window").hide();

    } else {
        IsValidate(currentDiv)
    }
});

$(".globalcity-close-btn").click(function (e) {
    closePopUp();

});

$(document).keyup(function (e) {
    if (e.keyCode == 27) closePopUp();   // esc
});

function closePopUp() {
    $('#dealer-request-popup').find('.contactdetails').show();
    $('#dealer-request-popup').find('.thank-you-msg').hide();
    $("#dealer-request-popup").hide();
    $(".blackOut-window").hide();
    $('#dealer-request-popup .error').hide();
}



function validateModel(targetFormDiv) {
    var model = $('#drpModel').val();

    if (model == null || model == "" || custName == undefined) {
        $(targetFormDiv).find(' .city-error-icon').show();
        $(targetFormDiv).find(' .city-err-msg').show().html('Please Select Your Model');
        return false;
    } else {
        $(targetFormDiv).find(' .city-error-icon').hide();
        $(targetFormDiv).find(' .city-err-msg').hide();
    }

    return true;
}

//disable city dropdown until make is selected
function disableCityDropdown(node) {
    if ($(node).closest('.chosen-container').attr('id') == "drpDealerMakes_chosen") {
        $('#drpDealerCity').empty();
        $('#drpDealerCity').prop("disabled", true);
        $('#drpDealerCity').trigger("chosen:updated");
    }
}

function redirectToLocateDealer() {
    var cityId = $('#drpDealerCity').val();
    var cityName = $('#drpDealerCity option:selected').text();
    var makeName = $('#drpDealerMakes_chosen .search-choice span').text();
    if ($('#drpDealerCity').children('option').length > 0) {
        if (cityId != "-1" && cityId != null) {
            location.href = "/" + formatSpecial(makeName) + '-dealer-showrooms/' + cityName.replace(" ", "").toLowerCase() + '-' + cityId+'/'
        } else if (typeof (makeName) != "undefined") {
            
            location.href = '/new/' + formatSpecial(makeName) + '-dealers/';
        }
    } else {
        ShakeFormView($(".dealer-make-input"));
        ShakeFormView($(".ldlanding-form-container .form-control-box"));
       $('.errCar').show();
    }
}

$('.chosen-container.chosen-container-multi ul.chosen-choices').live('click', function () {
    //debugger;
    $(this).find('.search-field > input').focus();
});

function prefillMakeInDropdown(makeId) {
    $('#drpDealerMakes').val(makeId);
    $('#drpDealerMakes').trigger("chosen:updated");
}

function prefillCityInDropdown(cityId) {
    $('#drpDealerCity').val(cityId);
}

function openDealerCityDrp() {
    if ($('#drpDealerCity').val() == null) {
        $('#drpDealerCity_chosen .chosen-choices .search-field input').trigger('click');
    }
}

//prefilling city dropdown with page city or next with global city in priority
function prefillCity() {
    if (typeof CityId !== 'undefined') { //page city
        prefillCityInDropdown(CityId);
    }
    if ($('#drpDealerCity').val() == null) {
        var globalCity = $.cookie('_CustCityIdMaster'); //global city
        prefillCityInDropdown(globalCity);
    }
}

