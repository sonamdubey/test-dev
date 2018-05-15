import React from 'react'
import Tabs from './Tabs'
import Disclaimer from './Disclaimer'
import Breadcrumb from '../Shared/Breadcrumb'

if (!process.env.SERVER) {
	require('../../../stylesheet/finance.sass');
}

class FinanceComponent extends React.Component {
	render() {
		return (
			<div className="finance-content">
				<div className="finance-content__body">
					<div className="finance-content__head">
					<h1 className="finance-head__title">Finance Know-how</h1>
				</div>
				<Tabs />
				<Disclaimer />
				<Breadcrumb breadcrumb={[{Href : '/m/',Title : 'Home'}, {Href : '',Title : 'EMI Calculator'}]}/>
			</div>
			</div>
		);
}
}

export default FinanceComponent
