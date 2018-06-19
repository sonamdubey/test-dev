import React from 'react';
import { connect } from 'react-redux'
import { bindActionCreators } from 'redux';
import { toJS } from '../../immutableWrapperContainer'

import EmiPieChart from './EmiPieChart'
import { formatToINR } from '../../utils/formatAmount'

import EMICalculatorHeader from './EMICalculatorHeader'

class PieBreakUp  extends React.Component {
	constructor(props) {
		super(props)
	}
	render() {
		const {
			onRoadPrice,
			downpayment,
			interestPayable,
			tenure,
			pieChartData
		} = this.props

		const loanAmount = parseFloat(onRoadPrice - downpayment)
		const pricipalLoan = formatToINR(loanAmount);
		const pieInterestAmt = formatToINR(interestPayable)

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
							<div>
									<div className="pie-breakup__title">
										<span className="pie-breakup__bullet-tenure"></span> Tenure
										<span className="pie-breakup_tenure">{parseInt(tenure)} Months</span>
									</div>
									
							</div>
					</div>
					<div className="pie-breakup-graph">
							<EmiPieChart pieChartData={pieChartData} />
					</div>
			</div>
		)
	}
}
module.exports = (PieBreakUp);