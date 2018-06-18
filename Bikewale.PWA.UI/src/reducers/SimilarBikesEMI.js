import { combineReducers } from 'redux-immutable'
import { fromJS } from 'immutable'
import { similarBikesEMI } from '../actionTypes/SimilarBikesEMI'
import { EmiCalculation } from '../components/Finance/EMICalculations'
const onRoadPriceLabel = "On-Road Price";
const emiLabel = "EMI";
var initialState = fromJS({
  data: [
    // {
    //   makeId: 0,
    //   makeName: "",
    //   modelId: 0,
    //   modelName: "",
    //   modelImage: "",
    //   onRoadPriceLabel: "",
    //   onRoadPriceAmount: 0,
    //   emiLabel: "",
    //   emiStart: 0
    // }
  ],
  modelId: -1,
  cityId: -1,
  downPayment: 0,
  tenure: 0,
  rateOfInt: 0,
  IsFetching: false
})

export function SimilarBikesEMI(state, action) {
  try {
    if (state == undefined || (state != undefined && state.size == 0)) {
      return initialState;
    }

    switch (action.type) {
      case similarBikesEMI.FETCHING_SIMILAR_BIKES:
        return state.setIn(['IsFetching'], true)
      case similarBikesEMI.FETCH_SIMILAR_BIKES_EMI:
        let modelEmiObj = action.payload.modelEmiObj
        return fromJS({
          ...state.toJS(),
          data: action.payload.data.map(x => {
            return {
            ...x,
            onRoadPriceLabel: onRoadPriceLabel,
            emiLabel: emiLabel
          }}),
          modelId: modelEmiObj.modelId,
          cityId: modelEmiObj.cityId,
          IsFetching: true
        })
      case similarBikesEMI.UPDATE_EMI:
        modelEmiObj = action.payload.modelEmiObj
        return fromJS({
          data: action.payload.similarBikesList.data.map(x => {
            let minDp = .1 * x.onRoadPriceAmount
            let maxDp = .4 * x.onRoadPriceAmount
            let currentDp = modelEmiObj.downPayment;
            let downPayment = currentDp >= minDp && currentDp <= maxDp ? currentDp : currentDp < minDp ? minDp : maxDp
            return {
            ...x,
            emiStart: EmiCalculation(x.onRoadPriceAmount - downPayment, modelEmiObj.tenure, modelEmiObj.rateOfInt) 
          }}),
          downPayment: modelEmiObj.downPayment,
          tenure: modelEmiObj.tenure,
          rateOfInt: modelEmiObj.rateOfInt,
          modelId: modelEmiObj.modelId,
          cityId: modelEmiObj.cityId,
          IsFetching: false,
        })
      case similarBikesEMI.FETCH_SIMILAR_BIKES_FAILED:
        return initialState
      default:
        return state
    }
  }
  catch (err) {
    console.log(err)
    return state;
  }
}
