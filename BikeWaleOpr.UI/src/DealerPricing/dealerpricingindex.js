$(function () {
    //var dpParams = {
    //    makesList: [
    //        { makeId: 0, makeName: "Select Make" }
    //    ],
    //    dealersList: [
    //        { dealerId: 0, dealerName: "Select Dealer" }
    //    ],
    //    operationsList: [
    //        { operationId: 0, operationName: "Selected an operation" },
    //        { operationId: 1, operationName: "Offers" },
    //        { operationId: 2, operationName: "Prices and Availability" },
    //        { operationId: 3, operationName: "Facilities" },
    //        { operationId: 4, operationName: "EMI" },
    //        { operationId: 5, operationName: "Dealer Disclaimer" },
    //        { operationId: 6, operationName: "Booking Amount" },
    //        { operationId: 7, operationName: "Benefits/USP" }
    //    ]
    //}

    //if (!$('#makesListString').length == 0) {
    //    dpParams.makesList = dpParams.makesList.concat(JSON.parse(atob($('#makesListString').val())));
    //}

    //if (!$('#dealersListString').length == 0) {
    //    dpParams.dealersList = dpParams.dealersList.concat(JSON.parse(atob($('#dealersListString').val())));
    //}

    if (!$('#selectedCategories').length == 0) {
        var selectedCategories = JSON.parse(atob($('#selectedCategories').val()));
        $('#ddlCategories').val(selectedCategories);
    }

    var ccpParams = {
        allcities: [
            { cityId: 0, cityName: "Select Cities" }
        ]
    }

    var dcpParams = {
        dealers: [
            { dealerId: 0, dealerName: "Select Dealers" }
        ]
    }

    var dealerCopyPricing = function (dealerCopyPricing, ddlDealersByCity) {
        self.selectedCity = null;
        self.selectedDealers = ko.observableArray();
        self.deafultDealers = [
            { dealerId: 0, dealerName: "Select Dealers" }
        ];

        self.dealers = ko.observableArray(dealerCopyPricing.dealers);

        self.onCityChange = function (data, event) {
            progress.showProgress();
            self.selectedCity = event.target.value;
            var dealersByCityURL = "/api/dealers/city/" + self.selectedCity + "/";

            $.ajax({
                type: "GET",
                url: dealersByCityURL,
                success: function (resultData, textStatus, xhr) {
                    if (resultData.length > 0) {
                        self.dealers(self.deafultDealers.concat(resultData));
                        ddlDealersByCity.find('option[value=0]').attr('disabled', 'disabled');
                        ddlDealersByCity.trigger("chosen:updated");
                        progress.hideProgress();
                    }
                    else {
                        Materialize.toast('No dealers fetched. Please try again.', 4000);
                    }
                }
            });
        }

        self.copyPricingToDealers = function (data, event) {
            var selectedDealersLength = self.selectedDealers().length;

            if (selectedDealersLength > 0) {
                var selectedDealerNames = "";
                selectedDealerNames = ddlDealersByCity.find('option[value=' + self.selectedDealers()[0] + ']').text();
                for (var i = 1; i < selectedDealersLength; i++) {
                    selectedDealerNames += ", " + ddlDealersByCity.find('option[value=' + self.selectedDealers()[i] + ']').text();
                }

                var versionIds = [];
                var itemIds = [];
                var itemValues = [];
                var dealerIds = self.selectedDealers();
                var cityIds = [];
                cityIds.push(parseInt(event.target.dataset.dealerId));
                var enteredBy = parseInt(event.target.dataset.enteredBy);

                $('tbody').find('tr').each(function () {
                    currentVersionId = parseInt(this.dataset.versionid);

                    $(this).find('[data-itemid]').each(function () {
                        if (this.dataset.value > 0) {
                            versionIds.push(currentVersionId);
                            itemIds.push(parseInt(this.dataset.itemid));
                            itemValues.push(parseInt(this.dataset.value));
                        }

                    });
                });

                var dealerVersionPriceDto = {
                    dealerIds: dealerIds,
                    cityIds: cityIds,
                    enteredBy: enteredBy,
                    versionIds: versionIds,
                    itemIds: itemIds,
                    itemValues: itemValues

                };

                $.ajax({
                    type: "POST",
                    url: "/api/dealers/saveprices/",
                    data: dealerVersionPriceDto,
                    success: function (resultData, textStatus, xhr) {
                        if (xhr.status == 200) {
                            Materialize.toast('Price copied successfully to ' + selectedDealerNames, 4000);
                        }
                        else {
                            Materialize.toast('Something went wrong, please try again', 4000);
                        }
                    }
                });
            }
            else {
                Materialize.toast('Please select at least one dealer', 4000);
            }
        }
    }

    var cityCopyPricing = function (cityCopyPricingParams, ddlCity) {
        self.selectedCities = ko.observableArray();
        self.defaultAllcities = [
            { cityId: 0, cityName: "Select Cities" }
        ];

        self.cities = ko.observableArray(cityCopyPricingParams.allcities);

        self.onStateChange = function (data, event) {
            progress.showProgress();
            var makesByCityURL = "/api/cities/state/" + event.target.value + "/";

            $.ajax({
                type: "GET",
                url: makesByCityURL,
                success: function (resultData, textStatus, xhr) {
                    if (resultData.length > 0) {                  
                        self.cities(self.defaultAllcities.concat(resultData));
                        ddlCity.find('option[value=0]').attr('disabled', 'disabled');
                        ddlCity.trigger("chosen:updated");
                        progress.hideProgress();
                    }
                    else {
                        Materialize.toast('No cities fetched. Please try again', 4000);
                    }
                }
            });
        }

        self.copyPricingToCity = function () {
            var selectedCitiesLength = self.selectedCities().length;

            if (selectedCitiesLength > 0) {
                var selectedCityNames = "";
                selectedCityNames = ddlCity.find('option[value=' + self.selectedCities()[0] + ']').text();
                for (var i = 1; i < selectedCitiesLength; i++) {
                    selectedCityNames += ", " + ddlCity.find('option[value=' + self.selectedCities()[i] + ']').text();
                }

                var versionIds = [];
                var itemIds = [];
                var itemValues = [];
                var dealerIds = [];
                dealerIds.push(parseInt(event.target.dataset.dealerId));
                var cityIds = self.selectedCities();
                var enteredBy = parseInt(event.target.dataset.enteredBy);
                var saveDealerPricingURL = "/api/dealers/saveprices/";

                $('tbody').find('tr').each(function () {
                    currentVersionId = parseInt(this.dataset.versionid);

                    $(this).find('[data-itemid]').each(function () {
                        if (this.dataset.value > 0) {
                            versionIds.push(currentVersionId);
                            itemIds.push(parseInt(this.dataset.itemid));
                            itemValues.push(parseInt(this.dataset.value));
                        }

                    });
                });

                var dealerVersionPriceDto = {
                    dealerIds: dealerIds,
                    cityIds: cityIds,
                    enteredBy: enteredBy,
                    versionIds: versionIds,
                    itemIds: itemIds,
                    itemValues: itemValues
                };

                $.ajax({
                    type: "POST",
                    url: saveDealerPricingURL,
                    data: dealerVersionPriceDto,
                    success: function (resultData, textStatus, xhr) {
                        Materialize.toast('Price copied successfully to ' + selectedCityNames, 4000);
                    },
                    complete: function (xhr) {
                        if(xhr == 404)
                            Materialize.toast('Something went wrong. Please try again', 4000);
                    }
                });
            }
            else {
                Materialize.toast('Please select at least one city', 4000);
            }
        }
    }

    //var dealerOperationModel = function (dealerOperationModelParams, ddlMakes, ddlDealers, ddlDealerCity, ddlDealerOperations) {
    //    var self = this;
    //    self.selectedCity = null;
    //    self.selectedOtherCity = null;
    //    self.selectedMake = ko.observable();
    //    self.selectedDealer = ko.observable();
    //    self.selectedOperation = ko.observable();
    //    self.defaultMakesList = [
    //        { makeId: 0, makeName: "Select Make" }
    //    ];
    //    self.defaultDealersList = [
    //        { dealerId: 0, dealerName: "Select Dealer" }
    //    ];

    //    var operationReferences = {
    //        1: "/newbikebooking/ManageOffers.aspx?dealerId=",
    //        3: "/newbikebooking/ManageDealerFacilities.aspx?dealerId=",
    //        4: "/newbikebooking/ManageDealerLoanAmounts.aspx?dealerId=",
    //        5: "/newbikebooking/ManageDealerDisclaimer.aspx?dealerId=",
    //        6: "/newbikebooking/ManageBookingAmount.aspx?dealerId=",
    //        7: "/newbikebooking/ManageDealerBenefits.aspx?dealerId="
    //    };

    //    self.makes = ko.observableArray(dealerOperationModelParams.makesList);

    //    self.dealers = ko.observableArray(dealerOperationModelParams.dealersList);

    //    self.operations = ko.observableArray(dealerOperationModelParams.operationsList);

    //    if (!$('#makesListString').length == 0 && !$('#dealersListString').length == 0) {
    //        self.selectedMake($('#makeId').val());
    //        self.selectedDealer($('#dealerId').val());
    //        self.selectedCity = $('#cityId').val();
    //        ddlDealerCity.val(self.selectedCity);
    //        self.selectedOperation(2);
    //        if (!$('#otherCityId').length == 0)
    //            $('#ddlOtherCity').val($('#otherCityId').val());
    //    }
    //    self.onCityChange = function (data, event) {
    //        self.selectedCity = event.target.value;

    //        if (self.selectedCity > 0) {
    //            progress.showProgress();
    //            var makesByCityURL = "/api/makes/city/" + self.selectedCity + "/";

    //            $.ajax({
    //                type: "GET",
    //                url: makesByCityURL,
    //                success: function (resultData, textStatus, xhr) {
    //                    if (resultData.length > 0) {
    //                        self.makes(self.defaultMakesList.concat(resultData));
    //                        ddlMakes.find('option[value=0]').attr('disabled', 'disabled');
    //                        self.selectedMake(undefined);
    //                        self.dealers(self.defaultDealersList);
    //                        ddlDealers.find('option[value=0]').attr('disabled', 'disabled');
    //                        self.selectedDealer(undefined);
    //                        self.selectedOperation(undefined);
    //                        ddlMakes.trigger("chosen:updated");
    //                        ddlDealers.trigger("chosen:updated");
    //                        ddlDealerOperations.trigger("chosen:updated");
    //                        progress.hideProgress();
    //                    }
    //                    else {
    //                        Materialize.toast('No makes returned. Please try again', 4000);
    //                    }
    //                },
    //                complete: function (xhr) {
    //                    if (xhr.status == 404) {
    //                        self.makes(self.defaultMakesList);
    //                        ddlMakes.find('option[value=0]').attr('disabled', 'disabled');
    //                        self.selectedMake(undefined);
    //                        self.dealers(self.defaultDealersList);
    //                        ddlDealers.find('option[value=0]').attr('disabled', 'disabled');
    //                        self.selectedDealer(undefined);
    //                        self.selectedOperation(undefined);
    //                        ddlMakes.trigger("chosen:updated");
    //                        ddlDealers.trigger("chosen:updated");
    //                        ddlDealerOperations.trigger("chosen:updated");
    //                        progress.hideProgress();
    //                        Materialize.toast(ddlDealerCity.find('option[value=' + self.selectedCity + ']').text() + ' has no dealers', 4000);
    //                    }
    //                }

    //            });
    //        }
    //    };

    //    self.onMakeChange = function (data, event) {
    //        if (self.selectedCity > 0 && self.selectedMake() > 0) {
    //            progress.showProgress();
    //            var dealersByCityAndMake = "/api/dealers/make/" + self.selectedMake() + "/city/" + self.selectedCity + "/";

    //            $.ajax({
    //                type: "GET",
    //                url: dealersByCityAndMake,
    //                success: function (resultData, textStatus, xhr) {
    //                    if (resultData.length > 0) {
    //                        self.dealers(self.defaultDealersList.concat(resultData));
    //                        ddlDealers.find('option[value=0]').attr('disabled', 'disabled');
    //                        self.selectedDealer(undefined);
    //                        ddlDealers.trigger("chosen:updated");
    //                        self.selectedOperation(undefined);
    //                        ddlDealerOperations.trigger("chosen:updated");
    //                        progress.hideProgress();
    //                    }
    //                    else {
    //                        Materialize.toast("No dealers returned. Please try again", 4000);
    //                    }
    //                },
    //                complete: function (xhr) {
    //                    if (xhr.status == 404) {
    //                        self.dealers(self.defaultDealersList);
    //                        ddlDealers.find('option[value=0]').attr('disabled', 'disabled');
    //                        self.selectedDealer(undefined);
    //                        self.selectedOperation(undefined);
    //                        ddlDealers.trigger("chosen:updated");
    //                        ddlDealerOperations.trigger("chosen:updated");
    //                        progress.hideProgress();
    //                        Materialize.toast(ddlMakes.find('option[value=' + self.selectedMake() + ']').text() + ' has no dealers', 4000);
    //                    }
    //                }

    //            });
    //        }
    //    };

    //    self.onOtherCityChange = function (data, event) {
    //        self.selectedOtherCity = event.target.value;
    //    };

    //    self.getPricing = function () {
    //        if (!self.selectedCity)
    //            Materialize.toast("Please select a dealer city", 4000);
    //        else if (!self.selectedMake())
    //            Materialize.toast("Please select a make", 4000);
    //        else if (!self.selectedDealer())
    //            Materialize.toast("Please select a dealer", 4000);
    //        else if (!self.selectedOperation())
    //            Materialize.toast("Please select a operation", 4000);
    //        else {
    //            if (self.selectedOperation() == 2) {
    //                var pricingPageURL = "/dealer/" + self.selectedDealer() + "/dealercity/" + self.selectedCity.toString() + "/brand/" + self.selectedMake() + "/pricing/?dealerName=" + $("#ddlDealers").find('option[value=' + self.selectedDealer() + ']').text() + "&cityName=" + $("#ddlDealerCity").find('option[value=' + self.selectedCity + ']').text();
    //                window.location = pricingPageURL;
    //            }
    //            else if (self.selectedOperation() == 7) {
    //                window.open("/newbikebooking/ManageDealerBenefits.aspx?dealerId=" + self.selectedDealer() + "&cityId=" + self.selectedCity, 'mywin', 'scrollbars=yes,left=0,top=0,width=1350,height=600');
    //            }
    //            else {
    //                window.open(operationReferences[self.selectedOperation()] + self.selectedDealer(), 'mywin', 'scrollbars=yes,left=0,top=0,width=1350,height=600');
    //            }
    //        }
    //    };

    //    self.getOtherCityPricing = function () {
    //        if (self.selectedOtherCity != null) {
    //            var pricingPageURL = "/dealer/" + self.selectedDealer() + "/dealercity/" + self.selectedCity + "/brand/" + self.selectedMake() + "/pricing/?otherCityId=" + self.selectedOtherCity + "&dealerName=" + $("#ddlDealers").find('option[value=' + self.selectedDealer() + ']').text() + "&cityName=" + $("#ddlOtherCity").find('option[value=' + self.selectedOtherCity + ']').text();
    //            window.location = pricingPageURL;
    //        }
    //        else
    //            Materialize.toast('Please select the city', 4000);
    //    };
    //}

    var priceSheetModel = function () {
        self.totalRowsCount = $('tbody').find('tr').length;
        self.selectedRowsCount = 0;

        self.onAllSelectChange = function (data, event) {
            if (event.target.checked)
                self.selectedRowsCount = 0;
            else
                self.selectedRowsCount = self.totalRowsCount;

            $('table').find('.rowCheckbox').each(function () {
                $(this).prop('checked', event.target.checked);
                $(this).trigger("change");
            });
        };

        self.onCheckboxChange = function (data, event) {
            if (event.target.checked) {
                self.selectedRowsCount++;
                var currentRow = $(event.target).closest('tr');
                currentRow.addClass("teal");
                currentRow.addClass("lighten-5");
                currentRow.find('.editableValue').each(function () {
                    $(this).html($("<input>", { "type": "number", "min": "0", "value": this.dataset.value }));
                });
                if (self.selectedRowsCount == totalRowsCount)
                    $("#allRowsSelect").prop("checked", true);
            }
            else {
                self.selectedRowsCount--;
                $("#allRowsSelect").prop('checked', false);
                var currentRow = $(event.target).closest('tr');
                currentRow.removeClass("teal");
                currentRow.removeClass("lighten-5");
                currentRow.find('.editableValue').each(function () {
                    $(this).html(this.dataset.value);
                });
            }
        };

        self.onColorAvailabilityClick = function (data, event) {
            window.open(event.target.dataset.url, 'mywin', 'scrollbars=yes,left=25%,top=25%,width=600,height=400');
        };

        self.onPriceOrAvaialabilityUpdateSucess = function (resultData) {
            $('tbody').find('tr').filter(':has(:checkbox:checked)').each(function () {

                if (resultData.isPriceSaved) {
                    $(this).find('[data-itemid]').each(function () {
                        this.dataset.value = $(this).find('input').val();
                    });
                }

                if (resultData.isAvailabilitySaved) {
                    $(this).find('[data-availability]').each(function () {
                        this.dataset.value = $(this).find('input').val();
                    });
                }

                $(this).find('input[type="checkbox"]').prop('checked', false).trigger('change');
            });

            if (resultData.isPriceSaved && resultData.isAvailabilitySaved)
                Materialize.toast('Price and availability updated successfully', 4000);
            else if (resultData.isPriceSaved)
                Materialize.toast('Price updated successfully.', 4000);
            else if (resultData.isAvailabilitySaved)
                Materialize.toast('Availability updated successfully', 4000);

            if (resultData.rulesUpdatedModelNames.length > 0)
                Materialize.toast("Rules have been added for this dealer for models " + resultData.rulesUpdatedModelNames, 4000);
        }

        self.onPriceOrAvaialabilityDeleteSucess = function (resultData) {
            $('tbody').find('tr').filter(':has(:checkbox:checked)').each(function () {

                if (resultData.isPriceDeleted) {
                    $(this).find('[data-itemid]').each(function () {
                        this.dataset.value = 0;
                    });
                }

                if (resultData.isAvailabilityDeleted) {
                    $(this).find('[data-availability]').each(function () {
                        this.dataset.value = 0;
                    });
                }

                $(this).find('input[type="checkbox"]').prop('checked', false).trigger('change');
            });

            if (resultData.isPriceDeleted && resultData.isAvailabilityDeleted)
                Materialize.toast('Price and availability deleted successfully', 4000);
            else if (resultData.isPriceDeleted)
                Materialize.toast('Price deleted successfully', 4000);
            else if (resultData.isAvailabilityDeleted)
                Materialize.toast('Availability deleted successfully', 4000);
        }

        self.onUpdateClick = function (data, event) {
            if (confirm("Do you want to update the pricing?")) {
                var versionIds = [];
                var bikeVersionIds = [];
                var bikeModelIds = [];
                var bikeModelNames = [];
                var itemIds = [];
                var itemValues = [];
                var versionAvailabilityDays = [];
                var dealerIds = [parseInt(event.target.dataset.dealerId)];
                var cityIds = [parseInt(event.target.dataset.cityId)];
                var enteredBy = parseInt(event.target.dataset.enteredBy);

                $('tbody').find('tr').filter(':has(:checkbox:checked)').each(function () {
                    var isPriceUpdated = false;
                    currentVersionId = parseInt(this.dataset.versionid);

                    $(this).find('[data-itemid]').each(function () {
                        if ($(this).find('input').val() != this.dataset.value) {
                            isPriceUpdated = true;
                            versionIds.push(currentVersionId);
                            itemIds.push(parseInt(this.dataset.itemid));
                            itemValues.push(parseInt($(this).find('input').val()));
                        }
                    });

                    $(this).find('[data-availability]').each(function () {
                        if ($(this).find('input').val() != this.dataset.value) {
                            bikeVersionIds.push(currentVersionId);
                            versionAvailabilityDays.push(parseInt($(this).find('input').val()));
                        }
                    });

                    if (isPriceUpdated) {
                        debugger;
                        bikeModelIds.push(parseInt(this.dataset.modelid));
                        bikeModelNames.push(this.dataset.modelname);
                    }
                });

                var dealerVersionPriceDto = {
                    dealerIds: dealerIds,
                    cityIds: cityIds,
                    enteredBy: enteredBy,
                    versionIds: versionIds,
                    itemIds: itemIds,
                    itemValues: itemValues
                };

                var dealerVersionAvailabilityDto = {
                    dealerId: parseInt(event.target.dataset.dealerId),
                    bikeVersionIds: bikeVersionIds,
                    numberOfDays: versionAvailabilityDays
                };


                var dealerPricesAvaialabilities = {
                    dealerVersionPrices: dealerVersionPriceDto,
                    dealerVersionAvailabilities: dealerVersionAvailabilityDto,
                    bikeModelIds: bikeModelIds,
                    makeId: $('#makeId').val(),
                    bikeModelNames: bikeModelNames
                };


                var saveDealerPricingURL = "/api/dealers/savepricesandavailability/";

                $.ajax({
                    type: "POST",
                    url: saveDealerPricingURL,
                    data: dealerPricesAvaialabilities,
                    success: function (resultData, textStatus, xhr) {
                        if (resultData.isPriceSaved || resultData.isAvailabilitySaved) {
                            self.onPriceOrAvaialabilityUpdateSucess(resultData);
                        }
                        else {
                            Materialize.toast('Something went wrong, please try again', 4000);
                        }
                    }
                });
            } else {
                Materialize.toast('Update cancelled', 4000);
            }
        };

        self.onDeleteClick = function (data, event) {
            if (confirm("Do you want to delete the pricing?")) {
                var bikeVersionIds = [];
                var dealerId = parseInt(event.target.dataset.dealerId);
                var cityId = parseInt(event.target.dataset.cityId);

                $('tr').filter(':has(:checkbox:checked)').each(function () {
                    bikeVersionIds.push(parseInt(this.dataset.versionid));
                });

                var dealerCityVersionsDto = {
                    dealerId: dealerId,
                    cityId: cityId,
                    bikeVersionIds: bikeVersionIds
                };

                $.ajax({
                    type: "POST",
                    url: "/api/dealers/deletepricesandavailability/",
                    data: dealerCityVersionsDto,
                    success: function (resultData, textStatus, xhr) {
                        self.onPriceOrAvaialabilityDeleteSucess(resultData);
                    },
                    complete: function (xhr) {
                        if(xhr == 404)
                            Materialize.toast('Something went wrong. Please try again', 4000);
                    }
                });
            }
            else {
                Materialize.toast('Delete cancelled', 4000);
            }
        }
    }

    var addCategoryModel = function () {
        self.onAddCategoryClick = function () {
            var selectedCategories = $('#ddlCategories').val();

            if (selectedCategories != null) {
                ddlCategories = $('#ddlCategories');
                priceSheetTable = $('table');

                $('thead').find('tr').each(function () {
                    var tdsStr = "", tdColor = $(this).find("th.colorAvailability");
                    for (var index = 0; index < selectedCategories.length; index++) {
                        itemName = $(ddlCategories).find('option[value=' + selectedCategories[index] + ']').text()
                        tdsStr += '<th class="editableColumns" data-item-id=' + selectedCategories[index] + '>' + itemName + '</th>';
                    }
                    tdsStr += '<th class="editableColumns">Availability(days)</th>';
                    $(this).find('th.editableColumns').remove();
                    $(this).append(tdsStr).append(tdColor);
                });

                $('tbody').find('tr').each(function () {
                    var tdsStr = "", tdColor = $(this).find("td.colorAvailability");
                    for (var index = 0; index < selectedCategories.length; index++) {
                        tdsStr += '<td class="editableValue" data-value="0" data-itemid="' + selectedCategories[index] + '">0</td>';
                    }
                    tdsStr += '<td data-availability="0" data-value="0" class="editableValue">0</td>';
                    $(this).find('td.editableValue').remove();
                    $(this).append(tdsStr).append(tdColor);
                });

                $('#allRowsSelect').attr('checked', true);
                $('#allRowsSelect').trigger('change');
                $(".rowCheckbox").attr('disabled', true);
                $('#allRowsSelect').attr('disabled', true);

                Materialize.toast('Added the selected categories successfully. Please fill in the values', 4000);
            }
            else
                Materialize.toast('Please select at least one category', 4000);
        }
    }

    var dealerPricingIndexPageModel = function () {
        var self = this;
        var tblPricingSheet = $('#tblPricingSheet');
        var ddlCity = $('#ddlCity');
        var ddlDealersByCity = $('#ddlDealersByCity');

        self.dealerOperationsModel = ko.observable(new dealerOperationModel(dpParams));
        if ($('#liCityCopyPricing').length > 0)
            self.cityCopyPricing = ko.observable(new cityCopyPricing(ccpParams, ddlCity));
        if ($('#liDealerCopyPricing').length > 0)
            self.dealerCopyPricing = ko.observable(new dealerCopyPricing(dcpParams, ddlDealersByCity));
        if (tblPricingSheet.length > 0) {
            self.priceSheetModel = ko.observable(new priceSheetModel());
        }
        if ($('#liAddCategory').length > 0)
            self.addCategoryModel = ko.observable(new addCategoryModel());
    };

    ko.applyBindings(new dealerPricingIndexPageModel());

    (function () {
        $('#ddlMakes').find('option[value=0]').attr('disabled', 'disabled');
        $('#ddlDealers').find('option[value=0]').attr('disabled', 'disabled');
        $('#ddlDealerOperations').find('option[value=0]').attr('disabled', 'disabled');
        $('#ddlCity').find('option[value=0]').attr('disabled', 'disabled');

        $('select.chosen-select').chosen({
           "width": "250px"
        });

        if ($('#tblPricingSheet').length > 0) {
            $('#ddlDealerOperations').val(2);
            $("#ddlDealerOperations").trigger('chosen:updated');
        }
    }());
});