import React from 'react';
import { connect } from 'react-redux'
import { bindActionCreators } from 'redux';
import { toJS } from '../../immutableWrapperContainer'

import { formatToINR } from '../../utils/formatAmount'
import PieChart from 'react-minimal-pie-chart'

export default class EmiPieChart extends React.Component {
	constructor(props){
		super(props)
		this.state = {
			intStatePay: this.props.pieChartData.intStatePay,
			pieLoanAmt: this.props.pieChartData.pieLoanAmt,
			pieDownPaymentAmt: this.props.pieChartData.pieDownPaymentAmt
		}
	}
	componentWillReceiveProps(nextProps){
		this.setState({
			intStatePay: nextProps.pieChartData.intStatePay,
			pieLoanAmt: nextProps.pieChartData.pieLoanAmt,
			pieDownPaymentAmt: nextProps.pieChartData.pieDownPaymentAmt
		})
	}
	render(){
		const pieData = [
			{
				value: this.state.pieDownPaymentAmt,
				color: '#fed1cd'
			},
			{
				value: this.state.pieLoanAmt,
				color: '#c3c3c3'
			},
			{
				value: this.state.intStatePay,
				color: '#f45944'
			}
		]

		let finalAmountPay = formatToINR(this.props.pieChartData.pieTotalPay)
		return(
			<div className="pie-graph-container">
			<div className="pie-graph__total-payment">
				<div className="pie-breakup__title">Total Payment</div>
				<div className="pie-center-amount">{(finalAmountPay)}</div>
			</div>
			<PieChart
				data={pieData}
				radius={48}
				lineWidth={10}
				startAngle={-90}
				rounded
			/>
			</div>
		)
	}
}
