var redMarkerImage = 'http://imgd3.aeplcdn.com/0x0/bw/static/design15/map-marker-red.png';

$(document).ready(function () {
    dropdown.setDropdown();
});

function initializeMap() {
    var mapCanvas = document.getElementById("dealer-map");
    var mapProp = {
        scrollwheel: false,
        streetViewControl: false,
        mapTypeControl: false,
        center: new google.maps.LatLng(19.0720625, 72.8550991),
        zoom: 15,
        disableDefaultUI: true,
        mapTypeId: google.maps.MapTypeId.ROADMAP,
    };

    var map = new google.maps.Map(mapCanvas, mapProp);

    var marker = new google.maps.Marker({
        position: mapProp.center,
        icon: redMarkerImage
    });

    marker.setMap(map);
}

initializeMap();

$('.dropdown-select-wrapper').on('click', '.dropdown-label', function () {
    dropdown.active($(this));
});

$('.dropdown-select-wrapper').on('click', '.dropdown-menu-list.dropdown-with-select li', function () {
    var element = $(this);
    if (!element.hasClass('active')) {
        dropdown.selectItem($(this));
        dropdown.selectOption($(this));
    }
    else {
        element.closest('.dropdown-menu').removeClass('dropdown-active');
    }
});

var dropdown = {
    setDropdown: function () {
        var selectDropdown = $('.dropdown-select');

        selectDropdown.each(function () {
            dropdown.setMenu($(this));
        });
    },

    setMenu: function (element) {
        $('<div class="dropdown-menu"></div>').insertAfter(element);
        dropdown.setStructure(element);
    },

    setStructure: function (element) {
        var elementValue = element.find('option:selected').text(),
			menu = element.next('.dropdown-menu');
        menu.append('<p class="dropdown-label">' + elementValue + '</p><div class="dropdown-list-wrapper"><p class="dropdown-selected-item">' + elementValue + '</p><ul class="dropdown-menu-list dropdown-with-select"></ul></div>');
        dropdown.setOption(element);
    },

    setOption: function (element) {
        var selectedIndex = element.find('option:selected').index(),
			menu = element.next('.dropdown-menu'),
			menuList = menu.find('ul');

        element.find('option').each(function (index) {
            if ($(this).val() != 0 && $(this).val().length != 0) { // check for dropdown label value
                menuList.append('<li data-option-value="' + $(this).val() + '">' + $(this).text() + '</li>');
            }
        });
    },

    active: function (label) {
        $('.dropdown-select-wrapper').find('.dropdown-menu').removeClass('dropdown-active');
        label.closest('.dropdown-menu').addClass('dropdown-active');
        if (label.closest('.dropdown-select-wrapper').hasClass('invalid')) {
            validate.dropdown.hideError(label);
        }
    },

    inactive: function () {
        $('.dropdown-select-wrapper').find('.dropdown-menu').removeClass('dropdown-active');
    },

    selectItem: function (element) {
        var elementValue = element.attr('data-option-value');
        if (elementValue != 0 || elementValue.length != 0) { // check for dropdown label value
            var elementText = element.text(),
			    menu = element.closest('.dropdown-menu'),
			    dropdownLabel = menu.find('.dropdown-label'),
			    selectedItem = menu.find('.dropdown-selected-item');

            element.siblings('li').removeClass('active');
            element.addClass('active');
            selectedItem.text(elementText);
            dropdownLabel.text(elementText);
            menu.removeClass('dropdown-active');
        }
    },

    selectOption: function (element) {
        var elementValue = element.attr('data-option-value'),
			wrapper = element.closest('.dropdown-select-wrapper'),
			selectDropdown = wrapper.find('.dropdown-select');

        selectDropdown.val(elementValue).trigger('change');
    }
};

$(document).on('click', function (event) {
    event.stopPropagation();
    var dropdown = $('.dropdown-menu');

    if (dropdown.hasClass('dropdown-active')) {
        if (!dropdown.is(event.target) && dropdown.has(event.target).length == 0) {
            dropdown.removeClass('dropdown-active');
        }
    }
});

var assistanceGetName = $('#assistanceGetName'),
    assistanceGetEmail = $('#assistanceGetEmail'),
    assistanceGetMobile = $('#assistanceGetMobile'),
    assistGetModel = $('#assistGetModel');

