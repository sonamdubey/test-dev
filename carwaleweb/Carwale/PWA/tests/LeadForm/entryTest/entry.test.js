require("jest-localstorage-mock");
import React from "react";
import "../../../src/LeadForm/styles/LeadForm.scss";
import {
  SET_LOCATION_SCREEN,
  SET_SORRY_SCREEN,
  SET_FORM_SCREEN,
  SET_SHOW,
  SET_WHATSAPP_FORM_SCREEN
} from "../../../src/LeadForm/Actions/ScreenActionTypes";
import { PREFILL_LOCATION } from "../../../src/LeadForm/Actions/FormActionTypes";
import { SET_CAMPAIGN } from "../../../src/LeadForm/Actions/FormActionTypes";
import "../../../style/location.scss";
import store from "../../../src/LeadForm/store";
import * as entry from "../../../src/LeadForm/entry";
import * as Analytics from "../../../src/utils/Analytics";

jest.mock("../../../src/LeadForm/styles/LeadForm.scss", () => {
  return jest.fn();
});

jest.mock("shallowequal", () => {
  return jest.fn();
});

jest.mock("GlobalStyle/location.scss", () => {
  return jest.fn();
});

//mocking analyitcs module
jest.mock("../../../src/utils/Analytics");

//mocking lead form container
jest.mock("../../../src/LeadForm/Containers/LeadForm", () => param => {
  return (
    <div>
      <div id="initialCityId">{param.initialCityId}</div>
      <div id="testDriveFlag">{param.testDriveChecked.toString()}</div>
      <div id="modelId">{param.modelDetail.modelId}</div>
      <div id="versionId">{param.modelDetail.versionId}</div>
    </div>
  );
});

jest.mock("../../../src/LeadForm/store", () => {
  return mockStore({
    screenData: {
      screen: 7
    },
    isLeadFormVisible: false,
    leadClickSource: {
      propId: 1,
      page: {
        platform: {
          id: 43,
          name: "Msite"
        },
        page: {
          name: "",
          id: ""
        }
      },
      isCitySet: false
    },
    NC: {
      campaign: {},
      leadForm: {}
    },
    location: {
      cityName: "",
      cityId: 0,
      areaName: "",
      areaId: 0,
      userConfirmed: false
    }
  });
});

let leadFormCta, clickEvt;
let storeCopy;

beforeAll(() => {
  let renderElem = document.createElement("div");
  renderElem.setAttribute("Id", "LeadForm");
  document.body.appendChild(renderElem);
  leadFormCta = document.createElement("SPAN");
  let t = document.createTextNode("EMI LINK");
  leadFormCta.appendChild(t);
  leadFormCta.setAttribute("campaigncta", "true");
  document.body.appendChild(leadFormCta);
  clickEvt = document.createEvent("HTMLEvents");
  clickEvt.initEvent("click", true, true);
  //mocking operations in analyitcs module
  Analytics.fireInteractiveTracking = jest.fn();
  Analytics.fireNonInteractiveTracking = jest.fn();
  window.registerCampaignEvent(document);
});

beforeEach(() => {
  let attributes = leadFormCta.attributes;
  for (let index in leadFormCta.attributes) {
    if (attributes[index].name != "data-campaign-event") {
      leadFormCta.removeAttribute(attributes[index].name);
    }
  }

  leadFormCta.setAttribute("campaigncta", "true");
  leadFormCta.setAttribute("modelid", "930");
  storeCopy = store;
  storeCopy.clearActions();
  storeCopy.getState().location = {};
  localStorage.clear();
});

