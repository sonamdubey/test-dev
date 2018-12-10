import { combineReducers } from 'redux'

import {
	NEXT_FILTER
} from '../actionTypes'


import {
	bodyType
} from './BodyType'

import {
	fuelType
} from './FuelType'

import {
	transmissionType
} from './TransmissionType'

import {
	seats
} from './Seats'

import {
	make
} from './Make'

import {
	toast
} from '../../reducers/Toast'

// budget
import {
	budget
} from './Budget'
// city
import {
	location
} from '../../reducers/Location'

import isActive   from '../../../src/Location/reducers/Popup'
// budget
import {
	filter
} from './Filter'

// model list
import {
	listingData
} from './ModelList'

import {
	filtersScreen
} from '../../filterPlugin/reducers/FiltersScreen'

import {
	compareCars
} from './CompareCars'

import {
	header
} from '../../reducers/Header'

import {
	footer
} from '../../reducers/Footer'
import {
	shortlistCars
} from './Shortlist'
import {
	headerWrapper
} from './HeaderWrapper'



const newCarFinder = combineReducers({
	budget,
	filter,
	bodyType,
	fuelType,
	transmissionType,
	seats,
	make,
	listingData,

	shortlistCars,
	headerWrapper,
	locationPopupActive:isActive
})

const rootReducer = combineReducers({
	location,
	header,
	newCarFinder,
	toast,
	compareCars,
	footer,
	filtersScreen
})

export default rootReducer
