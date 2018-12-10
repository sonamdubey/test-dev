var MahindraTUV300 = {
    doc: $(document),
    personLeadCity: {},
    newWidth: null,

    compareCarWithMahindraTUV300: function() { 

        MahindraTUV300.doc.on('click', '.comparo-accordion li', function () {
            if ($(this).parent().parent().find(".data").is(":hidden")) {
                $(".data").slideUp();
                $(this).parent().parent().find(".data").slideDown();
                $(".comparo-accordion li").find("h5").removeClass("active-text");
                $(this).find("h5").addClass("active-text");

                $(".comparo-accordion li").find("span").removeClass("minus-icon");
                $(this).find("span").addClass("minus-icon");
            }
        }),

        MahindraTUV300.doc.on('click', '.selected-car1', function () {
            $(this).children().toggleClass("plus-icon, minus-icon");
            $(".show-car-list1").toggleClass("hide");
        }),

        MahindraTUV300.doc.on('click', '.selected-car2', function () {
            $(this).children().toggleClass("plus-icon, minus-icon");
            $(".show-car-list2").toggleClass("hide");
        }),

        MahindraTUV300.doc.on('click', '.show-car-list1 li', function () {
            $("img.force-lazy").trigger('appear');
            var selectedCarName = $(this).text();
            $('#selected-car-name').text(selectedCarName);
            $(".show-car-list1").toggleClass("hide");
            $(".selected-car1").children().toggleClass("plus-icon, minus-icon");
        }),

        MahindraTUV300.doc.on('click', '.show-car-list2 li', function () {
            $("img.force-lazy").trigger('appear');
            var selectedCarName = $(this).text();
            $('#selected-car-name2').text(selectedCarName);
            $(".show-car-list2").toggleClass("hide");
            $(".selected-car2").children().toggleClass("plus-icon, minus-icon");
        }),

        MahindraTUV300.doc.on('click', '#compare-creta1', function () {
            $(".hide-content").addClass("hide");
            $(".creta-content.hide-content").removeClass("hide");
        }),

        MahindraTUV300.doc.on('click', '#compare-creta2', function () {
            $(".hide-content2").addClass("hide");
            $(".creta-content.hide-content2").removeClass("hide").addClass("active");
        }),

        MahindraTUV300.doc.on('click', '#compare-duster1', function () {
            $(".hide-content").addClass("hide");
            $(".duster-content.hide-content").removeClass("hide");
        }),

        MahindraTUV300.doc.on('click', '#compare-duster2', function () {
            $(".hide-content2").addClass("hide");
            $(".duster-content.hide-content2").removeClass("hide").addClass("active");
        }),

        MahindraTUV300.doc.on('click', '#compare-tuv3001', function () {
            $(".hide-content").addClass("hide");
            $(".tuv300-content.hide-content").removeClass("hide");
        }),

        MahindraTUV300.doc.on('click', '#compare-tuv3002', function () {
            $(".hide-content2").addClass("hide");
            $(".tuv300-content.hide-content2").removeClass("hide").addClass("active");
        }),

        MahindraTUV300.doc.on('click', '#compare-ecosport1', function () {
            $(".hide-content").addClass("hide");
            $(".ecosport-content.hide-content").removeClass("hide");
        }),
        MahindraTUV300.doc.on('click', '#compare-ecosport2', function () {
            $(".hide-content2").addClass("hide");
            $(".ecosport-content.hide-content2").removeClass("hide").addClass("active");
        })
    },

    responsive: {

        responsiveCarousel: function () {
            var jcarousel = $('.scorpio-carousel .jcarousel');
            jcarousel
                .on('jcarousel:reload jcarousel:create', function () {
                    var carousel = $(this);
                    if (MahindraTUV300.newWidth == null) {
                        MahindraTUV300.newWidth = carousel.innerWidth();
                    }
                   // carousel.jcarousel('items').css('width', Math.ceil(MahindraTUV300.newWidth)/3 + 'px');
                })
                .jcarousel();
        },

        registerEvent: function () {

            $('.full-width-carousel .jcarousel').jcarousel('scroll', 0);
            $(".full-width-carousel .jcarousel ul li").css("width", $(window).outerWidth());
            $('.scorpio-carousel .jcarousel').jcarousel('scroll', 0);
           // $(".scorpio-carousel .jcarousel ul li").css("width", $(window).outerWidth()/3);
            MahindraTUV300.newWidth = null;
            MahindraTUV300.responsive.responsiveCarousel();
            $(".content-parent").css({ 'width': $(".tuv-img-box").width() - 1});
            if ($(document).width() <= 622) {
                //$(".hide-content").removeClass("comp-border-right");
                $(".content-parent").css({ 'height': 'auto'});
            } else {
                //$(".hide-content").addClass("comp-border-right");
                $(".content-parent").css({ 'height': $(".tuv-img-box").height()});
            }
            if ($(".content-parent").get(0).scrollHeight > $(".content-parent").height()) {
                $(".tuv-content-box").addClass("zero-transform");
            } else {
                $(".tuv-content-box").removeClass("zero-transform");
            }
        }
    },

    lazyImg: {

        callEventLazy: function () {
            $("img.lazy").lazyload();
            $(window).load(function () {
                MahindraTUV300.lazyImg.applyLazyLoad();
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
                MahindraTUV300.leadForm.submit($(this));
            });

            if(typeof isMobileDevice == "undefined") {
                $("#personCity").cw_autocomplete({
                    resultCount: 5,
                    source: ac_Source.allCarCities,
                    click: function (event, ui, orgTxt) {
                        MahindraTUV300.personLeadCity.name = Common.utils.getSplitCityName(ui.item.label);
                        MahindraTUV300.personLeadCity.id = ui.item.id;
                        ui.item.value = MahindraTUV300.personLeadCity.name;
                    },
                    open: function (result) {
                        MahindraTUV300.personLeadCity.result = result;
                    },
                    afterfetch: function (result, searchtext) {
                        this.result = result;
                        if (typeof result == "undefined" || result.length <= 0)
                            MahindraTUV300.leadForm.showHideMatchError(true, $('#personCity'), "No city Match");
                        else
                            MahindraTUV300.leadForm.showHideMatchError(false, $('#personCity'));
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

                        MahindraTUV300.personLeadCity.name = Common.utils.getSplitCityName(selectionLabel);
                        MahindraTUV300.personLeadCity.id = selectionValue;
                        $(personCityInputField).val(MahindraTUV300.personLeadCity.name);
                    },

                    afterFetch: function (result, searchtext) {
                        MahindraTUV300.personLeadCity.result = result;

                        if (typeof result == "undefined" || result.length <= 0) {
                            MahindraTUV300.leadForm.showHideMatchError(true, $(personCityInputField).closest('.easy-autocomplete'), "No city Match");
                        }
                        else {
                            MahindraTUV300.leadForm.showHideMatchError(false, $(personCityInputField).closest('.easy-autocomplete'));
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

            if (MahindraTUV300.leadForm.processErrorResults(err, nameEle, emailEle, mobileEle, cityEle)) {
                $("#esLeadFormSubmit").html("Sumbmiting...").attr("disabled", true);
                MahindraTUV300.apiCall.leadPush(MahindraTUV300.leadForm.getUserObject(nameEle.val(), emailEle.val(), mobileEle.val()), MahindraTUV300.leadForm.processResponse);
            }
        },
        getUserObject: function (custname, custemail, custmobile) {
            var customerinfo = {
                carName: carName,
                name: custname,
                email: custemail,
                mobile: custmobile,
                cityid: MahindraTUV300.personLeadCity.id,
                versionId: "",
                modelId: modelid,
                makeId: "",
                leadType: PushleadType,
                cityName: MahindraTUV300.personLeadCity.name
            };
            return customerinfo;
        },
        processResponse: function (data) {
            $("#es-leadform").hide();
            $("#es-thankyou").show();
        },
        validateCity: function (targetId) {

            var cityVal = Common.utils.getSplitCityName(targetId.val());
            if (cityVal == $.cookie("_CustCityMaster") && typeof (MahindraTUV300.personLeadCity) != "undefined" && Number(MahindraTUV300.personLeadCity.id) > 0 && MahindraTUV300.personLeadCity.id == $.cookie("_CustCityIdMaster")) {
                if(typeof isMobileDevice == "undefined") {
                    MahindraTUV300.leadForm.showHideMatchError(false, targetId);
                }
                else {
                    MahindraTUV300.leadForm.showHideMatchError(false, $(targetId).closest('.easy-autocomplete'));
                }
                return true;
            }
            else if (cityVal == "" || targetId.hasClass('border-red') ||
                        (
                            ($('li.ui-menu-item a:visible:eq(0)').text() != cityVal && cityVal != "") &&
                            (typeof (MahindraTUV300.personLeadCity) == "undefined" || typeof (MahindraTUV300.personLeadCity.name) == "undefined" || MahindraTUV300.personLeadCity.name.toLowerCase() != cityVal.toLowerCase())
                        )
                   ) {
                if(typeof isMobileDevice == "undefined") {
                    MahindraTUV300.leadForm.showHideMatchError(true, targetId, "Please Enter City");
                }
                else {
                    MahindraTUV300.leadForm.showHideMatchError(true, $(targetId).closest('.easy-autocomplete'), "Please Enter City");
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
            if (!MahindraTUV300.leadForm.validateCity(cityEle)) {
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

    MahindraTUV300.responsive.registerEvent();
    MahindraTUV300.lazyImg.callEventLazy();
    MahindraTUV300.leadForm.registerEvent();
    MahindraTUV300.compareCarWithMahindraTUV300();
    MahindraTUV300.responsive.responsiveCarousel();

    $("#es-thankyou h2").html("Mahindra representative will get in touch you shortly");

    $(".read-more-btn").click(function () {
        var _self = $(this);
        var id = _self.attr('id');
        var label = $('#' + id).text() == "Read More" ? "ReadMore" : "ReadLess";
        if (!_self.hasClass("img-active")) {
            _self.addClass('img-active');
            _self.text("Read Less");
            _self.parent().siblings().show();
            MahindraTUV300.leadForm.TrackLinks(id, label);
        }
        else {
            _self.text("Read More");
            _self.removeClass('img-active');
            _self.parent().siblings().hide();
            MahindraTUV300.leadForm.TrackLinks(id, label);
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
            cat: 'mahindra_TUV300',
            act: 'Button_Clicked',
            lab: label
        });
    });
    $(".click_track_section").click(function () {
        label = $(this).attr('data-label');
        dataLayer.push({
            event: 'CWInteractive',
            cat: 'mahindra_TUV300',
            act: '5Reasons_clicked',
            lab: label
        });
    });
    dataLayer.push({
        event: 'CWNonInteractive',
        cat: 'mahindra_TUV300',
        act: 'TUV_300_shown'
    });
    $(".check-price").on('click', function () {
        MahindraTUV300.openPqWidget(4244, 932, 126);
    });
    $(".ask-expert").addClass("hide");
});

$(window).resize(function () {
    clearTimeout(resizeId);
    if ($(window).width() != windowWidth) {
        windowWidth = $(window).width();
        resizeId = setTimeout(MahindraTUV300.responsive.registerEvent, 500);
    }
    
});

$($(".tuv-img-box")[0]).on('load', function () {
    if ($(document).width() <= 622) {
        $(".hide-content").removeClass("comp-border-right");
        $(".content-parent").css({ 'height': 'auto' });
    } else {
        $(".hide-content").addClass("comp-border-right");
        $(".content-parent").css({ 'height': $(".tuv-img-box").height() });
    }
})

$(document).scroll(function () {
    console.log();
    if (MahindraTUV300.isScrolledIntoView($(".pq-box"))) {
        dataLayer.push({
            event: 'CWNonInteractive',
            cat: 'mahindra_TUV300',
            act:'PQ_banner_shown'
        });
        $(document).unbind('scroll');
    }
});

