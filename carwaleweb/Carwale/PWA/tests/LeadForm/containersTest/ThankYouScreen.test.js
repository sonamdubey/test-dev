import React from "react";
import ThankYouScreen from "../../../src/LeadForm/Containers/ThankYouScreen";
import ThankYouImage from "../../../src/LeadForm/Components/ThankYouImage";
import Form from "oxygen/lib/Form/Form";
import * as Validate from "../../../src/LeadForm/utils/Validate";
import * as ThankYouState from "../../../src/LeadForm/ActionCreators/ThankYouState";
import * as LogState from "../../../src/LeadForm/ActionCreators/LogState";
import * as ScreenState from "../../../src/LeadForm/ActionCreators/ScreenState";
import { platform } from "../../../src/enum/Platform";
import store from "../../../src/LeadForm/store";
import { CLEAR_LOG } from "../../../src/LeadForm/Actions/LogActionTypes";
import { submitBhriguLogs } from "../../../src/LeadForm/utils/BhriguSubmission";

jest.mock("../../../src/LeadForm/utils/BhriguSubmission", () => {
  return { submitBhriguLogs: jest.fn() };
});

let storeState = {
  leadClickSource: {
    propId: 1,
    page: {
      platform: {
        id: 1,
        name: ""
      },
      page: {
        name: ""
      }
    },
    isCitySet: false
  },
  NC: {
    leadForm: {
      MLASellers: {
        list: {}
      },
      recommendation: {
        list: {}
      },
      buyerInfo: {
        name: "",
        mobile: "",
        email: ""
      }
    },
    campaign: {
      name: "",
      featuredCarData: {
        modelId: 1
      }
    }
  },
  location: {
    cityName: ""
  },
  log: [{}]
};

jest.mock("../../../src/LeadForm/store", () => {
  let storeState = {
    leadClickSource: {
      propId: 1,
      page: {
        platform: {
          id: 1,
          name: ""
        },
        page: {
          name: ""
        }
      },
      isCitySet: false
    },
    NC: {
      leadForm: {
        MLASellers: {
          list: {}
        },
        recommendation: {
          list: {}
        },
        buyerInfo: {
          name: "",
          mobile: "",
          email: ""
        }
      },
      campaign: {
        name: "",
        featuredCarData: {
          modelId: 1
        }
      }
    },
    location: {
      cityName: ""
    },
    log: [{}]
  };
  return mockStore(storeState);
});

jest.mock("../../../src/LeadForm/utils/Validate");

jest.mock("../../../src/LeadForm/ActionCreators/ScreenState");

jest.mock("../../../src/LeadForm/ActionCreators/LogState");

jest.mock("../../../src/LeadForm/ActionCreators/ThankYouState");

let thankYouWrapper;

beforeEach(() => {
  if (Validate.validateEmail) {
    Validate.validateEmail.mockClear();
  }
  Validate.validateEmail = jest.fn(() => {
    return { isValid: true, errMessage: "" };
  });

  if (ThankYouState.setThankYouLead) {
    ThankYouState.setThankYouLead.mockClear();
  }
  ThankYouState.setThankYouLead = jest.fn(() => {
    return { type: "" };
  });

  if (LogState.setInfoLog) {
    LogState.setInfoLog.mockClear();
  }
  LogState.setInfoLog = jest.fn(() => {
    return { type: "" };
  });

  if (LogState.clearLog) {
    LogState.clearLog.mockClear();
  }
  LogState.clearLog = jest.fn(() => {
    return { type: "CLEAR_LOG" };
  });

  if (ScreenState.setHide) {
    ScreenState.setHide.mockClear();
  }
  ScreenState.setHide = jest.fn(() => {
    return { type: "" };
  });

  //mocking container
  thankYouWrapper = shallowWithStore(<ThankYouScreen />, store).dive();
});

describe("Testign ThankYou Screen Container - ComponentDidMout", () => {
  test("Call setInfoLog - ThankYou Screen shown", () => {
    let message =
      "Entering ThankYouScreen (Current Store data Provided in currentState)";

    expect(LogState.setInfoLog).toHaveBeenCalledWith(message);
  });

  test("Call setInfoLog - ThankYou Screen shown", () => {
    expect(submitBhriguLogs).toHaveBeenCalledWith([{}]);
  });

  test("Call setInfoLog - ThankYou Screen shown", () => {
    let clearLog = { type: CLEAR_LOG };
    let actions = store.getActions();
    expect(actions).toContainEqual(clearLog);
  });
});

