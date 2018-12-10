import React from 'react'
import { Switch, Route } from 'react-router-dom'
import Make from '../containers/Make'
import BodyType from '../containers/BodyType'
import FuelType from '../containers/FuelType'
import Budget from '../containers/Budget'
import {
	NEWCARFINDER_FILTERS_MAKE_ENDPOINT,
	NEWCARFINDER_FILTERS_BODYTYPE_ENDPOINT,
	NEWCARFINDER_FILTERS_FUELTYPE_ENDPOINT,
	NEWCARFINDER_FILTERS_BUDGET_ENDPOINT,
} from '../constants/index'

class FindCarFilterRouter extends React.Component {
	render() {
		return (
			<Switch>
				<Route path={NEWCARFINDER_FILTERS_MAKE_ENDPOINT} component={Make} />
				<Route path={NEWCARFINDER_FILTERS_BODYTYPE_ENDPOINT} component={BodyType} />
				<Route path={NEWCARFINDER_FILTERS_FUELTYPE_ENDPOINT} component={FuelType} />
				<Route exact path={NEWCARFINDER_FILTERS_BUDGET_ENDPOINT} component = { Budget } />
			</Switch>
		)
	}
}

export default FindCarFilterRouter
