
var dateValue = null;

var $dateInput = $('.datepicker').pickadate({
    selectMonths: true, // Creates a dropdown to control month
    min: -90,
    max: true,
    closeOnSelect: true,
    onClose: function () { if (dateValue != $("#questionDateEle").val()) { $("#questionDate").val($("#questionDateEle").val()); } },
    onOpen: function () { dateValue = $("#questionDateEle").val(); },
    onSet: function (ele) { if (ele.select) { this.close(); } },
    format: "dd-mmm-yyyy"
});
// Use the picker object directly.
var $dateInput = $dateInput.pickadate('picker')
var filters = $("#addMakeContainer");

var Filters = function () {
    var self = this;
    self.selectedMakeId;
    self.bikeModels = ko.observableArray();
    self.searchEmailId = ko.observable();
    self.selectedMakeMaskingName;
    self.selectedModelMaskingName;
    self.selectedModelCaption = ko.observable('Select Models');
    self.moderationStatus = ko.observable(1);
    self.Tags = ko.observable();

    self.changeMake = function (d, e) {
        var makeId = $(e.target).find('option:selected').attr("data-makeid");
        self.selectedMakeMaskingName = $(e.target).val();
        if (makeId) {
            self.selectedMakeId = parseInt(makeId);
        }
        self.selectedModelMaskingName = '';
        self.selectedModelCaption('Select Models');
        self.getTags();
        self.fetchModels();
    };

    self.changeModel = function (d, e) {
        var target = $(e.target)
        self.selectedModelMaskingName = target.val();
        self.selectedModelCaption(target.find('option:selected').text());
        self.getTags();
    }

    self.fetchModels = function () {
        if (self.selectedMakeId != undefined && self.selectedMakeId > 0) {
            return $.ajax({
                type: "GET",
                url: "/api/models/makeid/" + self.selectedMakeId + "/requesttype/8/",
                contentType: "application/json",
                dataType: 'json',
                success: function (response) {
                    if (response && response.length > 0) {
                        self.bikeModels(response);
                        $('select[name="ModelMaskingName"]').material_select();
                    }
                    else {
                        self.bikeModels([]);
                    }

                },
                complete: function (xhr) {
                    if (xhr.status != 200) {
                        self.bikeModels([]);
                    }
                    $('select[name="ModelMaskingName"]').material_select();
                }
            });
        }
        return Promise.resolve();
    };

    self.changeStatus = function (d, e) {
        var questionStatus = $(e.target).val();
        if (questionStatus && questionStatus > 0) {
            $("input[type='hidden'][name='ModerationStatus']").val(questionStatus);
        }
    };

    self.setPageFilters = function () {
        filtersApplied = false;
        if (filters) {            
            self.selectedMakeMaskingName = filters.data("makemaskingname");
            if (self.selectedMakeMaskingName != undefined) {
                $('select[name="MakeMaskingName"]').val(self.selectedMakeMaskingName).trigger("change").material_select();
                self.selectedMakeId = $('select[name="MakeMaskingName"]').find('option:selected').attr("data-makeid");
                self.fetchModels().then(function() {
                    self.selectedModelMaskingName = filters.data("modelmaskingname");
                    if (self.selectedModelMaskingName != undefined) {
                        $('select[name="ModelMaskingName"]').val(vmFilters.selectedModelMaskingName).trigger("change").material_select();
                        self.selectedModelCaption($('select[name="ModelMaskingName"]').find("option:selected").text());
                    }
                });
                filtersApplied = true;
            }
          
            var questionStatus = filters.data("questionstatus");
            if (questionStatus && questionStatus > 0) {
                self.moderationStatus(questionStatus)                
            }            

            $("input[type='radio'][name='rdoQuestionStatus'][value=" + self.moderationStatus() + "]").trigger("click");
            $("input[type='hidden'][name='QuestionStatus']").val(self.moderationStatus());

            if (filters.data("date")) {
                $dateInput.set('select', new Date(filters.data("date")));
                filtersApplied = true;
            }
            else {
                $dateInput.clear();
            }

            if (filters.data("email")) {
                self.searchEmailId(decodeURIComponent(filters.data("email")));
                filtersApplied = true;
            }

            $(document).find('.modal').modal();

            if (filtersApplied) {
                var ele = $("#addMakeContainer ul li").first();
                if (ele) {
                    ele.addClass("active");
                    ele.find(".collapsible-header").addClass("active");
                    ele.find(".collapsible-body").show();
                }

            }

        }

    };
    
    self.getTags = function () {
        self.Tags((self.selectedModelMaskingName === undefined || self.selectedModelMaskingName === "") ? self.selectedMakeMaskingName : self.selectedModelMaskingName);
    };
    
};

var vmFilters = new Filters;
if (filters) {
    ko.applyBindings(vmFilters, filters[0]);
    vmFilters.setPageFilters();
}



