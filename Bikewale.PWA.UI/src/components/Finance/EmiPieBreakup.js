import React from 'react';
import { connect } from 'react-redux'
import { bindActionCreators } from 'redux';
import { toJS } from '../../immutableWrapperContainer'

import PieChart from './EmiPieChart'
import { formatToINR } from '../../utils/formatAmount'

import EMICalculatorHeader from './EMICalculatorHeader'

class PieBreakUp  extends React.Component {
	constructor(props) {
		super(props)
	}
	render() {
		const loanAmount = parseFloat(this.props.sliderData.sliderDpData.onRoadPrice - this.props.sliderData.sliderDpData.values[0])
		let pricipalLoan = formatToINR(loanAmount);
		const pieInterestAmt = formatToINR(this.props.emiCalculationParam.interestPayable)
		return (
				<div>          
					<div className="pie-breakup-data">
							<div className="pie-breakup_unit">
									<div className="pie-breakup__title">
										<span className="pie-breakup_bullet pie-breakup_bullet-interest"></span> Total Interest Payable</div>
									<div className="pie-breakup__amount">{pieInterestAmt}</div>
							</div>
							<div className="pie-breakup_unit">
									<div className="pie-breakup__title">
										<span className="pie-breakup_bullet pie-breakup_bullet-loan"></span> Principle Loan Amount</div>
									<div className="pie-breakup__amount">{pricipalLoan}</div>
							</div>
							<div className="pie-breakup_unit">
									<div className="pie-breakup__title">
										<span className="pie-breakup__bullet-tenure"></span> Tenure
										<span className="pie-breakup_tenure">{parseInt(this.props.sliderData.sliderTenureData.values[0])} Months</span>
									</div>	
									
							</div>
					</div>
					<div className="pie-breakup-graph">
							<PieChart ref="pieChart" isAnimation={this.props.isAnimation} pieChartData={this.props.pieChartData} />
					</div>
			</div>
		)
	}
}
module.exports = (PieBreakUp);