/* input focus */
assistanceGetName.on("focus", function () {
    validate.onFocus(assistanceGetName);
});

assistanceGetEmail.on("focus", function () {
    validate.onFocus(assistanceGetEmail);
});

assistanceGetMobile.on("focus", function () {
    validate.onFocus(assistanceGetMobile);
});

/* input blur */
assistanceGetName.on("blur", function () {
    validate.onBlur(assistanceGetName);
});

assistanceGetEmail.on("blur", function () {
    validate.onBlur(assistanceGetEmail);
});

assistanceGetMobile.on("blur", function () {
    validate.onBlur(assistanceGetMobile);
});

$('#submitAssistanceFormBtn').on('click', function () {
    var isValidDetails = false;
    isValidDetails &= validateBike(assistGetModel);
    isValidDetails = ValidateUserDetail(assistanceGetName, assistanceGetEmail, assistanceGetMobile);
    if (isValidDetails) {
        // valid
    }
});

function ValidateUserDetail(fullName, emailid, mobile) {
    return validateUserInfo(fullName, emailid, mobile);
};

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
        validate.setError(leadUsername, 'Invalid name');
        isValid = false;
    }
    else if (nameLength == 0) {
        validate.setError(leadUsername, 'Please enter your name');
        isValid = false;
    }
    else if (nameLength >= 1) {
        validate.hideError(leadUsername);
        isValid = true;
    }
    return isValid;
};

var validateEmailId = function (leadEmailId) {
    var isValid = true,
		emailVal = leadEmailId.val(),
		reEmail = /^[A-z0-9._+-]+@[A-z0-9.-]+\.[A-z]{2,6}$/;
    if (emailVal == "") {
        validate.setError(leadEmailId, 'Please enter email id');
        isValid = false;
    }
    else if (!reEmail.test(emailVal)) {
        validate.setError(leadEmailId, 'Invalid Email');
        isValid = false;
    }
    return isValid;
};

var validateMobileNo = function (leadMobileNo) {
    var isValid = true,
		mobileVal = leadMobileNo.val(),
		reMobile = /^[1-9][0-9]{9}$/;
    if (mobileVal == "") {
        validate.setError(leadMobileNo, "Please enter your mobile no.");
        isValid = false;
    }
    else if (mobileVal[0] == "0") {
        validate.setError(leadMobileNo, "Mobile no. should not start with zero");
        isValid = false;
    }
    else if (!reMobile.test(mobileVal) && isValid) {
        validate.setError(leadMobileNo, "Mobile no. should be 10 digits only");
        isValid = false;
    }
    else
        validate.hideError(leadMobileNo)
    return isValid;
};

var validateBike = function (bike) {
    if (assistGetModel.val().length == 0) {
        validate.dropdown.setError(bike, 'Select a bike');
        return true;
    }
    else {
        validate.dropdown.hideError(bike);
        return false;
    }

}

/* form validation */
var validate = {
    setError: function (element, message) {
        var elementLength = element.val().length;
        errorTag = element.siblings('span.error-text');

        errorTag.show().text(message);
        if (!elementLength) {
            element.closest('.input-box').removeClass('not-empty').addClass('invalid');
        }
        else {
            element.closest('.input-box').addClass('not-empty invalid');
        }
    },

    hideError: function (element) {
        element.closest('.input-box').removeClass('invalid').addClass('not-empty');
        element.siblings('span.error-text').text('');
    },

    onFocus: function (inputField) {
        if (inputField.closest('.input-box').hasClass('invalid')) {
            validate.hideError(inputField);
        }
    },

    onBlur: function (inputField) {
        var inputLength = inputField.val().length;
        if (!inputLength) {
            inputField.closest('.input-box').removeClass('not-empty');
        }
        else {
            inputField.closest('.input-box').addClass('not-empty');
        }
    },

    dropdown: {
        setError: function (element, message) {
            var dropdownWrapper = element.closest('.dropdown-select-wrapper'),
                errorTag = dropdownWrapper.find('.error-text');

            dropdownWrapper.addClass('invalid');
            errorTag.show().text(message);
        },

        hideError: function (element) {
            var dropdownWrapper = element.closest('.dropdown-select-wrapper'),
                errorTag = dropdownWrapper.find('.error-text');

            dropdownWrapper.removeClass('invalid');
            errorTag.text('');
        }
    }
}