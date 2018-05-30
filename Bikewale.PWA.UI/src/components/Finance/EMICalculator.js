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

function getEMICalulatorData(sliderDp, sliderInt, sliderTenure) {
	const loanAmount = sliderDp.onRoadPrice - sliderDp.values[0];
	const emiCalculation = EmiCalculation(loanAmount, sliderTenure.values[0], sliderInt.values[0])
	const interestPayable = InterestPayable(loanAmount, sliderTenure.values[0], sliderInt.values[0])
	const totalBikeCost = TotalPrincipalAmt(sliderDp.values[0], loanAmount, sliderTenure.values[0], sliderInt.values[0])

	const downpayment = sliderDp.values[0]
	const tenure = sliderTenure.values[0]

	return {
		emiCalculation,
		interestPayable,
		totalBikeCost,
		downpayment,
		tenure
	}
}

export class EMICalculator extends React.Component {
	constructor(props) {
		super(props);
		this.state = getEMICalulatorData(this.props.sliderDp, this.props.sliderInt, this.props.sliderTenure)
	}

	componentWillReceiveProps(nextProps) {
		this.setState(getEMICalulatorData(nextProps.sliderDp, nextProps.sliderInt, nextProps.sliderTenure))
	}

	handleDownPaymentSliderDragMove = (sliderDp) => {
		this.setState(getEMICalulatorData(sliderDp, this.props.sliderInt, this.props.sliderTenure))
	}

	handleTenureSliderDragMove = (sliderTenure) => {
		this.setState(getEMICalulatorData(this.props.sliderDp, this.props.sliderInt, sliderTenure))
	}

	handleInterestSliderDragMove = (sliderInt) => {
		this.setState(getEMICalulatorData(this.props.sliderDp, sliderInt, this.props.sliderTenure))
	}

	render() {
		if (this.props.sliderDp.onRoadPrice === 0) {
			return null;
		}

		const {
			downpayment,
			tenure,
			emiCalculation,
			interestPayable,
			totalBikeCost
		} = this.state

		const {
			sliderDp,
			financeCitySelection,
			IsFetching
		} = this.props

		let pieLoanAmt = sliderDp.onRoadPrice - sliderDp.values[0];
		let finalIntPayable = formatToINR(parseInt(interestPayable));
		let finalTotalPrincipalAmt = formatToINR(totalBikeCost);

		let pieChartData = {
			intStatePay: interestPayable,
			pieLoanAmt: pieLoanAmt,
			pieTotalPay: totalBikeCost,
			interestPay: finalIntPayable,
			TotalPrincipalAmt: finalTotalPrincipalAmt
		}

		return (
			IsFetching ?
			<div className="emi-outer-container">
			  <div className="emi-spinner-container">
				<Spinner />
		      </div>
			  <div className="emi-piechart-spinner">
			  </div>
			</div>
			:
			<div className="emi-outer-container">
				<div className="emi-calci-container">
					<EMICalculatorHeader
						emiCalculation={emiCalculation}
						tenure={tenure}
						onRoadPrice={sliderDp.onRoadPrice}
						city={financeCitySelection}
					/>
					<div className="emi-slider-container">
						<DownPaymentSlider onSliderDragMove={this.handleDownPaymentSliderDragMove} />
						<div className="tenure-interest-slider">
							<div className="slider-input-container tenure-unit">
								<TenureSlider onSliderDragMove={this.handleTenureSliderDragMove} />
							</div>
							<div className="slider-input-container interest-unit">
								<InterestSlider onSliderDragMove={this.handleInterestSliderDragMove} />
							</div>
						</div>
					</div>
				</div>
				<div className="view-breakup-container">
					<PieBreakUp
						onRoadPrice={sliderDp.onRoadPrice}
						downpayment={downpayment}
						tenure={tenure}
						interestPayable={interestPayable}
						pieChartData={pieChartData}
					/>
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
	return {
		sliderDp,
		sliderTenure,
		sliderInt,
		financeCitySelection
	}
}

export default connect(mapStateToProps, null)(toJS(EMICalculator));
