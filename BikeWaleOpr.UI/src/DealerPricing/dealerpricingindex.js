$(function () {
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
            var bikeModelIds = [];
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
                cityIds.push(parseInt(self.selectedCity));
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
                    bikeModelIds.push(parseInt(this.dataset.modelid));
                });
                
                var dealerVersionPriceDto = {
                    dealerIds: dealerIds,
                    cityIds: cityIds,
                    enteredBy: enteredBy,
                    versionIds: versionIds,
                    itemIds: itemIds,
                    itemValues: itemValues,
                    bikeModelIds: bikeModelIds

                };
                var selectAll = !($("#allRowsSelect").val() == "true");
                $.ajax({
                    type: "POST",
                    url: selectAll ? "/api/dealers/saveprices/" : "/api/dealers/copyprices/",
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
                var bikeModelIds = [];
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
                    bikeModelIds.push(parseInt(this.dataset.modelid));
                });                
                var dealerVersionPriceDto = {
                    dealerIds: dealerIds,
                    cityIds: cityIds,
                    enteredBy: enteredBy,
                    versionIds: versionIds,
                    itemIds: itemIds,
                    itemValues: itemValues,
                    bikeModelIds: bikeModelIds
                };

                $.ajax({
                    type: "POST",
                    url: saveDealerPricingURL,
                    data: dealerVersionPriceDto,
                    success: function (resultData, textStatus, xhr) {
                        Materialize.toast('Price copied successfully to ' + selectedCityNames, 4000);
                    },
                    complete: function (xhr) {
                        if (xhr == 404)
                            Materialize.toast('Something went wrong. Please try again', 4000);
                    }
                });
            }
            else {
                Materialize.toast('Please select at least one city', 4000);
            }
        }
    }

    var bikewaleCopyPricing = function (ddlCity) {
        
        self.selectedCities = ko.observableArray();
        self.allCities = ko.observable(false);        
        self.defaultAllcities = [
            { cityId: 0, cityName: "Select Cities" }
        ];

        self.cities = ko.observableArray();
		self.onStateChangeBWPrice = function (data, event) {

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
        };        
        self.copyPricingToBikewalePrice = function () {

            var selectedCitiesLength = self.selectedCities().length;

            if (selectedCitiesLength > 0 || $('#chkAllCity').is(':checked')) {

                var exShowroomCategory = $('#ddlExShowroomCategory').val();
                var insuranceCategory = $('#ddlInsuranceCategory').val();
                var rtoCategory = $('#ddlRtoCategory').val();

                if (exShowroomCategory == insuranceCategory || exShowroomCategory == rtoCategory || insuranceCategory == rtoCategory) {
                    Materialize.toast('Ex-showroom, insurance and rto must have unique mapping.', 4000);
                    return false;
                }
                var versionAndPriceList = "";
                var table = $('#tblPricingSheet');
                
                var modelIds = "";
                table.find('tbody tr').each(function () {
                    var row = $(this), exshowroom, insurance, rto;
                    var versionId = row.data('versionid');
                    if (row.find('#checkbox-' + versionId).is(':checked')) {
                        exshowroom = row.find('td[data-itemid=' + exShowroomCategory + '] input').val();
                        insurance = row.find('td[data-itemid=' + insuranceCategory + '] input').val();
                        rto = row.find('td[data-itemid=' + rtoCategory + '] input').val();
                        modelIds += row.data('modelid') + ',';
                        if (exshowroom && insurance && rto) {
                            versionAndPriceList += versionId + "#c0l#" + exshowroom + "#c0l#" + insurance + "#c0l#" + rto + "|r0w|";
                        }
                        else {
                            Materialize.toast('Exshowroom, Insurance, RTO mapping is wrong.', 4000);
                            return false;
                        }
                    }
                });
                modelIds = modelIds.substring(0, modelIds.lastIndexOf(','));
                if (versionAndPriceList.trim()) {

                    versionAndPriceList = versionAndPriceList.substring(0, versionAndPriceList.lastIndexOf("|r0w|"));
                    var userId = table.data('userid');
                    var makeId = table.data('makeid');
                    var citiesIds = "";

                    if ($('#chkAllCity').is(':checked')) {
                        $(ddlCity).find('option').each(function () {
                            citiesIds += $(this).val() + '|r0w|';
                        });
                        citiesIds = citiesIds.substring(citiesIds.indexOf('|r0w|') + 5);
                    }
                    else {
                        for (var i = 0; i < self.selectedCities().length; i++) {
                            citiesIds += self.selectedCities()[i] + '|r0w|';
                        }
                    }

                    citiesIds = citiesIds.substring(0, citiesIds.lastIndexOf("|r0w|"));
                    var data = {
                        versionAndPriceList: versionAndPriceList,
                        citiesList: citiesIds,
                        makeid: makeId,
                        modelIds: modelIds,
                        userId: userId
                    }
                    var url = "/api/price/save/";
                    $.ajax({
                        type: "POST",
                        data: data,
                        url: url,
                        success: function (response) {
                            if (response) {
                                Materialize.toast("Prices saved successfully", 4000);
                            }
                            else {
                                Materialize.toast("Something went wrong. Prices were not saved.", 4000);
                            }
                        },
                        error: function (xhr, status, error) {
                            Materialize.toast("Something went wrong. Prices were not saved.", 4000);
                        }
                    });
                }
                else {
                    Materialize.toast("Check at least one version.", 4000);
                }

            }
            else {
                Materialize.toast('Select one or more cities.', 4000);
            }
        };
    }

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
                    $(this).html($("<input>", { "type": "text", "min": "0", "value": this.dataset.value }));
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
            var anyChange = false;

            $('tbody').find('tr').filter(':has(:checkbox:checked)').each(function () {
                var isPriceUpdated = false;
                currentVersionId = parseInt(this.dataset.versionid);

                $(this).find('[data-itemid]').each(function () {
                    if ($(this).find('input').val() != this.dataset.value) {
                        anyChange = true;
                        isPriceUpdated = true;
                        versionIds.push(currentVersionId);
                        itemIds.push(parseInt(this.dataset.itemid));
                        itemValues.push(parseInt($(this).find('input').val()));
                    }
                });

                $(this).find('[data-availability]').each(function () {
                    if ($(this).find('input').val() != this.dataset.value) {
                        anyChange = true;
                        bikeVersionIds.push(currentVersionId);
                        versionAvailabilityDays.push(parseInt($(this).find('input').val()));
                    }
                });

                if (isPriceUpdated) {
                    bikeModelIds.push(parseInt(this.dataset.modelid));
                    bikeModelNames.push(this.dataset.modelname);
                }
            });

            if (!anyChange) {
                Materialize.toast('No value updated', 4000);
            } else {
                if (confirm("Do you want to update the pricing?")) {
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

                    if (anyChange) {
                        $.ajax({
                            type: "POST",
                            url: saveDealerPricingURL,
                            data: dealerPricesAvaialabilities,
                            success: function (resultData, textStatus, xhr) {
                                $(".rowCheckbox").attr('disabled', false);
                                $('#allRowsSelect').attr('disabled', false);
                                if (resultData.isPriceSaved || resultData.isAvailabilitySaved) {
                                    self.onPriceOrAvaialabilityUpdateSucess(resultData);
                                }
                                else {
                                    Materialize.toast('Something went wrong, please try again', 4000);
                                }
                            }
                        });
                    }
                } else {
                    Materialize.toast('Update cancelled', 4000);
                }
            }
        };

        self.onDeleteClick = function (data, event) {
            var bikeVersionIds = [];
            var dealerId = parseInt(event.target.dataset.dealerId);
            var cityId = parseInt(event.target.dataset.cityId);
            var isBoxChecked = false;

            $('tr').filter(':has(:checkbox:checked)').each(function () {
                bikeVersionIds.push(parseInt(this.dataset.versionid));
                isBoxChecked = true;
            });

            if (!isBoxChecked) {
                Materialize.toast('No version selected', 4000);
            }
            else {
                if (confirm("Do you want to delete the pricing?")) {
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
                            if (xhr == 404)
                                Materialize.toast('Something went wrong. Please try again', 4000);
                        }
                    });
                }
                else {
                    Materialize.toast('Delete cancelled', 4000);
                }
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

                $('#allRowsSelect').prop('checked', true);
                $('#allRowsSelect').trigger('change');
                $(".rowCheckbox").prop('disabled', true);
                $('#allRowsSelect').prop('disabled', true);

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
        var ddlCityBwPrice = $('#ddlCityBwPrice');

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
        if ($('#liBikewaleCopyPricing').length > 0) {
            self.bikewaleCopyPricing = ko.observable(new bikewaleCopyPricing(ddlCityBwPrice));
        }
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
            $('#ddlDealerOperations').val(2).change();
            $("#ddlDealerOperations").trigger('chosen:updated');
        }
    }());
});
