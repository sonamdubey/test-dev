import React from 'react'
import PropTypes from 'prop-types'
import { connect } from 'react-redux'
import { bindActionCreators } from 'redux'
import Rheostat from '../../components/Rheostat'
import PitComponent from '../../components/RheostatPit'
import algorithm from '../../utils/rheostat/algorithms/fix'
import { algoObj } from '../../utils/rheostat/constants/tenureSnapPoints'
import { createNewSnapPoints } from '../../utils/rheostat/function/DiffSnapPoints'

import {
	updateTenureSlider,
	updateTenureInput
} from '../actionCreators/emiTenureSlider'

import {
	handleCursorPosition
} from '../../utils/Common'

import {
	trackingCategory
} from '../constants';

import {
	getModelData
} from '../utils/Prices';

import {
	isDesktop
} from '../../utils/Common';

import {
	fireTracking
} from '../actionCreators/emiData';

import {
	emiComponents
} from '../enum/emiComponents';

class emiTenure extends React.Component {
	constructor(props) {
		super(props)
		this.state = {
			isTenureValid: true,
			TenureErrorText: '',
			oldTenureAmount: 0,
			cursorPosition: 0,
			isFetching: true,
		}

		this.cursorTimer = null
	}

	componentDidMount() {
		const {
			updateTenureSlider,
			slider
		} = this.props
	}

	componentWillReceiveProps(nextprops) {
		const { inputBox } = nextprops
		this.validateTenure(inputBox.value)
	}

	componentDidUpdate() {
		if (!this.state.isFetching) {
			const {
				inputBox
			} = this.props

			const {
				oldTenureAmount,
				cursorPosition
			} = this.state

			let newPosition = handleCursorPosition(inputBox.value, oldTenureAmount, cursorPosition)

			if (this.TenureInputField === document.activeElement) {
				this.TenureInputField.classList.add('disable--cursor')

				clearTimeout(this.cursorTimer)
				this.cursorTimer = setTimeout(() => {
					this.TenureInputField.selectionStart = this.TenureInputField.selectionEnd = newPosition
					this.TenureInputField.classList.remove('disable--cursor')
				}, 0)
			}
		}
	}

	handleSliderChange = ({ values }) => {
		const {
			updateTenureSlider,
			inputBox
		} = this.props
		updateTenureSlider({ values, userChange: true })
		this.props.changeTenureValue(parseFloat(values[0]));
	}

	handleSliderDragStart = () => {
		this.TenureInputField.classList.add('disable--cursor')
		if ((this.refs.tenureSlider.getElementsByClassName('rheostat-handle')) != document.activeElement) {
			this.refs.tenureSlider.querySelectorAll('.rheostat-handle')[0].focus()
		}
	}

	handleSliderDragEnd = () => {
		this.TenureInputField.classList.remove('disable--cursor')
		if (this.TenureInputField === document.activeElement) {
			this.TenureInputField.selectionStart = this.TenureInputField.selectionEnd = this.TenureInputField.value.length
		}

		/**
		 * For Desktop, after `dragEnd` event invocation `click` event is fired.
		 * To avoid duplicate tracking, disable tracking from desktop.
		 */
		if (!isDesktop) {
			this.props.fireTracking({ gaTrackingAction: "Tenure_Slider_Clicked", brighuTrackingComponent: emiComponents.TenureSlider });
		}
	}

	handleSliderClick = () => {
		this.props.fireTracking({ gaTrackingAction: "Tenure_Slider_Clicked", brighuTrackingComponent: emiComponents.TenureSlider });
	}

	handleInputChange = (event) => {
		const {
			updateTenureInput,
			inputBox
		} = this.props

		let inputValue = (event.currentTarget.value)
		if (!isNaN(inputValue)) {
			updateTenureInput(inputValue, true)
		}
		this.setState({
			oldTenureAmount: inputBox.value,
			cursorPosition: this.TenureInputField.selectionEnd
		})
	}

	validateTenure = (updatedValue) => {
		const {
			slider
		} = this.props
		let re = /^[0-9]+(\.[0,5]{1})?$/
		let newupdatedValue = re.exec(updatedValue);
		if (updatedValue < slider.min) {
			this.setState({
				isTenureValid: false,
				TenureErrorText: 'Tenure should be between 1 to 7 years'
			})
		}
		else if (updatedValue > slider.max) {
			this.setState({
				isTenureValid: false,
				TenureErrorText: 'Tenure should be between 1 to 7 years'
			})
		}
		// else if((updatedValue * 10.0) % 10 != 5){
		// 	this.setState({
		// 		isTenureValid: false,
		// 		TenureErrorText: 'Invalid ghh amount'
		// 	})
		// }
		else if (!newupdatedValue) {
			this.setState({
				isTenureValid: false,
				TenureErrorText: 'You can only select full or half year'
			})
		}
		else {
			this.setState({
				isTenureValid: true,
				TenureErrorText: ''
			})
		}
	}

	setReference = (ref) => {
		this.TenureInputField = ref
	}
	updateTheInputValue = () => {
		this.setState({ ref: inputValue });
	}
	render() {
		let {
			slider,
			inputBox
		} = this.props
		let handleSnapPoints = createNewSnapPoints(algoObj);
		slider = {
			...slider,
			algorithm: { getPosition: algorithm.getPosition.bind(null, handleSnapPoints), getValue: algorithm.getValue.bind(null, handleSnapPoints) },
			className: 'slider-rheostat',
			pitComponent: PitComponent,
			pitPoints: [slider.min, slider.max],
			snap: true,
			snapPoints: handleSnapPoints,
			snapOnDragMove: true,
			disableSnapOnClick: false,
			onChange: this.handleSliderChange,
			onSliderDragStart: this.handleSliderDragStart,
			onSliderDragEnd: this.handleSliderDragEnd,
			onClick: this.handleSliderClick
		}
		let inputValue = inputBox.value == '0' ? '' : (inputBox.value)
		let inputErrorClass = this.state.isTenureValid ? '' : 'status-invalid'
		return (
			<div className="slider-input-container tenure-unit">
				<span className="slider-unit-title">Tenure <span className="unit-text">(Years)</span></span>
				<div className="slider-section" ref="tenureSlider">
					<Rheostat
						{...slider}
					/>
				</div>
				<div className={"input-box " + inputErrorClass}>
					<input
						ref={this.setReference}
						type="tel"
						className="slider-input"
						value={inputValue}
						maxLength="3"
						onChange={this.handleInputChange}
					/>
					<span className="error-text">
						{this.state.TenureErrorText}
					</span>
				</div>
			</div>
		)
	}
}

const mapStateToProps = (state) => {
	let activeModel = getModelData(state.newEmiPrices)

	const {
		slider,
		inputBox
	} = activeModel.data.vehicleTenure
	return {
		slider,
		inputBox
	}
}

const mapDispatchToProps = (dispatch, getState) => {
	return {
		updateTenureSlider: bindActionCreators(updateTenureSlider, dispatch),
		updateTenureInput: bindActionCreators(updateTenureInput, dispatch),
		fireTracking: bindActionCreators(fireTracking, dispatch)
	}
}


export default connect(mapStateToProps, mapDispatchToProps)(emiTenure)
