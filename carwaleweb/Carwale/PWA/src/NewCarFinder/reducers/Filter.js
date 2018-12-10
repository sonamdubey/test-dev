import { combineReducers } from 'redux'
import {
    NEXT_FILTER,
    SET_CURRENT_SCREEN, SET_ALL_FILTER_SCREEN
} from '../actionTypes'
import {
    NEWCARFINDER_FILTERS_BUDGET_ENDPOINT,
    NEWCARFINDER_FILTERS_BODYTYPE_ENDPOINT,
    NEWCARFINDER_FILTERS_FUELTYPE_ENDPOINT,
    NEWCARFINDER_FILTERS_MAKE_ENDPOINT
} from '../constants/index'

import { SELECT_CITY } from '../../actionTypes'

const defaultScreenOrder = [
    {
        name:"BudgetFilter",
        path: NEWCARFINDER_FILTERS_BUDGET_ENDPOINT
    },
    {
        name:"BodyTypeFilter",
        path: NEWCARFINDER_FILTERS_BODYTYPE_ENDPOINT
    },
    {
        name:"FuelTypeFilter",
        path: NEWCARFINDER_FILTERS_FUELTYPE_ENDPOINT
    },
    {
        name:"MakeFilter",
        path: NEWCARFINDER_FILTERS_MAKE_ENDPOINT
    }
    ]
export const getInitialScreenState = () => {
    return {
        "currentScreen": -1,
        "screenOrder":  defaultScreenOrder
    }
}

const filterScreens = (state = getInitialScreenState(), action ) => {
    switch(action.type){
        case SET_CURRENT_SCREEN:
            return {
                ...state,
                "currentScreen":action.screenId
            }
            case SET_ALL_FILTER_SCREEN:
            return{
                ...state,
                "screenOrder": (action.data && action.data.screenOrder) ? action.data.screenOrder : defaultScreenOrder
            }
            case SELECT_CITY:
            return {
                ...state,
                "currentScreen": 0
            }

    }
    return state
}

export const filter = combineReducers({
    filterScreens
})
