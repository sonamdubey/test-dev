import { similarBikesEMI } from '../actionTypes/SimilarBikesEMI'


import { fromJS } from 'immutable'
const getSimilarBikesData = (dispatch, modelEmiObj) => {
  var xhr = new XMLHttpRequest();
  xhr.onreadystatechange = () => {
    if (xhr.readyState === 4) {
      if (xhr.status === 200) {
        let bikeList = JSON.parse(xhr.responseText);
        let payload =  {
          data: bikeList.map(json => ({
            makeId: json.makeId,
            makeName: json.makeName,
            modelId: json.modelId,
            modelName: json.modelName,
            modelImage: json.hostUrl + '310x174' +json.originalImagePath,
            onRoadPriceAmount: json.onRoadPrice
          })),
          modelEmiObj
        }
        dispatch(
          {
            type: similarBikesEMI.FETCH_SIMILAR_BIKES_EMI
          , payload})
      }
      else {
        return null;
      }
    }
  }
  let url = '/api/pwa/similarbikes/model/' + modelEmiObj.modelId + '/finance/?topCount=9&cityId='+modelEmiObj.cityId;
  xhr.open('GET', url);
  xhr.send();

}

export const fetchSimilarBikes = (modelEmiObj) => (dispatch) => {
    getSimilarBikesData(dispatch, modelEmiObj)
}
export const updateSimilarBikesEmi = (similarBikesList, modelEmiObj) => (dispatch) => {
    dispatch({ 
      type: similarBikesEMI.UPDATE_EMI,
      payload:{
        similarBikesList,
        modelEmiObj
      } 
     })
}