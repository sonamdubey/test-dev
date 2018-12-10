import {make} from '../../../src/NewCarFinder/reducers/Make'
import {
	REQUEST_MAKE,
	RECEIVE_MAKE,
	MAKE_SELECTION
} from '../../../src/NewCarFinder/actionTypes'

let initialMakeState = {
	isFetching: false,
	data: {
		popularMakes:[],
		otherMakes:[]
	}
}

describe('Make reducer', () => {
    it('should return initial store', () => {
        expect(make(undefined, {})).toEqual(initialMakeState)
    })

    it('should return same state when action.type is not matched with any Case in reducer', () => {
        expect(make(initialMakeState, {type : 'ANY_OTHER_TYPE'})).toEqual(initialMakeState)
    })
})

describe('RequestMake reducer', () => {
    let makeRequest = {type: REQUEST_MAKE}
    it('should return given value', () => {
        expect(make(initialMakeState, makeRequest).isFetching).toBeTruthy()
    })
})
let expectedOutput = {
    isFetching:false,
    data: {
    popularMakes: [
        {
            "makeId": 10,
            "makeName": "Maruti Suzuki",
            "modelCount": 2,
            "isSelected":true
        }
    ],
    otherMakes:[
        {
            "makeId": 7,
            "makeName": "Honda",
            "modelCount": 0,
            "isSelected":false
        },
        {
            "makeId": 17,
            "makeName": "Toyota",
            "modelCount": 1,
            "isSelected":true
        }]
}
}
let actionMakeState = {
    type: RECEIVE_MAKE,
    data: {
        popularMakes: [
            {
                "makeId": 10,
                "makeName": "Maruti Suzuki",
                "modelCount": 2
            }
        ],
        otherMakes:[
            {
                "makeId": 7,
                "makeName": "Honda",
                "modelCount": 0
            },
            {
                "makeId": 17,
                "makeName": "Toyota",
                "modelCount": 1
            }]
    }
}
let selectionMakeState = {
    data: {
        popularMakes: [
            {
                "makeId": 10,
                "makeName": "Maruti Suzuki",
                "modelCount": 2,
                "isSelected":true
            }
        ],
        otherMakes:[
            {
                "makeId": 7,
                "makeName": "Honda",
                "modelCount": 0,
                "isSelected":false
            },
            {
                "makeId": 17,
                "makeName": "Toyota",
                "modelCount": 1,
                "isSelected":true
            }]
    }
}
describe('ReceiveMake reducer', () => {
    it('should return given value', () => {
        let result = make(selectionMakeState, actionMakeState)
        expect(result.isFetching).toBeFalsy()
        expect(result).toEqual(expectedOutput)
    })
})
describe('SelectionMake reducer', () => {
    let makeRequest = {type: MAKE_SELECTION,
    id:10}
    it('should toggle isSelected given value', () => {
        expect(make(selectionMakeState, makeRequest).data.popularMakes[0].isSelected).toBeFalsy()
    })
})
