
var versionul = $("#customizeBike ul.select-versionUL");
var colorsul = $("#customizeBike ul.select-colorUL");


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

ko.bindingHandlers.slider = {
    init: function (element, valueAccessor, allBindingsAccessor, bindingContext) {
        var options = allBindingsAccessor().sliderOptions || {};
        $("#" + element.id).slider(options);
        ko.utils.registerEventHandler("#" + element.id, "slide", function (event, ui) {
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
                    self.ActualSteps(self.ActualSteps() + 1);
                }
                else if (self.CurrentStep() == 3) self.ActualSteps(4);
                return true;
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

    self.bookNow = function (data, event)
    {
        var isSuccess = false;
        if (self.changedSteps() && (self.ActualSteps() > 3) && (self.Bike().bookingAmount() > 0)) {
            
            url =  "/api/UpdatePQ/";
            var objData = {
                "pqId": pqId,
                "versionId": self.Bike().selectedVersionId(),
            }
            $.ajax({
                type: "POST",
                url: (self.Bike().selectedColorId() > 0) ? url + "?colorId=" + self.Bike().selectedColorId() : url,
                async: true,
                data: ko.toJSON(objData),
                contentType: "application/json",
                success: function (response) {
                    var obj = ko.toJS(response);
                    if (obj.isUpdated)
                    {
                        isSuccess = true;
                        var cookieValue = "CityId=" + cityId + "&AreaId=" + areaId + "&PQId=" + pqId + "&VersionId=" + self.Bike().selectedVersionId() + "&DealerId=" + self.Dealer().DealerId();
                        SetCookie("_MPQ", cookieValue);
                        window.location = '/pricequote/bookingSummary_new.aspx';
                    }                        
                    else isSuccess = false;
                },
                error: function (xhr, ajaxOptions, thrownError) {
                    isSuccess = false;
                }
            });

            return isSuccess;
        }
        
        return isSuccess;

    }

    self.VersionChangeNotify = ko.computed(function () {
        if (self.SelectedVersion() != undefined && self.SelectedVersion() > 0) {
            self.EMI().exshowroomprice(self.Bike().versionPrice());
            self.EMI().loan(undefined);
        }
    });

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
    self.priceBreakupText = ko.observable();

    self.versionPrice = ko.computed(function () {
        var priceTxt = '';
        if(self.versionPriceBreakUp()!=undefined && self.versionPriceBreakUp().length > 0)
        {
            var total = 0, vlen = self.versionPriceBreakUp().length;
            
            for (i = 0; i < vlen ; i++) {
                total += self.versionPriceBreakUp()[i].Price;
                priceTxt += (self.versionPriceBreakUp()[i].ItemName).trim() + ' + ';
            }
        }
        self.priceBreakupText('(' + priceTxt.substr(0,priceTxt.length-2) + ')');
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

var BikeEMI = function () {
    var self = this;
    self.exshowroomprice = ko.observable();
    self.loan = ko.observable();

    self.tenure = ko.observable(36);
    self.rateofinterest = ko.observable(10);
    self.downPayment = ko.pureComputed({
        read: function () {
            console.log("loan : " + self.loan() + " exshowroom : " + self.exshowroomprice());
            if (self.loan() == undefined || isNaN(self.loan()) || self.loan() == null)
                self.loan($.LoanAmount(self.exshowroomprice(), 50));
            return (($.LoanAmount(self.exshowroomprice(), 100)) - self.loan());
        },
        write: function (value) {
            self.loan((($.LoanAmount(self.exshowroomprice(), 100))) - value);
        },
        owner: this
    });

    self.monthlyEMI = ko.pureComputed({
        read: function () {
            return $.calculateEMI(self.loan(), self.tenure(), self.rateofinterest());
        },
        owner: this
    });
}

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

$.calculateEMI = function (loanAmount, tenure, rateOfInterest) {
    var interest, totalRepay, finalEmi;
    try {
        interest = (loanAmount * tenure * rateOfInterest) / (12 * 100);
        totalRepay = loanAmount + interest;
        finalEmi = Math.ceil((totalRepay / tenure));
    }
    catch (e) {
        console.log(e.message);
    }
    return formatPrice(finalEmi);
};

$.LoanAmount = function (onRoadPrice, percentage) {
    var price;
    try {
        price = (onRoadPrice * percentage) / 100;
        price = Math.ceil(price / 100.0) * 100;
    }
    catch (e) {
        console.log(e.message);
    }
    return price;
};

$.priceRange = function (minRange, maxRange, divideIndex) {
    var priceRange;
    try {
        if (divideIndex == 1)
            priceRange = maxRange;
        else if (divideIndex > 0)
            priceRange = ((maxRange - minRange) * divideIndex);
        else
            priceRange = minRange;
        priceRange = Math.ceil(priceRange / 100.0) * 100;
    } catch (e) {
        console.log(e.message);
    }
    return formatPrice(priceRange);
};

$.valueFormatter = function (num) {
    if (num >= 100000) {
        return (num / 100000).toFixed(1).replace(/\.0$/, '') + 'L';
    }
    if (num >= 1000) {
        return (num / 1000).toFixed(1).replace(/\.0$/, '') + 'K';
    }
    return num;
}




var viewModel = new BookingConfigViewModel;
ko.applyBindings(viewModel, $("#bookingConfig")[0]);



