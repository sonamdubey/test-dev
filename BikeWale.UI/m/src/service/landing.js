var locatorSearchBar, ddlCities, ddlMakes, searchBrandDiv, searchCityDiv, key, selCityId;

docReady(function () {
    selCityId = $('#page-data').data('cityid');

    locatorSearchBar = $("#locatorSearchBar"),
        ddlCities = $("#sliderCityList"),
        ddlMakes = $("#sliderBrandList"),
        searchBrandDiv = $(".locator-search-brand"),
        searchCityDiv = $(".locator-search-city");


    searchBrandDiv.on('click', function () {
        $('.locator-city-slider-wrapper').hide();
        $('.locator-brand-slider-wrapper').show();
        locatorSearchBar.addClass('open').animate({ 'left': '0px' }, 500);
        $(".user-input-box").animate({ 'left': '0px' }, 500);
        $("#locatorBrandInput").focus();
        hideError(searchBrandDiv.find("div.locator-search-brand-form"));
        appendHash("locatorsearch");
    });


    searchCityDiv.on('click', function () {
        if ($('#sliderCityList li').length > 0) {
            $('.locator-brand-slider-wrapper').hide();
            $('.locator-city-slider-wrapper').show();
            locatorSearchBar.addClass('open').animate({ 'left': '0px' }, 500);
            $(".user-input-box").animate({ 'left': '0px' }, 500);
            $("#locatorCityInput").focus();
            hideError(searchCityDiv.find("div.locator-search-city-form"));
            appendHash("locatorsearch");
        }
        else {
            setError($("div.locator-search-brand-form"), "Please select brand!");
        }
    });


    $(document).ready(function () {
        $('#locatorBrandInput').fastLiveFilter('#sliderBrandList');
    });

    key = "ServiceCenterCitiesByMake_";
    bwcache.removeAll(true);
    bwcache.setScope('DLPage');    
    var selMakeId = 0;

    if ((ddlCities.find("li.activeCity")).length > 0) {
        $("div.locator-search-city-form span").text(ddlCities.find("li.activeCity:first").text());
    }
    ddlMakes.on("click", "li", function () {
        var _self = $(this),
                selectedElement = _self.text();
        setSelectedElement(_self, selectedElement);
        _self.addClass('activeBrand').siblings().removeClass('activeBrand');
        $("div.locator-search-brand-form").find("span").text(selectedElement);
        selMakeId = $(this).attr("makeId");
        getCities(selMakeId);
        $(".user-input-box").animate({ 'left': '100%' }, 500);

    });

    ddlCities.on("click", "li", function () {
        var _self = $(this),
            selectedElement = _self.text();
        setSelectedElement(_self, selectedElement);
        _self.addClass('activeCity').siblings().removeClass('activeCity');
        if (!isNaN(selMakeId) && selMakeId != "0") {
            selCityId = $(this).attr("cityId");
        }
        $(".user-input-box").animate({ 'left': '100%' }, 500);
        $("div.locator-search-city-form span").text(selectedElement);
    });

    $(".bwm-brand-city-box .back-arrow-box").on("click", function () {
        locatorSearchBar.removeClass("open").animate({ 'left': '100%' }, 500);
        $(".user-input-box").animate({ 'left': '100%' }, 500);
    });

    function locatorSearchClose() {
        $(".bwm-brand-city-box .back-arrow-box").trigger("click");
    }

    function setSelectedElement(_self, selectedElement) {
        _self.parent().prev("input[type='text']").val(selectedElement);
        locatorSearchBar.addClass('open').animate({ 'left': '100%' }, 500);
    };

    function getCities(mId) {
        ddlCities.empty();
        if (!isNaN(mId) && mId != "0") {
            if (!checkCacheCityAreas(mId)) {
                $.ajax({
                    type: "GET",
                    url: "/api/servicecenter/cities/make/" + mId + "/",
                    contentType: "application/json",
                    dataType: 'json',
                    beforeSend: function () {
                        $("div.locator-search-city-form span").text("Loading cities..");
                    },
                    success: function (data) {
                        bwcache.set(key + mId, data, 30);
                        $("div.locator-search-city-form span").text("Select city");
                        setOptions(data);
                    },
                    complete: function (xhr) {
                        if (xhr.status != 200) {
                            $("div.locator-search-city-form span").text("No cities available");
                            bwcache.set(key + mId, null, 30);
                            setOptions(null);
                        }
                        $('#locatorCityInput').fastLiveFilter('#sliderCityList');
                    }
                });
            }
            else {
                $("div.locator-search-city-form span").text("Select city");
                data = bwcache.get(key + mId);
                setOptions(data);
            }
        }
    }

    $("input[type='button'].locator-submit-btn").click(function () {
        ddlmakemasking = ddlMakes.find("li.activeBrand").attr("makeMaskingName");
        ddlcityId = ddlCities.find("li.activeCity").attr("cityId");
        if (!isNaN(selMakeId) && selMakeId != "0") {
            if (!isNaN(selCityId) && selCityId != "0") {
                ddlcityMasking = ddlCities.find("li.activeCity").attr("citymaskingname");
                bwcache.remove("userchangedlocation", true);
                window.location.href = "/m/service-centers/" + ddlmakemasking + "/" + ddlcityMasking + "/";
            }
            else {
                setError($("div.locator-search-city-form"), "Please select city!");
            }
        }
        else {
            setError($("div.locator-search-brand-form"), "Please select bike brand!");
        }
    });


    function checkCacheCityAreas(cityId) {
        bKey = key + cityId;
        if (bwcache.get(bKey)) return true;
        else return false;
    }

    function setOptions(optList) {
        if (optList != null) {
            $.each(optList, function (i, value) {
                ddlCities.append($('<li>').text(value.cityName).attr('cityId', value.cityId).attr('citymaskingname', value.cityMaskingName));
            });
        }
        else {
            $("div.locator-search-city-form span").text("No cities available");
        }

        if (optList) {
            var selectedElement = $.grep(optList, function (element, index) {
                return element.cityId == selCityId;
            });
            if (selectedElement.length > 0) {
                $("div.locator-search-city-form span").text(selectedElement[0].cityName);
                $('#sliderCityList li[cityId="' + selectedElement[0].cityId + '"]').addClass('activeCity');
            }
        }
    }

    var setError = function (element, msg) {
        element.addClass("border-red").siblings("span.errorIcon, div.errorText").show();
        element.siblings("div.errorText").text(msg);
    };

    var hideError = function (element) {
        element.removeClass("border-red").siblings("span.errorIcon, div.errorText").hide();
    };

    // faqs
    $('.accordion-list').on('click', '.accordion-head', function () {
        var element = $(this);

        if (!element.hasClass('active')) {
            accordion.open(element);
        }
        else {
            accordion.close(element);
        }
    });

    var accordion = {
        open: function (element) {
            var elementSiblings = element.closest('.accordion-list').find('.accordion-head.active');
            elementSiblings.removeClass('active').next('.accordion-body').slideUp();

            element.addClass('active').next('.accordion-body').slideDown();
        },

        close: function (element) {
            element.removeClass('active').next('.accordion-body').slideUp();
        }
    };

    jQuery.fn.fastLiveFilter = function (list, options) {
        // Options: input, list, timeout, callback
        options = options || {};
        list = jQuery(list);
        var input = this;
        var lastFilter = '', noResultLen = 0;
        var noResult = '<div class="noResult">No search found!</div>';
        var timeout = options.timeout || 100;
        var callback = options.callback || function (total) {
            noResultLen = list.siblings(".noResult").length;

            if (total == 0 && noResultLen < 1) {
                list.after(noResult).show();
            }
            else if (total > 0 && noResultLen > 0) {
                $('.noResult').remove();
            }
        };

        var keyTimeout;
        var lis = list.children();
        var len = lis.length;
        var oldDisplay = len > 0 ? lis[0].style.display : "block";
        callback(len); // do a one-time callback on initialization to make sure everything's in sync

        input.change(function () {
            // var startTime = new Date().getTime();
            var filter = input.val().toLowerCase();
            var li, innerText;
            var numShown = 0;
            for (var i = 0; i < len; i++) {
                li = lis[i];
                innerText = !options.selector ?
                    (li.textContent || li.innerText || "") :
                    $(li).find(options.selector).text();

                if (innerText.toLowerCase().indexOf(filter) >= 0) {
                    if (li.style.display == "none") {
                        li.style.display = oldDisplay;
                    }
                    numShown++;
                } else {
                    if (li.style.display != "none") {
                        li.style.display = "none";
                    }
                }
            }
            callback(numShown);
            return false;
        }).keydown(function () {
            clearTimeout(keyTimeout);
            keyTimeout = setTimeout(function () {
                if (input.val() === lastFilter) return;
                lastFilter = input.val();
                input.change();
            }, timeout);
        });
        return this; // maintain jQuery chainability
    }

});