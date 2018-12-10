import { budget } from '../../../src/NewCarFinder/reducers/Budget'
import snapPoints from '../../../src/NewCarFinder/utils/rheostat/constants/budgetSnapPoints'
import {BUDGET_SLIDER_MIN, BUDGET_SLIDER_MAX} from '../../../src/NewCarFinder/constants/index'
import {
	UPDATE_BUDGET_SLIDER_VALUE,
	UPDATE_BUDGET_INPUT_VALUE
} from '../../../src/NewCarFinder/actionTypes'

let initialState = {
    slider:{
        values:[0],
        min:BUDGET_SLIDER_MIN,
        max:BUDGET_SLIDER_MAX,
        userChange:false,
        snapPoints
    },
    inputBox:{
        value: 0
    }
}

describe('Budget reducer', () => {
    it('should return initial store', () => {
        expect(budget(undefined, {})).toEqual(initialState)
    })

    it('should return same state when action.type is not matched with any Case in reducer', () => {
        expect(budget(initialState, {type : 'ANY_OTHER_TYPE'})).toEqual(initialState)
    })
})

describe('Budget.inputBox resducer', () => {
    let state = {
        inputBox : {
            value: 500000
        }
    }
    let bugetAction = {type: UPDATE_BUDGET_INPUT_VALUE,
        value: 2500000}
    it('should return given value', () => {
        let expectedState = {value: 2500000}
        expect(budget(state, bugetAction).inputBox).toEqual(expectedState)
    })
})

describe('Budget.slider reducer', () => {
    let state = {
        slider:{
            min: BUDGET_SLIDER_MIN,
            max: BUDGET_SLIDER_MAX,
            values: [0],
            userChange:true,
            snapPoints
        }
    }
    it('should return given value', () => {
        let expectedState = {...state.slider, values: [2500000]}
        let sliderAction = {type: UPDATE_BUDGET_SLIDER_VALUE,
            values: [2500000],
            min: null,
            max: null,
            userChange: true}
        expect(budget(state, sliderAction).slider).toEqual(expectedState)
    })
})

