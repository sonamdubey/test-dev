
var flag = null;
var selectedFacilityId = null;
var facilityIdVal = 0;
var record = { Facility: "", IsActive: "", Id: "", FacilityId: "" };
var pageContainer = $("#dealerFacility");
$(document).ready(function () {
    BwOprHostUrl = document.getElementById("displayDealerFacility").getAttribute("data-BwOprHostUrl");
    
});

function addNewRow(record) {
  
    var table = document.getElementById("tblFacilityBody");
    
   
    if (record.FacilityId != 0)
    {
        facilityIdVal = record.FacilityId;
    }
    var newRow = table.insertRow(0);
    var facilityIdCol = newRow.insertCell(0);
    var facilityNameCol = newRow.insertCell(1);
    var activeStatusCol = newRow.insertCell(2);
    var editCol = newRow.insertCell(3);
    var statusId = "id_" + facilityIdVal;
    var innerbutton = ('<td><a href="javascript:void(0)"><i class="material-icons icon-blue" style="line-height: 32px;">mode_edit</i></a></td>');
    var checkStatus = (record.IsActive ? "checked" : "unchecked");
    var innercheckbox = ('<input type="checkbox" id="' + statusId + '" value= "' + record.IsActive + '"' + checkStatus + '/> <label for="' + statusId + '"></label>')
    facilityIdCol.innerHTML = facilityIdVal;
    facilityNameCol.innerHTML = record.Facility;
    activeStatusCol.innerHTML = innercheckbox;
    editCol.innerHTML = innerbutton;
}


var DealerFacilityOperations =function ()
{
    var self = this;
    self.facility = ko.observable("");
    self.activeStatus = ko.observable(false);
    self.isFacilityEdit = ko.observable(false);
    var dealerId = $('#txtDealerId').val();


    self.addFacility= function()
    {
       
        if ($('#chkActiveStatus').is(':checked'))		
          {		
              self.activeStatus(true);		
          }
        record.Facility = self.facility();
        record.IsActive = self.activeStatus();
        record.Id = dealerId;
        debugger;
        if (dealerId > 0)
        {
            if (flag == null) {
                $.ajax({
                    type: "POST",
                    url: BwOprHostUrl + "api/dealerfacility/add/",
                    data: record,
                    success: function () {
                        Materialize.toast('successfully added', 4000);
                    },
                    complete: function (xhr) {
                        addNewRow(record);
                    },
                    error: function (e) {
                        Materialize.toast('error occured', 4000);
                    }
                });
            }
        }
       
        
    }

    self.editFacility = function (d, e) {
        var thisrow = $(event.currentTarget).parent().parent();
        selectedFacilityId = thisrow.find('td[data-facilityId]').text()
        self.facility(thisrow.find('td[data-facilityName]').text());
        self.isFacilityEdit(true);

        var tab = pageContainer.find(".collapsible-header").first();
        if (!tab.hasClass("active")) {
            tab.click();
        }
      
}
    self.updateFacility = function (d, e) {
        if ($('#chkActiveStatus').is(':checked')) {
            self.activeStatus(true);
        }
        else
        {
            self.activeStatus(false);
     }

        record.Facility = self.facility();
        record.IsActive = self.activeStatus();
        record.FacilityId = selectedFacilityId;
        if (selectedFacilityId > 0) {
            
                $.ajax({
                    type: "POST",
                    url: BwOprHostUrl + "api/dealerfacility/update/",
                    data: record,
                    success: function () {
                        Materialize.toast('successfully updated', 4000);

                    },
                    complete: function (xhr) {
                        var $row = $('td[data-facilityId="' + record.FacilityId + '"]');
                        $row.parent().remove();
                        addNewRow(record);
                    },
                    error: function (e) {
                        Materialize.toast('error occured', 4000);
                    }
                });
          
        }
    }

    self.cancelEditFacility =function(d,e)
    {
        self.facility(undefined);
        self.activeStatus(false);
        self.isFacilityEdit(false);

    }
}

var viewModel = new DealerFacilityOperations();
ko.applyBindings(viewModel, $("#dealerFacility")[0]);