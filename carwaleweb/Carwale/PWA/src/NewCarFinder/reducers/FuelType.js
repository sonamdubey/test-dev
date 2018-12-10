import {
	REQUEST_FUEL_TYPE,
	RECEIVE_FUEL_TYPE,
	FUEL_TYPE_SELECTION,
	SET_FUEL_STORED_FILTERS
} from '../actionTypes'

let initialFuelTypeState = {
	isFetching: false,
	data: []
}

export const fuelType = (state = initialFuelTypeState, action) => {
	switch (action.type) {
		case REQUEST_FUEL_TYPE:
			return {
				...state,
				isFetching: true
			}

		case RECEIVE_FUEL_TYPE:
			let selectionState = state.data.reduce((acc, item) => {
				acc[item.id] = item
				return acc
			}, {})

			return {
				...state,
				isFetching: false,
				data: state.data.length ? action.data.map( item => {
					item.isSelected = (selectionState[item.id].isSelected && item.carCount > 0 )
					return item
				}) : action.data
			}

		case FUEL_TYPE_SELECTION:
			const newData = state.data.map(item => {
				if(item.id === action.id) {
					return {
						...item,
						isSelected: !item.isSelected
					}
				}
				else {
					return {
						...item
					}
				}
			})

			return {
				...state,
				data: newData
			}

			case SET_FUEL_STORED_FILTERS:
				const data = state.data.map(item => {
					if(action.storedFilters.indexOf(item.id) > -1 && item.carCount > 0) {
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

				return {
					...state,
					data: data
				}

		default:
			return state
	}
}
