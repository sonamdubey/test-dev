var pageMeta;

var ConfigurePageMetas = function () {
    var self = this;
    self.selectedMakeId = ko.observable();
    self.selectedModel = ko.observable();
    self.bikeModels = ko.observableArray();    
    self.pageList = ko.observableArray();
    self.selectedPage = ko.observable();        

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
    }

    $(document).on("change", "input[type=radio][name='platformId']", function () {
        if ($("input[name='platformId']:checked").val() == "1")
            vmConfigurePageMetas.pageList(JSON.parse($('#dektopPagesList').val()));
        else
            vmConfigurePageMetas.pageList(JSON.parse($('#mobilePagesList').val()));

        $('select[name="pageId"]').material_select();
    });


    if ($('#pageMetaId').val() > 0) {        
        if ($("input[name='platformId']:checked").val() == "1")
            vmConfigurePageMetas.pageList(JSON.parse($('#dektopPagesList').val()));
        else
            vmConfigurePageMetas.pageList(JSON.parse($('#mobilePagesList').val()));


        if ($('#selectPage') && $('#selectPage').attr('data-selectedPage') > 0)
            vmConfigurePageMetas.selectedPage($('#selectPage').attr('data-selectedPage'));       

        vmConfigurePageMetas.changeMake();
    }

    $('select[name="modelId"]').material_select();
    $('select[name="pageId"]').material_select();


    $('#btnConfigurePageMeta').click(function () {
        var isValid = true;

        if (!($('#selectMake').val())) {
            Materialize.toast("Please select Make", 3000);
            isValid = false;
        }

        if (!($('#selectPage').val())) {
            Materialize.toast("Please select Page", 3000);
            isValid = false;
        }

        if ((vmConfigurePageMetas.selectedPage() == '3' || vmConfigurePageMetas.selectedPage() == '4') && !($('#selectModel').val()))
        {
            Materialize.toast("Please select Model", 3000);
            isValid = false;
        }

        return isValid;
    });

});
