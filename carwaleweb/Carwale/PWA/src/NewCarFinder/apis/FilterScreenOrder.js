import { FILTER_SCREEN_ENDPOINT } from '../constants'

/**
 * This returns a promise to fetch
 * filter screen oredr from db
 * @return {Promise<>}
 */
const get = () => {
    return fetch(FILTER_SCREEN_ENDPOINT)
    .then(response => {
        if (!response.ok) { throw response }
        return response.json()
    })
}
const FilterScreenApi = {
    get
}


export default FilterScreenApi
