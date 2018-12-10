import React from "react";
import PropTypes from "prop-types";

import classNames from "classnames";

import CityAutocomplete from "./CityAutocomplete";
const PropTypeReactComponent = PropTypes.oneOfType([
  PropTypes.func,
  PropTypes.string
]);

const propTypes = {
  // Add custom class
  className: PropTypes.string,
  //sucess url
  successUrl: PropTypes.string,
  //check is it popup
  isPopup: PropTypes.bool,
  //if is popup than slide direction
  slideDirection: PropTypes.string,
  //back button
  isBackButton: PropTypes.bool,
  // Close button to close location screen
  closeable: PropTypes.bool,
  // Confirm button class
  confirmBtnClass: PropTypes.string,
  // Define confirm button type
  confirmBtnType: PropTypes.string,
  //popup is active
  isActive: PropTypes.bool,
  // flag for showing primary text
  isPrimaryTextActive: PropTypes.bool,
  // customize primary heading
  primaryText: PropTypes.string,
  // location selection subtitle visibility
  isSubtitleActive: PropTypes.bool,
  // Customize Subtitle Text
  subtitleText: PropTypes.string,
  //Handle click of back arrow button
  handleBackClick: PropTypes.func,
  // Callback function gets fired on city selection
  setCity: PropTypes.func,
  // Pass object with city details
  cityLocation: PropTypes.object,
  // Callback function gets fired on clear button
  clearSelection: PropTypes.func,
  // Cities with areas available
  citiesWithArea: PropTypes.array,
  // Auto detect location
  autoDetect: PropTypes.bool,
  // Callback function gets firen on 'detect location' click
  onAutoDetectClick: PropTypes.func,
  // Allow user to select area
  area: PropTypes.bool,
  // Callback function gets fired on area selection
  setArea: PropTypes.func,
  // Pass custom input component
  renderInputComponent: PropTypeReactComponent,
  // Callback function gets fired on confirm button click
  onConfirmClick: PropTypes.func,
  // Show Confirm Button in every cases
  forceShowConfirmButton: PropTypes.bool,
  // Disable confirm button
  disableConfirmBtn: PropTypes.bool,
  // The validation status
  validateStatus: PropTypes.oneOf(['error', '']),
  // The helper message
  helperText: PropTypes.string,
};

const defaultProps = {
  successUrl: "/",
  isPopup: false,
  slideDirection: "right",
  isBackButton: false,
  confirmBtnClass: "btn-confirm-city",
  confirmBtnType: "secondary",
  isActive: false,
  isSubtitleActive: true,
  SubtitleText: "",
  handleBackClick: null,
  setCity: null,
  cityLocation: {
    cityName: "Select City",
    cityId: -1
  },
  clearSelection: null,
  citiesWithArea: [1, 2, 10, 12],
  autoDetect: false,
  area: false,
  setArea: null,
  renderInputComponent: CityAutocomplete
};

class LocationContainer extends React.Component {
  constructor(props) {
    super(props);

    this.state = this.getLocationState(this.props);
  }

  componentWillReceiveProps = nextProps => {
    this.setState(this.getLocationState(nextProps));
  };

  getLocationState = props => {
    const {
      cityLocation,
      citiesWithArea,
      area,
      forceShowConfirmButton,
      isActive
    } = props;

    const isCitySelected = cityLocation.cityId > -1;
    const isAreaAvailable = citiesWithArea.indexOf(cityLocation.cityId) > -1;
    const isAreaSelected = cityLocation.areaId > -1;

    return {
      showArea: isCitySelected && isAreaAvailable && area,
      showConfirmBtn: forceShowConfirmButton
        ? forceShowConfirmButton
        : isCitySelected
          ? !(isAreaAvailable && area && !isAreaSelected)
          : false,
      isActive
    };
  };

  resetConfirmButton = () => {
    this.setState({
      showConfirmBtn: false
    });
  };

