var ddlModels = $('#ddlModels');
var ddlCities = $('#ddlCities');
var cityIds="",stateIds="",modelIds="",getCitiesFromDropDown, setRulesData;
var manufacturerRulesViewModel = function () {
    var self = this;
    self.isAllIndia = ko.observable(false);
    self.selectedMake = ko.observable();
    self.listModels = ko.observableArray([]);
    self.selectedState = ko.observable();
    self.listCities = ko.observableArray([]);
    self.listStates = ko.observableArray([]);
    self.isMultipleStates = ko.observable(false);
    self.selectMake = function () {
        self.selectedMake($('#ddlMakes option:selected').val());
        if (self.selectedMake() != undefined && self.selectedMake() > 0) {
            $.ajax({
                type: "GET",
                url: bwHostUrl + "/api/campaigns/manufacturer/models/makeId/"+self.selectedMake()+"/",
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
    };
    self.selectState = function () {
        self.listStates([]);
        var array = new Array();
        $('#ddlStates :selected').each(function() {
            array.push($(this).val());
        });
        self.listStates(array)
        if (self.listStates().length > 1)
        {
            self.isMultipleStates(true);
            stateIds = "";
            ko.utils.arrayForEach(self.listStates(), function (state) {
                 if(state)
                     stateIds = stateIds + "," + state;
            });
                
        }
        else {
            self.selectedState($('#ddlStates option:selected').val());
            if (self.selectedState() != undefined && self.selectedState() > 0) {
                stateIds = self.selectedState();
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
        }
    };

   
    self.showCities = ko.computed(function () {
        return (self.listStates() != null && self.listStates().length === 1);
    });    
};

$(document).ready(function () {
   
    mfgVM = new manufacturerRulesViewModel;
    ko.applyBindings(mfgVM, $('#configRules')[0]);
    $('.chip .close').on('click', function () {
        var clicked = $(this);
        var modelId = clicked.attr('data-modelid');
        var stateId = clicked.attr('data-stateid');
        var cityId = clicked.attr('data-cityid');
        var campaignId = $('#CampaignId').val();
        var userId = $('#UserId').val();
        var isAllIndia = clicked.attr('data-allindia');
        var obj = {
            'campaignId': campaignId,
            'modelId': modelId,
            'cityId': cityId,
            'stateId': stateId,
            'userId': userId,
            'isAllIndia' : isAllIndia
        }
        $.ajax({
            type: "POST",
            url: bwHostUrl + "/api/campaigns/manufacturer/deleterule/",
            data:obj,
            datatype: "json",
            complete: function (xhr) {
                
            }
        });
    });

    getCitiesFromDropDown = function () {
        if (!mfgVM.isMultipleStates()) {
            $('#ddlCities :selected').each(function () {
                if ($(this).val())
                cityIds = cityIds + $(this).val() + ',';
            });
        }
        else
            cityIds = null;
    };

    $("#btnAddRules").click(function () {
        var isValid = true;
        if (!$("#ddlModels :selected").val()) {
            isValid = false;
            Materialize.toast("Please select one or more models", 6000);
        }
        if (isValid)
            setRulesData();
        return isValid;
    });

    setRulesData = function () {
        $("#ddlModels :selected").each(function () {
            if($(this).val())
            modelIds = modelIds + $(this).val() + ',';
        });
        if (!mfgVM.isAllIndia()) {
            var rdoLoc = $("[name=location]:checked").val();
            switch (rdoLoc) {
                case "1":
                    cityIds = $("#txtCities").val();
                    break;
                case "2":
                    getCitiesFromDropDown();
                    break;
            }
        }
        else {
            cityIds = null;
            stateIds = null;
        }

        $('#hdnModelIdList').val(modelIds);
        $('#hdnCityIdList').val(cityIds);
        $('#hdnStateIdList').val(stateIds);
        $('#hdnIsAllIndia').val(mfgVM.isAllIndia());
    };
    
});