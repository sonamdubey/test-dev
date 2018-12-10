import { recommendation } from "../../../src/LeadForm/Reducers/Recommendation";
import { SET_RECOMMENDATION_DATA } from "../../../src/LeadForm/Actions/FormActionTypes";

const actionReco1 = () => ({
  type: SET_RECOMMENDATION_DATA,
  recoList: {
    "6666666666-1003": {
      campaign: {
        id: 5923,
        name: "Volkswagen Downtown"
      },
      carDetail: {
        hostUrl: "https://imgd.aeplcdn.com/",
        makeId: 20,
        makeName: "Volkswagen",
        modelId: 1003,
        modelName: "Polo",
        originalImgPath: "/cw/ec/22037/Volkswagen-Polo-Exterior-95634.jpg?wm=1"
      },
      priceOverview: { price: "₹ 8.29 Lakhs", priceStatus: 1 }
    }
  }
});

const actionReco2 = () => ({
  type: SET_RECOMMENDATION_DATA,
  recoList: null
});

const state = () => ({
  recoList: {
    list: {
      "6666666666-1003": {
        campaign: {
          id: 5923,
          name: "Volkswagen Downtown"
        },
        carDetail: {
          hostUrl: "https://imgd.aeplcdn.com/",
          makeId: 20,
          makeName: "Volkswagen",
          modelId: 1003,
          modelName: "Polo",
          originalImgPath:
            "/cw/ec/22037/Volkswagen-Polo-Exterior-95634.jpg?wm=1"
        },
        priceOverview: { price: "₹ 8.29 Lakhs", priceStatus: 1 }
      }
    }
  }
});

test("Reducer Recommendation ExpectedPayload {nullStateCheck, undefinedStateCheck, validStateCheck}", () => {
  expect(recommendation(state(), actionReco1())).toEqual({
    list: actionReco1().recoList
  });

  expect(recommendation(null, actionReco1())).toEqual({
    list: actionReco1().recoList
  });

  expect(recommendation(undefined, actionReco1())).toEqual({
    list: actionReco1().recoList
  });

  expect(() => recommendation(state(), actionReco2())).toThrow(
    "Recommendation action.RecoList input is null or undefined"
  );
  expect(() => recommendation(undefined, actionReco2())).toThrow(
    "Recommendation action.RecoList input is null or undefined"
  );
  expect(() => recommendation(null, actionReco2())).toThrow(
    "Recommendation action.RecoList input is null or undefined"
  );

  expect(recommendation(state(), { type: "DEFAULT" })).toEqual(state());
});
