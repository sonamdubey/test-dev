var JeepCompass = {
    doc: $(document),
    personLeadCity: {},
    newWidth: null,
    apiInput: {},

    compareCarWithJeepCompass: function() { 

    },

    responsive: {

        responsiveCarousel: function () {
            var jcarousel = $('.jeep-carousel .jcarousel');
            jcarousel
                .on('jcarousel:reload jcarousel:create', function () {
                    var carousel = $(this);
                    if (JeepCompass.newWidth == null) {
                        JeepCompass.newWidth = carousel.innerWidth();
                    }
                   // carousel.jcarousel('items').css('width', Math.ceil(JeepCompass.newWidth)/3 + 'px');
                })
                .jcarousel();
        },

        registerEvent: function () {

            $('.full-width-carousel .jcarousel').jcarousel('scroll', 0);
            $(".full-width-carousel .jcarousel ul li").css("width", $(window).outerWidth());
            $('.jeep-carousel .jcarousel').jcarousel('scroll', 0);

            JeepCompass.newWidth = null;
            JeepCompass.responsive.responsiveCarousel();
            $(".content-parent").css({ 'width': $(".jeep-img-box").width() - 1});
            if ($(document).width() <= 622) {
                $(".content-parent").css({ 'height': 'auto'});
            } else {
                $(".content-parent").css({ 'height': $(".jeep-img-box").height()});
            }
            for (var i = 0; i < $(".content-parent").length; i++) {
                if ($(".content-parent").get(i).scrollHeight > $(".content-parent").height()) {
                    $($(".jeep-content-box")[i]).addClass("zero-transform");
                } else {
                    $($(".jeep-content-box")[i]).removeClass("zero-transform");
                }
            }
        }
    },

    lazyImg: {

        callEventLazy: function () {
            $("img.lazy").lazyload();
            $(window).load(function () {
                JeepCompass.lazyImg.applyLazyLoad();
            });
        },

        applyLazyLoad: function () {
            $("img.lazy").lazyload({
                event: "imgLazyLoad",
                effect: "fadeIn"
            });
        }
    },

    leadForm: {

        registerEvent: function () {

            $("#esLeadFormSubmit").click(function () {
            	JeepCompass.leadForm.submit($(this));
            	
            });

            if(typeof isMobileDevice == "undefined") {
                $("#personCity").cw_autocomplete({
                    resultCount: 5,
                    source: ac_Source.allCarCities,
                    click: function (event, ui, orgTxt) {
                        JeepCompass.personLeadCity.name = Common.utils.getSplitCityName(ui.item.label);
                        JeepCompass.personLeadCity.id = ui.item.id;
                        ui.item.value = JeepCompass.personLeadCity.name;
                    },
                    open: function (result) {
                        JeepCompass.personLeadCity.result = result;
                    },
                    afterfetch: function (result, searchtext) {
                        this.result = result;
                        if (typeof result == "undefined" || result.length <= 0)
                            JeepCompass.leadForm.showHideMatchError(true, $('#personCity'), "No city Match");
                        else
                            JeepCompass.leadForm.showHideMatchError(false, $('#personCity'));
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

                        JeepCompass.personLeadCity.name = Common.utils.getSplitCityName(selectionLabel);
                        JeepCompass.personLeadCity.id = selectionValue;
                        $(personCityInputField).val(JeepCompass.personLeadCity.name);
                    },

                    afterFetch: function (result, searchText) {
                        JeepCompass.personLeadCity.result = result;

                        if (typeof result == "undefined" || result.length <= 0) {
                            JeepCompass.leadForm.showHideMatchError(true, $(personCityInputField).closest('.easy-autocomplete'), "No city Match");
                        }
                        else {
                            JeepCompass.leadForm.showHideMatchError(false, $(personCityInputField).closest('.easy-autocomplete'));
                        }
                    }
                });
            }
        },

        submit: function (event) {
            var nameEle = $(event).parent().find("#personName");
            //var emailEle = $(event).parent().find("#personEmail");
            var mobileEle = $(event).parent().find("#personMob");
            var cityEle = $(event).parent().find("#personCity");
            var err = form.validation.contact(nameEle.val(), "jeep@carwale.com", mobileEle.val()); //, $.trim(cityEle.val())); emailEle.val(), 

            if (JeepCompass.leadForm.processErrorResults(err, nameEle, mobileEle, cityEle)) { //emailEle,
                $("#esLeadFormSubmit").html("Submitting...").attr("disabled", true);
                JeepCompass.apiCall.leadPush(JeepCompass.leadForm.getUserObject(nameEle.val(), mobileEle.val()), JeepCompass.leadForm.processResponse); //emailEle.val(), 
            }
        },
        getUserObject: function (custname, custmobile) {
            var customerinfo = {
                carName: carName,
                name: custname,
                mobile: custmobile,
                cityid: JeepCompass.personLeadCity.id,
                versionId: "",
                modelId: modelid,
                makeId: "",
                leadType: PushleadType,
                cityName: JeepCompass.personLeadCity.name
            };
            return customerinfo;
        },
        processResponse: function (data) {
        	$("#es-leadform").hide();
        	$('.get-more-info').hide();
        	$("#es-thankyou").show();
        	$('#es-thankyou h2').css("color", "#fff");
            $(".request-callback-box .grid-4").css("background", "transparent");
        },
        validateCity: function (targetId) {

            var cityVal = Common.utils.getSplitCityName(targetId.val());
            if (cityVal == $.cookie("_CustCityMaster") && typeof (JeepCompass.personLeadCity) != "undefined" && Number(JeepCompass.personLeadCity.id) > 0 && JeepCompass.personLeadCity.id == $.cookie("_CustCityIdMaster")) {
                if(typeof isMobileDevice == "undefined") {
                    JeepCompass.leadForm.showHideMatchError(false, targetId);
                }
                else {
                    JeepCompass.leadForm.showHideMatchError(false, $(targetId).closest('.easy-autocomplete'));
                }                
                return true;
            }
            else if (cityVal == "" || targetId.hasClass('border-red') ||
                        (
                            ($('li.ui-menu-item a:visible:eq(0)').text() != cityVal && cityVal != "") &&
                            (typeof (JeepCompass.personLeadCity) == "undefined" || typeof (JeepCompass.personLeadCity.name) == "undefined" || JeepCompass.personLeadCity.name.toLowerCase() != cityVal.toLowerCase())
                        )
                   ) {
                if(typeof isMobileDevice == "undefined") {
                    JeepCompass.leadForm.showHideMatchError(true, targetId, "Please Enter City");
                }
                else {
                    JeepCompass.leadForm.showHideMatchError(true, $(targetId).closest('.easy-autocomplete'), "Please Enter City");
                }
                return false;
            }
            return true;
        },
        processErrorResults: function (err, nameEle, mobileEle, cityEle) { //emailEle,

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

                //if (err[1] == "") {
                //    emailEle.siblings().addClass('hide');
                //    emailEle.removeClass('border-red');
                //}
                //else {
                //    emailEle.siblings().removeClass('hide');
                //    emailEle.addClass('border-red');
                //    var errSpan = emailEle.siblings()[1];
                //    $(errSpan).text(err[1]);
                //    isFormValid = false;
                //}

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
            if (!JeepCompass.leadForm.validateCity(cityEle)) {
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
            var url = window.location.href.toLowerCase();
            JeepCompass.apiInput.DealerId =  6581;
            JeepCompass.apiInput.CityId = customerinfo.cityid;
            JeepCompass.apiInput.ActualDealerId = 0;
            JeepCompass.apiInput.ModelId = customerinfo.modelId;
            JeepCompass.apiInput.Email = '';
            JeepCompass.apiInput.Name = customerinfo.name;
            JeepCompass.apiInput.Mobile = customerinfo.mobile;
            JeepCompass.apiInput.BuyTimeText = "1 week";
            JeepCompass.apiInput.BuyTimeValue = 7;
            JeepCompass.apiInput.RequestType = 1;
            JeepCompass.apiInput.UtmaCookie = isCookieExists('__utma') ? $.cookie('__utma') : '';
            JeepCompass.apiInput.UtmzCookie = isCookieExists('_cwutmz') ? $.cookie('_cwutmz') : '';
            JeepCompass.apiInput.InquirySourceId = 191;
            JeepCompass.apiInput.LeadClickSource = 247;
            JeepCompass.apiInput.PlatformSourceId = 1;
            JeepCompass.apiInput.PQId = 0;
            JeepCompass.apiInput.ModelsHistory = '';
            JeepCompass.apiInput.IsAutoApproved = false;
            JeepCompass.apiInput.AssignedDealerId = -1;
            JeepCompass.apiInput.SponsoredBannerCookie = '';
            JeepCompass.apiInput.Comments = url.indexOf("source") > 0 ? Common.utils.getValueFromQS('source') : "";

            $.ajax({
                type: 'POST',
                url: '/webapi/DealerSponsoredAd/PostDealerInquiry/',
                data: JeepCompass.apiInput,
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
        
    }
}

var windowWidth, resizeId;

$(document).ready(function () {

    $("body").removeClass("rsz-lyt");
    windowWidth = $(window).width();

    $("#esLeadFormSubmit").attr('data-label', 'submit').addClass("click_track");
    $("#es-leadform").append('<div class="margin-top15"> Or Call us on <a href="tel:18002090230" style="text-decoration:none;color:#fff;">1800 2090 230</a></div>');
    $($("#es-leadform").children()[2]).hide(); 
    $("#personEmail").parent().hide();
    JeepCompass.responsive.registerEvent();
    JeepCompass.lazyImg.callEventLazy();
    JeepCompass.leadForm.registerEvent();
    JeepCompass.compareCarWithJeepCompass();
    JeepCompass.responsive.responsiveCarousel();
    JeepCompass.lazyImg.applyLazyLoad();

    $("#es-thankyou h2").html("Thank You for sharing your details. Our representative will call you back shortly.");

    $(".read-more-btn").click(function () {
        var _self = $(this);
        var id = _self.attr('id');
        var label = $('#' + id).text() == "Read More" ? "ReadMore" : "ReadLess";
        if (!_self.hasClass("img-active")) {
            _self.addClass('img-active');
            _self.text("Read Less");
            _self.parent().siblings().show();
            JeepCompass.leadForm.TrackLinks(id, label);
        }
        else {
            _self.text("Read More");
            _self.removeClass('img-active');
            _self.parent().siblings().hide();
            JeepCompass.leadForm.TrackLinks(id, label);
        }
    });

    $(".hide-content").addClass("comp-border-right");

    var label = '';
    $(".click_track").click(function () {
        label = $(this).attr('data-label');
        dataLayer.push({
            event: 'CWInteractive',
            cat: 'Jeep_Compass',
            act: 'Button_Clicked_' + label
        });
    });
    $(".click_track_section").click(function () {
        label = $(this).attr('data-label');
        dataLayer.push({
            event: 'CWInteractive',
            cat: 'Jeep_Compass',
            act: '5Reasons_clicked_' + label
        });
    });
    dataLayer.push({
        event: 'CWNonInteractive',
        cat: 'Jeep_Compass',
        act: 'Jeep_Compass_shown'
    });
    $(".check-price").on('click', function () {
        JeepCompass.openPqWidget(4732, 975, 128);
    });
    $(".ask-expert").addClass("hide");

    // Add smooth scrolling to Navigation links of Jeep Compass
    $(".spotlight__navigation-bar-list-item a").on('click', function (event) {
        if (this.hash !== "") {
            event.preventDefault();
            var hash = this.hash;
            $('html, body').animate({
                scrollTop: $(hash).offset().top
            }, 800, function () {
                window.location.hash = hash;
            });
        }

        $(this).parent().siblings().removeClass("spotlight__navigation__active");
        $(this).parent().addClass('spotlight__navigation__active');
    });


});

$(window).resize(function () {
    clearTimeout(resizeId);
    if ($(window).width() != windowWidth) {
        windowWidth = $(window).width();
        resizeId = setTimeout(JeepCompass.responsive.registerEvent, 500);
    }
});

$($(".jeep-img-box")[0]).on('load', function () {
    if ($(document).width() <= 622) {
        $(".hide-content").removeClass("comp-border-right");
        $(".content-parent").css({ 'height': 'auto' });
    } else {
        $(".hide-content").addClass("comp-border-right");
        $(".content-parent").css({ 'height': $("jeep-img-box").height() });
    }
})

$(document).scroll(function () {
    if (JeepCompass.isScrolledIntoView($(".pq-box"))) {
        dataLayer.push({
            event: 'CWNonInteractive',
            cat: 'Jeep_Compass',
            act:'PQ_banner_shown'
        });
        $(document).unbind('scroll');
    }
});
