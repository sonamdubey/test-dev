import React from 'react'
import { Redirect, MemoryRouter } from 'react-router-dom'

import BodyType from '../../../src/NewCarFinder/containers/BodyType'
import { SET_TOAST } from '../../../src/actionTypes'

import {
	SET_CURRENT_SCREEN,
	REQUEST_BODY_TYPE,
	RECEIVE_BODY_TYPE,
	BODY_TYPE_SELECTION
} from '../../../src/NewCarFinder/actionTypes'

import { formatBudgetTooltipValue } from '../../../src/NewCarFinder/utils/Budget'

const bodyTypeApiResponse = [
    { "id": 6, "name": "SUV/MUV", "carCount": 8, "description": "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Nam elementum.", "icon": "https://imgd.aeplcdn.com//0x0/cw/body/svg/suv_clr.svg", "lineIcon": "https://imgd.aeplcdn.com//0x0/cw/body/svg/suv.svg", "isSelected": true, "isRecommended": true },
    { "id": 3, "name": "Hatchback", "carCount": 7, "description": "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Nam elementum.", "icon": "https://imgd.aeplcdn.com//0x0/cw/body/svg/hatchback_clr.svg", "lineIcon": "https://imgd.aeplcdn.com//0x0/cw/body/svg/hatchback.svg", "isSelected": true, "isRecommended": true },
    { "id": 1, "name": "Sedan", "carCount": 5, "description": "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Nam elementum.", "icon": "https://imgd.aeplcdn.com//0x0/cw/body/svg/sedan_clr.svg", "lineIcon": "https://imgd.aeplcdn.com//0x0/cw/body/svg/sedan.svg", "isSelected": false, "isRecommended": false },
    { "id": 5, "name": "Convertible", "carCount": 1, "description": "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Nam elementum.", "icon": "https://imgd.aeplcdn.com//0x0/cw/body/svg/convertible_clr.svg", "lineIcon": "https://imgd.aeplcdn.com//0x0/cw/body/svg/convertible.svg", "isSelected": false, "isRecommended": false },
    { "id": 2, "name": "Coupe", "carCount": 1, "description": "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Nam elementum.", "icon": "https://imgd.aeplcdn.com//0x0/cw/body/svg/coupe_clr.svg", "lineIcon": "https://imgd.aeplcdn.com//0x0/cw/body/svg/coupe.svg", "isSelected": false, "isRecommended": false },
    { "id": 10, "name": "Compact Sedan", "carCount": 0, "description": "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Nam elementum.", "icon": "https://imgd.aeplcdn.com//0x0/cw/body/svg/compactsedan_clr.svg", "lineIcon": "https://imgd.aeplcdn.com//0x0/cw/body/svg/compactsedan.svg", "isSelected": false, "isRecommended": false },
    { "id": 4, "name": "Minivan/Van", "carCount": 0, "description": "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Nam elementum.", "icon": "https://imgd.aeplcdn.com//0x0/cw/body/svg/van_clr.svg", "lineIcon": "https://imgd.aeplcdn.com//0x0/cw/body/svg/van.svg", "isSelected": false, "isRecommended": false },
    { "id": 8, "name": "Station Wagon", "carCount": 0, "description": "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Nam elementum.", "icon": "https://imgd.aeplcdn.com//0x0/cw/body/svg/wagon_clr.svg", "lineIcon": "https://imgd.aeplcdn.com//0x0/cw/body/svg/wagon.svg", "isSelected": false, "isRecommended": false },
    { "id": 7, "name": "Truck", "carCount": 0, "description": "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Nam elementum.", "icon": "https://imgd.aeplcdn.com//0x0/cw/body/svg/truck_clr.svg", "lineIcon": "https://imgd.aeplcdn.com//0x0/cw/body/svg/truck.svg", "isSelected": false, "isRecommended": false }
]


