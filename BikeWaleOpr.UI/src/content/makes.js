//$('textarea#txtSynopsis').characterCounter();
if (msg != "") { Materialize.toast(msg, 5000); }

/* add make js*/
var addMakeViewModel = function () {
    var self = this;
    self.makeName = ko.observable(""),
    self.makeMsg = ko.observable("");
    self.makeMaskingMsg = ko.observable("");
    self.makeMaskingName = ko.computed(function () {
        var make = "";
        if (self.makeName() && self.makeName() != "") {
            make = self.makeName().trim().replace(/\s+/g, "").replace(/[^a-zA-Z0-9]+/g, '').toLowerCase();
            self.makeMaskingMsg("");
        }

        return make;
    });

    self.validateMakeSubmit = function () {
        var isValid = true;
        self.makeMsg("");
        self.makeMaskingMsg("");

        if (self.makeName() == "") {
            isValid = false;
            self.makeMsg("Invalid make name");

        }

        if (self.makeMaskingName() == "") {
            isValid = false;
            self.makeMaskingMsg("Invalid make masking name");
        }

        return isValid;
    }
}
var vmaddMake = new addMakeViewModel;

/* update make js*/


/* manage synopsis*/
var addSynopsis = function () {
    var self = this;
    self.makeSynopsis = ko.observable("");
    self.selectedMake = ko.observable(null);

    self.getSynopsis = function () {
        if (self.selectedMake() != null && self.selectedMake().makeId > 0) {
            $.ajax({
                type: "GET",
                url: "/api/makes/" + self.selectedMake().makeId + "/synopsis/",
                contentType: "application/json",
                dataType: "json",
                success: function (response) {
                    if (response != null) {
                        self.makeSynopsis(response);
                    }
                },
                complete: function (xhr) {
                    if (xhr.status != 200) {
                        self.makeSynopsis("");
                    }
                }
            });
        }
    }

    self.updateSynopsis = function () {
        if (self.selectedMake() != null && self.selectedMake().makeId > 0 && self.makeSynopsis() != "") {
            $.ajax({
                type: "POST",
                dataType: 'json',
                url: "/api/makes/" + self.selectedMake().makeId + "/synopsis/",
                contentType: 'application/x-www-form-urlencoded; charset=utf-8',
                data: '=' + encodeURIComponent(self.makeSynopsis()),
                complete: function (xhr) {
                    if (xhr.status == 200) {
                        Materialize.toast(self.selectedMake().makeName + " synopsis updated successfully", 5000);
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
    }
}
var vmAddSynopsis = new addSynopsis;

/* make table view model */
var makeViewModel = function () {
    var self = this;

    self.selectedMake = ko.observable(null);

    self.synopsis = ko.observable(vmAddSynopsis);
    self.addMake = ko.observable(vmaddMake);

    self.setmakedata = function (e) {
        ele = $(e.target).closest("tr");

        var objMake = {
            "makeId": ele.attr("data-makeId"),
            "makeName": ele.attr("data-makeName"),
            "maskingName": ele.attr("data-maskingname"),
            "new": ele.attr("data-new"),
            "used": ele.attr("data-used"),
            "futuristic": ele.attr("data-futuristic")
        }

        self.selectedMake(objMake);
    }

    self.selectedMake.subscribe(function () {
        if (self.selectedMake) {
            self.synopsis().selectedMake(self.selectedMake());
        }
    });
}

ko.applyBindings(makeViewModel, $("tblMakeData")[0]);