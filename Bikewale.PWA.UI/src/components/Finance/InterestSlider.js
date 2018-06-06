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
import { triggerGA } from '../../utils/analyticsUtils'

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

	handleSliderDragMove = ({ values }) => {
		const {
			slider,
			onSliderDragMove
		} = this.props

		if(onSliderDragMove) {
			onSliderDragMove({
				...slider,
				values
			})
		}
	}
	
	componentWillReceiveProps(nextProps){
		if(nextProps.slider.userChange){
		  if (gaObj != undefined) {
			triggerGA(gaObj.name, 'Interacted_With_EMI_Calculator', 'Interest Slider'); 
		  }
		}
	}

	handleOpen = () => {
		if (gaObj != undefined) {
			triggerGA(gaObj.name, 'ToolTip_Clicked', 'Interest'); 
		}
	}

	shouldComponentUpdate(nextProps, nextState) {
		return nextProps.slider.values != this.props.slider.values;
	}

	
	render() {
		let {
			slider
		} = this.props

		slider = {
			...slider,
			algorithm: {
			  getPosition: algorithm.getPosition.bind(null, slider.snapPoints),
			  getValue: algorithm.getValue.bind(null, slider.snapPoints)
			},
			className: 'slider-rheostat',
			pitComponent: PitComponent,
			pitPoints: [slider.min, slider.max],
			onChange: this.handleSliderChange,
			onSliderDragMove: this.handleSliderDragMove
		}
		return (
				<div className="emi-calci-header slider-input-container">
					<span className="slider__unit-title">Interest <span className="slider__unit-text">(%)</span></span>
                    <Tooltip onOpen = {this.handleOpen} placement="top-right" message="It is a flat interest rate - the interest rate is calculated on the full loan amount throughout the tenure without considering that monthly EMIs gradually reduce the principal amount">
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
		updateInterestSlider: bindActionCreators(updateInterestSlider, dispatch)
	}
}

module.exports = connect(mapStateToProps, mapDispatchToProps)(toJS(EMIInterest));