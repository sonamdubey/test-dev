
var versionul = $("#customizeBike ul.select-versionUL");
var colorsul = $("#customizeBike ul.select-colorUL");

var BookingConfigViewModel = function () {
    var self = this;
    self.Bike = ko.observable(new BikeDetails);
    self.Dealer = ko.observable(new BikeDealerDetails);
    self.EMI = ko.observable(new BikeEMI);
    self.CurrentStep = ko.observable(1);
    self.SelectedVersion = ko.observable();
    self.selectedColorId = ko.observable(0);
    self.ActualSteps = ko.observable(1);
    self.changedSteps = function () {
        if (self.Bike().selectedVersionId() > 0) {
            if (self.Bike().selectedColorId() > 0) {
                self.SelectedVersion(self.Bike().selectedVersionId());
                self.selectedColorId(self.Bike().selectedColorId());
                if (self.CurrentStep() != 3) {
                    self.CurrentStep(self.CurrentStep() + 1);
                    self.ActualSteps(self.ActualSteps() + 1)
                }

            }
            else {
                $("#customizeBike .select-colorh4").addClass("text-red");
                return false;
            }
        }
        else {
            $("#customizeBike .select-versionh4").addClass("text-red");
            return false;
        }

    };
}

var BikeEMI = function () {
    var self = this;
    self.MinPrice = ko.observable();
    self.MaxPrice = ko.observable();
}

var BikeDetails = function () {
    var self = this;
    self.bikeVersions = ko.observableArray(versionList);
    self.selectedVersionId = ko.observable(bikeVersionId);
    self.selectedVersion = ko.observable();
    self.versionPriceBreakUp = ko.observableArray([]);
    self.bookingAmount = ko.observable();
    self.waitingPeriod = ko.observable();
    self.selectedColorId = ko.observable();
    self.isInsuranceFree = ko.observable(insFree);
    self.insuranceAmount = ko.observable(insAmt);

    self.versionPrice = ko.computed(function () {
        var total = 0;
        for (i = 0; i < self.versionPriceBreakUp().length; i++) {
            total += self.versionPriceBreakUp()[i].Price;
        }
        return total;
    }, this);

    self.bikeName = ko.computed(function () {
        var _bikeName = '';
        if (self.selectedVersion() != undefined && self.selectedVersionId != undefined)
        {
            _bikeName = self.selectedVersion().Make.MakeName + ' ' + self.selectedVersion().Model.ModelName + ' ' + self.selectedVersion().MinSpec.VersionName;

        }
        return _bikeName;
    }, this);

    self.versionColors = ko.observableArray([]);
    self.priceListBreakup = ko.observableArray([]);
    self.versionSpecs = ko.observable();
    self.getVersion = function (data, event) {
        self.selectedVersionId(data);
        self.selectedColorId(0);

        $.each(self.bikeVersions(), function (key, value) {
            if (self.selectedVersionId() != undefined && self.selectedVersionId() > 0 && self.selectedVersionId() == value.MinSpec.VersionId) {
                self.selectedVersion(value);
                self.versionColors(value.BikeModelColors);
                self.versionSpecs(value.MinSpec);
                self.versionPriceBreakUp(value.PriceList);
                self.waitingPeriod(value.NoOfWaitingDays);
                self.bookingAmount(value.BookingAmount); 
                versionul.find("li").removeClass("selected-version text-bold border-dark-grey").addClass("text-light-grey border-light-grey").find("span.radio-btn").removeClass("radio-sm-checked").addClass("radio-sm-unchecked");
                versionul.find("li[versionId=" + self.selectedVersionId() + "]").removeClass("text-light-grey border-light-grey").addClass("selected-version text-bold border-dark-grey").find("span.radio-btn").removeClass("radio-sm-unchecked").addClass("radio-sm-checked");
                $("#customizeBike").find("h4.select-versionh4").removeClass("text-red");
                $("#selectedVersionId").val(self.selectedVersionId());
            }
        });
    };

    self.getColor = function (data, event) {
        self.selectedColorId(data.Id);
        var ele = colorsul.find("li[colorId=" + self.selectedColorId() + "]");
        colorsul.find("li").removeClass("selected-color text-bold text-white border-dark-grey").addClass("text-light-grey border-light-grey");
        colorsul.find("li").find('span.color-title-box').removeClass().addClass('color-title-box');
        ele.removeClass("text-light-grey border-light-grey").addClass("selected-color text-bold  border-dark-grey");
        $("#customizeBike").find("h4.select-colorh4").removeClass("text-red");
        bgcolor = ele.find('span.color-box').css('background-color');
        ele.find('span.color-title-box').addClass(getContrastYIQ(bgcolor));

        // }
    };
    self.getVersion(self.selectedVersionId());
}

