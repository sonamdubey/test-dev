import {
  fireInteractiveTracking,
  fireNonInteractiveTracking
} from "../../utils/Analytics";
import store from "../store";
import { screenType } from "../Enum/ScreenEnum";

let page = store.getState().leadClickSource.page;
let pageName = page.page.name;
let platformName = page.platform.name;

export function trackNameErr(nameErr, formType) {
  let action,
    label = "";

  let category = `LeadForm-${platformName}-${pageName}`;

  if (nameErr == "Please enter your name") {
    action = `${formType}Open-Error_Name_NotFilled`;
  } else if (nameErr == "Please enter only alphabets") {
    action = `${formType}Open-Error_Name_NonAlphabet`;
  } else {
    action = `${formType}Open-Error_Name_OneChar`;
  }
  fireInteractiveTracking(category, action, label);
}

export function trackMobileErr(mobileErr, formType) {
  let action,
    label = "";

  let category = `LeadForm-${platformName}-${pageName}`;

  if (mobileErr == "Please enter your mobile number") {
    action = `${formType}Open-Error_Mobile_NotFilled`;
  } else if (mobileErr == "Mobile number should be of 10 digits") {
    action = `${formType}Open-Error_Mobile_Not10Digits`;
  } else {
    action = `${formType}Open-Error_Mobile_NotValid`;
  }
  fireInteractiveTracking(category, action, label);
}

export function trackEmailErr(emailErr, formType) {
  let action,
    label = "";

  let category = `LeadForm-${platformName}-${pageName}`;

  if (emailErr == "Please enter your email") {
    action = `${formType}Open-Error_Email_NotFilled`;
  } else {
    action = `${formType}Open-Error_Email_NotValid`;
  }

  fireInteractiveTracking(category, action, label);
}

export function trackCloseClick(modelId) {
  let state = store.getState();
  let currentScreen = state.screenData.screen;

  let category = `LeadForm-${platformName}-${pageName}`;
  let action, label;

  if (currentScreen == screenType.MLA) {
    category = `${platformName}_newcarleadform_flowA_multiple_dealers_closed`;
    action = Object.keys(state.NC.leadForm.MLASellers.list).length;
    label = "";
  } else if (currentScreen == screenType.Reco) {
    action = "RecoScreen-Close_Button_Click";
    label = Object.keys(state.NC.leadForm.recommendation.list).length;
  } else if (currentScreen == screenType.Form) {
    action = "FormOpen-Close_Button_Click";
    label = modelId;
  } else if (currentScreen == screenType.WhatsAppForm) {
    action = "WhatsAppFormOpen-Close_Button_Click";
    label = modelId;
  } else if (currentScreen == screenType.Sorry) {
    action = "SorryMessage-Close_Button_Click";
    label = "";
  } else if (currentScreen == screenType.Location) {
    action = "Location-Close_Button_Click";
    label = "";
  } else {
    return;
  }

  fireInteractiveTracking(category, action, label);
}

export function trackBrowserBack(modelId) {
  let state = store.getState();
  let currentScreen = state.screenData.screen;

  let category = `LeadForm-${platformName}-${pageName}`;

  if (currentScreen == screenType.MLA) {
    category = `${platformName}_newcarleadform_flowA_multiple_dealers_browser_back_clicked`;
    action = Object.keys(state.NC.leadForm.MLASellers.list).length;
    label = "";
  } else if (currentScreen == screenType.Reco) {
    action = "RecoScreen-Browser_Back_Click";
    label = Object.keys(state.NC.leadForm.recommendation.list).length;
  } else if (currentScreen == screenType.Form) {
    action = "FormOpen-Browser_Back_Click";
    label = modelId;
  } else if (currentScreen == screenType.WhatsAppForm) {
    action = "WhatsAppFormOpen-Browser_Back_Click";
    label = modelId;
  } else if (currentScreen == screenType.Sorry) {
    action = "SorryMessage-Browser_Back_Click";
    label = "";
  } else if (currentScreen == screenType.Location) {
    action = "Location-Browser_Back_Click";
    label = "";
  } else {
    return;
  }

  fireInteractiveTracking(category, action, label);
}

export function trackSubmit(selectedOptions, totalOptions) {
  let state = store.getState();

  let currentScreen = state.screenData.screen;
  let label, category, action;

  if (currentScreen == screenType.MLA) {
    category = `${platformName}_newcarleadform_multiple_dealers_submitted`;
    action = `${selectedOptions}/${totalOptions}`;
    label = "";
  } else {
    category = `LeadForm-${platformName}-${pageName}`;
    action = "RecoScreen-Submit";
    label = `${selectedOptions}/${totalOptions}`;
  }

  fireInteractiveTracking(category, action, label);
}

