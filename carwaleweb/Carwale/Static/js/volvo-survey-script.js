var VolvoSurvey = {
    doc: $(document),
    window: $(window),
    windowSize: $(window).width(),
    userInput: {},
    reNumeric: /^[0-9]*$/,
    questionNo:1,

    pageload: function () {
        $("body").removeClass("rsz-lyt");
        $('.ques-sec').first().removeClass('hide');
        $('#image_' + VolvoSurvey.questionNo).removeClass('hide');
        $('.ques-sec').parent().children(':first-child').next().removeClass('hide');
        $('.ques-sec:last').addClass('volvo-survey-thanku-show');
        $('.question-nav .round-container').first().addClass('volvo-survey-nav-active');
        VolvoSurvey.registerEvents();
        VolvoSurvey.calcKwidHt();
        $(".volvo-survey-container").css("background-image", "none");

        if ($.cookie('_CustomerName') != null && $.cookie('_CustomerName').length > 0) {
            $('#txtName').val($.cookie("_CustomerName"));
        }

        if ($.cookie('_CustEmail') != null && $.cookie('_CustEmail').length > 0) {
            $('#txtEmailId').val($.cookie("_CustEmail"));
        }

        if ($.cookie("_CustMobile") != null)
        {
            $('#txtMobile').val($.cookie("_CustMobile"));
        }
        $('.volvo-form-elements input').on('click', function () {
        	$(this).closest('.form-control-box').siblings('.error-text').hide();
        });
    },

    registerEvents: function () {
    	if (window.location.search !== '?app=true') {
    		window.history.pushState('addPopup', "", "");
    	}
    	else {
    		window.onbeforeunload = function () {
    			return 'Are you sure want to close the survey?';
    		}
    	}
        VolvoSurvey.doc.on('click', '.close-survey__btn-no', function () {
            $('.volvo-survey__popup-overlay').hide();
        });
        VolvoSurvey.doc.on('click', '.close-survey__btn-yes', function () {
        	setTimeout(function () {
        		self.close();
        	}, 100);
        });
        VolvoSurvey.doc.on('click', '.start-survey', function () {

            VolvoSurvey.manageQstnBackgroudImgUrl('https://imgd.aeplcdn.com/0x0/cw/es/renault-kwid/question-bg-1.jpg?201610071603361763');

            $(".landing-section").addClass("hide");
            $(".volvo-survey-questions, .question-nav").removeClass("hide");
        });

        VolvoSurvey.doc.on('click', '.volvo-survey-thanku-show input[input=radio]', function () {
            $(".question-nav last").addClass("filled");

            VolvoSurvey.manageQstnBackgroudImgUrl('https://imgd.aeplcdn.com/0x0/cw/es/renault-kwid/landing-bg.jpg?201610071603361763');

            $(".question-nav").addClass("hide");

            VolvoSurvey.userInput.SurveyResponse = "";

            $('.ques-sec input:checked').each(function () {
                VolvoSurvey.userInput.SurveyResponse += $(this).attr("questionId") + "~" + $(this).val() + "$";
            });
            VolvoSurvey.userInput.Platform = platformId;
            VolvoSurvey.userInput.CampaignId = campaignId;
            VolvoSurvey.postResponse();
        });

        VolvoSurvey.doc.on('click', '.volvo-survey-next', function () {
            var isMultiselect = $(this).attr('ismultiselect');

            if ($(this).parent().find("input:checked").length > 0 || ($.trim($(this).parent().find("textarea").val()) != "" && $(this).parent().find("textarea").val() != undefined))
            {
                $('.error-text').hide();
                $('#image_' + VolvoSurvey.questionNo).addClass('hide');
                VolvoSurvey.questionNo++;
                $(this).parent().find("textarea").removeClass("textarea-error")
                $(this).parent().addClass("read-que");
                $('.ques-sec').removeClass('es-survey-specific');
                $(".title-class").addClass("hide");
                $(".round-container.volvo-survey-nav-active").addClass("filled").removeClass("empty");
                $(this).parent().addClass("hide");
                $(this).parent().next().removeClass("hide");

                $('#image_' + VolvoSurvey.questionNo).removeClass('hide');
                $(".round-container.volvo-survey-nav-active").removeClass("volvo-survey-nav-active").next().addClass("volvo-survey-nav-active");

                VolvoSurvey.manageQstnBackgroudImgUrl($(this).parent().next().attr('imgurl'));

                if ($(this).parent().hasClass('volvo-survey-thanku-show')) {
                    $(".question-nav last").addClass("filled");
                    $(".question-nav").addClass("hide");
                    $(".landing-section").addClass("hide");
                    $(this).parents(".volvo-survey-main-content").find(".volvo-thank-you ").removeClass("hide");
                }
                VolvoSurvey.submitData($(this));
            }
            else {
                $('.error-text').show();

            }
        });

        VolvoSurvey.doc.on('click', '.survey-textarea', function () {
            $(this).parent().find("textarea").removeClass("textarea-error");
        });

        VolvoSurvey.doc.on('click', '#btnSubmit', function () {
            var isNameInvalid = VolvoSurvey.checkNameInvalid();
            var isMobInvalid = VolvoSurvey.checkMobInvalid();
            var isEmailInvalid = VolvoSurvey.checkEmailInvalid();
            if (!isNameInvalid && !isMobInvalid && !isEmailInvalid) {
                Common.utils.trackAction('CWInteractive', 'Luxury_Survey', platformId == 1 ? 'Clicks' : 'Clicks_m', 'Submit');
                VolvoSurvey.userInput.Platform = platformId;
                VolvoSurvey.userInput.CampaignId = campaignId;
                VolvoSurvey.userInput.BasicInfo = {};
                VolvoSurvey.userInput.BasicInfo.Name = $.trim($('#txtName').val());
                VolvoSurvey.userInput.BasicInfo.Email = $.trim($('#txtEmailId').val());
                VolvoSurvey.userInput.BasicInfo.Mobile = $('#txtMobile').val();
                VolvoSurvey.postResponse();
                $('.timer__container').show();
                var timerCount = 3;
                $("#timercount").text(timerCount);
                var myTimer = setInterval(function () {
                    if (timerCount > 0) {
                        timerCount = timerCount - 1;
                        $("#timercount").text(timerCount);
                    }
                    else {
                        clearInterval(myTimer);
                       window.location.href = "https://www.carwale.com/";
                    }
                }, 1000);


            }

        });
        //skip survey
        VolvoSurvey.doc.on('click', '.exit-link', function () {
            $('.error-text').hide();
            $('#image_' + VolvoSurvey.questionNo).addClass('hide');
            VolvoSurvey.questionNo++;
            $(this).parent().find("textarea").removeClass("textarea-error")
            $(this).parent().addClass("read-que");
            $('.ques-sec').removeClass('es-survey-specific');
            $(".title-class").addClass("hide");
            $(".round-container.volvo-survey-nav-active").addClass("filled").removeClass("empty");
            $(this).parent().addClass("hide");
            $(this).parent().next().removeClass("hide");

            $('#image_' + VolvoSurvey.questionNo).removeClass('hide');
            $(".round-container.volvo-survey-nav-active").removeClass("volvo-survey-nav-active").next().addClass("volvo-survey-nav-active");

            VolvoSurvey.manageQstnBackgroudImgUrl($(this).parent().next().attr('imgurl'));
            //$('.volvo-survey-next').html('Submit');
            //$(this).parent().find('.volvo-survey-next').removeClass('hide');

            if ($(this).parent().hasClass('volvo-survey-thanku-show')) {
                $(".question-nav last").addClass("filled");
                $(".question-nav").addClass("hide");
                $(".landing-section").addClass("hide");
                $(this).parents(".volvo-survey-main-content").find(".volvo-thank-you ").removeClass("hide");
            }
        });

        VolvoSurvey.window.resize(function () {
            if ((VolvoSurvey.window).width() < 641) {
                $(".volvo-survey-container").css("background-image", "none");
            }
            else {
                if ($('.question-nav .round-container').hasClass('volvo-survey-nav-active'))
                {
                    var panelId = $('volvo-survey-nav-active').attr('data-id');
                    var imgUrl = $("#" + panelId).find(".imgurl");
                    $(".volvo-survey-container").css("background-image", "url(" + imgUrl + ")");
                }
            }
        });

    },
    submitData: function(element)
    {
        VolvoSurvey.userInput.SurveyResponse = "";
        VolvoSurvey.userInput.Answer = "";

        VolvoSurvey.userInput.Platform = platformId;
        VolvoSurvey.userInput.CampaignId = campaignId;

        var questionId = element.parent().attr("questionId");
        if (element.parent().attr("isTextType") == 'True') {
            VolvoSurvey.userInput.Answer += questionId + "~" + element.parent().find("textarea").val() + "$";
            VolvoSurvey.userInput.IsFreeTextResponse = true;
            VolvoSurvey.userInput.Answer = VolvoSurvey.userInput.Answer.substring(0, VolvoSurvey.userInput.Answer.length - 1);
        }
        else {
            element.parent().find("input:checked").each(function () {
                VolvoSurvey.userInput.SurveyResponse += questionId + "~" + $(this).val() + "$";
            });
            VolvoSurvey.userInput.IsFreeTextResponse = false;
            VolvoSurvey.userInput.SurveyResponse = VolvoSurvey.userInput.SurveyResponse.substring(0, VolvoSurvey.userInput.SurveyResponse.length - 1);
        }

        VolvoSurvey.postResponse();
    },
    checkNameInvalid: function () {
        var reName = /^([-a-zA-Z ']*)$/;
        if ($.trim($('#txtName').val()) == "") {
            $('#spntxtName').show();
            return true;
        }
        else if (!VolvoSurvey.validateInputField($("#txtName"), reName)) {
            $('#spntxtName').show().text("Please enter valid name.");
            return true;
        }
        else {
            $('#spntxtName').hide();
            return false;
        }
    },
    checkMobInvalid: function () {
        var isError = false;
        var reMobile = /^[6789]\d{9}$/;

        if ($('#txtMobile').val() == "") {
            $('#spntxtMobile').show();
            return true;
        }
        else if ($('#txtMobile').val() != "") {

            if (!VolvoSurvey.reNumeric.test($("#txtMobile").val())) {
                $('#spntxtMobile').show().text("Please enter numeric data only.");
                return true;
            }
            else if (!reMobile.test($("#txtMobile").val())) {
                $('#spntxtMobile').show().text("Please enter valid mobile number.");
                return true;
            }
            else if (!VolvoSurvey.reNumeric.test($("#txtMobile").val()) && $("#txtMobile").val().length < 10) {
                $('#spntxtMobile').show().text("Please enter 10 digit.");
                return true;
            }
            else {
                $('#spntxtMobile').hide();
                return false;
            }
        }
        else {
            $('#spntxtMobile').hide();
            return false;
        }
    },

    checkEmailInvalid: function () {
        var reEmail = /^[a-z0-9._%+-]+@[a-z0-9.-]+\.[a-z]{2,6}$/;
        if ($('#txtEmailId').val() == "") {
            $('#spntxtEmail').show();
            return true;
        }
        else if (!VolvoSurvey.validateInputField($("#txtEmailId"), reEmail)) {
            $('#spntxtEmail').show().text("Please enter valid email.");
            return true;
        }
        else {
            $('#spntxtEmail').hide();
            return false;
        }
    },

    getApiInput: function() {
        VolvoSurvey.userInput.Platform = platformId;
        VolvoSurvey.userInput.CampaignId = campaignId;
    },

    validateInputField: function (field, regex) {
        try {
            if (!regex.test(field.val().toLowerCase())) {
                return false;
            }
            return true;
        }
        catch (e) { console.log(e) }
        return false;
    },

    postResponse: function () {
        var cwc = $.cookie('CWC');
        cwc = cwc != null && cwc != "" ? cwc : -1;
        VolvoSurvey.userInput.CustomerId = $('#custId').val();
        Common.utils.ajaxCall({
            type: 'POST',
            url: '/api/survey/?cwcCookie=' + cwc,
            data: VolvoSurvey.userInput,
            contentType: "application/x-www-form-urlencoded",
            dataType: 'Json',
        }).done(function (data) {
            if (data != null)
                $('#custId').val(data);

            VolvoSurvey.clearData();
        });
    },

    calcKwidHt: function () {
        if ($(window).height() > 1319) {
            var kwidCointainerht = $(window).height() - 410;
            $(".volvo-survey-container").css("min-height", kwidCointainerht)
        }
    },
    clearData: function () {
        VolvoSurvey.userInput = {};
        $('input[type="radio"]:checked').prop('checked', false);
        $('input[type="checkbox"]:checked').prop('checked', false);
        $('.survey-textarea').val('');
    },

    manageQstnBackgroudImgUrl: function (imageUrl) {
        if (VolvoSurvey.windowSize > 641) {
            $(".volvo-survey-container").css("background-image", "url(" + imageUrl +")");
        }
        else {
            $(".volvo-survey-container").css("background-image", "none");
        }
    }
}

$(document).ready(function () {
    VolvoSurvey.pageload();
});

$(window).on("popstate", function (e) {
    $('.volvo-survey__popup-overlay').show();
});
