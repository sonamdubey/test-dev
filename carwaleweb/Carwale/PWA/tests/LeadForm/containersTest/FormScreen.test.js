import React from "react";
import FormScreen from "../../../src/LeadForm/Containers/FormScreen";
import * as Validate from "../../../src/LeadForm/utils/Validate";
import * as FormState from "../../../src/LeadForm/ActionCreators/FormState";
import * as ScreenState from "../../../src/LeadForm/ActionCreators/ScreenState";
import * as LogState from "../../../src/LeadForm/ActionCreators/LogState";
import dealersApi from "../../../src/LeadForm/Api/Dealer";
import MLAApi from "../../../src/LeadForm/Api/MLA";
import citiesApi from "../../../src/LeadForm/Api/City";
import * as ScreenHeight from "../../../src/LeadForm/utils/ScreenHeight";
import { setCityChange } from "../../../src/LeadForm/ActionCreators/Location";
import Checkbox from "oxygen/lib/Checkbox/Checkbox";
import Button from "oxygen/lib/Button/Button";
import { makeCancelable } from "../../../src/utils/CancelablePromise";
import * as ObjectMap from "../../../src/LeadForm/utils/ObjectMapping";
import store from "../../../src/LeadForm/store";
import FormScreenHeader from "../../../src/LeadForm/Components/FormScreenHeader";

jest.mock("../../../src/LeadForm/utils/Tracking", () => {
  return {
    trackNameErr: jest.fn(),
    trackMobileErr: jest.fn(),
    trackEmailErr: jest.fn()
  };
});

jest.mock("../../../src/LeadForm/store", () => {
  return mockStore({
    leadClickSource: {
      propId: 1,
      page: {
        platform: {
          name: ""
        },
        page: {
          name: ""
        }
      }
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
        campaign: {
          id: 1001,
          dealerId: 12,
          contactName: "Bahubali",
          isEmailRequired: false,
          isTestDriveCampaign: false,
          isTurboMla: false,
          leadPanel: 1,
          dealerAdminId: 1
        },
        featuredCarData: {
          modelId: 111,
          makeName: "Bugati",
          modelName: "V",
          hostUrl: "abc",
          originalImgPath: "xyz"
        },
        campaignType: 2
      }
    },
    location: {
      cityId: 0
    },
    isLeadFormVisible: true,
    interactionId: 1
  });
});

//mocking Validate module
jest.mock("../../../src/LeadForm/utils/Validate");

jest.mock("../../../src/LeadForm/Components/FormScreenHeader");

//mocking getDealersApi module
jest.mock("../../../src/LeadForm/Api/Dealer");

jest.mock("../../../src/LeadForm/Api/City");

jest.mock("../../../src/LeadForm/Api/MLA");

//mocking leadSubmissionApi module
jest.mock("../../../src/LeadForm/Api/LeadSubmission");

jest.mock("../../../src/LeadForm/utils/ObjectMapping");

jest.mock("../../../src/LeadForm/ActionCreators/FormState");

jest.mock("../../../src/LeadForm/ActionCreators/Location", () => {
  return {
    setCityChange: jest.fn(() => {
      return {
        type: ""
      };
    })
  };
});

jest.mock("../../../src/LeadForm/ActionCreators/MLAState");

jest.mock("../../../src/LeadForm/ActionCreators/LogState");

jest.mock("../../../src/utils/CancelablePromise", () => {
  return {
    makeCancelable: jest.fn(promise => ({
      promise,
      cancel: jest.fn(() => {
        return true;
      })
    }))
  };
});

const dealerApiResponse = [
  {
    id: 18541,
    name: " - Renault  Delhi West",
    area: "110115",
    distance: 0
  },
  {
    id: 19418,
    name: " - Renault Delhi East",
    area: "110092",
    distance: 0
  },
  {
    id: 22357,
    name: " - Renault Prashant Vihar",
    area: "110085",
    distance: 0
  }
];

const dealerResponse2 = [
  {
    id: 18541,
    name: " - Renault  Delhi West",
    area: "110115",
    distance: 0
  }
];

const MLAApiResponse = [
  {
    campaign: {
      id: 5593,
      dealerId: 11048,
      contactName: "Regent Honda Thane",
      contactNumber: "",
      contactEmail: "",
      type: 0,
      actionText: "Get Offers from Dealer",
      isEmailRequired: false,
      leadPanel: 2,
      showroomDealer: null,
      cvlDetails: { isCvl: false },
      predictionData: null,
      mutualLeads: true,
      dealerAdminId: 0
    },
    dealerDetails: {
      id: 11048,
      name: "Regent Honda Thane",
      area: "Ghodbunder Road",
      distance: 1
    },
    pageProperty: [],
    featuredCarData: {
      modelId: 1079
    },
    campaignType: 1
  },
  {
    campaign: {
      id: 7239,
      dealerId: 25339,
      contactName: "Honda India",
      contactNumber: "",
      contactEmail: "",
      type: 0,
      actionText: "Get Offers from Dealer",
      isEmailRequired: false,
      leadPanel: 2,
      showroomDealer: null,
      cvlDetails: { isCvl: false },
      predictionData: null,
      mutualLeads: true,
      dealerAdminId: 1
    },
    dealerDetails: { id: 25339, name: "Honda India", area: "", distance: 1 },
    pageProperty: [],
    featuredCarData: {
      modelId: 1079
    },
    campaignType: 1
  }
];

let cityApiResponse = [
  { id: 221, name: "Agra", cityMaskingName: null },
  { id: 128, name: "Ahmedabad", cityMaskingName: null },
  { id: 14, name: "Ahmednagar", cityMaskingName: null }
];

let storeCopy;
let FormWrapper;
let storeState;
let props = {
  modelDetail: {
    modelId: 1111,
    versionId: 2222
  }
};

storeState = {
  leadClickSource: {
    page: {
      platform: {
        name: ""
      },
      page: {
        name: ""
      }
    }
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
      campaign: {
        id: 1001,
        dealerId: 12,
        contactName: "",
        isEmailRequired: false,
        isTestDriveCampaign: false,
        leadPanel: 1,
        dealerAdminId: 1
      },
      featuredCarData: {
        modelId: 11
      }
    }
  },
  location: {
    cityId: 0,
    cityName: ""
  },
  isLeadFormVisible: true,
  interactionId: 1
};

