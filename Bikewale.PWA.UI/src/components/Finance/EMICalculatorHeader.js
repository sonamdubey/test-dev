import React from 'react';
import { connect } from 'react-redux'
import { bindActionCreators } from 'redux';
import { toJS } from '../../immutableWrapperContainer'
 import {
    monthsToYears
 } from '../../utils/formatMonthsYears'

import { formatToINR } from '../../utils/formatAmount'

class EMICalculatorHeader extends React.Component {
  constructor(props) {
    super(props);
    this.state = {
      monthsToYears: monthsToYears(this.props.tenure)
    }
  }

  componentWillReceiveProps(nextProps){
    this.setState({
      monthsToYears: monthsToYears(nextProps.tenure)
    })
  }
 
  render() {
    const {
      emiCalculation,
      onRoadPrice,
      city
    } = this.props

    const {
      monthsToYears
    } = this.state

    let finalEmi = formatToINR(emiCalculation)
    let orpAmount = formatToINR(onRoadPrice)
  
    return (
      <div className="emi-calci-header">
        <div className="emi-calci-top-header">
          <div className="emi-calci__orp-text">On-road price, {city.cityName}</div>
          <div className="emi-calci__orp-data">
            <span>{orpAmount}</span>
          </div>
        </div>
        <div className="emi-cost-container">
          <div className="emi-text">EMI</div>
          <div className="emi-total-cost">
            <span className="final-emi-text">{finalEmi}</span>
            <span className="emi-total-year">per month for <span className="emi-total-year-text">{monthsToYears}</span></span>
          </div>
        </div>
      </div>
    );
  }
}

export default (EMICalculatorHeader);
