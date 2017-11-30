var emiCaluculatorContainer, effect = 'slide', directionLeft = { direction: 'down' }, duration = 500;
var isMobile = $("#mobileEmiCalculator");
var emiPopup = $('#emiPopup');
emiCalculator = {
    open: function (element) {
        if (isMobile.length) {
            element.show(effect, directionLeft, duration, function () {
            });
            lockPopup();
        }
        else {
            element.fadeIn(100);
            popup.lock();
            //e.preventDefault();
        }
        window.history.pushState('addEMIPopup', '', '');
    },

    close: function (element) {
        if (isMobile.length) {
            element.hide(effect, directionLeft, duration, function () { });
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

$('#emiPopup .emi-popup-close-btn, .blackOut-window').mouseup(function () {
    emiCalculator.close(emiPopup);
    window.history.back();
});
$('#leadCapturePopup .leadCapture-close-btn').mouseup(function () {

    if (isMobile.length) {
        if ($('#leadCapturePopup').is(':visible')) {
            history.go(-2)
        }
    }
    else {
        history.go(-1)
    }

});
$(window).on('popstate', function (event) {
    if (emiPopup.is(':visible')) {
        emiCalculator.close(emiPopup);
    }
    if ($('#leadCapturePopup').is(':visible')) {
        emiCalculator.close($('#leadCapturePopup'));
    }
});


/* emi calculator */
bikeVersionPrice = "69278";

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

var BikeEMI = function () {
    var self = this;
    self.breakPoints = ko.observable(5);
    self.bikePrice = ko.observable(bikeVersionPrice);
    self.minDnPay = ko.observable(10 * bikeVersionPrice / 100);
    self.maxDnPay = ko.observable(40 * bikeVersionPrice / 100);
    self.minTenure = ko.observable(12);
    self.maxTenure = ko.observable(48);
    self.minROI = ko.observable(10);
    self.maxROI = ko.observable(15);

    self.processingFees = ko.observable(0);
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
            self.loan((($.LoanAmount(self.exshowroomprice(), 100))) - value);
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
            return (self.downPayment() + (self.monthlyEMI() * self.tenure()));
        },
        owner: this
    });
};

$.calculateEMI = function (loanAmount, tenure, rateOfInterest, proFees) {
    var interest, totalRepay, finalEmi;
    try {
        interest = (loanAmount * tenure * rateOfInterest) / (12 * 100);
        totalRepay = loanAmount + interest + proFees;
        finalEmi = Math.round((totalRepay / tenure));
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
    EMIviewModel = new BikeEMI;
    ko.applyBindings(EMIviewModel, $("#emiPopup")[0]);
} catch (e) {
    console.log(e.message);
}

$("#btnEmiQuote").on('click', function () {
    emiPopup.fadeOut(100);
});