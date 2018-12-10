import React from 'react'
import { Redirect } from 'react-router-dom'
import Listing from '../../../src/NewCarFinder/containers/Listing'
import { NEWCARFINDER_RESULTS_ENDPOINT } from '../../../src/NewCarFinder/constants/index';

describe('Tests for Listing component', () => {
    let state
    let props
    let store
    let enzymeWrapper

    const Wrapper = () => {
        if(!enzymeWrapper){
            store = mockStore(state)
            enzymeWrapper = shallowWithStore(<Listing {...props}/>, store)

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
            history: {
                replace:jest.fn()
            },
            location:{
                pathname: NEWCARFINDER_RESULTS_ENDPOINT,
                search: '?cityId=1&budget=1465060&bodyStyleIds=3%2C1'
            }
        }
        state = getMockInitialStore()
        const wrapper = Wrapper()
        expect(wrapper).toMatchSnapshot()
      });

      describe('ShouldComponentUpdate', () => {

        it('should return true if querystring is different', () => {
            props = {
                location:{
                    pathname: NEWCARFINDER_RESULTS_ENDPOINT,
                    search: '?cityId=1&budget=1465060&bodyStyleIds=3%2C1'
                }
            }

            let nextProps = {
                location:{
                    pathname: NEWCARFINDER_RESULTS_ENDPOINT,
                    search: '?cityId=10&budget=1465060&bodyStyleIds=3%2C1'
                }
            }
            state = getMockInitialStore()
            const wrapper = Wrapper()
            expect(wrapper.dive().instance().shouldComponentUpdate(nextProps)).toBeTruthy()

        })

        it('should return false if querystring is same', () => {
            props = {
                location:{
                    pathname: NEWCARFINDER_RESULTS_ENDPOINT,
                    search: '?cityId=1&budget=1465060&bodyStyleIds=3%2C1'
                }
            }

            let nextProps = {
                location:{
                    pathname: NEWCARFINDER_RESULTS_ENDPOINT,
                    search: '?cityId=1&budget=1465060&bodyStyleIds=3%2C1'
                }
            }
            state = getMockInitialStore()
            const wrapper = Wrapper()
            expect(wrapper.instance().shouldComponentUpdate(nextProps)).toBeFalsy()

        })
      })
})
