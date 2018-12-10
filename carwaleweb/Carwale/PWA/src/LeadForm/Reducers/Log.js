import {
  SET_INFO_LOG,
  SET_DEBUG_LOG,
  SET_ERROR_LOG,
  CLEAR_LOG
} from "../Actions/LogActionTypes";

let initialLogState = [];

export const log = (state = initialLogState, action) => {
  switch (action.type) {
    //When SetInfoLog Action Triggered
    case SET_INFO_LOG: {
      return [...state, { ...action.logData, timestamp: Date.now() }];
    }

    //When SetErrorLog Action Triggered
    case SET_ERROR_LOG: {
      return [...state, { ...action.logData, timestamp: Date.now() }];
    }

    //When SetDebugLog Action Triggered
    case SET_DEBUG_LOG: {
      return [...state, { ...action.logData, timestamp: Date.now() }];
    }

    case CLEAR_LOG:
      return [];

    default:
      return state;
  }
};
