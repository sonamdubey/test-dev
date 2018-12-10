
import { VERSION_CITY_PRICE_ENDPOINT } from '../constants'

/**
 * This returns a promise to fetch
 * version price in a given city
 * @param {int} cityId
 * @param {int} versionId
 * @return {Promise<T>}
 */
const getVersionPriceByCityId = (versionId,cityId) => {
    if(versionId && cityId){
        return fetch(VERSION_CITY_PRICE_ENDPOINT+versionId+"/?cityId="+cityId)
        .then(response => {
            if (!response.ok) { throw response }
            return response.json()
        })
    }
    else{
        return Promise.reject(`Invalid parameters: cityId:${cityId}, versionId:${versionId}`)
    }
}
const PricesApi = {
    getVersionPriceByCityId
}
export default PricesApi
