import React from 'react';
import { connect } from 'react-redux'
import { bindActionCreators } from 'redux';
import { toJS } from '../../immutableWrapperContainer'
import TenureSlider from './TenureSlider'
import {
		EmiCalculation,
		InterestPayable,
		TotalPrincipalAmt
 } from './EmiCalculations'
 import {
		monthsToYears
 } from '../../utils/formatMonthsYears'

import { formatToINR } from '../../utils/formatAmount'

class EMICalculatorHeader extends React.Component {
	constructor(props) {
		super(props);
		this.state = {monthsToYears: monthsToYears(this.props.sliderTenure.values)}
		}
	componentWillReceiveProps(nextProps){
		this.setState({monthsToYears: monthsToYears(nextProps.sliderTenure.values)})
	 }
	 
	render() {
				let finalEmi = formatToINR(this.props.emiCalculationParam.emiCalculation)
				let tenureText = (this.state.monthsToYears)
				let orpAmount = formatToINR(this.props.sliderDp.max)
		return (
				<div className="emi-calci-header">
					 <div className="emi-calci-top-header">
						 <div className="emi-calci__orp-text">On-road price, Navi Mumbai</div>
						 <div className="emi-calci__orp-data">
							 <span>{orpAmount}</span>
							 <a href="" className="price-link">View detailed price</a>
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

const mapStateToProps = (state) => {
	const sliderDp = state.getIn(['Emi', 'VehicleDownPayment', 'slider'])
	const sliderTenure = state.getIn(['Emi', 'VehicleTenure', 'slider'])
	const sliderInt = state.getIn(['Emi', 'VehicleInterest', 'slider'])

	return {
		sliderDp,
		sliderTenure,
		sliderInt
	}
}
module.exports = connect(mapStateToProps, null)(toJS(EMICalculatorHeader));
