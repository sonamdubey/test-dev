import { SET_MLA_DATA } from "../Actions/FormActionTypes";

let initialMLAState = {
  list: {}
};

export const MLASellers = (state = initialMLAState, action) => {
  switch (action.type) {
    //When SetMLAScreen Action Trigerred
    case SET_MLA_DATA:
      return {
        list: action.MLAList
      };

    default:
      return state;
  }
};
