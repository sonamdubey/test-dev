var vmSponsoredComparisonRules, pgSection = $("#addSponsoredComparisonRules"), targetModelEle = $("#txtTargetModel"),
    sponsoredModelEle = $("#txtSponsoredModel"), modalVersionMapping = $('#modalVersionMapping');

function formatPrice(x) {
    try { x = x.toString(); var lastThree = x.substring(x.length - 3); var otherNumbers = x.substring(0, x.length - 3); if (otherNumbers != '') lastThree = ',' + lastThree; var res = otherNumbers.replace(/\B(?=(\d{2})+(?!\d))/g, ",") + lastThree; return res; } catch (err) { }
}


var addSponsoredComparisonRules = function () {
    var self = this;
    self.comparisonId = ko.observable();
    self.sponsoredModel = ko.observable();
    self.targetModel = ko.observable();
    self.targetModelVersions = ko.observableArray([]);
    self.sponsoredModelVersions = ko.observableArray([]);
    self.impressionModel = ko.observable();
    self.isVersionMapping = ko.observable();
    self.targetSponsoredMapping = ko.observable();
    self.impressionUrls = ko.observable();

    self.setSponsoredModel = function (d, e) {
        var objModel = sponsoredModelEle.getSelectedItemData();
        self.sponsoredModel(objModel);
    };

    self.setTargetModel = function (d, e) {
        var objModel = targetModelEle.getSelectedItemData();
        self.targetModel(objModel);
    };

    self.showSuggestionList = function (response, searchText) {
        if(response && response.length > 0)
        {
            var data = $.grep(response, function (e) {
                return e.payload.modelId > 0 && e.payload.isNew != 'False' && e.payload.futuristic != 'True';
            });
        }
        return data;
    };

    self.init = function () {

        self.comparisonId(pgSection.data("comparisonid"));

        targetModelEle.bw_easyAutocomplete({
            source: 1,
            hosturlForAPI: 'http://localhost:9011',
            inputField: targetModelEle,
            click: self.setTargetModel,
            afterFetch: self.showSuggestionList
        });

        sponsoredModelEle.bw_easyAutocomplete({
            source: 1,
            hosturlForAPI: 'http://localhost:9011',
            inputField: sponsoredModelEle,
            click: self.setSponsoredModel,
            afterFetch:self.showSuggestionList
        });

        $('.modal').modal();
    }();

    self.getModelVersions = function () {
        var url = "/api/compare/sponsored/" + self.comparisonId() + "/target/" + self.targetModel().payload.modelId + "/sponsor/" + self.sponsoredModel().payload.modelId + "/"
        $.getJSON(url, function (response) {
            if (response) {
                if (response.sponsoredModelVersion) {
                    self.targetModelVersions(response.targetVersionsMapping);
                    self.sponsoredModelVersions(response.sponsoredModelVersion);
                    modalVersionMapping.find('select').material_select();
                }
            }
        })
        .fail(function () {
            Materialize.toast("Failed to load version mapping data", 3000);
        });
    };

    self.showVersionMapping = function (d,e) {

        if (self.validateModels()) {
            self.isVersionMapping($(e.target).prop("checked"))
            if (self.isVersionMapping()) {
                self.getModelVersions();
                modalVersionMapping.modal("open");
            }
            return true;
        }
        return false;
    };

    self.addSponsoredComparisonRules = function (d, e) {
        var ele = $(e.target);
        if(self.validateModels())
        {
            self.targetSponsoredMapping("");
            self.impressionUrls("");
            if(self.isVersionMapping())
            {
                var str = "",impressions="";
                modalVersionMapping.find("table tbody tr").each(function () {
                    var ele = $(this);
                    str += (ele.data("target-versionid") + ":" + ele.find("select").val() + ",");
                    impressions += ele.find("input[type=text].impression").val() + ",";
                    
                });
                self.impressionUrls(impressions);
                self.targetSponsoredMapping(str);
            }
            else {
                self.targetSponsoredMapping(self.targetModel().payload.modelId + ":" + self.sponsoredModel().payload.modelId);
                self.impressionUrls(self.impressionModel());
            }
            return true;
        }

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

    self.deleteSponsoredModelRules = function (d,e) {
        var ele = $(e.target);
        var modelId = ele.data("sponsored-modelid"), liWrapper = ele.closest("li");
        if (modelId > 0 && window.confirm("Do you want to delete this model rule?"))
        {
            var url = "/api/compare/sponsored/" + self.comparisonId() + "/model/" + modelId + "/rules/delete/";
            $.post(url, function (response) {
                if (response) {
                    liWrapper.remove();
                    Materialize.toast("Rule deleted", 3000);
                }
            }).fail(function () {
                Materialize.toast("Failed to delete rules", 3000);
            });
        }
    };

    self.deleteSponsoredVersionRules = function (d, e) {
        var ele = $(e.target);
        var versionId = ele.data("target-versionid"),chip = ele.closest("div.chip");
        if (versionId > 0 && window.confirm("Do you want to delete this model rule?")) {
            var url = "/api/compare/sponsored/" + self.comparisonId() + "/targetversion/" + versionId + "/rules/delete/";
            $.post(url, function (response) {
                if (response) {
                    chip.remove();
                    Materialize.toast("Rule deleted", 3000);
                }
            })
            .fail(function () {
                Materialize.toast("Failed to delete rules", 3000);
            });
        }
        return false;
    };


}

$(function () {
    vmSponsoredComparisonRules = new addSponsoredComparisonRules();
    ko.applyBindings(vmSponsoredComparisonRules, pgSection[0]);
});