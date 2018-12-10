import {
  SET_MLA_SCREEN,
  SET_RECOMMENDATION_SCREEN,
  SET_LOCATION_SCREEN,
  SET_THANKYOU_SCREEN,
  SET_SORRY_SCREEN,
  SET_HIDE,
  SET_SHOW,
  SET_FORM_SCREEN,
  SET_WHATSAPP_FORM_SCREEN
} from "../Actions/ScreenActionTypes";
import recommendationApi from "../Api/Recommendation";
import { screenType } from "../Enum/ScreenEnum";
import { setInfoLog, setErrorLog, clearLog } from "./LogState";
import {
  SET_RECOMMENDATION_DATA,
  SET_MLA_DATA
} from "../Actions/FormActionTypes";
import { fireNonInteractiveTracking } from "../../utils/Analytics";
import { recoMappingApiToStore } from "../utils/ObjectMapping";
import { makeCancelable } from "../../utils/CancelablePromise";
import { RefreshPage } from "../../utils/UrlFactory";
import { submitBhriguLogs } from "../utils/BhriguSubmission";

let recommendationAPIRequest = null;
/**
 * Called from FormState to set screen type
 * according to the logic
 */
export const setScreen = () => {
  return (dispatch, getState) => {
    let state = getState();
    let MLAData = state.NC.leadForm.MLASellers.list;
    let platform = state.leadClickSource.page.platform;
    let pageName = state.leadClickSource.page.page.name;
    let location = state.location;
    let modelId = state.NC.campaign.featuredCarData.modelId;
    let userMobileNumber = state.NC.leadForm.buyerInfo.mobile;
    let category, action, label;
    let interactionId = state.interactionId;

    if (recommendationAPIRequest != null) {
      recommendationAPIRequest.cancel();
    }

    recommendationAPIRequest = makeCancelable(
      recommendationApi.get(modelId, platform.id, location, userMobileNumber)
    );

    dispatch(setRecoData({}));
    recommendationAPIRequest.promise
      .then(recoData => {
        state = getState();
        dispatch(
          setInfoLog(
            "Fetched Reco data from Reco API for Reco Screen",
            interactionId
          )
        );
        let mappedRecoData = {};

        if (recoData != null && recoData.length != 0) {
          recoData = recoData.filter(reco => reco.carPricesOverview != null);
          recoData.map(reco => {
            let recoKey =
              state.NC.leadForm.buyerInfo.mobile +
              "-" +
              reco.carData.carModel.modelId;
            let recoObject = recoMappingApiToStore(reco);
            mappedRecoData[recoKey] = recoObject;
          });
        }

        if (Object.keys(mappedRecoData).length > 0) {
          dispatch(setRecoData(mappedRecoData));
          if (
            Object.keys(MLAData).length == 0 ||
            !state.NC.campaign.campaign.mutualLeads
          ) {
            dispatch(setRecoScreen());
          }
        } else {
          let currentScreen = state.screenData.screen;
          if (currentScreen != screenType.MLA) {
            dispatch(setThankYouScreen());
          }
        }
      })
      .catch(error => {
        state = getState();
        let currentScreen = state.screenData.screen;
        category = `LeadForm-${platform.name}-${pageName}`;
        action = "RecoScreen-Suggestions_Not_Shown";
        label = `${modelId}_${location.cityName}`;

        fireNonInteractiveTracking(category, action, label);
        dispatch(
          setErrorLog(`Error: ${error}. Found in Reco API`, interactionId)
        );
        if (currentScreen != screenType.MLA) {
          dispatch(setThankYouScreen());
        }
      });

    if (state.NC.campaign.campaign.mutualLeads) {
      if (Object.keys(MLAData).length > 0) {
        dispatch(setMLAScreen());
      } else {
        category = `${platform.name}_newcarleadform_multiple_dealers_not_shown`;
        action = "0";
        label = "";
        fireNonInteractiveTracking(category, action, label);
      }
    }
  };
};

/**
 * Dispatch action to set MLA screen
 */
export function setMLAScreen() {
  return {
    type: SET_MLA_SCREEN
  };
}

export function setMLAData(MLAData) {
  return {
    type: SET_MLA_DATA,
    MLAList: MLAData
  };
}

/**
 * Dispatch action to set Recommendation screen
 */
export function setRecoScreen() {
  return {
    type: SET_RECOMMENDATION_SCREEN
  };
}

export function setRecoData(recoData) {
  return {
    type: SET_RECOMMENDATION_DATA,
    recoList: recoData
  };
}

export const setLocationScreen = () => {
  return {
    type: SET_LOCATION_SCREEN
  };
};

export function setFormScreen() {
  return {
    type: SET_FORM_SCREEN
  };
}

export function setWhatsAppFormScreen() {
  return {
    type: SET_WHATSAPP_FORM_SCREEN
  };
}

/**
 * Dispatch action to set ThankYou screen
 */
export function setThankYouScreen() {
  return {
    type: SET_THANKYOU_SCREEN
  };
}

/**
 * Dispatch action to set Sorry screen
 */
export function setSorryScreen() {
  return {
    type: SET_SORRY_SCREEN
  };
}

export function setHide() {
  return (dispatch, getState) => {
    let message = "Leaving LeadForm (Current Store Provided in currentState)";
    dispatch(setInfoLog(message));

    let logs = getState().log;
    if (logs.length != 0) {
      submitBhriguLogs(logs);
      dispatch(clearLog());
    }

    dispatch({ type: SET_HIDE });

    let isCityChanged = getState().isCityChanged;
    if (isCityChanged) {
      RefreshPage();
    }
  };
}

export function setShow() {
  return {
    type: SET_SHOW
  };
}
