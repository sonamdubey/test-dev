import React from 'react'
import { Redirect } from 'react-router-dom'
import Budget from '../../../src/NewCarFinder/containers/Budget'
import {
	UPDATE_BUDGET_INPUT_VALUE
} from '../../../src/NewCarFinder/actionTypes'

import thunk from 'redux-thunk'
import configureStore from 'redux-mock-store'
import { Provider } from 'react-redux'

jest.mock('../../../src/NewCarFinder/apis/Budget', () => {
    const budgetApiResponse = {
        min:2,
        max:100,
        suggested:39
    }
    return {
        get: jest.fn(( cityId ) => Promise.resolve(budgetApiResponse))
    }
})
describe("Test for budget container", () => {
    let props
    let state = getMockInitialStore()
    let store
    let enzymeWrapper
    const Wrapper = () => {
        if(!enzymeWrapper){
            store = mockStore(state)
            enzymeWrapper = mount(<Provider store={store}><Budget {...props}/></Provider>)
        }
        return enzymeWrapper
    }
    beforeEach(
        () => {
            // props = undefined
            // state = undefined
            store = undefined
            enzymeWrapper = undefined
        })
    it("it should always show default seleced budget",() =>{
        const wrapper = Wrapper()
        expect(wrapper).toMatchSnapshot()

        setTimeout(function() {
            const expectedActions = store.getActions()
            expect(expectedActions).toHaveLength(3)

            expect(expectedActions).toContainEqual({
                type: UPDATE_BUDGET_INPUT_VALUE,
                value:3900000
            })

        }, 0);
    })
})
