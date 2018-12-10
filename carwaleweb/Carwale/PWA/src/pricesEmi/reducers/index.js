import { combineReducers } from 'redux'

import {
	vehicleDownPayment
} from './emiDownPayment'

import {
	vehicleTenure
} from './emiTenure'

import {
	vehicleInterest
} from './emiInterest'

import {
	vehiclePie
} from './emiPieData'

import {
	campaignTemplate
} from './dealerCampaignTemplate'

import {
	vehicleData
} from './VehicleData'

import {
	emiPopupState
} from './emiPopupState'

import {
	toast
} from '../../reducers/Toast'

import {
	newEmiPrices
} from './Prices'

const rootReducer = combineReducers({
	toast,
	newEmiPrices,
	emiPopupState
})

export default rootReducer
