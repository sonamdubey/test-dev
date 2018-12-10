var form = {

    validation: {

        contact: function (Name, Email, MobileNo) {
            var errMsgs = [];
            errMsgs[0] = form.validation.checkName($.trim(Name));
            errMsgs[1] = form.validation.checkEmail($.trim(Email));
            errMsgs[2] = form.validation.checkMobile($.trim(MobileNo));;
            return errMsgs;
        },
        checkName: function (name) {
            var reName = /^([-a-zA-Z ']*)$/;
            var nameMsg = "";
            if (name == "" || name == "Enter your name" || name == "Enter Your Name") {
                nameMsg = "Please provide your name";
            } else if (reName.test(name) == false) {
                nameMsg = "Please provide only alphabets";
            } else if (name.length == 1) {
                nameMsg = "Please provide your complete name";
            }
            return nameMsg;
        },
        checkEmail: function (email) {
            var reEmail = /^[a-z0-9._%+-]+@[a-z0-9.-]+\.[a-z]{2,6}$/;
            var emailMsg = "";
            if (email == "" || email == "Enter your e-mail id") {
                emailMsg = "Please provide your Email Id";
            } else if (!reEmail.test(email.toLowerCase())) {
                emailMsg = "Invalid Email Id";
            }
            return emailMsg;
        },
        checkMobile: function (mobileNo) {
            var reMobile = /^[6789]\d{9}$/;
            var mobileMsg = "";
            if (mobileNo == "" || mobileNo == "Enter your mobile number") {
                mobileMsg = "Please provide your mobile number";
            } else if (mobileNo.length != 10) {
                mobileMsg = "Enter your 10 digit mobile number";
            } else if (reMobile.test(mobileNo) == false) {
                mobileMsg = "Please provide a valid 10 digit Mobile number";
            }
            return mobileMsg;
        }

    }
};