import { buyerInfo } from "../../../src/LeadForm/Reducers/Buyer";
import {
  SET_BUYER_DATA,
  SET_EMAIL
} from "../../../src/LeadForm/Actions/FormActionTypes";

const actionBuyer1 = () => ({
  type: SET_BUYER_DATA,
  buyerInfo: null
});

const actionBuyer2 = () => ({
  type: SET_BUYER_DATA,
  buyerInfo: { name: "Ritesh", mobile: "8109378233" }
});

const actionDefault = () => ({
  type: "DEFAULT"
});

const actionEmail1 = () => ({
  type: SET_EMAIL,
  email: null
});

const actionEmail2 = () => ({
  type: SET_EMAIL,
  email: "sdf@kjb.com"
});

const state = () => ({
  name: "XYZ",
  mobile: "9109312457",
  email: "ssdf@kjb.com"
});

test("Reducer BuyerInfo ExpectedPayload {nullStateCheck, undefinedStateCheck, validStateCheck}", () => {
  //const expectedPayload = {name:'Ritesh', mobile:'8109378233', email:''};

  expect(buyerInfo(null, actionBuyer2())).toEqual({
    name: actionBuyer2().buyerInfo.name,
    mobile: actionBuyer2().buyerInfo.mobile
  });
  expect(buyerInfo(undefined, actionBuyer2())).toEqual({
    name: actionBuyer2().buyerInfo.name,
    mobile: actionBuyer2().buyerInfo.mobile,
    email: ""
  });
  expect(buyerInfo(state(), actionBuyer2())).toEqual({
    name: actionBuyer2().buyerInfo.name,
    mobile: actionBuyer2().buyerInfo.mobile,
    email: state().email
  });
  expect(() => buyerInfo(state(), actionBuyer1())).toThrow(
    "Buyer {BuyerInfo} action.BuyerInfo input is null or undefined"
  );
  expect(() => buyerInfo(null, actionBuyer1())).toThrow(
    "Buyer {BuyerInfo} action.BuyerInfo input is null or undefined"
  );

  expect(buyerInfo(null, actionEmail2())).toEqual({
    email: actionEmail2().email
  });
  expect(buyerInfo(undefined, actionEmail2())).toEqual({
    name: "",
    mobile: "",
    email: actionEmail2().email
  });
  expect(buyerInfo(state(), actionEmail2())).toEqual({
    name: state().name,
    mobile: state().mobile,
    email: actionEmail2().email
  });
  expect(() => buyerInfo(state(), actionEmail1())).toThrow(
    "Buyer {BuyerInfo} action.email input is null or undefined"
  );
  expect(() => buyerInfo(null, actionEmail1())).toThrow(
    "Buyer {BuyerInfo} action.email input is null or undefined"
  );

  expect(buyerInfo(state(), actionDefault())).toEqual(state());
});
