function bindManageBookingAmount() {
    var self = this;
    self.makeMsg = ko.observable();
    self.modelMsg = ko.observable();
    self.versionMsg = ko.observable();
    self.bookingMsg = ko.observable();
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
                url: "/api/makes/" + makeId + "/models/New/",
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
                url: '/api/models/' + modelId + '/versions/New/',
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
        if (self.selectedMakeId() == null && self.selectedMakeId() == "") {
            self.makeMsg("please select a make");
            isValid = false;
        }

        if (self.selectedModelId() == null && self.selectedModelId() == "") {
            self.modelMsg("please select a model");
            isValid = false;
        }

        if (self.selectedVersionId() == null && self.selectedVersionId() == "") {
            self.versionMsg("please select a version");
            isValid = false;
        }

        //if()
    };
}

$(document).ready(function () {
    $('select').material_select();
    ko.applyBindings(new bindManageBookingAmount);
});

function showToast(msg) {
    $('.toast').text(msg).stop().fadeIn(400).delay(3000).fadeOut(400);
}