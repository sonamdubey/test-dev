import React from 'react'
import { Redirect, MemoryRouter } from 'react-router-dom'

import FuelType from '../../../src/NewCarFinder/containers/FuelType'

import { SET_TOAST } from '../../../src/actionTypes'

import {
	SET_CURRENT_SCREEN,
	REQUEST_FUEL_TYPE,
	RECEIVE_FUEL_TYPE,
	FUEL_TYPE_SELECTION,
} from '../../../src/NewCarFinder/actionTypes'

const fuelTypeApiResponse = [
    {"id":1,"name":"Petrol","carCount":12,"description":"Indian auto industry offers several car models powered by engines running on petrol, diesel, CNG and LPG.","icon":"https://imgd.aeplcdn.com/0x0/cw/fuel/svg/petrol.svg","isSelected":false},
    {"id":2,"name":"Diesel","carCount":13,"description":"Get the list of cars Under 3 lakhs and their on road price, mileage, rating review. C\nheck out the list of best diesel, compact suv, sedan, hatchback cars below 3 lakhs available in India.","icon":"https://imgd.aeplcdn.com/0x0/cw/fuel/svg/diesel.svg","isSelected":false},
    {"id":3,"name":"CNG","carCount":0,"description":null,"icon":"https://imgd.aeplcdn.com/0x0/cw/fuel/svg/cng.svg","isSelected":false},
    {"id":4,"name":"LPG","carCount":0,"description":null,"icon":"https://imgd.aeplcdn.com/0x0/cw/fuel/svg/lpg.svg","isSelected":false},
    {"id":5,"name":"Electric","carCount":0,"description":null,"icon":"https://imgd.aeplcdn.com/0x0/cw/fuel/svg/electric.svg","isSelected":false}
]


// Mock body type api
jest.mock('../../../src/NewCarFinder/apis/FuelType', () => {
    const fuelTypeApiResponse = [
        {"id":1,"name":"Petrol","carCount":12,"description":"Indian auto industry offers several car models powered by engines running on petrol, diesel, CNG and LPG.","icon":"https://imgd.aeplcdn.com/0x0/cw/fuel/svg/petrol.svg","isSelected":false},
        {"id":2,"name":"Diesel","carCount":13,"description":"Get the list of cars Under 3 lakhs and their on road price, mileage, rating review. C\nheck out the list of best diesel, compact suv, sedan, hatchback cars below 3 lakhs available in India.","icon":"https://imgd.aeplcdn.com/0x0/cw/fuel/svg/diesel.svg","isSelected":false},
        {"id":3,"name":"CNG","carCount":0,"description":null,"icon":"https://imgd.aeplcdn.com/0x0/cw/fuel/svg/cng.svg","isSelected":false},
        {"id":4,"name":"LPG","carCount":0,"description":null,"icon":"https://imgd.aeplcdn.com/0x0/cw/fuel/svg/lpg.svg","isSelected":false},
        {"id":5,"name":"Electric","carCount":0,"description":null,"icon":"https://imgd.aeplcdn.com/0x0/cw/fuel/svg/electric.svg","isSelected":false}
    ]
    return {
        get: jest.fn((options) => Promise.resolve(fuelTypeApiResponse))
    }
})

//Mock Ripple js
jest.mock('../../../src/utils/Ripple')

describe("Tests for FuelType container component", () => {

    let props
    let store
    let enzymeWrapper
    let state
    const Wrapper = () => {
        if (!enzymeWrapper) {
            store = mockStore(state)
            enzymeWrapper = shallowWithStore(<FuelType {...props} />, store).dive()
        }
        return enzymeWrapper
    }

    beforeEach(() => {
        props = undefined
        state = getMockInitialStore()
        enzymeWrapper = undefined
        store = undefined
    })

    describe("when no fuel type data is available", () => {
        it("should match snapshot", () => {
            const wrapper = Wrapper()
            expect(wrapper).toMatchSnapshot()
        })
    })

    describe("when component is mounted", () => {
        it("should dispatch appropriate actions", (done) => {
            /**
             * documentation for done and use case : https://jasmine.github.io/2.5/introduction.html#section-58
             * and https://stackoverflow.com/questions/47275706/async-function-not-behaving-as-i-expect-with-jest
             */
            const wrapper = Wrapper()
            const expectedActions = store.getActions()
            expect(expectedActions).toHaveLength(2)

            expect(expectedActions).toContainEqual({
                type: SET_CURRENT_SCREEN,
                screenId: 2
            })

            expect(expectedActions).toContainEqual({
                type: REQUEST_FUEL_TYPE
            })

            // http://www.bambielli.com/til/2017-09-19-process-nexttick/
            // Please read this blog before attempting a change : http://voidcanvas.com/setimmediate-vs-nexttick-vs-settimeout/
            process.nextTick(() => {
                setTimeout(() => {
                    try {
                        expect(store.getActions()).toHaveLength(3)
                        expect(store.getActions()).toContainEqual({
                            type: RECEIVE_FUEL_TYPE,
                            data: fuelTypeApiResponse
                        })
                    } catch (e) {
                        return done(e);
                    }
                    done();
                },300)
            });
        })
    })

    describe("when data is available", () => {
        it("should match snapshot", () => {
            state.newCarFinder.fuelType.data = fuelTypeApiResponse
            const wrapper = Wrapper()
            expect(wrapper).toMatchSnapshot()
        })
    })

    describe("when fuel type with count > 0 is clicked", () => {
        it("should dispatch FUEL_TYPE_SELECTION action", () => {
            state.newCarFinder.fuelType.data = fuelTypeApiResponse
            const wrapper = Wrapper()
            let activeItems = fuelTypeApiResponse.filter(x => x.carCount > 0)
            if (activeItems.length) {
                const firstItem = wrapper.find('.fuel-type-list__item').findWhere(x => x.key() == activeItems[0].id)
                firstItem.simulate('click')
                const expectedActions = store.getActions()
                expect(expectedActions).toContainEqual({
                    type: FUEL_TYPE_SELECTION,
                    id: parseInt(firstItem.key())
                })
            }
        })
    })

    describe("when fuel type with count == 0 is clicked", () => {
        it("should not dispatch FUEL_TYPE_SELECTION action", () => {
            state.newCarFinder.fuelType.data = fuelTypeApiResponse
            const wrapper = Wrapper()
            let disabledItems = fuelTypeApiResponse.filter(x => x.carCount == 0)
            if (disabledItems.length) {
                const firstItem = wrapper.find('.fuel-type-list__item').findWhere(x => x.key() == disabledItems[0].id)
                firstItem.simulate('click')
                const expectedActions = store.getActions().filter(x => x.type == SET_TOAST)
                expect(expectedActions.length).toBe(1)
                //expect(expectedActions[0].message).toBe(``)
            }
        })
    })

})
