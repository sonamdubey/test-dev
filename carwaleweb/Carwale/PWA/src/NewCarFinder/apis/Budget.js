
import { SUGGESTED_BUDGET_ENDPOINT } from '../constants'

/**
 * This returns a promise to fetch
 * suggested budget in a given city
 * @param {int} cityId
 * @return {Promise<{min, max, suggested}>}
 */
const get = (cityId) => {
    if(cityId){
        return fetch(SUGGESTED_BUDGET_ENDPOINT+"?cityId="+cityId)
        .then(response => {
            if (!response.ok) { throw response }
            return response.json()
        })
    }
    else{
        return Promise.reject(`Invalid parameters: cityId:${cityId}`)
    }
}
const BudgetApi = {
    get
}
export default BudgetApi
