import { SET_PAGE } from "../actionTypes/index";

export const setPage = pageData => {
  return {
    type: SET_PAGE,
    pageData
  };
};
