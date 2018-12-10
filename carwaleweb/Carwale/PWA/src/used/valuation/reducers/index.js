import { combineReducers } from 'redux'

import { type } from './ValuationType'
import { manufacturingDetails } from './ManufacturingDetails'
import { city } from './City'
import { car } from './Car'
import { owners } from './Owners'
import { kmsDriven } from './Kilometer'
import { report } from './Report'

export const valuation = combineReducers({
	type,
	manufacturingDetails,
	city,
	car,
	owners,
	kmsDriven,
	report
})