//mocking screenheight module
ScreenHeight.setScreenBodyHeight = jest.fn(() => {});

//mocking setFormLead module
FormState.setFormLead = jest.fn(() => {
  return {
    type: ""
  };
});

LogState.setInfoLog = jest.fn(() => {
  return {
    type: ""
  };
});

LogState.setErrorLog = jest.fn(() => {
  return {
    type: ""
  };
});

LogState.setDebugLog = jest.fn(() => {
  return {
    type: ""
  };
});

ScreenState.setMLAData = jest.fn(() => {
  return {
    type: ""
  };
});

window.leadConversionTracking = {
  track: jest.fn()
};

beforeEach(() => {
  // mocking validatName function
  Validate.validateName = jest.fn().mockImplementation((optional, length) => {
    return {
      isValid: true,
      errMessage: ""
    };
  });

  Validate.validateMobile = jest.fn().mockImplementation((optional, length) => {
    return {
      isValid: true,
      errMessage: ""
    };
  });

  Validate.validateEmail = jest.fn().mockImplementation((optional, length) => {
    return {
      isValid: true,
      errMessage: ""
    };
  });

  Validate.validateCity = jest.fn().mockImplementation((optional, length) => {
    return {
      isValid: true,
      errMessage: ""
    };
  });

  Validate.validateDealer = jest.fn().mockImplementation((optional, length) => {
    return {
      isValid: true,
      errMessage: ""
    };
  });

  if (dealersApi.get) {
    dealersApi.get.mockClear();
  }

  if (citiesApi.get) {
    citiesApi.get.mockClear();
  }

  if (MLAApi.get) {
    MLAApi.get.mockClear();
  }

  dealersApi.get = jest.fn(() => Promise.resolve(dealerApiResponse));

  citiesApi.get = jest.fn(() => Promise.resolve(cityApiResponse));

  MLAApi.get = jest.fn(() => Promise.resolve(MLAApiResponse));

  //mocking store
  storeCopy = store;

  //mocking container
  FormWrapper = shallowWithStore(
    <FormScreen modelDetail={props.modelDetail} />,
    storeCopy
  ).dive();
});

describe("Testing Form screen container - DOM", () => {
  test("Show cross sell header - isCrossSell flag set", () => {
    let formScreenHeader = FormWrapper.find(FormScreenHeader);
    expect(formScreenHeader.props().isCrossSell).toBeTruthy();
    expect(formScreenHeader.props().imageUrl).toEqual("abc310x174xyz");
    expect(formScreenHeader.props().fullCarName).toEqual("Bugati V");
    expect(formScreenHeader.props().campaignName).toEqual("Bahubali");
  });

  test("Hide email Input - isEmailRequired flag in store not set", () => {
    let emailItemWrapper = FormWrapper.find('[label="Email-Id"]');
    expect(emailItemWrapper.exists()).toBeFalsy();
  });

  test("Show email Input - isEmailRequired flag in store set", () => {
    let testStoreState = {
      ...storeState,
      NC: {
        ...storeState.NC,
        campaign: {
          ...storeState.NC.campaign,
          campaign: {
            ...storeState.NC.campaign.campaign,
            isEmailRequired: true
          }
        }
      }
    };
    storeCopy = mockStore(testStoreState);
    FormWrapper = shallowWithStore(<FormScreen />, storeCopy).dive();
    let emailItemWrapper = FormWrapper.find('[label="Email-Id"]');
    expect(emailItemWrapper.exists()).toBeTruthy();
  });

  test("Hide city dropdown - showLocation not set", () => {
    let cityItemWrapper = FormWrapper.find('[label="City"]');
    expect(cityItemWrapper.exists()).toBeFalsy();
  });

  test("Show city dropdown - showLocation set", () => {
    FormWrapper.setState({ showLocation: true });
    let cityItemWrapper = FormWrapper.find('[label="City"]');
    expect(cityItemWrapper.exists()).toBeTruthy();
  });

  test("Hide seller dropdown - showSeller not set", () => {
    let dealerItemWrapper = FormWrapper.find('[label="Dealership"]');
    expect(dealerItemWrapper.exists()).toBeFalsy();
  });

  test("Show seller dropdown - showSeller set", () => {
    FormWrapper.setState({ showSeller: true });
    let dealerItemWrapper = FormWrapper.find('[label="Dealership"]');
    expect(dealerItemWrapper.exists()).toBeTruthy();
  });

  test("Hide test drive checkbox - isTestDriveCampaign flag in store not set", () => {
    let testDriveWrapper = FormWrapper.find(Checkbox);
    expect(testDriveWrapper.exists()).toBeFalsy();
  });

  test("Show test drive checkbox - isTestDriveCampaign flag in store set", () => {
    let testStoreState = {
      ...storeState,
      NC: {
        ...storeState.NC,
        campaign: {
          ...storeState.NC.campaign,
          campaign: {
            ...storeState.NC.campaign.campaign,
            isTestDriveCampaign: true
          }
        }
      }
    };
    storeCopy = mockStore(testStoreState);
    FormWrapper = shallowWithStore(<FormScreen />, storeCopy).dive();
    let testDriveWrapper = FormWrapper.find(Checkbox);
    expect(testDriveWrapper.exists()).toBeTruthy();
  });

  test("Show test drive disclaimer - test drive checked", () => {
    let testStoreState = {
      ...storeState,
      NC: {
        ...storeState.NC,
        campaign: {
          ...storeState.NC.campaign,
          campaign: {
            ...storeState.NC.campaign.campaign,
            isTestDriveCampaign: true
          }
        }
      }
    };
    storeCopy = mockStore(testStoreState);
    FormWrapper = shallowWithStore(<FormScreen />, storeCopy).dive();
    FormWrapper.setState({ testDriveChecked: true });
    let testDriveDisclaimerWrapper = FormWrapper.find(
      '[className="book-test-drive-disclaimer"]'
    );
    expect(testDriveDisclaimerWrapper.exists()).toBeTruthy();
  });

  test("Hide test drive disclaimer - test drive not checked", () => {
    let testStoreState = {
      ...storeState,
      NC: {
        ...storeState.NC,
        campaign: {
          ...storeState.NC.campaign,
          campaign: {
            ...storeState.NC.campaign.campaign,
            isTestDriveCampaign: true
          }
        }
      }
    };
    storeCopy = mockStore(testStoreState);
    FormWrapper = shallowWithStore(<FormScreen />, storeCopy).dive();
    let testDriveDisclaimerWrapper = FormWrapper.find(
      '[className="book-test-drive-disclaimer"]'
    );
    expect(testDriveDisclaimerWrapper.exists()).toBeFalsy();
  });
  test("Submit button enabled - button not clicked", () => {
    let submitWrapper = FormWrapper.find('[type="primary"]');
    expect(submitWrapper.props().disabled).toBeFalsy();
  });
  test("Submit button disabled - button clicked", () => {
    FormWrapper.setState({ isDisabled: true });
    let submitWrapper = FormWrapper.find(Button);
    // submitWrapper.simulate("click");
    expect(submitWrapper.dive().text()).toContain("Processing...");
    expect(submitWrapper.props().disabled).toBeTruthy();
    expect(FormWrapper.state().isDisabled).toBeTruthy();
  });
});

