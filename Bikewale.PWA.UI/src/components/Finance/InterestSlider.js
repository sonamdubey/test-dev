import React from 'react';
import { connect } from 'react-redux'
import { bindActionCreators } from 'redux';
import { toJS } from '../../immutableWrapperContainer'

import Rheostat from '../Shared/Rheostat'
import PitComponent from '../Shared/RheostatPit'
import Tooltip from '../Shared/Tooltip'
import algorithm from '../../utils/rheostat/algorithms/fixPoints'
import {createNewSnapPoints} from '../../utils/rheostat/function/DiffSnapPoints'

import { emiCalculatorAction } from '../../actionCreators/emiInterestSlider'
import { updateInterestSlider } from '../../actionCreators/emiInterestSlider'
import { startAnimation } from '../../actionCreators/pieAnimation'

class EMIInterest  extends React.Component {
	constructor(props) {
		super(props);
	}

	handleSliderChange = ({ values }) => {
		const {
			updateInterestSlider,
		} = this.props

		updateInterestSlider({ values, userChange: true })
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
		let handleSnapPoints = createNewSnapPoints({
			startPoint: slider.min,
			endPoint: slider.max,
			difference: 0.1
		});
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
			onClick: this.handlePieChartAnimation,
			onSliderDragEnd: this.handlePieChartAnimation
		}
		return (
				<div className="emi-calci-header slider-input-container">
					<span className="slider__unit-title">Interest <span className="slider__unit-text">(%)</span></span>
                    <Tooltip placement="top-right" message="It is a flat interest rate - the interest rate is calculated on the full loan amount throughout the tenure without considering that monthly EMIs gradually reduce the principal amount">
                        <span className="slider__info-icon"></span>
                    </Tooltip>
					 <div className="slider-section" ref="interestSlider">
							<Rheostat
									{...slider}
							/>
					</div>
				</div>
		);
	}
}

const mapStateToProps = (state) => {
	const slider = state.getIn(['Emi', 'VehicleInterest', 'slider'])

	return {
		slider
	}
}

const mapDispatchToProps = (dispatch, getState) => {
	return {
		updateInterestSlider: bindActionCreators(updateInterestSlider, dispatch),
		startAnimation: bindActionCreators(startAnimation, dispatch)
	}
}

module.exports = connect(mapStateToProps, mapDispatchToProps)(toJS(EMIInterest));