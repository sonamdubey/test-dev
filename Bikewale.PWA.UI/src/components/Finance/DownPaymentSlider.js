import React from 'react';
import { connect } from 'react-redux'
import { bindActionCreators } from 'redux';
import { toJS } from '../../immutableWrapperContainer'

import Rheostat from '../Shared/Rheostat'
import PitComponent from '../Shared/RheostatPit'
import Tooltip from '../Shared/Tooltip'
import sliderAlgorithm from '../../utils/rheostat/algorithms/linear'

import { emiCalculatorAction } from '../../actionCreators/emiDownPaymentSlider'
import { updateDownPaymentSlider } from '../../actionCreators/emiDownPaymentSlider'
import { startAnimation } from '../../actionCreators/pieAnimation'

import { formatToINR, formatToCurrency } from '../../utils/formatAmount'

class EMIDownPayment extends React.Component {
  constructor(props) {
    super(props);
    
  }

  handleSliderChange = ({ values }) => {
    const {
      updateDownPaymentSlider,
    } = this.props

    updateDownPaymentSlider({ values, userChange: true })
  }

  handlePieChartAnimation = () => {
    const {
      startAnimation
    } = this.props
    
    startAnimation()
  }

  updateLoanText() {
    let loanAmountUpdated = this.props.slider.onRoadPrice - this.props.slider.values[0];
    return loanAmountUpdated
  }

  render() {
    let {
      slider
    } = this.props
    slider = {
      ...slider,
      algorithm: sliderAlgorithm,
      className: 'rheostat-downpayment',
      pitComponent: PitComponent,
      pitPoints: [slider.min, slider.max],
      onChange: this.handleSliderChange.bind(this),
      onClick: this.handlePieChartAnimation.bind(this),
      onSliderDragEnd: this.handlePieChartAnimation.bind(this),
      handleTooltipLabel: formatToINR
    }
    let vehicleLoanAmount = formatToINR(this.updateLoanText());
    return (
      <div className="slider-input-container downpayment-unit">
        <span className="slider__unit-title">Down Payment</span>
        <div className="slider-section" ref="downPaymentSlider">
          <Rheostat
            {...slider}
          />
          <div className="slider__label">
            <span className="slider-label__left">Min. Down Payment </span>
            <Tooltip message="Banks expect a min. down payment of 10% of on-road price even if you have the best credit profile.">
              <span className="slider__info-icon"></span>
            </Tooltip>
          </div>
          <div className="vehicle-loan-text">
            Your loan amount will be <span className="vehicle-loan__amount">{vehicleLoanAmount}</span>
          </div>
        </div>
      </div>
    );
  }
}

const mapStateToProps = (state) => {
  const slider = state.getIn(['Emi', 'VehicleDownPayment', 'slider'])

  return {
    slider
  }
}

const mapDispatchToProps = (dispatch, getState) => {
  return {
    updateDownPaymentSlider: bindActionCreators(updateDownPaymentSlider, dispatch),
    startAnimation: bindActionCreators(startAnimation, dispatch)
  }
}

module.exports = connect(mapStateToProps, mapDispatchToProps)(toJS(EMIDownPayment));