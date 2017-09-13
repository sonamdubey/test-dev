function bindBikeSeriesData() {
    var self = this;
    self.seriesName = ko.observable("");
    self.seriesMsg = ko.observable("");
    self.seriesMaskingMsg = ko.observable("");
    self.selectedMakeId = ko.observable("");
    self.seriesMaskingName = ko.computed(function () {
        var series = "";
        if (self.seriesName() && self.seriesName() != "") {
            series = self.seriesName().trim().replace(/\s+/g, "").replace(/[^a-zA-Z0-9]+/g, '').toLowerCase();
            self.seriesMaskingMsg("");
        }
        return series;
    });

    self.validateSubmit = function () {
        try{
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
                console.log(self.selectedMakeId(), self.seriesName());
                $.ajax({
                    type: "POST",
                    url: "/api/bikeseries/add/?makeid=" + self.selectedMakeId() + "&makename=" + $("#selectMake option[value='" + self.selectedMakeId() + "']").text() + "&seriesname=" + self.seriesName() + "&seriesmaskingname=" + self.seriesMaskingName() + "&updatedby=" + $('#userId').val(),
                    success: function (response) {
                        console.log(response);
                    }, error: function (respose) {
                        console.log(respose);
                    }
                });
            }
            return isValid;
        } catch (e) {
            console.warn(e.message);
        }
    }


    self.selectedSeriesName = ko.observable(null);
    self.selectedSeriesMaskingName = ko.observable(null);
    
    self.selectedSeriesId = ko.observable(null);

    self.seriesNameUpdate = ko.observable(null);
    


    self.seriesMaskingNameUpdate = ko.computed(function () {
        var series = "";
        if (self.seriesNameUpdate() && self.seriesNameUpdate() != "") {
            series = self.seriesNameUpdate().trim().replace(/\s+/g, "").replace(/[^a-zA-Z0-9]+/g, '').toLowerCase();
            self.seriesMaskingMsg("");
        }
        return series;
    });

    var rowToEdit;
    self.editSeriesClick = function (event) {

        var seriesRow = rowToEdit = event.currentTarget.parentElement.parentElement;
        
        self.selectedSeriesName(seriesRow.children[1].innerText);
        self.selectedSeriesMaskingName(seriesRow.children[2].innerText);
        self.selectedSeriesId(seriesRow.attributes[0].value);

        self.seriesNameUpdate(self.selectedSeriesName());
        //self.seriesMaskingNameUpdate(self.selectedSeriesMaskingName());

        Materialize.updateTextFields();

    }

    
    
    self.validateUpdate = function () {
        try {
            var isValid = true;
            self.seriesMsg("");
            self.seriesMaskingMsg("");
            
            if (self.seriesNameUpdate() == "") {
                isValid = false;
                Materialize.toast("Invalid series name", 3000)
                self.seriesMsg("Invalid series name");
            }
            if (self.seriesMaskingNameUpdate() == "") {
                isValid = false;
                Materialize.toast("Invalid series masking name", 3000)
                self.seriesMaskingMsg("Invalid series masking name");
            }
            if (isValid) {
                console.log(self.selectedSeriesId(), self.seriesNameUpdate());
                $.ajax({
                    type: "POST",
                    url: "/api/bikeseries/edit/?seriesId=" + self.selectedSeriesId() + "&seriesname=" + self.seriesNameUpdate() + "&seriesmaskingname=" + $("#txtSeriesMaskingNameUpdate").val() + "&updatedby=" + $('#userId').val() + "/",
                    success: function (response) {
                        console.log(response);
                        if (response) {
                            rowToEdit.children[1].innerText = self.seriesNameUpdate();
                            rowToEdit.children[2].innerText = $("#txtSeriesMaskingNameUpdate").val();
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


    var rowToDelete;
    self.deleteSeries = function (event) {
        try{
            rowToDelete = event.currentTarget.parentElement.parentElement;
            var selectedSeriesId = rowToDelete.attributes[0].value;
            $.ajax({
                type: "POST",
                url: "/api/bikeseries/delete/?bikeSeriesId=" + selectedSeriesId,
                success: function (response) {
                    console.log(response);
                    if (response != null) {
                        rowToDelete.remove();
                    }
                },
                error: function (respose) {
                    Materialize.toast("Something went wrong, could't update.", 3000);
                }
            });
        }
        catch (e) {
            console.warn(e.message);
        }
    }

}
var bk = new bindBikeSeriesData;
$(document).ready(function () {
    try{
        ko.applyBindings(bk);
    } catch (e) {
        console.warn(e.message);
    }
});