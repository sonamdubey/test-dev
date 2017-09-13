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
}

$(document).ready(function () {
    try{
        ko.applyBindings(new bindBikeSeriesData);
    } catch (e) {
        console.warn(e.message);
    }
});