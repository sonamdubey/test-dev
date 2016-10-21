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

    $('.chosen-select').chosen();

    // set min budget list
    budgetForm.set.minList();

    $(window).on("scroll", function () {
        if ($(window).scrollTop() > 40)
            $('#header').removeClass("header-landing").addClass("header-fixed");
        else
            $('#header').removeClass("header-fixed").addClass("header-landing");
    });
});


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

$(document).mouseup(function (e) {
    var container = $('#search-form-budget');

    if (container.find('#min-max-budget-box').hasClass('open') && $('#budget-list-box').is(':visible')) {
        if (!container.is(e.target) && container.has(e.target).length === 0) {
            $('#min-max-budget-box').trigger('click');
        }
    }
});