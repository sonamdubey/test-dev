$(document).ready(function () {

    dataLayer.push({
        event: 'CWNonInteractive',
        cat: 'volvo_s60',
        act: 'volvo_s60_Shown'
    });

    $(".click_track").click(function(){	
        label=$(this).attr('data-label');
        action = $(this).attr('data-action');
        dataLayer.push({
            event:'CWInteractive',
            cat:'volvo_s60',
            act: action + " " + label,
            lab: label
        });
    });

    $('.carousel-navigation-photos .swiper-slide').first().addClass('swiper-slide-active');
    Volvo.leadForm.registerEvent();
});

var slideToClick = function (swiper) {
    var clickedSlide = swiper.slides[swiper.clickedIndex];
    swiper.slides.removeClass('swiper-slide-active');
    $(clickedSlide).addClass('swiper-slide-active');
    galleryTop.slideTo(swiper.clickedIndex, 500);   
    window.dispatchEvent(new Event('resize'));
};

var galleryThumbs = $('.carousel-navigation-photos').swiper({
    slideActiveClass: '',
    spaceBetween: 0,
    slidesPerView: 'auto',
    slideToClickedSlide: true,
    preloadImages: false,
    lazyLoading: true,
    lazyLoadingInPrevNext: true,
    watchSlidesProgress: true,
    watchSlidesVisibility: true,
    nextButton: '.navigation-photos .swiper-button-next',
    prevButton: '.navigation-photos .swiper-button-prev',
    onTap: slideToClick
});

var slidegalleryThumbs = function (swiper) {
    galleryThumbs.slideTo(swiper.activeIndex, 500);
    galleryThumbs.slides.removeClass('swiper-slide-active');
    galleryThumbs.slides[swiper.activeIndex].className += ' swiper-slide-active';
};

var galleryTop = $('.carousel-stage-photos').swiper({
    nextButton: '.stage-photos .swiper-button-next',
    prevButton: '.stage-photos .swiper-button-prev',
    spaceBetween: 10,
    preloadImages: false,
    lazyLoading: true,
    lazyLoadingInPrevNext: true,
    watchSlidesProgress: true,
    watchSlidesVisibility: true,
    onSlideChangeEnd: slidegalleryThumbs
});



$('.stage-photos').on('click', '.swiper-slide', function () {
    if (!$('body').hasClass('gallery-active')) {
        gallery.open();
        appendState('popupGallery');
        window.dispatchEvent(new Event('resize'));
    }
});

$(window).resize(function () {
    if ($('body').hasClass('gallery-active')) {
        gallery.setPosition();
    }
});

$(window).on('orientationchange', function () {
    $('.connected-carousels-photos .carousel-photos', function () {
        $(this).css({ "height": "auto" });
    });
});

$('#gallery-close-btn').on('click', function () {
    gallery.close();
});

