import React from "react";
import * as Validate from "../../../src/LeadForm/utils/Validate";
import * as ScreenHeight from "../../../src/LeadForm/utils/ScreenHeight";
import * as RecoState from "../../../src/LeadForm/ActionCreators/RecoState";
import RecoScreen from "../../../src/LeadForm/Containers/RecoScreen";
import store from "../../../src/LeadForm/store";

jest.mock("../../../src/LeadForm/utils/Tracking", () => {
  return {
    trackSubmit: jest.fn(),
    trackCheckItem: jest.fn(),
    trackSelectAll: jest.fn(),
    trackScreenShown: jest.fn()
  };
});

jest.mock("../../../src/LeadForm/utils/Validate");

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
      },
      isCitySet: false
    },
    NC: {
      leadForm: {
        MLASellers: {
          list: {}
        },
        recommendation: {
          list: {
            1: {
              priceOverview: { priceStatus: 1 },
              campaign: {
                id: 4193,
                name: "Sai Service Pvt. Ltd (Central)"
              },
              carDetail: {
                hostUrl: "",
                makeId: 10,
                makeName: "Maruti Suzuki",
                modelId: 840,
                modelName: "Alto K10",
                originalImgPath: ""
              }
            },
            2: {
              priceOverview: { priceStatus: 1 },
              campaign: {
                id: 4194,
                name: "Sai Service Pvt. Ltd (east)"
              },
              carDetail: {
                hostUrl: "",
                makeId: 10,
                makeName: "Maruti Suzuki",
                modelId: 840,
                modelName: "Alto K10",
                originalImgPath: ""
              }
            }
          }
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
    }
  });
});

// let store;
let RecoWrapper;

beforeEach(() => {
  //mocking screenheight module
  ScreenHeight.setScreenBodyHeight = jest.fn(() => {});

  if (Validate.validateButton) {
    Validate.validateButton.mockClear();
  }

  Validate.validateButton = jest.fn();

  RecoState.setRecoLead = jest.fn(selectedReco => {
    return { type: "" };
  });

  //mocking container
  RecoWrapper = shallowWithStore(<RecoScreen />, store).dive();
});

describe("Testing Reco screen container - DOM", () => {
  test("Show Remove All toggle - all options selected", () => {
    RecoWrapper.setState({ selectAll: true, selectedReco: ["1234-9876"] });
    let checkboxWrapper = RecoWrapper.find("[name='Select All']").dive();
    expect(checkboxWrapper.text()).toContain("Remove All");
  });

  test("Remove All CheckBox 'checked' - all options selected", () => {
    RecoWrapper.setState({ selectAll: true, selectedReco: ["1234-9876"] });
    let checkboxWrapper = RecoWrapper.find("[name='Select All']");
    expect(checkboxWrapper.props().checked).toBeTruthy();
  });

  test("Show Select All toggle - all options not selected", () => {
    let checkboxWrapper = RecoWrapper.find("[name='Select All']").dive();
    expect(checkboxWrapper.text()).toContain("Select All");
  });

  test("Select All CheckBox 'unchecked' - all options not selected", () => {
    RecoWrapper.setState({ selectAll: false });
    let checkboxWrapper = RecoWrapper.find("[name='Select All']");
    expect(checkboxWrapper.props().checked).toBeFalsy();
  });

  test("Submit button disabled - no options selected", () => {
    if (Validate.validateButton) {
      Validate.validateButton.mockClear();
    }
    Validate.validateButton = jest.fn(() => {
      return true;
    });

    let RecoWrapper = shallowWithStore(<RecoScreen />, store).dive();
    let BtnWrapper = RecoWrapper.find('[type="primary"]');
    expect(BtnWrapper.props().disabled).toBeTruthy();
    expect(Validate.validateButton).toHaveBeenCalledWith(false, 0);
  });

  test("Submit button enabled - any options selected", () => {
    if (Validate.validateButton) {
      Validate.validateButton.mockClear();
    }
    Validate.validateButton = jest.fn(() => {
      return false;
    });

    let RecoWrapper = shallowWithStore(<RecoScreen />, store).dive();
    RecoWrapper.setState({ selectedReco: [12] });
    let BtnWrapper = RecoWrapper.find('[type="primary"]');
    expect(BtnWrapper.props().disabled).toBeFalsy();
    expect(Validate.validateButton).toHaveBeenCalledWith(false, 1);
  });

  test("Reco CheckBoxGroup 'value' equals selected Reco - state's selectedReco filled", () => {
    RecoWrapper.setState({ selectedReco: [12] });
    let CheckboxGroup = RecoWrapper.find('[name="Suggested Cars"]');
    let expectedValue = [12];
    expect(CheckboxGroup.props().value).toEqual(expectedValue);
  });
});

describe("Testing Reco screen container - Interaction Events", () => {
  test("Toggle select all - select all changed", () => {
    let checkboxWrapper = RecoWrapper.find("[name='Select All']");
    let initialState = RecoWrapper.state().selectAll;
    checkboxWrapper.simulate("change");
    expect(RecoWrapper.state().selectAll).not.toEqual(initialState);
  });

  test("Set Selected Reco keys - currently deselected, select all changed", () => {
    let checkboxWrapper = RecoWrapper.find("[name='Select All']");
    checkboxWrapper.simulate("change");
    expect(RecoWrapper.state().selectedReco).toEqual(["1", "2"]);
  });

  test("Toggle select all - last deselected checkbox checked", () => {
    let checkboxGroupWrapper = RecoWrapper.find("[name='Suggested Cars']");
    checkboxGroupWrapper.simulate("change", ["1", "2"]);
    expect(RecoWrapper.state().selectAll).toBeTruthy();
  });

  test("Toggle select all - all checkboxes selected, checkbox changed", () => {
    let checkboxGroupWrapper = RecoWrapper.find("[name='Suggested Cars']");
    checkboxGroupWrapper.simulate("change", ["1", "2"]);
    let allSelected = RecoWrapper.state().selectAll;
    checkboxGroupWrapper.simulate("change", ["1"]);
    expect(RecoWrapper.state().selectAll).toEqual(!allSelected);
  });

  test("Set currently selected checkboxes - checkbox changed", () => {
    let checkboxGroupWrapper = RecoWrapper.find("[name='Suggested Cars']");
    checkboxGroupWrapper.simulate("change", ["1"]);
    expect(RecoWrapper.state().selectedReco).toEqual(["1"]);
  });

  test("Set Reco lead - submit button clicked", () => {
    let BtnWrapper = RecoWrapper.find('[type="primary"]');
    BtnWrapper.simulate("click");
    expect(RecoState.setRecoLead).toHaveBeenCalled();
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
