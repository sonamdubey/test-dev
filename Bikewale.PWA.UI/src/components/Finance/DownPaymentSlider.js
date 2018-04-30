import React from 'react';
import { connect } from 'react-redux'
import { bindActionCreators } from 'redux';
import { toJS } from '../../immutableWrapperContainer'

import Rheostat from '../Shared/Rheostat'
import PitComponent from '../Shared/RheostatPit'
import sliderAlgorithm from '../../utils/rheostat/algorithms/linear'

import { emiCalculatorAction } from '../../actionCreators/emiDownPaymentSlider'
import { updateDownPaymentSlider, updateDownPaymentInput } from '../../actionCreators/emiDownPaymentSlider'

import { formatToINR, formatToCurrency } from '../../utils/formatAmount'

class EMIDownPayment  extends React.Component {
  constructor(props) {
    super(props);
    this.state = {
      isDownpaymentValid: true,
      DownpaymentErrorText: '',
      oldDownpaymentAmount: 0,
      cursorPosition: 0,
      isFetching: true,
    }
    this.cursorTimer = null
  }
  componentDidMount(){
    const {
			slider
		} = this.props
  }
	componentDidUpdate(){
		if (!this.state.isFetching) {

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
  handleSliderChange = ({ values }) => {
		const {
			updateDownPaymentSlider,
		} = this.props

		updateDownPaymentSlider({ values, userChange: true })
	}
	handleSliderDragStart = () => {
		if((this.refs.downPaymentSlider.getElementsByClassName('rheostat-handle')) != document.activeElement){
			this.refs.downPaymentSlider.querySelectorAll('.rheostat-handle')[0].focus()
		}
	}
updateLoanText() {
		let loanAmountUpdated = this.props.slider.max - this.props.slider.values[0];
		return loanAmountUpdated
	}
  render() {
    let {
      slider
    } = this.props
    slider = {
			...slider,
			algorithm: sliderAlgorithm,
			className: 'slider-rheostat',
			pitComponent: PitComponent,
			pitPoints: [slider.min, slider.max],
			snap: true,
			snapOnDragMove: true,
			disableSnapOnClick: true,
			onChange: this.handleSliderChange,
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
	const slider = state.getIn(['Finance', 'VehicleDownPayment', 'slider'])

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