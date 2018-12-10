import {
  SET_RECOMMENDATION,
  SET_MLA,
  SET_SORRY,
  SET_THANKYOU,
  SET_FORMSCREEN,
  SET_HIDE,
  SET_MLA_SCREEN,
  SET_RECOMMENDATION_SCREEN,
  SET_SORRY_SCREEN,
  SET_THANKYOU_SCREEN,
  SET_FORM_SCREEN
} from "../../../src/LeadForm/Actions/ScreenActionTypes";
import { screen } from "../../../src/LeadForm/Reducers/Screen";
import { screenType } from "../../../src/LeadForm/Enum/ScreenEnum";

const actionMLA = () => ({
  type: SET_MLA_SCREEN
});

const actionReco = () => ({
  type: SET_RECOMMENDATION_SCREEN
});

const actionSorry = () => ({
  type: SET_SORRY_SCREEN
});

const actionThankYou = () => ({
  type: SET_THANKYOU_SCREEN
});

const actionForm = () => ({
  type: SET_FORM_SCREEN
});

const actionHide = () => ({
  type: SET_HIDE
});

const state = () => ({
  screen: screenType.Hide
});

test("Reducer Screen {MLA} {nullStateCheck, undefinedStateCheck, validStateCheck}", () => {
  expect(screen(state(), actionMLA())).toEqual({ screen: screenType.MLA });
  expect(screen(null, actionMLA())).toEqual({ screen: screenType.MLA });
  expect(screen(undefined, actionMLA())).toEqual({
    screen: screenType.MLA
  });
});

test("Reducer Screen {Reco} {nullStateCheck, undefinedStateCheck, validStateCheck}", () => {
  expect(screen(state(), actionReco())).toEqual({
    screen: screenType.Reco
  });
  expect(screen(null, actionReco())).toEqual({ screen: screenType.Reco });
  expect(screen(undefined, actionReco())).toEqual({
    screen: screenType.Reco
  });
});

test("Reducer Screen {Sorry} {nullStateCheck, undefinedStateCheck, validStateCheck}", () => {
  expect(screen(state(), actionSorry())).toEqual({
    screen: screenType.Sorry
  });
  expect(screen(null, actionSorry())).toEqual({ screen: screenType.Sorry });
  expect(screen(undefined, actionSorry())).toEqual({
    screen: screenType.Sorry
  });
});

test("Reducer Screen {ThankYou} {nullStateCheck, undefinedStateCheck, validStateCheck}", () => {
  expect(screen(state(), actionThankYou())).toEqual({
    screen: screenType.ThankYou
  });
  expect(screen(null, actionThankYou())).toEqual({
    screen: screenType.ThankYou
  });
  expect(screen(undefined, actionThankYou())).toEqual({
    screen: screenType.ThankYou
  });
});

test("Reducer Screen {Form} {nullStateCheck, undefinedStateCheck, validStateCheck}", () => {
  expect(screen(state(), actionForm())).toEqual({
    screen: screenType.Form
  });
  expect(screen(null, actionForm())).toEqual({ screen: screenType.Form });
  expect(screen(undefined, actionForm())).toEqual({
    screen: screenType.Form
  });
});

test("Reducer Screen {Default} {nullStateCheck, undefinedStateCheck, validStateCheck}", () => {
  expect(screen(state(), { type: "DEFAULT" })).toEqual({
    screen: screenType.Hide
  });
});
