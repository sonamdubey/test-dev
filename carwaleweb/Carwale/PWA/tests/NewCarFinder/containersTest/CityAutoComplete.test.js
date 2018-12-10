import React from 'react'
import { Redirect } from 'react-router-dom'

import CityAutocomplete from '../../../src/components/CityAutocomplete'
import Autocomplete from '../../../src/components/Autocomplete'

import { SELECT_CITY } from '../../../src/actionTypes'


describe("Tests for CityAutocomplete container component",()=>{

    let state
    let props
    let store
    let enzymeWrapper

    const Wrapper = () => {
        if(!enzymeWrapper){
            store = mockStore(state)
            enzymeWrapper = shallowWithStore(<CityAutocomplete {...props}/>, store)
                            .dive() // dive() because it is a connected component
        }
        return enzymeWrapper
    }

    beforeEach(()=>{
        props = undefined
        state = undefined
        enzymeWrapper = undefined
        store = undefined
    })

    it("should render w/o confirm when no city cookie present", () => {
        state = {
            location:{
                cityId:-1,
                cityName:'Select City'
            }
        }

        const wrapper = Wrapper()

        expect(wrapper.find(Autocomplete)).toHaveLength(1)
        expect(wrapper.find('.btn-confirm-city')).toHaveLength(0)
        expect(wrapper).toMatchSnapshot()

    })

    it("should render confirm when city cookie present", () => {
        state = {
            location:{
                cityId:1,
                cityName:'Mumbai, Maharashtra'
            }
        }

        const wrapper = Wrapper()

        expect(wrapper.find(Autocomplete)).toHaveLength(1)
        expect(wrapper.find('.btn-confirm-city')).toHaveLength(1)
        expect(wrapper).toMatchSnapshot()

    })

    it("should redirect to success url on pressing confirm", () => {
        state = {
            location:{
                cityId:1,
                cityName:'Mumbai, Maharashtra'
            }
        }

        props = { successUrl : '/find-car/filters'}

        const wrapper = Wrapper()

        expect(wrapper.find('.btn-confirm-city')).toHaveLength(1)

        expect(wrapper.find(Redirect)).toHaveLength(0)

        wrapper.find('.btn-confirm-city').simulate('click')

        expect(wrapper.find(Redirect)).toHaveLength(1)

        const expectedActions = store.getActions()
        expect(expectedActions).toHaveLength(1)

        expect(expectedActions).toContainEqual({
            type: SELECT_CITY,
            cityId: state.location.cityId,
            cityName: state.location.cityName,
            isConfirmBtnClicked: true
        })

        expect(wrapper).toMatchSnapshot()
    })

    it("should redirect to success url on list item selection", () => {
        state = {
            location:{
                cityId:-1,
                cityName:''
            }
        }

        props = { successUrl : '/find-car/filters'}

        const wrapper = Wrapper()

        const item = {
            result:"Mumbai, Maharashtra",
            payload:{
               cityId:1,
               cityName:"Mumbai"
           }
        }

        expect(wrapper.find(Redirect)).toHaveLength(0)

        wrapper.instance().onListItemSelection(item)

        // To update the shallow render tree @reference: https://github.com/airbnb/enzyme/issues/450
        wrapper.update()

        expect(wrapper.find(Redirect)).toHaveLength(1)
        const expectedActions = store.getActions()
        expect(expectedActions).toHaveLength(1)

        expect(expectedActions).toContainEqual({
            type: SELECT_CITY,
            cityId: item.payload.cityId,
            cityName: item.payload.cityName,
            isConfirmBtnClicked: true
        })

        expect(wrapper).toMatchSnapshot()
    })
})
