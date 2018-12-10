import {
	ADD_MODEL_TO_COMPARE,
	REMOVE_MODEL_FROM_COMPARE
} from '../actionTypes'

import Cookies from 'js-cookie'

const initialCompareCarsState = {
	active: false,
	count: 0,
	max: 2,
	cars: [],
	versionIds: [] //added so that map operation occurs only while adding or removing cars
}

export const compareCars = (state = initialCompareCarsState, action) => {
	let newCount
	let activeState

	switch (action.type) {
		case ADD_MODEL_TO_COMPARE:
			newCount = state.count + 1
			activeState = newCount === state.max ? true : false
			if (newCount <= state.max) {
				const cars = state.cars.concat([action.data])
				const versionIds = state.versionIds.concat([action.data.versionId])
				let teriminator = versionIds.length > 0 ? '|' : ''
				Cookies.set('CompareVersions', versionIds.join('|')+teriminator, { domain: COOKIE_DOMAIN })
				return {
					...state,
					active: activeState,
					count: newCount,
					cars,
					versionIds
				}
			}
			return state

		case REMOVE_MODEL_FROM_COMPARE:
			newCount = state.count - 1
			activeState = newCount < state.max ? false : true
			const cars = state.cars.filter(x => x.versionId != action.versionId)
			const versionIds = cars.map(x => x.versionId)
			let teriminator = versionIds.length > 0 ? '|' : ''
			Cookies.set('CompareVersions', versionIds.join('|')+teriminator, { domain: COOKIE_DOMAIN })
			return {
				...state,
				active: activeState,
				count: newCount,
				cars,
				versionIds
			}

		default:
			return state
	}
}
