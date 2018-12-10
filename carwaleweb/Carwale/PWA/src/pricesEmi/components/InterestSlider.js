import React from 'react'
import PropTypes from 'prop-types'
import { connect } from 'react-redux'
import { bindActionCreators } from 'redux'
import Rheostat from '../../components/Rheostat'
import PitComponent from '../../components/RheostatPit'
import algorithm from '../../utils/rheostat/algorithms/fix'
import {algoObj} from '../../utils/rheostat/constants/interesetSnapPoints'
import {createNewSnapPoints} from '../../utils/rheostat/function/DiffSnapPoints'

import {
	initToast,
	clearToast
} from '../../actionCreators/Toast'

import {
	updateInterestSlider,
	updateInterestInput
} from '../actionCreators/emiInterestSlider'

import {
	handleCursorPosition
} from '../../utils/Common'

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

class emiInterest extends React.Component {
	constructor(props) {
		super(props)
		this.state = {
			isInterestValid: true,
			InterestErrorText: '',
			oldInterestAmount: 0,
			cursorPosition: 0,
			isFetching: true,
			toastVisible: false
		}

		this.cursorTimer = null,
		this.setToastRef = this.setToastRef.bind(this)
		this.handleClickOutside = this.handleClickOutside.bind(this)
	}

componentDidMount(){
		const {
			updateInterestSlider,
			slider
		} = this.props
	}

componentWillReceiveProps(nextprops){
		const { inputBox } = nextprops
		this.validateInterest(inputBox.value)
	}
componentWillUnmount() {
	this.props.clearToast()
}
	componentDidUpdate(){
		if (!this.state.isFetching) {
			const {
				inputBox
			} = this.props

			const {
				oldInterestAmount,
				cursorPosition
			} = this.state

			let newPosition = handleCursorPosition(inputBox.value, oldInterestAmount, cursorPosition)

			if (this.InterestInputField === document.activeElement) {
				this.InterestInputField.classList.add('disable--cursor')

				clearTimeout(this.cursorTimer)
				this.cursorTimer = setTimeout(() => {
					this.InterestInputField.selectionStart = this.InterestInputField.selectionEnd = newPosition
					this.InterestInputField.classList.remove('disable--cursor')
				}, 0)
			}
		}
	}

	setToastRef(node) {
		this.toastRef = node;
	}
	handleClickOutside(event) {
		if(!this.state.toastVisible){
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
			updateInterestSlider,
			inputBox
		} = this.props

		updateInterestSlider({ values, userChange: true })
	}

	handleSliderDragStart = () => {
		this.InterestInputField.classList.add('disable--cursor')
		if(this.InterestInputField == document.activeElement){
			this.InterestInputField.blur()
		}
		this.refs.InterestSlider.querySelectorAll('.rheostat-handle')[0].focus()
	}

	handleSliderDragEnd = () => {
		this.InterestInputField.classList.remove('disable--cursor')
		if((this.refs.InterestSlider.getElementsByClassName('rheostat-handle')) != document.activeElement){
			this.refs.InterestSlider.querySelectorAll('.rheostat-handle')[0].focus()
		}

		/**
		 * For Desktop, after `dragEnd` event invocation `click` event is fired.
		 * To avoid duplicate tracking, disable tracking from desktop.
		 */
		if (!isDesktop) {
			this.props.fireTracking({ gaTrackingAction : "Interest_Slider_Clicked", brighuTrackingComponent : emiComponents.InterestSlider });
		}
	}

	handleSliderClick = () => {
		this.props.fireTracking({ gaTrackingAction : "Interest_Slider_Clicked", brighuTrackingComponent : emiComponents.InterestSlider });
	}

	handleInputChange = (event) => {
		const {
			updateInterestInput,
			inputBox
		} = this.props

		let inputValue = (event.currentTarget.value)
		if(!isNaN(inputValue)){
			updateInterestInput(inputValue, true)
		}
		this.setState({
			oldInterestAmount: inputBox.value,
			cursorPosition: this.InterestInputField.selectionEnd
		})
	}

	validateInterest = (updatedValue) => {
		const {
			slider
		} = this.props

		if (updatedValue < slider.min) {
			this.setState({
				isInterestValid: false,
				InterestErrorText: 'Interest rate should be between 1 to 15'
			})
		}
		else if(updatedValue > slider.max){
			this.setState({
				isInterestValid: false,
				InterestErrorText: 'Interest rate should be between 1 to 15'
			})
		}
		else {
			this.setState({
				isInterestValid: true,
				InterestErrorText: ''
			})
		}
	}

	setReference = (ref) => {
		this.InterestInputField = ref
	}
	handleToastClick(event){
		this.setToast(event)
		if(!this.state.toastVisible){
			document.addEventListener('click', this.handleClickOutside);
		}
	}
	setToast = (event) => {
		this.props.initToast({
			message: 'This is the interest rate at which you can avail your loan.',
			event
		})
	}
	handleOutsideClick = (e) => {
		this.handleToastClick();
	}
	render() {
        let {
            slider,
            inputBox
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
			onSliderDragStart: this.handleSliderDragStart,
			onSliderDragEnd: this.handleSliderDragEnd,
			onClick: this.handleSliderClick
		}
		let inputValue = inputBox.value == '0' ? '' : (inputBox.value)
		let inputErrorClass = this.state.isInterestValid ? '' : 'status-invalid'
		return (
				<div className="slider-input-container interest-unit">
                    <span className="slider-unit-title" ref={node => { this.node = node; }}>Interest <span className="unit-text">(%)</span> <span ref={this.setToastRef} onClick={this.handleToastClick.bind(this)} className="info-icon"></span></span>
					<div className="slider-section" ref="InterestSlider">
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
							maxLength="4"
							onChange={this.handleInputChange}
						/>
						<span className="error-text">
							{this.state.InterestErrorText}
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
	} = activeModel.data.vehicleInterest
	return {
		slider,
		inputBox
	}
}

const mapDispatchToProps = (dispatch, getState) => {
	return {
		updateInterestSlider: bindActionCreators(updateInterestSlider, dispatch),
		updateInterestInput: bindActionCreators(updateInterestInput, dispatch),
		initToast: bindActionCreators(initToast, dispatch),
		clearToast: bindActionCreators(clearToast, dispatch),
		fireTracking: bindActionCreators(fireTracking, dispatch)
	}
}


export default connect(mapStateToProps, mapDispatchToProps)(emiInterest)
