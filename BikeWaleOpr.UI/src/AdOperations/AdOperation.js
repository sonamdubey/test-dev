if (msg != "") { Materialize.toast(msg, 4000); }
var ddlModels = $('#ddlModels');


var adOperationsViewModel = function () {

    var self = this;


    self.selectedMake = ko.observable();
    self.listModels = ko.observableArray([]);

    self.selectMake = function () {
        self.selectedMake($('#ddlMakes option:selected').val());
        if (self.selectedMake() != undefined && self.selectedMake() > 0) {
            $.ajax({
                type: "GET",
                url: bwHostUrl + "/api/campaigns/manufacturer/models/makeId/" + self.selectedMake() + "/",
                datatype: "json",
                success: function (response) {
                    var models = ko.toJS(response);
                    if (models) {
                        self.listModels(models);
                    }
                },
                complete: function (xhr) {
                    ddlModels.material_select();
                }
            });
        }
        else {
            self.listModels([]);
            ddlModels.material_select();
        }
    };

    self.validation = function () {
        var isValidate=false;
        if ($('#ddlMakes').val() != "Select Make" || $('#ddlModels').val() != null)
            isValidate = true;
        else
        {
            if ($('#ddlMakes').val() == "Select Make") {
                Materialize.toast('Please select Make', 5000);
                isValidate &= false;
            }
            if (isValidate && $('#ddlModels').val() == "") {
                Materialize.toast('Please select Model', 5000);
                isValidate &= false;
            }

        }

        isValidate &= validateRadioButtons("AdMonetization");
        if (isValidate && $('#startDateEle').val() == "")
        {
            Materialize.toast('Please Enter Start Date', 5000);
            isValidate &= false;
        }
        if (isValidate && $('#endDateEle').val() == "") {
            Materialize.toast('Please Enter End Date', 5000);
            isValidate &= false;
        }
        if (isValidate) {
            saveAdOperation();
        }
      

    };


  function saveAdOperation(){


        var basicDetails = {
            "make":
                {
                    "MakeId": $('#ddlMakes').val()
                },
            "model": {
                "ModelId": $('#ddlModels').val()
            },
            "startTime": $('#startDateEle').val() + ' ' + $('#startTimeEle').val(),
            "endTime": $('#endDateEle').val() + ' ' + $('#endTimeEle').val(),
            "adOperationType": $('#chkShowPromotion').is(':checked') ? 1 : 2,
            "userId": userId


        };
        $.ajax({
            type: "POST",
            url: "/api/adoperations/save/",
            contentType: "application/json",
            data: ko.toJSON(basicDetails),
            success: function (response) {

                if (response) {
                    window.location.reload();
                    Materialize.toast('AdOperation saved', 4000);
                }
                else {
                    Materialize.toast('Something went wrong', 4000);
                }

            }
        });
    };
    function validateRadioButtons(groupName) {
        var isValid = true;
        if ($('input[name=' + groupName + ']:checked').length <= 0) {
            validate.setError($('input[name=' + groupName + ']').closest('ul'), 'Please select required field');
            Materialize.toast('Please select Promotion Type', 4000);
            isValid = false;
        } else {
            validate.hideError($('input[name=' + groupName + ']').closest('ul'));
            isValid = true;
        }
        return isValid;
    }
     self.updateAdOperation = function (e) {
        if (confirm('Are you sure?')) {
            var currentRow = $(e.target).closest("tr").first();
            var basicDetails = {
                "promotedBikeId": $(currentRow).data("promotedbikeid"),
                "adOperationType": $(currentRow).find("td[data-value='adoperationtype']").text().trim(),
                "lastUpdateBy": userId,
                "contractStatus": 3   //Status code for deleted
            }

            $.ajax({
                type: "POST",
                url: "/api/adoperation/update/",
                data: ko.toJSON(basicDetails),
                contentType: "application/json",

                success: function (response) {

                    window.location.reload();
                    Materialize.toast('Deleted Successfully', 4000);

                }
            });

        };
    };


};

        



$(document).ready(function () {

    mfgVM = new adOperationsViewModel;
    ko.applyBindings(mfgVM, $('#adOperationContainer')[0]);

    var $dateInput = $('.datepicker').pickadate({
        selectMonths: true,
        closeOnSelect: true,
        onClose: function () { if (dateValue != $("#reviewDateEle").val()) $("#reviewDate").val($("#reviewDateEle").val()); },
        onOpen: function () { dateValue = $("#reviewDateEle").val() },
        onSet: function (ele) { if (ele.select) { this.close(); } }
    });
    validate = {
        setError: function (element, message) {
            var elementLength = element.val().length,
                errorTag = element.siblings('.error-text');

            errorTag.text(message).show();
            if (!elementLength) {
                element.closest('.input-box').removeClass('not-empty').addClass('invalid');
            }
            else {
                element.closest('.input-box').addClass('not-empty invalid');
            }
        },
        hideError: function (element) {
            element.closest('.input-box').removeClass('invalid').addClass('not-empty');
            element.siblings('.error-text').text('');
        },
        onFocus: function (inputField) {
            if (inputField.closest('.input-box').hasClass('invalid')) {
                validate.hideError(inputField);
            }
        },
        onBlur: function (inputField) {
            var inputLength = inputField.val().length;
            if (!inputLength) {
                inputField.closest('.input-box').removeClass('not-empty');
            }
            else {
                inputField.closest('.input-box').addClass('not-empty');
            }
        }
    };
});