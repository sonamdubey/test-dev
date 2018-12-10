import React from 'react'
import { connect } from 'react-redux'
import { bindActionCreators } from 'redux'
import { selectKilometers } from '../actionCreators/kilometers'
import {trackForMobile, trackingActionType} from '../utils/valuationTracking'

import {
    formatToINR,
    removeComma
} from './../../../utils/Common'

class KilometersDriven extends React.Component {
    constructor(props) {
        super(props);
    }
    handleInputChange = (event) => {
        const inputValue = removeComma(event.currentTarget.value)
        this.props.selectKilometers(inputValue);
    }
    onBlur= () => {
        trackForMobile(trackingActionType.kilometerDrivenFocusOut, '')
    }
    render() {
        const validationStatus = this.props.kmsDriven.isValid ? '' : 'invalid';
        const inputValue = this.props.kmsDriven.value ? formatToINR(this.props.kmsDriven.value) : ""
        return (
            <div className="kilometer-driven__container">
                <p className="kilometer-driven__title">kilometers</p>
                <div className={"kilometer-driven__input-container " + validationStatus}>
                    <input type="text"
                        className="form-control"
                        name="kilometers"
                        id="KilometersDriven"
                        placeholder="Enter kilometers driven"
                        value={inputValue}
                        onChange={this.handleInputChange}
                        onBlur ={this.onBlur}
                        required />
                    <span className="error-text">{this.props.kmsDriven.errorText}</span>
                </div>
            </div>
        )
    }
}

const mapStateToProps = (state) => {
    const {
        kmsDriven
    } = state.usedCar.valuation
    return {
        kmsDriven
    }
}

const mapDispatchToProps = (dispatch) => {
    return {
        selectKilometers: bindActionCreators(selectKilometers, dispatch)
    }
}

export default connect(mapStateToProps, mapDispatchToProps)(KilometersDriven)
