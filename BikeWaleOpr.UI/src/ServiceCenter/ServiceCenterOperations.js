$(document).ready(function () {
    $(window).keydown(function (event) {
        if (event.keyCode == 13) {
            event.preventDefault();
            return false;
        }
    });
});

var serviceCenterOperation = function () {
var self = this;
self.validateEmail = function (email) {
 
    var re = /.*@.*/;
    if (re.test(email)) {

        return true;
    }
    else
    {
        return false;
    }
}



self.validateNumber = function (number) {

    if (number != undefined) {

        var re = /^[0-9,.]*$/;
        if (re.test(number)) {

            return true;
        }
        else {
            return false;
        }
      
    }
}




self.validatePincode=function(pincode)
{
    var re = /^\d{6}$/;
    if(re.test(pincode))
    {
        return true;
    }
    else
    {
        return false;
    }
}


self.formValidation = function () {

   
    $('.required').each(function () {
        var currentEle = $(this);

       
            if (currentEle.val().trim() == '') {
                currentEle.parent().first().find("label").attr("data-error", "Required ");
                currentEle.addClass("Invalid");
                isValid = false;
            }
            else {
                currentEle.removeClass("Invalid");
            }
       
       
    });


    var name = $('#txtServiceCenterName').val();
    var email = $('#txtServiceCenterEmail').val();
    var mobilenumber = $('#txtMobileNumber').val();
    var phonenumber = $('#txtPhoneNumber').val();
    var address = $('#txtServiceCenterAddress').val();
    var pincode = $('#txtPincode').val();
    var emailStatus = true;
    var pinStatus = true;
    var mobileStatus = true;
    var phoneStatus = true;

    if (name.trim() === "" && address.trim() === "") {

        Materialize.toast('Name and Address are required fields', 5000);
      
    }
    else {

        if (mobilenumber != "") {
            var mobileStatus = self.validateNumber(mobilenumber)
            var currentEle = $('#txtMobileNumber');
            if (!mobileStatus) {

                currentEle.parent().find("label").attr("data-error", "Enter correct number");
                currentEle.addClass("Invalid");
                isValid = false;

            }
            else {
                currentEle.removeClass("Invalid");
            }
        }

        if (phonenumber != "") {
            var phoneStatus = self.validateNumber(phonenumber)
            var currentEle = $('#txtPhoneNumber');
            if (!phoneStatus) {

                currentEle.parent().find("label").attr("data-error", "Enter correct number");
                currentEle.addClass("Invalid");
                isValid = false;

            }
            else {
                currentEle.removeClass("Invalid");
            }
        }



        if (email != "") {
            emailStatus = self.validateEmail(email);
            var currentEle = $('#txtServiceCenterEmail');
            if (!emailStatus) {

                currentEle.parent().find("label").attr("data-error", "Enter correct email");
                currentEle.addClass("Invalid");
                isValid = false;

            }
            else {
                currentEle.removeClass("Invalid");
            }
        }

        if (pincode != "") {
            pinStatus = self.validatePincode(pincode);
            var currentEle = $('#txtPincode');
            if (!pinStatus) {

                currentEle.parent().find("label").attr("data-error", "Enter correct pincode");
                currentEle.addClass("Invalid");
                isValid = false;

            }
            else {
                currentEle.removeClass("Invalid");
            }
        }

       
        var status = (phoneStatus && emailStatus && pinStatus && mobileStatus);
      
        return status;


    }


}



}

var viewModel = new serviceCenterOperation();
ko.applyBindings(viewModel, $("#AddServiceCenter")[0]);