var Volvo = {
    personLeadCity: {},
    customerInfo: {},
    leadForm: {

        registerEvent: function () {
            $("#firstForm").click(function () {
                if (Volvo.leadForm.checkEmpty($("#power"), "Please answer this question")) {
                    Volvo.customerInfo.power = $.trim($("#power").val());
                    $(".first-form").addClass('hide');
                    $(".personal-form").removeClass('hide');
                }
            });
            $("#basicInfoSubmit").click(function () {
                Volvo.leadForm.basicInfo();                
            });

            $("#additionalInfoSubmit").click(function () {
                Volvo.leadForm.submitData();
            });

            $("#socialInfoSubmit").click(function () {
                Volvo.leadForm.submitSocial();
            });
            $("#reset").click(function () {
                Volvo.leadForm.reset();
            });
            
            if(typeof isMobileDevice == "undefined") {
                $("#personCity").cw_autocomplete({
                    resultCount: 5,
                    source: ac_Source.allCarCities,
                    click: function (event, ui, orgTxt) {
                        Volvo.personLeadCity.name = Common.utils.getSplitCityName(ui.item.label);
                        Volvo.personLeadCity.id = ui.item.id;
                        ui.item.value = Volvo.personLeadCity.name;
                    },
                    open: function (result) {
                        Volvo.personLeadCity.result = result;
                    },
                    focusout: function () {
                        var cityVal = $("#personCity").val();
                        if ((Common.utils.getSplitCityName($('li.ui-state-focus a:visible').text().toLowerCase()) == cityVal.toLowerCase() || $('li.ui-state-focus a:visible').text().toLowerCase() == cityVal.toLowerCase()) && typeof (this.result) == "object") {
                            var focused = this.result[$('li.ui-state-focus').index()];
                            if (Volvo.personLeadCity == undefined) Volvo.personLeadCity = new Object();
                            if (focused != undefined) {
                                Volvo.personLeadCity.name = Common.utils.getSplitCityName(focused.label);
                                Volvo.personLeadCity.id = focused.id;
                            }
                        }
                    },
                    afterfetch: function (result, searchtext) {
                        this.result = result;
                        if (typeof result == "undefined" || result.length <= 0)
                            Volvo.leadForm.showHideMatchError(true, $('#personCity'), "No city Match");
                        else
                            Volvo.leadForm.showHideMatchError(false, $('#personCity'));
                    }
                });
            }
            else {
                var personCityInputField = $("#personCity");

                $(personCityInputField).cw_easyAutocomplete({
                    inputField: personCityInputField,
                    resultCount: 5,
                    source: ac_Source.allCarCities,
                    click: function (event) {
                        var selectionValue = personCityInputField.getSelectedItemData().value,
                        selectionLabel = personCityInputField.getSelectedItemData().label;

                        Volvo.personLeadCity.name = Common.utils.getSplitCityName(selectionLabel);
                        Volvo.personLeadCity.id = selectionValue;
                        $(personCityInputField).val(Volvo.personLeadCity.name);
                    },

                    afterFetch: function (result, searchText) {
                        Volvo.personLeadCity.result = result;

                        if (typeof result == "undefined" || result.length <= 0) {
                            Volvo.leadForm.showHideMatchError(true, $(personCityInputField).closest('.easy-autocomplete'), "No city Match");
                        }
                        else {
                            Volvo.leadForm.showHideMatchError(false, $(personCityInputField).closest('.easy-autocomplete'));
                        }
                    }
                });
            }
        },
        basicInfo: function (event) {

            var nameEle = $("#personName");
            var emailEle = $("#personEmail");
            var mobileEle = $("#personMob");
            var cityEle = $("#personCity");
            var err = form.validation.contact(nameEle.val(), emailEle.val(), mobileEle.val());

            Volvo.customerInfo.LeadTypeId = 22;
            Volvo.customerInfo.Name = $.trim($("#personName").val());
            Volvo.customerInfo.Email = $.trim($("#personEmail").val());
            Volvo.customerInfo.MobileNo = $("#personMob").val();
            Volvo.customerInfo.CityId = Volvo.personLeadCity.id;

            if (Volvo.leadForm.processErrorResults(err, nameEle, emailEle, mobileEle, cityEle)) {
                $(".personal-form").addClass('hide');
                $(".additional-form").removeClass('hide');
                
                Volvo.apiCall.leadPush()
            }
        },
        submitData: function () {
            if (Volvo.leadForm.validateAdditionalInfo()) {
                Volvo.customerInfo.Age = ($("#age").val()).slice(0, 2);
                Volvo.customerInfo.Profession = $.trim($("#profession").val());
                Volvo.customerInfo.CurrentCar = $.trim($("#carOwned").val());
                Volvo.customerInfo.Licence = $('#licence').is(':checked');

                Volvo.apiCall.leadPush()
                $(".additional-form").addClass('hide');
                $(".social-form").removeClass('hide');
            }
        },
        submitSocial: function () {
            Volvo.customerInfo.Facebook = $("#facebook").val();
            Volvo.customerInfo.Twitter = $("#twitter").val();
            Volvo.customerInfo.LinkdIn = $("#linkdIn").val();
            Volvo.customerInfo.Instagram = $("#instagram").val();
            
            Volvo.apiCall.leadPush()
            $(".social-form").addClass('hide');
            $(".greeting-section").removeClass('hide');
        },
        validateAdditionalInfo: function () {
            var isValid = Volvo.leadForm.checkEmpty($("#age"), "Please enter your age");
            isValid = Volvo.leadForm.checkEmpty($("#profession"), "Please enter your profession") && isValid;
            isValid = Volvo.leadForm.checkEmpty($("#carOwned"), "Please enter your current car owned") && isValid;
            if ($('#licence').is(':checked')) {
                $('#licence').parent().removeClass('text-red');
                isValid = true;
            }
            else
            {
                $('#licence').parent().addClass('text-red')
                isValid = false;
            }
            return isValid;
        },
        checkEmpty: function(element, error){
            var isValid = true;
            if ($.trim(element.val()) == "") {
                element.siblings().removeClass('hide');
                element.addClass('border-red');
                var errSpan = element.siblings()[1];
                $(errSpan).text(error);
                isValid = false;
            }
            else
            {
                element.siblings().addClass('hide');
                element.removeClass('border-red');
            }
            return isValid;
        },
        validateCity: function (targetId) {

            var cityVal = Common.utils.getSplitCityName(targetId.val());
            if (cityVal == $.cookie("_CustCityMaster") && typeof (Volvo.personLeadCity) != "undefined" && Number(Volvo.personLeadCity.id) > 0 && Volvo.personLeadCity.id == $.cookie("_CustCityIdMaster")) {
                if(typeof isMobileDevice == "undefined") {
                    Volvo.leadForm.showHideMatchError(false, targetId);
                }
                else {
                    Volvo.leadForm.showHideMatchError(false, $(targetId).closest('.easy-autocomplete'));
                }
                return true;
            }
            else if (cityVal == "" || targetId.hasClass('border-red') ||
                        (
                            ($('li.ui-menu-item a:visible:eq(0)').text() != cityVal && cityVal != "") &&
                            (typeof (Volvo.personLeadCity) == "undefined" || typeof (Volvo.personLeadCity.name) == "undefined" || Volvo.personLeadCity.name.toLowerCase() != cityVal.toLowerCase())
                        )
                   ) {
                if(typeof isMobileDevice == "undefined") {
                    Volvo.leadForm.showHideMatchError(true, targetId);
                }
                else {
                    Volvo.leadForm.showHideMatchError(true, $(targetId).closest('.easy-autocomplete'), "Please Enter City");
                }
                return false;
            }
            return true;
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
                    mobileEle.siblings().removeClass('hide');
                    mobileEle.addClass('border-red');
                    var errSpan = mobileEle.siblings('.cw-blackbg-tooltip');
                    $(errSpan).text(err[2]);
                    isFormValid = false;
                }
            }
            if (!Volvo.leadForm.validateCity(cityEle)) {
                isFormValid = false;
            }
            return isFormValid;
        },
        showHideMatchError: function (error, TargetId, errText) {
            if (error) {
                TargetId.siblings().removeClass('hide');
                TargetId.addClass('border-red');
                var errSpan = TargetId.siblings()[1];
                $(errSpan).text(errText);
            }
            else {
                TargetId.siblings().addClass('hide');
                TargetId.removeClass('border-red');
            }
        },
        reset: function () {
            Volvo.customerInfo.Id = -1;
            $("#personName, #personEmail, #personMob, #personCity, #power, #age, #profession, #carOwned, #facebook, #twitter, #linkdIn, #instagram").val('');
            $('#licence').attr('checked', false);
            $(".greeting-section").addClass('hide');
            $(".first-form").removeClass('hide');
        },

        isScrolledIntoView: function (elem) {
            var docViewTop = $(window).scrollTop();
            var docViewBottom = docViewTop + $(window).height();

            var elemTop = $(elem).offset().top;
            var elemBottom = elemTop + $(elem).height();

            return ((elemBottom <= docViewBottom) && (elemTop >= docViewTop));
        }
    },

    apiCall: {
        leadPush: function () {
            $.ajax({
                type: "POST",
                url: '/api/leadform/',
                data: Volvo.customerInfo,
                dataType: 'Json',
                success: function (response) {
                    if(response != null)
                    {
                        Volvo.customerInfo.Id = response;
                    }
                },
                error: function () {
                    
                }
            })
        }
    }
};

