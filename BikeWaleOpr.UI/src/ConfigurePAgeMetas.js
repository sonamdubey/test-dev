var pageMeta;
var ConfigurePageMetas = function () {
    var self = this;
    self.selectedMakeId = ko.observable();
    self.selectedModel = ko.observable();
    self.bikeModels = ko.observableArray();
    self.bindModelDropDown = ko.observable(false);
    self.pageList = ko.observableArray();
    self.selectedPage = ko.observable();

    self.changePageType = function (d, e) {        
        var pageType = $(e.target).val();
        if (parseInt(pageType) == "3" || parseInt(pageType) == "4")
        {
            self.bindModelDropDown(true);
        }
        else
        {
            self.bindModelDropDown(false);
        }
    };

    self.changeMake = function (d, e) {       
        var makeId = $(e.target).val();
        if (makeId) {
            self.selectedMakeId(parseInt(makeId));
        }

        if (self.selectedMakeId() && self.selectedMakeId() > 0 && self.bindModelDropDown()) {
            $.ajax({
                type: "GET",
                url: "/api/models/makeid/" + self.selectedMakeId() + "/requesttype/8/",
                contentType: "application/json",
                dataType: 'json',
                success: function (response) {
                    if (response && response.length > 0) {
                        self.bikeModels(response);
                    }
                    else {
                        self.bikeModels([]);
                    }

                },
                complete: function (xhr) {
                    if (xhr.status != 200) {
                        self.bikeModels([]);
                    }
                    $('select[name="modelId"]').material_select();
                }
            });
        }

    };

}

var vmConfigurePageMetas = new ConfigurePageMetas;
$(document).ready(function () {

    if ($(".stepper"))
    {
        $('.stepper').activateStepper({ autoFocusInput: false });
    }

    pageMeta = $('#pageMeta');

    if (pageMeta) {
        ko.applyBindings(vmConfigurePageMetas, pageMeta[0]);
    }
    $('select[name="modelId"]').material_select();
    $('select[name="page"]').material_select();
    $(document).on("change", "input[type=radio][name='rdoPlatform']", function () {
        if ($("input[name='rdoPlatform']:checked").val() == "1")
            vmConfigurePageMetas.pageList(JSON.parse( $('#dektopPagesList').val()));
        else
            vmConfigurePageMetas.pageList(JSON.parse( $('#mobilePagesList').val()));

        $('select[name="page"]').material_select();
    });
});


