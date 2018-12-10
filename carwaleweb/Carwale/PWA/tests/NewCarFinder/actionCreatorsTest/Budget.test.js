import * as Budget from '../../../src/NewCarFinder/actionCreators/Budget'
import {BUDGET_SLIDER_MIN, BUDGET_SLIDER_MAX} from '../../../src/NewCarFinder/constants/index'
import {
	UPDATE_BUDGET_SLIDER_VALUE,
	UPDATE_BUDGET_INPUT_VALUE
} from '../../src/NewCarFinder/actionTypes'

import * as tracking from '../../src/utils/Analytics'

import thunk from 'redux-thunk'
import React from 'react'
import configureStore from 'redux-mock-store'
import { Provider } from 'react-redux'


describe('Test updateBudgetInput', () => {
    let store
    let spy
    beforeEach(() => {
        store = mockStore(getMockInitialStore())
        spy  = jest.spyOn(tracking,'fireInteractiveTracking')
    })
    let sliderAction = {type: UPDATE_BUDGET_SLIDER_VALUE,
        values: [2500000],
        min: null,
        max: null,
        userChange: true}

    let bugetAction = {type: UPDATE_BUDGET_INPUT_VALUE,
        value: 2500000}

    let expectedSliderValue, expectedBugetValue

    it('should dispatch UPDATE_BUDGET_SLIDER_VALUE and UPDATE_BUDGET_INPUT_VALUE actions', () => {
        store.dispatch(Budget.updateBudgetInput(2500000, true))
        let actions = store.getActions()
        expectedSliderValue = {...sliderAction}
        expectedBugetValue = {...bugetAction}
        expect(actions).toContainEqual(expectedSliderValue)
        expect(actions).toContainEqual(expectedBugetValue)
        expect(spy).toHaveBeenCalled()
    })

    it('should dispatch actions with slider and input values to be BUDGET_SLIDER_MIN when input is less than BUDGET_SLIDER_MIN',
    () => {
       store.dispatch(Budget.updateBudgetInput(2500, true))
       expectedSliderValue = {...sliderAction, values:[BUDGET_SLIDER_MIN]}
       expectedBugetValue = {...bugetAction}
       expectedBugetValue.value = 2500
       let actions = store.getActions()
       expect(actions).toContainEqual(expectedSliderValue)
       expect(actions).toContainEqual(expectedBugetValue)
       expect(spy).toHaveBeenCalled()
    })

    it('should dispatch UPDATE_BUDGET_SLIDER_VALUE and UPDATE_BUDGET_INPUT_VALUE actions when usechange is false', () => {
        store.dispatch(Budget.updateBudgetInput(2500000, false))
        expectedSliderValue = {...sliderAction, userChange: false}
        expectedBugetValue = {...bugetAction}
        let actions = store.getActions()
        expect(actions).toContainEqual(expectedSliderValue)
        expect(actions).toContainEqual(expectedBugetValue)
        expect(spy).toHaveBeenCalled()
    })

    it('should dispatch actions with slider and input values to be BUDGET_SLIDER_MAX when input is grater than BUDGET_SLIDER_MAX',
    () => {
       store.dispatch(Budget.updateBudgetInput(25000000, true))
       expectedSliderValue = {...sliderAction, values: [BUDGET_SLIDER_MAX]}
       expectedBugetValue = {...bugetAction}
       expectedBugetValue.value = 25000000
       let actions = store.getActions()
       expect(actions).toContainEqual(expectedSliderValue)
       expect(actions).toContainEqual(expectedBugetValue)
       expect(spy).toHaveBeenCalled()
   })
})
