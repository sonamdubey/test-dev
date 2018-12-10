import { SET_PAGE } from "../actionTypes/index";

let pageDetails = JSON.parse(sessionStorage.getItem("PAGE"));
let platformDetails = JSON.parse(sessionStorage.getItem("PLATFORM"));
let uninitializedState = {
  id: -1,
  name: ""
};

let getPageDetails = function() {
  let pageDetails = JSON.parse(sessionStorage.getItem("PAGE"));
  let platformDetails = JSON.parse(sessionStorage.getItem("PLATFORM"));
  pageDetails = pageDetails ? pageDetails : uninitializedState;
  platformDetails = platformDetails ? platformDetails : uninitializedState;
  return {
    page: pageDetails,
    applicationId: 1,
    platform: platformDetails
  };
};

export const page = (state, action) => {
  let stateFromSession = getPageDetails();
  switch (action.type) {
    case SET_PAGE:
      return { ...stateFromSession, ...action.pageData };
    default:
      return stateFromSession;
  }
};
