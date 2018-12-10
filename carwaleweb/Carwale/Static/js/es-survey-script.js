var ESSurvey = {
    doc: $(document),
    window: $(window),
    windowSize: $(window).width(),
    userInput: {},
    reNumeric: /^[0-9]*$/,

    pageload: function () {
        $("body").removeClass("rsz-lyt");
        $('.ques-sec').first().removeClass('hide'); 
        $('.ques-sec').parent().children(':first-child').next().removeClass('hide');
        $('.ques-sec:last').addClass('kwid-thanku-show');
        $('.question-nav .round-container').first().addClass('kwid-nav-active');
        ESSurvey.registerEvents();
        ESSurvey.calcKwidHt();
        $(".kwid-container").css("background-image", "none");
        
    },

    registerEvents: function () {
        ESSurvey.doc.on('click', '.start-survey', function () {
            
            ESSurvey.manageQstnBackgroudImgUrl('https://imgd.aeplcdn.com/0x0/cw/es/renault-kwid/question-bg-1.jpg?201610071603361763');

            $(".landing-section").addClass("hide");
            $(".kwid-questions, .question-nav").removeClass("hide");
        });        

        ESSurvey.doc.on('click', '.kwid-thanku-show input[input=radio]', function () {
            $(".question-nav last").addClass("filled");
            
            ESSurvey.manageQstnBackgroudImgUrl('https://imgd.aeplcdn.com/0x0/cw/es/renault-kwid/landing-bg.jpg?201610071603361763');

            $(".question-nav").addClass("hide");

            ESSurvey.userInput.SurveyResponse = "";

            $('.ques-sec input:checked').each(function () {
                ESSurvey.userInput.SurveyResponse += $(this).attr("questionId") + "~" + $(this).val() + "$";
            });
            ESSurvey.userInput.Platform = platformId;
            ESSurvey.userInput.CampaignId = campaignId;
            ESSurvey.postResponse();
        });

        ESSurvey.doc.on('click', '.kwid-next', function () {
            var isMultiselect = $(this).attr('ismultiselect');

            if ($(this).parent().find("input:checked").length > 0 || ($.trim($(this).parent().find("textarea").val()) != "" && $(this).parent().find("textarea").val() != undefined))
            {
                $(this).parent().find("textarea").removeClass("textarea-error")
                $(this).parent().addClass("read-que");
                $('.ques-sec').removeClass('es-survey-specific');
                $(".title-class").addClass("hide");
                $(".round-container.kwid-nav-active").addClass("filled").removeClass("empty");
                $(this).parent().addClass("hide");
                $(this).parent().next().removeClass("hide");
                $(".round-container.kwid-nav-active").removeClass("kwid-nav-active").next().addClass("kwid-nav-active");

                ESSurvey.manageQstnBackgroudImgUrl($(this).parent().next().attr('imgurl'));
                $('.kwid-next').html('Submit');
                //$(this).parent().find('.kwid-next').removeClass('hide');

                if ($(this).parent().hasClass('kwid-thanku-show')) {
                    $(".question-nav last").addClass("filled");
                    
                    $(".question-nav").addClass("hide");                    
                    ESSurvey.userInput.SurveyResponse = "";
                    ESSurvey.userInput.Answer = "";                    

                    $('.ques-sec input:checked').each(function () {
                        ESSurvey.userInput.SurveyResponse += $(this).attr("questionId") + "~" + $(this).val() + "$";
                        ESSurvey.userInput.IsFreeTextResponse = false;
                    });
                    ESSurvey.userInput.Platform = platformId;
                    ESSurvey.userInput.CampaignId = campaignId;

                    $('.questions').each(function () {
                        ESSurvey.userInput.Answer += $(this).attr("questionId") + "~" + $(this).find("textarea").val() + "$";
                        ESSurvey.userInput.IsFreeTextResponse = true;
                    });
                    ESSurvey.postResponse();
                }
            }
            else {
                $(this).parent().find("textarea").addClass("textarea-error")
            }
        });

        ESSurvey.doc.on('click', '.survey-textarea', function () {
            $(this).parent().find("textarea").removeClass("textarea-error")
        });

        ESSurvey.doc.on('click', '.question-nav div', function () {
            
            if ($(this).hasClass('filled') || $(this).prev().hasClass('filled'))
            {
                var panel = $(this).closest(".kwid-questions");
                panel.find(".question-nav div").removeClass("kwid-nav-active");
                $(this).addClass("kwid-nav-active");

                var panelId = $(this).attr("data-id");
                panel.find(".ques-sec").addClass("hide")
                $("#" + panelId).removeClass("hide");

                ESSurvey.manageQstnBackgroudImgUrl($("#" + panelId).attr('imgurl'));
            }
        });

        ESSurvey.doc.on('click', '#btnSubmit', function () {
            var isNameInvalid = ESSurvey.checkNameInvalid();
            var isMobInvalid = ESSurvey.checkMobInvalid();
            var isEmailInvalid = ESSurvey.checkEmailInvalid();
            if (isNameInvalid || isMobInvalid || isEmailInvalid) {
                return false;
            }
            else
                return true;
        });

        ESSurvey.window.resize(function () {
            if ((ESSurvey.window).width() < 641) {
                $(".kwid-container").css("background-image", "none");
            }
            else {
                if ($('.question-nav .round-container').hasClass('kwid-nav-active'))
                {
                    var panelId = $('kwid-nav-active').attr('data-id');
                    var imgUrl = $("#" + panelId).find(".imgurl");
                    $(".kwid-container").css("background-image", "url(" + imgUrl + ")");
                }
            }
        });
    
    },

    checkNameInvalid: function () {
        var reName = /^([-a-zA-Z ']*)$/;
        if ($.trim($('#txtName').val()) == "") {
            $('#spntxtName').show();
            return true;
        }
        else if (!ESSurvey.validateInputField($("#txtName"), reName)) {
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

            if (!ESSurvey.reNumeric.test($("#txtMobile").val())) {
                $('#spntxtMobile').show().text("Please enter numeric data only.");
                return true;
            }
            else if (!reMobile.test($("#txtMobile").val())) {
                $('#spntxtMobile').show().text("Please enter valid mobile number.");
                return true;
            }
            else if (!ESSurvey.reNumeric.test($("#txtMobile").val()) && $("#txtMobile").val().length < 10) {
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
        else if (!ESSurvey.validateInputField($("#txtEmailId"), reEmail)) {
            $('#spntxtEmail').show().text("Please enter valid email.");
            return true;
        }
        else {
            $('#spntxtEmail').hide();
            return false;
        }
    },

    getApiInput: function() {        
        ESSurvey.userInput.Platform = platformId;
        ESSurvey.userInput.CampaignId = campaignId;
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
        ESSurvey.userInput.Answer = ESSurvey.userInput.Answer.substring(0, ESSurvey.userInput.Answer.length - 1);
        Common.utils.ajaxCall({
            type: 'POST',
            url: '/api/survey/?cwcCookie=' +cwc,
            data: ESSurvey.userInput,
            contentType: "application/x-www-form-urlencoded",
            dataType: 'Json',
        }).done(function (data) {
            if (data != null)
                $('#custId').val(data);

            ESSurvey.clearData();
        });
    },

    calcKwidHt: function () {
        if ($(window).height() > 1319) {
            var kwidCointainerht = $(window).height() - 410;
            $(".kwid-container").css("min-height", kwidCointainerht)
        }
    },
    clearData: function () {
        ESSurvey.userInput = {};
        $('input[type="radio"]:checked').prop('checked', false);
        $('input[type="checkbox"]:checked').prop('checked', false);
        $('.survey-textarea').val('');
    },

    manageQstnBackgroudImgUrl: function (imageUrl) {
        if (ESSurvey.windowSize > 641) {
            $(".kwid-container").css("background-image", "url(" + imageUrl +")");
        }
        else {
            $(".kwid-container").css("background-image", "none");
        }
    }
}

$(document).ready(function () {
    ESSurvey.pageload();
});
