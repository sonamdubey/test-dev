import { similarBikesEMI } from '../actionTypes/SimilarBikesEMI'

const fetchSimilarBikesData = () => {
  return {
    type: similarBikesEMI.FETCH_SIMILAR_BIKES_EMI
  }
}

export const fetchSimilarBikes = () => (dispatch) => {
  dispatch(fetchSimilarBikesData())
}
