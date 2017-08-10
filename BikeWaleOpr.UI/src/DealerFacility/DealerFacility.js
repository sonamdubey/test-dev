

var facilityActiveStatus = null;
var record = { Facility: "", IsActive: "", Id: "", FacilityId: "", LastUpdatedById: "" };
var pageContainer = $("#dealerFacility");
var today = new Date();

$(document).ready(function () {
    try {
        BwOprHostUrl = document.getElementById("displayDealerFacility").getAttribute("data-BwOprHostUrl");
    }
    catch (e) {
        console.warn(e);
    }
});



var dealerFacilityOperations = function () {
    var self = this;
    self.facility = ko.observable("");
    self.facilityId = ko.observable("");
    self.activeStatus = ko.observable(false);
    self.isFacilityEdit = ko.observable(false);
    self.dealerId = $('#txtDealerId').val();
    self.lastUpdatedById = $('#dealerFacility').attr('data-currentUserId');
    self.currentUserName = $('#dealerFacility').attr('data-currentuser');
   

    self.activeIcon = function (iconStatus) {

        try
        {
            if (iconStatus) {
                facilityActiveStatus = ('<i class="material-icons icon-blue">check_box</i>');
            }
            else {
                facilityActiveStatus = ('<i class="material-icons icon-red">clear</i>');
            }
            return facilityActiveStatus;
        }
       
        catch (e) {
            console.warn(e);
        }
    }

    self.addNewRow = function (record) {

        try
        {
                var tableBody = $('#tblFacilityBody');
                var newRow = ('<tr data-id="">' +
                    '<td data-element="facilityid"></td>' +
                    '<td data-element="facilityname"></td>' +
                    '<td data-element="activeicon" data-status=' + record.IsActive + '>' +
                    '<td data-element="editicon"><a href="javascript:void(0)" data-bind="click : editFacility"><i class="material-icons icon-blue" style="line-height: 32px;">mode_edit</i></a></td>' +
                    '<td data-element="updatedby"></td>' +
                    '<td data-element="updatedon"></td>' +
                    '</tr>');

                $(newRow).prependTo("table > tbody:last");
                trNew = tableBody.find("tr:first");

                if (record.FacilityId > 0) {
                    self.facilityId = record.FacilityId;
                }

                var innerbutton = ('<a href="javascript:void(0)" data-bind="click : $root.editFacility"><i class="material-icons icon-blue" style="line-height: 32px;">mode_edit</i></a>');

                facilityActiveStatus = self.activeIcon(record.IsActive);

                trNew.attr('data-id', self.facilityId);
                trNew.find('td[data-element="facilityid"]').text(self.facilityId);
                trNew.find('td[data-element="facilityname"]').text(record.Facility);
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
       
    }


    self.addFacility = function () {
        try
        {
            var status = self.validate();
            if ($('#chkActiveStatus').is(':checked')) {
                self.activeStatus(true);
            }
            else {
                self.activeStatus(false);
            }
            record.Facility = self.facility();
            record.IsActive = self.activeStatus();
            record.Id       = self.dealerId;
            record.LastUpdatedById = self.lastUpdatedById;
            if (status) {
                if (self.dealerId > 0) {
              
                    $.ajax({
                        type: "POST",
                        url: BwOprHostUrl + "api/dealerfacility/add/",
                        data: record,
                        success: function (data) {
                            record.FacilityId = data;
                            self.addNewRow(record);
                            self.facility("");
                            Materialize.toast('successfully added', 4000);
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


    }

    self.editFacility = function (d, e) {

        try
        {
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

    }

    self.validate = function () {

        try
        {
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
    }

    self.updateFacility = function (d, e) {

        try
        {
            var status = self.validate();
            if ($('#chkActiveStatus').is(':checked')) {
                self.activeStatus(true);
            }
            else {
                self.activeStatus(false);
            }

            record.Facility = self.facility();
            record.IsActive = self.activeStatus();
            record.FacilityId = self.facilityId;
            record.LastUpdatedById = self.lastUpdatedById;

            facilityActiveStatus = self.activeIcon(record.IsActive);

            if (status) {
                if (self.facilityId > 0) {

                    $.ajax({
                        type: "POST",
                        url: BwOprHostUrl + "api/dealerfacility/update/",
                        data: record,
                        success: function () {
                            var row = $('table tbody tr[data-id=' + record.FacilityId + ']')
                            // updating the selected row       
                            row.find('td[data-element="facilityid"]').text(self.facilityId);
                            row.find('td[data-element="facilityname"]').text(self.facility());
                            row.find('td[data-element="activeicon"]').html(facilityActiveStatus);
                    
                            row.find('td[data-element="updatedby"]').text(self.currentUserName);
                            row.find('td[data-element="updatedon"]').text(today.toLocaleString());

                            Materialize.toast('successfully updated', 4000);

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

    }

    self.cancelEditFacility = function (d, e) {
        try
        {
            self.facility(undefined);
            self.activeStatus(false);
            self.isFacilityEdit(false);
            $('#txtFacilityName').removeClass("Invalid");
     }
      catch (e) {
            console.warn(e);
        }

    }

    };



var viewModel = new dealerFacilityOperations();
ko.applyBindings(viewModel, $("#dealerFacility")[0]);