import React from 'react'
import Autocomplete from '../../../src/components/Autocomplete'


describe("Tests for Autocomplete component", () => {
    let props = {}

    let enzymeWrapper

    const Wrapper = () => {
        if(!enzymeWrapper){
            enzymeWrapper = shallow(<Autocomplete { ...props } />)
        }
        return enzymeWrapper
    }

    beforeEach(() => {
        props.inputEvents = {
            onChange: jest.fn(),
            onBlur: jest.fn(),
            onFocus: jest.fn(),
            onClear: jest.fn()
        }
        props.inputProps = {
            id:"",
            placeholder:"",
            className:"",
            value:""
        }
        props.renderSuggestionList = jest.fn()
        props.list = []
    })

    it('should display the value passed', () => {
        props.inputProps = {
            ...props.inputProps,
            value:"Mumbai, Maharashtra",
            placeholder:"Type your city",
            id:"city-input",
            className:"city-field-class"
        }
        const wrapper = Wrapper()
        const inputField = wrapper.find('input').first()
        expect(inputField.props().value).toBe(props.inputProps.value)
        expect(inputField.props().placeholder).toBe(props.inputProps.placeholder)
        expect(inputField.props().id).toBe(props.inputProps.id)
        expect(inputField.props().className).toBe(props.inputProps.className)
        expect(wrapper).toMatchSnapshot()
    })

    it('should call respective methods when events are triggered', () => {
        const wrapper = Wrapper()

        const inputField = wrapper.find('input')

        const instanceEventHandlers = wrapper.instance().props.inputEvents

        expect(wrapper.instance().props.renderSuggestionList).toBeCalledWith(props.list)

        inputField.simulate('focus')
        expect(instanceEventHandlers.onFocus).toBeCalled()

        inputField.simulate('blur')
        expect(instanceEventHandlers.onBlur).toBeCalled()

        inputField.simulate('change', { target: { value: 'mum'}})
        expect(instanceEventHandlers.onChange).toBeCalled()

        wrapper.find('.clear-field-target').simulate('click')
        expect(instanceEventHandlers.onClear).toBeCalled()

    })
})
