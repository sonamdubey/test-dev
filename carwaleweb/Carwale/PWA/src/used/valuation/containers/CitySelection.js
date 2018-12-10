import React from 'react'
import { connect } from 'react-redux'
import { bindActionCreators } from 'redux'

import CitySelectionInput from '../components/CitySelectionInput'
import CitySelectionPopup from './CitySelectionPopup'
import { toggleCityPopup, selectCity, clearCity } from '../actionCreators/CityPopup'
import ApiCall from '../apis/ValuationAPIs';
import {trackForMobile, trackingActionType} from '../utils/valuationTracking'
import { CITY_MASKING_NAME_END_POINT } from '../constants/index';
class CitySelection extends React.Component {
  constructor(props) {
    super(props);
  }

  toggleCityPopup = () => {
    if (!this.props.isActive) {
      window.addEventListener('popstate', this.onPopState);
      history.pushState("cityPopUp", "")
    }
    else {
      window.removeEventListener('popstate', this.onPopState)
    }
    this.props.toggleCityPopup(this.props.isActive)
  }

  setCity = (selectedCity) => {
      trackForMobile(trackingActionType.citySelect, selectedCity.cityName)
    this.props.selectCity(selectedCity)
    this.PopupBackClick()
  }
  setCityFromQS = (selectedCity) => {
    this.props.selectCity(selectedCity)
  }

  clearCity = () => {
    this.props.clearCity();
  }

  onPopState = () => {
    this.toggleCityPopup()
  }

  PopupBackClick = () => {
    history.back()
  }


  render() {
    return (
      <div className="city-selection">
        <CitySelectionInput
          onClickHandler={this.toggleCityPopup}
          value={this.props.selected.cityName}
          isValid={this.props.isValid} />
        <CitySelectionPopup
          isActive={this.props.isActive}
          handleBackClick={this.PopupBackClick}
          setCity={this.setCity}
          cityLocation={this.props.selected}
          clearSelection={this.clearCity} />
      </div>
    )
  }
  componentDidMount() {
    let cityFromQS = this.props.city;
    if (cityFromQS) {
      ApiCall.get(CITY_MASKING_NAME_END_POINT + cityFromQS + "/")
        .then((data) => {
          if (data) {
            this.setCityFromQS(data);
          }
        })
        .catch(error => console.log(error));
    }
  }
}

const mapStateToProps = state => {
  const { isActive, selected, isValid } = state.usedCar.valuation.city
  return {
    isActive, selected, isValid
  }
}

const mapDispatchToProps = (dispatch) => {
  return {
    toggleCityPopup: bindActionCreators(toggleCityPopup, dispatch),
    selectCity: bindActionCreators(selectCity, dispatch),
    clearCity: bindActionCreators(clearCity, dispatch)
  }
}

export default connect(mapStateToProps, mapDispatchToProps)(CitySelection)
