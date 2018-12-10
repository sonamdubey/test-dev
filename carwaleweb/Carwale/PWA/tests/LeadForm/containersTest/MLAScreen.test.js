import React from "react";
import MLAScreen from "../../../src/LeadForm/Containers/MLAScreen";
import * as Validate from "../../../src/LeadForm/utils/Validate";
import Checkbox from "oxygen/lib/Checkbox/Checkbox";
import * as ScreenHeight from "../../../src/LeadForm/utils/ScreenHeight";
import * as MLAState from "../../../src/LeadForm/ActionCreators/MLAState";
import * as ScreenState from "../../../src/LeadForm/ActionCreators/ScreenState";

//mocking Validate module
jest.mock("../../../src/LeadForm/utils/Validate");

jest.mock("../../../src/LeadForm/utils/Tracking", () => {
  return {
    trackSubmit: jest.fn(),
    trackCheckItem: jest.fn(),
    trackSelectAll: jest.fn(),
    trackScreenShown: jest.fn(),
    trackSkipClick: jest.fn(),
    trackSkipShown: jest.fn()
  };
});

//mocking ScreenState module
jest.mock("../../../src/LeadForm/ActionCreators/ScreenState", () => {
  return {
    setRecoScreen: jest.fn(() => {
      return {
        type: ""
      };
    })
  };
});

jest.mock("../../../src/LeadForm/ActionCreators/MLAState", () => {
  return {
    setMLALead: jest.fn(() => {
      return {
        type: ""
      };
    })
  };
});

let MLAwrapper;
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
        list: {
          "9988-7654": {
            seller: {
              name: "",
              area: "",
              distance: -1
            }
          },
          "7788-3456": {
            seller: {
              name: "",
              area: "",
              distance: -1
            }
          }
        }
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
  }
};
let store = mockStore(storeState);

//mocking screenheight module
ScreenHeight.setScreenBodyHeight = jest.fn(() => {});

beforeEach(() => {
  // mocking validatButton function
  Validate.validateButton = jest.fn().mockImplementation((optional, length) => {
    return false;
  });

  //mocking container
  MLAwrapper = shallowWithStore(<MLAScreen />, store).dive();
});

describe("Testing MLA screen container - DOM", () => {
  test("Show Remove All toggle - all options selected", () => {
    MLAwrapper.setState({ selectAll: true, selectedMLA: ["11123-890"] });
    let checkboxWrapper = MLAwrapper.find(Checkbox);
    expect(checkboxWrapper.dive().text()).toContain("Remove All");
  });

  test("Show Select All toggle - all options not selected", () => {
    let checkboxWrapper = MLAwrapper.find(Checkbox);
    expect(checkboxWrapper.dive().text()).toContain("Select All");
  });

  test("Show Skip button - Recommendations available", () => {
    let testStoreState = {
      ...storeState,
      NC: {
        leadForm: {
          ...storeState.NC.leadForm,
          recommendation: {
            list: { "9988776655": { id: 12345 } }
          }
        },
        campaign: {
          ...storeState.NC.campaign
        }
      }
    };
    let storeCopy = mockStore(testStoreState);
    MLAwrapper = shallowWithStore(<MLAScreen />, storeCopy).dive();

    let skipWrapper = MLAwrapper.find('[className="dealer-skip-button"]');
    expect(skipWrapper.exists()).toBeTruthy();
  });

  test("Hide Skip button - Recommendations not available", () => {
    let skipWrapper = MLAwrapper.find('[className="dealer-skip-button"]');
    expect(skipWrapper.exists()).toBeFalsy();
  });

  test("Submit button disabled - no options selected", () => {
    Validate.validateButton.mockClear();
    Validate.validateButton = jest.fn(() => {
      return true;
    });
    MLAwrapper = shallowWithStore(<MLAScreen />, store).dive();
    let submitWrapper = MLAwrapper.find('[type="primary"]');
    expect(Validate.validateButton).toHaveBeenCalledWith(false, 0);
    expect(submitWrapper.props().disabled).toBeTruthy();
  });

  test("Submit button enabled - any options selected", () => {
    let submitWrapper = MLAwrapper.find('[type="primary"]');
    MLAwrapper.setState({ selectedMLA: ["9999-9999", "8888-8888"] });
    expect(Validate.validateButton).toHaveBeenCalledWith(false, 2);
    expect(submitWrapper.props().disabled).toBeFalsy();
  });

  test("All MLA checkboxes unchecked - turboMLA not set in campaign", () => {
    expect(MLAwrapper.state().selectedMLA).toHaveLength(0);
  });

  test("All MLA checkboxes checked - turboMLA set in campaign", () => {
    let testStoreState = {
      ...storeState,
      NC: {
        leadForm: {
          ...storeState.NC.leadForm
        },
        campaign: {
          ...storeState.NC.campaign,
          campaign: {
            ...storeState.NC.campaign.campaign,
            isTurboMla: true
          }
        }
      }
    };
    let storeCopy = mockStore(testStoreState);
    MLAwrapper = shallowWithStore(<MLAScreen />, storeCopy).dive();
    expect(MLAwrapper.state().selectAll).toBeTruthy();
  });
});

