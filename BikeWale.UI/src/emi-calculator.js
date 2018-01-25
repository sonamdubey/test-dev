var emiCaluculatorContainer;
var isMobile = $("#mobileEmiCalculator");
var emiPopup = $('#emiPopup');
emiCalculator = {
    setting: {
        effect: 'slide',
        directionleft: {'direction':'down'},
        duration: 500

    },
    open: function (element) {
        if (isMobile.length) {
            element.show(emiCalculator.setting.effect, emiCalculator.setting.directionleft, emiCalculator.setting.duration, function () {
            });
            lockPopup();
        }
        else {
            element.fadeIn(100);
            popup.lock();
        }
        window.history.pushState('addEMIPopup', '', '');
    },

    close: function (element) {
        if (isMobile.length) {
            element.hide(emiCalculator.setting.effect, emiCalculator.setting.directionleft, emiCalculator.setting.duration, function () { });
            unlockPopup();
        }
        else {
            popup.unlock();
            element.fadeOut(100);
        }
    }
};
$('body').on('click', "#emiCalculatorLink", function (e) {
    emiCalculator.open(emiPopup);
});

$('body').on('keypress', function (e) {
    if (e.keyCode === 27 && $("#emiPopup") && $("#emiPopup").is(":visible")) {
        emiCalculator.close(emiPopup);
        window.history.back();
    }
});

$('#emiPopup .emi-popup-close-btn, .blackOut-window').mouseup(function () {
    if (emiPopup.is(':visible')) {
        emiCalculator.close(emiPopup);
        window.history.back();
    }
});
$('#leadCapturePopup .leadCapture-close-btn').mouseup(function () {

    if (isMobile.length) {
        if ($('#leadCapturePopup').is(':visible')) {
            history.go(-2)
        }
    }
});
$(window).on('popstate', function (event) {
    if (emiPopup.is(':visible')) {
        emiCalculator.close(emiPopup);
    }
    if ($('#leadCapturePopup').is(':visible') && window.history.state !== "leadCapture") {
        emiCalculator.close($('#leadCapturePopup'));
    }
});
$(document).keyup(function (e) {
    if (e.keyCode == 27) {
        if (emiPopup.is(':visible')) {
            emiCalculator.close(emiPopup);
            window.history.back();
        }
    }
});

ko.bindingHandlers.slider = {
    init: function (element, valueAccessor, allBindingsAccessor, bindingContext) {
        var options = allBindingsAccessor().sliderOptions || {};
        $("#" + element.id).slider(options);
        ko.utils.registerEventHandler("#" + element.id, "slide", function (event, ui) {
            try {
                var obj = $("#" + element.id);
                if (obj.attr("data-lab") !== undefined) {
                    triggerGA(obj.attr("data-cat"), obj.attr("data-act"), obj.attr("data-lab"));
                }
                else if (obj.attr("data-var") !== undefined) {
                    triggerGA(obj.attr("data-cat"), obj.attr("data-act"), window[obj.attr("data-var")]);
                }
                else if (obj.attr("data-func") !== undefined) {
                    triggerGA(obj.attr("data-cat"), obj.attr("data-act"), eval(obj.attr("data-func") + '()'));
                }
            }
            catch (e) {
            }
            var observable = valueAccessor();
            observable(ui.value);
        });
    },
    update: function (element, valueAccessor, allBindingsAccessor, bindingContext) {
        var options = allBindingsAccessor().sliderOptions || {};
        $("#" + element.id).slider(options);
        var value = ko.utils.unwrapObservable(valueAccessor());
        if (isNaN(value)) value = 0;
        $("#" + element.id).slider("value", value);
    }
};

