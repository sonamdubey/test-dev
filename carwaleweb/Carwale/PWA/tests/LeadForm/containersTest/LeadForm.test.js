import React from "react";

import LeadForm from "../../../src/LeadForm/Containers/LeadForm";
import { screenType } from "../../../src/LeadForm/Enum/ScreenEnum";
import { setHide } from "../../../src/LeadForm/ActionCreators/ScreenState";
import { platform } from "../../../src/enum/Platform";
import LocationScreen from "../../../src/LeadForm/Containers/LocationScreen";
import MLAScreen from "../../../src/LeadForm/Containers/MLAScreen";
import RecoScreen from "../../../src/LeadForm/Containers/RecoScreen";
import ThankYouScreen from "../../../src/LeadForm/Containers/ThankYouScreen";
import SorryScreen from "../../../src/LeadForm/Components/SorryScreen";
import store from "../../../src/LeadForm/store";

jest.mock("../../../src/LeadForm/utils/Tracking", () => {
  return {
    trackCloseClick: jest.fn()
  };
});

jest.mock("../../../src/LeadForm/store", () => {
  return mockStore({
    screenData: {
      screen: 1
    },
    isLeadFormVisible: true,
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

jest.mock("../../../src/utils/UrlFactory", () => {
  return { RefreshPage: jest.fn() };
});

jest.mock("../../../src/LeadForm/Containers/LocationScreen", () => {
  return jest.fn();
});

jest.mock("../../../src/LeadForm/Containers/FormScreen", () => {
  return jest.fn();
});

jest.mock("../../../src/LeadForm/Components/SorryScreen", () => {
  return jest.fn();
});

jest.mock("../../../src/LeadForm/Containers/MLAScreen", () => {
  return jest.fn();
});

jest.mock("../../../src/LeadForm/Containers/RecoScreen", () => {
  return jest.fn();
});

jest.mock("../../../src/LeadForm/Containers/ThankYouScreen", () => {
  return jest.fn();
});

jest.mock("../../../src/utils/Analytics", () => {
  return { fireInteractiveTracking: jest.fn() };
});

jest.mock("../../../src/LeadForm/ActionCreators/ScreenState", () => {
  return {
    setHide: jest.fn(() => {
      return { type: "" };
    }),
    setLocationScreen: jest.fn(() => {
      return { type: "" };
    }),
    setSorryScreen: jest.fn(() => {
      return { type: "" };
    }),
    setFormScreen: jest.fn(() => {
      return { type: "" };
    })
  };
});

let storeCopy;
let leadFormWrapper;

let storeState = {
  screenData: {
    screen: screenType.Form
  },
  isLeadFormVisible: true,
  leadClickSource: {
    propId: 1,
    page: {
      platform: {
        id: platform.DESKTOP.id,
        name: platform.DESKTOP.name
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
};

let modelDetail = {
  modelId: 1,
  versionId: 1
};

let cityId = 123;

beforeEach(() => {
  //mocking store
  storeCopy = store;
  window.history.back = jest.fn();

  //mocking container
  leadFormWrapper = shallowWithStore(
    <LeadForm modelDetail={modelDetail} initialCityId={cityId} />,
    storeCopy
  ).dive();
});

describe("Testing LeadForm  container - DOM", () => {
  test("LeadForm Visible - CTA Clicked", () => {
    let rootDivWrapper = leadFormWrapper.find("div").at(0);
    expect(rootDivWrapper.props().className).toEqual("lead-popup--visible");
  });

  test("LeadForm not Visible - Close button clicked", () => {
    let testStoreState = { ...storeState, isLeadFormVisible: false };
    let testStore = mockStore(testStoreState);
    let leadFormWrapper = shallowWithStore(
      <LeadForm modelDetail={modelDetail} initialCityId={cityId} />,
      testStore
    ).dive();
    let rootDivWrapper = leadFormWrapper.find("div").at(0);
    expect(rootDivWrapper.props().className).toEqual("");
  });

  test("Background blackout on Desktop - CTA clicked", () => {
    let blackout = leadFormWrapper.find("[className='lead-popup-bg-window']");
    expect(blackout.exists()).toBeTruthy();
  });

  test("Close Btn is white - current Screen is MLA", () => {
    let testStoreState = {
      ...storeState,
      screenData: {
        screen: screenType.MLA
      }
    };
    let testStore = mockStore(testStoreState);
    leadFormWrapper = shallowWithStore(
      <LeadForm modelDetail={modelDetail} initialCityId={cityId} />,
      testStore
    ).dive();
    let closeBtn = leadFormWrapper.find("span").at(0);
    expect(closeBtn.props().className).toEqual(
      "popup-close-btn popup-close-btn--white"
    );
  });

  test("Close Btn is white - current Screen is Reco", () => {
    let testStoreState = {
      ...storeState,
      screenData: {
        screen: screenType.Reco
      }
    };
    let testStore = mockStore(testStoreState);
    leadFormWrapper = shallowWithStore(
      <LeadForm modelDetail={modelDetail} initialCityId={cityId} />,
      testStore
    ).dive();
    let closeBtn = leadFormWrapper.find("span").at(0);
    expect(closeBtn.props().className).toEqual(
      "popup-close-btn popup-close-btn--white"
    );
  });

  test("Close Btn is white - current Screen is ThankYou and email is empty", () => {
    let testStoreState = {
      ...storeState,
      screenData: {
        screen: screenType.ThankYou
      }
    };
    let testStore = mockStore(testStoreState);
    leadFormWrapper = shallowWithStore(
      <LeadForm modelDetail={modelDetail} initialCityId={cityId} />,
      testStore
    ).dive();
    let closeBtn = leadFormWrapper.find("span").at(0);
    expect(closeBtn.props().className).toEqual(
      "popup-close-btn popup-close-btn--white"
    );
  });

  test("Close Btn is Black - current Screen is Form", () => {
    let closeBtn = leadFormWrapper.find("span").at(0);
    expect(closeBtn.props().className).toEqual(
      "popup-close-btn popup-close-btn--black"
    );
  });

  test("Close Btn is Black - current Screen is Location", () => {
    let testStoreState = {
      ...storeState,
      screenData: {
        screen: screenType.Location
      }
    };
    let testStore = mockStore(testStoreState);
    leadFormWrapper = shallowWithStore(
      <LeadForm modelDetail={modelDetail} initialCityId={cityId} />,
      testStore
    ).dive();
    let closeBtn = leadFormWrapper.find("span").at(0);
    expect(closeBtn.props().className).toEqual(
      "popup-close-btn popup-close-btn--black"
    );
  });

  test("Close Btn is Black - current Screen is Sorry", () => {
    let testStoreState = {
      ...storeState,
      screenData: {
        screen: screenType.Sorry
      }
    };
    let testStore = mockStore(testStoreState);
    leadFormWrapper = shallowWithStore(
      <LeadForm modelDetail={modelDetail} initialCityId={cityId} />,
      testStore
    ).dive();
    let closeBtn = leadFormWrapper.find("span").at(0);
    expect(closeBtn.props().className).toEqual(
      "popup-close-btn popup-close-btn--black"
    );
  });

  test("Close Btn is Black - current Screen is Thankyou and email is available", () => {
    let testStoreState = {
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
      },
      screenData: {
        screen: screenType.ThankYou
      }
    };
    let testStore = mockStore(testStoreState);
    leadFormWrapper = shallowWithStore(
      <LeadForm modelDetail={modelDetail} initialCityId={cityId} />,
      testStore
    ).dive();
    let closeBtn = leadFormWrapper.find("span").at(0);
    expect(closeBtn.props().className).toEqual(
      "popup-close-btn popup-close-btn--black"
    );
  });

  test("Location Screen shown - current Screen in store is Location", () => {
    let testStoreState = {
      ...storeState,
      screenData: {
        screen: screenType.Location
      }
    };
    let testStore = mockStore(testStoreState);
    leadFormWrapper = shallowWithStore(
      <LeadForm modelDetail={modelDetail} initialCityId={cityId} />,
      testStore
    ).dive();
    expect(
      leadFormWrapper.contains(<LocationScreen modelDetail={modelDetail} />)
    ).toBeTruthy();
  });

  test("MLA Screen shown - current Screen in store is MLA", () => {
    let testStoreState = {
      ...storeState,
      screenData: {
        screen: screenType.MLA
      }
    };
    let testStore = mockStore(testStoreState);
    leadFormWrapper = shallowWithStore(
      <LeadForm modelDetail={modelDetail} initialCityId={cityId} />,
      testStore
    ).dive();
    expect(
      leadFormWrapper.contains(<MLAScreen modelDetail={modelDetail} />)
    ).toBeTruthy();
  });

  test("Reco Screen shown - current Screen in store is Reco", () => {
    let testStoreState = {
      ...storeState,
      screenData: {
        screen: screenType.Reco
      }
    };
    let testStore = mockStore(testStoreState);
    leadFormWrapper = shallowWithStore(
      <LeadForm modelDetail={modelDetail} initialCityId={cityId} />,
      testStore
    ).dive();
    expect(leadFormWrapper.contains(<RecoScreen />)).toBeTruthy();
  });

  test("ThankYou Screen shown - current Screen in store is ThankYou", () => {
    let testStoreState = {
      ...storeState,
      screenData: {
        screen: screenType.ThankYou
      }
    };
    let testStore = mockStore(testStoreState);
    leadFormWrapper = shallowWithStore(
      <LeadForm modelDetail={modelDetail} initialCityId={cityId} />,
      testStore
    ).dive();
    expect(leadFormWrapper.contains(<ThankYouScreen />)).toBeTruthy();
  });

  test("Sorry Screen shown - current Screen in store is Sorry", () => {
    let testStoreState = {
      ...storeState,
      screenData: {
        screen: screenType.Sorry
      }
    };
    let testStore = mockStore(testStoreState);
    leadFormWrapper = shallowWithStore(
      <LeadForm modelDetail={modelDetail} initialCityId={cityId} />,
      testStore
    ).dive();
    expect(
      leadFormWrapper.contains(
        <SorryScreen setLog={leadFormWrapper.instance().setSorryScreenLog} />
      )
    ).toBeTruthy();
  });
});

describe("Testing LeadForm  container - Interaction Events", () => {
  test("Hide Lead Form - Close button clicked", () => {
    let closeBtn = leadFormWrapper.find("span").at(0);
    closeBtn.simulate("click");
    expect(setHide).toHaveBeenCalled();
  });

  test("Go back in history - Close cliked on platform other than Desktop", () => {
    let testStoreState = {
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
    let testStore = mockStore(testStoreState);
    leadFormWrapper = shallowWithStore(
      <LeadForm modelDetail={modelDetail} initialCityId={cityId} />,
      testStore
    ).dive();
    let closeBtn = leadFormWrapper.find("span").at(0);
    closeBtn.simulate("click");
    expect(window.history.back).toHaveBeenCalled();
  });

  test("Hide LeadForm - Blackout background clicked", () => {
    let blackoutDiv = leadFormWrapper.find("div").at(2);
    blackoutDiv.simulate("click");
    expect(setHide).toHaveBeenCalled();
  });
});

describe("Testing LeadForm  container - Tracking Events", () => {});
