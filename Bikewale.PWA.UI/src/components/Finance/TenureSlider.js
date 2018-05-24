import React from 'react';
import { connect } from 'react-redux'
import { bindActionCreators } from 'redux';
import { toJS } from '../../immutableWrapperContainer'

import Rheostat from '../Shared/Rheostat'
import PitComponent from '../Shared/RheostatPit'
import algorithm from '../../utils/rheostat/algorithms/fixPoints'

import { emiCalculatorAction } from '../../actionCreators/emiTenureSlider'
import { updateTenureSlider } from '../../actionCreators/emiTenureSlider'
import { startAnimation } from '../../actionCreators/pieAnimation'

import { formatToRound } from '../../utils/formatAmount'

class EMITenure  extends React.Component {
  constructor(props) {
    super(props);
  }

  handleSliderChange = ({ values }) => {
    const {
      updateTenureSlider,
    } = this.props

    updateTenureSlider({ values, userChange: true })
  }

  handlePieChartAnimation = () => {
    const {
      startAnimation
    } = this.props

    startAnimation()
  }

  render() {
    let {
      slider
    } = this.props
    
    slider = {
      ...slider,
      className: 'slider-rheostat',
      pitComponent: PitComponent,
      pitPoints: [slider.min, slider.max],
      handleTooltipLabel: formatToRound,
      onChange: this.handleSliderChange,
      onClick: this.handlePieChartAnimation,
      onSliderDragEnd: this.handlePieChartAnimation
    }
    return (
        <div className="emi-calci-header slider-input-container">
          <span className="slider__unit-title">Tenure <span className="slider__unit-text">(Months)</span></span>
           <div className="slider-section" ref="tenureSlider">
              <Rheostat
                  {...slider}
              />
          </div>
        </div>
    );
  }
}

const mapStateToProps = (state) => {
  const slider = state.getIn(['Emi', 'VehicleTenure', 'slider'])

  return {
    slider
  }
}

const mapDispatchToProps = (dispatch, getState) => {
  return {
    updateTenureSlider: bindActionCreators(updateTenureSlider, dispatch),
    startAnimation: bindActionCreators(startAnimation, dispatch)
  }
}

module.exports = connect(mapStateToProps, mapDispatchToProps)(toJS(EMITenure));