  renderLocationDetector = () => {
    if (this.props.autoDetect) {
      return (
        <div className="location-detector">
          <span
            className="location-detector__button"
            onClick={this.props.onAutoDetectClick}
          >
            Detect my location
          </span>
        </div>
      );
    }
  };

  confirmCity = city => {
    const { cityLocation, setCity } = this.props;

    // If user clicks on 'confim' button,
    // get city details from `cityLocation` props
    if (!city) {
      city = cityLocation;
    }

    setCity({
      cityId: city.cityId,
      cityName: city.cityName,
      cityMaskingName: city.cityMaskingName,
      isConfirmBtnClicked: true
    });
  };

  confirmArea = area => {
    const { setArea } = this.props;

    setArea({
      areaId: area.areaId,
      areaName: area.areaName
    });
  };

  handleConfirmClick = () => {
    const { onConfirmClick } = this.props;

    onConfirmClick ? onConfirmClick() : this.confirmCity();
  };

  clearSelection = () => {
    if (this.props.clearSelection) {
      this.props.clearSelection();
    }
  };

  setTitle = () => {
    const { showArea, showConfirmBtn } = this.state;

    return this.props.isPrimaryTextActive ? (
      <p className="location-selection__title">{this.props.primaryText}</p>
    ) : (
        <p className="location-selection__title">
          {showConfirmBtn ? "Confirm" : "Select"} your{" "}
          {showArea ? "Area" : "City"}
        </p>
      );
  };

  render() {
    const {
      className,
      confirmBtnClass,
      confirmBtnType,
      isActive,
      isPopup,
      isBackButton,
      isPrimaryTextActive,
      primaryText,
      isSubtitleActive,
      subtitleText,
      slideDirection,
      handleBackClick,
      closeable,
      onClose,
      cityLocation,
      renderInputComponent: Autocomplete,
      setClearButton,
      citiesWithArea,
      validateStatus,
      helperText,
      disableConfirmBtn,
    } = this.props;

    // Location tags events
    const { onCityRemove, onAreaRemove } = this.props;

    const { showConfirmBtn } = this.state;

    const screenClassName = classNames("location-screen", className, {
      "location-popup--active": isActive,
      [`location-popup location-popup--${slideDirection}`]: isPopup
    });

    const confirmBtnClassName = classNames(confirmBtnClass, {
      [`btn-${confirmBtnType}`]: confirmBtnType,
    });
    return (
      <div className={screenClassName}>
        <div className="screen__head">
          {isBackButton ? (
            <span
              className="location-content__back-arrow"
              onClick={handleBackClick}
            />
          ) : (
              ""
            )}
          {closeable && (
            <span className="location-content__close" onClick={onClose} />
          )}
        </div>
        <div className="screen__body">
          <div className="location__content">
            <div className="location-content__head">
              {this.setTitle()}
              {isSubtitleActive ? (
                <p className="location-selection__subtitle">{subtitleText}</p>
              ) : (
                  ""
                )}
            </div>
            <div className="location-content__body">
              {
                  <Autocomplete
                    setCity={this.confirmCity}
                    cityLocation={cityLocation}
                    setClearButton={setClearButton}
                    clearSelection={this.clearSelection}
                    onChange={this.resetConfirmButton}
                    onClear={this.resetConfirmButton}
                    showConfirmBtn={showConfirmBtn}
                    setArea={this.confirmArea}
                    onCityRemove={onCityRemove}
                    onAreaRemove={onAreaRemove}
                    citiesWithArea={citiesWithArea}
                    validateStatus={validateStatus}
                    helperText={helperText}
                    isActive={isActive}
                 />
              }
              {this.renderLocationDetector()}
              {showConfirmBtn && (
                <button
                  className={confirmBtnClassName}
                  onClick={this.handleConfirmClick}
                  disabled={disableConfirmBtn}
                >
                  Confirm
                </button>
              )}
            </div>
          </div>
        </div>
      </div>
    );
  }
}

LocationContainer.propTypes = propTypes;
LocationContainer.defaultProps = defaultProps;

export default LocationContainer;
