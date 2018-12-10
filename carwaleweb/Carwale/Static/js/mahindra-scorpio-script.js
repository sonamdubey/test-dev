var MahindraScorpio = {
    doc: $(document),
    personLeadCity: {},
    newWidth: null,

    compareCarWithMahindraScorpio: function() { 

    },

    responsive: {

        responsiveCarousel: function () {
            var jcarousel = $('.scorpio-carousel .jcarousel');
            jcarousel
                .on('jcarousel:reload jcarousel:create', function () {
                    var carousel = $(this);
                    if (MahindraScorpio.newWidth == null) {
                        MahindraScorpio.newWidth = carousel.innerWidth();
                    }
                   // carousel.jcarousel('items').css('width', Math.ceil(MahindraScorpio.newWidth)/3 + 'px');
                })
                .jcarousel();
        },

        registerEvent: function () {

            $('.full-width-carousel .jcarousel').jcarousel('scroll', 0);
            $(".full-width-carousel .jcarousel ul li").css("width", $(window).outerWidth());
            $('.scorpio-carousel .jcarousel').jcarousel('scroll', 0);

            MahindraScorpio.newWidth = null;
            MahindraScorpio.responsive.responsiveCarousel();
            $(".content-parent").css({ 'width': $(".scorpio-img-box").width() - 1});
            if ($(document).width() <= 622) {
                $(".content-parent").css({ 'height': 'auto'});
            } else {
                $(".content-parent").css({ 'height': $(".scorpio-img-box").height()});
            }
            if ($(".content-parent").get(0).scrollHeight > $(".content-parent").height()) {
                $(".scorpio-content-box").addClass("zero-transform");
            } else {
                $(".scorpio-content-box").removeClass("zero-transform");
            }
        }
    },

    lazyImg: {

        callEventLazy: function () {
            $("img.lazy").lazyload();
            $(window).load(function () {
                MahindraScorpio.lazyImg.applyLazyLoad();
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
                MahindraScorpio.leadForm.submit($(this));
            });

            if(typeof isMobileDevice == "undefined") {
                $("#personCity").cw_autocomplete({
                    resultCount: 5,
                    source: ac_Source.allCarCities,
                    click: function (event, ui, orgTxt) {
                        MahindraScorpio.personLeadCity.name = Common.utils.getSplitCityName(ui.item.label);
                        MahindraScorpio.personLeadCity.id = ui.item.id;
                        ui.item.value = MahindraScorpio.personLeadCity.name;
                    },
                    open: function (result) {
                        MahindraScorpio.personLeadCity.result = result;
                    },
                    afterfetch: function (result, searchtext) {
                        this.result = result;
                        if (typeof result == "undefined" || result.length <= 0)
                            MahindraScorpio.leadForm.showHideMatchError(true, $('#personCity'), "No city Match");
                        else
                            MahindraScorpio.leadForm.showHideMatchError(false, $('#personCity'));
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

                        MahindraScorpio.personLeadCity.name = Common.utils.getSplitCityName(selectionLabel);
                        MahindraScorpio.personLeadCity.id = selectionValue;
                        $(personCityInputField).val(MahindraScorpio.personLeadCity.name);
                    },

                    afterFetch: function (result, searchText) {
                        MahindraScorpio.personLeadCity.result = result;

                        if (typeof result == "undefined" || result.length <= 0) {
                            MahindraScorpio.leadForm.showHideMatchError(true, $(personCityInputField).closest('.easy-autocomplete'), "No city Match");
                        }
                        else {
                            MahindraScorpio.leadForm.showHideMatchError(false, $(personCityInputField).closest('.easy-autocomplete'));
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

            if (MahindraScorpio.leadForm.processErrorResults(err, nameEle, emailEle, mobileEle, cityEle)) {
                $("#esLeadFormSubmit").html("Sumbmiting...").attr("disabled", true);
                MahindraScorpio.apiCall.leadPush(MahindraScorpio.leadForm.getUserObject(nameEle.val(), emailEle.val(), mobileEle.val()), MahindraScorpio.leadForm.processResponse);
            }
        },
        getUserObject: function (custname, custemail, custmobile) {
            var customerinfo = {
                carName: carName,
                name: custname,
                email: custemail,
                mobile: custmobile,
                cityid: MahindraScorpio.personLeadCity.id,
                versionId: "",
                modelId: modelid,
                makeId: "",
                leadType: PushleadType,
                cityName: MahindraScorpio.personLeadCity.name
            };
            return customerinfo;
        },
        processResponse: function (data) {
            $("#es-leadform").hide();
            $("#es-thankyou").show();
        },
        validateCity: function (targetId) {

            var cityVal = Common.utils.getSplitCityName(targetId.val());
            if (cityVal == $.cookie("_CustCityMaster") && typeof (MahindraScorpio.personLeadCity) != "undefined" && Number(MahindraScorpio.personLeadCity.id) > 0 && MahindraScorpio.personLeadCity.id == $.cookie("_CustCityIdMaster")) {
                if(typeof isMobileDevice == "undefined") {
                    MahindraScorpio.leadForm.showHideMatchError(false, targetId);
                }
                else {
                    MahindraScorpio.leadForm.showHideMatchError(false, $(targetId).closest('.easy-autocomplete'));
                }
                return true;
            }
            else if (cityVal == "" || targetId.hasClass('border-red') ||
                        (
                            ($('li.ui-menu-item a:visible:eq(0)').text() != cityVal && cityVal != "") &&
                            (typeof (MahindraScorpio.personLeadCity) == "undefined" || typeof (MahindraScorpio.personLeadCity.name) == "undefined" || MahindraScorpio.personLeadCity.name.toLowerCase() != cityVal.toLowerCase())
                        )
                   ) {
                if(typeof isMobileDevice == "undefined") {
                    MahindraScorpio.leadForm.showHideMatchError(true, targetId, "Please Enter City");
                }
                else {
                    MahindraScorpio.leadForm.showHideMatchError(true, $(targetId).closest('.easy-autocomplete'), "Please Enter City");
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
            if (!MahindraScorpio.leadForm.validateCity(cityEle)) {
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
    },
    isScrolledIntoView : function (elem) {
        var docViewTop = $(window).scrollTop();
        var docViewBottom = docViewTop + $(window).height();

        var elemTop = $(elem).offset().top;
        var elemBottom = elemTop + $(elem).height();

        return ((elemBottom <= docViewBottom) && (elemTop >= docViewTop));
    },
    openPqWidget : function (versionId, modelId, pageId) {
        var caption = "";
        var url = "/new/quickpqwidget.aspx?model=" + modelId + "&version=" + versionId + "&pageid=" + pageId;
        var applyIframe = false;
        var GB_Html = "";
        GB_show(caption, url, 230, 500, applyIframe, GB_Html);
    }
}

var windowWidth, resizeId;

$(document).ready(function () {

    $("body").removeClass("rsz-lyt");
    windowWidth = $(window).width();

    $("#esLeadFormSubmit").attr('data-label', 'submit').addClass("click_track");
    $("#es-leadform").append('<div class="margin-top15">Or Call us on 1800 2090 230</div>')

    MahindraScorpio.responsive.registerEvent();
    MahindraScorpio.lazyImg.callEventLazy();
    MahindraScorpio.leadForm.registerEvent();
    MahindraScorpio.compareCarWithMahindraScorpio();
    MahindraScorpio.responsive.responsiveCarousel();

    $("#es-thankyou h2").html("Mahindra representative will get in touch you shortly");

    $(".read-more-btn").click(function () {
        var _self = $(this);
        var id = _self.attr('id');
        var label = $('#' + id).text() == "Read More" ? "ReadMore" : "ReadLess";
        if (!_self.hasClass("img-active")) {
            _self.addClass('img-active');
            _self.text("Read Less");
            _self.parent().siblings().show();
            MahindraScorpio.leadForm.TrackLinks(id, label);
        }
        else {
            _self.text("Read More");
            _self.removeClass('img-active');
            _self.parent().siblings().hide();
            MahindraScorpio.leadForm.TrackLinks(id, label);
        }
    });

    $(".hide-content").addClass("comp-border-right");

    $("#assistant-btn, #assistance-feature-btn, .book-drive, .request-callback-div").click(function () {
        $('html, body').animate({
            scrollTop: $(".pq-box").offset().top
        }, 500);
    });

    var label = '';
    $(".click_track").click(function () {
        label = $(this).attr('data-label');
        dataLayer.push({
            event: 'CWInteractive',
            cat: 'mahindra_scorpio',
            act: 'Button_Clicked_' + label
        });
    });
    $(".click_track_section").click(function () {
        label = $(this).attr('data-label');
        dataLayer.push({
            event: 'CWInteractive',
            cat: 'mahindra_scorpio',
            act: '5Reasons_clicked_' + label
        });
    });
    dataLayer.push({
        event: 'CWNonInteractive',
        cat: 'mahindra_scorpio',
        act: 'scorpio_shown'
    });
    $(".check-price").on('click', function () {
        MahindraScorpio.openPqWidget(3708, 570, 127);
    });
    $(".ask-expert").addClass("hide");
});

$(window).resize(function () {
    clearTimeout(resizeId);
    if ($(window).width() != windowWidth) {
        windowWidth = $(window).width();
        resizeId = setTimeout(MahindraScorpio.responsive.registerEvent, 500);
    }
    
});

$($(".scorpio-img-box")[0]).on('load', function () {
    if ($(document).width() <= 622) {
        $(".hide-content").removeClass("comp-border-right");
        $(".content-parent").css({ 'height': 'auto' });
    } else {
        $(".hide-content").addClass("comp-border-right");
        $(".content-parent").css({ 'height': $(".scorpio-img-box").height() });
    }
})

$(document).scroll(function () {
    console.log();
    if (MahindraScorpio.isScrolledIntoView($(".pq-box"))) {
        dataLayer.push({
            event: 'CWNonInteractive',
            cat: 'mahindra_scorpio',
            act:'PQ_banner_shown'
        });
        $(document).unbind('scroll');
    }
});

