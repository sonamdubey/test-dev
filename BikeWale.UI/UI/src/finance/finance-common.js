
var basicInfo = {
        
    validateUserName: function (elem) {
        var isValid;
        var nameRegex = /^[a-zA-Z]{1,26}$/,
            value = $(elem)[0].value.trim();
        if (value.length == 0) {
            validate.setError(elem, "Please enter required field");
            isValid = false;
        }
        else if (nameRegex.test(value) && value.length > 1) {
            validate.hideError(elem);
            isValid = true;
        }
        else {
            validate.setError(elem, "Please enter valid name");
            isValid = false;
        }
        return isValid;
    },

    validateRadioButtons:function(groupName) {
            var isValid = true;
            if ($('input[name=' + groupName + ']:checked').length <= 0) {
                validate.setError($('input[name=' + groupName + ']').closest('ul'), 'Please select required field');
                isValid = false;
            } else {
                validate.hideError($('input[name=' + groupName + ']').closest('ul'));
                isValid = true;
            }
            return isValid;
    },

     validatePhoneNumber:function(inputMobile) {
            var regMob = new RegExp('^((7)|(8)|(9))[0-9]{9}$', 'i'),
                value = $(inputMobile).val();

            var regChar = /[A-z]+/;

            if (value.length == 0) {
                validate.setError($(inputMobile), "Please enter Mobile number");
                return false;
            }
            if (regChar.test(value)) {
            	validate.setError($(inputMobile), "Please enter numberic input");
            	return false;
            }
            if (value.length < 10) {
                validate.setError($(inputMobile), "Please enter 10 digits");
                return false;
            }
            if (!regMob.test(value)) {
                validate.setError($(inputMobile), "Mobile Number should start with only 7, 8 or 9");
                return false;
            }

            validate.hideError($(inputMobile));
            return true;
     },

      validateAddress:function(inputAddress) {
             var isValid = true, addressVal = $(inputAddress)[0].value.trim(),
               regex = /^[A-z0-9 #\/()-.,:\[\]]*$/;

             if (addressVal == ""||addressVal.length<10) {
                 validate.setError(inputAddress, "Address is too short");
                 isValid = false;
             }
             else if (!regex.test(addressVal)) {
                 validate.setError(inputAddress, "Please enter valid Address");
                 isValid = false;
             }

             return isValid;
      },
      validateRadioButtons:function(groupName) {
          var isValid = true;
          if ($('input[name=' + groupName + ']:checked').length <= 0) {
              validate.setError($('input[name=' + groupName + ']').closest('ul'), 'Please select required field');
              isValid = false;
          } else {
              validate.hideError($('input[name=' + groupName + ']').closest('ul'));
              isValid = true;
          }
          return isValid;
      },


       validateLandmark:function(inputLandmark) {
              var isValid = true, landmarkVal = $(inputLandmark)[0].value.trim(),
                regex = /^[A-z0-9 #\/()-.,:\[\]]*$/;

              if (landmarkVal == "") {
                  validate.setError(inputLandmark, "Please enter Landmark");
                  isValid = false;
              }
              else if (!regex.test(landmarkVal)) {
                  validate.setError(inputLandmark, "Please enter valid Landmark");
                  isValid = false;
              }

              return isValid;
       },

        validateTitleBox: function (inputSelectBox) {
            var isValid = true, selectboxVal = $(inputSelectBox).val();

            if (selectboxVal == "-1") {
                validate.setError(inputSelectBox, "Required");
                isValid = false;
            }

            return isValid;
        },
        
        validateResidingSince: function (inputResidingSince) {
        	var residingsinceVal = $(inputResidingSince).val();
        	var regChar = /[A-z]+/;

        	if (!residingsinceVal.length) {
        		validate.setError($(inputResidingSince), "Please enter residing since");
        		return false;
        	}

        	if (regChar.test(residingsinceVal)) {
        		validate.setError($(inputResidingSince), "Please enter numberic input");
        		return false;
        	}

        	if (parseInt(residingsinceVal) > 100) {
        		validate.setError($(inputResidingSince), "Residing since should be less then 100 years");
        		return false;
        	}

        	return true;
         },

          validatePinCode:function(inputPincode) {
                 var isValid = true,
                            pinCodeValue = inputPincode.val().trim(),
                            rePinCode = /^[1-9][0-9]{5}$/;

                 if (pinCodeValue.indexOf(',') > 0)
                     pinCodeValue = pinCodeValue.substring(0, 6);

                 if (pinCodeValue == "") {
                     validate.setError(inputPincode, 'Please enter pincode');
                     isValid = false;
                 }
                 else if (!rePinCode.test(pinCodeValue)) {
                     validate.setError(inputPincode, 'Invalid pincode');
                     isValid = false;
                 } else if (pincodeId <= 0) {
                     validate.setError(inputPincode, 'Please select pincode from given list.');
                     isValid = false;
                 } 

                 return isValid;
          },
          
          validateEmail: function (inputEmail) {
              var isValid = true,
                  emailValue = inputEmail.val().trim(),
                  regEmail = /^[A-z0-9._+-]+@[A-z0-9.-]+\.[A-z]{2,6}$/;

              if (emailValue == "") {
                  validate.setError(inputEmail, 'Please enter Email Id');
                  isValid = false;
              }
              else if (!regEmail.test(emailValue)) {
                  validate.setError($(inputEmail), 'Please enter valid Email Id');
                  isValid = false;
              }

              return isValid;
          },

          validateDOB: function (inputAge) {
              var dob = $(inputAge).val().trim();

              var isValid = true,
                  setDate = $(inputAge).val(),
                  date1 = new Date(setDate),
                  date2 = new Date(),
                  timeDiff = Math.abs(date2.getTime() - date1.getTime()),
                  diffDays = Math.ceil(timeDiff / (1000 * 3600 * 24)),
                  diffYears = diffDays / 365;
              if (dob.length == 0) {
                  validate.setError(inputAge, 'Please enter Age');
                  isValid = false;
              }
              else if (diffYears < 18) {
                  validate.setError(inputAge, 'Age should be greater than 18');
                  isValid = false;
              }
              return isValid;
          }

}


var employmentDetails = {   
    

     validateWorkingSince:function(inputWorkingSince) {
     	var workingsinceVal = $(inputWorkingSince).val();
     	var regChar = /[A-z]+/, isInputEnabled = !($(inputWorkingSince).prop('disabled'));

     	if (isInputEnabled) {
     	    if (!workingsinceVal.length) {
     	        validate.setError(inputWorkingSince, "Please enter working since");
     	        return false;
     	    }

     	    if (regChar.test(workingsinceVal)) {
     	        validate.setError(inputWorkingSince, "Please enter numberic input");
     	        return false;
     	    }

     	    if (parseInt(workingsinceVal) >= 500) {
     	        validate.setError(inputWorkingSince, "Please enter valid months");
     	        return false;
     	    }
     	} else {
     	    validate.hideError($(inputWorkingSince));
     	}

     	return true;
     },

     validateNetPrimaryIncome: function (inputNetPrimaryIncome) {
     	var newprimaryincomeVal = $(inputNetPrimaryIncome).val();
     	var regChar = /[A-z]+/, isInputEnabled = !($(inputNetPrimaryIncome).prop('disabled'));

     	if (isInputEnabled) {
     	    if (!newprimaryincomeVal.length) {
     	        validate.setError($(inputNetPrimaryIncome), "Please enter Net primary income");
     	        return false;
     	    }

     	    if (regChar.test(newprimaryincomeVal)) {
     	        validate.setError($(inputNetPrimaryIncome), "Please enter numeric input");
     	        return false;
     	    }
     	} else {
     	    validate.hideError($(inputNetPrimaryIncome));
     	}
     	return true;
      },

      validateNoOfDependants:function(inputNoOfDependants) {
      	var noofdependantsVal = $(inputNoOfDependants).val();
      	var regChar = /[A-z]+/, isInputEnabled = !($(inputNoOfDependants).prop('disabled'));
      	if (isInputEnabled) {
      	    if (!noofdependantsVal.length) {
      	        validate.setError($(inputNoOfDependants), "Please enter number of dependents");
      	        return false;
      	    }

      	    if (regChar.test(noofdependantsVal)) {
      	        validate.setError($(inputNoOfDependants), "Please enter numberic input");
      	        return false;
      	    }

      	    if (parseInt(noofdependantsVal) > 99) {
      	        validate.setError($(inputNoOfDependants), "Number of dependents should be less then 100");
      	        return false;
      	    }
      	} else {
      	    validate.hideError($(inputNoOfDependants));
      	}

      	return true;
      },

      validateOtherCompany: function (inputOtherCompany) {
          var isValid = true, otherCompanyVal = $(inputOtherCompany)[0].value.trim(),
            regex = /^[A-z0-9 #\/()-.,:\[\]]*$/, isInputEnabled = !($(inputOtherCompany).prop('disabled'));
          if (isInputEnabled) {
              if (otherCompanyVal == "") {
                  validate.setError(inputOtherCompany, "Please enter Company Name");
                  isValid = false;
              }
              else if (!regex.test(otherCompanyVal)) {
                  validate.setError(inputOtherCompany, "Please enter valid Company Name");
                  isValid = false;
              }
          } else {
              validate.hideError($(inputOtherCompany));
          }

          return isValid;
      },
      validateCompany: function (inputCompany) {
          var isValid = true, inputCompanyval = $(inputCompany)[0].value.trim(), isInputEnabled = !($(inputCompany).prop('disabled'));

          if (isInputEnabled) {
              if (inputCompanyval == "") {
                  validate.setError(inputCompany, "Please enter Company Name");
                  isValid = false;
              }
              else if (companyId <= 0) {
                  validate.setError(inputCompany, "Please select Company Name from list");
                  isValid = false;
              }
          } else {
              validate.hideError(inputCompany);
          }

          return isValid;
      },

}



var otherDetails = {
    
     validateAadharCard: function (inputIdValue) {
             var isValid = true, idVal = $(inputIdValue).val(),
                regexAadhar = /^[0-9]{12}$/;
             if (!regexAadhar.test(idVal)) {
                 validate.setError($('#financeCorrespondingIDNo'), "Please enter valid aadhar number");
                 isValid = false;
             }
             return isValid;
      }, 
        
    
      



      validateDrivingLicense: function (inputIdValue) {
                 var isValid = true, idVal = $(inputIdValue).val(),
                  regexLicense = /^[a-zA-Z0-9]{1,20}$/;
                 if (!regexLicense.test(idVal)) {
                     validate.setError($('#financeCorrespondingIDNo'), "Please enter valid license number");
                     isValid = false;
                 }
                 return isValid;
      },


      validatePanCard: function (inputIdValue) {
             var isValid = true, idVal = $(inputIdValue).val(),
              regexPan = /^[a-zA-Z0-9]{10}$/;
             if (!regexPan.test(idVal)) {
                 validate.setError($('#financeCorrespondingIDNo'), "Please enter valid pancard number");
                 isValid = false;
             }
             return isValid;
      },


      validatePassport: function (inputIdValue) {
             var isValid = true, idVal = $(inputIdValue).val(),
             regexPassport = /^[a-zA-Z0-9]{1,12}$/;
             if (!regexPassport.test(idVal)) {
                 validate.setError($('#financeCorrespondingIDNo'), "Please enter valid passport number");
                 isValid = false;
             }
             return isValid;
      },


      validateVoterId: function (inputIdValue) {
             var isValid = true, idVal = $(inputIdValue).val(),
             regexVoterId = /^[a-zA-Z0-9]{1,17}$/;
             if (!regexVoterId.test(idVal)) {
                 validate.setError($('#financeCorrespondingIDNo'), "Please enter valid voterid number");
                 isValid = false;
             }
             return isValid;
      },


      validateGovId: function (inputIdValue) {
             var isValid = true, idVal = $(inputIdValue).val(),
            regexGovId = /^[a-zA-Z0-9]{1,20}$/;
             if (!regexGovId.test(idVal)) {
                 validate.setError($('#financeCorrespondingIDNo'), "Please enter valid govt id number");
                 isValid = false;
             }
             return isValid;
      },

      validateOtherIDProof: function (inputIdValue) {
          var isValid = true, idVal = $(inputIdValue).val(),
         regexGovId = /^[a-zA-Z0-9]{1,20}$/;
          if (!regexGovId.test(idVal)) {
              validate.setError($('#financeCorrespondingIDNo'), "Please enter valid id proof number");
              isValid = false;
          }
          return isValid;
      },

      validateDOP: function (inputAge) {
          var dob = $(inputAge).val().trim();

          var isValid = true,
              setDate = $(inputAge).val()

          if (dob.length == 0) {
              validate.setError(inputAge, 'Please enter date');
              isValid = false;
          }

          return isValid;
      },

      validateBankAccount: function (inputBank) {
          var isValid = true, idVal = $(inputBank).val(), regexBankNo = /^\d{9,16}$/;
          if (idVal != "" && !regexBankNo.test(idVal)) {
              validate.setError($(inputBank), "Please enter valid id bank account number");
              isValid = false;
          }
          return isValid;
      }
}


var selectBox = {

    validateSelectBox: function (inputSelectBox) {
        var isValid = true, selectboxVal = parseInt($(inputSelectBox).val()), isFieldEnabled = !($(inputSelectBox).prop('disabled'));

        if (isFieldEnabled) {
            if (selectboxVal == "-1" || selectboxVal == "0") {
                validate.setError(inputSelectBox, "Please choose one option");
                isValid = false;
            }
        } else {
            validate.hideError(inputSelectBox);
        }
        

        return isValid;
    }
}

function checkPinCode(pinCode, inputPincode) {
    var isValid = false;
    $.ajax({
        async: false,
        type: "GET",
        url: "/api/autosuggest/?source=6&inputText=" + pinCode + "&noofrecords=5",
        contentType: "application/json",
        dataType: "json",
        success: function (data) {
            if (data && data.suggestionList.length > 0) {
                $(inputPincode).val(data.suggestionList[0].text);
                isValid = true;
            }
            else {
                validate.setError($(inputPincode), 'We do not serve in this area');
                isValid = false;
            }
        }
    });
    return isValid;
};