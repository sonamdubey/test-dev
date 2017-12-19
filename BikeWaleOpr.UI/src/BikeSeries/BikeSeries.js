function bindBikeSeriesData() {
    var self = this;
    self.seriesName = ko.observable("");
    self.seriesMsg = ko.observable("");
    self.seriesMaskingMsg = ko.observable("");
    self.selectedMakeId = ko.observable("");
    self.selectedMakeUpdateId = ko.observable("");
    self.seriesMaskingName = ko.observable("");
    self.seriesName.subscribe(function () {
        var series = "";
        if (self.seriesName() && self.seriesName() != "") {
            series = self.seriesName().trim().replace(/\s+/g, "-").replace(/[^a-zA-Z0-9\-]+/g, '').toLowerCase();
            self.seriesMaskingMsg("");
        }
        self.seriesMaskingName(series);
        Materialize.updateTextFields();
    });
    
    self.selectedSeriesId = ko.observable(null);
    self.selectedSeriesName = ko.observable(null);
    self.seriesNameUpdate = ko.observable(null);
    self.seriesMaskingNameUpdate = ko.observable(null);
    self.isSeriesUrl = ko.observable(false);
    self.selectedBodyStyleName = ko.observable("");
    self.selectedBodyStyleId = ko.observable(null); //optional
    self.isBodyStyleShown = ko.observable(false);
    self.selectedBodyStyleUpdateId = ko.observable(null);
    self.seriesEditMsg = ko.observable("");
    self.seriesMaskingEditMsg = ko.observable("");
    self.seriesNameUpdate.subscribe(function () {
        var series = "";
        if (self.seriesNameUpdate() && self.seriesNameUpdate() != "") {
            series = self.seriesNameUpdate().trim().replace(/\s+/g, "-").replace(/[^a-zA-Z0-9\-]+/g, '').toLowerCase();
            self.seriesMaskingMsg("");
            self.seriesMaskingNameUpdate(series);
        }
        Materialize.updateTextFields();
    });
    self.deleteSeriesId = ko.observable(null);
    self.isSeriesEditUrl = ko.observable(false);
    self.seriesSynopsis = ko.observable(null);
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
                Materialize.toast("Invalid series name", 3000)
                self.seriesMsg("Invalid series name");
            }
            if (self.seriesMaskingName() == "") {
                isValid = false;
                Materialize.toast("Invalid series masking name", 3000)
                self.seriesMaskingMsg("Invalid series masking name");
            }
            if (self.isSeriesUrl()) {
                if (!self.selectedBodyStyleId() || self.selectedBodyStyleId()==0) {
                    isValid = false;
                    Materialize.toast("Please select Body Style", 3000);
                }
            }
            else {
                // No bodystyle if seriesUrl not selected
                self.selectedBodyStyleId("0");
            }
            if (isValid) {
                $.ajax({
                    type: "POST",
                    url: "/api/make/" + self.selectedMakeId() + "/series/add/?seriesname=" + self.seriesName() + "&seriesmaskingname=" + self.seriesMaskingName() + "&updatedby=" + $('#userId').val() + "&isseriespageurl=" + self.isSeriesUrl() + "&bodystyleid=" + self.selectedBodyStyleId(),
                    success: function (response) {
                        if (response.seriesId > 0) {
                            var bodystyle = (response.bodystyle.bodyStyleId == 0 || response.bodystyle.bodyStyleId == null) ? "N/A" : $("#selectBodyStyle option[value=" + response.bodystyle.bodyStyleId + "]").text();
                            $(
                                                    "<tr data-seriesid='" + response.seriesId + "'>"
                                                        + "<td>" + response.seriesId + "</td>"
                                                        + "<td class='teal lighten-4'>" + response.seriesName + "</td>"
                                                        + "<td>" + response.seriesMaskingName + "</td>"
                                                        + "<td><i class='material-icons " + (response.isSeriesPageUrl ? 'icon-green' : 'icon-red') + "'>" + (response.isSeriesPageUrl ? 'done' : 'close') + "</i></td>"
                                                        + "<td>" + bodystyle + "</td>"
                                                        + "<td>" + $("#selectMake option[value=" + response.make.makeId + "]").text() + "</td>"
                                                        + "<td><a href='#!' data-target='modal-series-synopsis' data-bind='event : {click : function(d,e) { getSeriesSynopsis(e) }}'><i class='material-icons icon-green'>add_circle</i></a></td>"
                                                        + "<td><a href='#series-edit-update' class='tooltipped' href='javascript:void(0)' data-delay='100' data-makeid=" + response.make.makeId +" data-bodystyleid=" +response.bodystyle.bodyStyleId + " data-tooltip='Edit Series' rel='nofollow' data-bind='event: {click: function(d, e) { editSeriesClick(e) }}'><i class='material-icons icon-blue'>mode_edit</i></a></td>"
                                                        + "<td><a class='tooltipped' href='javascript:void(0)' data-delay='100' data-tooltip='Delete Series' rel='nofollow' data-target='delete-confirm-modal' data-bind='event:{click: function(d, e){ deleteEvent(e)}}'><i class='material-icons icon-red'>delete</i></a></td>"
                                                        + "<td>" + response.createdOn + "</td>"
                                                        + "<td>" + response.updatedOn + "</td>"
                                                        + "<td>" + response.updatedBy + "</td>"
                                                     + "</tr>"
                                                    ).insertBefore('#bikeSeriesList > tbody > tr:first');
                            var row = $("tr[data-seriesid=" + response.seriesId + "]")[0];
                            ko.applyBindings(bikeSeriesVM, row);
                            Materialize.toast("New bike series added", 3000);
                            self.seriesName("");

                            $("#txtSeriesName").removeClass("valid");
                            $("#txtSeriesMaskingName").removeClass("valid");
                            $("#selectMake").val("");
                            $('#selectMake').material_select();
                        } else {
                            Materialize.toast(response.message,3000);
                        }
                    }, error: function (response) {
                        Materialize.toast(response.responseJSON.Message, 3000);
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
        self.isSeriesEditUrl(rowToEdit.children[3].children[0].innerText == "done" ? true : false);
        self.selectedSeriesName(seriesRow.children[1].innerText);
        selectedSeriesMaskingName = seriesRow.children[2].innerText;
        self.selectedSeriesId(seriesRow.children[0].innerText); //seriesId
        self.seriesNameUpdate(self.selectedSeriesName());
        self.seriesMaskingNameUpdate(selectedSeriesMaskingName);
        self.selectedMakeUpdateId($(event.currentTarget).data("makeid"));
        self.selectedBodyStyleUpdateId($(event.currentTarget).data("bodystyleid"));
        $('#selectBodyStyleUpdate').material_select();
        Materialize.updateTextFields();
        $('.modal').css({ 'overflow-y': 'visible' });
        setTimeout(function () { $('.modal').css({ 'overflow-y': 'visible' }); }, 300);

    }

    self.validateUpdate = function () {
        try {
            var isValid = true;
            self.seriesEditMsg("");
            self.seriesMaskingEditMsg("");
            if (self.seriesNameUpdate() == "") {
                isValid = false;
                Materialize.toast("Invalid series name", 3000);
                self.seriesEditMsg("Invalid series name");
            }
            if (self.seriesMaskingNameUpdate() == "") {
                isValid = false;
                Materialize.toast("Invalid series masking name", 3000)
                self.seriesMaskingEditMsg("Invalid series masking name");
            }
            if (self.isSeriesEditUrl()) {
                if (!self.selectedBodyStyleUpdateId() || self.selectedBodyStyleUpdateId() == 0) {
                    isValid = false;
                    Materialize.toast("Please select Body Style", 3000)
                }
            }
            else {
                // No bodystyle if seriesUrl not selected
                self.selectedBodyStyleUpdateId("0");
            }
            if (isValid) {
                $.ajax({
                    type: "POST",
                    url: "/api/series/" + self.selectedSeriesId() + "/edit/?seriesname=" + self.seriesNameUpdate() + "&seriesmaskingname=" + self.seriesMaskingNameUpdate() + "&updatedby=" + $('#userId').val() + "&isseriespageurl=" + self.isSeriesEditUrl() + "&makeId=" + self.selectedMakeUpdateId() + "&bodystyleid=" + self.selectedBodyStyleUpdateId(),
                    success: function (response) {

                        if (response != null) {
                            $('#series-edit-update').modal('close');
                            rowToEdit.children[1].innerText = self.seriesNameUpdate();
                            rowToEdit.children[2].innerText = self.seriesMaskingNameUpdate();
                            if (self.isSeriesEditUrl()) {
                                rowToEdit.children[3].children[0].classList.remove("icon-red");
                                rowToEdit.children[3].children[0].classList.add("icon-green");
                                rowToEdit.children[3].children[0].innerText = "done";
                            }
                            else {
                                rowToEdit.children[3].children[0].classList.remove("icon-green");
                                rowToEdit.children[3].children[0].classList.add("icon-red");
                                rowToEdit.children[3].children[0].innerText = "close";
                            }
                            if (self.selectedBodyStyleUpdateId() != 0) {
                                rowToEdit.children[4].innerText = $("#selectBodyStyleUpdate option[value=" + self.selectedBodyStyleUpdateId() + "]").text();
                            }
                            else {
                                rowToEdit.children[4].innerText = "N/A";
                            }
                            $(rowToEdit.children[7].children[0]).data("bodystyleid", self.selectedBodyStyleUpdateId());
                            Materialize.toast("Bike series has been updated successfully.", 3000);
                            $("#txtSeriesNameUpdate").removeClass("valid");
                            $("#txtSeriesMaskingNameUpdate").removeClass("valid");
                        }

                    },
                    error: function (response) {
                        if (response && response.responseJSON && response.responseJSON.Message)
                            Materialize.toast(response.responseJSON.Message, 3000);
                        else
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
                url: "/api/make/series/" + selectedSeriesId + "/delete/?deletedBy=" + $('#userId').val(),
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

    self.getSeriesSynopsis = function (event) {
        try{
            var targetedRow = $(event.target).closest("tr");
            self.selectedSeriesId($(targetedRow).data("seriesid"));
            self.selectedSeriesName($(targetedRow).find("td:eq(1)").text());
            if (self.selectedSeriesId()) {
                $.ajax({
                    type: "GET",
                    url: "/api/series/" + self.selectedSeriesId() + "/synopsis/",
                    contentType: "application/json",
                    dataType: "json",
                    success: function (response) {
                        if (response != null) {
                            self.seriesSynopsis(response.bikeDescription);
                            Materialize.updateTextFields();
                        }
                    },
                    complete: function (xhr) {
                        if (xhr.status != 200) {
                            self.makeSynopsis("");
                        }
                    }
                });
            }
        }catch(e){
            console.warn(e.message);
        }

    }

    self.updateSeriesSynopsis = function () {
        try{
            if (self.selectedSeriesId()) {
                var synopsisData = {
                    "bikeDescription": self.seriesSynopsis()
                }
                $.ajax({
                    type: "POST",
                    dataType: 'json',
                    url: "/api/series/" + self.selectedSeriesId() + "/synopsis/",
                    contentType: "application/json",
                    data: ko.toJSON(synopsisData),
                    complete: function (xhr) {
                        if (xhr.status == 200 && xhr.responseJSON) {
                            Materialize.toast(self.selectedSeriesName() + " synopsis updated successfully", 5000);
                        }
                        else if (xhr.status == 400) {
                            Materialize.toast("Please enter valid data", 5000);
                        } else {
                            Materialize.toast("Something went wrong while updating synopsis. Please try again.", 5000);
                        }
                    }
                });
            } else {
                Materialize.toast("Please enter valid data", 5000);
            }
        }catch(e){
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