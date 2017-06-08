docReady(function () {
    showPage(1);
    $('.survey-checkbox').click(function () {
        var questionContainer = $(this).closest(".question-box"),
            qNum = $(questionContainer).attr('data-qNumber') * 1;

        if ($(this).attr('checked')) {
            if (qNum == 1) {
                $(questionContainer).find("select").prop('selectedIndex', 0);
            } else {
                $(questionContainer).find("select").val(null).trigger("chosen:updated");
            }
            $(questionContainer).attr('data-attempt', "true");
            var errElem = $(questionContainer).find(".error-text");
            hideError(errElem);
        } else {
            if ($(questionContainer).attr('data-attempt') == "true") {
                $(questionContainer).attr('data-attempt', "false");
            }
        }
    });
    $(".survey-q2-select").chosen().change(function () {
        var qContainer = $(this).closest(".question-box"),
            errElem = $(qContainer).find(".error-text");
        $(qContainer).find("input[type = checkbox]").attr('checked', false);
        $(qContainer).attr('data-attempt', true);
        hideError(errElem);
    });
    $(".survey-select").change(function () {
        var qContainer = $(this).closest(".question-box"),
            errElem = $(qContainer).find(".error-text");
        $(qContainer).find("input[type = checkbox]").attr('checked', false);
        $(qContainer).attr('data-attempt', true);
        hideError(errElem);
    });
    $(".survey-page1__btn").on('click', function () {
        pageOneValidation();
    });
    $(".survey-page2__btn").on('click', function () {
        pageTwoValidation();
    });

    $("input[name=SeenThisAd]").click(function () {
        var conditionalQ = $(".conditional-question")
        if ($(this).val() == "Yes") {
            $(conditionalQ).show();
        } else {
            $(conditionalQ).hide();
        }
    });
    $(".survey-select-conditional").on('change', function () {
        $(this).closest(".conditional-question").attr('data-attempt', true);
    });
});

function pageOneValidation() {
    var q1 = ($(".question1").attr('data-attempt') === "true"),
        q2 = ($(".question2").attr('data-attempt') === "true"),
        q1ErrorElem = $(".error-text-q1"),
        q2ErrorElem = $(".error-text-q2");

    if (q1 && q2) {
        showPage(2);
        $(".q2-circle, .q2-line").addClass("active");
    } else if (q2) {
        showError(q1ErrorElem);
    } else if (q1) {
        showError(q2ErrorElem);
    } else {
        showError(q1ErrorElem);
        showError(q2ErrorElem);
    }
}
function pageTwoValidation() {
    var q3ErrorElem = $(".error-text-q3"),
        q4ErrorElem = $(".error-text-q4"),
        conditionalQ4a = ($("#adnumberofviews").attr('data-attempt') == "true"),
        conditionalQ4b = ($("#admedium").attr('data-attempt') == "true");
    if ($(".survey-q3-input").val().length === 0) {
        showError(q3ErrorElem);
        q3 = false;
    } else {
        q3 = true;
        hideError(q3ErrorElem);
    }
    if ($('input[name=SeenThisAd]:checked').val() == "Yes") {
        if (q3 && conditionalQ4a && conditionalQ4b) {
            showPage(3);
            $(".q3-circle, .q3-line").addClass("active");
        } else if (conditionalQ4a && conditionalQ4b) {
            hideError(q4ErrorElem);
        }
        else {
            showError(q4ErrorElem);
        }
    } else if ($('input[name=SeenThisAd]:checked').val() == "No") {
        if (q3) {
            showPage(3);
            $(".q3-circle, .q3-line").addClass("active");
        } else {
            hideError(q4ErrorElem);
        }
    } else {
        showError(q4ErrorElem);
    }
}

function Validate() {
    var q5ErrorElem = $(".error-text-q5"), q6ErrorElem = $(".error-text-q6"), q7ErrorElem = $(".error-text-q7"),q5 =false,q6=false,q7=false;
    if ($(".survey-q5-input").val().length === 0) {
        showError(q5ErrorElem);
        q5 = false;
    } else {
        q5 = true;
        hideError(q5ErrorElem);
    }

    if ($(".survey-q6-input").val().length === 0) {
        showError(q6ErrorElem);
        q6 = false;
    } else {
        q6 = true;
        hideError(q6ErrorElem);
    }

    if ($(".survey-q7-input").val().length === 0) {
        showError(q7ErrorElem);
        q7 = false;
    } else {
        q7 = true;
        hideError(q7ErrorElem);
    }
    if (q5 && q6 && q7) 
        return true;
    else 
        return false;
    
}
function showError(elem) {
    $(elem).removeClass("visibility-off").addClass("visibility-on");
}
function hideError(elem) {
    $(elem).removeClass("visibility-on").addClass("visibility-off");
}
function showPage(id) {
    $(".survey-page").css({ "display": "none" });
    $("#page" + id).show();
}

docReady(function () {
});
