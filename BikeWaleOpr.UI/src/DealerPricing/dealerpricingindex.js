var vmDealerPricing;
$(function () {
    var dllDealerCity = $('#ddlDealerCity');

    var dpParams = { 
        makesList: [],
        dealersList: [],
        operationsList: [
            { operationId: 1, operationName: "Manage Offers" },
            { operationId: 2, operationName: "Manage Prices and Availability" },
            { operationId: 3, operationName: "Manage Facilities" },
            { operationId: 4, operationName: "Manage EMI" },
            { operationId: 5, operationName: "Manage Dealer Disclaimer" },
            { operationId: 6, operationName: "Manage Booking Amount" },
            { operationId: 7, operationName: "Manage Benefits/USP" }
        ]
    }

    if (!$('#makesListString').length == 0) {
        debugger;
        dpParams.makesList = JSON.parse(atob($('#makesListString').val()));
    }

    if (!$('#dealersListString').length == 0) {
        debugger
        dpParams.dealersList = JSON.parse(atob($('#dealersListString').val()));
    }



    var ccpParams = {
        allcities: []
    }

    var dcpParams = {
        dealers: []
    }

    var dealerCopyPricing = function (dealerCopyPricing) {
        self.selectedCity = null;;
        self.selectedDealers = ko.observableArray();

        self.dealers = ko.observableArray(dealerCopyPricing.dealers);

        self.onCityChange = function (data, event) {
            self.selectedCity = event.target.value;
            var dealersByCityURL = "/api/dealers/city/" + self.selectedCity + "/";
            //Materialize.toast('Retriving makes',300);

            $.ajax({
                type: "GET",
                url: dealersByCityURL,
                success: function (resultData, textStatus, xhr) {
                    if (xhr.status == 200 && resultData.length > 0) {
                        //Materialize.toast.removeAll();
                        self.dealers(resultData);
                        $('select').material_select();
                    }
                    else {
                        Materialize.toast('Something went wrong, please try again', 4000);
                    }
                }
            });
        }
    }

    var cityCopyPricing = function (cityCopyPricingParams) {
        self.selectedState = null;;
        self.selectedCities = ko.observableArray();
        
        self.cities = ko.observableArray(cityCopyPricingParams.allcities);

        self.onStateChange = function (data, event) {
            self.selectedState = event.target.value;
            var makesByCityURL = "/api/cities/state/" + self.selectedState + "/";
            //Materialize.toast('Retriving makes', 300);

            $.ajax({
                type: "GET",
                url: makesByCityURL,
                success: function (resultData, textStatus, xhr) {
                    if (xhr.status == 200 && resultData.length > 0) {
                        //Materialize.toast.removeAll();
                        debugger;
                        self.cities(resultData);
                        $('select').material_select();
                    }
                    else {
                        Materialize.toast('Something went wrong, please try again', 4000);
                    }
                }
            });
        } 

        self.copyPricingToCity = function () {
            debugger;
        }
    }

    var dealerOperationModel = function (dealerOperationModelParams) {
        var self = this;

        self.selectedCity = null;
        self.selectedOtherCity = null;
        self.selectedMake = ko.observable();
        self.selectedDealer = ko.observable();
        self.selectedOperation = ko.observable();

        self.makes = ko.observableArray(dealerOperationModelParams.makesList);

        self.dealers = ko.observableArray(dealerOperationModelParams.dealersList);

        self.operations = ko.observableArray(dealerOperationModelParams.operationsList);

        if (!$('#makesListString').length == 0 && !$('#dealersListString').length == 0) {
            (function () {
                debugger;
                $('select').material_select();
                self.selectedMake($('#makeId').val());
                self.selectedDealer($('#dealerId').val());
                self.selectedCity = $('#cityId').val();
                $('#ddlDealerCity').val(self.selectedCity);
                if(!$('#otherCityId').length == 0)
                    $('#ddlOtherCity').val($('#otherCityId').val());
            }());
        }
        self.onCityChange = function (data, event) {
            self.selectedCity = event.target.value;
            var makesByCityURL = "/api/makes/city/" + self.selectedCity + "/";
            //Materialize.toast('Retriving makes',300);

            $.ajax({
                type: "GET",
                url: makesByCityURL,
                success: function (resultData, textStatus, xhr) {
                    if (xhr.status == 200 && resultData.length > 0) {
                        //Materialize.toast.removeAll();
                        self.makes(resultData);
                        $('select').material_select();
                    }
                    else {
                        Materialize.toast('Something went wrong, please try again', 4000);
                    }
                }
            });
        };

        self.onMakeChange = function (data, event) {
            var dealersByCityAndMake = "/api/dealers/make/" + self.selectedMake() + "/city/" + self.selectedCity + "/";
            //Materialize.toast('Retriving dealers');

            $.ajax({
                type: "GET",
                url: dealersByCityAndMake,
                success: function (resultData, textStatus, xhr) {
                    if (xhr.status == 200 && resultData.length > 0) {
                        //Materialize.toast.removeAll();
                        self.dealers(resultData);
                        $('select').material_select();
                    }
                    else {
                        Materialize.toast('Something went wrong, please try again', 4000);
                    }
                }
            });
        };

        self.onOtherCityChange = function (data, event) {
            self.selectedOtherCity = event.target.value;
        };

        self.getPricing = function () {
            if (self.selectedOperation() == 1) {

            }
            else if (self.selectedOperation() == 2) {
                var pricingPageURL = "/dealerbikepricing/" + self.selectedDealer() + "/dealercity/" + self.selectedCity.toString() + "/brand/" + self.selectedMake() + "/";
                window.location = pricingPageURL;
            }
        };

        self.getOtherCityPricing = function () {
            debugger;
            if (self.selectedOtherCity != null) {
                var pricingPageURL = "/dealerbikepricing/" + self.selectedDealer().toString() + "/dealercity/" + self.selectedCity + "/brand/" + self.selectedMake() + "/?otherCityId=" + self.selectedOtherCity;
                window.location = pricingPageURL;
            }
        };
    }
    
    var dealerPricingIndexPageModel = function () {
        var self = this;
        self.dealerOperationsModel = ko.observable(new dealerOperationModel(dpParams));
        self.cityCopyPricing = ko.observable(new cityCopyPricing(ccpParams));
        self.dealerCopyPricing = ko.observable(new dealerCopyPricing(dcpParams));
    }

    ko.applyBindings(new dealerPricingIndexPageModel());

    (function () {
        $('select').material_select();
    }());

});