import React from "react";

import LocationScreen from "../../../src/LeadForm/Containers/LocationScreen";
import { platform } from "../../../src/enum/Platform";
import LocationContainer from "GlobalComponent/LocationContainer";
import {
  SetLocation,
  setLeadSourceCity
} from "../../../src/LeadForm/ActionCreators/Location";
import { makeCancelable } from "../../../src/utils/CancelablePromise";
import campaignApi from "../../../src/LeadForm/Api/Campaign";
import { setCampaignModelKey } from "../../../src/LeadForm/ActionCreators/Campaign";
import {
  setFormScreen,
  setSorryScreen
} from "../../../src/LeadForm/ActionCreators/ScreenState";
import * as LogState from "../../../src/LeadForm/ActionCreators/LogState";
import store from "../../../src/LeadForm/store";

jest.mock("../../../src/LeadForm/store", () => {
  return mockStore({
    leadClickSource: {
      propId: 1,
      page: {
        platform: {
          id: 1,
          name: ""
        },
        page: {
          name: ""
        },
        applicationId: 111
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
      campaign: {}
    },
    location: {
      cityId: 123,
      cityName: "",
      areaId: 124,
      areaName: "",
      userConfirmed: false
    },
    isLeadFormVisible: true,
    interactionId: 1
  });
});

jest.mock("GlobalStyle/location.scss", () => {
  return jest.fn();
});

jest.mock("GlobalActionCreator/CityAutocomplete", () => {
  return {
    setArea: jest.fn(() => {
      return { type: "" };
    }),
    setCity: jest.fn(() => {
      return { type: "" };
    })
  };
});

jest.mock("../../../src/utils/Analytics", () => {
  return {
    fireInteractiveTracking: jest.fn(),
    fireNonInteractiveTracking: jest.fn()
  };
});

jest.mock("../../../src/LeadForm/ActionCreators/ScreenState", () => {
  return {
    setSorryScreen: jest.fn(() => {
      return { type: "" };
    }),
    setFormScreen: jest.fn(() => {
      return { type: "" };
    })
  };
});

jest.mock("../../../src/LeadForm/ActionCreators/Location", () => {
  return {
    setLeadSourceCity: jest.fn(() => {
      return { type: "" };
    }),
    SetLocation: jest.fn(() => {
      return { type: "" };
    }),
    setCityChange: jest.fn(() => {
      return { type: "" };
    })
  };
});

jest.mock("../../../src/LeadForm/ActionCreators/LogState");

jest.mock("../../../src/LeadForm/ActionCreators/Campaign", () => {
  return {
    setCampaignModelKey: jest.fn(() => {
      return { type: "" };
    })
  };
});

jest.mock("../../../src/LeadForm/Api/Campaign");

jest.mock("../../../src/utils/CancelablePromise", () => {
  return {
    makeCancelable: jest.fn(promise => ({
      promise,
      cancel: jest.fn()
    }))
  };
});

// let store;
let locationWrapper;

const cityEvent = {
  cityId: 10,
  cityMaskingName: "newdelhi",
  cityName: "New Delhi",
  isConfirmBtnClicked: true
};

const areaEvent = {
  areaId: 3710,
  areaName: "Janakpuri"
};

let storeState = {
  leadClickSource: {
    propId: 1,
    page: {
      platform: {
        id: platform.DESKTOP.id,
        name: ""
      },
      page: {
        name: ""
      },
      applicationId: 111
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
    campaign: {}
  },
  location: {
    cityId: 123,
    cityName: "",
    areaId: 124,
    areaName: "",
    userConfirmed: false
  },
  isLeadFormVisible: true
};

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

const modelDetail = { modelId: 1, versionId: 1 };
beforeEach(() => {
  if (campaignApi.get) {
    campaignApi.get.mockClear();
  }

  campaignApi.get = jest.fn(() => {
    const campaignResponse = {
      name: "xyz",
      featuredCarData: {
        modelId: 1
      }
    };
    return Promise.resolve(campaignResponse);
  });

  //mocking container
  locationWrapper = shallowWithStore(
    <LocationScreen modelDetail={modelDetail} />,
    store
  ).dive();
});

describe("Testing LocationScreen  container - DOM", () => {
  test("LocationContainer visible - CTA Clicked", () => {
    let locationContainer = locationWrapper.find(LocationContainer);
    expect(locationContainer.exists()).toBeTruthy();
  });

  test("validateStatus is empty in LocationContainer - location is valid", () => {
    let locationContainer = locationWrapper.find(LocationContainer);
    expect(locationContainer.props().validateStatus).toEqual("");
  });

  test("validateStatus is 'error' in LocationContainer - location is invalid", () => {
    locationWrapper.setState({ isValid: false });
    let locationContainer = locationWrapper.find(LocationContainer);
    expect(locationContainer.props().validateStatus).toEqual("error");
  });
});

describe("Testing LocationScreen  container - Interactive Events", () => {
  test("Set location in store - confirm clicked,validation successful", () => {
    let locationContainer = locationWrapper.find(LocationContainer).dive();
    let btnWrapper = locationContainer.find("button");
    btnWrapper.simulate("click");
    let stateLocation = {
      cityId: 123,
      cityName: "",
      areaId: 124,
      areaName: "",
      userConfirmed: false
    };
    expect(SetLocation).toHaveBeenCalledWith(stateLocation);
  });

  test("Set LeadSourceCity in store - confirm clicked,validation successful", () => {
    let locationContainer = locationWrapper.find(LocationContainer).dive();
    let btnWrapper = locationContainer.find("button");
    btnWrapper.simulate("click");
    expect(setLeadSourceCity).toHaveBeenCalledWith(false);
  });

  test("Cancel Campaign Api request - confirm clicked, validation success, already Api request exist", () => {
    locationWrapper.instance().campaignRequest = makeCancelable(
      Promise.resolve()
    );
    let spy = jest.spyOn(locationWrapper.instance().campaignRequest, "cancel");
    let locationContainer = locationWrapper.find(LocationContainer).dive();
    let btnWrapper = locationContainer.find("button");
    btnWrapper.simulate("click");
    expect(spy).toHaveBeenCalled();
  });

  test("Campaign Api request made- confirm clicked, validation success", () => {
    let locationContainer = locationWrapper.find(LocationContainer).dive();
    let btnWrapper = locationContainer.find("button");
    btnWrapper.simulate("click");
    let locationParam = {
      cityId: 123,
      cityName: "",
      areaId: 124,
      areaName: "",
      userConfirmed: false
    };
    let platformIdParam = 1;
    let applicationIdParam = 111;
    expect(campaignApi.get).toHaveBeenCalledWith(
      locationParam,
      modelDetail,
      platformIdParam,
      applicationIdParam
    );
  });

  test("Disable Confirm butn- confirm clicked, validation success", () => {
    let locationContainer = locationWrapper.find(LocationContainer).dive();
    let btnWrapper = locationContainer.find("button");
    btnWrapper.simulate("click");
    expect(locationWrapper.state().isCampaignRequestFetching).toBeTruthy();
  });

  test("Set Campaign and modelId in store- confirm clicked, validation success, campaign api promise resolve", () => {
    let locationContainer = locationWrapper.find(LocationContainer).dive();
    let btnWrapper = locationContainer.find("button");
    btnWrapper.simulate("click");
    let apiResponse = {
      name: "xyz",
      featuredCarData: {
        modelId: 1
      }
    };
    process.nextTick(() => {
      // try {
      expect(setCampaignModelKey).toHaveBeenCalledWith(
        apiResponse,
        modelDetail.modelId
      );
      // } catch (e) {
      //   console.log(e);
      // }
    });
  });

  test("Show FormScreen- confirm clicked, validation success, campaign api promise resolve", () => {
    let locationContainer = locationWrapper.find(LocationContainer).dive();
    let btnWrapper = locationContainer.find("button");
    btnWrapper.simulate("click");
    process.nextTick(() => {
      // try {
      expect(setFormScreen).toHaveBeenCalled();
      // } catch (e) {
      //   console.log(e);
      // }
    });
  });

  test("'Fetched Campaign data from API' Logged- confirm clicked, validation success, campaign api promise resolve", () => {
    let locationContainer = locationWrapper.find(LocationContainer).dive();
    let btnWrapper = locationContainer.find("button");
    btnWrapper.simulate("click");

    process.nextTick(() => {
      // try {
      expect(LogState.setInfoLog).toHaveBeenCalledWith(
        "Fetched Campaign data from API",
        1
      );
      // } catch (e) {
      //   console.log(e);
      // }
    });
  });

  test("Reset the error states- city selected from suggestions", () => {
    locationWrapper.instance().selectCity(cityEvent);
    expect(locationWrapper.state().isValid).toBeTruthy();
    expect(locationWrapper.state().helperText).toEqual("");
  });

  test("Set city in localstate- city selected", () => {
    locationWrapper.instance().selectCity(cityEvent);
    expect(locationWrapper.state().location.cityId).toEqual(10);
    expect(locationWrapper.state().location.cityName).toEqual("New Delhi");
  });

  test("Reset the error states- area selected from suggestions", () => {
    locationWrapper.instance().selectArea(areaEvent);
    expect(locationWrapper.state().isValid).toBeTruthy();
    expect(locationWrapper.state().helperText).toEqual("");
  });

  test("Set area in localstate- area selected", () => {
    locationWrapper.instance().selectArea(areaEvent);
    expect(locationWrapper.state().location.areaId).toEqual(3710);
    expect(locationWrapper.state().location.areaName).toEqual("Janakpuri");
  });

  test("Reset the error states- city removed from autocomplete", () => {
    locationWrapper.instance().handleCityRemove();
    expect(locationWrapper.state().isValid).toBeTruthy();
    expect(locationWrapper.state().helperText).toEqual("");
  });

  test("Reset location in localstate - city removed from autocomplete", () => {
    let location = {
      cityId: -1,
      cityName: "Select City",
      areaId: -1,
      areaName: "Select Area"
    };
    locationWrapper.instance().handleCityRemove();
    expect(locationWrapper.state().location).toEqual(location);
  });

  test("Reset the error states- area removed from autocomplete", () => {
    locationWrapper.instance().handleAreaRemove();
    expect(locationWrapper.state().isValid).toBeTruthy();
    expect(locationWrapper.state().helperText).toEqual("");
  });

  test("Reset location in localstate- area removed from autocomplete", () => {
    let location = {
      cityId: 123,
      cityName: "",
      areaId: -1,
      areaName: "Select Area",
      userConfirmed: false
    };
    locationWrapper.instance().handleAreaRemove();
    expect(locationWrapper.state().location).toEqual(location);
  });

  test("Show sorry screen - Campaign api fails", () => {
    campaignApi.get.mockClear();
    campaignApi.get = jest.fn(() => Promise.reject("Api failed"));
    let locationContainer = locationWrapper.find(LocationContainer).dive();
    let btnWrapper = locationContainer.find("button");
    btnWrapper.simulate("click");
    process.nextTick(() => {
      // try {
      expect(setSorryScreen).toHaveBeenCalled();
      // } catch (e) {
      //   console.log(e);
      // }
    });
  });

  test("`Error: Api failed. Found in Campaign API` Logged - Campaign api fails", () => {
    campaignApi.get.mockClear();
    campaignApi.get = jest.fn(() => Promise.reject("Api failed"));
    let locationContainer = locationWrapper.find(LocationContainer).dive();
    let btnWrapper = locationContainer.find("button");
    btnWrapper.simulate("click");
    process.nextTick(() => {
      // try {
      expect(LogState.setErrorLog).toHaveBeenCalledWith(
        `Error: Api failed. Found in Campaign API`,
        1
      );
      // } catch (e) {
      //   console.log(e);
      // }
    });
  });
});

describe("Testing LocationScreen  container - shoudlComponentUpdate", () => {
  test("LocationContainer attribute helperText and validateStatus empty  - LeadForm closed", () => {
    locationWrapper.setProps({ isLeadFormVisible: false });
    let locationContainer = locationWrapper.find(LocationContainer);
    expect(locationContainer.props().helperText).toEqual("");
    expect(locationContainer.props().validateStatus).toEqual("");
  });

  test("LocationContainer attribute cityLocation update - City changed in localstate", () => {
    locationWrapper.setState({
      location: { ...storeState.location, cityId: 700 }
    });
    let locationContainer = locationWrapper.find(LocationContainer);
    let cityLocation = {
      cityId: 700,
      cityName: "",
      areaId: 124,
      areaName: "",
      userConfirmed: false
    };
    expect(locationContainer.props().cityLocation).toEqual(cityLocation);
  });

  test("LocationContainer attribute cityLocation update - area changed in localstate", () => {
    locationWrapper.setState({
      location: { ...storeState.location, areaId: 700 }
    });
    let locationContainer = locationWrapper.find(LocationContainer);
    let cityLocation = {
      cityId: 123,
      cityName: "",
      areaId: 700,
      areaName: "",
      userConfirmed: false
    };
    expect(locationContainer.props().cityLocation).toEqual(cityLocation);
  });

  test("LocationContainer attribute validateStatus 'error' - validity changed in localstate", () => {
    locationWrapper.setState({ isValid: false });
    let locationContainer = locationWrapper.find(LocationContainer);
    expect(locationContainer.props().validateStatus).toEqual("error");
  });

  test("LocationContainer attribute validateStatus empty - validity changed in localstate", () => {
    locationWrapper.setState({ isValid: true });
    let locationContainer = locationWrapper.find(LocationContainer);
    expect(locationContainer.props().validateStatus).toEqual("");
  });
});

describe("Testing LocationScreen  container - componentWillReceiveProps", () => {
  test("Update Location in localstate - location changed in store", () => {
    locationWrapper.setProps({
      location: { ...storeState.location, cityId: 786 }
    });
    expect(locationWrapper.state().location.cityId).toEqual(786);
  });
});
