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
			totalBikeCost: TotalPrincipalAmt(nextProps.sliderDp.values[0], loanAmount, nextProps.sliderTenure.values[0], nextProps.sliderInt.values[0])
		})
		const {
			selectBikePopup,
			openEmiCalculator
		} = this.props
		if(nextProps.selectBikePopup.versionList.length > 0 && nextProps.selectBikePopup.selectedVersionIndex > -1
			&& nextProps.sliderDp.onRoadPrice > 0
			&& (this.props.sliderDp.onRoadPrice != nextProps.sliderDp.onRoadPrice
				|| selectBikePopup.selectedVersionIndex != nextProps.selectBikePopup.selectedVersionIndex)){
					openEmiCalculator(nextProps.selectBikePopup.versionList[nextProps.selectBikePopup.selectedVersionIndex].price)
				}
	}
	// shouldComponentUpdate(nextProps, nextState) {
	// 	const {
	// 		selectBikePopup,
	// 		sliderDp,
	// 		sliderTenure,
	// 		sliderInt
	// 	} = this.props
	// 	if (nextProps.sliderInt.values[0] !== sliderInt.values[0] || nextProps.sliderTenure.values[0] !== sliderTenure.values[0]
	// 		|| nextProps.sliderDp.values[0] !== sliderDp.values[0]) {
	// 		return true
	// 	}
	// 	else {
	// 		return selectBikePopup.versionList.length > 0 && selectBikePopup.selectedVersionIndex > -1
	// 			&& nextProps.sliderDp.onRoadPrice > 0
	// 			&& (this.props.sliderDp.onRoadPrice != nextProps.sliderDp.onRoadPrice
	// 				|| selectBikePopup.selectedVersionIndex != nextProps.selectBikePopup.selectedVersionIndex)

	// 	}

	// }
	componentWillMount() {
		const {
			selectBikePopup,
			openEmiCalculator
		} = this.props
		if (selectBikePopup.versionList.length > 0 && selectBikePopup.selectedVersionIndex > -1) {
			openEmiCalculator(selectBikePopup.versionList[selectBikePopup.selectedVersionIndex].price)
		}
	}
	// componentDidUpdate() {
	// 	const {
	// 		selectBikePopup,
	// 		openEmiCalculator
	// 	} = this.props
	// 	openEmiCalculator(selectBikePopup.versionList[selectBikePopup.selectedVersionIndex].price)
	// }
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
	const pieAnimate = state.getIn(['Emi', 'PieAnimation', 'isAnimate'])
	const financeCitySelection = state.getIn(['Finance', 'FinanceCityPopup', 'Selection'])
	const selectBikePopup = state.getIn(['Finance', 'SelectBikePopup', 'Selection'])
	return {
		sliderDp,
		sliderTenure,
		sliderInt,
		pieAnimate,
		financeCitySelection,
		selectBikePopup
	}
}
const mapDispatchToProps = (dispatch) => {
	return {
		openEmiCalculator: bindActionCreators(openEmiCalculator, dispatch)
	}
}

export default connect(mapStateToProps, mapDispatchToProps)(toJS(EMICalculator));
