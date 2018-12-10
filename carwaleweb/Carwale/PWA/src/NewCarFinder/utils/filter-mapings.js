import ncfDefaultFilters from '../../filterPlugin/constants/NCFdefaultfilters';
import { SEO_TYPE } from '../constants/index'
import { serialzeObjectToQueryString } from '../../utils/Common'
import Cookies from "js-cookie";
import { NEWCARFINDER_RESULTS_ENDPOINT } from '../constants/index'
/*
* This file contain all the
* required function to map
* url  to qs
*/

/**
 * @param props - Route Component Props
 * @param type - SEO_TYPE, based on which props handling changes
 */
export const mapPropsObjectFromSEOUrls = (props,type) => {
    let qsObj
    switch(type) {
        case SEO_TYPE.SEO_SINGLE:
            let dimension = props.match.params[0]
            qsObj = getQSForName(dimension)
            qsObj.cityId = parseInt(Cookies.get('_CustCityIdMaster')) || -1
            break
        default:
            qsObj = {}
    }
    let qs = serialzeObjectToQueryString(qsObj)
    const location = {
        ... props.location,
        search: qs ? '?' + qs : '',
        pathname: NEWCARFINDER_RESULTS_ENDPOINT
    }
    return {
        ... props,
        location
    }
}

/**
 * @param {string} name - URL part that needs to be parsed
 * @returns returns object that contain filter selections
 */
const getQSForName = (name) => {
    let selectedObj =  getBodyTypeQS(name)
    if(selectedObj){
        return selectedObj
    }
    selectedObj = getFuelTypeQS(name)
    if(selectedObj){
        return selectedObj
    }
    selectedObj =  getMakeQS(name)
    if(selectedObj){
        return selectedObj
    }
    selectedObj =  getTransmissionTypeQS(name)
    if(selectedObj){
        return selectedObj
    }
    return {}
}

/**
 * @param {string} name
 * @returns budget object representing filter selection
 */
const getBudgetQS = (name) => {
    if(typeof(name) != "undefined" && !isNaN(name)){
        let budget = Number(name) * 100000
        return { budget : budget  }
    }
    return null
}
/**
 * @param {string} name
 * @returns fuel type  object representing filter selection
 */
const getFuelTypeQS = (name) => {
    if(typeof(name) != "undefined"){
        name = name.toLowerCase()
        let selected
        let fuelTypes = ncfDefaultFilters().newCarFinder.fuelType.data
        fuelTypes.forEach(function(element){
            if(element["name"].replace(/([^a-zA-Z]*)/g,'').toLowerCase() == name){
                selected = element["id"]
            }
        })
        return selected ? { fuelTypeIds : selected  } : null
    }
    return null
}
/**
 * @param {string} name
 * @returns make object representing filter selection
 */
const getMakeQS = (name) => {
    if(typeof(name) != "undefined"){
        name = name.toLowerCase()
        let selected
        let makes = ncfDefaultFilters().newCarFinder.make.data
        makes.forEach(function(element){
            if(element["makeName"].replace(/([^a-zA-Z]*)/g,'').toLowerCase() == name){
                selected = element["makeId"]
            }
        })
        return selected ?  { carMakeIds : selected  } : null
    }
    return null
}
/**
 * Returns body type object and for minvan or van kind of
 * scenarios it returns Minivan/Van id
 * (name should match at least on one of part separated by slash)
 * @param {string} name
 * @returns body type object representing filter selection
 */
const getBodyTypeQS = (name) => {
    if(typeof(name) != "undefined"){
        name = name.toLowerCase()
        let selected
        let bodyTypes = ncfDefaultFilters().newCarFinder.bodyType.data
        bodyTypes.forEach(function(element){
            let bodyTypeNames = element["name"].split('/')
            bodyTypeNames.forEach(function(partEle){
                if(name == partEle.replace(/([^a-zA-Z]*)/g,'').toLowerCase()){
                    selected = element["id"]
                }
            })
        })
        return selected ?  { bodyStyleIds : selected  } : null
    }
    return null
}
/**
 * @param {string} name
 * @returns transmission object representing filter selection
 */
const getTransmissionTypeQS = (name) => {
    if(typeof(name) != "undefined"){
        let selected
        name = name.toLowerCase()
        let transmissionTypes = ncfDefaultFilters().newCarFinder.transmissionType.data
        transmissionTypes.forEach(function(element){
            if(element["name"].replace(/([^a-zA-Z]*)/g,'').toLowerCase() == name){
                selected = element["id"]
            }
        })
        return selected ?  { transmissionTypeIds : selected  } : null
    }
    return null
}
/**
 * Returns object with id and for 8plus it
 * return id corresponding to 8+ seaters
 * @param {string} name
 * @returns seat object representing filter selection
 */
const getSeatQS = (name) => {
    if(typeof(name) != "undefined"){
        name = name.toLowerCase()
        let selected
        let seats = ncfDefaultFilters().newCarFinder.seats.data
        seats.forEach(function(element){
            if(element["name"].replace(/([^0-9+]*)/g,'').replace("+","plus").toLowerCase() == name){
                selected = element["id"]
            }
        })
        return selected ? { seats : selected  } : null
    }
    return null
}
