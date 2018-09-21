$("#financeDetails ul.select-financeUL li div").click(function () {
    if (!$(this).hasClass("selected-finance")) {
        $("#financeDetails ul.select-financeUL li div").removeClass("selected-finance text-bold border-dark-grey").addClass("text-light-grey border-light-grey").find("span.radio-btn").removeClass("radio-btn-checked").addClass("radio-btn-unchecked");
        $(this).removeClass("text-light-grey border-light-grey").addClass("selected-finance text-bold border-dark-grey").find("span.radio-btn").removeClass("radio-btn-unchecked").addClass("radio-btn-checked");
        $("#financeDetails").find("h4.select-financeh4").removeClass("text-red");
        validateTabC = 0;
        $('#dealerDetailsTab').addClass('disabled-tab').removeClass('active-tab text-bold');
    }
    if ($(this).hasClass("finance-required")) {
        $(".finance-emi-container").show();
        $(".disclaimer-container").show();
    }
    else {
        $(".finance-emi-container").hide();
        $(".disclaimer-container").hide();
    }
});

function viewMore(id) {
    $(id).closest('li').nextAll('li').toggleClass('hide');
    $(id).text($(id).text() == '(view more)' ? '(view less)' : '(view more)');
};

var versionul = $("#customizeBike ul.select-versionUL");
var colorsul = $("#customizeBike ul.select-colorUL");

