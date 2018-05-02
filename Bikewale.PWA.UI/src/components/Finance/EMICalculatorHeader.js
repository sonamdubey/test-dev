import React from 'react';
import { connect } from 'react-redux'
import { bindActionCreators } from 'redux';
import { toJS } from '../../immutableWrapperContainer'
import TenureSlider from './TenureSlider'
import {
    EmiCalculation,
    InterestPayable,
    TotalPrincipalAmt
 } from './EmiCalculations'
 import {
    monthsToYears
 } from '../../utils/formatMonthsYears'

import { formatToINR } from '../../utils/formatAmount'

class EMICalculatorHeader extends React.Component {
  constructor(props) {
    super(props);
    this.state = {EmiCalculation: EmiCalculation(this.props.sliderDp, this.props.sliderTenure, this.props.sliderInt),
                  InterestPayable: InterestPayable(this.props.sliderDp, this.props.sliderTenure, this.props.sliderInt),
                  TotalPrincipalAmt: TotalPrincipalAmt(this.props.sliderDp, this.props.sliderTenure, this.props.sliderInt),
                  monthsToYears: monthsToYears(this.props.sliderTenure.values)
                 }
    }
  componentWillReceiveProps(nextProps){
    this.setState({ EmiCalculation : EmiCalculation(nextProps.sliderDp, nextProps.sliderTenure, nextProps.sliderInt),
                    InterestPayable : InterestPayable(nextProps.sliderDp, nextProps.sliderTenure, nextProps.sliderInt),
                    TotalPrincipalAmt : TotalPrincipalAmt(nextProps.sliderDp, nextProps.sliderTenure, nextProps.sliderInt),
                    monthsToYears: monthsToYears(nextProps.sliderTenure.values)
                  })
   }
   
  render() {
        let finalEmi = formatToINR(this.state.EmiCalculation)
        let tenureText = (this.state.monthsToYears)
        let orpAmount = formatToINR(this.props.sliderDp.max)
    return (
        <div className="emi-calci-header">
           <div className="emi-calci-top-header">
             <div className="emi-calci__orp-text">On-road price, Navi Mumbai</div>
             <div className="emi-calci__orp-data">
               <span>{orpAmount}</span>
               <a href="" className="price-link">View detailed price</a>
             </div>
           </div>
          <div className="emi-cost-container">
            <div className="emi-text">EMI</div>
            <div className="emi-total-cost">
              <span className="final-emi-text">{finalEmi}</span>
              <span className="emi-total-year">per month for <span className="emi-total-year-text">{tenureText}</span></span>
            </div>
          </div>
        </div>
    );
  }
}

const mapStateToProps = (state) => {
  const sliderDp = state.getIn(['Finance', 'VehicleDownPayment', 'slider'])
  const sliderTenure = state.getIn(['Finance', 'VehicleTenure', 'slider'])
  const sliderInt = state.getIn(['Finance', 'VehicleInterest', 'slider'])

	return {
		sliderDp,
    sliderTenure,
    sliderInt
	}
}
module.exports = connect(mapStateToProps, null)(toJS(EMICalculatorHeader));
