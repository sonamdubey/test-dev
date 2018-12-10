var VolvoXc40 = {
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

                        VolvoXc40.bookingLeadCity.name = Common.utils.getSplitCityName(selectionLabel);
                        VolvoXc40.bookingLeadCity.id = selectionValue;
                        $(personCityInputField).val(VolvoXc40.bookingLeadCity.name);
                        },

                                afterFetch: function (result, searchText) {
                        VolvoXc40.bookingLeadCity.result = result;

                        if (typeof result == "undefined" || result.length <= 0) {
                            VolvoXc40.showHideMatchError(true, $(personCityInputField).closest('.easy-autocomplete'), "No city Match");
                        }
                        else {
                            VolvoXc40.showHideMatchError(false, $(personCityInputField).closest('.easy-autocomplete'));
                        }
                    }
                    });
            }
            else {
                $("#booking__personCity").cw_autocomplete({
                    resultCount: 5,
                    source: ac_Source.allCarCities,
                    click: function (event, ui, orgTxt) {
                        VolvoXc40.bookingLeadCity.name = Common.utils.getSplitCityName(ui.item.label);
                        VolvoXc40.bookingLeadCity.id = ui.item.id;
                        ui.item.value = VolvoXc40.bookingLeadCity.name;
                    },
                    open: function (result) {
                        VolvoXc40.bookingLeadCity.result = result;
                    },
                    afterfetch: function (result, searchtext) {
                        this.result = result;
                        if (typeof result == "undefined" || result.length <= 0)
                            VolvoXc40.showHideMatchError(true, $('#booking__personCity'), "No city Match");
                        else
                            VolvoXc40.showHideMatchError(false, $('#booking__personCity'));
                    }
                });
            }
        },
        bookingEvents: function () {

            $("#ad-container__startBooking").on("click", function () {
                VolvoXc40.scrollToBooking();
                $("#startBooking").trigger("click");
            });

            $("#startBooking").on("click", function () {
                $('input[name=VersionId]').trigger("click");
                $(".screen").addClass("hide");
                $("#step1, .booking__car-info").removeClass("hide");
                $(".booking__status").removeClass(VolvoXc40.lastStatus + " hide").addClass("first");
                $(".booking__status li:nth-child(1)").addClass("active");
                VolvoXc40.lastStatus = "first";
                VolvoXc40.lazyImg.applyLazyLoad();
                VolvoXc40.koViewModel.imageSrc("https://imgd.aeplcdn.com/0x0/cw/es/volvo/v90/car-brightsilver.jpg");   // bind default image
            });

            $("#step1Btn").on("click", function () {
                VolvoXc40.step1Validation();
            });

            $("#step2Btn").on("click", function () {
                VolvoXc40.step2Validation();
            });                     

            $("#step3Btn").on("click", function () {
                VolvoXc40.step3Validation();
            });
            $("#onlinePay").on("click", function () {
                VolvoXc40.inquiryInfo.PaymentType = 4;                
                VolvoXc40.apiCall.leadPush(false);
                $('input[name=PaymentType]').val(4);                
            });

            $("#chequePay").on("click", function () {
                VolvoXc40.inquiryInfo.PaymentType = 2;                
                $('input[name=PaymentType]').val(2);
                if ($.trim($("#address").val()) != "") {
                    $("#address").siblings().removeClass('error-text');
                    $("#address").removeClass('border-red');
                    VolvoXc40.customerInfo.Address = $("#address").val();
                    $.when(VolvoXc40.apiCall.leadPush(false)).done(function () {
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
                $(".booking__status").removeClass(VolvoXc40.lastStatus).addClass(VolvoXc40.statusObj[id]);
                VolvoXc40.lastStatus = VolvoXc40.statusObj[id];
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
                    VolvoXc40.lazyImg.callEventLazy();
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
                   
                    VolvoXc40.galleryPopup.show();
                    $(".gallery__popup img").attr("src", largeImgSrc);
                    VolvoXc40.galleryPopup.find(".video-iframe-wrapper").hide();
                }
                else {
                    var videoSource = $(this).find("img").attr("data-video");
                    videoSource = videoSource + "&autoplay=1 ";
                    VolvoXc40.galleryPopup.show();
                    $('#iframe-video').attr('src', videoSource);
                    VolvoXc40.galleryPopup.find(".video-iframe-wrapper").show();
                    player.playVideo();
                }
            });
            $(".gallery__cross").on("click", function () {
                VolvoXc40.galleryPopup.hide();
                $(".gallery__popup img").attr("src", '');
                $("#iframe-video").attr("src", "");
                player.stopVideo();
            });
        }
    },

    pageLoad: function () {
        VolvoXc40.responsive.initAutocomplete();
        VolvoXc40.responsive.bookingEvents();
        VolvoXc40.responsive.galleryEvents();
        VolvoXc40.responsive.reasonEvents();
        VolvoXc40.lazyImg.callEventLazy();
        VolvoXc40.lazyImg.applyLazyLoad();
        VolvoXc40.apiCall.getVersionColorsData(1084);
        VolvoXc40.prefillCity();
        VolvoXc40.apiCall.bindDealerCities(VolvoXc40.prefillDealerData);
        VolvoXc40.pageLoadRegisterEvents();
    },

    pageLoadRegisterEvents: function () {
        $("body").removeClass("rsz-lyt");
        windowWidth = $(window).width();

        $("#esLeadFormSubmit").attr('data-label', 'submit').addClass("click_track");
        $("#es-leadform").append('<div class="margin-top15">Or Call us on <a href="tel:18002090230" class="text-green font16 text-bold toll-free-number" title="Toll Free">1800-2090-230</a></div>');
        $($("#es-leadform").children()[2]).hide();
        $($("#es-leadform").children()[0]).text("Please share your contact details");
        $("#personEmail").parent().hide();

        VolvoXc40.customerInfo.CustomerId = -1;

        $("#es-thankyou h2").html("Thank You for sharing your details. Our representative will call you back shortly.");
        dataLayer.push({
            event: 'CWNonInteractive',
            cat: 'volvo-XC40-micro-site',
            act: VolvoBookingEngineCategory,
            lab: 'volvo-xc40-micro-site_shown'
        });
        $(".ask-expert").addClass("hide");


        $('#drpDealerCity').live('change', function () {
            if (!VolvoXc40.firstTimeLoad) {
                var cityId = $('#drpDealerCity').val();
                $.when(VolvoXc40.apiCall.getDealersByCity(cityId)).done(function (NCD_RESPONSE) {
                    VolvoXc40.koViewModel.LocateDealerList([]);
                    $.each(NCD_RESPONSE.newCarDealers, function (key, value) {
                        if (isLatLongValid(value.latitude, value.longitude)) {
                            VolvoXc40.koViewModel.LocateDealerList.push(value);
                        }
                    });
                    VolvoXc40.apiCall.callToMapApi();
                })
            }
        });
        $('#drpFormDealerCity').live('change', function () {
            if (!VolvoXc40.firstTimeLoad) {
                VolvoXc40.inquiryInfo.DealerId = 0;
                var cityId = $('#drpFormDealerCity').val();
                $.when(VolvoXc40.apiCall.getDealersByCity(cityId)).done(function (NCD_RESPONSE) {
                    VolvoXc40.koViewModel.DealerInfoList([]);
                    $.each(NCD_RESPONSE.newCarDealers, function (key, value) {
                        if (isLatLongValid(value.latitude, value.longitude)) {
                            VolvoXc40.koViewModel.DealerInfoList.push(value);
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
        ko.applyBindings(VolvoXc40.koViewModel, $('.ameo-main')[0]);
        $("#setPlatform").val(isApp);
    },

    variantClick: function (data) {
        if (VolvoXc40.prevVersionId != data.version.id) {
            $("#bookingSummary").text("Booking Summary");
            VolvoXc40.koViewModel.exteriorColorData(data.exteriorColor);
            VolvoXc40.koViewModel.summaryData(data.version.transmission);
            VolvoXc40.koViewModel.extColorName("");
            VolvoXc40.koViewModel.intColorName("");
            $(".booking__exterior-container, .booking__interior-container").attr("data-validate", false);
            VolvoXc40.prevVersionId = data.version.id;
            VolvoXc40.inquiryInfo.VersionId = data.version.id;
            $(".booking__interior-container").addClass("hide"),
            $('input[name=VersionId]').val(data.version.id);
            $("#init-text").hide();
        }
    },

    extColorClick: function (data) {
        if (VolvoXc40.prevExtColorId != data.id) {
            VolvoXc40.koViewModel.interiorColorData(data.interiorColor);
            VolvoXc40.koViewModel.extColorName(data.name);
            $('#step2').find(".booking__interior").removeClass('Active');
            $(".booking__interior-container").attr("data-validate", false);
            VolvoXc40.koViewModel.intColorName("");
            var imageUrl = data.hostUrl + data.originalImgPath;
            VolvoXc40.koViewModel.imageSrc(imageUrl);
            VolvoXc40.prevExtColorId = data.id;
            VolvoXc40.inquiryInfo.ExteriorColorId = data.id;
            $('input[name=ExteriorColorId]').val(data.id);
        }
    },

    intColorClick: function (data) {
        VolvoXc40.koViewModel.intColorName(data.name);
        VolvoXc40.inquiryInfo.InteriorColorId = data.id;
        $('input[name=InteriorColorId]').val(data.id);
    },

    dealerClick: function(data) {
        VolvoXc40.koViewModel.dealerName(data.name + ", " + data.cityName);
        VolvoXc40.inquiryInfo.DealerId = data.dealerId;
        $('input[name=DealerId]').val(data.dealerId);
    },    

    step1Validation: function (event) {
        var nameEle = $("#booking__personName");
        var mobileEle = $("#booking__personMob");
        var emailEle = $("#booking__personEmail");
        var cityEle = $("#booking__personCity");
        var err = form.validation.contact(nameEle.val(), emailEle.val(), mobileEle.val());

        if (VolvoXc40.processErrorResults(err, nameEle, emailEle, mobileEle, cityEle)) {
            $(".screen").addClass("hide");
            $("#step2").removeClass("hide");
            $(".booking__status").removeClass(VolvoXc40.lastStatus).addClass("second");
            $(".booking__status li:nth-child(2)").addClass("active");
            
            VolvoXc40.apiCall.leadPush(true);
            
            VolvoXc40.setPGCookie();
            VolvoXc40.lastStatus = "second";
            VolvoXc40.scrollToBooking();
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
                    $(".booking__status").removeClass(VolvoXc40.lastStatus).addClass("third");
                    $(".booking__status li:nth-child(3)").addClass("active");
                    VolvoXc40.lastStatus = "third";
                    VolvoXc40.scrollToBooking();
                }
            }
        }
    },
    step3Validation: function (event) {
        if (VolvoXc40.inquiryInfo.DealerId != undefined && VolvoXc40.inquiryInfo.DealerId > 0) {
            $('#step3 .error-text').text("");
            $(".screen").addClass("hide");
            $('#dealer').removeClass('hide');
            $("#step4").removeClass("hide");
            $("#bookingAmt").show();
            $(".booking__status").removeClass(VolvoXc40.lastStatus).addClass("fourth");
            $(".booking__status li:nth-child(4)").addClass("active");
            $('#dealer').text()
            VolvoXc40.lastStatus = "fourth";
            VolvoXc40.scrollToBooking();
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
                VolvoXc40.lazyImg.applyLazyLoad();
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
        if (!VolvoXc40.validateCity(cityEle)) {
            isFormValid = false;
        }
        return isFormValid;
    },
    validateCity: function (targetId) {

        var cityVal = Common.utils.getSplitCityName(targetId.val());
        if (cityVal == $.cookie("_CustCityMaster") && typeof (VolvoXc40.bookingLeadCity) != "undefined" && Number(VolvoXc40.bookingLeadCity.id) > 0 && VolvoXc40.bookingLeadCity.id == $.cookie("_CustCityIdMaster")) {
            if (isMobile) {
                VolvoXc40.showHideMatchError(false, $(targetId).closest('.easy-autocomplete'));
            }
            else {
                VolvoXc40.showHideMatchError(false, targetId);
            }
            return true;
        }
        else if (cityVal == "" || targetId.hasClass('border-red') ||
                    (
                        ($('li.ui-menu-item a:visible:eq(0)').text() != cityVal && cityVal != "") &&
                        (typeof (VolvoXc40.bookingLeadCity) == "undefined" || typeof (VolvoXc40.bookingLeadCity.name) == "undefined" || VolvoXc40.bookingLeadCity.name.toLowerCase() != cityVal.toLowerCase())
                    )
               ) {
            if (isMobile) {
                VolvoXc40.showHideMatchError(true, $(targetId).closest('.easy-autocomplete'), "Please Enter City");
            }
            else {
                VolvoXc40.showHideMatchError(true, targetId, "Please Enter City");
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
        VolvoXc40.customerInfo.BasicInfo = {},
        VolvoXc40.customerInfo.BasicInfo.Name = $("#booking__personName").val();
        VolvoXc40.customerInfo.BasicInfo.Email = $("#booking__personEmail").val();
        VolvoXc40.customerInfo.BasicInfo.Mobile = $("#booking__personMob").val();
        VolvoXc40.customerInfo.CityId = VolvoXc40.bookingLeadCity.id;
        VolvoXc40.customerInfo.Platform = isMobile ? 43 : 1;
        $('input[name=PlatformId]').val(VolvoXc40.customerInfo.Platform);
    },
    getInquiryData: function() {
        VolvoXc40.inquiryInfo.Id = -1;
        VolvoXc40.inquiryInfo.CustomerId = VolvoXc40.customerInfo.CustomerId;
        VolvoXc40.inquiryInfo.BookingAmount = 1;
        VolvoXc40.inquiryInfo.PlatformId = VolvoXc40.customerInfo.Platform;
    },
    apiCall: {
        leadPush: function (isAsync) {
            VolvoXc40.getInput();
            $.ajax({
                type: "POST",
                url: '/api/escustomer/',
                data: VolvoXc40.customerInfo,
                dataType: 'Json',
                async: isAsync,
                success: function (response) {
                    if (response != null) {
                        VolvoXc40.customerInfo.CustomerId = response;
                        $('input[name=CustomerId]').val(VolvoXc40.customerInfo.CustomerId);
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
                            VolvoXc40.koViewModel.cities.push(value);
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
                    VolvoXc40.koViewModel.versionData(data);     
                }
            });
        },        

        callToMapApi: function () {
            plotGoogleMap(VolvoXc40.koViewModel.LocateDealerList(), "divMap", true);
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
                $.when(VolvoXc40.apiCall.getDealersByCity(1)).done(function (NCD_RESPONSE) {
                    $.each(NCD_RESPONSE.newCarDealers, function (key, value) {
                        if (isLatLongValid(value.latitude, value.longitude)) {
                            VolvoXc40.koViewModel.LocateDealerList.push(value);
                            VolvoXc40.koViewModel.DealerInfoList.push(value);
                        }
                    });
                    VolvoXc40.apiCall.callToMapApi();
                })

            }
            catch (e) { console.log(e) };
            VolvoXc40.firstTimeLoad = false;
        }
    },
    
    prefillCity: function () {
        if (Number(masterCityIdCookie) > 0 && masterCityNameCookie != null && $.trim(masterCityNameCookie) != "" && $.trim(masterCityNameCookie) != "Select City") {
            $('#booking__personCity').val(masterCityNameCookie);
            VolvoXc40.bookingLeadCity.name = masterCityNameCookie;
            VolvoXc40.bookingLeadCity.id = masterCityIdCookie;
        }
        else if (isCookieExists('_CustCity') && $.cookie('_CustCity') != null && $.trim($.cookie('_CustCity')) != "")
        {
            $('#booking__personCity').val($.cookie('_CustCity'));
            VolvoXc40.bookingLeadCity.name = $.cookie('_CustCity');
            VolvoXc40.bookingLeadCity.id = $.cookie('_CustCityId');
        }
    },

}

var windowWidth, resizeId;

$(document).ready(function () {
	VolvoXc40.pageLoad();
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