describe("Testing Form screen container - Interaction events", () => {
  test("Validate name - name changed", () => {
    let nameWrapper = FormWrapper.find('[label="Name"]');
    nameWrapper.simulate("blur", { target: { value: "test" } });
    expect(Validate.validateName).toHaveBeenCalled();
  });

  test("Set validation - name changed", () => {
    let nameWrapper = FormWrapper.find('[label="Name"]');
    nameWrapper.simulate("blur", { target: { value: "test" } });
    expect(FormWrapper.state().validation.name).toEqual({
      isValid: true,
      errMessage: ""
    });
  });

  test("Set name - name changed", () => {
    let nameWrapper = FormWrapper.find('[label="Name"]');
    nameWrapper.simulate("blur", { target: { value: "test" } });
    expect(FormWrapper.state().buyerInfo.name).toEqual("test");
  });

  test("Validate mobile - mobile changed", () => {
    let mobileWrapper = FormWrapper.find('[label="Contact Number"]');
    mobileWrapper.simulate("blur", { target: { value: "9999988888" } });
    expect(Validate.validateMobile).toHaveBeenCalled();
  });

  test("Set validation - mobile changed", () => {
    let mobileWrapper = FormWrapper.find('[label="Contact Number"]');
    mobileWrapper.simulate("blur", { target: { value: "9999988888" } });
    expect(FormWrapper.state().validation.mobile).toEqual({
      isValid: true,
      errMessage: ""
    });
  });

  test("Set mobile - mobile changed", () => {
    let mobileWrapper = FormWrapper.find('[label="Contact Number"]');
    mobileWrapper.simulate("blur", { target: { value: "9999988888" } });
    expect(FormWrapper.state().buyerInfo.mobile).toEqual("9999988888");
  });

  test("Validate email - email changed", () => {
    let testStoreState = {
      ...storeState,
      NC: {
        ...storeState.NC,
        campaign: {
          ...storeState.NC.campaign,
          campaign: {
            ...storeState.NC.campaign.campaign,
            isEmailRequired: true
          }
        }
      }
    };
    storeCopy = mockStore(testStoreState);
    FormWrapper = shallowWithStore(<FormScreen />, storeCopy).dive();
    let emailWrapper = FormWrapper.find('[label="Email-Id"]');
    emailWrapper.simulate("blur", { target: { value: "ab@cd.ef" } });
    expect(Validate.validateEmail).toHaveBeenCalled();
  });

  test("Set validation - email changed", () => {
    let testStoreState = {
      ...storeState,
      NC: {
        ...storeState.NC,
        campaign: {
          ...storeState.NC.campaign,
          campaign: {
            ...storeState.NC.campaign.campaign,
            isEmailRequired: true
          }
        }
      }
    };
    storeCopy = mockStore(testStoreState);
    FormWrapper = shallowWithStore(
      <FormScreen modelDetail={props.modelDetail} />,
      storeCopy
    ).dive();
    let emailWrapper = FormWrapper.find('[label="Email-Id"]');
    emailWrapper.simulate("blur", { target: { value: "ab@cd.ef" } });
    expect(FormWrapper.state().validation.email).toEqual({
      isValid: true,
      errMessage: ""
    });
  });

  test("Set email - email changed", () => {
    let testStoreState = {
      ...storeState,
      NC: {
        ...storeState.NC,
        campaign: {
          ...storeState.NC.campaign,
          campaign: {
            ...storeState.NC.campaign.campaign,
            isEmailRequired: true
          }
        }
      }
    };
    storeCopy = mockStore(testStoreState);
    FormWrapper = shallowWithStore(<FormScreen />, storeCopy).dive();
    let emailWrapper = FormWrapper.find('[label="Email-Id"]');
    emailWrapper.simulate("blur", { target: { value: "ab@cd.ef" } });
    expect(FormWrapper.state().buyerInfo.email).toEqual("ab@cd.ef");
  });

  test("Set assignedDealerId - city dropdown changed", () => {
    FormWrapper.setState({ showLocation: true, assignedDealerId: "23" });
    let cityItemWrapper = FormWrapper.find('[label="City"]');
    cityItemWrapper.simulate("change", { id: 1234 });
    expect(FormWrapper.state().assignedDealerId).toEqual("-1");
  });

  test("Clear dealerSelectItem - city dropdown change", () => {
    FormWrapper.setState({ showLocation: true, dealerSelectItems: [11, 22] });
    let cityItemWrapper = FormWrapper.find('[label="City"]');
    cityItemWrapper.simulate("change", { id: 1234 });
    expect(FormWrapper.state().dealerSelectItems).toEqual([]);
  });

  test("Reset showSeller - city dropdown change", () => {
    FormWrapper.setState({ showLocation: true, showSeller: true });
    let cityItemWrapper = FormWrapper.find('[label="City"]');
    cityItemWrapper.simulate("change", { id: 1234 });
    expect(FormWrapper.state().showSeller).toBeFalsy();
  });

  test("Set cityId - city dropdown change", () => {
    FormWrapper.setState({ showLocation: true, cityId: "-1" });
    let cityItemWrapper = FormWrapper.find('[label="City"]');
    cityItemWrapper.simulate("change", { id: 1234 });
    expect(FormWrapper.state().cityId).toEqual(1234);
  });

  test("Set cityName - city dropdown change", () => {
    FormWrapper.setState({ showLocation: true, cityName: "-1" });
    let cityItemWrapper = FormWrapper.find('[label="City"]');
    cityItemWrapper.simulate("change", { name: "Mumbai" });
    expect(FormWrapper.state().cityName).toEqual("Mumbai");
  });

  test("Set city validation - city dropdown change", () => {
    FormWrapper.setState({ showLocation: true });
    let cityItemWrapper = FormWrapper.find('[label="City"]');
    cityItemWrapper.simulate("change", { id: 1234 });
    expect(FormWrapper.state().validation.city).toEqual({
      isValid: true,
      errMessage: ""
    });
  });

  test("Set assignedDealerId- dealer dropdown change", () => {
    FormWrapper.setState({ showSeller: true, assignedDealerId: 1244 });
    let dealerItemWrapper = FormWrapper.find('[label="Dealership"]');
    dealerItemWrapper.simulate("change", { id: 1234 });
    expect(FormWrapper.state().assignedDealerId).toEqual(1234);
  });

  test("Set dealer validation - dealer dropdown change", () => {
    FormWrapper.setState({ showSeller: true });
    let dealerItemWrapper = FormWrapper.find('[label="Dealership"]');
    dealerItemWrapper.simulate("change", { id: 1234 });
    expect(FormWrapper.state().validation.dealer).toEqual({
      isValid: true,
      errMessage: ""
    });
  });
  test("Set isTestdriveChecked- test drive checkbox change", () => {
    let testStoreState = {
      ...storeState,
      NC: {
        ...storeState.NC,
        campaign: {
          ...storeState.NC.campaign,
          campaign: {
            ...storeState.NC.campaign.campaign,
            isTestDriveCampaign: true
          }
        }
      }
    };
    storeCopy = mockStore(testStoreState);
    FormWrapper = shallowWithStore(<FormScreen />, storeCopy).dive();
    let testDriveWrapper = FormWrapper.find(Checkbox);
    testDriveWrapper.simulate("change");
    expect(FormWrapper.state().testDriveChecked).toBeTruthy();
  });

  test("Validate name - button clicked", () => {
    FormWrapper.setState({
      buyerInfo: {
        ...FormWrapper.state().buyerInfo,
        name: "test"
      }
    });
    let submitWrapper = FormWrapper.find('[type="primary"]');
    submitWrapper.simulate("click");
    expect(Validate.validateName).toHaveBeenCalledWith("test", false);
  });

  test("Validate mobile - button clicked", () => {
    FormWrapper.setState({
      buyerInfo: {
        ...FormWrapper.state().buyerInfo,
        mobile: "9988776655"
      }
    });
    let submitWrapper = FormWrapper.find('[type="primary"]');
    submitWrapper.simulate("click");
    expect(Validate.validateMobile).toHaveBeenCalledWith("9988776655", false);
  });

  test("Validate email - button clicked", () => {
    let testStoreState = {
      ...storeState,
      NC: {
        ...storeState.NC,
        campaign: {
          ...storeState.NC.campaign,
          campaign: {
            ...storeState.NC.campaign.campaign,
            isEmailRequired: true
          }
        }
      }
    };
    storeCopy = mockStore(testStoreState);
    FormWrapper = shallowWithStore(
      <FormScreen modelDetail={props.modelDetail} />,
      storeCopy
    ).dive();
    FormWrapper.setState({
      buyerInfo: {
        ...FormWrapper.state().buyerInfo,
        email: "ab@cd.ef"
      }
    });
    let submitWrapper = FormWrapper.find('[type="primary"]');
    submitWrapper.simulate("click");
    expect(Validate.validateEmail).toHaveBeenCalledWith("ab@cd.ef", false);
  });

  test("Validate city - button clicked", () => {
    FormWrapper.setState({ showLocation: true, cityId: "-1" });
    let submitWrapper = FormWrapper.find('[type="primary"]');
    submitWrapper.simulate("click");
    expect(Validate.validateCity).toHaveBeenCalledWith(false);
  });

  test("Validate dealer - button clicked", () => {
    FormWrapper.setState({ showSeller: true, assignedDealerId: "-1" });
    let submitWrapper = FormWrapper.find('[type="primary"]');
    submitWrapper.simulate("click");
    expect(Validate.validateDealer).toHaveBeenCalledWith(false);
  });

  test("Validate dealer - button clicked", () => {
    FormWrapper.setState({ showSeller: true, assignedDealerId: "-1" });
    let submitWrapper = FormWrapper.find('[type="primary"]');
    submitWrapper.simulate("click");
    expect(Validate.validateDealer).toHaveBeenCalledWith(false);
  });

  test("Cancel city api request - button clicked", () => {
    FormWrapper.setState({ showSeller: false, showLocation: false });
    let promise = Promise.resolve();
    let submitWrapper = FormWrapper.find('[type="primary"]');
    FormWrapper.instance().cityAPIRequest = makeCancelable(promise);
    let spyCity = jest.spyOn(FormWrapper.instance().cityAPIRequest, "cancel");
    submitWrapper.simulate("click");
    expect(spyCity).toHaveBeenCalled();
  });

  test("Cancel dealer api request - button clicked", () => {
    FormWrapper.setState({ showSeller: false, showLocation: false });
    let promise = Promise.resolve();
    let submitWrapper = FormWrapper.find('[type="primary"]');
    FormWrapper.instance().dealerAPIRequest = makeCancelable(promise);
    let spyDealer = jest.spyOn(
      FormWrapper.instance().dealerAPIRequest,
      "cancel"
    );
    submitWrapper.simulate("click");
    expect(spyDealer).toHaveBeenCalled();
  });

  test("Set isDisabled - button clicked", () => {
    FormWrapper.setState({ showSeller: false, showLocation: false });
    let submitWrapper = FormWrapper.find('[type="primary"]');
    submitWrapper.simulate("click");
    expect(FormWrapper.state().isDisabled).toBeTruthy();
  });

  test("Set Form Lead - button clicked", () => {
    FormWrapper.setState({ showSeller: false, showLocation: false });
    let submitWrapper = FormWrapper.find('[type="primary"]');
    submitWrapper.simulate("click");
    const {
      buyerInfo,
      cityId,
      cityName,
      assignedDealerId,
      testDriveChecked,
      showLocation
    } = FormWrapper.state();
    const modelDetail = props.modelDetail;
    const formState = {
      buyerInfo,
      cityId,
      cityName,
      assignedDealerId,
      testDriveChecked,
      modelDetail,
      showLocation
    };
    expect(FormState.setFormLead).toHaveBeenCalledWith(formState);
  });

  test("Set City Change flag - button clicked", () => {
    FormWrapper.setState({ showSeller: false, showLocation: true });
    let submitWrapper = FormWrapper.find('[type="primary"]');
    submitWrapper.simulate("click");
    expect(setCityChange).toHaveBeenCalledWith(true);
  });

  test("Track Lead Conversion - button clicked", () => {
    FormWrapper.setState({ showSeller: false, showLocation: true });
    let submitWrapper = FormWrapper.find('[type="primary"]');
    submitWrapper.simulate("click");
    expect(window.leadConversionTracking.track).toHaveBeenCalledWith(1, 12);
  });

  test("Validation changed - button clicked", () => {
    let submitWrapper = FormWrapper.find('[type="primary"]');
    let validation = FormWrapper.state().validation;
    FormWrapper.setState({
      validation: {
        ...FormWrapper.state().validation,
        name: {
          isValid: false,
          errMessage: "Please enter name"
        }
      }
    });
    submitWrapper.simulate("click");
    expect(FormWrapper.state().validation).not.toEqual(validation);
  });
});

