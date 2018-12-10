import React from 'react'
import BodyTypeItem from '../../../src/NewCarFinder/components/BodyTypeItem'


describe("Tests for BodyTypeItem component", () => {
    let props = {
        item : {
            "id": 6,
            "name": "SUV/MUV",
            "carCount": 8,
            "description": "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Nam elementum.",
            "icon": "https://imgd.aeplcdn.com//0x0/cw/body/svg/suv_clr.svg",
            "lineIcon": "https://imgd.aeplcdn.com//0x0/cw/body/svg/suv.svg",
            "isSelected": true,
            "isRecommended": true
        }
    }

    let enzymeWrapper

    const Wrapper = () => {
        if(!enzymeWrapper){
            enzymeWrapper = shallow(<BodyTypeItem { ...props } />)
        }
        return enzymeWrapper
    }
    beforeEach(() => {
        enzymeWrapper = undefined
    })
    describe("when item is not selected", () => {
        it("should match snapshot", () => {
            props.item.isSelected = false
            const wrapper = Wrapper()
            expect(wrapper).toMatchSnapshot()
        })
    })

    describe("when item is selected", () => {
        it("should match snapshot", () => {
            props.item.isSelected = true
            const wrapper = Wrapper()
            expect(wrapper).toMatchSnapshot()
        })
    })
})
