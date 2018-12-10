var VolvoV90 = {
    doc: $(document),
    bookingLeadCity: {},
    newWidth: null,
    customerInfo: {},
    inquiryInfo: {},
    statusObj: {
        "1": "first",
        "2": "second",
        "3": "third",
        "4": "fourth",
        "5": "fifth"
    },
    lastStatus: "first",
    firstTimeLoad: true,
    prevVersionId: 0,
    prevExtColorId: 0,
    galleryPopup: $("#galleryPopup"),
    specialElementHandlers: {
        '#editor': function (element, renderer) {
            return true;
        }
    },

    responsive: {
        initAutocomplete: function () {
            if (isMobile) {
                var personCityInputField = $("#booking__personCity");

                $(personCityInputField).cw_easyAutocomplete({
                    inputField: personCityInputField,
                    resultCount: 5,
                        source: ac_Source.allCarCities,
                            click: function (event) {
                                var selectionValue = personCityInputField.getSelectedItemData().value,
                        selectionLabel = personCityInputField.getSelectedItemData().label;

                        VolvoV90.bookingLeadCity.name = Common.utils.getSplitCityName(selectionLabel);
                        VolvoV90.bookingLeadCity.id = selectionValue;
                        $(personCityInputField).val(VolvoV90.bookingLeadCity.name);
                        },

                                afterFetch: function (result, searchText) {
                        VolvoV90.bookingLeadCity.result = result;

                        if (typeof result == "undefined" || result.length <= 0) {
                            VolvoV90.showHideMatchError(true, $(personCityInputField).closest('.easy-autocomplete'), "No city Match");
                        }
                        else {
                            VolvoV90.showHideMatchError(false, $(personCityInputField).closest('.easy-autocomplete'));
                        }
                    }
                    });
            }
            else {
                $("#booking__personCity").cw_autocomplete({
                    resultCount: 5,
                    source: ac_Source.allCarCities,
                    click: function (event, ui, orgTxt) {
                        VolvoV90.bookingLeadCity.name = Common.utils.getSplitCityName(ui.item.label);
                        VolvoV90.bookingLeadCity.id = ui.item.id;
                        ui.item.value = VolvoV90.bookingLeadCity.name;
                    },
                    open: function (result) {
                        VolvoV90.bookingLeadCity.result = result;
                    },
                    afterfetch: function (result, searchtext) {
                        this.result = result;
                        if (typeof result == "undefined" || result.length <= 0)
                            VolvoV90.showHideMatchError(true, $('#booking__personCity'), "No city Match");
                        else
                            VolvoV90.showHideMatchError(false, $('#booking__personCity'));
                    }
                });
            }
        },
        bookingEvents: function () {

            $("#ad-container__startBooking").on("click", function () {
                VolvoV90.scrollToBooking();
                $("#startBooking").trigger("click");
            });

            $("#startBooking").on("click", function () {
                $('input[name=VersionId]').trigger("click");
                $(".screen").addClass("hide");
                $("#step1, .booking__car-info").removeClass("hide");
                $(".booking__status").removeClass(VolvoV90.lastStatus + " hide").addClass("first");
                $(".booking__status li:nth-child(1)").addClass("active");
                VolvoV90.lastStatus = "first";
                VolvoV90.lazyImg.applyLazyLoad();
                VolvoV90.koViewModel.imageSrc("https://imgd.aeplcdn.com/0x0/cw/es/volvo/v90/car-brightsilver.jpg");   // bind default image
            });

            $("#step1Btn").on("click", function () {
                VolvoV90.step1Validation();
            });

            $("#step2Btn").on("click", function () {
                VolvoV90.step2Validation();
            });                     

            $("#step3Btn").on("click", function () {
                VolvoV90.step3Validation();
            });
            $("#onlinePay").on("click", function () {
                VolvoV90.inquiryInfo.PaymentType = 4;                
                VolvoV90.apiCall.leadPush(false);
                $('input[name=PaymentType]').val(4);                
            });

            $("#chequePay").on("click", function () {
                VolvoV90.inquiryInfo.PaymentType = 2;                
                $('input[name=PaymentType]').val(2);
                if ($.trim($("#address").val()) != "") {
                    $("#address").siblings().removeClass('error-text');
                    $("#address").removeClass('border-red');
                    VolvoV90.customerInfo.Address = $("#address").val();
                    $.when(VolvoV90.apiCall.leadPush(false)).done(function () {
                        return true;
                    });
                }
                else {
                    $("#address").siblings().addClass('error-text');
                    $("#address").addClass('border-red');
                    return false;
                }
            });

            $(".booking__status").on("click", "li.active", function () {
                var id = $(this).attr('data-index');
                $(".screen").addClass("hide");
                $("#step" + id).removeClass("hide");
                $(".booking__status").removeClass(VolvoV90.lastStatus).addClass(VolvoV90.statusObj[id]);
                VolvoV90.lastStatus = VolvoV90.statusObj[id];
            });
            $('#step2').on("click", ".booking__version-details .custom-radio-list", function (event) {
                $(".booking__version-details").attr("data-validate", true);
                $(".booking__version-details .error-text").empty();
                $(".booking__exterior-container").removeClass("hide");
                $(this).closest(".custom-radio-list").find("input[type='radio']").attr("checked", true);
            });

            $("#step2").on("click", ".booking__exterior", function () {
                $(".booking__exterior-container").attr("data-validate", true);
                $(".booking__exterior-container .error-text").empty();
                $(".booking__interior-container").removeClass("hide");
                $(".booking__exterior").removeClass("active");
                $(this).addClass("active");
            });

            $("#step2").on("click", ".booking__interior", function () {
                $(".booking__interior-container").attr("data-validate", true);
                $(".booking__interior-container .error-text").empty();
                $(".booking__interior").removeClass("active");
                $(this).addClass("active");
            });
            $('#step3').on("click", ".booking__address-details .custom-radio-list", function (event) {
                $(this).closest(".custom-radio-list").find("input[type='radio']").attr("checked", true);
            });
        },        

        reasonEvents: function () {
            $(document).on("click", ".state-tabs", function () {
                $(".white-patch").removeClass("hide");
                var setTime = setTimeout(function () {
                    VolvoV90.lazyImg.callEventLazy();
                    clearTimeout(setTime);
                }, 50);
            });
            $(".cross-icon").on("click", function () {
                $(".state-tabs").removeClass("active");
                $(".white-patch").addClass("hide");
                $(".cw-tabs-data").css({ "display": "none" })
            });
        },
        galleryEvents: function () {
            $(".gallery__li").on("click", function () {
                if (!$(this).hasClass('li__type-video')) {
                    var imgSrc = $(this).find("img").attr("src"),
                        imgSrcArr = imgSrc.split('-sm'),
                        largeImgSrc = imgSrcArr[0] + '-lg' + imgSrcArr[1];
                   
                    VolvoV90.galleryPopup.show();
                    $(".gallery__popup img").attr("src", largeImgSrc);
                    VolvoV90.galleryPopup.find(".video-iframe-wrapper").hide();
                }
                else {
                    var videoSource = $(this).find("img").attr("data-video");
                    videoSource = videoSource + "&autoplay=1 ";
                    VolvoV90.galleryPopup.show();
                    $('#iframe-video').attr('src', videoSource);
                    VolvoV90.galleryPopup.find(".video-iframe-wrapper").show();
                    player.playVideo();
                }
            });
            $(".gallery__cross").on("click", function () {
                VolvoV90.galleryPopup.hide();
                $(".gallery__popup img").attr("src", '');
                $("#iframe-video").attr("src", "");
                player.stopVideo();
            });
        }
    },

    pageLoad: function () {
        VolvoV90.responsive.initAutocomplete();
        VolvoV90.responsive.bookingEvents();
        VolvoV90.responsive.galleryEvents();
        VolvoV90.responsive.reasonEvents();
        VolvoV90.lazyImg.callEventLazy();
        VolvoV90.lazyImg.applyLazyLoad();
        VolvoV90.apiCall.getVersionColorsData(1084);
        VolvoV90.prefillCity();
        VolvoV90.apiCall.bindDealerCities(VolvoV90.prefillDealerData);
        VolvoV90.pageLoadRegisterEvents();
    },

    pageLoadRegisterEvents: function () {
        $("body").removeClass("rsz-lyt");
        windowWidth = $(window).width();

        $("#esLeadFormSubmit").attr('data-label', 'submit').addClass("click_track");
        $("#es-leadform").append('<div class="margin-top15">Or Call us on <a href="tel:18002090230" class="toll-free-number text-grey">1800-2090-230</a></div>');
        $($("#es-leadform").children()[2]).hide();
        $($("#es-leadform").children()[0]).text("Please share your contact details");
        $("#personEmail").parent().hide();

        VolvoV90.customerInfo.CustomerId = -1;

        $("#es-thankyou h2").html("Thank You for sharing your details. Our representative will call you back shortly.");
        dataLayer.push({
            event: 'CWNonInteractive',
            cat: 'volvo-XC40-micro-site',
            act: VolvoBookingEngineCategory,
            lab: 'volvo-xc40-micro-site_shown'
        });
        $(".ask-expert").addClass("hide");


        $('#drpDealerCity').live('change', function () {
            if (!VolvoV90.firstTimeLoad) {
                var cityId = $('#drpDealerCity').val();
                $.when(VolvoV90.apiCall.getDealersByCity(cityId)).done(function (NCD_RESPONSE) {
                    VolvoV90.koViewModel.LocateDealerList([]);
                    $.each(NCD_RESPONSE.newCarDealers, function (key, value) {
                        if (isLatLongValid(value.latitude, value.longitude)) {
                            VolvoV90.koViewModel.LocateDealerList.push(value);
                        }
                    });
                    VolvoV90.apiCall.callToMapApi();
                })
            }
        });
        $('#drpFormDealerCity').live('change', function () {
            if (!VolvoV90.firstTimeLoad) {
                VolvoV90.inquiryInfo.DealerId = 0;
                var cityId = $('#drpFormDealerCity').val();
                $.when(VolvoV90.apiCall.getDealersByCity(cityId)).done(function (NCD_RESPONSE) {
                    VolvoV90.koViewModel.DealerInfoList([]);
                    $.each(NCD_RESPONSE.newCarDealers, function (key, value) {
                        if (isLatLongValid(value.latitude, value.longitude)) {
                            VolvoV90.koViewModel.DealerInfoList.push(value);
                        }
                    });
                })
            }
        });

        $('.map-dealer-list-card').live('click', function () {
            if ($(this).find('h4 i.icon-view').hasClass('uncheck')) {
                showBasicDealerDetails($(this).attr('id'));
                $('h4 i.icon-view').removeClass('fa-check-square-o').addClass('uncheck');
                $(this).find('h4 i.icon-view').removeClass('uncheck').addClass('fa-check-square-o');
            } else {
                $(this).find('h4 i.icon-view').removeClass('fa-check-square-o').addClass('uncheck');
                hideInfoWindow();
            }
        });
        ko.applyBindings(VolvoV90.koViewModel, $('.ameo-main')[0]);
        $("#setPlatform").val(isApp);
    },

    variantClick: function (data) {
        if (VolvoV90.prevVersionId != data.version.id) {
            $("#bookingSummary").text("Booking Summary");
            VolvoV90.koViewModel.exteriorColorData(data.exteriorColor);
            VolvoV90.koViewModel.summaryData(data.version.transmission);
            VolvoV90.koViewModel.extColorName("");
            VolvoV90.koViewModel.intColorName("");
            $(".booking__exterior-container, .booking__interior-container").attr("data-validate", false);
            VolvoV90.prevVersionId = data.version.id;
            VolvoV90.inquiryInfo.VersionId = data.version.id;
            $(".booking__interior-container").addClass("hide"),
            $('input[name=VersionId]').val(data.version.id);
            $("#init-text").hide();
        }
    },

    extColorClick: function (data) {
        if (VolvoV90.prevExtColorId != data.id) {
            VolvoV90.koViewModel.interiorColorData(data.interiorColor);
            VolvoV90.koViewModel.extColorName(data.name);
            $('#step2').find(".booking__interior").removeClass('Active');
            $(".booking__interior-container").attr("data-validate", false);
            VolvoV90.koViewModel.intColorName("");
            var imageUrl = data.hostUrl + data.originalImgPath;
            VolvoV90.koViewModel.imageSrc(imageUrl);
            VolvoV90.prevExtColorId = data.id;
            VolvoV90.inquiryInfo.ExteriorColorId = data.id;
            $('input[name=ExteriorColorId]').val(data.id);
        }
    },

    intColorClick: function (data) {
        VolvoV90.koViewModel.intColorName(data.name);
        VolvoV90.inquiryInfo.InteriorColorId = data.id;
        $('input[name=InteriorColorId]').val(data.id);
    },

    dealerClick: function(data) {
        VolvoV90.koViewModel.dealerName(data.name + ", " + data.cityName);
        VolvoV90.inquiryInfo.DealerId = data.dealerId;
        $('input[name=DealerId]').val(data.dealerId);
    },    

    step1Validation: function (event) {
        var nameEle = $("#booking__personName");
        var mobileEle = $("#booking__personMob");
        var emailEle = $("#booking__personEmail");
        var cityEle = $("#booking__personCity");
        var err = form.validation.contact(nameEle.val(), emailEle.val(), mobileEle.val());

        if (VolvoV90.processErrorResults(err, nameEle, emailEle, mobileEle, cityEle)) {
            $(".screen").addClass("hide");
            $("#step2").removeClass("hide");
            $(".booking__status").removeClass(VolvoV90.lastStatus).addClass("second");
            $(".booking__status li:nth-child(2)").addClass("active");
            
            VolvoV90.apiCall.leadPush(true);
            
            VolvoV90.setPGCookie();
            VolvoV90.lastStatus = "second";
            VolvoV90.scrollToBooking();
        }
    },
    step2Validation: function (event) {
        var q1 = $(".booking__version-details").attr("data-validate");
        var q2 = $(".booking__exterior-container").attr("data-validate");
        var q3 = $(".booking__interior-container").attr("data-validate");
        if (q1 == undefined || q1 == "false") {
            $(".booking__version-details .error-text").text("Please select one option");
        } else {
            if (q2 == undefined || q2 == "false") {
                $(".booking__exterior-container .error-text").text("Please select one option");
            } else {
                if (q3 == undefined || q3 == "false") {
                    $(".booking__interior-container .error-text").text("Please select one option");
                } else {
                    $(".screen").addClass("hide");
                    $("#step3").removeClass("hide");
                    $(".booking__status").removeClass(VolvoV90.lastStatus).addClass("third");
                    $(".booking__status li:nth-child(3)").addClass("active");
                    VolvoV90.lastStatus = "third";
                    VolvoV90.scrollToBooking();
                }
            }
        }
    },
    step3Validation: function (event) {
        if (VolvoV90.inquiryInfo.DealerId != undefined && VolvoV90.inquiryInfo.DealerId > 0) {
            $('#step3 .error-text').text("");
            $(".screen").addClass("hide");
            $('#dealer').removeClass('hide');
            $("#step4").removeClass("hide");
            $("#bookingAmt").show();
            $(".booking__status").removeClass(VolvoV90.lastStatus).addClass("fourth");
            $(".booking__status li:nth-child(4)").addClass("active");
            $('#dealer').text()
            VolvoV90.lastStatus = "fourth";
            VolvoV90.scrollToBooking();
        }
        else {
            $('#step3 .error-text').text("Please select dealer.");
        }
    },
    scrollToBooking: function () {
        $('html, body').animate({
            scrollTop: $(".booking_head").offset().top
        }, 10);
    },
    lazyImg: {

        callEventLazy: function () {
            $("img.lazy").lazyload();
            $("div.lazy").lazyload();
            $(window).load(function () {
                VolvoV90.lazyImg.applyLazyLoad();
            });
        },

        applyLazyLoad: function () {
            $("img.lazy").lazyload({
                event: "imgLazyLoad",
                effect: "fadeIn"
            });
        }
    },
    processErrorResults: function (err, nameEle, emailEle, mobileEle, cityEle) {
        var isFormValid = true;
        if (err && err.length > 2) {

            if (err[0] == "") {
                nameEle.siblings().addClass('hide');
                nameEle.removeClass('border-red');
            }
            else {
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
                mobileEle.siblings('.cw-blackbg-tooltip, .error-icon').removeClass('hide');
                mobileEle.addClass('border-red');
                var errSpan = mobileEle.siblings()[2];
                $(errSpan).text(err[2]);
                isFormValid = false;
            }
        }
        if (!VolvoV90.validateCity(cityEle)) {
            isFormValid = false;
        }
        return isFormValid;
    },
    validateCity: function (targetId) {

        var cityVal = Common.utils.getSplitCityName(targetId.val());
        if (cityVal == $.cookie("_CustCityMaster") && typeof (VolvoV90.bookingLeadCity) != "undefined" && Number(VolvoV90.bookingLeadCity.id) > 0 && VolvoV90.bookingLeadCity.id == $.cookie("_CustCityIdMaster")) {
            if (isMobile) {
                VolvoV90.showHideMatchError(false, $(targetId).closest('.easy-autocomplete'));
            }
            else {
                VolvoV90.showHideMatchError(false, targetId);
            }
            return true;
        }
        else if (cityVal == "" || targetId.hasClass('border-red') ||
                    (
                        ($('li.ui-menu-item a:visible:eq(0)').text() != cityVal && cityVal != "") &&
                        (typeof (VolvoV90.bookingLeadCity) == "undefined" || typeof (VolvoV90.bookingLeadCity.name) == "undefined" || VolvoV90.bookingLeadCity.name.toLowerCase() != cityVal.toLowerCase())
                    )
               ) {
            if (isMobile) {
                VolvoV90.showHideMatchError(true, $(targetId).closest('.easy-autocomplete'), "Please Enter City");
            }
            else {
                VolvoV90.showHideMatchError(true, targetId, "Please Enter City");
            }
            return false;
        }
        return true;
    },
    showHideMatchError: function (error, TargetId, errText) {
        if (error) {
            TargetId.siblings('.error-icon').removeClass('hide');
            TargetId.siblings('.cw-blackbg-tooltip').removeClass('hide').text(errText);
            TargetId.addClass('border-red');
        }
        else {
            TargetId.siblings().addClass('hide');
            TargetId.removeClass('border-red');
        }
    },
    isScrolledIntoView: function (elem) {
        var docViewTop = $(window).scrollTop();
        var docViewBottom = docViewTop + $(window).height();

        var elemTop = $(elem).offset().top;
        var elemBottom = elemTop + $(elem).height();

        return ((elemBottom <= docViewBottom) && (elemTop >= docViewTop));
    },
    getInput: function () {
        VolvoV90.customerInfo.BasicInfo = {},
        VolvoV90.customerInfo.BasicInfo.Name = $("#booking__personName").val();
        VolvoV90.customerInfo.BasicInfo.Email = $("#booking__personEmail").val();
        VolvoV90.customerInfo.BasicInfo.Mobile = $("#booking__personMob").val();
        VolvoV90.customerInfo.CityId = VolvoV90.bookingLeadCity.id;
        VolvoV90.customerInfo.Platform = isMobile ? 43 : 1;
        $('input[name=PlatformId]').val(VolvoV90.customerInfo.Platform);
    },
    getInquiryData: function() {
        VolvoV90.inquiryInfo.Id = -1;
        VolvoV90.inquiryInfo.CustomerId = VolvoV90.customerInfo.CustomerId;
        VolvoV90.inquiryInfo.BookingAmount = 1;
        VolvoV90.inquiryInfo.PlatformId = VolvoV90.customerInfo.Platform;
    },
    apiCall: {
        leadPush: function (isAsync) {
            VolvoV90.getInput();
            $.ajax({
                type: "POST",
                url: '/api/escustomer/',
                data: VolvoV90.customerInfo,
                dataType: 'Json',
                async: isAsync,
                success: function (response) {
                    if (response != null) {
                        VolvoV90.customerInfo.CustomerId = response;
                        $('input[name=CustomerId]').val(VolvoV90.customerInfo.CustomerId);
                    }
                }
            })
        },
        bindDealerCities: function (callback) {
            $('#drpDealerCity').empty();
            var dealerCities = [];
            $.ajax({
                type: 'GET',
                url: '/webapi/NewCarDealers/cities/?makeId=37',
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
                        $.each(dealerCities, function (key, value) {
                            VolvoV90.koViewModel.cities.push(value);
                        });
                        if (callback)
                            callback(true);
                    }
                }
            });
        },
        getDealersByCity: function (cityId) {
            return Common.utils.ajaxCall({
                type: 'GET',
                url: '/webapi/NewCarDealers/showrooms/?makeId=37&cityId=' + cityId,
                dataType: 'Json',
            });            
        },        


        getVersionColorsData: function (modelId) {
            Common.utils.ajaxCall({
                type: 'GET',
                url: '/api/booking/volvo/?modelId=' + modelId,    // volvo modelid
                contentType: "application/x-www-form-urlencoded",
                dataType: 'Json',
            }).done(function (data) {
                if (data != null) {
                    VolvoV90.koViewModel.versionData(data);     
                }
            });
        },        

        callToMapApi: function () {
            plotGoogleMap(VolvoV90.koViewModel.LocateDealerList(), "divMap", true);
        }
    },
    setPGCookie: function () {
        document.cookie = '_CustEmail=' + $("#booking__personEmail").val() + '; expires = ' + permanentCookieTime() + "; domain=" + defaultCookieDomain + '; path =/';
        document.cookie = '_CustomerName=' + $("#booking__personName").val() + '; expires = ' + permanentCookieTime() + "; domain=" + defaultCookieDomain + '; path =/';
        document.cookie = '_CustMobile=' + $("#booking__personMob").val() + '; expires = ' + permanentCookieTime() + "; domain=" + defaultCookieDomain + '; path =/';        
    },
    koViewModel: {
        versionData: ko.observableArray([]),
        exteriorColorData: ko.observableArray([]),
        interiorColorData: ko.observableArray([]),
        summaryData: ko.observable(),
        extColorName: ko.observable(),
        intColorName: ko.observable(),
        LocateDealerList: ko.observableArray(),
        DealerInfoList: ko.observableArray(),
        cities: ko.observableArray(),
        dealerName: ko.observable(),
        imageSrc: ko.observable()
    },
    prefillDealerData: function (response) {
        if (response) {
            $('#drpDealerCity').val(1);
            $('#drpFormDealerCity').val(1)
            try {
                $.when(VolvoV90.apiCall.getDealersByCity(1)).done(function (NCD_RESPONSE) {
                    $.each(NCD_RESPONSE.newCarDealers, function (key, value) {
                        if (isLatLongValid(value.latitude, value.longitude)) {
                            VolvoV90.koViewModel.LocateDealerList.push(value);
                            VolvoV90.koViewModel.DealerInfoList.push(value);
                        }
                    });
                    VolvoV90.apiCall.callToMapApi();
                })

            }
            catch (e) { console.log(e) };
            VolvoV90.firstTimeLoad = false;
        }
    },
    
    prefillCity: function () {
        if (Number(masterCityIdCookie) > 0 && masterCityNameCookie != null && $.trim(masterCityNameCookie) != "" && $.trim(masterCityNameCookie) != "Select City") {
            $('#booking__personCity').val(masterCityNameCookie);
            VolvoV90.bookingLeadCity.name = masterCityNameCookie;
            VolvoV90.bookingLeadCity.id = masterCityIdCookie;
        }
        else if (isCookieExists('_CustCity') && $.cookie('_CustCity') != null && $.trim($.cookie('_CustCity')) != "")
        {
            $('#booking__personCity').val($.cookie('_CustCity'));
            VolvoV90.bookingLeadCity.name = $.cookie('_CustCity');
            VolvoV90.bookingLeadCity.id = $.cookie('_CustCityId');
        }
    },

}

var windowWidth, resizeId;

$(document).ready(function () {
	VolvoV90.pageLoad();
	impressionTracking(VolvoBookingEngineCategory);
});
$(document).keyup(function (e) {
    if (e.keyCode == 27) { 
        Common.utils.unlockPopup();
        $(".address-popup").addClass("hide");
        
    }
});

var VolvoBookingEngineCategory;
if (isMobile != null && isMobile) {
	VolvoBookingEngineCategory = 'volvo-xc40-micro-site_m'
}
else {
	VolvoBookingEngineCategory = 'volvo-xc40-micro-site_d'
}