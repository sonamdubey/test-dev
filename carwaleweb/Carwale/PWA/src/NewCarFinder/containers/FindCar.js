import React from 'react'
import { bindActionCreators } from 'redux'
import { Redirect } from 'react-router-dom'
import { storage } from '../../utils/Storage'
import { fireNonInteractiveTracking } from '../../utils/Analytics'
import { NEWCARFINDER_FILTERS_BUDGET_ENDPOINT } from '../constants/index'
import { connect } from 'react-redux'
import { fetchFilterScreen } from '../actionCreators/Filter'
import { trackCustomData } from '../../utils/cwTrackingPwa'
import {CATEGORY_NAME} from '../constants/index'
/**
 * This class represents the landing component
 * for New car finder
 * @class FindCar
 * @extends {React.Component}
 */
class FindCar extends React.Component {
	constructor(props) {
		super(props)
	}

	componentDidMount() {
		const { fetchFilterScreen } = this.props
		fetchFilterScreen()
	}

	componentWillMount(){
		let ncfVisit = storage.getValue('NCFVisit')
		let visitCount = 1
		let currentVisitTimeStamp = Date.now()
		if (ncfVisit) { //returning user
			let lastVisitStartTimeStamp

			[visitCount, lastVisitStartTimeStamp] = ncfVisit.split('|')

			visitCount = parseInt(visitCount)

			//parse and check time stamp diff > 30 min
			lastVisitStartTimeStamp = parseInt(lastVisitStartTimeStamp)

			if ((currentVisitTimeStamp - lastVisitStartTimeStamp) > 1800000) {
				visitCount = visitCount + 1 //increase visit count
				//fire tracking
				fireNonInteractiveTracking(CATEGORY_NAME, 'Visit', 'Returning User')
				trackCustomData(CATEGORY_NAME,'ReturningUserVisit',"visitCount="+visitCount,false)
			}
		}
		else { //new user
			//set new user tracking label
			fireNonInteractiveTracking(CATEGORY_NAME, 'Visit', 'New User')
			trackCustomData(CATEGORY_NAME,'NewUserVisit',"visitCount="+visitCount,false)
		}
		//store NCFVisit value with count and currTS
		storage.setValue('NCFVisit', visitCount + '|' + currentVisitTimeStamp)
	}

	render() {
			return (
				<Redirect to={this.props.nextPath} />
			)
	}
}

const mapStateToProps = () => {
	const nextPath =  NEWCARFINDER_FILTERS_BUDGET_ENDPOINT
	return {
		nextPath
	}
}
const mapDispatchToProps = (dispatch) => {
	return {
		fetchFilterScreen: bindActionCreators(fetchFilterScreen, dispatch)
	}
}

export default connect(mapStateToProps, mapDispatchToProps)(FindCar)
