if (msg != "") { Materialize.toast(msg, 4000); }
var ddlModels = $('#ddlModels');
var ddlCities = $('#ddlCities');
var cityIds = "", stateIds = "", modelIds = "";
var getCitiesFromDropDown, setRulesData, validateInput, showAllIndiaAlert, showDeleteAlert;
var manufacturerRulesViewModel = function () {
    var self = this;
    self.onExShowroom = onExShowroom;
    self.AllIndiaValue = ko.observable();
    self.isAllIndia = ko.observable(false);
    self.selectedMake = ko.observable();
    self.listModels = ko.observableArray([]);
    self.selectedState = ko.observable();
    self.listCities = ko.observableArray([]);
    self.listStates = ko.observableArray([]);
    self.isAllIndia.ForSubmit = ko.computed({
        read: function () {
            return this.isAllIndia().toString();
        },
        write: function (newValue) {
            this.isAllIndia(newValue === "1");
        },
        owner: this
    });

    self.selectMake = function () {
        self.selectedMake($('#ddlMakes option:selected').val());
        if (self.selectedMake() != undefined && self.selectedMake() > 0) {
            $.ajax({
                type: "GET",
                url: bwHostUrl + "/api/campaigns/manufacturer/models/makeId/" + self.selectedMake() + "/",
                datatype: "json",
                success: function (response) {
                    var models = ko.toJS(response);
                    if (models) {
                        self.listModels(models);
                    }
                },
                complete: function (xhr) {
                    ddlModels.material_select();
                }
            });
        }
        else {
            self.listModels([]);
            ddlModels.material_select();
        }
    };

    self.selectStates = function () {
        self.listStates([]);
        var array = new Array();
        $('#ddlStatesOnly :selected').each(function () {
            array.push($(this).val());
        });
        self.listStates(array)
        stateIds = "";
        if (self.listStates().length > 0) {
            ko.utils.arrayForEach(self.listStates(), function (state) {
                if (state)
                    stateIds = stateIds + "," + state;
            });
        };
    };

    self.selectState = function () {
        self.selectedState($('#ddlStates option:selected').val());
        if (self.selectedState() != undefined && self.selectedState() > 0) {
            $.ajax({
                type: "GET",
                url: bwHostUrl + "/api/campaigns/manufacturer/cities/stateId/" + self.selectedState() + "/",
                datatype: "json",
                success: function (response) {
                    var cities = ko.toJS(response);
                    if (cities) {
                        self.listCities(cities);
                    }
                },
                complete: function (xhr) {
                    ddlCities.material_select();
                }
            });
        }
        else {
            self.listCities([]);
            ddlCities.material_select();
        }
    };

};

