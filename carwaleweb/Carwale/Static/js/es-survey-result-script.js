var Electric = {
    personLeadCity: {},
    newWidth: null,
    firstTimeLoad: true,
    responsive: {

        responsiveCarousel: function () {
            var jcarousel = $('.vehicles .jcarousel');
            jcarousel
                .on('jcarousel:reload jcarousel:create', function () {
                    var carousel = $(this);
                    if (Electric.newWidth == null) {
                        Electric.newWidth = carousel.innerWidth();
                    }
                    carousel.jcarousel('items').css('width', Math.ceil(Electric.newWidth) + 'px');
                })
                .jcarousel();
        },

        //responsiveFB:function()
        //{
        //    var container_width = $('#pageFbContainer').width();
        //    $('#pageFbContainer').html('<div class="fb-page" data-href="https://www.facebook.com/carwale" data-width="' + container_width + '" data-height="430" data-small-header="false" data-adapt-container-width="true" data-hide-cover="false" data-show-facepile="false" data-show-posts="true"><div class="fb-xfbml-parse-ignore"><blockquote cite="https://www.facebook.com/carwale"><a href="https://www.facebook.com/carwale">Facebook</a></blockquote></div></div>');
        //    FB.XFBML.parse();
        //},

        registerEvent: function () {
            $('.full-width-carousel .jcarousel').jcarousel('scroll', 0);
            $(".full-width-carousel .jcarousel ul li").css("width", $(window).width());
            var activeTab = $("#mahindra-cw-tabs li").first();
            activeTab.trigger("click");
            $('.vehicles .jcarousel').jcarousel('scroll', 0);
            Electric.newWidth = null;
            Electric.responsive.responsiveCarousel();
            
            $("#mahindra-cw-tabs li").click(function () {
                if (!$(this).hasClass('active')) {
                    var label = $(this).attr('data-tabs');
                    Electric.leadForm.TrackLinks("mahindra-cw-tabs", label);
                }
            });
        },

        testimonialsReset: function ()
        {
            if ($(window).width() <= 360) {
                $(".testimonials-carousel ul li").removeClass("active-test");
                $(".testimonials-carousel ul li:first").addClass("active-test");
                $(".testimonials-data").addClass("hide");
                $("#test1").removeClass("hide");
                $('.testimonials-carousel .jcarousel').on('jcarousel:visiblein', 'li', function (event, carousel) {
                    $(this).trigger('click');
                });
            }   
        }
    },

    lazyImg: {

        callEventLazy: function () {
            $("img.lazy").lazyload();
            $(window).load(function () {
                Electric.lazyImg.applyLazyLoad();
            });
        },

        applyLazyLoad: function () {
            try {
                var lazyImg = $(document).find('.card-jcarousel .jcarousel img.lazy');
                for (var i = 0; i < lazyImg.length; i++) {
                    var $lazyImg = $(lazyImg[i]);
                    var dataOriginal = $lazyImg.attr('data-original');
                    var dataSrc = $lazyImg.attr('src');
                    if (dataSrc == '' || dataSrc == undefined || dataSrc == null) {
                        $lazyImg.attr('src', dataOriginal);
                    }
                }
            } catch (e) { }
        }
    },

    leadForm: {

        registerEvent: function () {

            $("#esLeadFormSubmit").click(function () {
                Electric.leadForm.submit($(this));
            });

            if(typeof isMobileDevice == "undefined") {
                $("#personCity").cw_autocomplete({
                    resultCount: 5,
                    source: ac_Source.allCarCities,
                    click: function (event, ui, orgTxt) {
                        Electric.personLeadCity.name = Common.utils.getSplitCityName(ui.item.label);
                        Electric.personLeadCity.id = ui.item.id;
                        ui.item.value = Electric.personLeadCity.name;
                    },
                    open: function (result) {
                        Electric.personLeadCity.result = result;
                    },
                    afterfetch: function (result, searchtext) {
                        this.result = result;
                        if (typeof result == "undefined" || result.length <= 0)
                            Electric.leadForm.showHideMatchError(true, $('#personCity'), "No city Match");
                        else
                            Electric.leadForm.showHideMatchError(false, $('#personCity'));
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
                        
                        Electric.personLeadCity.name = Common.utils.getSplitCityName(selectionLabel);
                        Electric.personLeadCity.id = selectionValue;
                        $(personCityInputField).val(Electric.personLeadCity.name);
                    },

                    afterFetch: function (result, searchText) {
                        Electric.personLeadCity.result = result;

                        if (typeof result == "undefined" || result.length <= 0) {
                            Electric.leadForm.showHideMatchError(true, $(personCityInputField).closest('.easy-autocomplete'), "No city Match");
                        }
                        else {
                            Electric.leadForm.showHideMatchError(false, $(personCityInputField).closest('.easy-autocomplete'));
                        }
                    }
                });
            }       
        },

        submit: function (event) {

            var nameEle = $(event).parent().find("#personName");
            var emailEle = $(event).parent().find("#personEmail");
            var mobileEle = $(event).parent().find("#personMob");
            var cityEle = $(event).parent().find("#personCity");
            var err = form.validation.contact(nameEle.val(), emailEle.val(), mobileEle.val()); //, $.trim(cityEle.val()));

            if (Electric.leadForm.processErrorResults(err, nameEle, emailEle, mobileEle, cityEle)) {
                $("#esLeadFormSubmit").html("Sumbmiting...").attr("disabled", true);
                Electric.apiCall.leadPush(Electric.leadForm.getUserObject(nameEle.val(), emailEle.val(), mobileEle.val()), Electric.leadForm.processResponse);
            }
        },
        getUserObject: function (custname, custemail, custmobile) {
            var customerinfo = {
                carName: carName,
                name: custname,
                email: custemail,
                mobile: custmobile,
                cityid: Electric.personLeadCity.id,
                versionId: "",
                modelId: modelid,
                makeId: "",
                leadType: PushleadType,
                cityName: Electric.personLeadCity.name
            };
            return customerinfo;
        },
        processResponse: function (data) {
            $("#es-leadform").hide();
            $("#es-thankyou").show();
        },
        validateCity: function (targetId) {

            var cityVal = Common.utils.getSplitCityName(targetId.val());
            if (cityVal == $.cookie("_CustCityMaster") && typeof (Electric.personLeadCity) != "undefined" && Number(Electric.personLeadCity.id) > 0 && Electric.personLeadCity.id == $.cookie("_CustCityIdMaster")) {
                if(typeof isMobileDevice == "undefined") {
                    Electric.leadForm.showHideMatchError(false, targetId);
                }
                else {
                    Electric.leadForm.showHideMatchError(false, $(targetId).closest('.easy-autocomplete'));
                }
                return true;
            }
            else if (cityVal == "" || targetId.hasClass('border-red') ||
                        (
                            ($('li.ui-menu-item a:visible:eq(0)').text() != cityVal && cityVal != "") &&
                            (typeof (Electric.personLeadCity) == "undefined" || typeof (Electric.personLeadCity.name) == "undefined" || Electric.personLeadCity.name.toLowerCase() != cityVal.toLowerCase())
                        )
                   ) {
                if(typeof isMobileDevice == "undefined") {
                    Electric.leadForm.showHideMatchError(true, targetId, "Please Enter City");
                }
                else {
                    Electric.leadForm.showHideMatchError(true, $(targetId).closest('.easy-autocomplete'), "Please Enter City");
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
                    mobileEle.siblings('.cw-blackbg-tooltip, .error-icon').removeClass('hide');
                    mobileEle.addClass('border-red');
                    var errSpan = mobileEle.siblings()[2];
                    $(errSpan).text(err[2]);
                    isFormValid = false;
                }
            }
            if (!Electric.leadForm.validateCity(cityEle)) {
                isFormValid = false;
            }
            return isFormValid;
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
        TrackLinks: function (id, label) {
            var labelText = "";
            if (id == 'main-section')
                labelText = "Click_FirstSection_" + label;
            if (id == 'electric-e2o')
                labelText = "Click_e2oTab_" + label;
            if (id == 'electric-eVerito')
                labelText = "Click_eVeritoTab_" + label;
            if (id == 'mahindra-cw-tabs')
                labelText = "Click_" + label + "_Tab";
            Common.utils.trackAction('CWInteractive', 'CWSpecials', 'ElectricSection', labelText);
        }
    },

    apiCall: {

        leadPush: function (customerinfo, callback) {

            $.ajax({
                type: "POST",
                url: "/ajaxpro/CarwaleAjax.AjaxResearch,Carwale.ashx",
                data: '{"carName":"' + customerinfo.carName + '", "custName":"' + customerinfo.name + '", "email":"' + customerinfo.email + '", "mobile":"' + customerinfo.mobile + '", "selectedCityId":"' + customerinfo.cityid + '", "versionId":"' + customerinfo.versionId + '", "modelId":"' + customerinfo.modelId + '", "makeId":"' + customerinfo.makeId + '", "leadtype":"' + customerinfo.leadType + '", "cityName":"' + customerinfo.cityName + '"}',
                beforeSend: function (xhr) { xhr.setRequestHeader("X-AjaxPro-Method", "PushCRM"); },
                success: function (response) {
                    callback(response);
                },
                error: function () {
                    callback(null);
                }
            });
        }
    },
    map: {
        bindDealers: function (callback) {
            $('#drpDealerCity').empty();
            var dealerCities = [];
            $.ajax({
                type: 'GET',
                url: '/webapi/NewCarDealers/cities/?makeId=45',
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
                            Electric.koCityModel.cities.push(value);
                        });
                        ko.applyBindings(Electric.koCityModel, $('#drpDealerCity')[0]);
                        if (callback)
                            callback(true);
                    }
                }
            });
        },
        callToMapApi: function (cityId) {

            var NCD_RESPONSE;
            var cityId = cityId != "" && cityId != undefined ? cityId : $('#drpDealerCity').val();
            Electric.koViewModel.DealerDataList([]);
            $.ajax({
                type: 'GET',
                url: '/webapi/NewCarDealers/showrooms/?makeId=45&cityId=' + cityId,
                dataType: 'Json',
                success: function (response) {
                    try {
                        NCD_RESPONSE = response;
                        $.each(NCD_RESPONSE.newCarDealers, function (key, value) {
                            if (isLatLongValid(value.latitude, value.longitude)) {
                                Electric.koViewModel.DealerDataList.push(value);
                            }
                        });
                        plotGoogleMap(Electric.koViewModel.DealerDataList(), "divMap", true);
                    }
                    catch (err) {
                        console.log('DealerByCityId ' + err.message);
                    }
                }
            });
        }

    },
    koViewModel: {
        DealerDataList: ko.observableArray()
    },
    koCityModel: {
        cities: ko.observableArray()
    },
    prefillCityData: function (response) {
        if (response) {
            Electric.map.callToMapApi(10);
            $('#drpDealerCity').val(10);
            Electric.firstTimeLoad = false;
        }
    }
};

