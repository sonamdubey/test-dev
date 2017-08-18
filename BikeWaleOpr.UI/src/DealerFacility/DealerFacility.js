var facilityActiveStatus = null;
var record = { facilityName: "", isActive: "", dealerId: "", facilityId: "", lastUpdatedById: "" };
var pageContainer = $("#dealerFacility");
var today = new Date();

var dealerFacilityOperations = function () {
    var self = this;
    self.facility = ko.observable("");
    self.facilityId = ko.observable("");
    self.activeStatus = ko.observable(false);
    self.isFacilityEdit = ko.observable(false);
    self.dealerId = $('#txtDealerId').val();
    self.lastUpdatedById = $('#dealerFacility').data('currentuserid');
    self.currentUserName = $('#dealerFacility').data('currentuser');
    self.dealerOperationsModel = ko.observable(new dealerOperationModel(dpParams));

    self.activeIcon = function (iconStatus) {

        try {
            if (iconStatus) {
                facilityActiveStatus = ('<i class="material-icons icon-green">done</i>');
            }
            else {
                facilityActiveStatus = ('<i class="material-icons icon-red">clear</i>');
            }
            return facilityActiveStatus;
        }

        catch (e) {
            console.warn(e);
        }
    };

    self.addNewRow = function (record) {

        try {
            var tableBody = $('#tblFacilityBody');
            var newRow = ('<tr data-id="">' +
                '<td data-element="facilityid"></td>' +
                '<td data-element="facilityname"></td>' +
                '<td data-element="activeicon" data-status=' + record.isActive + '>' +
                '<td data-element="editicon"><a href="javascript:void(0)" data-bind="click : editFacility"><i class="material-icons icon-blue" style="line-height: 32px;">mode_edit</i></a></td>' +
                '<td data-element="updatedby"></td>' +
                '<td data-element="updatedon"></td>' +
                '</tr>');

            $(newRow).prependTo("table > tbody:last");
            trNew = tableBody.find("tr:first");

            if (record.facilityId > 0) {
                self.facilityId = record.facilityId;
            }

            var innerbutton = ('<a href="javascript:void(0)" data-bind="click : $root.editFacility"><i class="material-icons icon-blue" style="line-height: 32px;">mode_edit</i></a>');

            facilityActiveStatus = self.activeIcon(record.isActive);

            trNew.attr('data-id', self.facilityId);
            trNew.find('td[data-element="facilityid"]').text(self.facilityId);
            trNew.find('td[data-element="facilityname"]').text(record.facilityName);
            trNew.find('td[data-element="activeicon"]').html(facilityActiveStatus);
            trNew.find('td[data-element="editicon"]').html(innerbutton);
            trNew.find('td[data-element="updatedby"]').text(self.currentUserName);
            trNew.find('td[data-element="updatedon"]').text(today.toLocaleString());

            var newRowElement = $('table tbody tr[data-id=' + self.facilityId + ']')

            ko.applyBindings(self, newRowElement[0]);
        }

        catch (e) {
            console.warn(e);
        }

    };


    self.addFacility = function () {
        try {
            var status = self.validate();
            if ($('#chkActiveStatus').is(':checked')) {
                self.activeStatus(true);
            }
            else {
                self.activeStatus(false);
            }
            record.facilityName = self.facility();
            record.isActive = self.activeStatus();
            record.dealerId = self.dealerId;
            record.lastUpdatedById = self.lastUpdatedById;
            if (status) {
                if (self.dealerId > 0) {

                    $.ajax({
                        type: "POST",
                        url: "/api/dealers/" + record.dealerId + "/facilities/",
                        contentType: "application/json",
                        data: ko.toJSON(record),
                        success: function (data) {
                            record.facilityId = data;
                            self.addNewRow(record);
                            self.facility("");
                            Materialize.toast('Successfully added dealer facility', 4000);
                        },
                        error: function (e) {
                            Materialize.toast('error occured', 4000);
                        }
                    });

                }

            }

        }
        catch (e) {
            console.warn(e);
        }


    };

    self.editFacility = function (d, e) {

        try {
            var facilityName = $('#txtFacilityName');
            var thisrow = $(e.currentTarget).closest('tr');
            var facilityLabel = facilityName.next();

            if (facilityName.hasClass("Invalid")) {
                facilityName.removeClass("Invalid");
            }

            if (!facilityLabel.hasClass("active")) {
                facilityLabel.addClass("active");
            }

            self.facilityId = thisrow.find('td[data-element="facilityid"]').text();
            self.facility(thisrow.find('td[data-element="facilityname"]').text());

            var isChecked = (thisrow.find('td[data-element="activeicon"]').attr('data-status').toLowerCase() == 'true')

            $('#chkActiveStatus').prop('checked', isChecked);
            self.activeStatus(isChecked);

            self.isFacilityEdit(true);

            var tab = pageContainer.find(".collapsible-header").first();
            if (!tab.hasClass("active")) {
                tab.click();
            }
        }
        catch (e) {
            console.warn(e);
        }

    };

    self.validate = function () {

        try {
            var currentEle = $('#txtFacilityName');
            var isValid = true;
            if (currentEle.val().trim() == '') {
                currentEle.addClass("Invalid");
                isValid = false;
            }
            else {
                currentEle.removeClass("Invalid");
                isValid = true;
            }
            return isValid;
        }
        catch (e) {
            console.warn(e);
        }
    };

    self.updateFacility = function (d, e) {

        try {
            var status = self.validate();
            if ($('#chkActiveStatus').is(':checked')) {
                self.activeStatus(true);
            }
            else {
                self.activeStatus(false);
            }

            record.facilityName = self.facility();
            record.isActive = self.activeStatus();
            record.facilityId = self.facilityId;
            record.lastUpdatedById = self.lastUpdatedById;
            record.dealerId = self.dealerId;
            facilityActiveStatus = self.activeIcon(record.isActive);

            if (status) {
                if (self.facilityId > 0) {

                    $.ajax({
                        type: "POST",
                        url: "/api/dealers/" + (record.dealerId) + "/facilities/" + (record.facilityId),
                        contentType: "application/json",
                        data: ko.toJSON(record),
                        success: function () {
                            var row = $('table tbody tr[data-id=' + record.facilityId + ']')
                            // updating the selected row       
                            row.find('td[data-element="facilityid"]').text(self.facilityId);
                            row.find('td[data-element="facilityname"]').text(self.facility());
                            row.find('td[data-element="activeicon"]').html(facilityActiveStatus);
                            row.find('td[data-element="activeicon"]').attr('data-status', self.activeStatus());
                            row.find('td[data-element="updatedby"]').text(self.currentUserName);
                            row.find('td[data-element="updatedon"]').text(today.toLocaleString());

                            Materialize.toast('Successfully updated dealer facility', 4000);

                        },
                        error: function (e) {
                            Materialize.toast('error occured', 4000);
                        }
                    });

                }
            }

        }
        catch (e) {
            console.warn(e);
        }

    };

    self.cancelEditFacility = function (d, e) {
        try {
            self.facility(undefined);
            self.activeStatus(false);
            self.isFacilityEdit(false);
            $('#txtFacilityName').removeClass("Invalid");
        }
        catch (e) {
            console.warn(e);
        }

    };

};

var viewModel = new dealerFacilityOperations();
ko.applyBindings(viewModel, $("#dealerFacility")[0]);

(function () {
    $('select.chosen-select').chosen({
        "width": "250px"
    });

    $('#ddlDealerOperations').val(3);
    $("#ddlDealerOperations").trigger('chosen:updated')
}());
