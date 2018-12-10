$(document).ready(
	function () {
		$(".flip").flip({
			axis: 'y',
			trigger: 'manual',
			reverse: true
		});
	}
);
function ValidateContactDetails(name, email, mobileNo) {
	var errMsgs = [];
	var reName = /^([-a-zA-Z ']*)$/;
	var reEmail = /^[a-z0-9._%+-]+@[a-z0-9.-]+\.[a-z]{2,6}$/;
	var reMobile = /^[6789]\d{9}$/;

	if (name == "" || name == "Enter your name" || name == "Enter Your Name") {
		errMsgs[0] = "Please provide your name";
	} else if (reName.test(name) == false) {
		errMsgs[0] = "Please provide only alphabets";
	} else if (name.length == 1) {
		errMsgs[0] = "Please provide your complete name";
	} else {
		errMsgs[0] = "";
	}
	if (email == "" || email == "Enter your e-mail id") {
		errMsgs[1] = "Please provide your Email Id";
	} else if (!reEmail.test(email.toLowerCase())) {
		errMsgs[1] = "Invalid Email Id";
	} else {
		errMsgs[1] = "";
	}
	if (mobileNo == "" || mobileNo == "Enter your mobile number") {
		errMsgs[2] = "Please provide your mobile number";
	} else if (mobileNo.length != 10) {
		errMsgs[2] = "Please provide your 10 digit mobile number";
	} else if (reMobile.test(mobileNo) == false) {
		errMsgs[2] = "Please provide a valid 10 digit Mobile number";
	} else {
		errMsgs[2] = "";
	}
	return errMsgs;
}

function handleError(error, inputElement) {
	if (!error) {
		inputElement.siblings().addClass('hide');
		inputElement.removeClass('border-red');
	}
	else {
		inputElement.siblings().removeClass('hide');
		inputElement.addClass('border-red');
		var errSpan = inputElement.siblings('.cw-blackbg-tooltip')[0];
		$(errSpan).text(error);
		return false;
	}
	return true;
}

function pushInquiry(submitBtn,cityId) {
	var modelId = $(submitBtn).attr("modelid");
	var nameEle = $(submitBtn).parent().find("input[name='userName']");
	var emailEle = $(submitBtn).parent().find("input[name='userEmail']");
	var mobileEle = $(submitBtn).parent().find("input[name='userMobile']");
	var inputValues = [nameEle.val().trim(),emailEle.val().trim(),mobileEle.val().trim()];
	var err = ValidateContactDetails(inputValues[0], inputValues[1], inputValues[2]);
	var isFormValid = true;
	isFormValid = handleError(err[0], nameEle) & handleError(err[1], emailEle) & handleError(err[2], mobileEle); //& used to exec all conditions
	if (isFormValid) {
	    if (cityId != null && cityId != undefined) {
	        PushCRMLead(inputValues[0], inputValues[1], inputValues[2], cityId, '', 4, '', '', modelId, '', submitBtn);
	    }
		else if (isCookieExists("_CustCityIdMaster") && Number($.cookie("_CustCityIdMaster")) > 0) {
			PushCRMLead(inputValues[0], inputValues[1], inputValues[2], $.cookie("_CustCityIdMaster"), '', 4, '', '', modelId, '', submitBtn);
		}
		else if ((typeof (geoCityId) != "undefined" && geoCityId != "" && geoCityId > 0)) {
			PushCRMLead(inputValues[0], inputValues[1], inputValues[2], geoCityId, '', 4, '', '', modelId, '', submitBtn);
		}
		else
			PushCRMLead(inputValues[0], inputValues[1], inputValues[2], '', '', 4, '', '', modelId, '', submitBtn);
	}

}

function PushCRMLead(name, email, mobile, city, cityName, leadType, carName, makeId, modelId, versionId, submitBtn) {
	$.ajax({
		type: "POST",
		url: "/ajaxpro/CarwaleAjax.AjaxResearch,Carwale.ashx",
		data: '{"carName":"' + carName + '", "custName":"' + name + '", "email":"' + email + '", "mobile":"' + mobile + '", "selectedCityId":"' + city + '", "versionId":"' + versionId + '", "modelId":"' + modelId + '", "makeId":"' + makeId + '", "leadtype":"' + leadType + '", "cityName":"' + cityName + '"}',
		beforeSend: function (xhr) { xhr.setRequestHeader("X-AjaxPro-Method", "PushCRM"); },
		success: function (response) {
			var responseObj = eval('(' + response + ')');
			if (responseObj.value) {
				$(submitBtn).parents("li").find('.formContent').addClass('hide');
				$(submitBtn).parents("li").find('.thankYouForm').removeClass('hide');
			}
		}
	});
}