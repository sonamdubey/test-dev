import React from 'react'
import { Switch, Route, Router, Redirect } from 'react-router-dom'

import FindCar from '../containers/FindCar'
import FindCarFilters from '../containers/FindCarFilters'
import Listing from '../containers/Listing'
import {
	NEWCARFINDER_ENDPOINT,
	NEWCARFINDER_FILTERS_ENDPOINT,
	NEWCARFINDER_RESULTS_ENDPOINT,
	NEWCARFINDER_SEO_SINGLE_DIMENSION_ENDPOINT,
	SEO_TYPE
} from '../constants/index'
import { mapPropsObjectFromSEOUrls } from '../utils/filter-mapings'

class AppRoutes extends React.Component {
	render() {
		return (
			<Switch>
				<Route
					exact path={NEWCARFINDER_ENDPOINT}
					component={FindCar}
				/>
				<Route
					path={NEWCARFINDER_FILTERS_ENDPOINT}
					component={FindCarFilters}
				/>
				<Route
					path={NEWCARFINDER_RESULTS_ENDPOINT}
					render={(props) => <Listing isSEOPage={false} {...props}/> }
				/>
				<Route
					exact
					path={NEWCARFINDER_SEO_SINGLE_DIMENSION_ENDPOINT}
					render={(props) => <Listing isSEOPage={true} {... mapPropsObjectFromSEOUrls(props,SEO_TYPE.SEO_SINGLE)}/> }
				/>
			</Switch>
		)
	}
}

export default AppRoutes
