var Terrano = {
    doc: $(document),
    personLeadCity: {},
    newWidth: null,

    compareCarWithTerrano: function() { 

        Terrano.doc.on('click', '.comparo-accordion li', function () {
            if ($(this).parent().parent().find(".data").is(":hidden")) {
                $(".data").slideUp();
                $(this).parent().parent().find(".data").slideDown();
                $(".comparo-accordion li").find("h5").removeClass("active-text");
                $(this).find("h5").addClass("active-text");

                $(".comparo-accordion li").find("span").removeClass("minus-icon");
                $(this).find("span").addClass("minus-icon");
            }
        }),

        Terrano.doc.on('click', '.selected-car', function () {
            $(this).children().toggleClass("plus-icon, minus-icon");
            $(".show-car-list").toggleClass("hide");
        }),

        Terrano.doc.on('click', '.show-car-list li', function () {
            $("img.force-lazy").trigger('appear');
            var selectedCarName = $(this).text();
            $('#selected-car-name').text(selectedCarName);
            $(".show-car-list").toggleClass("hide");
            $(".selected-car").children().toggleClass("plus-icon, minus-icon");
        }),

        Terrano.doc.on('click', '#compare-creta', function () {
            $(".hide-content").addClass("hide");
            $(".creta-content").removeClass("hide")  
        }),

        Terrano.doc.on('click', '#compare-duster', function () {
            $(".hide-content").addClass("hide");
            $(".duster-content").removeClass("hide")
        }),

        Terrano.doc.on('click', '#compare-tuv300', function () {
            $(".hide-content").addClass("hide");
            $(".tuv300-content").removeClass("hide")
        }),

        Terrano.doc.on('click', '#compare-ecosport', function () {
            $(".hide-content").addClass("hide");
            $(".ecosport-content").removeClass("hide")
        })
    },

    responsive: {

        responsiveCarousel: function () {
            var jcarousel = $('.vehicles .jcarousel');
            jcarousel
                .on('jcarousel:reload jcarousel:create', function () {
                    var carousel = $(this);
                    if (Terrano.newWidth == null) {
                        Terrano.newWidth = carousel.innerWidth();
                    }
                    carousel.jcarousel('items').css('width', Math.ceil(Terrano.newWidth) + 'px');
                })
                .jcarousel();
        },

        registerEvent: function () {

            $('.full-width-carousel .jcarousel').jcarousel('scroll', 0);
            $(".full-width-carousel .jcarousel ul li").css("width", $(window).outerWidth());
            Terrano.newWidth = null;
            Terrano.responsive.responsiveCarousel();
           
        }
    },

    lazyImg: {

        callEventLazy: function () {
            $("img.lazy").lazyload();
            $(window).load(function () {
                Terrano.lazyImg.applyLazyLoad();
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
                Terrano.leadForm.submit($(this));
            });

            if(typeof isMobileDevice == "undefined") {
                $("#personCity").cw_autocomplete({
                    resultCount: 5,
                    source: ac_Source.allCarCities,
                    click: function (event, ui, orgTxt) {
                        Terrano.personLeadCity.name = Common.utils.getSplitCityName(ui.item.label);
                        Terrano.personLeadCity.id = ui.item.id;
                        ui.item.value = Terrano.personLeadCity.name;
                    },
                    open: function (result) {
                        Terrano.personLeadCity.result = result;
                    },
                    afterfetch: function (result, searchtext) {
                        this.result = result;
                        if (typeof result == "undefined" || result.length <= 0)
                            Terrano.leadForm.showHideMatchError(true, $('#personCity'), "No city Match");
                        else
                            Terrano.leadForm.showHideMatchError(false, $('#personCity'));
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

                        Terrano.personLeadCity.name = Common.utils.getSplitCityName(selectionLabel);
                        Terrano.personLeadCity.id = selectionValue;
                        $(personCityInputField).val(Terrano.personLeadCity.name);
                    },

                    afterFetch: function (result, searchText) {
                        Terrano.personLeadCity.result = result;

                        if (typeof result == "undefined" || result.length <= 0) {
                            Terrano.leadForm.showHideMatchError(true, $(personCityInputField).closest('.easy-autocomplete'), "No city Match");
                        }
                        else {
                            Terrano.leadForm.showHideMatchError(false, $(personCityInputField).closest('.easy-autocomplete'));
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

            if (Terrano.leadForm.processErrorResults(err, nameEle, emailEle, mobileEle, cityEle)) {
                $("#esLeadFormSubmit").html("Sumbmiting...").attr("disabled", true);
                Terrano.apiCall.leadPush(Terrano.leadForm.getUserObject(nameEle.val(), emailEle.val(), mobileEle.val()), Terrano.leadForm.processResponse);
            }
        },
        getUserObject: function (custname, custemail, custmobile) {
            var customerinfo = {
                carName: carName,
                name: custname,
                email: custemail,
                mobile: custmobile,
                cityid: Terrano.personLeadCity.id,
                versionId: "",
                modelId: modelid,
                makeId: "",
                leadType: PushleadType,
                cityName: Terrano.personLeadCity.name
            };
            return customerinfo;
        },
        processResponse: function (data) {
            $("#es-leadform").hide();
            $("#es-thankyou").show();
        },
        validateCity: function (targetId) {

            var cityVal = Common.utils.getSplitCityName(targetId.val());
            if (cityVal == $.cookie("_CustCityMaster") && typeof (Terrano.personLeadCity) != "undefined" && Number(Terrano.personLeadCity.id) > 0 && Terrano.personLeadCity.id == $.cookie("_CustCityIdMaster")) {
                if(typeof isMobileDevice == "undefined") {
                    Terrano.leadForm.showHideMatchError(false, targetId);
                }
                else {
                    Terrano.leadForm.showHideMatchError(false, $(targetId).closest('.easy-autocomplete'));
                }
                return true;
            }
            else if (cityVal == "" || targetId.hasClass('border-red') ||
                        (
                            ($('li.ui-menu-item a:visible:eq(0)').text() != cityVal && cityVal != "") &&
                            (typeof (Terrano.personLeadCity) == "undefined" || typeof (Terrano.personLeadCity.name) == "undefined" || Terrano.personLeadCity.name.toLowerCase() != cityVal.toLowerCase())
                        )
                   ) {
                if(typeof isMobileDevice == "undefined") {
                    Terrano.leadForm.showHideMatchError(true, targetId, "Please Enter City");
                }
                else {
                    Terrano.leadForm.showHideMatchError(true, $(targetId).closest('.easy-autocomplete'), "Please Enter City");
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
            if (!Terrano.leadForm.validateCity(cityEle)) {
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
    }
};

var windowWidth, resizeId;

$(document).ready(function () {    
    $("body").removeClass("rsz-lyt");
    windowWidth = $(window).width();

    Terrano.responsive.registerEvent();
    Terrano.lazyImg.callEventLazy();
    Terrano.leadForm.registerEvent();
    Terrano.compareCarWithTerrano();
    Terrano.responsive.responsiveCarousel();

    $(".read-more-btn").click(function () {
        var _self = $(this);
        var id = _self.attr('id');
        var label = $('#' + id).text() == "Read More" ? "ReadMore" : "ReadLess";
        if (!_self.hasClass("img-active")) {
            _self.addClass('img-active');
            _self.text("Read Less");
            _self.parent().siblings().show();
            Terrano.leadForm.TrackLinks(id, label);
        }
        else {
            _self.text("Read More");
            _self.removeClass('img-active');
            _self.parent().siblings().hide();
            Terrano.leadForm.TrackLinks(id, label);
        }
    });

    
    
});

$(window).resize(function () {
    clearTimeout(resizeId);
    if ($(window).width() != windowWidth) {
        windowWidth = $(window).width();
        resizeId = setTimeout(Terrano.responsive.registerEvent, 500);
    }
    
});

