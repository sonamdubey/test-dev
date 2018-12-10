export function validateName(custName, optional) {
  const reName = /^([-a-zA-Z ']*)$/;
  custName = custName.trim();
  if (custName == "") {
    if (!optional) {
      return {
        isValid: false,
        errMessage: "Please enter your name"
      };
    } else {
      return {
        isValid: true,
        errMessage: ""
      };
    }
  } else if (!reName.test(custName)) {
    return {
      isValid: false,
      errMessage: "Please enter only alphabets"
    };
  } else if (custName.length == 1) {
    return {
      isValid: false,
      errMessage: "Please enter your complete name"
    };
  } else
    return {
      isValid: true,
      errMessage: ""
    };
}

export function validateMobile(custMobile, optional) {
  const reMobile = /^[6789]\d{9}$/;
  if (custMobile == "") {
    if (!optional) {
      return {
        isValid: false,
        errMessage: "Please enter your mobile number"
      };
    } else {
      return {
        isValid: true,
        errMessage: ""
      };
    }
  } else if (custMobile.length != 10) {
    return {
      isValid: false,
      errMessage: "Mobile number should be of 10 digits"
    };
  } else if (!reMobile.test(custMobile)) {
    return {
      isValid: false,
      errMessage: "Please provide a valid 10 digit Mobile number"
    };
  } else
    return {
      isValid: true,
      errMessage: ""
    };
}

export function validateEmail(custEmail, optional) {
  const reEmail = /^[a-z0-9._%+-]+@[a-z-]{2,}\.[a-z]{2,}(\.[a-z]{1,}|$)$/;
  custEmail = custEmail.toLowerCase();
  if (custEmail == "") {
    if (optional) {
      return {
        isValid: true,
        errMessage: ""
      };
    } else {
      return {
        isValid: false,
        errMessage: "Please enter your email"
      };
    }
  } else if (!reEmail.test(custEmail)) {
    return {
      isValid: false,
      errMessage: "Please enter valid email"
    };
  } else
    return {
      isValid: true,
      errMessage: ""
    };
}

export function validateCity(optional, cityId) {
  if (!optional && cityId < 0) {
    return {
      isValid: false,
      errMessage: "Please select valid option"
    };
  }
  return {
    isValid: true,
    errMessage: ""
  };
}

export function validateDealer(optional) {
  if (!optional) {
    return {
      isValid: false,
      errMessage: "Please select one option"
    };
  }
  return {
    isValid: true,
    errMessage: ""
  };
}

export function validateButton(optional, listLength) {
  if (optional) {
    return false;
  } else if (listLength == 0) {
    return true;
  }
  return false;
}
