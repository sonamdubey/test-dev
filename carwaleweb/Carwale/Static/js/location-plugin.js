var LocationSearch = function (clickSelector, options) {
    var defaults = {
        cityClassName: 'form-control margin-bottom20 city-search',
        cityPlaceholder: "Select City",
        areaClassName: 'form-control margin-top15',
        areaIdentifierClass: 'pqlocation-plugin-area',
        areaPlaceholder: "Select Area",
        source: ac_Source.allCarCities,
        resultCount: 8,
        width: 600,
        isDirectCallback: false,
        prefillPopup: true,
        ctaText: "CHECK NOW",
        defaultPopupOpen: false,
        isAreaOptional: false,
        isShowLoader: true,
        setGlobalCookie: true,
        defaultPopup: true
    };
    var thisClickElement;
    var popupSelector;
    var locationObj = {};
    var popupSelectorId = 'askLocation_' + parseInt((Math.random() * 10000), 10);
    var options = $.extend(defaults, options || {});

    var html = '';
    var crossIcon = '<span class="cwsprite cross-md-dark-grey cur-pointer location-close-icon city-popup__cross-icon"></span>';

    var submitButton;
    if (!options.defaultPopup) {
        submitButton = ' btn-secondary';
        crossIcon = '<span class="location-close-icon popup-close-button"></span>';
    }
    else {
        submitButton = ' btn-orange margin-top20';
    }

    var cityInputContainer = '';
    cityInputContainer += '<div class="city__input-container">';
    cityInputContainer += '<div class="form-control-box city-input-box"> </div>';
    cityInputContainer += '<div class="form-control-box area-input-box"> </div>';
    cityInputContainer += '<input type="button" id="ctaClick" value="Show On-Road Price" class="btn' + submitButton + ' location-plugin-btn">';
    cityInputContainer += '</div>';
    cityInputContainer += '</div>';
    cityInputContainer += '</section>';
    cityInputContainer += '</section>';

    if (!options.defaultPopup) {
        html += '<div class="location-selection-screen select-city-popup hide" id="' + popupSelectorId + '">';
        html += '<div class="screen-head">';
        html += '<h2 class="select-city-head__title">Select your City</h2>';
        html += '<p class="select-city-head__description">This is necessary to personalise results for you</p>';
        html += crossIcon;
        html += '</div>';
        html += '<div class="screen-body">';
        html += cityInputContainer;
    }
    else {
        html += '<div class="select-city-popup hide" id="' + popupSelectorId + '">';
        html += crossIcon;
        html += '<span class="cw-circle-icon locatedealer-icon"></span>';
        html += '<h2 class="select-city-popup__heading">Select your City</h2>';
        html += cityInputContainer;
    }

    var cityErrorHtml = '<span class="fa fa-spinner fa-spin position-abt pos-right10 pos-top15 text-black" style="display: none;"></span> <span class="cwsprite error-icon PriceCarErrIcn cityErrIcon hide"></span> <div class="cw-blackbg-tooltip PriceCarErrMsg cityErrorMsg hide"></div>';

    var areaErrorHtml = '<span class="fa fa-spinner fa-spin position-abt pos-right10 pos-top15 text-black" style="display: none;"></span> <span class="cwsprite error-icon PriceCarErrIcn areaErrIcon hide"></span> <div class="cw-blackbg-tooltip PriceCarErrMsg areaErrorMsg hide"></div>';

    var isAreaApplicable = askingAreaCityId;
    var citySlider = {};

    var isLocationAvailable = true;

    var hideCityError = function () {
        var popupContainer = $(popupSelector);
        var cityErrorElement = popupContainer.find('.cityErrorMsg');

        cityErrorElement.text('');
        if (!options.defaultPopup) {
            popupContainer.find('.city-input-box').removeClass('invalid');
        }
        else {
            popupContainer.find('.cityErrIcon').hide();
            cityErrorElement.hide();
        }
    }

    var hideAreaError = function () {
        var popupContainer = $(popupSelector);
        var areaErrorElement = popupContainer.find('.areaErrorMsg');

        areaErrorElement.text('');
        if (!options.defaultPopup) {
            popupContainer.find('.area-input-box').removeClass('invalid');
        }
        else {
            popupContainer.find('.areaErrIcon').hide();
            areaErrorElement.hide();
        }
    }

    var showCityError = function (displayText) {
        var popupContainer = $(popupSelector);
        var cityErrorElement = popupContainer.find('.cityErrorMsg');

        cityErrorElement.text(displayText);
        if (!options.defaultPopup) {
            popupContainer.find('.city-input-box').addClass('invalid');
        }
        else {
            popupContainer.find('.cityErrIcon').show();
            cityErrorElement.show();
        }
    }

    var showAreaError = function (displayText) {
        var popupContainer = $(popupSelector);
        var areaErrorElement = popupContainer.find('.areaErrorMsg');

        areaErrorElement.text(displayText);
        if (!options.defaultPopup) {
            popupContainer.find('.area-input-box').addClass('invalid');
        }
        else {
            popupContainer.find('.areaErrIcon').show();
            areaErrorElement.show();
        }
    }

    var showProcessing = function () {
        $(popupSelector).find('[type= "button"]').val("Processing...");
        $(popupSelector).find('[type= "button"]').attr('disabled', true);
    }

    var hideProcessing = function () {
        $(popupSelector).find('[type= "button"]').val(options.ctaText);
        $(popupSelector).find('[type= "button"]').attr('disabled', false);
    }

    var deleteSearch = function (selector, className) {
        var contentBox = $(selector).find("." + className);
        if (contentBox.length > 0) {
            contentBox.children().remove();
            locationObj = {};
        }
    }

    var closePopup = function () {
        var popupContainer = $(popupSelector);
        hideProcessing();
        hideCityError();
        hideAreaError();
        popupContainer.find('.city-input-box').find(':input').attr('value', '');
        popupContainer.find('.area-input-box').find(':input').remove();
        $(".ui-autocomplete").hide();
        locationObj = {};
        if (!options.defaultPopup) {
            popupContainer.removeClass('popup--active');
        }
        else {
            popupContainer.hide();
        }
    }

    var validateLocation = function () {
        if (cityValidation() && (options.isAreaOptional || areaValidation())) {
            return true;
        } else {
            return false;
        }
    }

    var cityValidation = function () {
        if ((locationObj === undefined || locationObj === null || ($.isEmptyObject(locationObj)) || (locationObj.cityId === undefined || locationObj.cityId <= 0))) {
            showCityError("Please select your city");
            $(popupSelector).find(".city-input-box input").focus();
            return false;
        }
        return true;
    }

    var areaValidation = function () {
        if ($.inArray(Number(locationObj.cityId), isAreaApplicable) >= 0 && (locationObj.areaId === undefined || locationObj.areaId <= 0)) {
            showAreaError("Please select your area");
            $(popupSelector).find(".area-input-box input").focus();
            return false;
        }
        return true;
    }

    var openPopUpClick = function () {
        if (popupSelector.length > 0) {
            var popupContainer = $(popupSelector);

            $('.blackOut-window').show();
            $(popupSelector).find('.city-search').focus();
            Common.utils.lockPopup();

            if (!options.defaultPopup) {
                popupContainer.addClass('popup--active');
            }
            else {
                popupContainer.show();
            }
        }
    }

    var validateAndOpenPopup = function () {
        if (options.isDirectCallback) {
            var validOutput = options.validationFunction();
            if (validOutput && validOutput.isComplete) {
                if (options.setGlobalCookie) {
                    setLocationCookie(validOutput);
                }
                options.callback(validOutput);
            }
            else {
                openPopUpClick();
                if (options.prefillPopup == true && validOutput) {
                    prefill(validOutput, popupSelector);
                }
            }
        }
        else {
            openPopUpClick();
        }
    };

    var callToAction = function () {
        if (options.setGlobalCookie) {
            setLocationCookie(locationObj);
        }
        if (options.isShowLoader) {
            showProcessing();
        }
        options.callback(locationObj);
        var scrollTop = parseInt($('html').css('top'));
        $('html').removeClass('lock-browser-scroll');
        $('html,body').scrollTop(-scrollTop);
        Common.isScrollLocked = false;
    };

    var ctaCallAction = function () {
        if (validateLocation()) {
            
            callToAction();
        }
    }

    var enterKeyCallAction = function (e) {
        if (e.which === 13 && cityValidation() && areaValidation()) {
            callToAction();
        }
    }

    var registerEvents = function (options) {
        $(popupSelector).on('click', '.location-close-icon', function () {
            closePopup();
            Common.utils.unlockPopup();
        });
        $(document).on('click', '.blackOut-window', function () {
            closePopup();
            Common.utils.unlockPopup();
        });

        $(popupSelector).on('click', '#ctaClick', function () {
            ctaCallAction();
        });
        $(popupSelector).keyup(function (e) {
            enterKeyCallAction(e);
        });
        $(document).keyup(function (e) {
            if (e.keyCode == 27) {
                closePopup();
                Common.utils.unlockPopup();
            }
        });
    }
    var bindAutocompleteCity = function (selector, containerSelector) {
        return {
            inputField:selector,
            resultCount: options.resultCount,
            source: ac_Source.allCarCities,
            beforefetch: function () {
                hideCityError();
                var cityDiv = $(containerSelector).find(".easy-autocomplete-container");
                if(cityDiv.length > 0)
                {
                    $(cityDiv[0]).show();
                }
            },
            onClear: function () {
                citySlider = {};
                locationObj = {};
                hideCityError();
                hideAreaError();
                deleteSearch(containerSelector, 'area-input-box');
            },
            click: function (event, ui, orgTxt) {
                deleteSearch(containerSelector, 'area-input-box');
                if (typeof (platform) != "undefined" && (platform == 43 || platform == 74)) {
                    citySlider.Name = Common.utils.getSplitCityName($(selector).getSelectedItemData().label);
                    citySlider.Id = $(selector).getSelectedItemData().value;
                    $(containerSelector.find(".easy-autocomplete-container")[0]).hide();
                }
                else {
                    citySlider.Name =  Common.utils.getSplitCityName(ui.item.label);
                    citySlider.Id = ui.item.id;
                    ui.item.value = citySlider.Name;
                }
                locationObj = { cityId: citySlider.Id, cityName: citySlider.Name };
                hideCityError();

                if ($.inArray(Number(citySlider.Id), isAreaApplicable) >= 0) {
                    searchInit(containerSelector, {
                        areaClassName: options.areaClassName,
                        areaPlaceholder: options.areaPlaceholder,
                        source: ac_Source.areaLocation,
                        cityId: citySlider.Id,
                        cityName: citySlider.Name,
                        areaIdentifierClass: options.areaIdentifierClass
                    });
                    $(popupSelector).find(".area-input-box input").focus();
                }
                if(options.setGlobalCookie && !isLocationAvailable)
                {
                    setLocationCookie(locationObj);
                }
            },
            open: function (result) {
                citySlider.result = result;
            },
            afterfetch: function (result, searchtext) {
                if (result == undefined || result.length <= 0) {
                    showCityError("Sorry! No matching results found. Try again.");

                } else {
                    hideCityError();
                }
            }
        };
    }
    var bindAutocompleteArea = function (selector, options) {
        return {
            inputField: selector,
            resultCount: options.resultCount,
            source: ac_Source.areaLocation,
            cityId: options.cityId,
            beforefetch: function () {
                this.cityId = options.cityId;
                hideAreaError();
            },
            onClear: function () {
                citySlider = {};
                locationObj = { cityId: options.cityId, cityName: options.cityName };
                hideAreaError();
            },
            click: function (event, ui, orgTxt) {
                var zoneId;
                var zoneName;
                if (typeof (platform) != "undefined" && (platform == 43 || platform == 74)) {
                    var locationValue = $(selector).getSelectedItemData();
                    citySlider.Name = Common.utils.getSplitCityName(locationValue.payload.areaName);
                    citySlider.Id = locationValue.payload.areaId;
                    zoneId = locationValue.payload.zoneId;
                    zoneName = locationValue.payload.zoneName;
                }
                else {
                    citySlider.Name = Common.utils.getSplitCityName(ui.item.label);
                    citySlider.Id = ui.item.id;
                    ui.item.value = citySlider.Name;
                    zoneId = ui.item.payload.zoneId;
                    zoneName = ui.item.payload.zoneName;
                }
                locationObj = { cityId: options.cityId, cityName: options.cityName, areaId: citySlider.Id, areaName: citySlider.Name, zoneId: zoneId, zoneName: zoneName };
                hideAreaError();
            },
            afterfetch: function (result, searchtext) {
                if (result == undefined || result.length <= 0) {
                    showAreaError('Sorry! No matching results found. Try again.');
                } else {
                    hideAreaError();
                }
            },
            open: function (result) {
                citySlider.result = result;
            }
        };

    }
    var bindCities = function (inputSelector, containerSelector) {
        
        if (typeof (platform) != "undefined" && (platform == 43 || platform == 74)) {
            $(inputSelector).cw_easyAutocomplete(bindAutocompleteCity(inputSelector,containerSelector));
        }
        else {
            $(inputSelector).cw_autocomplete(bindAutocompleteCity(inputSelector,containerSelector)).autocomplete("widget").css({
                'position': 'fixed',
                'z-index': 100002
            });
        }
    }

    var bindArea = function (selector, options) {
        if (typeof (platform) != "undefined" && (platform == 43 || platform == 74)) {
            $(selector).cw_easyAutocomplete(bindAutocompleteArea(selector,options));
        }
        else{
            $(selector).cw_autocomplete(bindAutocompleteArea(selector,options))
             .autocomplete("widget").css({
                    'position': 'fixed',
                    'z-index': 100002
                });
        }
    }

    var bindAutoComplete = function (inputSelector, options, containerSelector) {
        switch (options.source) {
            case ac_Source.allCarCities:
                bindCities(inputSelector, containerSelector);
                break;
            case ac_Source.areaLocation:
                bindArea(inputSelector, options);
                break;
        }
    }

    var injectPopupHtml = function (options) {
        var container = document.createElement('div');
        container.innerHTML = html;
        document.body.appendChild(container);
        popupSelector = $('#' + popupSelectorId);
        $(popupSelector).find('[type= "button"]').val(options.ctaText);
    }

    var bindPopup = function (options) {
        injectPopupHtml(options);
        searchInit(popupSelector, {
            cityClassName: options.cityClassName,
            cityPlaceholder: options.cityPlaceholder,
            source: ac_Source.allCarCities,
            areaClassName: options.areaClassName,
            areaPlaceholder: options.areaPlaceholder,
            areaIdentifierClass: options.areaIdentifierClass,
            showCityPopup: false
        });
        registerEvents(options);
    }

    var createInputTag = function (selector, options) {
        var inputTag = document.createElement("input");
        inputTag.setAttribute("type", "text");
        if (options.source == ac_Source.allCarCities) {
            $(selector).find('.city-input-box').prepend(inputTag);
            $(selector).find('.city-input-box').append(cityErrorHtml);
            return inputTag;
        }
        else if (options.source == ac_Source.areaLocation) {
            $(selector).find('.area-input-box').prepend(inputTag);
            $(selector).find('.area-input-box').append(areaErrorHtml);
            return inputTag;
        }
    }

    var bindOptions = function (inputSelector, options) {
        if (options.source == ac_Source.allCarCities) {
            inputSelector.placeholder = options.cityPlaceholder;
            inputSelector.width = options.width;
            inputSelector.className = options.cityClassName;
        }
        if (options.source == ac_Source.areaLocation) {
            inputSelector.placeholder = options.areaPlaceholder;
            inputSelector.width = options.width;
            inputSelector.className = options.areaClassName + " " + options.areaIdentifierClass;
        }
    }

    var prefill = function (location, divSelector) {
        if (location != undefined && !$.isEmptyObject(location) && typeof(location.cityId) != "undefined" && location.cityId > 0) {
            if (location.cityName != undefined) {
                $(divSelector).find('.city-search').val(location.cityName);
                deleteSearch(divSelector, 'area-input-box');
            }
            locationObj = location;
            if ($.inArray(Number(location.cityId), isAreaApplicable) >= 0) {
                searchInit(divSelector, {
                    areaClassName: options.areaClassName,
                    areaPlaceholder: options.areaPlaceholder,
                    source: ac_Source.areaLocation,
                    cityId: location.cityId,
                    cityName: location.cityName,
                    areaIdentifierClass: options.areaIdentifierClass
                });
                if (location.areaName != undefined) {
                    $(divSelector).find("." + options.areaIdentifierClass).val(location.areaName);
                }
            }
            $(divSelector).find('.city-search').off("blur");
            $(divSelector).find('.' + options.areaIdentifierClass).focus();
        }
    }

    var searchInit = function (selector, options) {
        popupSelector = options.customPopUpSelector || selector;
        var inputSelector = createInputTag(selector, options);
        bindOptions(inputSelector, options);
        bindAutoComplete(inputSelector, options, selector);
    }

    var destroyHtml = function () {
        $('#' + popupSelectorId).remove();
        $(clickSelector).unbind('click');
        $(clickSelector).data("location-plugin", false);
    }

    var setLocationCookie = function (location) {
        var now = new Date();
        var Time = now.getTime();
        Time += 1000 * 60 * 60 * 24 * 30;
        now.setTime(Time);

        document.cookie = '_CustCityIdMaster=' + location.cityId + '; expires = ' + now.toGMTString() + '; domain=' + defaultCookieDomain + '; path =/';
        document.cookie = '_CustCityMaster=' + location.cityName + '; expires = ' + now.toGMTString() + '; domain=' + defaultCookieDomain + '; path =/';
        document.cookie = '_CustZoneIdMaster=' + (location.zoneId || -1) + '; expires = ' + now.toGMTString() + '; domain=' + defaultCookieDomain + '; path =/';
        document.cookie = '_CustAreaId=' + (location.areaId || -1) + '; expires = ' + now.toGMTString() + '; domain=' + defaultCookieDomain + '; path =/';
        document.cookie = '_CustAreaName=' + (location.areaName || "Select Area") + '; expires = ' + now.toGMTString() + '; domain=' + defaultCookieDomain + '; path =/';

    }

    this.getLocation = function () {
        return locationObj;
    };
    this.selector = function () {
        return thisClickElement;
    };
    this.prefill = function (location, divSelector) {
        prefill(location, divSelector);
    };
    this.setLocation = function (currentLocationObj) {
        locationObj = currentLocationObj;
    };
    this.validateLocation = function () {
        if(validateLocation())
        {
            if(options.setGlobalCookie)
            {
                setLocationCookie(locationObj);
            }
            return true;
        }
        return false;
    };
    this.destroy = function () {
        return destroyHtml();
    };
    this.closePopup = function () {
        return closePopup();
    };
    var thisInstance = this;

    var init = function (options) {
        if (!$(clickSelector).data("location-plugin")) {
            $(clickSelector).data("location-plugin", thisInstance);
            if (options.showCityPopup == true) {
                $(clickSelector).on("click", function () {
                    thisClickElement = $(this);
                    validateAndOpenPopup();
                });
                bindPopup(options);
            }
            else {
                searchInit(clickSelector, options);
            }
            if (options.defaultPopupOpen) {
                validateAndOpenPopup();
            }
        }
        else {
            thisInstance = $(clickSelector).data("location-plugin");
        }
        isLocationAvailable = $.cookie("_CustCityIdMaster") == null || $.cookie("_CustCityIdMaster") <= 0 ? false : true;

    };

    init(options);
    return thisInstance;
}