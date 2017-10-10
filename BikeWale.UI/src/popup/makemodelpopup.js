var ddlMakesPopup, makeId, modelId, modelName;
var ddlModelsPopup;

var flag = true;
var vmMakeModelPopupModel;


var makeModelPopupModel = function () {
    var self = this;
    var modelCacheKeyPrefix = "userReviewsPopupModels-";
    self.selectedMake = ko.observable();
    self.models = ko.observableArray([]);
    self.selectedModel = ko.observable();

    self.showMakeModelError = function (errMsgParent, errMsg) {
        errMsgParent.find('.error-tooltip-siblings').show();
        errMsgParent.css({ 'border-color': 'red' });
        errMsgParent.find('.bw-blackbg-tooltip').text("Please select " + errMsg);
    };

    self.removeMakeModelError = function (errMsgParent) {
        errMsgParent.css({ 'border-color': '#ccc' });
        errMsgParent.find('.error-tooltip-siblings').hide();
        errMsgParent.find('.bw-blackbg-tooltip').text("");
    };

    self.checkCacheModels = function checkCacheCityAreas(cityId) {
        bKey = modelCacheKeyPrefix + cityId;
        if (bwcache.get(bKey)) return true;
        else return false;
    };

    self.onModelChange = function () {
        if (!isNaN(self.selectedModel()) && self.selectedModel() != "0") {
            self.removeMakeModelError($("#divModelsPopup.form-control-box"));
        }
    };

  
    self.onMakeChange = function () {
        if (!isNaN(self.selectedMake()) && self.selectedMake() != "0") {
            self.removeMakeModelError($("#divMakesPopup.form-control-box"));
            $("#divModelsPopup .divModelsSelectPopup").hide();
            $("#divModelsPopup .placeholder-loading-text").show();

            if (!self.checkCacheModels(self.selectedMake())) {
                $.ajax({
                    type: "GET",
                    url: "/api/modellist/?requestType=3&makeId=" + self.selectedMake(),
                    contentType: "application/json",
                    dataType: 'json',
                    success: function (responseData) {
                        bwcache.set(modelCacheKeyPrefix + self.selectedMake(), responseData.modelList, 15);
                        self.models(responseData.modelList);
                         self.selectedModel(undefined);
                        ddlModelsPopup.removeAttr('disabled');
                        ddlModelsPopup.trigger("chosen:updated");
                    },
                    complete: function (xhr) {
                        if (xhr.status == 404 || xhr.status == 204) {
                        }
                    }
                });
            }
            else {
                cachedModels = bwcache.get(modelCacheKeyPrefix + self.selectedMake());
                self.models(cachedModels);
                    self.selectedModel(undefined);
                
                ddlModelsPopup.removeAttr('disabled');
                ddlModelsPopup.trigger("chosen:updated");
            }
        }
        else {
            self.models(null);
            self.selectedModel(undefined);
            ddlModelsPopup.trigger("chosen:updated");
        }

        $("#divModelsPopup .placeholder-loading-text").hide();
        $("#divModelsPopup .divModelsSelectPopup").show();
    };

    self.onClickWriteReview = function () {
        var isValidMakeModel = false;
        if (Number(self.selectedMake()) <= 0 || isNaN(Number(self.selectedMake())))
            self.showMakeModelError($("#divMakesPopup.form-control-box"), "Make");
        else if (Number(self.selectedModel()) <= 0 || isNaN(Number(self.selectedModel())))
            self.showMakeModelError($("#divModelsPopup.form-control-box"), "Model");
        else
            isValidMakeModel = true;

        if (isValidMakeModel)
            window.location = "/rate-your-bike/" + self.selectedModel() + "/?q=" + $('#querystring').data('query');

    };

    self.prefillMakeModel = function () {
        ddlMakesPopup.val(makeId);
        self.selectedMake(makeId);
        ddlMakesPopup.trigger("chosen:updated");

        self.models([{ "modelId": modelId, "modelName": modelName }]);
        self.selectedModel(modelId);
        ddlModelsPopup.prop("disabled", false);
    };
};



docReady(function () {
    ddlMakesPopup = $('#ddlMakesPopup');
    ddlModelsPopup = $('#ddlModelsPopup');

    ddlModelsPopup.prop('disabled', true);
   
    // Make Model info
    makeId = $('#makeId').val(),
        modelId = $('#modelId').val(),
        modelName = $('#modelName').val();

    $('.error-tooltip-siblings').hide();
    $("#divModelsPopup .placeholder-loading-text").hide();

    ddlMakesPopup.trigger("chosen:updated").chosen({ "width": "100%" });
    ddlModelsPopup.trigger("chosen:updated").chosen({ "width": "100%" });

    vmMakeModelPopupModel = new makeModelPopupModel();
    ko.applyBindings(vmMakeModelPopupModel, $("#divUserReviewBikePopup")[0]);

    if (makeId > 0 && modelId > 0) {
        vmMakeModelPopupModel.prefillMakeModel();
    }

    if (modelId > 0)
    {
        $('#ddlModelsPopup_chosen').one("click", (function () {

            ddlModelsPopup.find("option:first").attr("value", "0");
            ddlMakesPopup.val(makeId).trigger("change");

        }));
    }
   
    
   

   
    $('#divUserReviewBikePopup .close-btn, .blackOut-window').mouseup(function () {
        popup.unlock();
        $('#divUserReviewBikePopup').fadeOut(100);
    });

   
    $('body').on('click', "#bannerTargetBtn", function (e) {
        $('#divUserReviewBikePopup').fadeIn(100);
        popup.lock();
        e.preventDefault();
        $("#errMsgPopUp").empty();

        ddlMakesPopup.trigger("chosen:updated");
        ddlModelsPopup.trigger("chosen:updated");

        ddlMakesPopup.siblings(".chosen-container").find("input[type=text]").prop("placeholder", ddlMakesPopup.attr("data-placeholder"));
    });
   
    
   
});
