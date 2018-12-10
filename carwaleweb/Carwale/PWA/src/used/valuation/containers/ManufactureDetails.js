import React from 'react'
import { connect } from 'react-redux'
import { bindActionCreators } from 'redux'

import PropTypes from 'prop-types';
import { selectManufacturingDetails } from '../actionCreators/ManufacturingDetails'
import {trackForMobile, trackingActionType} from '../utils/valuationTracking'

class ManufacturingDetails extends React.Component {
    constructor(props) {
        super(props);
        this.state = {
            isEmpty: true,
            dateValue: ''
        }
        const today = new Date();
        let month = today.getMonth()+1;
        if (month < 10) {
            month = '0' + month;
        }
        this.mindate = props.manufacturingDetails.minYear + '-01';
        this.maxdate = props.manufacturingDetails.maxYear + '-' + month;
        let selectedYear = props.manufacturingDetails.year;
        let selectedMonth = props.manufacturingDetails.month;
        if (selectedYear > props.manufacturingDetails.minYear &&
            selectedYear < props.manufacturingDetails.maxYear
            && selectedMonth) {
            if (selectedMonth < 10) {
                selectedMonth = '0' + selectedMonth;
            }
            this.state = {
                isEmpty: false,
                dateValue: selectedYear + "-" + selectedMonth
            }
        }
    }
    handleRadioChange = (event) => {
        if (event.target.value.length > 0) {
            this.setState({
                isEmpty: false,
                dateValue: event.target.value
            });
            trackForMobile(trackingActionType.manufacturingYearSelect, '')
            this.props.selectManufacturingDetails(event.target.value);
        }
    }
    render() {
        const validationStatus = this.props.manufacturingDetails.isValid ? '' : 'invalid';
        const itemStatus = this.state.isEmpty ? 'active' : '';
        return (
            <div className="manufacture__container">
                <p className="manufacture__title">Manufacturing Month and Year</p>
                <div className={"manufacturing-date " + validationStatus}>
                    <input type="month"
                        className="form-control date-input"
                        name="month-picker"
                        id="monthPicker"
                        min={this.mindate}
                        max={this.maxdate}
                        placeholder="Select date"
                        value={this.state.dateValue}
                        onChange={this.handleRadioChange}
                        required />
                    <label
                        htmlFor="monthPicker"
                        className={"manufacturing-date__label " + itemStatus}
                    >
                        Select Year
                </label>
                    <span className="error-text">Please select year</span>
                </div>
            </div>
        )
    }
}
const mapDispatchToProps = (dispatch,getState) => {
    return {
        selectManufacturingDetails: bindActionCreators(selectManufacturingDetails, dispatch)
    }
}

const mapStateToProps = (state) => {
    const {
        manufacturingDetails
    } = state.usedCar.valuation

    return {
        manufacturingDetails
    }
}
export default connect(mapStateToProps, mapDispatchToProps)(ManufacturingDetails)

