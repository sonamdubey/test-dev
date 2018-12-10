import {
	SELECT_OWNER,
	VALIDATE_FORM
} from '../actionTypes'
import {deserialzeQueryStringToObject} from '../../../utils/Common';

let initialOwnersState = {
	isValid: true,
	data: [
		{
			id: 1,
			name: "First owner",
			isSelected: false
		},
		{
			id: 2,
			name: "Second owner",
			isSelected: false
		},
		{
			id: 3,
			name: "Third owner",
			isSelected: false
		},
		{
			id: 8,
			name: "Four or more",
			isSelected: false
		}
	]
}

const getInitialOwnersState = () => {
	let { owner } = deserialzeQueryStringToObject(window.location.search);
	if (owner) {
		for (let i = 0; i < initialOwnersState.data.length; i++) {
			if (initialOwnersState.data[i].id == parseInt(owner)) {
				initialOwnersState.data[i].isSelected = true;
				break;
			}
		}
	}
	return initialOwnersState;
}

const validate = (data) => {
	let isValid = false;
	for (let i = 0; i < data.length; i++) {
		if (data[i].isSelected) {
			isValid = true;
			break;
		}
	}
	return isValid;
}
export const owners = (state = getInitialOwnersState(), action) => {
	switch (action.type) {
		case SELECT_OWNER:
			let newData = state.data.map(item => {
				if (item.id === action.id) {
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
				data: newData,
				isValid: true
			}
		case VALIDATE_FORM:
			return {
				...state,
				isValid: validate(state.data)
			}
		default:
			return state
	}
}
