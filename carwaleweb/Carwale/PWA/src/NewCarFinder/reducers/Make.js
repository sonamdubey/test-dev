import {
	REQUEST_MAKE,
	RECEIVE_MAKE,
	MAKE_SELECTION,
	SET_MAKE_STORED_FILTERS
} from '../actionTypes'

let initialMakeState = {
	isFetching: false,
	data: {
		popularMakes:[],
		otherMakes:[]
	}
}

export const make = (state = initialMakeState, action) => {
	switch (action.type) {
		case REQUEST_MAKE:
			return {
				...state,
				isFetching: true
			}

		case RECEIVE_MAKE:
		let popularSelectionState, otherSelectionState, popularMakes, otherMakes,makedata

		if(state.data.popularMakes!= undefined){
			popularSelectionState = state.data.popularMakes.reduce((acc, item) => {
				acc[item.makeId] = item
				return acc
			}, {})
		}
		if(state.data.otherMakes!= undefined){
			otherSelectionState = state.data.otherMakes.reduce((acc, item) => {
				acc[item.makeId] = item
				return acc
			}, {})
		}
		let selectionState = {...popularSelectionState, ...otherSelectionState}


		popularMakes= action.data.popularMakes.map( item => {
				item.isSelected = (selectionState[item.makeId] != undefined && selectionState[item.makeId].isSelected && item.modelCount > 0)
					return item
				})
		otherMakes= action.data.otherMakes.map( item => {
			item.isSelected = (selectionState[item.makeId] != undefined && selectionState[item.makeId].isSelected && item.modelCount > 0)
				return item
			})

        makedata = {popularMakes: popularMakes, otherMakes: otherMakes}
		return {
			...state,
			isFetching: false,
			data:makedata
		}

		case MAKE_SELECTION:
			const newData = state.data.popularMakes.map(item => {
				if (item.makeId === action.id) {
					return {
						...item,
						isSelected: !item.isSelected
					}
				}
				else {
					return item
				}
			})
			const otherNewData = state.data.otherMakes.map(item => {
				if (item.makeId === action.id) {
						return {
							...item,
							isSelected: !item.isSelected
						}
					}
					else {
						return item
					}
			})
			const concatedData = {popularMakes : newData, otherMakes : otherNewData}
			return {
				...state,
				data: concatedData
			}

		case SET_MAKE_STORED_FILTERS:
			const data = state.data.popularMakes.map(item => {
				if (action.storedFilters.indexOf(item.makeId) > -1 && item.modelCount > 0) {
					return {
						...item,
						isSelected: true
					}
				}
				else {
					return {
						...item,
						isSelected: false
					}
				}
			})
			const otherData = state.data.otherMakes.map(item => {
				if (action.storedFilters.indexOf(item.makeId) > -1 && item.modelCount > 0) {
						return {
							...item,
							isSelected: true
						}
					}
					else {
						return item
					}
			})
			const combinedData = {popularMakes : data, otherMakes : otherData}
			return {
				...state,
				data: combinedData
			}
		default:
			return state
	}
}
