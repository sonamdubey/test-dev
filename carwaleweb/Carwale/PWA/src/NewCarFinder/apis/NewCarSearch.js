
import { NEWCAR_SEARCH_ENDPOINT } from '../constants'
import { serialzeObjectToQueryString } from '../../utils/Common'

/**
 * This returns a promise to fetch
 * suggested budget in a given city
 * @param {int} options
 * @return {Promise}
 */
const get = (options) => {
    const queryString = typeof options === "object" ? serialzeObjectToQueryString(options) : options
    return fetch(NEWCAR_SEARCH_ENDPOINT+"?"+ queryString)
    .then(response => {
        if (!response.ok) { throw response }
        return response.json()
    })
}

const getCount = (options) => {
    options = {
        countOnly:true,
        ...options
    }
    return get(options)
}

const getPage = (options, pageNo=1, pageSize=10) => {
    options = {
        countOnly:false,
        pageNo,
        pageSize,
        ...options
    }
    return get(options)
}

const NewCarSearchApi = {
    get,
    getCount,
    getPage
}
export default NewCarSearchApi
