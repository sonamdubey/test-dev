import React from 'react'
import { connect } from 'react-redux'
import { bindActionCreators } from 'redux'
import EmiPieChart from './EmiPieChart'
import {
	formatToINR
} from './../../utils/amountFormat'
import {
	updatePieValueData
} from '../actionCreators/emiPieData'

import {
    getModelData
} from '../utils/Prices'

class PieBreakUp extends React.Component {
    constructor(props) {
        super(props)
        const onroadPrice = (this.props.sliderDP.max)
    }
    componentDidMount() {
        const {
                updatePieValueData
            } = this.props
    }
    render() {
         let pricipalLoan = formatToINR(this.props.pieLoanAmt);
        return (
            <div className="">
                <div className="pie-breakup-graph">
                    <EmiPieChart ref="pieChart" isAnimation={this.props.isAnimation} intStatePay={this.props.intStatePay} pieLoanAmt={this.props.pieLoanAmt} pieTotalPay={this.props.pieTotalPay} pieReveal={this.props.pieReveal}/>
                </div>
                <div className="pie-breakup-data">
                    <div className="breakup-unit total-interest">
                        <div className="title-text">
                            <span className="bullet"></span> Total Interest Payable</div>
                        <div className="breakup-amount">{this.props.interestPay}</div>
                    </div>
                    <div className="breakup-unit pricipal-loan">
                        <div className="title-text"><span className="bullet"></span> Principal Loan Amount</div>
                        <div className="breakup-amount">{pricipalLoan}</div>
                    </div>
                </div>
            </div>
        )
    }
}

const mapStateToProps = (state) => {
    let activeModel = getModelData(state.newEmiPrices)

	const {
        slider: sliderDP
	} = activeModel.data.vehicleDownPayment
	return {
        sliderDP
	}
}

const mapDispatchToProps = (dispatch, getState) => {
	return {
		updatePieValueData: bindActionCreators(updatePieValueData, dispatch)
	}
}

export default connect(mapStateToProps, mapDispatchToProps)(PieBreakUp)