describe("Testing Form screen container - componentDidMount", () => {
  test("Get dealers list - form screen mounted with lead panel 1/3", () => {
    let testStoreState = {
      ...storeState,
      location: {
        cityId: 10
      }
    };
    storeCopy = mockStore(testStoreState);
    FormWrapper = shallowWithStore(<FormScreen />, storeCopy).dive();
    expect(dealersApi.get).toHaveBeenCalled();
  });

  test("Set dealerAPIRequest - form screen mounted with lead panel 1/3", () => {
    let testStoreState = {
      ...storeState,
      location: {
        cityId: 10
      }
    };
    storeCopy = mockStore(testStoreState);
    let FormWrapperTest = shallowWithStore(
      <FormScreen modelDetail={props.modelDetail} />,
      storeCopy
    ).dive();
    process.nextTick(() => {
      // try {
      expect(FormWrapperTest.instance().dealerAPIRequest).toBeNull();
      // } catch (e) {
      //   console.log(e);
      // }
    });
  });

  test("Set dealerSelectItems (dealerList>1) - form screen mounted with lead panel 1/3", () => {
    let testStoreState = {
      ...storeState,
      location: {
        cityId: 10
      }
    };
    storeCopy = mockStore(testStoreState);
    let FormWrapperTest = shallowWithStore(<FormScreen />, storeCopy).dive();
    process.nextTick(() => {
      // try {
      expect(FormWrapperTest.state().dealerSelectItems).toHaveLength(3);
      // } catch (e) {
      //   console.log(e);
      // }
    });
  });

  test("Set showSeller (dealerList>1) - form screen mounted with lead panel 1/3", () => {
    let testStoreState = {
      ...storeState,
      location: {
        cityId: 10
      }
    };
    storeCopy = mockStore(testStoreState);
    let FormWrapperTest = shallowWithStore(<FormScreen />, storeCopy).dive();
    process.nextTick(() => {
      expect(FormWrapperTest.state().showSeller).toBeTruthy();
    });
  });

  test("Set dealerSelectItems (dealerList=1) - form screen mounted with lead panel 1/3", () => {
    dealersApi.get.mockClear();
    dealersApi.get = jest
      .fn()
      .mockImplementation(() => Promise.resolve(dealerResponse2));
    let testStoreState = {
      ...storeState,
      location: {
        cityId: 10
      }
    };
    storeCopy = mockStore(testStoreState);
    let FormWrapperTest = shallowWithStore(<FormScreen />, storeCopy).dive();
    process.nextTick(() => {
      expect(FormWrapperTest.state().dealerSelectItems).toHaveLength(1);
    });
  });

  test("Set showSeller (dealerList=1) - form screen mounted with lead panel 1/3", () => {
    dealersApi.get.mockClear();
    dealersApi.get = jest.fn(() => Promise.resolve(dealerResponse2));
    let testStoreState = {
      ...storeState,
      location: {
        cityId: 10
      }
    };
    storeCopy = mockStore(testStoreState);
    let FormWrapperTest = shallowWithStore(<FormScreen />, storeCopy).dive();
    process.nextTick(() => {
      expect(FormWrapperTest.state().showSeller).toBeFalsy();
    });
  });

  test("Set assignedDealerId (dealerList=1) - form screen mounted with lead panel 1/3", () => {
    dealersApi.get.mockClear();
    dealersApi.get = jest.fn(() => Promise.resolve(dealerResponse2));
    let testStoreState = {
      ...storeState,
      location: {
        cityId: 10
      }
    };
    storeCopy = mockStore(testStoreState);
    let FormWrapperTest = shallowWithStore(<FormScreen />, storeCopy).dive();
    process.nextTick(() => {
      expect(FormWrapperTest.state().assignedDealerId).toEqual(18541);
    });
  });

  test("Set dealerSelectItems (dealerList=null) - form screen mounted with lead panel 1/3", () => {
    dealersApi.get.mockClear();
    dealersApi.get = jest.fn(() => Promise.resolve(null));
    let testStoreState = {
      ...storeState,
      location: {
        cityId: 10
      }
    };
    storeCopy = mockStore(testStoreState);
    let FormWrapperTest = shallowWithStore(<FormScreen />, storeCopy).dive();
    process.nextTick(() => {
      expect(FormWrapperTest.state().dealerSelectItems).toHaveLength(0);
    });
  });

  test("Set assignedDealerId (dealerList=null) - form screen mounted with lead panel 1/3", () => {
    dealersApi.get.mockClear();
    dealersApi.get = jest.fn(() => Promise.resolve(null));
    let testStoreState = {
      ...storeState,
      location: {
        cityId: 10
      }
    };
    storeCopy = mockStore(testStoreState);
    let FormWrapperTest = shallowWithStore(<FormScreen />, storeCopy).dive();
    process.nextTick(() => {
      expect(FormWrapperTest.state().assignedDealerId).toEqual("-1");
    });
  });

  test("Set log executed - form screen mounted with lead panel 1/3", () => {
    dealersApi.get.mockClear();
    dealersApi.get = jest.fn(() => Promise.resolve(dealerApiResponse));
    let testStoreState = {
      ...storeState,
      location: {
        cityId: 10
      }
    };
    storeCopy = mockStore(testStoreState);
    let FormWrapperTest = shallowWithStore(<FormScreen />, storeCopy).dive();
    process.nextTick(() => {
      // try {
      expect(LogState.setInfoLog).toHaveBeenCalledWith(
        "Fetched Seller data from API",
        1
      );
      // } catch (e) {
      //   console.log(e);
      // }
    });
  });

  test("Set log executed (dealer Api failed) - form screen mounted with lead panel 1/3", () => {
    dealersApi.get.mockClear();
    dealersApi.get = jest.fn(() => Promise.reject("Dealer Api failed"));
    let testStoreState = {
      ...storeState,
      location: {
        cityId: 10
      }
    };
    storeCopy = mockStore(testStoreState);
    let FormWrapperTest = shallowWithStore(<FormScreen />, storeCopy).dive();
    process.nextTick(() => {
      // try {
      expect(LogState.setErrorLog).toHaveBeenCalledWith(
        `Error: Dealer Api failed. Found in Seller API`,
        1
      );
      // } catch (e) {
      //   console.log(e);
      // }
    });
  });

  test("Get MLA data (api call executed) - form screen mounted with mutual leads set", () => {
    let testStoreState = {
      ...storeState,
      NC: {
        ...storeState.NC,
        campaign: {
          ...storeState.NC.campaign,
          campaign: {
            ...storeState.NC.campaign.campaign,
            mutualLeads: true
          }
        }
      }
    };
    storeCopy = mockStore(testStoreState);
    let FormWrapperTest = shallowWithStore(<FormScreen />, storeCopy).dive();
    expect(MLAApi.get).toHaveBeenCalled();
  });

  test("Set MLAAPIRequest (api call executed) - form screen mounted with mutual leads set", () => {
    let testStoreState = {
      ...storeState,
      NC: {
        ...storeState.NC,
        campaign: {
          ...storeState.NC.campaign,
          campaign: {
            ...storeState.NC.campaign.campaign,
            mutualLeads: true
          }
        }
      }
    };

    storeCopy = mockStore(testStoreState);
    let FormWrapperTest = shallowWithStore(<FormScreen />, storeCopy).dive();
    process.nextTick(() => {
      expect(FormWrapperTest.instance().MLAAPIRequest).toBeNull();
    });
  });

  test("Set Log (api call executed) - form screen mounted with mutual leads set", () => {
    let testStoreState = {
      ...storeState,
      NC: {
        ...storeState.NC,
        campaign: {
          ...storeState.NC.campaign,
          campaign: {
            ...storeState.NC.campaign.campaign,
            mutualLeads: true
          }
        }
      }
    };

    storeCopy = mockStore(testStoreState);
    let FormWrapperTest = shallowWithStore(<FormScreen />, storeCopy).dive();
    process.nextTick(() => {
      // try {
      expect(LogState.setInfoLog).toHaveBeenCalledWith(
        "Fetched MLA Data from API",
        1
      );
      // } catch (e) {
      //   console.log(e);
      // }
    });
  });

  test("Get mapped object for store (api call executed) - form screen mounted with mutual leads set", () => {
    let testStoreState = {
      ...storeState,
      NC: {
        ...storeState.NC,
        campaign: {
          ...storeState.NC.campaign,
          campaign: {
            ...storeState.NC.campaign.campaign,
            mutualLeads: true
          }
        }
      }
    };

    storeCopy = mockStore(testStoreState);
    let FormWrapperTest = shallowWithStore(<FormScreen />, storeCopy).dive();
    ObjectMap.MLAMappingApiToStore = jest.fn();
    process.nextTick(() => {
      expect(ObjectMap.MLAMappingApiToStore).toHaveBeenCalled();
    });
  });

  test("Set MLA lead (api call executed) - form screen mounted with mutual leads set", () => {
    let testStoreState = {
      ...storeState,
      NC: {
        ...storeState.NC,
        campaign: {
          ...storeState.NC.campaign,
          campaign: {
            ...storeState.NC.campaign.campaign,
            mutualLeads: true
          }
        }
      }
    };

    storeCopy = mockStore(testStoreState);
    let FormWrapperTest = shallowWithStore(<FormScreen />, storeCopy).dive();
    process.nextTick(() => {
      expect(ScreenState.setMLAData).toHaveBeenCalled();
    });
  });

  test("Set log (api call failure) - form screen mounted with mutual leads set", () => {
    MLAApi.get.mockClear();
    MLAApi.get = jest.fn(() => Promise.reject("MLAApi failed"));
    let testStoreState = {
      ...storeState,
      NC: {
        ...storeState.NC,
        campaign: {
          ...storeState.NC.campaign,
          campaign: {
            ...storeState.NC.campaign.campaign,
            mutualLeads: true
          }
        }
      }
    };

    storeCopy = mockStore(testStoreState);
    let FormWrapperTest = shallowWithStore(<FormScreen />, storeCopy).dive();
    process.nextTick(() => {
      // try {
      expect(LogState.setErrorLog).toHaveBeenCalledWith(
        "Error: MLAApi failed. Found in MLA API",
        1
      );
      // } catch (e) {
      //   console.log(e);
      // }
    });
  });

  test("Get cities list (api call executed) - form screen mounted with mutual leads set", () => {
    process.nextTick(() => {
      expect(citiesApi.get).toHaveBeenCalled();
    });
  });

  test("Set cityApiRequest (api call executed) - form screen mounted with mutual leads set", () => {
    process.nextTick(() => {
      expect(FormWrapper.instance().cityAPIRequest).toBeNull();
    });
  });

  test("Set cityApiRequest (api call executed) - form screen mounted with mutual leads set", () => {
    process.nextTick(() => {
      expect(FormWrapper.instance().cityAPIRequest).toBeNull();
    });
  });

  test("Set citySelectItems (api call executed) - form screen mounted with mutual leads set", () => {
    process.nextTick(() => {
      expect(FormWrapper.state().citySelectItems).toHaveLength(3);
    });
  });

  test("Set citySelectItems (api call executed) - form screen mounted with mutual leads set", () => {
    process.nextTick(() => {
      expect(FormWrapper.state().citySelectItems).toHaveLength(3);
    });
  });

  test("Set showLocation (api call executed) - form screen mounted with mutual leads set", () => {
    process.nextTick(() => {
      expect(FormWrapper.state().showLocation).toBeTruthy();
    });
  });

  test("Set citySelectItems (api call executed but data is null) - form screen mounted with mutual leads set", () => {
    citiesApi.get.mockClear();
    citiesApi.get = jest.fn(() => Promise.resolve(null));
    let FormWrapperTest = shallowWithStore(<FormScreen />, storeCopy).dive();
    process.nextTick(() => {
      expect(FormWrapperTest.state().citySelectItems).toHaveLength(0);
    });
  });

  test("Set showLocation (api call executed but data is null) - form screen mounted with mutual leads set", () => {
    citiesApi.get.mockClear();
    citiesApi.get = jest.fn(() => Promise.resolve(null));
    let FormWrapperTest = shallowWithStore(<FormScreen />, storeCopy).dive();
    process.nextTick(() => {
      expect(FormWrapperTest.state().showLocation).toBeFalsy();
    });
  });

  test("Set log (api call executed) - form screen mounted with mutual leads set", () => {
    process.nextTick(() => {
      // try {
      expect(LogState.setInfoLog).toHaveBeenCalledWith(
        "Fetched Location data from API",
        1
      );
      // } catch (e) {
      //   console.log(e);
      // }
    });
  });

  test("Set log (api call executed) - form screen mounted with mutual leads set", () => {
    citiesApi.get.mockClear();
    citiesApi.get = jest.fn(() => Promise.reject("Location Api failed"));
    FormWrapper = shallowWithStore(<FormScreen />, storeCopy).dive();
    process.nextTick(() => {
      // try {
      expect(LogState.setErrorLog).toHaveBeenCalledWith(
        "Error: Location Api failed. Found in Location API",
        1
      );
      // } catch (e) {
      //   console.log(e);
      // }
    });
  });
});

