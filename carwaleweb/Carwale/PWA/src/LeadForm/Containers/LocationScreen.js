import React from "react";

import { bindActionCreators } from "redux";
import { connect } from "react-redux";
import shallowequal from "shallowequal";

import LocationContainer from "GlobalComponent/LocationContainer";
import LocationTags from "GlobalComponent/LocationTags";
import { setCity } from "GlobalActionCreator/CityAutocomplete";
import { setInfoLog, setErrorLog } from "../ActionCreators/LogState";
import { setFormScreen, setSorryScreen } from "../ActionCreators/ScreenState";
import campaignApi from "../Api/Campaign";
import { makeCancelable } from "../../utils/CancelablePromise";
import { setCampaignModelKey } from "../ActionCreators/Campaign";
import store from "../store";
import "GlobalStyle/location.scss";
import {
  fireNonInteractiveTracking,
  fireInteractiveTracking
} from "../../utils/Analytics";
import { AreaCities } from "../utils/Globals";
import {
  SetLocation,
  setLeadSourceCity,
  setCityChange
} from "../ActionCreators/Location";

export class LocationScreen extends React.Component {
  constructor(props) {
    super(props);

    this.platform = this.props.page.platform;
    this.page = this.props.page.page;

    this.category = `LeadForm-${this.platform.name}-${this.page.name}`;
    this.action;
    this.label;

    this.state = {
      location: this.props.location,
      isValid: true,
      helperText: "",
      isCampaignRequestFetching: false
    };
  }

  componentWillReceiveProps(nextProps) {
    if (
      !shallowequal(this.state.location, nextProps.location) &&
      nextProps.location != null &&
      nextProps.location.cityId > 0
    ) {
      this.setState({
        location: nextProps.location
      });
    }
  }

  selectCity = city => {
    let location = {
      ...this.state.location,
      cityId: city.cityId,
      cityName: city.cityName
    };
    if (
      this.props.location.cityId === -1 &&
      (this.props.location.cityName === "Select City" ||
        this.props.location.cityName === "")
    ) {
      this.SetLocation(location);
      this.props.setCityChange(true);
    }
    this.resetError();
    this.setState({
      location: {
        ...this.state.location,
        cityId: city.cityId,
        cityName: city.cityName
      }
    });
  };

  selectArea = area => {
    this.resetError();
    this.setState({
      location: {
        ...this.state.location,
        areaId: area.areaId,
        areaName: area.areaName
      }
    });
  };

  handleCityRemove = () => {
    let location = {
      cityId: -1,
      cityName: "Select City",
      areaId: -1,
      areaName: "Select Area"
    };
    this.resetError();
    this.setState({
      location
    });
  };

  handleAreaRemove = () => {
    let location = {
      areaId: -1,
      areaName: "Select Area"
    };
    this.resetError();
    this.selectArea(location);
  };

  handleValidation = () => {
    const { cityId, areaId } = this.state.location;
    if (cityId > 0) {
      if (AreaCities.includes(cityId)) {
        if (areaId < 0) {
          this.setState({
            isValid: false,
            helperText: "Select area!"
          });

          return false;
        }
      }
      return true;
    }

    this.setState({
      isValid: false,
      helperText: "Select city!"
    });

    return false;
  };

  resetError = () => {
    (this.state.isValid = true), (this.state.helperText = "");
  };

  SetLocation = location => {
    this.props.SetLocation(location);
  };

  handleConfirmButtonClick = () => {
    if (this.handleValidation()) {
      let { location } = this.props;

      this.action = "Location-City_Filled_In";
      this.label = `${this.props.modelDetail.modelId}_${
        this.state.location.cityName
      }`;
      fireInteractiveTracking(this.category, this.action, this.label);

      this.SetLocation(this.state.location);
      this.props.setLeadSourceCity(false);

      if (this.state.location.cityId != location.cityId) {
        this.props.setCityChange(true);
      }

      this.fetchCampaign();
    } else {
      return false;
    }
  };

