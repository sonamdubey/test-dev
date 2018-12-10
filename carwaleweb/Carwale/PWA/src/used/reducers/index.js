import { combineReducers } from 'redux'

// common
// city
import { location } from '../../reducers/Location'
import { header } from '../../reducers/Header'
import { footer } from '../../reducers/Footer'

// used car valuation
import { valuation } from '../valuation/reducers'

const usedCar = combineReducers({
	valuation
})

const rootReducer = combineReducers({
	location,
	header,
	usedCar,
	footer
})

export default rootReducer
