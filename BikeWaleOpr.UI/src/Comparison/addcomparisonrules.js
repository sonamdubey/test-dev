var vmSponsoredComparisonRules,pgSection=$("#addSponsoredComparisonRules"), targetModelEle = $("#txtTargetModel"),
    sponsoredModelEle = $("#txtSponsoredModel"), modalVersionMapping = $('#modalVersionMapping');


var addSponsoredComparisonRules = function()
{
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
            inputField : targetModelEle,
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

    };

    self.showVersionMapping = function () {
        if (self.validateModels())
        {
            self.getModelVersions();
            modalVersionMapping.modal("open");
           return true;
        }
         return false;
    };

    self.addSponsoredCamparisonRules = function () {
        return false;
    };

    self.validateModels = function () {
        var isValid = false;
        if(self.sponsoredModel() && self.sponsoredModel().payload.modelId != "0")
        {
            isValid = true;
        }
        else
        {
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