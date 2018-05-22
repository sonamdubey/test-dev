import React from 'react'
import Tabs from './Tabs'
import Disclaimer from './Disclaimer'
import Breadcrumb from '../Shared/Breadcrumb'
import AdUnit from '../AdUnit'
import Footer from '../Shared/Footer'
import { GA_PAGE_MAPPING } from '../../utils/constants'
if (!process.env.SERVER) {
	require('../../../stylesheet/finance.sass');
}

class FinanceComponent extends React.Component {
	constructor(props){
		super(props)
		if(typeof(gaObj)!="undefined")
		{
		    gaObj = GA_PAGE_MAPPING["FinancePage"];
		}
	}
	render() {
		return (
			<div className="finance-content">
				<AdUnit uniqueKey={'finance-page'} tags={null} adSlot={'/1017752/BikeWale_Finance_Top_320x50'} adDimension={[320, 50]} adContainerId={'div-gpt-ad-1525945337139-0'} />
				<div className="finance-content__body">
					<div className="finance-content__head">
						<h1 className="finance-head__title">EMI Calculator - Calculate Your Bike Loan EMI</h1>
					</div>
					<Tabs />
					<Disclaimer />
					<Breadcrumb breadcrumb={[{ Href: '/m/', Title: 'Home' }, { Href: '', Title: 'EMI Calculator' }]} />
				</div>
				<AdUnit uniqueKey={'finance-page'} tags={null} adSlot={'/1017752/BikeWale_Finance_Bottom_320x50'} adDimension={[[320, 50], [320, 100]]} adContainerId={'div-gpt-ad-1525945337139-2'} />
				<Footer/>
			</div>
		);
	}
}

export default FinanceComponent