var BikeEMI = function (bikeVersionPrice,emiObj) {
    var self = this;
    self.minDownPayment = emiObj.minDownPayment === undefined ? (10 * bikeVersionPrice / 100) : emiObj.minDownPayment;
    self.maxDownPayment = emiObj.maxDownPayment === undefined ? (40 * bikeVersionPrice / 100) : emiObj.maxDownPayment;
    self.minTen = emiObj.minTenure === undefined ? 12 : emiObj.minTenure;
    self.maxTen = emiObj.maxTenure === undefined ? 48 : emiObj.maxTenure;
    self.minRateOfInterest = emiObj.minRateOfInterest === undefined ? 10 : emiObj.minRateOfInterest;
    self.maxRateOfInterest = emiObj.maxRateOfInterest === undefined ? 15 : emiObj.maxRateOfInterest;
    self.processingFee = emiObj.processingFee === undefined ? 0 : emiObj.processingFee;
    self.breakPoints = ko.observable(5);
    self.bikePrice = ko.observable(bikeVersionPrice);
    self.minDnPay = ko.observable(self.minDownPayment);
    self.maxDnPay = ko.observable(self.maxDownPayment);
    self.minTenure = ko.observable(self.minTen);
    self.maxTenure = ko.observable(self.maxTen);
    self.minROI = ko.observable(self.minRateOfInterest);
    self.maxROI = ko.observable(self.maxRateOfInterest);

    self.processingFees = ko.observable(self.processingFee);
    self.exshowroomprice = ko.observable(bikeVersionPrice);
    self.loan = ko.observable();

    self.tenure = ko.observable((self.maxTenure() - self.minTenure()) / 2 + self.minTenure());
    self.rateofinterest = ko.observable((self.maxROI() - self.minROI()) / 2 + self.minROI());
    self.downPayment = ko.pureComputed({
        read: function () {
            if (self.loan() == undefined || isNaN(self.loan()) || self.loan() == null)
                self.loan($.LoanAmount(self.exshowroomprice(), 70));
            return (($.LoanAmount(self.exshowroomprice(), 100)) - self.loan());
        },
        write: function (value) {
            self.loan(($.LoanAmount(self.exshowroomprice(), 100)) - value);
        },
        owner: this
    });

    self.monthlyEMI = ko.pureComputed({
        read: function () {
            var calculatedEMI = $.calculateEMI(self.loan(), self.tenure(), self.rateofinterest(), self.processingFees());
            if ($("#spnEMIAmount") && $("#spnEMIAmount").length > 0 && calculatedEMI != "0") $("#spnEMIAmount").text(formatPrice(calculatedEMI)); else { $("#spnEMIAmount").parent().addClass("hide"); }
            return calculatedEMI;
        },
        owner: this
    });
    self.totalPayable = ko.pureComputed({
        read: function () {
            return (self.downPayment() + (self.monthlyEMI() * self.tenure()) + self.processingFees());
        },
        owner: this
    });
};

$.calculateEMI = function (loanAmount, tenure, rateOfInterest, proFees) {
    var finalEmi;
    try {
        finalEmi = Math.round((loanAmount * rateOfInterest / 1200) / (1 - Math.pow((1 + rateOfInterest / 1200), (-1.0 * tenure))));
    }
    catch (e) {
    }
    return finalEmi;
};

$.LoanAmount = function (onRoadPrice, percentage) {
    var price;
    try {
        price = (onRoadPrice * percentage) / 100;
        price = Math.round(price);
    }
    catch (e) {
    }
    return price;
};

$.createSliderPoints = function (index, min, max, breaks, sliderType) {
    var svar = "";
    try {
        switch (sliderType) {
            case 1:
                svar = $.valueFormatter(Math.round(min + (index * (max - min) / breaks)));
                break;
            case 2:
                svar = Math.round(min + (index * (max - min) / breaks));
                break;
            default:
                svar = (min + (index * (max - min) / breaks)).toFixed(2);
                break;
        }
    } catch (e) {

    }
    return svar;
};

$.valueFormatter = function (num) {
    if (isNaN(num)) {
        if (num >= 100000) {
            return (num / 100000).toFixed(1).replace(/\.0$/, '') + 'L';
        }
        if (num >= 1000) {
            return (num / 1000).toFixed(1).replace(/\.0$/, '') + 'K';
        }
    }
    return num;
};

var EMIviewModel;

try {
    var emiPopupWidget = $("#emiPopup");
    if (emiPopupWidget && emiPopupWidget.length > 0) {
        bikeVersionPrice = emiPopupWidget.data("bikeversionprice");
        var emiObj = $("#emiWidgetInitialValues").val() ? JSON.parse(atob($("#emiWidgetInitialValues").val())) : {};
        EMIviewModel = new BikeEMI(bikeVersionPrice, emiObj);
        ko.applyBindings(EMIviewModel, emiPopupWidget[0]);
    }
} catch (e) {
    console.log(e.message);
}

$("#btnEmiQuote").on('click', function () {
    emiPopup.fadeOut(100);
    if (isMobile) {
        unlockPopup();
    }
    
});