var otpVerification = function () {
    var _otpVariables = {
        sourceModule: 1,  //For UsedcarLead module
        mobileVerificationByType: {
            otp: 1,
            missedCall: 2,
            otpAndMissedCall: 3
        },
        defaultOtpLength:5,
        defaultValidityInMins: 30
    };

    var _getMobileVerificationApiData = function (mobile) {
        var mobileVerificationApiData = {
            "sourceModule": _otpVariables.sourceModule,
            "mobileVerificationByType": _otpVariables.mobileVerificationByType.otpAndMissedCall,
            "validityInMins": _otpVariables.defaultValidityInMins,
            "otpLength": _otpVariables.defaultOtpLength
        };
        if (mobile) {
            mobileVerificationApiData["mobile"] = mobile;
        }
        return mobileVerificationApiData;
    };

    var sendOtp = function (mobileNumber, sourceId) {
        return $.ajax({
            type: 'POST',
            headers: { "sourceid": sourceId },
            url: '/api/v1/mobile/' + mobileNumber + '/verification/start/',
            data: JSON.stringify(_getMobileVerificationApiData()),
            contentType: 'application/json',
            dataType: 'json'
        });
    };

    var verifyMobile = function (mobileNumber, sourceId) {
        return $.ajax({
            type: 'GET',
            headers: { "sourceid": sourceId },
            url: '/api/v1/mobile/' + mobileNumber + '/verification/status/',
            dataType: 'Json'
        });
    };

    var verifyOtp = function (mobileNo, otp, sourceId) {
        return $.ajax({
            type: 'GET',
            url: '/api/v1/mobile/' + mobileNo + '/verification/verifyotp/?otpCode=' + otp + '&sourceModule=' + _otpVariables.sourceModule,
            headers: { 'SourceId': sourceId },
            dataType: 'Json'
        });
    };

    var resendOtp = function (mobileNo, sourceId) {
        return $.ajax({
            type: "POST",
            url: "/api/v1/resendotp/",
            headers: { 'sourceid': sourceId },
            data: JSON.stringify(_getMobileVerificationApiData(mobileNo)),
            contentType: 'application/json',
            dataType: 'json'
        });
    };
    return { sendOtp: sendOtp, verifyOtp: verifyOtp, resendOtp: resendOtp, verifyMobile: verifyMobile };
}();