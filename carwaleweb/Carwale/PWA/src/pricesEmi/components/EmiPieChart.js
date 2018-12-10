import React from 'react'
import Pie from './Pie'

import {
	isDesktop
} from '../../utils/Common'

import {
	formatToINR
} from '../../utils/amountFormat'
export default class EmiPieChart extends React.Component {
    constructor(props){
        super(props)
        this.state = {
			data: [this.props.intStatePay, this.props.pieLoanAmt]
        }
    }
    componentDidMount(){
        this.setState({
            data: [this.props.intStatePay, this.props.pieLoanAmt]
        })
    }
    componentWillReceiveProps(){
        this.setState({
            data: [this.props.intStatePay, this.props.pieLoanAmt]
        })
    }
    render(){
		let colors = ['#ef9423', '#00afa0'];
		let finalAmountPay = formatToINR(this.props.pieTotalPay)
		let data =[
			{
				value: this.props.intStatePay,
				color: colors[0]
			},
			{
				value: this.props.pieLoanAmt,
				color: colors[1]
			}
		]
        let pieReveal = isDesktop ? 100 : (this.props.pieReveal || 0)
        return(
            <div className="pie-graph-container">
            <div className="totalpayment">
             <div className="pie-bullet-unit">
                    {colors.map((colorCode) => {
                        return (
                            <span key={colorCode} className="pieBullet" style={{ backgroundColor: colorCode}}></span>
                        )
                    })}
                 </div>
                <div>Total Payment</div>
                <div className="pie-center-amount">{finalAmountPay}</div>
            </div>
			<Pie data={data} paddingAngle={2} animate={isDesktop ? false : true} reveal={pieReveal}/>
            </div>
        )
    }
}
