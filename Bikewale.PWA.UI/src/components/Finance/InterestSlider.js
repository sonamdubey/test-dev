import React from 'react';
import { connect } from 'react-redux'
import { bindActionCreators } from 'redux';
import { toJS } from '../../immutableWrapperContainer'

import Rheostat from '../Shared/Rheostat'
import PitComponent from '../Shared/RheostatPit'
import algorithm from '../../utils/rheostat/algorithms/fix'
import {algoObj} from '../../utils/rheostat/constants/interestSnapPoints'
import {createNewSnapPoints} from '../../utils/rheostat/function/DiffSnapPoints'

import { emiCalculatorAction } from '../../actionCreators/emiInterestSlider'
import { updateInterestSlider } from '../../actionCreators/emiInterestSlider'

import { formatToRound } from '../../utils/formatAmount'

class EMIInterest  extends React.Component {
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
			updateInterestSlider,
		} = this.props

		updateInterestSlider({ values, userChange: true })
	}
	handleSliderDragStart = () => {
		if((this.refs.interestSlider.getElementsByClassName('rheostat-handle')) != document.activeElement){
			this.refs.interestSlider.querySelectorAll('.rheostat-handle')[0].focus()
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
			onChange: this.handleSliderChange,
		}
    return (
        <div className="emi-calci-header slider-input-container">
					<span className="slider__unit-title">Interest <span className="slider__unit-text">(%)</span></span>
           <div className="slider-section" ref="interesttSlider">
              <Rheostat
                  {...slider}
              />
					</div>
        </div>
    );
  }
}

const mapStateToProps = (state) => {
	const slider = state.getIn(['Finance', 'VehicleInterest', 'slider'])

	return {
		slider
	}
}

const mapDispatchToProps = (dispatch, getState) => {
	return {
		updateInterestSlider: bindActionCreators(updateInterestSlider, dispatch)
	}
}

module.exports = connect(mapStateToProps, mapDispatchToProps)(toJS(EMIInterest));