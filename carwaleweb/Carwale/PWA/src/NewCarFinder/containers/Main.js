import React from 'react'
import { Switch, Route, withRouter } from 'react-router-dom'
import AppRoutes from '../routes'

import Toast from '../../components/Toast'
import HeaderWrapper from './HeaderWrapper'
import NavigationDrawer from '../../containers/NavigationDrawer'

import { firePageView } from '../../utils/Analytics'
import { trackCustomData } from '../../utils/cwTrackingPwa'
import {PAGE_VIEW_CATEGORY} from '../constants/index'
import Location from './Location'

class Main extends React.Component {
	componentWillMount() {
		trackCustomData(PAGE_VIEW_CATEGORY,this.props.location.pathname, "NA", false)
	}

	componentDidUpdate (prevProps) {
		if(this.props.location.pathname != prevProps.location.pathname
			|| this.props.location.search != prevProps.location.search){
				this.handleLocationChange(this.props.location)
		}
	}
	handleLocationChange = (location) => {
		firePageView(location)
		trackCustomData(PAGE_VIEW_CATEGORY, location.pathname, "NA", false)
	}

	render() {
		return (
			<div>
				<HeaderWrapper />
				<AppRoutes />
				<Location/>
				<Toast />
				<NavigationDrawer />
			</div>
		)
	}
}

export default withRouter(Main)
