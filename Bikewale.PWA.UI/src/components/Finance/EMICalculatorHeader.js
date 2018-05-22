import React from 'react';
import { connect } from 'react-redux'
import { bindActionCreators } from 'redux';
import { toJS } from '../../immutableWrapperContainer'
 import {
		monthsToYears
 } from '../../utils/formatMonthsYears'

import { formatToINR } from '../../utils/formatAmount'

class EMICalculatorHeader extends React.Component {
	constructor(props) {
		super(props);
		this.state = {monthsToYears: monthsToYears(this.props.sliderData.sliderTenureData.values)}
		}
	componentWillReceiveProps(nextProps){
		this.setState({monthsToYears: monthsToYears(nextProps.sliderData.sliderTenureData.values)})
	 }
	 
	render() {
				let finalEmi = formatToINR(this.props.emiCalculationParam.emiCalculation)
				let tenureText = (this.state.monthsToYears)
				let orpAmount = formatToINR(this.props.sliderData.sliderDpData.onRoadPrice)
		return (
				<div className="emi-calci-header">
					 <div className="emi-calci-top-header">
						 <div className="emi-calci__orp-text">On-road price, {this.props.sliderData.cityData.cityName}</div>
						 <div className="emi-calci__orp-data">
							 <span>{orpAmount}</span>
							 <a href="" className="price-link" title="View detailed price">View detailed price</a>
						 </div>
					 </div>
					<div className="emi-cost-container">
						<div className="emi-text">EMI</div>
						<div className="emi-total-cost">
							<span className="final-emi-text">{finalEmi}</span>
							<span className="emi-total-year">per month for <span className="emi-total-year-text">{tenureText}</span></span>
						</div>
					</div>
				</div>
		);
	}
}

export default (EMICalculatorHeader);
