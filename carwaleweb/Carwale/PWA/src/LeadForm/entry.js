//react 16 requirements https://reactjs.org/docs/javascript-environment-requirements.html
import "raf/polyfill";
import "core-js/es6/map";
import "core-js/es6/set";
import "core-js/fn/object/assign";
import "core-js/fn/array/find-index";
import "core-js/fn/array/includes";
require("es6-promise").polyfill();
import "isomorphic-fetch";

import React from "react";
import ReactDOM from "react-dom";
import { Provider } from "react-redux";

import store from "./store";
import LeadForm from "./Containers/LeadForm";
import "./styles/LeadForm.scss";
import { fireNonInteractiveTracking } from "../utils/Analytics";
import { screenType } from "./Enum/ScreenEnum";
import { platform } from "../enum/Platform";
import { PrefillLocation, setLeadSourceCity } from "./ActionCreators/Location";
import { setCampaign } from "./ActionCreators/Campaign";
import { setPropId } from "./ActionCreators/PropId";
import {
  setFormScreen,
  setSorryScreen,
  setLocationScreen,
  setHide,
  setShow,
  setWhatsAppFormScreen
} from "./ActionCreators/ScreenState";
import { CheckLocation } from "./utils/Globals";
import { clearLeadId } from "./ActionCreators/LeadId";
import { setTimeout } from "timers";
import { setInteractionId } from "./ActionCreators/InteractionId";
import { setInfoLog } from "./ActionCreators/LogState";
import { setOthers } from "./ActionCreators/Others";
import { trackBrowserBack } from "./utils/Tracking";

let isPopListener = false;

window.registerCampaignEvent = selector => {
  let ctaList = selector.querySelectorAll("[campaigncta]");

  for (let i = 0; i < ctaList.length; i++) {
    if (!ctaList[i].getAttribute("data-campaign-event")) {
      ctaList[i].setAttribute("data-campaign-event", true);
      ctaList[i].addEventListener("click", function(event) {
        clickHandler(event.target.attributes);
      });
    }
  }
};

registerEvents();

function LoadLeadform(initialData) {
  ReactDOM.render(
    <Provider store={store}>
      <LeadForm {...initialData} />
    </Provider>,
    document.getElementById("LeadForm")
  );
}

function hideLeadForm(event) {
  let isLeadFormVisible = store.getState().isLeadFormVisible;
  let currentHistoryState = event.state;
  if (
    currentHistoryState != null &&
    currentHistoryState.Popup == "LeadForm" &&
    isLeadFormVisible
  ) {
    trackBrowserBack();
    store.dispatch(setHide());
  }
}

function pushHistory() {
  if (!isPopListener) {
    window.addEventListener("popstate", event => hideLeadForm(event));
    isPopListener = true;
  }

  window.history.replaceState({ Popup: "LeadForm" }, "LeadForm");
  window.history.pushState(null, "");
}

function registerEvents() {
  window.registerCampaignEvent(document);
}

function clickHandler(attr) {
  let interactionId = store.getState().interactionId;
  store.dispatch(setInteractionId(++interactionId));

  let message =
    "Entering LeadForm (Current Store data Provided in currentState)";
  store.dispatch(setInfoLog(message));

  let ctaAttributes = {
    propId: attr.propertyid ? attr.propertyid.value : 0,
    modelId: attr.modelid ? attr.modelid.value : 0,
    versionId: attr.versionid ? attr.versionid.value : 0,
    campaignDealerId: attr.campaignid_dealerid
      ? attr.campaignid_dealerid.value
      : 0,
    userLocation: attr.userlocation ? attr.userlocation.value : 0,
    testDriveChecked: attr.testdrivechecked
      ? attr.testdrivechecked.value.toLowerCase() == "true"
      : false,
    others: attr.others ? attr.others.value : "{}"
  };

  SetInitialState(ctaAttributes);

  let isDesktop =
    store.getState().leadClickSource.page.platform.id == platform.DESKTOP.id;

  if (!isDesktop) {
    pushHistory();
  }
}

window.CampaignCTAClickHandler = clickHandler;

function SetInitialState(ctaAttributes) {
  let location = GetLocation(ctaAttributes.userLocation);
  let state = store.getState();
  let leadClickSource = state.leadClickSource;
  let isCitySet = leadClickSource.isCitySet;
  let platformName = leadClickSource.page.platform.name;
  let pageName = leadClickSource.page.page.name;
  let category = `LeadForm-${platformName}-${pageName}`;
  let action, label;

  store.dispatch(setOthers(JSON.parse(ctaAttributes.others)));
  //To reset the isCitySet Flag
  if (!isCitySet) {
    store.dispatch(setLeadSourceCity(true));
  }

  if (
    location != null &&
    location.cityId != undefined &&
    location.cityId != state.location.cityId
  ) {
    store.dispatch(PrefillLocation(location));
  } else {
    location = state.location;
  }

  let modelDetail = GetModelDetails(ctaAttributes);

  let campaignDetails = GetCampaignDetails(
    ctaAttributes.campaignDealerId,
    modelDetail.modelId
  );

  if (campaignDetails) {
    label = modelDetail.modelId;

    if (modelDetail.modelId == 0) {
      modelDetail.modelId = campaignDetails.featuredCarData.modelId;
    }
    store.dispatch(setCampaign(campaignDetails));
    if (ctaAttributes.propId == 25) {
      action = "WhatsAppFormOpen-Without_Asking_City";
      store.dispatch(setWhatsAppFormScreen());
    } else {
      action = "FormOpen-Without_Asking_City";
      store.dispatch(setFormScreen());
    }
    fireNonInteractiveTracking(category, action, label);
  } else {
    if (CheckLocation(location)) {
      store.dispatch(setSorryScreen());
    } else {
      store.dispatch(setInfoLog("Entering Location Screen"));
      store.dispatch(setLocationScreen());
    }
  }

  let initialData = {
    modelDetail: modelDetail,
    testDriveChecked: ctaAttributes.testDriveChecked
  };

  //clearing lead id before re-opening lead form
  store.dispatch(clearLeadId());
  store.dispatch(setPropId(ctaAttributes.propId));

  LoadLeadform(initialData);
  setTimeout(() => {
    store.dispatch(setShow());
  }, 0);
}

function GetLocation(userlocation) {
  return userlocation ? JSON.parse(userlocation) : null;
}

function GetModelDetails(ctaAttributes) {
  let modelId = ctaAttributes.modelId;
  let versionId = ctaAttributes.versionId;
  return { modelId, versionId };
}

function GetCampaignDetails(campaignDealerId, modelId) {
  let key = 0;
  if (campaignDealerId && campaignDealerId != "0") {
    key = campaignDealerId;
  } else if (modelId) {
    key = modelId;
  }
  return JSON.parse(sessionStorage.getItem("LEADFORM_" + key));
}
