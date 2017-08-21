function NewBikeDealerEMIViewModel() {

    var self = this;

    self.hdnLoanAmountId = ko.observable($("#hdnLoanAmountId").data('value'));
    self.txtMinPayment = ko.observable($("#txtMinPayment").data('value'));
    self.txtMaxPayment = ko.observable($("#txtMaxPayment").data('value'));
    self.txtMinTenure = ko.observable($("#txtMinTenure").data('value'));
    self.txtMaxTenure = ko.observable($("#txtMaxTenure").data('value'));
    self.txtMinROI = ko.observable($("#txtMinROI").data('value'));
    self.txtMaxROI = ko.observable($("#txtMaxROI").data('value'));
    self.txtMinLtv = ko.observable($("#txtMinLtv").data('value'));
    self.txtMaxLtv = ko.observable($("#txtMaxLtv").data('value'));
    self.textLoanProvider = ko.observable($("#textLoanProvider").data('value'));
    self.txtFees = ko.observable($("#txtFees").data('value'));
    
    self.errorMsgNumber = ko.observable("Only numbers allowed.");
    self.errorMsgWholeNumber =  ko.observable("Only whole numbers allowed.");

    self.dealerOperationsModel = ko.observable(new dealerOperationModel(dpParams));

    self.Reset = function () {

        self.txtMinPayment("");
        self.txtMaxPayment("");
        self.txtMinTenure("");
        self.txtMaxTenure("");
        self.txtMinROI("");
        self.txtMaxROI("");
        self.txtMinLtv("");
        self.txtMaxLtv("");
        self.textLoanProvider("");
        self.txtFees("");
    };

    self.SaveEMI_Validate = function () {
        self.errorMsgNumber("Only numbers allowed upto 100.");
        self.errorMsgWholeNumber("Only whole numbers allowed.");
        var isValid = true;
        $('input[type="text"].emiInfo').each(function () {
            var value = $.trim($(this).val());
            if (value == '') {
                isValid = false;
                $(this).addClass('InValid');
            }
            else {
                $(this).val(value);
                $(this).removeClass('InValid');
            }
        });

        if (isValid) {
            var value = "";
            var reg = /^\d+(\.\d+)?$/;
            $('input[type="text"].perc').each(function () {
                value = $(this).val();
                if (reg.test(value) && parseFloat(value) >= 0 && parseFloat(value) <= 100) {
                    $(this).removeClass('InValid');
                }
                else {
                    isValid = false;
                    $(this).addClass('InValid');
                }
            });

        }

        if (isValid) {

            var reg = /^\d+$/;
            $('input[type="text"].int').each(function () {
                if (reg.test($(this).val())) {
                    $(this).removeClass('InValid');
                }
                else {
                    isValid = false;
                    $(this).addClass('InValid');
                }
            });
        }

        if (isValid) {

            var reg = /^\d+$/;
            var value = "";
            $('input[type="text"].intperc').each(function () {
                value = $(this).val();
                if (reg.test(value) && parseFloat(value) >= 0 && parseFloat(value) <= 100) {
                    $(this).removeClass('InValid');
                }
                else {
                    isValid = false;
                    $(this).addClass('InValid');
                }
            });
        }

        if (isValid) {
            var reg = /^\d+(\.\d+)?$/;
            $('input[type="text"].double').each(function () {
                if (reg.test($(this).val())) {
                    $(this).removeClass('InValid');
                }
                else {
                    isValid = false;
                    $(this).addClass('InValid');
                }
            });
        }
        if (isValid) {
            if (self.txtMinPayment() > self.txtMaxPayment()) {
                isValid = false;
                self.errorMsgNumber("");
                $("#txtMinPayment").addClass('InValid');
                $("#txtMaxPayment").addClass('InValid');
                Materialize.toast('Min field should be less than Max field', 5000);
            }
            else if (self.txtMinTenure() > self.txtMaxTenure()) {

                self.errorMsgWholeNumber("");
                $("#txtMinPayment").removeClass('InValid');
                $("#txtMaxPayment").removeClass('InValid');


                isValid = false;
                $("#txtMinTenure").addClass('InValid');
                $("#txtMaxTenure").addClass('InValid');
                Materialize.toast('Min field should be less than Max field', 5000);
            }
            else if (self.txtMinROI() > self.txtMaxROI()) {

                self.errorMsgNumber("");
                $("#txtMinTenure").removeClass('InValid');
                $("#txtMaxTenure").removeClass('InValid');

                isValid = false;
                $("#txtMinROI").addClass('InValid');
                $("#txtMaxROI").addClass('InValid');
                Materialize.toast('Min field should be less than Max field', 5000);
            }
            else if (self.txtMinLtv() > self.txtMaxLtv()) {

                self.errorMsgNumber("");
                $("#txtMinROI").removeClass('InValid');
                $("#txtMaxROI").removeClass('InValid');

                isValid = false;
                $("#txtMinLtv").addClass('InValid');
                $("#txtMaxLtv").addClass('InValid');
                Materialize.toast('Min field should be less than Max field', 5000);
            }
            else {
                isValid = true;
                $("#txtMinLtv").removeClass('InValid');
                $("#txtMaxLtv").removeClass('InValid');

            }
        }

        

        return isValid;
    };

    self.Delete = function () {

        $.ajax({
            type: "GET",
            url: "/api/Dealers/DeleteDealerEMI/?id=" + self.hdnLoanAmountId(),
            success: function (response) {
                self.Reset();
                self.hdnLoanAmountId('');
                $("#btnSaveEMI").html('Add EMI');
                $('#btnDelete').hide();
                Materialize.toast('Data has been deleted.', 4000);
            }
        });

    };
}

var vmNewBikeDealerEMI = new NewBikeDealerEMIViewModel();
$(document).ready(function () {
    try {
        ko.applyBindings(vmNewBikeDealerEMI);

        if (vmNewBikeDealerEMI.hdnLoanAmountId() != "") {
            $("#btnSaveEMI").html('Update EMI');
        }
    } catch (e) {
        console.log(e.message);
    }

    (function () {
        $('select.chosen-select').chosen({
            "width": "250px"
        });

        $('#ddlDealerOperations').val(4);
        $("#ddlDealerOperations").trigger('chosen:updated')
    }());
});




