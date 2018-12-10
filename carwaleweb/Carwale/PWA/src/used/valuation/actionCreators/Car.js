import { APPLY_CAR_FILTER,CAR_VALIDATION_STATE, CHANGE_CAR_STATE, REQUEST_MAKE_DATA, REQUEST_MODEL_DATA, REQUEST_VERSION_DATA, SELECT_MAKE, SELECT_MODEL, SELECT_VERSION, SET_MAKE_DATA, SET_MODEL_DATA, SET_VERSION_DATA, SHOW_CAR_POPUP,CLEAR_CAR_SELECTION_DATA,RESET_CAR_SELECTED_DATA, RESET_INITIAL_STATE } from '../actionTypes';
import ApiCall from '../apis/ValuationAPIs';
import { MAKE_END_POINT, MODEL_END_POINT, VERSION_END_POINT,VERSION_DETAILS_END_POINT,MODEL_DETAILS_END_POINT, MIN_MANUFACTURING_YEAR,DEFAULT_MANUFACTURING_YEAR } from '../constants/index';
import { validateManufacturingDetails } from '../utils/validations';

const carSelectionPopup = value => {
    return {
        type: SHOW_CAR_POPUP,
        value
    }
}
const requestMakeData = value => {
    return {
        type: REQUEST_MAKE_DATA,
        value
    }
}
const setMakeSelectionData = value => {
    return {
        type: SET_MAKE_DATA,
        value
    }
}
const selectMake = value => {
    return {
        type: SELECT_MAKE,
        value
    }
}
const requestModelData = value => {
    return {
        type: REQUEST_MODEL_DATA,
        value
    }
}
const setModelSelectionData = value => {
    return {
        type: SET_MODEL_DATA,
        value
    }
}
const selectModel = value => {
    return {
        type: SELECT_MODEL,
        value
    }
}
const requestVersionData = value => {
    return {
        type: REQUEST_VERSION_DATA,
        value
    }
}
const setVersionSelectionData = value => {
    return {
        type: SET_VERSION_DATA,
        value
    }
}
const selectVersion = value => {
    return {
        type: SELECT_VERSION,
        value
    }
}
const changeCarState = value => {
    return {
        type: CHANGE_CAR_STATE,
        value
    }
}
const applyCarFilter = value => {
    return {
        type: APPLY_CAR_FILTER,
        value
    }
}
const updateCarValidationState = value =>{
    return{
        type : CAR_VALIDATION_STATE,
        value
    }
}
const isValidToFetchMake = (state)=>{
    return state && typeof state.usedCar.valuation !=='undefined' && validateManufacturingDetails(state.usedCar.valuation.manufacturingDetails);
}
const clearCarSelectionData = () =>{
    return{
        type:CLEAR_CAR_SELECTION_DATA
    }
}
const resetCarSelectedData = ()=>{
    return{
        type:RESET_CAR_SELECTED_DATA
    }
}
const resetCarIntialState = () =>{
    return {
        type: RESET_INITIAL_STATE
    }
}
const fillMakes = (options,dispatch,waitTime) =>{
    waitTime = typeof waitTime ==='undefined' ? 0 : waitTime;
    ApiCall
    .get(MAKE_END_POINT, options)
    .then(data => {
        setTimeout(() => dispatch(setMakeSelectionData({
            data: data,
        })),waitTime)
    })
    .catch(error => {
        setTimeout(() => dispatch(setMakeSelectionData({
            data: [],
        })),waitTime)
    })
}

const fillModels = (options,dispatch,waitTime) =>{
    waitTime = typeof waitTime ==='undefined' ? 0 : waitTime;
    ApiCall
    .get(MODEL_END_POINT, options)
    .then(data => {
        setTimeout(() => dispatch(setModelSelectionData({
            data: data,
        })), waitTime)
    })
    .catch(error => {
        setTimeout(() => dispatch(setModelSelectionData({
            data: [],
        })), waitTime)
    })
}

const fillVersions = (options,dispatch,waitTime) =>{
    waitTime = typeof waitTime ==='undefined' ? 0 : waitTime;
    ApiCall
    .get(VERSION_END_POINT,options)
    .then(data => {
        setTimeout(() => dispatch(setVersionSelectionData({
            data: data,
        })), waitTime)
    })
    .catch(error => {
        setTimeout(() => dispatch(setVersionSelectionData({
            data: [],
        })), waitTime)
    })
}
const prefillCarDetails = (state,dispatch) =>{
    let options = {};
    options["type"]="used";
    options["year"] = state.manufacturingDetails.year;
    options["module"]= 2; // sellcar
    fillMakes(options,dispatch,0);
    options["makeid"] = state.car.selected.make.id;
    fillModels(options,dispatch,0);
    options["modelid"] = state.car.selected.model.id;
    fillVersions(options,dispatch,0);
}
export const preSelectCarDetails = (value) => (dispatch,getState) =>{
    if(value){
        ApiCall
        .get(VERSION_DETAILS_END_POINT,value)
        .then(versionData =>{
            dispatch(selectMake({id:versionData.MakeId,name:versionData.MakeName}));
            ApiCall
            .get(MODEL_DETAILS_END_POINT,{modelid:versionData.ModelId})
            .then(modelData =>{
                dispatch(selectModel({id:versionData.ModelId,name:versionData.ModelName,rootName:modelData.RootName}));
                dispatch(selectVersion({id:versionData.VersionId,name:versionData.VersionName}));
                prefillCarDetails(getState().usedCar.valuation,dispatch);
            })
        })
    }
}
export const clearCarSelection = () => dispatch =>{
    dispatch(clearCarSelectionData());
    dispatch(resetCarSelectedData());
    dispatch(resetCarIntialState());
}
export const carPopupStateSelection = value => dispatch => {
    dispatch(changeCarState(value))
}
export const showCarPopup = value => (dispatch,getState) => {
    let state = getState();
    if(isValidToFetchMake(state)){
        dispatch(carSelectionPopup(value));
    }else{
        console.log("inside invalid state");
        dispatch(updateCarValidationState({isValid:false,errorText:'Please select year first'})); // user needs to select year first
    }
}
 export const fetchMakes = value => (dispatch,getState) => {
    let state = getState();
    if (isValidToFetchMake(state)) {
        dispatch(requestMakeData(value));
        fillMakes(value.options,dispatch,300);
    }
}
export const makeSelection = value => dispatch => {
    dispatch(selectMake(value));
    if (value.state) {
        dispatch(changeCarState(value.state))
    }
}
export const fetchModels = value => dispatch => {
    dispatch(changeCarState(value));
    dispatch(requestModelData(value));
    fillModels(value.options,dispatch,300);
}
export const modelSelection = value => dispatch => {
    dispatch(selectModel(value));
    if (value.state) {
        dispatch(changeCarState(value.state))
    }
}
export const fetchVersions = value => dispatch => {
    dispatch(changeCarState(value));
    dispatch(requestVersionData(value));
    fillVersions(value.options,dispatch,300);
}
export const versionSelection = value => dispatch => {
    dispatch(selectVersion(value));
    if (value.state) {
        dispatch(changeCarState(value.state))
    }
}
export const filterCar = value => dispatch => {
    dispatch(applyCarFilter(value));
}
