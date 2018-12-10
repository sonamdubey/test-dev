import { SET_LOCATION, PREFILL_LOCATION } from "../Actions/FormActionTypes";
import { SET_LEAD_SOURCE_CITY } from "../Actions/IsCitySetActionType";
import { SET_CITY_CHANGE } from "../Actions/IsCityChanged";

/**
 * Call the Api to post lead data
 * and return corresponding action to reducers
 */
export const PrefillLocation = location => {
  return {
    type: PREFILL_LOCATION,
    location
  };
};

export const SetLocation = location => {
  return {
    type: SET_LOCATION,
    location
  };
};

export const setLeadSourceCity = isCitySet => {
  return {
    type: SET_LEAD_SOURCE_CITY,
    isCitySet
  };
};

export const setCityChange = isCityChanged => {
  return {
    type: SET_CITY_CHANGE,
    isCityChanged
  };
};
