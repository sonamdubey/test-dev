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

import { formatToINR, formatToCurrency } from '../../utils/formatAmount'
import { triggerGA } from '../../utils/analyticsUtils'
class EMIDownPayment extends React.Component {
  constructor(props) {
    super(props);
    
    this.state = {
      values: this.props.slider.values
    }    
  }

  handleSliderChange = ({ values }) => {
    const {
      slider,
      onSliderDragMove,
      updateDownPaymentSlider,
    } = this.props
    updateDownPaymentSlider({ values, userChange: true })
    if(onSliderDragMove) {
      onSliderDragMove({
        ...slider,
        values
      })
    }
  }
  
  handleSliderDragMove = ({ values }) => {
    const {
      slider,
      onSliderDragMove
    } = this.props

    this.setState({
      values
    })

    if(onSliderDragMove) {
      onSliderDragMove({
        ...slider,
        values
      })
    }
  }
  
  handleOpen = () => {
    if (gaObj != undefined) {
      triggerGA(gaObj.name, 'ToolTip_Clicked', 'Down Payment'); 
    }
  }

  updateLoanText() {
    let loanAmountUpdated = this.props.slider.onRoadPrice - this.state.values[0];
    return loanAmountUpdated
  }

  componentWillReceiveProps(nextProps) {
    if (nextProps.slider.userChange) {
      if (gaObj != undefined) {
        triggerGA(gaObj.name, 'Interacted_With_EMI_Calculator', 'Down Payment Slider');
      }
    }
  }

  shouldComponentUpdate(nextProps, nextState) {
    return nextProps.slider.onRoadPrice != this.props.slider.onRoadPrice || nextProps.slider.values[0] != this.props.slider.values[0] ||
    nextState.values[0] != this.state.values[0]; 
  }

  render() {
    let {
      slider
    } = this.props
    slider = {
      ...slider,
      snap: true,
      snapOnDragMove: true,
      algorithm: sliderAlgorithm,
      className: 'rheostat-downpayment',
      pitComponent: PitComponent,
      pitPoints: [slider.min, slider.max],
      onChange: this.handleSliderChange.bind(this),
      onSliderDragMove: this.handleSliderDragMove,
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
            <Tooltip onOpen = {this.handleOpen} message="Banks expect a min. down payment of 10% of on-road price even if you have the best credit profile.">
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
    updateDownPaymentSlider: bindActionCreators(updateDownPaymentSlider, dispatch)
  }
}

module.exports = connect(mapStateToProps, mapDispatchToProps)(toJS(EMIDownPayment));