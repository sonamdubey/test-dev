import React from 'react'
import { connect } from 'react-redux'
import { bindActionCreators } from 'redux'

import LocationContainer from '../../components/LocationContainer'
import Toast from '../../components/Toast'
import LocationTags from '../../components/LocationTags'

import { setCity, setArea, detectLocation, selectAreaCities } from '../../actionCreators/CityAutocomplete'
import { openPopup, closePopup } from '../actionCreators'
import { clearToast } from '../../actionCreators/Toast'

import { lockScroll, unlockScroll } from '../../utils/ScrollLock'
import SpeedometerLoader from '../../components/SpeedometerLoader'
import '../../../style/location.scss'
import { fireInteractiveTracking } from '../../utils/Analytics';

class Location extends React.Component {
  constructor(props) {
    super(props);

    this.state = {
      location: this.props.location,
      validation: {
        isValid: true,
        message: '',
      }
    }
  }

  componentWillUnmount() {
    this.props.clearToast()
  }

  componentDidMount() {
    const { isActive } = this.props;

    if (isActive) {
      this.togglePopup(isActive);
    }
  }

  componentWillUnmount() {
    this.props.clearToast()
  }

  componentWillReceiveProps(nextProps) {
    const {
      isActive,
      location,
    } = this.props;

    if (location !== nextProps.location) {
      this.setState({
        location: nextProps.location
      })
    }

    if (isActive !== nextProps.isActive) {
      this.togglePopup(nextProps.isActive);
    }
  }

  shouldComponentUpdate(nextProps, nextState) {
    const {
      isActive,
      location,
    } = this.props;

    if (isActive !== nextProps.isActive || location !== nextProps.location || this.state.location !== nextState.location) {
      return true;
    }

    return false;
  }

  selectCity = (city) => {
    let location = this.state.location;
    this.props.setCity(city);
    fireInteractiveTracking("GlobalCityPopUp", "Type_City_Select_List", this._getGALabel());
  }

  selectArea = (area) => {
    this.props.setArea(area)
    fireInteractiveTracking("GlobalCityPopUp", "Type_Area_Select_List", this._getGALabel());
  }

  handleCityRemove = () => {
    let location = this.state.location;
    this.setState({
      location: {
        ...this.state.location,
        cityId: -1,
        cityName: 'Select City',
        areaId: -1,
        areaName: 'Select Area',
      }
    });
    fireInteractiveTracking("GlobalCityPopUp", "Typed_City_Clear", this._getGALabel(location));
  }

  handleAreaRemove = () => {
    let location = this.state.location;
    this.setState({
      location: {
        ...this.state.location,
        areaId: -1,
        areaName: 'Select Area',
      }
    });
    fireInteractiveTracking("GlobalCityPopUp", "Typed_Area_Clear", this._getGALabel(location));
  }

  handleClose = () => {
    this.props.closePopup()
    this.props.selectAreaCities([]);
  }

  handleConfirmClick = () => {
    this.props.closePopup();
    fireInteractiveTracking("GlobalCityPopUp", "Typed_CityArea_Confirm", this._getGALabel());
  }

  handleAutoDetectClick = () => {
    this.props.detectLocation();
    fireInteractiveTracking("GlobalCityPopUp", "DetectMyLocation_Click", this._getGALabel());
  }

  togglePopup = (isActive) => {
    if (isActive) {
      lockScroll();
    }
    else {
      unlockScroll();
    }
  }

  _getGALabel = (loc) => {
    let location = loc || this.state.location;
    return (location.cityId > 0 ? location.cityName : "") +
      (location.areaId > 0 ? "|" + location.areaName : "");
  }

  isFetchingLocation = () => {
    if (this.state.location.isFetching) {
      return <SpeedometerLoader />
    }
  }

  render() {
    const {
      location,
    } = this.state;

    const {
      isActive,
      shouldShowCrossIcon
    } = this.props;

    return (
      <div className="location-root">
        {this.isFetchingLocation()}
        <LocationContainer
          className="global-location-screen"
          isPopup
          closeable={shouldShowCrossIcon}
          autoDetect
          isActive={isActive}
          onClose={this.handleClose}
          setClearButton={false}
          isSubtitleActive={false}
          cityLocation={location}
          area
          setCity={this.selectCity}
          setArea={this.selectArea}
          onCityRemove={this.handleCityRemove}
          onAreaRemove={this.handleAreaRemove}
          renderInputComponent={LocationTags}
          onConfirmClick={this.handleConfirmClick}
          onAutoDetectClick={this.handleAutoDetectClick}
          citiesWithArea={location.areaCities}
        />
        <Toast />
      </div>
    )
  }
}

const mapStateToProps = (state) => {
  const {
    isActive,
    location,
    shouldShowCrossIcon
  } = state;

  return {
    isActive,
    location,
    shouldShowCrossIcon
  }
}

const mapDispatchToProps = (dispatch) => {
  return {
    setCity: bindActionCreators(setCity, dispatch),
    setArea: bindActionCreators(setArea, dispatch),
    openPopup: bindActionCreators(openPopup, dispatch),
    closePopup: bindActionCreators(closePopup, dispatch),
    detectLocation: bindActionCreators(detectLocation, dispatch),
    selectAreaCities: bindActionCreators(selectAreaCities, dispatch),
    clearToast: bindActionCreators(clearToast, dispatch),
  }
}

export default connect(mapStateToProps, mapDispatchToProps)(Location)