var windowWidth, resizeId;

$(document).ready(function () {
    Electric.map.bindDealers(Electric.prefillCityData);
    $('#drpDealerCity').live('change', function () {
        if (!Electric.firstTimeLoad)
            Electric.map.callToMapApi();
    });

    $('.map-dealer-list-card').live('click', function () {
        //var showBasicDealerDetails = $(this).attr('id')
        if ($(this).find('h4 i.icon-view').hasClass('uncheck') ) {
            showBasicDealerDetails($(this).attr('id'));
            $('h4 i.icon-view').removeClass('fa-check-square-o').addClass('uncheck');
            $(this).find('h4 i.icon-view').removeClass('uncheck').addClass('fa-check-square-o');
        }else{
            $(this).find('h4 i.icon-view').removeClass('fa-check-square-o').addClass('uncheck');
            hideInfoWindow();
        }        
    });
    ko.applyBindings(Electric.koViewModel, $('#dealerDiv')[0]);


    $("body").removeClass("rsz-lyt");
    windowWidth = $(window).width();
    
    Electric.responsive.registerEvent();
    Electric.lazyImg.callEventLazy();
    Electric.leadForm.registerEvent();
    Electric.responsive.testimonialsReset();
    Electric.responsive.responsiveCarousel();

    $(".read-more-btn").click(function () {
        var _self = $(this);
        var id = _self.attr('id');
        var label = $('#' + id).text() == "Read More" ? "ReadMore" : "ReadLess";
        if (!_self.hasClass("img-active")) {
            _self.addClass('img-active');
            _self.text("Read Less");
            _self.parent().siblings().show();
            Electric.leadForm.TrackLinks(id, label);
        }
        else {
            _self.text("Read More");
            _self.removeClass('img-active');
            _self.parent().siblings().hide();
            Electric.leadForm.TrackLinks(id, label);
        }
    });

    $(".testimonials-carousel .jcarousel ul li").click(function () {
        var testpanel = $(this).closest(".testimonials-carousel");
        testpanel.find("li").removeClass("active-test");
        $(this).addClass("active-test");

        var testpanelId = $(this).attr("data-id");
        testpanel.find(".testimonials-data").hide();
        $("#" + testpanelId).show();
    });

    $(".cw-tabs li").on('click', function () {
        $(".result-card").hide();
    });
    $(".jcarousel-control-right").on('click', function () {
        $("img.force-lazy").trigger('appear');
    });
    
    $('.d-survey-popup-close').on('click', function () {
        $('.survey-result-contentbox').hide();
        $(".result-card").show();
        $(".cw-tabs li").removeClass('active');
    });

    // on mobile open pop-up code start here
    $(".mob-result-container li").click(function () {
        $("img.force-lazy").trigger('appear');
        $('.blackOut-window').show();
        Common.utils.lockPopup();
    });
    $('.blackOut-window, .survey-popup-close').on('click', function () {
        $('.survey-result-popup').hide();
        $('.blackOut-window').hide();
        Common.utils.unlockPopup();
    });
    $(document).keydown(function (e) {
        // ESCAPE key pressed
        if (e.keyCode == 27) {
            $('.survey-result-popup').hide();
            $('.blackOut-window').hide();
            Common.utils.unlockPopup();
        }
    });
    
});

$(window).resize(function () {
    clearTimeout(resizeId);
    if ($(window).width() != windowWidth) {
        windowWidth = $(window).width();
        resizeId = setTimeout(Electric.responsive.registerEvent, 500);
        //Electric.responsive.responsiveFB();
        Electric.responsive.testimonialsReset();
    }    
});


