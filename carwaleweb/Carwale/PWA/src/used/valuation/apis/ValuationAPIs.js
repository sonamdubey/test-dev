import {
    serialzeObjectToQueryString
} from './../../../utils/Common'
/**
 * This returns a promise to fetch
 * get makes
 * @return {Promise<>}
 */
const get = (url, options, responseType = 'json') => {
    let fetchUrl = url
    if(options){
        fetchUrl = url + '?' + serialzeObjectToQueryString(options)
    }
    return fetch(fetchUrl)
        .then(response => {
            if (!response.ok) {
                throw response
            }
            if(responseType==='json')
                return response.json()
            return response.text();
        })
}
const ApiCall = {
    get
}
export default ApiCall
