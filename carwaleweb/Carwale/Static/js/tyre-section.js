var tyresHomepage = {

    carRootId: 0,
    carRootName: "",
    carRootYear: "",
    carRootSelector: $('#tyreCarRoot'),
	carYearSelectorWrapper: $('#tyreYearSelection'),
	carYearSelector: $('#drpManufYear'),

    registerEvents: function () {
        var prevRootId;        
        ko.applyBindings(tyresHomepage.koTyreViewModel, $("#drpManufYear")[0]);
        ko.applyBindings(tyresHomepage.koTyreViewModel, $("#showModels")[0]);
        tyresHomepage.carRootSelector.val('');

        $('#tyreSearch').on('click', function (e) {
            e.stopPropagation();
            tyresHomepage.validateFormDetails();
        });

        $(document).on('click', '.displayModel', function () {
            var models = $(this).attr('ModelIds');
            var makeName = $(this).attr('MakeName');
            var modelName = $(this).attr('ModelName');
            var label = tyresHomepage.carRootName + '_' + $('#drpManufYear').val();
            Common.utils.trackAction('CWInteractive', 'Tyres_Section_d', 'Vehicle Searched from HP', label);
            tyresHomepage.getSearchResultsPage(makeName + '-' + modelName, models);
        });

        $('#globalPopupBlackOut, .tyre-info-close').on('click', function () {
            $(".search-popup").hide();
            Common.utils.unlockPopup();
        });

        $(document).keydown(function (e) {
            if ((e.keyCode == 27) && ($('.search-popup').is(":visible"))) {
                $(".search-popup").hide();
                $('#drpManufYear_chosen').removeClass('chosen-container-active');
                Common.utils.unlockPopup();
            }
        });

        var objTyreRootCar = new Object();
        tyresHomepage.carRootSelector.cw_autocomplete({            
            resultCount: 10,
            source: ac_Source.accessories,
            onClear: function () {
                objTyreRootCar = new Object();
            },
            click: function (event, ui, orgTxt) {                
                objTyreRootCar.Name = formatSpecial(ui.item.label);
                objTyreRootCar.Id = formatSpecial(ui.item.id);                
                tyresHomepage.carRootId = objTyreRootCar.Id;
                tyresHomepage.carRootName = ui.item.label;
                tyresHomepage.showManufacturingDates();
                $(".cw-blackbg-tooltip").addClass("hide");
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

        $(document).on('change', '#drpManufYear', function () {
            var optionSelected = $("option:selected", this);            
            tyresHomepage.carRootYear = $.trim(optionSelected.text());
            if (optionSelected.val() > 0) {
                $('#errManufactYear').addClass('hide');
                tyresHomepage.validateFormDetails();
            }
        });
        $("a.view-more-btn").click(function (e) {
            e.preventDefault();
            var hiddenElem = $(this).closest('.tyre-home-carousel').find('.news-body-list')
            if (hiddenElem.hasClass("hide")) {
                hiddenElem.fadeIn(1000,function () {
                    hiddenElem.removeClass("hide").find('img.lazy').lazyload();
               });
            }
            else {
                $("html, body").animate({
                    scrollTop: ($(".news-type-container").closest("section").offset().top - 60)
                }, 1000);
                hiddenElem.fadeOut(800, function () {
                    hiddenElem.addClass("hide");
                });
            }
            var moreContent = $(this).find("span");
            moreContent.text(moreContent.text() === "more" ? "less" : "more");
        });
    },

    bindManufacturingdates: function (rootId) {
        $.when(tyresHomepage.getCarRootManufacturingDates(rootId)).done(function (data) {

            if (data != null) {
                tyresHomepage.koTyreViewModel.tyreDate(data);
                var label = tyresHomepage.carRootName + '_' + data[data.length - 1] + '-' + data[0];
                Common.utils.trackAction('CWInteractive', 'Tyres_Section_d', 'Vehicle chosen on HP', label);
            }
            else {
                var currentYear = new Date().getFullYear();
                var startYear = currentYear - 8;
                var defaultYears = [];
                for (var index = currentYear; index >= startYear ; index--) {
                    defaultYears.push(index);
                }
                Common.utils.trackAction('CWInteractive', 'Tyres_Section_d', 'Vehicle chosen on HP', tyresHomepage.carRootName + '_' +'null');
                tyresHomepage.koTyreViewModel.tyreDate(defaultYears);
            }

			var chosenDropdown = $(tyresHomepage.carYearSelectorWrapper).find('.chosen-container');
			
			if(!chosenDropdown.length) {
				$(tyresHomepage.carYearSelector).chosen({
					disable_search: true
				});
			}
			else {
				tyresHomepage.carYearSelector.trigger("chosen:updated");
			}
        	tyresHomepage.carYearSelector.trigger("chosen:open");
        });
    },

    showPopup: function () {
        Common.utils.lockPopup();
        $(".search-popup").show();
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
        if (!$('#drpManufYear').val() > 0) {
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
        window.location.href = '/tyres/' + rootName + '-tyres/?cmids=' + modelIds + '&year=' + tyresHomepage.carRootYear;        
    },

    showSearchResultPage: function () {
        $.when(tyresHomepage.getCarModelsByRootAndYear()).done(function (data) {
            if (data != null && data.length > 0) {
                $('.chosen-single').attr("style", "color:#666;")
                if (data.length == 1) {
                    tyresHomepage.getSearchResultsPage(data[0].makeName + '-' + data[0].displayName, data[0].modelIds);
                    Common.utils.trackAction('CWInteractive', 'Tyres_Section_d', 'Vehicle Searched from HP', tyresHomepage.carRootName + '_' + $('#drpManufYear').val());
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
    tyresHomepage.registerEvents();
});


$("ul.news-body-UL li").click(function () {
    $("ul.news-body-list").slideUp();
    $('.view-more-btn').find("span").text("more");
});
