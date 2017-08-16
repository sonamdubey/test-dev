function bindManageBookingAmount() {
    try{
        var self = this;
        self.selectedMakeId = ko.observable();
        self.selectedModelId = ko.observable();
        self.selectedVersionId = ko.observable();
        self.bookingAmount = ko.observable();
        self.models = ko.observableArray([]);
        self.versions = ko.observableArray([]);
        self.bookingMsg = ko.observable("");
        var params = window.location.href.slice(window.location.href.indexOf('?') + 1).split('&');
        var dealerId = params[0].split('=')[1];
        $('#dealerId').val(dealerId);

        self.getModels = function () {
            var makeId = self.selectedMakeId();
            var requestType = $('#requestType').val();
            if (makeId && makeId > 0) {
                $.ajax({
                    type: "GET",
                    url: "/api/makes/" + makeId + "/models/" + requestType + "/",
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
                        self.versions([]);
                        $('#selectModel').material_select();
                    }
                });
            }
        };
        self.getVersions = function () {
            var modelId = self.selectedModelId();
            var requestType = $('#requestType').val();
            if (modelId && modelId > 0) {
                $.ajax({
                    type: 'GET',
                    url: '/api/models/' + modelId + '/versions/' + requestType + '/',
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
                        $('#selectVersion').material_select();
                    }
                });
            }
        };
        self.validateBookingSubmit = function () {
            var isValid = true;
            self.bookingMsg("");
            if (!self.bookingAmount() || self.bookingAmount() < 0) {
                self.bookingMsg("please input valid amount");
                isValid = false;
            }
            if (!self.selectedMakeId() || self.selectedMakeId() == "") {
                showToast("Please select Make and Model");
                isValid = false;
                return isValid;
            }

            if (!self.selectedModelId() || self.selectedModelId() == "") {
                showToast("please select a model");
                isValid = false;
                return isValid;
            }
            return isValid;
        };

        self.editBooking = function (e) {
            self.bookingMsg("");
            $('#btnSave').html('Update');
            var element = $(e.target).closest("tr");
            $('#bookingId').val($(element[0]).data("id"));

            self.clearDropdown($('#selectMake'), $(element[0]).data("makeid"));
            self.clearDropdown($('#selectModel'), $(element[0]).data("modelid"));
            self.clearDropdown($('#selectVersion'), $(element[0]).data("versionid"));

            self.bookingAmount($(element[0]).data("amount"));

            Materialize.updateTextFields();
            $('#cancelEdit').removeClass("hide");
        };

        self.cancelEdit = function () {
            self.bookingMsg("");
            $('#btnSave').html('Save');
            $('#bookingId').val("");
            self.bookingAmount("");
            self.versions([]);
            $('#selectVersion').material_select();
            self.models([]);
            $('#selectModel').material_select();
            $('#selectMake').val(0);
            $('#selectMake').material_select();
            $("#selectMake").trigger('change');
            $('#cancelEdit').addClass("hide");
            Materialize.updateTextFields();
        };

        self.deleteBooking = function (e) {
            var element = $(e.target).closest("tr");
            var isActive = $(element[0]).data("isactive");
            if (isActive == "True") {
                $('#bookingId').val($(element[0]).data("id"));
                if (confirm("Are you sure you want to delete this record")) {
                    $.ajax({
                        type: "GET",
                        url: "/api/Dealers/DeleteBookingAmount/?bookingId=" + $('#bookingId').val(),
                        success: function (response) {
                            var activeIcon = $(e.target).closest("tr").find('.isActive');
                            $(activeIcon).removeClass('icon-green');
                            $(activeIcon).addClass('icon-red');
                            $(activeIcon).html('close');
                            $(element[0]).data("isactive", "False");
                        },
                        complete: function (xhr) {
                            if (xhr.status != 200) {
                                showToast('some error occurred');
                            }
                        }
                    });
                }
            }
            else {
                showToast("Booking alreay deleted");
            }
        };

        self.clearDropdown = function (dropdownName, index) {
            dropdownName.val(index);
            dropdownName.material_select();
            dropdownName.trigger('change');
        };

    } catch (ex) {
        showToast(ex.message);
    }
}

$(document).ready(function () {
    $('#selectMake').material_select();
    ko.applyBindings(new bindManageBookingAmount);
});

function showToast(msg) {
    Materialize.toast(msg, 4000);
}