describe("Testing Form screen container - shouldCompenentUpdate", () => {
  test("Reset Error States - LeadForm Visibilty changed", () => {
    FormWrapper.setState({
      validation: {
        name: {
          isValid: false,
          errMessage: ""
        },
        mobile: {
          isValid: false,
          errMessage: ""
        },
        city: {
          isValid: false,
          errMessage: ""
        },
        dealer: {
          isValid: false,
          errMessage: ""
        },
        email: {
          isValid: false,
          errMessage: ""
        }
      }
    });
    FormWrapper.setProps({ isLeadFormVisible: false });
    let validation = {
      name: {
        isValid: true,
        errMessage: ""
      },
      mobile: {
        isValid: true,
        errMessage: ""
      },
      city: {
        isValid: true,
        errMessage: ""
      },
      dealer: {
        isValid: true,
        errMessage: ""
      },
      email: {
        isValid: true,
        errMessage: ""
      }
    };
    expect(FormWrapper.state().validation).toEqual(validation);
  });

  test("'Name or Mobile Changed' Logged - Name or Mobile changed", () => {
    let currentName = FormWrapper.state().buyerInfo.name;
    FormWrapper.setState({
      buyerInfo: {
        ...FormWrapper.state().buyerInfo,
        name: "test"
      }
    });
    expect(LogState.setDebugLog.mock.calls[0][0]).toBe(
      "Name or Mobile Changed"
    );
  });

  test("'City Changed' Logged - City Dropdown changed", () => {
    LogState.setDebugLog.mockClear();
    let currentCity = FormWrapper.state().cityId;
    FormWrapper.setState({
      cityId: 10
    });
    expect(LogState.setDebugLog.mock.calls[0][0]).toBe("CityId Changed");
  });

  test("'AssignedDealerId Changed' Logged - Dealer Dropdown changed/selected city has only 1 dealer", () => {
    LogState.setDebugLog.mockClear();
    let currentDealer = FormWrapper.state().assignedDealerId;
    FormWrapper.setState({
      assignedDealerId: 10
    });
    expect(LogState.setDebugLog.mock.calls[0][0]).toBe(
      "AssignedDealerId Changed"
    );
  });

  test("Cancel the previous dealerApiRequest - City Changed", () => {
    let promise = Promise.resolve();
    FormWrapper.instance().dealerAPIRequest = makeCancelable(promise);
    let spy = jest.spyOn(FormWrapper.instance().dealerAPIRequest, "cancel");
    FormWrapper.setState({
      showLocation: true,
      cityId: 10
    });
    expect(spy).toHaveBeenCalled();
  });

  test("'Fetch Sellers' Logged - City Changed", () => {
    FormWrapper.setState({
      showLocation: true,
      cityId: 10
    });
    process.nextTick(() => {
      // try {
      expect(LogState.setInfoLog).toHaveBeenCalledWith(
        "Fetched Seller data from API when city selected",
        1
      );
      // } catch (e) {
      //   console.log(e);
      // }
    });
  });

  test("DealerApiRequest reset - City Changed, promise resolved", () => {
    let FormWrapperTest = shallowWithStore(<FormScreen />, storeCopy).dive();
    FormWrapperTest.setState({
      showLocation: true
    });
    FormWrapperTest.setState({
      cityId: 121
    });
    //expect(dealersApi.get).toHaveBeenCalled();
    process.nextTick(() => {
      // try {
      expect(FormWrapperTest.instance().dealerAPIRequest).toBeNull();
      // } catch (e) {
      //   console.log(e);
      // }
    });
  });

  test("Dealer Dropdown shown  - City Changed, >1 Dealers received", () => {
    let FormWrapperTest = shallowWithStore(<FormScreen />, storeCopy).dive();
    FormWrapperTest.setState({
      showLocation: true
    });
    FormWrapperTest.setState({
      cityId: 121
    });
    process.nextTick(() => {
      expect(FormWrapperTest.state().showSeller).toBeTruthy();
    });
  });

  test("assignedDealerId reset  - City Changed, >1 Dealers received", () => {
    let FormWrapperTest = shallowWithStore(<FormScreen />, storeCopy).dive();
    FormWrapperTest.setState({
      showLocation: true
    });
    FormWrapperTest.setState({
      cityId: 121
    });
    process.nextTick(() => {
      expect(FormWrapperTest.state().assignedDealerId).toEqual("-1");
    });
  });

  test("Dealers List set in localstate  - City Changed, >1 Dealers received", () => {
    let FormWrapperTest = shallowWithStore(<FormScreen />, storeCopy).dive();
    FormWrapperTest.setState({
      showLocation: true
    });
    FormWrapperTest.setState({
      cityId: 121
    });
    process.nextTick(() => {
      expect(FormWrapperTest.state().dealerSelectItems).toHaveLength(3);
    });
  });

  test("Dealer List set in localstate  - City Changed, 1 Dealer received", () => {
    dealersApi.get.mockClear();
    dealersApi.get = jest.fn(() => Promise.resolve(dealerResponse2));
    let FormWrapperTest = shallowWithStore(<FormScreen />, storeCopy).dive();
    FormWrapperTest.setState({
      showLocation: true
    });
    FormWrapperTest.setState({
      cityId: 121
    });
    process.nextTick(() => {
      expect(FormWrapperTest.state().dealerSelectItems).toHaveLength(1);
    });
  });

  test("Dealer Dropdown Hide  - City Changed, 1 Dealer received", () => {
    dealersApi.get.mockClear();
    dealersApi.get = jest.fn(() => Promise.resolve(dealerResponse2));
    let FormWrapperTest = shallowWithStore(<FormScreen />, storeCopy).dive();
    FormWrapperTest.setState({
      showLocation: true
    });
    FormWrapperTest.setState({
      cityId: 121
    });
    process.nextTick(() => {
      expect(FormWrapperTest.state().showSeller).toBeFalsy();
    });
  });
  test("assignedDealerId set in localstate  - City Changed, 1 Dealer received", () => {
    dealersApi.get.mockClear();
    dealersApi.get = jest.fn(() => Promise.resolve(dealerResponse2));
    let FormWrapperTest = shallowWithStore(<FormScreen />, storeCopy).dive();
    FormWrapperTest.setState({
      showLocation: true
    });
    FormWrapperTest.setState({
      cityId: 121
    });
    process.nextTick(() => {
      expect(FormWrapperTest.state().assignedDealerId).toEqual(18541);
    });
  });

  test("Error Logged  - Error in Dealer Api call bcoz previous DealerApiRequest get cancelled", () => {
    dealersApi.get.mockClear();
    dealersApi.get = jest.fn(() => Promise.reject({ isCanceled: true }));
    let FormWrapperTest = shallowWithStore(<FormScreen />, storeCopy).dive();
    FormWrapperTest.setState({
      showLocation: true
    });
    FormWrapperTest.setState({
      cityId: 121
    });
    process.nextTick(() => {
      // try {
      expect(LogState.setErrorLog).toHaveBeenCalledWith(
        "Request canceled for cityId 121",
        1
      );
      // } catch (e) {
      //   console.log(e);
      // }
    });
  });

  test("Error Logged  - previous DealerApiRequest not cancelled but Error in Dealer Api call", () => {
    dealersApi.get.mockClear();
    dealersApi.get = jest.fn(() => Promise.reject("Dealer Api failed"));
    let FormWrapperTest = shallowWithStore(<FormScreen />, storeCopy).dive();
    FormWrapperTest.setState({
      showLocation: true
    });
    FormWrapperTest.setState({
      cityId: 121
    });
    process.nextTick(() => {
      // try {
      expect(LogState.setErrorLog).toHaveBeenCalledWith(
        "Error: Dealer Api failed. Found in Seller API when city selected",
        1
      );
      // } catch (e) {
      //   console.log(e);
      // }
    });
  });

  test("'Updated Campaign shown' Logged  - Campaign changed/ Same Campaign but Dealer changed", () => {
    FormWrapper.setProps({
      campaign: {
        ...storeState.NC.campaign,
        campaign: {
          ...storeState.NC.campaign.campaign,
          dealerId: 890
        }
      }
    });
    expect(LogState.setInfoLog).toHaveBeenCalledWith(
      "Updated Campaign shown (Current Store data Provided in currentState)"
    );
  });

  test("'City dropdown shown' Logged  - City Dropdown shown", () => {
    FormWrapper.setState({ showLocation: true });
    expect(LogState.setInfoLog).toHaveBeenCalledWith(
      "City dropdown shown (Current Store data Provided in currentState)"
    );
  });

  test("'dealer dropdown shown' Logged  - dealer dropdown shown", () => {
    FormWrapper.setState({ showSeller: true });
    console.log(FormWrapper.state().showSeller);
    expect(LogState.setInfoLog).toHaveBeenCalledWith(
      "Dealer dropdown shown (Current Store data Provided in currentState)"
    );
  });

  test("'Email input shown' Logged  - Campaign with mandatory email", () => {
    FormWrapper.setProps({
      campaign: {
        ...storeState.NC.campaign,
        campaign: {
          ...storeState.NC.campaign.campaign,
          isEmailRequired: true
        }
      }
    });
    expect(LogState.setInfoLog).toHaveBeenCalledWith(
      "Email input shown (Current Store data Provided in currentState)"
    );
  });
});

//TODO
describe("Testing Form screen container - componentDidUpdate", () => {});

describe("Testing Form screen container - Tracking events", () => {
  test("Error message shown - name not valid", () => {});
  test("Name focused - name input clicked", () => {});
  test("Error message shown - mobile not valid", () => {});
  test("Mobile focused - mobile input clicked", () => {});
  test("Error message shown - email not valid", () => {});
  test("Email focused - email input clicked", () => {});
});
