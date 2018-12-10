
import { MAKE_ENDPOINT } from '../constants'
import { serialzeObjectToQueryString } from '../../utils/Common'
/**
 * This returns a promise to fetch
 * fuel types availability for the selected filters
 * @return {Promise<>}
 */
const get = (options) => {
    return fetch(MAKE_ENDPOINT+"?CountOnly=true&"+serialzeObjectToQueryString(options))
    .then(response => {
        if (!response.ok) { throw response }
        return response.json()
    })
}
const MakeApi = {
    get
}
export default MakeApi
