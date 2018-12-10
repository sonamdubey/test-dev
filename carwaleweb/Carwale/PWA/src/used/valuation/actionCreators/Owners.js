import {
	SELECT_OWNER
} from '../actionTypes'

const ownerSelection = id => {
	return {
		type: SELECT_OWNER,
		id
	}
}

const shouldOwnerUpdate = item => {
	if (!item.isSelected) {
		return true
	}
	return false
}

export const selectOwner = item => (dispatch) => {
	if (shouldOwnerUpdate(item)) {
		dispatch(ownerSelection(item.id))
	}
}
