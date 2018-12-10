
import { DEFAULT_FILTERS_ENDPOINT } from '../constants'

/**
 * This returns a promise to fetch
 * all filters available
 * @return {Promise<{allFilters}>}
 */
const get = () => {
        return fetch(DEFAULT_FILTERS_ENDPOINT)
        .then(response => {
            if (!response.ok) { throw response }
            return response.json()
        })
}
const DefaultFiltersApi = {
    get
}
export default DefaultFiltersApi