$(document).ready(function () {

    mfgVM = new manufacturerRulesViewModel;
    ko.applyBindings(mfgVM, $('#configRules')[0]);

    if (mfgVM.onExShowroom) {
        mfgVM.isAllIndia(true);
        $('#rdoLocation1').prop("checked", true);
    }

    $("[name=location]").change(function () {
        var rdo = $("[name=location]:checked").val();
        if (mfgVM.onExShowroom && rdo != "1") {
            $('#rdoLocation1').prop("checked", true);
            showAllIndiaAlert();
        }
    });

    $('.chip .close').on('click', function (e) {
        var clicked = $(this);
        var modelId, stateId, cityId, campaignId, userId, isAllIndia, el, elementName;
        el = clicked.attr('data-element');
        elementName = clicked.attr('data-name');
        modelId = clicked.attr('data-modelid');
        stateId = clicked.attr('data-stateid');
        cityId = clicked.attr('data-cityid');
        campaignId = $('#CampaignId').val();
        userId = $('#UserId').val();
        isAllIndia = clicked.attr('data-allindia');
        
        var obj = {
            'campaignId': campaignId,
            'modelId': modelId,
            'cityId': cityId,
            'stateId': stateId,
            'userId': userId,
            'isAllIndia': isAllIndia
        }
        var confirmDel = showDeleteAlert(elementName);
        if (confirmDel) {
            $.ajax({
                type: "POST",
                url: bwHostUrl + "/api/campaigns/manufacturer/deleterule/",
                data: obj,
                datatype: "json",
                complete: function (xhr) {
                }
            });
            if (el == "state") {
                clicked.parents('div.state').hide();
                var childs = $("[class^=state]");
                childs.click(function () {
                    $(this).hide();
                    if (!childs.filter(':visible').length) {
                        clicked.parents('li').hide();
                    }
                });
            }
            if (el == "model" || cityId == 0) {
                clicked.parents('li').hide();
            }
        }
        else {
            e.stopPropagation();
        }

    });

    $('#ddlModels').on('change', function () {
        var option = $(this).find(':selected').val();
        if (option == "") {
            $('#ddlModels option').each(function () {
                $(this).prop('selected', true);
            });
            $('#ddlModels option').first().prop('selected', false);
            $(this).material_select();
        }
      });

    $('#ddlCities').on('change', function () {
        var option = $(this).find(':selected').val();
        if (option == "") {
            $('#ddlCities option').each(function () {
                $(this).prop('selected', false);
            });
            $(this).material_select();
        }
       else if ($('#ddlCities option:selected').length > 1) {
            $('#ddlCities option').first().prop('selected', false);
       }       
    });

    showAllIndiaAlert = function () {
        Materialize.toast("This campaign is configured on ex-Showroom prices. You can run it for all India only!", 5000);
    };

    showDeleteAlert = function (element) {
        return confirm("Are you sure you want to delete all rules for " + element + "?");
    };

    getCitiesFromDropDown = function () {
        cityIds = "";
        $('#ddlCities :selected').each(function () {
            if ($(this).val()) {
                cityIds = cityIds + $(this).val() + ',';
            }
            });
        if ((cityIds == "" || cityIds==undefined) && mfgVM.selectedState()>0)
            stateIds = mfgVM.selectedState();
    };

    validateInput = function () {
        var isValid = true;
        var rdoModel = $("[name=rdoModel]:checked").val();
        switch (rdoModel) {
            case "1":
                if (!$("#ddlModels :selected").val()) {
                    isValid = false;
                    Materialize.toast("Please select one or more models", 6000);
                }
                break;
            case "2":
                modelIds = $("#txtModels").val().trim();
                var pat = /^[\d\s,]*$/;
                if (!pat.test(modelIds)) {
                    validate.inputField.showError($("#txtModels"));
                    isValid = false;
                }
                break;
        }
        if ($("#ddlModels :selected").val().length == 0 && modelIds.length == 0) {
            isValid = false;
            Materialize.toast("Please enter one or more models", 6000);
        }
        var rdoLoc = $("[name=location]:checked").val();
        switch (rdoLoc) {
            case "1":
                cityIds = "0";
                stateIds = null;
                break;
            case "2":
                cityIds = null;
                break;
            case "3":
                stateIds = null;
                getCitiesFromDropDown();
                break;
            case "4":
                cityIds = $("#txtCities").val().trim();
                var pat = /^[\d\s,]*$/;
                if (!pat.test(cityIds)) {
                    validate.inputField.showError($("#txtCities"));
                    isValid = false;
                }
                break;
        }
        if ((cityIds == "" || cityIds == null) && (stateIds == "" || stateIds == null)) {
            isValid = false;
            Materialize.toast("Please select location", 6000);
        }
        return isValid;
    };

    $("#btnAddRules").click(function () {
        var valid = validateInput();
        if (valid)
            setRulesData();
        return valid;
    });

    setRulesData = function () {
        $("#ddlModels :selected").each(function () {
            if ($(this).val())
                modelIds = modelIds + $(this).val() + ',';
        });
        $('#hdnModelIdList').val(modelIds);
        $('#hdnCityIdList').val(cityIds);
        $('#hdnStateIdList').val(stateIds);
        $('#hdnIsAllIndia').val(mfgVM.isAllIndia());
    };

    $('.m9 .collapsible .collapsible-header').on('click', function (event) {
        var target = $(this);
        setTimeout(function () {
            if (target.length) {
                event.preventDefault();
                $('html, body').animate({
                    scrollTop: target.offset().top
                }, 500);
            }
        }, 300);
    });

});