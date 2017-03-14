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
        if (self.makeName() && self.makeName() != "")
        {
            make = self.makeName().trim().replace(/\s+/g, "").replace(/[^a-zA-Z0-9]+/g, '').toLowerCase();
            self.makeMaskingMsg("");
        }

        return make;
    });

    self.validateMakeSubmit = function()
    {        
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
        objMake = self.selectedMake();        
        if (objMake != null)
        {
            var selMakeId = objMake.makeId;

            $.ajax({
                type: "GET",
                url: "/api/makes/" + objMake.makeId + "/synopsis/",
                contentType: "application/json",
                dataType: "json",
                async: false,
                success: function (response) {
                    if (response != null)
                    {
                        alert(response);
                        self.makeSynopsis(response);
                    }                    
                }
            });
        }
    }

    self.updateSynopsis = function () {


    }
}
var vmAddSynopsis = new addSynopsis;

/* make table view model */
var makeViewModel = function () {
    var self = this;

    self.selectedMake = ko.observable(null);

    self.synopsis = ko.observable(vmAddSynopsis);
    self.addMake = ko.observable(vmaddMake);

    self.setmakedata = function(e)
    {
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
        if(self.selectedMake)
        {
            self.synopsis().selectedMake(self.selectedMake());
        }
    });
}

ko.applyBindings(makeViewModel, $("tblMakeData")[0]);