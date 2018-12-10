
import { SUGGESTED_BODYTYPE_ENDPOINT } from '../constants'
import { serialzeObjectToQueryString } from '../../utils/Common'

/**
 * This returns a promise to fetch
 * suggested bodytype in a given budget,city
 * @return {Promise<>}
 */
const get = (options) => {
    return fetch(SUGGESTED_BODYTYPE_ENDPOINT+"?"+serialzeObjectToQueryString(options))
    .then(response => {
        if (!response.ok) { throw response }
        return response.json()
    })
}
const BodyTypeApi = {
    get
}
export default BodyTypeApi
