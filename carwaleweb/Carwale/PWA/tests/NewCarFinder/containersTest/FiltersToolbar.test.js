import React from 'react'
import { Redirect } from 'react-router-dom'
import FiltersToolbar from '../../../src/NewCarFinder/containers/FiltersToolbar'
import { NEWCARFINDER_RESULTS_ENDPOINT } from '../../../src/NewCarFinder/constants/index'
import {OPEN_FILTER_SCREEN} from '../../../src/NewCarFinder/actionTypes'

describe('Tests for FiltersToolbar component', () => {
    let state
    let props
    let store
    let enzymeWrapper

    const Wrapper = () => {
        if(!enzymeWrapper){
            store = mockStore(state)
            enzymeWrapper = shallowWithStore(<FiltersToolbar {...props}/>, store)

        }
        return enzymeWrapper
    }

    beforeEach( () => {
        props = undefined
        state = undefined
        enzymeWrapper = undefined
        store = undefined
    })

    it('should render snapshot', () => {
        props = {
            selectionObject:{
                'Budget' : '₹ 5 lakh',
                'Body Type':'Sedan, Coupe',
                'Fuel Type':'Diesel, Petrol'
            }
        }
        const wrapper = Wrapper().dive()
        expect(wrapper).toMatchSnapshot()
    });

    describe('on clicking edit button', () => {

        //https://github.com/airbnb/enzyme/issues/697
        //https://github.com/airbnb/enzyme/issues/208
        //This test requires more research,
        //the methods suggested in above issues did not prove useful
        //https://github.com/airbnb/enzyme/issues/365
        // it('should call handleEditClick',() => {
        //     props = {
        //         selectionObject:{
        //             'Budget' : '₹ 5 lakh',
        //             'Body Type':'Sedan, Coupe',
        //             'Fuel Type':'Diesel, Petrol'
        //         },
        //         openFilterScreen : jest.fn()
        //     }

        //     const handleEditClick = jest.fn()
        //     //const oldhandleEditClick = FiltersToolbar.WrappedComponent.prototype.handleEditClick
        //     //FiltersToolbar.WrappedComponent.prototype.handleEditClick = handleEditClick
        //     const wrapper = shallow(<FiltersToolbar.WrappedComponent {...props}/>)
        //     console.log(wrapper.instance())
        //     wrapper.instance().handleEditClick = handleEditClick
        //     wrapper.update()
        //     const editButton = wrapper.find('.filter__edit')
        //     editButton.simulate('click')
        //     expect(handleEditClick).toBeCalled()
        //     //FiltersToolbar.WrappedComponent.prototype.handleEditClick = oldhandleEditClick
        // })

        it('should call openFilterScreen and trackEvent',() => {
            const wrapper = Wrapper().dive()
            wrapper.instance().trackEvent = jest.fn()

            const editButton = wrapper.find('.filter__edit')
            editButton.simulate('click')
            expect(wrapper.instance().trackEvent).toBeCalledWith('EditOptionClick')

            const expectedActions = store.getActions()
            expect(expectedActions.length).toBe(1)
            expect(expectedActions).toContainEqual({
                type: OPEN_FILTER_SCREEN,
                activeAccordion: undefined
            })
        })
    })

    describe('on clicking FilterTab', () => {
        it('should call openFilterScreen and trackEvent',() => {
            props = {
                selectionObject:{
                    'Budget' : '₹ 5 lakh',
                    'Body Type':'Sedan, Coupe',
                    'Fuel Type':'Diesel, Petrol'
                }
            }
            const wrapper = Wrapper().dive()
            wrapper.instance().trackEvent = jest.fn()

            const editButton = wrapper.find('[data-filter-type="Budget"]')
            editButton.simulate('click', {currentTarget : {
                getAttribute: () => 'Budget'
            }})
            expect(wrapper.instance().trackEvent).toBeCalledWith('EditOptionClick_Budget')

            const expectedActions = store.getActions()
            expect(expectedActions.length).toBe(1)
            expect(expectedActions).toContainEqual({
                type: OPEN_FILTER_SCREEN,
                activeAccordion: 'Budget'
            })
        })
    })


})
