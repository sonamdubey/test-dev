import { combineReducers } from 'redux-immutable'
import { fromJS } from 'immutable'

import { similarBikesEMI } from '../actionTypes/SimilarBikesEMI'

var initialState = fromJS({
  data: [
    {
      makeId: 7,
      makeName: "Honda",
      modelId: 100,
      modelName: "CBR500R",
      modelImage: "https://imgd.aeplcdn.com//310x174//bw/upcoming/honda-cb500x-421.jpg",
      onRoadPriceLabel: "On-Road Price",
      onRoadPriceAmount: 235000,
      emiLabel: "EMI",
      emiStart: 16734
    },
    {
      makeId: 7,
      makeName: "Honda",
      modelId: 1142,
      modelName: "Activa",
      modelImage: "https://imgd.aeplcdn.com//310x174//bw/models/honda-activa-5g.jpg",
      onRoadPriceLabel: "On-Road Price",
      onRoadPriceAmount: 75000,
      emiLabel: "EMI",
      emiStart: 12000
    },
    {
      makeId: 7,
      makeName: "Honda",
      modelId: 59,
      modelName: "CB Shine",
      modelImage: "https://imgd.aeplcdn.com//310x174//bw/models/honda-shine.jpg",
      onRoadPriceLabel: "On-Road Price",
      onRoadPriceAmount: 123000,
      emiLabel: "EMI",
      emiStart: 14534
    },
    {
      makeId: 7,
      makeName: "Honda",
      modelId: 693,
      modelName: "CB Unicorn 150",
      modelImage: "https://imgd.aeplcdn.com//310x174//bw/models/honda-cb-unicorn-150.jpg",
      onRoadPriceLabel: "On-Road Price",
      onRoadPriceAmount: 112000,
      emiLabel: "EMI",
      emiStart: 10205
    }
  ]
})

export function SimilarBikesEMI(state, action) {
  try {
    if (!state) {
      return initialState;
    }

    switch (action.type) {
      case similarBikesEMI.FETCH_SIMILAR_BIKES_EMI:
        return initialState;

      default:
        return state
    }
  }
  catch (err) {
    console.log(err)
    return state;
  }
}
