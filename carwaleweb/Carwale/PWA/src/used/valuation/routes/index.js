import React from 'react';
import { Switch, Route } from 'react-router-dom';
import { VALUATION_ENDPOINT } from '../constants';
import Valuation from '../containers/Valuation';
const ValuationRoutes = () => {
	return (
		<Switch>
			<Route
				exact path={VALUATION_ENDPOINT}
				component={Valuation}
			/>
		</Switch>
	)
}

export default ValuationRoutes
