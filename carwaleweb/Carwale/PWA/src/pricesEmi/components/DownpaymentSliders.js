import React from 'react'
import PropTypes from 'prop-types'
import { connect } from 'react-redux'
import { bindActionCreators } from 'redux'
import Rheostat from '../../components/Rheostat'
import PitComponent from '../../components/RheostatPit'
import sliderAlgorithm from '../../utils/rheostat/algorithms/linear'
import CreateToastmsg from './ToastMessage'

import {
	initToast,
	clearToast
} from '../../actionCreators/Toast'

import {
	updateDownPaymentSlider,
	updateDownPaymentInput
} from '../actionCreators/emiDownPaymentSlider'

import {
	formatValueWithComma,
	handleCommaDelete,
	handleCursorPosition
} from '../../utils/Common'

import {
	formatToCurrency,
	formatToINR
} from './../../utils/amountFormat'

import {
	getModelData
} from '../utils/Prices'

import {
	isDesktop
} from '../../utils/Common';

import {
	fireTracking
} from '../actionCreators/emiData';

import {
	emiComponents
} from '../enum/emiComponents';

class emiDownPayment extends React.Component {
	constructor(props) {
		super(props)
		this.state = {
			isDownpaymentValid: true,
			DownpaymentErrorText: '',
			oldDownpaymentAmount: 0,
			cursorPosition: 0,
			isFetching: true,
			toastVisible: false
		}

		this.cursorTimer = null,
			this.setToastRef = this.setToastRef.bind(this)
		this.handleClickOutside = this.handleClickOutside.bind(this)
	}

	componentDidMount() {
		const {
			updateDownPaymentSlider,
			slider
		} = this.props
	}

	componentWillReceiveProps(currentEmiObj) {
		const { inputBox } = currentEmiObj
		this.validateDownpayment(inputBox.value, currentEmiObj)
	}
	componentWillUnmount() {
		this.props.clearToast()
	}
	componentDidUpdate() {
		if (!this.state.isFetching) {
			const {
				inputBox
			} = this.props

			const {
				oldDownpaymentAmount,
				cursorPosition
			} = this.state

			let newPosition = handleCursorPosition(inputBox.value, oldDownpaymentAmount, cursorPosition)

			if (this.DownPaymentInputField === document.activeElement) {
				this.DownPaymentInputField.classList.add('disable--cursor')

				clearTimeout(this.cursorTimer)
				this.cursorTimer = setTimeout(() => {
					this.DownPaymentInputField.selectionStart = this.DownPaymentInputField.selectionEnd = newPosition
					this.DownPaymentInputField.classList.remove('disable--cursor')
				}, 0)
			}
		}
	}
	setToastRef(node) {
		this.toastRef = node;
	}
	handleClickOutside(event) {
		if (!this.state.toastVisible) {
			document.removeEventListener('click', this.handleClickOutside);
		}
		if (this.toastRef && !this.toastRef.contains(event.target)) {
			this.props.clearToast()
		}
		this.setState(prevState => ({
			toastVisible: prevState.toastVisible,
		}));
	}
	handleSliderChange = ({ values }) => {
		const {
			updateDownPaymentSlider,
			inputBox
		} = this.props
		updateDownPaymentSlider({ values, userChange: true })
	}

	handleSliderDragStart = () => {
		this.DownPaymentInputField.classList.add('disable--cursor')
		if ((this.refs.downPaymentSlider.getElementsByClassName('rheostat-handle')) != document.activeElement) {
			this.refs.downPaymentSlider.querySelectorAll('.rheostat-handle')[0].focus()
		}
	}

	handleSliderDragEnd = () => {
		this.DownPaymentInputField.classList.remove('disable--cursor')
		if (this.DownPaymentInputField === document.activeElement) {
			this.DownPaymentInputField.selectionStart = this.DownPaymentInputField.selectionEnd = this.DownPaymentInputField.value.length
		}

		/**
		 * For Desktop, after `dragEnd` event invocation `click` event is fired.
		 * To avoid duplicate tracking, disable tracking from desktop.
		 */
		if (!isDesktop) {
			this.props.fireTracking({ gaTrackingAction: "DownPayment_Slider_Clicked", brighuTrackingComponent: emiComponents.DownpaymentSlider });
		}
	}

