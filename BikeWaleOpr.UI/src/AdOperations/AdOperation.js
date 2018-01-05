if (msg != "") { Materialize.toast(msg, 4000); }
var ddlModels = $('#ddlModels');


var adOperationsViewModel = function () {

    var self = this;

   
    self.selectedMake = ko.observable();
    self.listModels = ko.observableArray([]);

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
    self.saveAdOperation = function () {
        var basicDetails = {
            "make":
                {
                    "MakeId": $('#ddlMakes').val()
                },
            "model": {
                "ModelId": $('#ddlModels').val()
            },
            "startTime": $('#startDateEle').val() + ' ' + $('#startTimeEle').val(),
            "endTime": $('#endDateEle').val() + ' ' + $('#endTimeEle').val(),
            "adOperationType": $('#chkShowPromotion').is(':checked') ? 1 : 2,
            "userId": userId
            
        
        }
        $.ajax({
            type: "POST",
            url: "/api/adoperations/save/",
            contentType: "application/json",
            data: ko.toJSON(basicDetails),
            success: function (response) {
               
                
                Materialize.toast('AdOperation saved', 4000);
               
            }
        });
    };
    self.updateAdOperation = function (e) {
        var currentRow = $(e.target).closest("tr").first();
        var basicDetails = {
            "promotedBikeId": $(currentRow).data("promotedbikeid"),
            "AdOperationType": $(currentRow).find("td").data("adoperationtype"),
            "LastUpdatedById": userId,
            "ContractStatus": $(currentRow).find("td").data("contractstatus")
        }

        $.ajax({
            type: "POST",
            url: "/api/adoperations/update/",
            contentType: "application/json",
            data: ko.toJSON(basicDetails),
            success: function (response) {


                Materialize.toast('Selected bike deleted', 4000);

            }
        });

    };
}


$(document).ready(function () {

    mfgVM = new adOperationsViewModel;
    ko.applyBindings(mfgVM, $('#adOperationContainer')[0]);

    var $dateInput = $('.datepicker').pickadate({
        selectMonths: true,
        closeOnSelect: true,
        onClose: function () { if (dateValue != $("#reviewDateEle").val()) $("#reviewDate").val($("#reviewDateEle").val()); },
        onOpen: function () { dateValue = $("#reviewDateEle").val() },
        onSet: function (ele) { if (ele.select) { this.close(); } }
    });
});