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
            series = self.seriesName().trim().replace(/\s+/g, "").replace(/[^a-zA-Z0-9]+/g, '').toLowerCase();
            self.seriesMaskingMsg("");
            $('#txtSeriesMaskingLabel').addClass('active');
        }
        self.seriesMaskingName(series);
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
                $.ajax({
                    type: "POST",
                    url: "/api/bikeseries/add/?makeid=" + self.selectedMakeId() + "&seriesname=" + self.seriesName() + "&seriesmaskingname=" + self.seriesMaskingName() + "&updatedby=" + $('#userId').val(),
                    success: function (response) {
                        $(
                            "<tr>"
                                + "<td>"+response.seriesId+"</td>"
                                + "<td class='teal lighten-4'>"+response.seriesName+"</td>"
                                + "<td>"+response.seriesMaskingName+"</td>"
                                + "<td>"+$("#selectMake option[value="+response.make.makeId+"]").text()+"</td>"
                                + "<td><a href='#modal-make-update' class='tooltipped' href='javascript:void(0)' data-delay='100' data-tooltip='Edit Series' rel='nofollow' data-bind='event: {click: function(d, e) { editSeries(e) }}'><i class='material-icons icon-blue'>mode_edit</i></a></td>"
                                + "<td><a class='tooltipped' href='javascript:void(0)' data-delay='100' data-tooltip='Delete Series' rel='nofollow' data-bind='event: {click: function(d, e) { deleteSeries(e) }}'><i class='material-icons icon-red'>delete</i></a></td>"
                                + "<td>"+response.createdOn+"</td>"
                                + "<td>"+response.updatedOn+"</td>"
                                + "<td>"+response.updatedBy+"</td>"
                             + "</tr>"
                            ).insertBefore('#bikeSeriesList > tbody > tr:first');
                        Materialize.toast("New bike series added", 3000);
                        self.seriesName("");
                        $("#texSeriesLabel").removeClass("active");
                        $("#txtSeriesMaskingLabel").removeClass("active");
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
}
var bikeSeriesVM = new bindBikeSeriesData;
$(document).ready(function () {
    try{
        ko.applyBindings(bikeSeriesVM);
    } catch (e) {
        console.warn(e.message);
    }
});