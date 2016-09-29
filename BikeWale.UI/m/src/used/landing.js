var effect = 'slide',
    options = { direction: 'right' },
    duration = 500,
    citySearchInput = $('#getCityInput');


var counter = 0;
var budgetArr = [
	{ amount: "0", value: 0 },
	{ amount: "30K", value: 30000 },
	{ amount: "40K", value: 40000 },
	{ amount: "50K", value: 50000 },
	{ amount: "60K", value: 60000 },
	{ amount: "70K", value: 70000 },
	{ amount: "80K", value: 80000 },
	{ amount: "90K", value: 90000 },
	{ amount: "1L", value: 100000 },
	{ amount: "1.5L", value: 150000 },
	{ amount: "2L", value: 200000 },
	{ amount: "2.5L", value: 250000 },
	{ amount: "3L", value: 300000 },
	{ amount: "3.5L", value: 350000 },
	{ amount: "5L", value: 500000 },
	{ amount: "7.5L", value: 750000 },
	{ amount: "10L", value: 1000000 },
	{ amount: "12.5L", value: 1250000 },
	{ amount: "15L", value: 1500000 },
	{ amount: "30L", value: 3000000 },
	{ amount: "60L", value: 6000000 }
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
        $('#search-form-city p').addClass('text-default').text(elementText);
    }
};

/* budget */
$('#min-max-budget-box').on('click', function () {
    var formElement = $(this);

    if (!formElement.hasClass('open')) {
        formElement.addClass('open');
    }
    else {
        formElement.removeClass('open');
    }
});

$('#min-input-box').on('click focus', function () {
    budgetForm.inputBoxFocus.minList();
});

$('#max-input-box').on('click focus', function () {
    budgetForm.set.maxList();
});

$('#min-budget-list').on('click', 'li', function () {
    var element = $(this);

    budgetForm.listItemClick.minList(element);
});

var budgetForm = {

    minInputBox: $('#min-input-box'),

    minBudgetList: $('#min-budget-list'),

    maxInputBox: $('#max-input-box'),

    maxBudgetList: $('#max-budget-list'),

    set: {
        minList: function () {
            var i;
            for (i in budgetArr) {
                if (counter < 6) {
                    budgetForm.minBudgetList.append("<li data-value=" + budgetArr[i].value + ">" + budgetArr[i].amount + "</li>");
                    counter++;
                }
            }
        },

        maxList: function () {
            if (!budgetForm.maxBudgetList.hasClass('refMinList')) {
                var defaultValue = 30000;
                budgetForm.generateMaxList(defaultValue);
            }

            budgetForm.minBudgetList.hide();
            budgetForm.maxBudgetList.show();
        }
    },

    inputBoxAmout: {
        minAmount: function (userMinAmount) {
            if (budgetForm.minInputBox.val() == "") {
                budgetForm.maxInputBox.val('').attr('data-value', '');
                $('#min-amount').html('0');
            }
            else {
                $("#budget-default-label").hide();
                var formattedValue = budgetForm.newUserInputPrice(userMinAmount);
                $('#min-amount').text(formattedValue);
                budgetForm.maxInputBox.val(formattedValue).attr('data-value', userMinAmount);
            }
            if ($("#budget-default-label").is(':visible'))
                $('#min-amount').html('');
        },
    },

    inputBoxFocus: {
        minList: function () {
            budgetForm.maxBudgetList.hide();
            budgetForm.minBudgetList.show();
        }
    },

    listItemClick: {
        minList: function (element) {
            
        }
    },

    generateMaxList: function (dataValue) {
        counter = 0;
        budgetForm.maxBudgetList.empty();
        dataValue = parseInt(dataValue);
        var i;
        for (i in budgetArr) {
            var indexValue = budgetArr[i].value;
            if (dataValue < indexValue && counter < 6) {
                budgetForm.maxBudgetList.append("<li data-value=" + budgetArr[i].value + ">" + budgetArr[i].amount + "</li>");
                counter++;
            }
        }
    }
}


/* popup state */
var appendState = function (state) {
    window.history.pushState(state, '', '');
};

$(window).on('popstate', function (event) {
    if ($('#city-slider').is(':visible')) {
        citySlider.close();
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