import {
    KILOMETERS_CHANGED
} from '../actionTypes'

const kilometersChanged = value =>{
    return {
        type : KILOMETERS_CHANGED,
        value
    }
}

export const selectKilometers = value => dispatch =>{
    dispatch(kilometersChanged(value));
}
