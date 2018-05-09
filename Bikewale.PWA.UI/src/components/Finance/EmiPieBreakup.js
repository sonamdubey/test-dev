import React from 'react';
import { connect } from 'react-redux'
import { bindActionCreators } from 'redux';
import { toJS } from '../../immutableWrapperContainer'

import PieChart from './EmiPieChart'
import { formatToINR } from '../../utils/formatAmount'

import { emiCalculatorAction } from '../../actionCreators/emiTenureSlider'

import EMICalculatorHeader from './EMICalculatorHeader'
import DownPaymentSlider from './DownPaymentSlider'
import TenureSlider from './TenureSlider'
import InterestSlider from './InterestSlider'
import {
		EmiCalculation,
		InterestPayable,
		TotalPrincipalAmt
 } from './EmiCalculations'

class PieBreakUp  extends React.Component {
	constructor(props) {
		super(props)

	}
	render() {
		const loanAmount = parseFloat(this.props.sliderDp.max - this.props.sliderDp.values[0])
		let pricipalLoan = formatToINR(loanAmount);
		const pieInterestAmt = formatToINR(this.props.emiCalculationParam.interestPayable)
		return (
				<div>          
					<div className="pie-breakup-data">
							<div className="pie-breakup_unit">
									<div className="pie-breakup_title">
										<span className="pie-breakup_bullet pie-breakup_bullet-interest"></span> Total Interest Payable</div>
									<div className="pie-breakup_amount">{pieInterestAmt}</div>
							</div>
							<div className="pie-breakup_unit">
									<div className="pie-breakup_title">
										<span className="pie-breakup_bullet pie-breakup_bullet-loan"></span> Principle Loan Amount</div>
									<div className="pie-breakup_amount">{pricipalLoan}</div>
							</div>
							<div className="pie-breakup_unit">
									<div className="pie-breakup_title">
										<span className="pie-breakup_bullet-tenure"></span> Tenure
										<div className="pie-breakup_tenure">{this.props.sliderTenure.values[0]} Months</div>
									</div>	
									
							</div>
					</div>
					<div className="pie-breakup-graph">
							<PieChart ref="pieChart" isAnimation={this.props.isAnimation} pieChartObjc={this.props.pieChartObjc} />
					</div>
			</div>
		)
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

module.exports = connect(mapStateToProps, null)(toJS(PieBreakUp));