// Mock body type api
jest.mock('../../../src/NewCarFinder/apis/BodyType', () => {
    const bodyTypeApiResponse = [
        { "id": 6, "name": "SUV/MUV", "carCount": 8, "description": "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Nam elementum.", "icon": "https://imgd.aeplcdn.com//0x0/cw/body/svg/suv_clr.svg", "lineIcon": "https://imgd.aeplcdn.com//0x0/cw/body/svg/suv.svg", "isSelected": true, "isRecommended": true },
        { "id": 3, "name": "Hatchback", "carCount": 7, "description": "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Nam elementum.", "icon": "https://imgd.aeplcdn.com//0x0/cw/body/svg/hatchback_clr.svg", "lineIcon": "https://imgd.aeplcdn.com//0x0/cw/body/svg/hatchback.svg", "isSelected": true, "isRecommended": true },
        { "id": 1, "name": "Sedan", "carCount": 5, "description": "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Nam elementum.", "icon": "https://imgd.aeplcdn.com//0x0/cw/body/svg/sedan_clr.svg", "lineIcon": "https://imgd.aeplcdn.com//0x0/cw/body/svg/sedan.svg", "isSelected": false, "isRecommended": false },
        { "id": 5, "name": "Convertible", "carCount": 1, "description": "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Nam elementum.", "icon": "https://imgd.aeplcdn.com//0x0/cw/body/svg/convertible_clr.svg", "lineIcon": "https://imgd.aeplcdn.com//0x0/cw/body/svg/convertible.svg", "isSelected": false, "isRecommended": false },
        { "id": 2, "name": "Coupe", "carCount": 1, "description": "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Nam elementum.", "icon": "https://imgd.aeplcdn.com//0x0/cw/body/svg/coupe_clr.svg", "lineIcon": "https://imgd.aeplcdn.com//0x0/cw/body/svg/coupe.svg", "isSelected": false, "isRecommended": false },
        { "id": 10, "name": "Compact Sedan", "carCount": 0, "description": "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Nam elementum.", "icon": "https://imgd.aeplcdn.com//0x0/cw/body/svg/compactsedan_clr.svg", "lineIcon": "https://imgd.aeplcdn.com//0x0/cw/body/svg/compactsedan.svg", "isSelected": false, "isRecommended": false },
        { "id": 4, "name": "Minivan/Van", "carCount": 0, "description": "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Nam elementum.", "icon": "https://imgd.aeplcdn.com//0x0/cw/body/svg/van_clr.svg", "lineIcon": "https://imgd.aeplcdn.com//0x0/cw/body/svg/van.svg", "isSelected": false, "isRecommended": false },
        { "id": 8, "name": "Station Wagon", "carCount": 0, "description": "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Nam elementum.", "icon": "https://imgd.aeplcdn.com//0x0/cw/body/svg/wagon_clr.svg", "lineIcon": "https://imgd.aeplcdn.com//0x0/cw/body/svg/wagon.svg", "isSelected": false, "isRecommended": false },
        { "id": 7, "name": "Truck", "carCount": 0, "description": "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Nam elementum.", "icon": "https://imgd.aeplcdn.com//0x0/cw/body/svg/truck_clr.svg", "lineIcon": "https://imgd.aeplcdn.com//0x0/cw/body/svg/truck.svg", "isSelected": false, "isRecommended": false }
    ]
    return {
        get: jest.fn((cityId, budget) => Promise.resolve(bodyTypeApiResponse))
    }
})

//Mock Ripple js
jest.mock('../../../src/utils/Ripple')

describe("Tests for BodyType container component", () => {

    let props
    let store
    let enzymeWrapper
    let state
    const Wrapper = () => {
        if (!enzymeWrapper) {
            store = mockStore(state)
            enzymeWrapper = shallowWithStore(<BodyType {...props} />, store).dive()
        }
        return enzymeWrapper
    }

    beforeEach(() => {
        props = undefined
        state = getMockInitialStore()
        enzymeWrapper = undefined
        store = undefined
    })

    describe("when no body type data is available", () => {
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
                screenId: 1
            })

            expect(expectedActions).toContainEqual({
                type: REQUEST_BODY_TYPE
            })

            //http://www.bambielli.com/til/2017-09-19-process-nexttick/
            process.nextTick(() => {
                setTimeout(() => {
                    try {
                        expect(store.getActions()).toHaveLength(3)
                        expect(store.getActions()).toContainEqual({
                            type: RECEIVE_BODY_TYPE,
                            data: bodyTypeApiResponse
                        })
                    } catch (e) {
                        return done(e);
                    }
                    done();
                }, 300)
            });
        })
    })

    describe("when data is available", () => {
        it("should match snapshot", () => {
            state.newCarFinder.bodyType.data = bodyTypeApiResponse
            const wrapper = Wrapper()
            expect(wrapper).toMatchSnapshot()
        })
    })

    describe("when bodyType with count > 0 is clicked", () => {
        it("should dispatch BODY_TYPE_SELECTION action", () => {
            state.newCarFinder.bodyType.data = bodyTypeApiResponse
            const wrapper = Wrapper()
            let activeItems = bodyTypeApiResponse.filter(x => x.carCount > 0)
            if (activeItems.length) {
                const firstItem = wrapper.find('.body-type-list__item').findWhere(x => x.key() == activeItems[0].id)
                firstItem.simulate('click')
                const expectedActions = store.getActions()
                expect(expectedActions).toContainEqual({
                    type: BODY_TYPE_SELECTION,
                    id: parseInt(firstItem.key())
                })
            }
        })
    })

    describe("when bodyType with count == 0 is clicked", () => {
        it("should dispatch BODY_TYPE_SELECTION action", () => {
            state.newCarFinder.bodyType.data = bodyTypeApiResponse
            const wrapper = Wrapper()
            let disabledItems = bodyTypeApiResponse.filter(x => x.carCount == 0)
            if (disabledItems.length) {
                const firstItem = wrapper.find('.body-type-list__item').findWhere(x => x.key() == disabledItems[0].id)
                firstItem.simulate('click')
                const expectedActions = store.getActions().filter(x => x.type == SET_TOAST)
                expect(expectedActions.length).toBe(1)
                expect(expectedActions[0].message).toBe(`There is no ${disabledItems[0].name} available for ${formatBudgetTooltipValue(wrapper.instance().props.budget)} in ${wrapper.instance().props.cityName}.`)
            }
        })
    })

})
