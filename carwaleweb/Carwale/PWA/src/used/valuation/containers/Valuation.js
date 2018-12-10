import React from 'react'
import { connect } from 'react-redux'
import { bindActionCreators } from 'redux'
import style from '../../../../style/car-valuation.scss'

import Owners from './Owners'
import ValuationSwitch from './ValuationSwitch'
import ManufacturingDetails from './ManufactureDetails'
import CitySelection from './CitySelection'
import CarSelection from './CarSelection'
import KilometersDriven from './KilometersDriven'
import CheckReport from '../containers/CheckReport'
import ValuationReport from '../containers/ValuationReport';
import { VALUATION_ENDPOINT } from '../constants/index';
import { selectCity, fetchCityMaskingName } from '../actionCreators/CityPopup'
import {trackForMobile, trackingActionType} from '../utils/valuationTracking'
import { deserialzeQueryStringToObject } from '../../../utils/Common';
import {fireNonInteractiveTracking} from '../../../utils/Analytics'

class Valuation extends React.Component {
    render() {
        let { city } = deserialzeQueryStringToObject(window.location.search);
        return (
            <div className="valuation__content">
                <div className="valuation-content__head">
                    <h1 className="valuation__heading">Used Car Valuation</h1>
                </div>
                <div className="valuation-content__body">
                    <ValuationSwitch />
                    <ManufacturingDetails />
                    <CitySelection city={city} />
                    <CarSelection />
                    <Owners />
                    <KilometersDriven />
                    <CheckReport />
                    <ValuationReport />
                </div>
            </div>
        )
    }
    componentDidMount() {
        history.replaceState("valuationForm", "", VALUATION_ENDPOINT);//check if this can be replace state
        const location = this.props.location
        if (location.cityId != -1) {
            this.props.fetchCityMaskingName(location.cityId)
            let citySelection = {
                cityId: location.cityId,
                cityName: location.cityName,
                cityMaskingName: '',
                isConfirmBtnClicked: false
            }
            this.props.selectCity(citySelection)
        }
        trackForMobile(trackingActionType.valuationFormLoad, trackingActionType.valuationFormLoad)
    }
}

const mapStateToProps = state => {
    const { location } = state
    return {
        location
    }
}

const mapDispatchToProps = dispatch => {
    return {
        selectCity: bindActionCreators(selectCity, dispatch),
        fetchCityMaskingName: bindActionCreators(fetchCityMaskingName, dispatch)
    }
}

export default connect(mapStateToProps, mapDispatchToProps)(Valuation)
