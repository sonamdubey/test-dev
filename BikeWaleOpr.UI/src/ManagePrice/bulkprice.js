function bulkPriceViewModel() {
    var self = this;
    self.selectedMake = ko.observable("");
    self.selectedOperation = ko.observable("");
    self.selectedState = ko.observable();
    self.checkValidation = function (data,event) {
        if(self.selectedOperation()=="")
        {
            shouldShowOperationMsg(true);
            return false; 
        }
        else
        {
            if (self.selectedOperation() == "mappedcities")
            {
                if (self.selectedState() == "") {
                    shouldShowStateMsg(true);
                    return false;
                }
                else {
                    $('#maketext').remove();
                    return true;
                }    
            }
            else
            {
                if (self.selectedMake() == "") {
                    shouldShowMakeMsg(true);
                    return false;
                }
                else {
                    $('#statetext').remove();
                    return true;
                }
            }
        }
    }
    self.computedUrl = ko.computed(function () {
        if (self.selectedOperation() == "mappedcities") {
            $('#statetext').val($('#selectState').find('option:selected').text().trim());
            return "/prices/bulkupload/mappedcities" + "/state/"+ selectedState() + '/';
        }
        else if (self.selectedOperation() == "mappedbikes") {
            $('#maketext').val($('#selectMake').find('option:selected').text().trim());
            return "/prices/bulkupload/mappedbikes/make/" + selectedMake() + "/";
        }
        else {
            $('#maketext').val($('#selectMake').find('option:selected').text().trim());
            return "/prices/bulkupload/make/"+ selectedMake() +"/";
        }
    });
    
    self.shouldShowMakeMsg= ko.observable(false);
    self.shouldShowOperationMsg = ko.observable(false);
    self.shouldShowStateMsg = ko.observable(false);
}
$(function () {
    ko.applyBindings(bulkPriceViewModel, $("#operationSection")[0]);
    $('.chosen-select').chosen({
        no_results_text: "Oops, nothing found!",
        width: "100%",
        rtl: true
    });
    $('.tooltipped').tooltip();
    $('#selectMake').val($('#selected_make').val()).change();
    $('#selectMake').trigger('chosen:updated');
    $('#selectOperation').val($('#operation').val()).change();
    $('#selectOperation').trigger('chosen:updated');
    $('#selectState').val($('#selected_state').val()).change();
    $('#selectState').trigger('chosen:updated');

    if ($('#showMessage').length != 0) {
        var makeName = $("#selectMake option:selected").text();
        Materialize.toast('Mapped Bikes for' + makeName + 'are not available', 4000);
    }
});
