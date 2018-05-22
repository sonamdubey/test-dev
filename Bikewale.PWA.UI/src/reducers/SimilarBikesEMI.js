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
  ]
})

export function SimilarBikesEMI(state , action) {
  try {
    if (state != null && state.size == 0) {
      return initialState;
    }

    switch (action.type) {
      case similarBikesEMI.FETCH_SIMILAR_BIKES_EMI:
        let modelEmiObj = action.payload.modelEmiObj
        return fromJS({
          data: action.payload.data.map(x => ({
            ...x,
            emiStart: EmiCalculation(x.onRoadPriceAmount - (x.onRoadPriceAmount * .3), modelEmiObj.tenure, modelEmiObj.rateOfInt),
            onRoadPriceLabel: onRoadPriceLabel,
            emiLabel: emiLabel
          }))
        })
      case similarBikesEMI.UPDATE_EMI:
        modelEmiObj = action.payload.modelEmiObj
        return fromJS({
          data: action.payload.similarBikesList.data.map(x => ({
            ...x,
            emiStart: EmiCalculation(x.onRoadPriceAmount - (x.onRoadPriceAmount * .3), modelEmiObj.tenure, modelEmiObj.rateOfInt) 
          }))
        })

      default:
        return state
    }
  }
  catch (err) {
    console.log(err)
    return state;
  }
}
