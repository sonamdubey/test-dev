import { FETCHING_VALUATION, FETCHED_VALUATION_SUCCESS, SET_REDIRECT_TO_CAMPAIGN, CLOSE_VALUATION_REPORT } from '../actionTypes/index'
import { BUY_CAR_ID } from '../constants/index'
import {
	serialzeObjectToQueryString
} from './../../../utils/Common'
let initialReportState = {
	active: false,
	isFetching: false,
	valuationHtml: '',
	valuationQs: '',
	campaignUrl: '',
	valuationQs: ''
}

export const report = (state = initialReportState, action) => {
	switch (action.type) {
		case FETCHING_VALUATION:
			return {
				...state,
				isFetching: true,

			}
		case FETCHED_VALUATION_SUCCESS:
			return {
				...state,
				active: true,
				valuationHtml: action.valuationData,
				isFetching: false,
				valuationQs: action.queryString
			}
		case SET_REDIRECT_TO_CAMPAIGN:
			return {
				...state,
				campaignUrl: getCampaignUrl(action.dataObj, action.campaignType)
			}
		case CLOSE_VALUATION_REPORT:
			return {
				...state,
				active: false
			}
		default:
			return state
	}
}

const getCampaignUrl = (dataObj, campaignType) => {
	if (campaignType == BUY_CAR_ID) {
		const param = dataObj.buyCarParam
		return '/used/' + processMakeNameTerm(param.makeName) + '-' + processUrlTerms(param.rootName) + '-cars-in-' + processUrlTerms(param.cityMaskingName) + '/'
	}
	else {
		return '/used/sell/?' + serialzeObjectToQueryString(dataObj.sellCarParam)
	}
}

const processUrlTerms = (term) => {
	return term.replace(/\s+/g, '').toLowerCase();
}

const processMakeNameTerm = (term) => {
	return term.replace("-"," ").replace(/\s+/g, '').toLowerCase();
}
