var vmSponsoredComparisonRules, pgSection = $("#addSponsoredComparisonRules"), targetModelEle = $("#txtTargetModel"),
    sponsoredModelEle = $("#txtSponsoredModel"), modalVersionMapping = $('#modalVersionMapping');


var addSponsoredComparisonRules = function () {
    var self = this;
    self.comparisonId = ko.observable();
    self.sponsoredModel = ko.observable();
    self.targetModel = ko.observable();
    self.targetModelVersions = ko.observableArray([]);
    self.sponsoredModelVersions = ko.observableArray([]);

    self.setSponsoredModel = function (d, e) {
        var objModel = sponsoredModelEle.getSelectedItemData();
        self.sponsoredModel(objModel);
    };

    self.setTargetModel = function (d, e) {
        var objModel = targetModelEle.getSelectedItemData();
        self.targetModel(objModel);
    };

    self.init = function () {

        self.comparisonId(pgSection.data("comparisonid"));

        targetModelEle.bw_easyAutocomplete({
            source: 1,
            hosturlForAPI: 'http://localhost:9011',
            inputField: targetModelEle,
            click: self.setTargetModel
        });

        sponsoredModelEle.bw_easyAutocomplete({
            source: 1,
            hosturlForAPI: 'http://localhost:9011',
            inputField: sponsoredModelEle,
            click: self.setSponsoredModel
        });

        $('.modal').modal();
    }();

    self.getModelVersions = function () {
        var url = "/api/compare/sponsored/" + self.comparisonId() + "/target/" + self.targetModel().payload.modelId + "/sponsor/" + self.sponsoredModel().payload.modelId + "/"
        $.getJSON(url, function (response) {
            if (response) {
                if (response.SponsoredVersionsMapping) {
                    self.targetModelVersions(response.SponsoredVersionsMapping);
                    self.sponsoredModelVersions(response.SponsoredModelVersion);
                    modalVersionMapping.find('select').material_select();
                }
            }
            debugger;
        })
        .fail(function () {
            Materialize.toast("Failed to load version mapping data", 3000);
        });
    };

    self.showVersionMapping = function (d,e) {

        if (self.validateModels()) {
            if ($(e.target).prop("checked")) {
                self.getModelVersions();
                modalVersionMapping.modal("open");
            }
            return true;
        }
        return false;
    };

    self.addSponsoredCamparisonRules = function () {
        return false;
    };

    self.validateModels = function () {
        var isValid = false;
        if (self.sponsoredModel() && self.sponsoredModel().payload.modelId != "0") {
            isValid = true;
        }
        else {
            self.sponsoredModel({ payload: { modelId: "0" } });
        }

        if (self.targetModel() && self.targetModel().payload.modelId != "0") {
            isValid = true;
        }
        else self.targetModel({ payload: { modelId: "0" } });

        return isValid;

    };


}

$(function () {
    vmSponsoredComparisonRules = new addSponsoredComparisonRules();
    ko.applyBindings(vmSponsoredComparisonRules, pgSection[0]);
});