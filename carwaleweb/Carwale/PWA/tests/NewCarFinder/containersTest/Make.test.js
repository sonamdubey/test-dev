import React from 'react'
import { Redirect, MemoryRouter } from 'react-router-dom'

import Make from '../../../src/NewCarFinder/containers/Make'

import {
	SET_TOAST
} from '../../../src/actionTypes'

import {
	SET_CURRENT_SCREEN,
	REQUEST_MAKE,
	RECEIVE_MAKE,
	MAKE_SELECTION,
} from '../../../src/NewCarFinder/actionTypes'


const makeData = {
"popularMakes": [
    {"makeId": 50,"makeName": "Force Motors","modelCount": 2}
],
"otherMakes": [
    {"makeId": 10,"makeName": "Maruti Suzuki","modelCount": 0},
    {"makeId": 7,"makeName": "Honda","modelCount": 0},
    {"makeId": 17,"makeName": "Toyota","modelCount": 0}
]
}

jest.mock('../../src/NewCarFinder/apis/Make', () => {
    const makeData = {
        "popularMakes": [
            {"makeId": 50,"makeName": "Force Motors","modelCount": 2}
        ],
        "otherMakes": [
            {"makeId": 10,"makeName": "Maruti Suzuki","modelCount": 0},
            {"makeId": 7,"makeName": "Honda","modelCount": 0},
            {"makeId": 17,"makeName": "Toyota","modelCount": 0}
        ]
        }
    return {
        get: jest.fn((options) => Promise.resolve(makeData))
    }
})

jest.mock('../../../src/utils/Ripple')


describe("Tests for Make container component", () => {

    let props
    let store
    let enzymeWrapper
    let state
    const Wrapper = () => {
        if (!enzymeWrapper) {
            store = mockStore(state)
            enzymeWrapper = shallowWithStore(<Make {...props} />, store).dive()
        }
        return enzymeWrapper
    }

    beforeEach(() => {
        props = undefined
        state = getMockInitialStore()
        enzymeWrapper = undefined
        store = undefined
    })

    describe("when no make data is available", () => {
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
                screenId: 3
            })

            expect(expectedActions).toContainEqual({
                type: REQUEST_MAKE
            })

            // http://www.bambielli.com/til/2017-09-19-process-nexttick/
            // Please read this blog before attempting a change : http://voidcanvas.com/setimmediate-vs-nexttick-vs-settimeout/
            process.nextTick(() => {
                setTimeout(() => {
                    try {
                        expect(store.getActions()).toHaveLength(3)
                        expect(store.getActions()).toContainEqual({
                            type: RECEIVE_MAKE,
                            data: makeData
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
            state.newCarFinder.make.data = makeData
            const wrapper = Wrapper()
            expect(wrapper).toMatchSnapshot()
        })
    })

    describe("when MAKE with count > 0 is clicked", () => {
        it("should dispatch Make_SELECTION action", () => {
            state.newCarFinder.make.data = makeData
            const wrapper = Wrapper()
            let activeItems = makeData.popularMakes.filter(x=> x.isSelected)
            activeItems.concat( makeData.otherMakes.filter(x=> x.isSelected))
           // let activeItems = makeData.filter(x => x.carCount > 0)
            if (activeItems.length) {
                const firstItem = wrapper.find('.make-list__item').findWhere(x => x.key() == activeItems[0].makeId)
                firstItem.simulate('click')
                const expectedActions = store.getActions()
                expect(expectedActions).toContainEqual({
                    type: MAKE_SELECTION,
                    id: parseInt(firstItem.key())
                })
            }
        })
     })

    describe("when make with modelcount == 0 is clicked", () => {
        it("should not dispatch MAKE_SELECTION action", () => {
            state.newCarFinder.make.data = makeData
            const wrapper = Wrapper()
            let disabledItems = makeData.popularMakes.filter(x => x.isSelected == 0)
            disabledItems.concat(makeData.otherMakes.filter(x => x.isSelected == 0))
            if (disabledItems.length) {
                const firstItem = wrapper.find('.make-list__item').findWhere(x => x.key() == disabledItems[0].makeId)
                firstItem.simulate('click')
                const expectedActions = store.getActions().filter(x => x.type == SET_TOAST)
                expect(expectedActions.length).toBe(1)
                //expect(expectedActions[0].message).toBe(``)
            }
        })
    })

})