var BookingConfigViewModel = function () {
    var self = this;
    self.Bike = ko.observable(new BikeDetails);
    self.Dealer = ko.observable(new BikeDealerDetails);
    self.EMI = ko.observable(new BikeEMI);
    self.CurrentStep = ko.observable(1);
    self.IsMapLoaded = false;
    self.SelectedVersion = ko.observable();
    self.UserOptions = ko.observable();
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
                    $('html, body').animate({ scrollTop: 0 }, 300);
                }
                else if (self.CurrentStep() == 3) {
                    self.CurrentStep(4);
                    self.ActualSteps(4);
                    $('html, body').animate({ scrollTop: 0 }, 300);
                }

                if (self.CurrentStep() == 2) {
                    dataLayer.push({ 'event': 'Bikewale_all', 'cat': 'Booking_Config_Page', 'act': 'Step_1_Successful_Submit', 'lab': thisBikename + '_' + getCityArea });
                }
                else if (self.CurrentStep() == 3) {
                    dataLayer.push({ 'event': 'Bikewale_all', 'cat': 'Booking_Config_Page', 'act': 'Step_2_Successful_Submit', 'lab': thisBikename + '_' + getCityArea });
                }
                return true;

            }
            else {
                $('html, body').animate({ scrollTop: $(".select-colorh4").first().offset().top }, 300);
                $("#customizeBike .select-colorh4").addClass("text-red").shake();
                dataLayer.push({ 'event': 'Bikewale_all', 'cat': 'Booking_Config_Page', 'act': 'Step_1_Submit_Error_versionColorMissing', 'lab': thisBikename + '_' + getCityArea });
                return false;
            }
        }
        else {
            $('html, body').animate({ scrollTop: $(".select-versionh4").first().offset().top }, 300);
            $("#customizeBike .select-versionh4").addClass("text-red").shake();
            return false;
        }
    };

    self.UpdateVersion = function (data, event) {
        url = "/api/UpdatePQ/";
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
            dataType: 'json',
            success: function (response) {
                var obj = ko.toJS(response);
                if (obj.isUpdated) {
                    isSuccess = true;
                    var cookieValue = "CityId=" + cityId + "&AreaId=" + areaId + "&PQId=" + pqId + "&VersionId=" + self.Bike().selectedVersionId() + "&DealerId=" + self.Dealer().DealerId();
                    //SetCookie("_MPQ", cookieValue);                    
                    history.replaceState(null, null, "?MPQ=" + Base64.encode(cookieValue));
                }
                else isSuccess = false;
            },
            error: function (xhr, ajaxOptions, thrownError) {
                isSuccess = false;
            }
        });
    }

    self.bookNow = function (data, event) {
        var isSuccess = false;
        if (self.changedSteps() && (self.CurrentStep() > 3) && (self.Bike().bookingAmount() > 0)) {

            var curUserOptions = self.Bike().selectedVersionId().toString() + self.Bike().selectedColorId().toString();
            if (self.UserOptions() != curUserOptions) {
                self.UserOptions(curUserOptions);

                url = "/api/UpdatePQ/";
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
                    dataType: 'json',
                    success: function (response) {
                        var obj = ko.toJS(response);
                        if (obj.isUpdated) {
                            isSuccess = true;
                            var cookieValue = "CityId=" + cityId + "&AreaId=" + areaId + "&PQId=" + pqId + "&VersionId=" + self.Bike().selectedVersionId() + "&DealerId=" + self.Dealer().DealerId();
                            //SetCookie("_MPQ", cookieValue);
                            window.location.href = '/m/pricequote/bookingSummary_new.aspx?MPQ=' + Base64.encode(cookieValue);
                        }
                        else isSuccess = false;
                    },
                    error: function (xhr, ajaxOptions, thrownError) {
                        isSuccess = false;
                    }
                });
            }
            else {
                if ((self.Bike().bookingAmount() > 0)) {
                    var cookieValue = "CityId=" + cityId + "&AreaId=" + areaId + "&PQId=" + pqId + "&VersionId=" + self.Bike().selectedVersionId() + "&DealerId=" + self.Dealer().DealerId();
                    window.location.href = '/m/pricequote/bookingSummary_new.aspx?MPQ=' + Base64.encode(cookieValue);
                }
                isSuccess = true;
            }
            dataLayer.push({ 'event': 'Bikewale_all', 'cat': 'Booking_Config_Page', 'act': 'Step 3_Book_Now_Click', 'lab': thisBikename + '_' + getCityArea });
        }

        return isSuccess;

    };

    self.VersionChangeNotify = ko.computed(function () {
        if (self.SelectedVersion() != undefined && self.SelectedVersion() > 0) {
            self.EMI().exshowroomprice(self.Bike().versionPrice());
            self.EMI().loan(undefined);
        }
    });

    self.IsUserTestimonials = ko.computed(function (data, event) {
        return (self.Bike().bookingAmount() > 0) ? true : false;
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
    self.discountList = ko.observableArray(discountDetail);

    self.totalDiscount = ko.computed(function () {
        var discount = 0;
        var vlen = self.discountList().length;
        if (self.discountList() != undefined && vlen > 0) {
            for (i = 0; i < vlen ; i++) {
                discount += self.discountList()[i].Price;
            }
        }
        //console.log(discount);
        return discount;
    }, this);

    self.versionPrice = ko.computed(function () {
        var priceTxt = '';
        if (self.versionPriceBreakUp() != undefined && self.versionPriceBreakUp().length > 0) {
            var total = 0, vlen = self.versionPriceBreakUp().length;

            for (i = 0; i < vlen ; i++) {
                total += self.versionPriceBreakUp()[i].Price;
                priceTxt += (self.versionPriceBreakUp()[i].ItemName).trim() + ' + ';
            }
        }
        self.priceBreakupText('(' + priceTxt.substr(0, priceTxt.length - 2) + ')');
        return total;
    }, this);

    self.bikeName = ko.computed(function () {
        var _bikeName = '';
        if (self.selectedVersion() != undefined && self.selectedVersionId != undefined) {
            _bikeName = self.selectedVersion().Make.makeName + ' ' + self.selectedVersion().Model.ModelName + ' ' + self.selectedVersion().MinSpec.VersionName;

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
        if ((self.bookingAmount() != undefined) && (self.bookingAmount() > 0)) {
            $('#testimonialWrapper').show();
        }
        else {
            $('#testimonialWrapper').hide();
        }
    };

    self.getColor = function (data, event) {
        self.selectedColorId(data.ColorId);
        if (data.NoOfDays != -1)
            self.waitingPeriod(data.NoOfDays);
        else self.waitingPeriod(self.selectedVersion().NoOfWaitingDays);
        var ele = colorsul.find("li[colorId=" + self.selectedColorId() + "]");
        colorsul.find("li").removeClass("selected-color text-bold text-white border-dark-grey").addClass("text-light-grey border-light-grey");
        colorsul.find("li").find('span.color-title-box').removeClass().addClass('color-title-box');
        colorsul.find("li").find('span.color-availability-box').show();
        ele.removeClass("text-light-grey border-light-grey").addClass("selected-color text-bold  border-dark-grey");
        $("#customizeBike").find("h4.select-colorh4").removeClass("text-red");
        if (data.HexCode.length > 2) {
            bgcolor = ele.find('span.color-box span').first().next().css('background-color');
        }
        else {
            bgcolor = ele.find('span.color-box span').first().css('background-color');
        }
        ele.find('span.color-title-box').addClass(getContrastYIQ(bgcolor));
        ele.find('span.color-availability-box').hide();
        // }
    };
    self.getVersion(self.selectedVersionId());
}

var BikeEMI = function () {
    var self = this;
    self.exshowroomprice = ko.observable();
    self.loan = ko.observable();
    self.tenure = ko.observable(36);
    self.rateofinterest = ko.observable(14);
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
            return $.calculateEMI(self.loan(), self.tenure(), self.rateofinterest());
        },
        owner: this
    });
}

