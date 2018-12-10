import React from 'react'
import { Redirect } from 'react-router-dom'
import FindCar from '../../../src/NewCarFinder/containers/FindCar'

describe('Tests for FindCar component', () => {
    it('should render a Redirect to /select-city', function() {
        let state = getMockInitialStore()
        let store = mockStore(state)
        const wrapper = shallowWithStore(<FindCar />, store)
        expect(wrapper).toMatchSnapshot()
      });
})
