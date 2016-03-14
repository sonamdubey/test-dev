var variantsDropdown = $(".variants-dropdown"),
variantSelectionTab = $(".variant-selection-tab"),
variantUL = $(".variants-dropdown-list"),
variantListLI = $(".variants-dropdown-list li");

variantsDropdown.click(function (e) {
    if (!variantsDropdown.hasClass("open"))
        $.variantChangeDown(variantsDropdown);
    else
        $.variantChangeUp(variantsDropdown);
});

$.variantChangeDown = function (variantsDropdown) {
    variantsDropdown.addClass("open");
    variantUL.show();
};

$.variantChangeUp = function (variantsDropdown) {
    variantsDropdown.removeClass("open");
    variantUL.slideUp();
};

//TODO handle the version selection event

$(document).mouseup(function (e) {
    if (!$(".variants-dropdown, .variant-selection-tab, .variant-selection-tab #upDownArrow").is(e.target)) {
        $.variantChangeUp($(".variants-dropdown"));
    }
});

var assistanceGetName = $('#assistanceGetName'),
    assistanceGetEmail = $('#assistanceGetEmail'),
    assistanceGetMobile = $('#assistanceGetMobile');

$('#buyingAssistanceSubmitBtn').on('click', function () {
    if (validateUserInfo(assistanceGetName, assistanceGetEmail, assistanceGetMobile)) {
    }
});

var validateUserInfo = function (leadUsername, leadEmailId, leadMobileNo) {
    var isValid = true;
    isValid = validateUserName(leadUsername);
    isValid &= validateEmailId(leadEmailId);
    isValid &= validateMobileNo(leadMobileNo);
    return isValid;
};

var validateUserName = function (leadUsername) {
    var isValid = true,
		nameLength = leadUsername.val().length;
    if (leadUsername.val().indexOf('&') != -1) {
        setError(leadUsername, 'Invalid name');
        isValid = false;
    }
    else if (nameLength == 0) {
        setError(leadUsername, 'Please enter your name');
        isValid = false;
    }
    else if (nameLength >= 1) {
        hideError(leadUsername);
        isValid = true;
    }
    return isValid;
};

var validateEmailId = function (leadEmailId) {
    var isValid = true,
		emailVal = leadEmailId.val(),
		reEmail = /^[A-z0-9._+-]+@[A-z0-9.-]+\.[A-z]{2,6}$/;
    if (emailVal == "") {
        setError(leadEmailId, 'Please enter email id');
        isValid = false;
    }
    else if (!reEmail.test(emailVal)) {
        setError(leadEmailId, 'Invalid Email');
        isValid = false;
    }
    return isValid;
};

var validateMobileNo = function (leadMobileNo) {
    var isValid = true,
		mobileVal = leadMobileNo.val(),
		reMobile = /^[0-9]{10}$/;
    if (mobileVal == "") {
        setError(leadMobileNo, "Please enter your mobile no.");
        isValid = false;
    }
    else if (!reMobile.test(mobileVal) && isValid) {
        setError(leadMobileNo, "Mobile no. should be 10 digits");
        isValid = false;
    }
    else
        hideError(leadMobileNo)
    return isValid;
};

var setError = function (element, msg) {
    element.addClass("border-red").siblings("span.errorIcon, div.errorText").show();
    element.siblings("div.errorText").text(msg);
};

var hideError = function (element) {
    element.removeClass("border-red").siblings("span.errorIcon, div.errorText").hide();
};

var prevEmail = "",
	prevMobile = "";

$("#assistanceGetName").on("focus", function () {
    hideError($(this));
});

$("#assistanceGetEmail").on("focus", function () {
    hideError($(this));
    prevEmail = $(this).val().trim();
});

$("#assistanceGetMobile").on("focus", function () {
    hideError($(this));
    prevMobile = $(this).val().trim();
});

$(document).ready(function () {
    var $window = $(window),
        disclaimerText = $('#disclaimerText'),
        PQDealerSidebarContainer = $('#PQDealerSidebarContainer'),
        dealerPriceQuoteContainer = $('#dealerPriceQuoteContainer'),
        PQDealerSidebarHeight;
    $(window).scroll(function () {
        PQDealerSidebarHeight = PQDealerSidebarContainer.height();
        var windowScrollTop = $window.scrollTop(),
            disclaimerTextOffset = disclaimerText.offset(),
            dealerPriceQuoteContainerOffset = dealerPriceQuoteContainer.offset();
        if (windowScrollTop < dealerPriceQuoteContainerOffset.top - 50) {
            PQDealerSidebarContainer.css({ 'position': 'relative', 'top': '0', 'right': '0' })
        }
        else if (windowScrollTop > (disclaimerTextOffset.top - PQDealerSidebarHeight - 80)) {
                PQDealerSidebarContainer.css({ 'position': 'relative', 'top': disclaimerTextOffset.top - PQDealerSidebarHeight - 150, 'right': '0' })
            }
            else {
                PQDealerSidebarContainer.css({ 'position': 'fixed', 'top': '50px', 'right': '187px' })
            }
    });
});

$(document).ready(function (e) {

    sliderComponentA = $("#downPaymentSlider").slider({
        range: "min",
        min: 0,
        max: 1000000,
        step: 50000,
        value: 50000,
        slide: function (e, ui) {
            changeComponentBSlider(e, ui);
        },
        change: function (e, ui) {
            changeComponentBSlider(e, ui);
        }
    })

    sliderComponentB = $("#loanAmountSlider").slider({
        range: "min",
        min: 0,
        max: 1000000,
        step: 50000,
        value: 1000000 - $('#downPaymentSlider').slider("option", "value"),
        slide: function (e, ui) {
            changeComponentASlider(e, ui);
        },
        change: function (e, ui) {
            changeComponentASlider(e, ui);
        }
    });

    $("#tenureSlider").slider({
        range: "min",
        min: 12,
        max: 84,
        step: 6,
        value: 36,
        slide: function (e, ui) {
            $("#tenurePeriod").text(ui.value);
        }
    });

    $("#rateOfInterestSlider").slider({
        range: "min",
        min: 0,
        max: 20,
        step: 0.25,
        value: 5,
        slide: function (e, ui) {
            $("#rateOfInterestPercentage").text(ui.value);
        }
    });

    $("#downPaymentAmount").text($("#downPaymentSlider").slider("value"));
    $("#loanAmount").text($("#loanAmountSlider").slider("value"));
    $("#tenurePeriod").text($("#tenureSlider").slider("value"));
    $("#rateOfInterestPercentage").text($("#rateOfInterestSlider").slider("value"));

});

function changeComponentBSlider(e, ui) {
    if (!e.originalEvent) return;
    var totalAmount = 1000000;
    var amountRemaining = totalAmount - ui.value;
    $('#loanAmountSlider').slider("option", "value", amountRemaining);
    $("#loanAmount").text(amountRemaining);
    $("#downPaymentAmount").text(ui.value);
};

function changeComponentASlider(e, ui) {
    if (!e.originalEvent) return;
    var totalAmount = 1000000;
    var amountRemaining = totalAmount - ui.value;
    $('#downPaymentSlider').slider("option", "value", amountRemaining);
    $("#downPaymentAmount").text(amountRemaining);
    $("#loanAmount").text(ui.value);
};