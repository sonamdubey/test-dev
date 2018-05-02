import React from 'react';
import { connect } from 'react-redux'
import { bindActionCreators } from 'redux';
import { toJS } from '../../immutableWrapperContainer'

import Rheostat from '../Shared/Rheostat'
import PitComponent from '../Shared/RheostatPit'
import algorithm from '../../utils/rheostat/algorithms/fix'
import {algoObj} from '../../utils/rheostat/constants/tenureSnapPoints'
import {createNewSnapPoints} from '../../utils/rheostat/function/DiffSnapPoints'

import { emiCalculatorAction } from '../../actionCreators/emiTenureSlider'
import { updateTenureSlider } from '../../actionCreators/emiTenureSlider'

import { formatToRound } from '../../utils/formatAmount'

class EMITenure  extends React.Component {
  constructor(props) {
    super(props);
  }
  componentDidMount(){
    const {
			slider
		} = this.props
  }
  handleSliderChange = ({ values }) => {
		const {
			updateTenureSlider,
		} = this.props

		updateTenureSlider({ values, userChange: true })
	}
	handleSliderDragStart = () => {
		if((this.refs.tenureSlider.getElementsByClassName('rheostat-handle')) != document.activeElement){
			this.refs.tenureSlider.querySelectorAll('.rheostat-handle')[0].focus()
		}
	}
  render() {
    let {
      slider
    } = this.props
		let handleSnapPoints = createNewSnapPoints(algoObj);
    slider = {
			...slider,
			algorithm: {getPosition: algorithm.getPosition.bind(null, handleSnapPoints), getValue: algorithm.getValue.bind(null,handleSnapPoints)},
			className: 'slider-rheostat',
			pitComponent: PitComponent,
			pitPoints: [slider.min, slider.max],
			snap: true,
			snapPoints: handleSnapPoints,
			snapOnDragMove: true,
			disableSnapOnClick: false,
			handleTooltipLabel: formatToRound,
			onChange: this.handleSliderChange,
		}
    return (
        <div className="emi-calci-header slider-input-container">
					<span className="slider__unit-title">Tenure <span className="slider__unit-text">(Months)</span></span>
           <div className="slider-section" ref="tenuretSlider">
              <Rheostat
                  {...slider}
              />
					</div>
        </div>
    );
  }
}

const mapStateToProps = (state) => {
	const slider = state.getIn(['Finance', 'VehicleTenure', 'slider'])

	return {
		slider
	}
}

const mapDispatchToProps = (dispatch, getState) => {
	return {
		updateTenureSlider: bindActionCreators(updateTenureSlider, dispatch)
	}
}

module.exports = connect(mapStateToProps, mapDispatchToProps)(toJS(EMITenure));