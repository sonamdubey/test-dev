import {
	REQUEST_BODY_TYPE,
	RECEIVE_BODY_TYPE,
	BODY_TYPE_SELECTION,
	SET_BODY_STORED_FILTERS
} from '../actionTypes'

let initialBodyTypeState = {
	isFetching: false,
	data: []
}

export const bodyType = (state = initialBodyTypeState, action) => {
	switch (action.type) {
		case REQUEST_BODY_TYPE:
			return {
				...state,
				isFetching: true
			}

		case RECEIVE_BODY_TYPE:
			let selectionState = state.data.reduce((acc, item) => {
				acc[item.id] = item
				return acc
			}, {})

			return {
				...state,
				isFetching: false,
				data: state.data.length ? action.data.map(item => {
					item.isSelected = (!(item.isRecommended ^ selectionState[item.id].isRecommended) ? selectionState[item.id].isSelected : item.isSelected) && item.carCount > 0
					return item
				}) : action.data
			}

		case BODY_TYPE_SELECTION:
			const newData = state.data.map(item => {
				if (item.id === action.id) {
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

		case SET_BODY_STORED_FILTERS:
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
