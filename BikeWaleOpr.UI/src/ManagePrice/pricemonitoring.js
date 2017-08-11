var vmPriceMonitoringIndex = new bikeModelsViewModel();

$(document).ready(function () {

    try {

        ko.applyBindings(vmPriceMonitoringIndex, $("#makeModelContainer")[0]);
        init();
    } catch (e) {

    }

});

function init() {
    try {
        var ddlStates = $("#ddlStates");
        var ddlMakes = $("#ddlMakes");
        var ddlModels = $("#ddlModels");
        var modelList = ddlModels.data('model');
       

     

        if ($.trim(modelList)) {
            modelList.unshift({ "modelId": 0, "modelName": "All" });
            vmPriceMonitoringIndex.modelList(modelList);
        }
     
        vmPriceMonitoringIndex.selectedModel(ddlModels.data('modelid'));

        ddlStates.material_select();
        ddlMakes.material_select();
        ddlModels.material_select();
    } catch (e) {

    }
}

function bikeModelsViewModel() {
    try {
        var self = this;
        self.modelList = ko.observableArray(null);
        self.selectedState = ko.observable($("#ddlStates").data("stateid"));
        self.selectedMake = ko.observable();
        self.selectedModel = ko.observable();

        self.selectMake = function () {
            self.selectedMake($('#ddlMakes option:selected').val());
            if (self.selectedMake() != 'undefined' && self.selectedMake() > 0) {
                $.ajax({
                    type: "GET",
                    url:"/api/makes/"+ self.selectedMake() + "/models/2" + "/",
                    datatype: "json",
                    async: false,
                    success: function (response) {
                        response.unshift({ "modelId": "0", "modelName": "All" });
                        self.modelList(response);
                    },
                    complete: function () {
                        $("#ddlModels").material_select();

                    },
                    error: function () {
                        Materialize.toast("Connection Problem/ Internal Server Error", 4000);
                    }
                });

            }
            else {
                self.modelList([]);
                $("#ddlModels").material_select();

            }
        };


    } catch (e) {

    }
}