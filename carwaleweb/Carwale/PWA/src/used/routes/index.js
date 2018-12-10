import React from 'react';
import { Switch } from 'react-router-dom';
import ValuationRoutes from '../valuation/routes';

class AppRoutes extends React.Component {
	render() {
		return (
			<Switch>
				<ValuationRoutes />
			</Switch>
		)
	}
}

export default AppRoutes