  fetchCampaign = () => {
    let interactionId = store.getState().interactionId;
    if (this.state.location.cityId > 0) {
      if (this.campaignRequest) {
        this.campaignRequest.cancel();
      }
      this.campaignRequest = makeCancelable(
        campaignApi.get(
          this.state.location,
          this.props.modelDetail,
          this.platform.id,
          this.props.page.applicationId
        )
      );

      this.setState({
        isCampaignRequestFetching: true
      });
      this.campaignRequest.promise
        .then(campaign => {
          this.action = "FormOpen-After_City_Filled";
          this.label =
            this.props.modelDetail.modelId + "_" + this.state.location.cityName;

          this.props.setCampaignModelKey(
            campaign,
            this.props.modelDetail.modelId
          );

          this.props.setInfoLog(
            "Fetched Campaign data from API",
            interactionId
          );

          this.props.setInfoLog("Leaving LocationScreen", interactionId);
          this.props.setFormScreen();

          fireInteractiveTracking(this.category, this.action, this.label);
        })
        .catch(error => {
          this.action = "SorryMessage-After_City_Filled";
          this.label =
            this.props.modelDetail.modelId + "_" + this.state.location.cityName;

          this.props.setInfoLog("Leaving LocationScreen", interactionId);
          this.props.setSorryScreen();

          fireNonInteractiveTracking(this.category, this.action, this.label);

          this.props.setErrorLog(
            `Error: ${error}. Found in Campaign API`,
            interactionId
          );
        });
    }
  };

  shouldComponentUpdate(nextProps, nextState) {
    if (
      this.props.isLeadFormVisible != nextProps.isLeadFormVisible &&
      !nextProps.isLeadFormVisible
    ) {
      this.setState({
        isValid: true,
        helperText: ""
      });
    }
    if (
      this.state.location.cityId != nextState.location.cityId ||
      this.state.location.areaId != nextState.location.areaId ||
      this.state.isValid !== nextState.isValid ||
      this.state.isCampaignRequestFetching !==
        nextState.isCampaignRequestFetching
    ) {
      return true;
    }

    return false;
  }

  render() {
    const {
      location,
      isValid,
      helperText,
      isCampaignRequestFetching
    } = this.state;
    return (
      <LocationContainer
        setCity={this.selectCity}
        setArea={this.selectArea}
        onCityRemove={this.handleCityRemove}
        onAreaRemove={this.handleAreaRemove}
        area
        setClearButton={false}
        isActive
        isPrimaryTextActive
        primaryText="Please tell us your city"
        isSubtitleActive
        subtitleText="This allows us to provide relevant content for you"
        confirmBtnClass="lead-location__confirm-btn"
        confirmBtnType="primary"
        cityLocation={location}
        forceShowConfirmButton
        renderInputComponent={LocationTags}
        onConfirmClick={this.handleConfirmButtonClick}
        validateStatus={!isValid ? "error" : ""}
        helperText={helperText}
        disableConfirmBtn={isCampaignRequestFetching}
      />
    );
  }
}

const mapStateToProps = state => {
  const { page } = state.leadClickSource;
  const { location, isLeadFormVisible } = state;

  return {
    location: location,
    page,
    isLeadFormVisible
  };
};

const mapDispatchToProps = dispatch => {
  return {
    setCity: bindActionCreators(setCity, dispatch),
    setFormScreen: bindActionCreators(setFormScreen, dispatch),
    setSorryScreen: bindActionCreators(setSorryScreen, dispatch),
    setInfoLog: bindActionCreators(setInfoLog, dispatch),
    setErrorLog: bindActionCreators(setErrorLog, dispatch),
    setCampaignModelKey: bindActionCreators(setCampaignModelKey, dispatch),
    SetLocation: bindActionCreators(SetLocation, dispatch),
    setLeadSourceCity: bindActionCreators(setLeadSourceCity, dispatch),
    setCityChange: bindActionCreators(setCityChange, dispatch)
  };
};

export default connect(
  mapStateToProps,
  mapDispatchToProps
)(LocationScreen);
