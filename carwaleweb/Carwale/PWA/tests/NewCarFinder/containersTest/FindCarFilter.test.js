import React from 'react'
import { Redirect, MemoryRouter } from 'react-router-dom'

import FindCarFilters from '../../../src/NewCarFinder/containers/FindCarFilters'
import { NEWCARFINDER_ENDPOINT, NEWCARFINDER_FILTERS_ENDPOINT, NEWCARFINDER_FILTERS_BUDGET_ENDPOINT, NEWCARFINDER_FILTERS_BODYTYPE_ENDPOINT } from '../../../src/NewCarFinder/constants/index';


describe("Tests for FindCarFilters container component",()=>{

    let state
    let props
    let store
    let enzymeWrapper

    const Wrapper = () => {
        if(!enzymeWrapper){
            store = mockStore(state)
            //https://stackoverflow.com/questions/44204828/testing-react-component-enclosed-in-withrouter-preferably-using-jest-enzyme
            //https://github.com/ReactTraining/react-router/blob/v4.1.1/packages/react-router/modules/withRouter.js
            //https://github.com/reactjs/react-redux/blob/master/docs/api.md#static-properties
            //https://reactjs.org/docs/higher-order-components.html#static-methods-must-be-copied-over
            /**
             * withRouter has static property WrappedComponent which refers to the component passed to it.
             * But since withRouter uses hoistStatics to copy statics over from passed component and redux connected component also defines
             * a static WrappedComponent refering to the component passed to connect, the WrappedComponent property of connected component is
             * copied over to the WrappedComponent property of the withRouter component
             * e.g. const enhancedComponent = withRouter(connect(mapStateToProps)(DummyComponent))
             *      enhancedComponent.WrappedComponent === DummyComponent //true
             */
            enzymeWrapper = shallowWithStore(<FindCarFilters.WrappedComponent {...props}/>, store)

        }
        return enzymeWrapper
    }

    beforeEach( () => {
        props = undefined
        state = undefined
        enzymeWrapper = undefined
        store = undefined
    })
    describe("when user location is not confirmed",() => {
        it("should render Redirect", () => {

            props = {
                history: {
                    replace:jest.fn()
                },
                location:{
                    pathname: NEWCARFINDER_FILTERS_ENDPOINT
                },
                locationConfirmed: false,
                // since we will be mounting only the innermost component, these have to be mocked
                hideHeader : jest.fn(),
                hideFooter : jest.fn()
            }
            const wrapper = Wrapper()

            expect(wrapper).toMatchSnapshot()
        })
    })

    describe("when user location is  confirmed",() => {
        it("should render FindCarFilterRouter and FiltersFooter", () => {

            props = {
                history: {
                    replace:jest.fn()
                },
                location:{
                    pathname: NEWCARFINDER_FILTERS_ENDPOINT
                },
                locationConfirmed: true,
                hideHeader : jest.fn(),
                hideFooter : jest.fn()
            }
            const wrapper = Wrapper()

            expect(wrapper).toMatchSnapshot()
        })
    })

    describe("ShouldComponentUpdate",() => {
        beforeEach(() => {
            props = {
                location:{
                    pathname: NEWCARFINDER_FILTERS_BUDGET_ENDPOINT
                },
                locationConfirmed: true,
                hideHeader : jest.fn(),
                hideFooter : jest.fn()
            }
        })
        it("should return true if path changes", () => {
            const wrapper = Wrapper()
            const nextProps = {
                ...props,
                location : {
                    pathname: NEWCARFINDER_FILTERS_BODYTYPE_ENDPOINT
                }
            }
            expect(wrapper.instance().shouldComponentUpdate(nextProps)).toBeTruthy()
        })
    })
})