ko.bindingHandlers.googlemap = {
    init: function (element, valueAccessor) {
        var
          value = valueAccessor(),
          latLng = new google.maps.LatLng(value.latitude, value.longitude),
          mapOptions = {
              zoom: 10,
              center: latLng,
              mapTypeId: google.maps.MapTypeId.ROADMAP
          },
          map = new google.maps.Map(element, mapOptions),
          marker = new google.maps.Marker({
              position: latLng,
              map: map
          });
    }
};

ko.bindingHandlers.CurrencyText = {
    update: function (element, valueAccessor) {
        var amount = valueAccessor();
        var formattedAmount = ko.unwrap(amount) !== null ? formatPrice(amount) : 0;
        $(element).text(formattedAmount);
    }
};

function formatPrice(price) {
    price = price.toString();
    var lastThree = price.substring(price.length - 3);
    var otherNumbers = price.substring(0, price.length - 3);
    if (otherNumbers != '')
        lastThree = ',' + lastThree;
    var price = otherNumbers.replace(/\B(?=(\d{2})+(?!\d))/g, ",") + lastThree;
    return price;
}

$("#configBtnWrapper").on('click', 'span.viewBreakupText', function () {
    $("div#breakupPopUpContainer").show();
    $(".blackOut-window").show();
});

$(".breakupCloseBtn,.blackOut-window").on('mouseup click', function (e) {
    $("div#breakupPopUpContainer").hide();
    $(".blackOut-window").hide();
});

$(document).on('keydown', function (e) {
    if (e.keyCode === 27) {
        $("div.breakupCloseBtn").click();
    }
});

$("#financeDetails ul.select-financeUL li").click(function () {
    if (!$(this).hasClass("selected-finance")) {
        $("#financeDetails ul.select-financeUL li").removeClass("selected-finance text-bold border-dark-grey").addClass("text-light-grey border-light-grey").find("span.radio-btn").removeClass("radio-sm-checked").addClass("radio-sm-unchecked");
        $(this).removeClass("text-light-grey border-light-grey").addClass("selected-finance text-bold border-dark-grey").find("span.radio-btn").removeClass("radio-sm-unchecked").addClass("radio-sm-checked");
        $("#financeDetails").find("h4.select-financeh4").removeClass("text-red");
        validateTabC = 0;
        $('#dealerDetailsTab').addClass('disabled-tab').removeClass('active-tab text-bold');
    }
});

$("#financeDetails ul.select-financeUL li").click(function () {
    if ($(this).hasClass("finance-required"))
        $(".finance-emi-container").show();
    else $(".finance-emi-container").hide();
});

var sliderComponentA, sliderComponentB;

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

function getContrastYIQ(colorCode) {

    if (/rgb/i.test(colorCode)) {
        l = colorCode.length;
        colorCode = colorCode.substr(4, l - 1);
        b = colorCode.split(",");
        R = parseInt(b[0], 16);
        G = parseInt(b[1], 16);
        B = parseInt(b[2], 16);

        yiq = ((R * 299) + (G * 587) + (B * 114)) / 1000;
        return (yiq >= 300) ? 'text-black' : 'text-white';
        //return Brightness(Math.sqrt(R * R * .241 + G * G * .691 + B * B * .068)) < 130 ? '#FFFFFF' : '#000000';

    }
    else if (/#/i.test(colorCode)) {
        vl = colorCode.length;
        colorCode = colorCode.substr(1, l - 1);
        r = parseInt(colorCode.substr(0, 2), 16);
        g = parseInt(colorCode.substr(2, 2), 16);
        b = parseInt(colorCode.substr(4, 2), 16);
        yiq = ((r * 299) + (g * 587) + (b * 114)) / 1000;
        return (yiq >= 300) ? 'text-black' : 'text-white';

    }
    else {
        return "text-light-grey";
    }

}


var viewModel = new BookingConfigViewModel;
ko.applyBindings(viewModel, $("#bookingConfig")[0]);



