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
            } else {
                self.versions([]);
                $('#selectVersion').material_select();
            }
        };
        self.validateBookingSubmit = function () {
            var isValid = true;
            self.bookingMsg(undefined);
            $('#bookingAmount').removeClass('invalid');
            var bookingAmount = self.bookingAmount();
            if (!bookingAmount || !/^\d+$/.test(bookingAmount)) {
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
            if ($('#bookingId').val()) {
                $("tr[data-id='" + $('#bookingId').val() + "']")[0].bgColor = "";
            }
            element[0].bgColor = '#e0f2f1';
            $('#bookingId').val($(element[0]).data("id"));

            self.clearDropdown($('#selectMake'), $(element[0]).data("makeid"));
            self.clearDropdown($('#selectModel'), $(element[0]).data("modelid"));
            self.clearDropdown($('#selectVersion'), $(element[0]).data("versionid"));

            self.bookingAmount($(element[0]).data("amount"));

            Materialize.updateTextFields();
            $('#cancelEdit').removeClass("hide");
        };

        self.cancelEdit = function () {
            $("tr[data-id='" + $('#bookingId').val() + "']")[0].bgColor = "";
            self.bookingMsg("");
            $('#btnSave').html('Save');
            $('#bookingId').val("");
            self.bookingAmount("");
            self.versions([]);
            $('#selectVersion').prop('disabled', false);
            $('#selectVersion').material_select();
            self.models([]);
            $('#selectModel').prop('disabled', false);
            $('#selectModel').material_select();
            $('#selectMake').prop('disabled', false);
            $('#selectMake').val(0);
            $('#selectMake').material_select();
            $('#selectMake').trigger('change');
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
                            showToast('Booking amount deleted');
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
            dropdownName.prop('disabled', true);
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
    var updateMessage = $('#manageBookingAmount').data('message');
    if (updateMessage) {
        showToast(updateMessage);
    }
});

function showToast(msg) {
    Materialize.toast(msg, 3000);
}