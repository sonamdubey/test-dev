$(document).ready(function () {
    if ($("#ddlMake").val() != 0) {
        var make = $("#ddlMake").val().split('_')[0];
        var request = "USERREVIEW";
        LoadModels(make, request);
    }
    else {
        $("#ddlModel").val(0).attr("disabled", true);
    }

    $("#ddlMake").change(function () {

        $("#ddlModel").val(0);
        $("#ddlModel").selectmenu("refresh", true);

        var makeId = $(this).val().split('_')[0];
        var requestType = "USERREVIEW";

        if (makeId != 0) {

            $("#imgLoaderMake").show();
            LoadModels(makeId, requestType);
        }
        else {
            $("#ddlModel").val(0).attr("disabled", true);
        }
    });

    $("#btnSubmit").click(function () {
        if (isValid())
            location.href = '/m/' + $("#ddlMake").val().split('_')[1] + '-' + "bikes/" + $("#ddlModel").val().split('_')[1] + "/user-reviews/";
    });

    function isValid() {
        var isError = true;
        var errormsg = "";

        if ($("#ddlMake").val() == "0") {
            errormsg = "Please select make";
            if ($("#ddlModel").val() == "0")
                errormsg = "Please select make & model ";
            isError = false;
        }
        else if ($("#ddlModel").val() == "0") {
            errormsg = "Please select model ";
            isError = false;
        }

        if (!isError) {
            $("#spnError").html(errormsg);
            $("#popupDialog").popup("open");
        }

        return isError;
    }

    function LoadModels(makeId, requestType) {
        $.ajax({
            type: "POST",
            url: "/ajaxpro/Bikewale.Ajax.AjaxCommon,Bikewale.ashx",
            data: '{"requestType":"' + requestType + '", "makeId":"' + makeId + '"}',
            beforeSend: function (xhr) { xhr.setRequestHeader("X-AjaxPro-Method", "GetModelsWithMappingName"); },
            success: function (response) {
                $("#imgLoaderMake").hide();
                var responseJSON = eval('(' + response + ')');
                var resObj = eval('(' + responseJSON.value + ')');
                var dependentCmbs = new Array;
                bindDropDownList(resObj, $("#ddlModel"), "", dependentCmbs, "--Select Model--");
            }
        });
        $("#ddlModel").val(0).attr("disabled", false);
    }

    var effect = 'slide',
    optionRight = { direction: 'right' },
    duration = 500;

    var bikePopup = {

        container: $('#select-bike-cover-popup'),
        loader: $('.cover-popup-loader-body'),
        makeBody: $('#select-make-wrapper'),
        modelBody: $('#select-model-wrapper'),

        open: function () {
            bikePopup.container.show(effect, optionRight, duration, function () {
                bikePopup.container.addClass('extra-padding');
                $('html, head').addClass('lock-browser-scroll');
            });
        },

        close: function () {
            bikePopup.container.hide(effect, optionRight, duration, function () {
                bikePopup.container.removeClass('extra-padding');
                $('html, head').removeClass('lock-browser-scroll');
            });
        },
        scrollToHead: function () {
            bikePopup.container.animate({ scrollTop: 0 });
        }
    };

    var bikeSelection = function () {
        var self = this;
        self.make = ko.observable();
        self.model = ko.observable();
        self.IsLoading = ko.observable(false);
        self.LoadingText = ko.observable('Loading...');
        self.currentStep = ko.observable(0);
        self.lastStep = ko.observable(3);
        self.modelArray = ko.observableArray();
        self.makeChanged = function (data, event) {
            self.currentStep(self.currentStep() + 1);

            self.LoadingText("Loading bike models...");
            var element = $(event.currentTarget).find("span"), _modelsCache;

            self.make({
                id: element.data("id"),
                name: element.text().trim()
            })

            bikePopup.scrollToHead();

            try {
                if (self.make() && self.make().id > 0) {
                    self.modelArray(null);
                    self.IsLoading(true);
                    var _cmodelsKey = "models_" + self.make().id;
                    _modelsCache = bwcache.get(_cmodelsKey, true);
                    if (!_modelsCache) {
                        $.getJSON("/api/modellist/?requestType=2&makeId=" + self.make().id)
                        .done(function (res) {
                            self.modelArray(res.modelList);
                            bwcache.set(_cmodelsKey, res, true);
                        })
                        .fail(function () {
                            self.make(null);
                            self.modelArray(null);
                        })
                        .always(function () {
                            self.IsLoading(false);
                        });
                    }
                    else {
                        self.modelArray(_modelsCache.modelList);
                        self.IsLoading(false);
                    }
                }
            } catch (e) {
                console.warn(e);
            }

        };

        self.modelChanged = function (data, event) {
            self.model(data);
            if (self.model() && self.model().modelId > 0) {
                self.currentStep(self.currentStep() + 1);
                self.LoadingText("Redirecting to bike rating...");
                self.IsLoading(true);
                window.location = "/m/rate-your-bike/" + self.model().modelId + "/?q=" + returnUrl;
            }
        }

        self.modelBackBtn = function () {
            self.currentStep(self.currentStep() - 1);
        };

        self.closeBikePopup = function () {
            self.currentStep(0);
            history.back();
        };
    };

    var rateBikeVM = function () {
        var self = this;
        self.bikeSelection = ko.observable(new bikeSelection());
        self.bikePopup = bikePopup;
        self.bike = ko.observable();

        self.openBikeSelection = function (bike) {

            try {
                self.bikePopup.open();
                window.history.pushState('selectBike', '', '');
                self.bikeSelection().currentStep(1);
            } catch (e) {
                console.warn(e)
            }
        };
    };

    var vmRateBikeVM = new rateBikeVM();

    ko.applyBindings(vmRateBikeVM, document.getElementById("rate-bike-landing"));

    var hashParameter = window.location.hash;
    if (hashParameter == "#selectBike") {
        vmRateBikeVM.openBikeSelection();
    }


    $(window).on('popstate', function (event) {
        if ($('#select-bike-cover-popup').is(':visible')) {
            bikePopup.close();
        }
    });

});