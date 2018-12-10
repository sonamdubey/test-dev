import {
  SET_INFO_LOG,
  SET_DEBUG_LOG,
  SET_ERROR_LOG,
  CLEAR_LOG
} from "../Actions/LogActionTypes";
import { logLevel } from "../Enum/LogLevel";
import { getStateFromStore } from "../utils/StoreData";

export function setInfoLog(message, interactionId) {
  return (dispatch, getState) => {
    const state = getState();
    dispatch({
      type: SET_INFO_LOG,
      logData: {
        message,
        level: logLevel.INFO,
        currentState: getStateFromStore(state),
        interactionId:
          interactionId == undefined ? state.interactionId : interactionId
      }
    });
  };
}

export function setErrorLog(message, interactionId) {
  return (dispatch, getState) => {
    const state = getState();
    dispatch({
      type: SET_ERROR_LOG,
      logData: {
        message,
        level: logLevel.ERROR,
        currentState: getStateFromStore(state),
        interactionId:
          interactionId == undefined ? state.interactionId : interactionId
      }
    });
  };
}

export function setDebugLog(message, currentState, nextState, interactionId) {
  return (dispatch, getState) => {
    const state = getState();
    dispatch({
      type: SET_DEBUG_LOG,
      logData: {
        message,
        level: logLevel.DEBUG,
        currentState: currentState,
        nextState: nextState,
        interactionId:
          interactionId == undefined ? state.interactionId : interactionId
      }
    });
  };
}

export function clearLog() {
  return {
    type: CLEAR_LOG
  };
}
