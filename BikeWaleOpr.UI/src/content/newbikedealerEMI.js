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

    self.Reset = function () {
        $("#txtMaxPayment").val("");
        self.txtMinPayment = ko.observable("");
        self.txtMaxPayment = ko.observable(null);
        self.txtMinTenure = ko.observable(null);
        self.txtMaxTenure = ko.observable(null);
        self.txtMinROI = ko.observable(null);
        self.txtMaxROI = ko.observable(null);
        self.txtMinLtv = ko.observable(null);
        self.txtMaxLtv = ko.observable(null);
        self.textLoanProvider = ko.observable(null);
        self.txtFees = ko.observable(null);
    };

    self.SaveEMI_Validate = function () {
        if (self.txtMinPayment()) {
            console.log("12asdfafd");
        }
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
});