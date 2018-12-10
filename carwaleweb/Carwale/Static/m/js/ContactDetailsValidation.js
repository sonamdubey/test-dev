function ValidateContactDetails(Name, Email, MobileNo) {
    var errMsgs = [];

    //regular expressions
    var reName = /^([-a-zA-Z ']*)$/;
    var reEmail = /^[a-z0-9._%+-]+@[a-z0-9.-]+\.[a-z]{2,6}$/;
    var reMobile = /^[6789]\d{9}$/;

    var name = $.trim(Name);
    var email = $.trim(Email);
    var mobileNo = $.trim(MobileNo);

    var nameMsg, emailMsg, mobileMsg;

    //get appropriate validation msg for 'Name' 
    if (name == "" || name == "Enter your name" || name == "Enter Your Name") {
        nameMsg = "Please enter your name";
    }  else if (reName.test(name) == false) {
        nameMsg = "Please enter only alphabets";
    } else if (name.length == 1) {
        nameMsg = "Please enter your complete name";
    } else {
        nameMsg = "";
    }

    //get appropriate validation msg for 'Email' 
    if (email == "" || email == "Enter your e-mail id") {
        emailMsg = "Please enter your Email Id";
    } else if (!reEmail.test(email.toLowerCase())) {
        emailMsg = "Invalid Email Id";
    } else {
        emailMsg = "";
    }

    //get appropriate validation msg for 'MobileNo' 
    if (mobileNo == "" || mobileNo == "Enter your mobile number") {
        mobileMsg = "Please enter your mobile number";
    } else if (mobileNo.length != 10) {
        mobileMsg = "Enter your 10 digit mobile number";
    } else if (reMobile.test(mobileNo) == false) {
        mobileMsg = "Please provide a valid 10 digit Mobile number";
    } else {
        mobileMsg = "";
    }

    //error messages
    errMsgs[0] = nameMsg;
    errMsgs[1] = emailMsg;
    errMsgs[2] = mobileMsg;

    return errMsgs;
}