describe("Testing MLA screen container - Interaction Events", () => {
  test("Toggle select all - select all changed", () => {
    let checkboxWrapper = MLAwrapper.find(Checkbox);
    let initialState = MLAwrapper.state().selectAll;
    checkboxWrapper.simulate("change");
    expect(MLAwrapper.state().selectAll).not.toEqual(initialState);
  });

  test("Set Selected MLA keys - currently deselected, select all changed", () => {
    let checkboxWrapper = MLAwrapper.find(Checkbox);
    checkboxWrapper.simulate("change");
    expect(MLAwrapper.state().selectedMLA).toHaveLength(2);
  });

  test("Toggle select all - last deselected checkbox checked", () => {
    let checkboxGroupWrapper = MLAwrapper.find('[name="Other Dealers"]');
    checkboxGroupWrapper.simulate("change", ["9988-7654"]);
    expect(MLAwrapper.state().selectAll).toBeFalsy();

    checkboxGroupWrapper.simulate("change", ["9988-7654", "7788-3456"]);
    expect(MLAwrapper.state().selectAll).toBeTruthy();
  });

  test("Toggle select all - all checkboxes selected, checkbox changed", () => {
    let checkboxGroupWrapper = MLAwrapper.find('[name="Other Dealers"]');
    checkboxGroupWrapper.simulate("change", ["9988-7654", "7788-3456"]);
    expect(MLAwrapper.state().selectAll).toBeTruthy();

    checkboxGroupWrapper.simulate("change", ["9988-7654"]);
    expect(MLAwrapper.state().selectAll).toBeFalsy();
  });

  test("Set currently selected checkboxes - checkbox changed", () => {
    let checkboxGroupWrapper = MLAwrapper.find('[name="Other Dealers"]');
    checkboxGroupWrapper.simulate("change", ["9988-7654", "7788-3456"]);
    expect(MLAwrapper.state().selectedMLA).toHaveLength(2);

    checkboxGroupWrapper.simulate("change", ["9988-7654"]);
    expect(MLAwrapper.state().selectedMLA).toHaveLength(1);
  });

  test("Set MLA lead - submit button clicked", () => {
    let submitWrapper = MLAwrapper.find('[type="primary"]');
    submitWrapper.simulate("click");
    expect(MLAState.setMLALead).toHaveBeenCalled();
  });

  test("Set Reco screen - skip button clicked", () => {
    let testStoreState = {
      ...storeState,
      NC: {
        leadForm: {
          ...storeState.NC.leadForm,
          recommendation: {
            list: { "9988776655": { id: 12345 } }
          }
        },
        campaign: {
          ...storeState.NC.campaign
        }
      }
    };
    let storeCopy = mockStore(testStoreState);
    MLAwrapper = shallowWithStore(<MLAScreen />, storeCopy).dive();
    let skipWrapper = MLAwrapper.find('[className="dealer-skip-button"]');
    skipWrapper.simulate("click");
    expect(ScreenState.setRecoScreen).toHaveBeenCalled();
  });
});

describe("Testing MLA screen container - Tracking Events", () => {
  test("Skip button shown - recommendations available", () => {});
  test("All options selected - currently deselected, select all changed", () => {});
  test("All options deselected - currently selected, select all changed", () => {});
  test("Checkbox selected - currently deselected, checkbox changed", () => {});
  test("Checkbox deselected - currently selected, checkbox changed", () => {});
  test("Log Lead Submit - submit button clicked", () => {});
  test("Lead Submit - submit button clicked", () => {});
  test("Log MLA skipped - skip button clicked", () => {});
  test("MLA skipped - skip button clicked", () => {});
});