describe("Testing ThankYou screen container - DOM", () => {
  test("ThankYou with Email shown - email in store is empty", () => {
    let formWrapper = thankYouWrapper.find('[className="lead-popup-form"]');
    expect(formWrapper.exists()).toBeTruthy();
  });

  test("ThankYou with Image shown - email is available in store", () => {
    let storeStateWithEmail = {
      ...storeState,
      NC: {
        ...storeState.NC,
        leadForm: {
          ...storeState.NC.leadForm,
          buyerInfo: {
            ...storeState.NC.leadForm.buyerInfo,
            email: "joker@joker.com"
          }
        }
      }
    };
    let storeWithEmail = mockStore(storeStateWithEmail);
    let thankYouWrapper = shallowWithStore(
      <ThankYouScreen />,
      storeWithEmail
    ).dive();
    let imageWrapper = thankYouWrapper.find(ThankYouImage);
    expect(imageWrapper.exists()).toBeTruthy();
  });

  test("FormItem ValidateStatus is empty - email shown, validEmail.isValid is true in localstate", () => {
    const FormItem = Form.Item;
    thankYouWrapper.setState({ validEmail: { isValid: true } });
    let formItemWrapper = thankYouWrapper.find(FormItem);
    expect(formItemWrapper.first().props().validateStatus).toEqual("");
  });

  test("FormItem ValidateStatus is 'error' - email shown, validEmail.isValid is false in localstate ", () => {
    const FormItem = Form.Item;
    thankYouWrapper.setState({ validEmail: { isValid: false } });
    let formItemWrapper = thankYouWrapper.find(FormItem);
    expect(formItemWrapper.first().props().validateStatus).toEqual("error");
  });
});

describe("Testing ThankYou screen container - Interaction Events", () => {
  test("Set email in local state - correct/incorrect email filled, onBlur of email field", () => {
    let inputWrapper = thankYouWrapper.find("[id='email']");
    inputWrapper.simulate("blur", { target: { value: "abc@xyz.com" } });
    expect(thankYouWrapper.state().email).toEqual("abc@xyz.com");
  });

  test("Set validEmail to false with error message - incorrect email filled, onBlur of email field", () => {
    if (Validate.validateEmail) {
      Validate.validateEmail.mockClear();
    }

    Validate.validateEmail = jest.fn(() => {
      return { isValid: false, errMessage: "Please enter valid email" };
    });
    let inputWrapper = thankYouWrapper.find("[id='email']");
    inputWrapper.simulate("blur", { target: { value: "abc@x.c" } });
    expect(thankYouWrapper.state().validEmail.isValid).toBeFalsy();
    expect(thankYouWrapper.state().validEmail.errMessage).toEqual(
      "Please enter valid email"
    );
    expect(Validate.validateEmail).toHaveBeenCalledWith("abc@x.c", true);
  });

  test("Set validEmail to false with error message- incorrect email filled, onBlur of email field", () => {
    if (Validate.validateEmail) {
      Validate.validateEmail.mockClear();
    }

    Validate.validateEmail = jest.fn(() => {
      return { isValid: false, errMessage: "Please enter your email" };
    });
    let inputWrapper = thankYouWrapper.find("[id='email']");
    inputWrapper.simulate("blur", { target: { value: "" } });
    expect(thankYouWrapper.state().validEmail.isValid).toBeFalsy();
    expect(thankYouWrapper.state().validEmail.errMessage).toEqual(
      "Please enter your email"
    );
    expect(Validate.validateEmail).toHaveBeenCalledWith("", true);
  });

  test("Call setThankYouLead - Done button clicked, validation success", () => {
    if (Validate.validateEmail) {
      Validate.validateEmail.mockClear();
    }

    Validate.validateEmail = jest.fn(() => {
      return { isValid: true, errMessage: "" };
    });
    thankYouWrapper.setState({ email: "joker@joker.com" });
    let ButtonWrapper = thankYouWrapper.find("[type='primary']");
    ButtonWrapper.simulate("click");
    expect(ThankYouState.setThankYouLead).toHaveBeenCalledWith(
      "joker@joker.com"
    );
  });

  test("Call setHide {Desktop} - Done button clicked without entering any email", () => {
    let ButtonWrapper = thankYouWrapper.find("[type='primary']");
    ButtonWrapper.simulate("click");
    expect(ScreenState.setHide).toHaveBeenCalled();
  });

  test("Call setHide and go back in history {Other}- Done button clicked without entering any email", () => {
    let stateOtherPlatform = {
      ...storeState,
      leadClickSource: {
        ...storeState.leadClickSource,
        page: {
          ...storeState.leadClickSource.page,
          platform: {
            id: platform.MOBILE.id,
            name: platform.MOBILE.name
          }
        }
      }
    };
    let storeOtherPlatform = mockStore(stateOtherPlatform);
    let thankYouWrapper = shallowWithStore(
      <ThankYouScreen />,
      storeOtherPlatform
    ).dive();

    window.history.back = jest.fn();
    let ButtonWrapper = thankYouWrapper.find("[type='primary']");
    ButtonWrapper.simulate("click");
    expect(ScreenState.setHide).toHaveBeenCalled();
    expect(window.history.back).toHaveBeenCalled();
  });
});

describe("Testing ThankYou screen container - Tracking Events", () => {});
