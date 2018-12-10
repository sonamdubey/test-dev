import { SET_RECOMMENDATION_DATA } from "../Actions/FormActionTypes";

let initialRecoState = {
  list: {}
};

export const recommendation = (state = initialRecoState, action) => {
  switch (action.type) {
    //When SetRecoScreen Action Trigerred
    case SET_RECOMMENDATION_DATA: {
      if (action.recoList == null || action.recoList == undefined) {
        throw "Recommendation action.RecoList input is null or undefined";
      }
      return {
        list: action.recoList
      };
    }

    default:
      return state;
  }
};
