import React from 'react';
import { connect } from 'react-redux'
import { bindActionCreators } from 'redux';
import { toJS } from '../../immutableWrapperContainer'

import EMICalculatorHeader from './EMICalculatorHeader'
import DownPaymentSlider from './DownPaymentSlider'
import TenureSlider from './TenureSlider'
import InterestSlider from './InterestSlider'
import {
		EmiCalculation,
		InterestPayable,
		TotalPrincipalAmt
 } from './EmiCalculations'
import PieBreakUp from './EmiPieBreakup'
import { formatToINR } from '../../utils/formatAmount'

class EMICalculator extends React.Component {
	constructor(props) {
		super(props);
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
	 
	render() {
		let pieInterestAmt = (this.state.interestPayable)
		let pieLoanAmt = (this.props.sliderDp.max - this.props.sliderDp.values[0]); 
		let pieTotalAmtPay = this.state.totalPrincipalAmt  
		let finalIntPayable = formatToINR(parseInt(this.state.InterestPayable))
		let finalTotalPrincipalAmt = formatToINR(this.state.totalPrincipalAmt)
		let pieChartData = {
			intStatePay: pieInterestAmt,
			pieLoanAmt: pieLoanAmt,
			pieTotalPay: pieTotalAmtPay,
			interestPay: finalIntPayable,
			TotalPrincipalAmt: finalTotalPrincipalAmt
		}
		let emiCalculationParam = {
			emiCalculation: this.state.emiCalculation,
			interestPayable: this.state.interestPayable,
			totalPrincipalAmt: this.state.totalPrincipalAmt
		}
		let sliderData = {
			sliderDpData: this.props.sliderDp,
			sliderTenureData: this.props.sliderTenure,
			sliderInterestData: this.props.sliderInt,
		}
		return (
			<div className="emi-outer-container">
				<div className="emi-calci-container">
					 <EMICalculatorHeader sliderData={sliderData} emiCalculationParam={emiCalculationParam} />
					 <div className="emi-slider-container">
							<DownPaymentSlider/>
							<div className="tenure-interest-slider">
								<div className="slider-input-container tenure-unit">
									<TenureSlider/>
								</div>
								<div className="slider-input-container interest-unit">
									<InterestSlider/>
								</div>
							</div>
						</div>
						
				</div>
				<div className="view-breakup-container">
						<PieBreakUp ref="piebreakup" sliderData={sliderData} isAnimation={this.props.pieAnimate} emiCalculationParam={emiCalculationParam} pieChartData={pieChartData}/>
				</div>
			</div>
		);
	}
}

const mapStateToProps = (state) => {
	const sliderDp = state.getIn(['Emi', 'VehicleDownPayment', 'slider'])
	const sliderTenure = state.getIn(['Emi', 'VehicleTenure', 'slider'])
	const sliderInt = state.getIn(['Emi', 'VehicleInterest', 'slider'])
	const pieAnimate = state.getIn(['Emi', 'PieAnimation', 'isAnimate'])

	return {
		sliderDp,
		sliderTenure,
		sliderInt,
		pieAnimate
	}
}
module.exports = connect(mapStateToProps, null)(toJS(EMICalculator));