ko.bindingHandlers.googlemap = {
    update: function (element, valueAccessor) {
        if (!viewModel.IsMapLoaded && viewModel.CurrentStep() > 2) {
            value = valueAccessor(),
          latLng = new google.maps.LatLng(value.latitude, value.longitude),
          mapOptions = {
              zoom: 13,
              center: latLng,
              mapTypeId: google.maps.MapTypeId.ROADMAP
          },
          map = new google.maps.Map(element, mapOptions),
          marker = new google.maps.Marker({
              title: "Dealer's Location",
              position: latLng,
              map: map,
              animation: google.maps.Animation.DROP
          });

            google.maps.event.addListenerOnce(map, 'idle', function () {
                viewModel.IsMapLoaded = true;
            });

        }
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

ko.bindingHandlers.BikeAvailability = {
    init: function (element, valueAccessor) {
        availText = "";
        period = ko.unwrap(valueAccessor()) !== null ? valueAccessor().Days : -1;

        if (period < 0)
            period = viewModel.Bike().waitingPeriod();

        if (period >= 0) {
            if (period == 1) {
                availText = "<span class='text-light-grey'>" + valueAccessor().CustomText + period + "  day </span>";
            }
            else if (period > 1) {
                availText = "<span class='text-light-grey'>" + valueAccessor().CustomText + period + " days </span>";
            }
            else {
                availText = "<span class='text-green text-bold'>Now available</span>";
            }
        }
        else {
            availText = "<span class='text-red text-bold'>Not available</span>";
        }

        $(element).html(availText);
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
    appendHash("viewBreakup");
});

$(".breakupCloseBtn, .blackOut-window").on('click', function (e) {
    viewBreakUpClosePopup();
    window.history.back();
});

var viewBreakUpClosePopup = function () {
    $("div#breakupPopUpContainer").hide();
    $(".blackOut-window").hide();
};

$(document).on('keydown', function (e) {
    if (e.keyCode === 27) {
        $("div.breakupCloseBtn").click();
        $("div#termsPopUpContainer").hide();
        $(".blackOut-window").hide();
    }
});

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
        //console.log(e.message);
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
        //console.log(e.message);
    }
    return price;
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


function setColor() {
    var vc = viewModel.Bike().versionColors();
    if (preSelectedColor > 0) {
        if (vc != null && vc.length > 0) {
            $.each(vc, function (key, value) {
                if (value.ColorId == preSelectedColor) {
                    viewModel.Bike().getColor(value);
                    viewModel.CurrentStep(3);
                    viewModel.ActualSteps(3);
                    viewModel.SelectedVersion(viewModel.Bike().selectedVersionId());
                    viewModel.Bike().selectedColorId(value.ColorId);
                }
            });
        }
    }
}

var viewModel = new BookingConfigViewModel;
ko.applyBindings(viewModel, $("#bookingConfig")[0]);
setColor();
viewModel.UserOptions(viewModel.Bike().selectedVersionId().toString() + viewModel.Bike().selectedColorId().toString());

$('.tnc').on('click', function (e) {
    LoadTerms($(this).attr("id"));
});

function LoadTerms(offerId) {
    $("div#termsPopUpContainer").show();
    $(".blackOut-window").show();
    if (offerId != 0 && offerId != null) {
        $('#termspinner').show();
        $('#terms').empty();
        $.ajax({
            type: "GET",
            url: "/api/Terms/?offerId=" + offerId,
            dataType: 'json',
            success: function (response) {
                if (response != null)
                    $('#terms').html(response);
            },
            error: function (request, status, error) {
                $("div#termsPopUpContainer").hide();
                $(".blackOut-window").hide();
            }
        });
    } else {
        $("#terms").load("/UI/statichtml/tnc.html");
    }
    $('#termspinner').hide();
}

$(".termsPopUpCloseBtn").on('mouseup click', function (e) {
    $("div#termsPopUpContainer").hide();
    $(".blackOut-window").hide();
});