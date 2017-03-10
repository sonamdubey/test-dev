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

    $('#drpCities.chosen-select').chosen();

    // set min budget
    budgetForm.set.minList();
    
    var obj = GetGlobalLocationObject();
    if (obj != null) {
        $("#drpCities").val(obj.CityId).trigger("chosen:updated");
    }
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
$('#searchCityBudget').on('click', function () {
    var drpCities = $('#drpCities option:selected');
    var cityMasking = drpCities.attr("data-citymaskingname") != null ? drpCities.attr("data-citymaskingname") : "";
    var cityId = drpCities.attr("data-item-id") != null ? drpCities.attr("data-item-id") : "";
    searchUsedVM.cityMaskingName(cityMasking);
    searchUsedVM.cityId(cityId); 
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
            searchUsedVM.minAmount(dataValue);
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
            searchUsedVM.maxAmount(dataValue);

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
            searchUsedVM.maxAmount('');
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
        budgetForm.dropdown.selectedMaxAmount.html("- MAX");
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


function searchModel() {
    var self = this;
    self.baseUrl = ko.observable(''),
    self.cityMaskingName = ko.observable(''),
    self.cityId = ko.observable(''),
    self.minAmount = ko.observable(''),
    self.maxAmount = ko.observable(''),
    self.createUrl = function () {
        if (self.minAmount() == "" && self.maxAmount() == "")
            return self.baseUrl();
        else {
                if (self.minAmount() == "")
                    return self.baseUrl() + "#budget=0+" + self.maxAmount()
                else if (self.maxAmount() == "")
                    return self.baseUrl() + "#budget=" + self.minAmount() + "+200000";
                else
                    return self.baseUrl() + "#budget=" + self.minAmount() + "+" + self.maxAmount();
        }
        return '';
    },
    self.redirectUrl = ko.computed(function () {
        
        if (self.cityMaskingName() == "") {
            self.baseUrl("bikes-in-india/");
        }
        else {
            self.baseUrl("bikes-in-" + self.cityMaskingName() + "/");
        }
        return self.createUrl();
    })


};
var searchUsedVM = new searchModel();
ko.applyBindings(searchUsedVM, document.getElementById("search-used-bikes"));
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
   
    $('#listingProfileId').val("");
});

$('#profile-id-popup').on('click', '.close-btn', function () {
    profileID.close();
});

$('#search-profile-id-btn').on('click', function () {
if (validateProfileId(listingProfileId)) {
            $.ajax({
                type: "GET",
                url: "/api/used/inquiry/url/" + listingProfileId.val() + "/" + CustomerId + "/",
                headers: {"platformId": 1},
                dataType: 'json',
                success: function (data) {
                    if (data != null) {
                        if (!data.isRedirect)
                            validate.setError(listingProfileId, data.message);
                        else
                            window.location.href = data.url;
                    }
                },
                complete: function (xhr) {
                    if (xhr.status != 200) {
                        validate.setError(listingProfileId, 'Please enter correct profile id');
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
    else if (!(/^((d|D)|(s|S))[0-9]+$/.test(profileId))) {
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
        $('.blackOut-window').show();
        $('body').addClass('lock-browser-scroll');
    },

    close: function () {
        $('body').removeClass('lock-browser-scroll');
        $('.blackOut-window').hide();
        profileID.popup.hide();
    }
};
