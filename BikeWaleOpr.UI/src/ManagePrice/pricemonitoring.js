var vmPriceMonitoringIndex = new bikeModelsViewModel();

$(document).ready(function () {
    ko.applyBindings(vmPriceMonitoringIndex, $("#makeModelContainer")[0]);
   
    init();
});

function init() {
    try {
        var ddlStates = $("#ddlStates");
        var ddlMakes = $("#ddlMakes");
        var ddlModels = $("#ddlModels");

        ddlStates.material_select();
        ddlMakes.material_select();

        vmPriceMonitoringIndex.selectedMake(ddlMakes.data('makeid'));
        vmPriceMonitoringIndex.selectMake();
        vmPriceMonitoringIndex.selectedModel(ddlModels.data('modelid'));

        ddlModels.material_select();
    } catch (e) {

    }
}

function bikeModelsViewModel() {
    var self = this;
    self.modelList = ko.observableArray(null);
    self.selectedState = ko.observable($("#ddlStates").data("stateid"));
    self.selectedMake = ko.observable($("#ddlMakes").data('makeid'));
    self.selectedModel = ko.observable();
    
    self.selectMake = function () {
        self.selectedMake($('#ddlMakes option:selected').val());
        if (self.selectedMake() != 'undefined' && self.selectedMake() > 0) {
            $.ajax({
                type: "GET",
                url: "/api/manageprices/getmodels/" + self.selectedMake() + "/",
                datatype: "json",
                async: false,
                success: function (response) {
                    response.unshift({ "Id": "0", "Name": "All" });
                    var models = ko.toJS(response);
                    if (models) {
                        self.modelList(models);
                    }
                },
                complete: function () {
                    $("#ddlModels").material_select();
                    
                },
                error: function () {
                    Materialize.toast("Connection Problem/ Internal Server Error", 4000);
                }
            });
            //if ($("#ddlStates").val() != 0) {
            //    $("#btnShow").prop('disabled', false);
            //}
            
        }
        else {
            self.modelList([]);
            $("#ddlModels").material_select();

        }
    };

    self.selectState = function () {
        //if ($("#ddlMakes").val() != 0) {
        //    $("#btnShow").prop('disabled', false);
        //}
    }
}