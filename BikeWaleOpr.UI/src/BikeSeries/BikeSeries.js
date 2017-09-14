function bindBikeSeriesData() {
    var self = this;
    self.seriesName = ko.observable("");
    self.seriesMsg = ko.observable("");
    self.seriesMaskingMsg = ko.observable("");
    self.selectedMakeId = ko.observable("");
    self.seriesMaskingName = ko.observable("");
    self.seriesName.subscribe(function () {
        var series = "";
        if (self.seriesName() && self.seriesName() != "") {
            series = self.seriesName().trim().replace(" ", "-").replace(/\s+/g, "").replace(/[^a-zA-Z0-9\-]+/g, '').toLowerCase();
            self.seriesMaskingMsg("");
        }
        self.seriesMaskingName(series);
        Materialize.updateTextFields();
    });
    
    self.selectedSeriesId = ko.observable(null);
    self.selectedSeriesName = ko.observable(null);
    self.seriesNameUpdate = ko.observable(null);
    self.seriesMaskingNameUpdate = ko.observable(null);

    self.seriesNameUpdate.subscribe(function () {
        var series = "";
        if (self.seriesNameUpdate() && self.seriesNameUpdate() != "") {
            series = self.seriesNameUpdate().trim().replace(/\s+/g, "").replace(/[^a-zA-Z0-9]+/g, '').toLowerCase();
            self.seriesMaskingMsg("");
            self.seriesMaskingNameUpdate(series);
        }
        Materialize.updateTextFields();
    });
    self.deleteSeriesId = ko.observable(null);

    self.validateSubmit = function () {
        try {
            var isValid = true;
            self.seriesMsg("");
            self.seriesMaskingMsg("");
            if (!self.selectedMakeId() || self.selectedMakeId() == "") {
                isValid = false;
                Materialize.toast("Please select Make", 3000);
            }
            if (self.seriesName() == "") {
                isValid = false;
                Materialize.toast("Invalid make name", 3000)
                self.seriesMsg("Invalid make name");
            }
            if (self.seriesMaskingName() == "") {
                isValid = false;
                Materialize.toast("Invalid make masking name", 3000)
                self.seriesMaskingMsg("Invalid make masking name");
            }
            if (isValid) {
                $.ajax({
                    type: "POST",
                    url: "/api/bikeseries/add/?makeid=" + self.selectedMakeId() + "&seriesname=" + self.seriesName() + "&seriesmaskingname=" + self.seriesMaskingName() + "&updatedby=" + $('#userId').val(),
                    success: function (response) {
                        $(
                            "<tr>"
                                + "<td>" + response.seriesId + "</td>"
                                + "<td class='teal lighten-4'>" + response.seriesName + "</td>"
                                + "<td>" + response.seriesMaskingName + "</td>"
                                + "<td>" + $("#selectMake option[value=" + response.make.makeId + "]").text() + "</td>"
                                + "<td><a href='#modal-make-update' class='tooltipped' href='javascript:void(0)' data-delay='100' data-tooltip='Edit Series' rel='nofollow' data-bind='event: {click: function(d, e) { editSeriesClick(e) }}'><i class='material-icons icon-blue'>mode_edit</i></a></td>"
                                + "<td><a class='tooltipped' href='javascript:void(0)' data-delay='100' data-tooltip='Delete Series' rel='nofollow' data-target='delete-confirm-modal' data-bind='event:{click: function(d, e){ deleteEvent(e)}}'><i class='material-icons icon-red'>delete</i></a></td>"
                                + "<td>" + response.createdOn + "</td>"
                                + "<td>" + response.updatedOn + "</td>"
                                + "<td>" + response.updatedBy + "</td>"
                             + "</tr>"
                            ).insertBefore('#bikeSeriesList > tbody > tr:first');
                        ko.cleanNode($("#manageBikeSeries")[0]);
                        ko.applyBindings(bikeSeriesVM, $("#manageBikeSeries")[0]);
                        Materialize.toast("New bike series added", 3000);
                        self.seriesName("");
                        
                        $("#txtSeriesName").removeClass("valid");
                        $("#txtSeriesMaskingName").removeClass("valid");
                        $("#selectMake").val("");
                        $('#selectMake').material_select();
                    }, error: function (respose) {
                        Materialize.toast(respose.responseJSON.Message, 3000);
                    }
                });
            }
            return isValid;
        } catch (e) {
            console.warn(e.message);
        }
    }


    var rowToEdit, selectedSeriesMaskingName;

    self.editSeriesClick = function (event) {
        var seriesRow = rowToEdit = event.currentTarget.parentElement.parentElement;
        self.selectedSeriesName(seriesRow.children[1].innerText);
        selectedSeriesMaskingName = seriesRow.children[2].innerText;

        self.selectedSeriesId(seriesRow.children[0].innerText); //seriesId
        self.seriesNameUpdate(self.selectedSeriesName());
        self.seriesMaskingNameUpdate(selectedSeriesMaskingName);

        Materialize.updateTextFields();

    }

    self.validateUpdate = function () {
        try {
            var isValid = true;

            if (self.seriesNameUpdate() == "") {
                isValid = false;
                Materialize.toast("Invalid series name", 3000);
            }
            if (self.seriesMaskingNameUpdate() == "") {
                isValid = false;
                Materialize.toast("Invalid series masking name", 3000)
            }
            if (isValid) {
                $.ajax({
                    type: "POST",
                    url: "/api/bikeseries/edit/?seriesId=" + self.selectedSeriesId() + "&seriesname=" + self.seriesNameUpdate() + "&seriesmaskingname=" + self.seriesMaskingNameUpdate() + "&updatedby=" + $('#userId').val(),
                    success: function (response) {
                        if (response != null) {
                            rowToEdit.children[1].innerText = self.seriesNameUpdate();
                            rowToEdit.children[2].innerText = self.seriesMaskingNameUpdate();
                            Materialize.toast("Bike series has been updated successfully.", 3000);
                        }
                    },
                    error: function (respose) {
                        Materialize.toast("Something went wrong, could't update.", 3000);
                    }
                });
            }
            return isValid;
        } catch (e) {
            console.warn(e.message);
        }
    }
    
    self.deleteEvent = function (event) {
        var rowToDelete = event.currentTarget.parentElement.parentElement;
        self.selectedSeriesName(rowToDelete.children[1].innerText);
        self.deleteSeriesId(rowToDelete);
    }
    
    self.deleteSeries = function (event) {
        try {
            
            var selectedSeriesId = self.deleteSeriesId().children[0].innerText; //seriesId
            $.ajax({
                type: "POST",
                url: "/api/bikeseries/delete/?bikeSeriesId=" + selectedSeriesId,
                success: function (response) {
                    if (response != null) {
                        self.deleteSeriesId().remove();
                        Materialize.toast("Bike series has been deleted successfully.", 3000);
                    }
                },
                error: function (respose) {
                    Materialize.toast("Something went wrong, could't delete.", 3000);
                }
            });
        }
        catch (e) {
            console.warn(e.message);
        }
    }
}


var bikeSeriesVM = new bindBikeSeriesData;
$(document).ready(function () {
    try{
        ko.applyBindings(bikeSeriesVM, $("#manageBikeSeries")[0]);
    } catch (e) {
        console.warn(e.message);
    }
});