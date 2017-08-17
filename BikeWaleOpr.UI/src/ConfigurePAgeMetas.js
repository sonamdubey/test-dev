var pageMeta;

var ConfigurePageMetas = function () {
    var self = this;
    self.selectedMakeId = ko.observable();
    self.selectedModel = ko.observable();
    self.bikeModels = ko.observableArray();    
    self.pageList = ko.observableArray();
    self.selectedPage = ko.observable();        

    self.validateData = function (d, e) {
        var isValid = true;

        if (!self.selectedMakeId()) {
            Materialize.toast("Please select Make", 3000);
            isValid = false;
        }

        if (!self.selectedPage()) {
            Materialize.toast("Please select Page", 3000);
            isValid = false;
        }

        if ((self.selectedPage() == '3' || self.selectedPage() == '4') && !self.selectedModel()) {
            Materialize.toast("Please select Model", 3000);
            isValid = false;
        }

        if (!isValid) {
            e.preventDefault();
        }
        
        $('#makeName').val($("#selectMake option:selected").text());
        $('#modelName').val($("#selectModel option:selected").text());
        $('#pageName').val($("#selectPage option:selected").text());
        $('#selectPage').prop('disabled', false);
        $('#selectModel').prop('disabled', false);
        $('#selectMake').prop('disabled', false);

        return isValid;
    };

    self.changeMake = function () {       
        var makeId = $('#selectMake').find(":selected").val();
        if (makeId) {
            self.selectedMakeId(parseInt(makeId));
        }       
        if (self.selectedMakeId() && self.selectedMakeId() > 0) {
            $.ajax({
                type: "GET",
                url: "/api/models/makeid/" + self.selectedMakeId() + "/requesttype/8/",
                contentType: "application/json",
                dataType: 'json',
                success: function (response) {                    
                    if (response && response.length > 0) {                      
                        self.bikeModels(response);

                        if ($('#selectModel') && $('#selectModel').attr('data-selectedModel') > 0)
                            vmConfigurePageMetas.selectedModel($('#selectModel').attr('data-selectedModel'));                       
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

    self.selectPlatform = function () {
        if ($("input[name='platformId']:checked").val() == "1")
            self.pageList(JSON.parse($('#dektopPagesList').val()));
        else
            self.pageList(JSON.parse($('#mobilePagesList').val()));

        $('select[name="pageId"]').material_select();
    };
    
    self.init = function () {
        if ($('#pageMetaId').val() > 0) {        
        if ($("input[name='platformId']:checked").val() == "1")
            self.pageList(JSON.parse($('#dektopPagesList').val()));
        else
            self.pageList(JSON.parse($('#mobilePagesList').val()));


        if ($('#selectPage') && $('#selectPage').attr('data-selectedPage') > 0)
            self.selectedPage($('#selectPage').attr('data-selectedPage'));       

        self.changeMake();
    }

    $('select[name="modelId"]').material_select();
    $('select[name="pageId"]').material_select();   
    };
}

var vmConfigurePageMetas = new ConfigurePageMetas;
$(document).ready(function () {

    if ($(".stepper")) {
        $('.stepper').activateStepper({ autoFocusInput: false });
    }

    pageMeta = $('#pageMeta');
    $('.stepper').nextStep();

    if (pageMeta) {
        ko.applyBindings(vmConfigurePageMetas, pageMeta[0]);
        vmConfigurePageMetas.init();
    }        

});
