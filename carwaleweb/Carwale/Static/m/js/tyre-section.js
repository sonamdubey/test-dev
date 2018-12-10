var tyresHomepage = {

    carRootId: 0,
    carRootName: "",
    carRootYear: "",
    carRootSelector: $('#tyreCarRoot'),

    registerEvents: function () {
        var prevRootId;
        $('.strip').on('click', function () {
            $('.year-group').toggle();
            $(".tyre-accordion-icon").toggleClass("open", "");
        });

        ko.applyBindings(tyresHomepage.koTyreViewModel, $('#carYearContainer')[0]);
        ko.applyBindings(tyresHomepage.koTyreViewModel, $("#showModels")[0]);

        //pill box common click function
        $(document).on('click', '#carYearContainer .pill-box', function () {
            var $this = $(this);
            $this.siblings().removeClass('active-box');
            $this.addClass('active-box');
            tyresHomepage.carRootYear = $this.text();
            $(".cw-blackbg-tooltip").addClass("hide");
            tyresHomepage.validateFormDetails();
        });

        $('#tyreSearch').on('click', function (e) {
            e.stopPropagation();
            tyresHomepage.validateFormDetails();
        });

        $(document).on('click', '.displayModel', function () {
            var models = $(this).attr('ModelIds');
            var makeName = $(this).attr('MakeName');
            var modelName = $(this).attr('ModelName');

            tyresHomepage.getSearchResultsPage(makeName+'-'+modelName, models);
        });
        $('#globalPopupBlackOut, .tyre-info-close').on('click', function () {
            window.history.back();
            $(".search-popup").hide();
            Common.utils.unlockPopup();
        });

        $(window).on("popstate", function (e) {
            if ($('.search-popup').is(":visible")) {
                $('.search-popup').hide();
                Common.utils.unlockPopup();
            }
        });

        $(document).keydown(function (e) {
            if (e.keyCode == 27) {
                window.history.back();
            }
        });

        var objTyreRootCar = new Object();
        tyresHomepage.carRootSelector.cw_easyAutocomplete({
            inputField: tyresHomepage.carRootSelector,
            resultCount: 5,
            source: ac_Source.accessories,
            onClear: function () {
                objTyreRootCar = new Object();
            },
            click: function (event, ui, orgTxt) {
                var selectionValue = tyresHomepage.carRootSelector.getSelectedItemData().value,
                selectionLabel = tyresHomepage.carRootSelector.getSelectedItemData().label;
                objTyreRootCar.Name = formatSpecial(selectionLabel);
                objTyreRootCar.Id = selectionValue;
                tyresHomepage.carRootId = objTyreRootCar.Id;
                tyresHomepage.carRootName = selectionLabel;
                tyresHomepage.showManufacturingDates();
                $(".cw-blackbg-tooltip").addClass("hide");
                $(".strip").removeClass("pointerEvent");
                $(".pill-box").siblings().removeClass("active-box");
                tyresHomepage.carRootSelector.blur();                
            },
            afterFetch: function (result) {
                objTyreRootCar.result = result;
            },
            focusout: function () {
                if ((objTyreRootCar.Name == undefined || objTyreRootCar.Name == null || objTyreRootCar.Name == '') && objTyreRootCar.result != undefined && objTyreRootCar.result != null && objTyreRootCar.result.length > 0) {
                    if (objTyreRootCar.result[0].label.toLowerCase().indexOf(tyresHomepage.carRootSelector.val().toLowerCase()) == 0) {
                        objTyreRootCar.Name = formatSpecial(objTyreRootCar.result[0].label);
                        objTyreRootCar.Id = formatSpecial(objTyreRootCar.result[0].id);
                        tyresHomepage.carRootId = objTyreRootCar.Id;
                        tyresHomepage.carRootName = objTyreRootCar.result[0].label;
                        tyresHomepage.carRootSelector.val(objTyreRootCar.result[0].label);
                    }
                }
            }
        });
    },

    bindManufacturingdates: function (rootId) {
        $.when(tyresHomepage.getCarRootManufacturingDates(rootId)).done(function (data) {

            if (data != null) {
                tyresHomepage.koTyreViewModel.tyreDate(data);
                var dataLen = data.length;
                Common.utils.trackAction("CWInteractive", "Tyres_Section_m", "Vehicle chosen on HP", tyresHomepage.carRootName + "_" + data[dataLen -1] + "-" + data[0]);
            }
            else {
                var currentYear = new Date().getFullYear();
                var startYear = currentYear - 8;
                var defaultYears = [];
                for (var index = currentYear; index >= startYear ; index--) {
                    defaultYears.push(index);
                }
                tyresHomepage.koTyreViewModel.tyreDate(defaultYears);
                Common.utils.trackAction("CWInteractive", "Tyres_Section_m", "Vehicle chosen on HP", tyresHomepage.carRootName + "_null");
            }
            if (!$(".tyre-accordion-icon").hasClass('open')) {
                $('.strip').click();
            }
        });
    },

    showPopup: function () {
        Common.utils.lockPopup();
        $(".search-popup").show();
        window.history.pushState('addPopup', "", "");
    },

    checkCarRootInvalid: function () {
        var tyreRootCarField = $.trim(tyresHomepage.carRootSelector.val());
        if (tyreRootCarField == "" || tyreRootCarField == "Type to select car name") {
            $('#errTxtCarRoot').removeClass('hide');
            return true;
        }
        else {
            $('#errTxtCarRoot').addClass('hide');
            return false;
        }
    },

    checkManufacturingYearInvalid: function () {
        if (!$('.year-group').find('.pill-box').hasClass('active-box')) {
            $('#errManufactYear').removeClass('hide');
            return true;
        }
        else {
            $('#errManufactYear').addClass('hide');
            return false;
        }
    },

    showManufacturingDates: function () {
        var prevRootId = 0;
        if (tyresHomepage.carRootId > 0 && tyresHomepage.carRootId != prevRootId) {
            tyresHomepage.bindManufacturingdates(tyresHomepage.carRootId);
            prevRootId = tyresHomepage.carRootId;
        }
    },

    trackSearchClick: function () {
        var label = tyresHomepage.carRootName + "_" + tyresHomepage.carRootYear;
        Common.utils.trackAction('CWInteractive', 'Tyres-Section', 'Vehicle-Search-button-click-HP', label);
    },

    validateFormDetails: function () {

        var isCarRootInvalid = tyresHomepage.checkCarRootInvalid();
        var ManufacturingYearInvalid = tyresHomepage.checkManufacturingYearInvalid();

        if (isCarRootInvalid || ManufacturingYearInvalid) {
            return false;
        }
        else {
            Common.utils.trackAction("CWInteractive", "Tyres_Section_m", "Vehicle Searched from HP", tyresHomepage.carRootName + "_" + tyresHomepage.carRootYear);
            tyresHomepage.trackSearchClick();
            tyresHomepage.showSearchResultPage();
            return true;
        }
    },

    getCarRootManufacturingDates: function (rootId) {
        return Common.utils.ajaxCall({
            type: 'GET',
            url: '/api/roots/' + rootId + '/models/years/',
            dataType: 'Json'
        });
    },

    getCarModelsByRootAndYear: function () {
        return Common.utils.ajaxCall({
            type: 'GET',
            url: '/api/root/' + tyresHomepage.carRootId + '/models/?year=' + tyresHomepage.carRootYear,
            dataType: 'Json'
        });
    },

    getSearchResultsPage: function (rootName, modelIds) {
        rootName = rootName.replace(/\s+/g, '').toLowerCase();
        window.location.href = '/m/tyres/' + rootName + '-tyres/?cmids=' + modelIds + '&year=' + tyresHomepage.carRootYear;
    },

    showSearchResultPage: function () {
        $.when(tyresHomepage.getCarModelsByRootAndYear()).done(function (data) {
            if (data != null && data.length > 0) {
                if (data.length == 1) {
                    tyresHomepage.getSearchResultsPage(data[0].makeName + '-' + data[0].displayName, data[0].modelIds);
                }
                else if (data.length > 1) {
                    tyresHomepage.showPopup();
                    tyresHomepage.koTyreViewModel.modelsData(data);
                }
            }
            else {
                console.log("something went wrong try different combination");
            }
        });
    },

    koTyreViewModel: {
        tyreDate: ko.observableArray([]),
        modelsData: ko.observable([])
    },
}

$(document).ready(function () {
    $('#news-reviews-videos-container .swiper-pagination').hide();
    tyresHomepage.registerEvents();
});
