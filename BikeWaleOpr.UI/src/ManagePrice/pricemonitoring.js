var vmPriceMonitoringIndex = new bikeModelsViewModel();

$(document).ready(function () {

    try {

        ko.applyBindings(vmPriceMonitoringIndex, $("#makeModelContainer")[0]);
        init();
        if ($("#priceMonitoringTable").length == 0 && $("#ddlStates").data('stateid') != 0)
            Materialize.toast("No price details/versions available for selected model.", 7000);
            
    } catch (e) {

    }

});

function init() {
    try {
        var ddlStates = $("#ddlStates");
        var ddlMakes = $("#ddlMakes");
        var ddlModels = $("#ddlModels");
        var modelList = $("#hdnModelList").val();

        if (modelList != "null") {
            modelList = JSON.parse(modelList);
            modelList.unshift({ "modelId": 0, "modelName": "All" });
            vmPriceMonitoringIndex.modelList(modelList);
        }

        vmPriceMonitoringIndex.selectedModel(ddlModels.data('modelid'));

        ddlStates.material_select();
        ddlMakes.material_select();
        ddlModels.material_select();

    } catch (e) {
        console.warn(e.message);
    }
}

function bikeModelsViewModel() {
    var self = this;
    self.modelList = ko.observableArray(null);
    self.selectedState = ko.observable($("#ddlStates").data("stateid"));
    self.selectedMake = ko.observable();
    self.selectedModel = ko.observable();

    self.selectMake = function () {
        if (self.selectedMake() != 'undefined' && self.selectedMake() > 0) {
            $.ajax({
                type: "GET",
                url: "/api/makes/" + self.selectedMake() + "/models/2/",
                datatype: "json",
                async: false,
                success: function (response) {
                    if ($.trim(response)) {
                        response.unshift({ "modelId": "0", "modelName": "All" });
                    }
                    else {
                        Materialize.toast("No model available for selected make.", 7000);
                    }
                    self.modelList(response);

                },
                complete: function () {
                    $("#ddlModels").material_select();

                },
                error: function () {
                    Materialize.toast("Connection Problem/ Internal Server Error", 7000);
                }
            });

        }
        else {
            self.modelList([]);
            $("#ddlModels").material_select();

        }
    };
}