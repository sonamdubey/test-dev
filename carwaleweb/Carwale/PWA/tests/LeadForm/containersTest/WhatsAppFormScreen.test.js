import React from "react";
import * as Validate from "../../../src/LeadForm/utils/Validate";
import * as WhatsAppFormState from "../../../src/LeadForm/ActionCreators/WhatsAppFormState";
import * as LogState from "../../../src/LeadForm/ActionCreators/LogState";
import store from "../../../src/LeadForm/store";
import FormScreenHeader from "../../../src/LeadForm/Components/FormScreenHeader";
import WhatsAppFormScreen from "../../../src/LeadForm/Containers/WhatsAppFormScreen";

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
jest.mock("../../../src/LeadForm/utils/Validate", () => {
  return {
    validateMobile: jest.fn(() => {
      return {
        isValid: true,
        errMessage: ""
      };
    }),
    validateName: jest.fn(() => {
      return {
        isValid: true,
        errMessage: ""
      };
    })
  };
});

jest.mock("../../../src/LeadForm/ActionCreators/LogState", () => {
  return {
    setInfoLog: jest.fn(() => {
      return {
        type: ""
      };
    }),
    setDebugLog: jest.fn(() => {
      return {
        type: ""
      };
    })
  };
});

jest.mock("../../../src/LeadForm/Components/FormScreenHeader");

jest.mock("../../../src/LeadForm/ActionCreators/WhatsAppFormState", () => {
  return {
    setWhatsAppFormLead: jest.fn(() => {
      return {
        type: ""
      };
    })
  };
});

jest.mock("../../../src/LeadForm/utils/Tracking", () => {
  return {
    trackNameErr: jest.fn(),
    trackMobileErr: jest.fn()
  };
});

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
    cityId: 0
  },
  isLeadFormVisible: true,
  interactionId: 1
};

window.leadConversionTracking = {
  track: jest.fn()
};

beforeEach(() => {
  //mocking container
  FormWrapper = shallowWithStore(
    <WhatsAppFormScreen modelDetail={props.modelDetail} />,
    store
  ).dive();
});

describe("Testing WhatsApp Form screen container - DOM", () => {
  test("Show Form Screen header - isCrossSell flag not set", () => {
    let formScreenHeader = FormWrapper.find(FormScreenHeader);
    expect(formScreenHeader.props().isCrossSell).toBeFalsy();
    expect(formScreenHeader.props().campaignName).toEqual("Bahubali");
  });
});

describe("Testing WhatsApp Form screen container - Interaction events", () => {
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

  test("Validate name - button clicked", () => {
    FormWrapper.setState({
      buyerInfo: {
        ...FormWrapper.state().buyerInfo,
        name: "test"
      }
    });
    let submitWrapper = FormWrapper.find('[type="secondary"]');
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
    let submitWrapper = FormWrapper.find('[type="secondary"]');
    submitWrapper.simulate("click");
    expect(Validate.validateMobile).toHaveBeenCalledWith("9988776655", false);
  });

  test("Set WhatsApp Form Lead - button clicked", () => {
    let submitWrapper = FormWrapper.find('[type="secondary"]');
    submitWrapper.simulate("click");
    const { buyerInfo } = FormWrapper.state();
    const modelDetail = props.modelDetail;
    const formState = {
      buyerInfo,
      modelDetail
    };
    expect(WhatsAppFormState.setWhatsAppFormLead).toHaveBeenCalledWith(
      formState
    );
  });

  test("Track Lead Conversion - button clicked", () => {
    let submitWrapper = FormWrapper.find('[type="secondary"]');
    submitWrapper.simulate("click");
    expect(window.leadConversionTracking.track).toHaveBeenCalledWith(1, 12);
  });

  test("Validation changed - button clicked", () => {
    let submitWrapper = FormWrapper.find('[type="secondary"]');
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
      }
    };
    expect(FormWrapper.state().validation).toEqual(validation);
  });

  test("'Name or Mobile Changed' Logged - Name or Mobile changed", () => {
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
});
