import {
  validateName,
  validateMobile,
  validateEmail,
  validateCity,
  validateDealer,
  validateButton
} from "../../../src/LeadForm/utils/Validate";

test("ValidateName utils testOutput testCases", () => {
  const testCase1 = "";
  const testCase2 = "123@RJ";
  const testCase3 = "R";
  const testCase4 = "Ritesh Jaiswal";
  const optional1 = true;
  const optional2 = false;

  const testOutput1 = {
    isValid: false,
    errMessage: "Please enter your name"
  };
  const testOutput2 = {
    isValid: false,
    errMessage: "Please enter only alphabets"
  };
  const testOutput3 = {
    isValid: false,
    errMessage: "Please enter your complete name"
  };
  const testOutput4 = {
    isValid: true,
    errMessage: ""
  };

  expect(validateName(testCase1, optional1)).toEqual(testOutput4);
  expect(validateName(testCase2, optional1)).toEqual(testOutput2);
  expect(validateName(testCase3, optional1)).toEqual(testOutput3);
  expect(validateName(testCase4, optional1)).toEqual(testOutput4);
  expect(validateName(testCase1, optional2)).toEqual(testOutput1);
  expect(validateName(testCase2, optional2)).toEqual(testOutput2);
  expect(validateName(testCase3, optional2)).toEqual(testOutput3);
  expect(validateName(testCase4, optional2)).toEqual(testOutput4);
});

test("ValidateEmail utils testOutput testCases", () => {
  const testCase1 = "";
  const testCase2 = "123RJ";
  const testCase3 = "Rwsc@dsdf.fcdf";
  const optional1 = true;
  const optional2 = false;

  const testOutput1 = {
    isValid: true,
    errMessage: ""
  };
  const testOutput2 = {
    isValid: false,
    errMessage: "Please enter your email"
  };
  const testOutput3 = {
    isValid: false,
    errMessage: "Please enter valid email"
  };

  expect(validateEmail(testCase1, optional1)).toEqual(testOutput1);
  expect(validateEmail(testCase2, optional1)).toEqual(testOutput3);
  expect(validateEmail(testCase3, optional1)).toEqual(testOutput1);
  expect(validateEmail(testCase1, optional2)).toEqual(testOutput2);
  expect(validateEmail(testCase2, optional2)).toEqual(testOutput3);
  expect(validateEmail(testCase3, optional2)).toEqual(testOutput1);
});

test("ValidateMobile utils testOutput testCases", () => {
  const testCase1 = "";
  const testCase2 = "24332";
  const testCase3 = "2435432223";
  const testCase4 = "8888888888";
  const optional1 = true;
  const optional2 = false;

  const testOutput1 = {
    isValid: false,
    errMessage: "Please enter your mobile number"
  };
  const testOutput2 = {
    isValid: false,
    errMessage: "Mobile number should be of 10 digits"
  };
  const testOutput3 = {
    isValid: false,
    errMessage: "Please provide a valid 10 digit Mobile number"
  };
  const testOutput4 = {
    isValid: true,
    errMessage: ""
  };

  expect(validateMobile(testCase1, optional1)).toEqual(testOutput4);
  expect(validateMobile(testCase2, optional1)).toEqual(testOutput2);
  expect(validateMobile(testCase3, optional1)).toEqual(testOutput3);
  expect(validateMobile(testCase4, optional1)).toEqual(testOutput4);
  expect(validateMobile(testCase1, optional2)).toEqual(testOutput1);
  expect(validateMobile(testCase2, optional2)).toEqual(testOutput2);
  expect(validateMobile(testCase3, optional2)).toEqual(testOutput3);
  expect(validateMobile(testCase4, optional2)).toEqual(testOutput4);
});

test("ValidateCity utils testOutput testCases", () => {
  const optional1 = true;
  const optional2 = false;

  const testOutput1 = {
    isValid: true,
    errMessage: ""
  };
  const testOutput2 = {
    isValid: false,
    errMessage: "Please select one option"
  };

  expect(validateCity(optional1)).toEqual(testOutput1);
  expect(validateCity(optional2)).toEqual(testOutput2);
});

test("ValidateDealer utils testOutput testCases", () => {
  const optional1 = true;
  const optional2 = false;

  const testOutput1 = {
    isValid: true,
    errMessage: ""
  };
  const testOutput2 = {
    isValid: false,
    errMessage: "Please select one option"
  };

  expect(validateDealer(optional1)).toEqual(testOutput1);
  expect(validateDealer(optional2)).toEqual(testOutput2);
});

test("ValidateButton utils testOutput testCases", () => {
  const testCase1 = 0;
  const testCase2 = 5;
  const optional1 = true;
  const optional2 = false;

  expect(validateButton(optional1, testCase1)).toBeFalsy();
  expect(validateButton(optional1, testCase2)).toBeFalsy();
  expect(validateButton(optional2, testCase1)).toBeTruthy();
  expect(validateButton(optional2, testCase2)).toBeFalsy();
});
