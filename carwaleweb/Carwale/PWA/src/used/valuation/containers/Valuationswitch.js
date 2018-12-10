import React from 'react'
import { connect } from 'react-redux'
import { bindActionCreators } from 'redux'
import PropTypes from 'prop-types';

import SwitchElement from '../components/SwitchElement'
import { selectValuationType } from '../actionCreators/ValuationType'
import {trackForMobile, trackingActionType} from '../utils/valuationTracking'
const propTypes = {
    //on radio button change
    onSwitchChange: PropTypes.func
}
const defaultProps = {
    onSwitchChange: null
}
class ValuationSwitch extends React.Component {
    constructor(props) {
        super(props);
    }
 onSwitchChange = (event) => {
        trackForMobile(trackingActionType.valuationTypeSelect, '')
        this.props.selectValuationType(event.currentTarget.value);
    }
    render() {
        return (
            <div className="valuation-switch__container" >
                <p className="valuation-switch__title">I want to</p>
                <div className="valuation-switch__togglewrapper">
                    <SwitchElement
                        labelName="Buy a Car"
                        radioButtonName="valuationType"
                        value={1}
                        status={(this.props.type === 1) ? 'checked': ''}
                        onSwitchChange={this.onSwitchChange}
                        id="ValuationTypeSwitcher1"
                    />
                    <SwitchElement
                        labelName="Sell a Car"
                        radioButtonName="valuationType"
                        value={2}
                        status={(this.props.type === 2) ? 'checked': ''}
                        onSwitchChange={this.onSwitchChange}
                        id="ValuationTypeSwitcher2"
                        />
                </div>
            </div>
        )
    }
}
ValuationSwitch.propTypes = propTypes
ValuationSwitch.defaultProps  = defaultProps


const mapStateToProps = (state) => {
    const {
        type
    } = state.usedCar.valuation

    return {
        type
    }
}
const mapDispatchToProps = (dispatch) => {
    return {
        selectValuationType: bindActionCreators(selectValuationType, dispatch)
    }
}

export default connect(mapStateToProps, mapDispatchToProps)(ValuationSwitch)