/* popup state */
var appendState = function (state) {
    window.history.pushState(state, '', '');
};

$(window).on('popstate', function (event) {
    if ($('#gallery-blackOut-window').is(':visible')) {
        gallery.close();
    }
});

var popup = {
    lock: function (blackOutWindow) {
        var htmlElement = $('html'), bodyElement = $('body');
        $(blackOutWindow).show();
        if ($(document).height() > $(window).height()) {
            var windowScrollTop = htmlElement.scrollTop() ? htmlElement.scrollTop() : bodyElement.scrollTop();
            if (windowScrollTop < 0) {
                windowScrollTop = 0;
            }
            htmlElement.addClass('lock-browser-scroll').css('top', -windowScrollTop);
        }
    },
    unlock: function (blackOutWindow) {
        var htmlElement = $('html'),
            windowScrollTop = parseInt(htmlElement.css('top'));

        $(blackOutWindow).hide();
        htmlElement.removeClass('lock-browser-scroll');
        $('html, body').scrollTop(-windowScrollTop);
    }
};

var gallery = {
    open: function () {
        $('body').addClass('gallery-active');
        popup.lock('.gallery-blackOut-window');
        gallery.setPosition();
    },
    close: function () {
        $('body').removeClass('gallery-active');
        popup.unlock('.gallery-blackOut-window');
        gallery.resetPosition();
    },
    setPosition: function () {
        var topPosition = ($(window).height() / 2) - (($('.stage-photos').height() + $('.navigation-photos').height()) / 2);
        $('.connected-carousels-photos').css({
            'top': topPosition
        });
    },
    resetPosition: function () {
        $('.connected-carousels-photos').css({
            'top': 0
        });
    }
};

$(document).scroll(function () {
    console.log();
    if (Volvo.leadForm.isScrolledIntoView($(".contact-section"))) {
        dataLayer.push({
            event: 'CWNonInteractive',
            cat: 'volvo_s60',
            act: 'volvo_s60_page end'
        });
        $(document).unbind('scroll');
    }
});