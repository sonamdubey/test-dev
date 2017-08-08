function bindManageBookingAmount() {
    var self = this;
    self.makeMsg = ko.observable("");
    self.modelMsg = ko.observable("");
    self.versionMsg = ko.observable("");
    self.bookingMsg = ko.observable("");
    self.selectedMakeId = ko.observable();
    self.selectedModelId = ko.observable();
    self.selectedVersionId = ko.observable();
    self.bookingAmount = ko.observable();
    self.models = ko.observableArray([]);
    self.versions = ko.observableArray([]);
    self.getModels = function () {
        var makeId = self.selectedMakeId();
        if (makeId && makeId > 0) {
            $.ajax({
                type: "GET",
                url: "/api/makes/" + makeId + "/models/2/",
                contentType: "application/json",
                dataType: 'json',
                async: false,
                success: function (response) {
                    if (response) {
                        self.models(response);
                    }
                },
                complete: function (xhr) {
                    if (xhr.status != 200) {
                        showToast('some error occurred');
                        self.models([]);
                    }
                    $('select').material_select();
                }
            });
        }
    };
    self.getVersions = function () {
        var modelId = self.selectedModelId();
        if (modelId && modelId > 0) {
            $.ajax({
                type: 'GET',
                url: '/api/models/' + modelId + '/versions/2/',
                contentType: 'application/json',
                async: false,
                success: function (response) {
                    if (response) {
                        self.versions(response);
                    }
                },
                complete: function (xhr) {
                    if (xhr.status != 200) {
                        showToast('some error occurred');
                        self.versions([]);
                    }
                    $('select').material_select();
                }
            });
        }
    };
    self.validateBookingSubmit = function () {
        var isValid = true;
        self.makeMsg("");
        self.modelMsg("");
        self.versionMsg("");
        self.bookingMsg("");
        if (self.selectedMakeId() == null || self.selectedMakeId() == "") {
            self.makeMsg("please select a make");
            isValid = false;
        }

        if (self.selectedModelId() == null || self.selectedModelId() == "") {
            self.modelMsg("please select a model");
            isValid = false;
        }

        if (self.selectedVersionId() == null || self.selectedVersionId() == "") {
            self.versionMsg("please select a version");
            isValid = false;
        }

        if (self.bookingAmount() == null || self.bookingAmount() < 0) {
            self.bookingMsg("please input valid amount");
            isValid = false;
        }
        return isValid;
    };

    self.editBooking = function (e) {
        element = $(e.target).closest("tr");
        self.selectedMakeId($(element[0]).data("makeid"));
        self.selectedModelId($(element[0]).data("modelid"));
         $('select').material_select();
        self.selectedVersionId($(element[0]).data("versionid"));
        self.bookingAmount($(element[0]).data("amount"));
    };
}

$(document).ready(function () {
    $('select').material_select();
    ko.applyBindings(new bindManageBookingAmount);
});

function showToast(msg) {
    Materialize.toast(msg, 4000);
}