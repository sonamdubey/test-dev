var effect = 'slide',
    options = { direction: 'right' },
    duration = 500,
    citySearchInput = $('#getCityInput');


var counter = 0, listItemCount = 7;
var budgetArr = [
	{ amount: "0", value: 0 },
	{ amount: "10K", value: 10000 },
	{ amount: "20K", value: 20000 },
	{ amount: "35K", value: 35000 },
	{ amount: "50K", value: 50000 },
	{ amount: "80K", value: 80000 },
	{ amount: "1.25L", value: 125000 },
	{ amount: "2L+", value: 200000 }
];

$(document).ready(function () {
    $('#getCityInput').fastLiveFilter('#city-slider-list');

    // set min budget list
    budgetForm.set.minList();
});

$('#more-brand-tab').click(function () {
    var target = $(this),
        moreBrands = $('#more-brand-nav');
    
    if (!target.hasClass('active')) {
        collapse.open(target, moreBrands, 'Collapse');
    }
    else {
        collapse.close(target, moreBrands, 'View all brands');
    }
});

var collapse = {
    open: function (targetLink, content, message) {
        var contentWrapper = content.closest('div');
        content.slideDown();
        targetLink.addClass('active').text(message);
        $('html, body').animate({
            scrollTop: contentWrapper.offset().top
        }, 500);
    },

    close: function (targetLink, content, message) {
        content.hide();
        targetLink.removeClass('active').text(message);
    }
};

/* city slider */
$('#search-form-city').on('click', function () {
    citySlider.open(citySearchInput);
    appendState('citySelection');
});

$('#close-city-slider').on('click', function () {
    citySlider.close();
});

$('#city-slider-list').on('click', 'li', function () {
    citySlider.selection($(this));
    citySlider.close();
});

var citySlider = {
    container: $('#city-slider'),

    open: function (inputBox) {
        citySlider.container.show(effect, options, duration, function () {
            $('html, body').addClass('lock-browser-scroll');
            citySlider.container.addClass('input-fixed');
            inputBox.focus();
        });
    },

    close: function () {
        citySlider.container.removeClass('input-fixed');
        $('html, body').removeClass('lock-browser-scroll');
        citySlider.container.hide(effect, options, duration);
    },

    selection: function (element) {
        var elementText = element.text();
        $('#search-form-city p').text(elementText);
    }
};

/* budget */
$('#min-max-budget-box').on('click', function () {
    var formElement = $(this);

    if (!formElement.hasClass('open')) {
        formElement.addClass('open');
        $('#min-input-label').trigger('click');
    }
    else {
        formElement.removeClass('open');
    }
});

$('#min-input-label').on('click', function () {
    budgetForm.labelBoxClick.minList();
});

$('#max-input-label').on('click', function () {
    budgetForm.labelBoxClick.maxList();
});

$('#min-budget-list').on('click', 'li', function () {
    var element = $(this);

    budgetForm.listItemClick.minList(element);
});

$('#max-budget-list').on('click', 'li', function () {
    var element = $(this);

    budgetForm.listItemClick.maxList(element);
});


var budgetForm = {

    content: $('#min-max-budget-box'),

    dropdown: {
        defaultLabel: $('#budget-default-label'),

        selectedMinAmount: $('#min-amount'),

        selectedMaxAmount: $('#max-amount')
    },

    minInputBox: $('#min-input-label'),

    minBudgetList: $('#min-budget-list'),

    maxInputBox: $('#max-input-label'),

    maxBudgetList: $('#max-budget-list'),

    set: {
        minList: function () {
            var i;
            for (i in budgetArr) {
                if (counter < listItemCount) {
                    budgetForm.minBudgetList.append("<li data-value=" + budgetArr[i].value + ">" + budgetArr[i].amount + "</li>");
                    counter++;
                }
            }
        },

        maxList: function () {
            // if min amount is from min-list
            if (!budgetForm.maxBudgetList.hasClass('refere-min-list')) {
                var defaultValue = 10000;
                budgetForm.generateMaxList(defaultValue);
            }
        }
    },

    labelBoxClick: {
        minList: function () {
            budgetForm.maxBudgetList.hide();
            budgetForm.minBudgetList.show();

            $('#search-form-budget').removeClass('max-active');
        },

        maxList: function () {
            budgetForm.set.maxList();

            budgetForm.minBudgetList.hide();
            budgetForm.maxBudgetList.show();

            $('#search-form-budget').addClass('max-active');
        }
    },

    listItemClick: {
        minList: function (element) {
            var dataValue = element.attr('data-value');
            
            budgetForm.minInputBox.attr('data-value', dataValue);

            budgetForm.inputBoxAmount.minAmount(element);

            budgetForm.generateMaxList(dataValue);

            budgetForm.minBudgetList.hide();
            if (dataValue != '125000') { // second last element of budget list
                budgetForm.maxBudgetList.show().addClass('refere-min-list');
            }
            else {
                budgetForm.maxBudgetList.addClass('refere-min-list').find('li').trigger('click');
            }

            $('#search-form-budget').addClass('max-active');
        },

        maxList: function (element) {
            var dataValue = element.attr("data-value");

            budgetForm.maxInputBox.attr('data-value', dataValue);

            if (typeof (budgetForm.minInputBox.attr('data-value')) == 'undefined') {
                budgetForm.minInputBox.attr('data-value', 0);
                budgetForm.dropdown.selectedMinAmount.html('0');
                budgetForm.dropdown.defaultLabel.hide();
            }

            budgetForm.inputBoxAmount.maxAmount(element);

            budgetForm.content.removeClass("open");
            
        }
    },

    inputBoxAmount: {
        minAmount: function (element) {
            var elementValue = element.text();
                        
            budgetForm.dropdown.selectedMinAmount.text(elementValue);
            budgetForm.dropdown.defaultLabel.hide();

            if (typeof (budgetForm.maxInputBox.attr('data-value')) == 'undefined') {
                budgetForm.dropdown.selectedMaxAmount.html("- MAX");
            }
        },

        maxAmount: function (element) {
            var elementValue = element.text();

            budgetForm.dropdown.selectedMaxAmount.text(' - ' + elementValue);
        }
    },

    generateMaxList: function (dataValue) {
        var i;

        counter = 0;
        budgetForm.maxBudgetList.empty();
        dataValue = parseInt(dataValue);
        
        for (i in budgetArr) {
            var indexValue = budgetArr[i].value;
            if (dataValue < indexValue && counter < listItemCount) {
                budgetForm.maxBudgetList.append("<li data-value=" + budgetArr[i].value + ">" + budgetArr[i].amount + "</li>");
                counter++;
            }
        }
    }
}