export function trackCheckItem(selectedOptions, checkEvent, optionsDictionary) {
  let state = store.getState();

  let currentScreen = state.screenData.screen;
  let label, category, action;
  let checkItem = getIndex(selectedOptions, checkEvent, optionsDictionary);
  let totalOptions = Object.keys(optionsDictionary).length;

  if (checkItem.selected) {
    if (currentScreen == screenType.MLA) {
      category = `${platformName}_newcarleadform_flowA_multiple_dealers_dealer_selected`;
      action = `${checkItem.index}/${totalOptions}`;
      label = "";
    } else {
      category = `LeadForm-${platformName}-${pageName}`;
      action = "RecoScreen-Suggestion_Checked";
      label = `${checkItem.index}/${totalOptions}`;
    }
  } else {
    if (currentScreen == screenType.MLA) {
      category = `${platformName}_newcarleadform_flowA_multiple_dealers_dealer_deselected`;
      action = `${checkItem.index}/${totalOptions}`;
      label = "";
    } else {
      category = `LeadForm-${platformName}-${pageName}`;
      label = `${checkItem.index}/${totalOptions}`;
      action = "RecoScreen-Suggestion_Unchecked";
    }
  }
  fireInteractiveTracking(category, action, label);
}

function findDifference(prevCheck, currentCheck) {
  let prevCheckLength = prevCheck.length;
  let currentCheckLength = currentCheck.length;
  let difference;
  if (prevCheckLength < currentCheckLength) {
    if (prevCheckLength != 0) {
      prevCheck.forEach(element => {
        currentCheck.splice(currentCheck.indexOf(element), 1);
      });
    }
    difference = currentCheck[0];
  } else {
    if (currentCheck != 0) {
      currentCheck.forEach(element => {
        prevCheck.splice(prevCheck.indexOf(element), 1);
      });
    }
    difference = prevCheck[0];
  }

  return difference;
}

function getIndex(prevCheck, currentCheck, optionsDictionary) {
  let index;
  let difference;
  let selected;
  let prevCheckLength = prevCheck.length;
  let currentCheckLength = currentCheck.length;

  difference = findDifference(prevCheck, currentCheck);
  selected = prevCheckLength < currentCheckLength ? true : false;
  index = Object.keys(optionsDictionary).indexOf(difference) + 1;

  return { selected, index };
}

export function trackSelectAll(isSelectAll, totalOptions) {
  let state = store.getState();
  let currentScreen = state.screenData.screen;
  let label, category, action;

  if (!isSelectAll) {
    if (currentScreen == screenType.Reco) {
      category = `LeadForm-${platformName}-${pageName}`;
      action = "RecoScreen-SelectAll_Clicked";
      label = totalOptions;
    } else {
      action = totalOptions;
      category = `${platformName}_newcarleadform_flowA_multiple_dealers_select_all_selected`;
      label = "";
    }
  } else {
    if (currentScreen == screenType.Reco) {
      action = "RecoScreen-RemoveAll_Clicked";
      category = `LeadForm-${platformName}-${pageName}`;
      label = totalOptions;
    } else {
      action = totalOptions;
      category = `${platformName}_newcarleadform_flowA_multiple_dealers_select_all_deselected`;
      label = "";
    }
  }

  fireInteractiveTracking(category, action, label);
}

export function trackScreenShown(totalOptions) {
  let state = store.getState();

  let currentScreen = state.screenData.screen;
  let label, category, action;

  if (currentScreen == screenType.MLA) {
    category = `${platformName}_newcarleadform_multiple_dealers_shown`;
    action = totalOptions;
    label = "";
  } else {
    category = `LeadForm-${platformName}-${pageName}`;
    action = "RecoScreen-Suggestions_Shown";
    label = `${totalOptions}_${state.NC.campaign.featuredCarData.modelId}_${
      state.location.cityName
    }`;
  }

  fireNonInteractiveTracking(category, action, label);
}

export function trackSkipClick(totalOptions) {
  let category = `${platformName}_newcarleadform_multiple_dealers_skipped`;
  let action = totalOptions;
  let label = "";

  fireInteractiveTracking(category, action, label);
}

export function trackSkipShown(MLALength) {
  fireNonInteractiveTracking(
    `${platformName}_newcarleadform_multiple_dealers_shown_skip_shown`,
    MLALength,
    ""
  );
}