describe("Initialize lead form", () => {
  test("Binidng of Click Listener on CTA - On entry load", () => {
    let ctaList = document.querySelectorAll("[campaigncta]");
    ctaList.forEach(cta => {
      expect(cta.getAttribute("data-campaign-event")).toBeTruthy();
    });
  });

  //sorry screen tests
  test("Location available in CTA, Campaign not available - Set Sorry Screen", () => {
    let userLocation = '{"cityId":13, "cityName":"Navi Mumbai"}';
    leadFormCta.setAttribute("userLocation", userLocation);
    leadFormCta.dispatchEvent(clickEvt);
    let firedActions = storeCopy.getActions();

    expect(firedActions).toContainEqual(
      expect.objectContaining({
        type: SET_SORRY_SCREEN
      })
    );
  });

  test("Location available in Cookies, Campaign not available - Set Sorry Screen", () => {
    storeCopy.getState().location.cityId = 1;
    storeCopy.getState().location.cityName = "Mumbai";
    storeCopy.getState().location.areaId = 55;
    storeCopy.getState().location.areaName = "Andheri (W)";

    leadFormCta.dispatchEvent(clickEvt);
    let firedActions = storeCopy.getActions();
    expect(firedActions).toContainEqual(
      expect.objectContaining({
        type: SET_SORRY_SCREEN
      })
    );
  });

  //navigation history tests
  test("Msite - Navigation History is set", () => {
    storeCopy.getState().leadClickSource.page.platform.id = 43;
    let initialHistoryLength = window.history.length;
    leadFormCta.dispatchEvent(clickEvt);

    expect(initialHistoryLength + 1).toEqual(window.history.length);
  });

  test("Desktop - Navigation History is not set", () => {
    storeCopy.getState().leadClickSource.page.platform.id = 1;
    let initialHistoryLength = window.history.length;
    leadFormCta.dispatchEvent(clickEvt);

    expect(initialHistoryLength).toEqual(window.history.length);
  });

  //CTA attributes tests
  test("VersionId is Set in CTA attributes - Passed to lead Form", () => {
    leadFormCta.setAttribute("versionid", 5786);
    leadFormCta.dispatchEvent(clickEvt);

    expect(document.getElementById("versionId").innerHTML).toEqual("5786");
  });

  test("VersionId is not Set in CTA attributes - Not Passed to lead Form", () => {
    leadFormCta.dispatchEvent(clickEvt);
    expect(document.getElementById("versionId").innerHTML).toEqual("0");
  });

  test("Testdrive is set true in CTA attributes - set in Lead form", () => {
    leadFormCta.setAttribute("testDriveChecked", true);
    leadFormCta.dispatchEvent(clickEvt);

    expect(document.getElementById("testDriveFlag").innerHTML).toEqual("true");
  });

  test("Testdrive is set false in CTA attributes - not set in Lead form", () => {
    leadFormCta.setAttribute("testDriveChecked", false);
    leadFormCta.dispatchEvent(clickEvt);

    expect(document.getElementById("testDriveFlag").innerHTML).toEqual("false");
  });

  test("Testdrive is not set in CTA attributes - not set in Lead form", () => {
    leadFormCta.dispatchEvent(clickEvt);

    expect(document.getElementById("testDriveFlag").innerHTML).toEqual("false");
  });

  //CTA click tests
  test("CTA click - Show Lead Form", () => {
    leadFormCta.dispatchEvent(clickEvt);
    process.nextTick(() => {
      setTimeout(() => {
        expect(store.getActions()[0]).toEqual({
          type: SET_SHOW
        });
      }, 0);
    });
  });

  //location test cases
  test("Location not set - Set Location Screen", () => {
    leadFormCta.dispatchEvent(clickEvt);
    let firedActions = storeCopy.getActions();

    expect(firedActions).toContainEqual(
      expect.objectContaining({
        type: SET_LOCATION_SCREEN
      })
    );
  });

  test("Url City set area not set - Set Location Screen", () => {
    let userLocation = '{"cityId":1, "cityName":"Mumbai"}';
    leadFormCta.setAttribute("userLocation", userLocation);
    leadFormCta.dispatchEvent(clickEvt);
    let firedActions = storeCopy.getActions();

    expect(firedActions).toContainEqual(
      expect.objectContaining({
        type: SET_LOCATION_SCREEN
      })
    );
  });

  test("City set in cookies area not set - Set Location Screen", () => {
    storeCopy.getState().location.cityId = 1;
    storeCopy.getState().location.cityName = "Mumbai";
    leadFormCta.dispatchEvent(clickEvt);
    let firedActions = storeCopy.getActions();

    expect(firedActions).toContainEqual(
      expect.objectContaining({
        type: SET_LOCATION_SCREEN
      })
    );
  });

  test("Location set in cookies - Do not Set Location Screen", () => {
    storeCopy.getState().location.cityId = 13;
    storeCopy.getState().location.cityName = "Navi Mumbai";
    leadFormCta.dispatchEvent(clickEvt);
    let firedActions = storeCopy.getActions();

    expect(firedActions).not.toContainEqual(
      expect.objectContaining({
        type: SET_LOCATION_SCREEN
      })
    );
  });

  test("Url Location set - Do not Set Location Screen", () => {
    let userLocation = '{"cityId":13, "cityName":"Navi Mumbai"}';
    leadFormCta.setAttribute("userLocation", userLocation);
    leadFormCta.dispatchEvent(clickEvt);
    let firedActions = storeCopy.getActions();

    expect(firedActions).not.toContainEqual(
      expect.objectContaining({
        type: SET_LOCATION_SCREEN
      })
    );
  });

  //form screen test cases
  test("Location and Campaign set in session storage - Open form screen", () => {
    let campaignid_dealerid = "4193_10178";
    let campaignkey = "LEADFORM_4193_10178";
    let campaignValue = '{ "campaign": true }';
    sessionStorage.setItem(campaignkey, campaignValue);

    leadFormCta.setAttribute("campaignid_dealerid", campaignid_dealerid);
    leadFormCta.dispatchEvent(clickEvt);
    let firedActions = storeCopy.getActions();

    expect(firedActions).toContainEqual(
      expect.objectContaining({
        type: SET_FORM_SCREEN
      })
    );
  });

  test("Campaign for modelId available in session storage - Open form screen", () => {
    let modelIdKey = "LEADFORM_930";
    let campaignValue = '{ "campaign": true }';
    sessionStorage.setItem(modelIdKey, campaignValue);

    leadFormCta.dispatchEvent(clickEvt);
    let firedActions = storeCopy.getActions();

    expect(firedActions).toContainEqual(
      expect.objectContaining({
        type: SET_FORM_SCREEN
      })
    );
  });

  test("Campaign for modelId available in session storage and propId is 25 - Open whatsapp form screen", () => {
    let modelIdKey = "LEADFORM_930";
    let campaignValue = '{ "campaign": true }';
    leadFormCta.setAttribute("propertyid", 25);
    sessionStorage.setItem(modelIdKey, campaignValue);

    leadFormCta.dispatchEvent(clickEvt);
    let firedActions = storeCopy.getActions();

    expect(firedActions).toContainEqual({ type: SET_WHATSAPP_FORM_SCREEN });
  });

  //campaign data tests
  test("Location and Campaign set in session storage - Get campaign data", () => {
    let campaignid_dealerid = "4193_10178";
    let campaignkey = "LEADFORM_4193_10178";
    let campaignValue = '{ "campaign": true }';
    sessionStorage.setItem(campaignkey, campaignValue);

    leadFormCta.setAttribute("campaignid_dealerid", campaignid_dealerid);
    leadFormCta.dispatchEvent(clickEvt);
    let firedActions = storeCopy.getActions();

    expect(firedActions).toContainEqual(
      expect.objectContaining({
        type: SET_CAMPAIGN
      })
    );
  });

  test("Campaign available in session storage - get campaign by modelId", () => {
    let modelIdKey = "LEADFORM_930";
    let campaignValue = '{ "campaign": true }';
    sessionStorage.setItem(modelIdKey, campaignValue);
    leadFormCta.dispatchEvent(clickEvt);
    let firedActions = storeCopy.getActions();

    expect(firedActions).toContainEqual(
      expect.objectContaining({
        type: SET_CAMPAIGN
      })
    );
  });
});
