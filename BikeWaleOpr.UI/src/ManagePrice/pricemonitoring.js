//Created By : Ashutosh Sharma
//Discription : JS file for price monitoring page.
$(document).ready(function () {
    ko.applyBindings(new bikeModelsViewModel(), $("#makeModelContainer")[0]);
    $("#btnShow").prop('disabled', true);

    $("#ddlMakes").change();
    $("#ddlModels").val($("#ddlModels").attr("data-content"));
    $("#ddlModels").material_select();
});
function bikeModelsViewModel() {
    var self = this;
    self.modelList = ko.observableArray(null);
    self.selectedMake = ko.observable();
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
                    response.unshift({ "ModelId": "0", "ModelName": "Any" });
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
            $("#btnShow").prop('disabled', false);
        }
        else {
            self.modelList([]);
            $("#ddlModels").material_select();

        }
    };


}