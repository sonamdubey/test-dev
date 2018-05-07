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
    const { state } = this.props;
    const PieInterestAmt = (this.state.interestPayable)
    const PieLoanAmt = (this.props.sliderDp.max - this.props.sliderDp.values[0]); 
    const PieTotalAmtPay = this.state.totalPrincipalAmt  
    let finalIntPayable = formatToINR(parseInt(this.state.InterestPayable))
    let finalTotalPrincipalAmt = formatToINR(this.state.totalPrincipalAmt)
    return (
      <div className="emi-outer-container">
        <div className="emi-calci-container">
           <EMICalculatorHeader />
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
            <PieBreakUp ref="piebreakup" isAnimation={this.props.pieAnimate} intStatePay={PieInterestAmt} pieLoanAmt={PieLoanAmt} pieTotalPay={PieTotalAmtPay} interestPay={finalIntPayable} TotalPrincipalAmt={finalTotalPrincipalAmt}/>
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
