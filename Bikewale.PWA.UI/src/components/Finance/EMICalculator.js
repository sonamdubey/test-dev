import React from 'react';
import { connect } from 'react-redux'
import { bindActionCreators } from 'redux';
import { toJS } from '../../immutableWrapperContainer'

import EMICalculatorHeader from './EMICalculatorHeader'
import DownPaymentSlider from './DownPaymentSlider'
import TenureSlider from './TenureSlider'
import InterestSlider from './InterestSlider'
import Spinner from '../Shared/SpinnerRelative'

import {
	EmiCalculation,
	InterestPayable,
	TotalPrincipalAmt
} from './EmiCalculations'
import PieBreakUp from './EmiPieBreakup'
import { formatToINR } from '../../utils/formatAmount'
import { openEmiCalculator } from '../../actionCreators/emiDownPaymentSlider'
export class EMICalculator extends React.Component {
	constructor(props) {
		super(props);
		let loanAmount = this.props.sliderDp.onRoadPrice - this.props.sliderDp.values[0];
		this.state = {
			emiCalculation: EmiCalculation(loanAmount, this.props.sliderTenure.values[0], this.props.sliderInt.values[0]),
			interestPayable: InterestPayable(loanAmount, this.props.sliderTenure.values[0], this.props.sliderInt.values[0]),
			totalBikeCost: TotalPrincipalAmt(this.props.sliderDp.values[0], loanAmount, this.props.sliderTenure.values[0], this.props.sliderInt.values[0])
		}
	}
	componentWillReceiveProps(nextProps) {
		let loanAmount = nextProps.sliderDp.onRoadPrice - nextProps.sliderDp.values[0];
		this.setState({
			emiCalculation: EmiCalculation(loanAmount, nextProps.sliderTenure.values[0], nextProps.sliderInt.values[0]),
			interestPayable: InterestPayable(loanAmount, nextProps.sliderTenure.values[0], nextProps.sliderInt.values[0]),
			totalBikeCost: TotalPrincipalAmt(nextProps.sliderDp.values[0], loanAmount, nextProps.sliderTenure.values[0], nextProps.sliderInt.values[0]),
		})
	}
	render() {
		if (this.props.sliderDp.onRoadPrice === 0) {
			return null;
		}
		let pieInterestAmt = (this.state.interestPayable);
		let pieLoanAmt = (this.props.sliderDp.onRoadPrice - this.props.sliderDp.values[0]);
		let pieTotalAmtPay = this.state.totalBikeCost;
		let finalIntPayable = formatToINR(parseInt(this.state.InterestPayable));
		let finalTotalPrincipalAmt = formatToINR(this.state.totalBikeCost);
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
			cityData: this.props.financeCitySelection
		}
		return (
			this.props.IsFetching ?
			<div className="emi-spinner-container">
				<Spinner />
			</div>
			:
			<div className="emi-outer-container">
				<div className="emi-calci-container">
					<EMICalculatorHeader sliderData={sliderData} emiCalculationParam={emiCalculationParam} />
					<div className="emi-slider-container">
						<DownPaymentSlider />
						<div className="tenure-interest-slider">
							<div className="slider-input-container tenure-unit">
								<TenureSlider />
							</div>
							<div className="slider-input-container interest-unit">
								<InterestSlider />
							</div>
						</div>
					</div>
				</div>
				<div className="view-breakup-container">
					<PieBreakUp ref="piebreakup" sliderData={sliderData} isAnimation={this.props.pieAnimate} emiCalculationParam={emiCalculationParam} pieChartData={pieChartData} />
				</div>
			</div>
		);
	}
}

const mapStateToProps = (state) => {
	const sliderDp = state.getIn(['Emi', 'VehicleDownPayment', 'slider'])
	const sliderTenure = state.getIn(['Emi', 'VehicleTenure', 'slider'])
	const sliderInt = state.getIn(['Emi', 'VehicleInterest', 'slider'])
	const financeCitySelection = state.getIn(['Finance', 'FinanceCityPopup', 'Selection'])
	const selectBikePopup = state.getIn(['Finance', 'SelectBikePopup', 'Selection'])
	return {
		sliderDp,
		sliderTenure,
		sliderInt,
		financeCitySelection,
		selectBikePopup
	}
}

export default connect(mapStateToProps, null)(toJS(EMICalculator));