/* profile id */
var listingProfileId = $('#listingProfileId');

listingProfileId.on("focus", function () {
    validate.onFocus(listingProfileId);
});

/* input blur */
listingProfileId.on("blur", function () {
    validate.onBlur(listingProfileId);
});

$('#profile-id-popup-target').on('click', function () {
    profileID.open();
    appendState('profileId');    
});

$('#profile-id-popup').on('click', '.close-btn', function () {
    profileID.close();
});

$('#search-profile-id-btn').on('click', function () {
    if (validateProfileId(listingProfileId)) {
        $.ajax({
            type: "GET",
            url: "/api/used/inquiry/url/" + listingProfileId.val() + "/",
            dataType: 'json',
            success: function (data) {
                window.location = "/m" + data.url;
            },
            complete: function (xhr) {
                if (xhr.status == 400 || xhr.status == 500) {
                    validate.setError($('.boundary'), 'Please enter correct profile id');
                }
            }
        });
    }
});

function validateProfileId(inputBox) {
    var isValid = true;
    var profileId = inputBox.val();
    if (profileId == '') {
        isValid = false;
        validate.setError(inputBox, 'Please enter profile id');
    }
    else if (!(profileId.charAt(0).toLowerCase() == 's' || profileId.charAt(0).toLowerCase() == 'd')) {
        isValid = false;
        validate.setError(inputBox, 'Please enter correct profile id');
    }
    return isValid;
}

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
    }
}

var profileID = {
    popup: $('#profile-id-popup'),

    open: function () {
        profileID.popup.show();
        $('html, body').addClass('lock-browser-scroll');
    },

    close: function () {
        $('html, body').removeClass('lock-browser-scroll');
        profileID.popup.hide();
    }
};

$(document).on('click', function (e) {
    var container = $('#search-form-budget');

    if (container.find('#min-max-budget-box').hasClass('open') && $('#budget-list-box').is(':visible')) {
        if (!container.is(e.target) && container.has(e.target).length === 0) {
            $('#min-max-budget-box').trigger('click');
        }
    }
});

/* popup state */
var appendState = function (state) {
    window.history.pushState(state, '', '');
};

$(window).on('popstate', function (event) {
    if ($('#city-slider').is(':visible')) {
        citySlider.close();
    }
    if ($('#profile-id-popup').is(':visible')) {
        profileID.close();
    }
});

jQuery.fn.fastLiveFilter = function (list, options) {
    // Options: input, list, timeout, callback
    options = options || {};
    list = jQuery(list);
    var input = this;
    var lastFilter = '', noResultLen = 0;
    var noResult = '<div class="noResult">No search found!</div>';
    var timeout = options.timeout || 100;
    var callback = options.callback || function (total) {
        noResultLen = list.siblings(".noResult").length;

        if (total == 0 && noResultLen < 1) {
            list.after(noResult).show();
        }
        else if (total > 0 && noResultLen > 0) {
            $('.noResult').remove();
        }
    };

    var keyTimeout;
    var lis = list.children();
    var len = lis.length;
    var oldDisplay = len > 0 ? lis[0].style.display : "block";
    callback(len); // do a one-time callback on initialization to make sure everything's in sync

    input.change(function () {
        // var startTime = new Date().getTime();
        var filter = input.val().toLowerCase();
        var li, innerText;
        var numShown = 0;
        for (var i = 0; i < len; i++) {
            li = lis[i];
            innerText = !options.selector ?
                (li.textContent || li.innerText || "") :
                $(li).find(options.selector).text();

            if (innerText.toLowerCase().indexOf(filter) >= 0) {
                if (li.style.display == "none") {
                    li.style.display = oldDisplay;
                }
                numShown++;
            } else {
                if (li.style.display != "none") {
                    li.style.display = "none";
                }
            }
        }
        callback(numShown);
        return false;
    }).keydown(function () {
        clearTimeout(keyTimeout);
        keyTimeout = setTimeout(function () {
            if (input.val() === lastFilter) return;
            lastFilter = input.val();
            input.change();
        }, timeout);
    });
    return this; // maintain jQuery chainability
}