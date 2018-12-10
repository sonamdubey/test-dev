
import { FUELTYPE_ENDPOINT } from '../constants'
import { serialzeObjectToQueryString } from '../../utils/Common'
/**
 * This returns a promise to fetch
 * fuel types availability for the selected filters
 * @return {Promise<>}
 */
const get = (options) => {
    return fetch(FUELTYPE_ENDPOINT+"?"+serialzeObjectToQueryString(options))
    .then(response => {
        if (!response.ok) { throw response }
        return response.json()
    })
}
const FuelTypeApi = {
    get
}
export default FuelTypeApi
