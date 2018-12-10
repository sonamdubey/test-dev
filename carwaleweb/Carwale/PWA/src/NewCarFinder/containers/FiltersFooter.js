import React from 'react'
import PropTypes from 'prop-types'
import { connect } from 'react-redux'
import { bindActionCreators } from 'redux'
import { withRouter } from 'react-router-dom'

import {
	createRipple
} from '../../utils/Ripple'

import NewCarSearchApi from '../apis/NewCarSearch'

import {
	fireInteractiveTracking
} from '../../utils/Analytics'

import {
	makeCancelable
} from '../../utils/CancelablePromise'

import {
	hideShortlistPopup
} from '../actionCreators/Shortlist'

import {
	getNCFFilterParams
} from '../selectors/NCFSelectors'
import { serialzeObjectToQueryString } from '../../utils/Common'
import { NEWCARFINDER_RESULTS_ENDPOINT, BUDGET_SLIDER_MIN } from '../constants/index'
import {CATEGORY_NAME} from '../constants/index';
import { trackCustomData } from '../../utils/cwTrackingPwa';

class FiltersFooter extends React.Component {
	constructor(props) {
		super(props)
		this.state = {
			count: 0
		}
		this.apiRequest = null
		this.newCarSearchApiCalls = {}
	}
	updateResultCount = (filterParams) => {
		this.apiRequest && this.apiRequest.cancel()
		if (filterParams.budget && filterParams.budget >= BUDGET_SLIDER_MIN) {
			let apiRequest = makeCancelable(NewCarSearchApi.getCount(filterParams))
			apiRequest
				.promise
				.then(x => {
					this.setState({ count: x.totalModels })
					this.newCarSearchApiCalls[JSON.stringify(filterParams)] = x.totalModels
				})
				.catch(error => {
					if (error.isCanceled) {
						console.log("Request cancelled for: ", filterParams.budget)
					}
					else {
						console.log(error)
						//TODO: handle search api failure
					}
				})
			this.apiRequest = apiRequest
		}
		else {
			this.setState({ count: 0 })
		}
	}

	componentDidMount(){
		const { filterParams } = this.props
		Object.keys(filterParams).length > 1 && this.updateResultCount(filterParams)
	}

	shouldComponentUpdate(nextProps, nextState){
		return (nextState.count != this.state.count)
	}

	componentWillReceiveProps(nextProps){
		if(this.props.removedModelIds != undefined){
			nextProps.filterParams.removedModelIds =this.props.removedModelIds
		}
		const nextFilterParamsStr = JSON.stringify(nextProps.filterParams)
		if (nextFilterParamsStr != JSON.stringify(this.props.filterParams)) {
			if (this.newCarSearchApiCalls[nextFilterParamsStr] == undefined) {
				this.updateResultCount(nextProps.filterParams)
			}
			else {
				this.apiRequest && this.apiRequest.cancel()
				this.setState({ count: this.newCarSearchApiCalls[nextFilterParamsStr] })
			}
		}
	}

	handleNextFilter = (event) => {
		if (this.state.count) {
			createRipple(event)
			fireInteractiveTracking(CATEGORY_NAME, "NCF_" + this.props.currentScreenName, "Next Click")
			trackCustomData(CATEGORY_NAME,"FilterPageNextClick","screen="+this.props.currentScreenName,false)
			this.props.history.push(this.props.nextPath)
			console.log(this.props.history)
		}
	}

	handleShowResult = (event) => {
		if (this.state.count) {
			createRipple(event)
			fireInteractiveTracking(CATEGORY_NAME, "NCF_" + this.props.currentScreenName, "Show Result Click")
			trackCustomData(CATEGORY_NAME,"FilterPageShowResultsClick","screen="+this.props.currentScreenName,false)
			this.props.history.push(NEWCARFINDER_RESULTS_ENDPOINT + '?' + serialzeObjectToQueryString(this.props.filterParams))
		}
		this.props.hideShortlistPopup()
	}

	componentWillUnmount(){
		this.apiRequest && this.apiRequest.cancel()
	}

	render() {
		const { count } = this.state
		const activeClass = count > 0 ? "active" : ""

		return (
			<div id="filtersFooter" className="filters__footer">
				<span
					className={"filters__result " + activeClass}
					onClick={this.handleShowResult}
				>
					{count > 0 ? `Show Result${count > 1 ? 's' : ''} (${count})` : 'No Result'}
				</span>
				<span
					className={"filters__next-btn " + activeClass}
					onClick={this.handleNextFilter}
				>
					Next
				</span>
			</div>
		)
	}
}


const mapStateToProps = (state) => {
	const filterScreens = state.newCarFinder.filter.filterScreens
	const currentScreen = filterScreens.currentScreen
	const currentFilters = filterScreens.screenOrder.slice(0, currentScreen + 1).reduce((acc, screen) => acc.concat([screen.name]), [])
	const filterParams = getNCFFilterParams(state, currentFilters)
	const nextPath = filterScreens.screenOrder.length > currentScreen + 1 ? filterScreens.screenOrder[currentScreen + 1].path : (NEWCARFINDER_RESULTS_ENDPOINT + '?' + serialzeObjectToQueryString(filterParams))
	const currentScreenName = currentScreen >= 0 && currentScreen < filterScreens.screenOrder.length ? filterScreens.screenOrder[currentScreen].name : ''
  const removedModelIds = state.newCarFinder.shortlistCars.removedModelIds
	return {
		nextPath,
		filterParams,
		currentScreenName,
		removedModelIds
	}
}
const mapDispatchToProps = (dispatch,getState) => {
	return {
		hideShortlistPopup: bindActionCreators(hideShortlistPopup, dispatch)
	}
}

export default withRouter(connect(mapStateToProps,mapDispatchToProps)(FiltersFooter))
