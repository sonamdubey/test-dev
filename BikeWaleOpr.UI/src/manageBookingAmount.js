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
        self.dealerOperationsModel = ko.observable(new dealerOperationModel(dpParams));


        self.getModels = function () {
            try{
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
                                Materialize.toast('some error occurred', 3000);
                                self.models([]);
                            }
                            self.versions([]);
                            $('#selectModel').material_select();
                        }
                    });
                }
            } catch (e) {
                console.warn(e.message);
            }
        };
        self.getVersions = function () {
            try {
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
                                Materialize.toast('some error occurred', 3000);
                                self.versions([]);
                            }
                            $('#selectVersion').material_select();
                        }
                    });
                } else {
                    self.versions([]);
                    $('#selectVersion').material_select();
                }
            } catch (e) {
                console.warn(e.message);
            }
        };
        self.validateBookingSubmit = function () {
            try {
                var isValid = true;
                self.bookingMsg(undefined);
                $('#bookingAmount').removeClass('invalid');
                var bookingAmount = self.bookingAmount();
                if (!bookingAmount || !/^\d+$/.test(bookingAmount)) {
                    self.bookingMsg("please input valid amount");
                    isValid = false;
                }
                if (!self.selectedMakeId() || self.selectedMakeId() == "") {
                    Materialize.toast("Please select Make and Model", 3000);
                    isValid = false;
                    return isValid;
                }

                if (!self.selectedModelId() || self.selectedModelId() == "") {
                    Materialize.toast("please select a model", 3000);
                    isValid = false;
                    return isValid;
                }
                return isValid;
            } catch (e) {
                console.warn(e.message);
            }
            return false;
        };

        self.editBooking = function (e) {
            try {
                self.bookingMsg("");
                $('#btnSave').html('Update');
                var element = $(e.target).closest("tr");
                if ($('#bookingId').val()) {
                    $("tr[data-id='" + $('#bookingId').val() + "']")[0].bgColor = "";
                }
                if (element) {
                    element[0].bgColor = '#e0f2f1';
                    $('#bookingId').val($(element[0]).data("id"));

                    self.clearDropdown($('#selectMake'), $(element[0]).data("makeid"));
                    self.clearDropdown($('#selectModel'), $(element[0]).data("modelid"));
                    self.clearDropdown($('#selectVersion'), $(element[0]).data("versionid"));

                    self.bookingAmount($(element[0]).data("amount"));

                    Materialize.updateTextFields();
                    $('#cancelEdit').removeClass("hide");
                }

                $('.booking-amount-collapsible').each(function(){
                    if (!$(this).hasClass('active'))
                        $(this).click();
                });
            } catch (e) {
                console.warn(e.message);
            }
        };

        self.cancelEdit = function () {
            try {
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
                $('#selectMake').val(parseInt($('#makeId').val()));
                $('#selectMake').material_select();
                $('#selectMake').trigger('change');
                $('#cancelEdit').addClass("hide");
                Materialize.updateTextFields();
            } catch (e) {
                console.warn(e.message);
            }
        };

        self.deleteBooking = function (e) {
            try {
                var element = $(e.target).closest("tr");
                if (element) {
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
                                    Materialize.toast('Booking amount deleted', 3000);
                                },
                                complete: function (xhr) {
                                    if (xhr.status != 200) {
                                        Materialize.toast('some error occurred', 3000);
                                    }
                                }
                            });
                        }
                    }
                    else {
                        Materialize.toast("Booking alreay deleted", 3000);
                    }
                }
            } catch (e) {
                console.warn(e.message);
            }
        };

        self.clearDropdown = function (dropdownName, index) {
            dropdownName.prop('disabled', true);
            dropdownName.val(index);
            dropdownName.material_select();
            dropdownName.trigger('change');
        };

        self.selectedMakeId(parseInt($('#makeId').val()));
        

    } catch (ex) {
        console.warn(e.message);
    }
}

$(document).ready(function () {
    try {
        ko.applyBindings(new bindManageBookingAmount);
        $('#selectMake').material_select();
        $('#selectMake').trigger('change');
        var updateMessage = $('#manageBookingAmount').data('message');
        if (updateMessage) {
            Materialize.toast(updateMessage, 3000);
        }
    } catch (e) {
        console.warn(e.message);
    }

    $('select.chosen-select').chosen({
        "width": "250px"
    });

    $('#ddlDealerOperations').val(6);
    $("#ddlDealerOperations").trigger('chosen:updated');
});
