var dpParams = {
    makesList: [
        { makeId: 0, makeName: "Select Make" }
    ],
    dealersList: [
        { dealerId: 0, dealerName: "Select Dealer" }
    ],
    operationsList: [
        { operationId: 0, operationName: "Selected an operation" },
        { operationId: 1, operationName: "Offers" },
        { operationId: 2, operationName: "Prices and Availability" },
        { operationId: 3, operationName: "Facilities" },
        { operationId: 4, operationName: "EMI" },
        { operationId: 5, operationName: "Dealer Disclaimer" },
        { operationId: 6, operationName: "Booking Amount" },
        { operationId: 7, operationName: "Benefits/USP" }
    ]
};

if (!$('#makesListString').length == 0 && $('#makesListString').val().length > 0 ) {
    dpParams.makesList = dpParams.makesList.concat(JSON.parse(atob($('#makesListString').val())));
}

if (!$('#dealersListString').length == 0 && $('#dealersListString').val().length > 0) {
    dpParams.dealersList = dpParams.dealersList.concat(JSON.parse(atob($('#dealersListString').val())));
}

var dealerOperationModel = function (dealerOperationModelParams) {
    var self = this;
    var ddlMakes = $("#ddlMakes");
    var ddlDealers = $("#ddlDealers");
    var ddlDealerCity = $("#ddlDealerCity");
    var ddlDealerOperations = $("#ddlDealerOperations");

    self.selectedCity = null;
    self.selectedOtherCity = null;
    self.selectedMake = ko.observable();
    self.selectedDealer = ko.observable();
    self.selectedOperation = ko.observable();
    self.defaultMakesList = [
        { makeId: 0, makeName: "Select Make" }
    ];
    self.defaultDealersList = [
        { dealerId: 0, dealerName: "Select Dealer" }
    ];

    var operationReferences = {
        1: "/newbikebooking/ManageOffers.aspx?dealerId=",
        3: "/newbikebooking/ManageDealerFacilities.aspx?dealerId=",
        4: "/newbikebooking/ManageDealerLoanAmounts.aspx?dealerId=",
        5: "/newbikebooking/ManageDealerDisclaimer.aspx?dealerId=",
        6: "/newbikebooking/ManageBookingAmount.aspx?dealerId=",
        7: "/newbikebooking/ManageDealerBenefits.aspx?dealerId="
    };

    self.makes = ko.observableArray(dealerOperationModelParams.makesList);

    self.dealers = ko.observableArray(dealerOperationModelParams.dealersList);

    self.operations = ko.observableArray(dealerOperationModelParams.operationsList);

    if (!$('#makesListString').length == 0 && !$('#dealersListString').length == 0) {
        self.selectedMake($('#makeId').val());
        self.selectedDealer($('#dealerId').val());
        self.selectedCity = $('#cityId').val();
        ddlDealerCity.val(self.selectedCity);
        if (!$('#otherCityId').length == 0)
            $('#ddlOtherCity').val($('#otherCityId').val());
    }
    self.onCityChange = function (data, event) {
        self.selectedCity = event.target.value;

        if (self.selectedCity > 0) {
            progress.showProgress();
            var makesByCityURL = "/api/makes/city/" + self.selectedCity + "/";

            $.ajax({
                type: "GET",
                url: makesByCityURL,
                success: function (resultData, textStatus, xhr) {
                    if (resultData.length > 0) {
                        self.makes(self.defaultMakesList.concat(resultData));
                        ddlMakes.find('option[value=0]').attr('disabled', 'disabled');
                        self.selectedMake(undefined);
                        self.dealers(self.defaultDealersList);
                        ddlDealers.find('option[value=0]').attr('disabled', 'disabled');
                        self.selectedDealer(undefined);
                        self.selectedOperation(undefined);
                        ddlMakes.trigger("chosen:updated");
                        ddlDealers.trigger("chosen:updated");
                        ddlDealerOperations.trigger("chosen:updated");
                        progress.hideProgress();
                    }
                    else {
                        Materialize.toast('No makes returned. Please try again', 4000);
                    }
                },
                complete: function (xhr) {
                    if (xhr.status == 404) {
                        self.makes(self.defaultMakesList);
                        ddlMakes.find('option[value=0]').attr('disabled', 'disabled');
                        self.selectedMake(undefined);
                        self.dealers(self.defaultDealersList);
                        ddlDealers.find('option[value=0]').attr('disabled', 'disabled');
                        self.selectedDealer(undefined);
                        self.selectedOperation(undefined);
                        ddlMakes.trigger("chosen:updated");
                        ddlDealers.trigger("chosen:updated");
                        ddlDealerOperations.trigger("chosen:updated");
                        progress.hideProgress();
                        Materialize.toast(ddlDealerCity.find('option[value=' + self.selectedCity + ']').text() + ' has no dealers', 4000);
                    }
                }

            });
        }
    };

    self.onMakeChange = function (data, event) {
        if (self.selectedCity > 0 && self.selectedMake() > 0) {
            progress.showProgress();
            var dealersByCityAndMake = "/api/dealers/make/" + self.selectedMake() + "/city/" + self.selectedCity + "/";

            $.ajax({
                type: "GET",
                url: dealersByCityAndMake,
                success: function (resultData, textStatus, xhr) {
                    if (resultData.length > 0) {
                        self.dealers(self.defaultDealersList.concat(resultData));
                        ddlDealers.find('option[value=0]').attr('disabled', 'disabled');
                        self.selectedDealer(undefined);
                        ddlDealers.trigger("chosen:updated");
                        self.selectedOperation(undefined);
                        ddlDealerOperations.trigger("chosen:updated");
                        progress.hideProgress();
                    }
                    else {
                        Materialize.toast("No dealers returned. Please try again", 4000);
                    }
                },
                complete: function (xhr) {
                    if (xhr.status == 404) {
                        self.dealers(self.defaultDealersList);
                        ddlDealers.find('option[value=0]').attr('disabled', 'disabled');
                        self.selectedDealer(undefined);
                        self.selectedOperation(undefined);
                        ddlDealers.trigger("chosen:updated");
                        ddlDealerOperations.trigger("chosen:updated");
                        progress.hideProgress();
                        Materialize.toast(ddlMakes.find('option[value=' + self.selectedMake() + ']').text() + ' has no dealers', 4000);
                    }
                }

            });
        }
    };

    self.onOtherCityChange = function (data, event) {
        self.selectedOtherCity = event.target.value;
    };

    self.getPricing = function () {
        debugger;
        if (!parseInt(self.selectedCity) > 0)
            Materialize.toast("Please select a dealer city", 4000);
        else if (!self.selectedMake())
            Materialize.toast("Please select a make", 4000);
        else if (!self.selectedDealer())
            Materialize.toast("Please select a dealer", 4000);
        else if (!self.selectedOperation())
            Materialize.toast("Please select a operation", 4000);
        else {
            if (self.selectedOperation() == 2) {
                var pricingPageURL = "/dealers/" + self.selectedDealer() + "/dealercity/" + self.selectedCity.toString() + "/brand/" + self.selectedMake() + "/pricing/?dealerName=" + $("#ddlDealers").find('option[value=' + self.selectedDealer() + ']').text() + "&cityName=" + $("#ddlDealerCity").find('option[value=' + self.selectedCity + ']').text();
                window.location = pricingPageURL;
            }
            else if (self.selectedOperation() == 3) {
                window.location = "/dealers/" + self.selectedDealer() + "/facilities/?cityId=" + self.selectedCity + "&makeId=" + self.selectedMake() + "&dealerName=" + ddlDealers.find('option[value=' + self.selectedDealer() + ']').text();
            }
            else if (self.selectedOperation() == 4) {
                window.location = "/dealers/" + self.selectedDealer() + "/emi/?cityId=" + self.selectedCity + "&makeId=" + self.selectedMake() + "&dealerName=" + ddlDealers.find('option[value=' + self.selectedDealer() + ']').text();
            }
            else if (self.selectedOperation() == 7) {
                window.open("/newbikebooking/ManageDealerBenefits.aspx?dealerId=" + self.selectedDealer() + "&cityId=" + self.selectedCity, 'mywin', 'scrollbars=yes,left=0,top=0,width=1350,height=600');
            }
            else {
                window.open(operationReferences[self.selectedOperation()] + self.selectedDealer(), 'mywin', 'scrollbars=yes,left=0,top=0,width=1350,height=600');
            }
        }
    };

    self.getOtherCityPricing = function () {
        if (self.selectedOtherCity != null) {
            var pricingPageURL = "/dealers/" + self.selectedDealer() + "/dealercity/" + self.selectedCity + "/brand/" + self.selectedMake() + "/pricing/?otherCityId=" + self.selectedOtherCity + "&dealerName=" + ddlDealers.find('option[value=' + self.selectedDealer() + ']').text() + "&cityName=" + $("#ddlOtherCity").find('option[value=' + self.selectedOtherCity + ']').text();
            window.location = pricingPageURL;
        }
        else
            Materialize.toast('Please select the city', 4000);
    };
}