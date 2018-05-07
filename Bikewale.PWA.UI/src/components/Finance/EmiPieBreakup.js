import React from 'react';
import { connect } from 'react-redux'
import { bindActionCreators } from 'redux';
import { toJS } from '../../immutableWrapperContainer'

import PieChart from './EmiPieChart'
import { formatToINR } from '../../utils/formatAmount'

import { emiCalculatorAction } from '../../actionCreators/emiTenureSlider'
import { updatePieValueData } from '../../actionCreators/emiPieData'

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
		this.state = {emiCalculation: EmiCalculation(this.props.sliderDp, this.props.sliderTenure, this.props.sliderInt),
									interestPayable: InterestPayable(this.props.sliderDp, this.props.sliderTenure, this.props.sliderInt),
									totalPrincipalAmt: TotalPrincipalAmt(this.props.sliderDp, this.props.sliderTenure, this.props.sliderInt),
									}
	}
	componentWillReceiveProps(nextProps){
		this.setState({ emiCalculation : EmiCalculation(nextProps.sliderDp, nextProps.sliderTenure, nextProps.sliderInt),
										interestPayable : InterestPayable(nextProps.sliderDp, nextProps.sliderTenure, nextProps.sliderInt),
										totalPrincipalAmt : TotalPrincipalAmt(nextProps.sliderDp, nextProps.sliderTenure, nextProps.sliderInt),
									})
	 }
	componentDidMount(){
		const {
				updatePieValueData
		} = this.props
	}
	render() {
		const loanAmount = parseFloat(this.props.sliderDp.max - this.props.sliderDp.values[0])
		let pricipalLoan = formatToINR(loanAmount);
		const PieInterestAmt = formatToINR(this.state.interestPayable)
		let colors = ['#f45944', '#fed1cd'];
		return (
				<div>          
					<div className="pie-breakup-data">
							<div className="breakup-unit">
									<div className="title-text">
										<span className="pieBullet" style={{ backgroundColor: colors[0]}}></span> Total Interest Payable</div>
									<div className="breakup-amount">{PieInterestAmt}</div>
							</div>
							<div className="breakup-unit">
									<div className="title-text">
										<span className="pieBullet" style={{ backgroundColor: colors[1]}}></span> Principle Loan Amount</div>
									<div className="breakup-amount">{pricipalLoan}</div>
							</div>
							<div className="breakup-unit">
									<div className="title-text">Tenure {this.props.sliderTenure.values[0]}</div>
							</div>
					</div>
					<div className="pie-breakup-graph">
							<PieChart ref="pieChart" isAnimation={this.props.isAnimation} intStatePay={this.props.intStatePay} pieLoanAmt={this.props.pieLoanAmt} pieTotalPay={this.props.pieTotalPay} />
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

const mapDispatchToProps = (dispatch, getState) => {
	return {
		updatePieValueData: bindActionCreators(updatePieValueData, dispatch)
	}
}

module.exports = connect(mapStateToProps, mapDispatchToProps)(toJS(PieBreakUp));