	handleSliderClick = () => {
		this.props.fireTracking({ gaTrackingAction: "DownPayment_Slider_Clicked", brighuTrackingComponent: emiComponents.DownpaymentSlider });
	}
	handleInputChange = (event) => {
		const {
			updateDownPaymentInput,
			inputBox
		} = this.props

		let inputValue = formatValueWithComma(event.currentTarget)

		if (!isNaN(inputValue)) {
			updateDownPaymentInput(inputValue, true)
		}
		this.setState({
			oldDownpaymentAmount: inputBox.value,
			cursorPosition: this.DownPaymentInputField.selectionEnd
		})
	}

	handleKeyDown = (event) => {
		handleCommaDelete(event)
	}

	formatTooltipLabel = (value) => {
		return formatToCurrency(value, {
			thousandDecimal: 0,
			lakhDecimal: 1,
			plusSign: false
		})
	}

	validateDownpayment = (updatedValue, currentEmiObj) => {
		const {
			slider
		} = this.props

		if (updatedValue < currentEmiObj.slider.min) {
			this.setState({
				isDownpaymentValid: false,
				DownpaymentErrorText: 'Please enter valid down payment amount'
			})
		}
		else if (updatedValue > currentEmiObj.slider.max) {
			this.setState({
				isDownpaymentValid: false,
				DownpaymentErrorText: 'Please enter valid down payment amount'
			})
		}
		else {
			this.setState({
				isDownpaymentValid: true,
				DownpaymentErrorText: ''
			})
		}
	}

	setReference = (ref) => {
		this.DownPaymentInputField = ref
	}

	updateLoanText() {
		let loanAmountUpdated = this.props.slider.max - this.props.slider.values[0];
		return loanAmountUpdated
	}
	handleToastClick(event) {
		this.setToast(event)
		if (!this.state.toastVisible) {
			document.addEventListener('click', this.handleClickOutside);
		}
	}
	setToast = (event) => {
		this.props.initToast({
			message: 'This is the minimum upfront payment that you need to make.',
			event
		})
	}
	render() {
		let {
			slider,
			inputBox
		} = this.props

		slider = {
			...slider,
			algorithm: sliderAlgorithm,
			className: 'slider-rheostat',
			pitComponent: PitComponent,
			pitPoints: [slider.min, slider.max],
			pitPointLabel: formatToINR,
			snap: true,
			snapOnDragMove: true,
			onChange: this.handleSliderChange,
			onSliderDragStart: this.handleSliderDragStart,
			onSliderDragEnd: this.handleSliderDragEnd,
			handleTooltipLabel: this.formatTooltipLabel,
			onClick: this.handleSliderClick
		}
		let inputValue = inputBox.value == '0' ? '' : formatToINR(inputBox.value, false)
		let inputErrorClass = this.state.isDownpaymentValid ? '' : 'status-invalid'
		let vehicleLoanAmount = formatToINR(this.updateLoanText());
		return (
			<div className="slider-input-container downpayment-unit">
				<span className="slider-unit-title">Down Payment</span>
				<div className="slider-section" ref="downPaymentSlider">
					<Rheostat
						{...slider}
					/>
					<div className="sliderLabel">
						<span className="sliderLabelLeft">Min. Down Payment <span ref={this.setToastRef} onClick={this.handleToastClick.bind(this)} className="info-icon"></span></span>
						<span className="sliderLabelRight">{this.props.slider.sliderTitleRight}</span>
					</div>
				</div>
				<div className={"input-box " + inputErrorClass}>
					<input
						ref={this.setReference}
						type="tel"
						className="slider-input"
						value={inputValue}
						maxLength="11"
						onChange={this.handleInputChange}
						onKeyDown={this.handleKeyDown}
					/>
					<span className="slider-input-prefix">&#x20b9;</span>
					<span className="error-text">
						{this.state.DownpaymentErrorText}
					</span>
				</div>
				<div className="vehicle-loan-text">
					Your loan amount will be <span className="vehicle-loan-amount">{vehicleLoanAmount}</span>
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
	} = activeModel.data.vehicleDownPayment

	return {
		slider,
		inputBox
	}
}

const mapDispatchToProps = (dispatch, getState) => {
	return {
		updateDownPaymentSlider: bindActionCreators(updateDownPaymentSlider, dispatch),
		updateDownPaymentInput: bindActionCreators(updateDownPaymentInput, dispatch),
		initToast: bindActionCreators(initToast, dispatch),
		clearToast: bindActionCreators(clearToast, dispatch),
		fireTracking: bindActionCreators(fireTracking, dispatch)
	}
}


export default connect(mapStateToProps, mapDispatchToProps)(emiDownPayment)
