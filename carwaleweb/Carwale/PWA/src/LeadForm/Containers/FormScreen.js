import React, { Component } from "react";
import { bindActionCreators } from "redux";
import { connect } from "react-redux";
import shallowequal from "shallowequal";

import Form from "oxygen/lib/Form";
import Input from "oxygen/lib/Input";
import Button from "oxygen/lib/Button";
import Select from "oxygen/lib/Select";
import Checkbox from "oxygen/lib/Checkbox/Checkbox";

import FormScreenHeader from "../Components/FormScreenHeader";
import store from "../store";
import { setFormLead } from "../ActionCreators/FormState";
import {
  setErrorLog,
  setDebugLog,
  setInfoLog
} from "../ActionCreators/LogState";
import { setMLAData } from "../ActionCreators/ScreenState";
import dealersApi from "../Api/Dealer";
import citiesApi from "../Api/City";
import MLAApi from "../Api/MLA";
import { makeCancelable } from "../../utils/CancelablePromise";
import {
  validateMobile,
  validateName,
  validateCity,
  validateEmail,
  validateDealer
} from "../utils/Validate";
import { MLAMappingApiToStore } from "../utils/ObjectMapping";
import { fireInteractiveTracking } from "../../utils/Analytics";
import { setCityChange, setLeadSourceCity } from "../ActionCreators/Location";
import { trackNameErr, trackMobileErr, trackEmailErr } from "../utils/Tracking";
import CityDropdown from "../Components/CityDropdown";
import { formType } from "../Enum/FormType";

const FormItem = Form.Item;

export class FormScreen extends Component {
  constructor(props) {
    super(props);
    const { location, buyerInfo } = this.props;
    const { page } = this.props;
    (this.optionalCity = false),
      (this.optionalDealer = false),
      (this.optionalName = false),
      (this.optionalMobile = false),
      (this.optionalEmail = false);

    this.state = {
      buyerInfo: {
        name: buyerInfo.name,
        mobile: buyerInfo.mobile,
        email: buyerInfo.email
      },
      assignedDealerId: "-1",
      cityId: location.cityId,
      cityName: location.cityName,
      showLocation: false,
      showSeller: false,
      citySelectItems: [],
      dealerSelectItems: [],
      testDriveChecked: this.props.testDriveChecked,
      validation: {
        name: {
          isValid: true,
          errMessage: ""
        },
        mobile: {
          isValid: true,
          errMessage: ""
        },
        city: {
          isValid: true,
          errMessage: ""
        },
        dealer: {
          isValid: true,
          errMessage: ""
        },
        email: {
          isValid: true,
          errMessage: ""
        }
      },
      isDisabled: false
    };
    this.MLAAPIRequest = null;
    this.CityAPIRequest = null;
    this.DealerAPIRequest = null;

    this.platformName = page.platform.name;
    this.pageName = page.page.name;

    this.category = `LeadForm-${this.platformName}-${this.pageName}`;
    this.action;
    this.label;
  }

  getDealersList() {
    let { campaign, featuredCarData } = this.props.campaign;
    let { location } = this.props;
    let interactionId = store.getState().interactionId;
    if (
      location.cityId > 0 &&
      (campaign.leadPanel == 1 || campaign.leadPanel == 3)
    ) {
      let message = "Fetched Seller data from API";
      let request = makeCancelable(
        dealersApi.get(campaign.id, featuredCarData.modelId, location.cityId)
      ); //dealer list api

      this.dealerAPIRequest = request;

      request.promise
        .then(dealerList => {
          this.dealerAPIRequest = null;
          if (dealerList.length > 1) {
            this.setState({
              dealerSelectItems: dealerList,
              showSeller: true
            });
          } else {
            this.setState({
              dealerSelectItems: dealerList == null ? [] : dealerList,
              showSeller: false,
              assignedDealerId: dealerList.length == 1 ? dealerList[0].id : "-1"
            });
          }
          this.props.setInfoLog(message, interactionId);
        })
        .catch(error => {
          this.dealerAPIRequest = null;
          this.props.setErrorLog(
            `Error: ${error}. Found in Seller API`,
            interactionId
          );
        });
    }
  }

  getMLAData(cityId) {
    let { campaign, featuredCarData } = this.props.campaign;
    let { location, page } = this.props;
    cityId = cityId != undefined ? cityId : location.cityId;

    let interactionId = store.getState().interactionId;
    if (campaign.mutualLeads && cityId > 0) {
      let request = makeCancelable(
        MLAApi.get(
          cityId,
          location.areaId < 0 ? 0 : location.areaId,
          featuredCarData.modelId,
          page
        )
      ); //mla API
      this.MLAAPIRequest = request;

      request.promise
        .then(MLAData => {
          this.MLAAPIRequest = null;
          let filteredMLA = [];
          let message = "Fetched MLA Data from API";
          let MLADictionary = {};
          this.props.setInfoLog(message, interactionId);

          if (MLAData != null && MLAData.length != 0) {
            filteredMLA = MLAData.filter(MLADataItem => {
              if (
                campaign.id != MLADataItem.campaign.id &&
                MLADataItem.campaign.mutualLeads &&
                (campaign.dealerAdminId == 0 ||
                  campaign.dealerAdminId != MLADataItem.campaign.dealerAdminId)
              ) {
                return MLADataItem;
              }
            });
            if (filteredMLA.length > 0) {
              filteredMLA.map(MLA => {
                let MLAKey = `${MLA.campaign.id}-${MLA.dealerDetails.id}`;
                let MLAObject = MLAMappingApiToStore(MLA);
                MLADictionary[MLAKey] = MLAObject;
              });
            }
            if (Object.keys(MLADictionary).length > 0) {
              this.props.setMLAData(MLADictionary);
            }
          }
        })
        .catch(error => {
          this.MLAAPIRequest = null;
          let message = `Error: ${error}. Found in MLA API`;
          this.props.setErrorLog(message, interactionId);
        });
    }
  }

  getCitiesList() {
    // Get Location list when Location is not available
    let { campaign, featuredCarData } = this.props.campaign;
    let { location } = this.props;
    let interactionId = store.getState().interactionId;
    if (location.cityId <= 0) {
      //cities list
      store.dispatch(setLeadSourceCity(false));
      let request = makeCancelable(
        citiesApi.get(campaign.id, featuredCarData.modelId)
      );
      this.cityAPIRequest = request;
      let message = "Fetched Location data from API";
      request.promise
        .then(cityList => {
          this.cityAPIRequest = null;
          let cityData = [];
          if (cityList != null && cityList.length != 0) {
            cityData = cityList;
          }
          this.setState({
            citySelectItems: cityData,
            showLocation: cityData.length == 0 ? false : true
          });

          this.props.setInfoLog(message, interactionId);
        })
        .catch(error => {
          this.cityAPIRequest = null;
          this.props.setErrorLog(
            `Error: ${error}. Found in Location API`,
            interactionId
          );
        });
    }
  }

  getSecondaryData() {
    this.resetDealerList();
    this.resetCityList();
    this.getDealersList();
    this.getMLAData();
    this.getCitiesList();
  }

  resetErrorState(nextState) {
    let validationObj = {
      name: {
        isValid: true,
        errMessage: ""
      },
      mobile: {
        isValid: true,
        errMessage: ""
      },
      city: {
        isValid: true,
        errMessage: ""
      },
      dealer: {
        isValid: true,
        errMessage: ""
      },
      email: {
        isValid: true,
        errMessage: ""
      }
    };
    this.setState({ validation: validationObj });
  }

  resetDealerList() {
    //TODO Test
    if (this.dealerAPIRequest) {
      this.dealerAPIRequest.cancel();
      this.dealerAPIRequest = null;
    }
    this.state.dealerSelectItems = [];
    this.state.showSeller = false;
    this.state.assignedDealerId = "-1";
  }

  resetCityList() {
    //TODO Test
    if (this.cityAPIRequest) {
      this.cityAPIRequest.cancel();
      this.cityAPIRequest = null;
    }
    this.state.citySelectItems = [];
    this.state.showLocation = false;
  }

  componentDidUpdate() {
    if (this.props.campaign.campaign.id != this.campaignId) {
      this.campaignId = this.props.campaign.campaign.id;
      this.getSecondaryData();
      let message = "Re-Entering FormScreen";
      this.props.setInfoLog(message);
    }
  }

  componentDidMount() {
    this.campaignId = this.props.campaign.campaign.id;
    this.getSecondaryData();
    let message = "Entering FormScreen";
    this.props.setInfoLog(message);
    if (this.props.campaign.campaign.isEmailRequired) {
      let message =
        "Email input shown (Current Store data Provided in currentState)";
      this.props.setInfoLog(message);
    }
  }

  componentWillUnmount() {
    if (this.MLAAPIRequest != null) this.MLAAPIRequest.cancel();

    if (this.cityAPIRequest != null) this.cityAPIRequest.cancel();

    if (this.dealerAPIRequest != null) {
      this.dealerAPIRequest.cancel();
    }
  }

  shouldComponentUpdate(nextProps, nextState) {
    if (this.props.isLeadFormVisible != nextProps.isLeadFormVisible) {
      this.resetErrorState();
    }

    let { campaign, featuredCarData } = this.props.campaign;
    let { location } = this.props;
    // Log when name or mobile is changed
    if (
      this.state.buyerInfo.name != nextState.buyerInfo.name ||
      this.state.buyerInfo.mobile != nextState.buyerInfo.mobile
    ) {
      let message = "Name or Mobile Changed";
      this.props.setDebugLog(message, this.state, nextState);
    }

    // Log when city is changed
    if (this.state.cityId != nextState.cityId) {
      let message = "CityId Changed";
      this.props.setDebugLog(message, this.state, nextState);
    }

    //Get Dealers list based on Location and handle promise cancellation
    // Log when dealer is changed
    if (this.state.assignedDealerId != nextState.assignedDealerId) {
      let message = "AssignedDealerId Changed";
      this.props.setDebugLog(message, this.state, nextState);
    }

    // Get Dealers list based on Location and handle promise cancellation
    if (
      this.state.showLocation &&
      nextState.cityId > 0 &&
      this.state.cityId != nextState.cityId
    ) {
      if (this.dealerAPIRequest != null) {
        this.dealerAPIRequest.cancel();
      }

      let request = makeCancelable(
        dealersApi.get(campaign.id, featuredCarData.modelId, nextState.cityId)
      );

      this.dealerAPIRequest = request;
      let interactionId = store.getState().interactionId;
      request.promise
        .then(dealerList => {
          this.dealerAPIRequest = null;
          let message = "Fetched Seller data from API when city selected";
          this.props.setInfoLog(message, interactionId);

          if (dealerList.length > 1) {
            this.setState({
              dealerSelectItems: dealerList,
              showSeller: true,
              assignedDealerId: "-1"
            });
          } else {
            this.setState({
              dealerSelectItems: dealerList == null ? [] : dealerList,
              showSeller: false,
              assignedDealerId: dealerList.length == 1 ? dealerList[0].id : "-1"
            });
          }
        })
        .catch(error => {
          let message;
          if (error.isCanceled) {
            message = `Request canceled for cityId ${nextState.cityId}`;
            this.props.setErrorLog(message, interactionId);
          } else {
            message = `Error: ${error}. Found in Seller API when city selected`;
            this.props.setErrorLog(message, interactionId);
          }
        });
    }
    if (
      nextProps.campaign.campaign.id != campaign.id ||
      nextProps.campaign.campaign.dealerId != campaign.dealerId
    ) {
      let message =
        "Updated Campaign shown (Current Store data Provided in currentState)";
      this.props.setInfoLog(message);
    }
    if (this.state.showLocation != nextState.showLocation) {
      let message =
        "City dropdown shown (Current Store data Provided in currentState)";
      this.props.setInfoLog(message);
    }
    if (this.state.showSeller != nextState.showSeller) {
      let message =
        "Dealer dropdown shown (Current Store data Provided in currentState)";
      this.props.setInfoLog(message);
    }
    if (
      campaign.isEmailRequired != nextProps.campaign.campaign.isEmailRequired
    ) {
      let message =
        "Email input shown (Current Store data Provided in currentState)";
      this.props.setInfoLog(message);
    }
    if (
      nextProps.campaign.campaign.id != campaign.id ||
      nextProps.campaign.campaign.dealerId != campaign.dealerId
    ) {
      return true;
    }
    if (this.state.showLocation != nextState.showLocation) {
      return true;
    }
    if (this.state.showSeller != nextState.showSeller) {
      return true;
    }
    if (
      campaign.isEmailRequired != nextProps.campaign.campaign.isEmailRequired
    ) {
      return true;
    }
    if (nextProps.testDriveChecked != this.props.testDriveChecked) {
      this.setState({ testDriveChecked: nextProps.testDriveChecked });
      return false;
    }

    if (this.state.testDriveChecked != nextState.testDriveChecked) {
      return true;
    }
    if (!shallowequal(this.state.validation, nextState.validation)) {
      return true;
    }
    if (this.state.isDisabled != nextState.isDisabled) {
      return true;
    }
    return false;
  }

  handleNameChange = event => {
    let inputName = event.target.value;
    let nameValidation = validateName(inputName, this.optionalName);
    if (!nameValidation.isValid) {
      trackNameErr(nameValidation.errMessage, formType.FORM);
    }

    this.setState({
      validation: {
        ...this.state.validation,
        name: nameValidation
      },
      buyerInfo: { ...this.state.buyerInfo, name: inputName.trim() }
    });
  };

  handleMobileChange = event => {
    let inputMobile = event.target.value;
    let mobileValidation = validateMobile(inputMobile, this.optionalMobile);
    if (!mobileValidation.isValid) {
      trackMobileErr(mobileValidation.errMessage, formType.FORM);
    }

    this.setState({
      validation: {
        ...this.state.validation,
        mobile: mobileValidation
      },
      buyerInfo: { ...this.state.buyerInfo, mobile: inputMobile }
    });
  };

  handleEmailChange = event => {
    let trimEmail = event.target.value.trim();
    let emailValidation = validateEmail(trimEmail, this.optionalEmail);
    if (!emailValidation.isValid) {
      trackEmailErr(emailValidation.errMessage, formType.FORM);
    }

    this.setState({
      buyerInfo: { ...this.state.buyerInfo, email: trimEmail },
      validation: {
        ...this.state.validation,
        email: emailValidation
      }
    });
  };

  handleCityDropDownChange = event => {
    let selectedCityId = event.id;
    let cityValidation = validateCity(this.optionalCity, selectedCityId);
    this.setState({
      assignedDealerId: "-1",
      dealerSelectItems: [],
      showSeller: false,
      cityId: event.id,
      cityName: event.name,
      validation: {
        ...this.state.validation,
        city: cityValidation
      }
    });

    //on city change, get mla data for the selected city
    this.getMLAData(selectedCityId);
  };

  handleDealerDropDownChange = event => {
    this.setState({
      assignedDealerId: event.id,
      validation: {
        ...this.state.validation,
        dealer: {
          isValid: true,
          errMessage: ""
        }
      }
    });
  };

  handleTestDriveChange = () => {
    this.setState({
      testDriveChecked: !this.state.testDriveChecked
    });
  };

  trackLeadConversion() {
    let leadConversionTracking = window.leadConversionTracking;
    if (leadConversionTracking) {
      let { dealerId } = this.props.campaign.campaign;
      let { propId } = store.getState().leadClickSource;
      leadConversionTracking.track(propId, dealerId);
    }
  }

  handleSubmitClick = () => {
    let {
      buyerInfo,
      showLocation,
      showSeller,
      cityId,
      cityName,
      assignedDealerId,
      testDriveChecked
    } = this.state;

    let validName = this.state.validation.name,
      validMobile = this.state.validation.mobile,
      validEmail = this.state.validation.email,
      validCity = this.state.validation.city,
      validDealer = this.state.validation.dealer;

    const modelDetail = this.props.modelDetail;

    const formScreenState = {
      buyerInfo,
      cityId,
      cityName,
      assignedDealerId,
      testDriveChecked,
      modelDetail,
      showLocation
    };
    validName = validateName(buyerInfo.name, this.optionalName);
    if (!validName.isValid) {
      trackNameErr(validName.errMessage, formType.FORM);
    }

    validMobile = validateMobile(buyerInfo.mobile, this.optionalMobile);
    if (!validMobile.isValid) {
      trackMobileErr(validMobile.errMessage, formType.FORM);
    }

    if (this.props.campaign.campaign.isEmailRequired) {
      validEmail = validateEmail(buyerInfo.email, this.optionalEmail);
      if (!validEmail.isValid) {
        trackEmailErr(validEmail.errMessage, formType.FORM);
      }
    }

    if (showLocation && cityId < 0) {
      validCity = validateCity(this.optionalCity, cityId);
    } else if (showSeller && assignedDealerId == "-1") {
      validDealer = validateDealer(this.optionalDealer);
    } else if (
      validName.isValid &&
      validMobile.isValid &&
      (!this.props.campaign.campaign.isEmailRequired || validEmail.isValid)
    ) {
      this.category = `LeadForm-${this.platformName}-${this.pageName}`;
      this.action = "FormOpen-Submit";
      this.label = this.props.modelDetail.modelId;

      if (this.cityAPIRequest != null) {
        this.cityAPIRequest.cancel();
        this.cityAPIRequest = null;
      }
      if (this.dealerAPIRequest != null) {
        this.dealerAPIRequest.cancel();
        this.dealerAPIRequest = null;
      }
      this.setState({
        isDisabled: true
      });
      this.props.setFormLead(formScreenState);

      if (showLocation) {
        this.props.setCityChange(true);
      }

      this.trackLeadConversion();
      fireInteractiveTracking(this.category, this.action, this.label);
    }

    if (
      !validName.isValid ||
      !validMobile.isValid ||
      !validEmail.isValid ||
      !validDealer.isValid ||
      !validCity.isValid
    ) {
      this.setState({
        validation: {
          name: validName,
          mobile: validMobile,
          email: validEmail,
          dealer: validDealer,
          city: validCity
        }
      });
    }
  };

  handleTracking = action => {
    fireInteractiveTracking(this.category, action, "");
  };

  render() {
    let {
      validation,
      citySelectItems,
      dealerSelectItems,
      showLocation,
      showSeller,
      isDisabled,
      testDriveChecked
    } = this.state;
    let { buyerInfo } = this.props;
    let { campaign, campaignType, featuredCarData } = this.props.campaign;
    let fullCarName, imageUrl;
    let isCrossSell = campaignType == 2 || campaignType == 3;
    if (isCrossSell) {
      imageUrl = `${featuredCarData.hostUrl}310x174${
        featuredCarData.originalImgPath
      }`;
      fullCarName = `${featuredCarData.makeName} ${featuredCarData.modelName}`;
    }
    return (
      <div className="customer-info-form-wrapper">
        <Form className="lead-popup-form">
          <FormScreenHeader
            isCrossSell={isCrossSell}
            imageUrl={imageUrl}
            campaignName={campaign.contactName}
            fullCarName={fullCarName}
          />
          <FormItem
            validateStatus={validation.name.isValid ? "" : "error"}
            helperText={validation.name.errMessage}
          >
            <Input
              label="Name"
              placeholder="Name"
              defaultValue={buyerInfo.name}
              onBlur={this.handleNameChange}
              onFocus={this.handleTracking.bind(this, "FormOpen-Name_Focus")}
            />
          </FormItem>
          <FormItem
            validateStatus={validation.mobile.isValid ? "" : "error"}
            helperText={validation.mobile.errMessage}
          >
            <Input
              label="Contact Number"
              defaultValue={buyerInfo.mobile}
              type="tel"
              placeholder="Contact Number"
              onBlur={this.handleMobileChange}
              onFocus={this.handleTracking.bind(this, "FormOpen-Mobile_Focus")}
              prefix="+91"
              maxLength={10}
            />
          </FormItem>
          {campaign.isEmailRequired && (
            <FormItem
              validateStatus={validation.email.isValid ? "" : "error"}
              helperText={validation.email.errMessage}
            >
              <Input
                label="Email-Id"
                defaultValue={buyerInfo.email}
                placeholder="Email"
                onBlur={this.handleEmailChange}
                onFocus={this.handleTracking.bind(this, "FormOpen-Email_Focus")}
              />
            </FormItem>
          )}
          {showLocation && (
            <FormItem
              validateStatus={validation.city.isValid ? "" : "error"}
              helperText={validation.city.errMessage}
            >
              <CityDropdown
                placeholder="Select City"
                label="City"
                optionValue="id"
                optionLabel="name"
                options={citySelectItems}
                onChange={this.handleCityDropDownChange}
              />
            </FormItem>
          )}
          {showSeller && (
            <FormItem
              validateStatus={validation.dealer.isValid ? "" : "error"}
              helperText={validation.dealer.errMessage}
            >
              <Select
                placeholder="Select Your Nearest Dealership"
                label="Dealership"
                optionValue="id"
                optionLabel="name"
                options={dealerSelectItems}
                onChange={this.handleDealerDropDownChange}
              />
            </FormItem>
          )}
          {campaign.isTestDriveCampaign && (
            <FormItem>
              <Checkbox
                name="test-drive"
                onChange={this.handleTestDriveChange}
                className="book-test-drive"
                checked={testDriveChecked}
              >
                I'm also interested in taking a test drive
              </Checkbox>
              {testDriveChecked && (
                <p className="book-test-drive-disclaimer">
                  A CarWale executive will call you and assist you with
                  scheduling your test drive
                </p>
              )}
            </FormItem>
          )}
          <FormItem>
            <Button
              type="primary"
              disabled={isDisabled}
              onClick={this.handleSubmitClick}
            >
              {isDisabled ? "Processing..." : "Submit"}
            </Button>
          </FormItem>
        </Form>
        <div className="user-agreement">
          <span>
            By proceeding ahead you agree to CarWale{" "}
            <a href="/visitoragreement.aspx">visitor agreement </a>
            <span className="privacy-policy">
              and <a href="/privacypolicy.aspx">privacy policy</a>
            </span>
          </span>
        </div>
      </div>
    );
  }
}

/**
 * Maps state in store
 * to props of the container
 */
function mapStateToProps(state) {
  const { page } = state.leadClickSource;
  const { buyerInfo } = state.NC.leadForm;
  const { campaign } = state.NC;
  const { location, isLeadFormVisible } = state;
  return { buyerInfo, campaign, location, page, isLeadFormVisible };
}

/**
 * Maps dispatch of bound actioncreators to
 * props of the container
 */
function mapDispatchToProps(dispatch) {
  return bindActionCreators(
    {
      setFormLead,
      setInfoLog,
      setErrorLog,
      setDebugLog,
      setMLAData,
      setCityChange
    },
    dispatch
  );
}

export default connect(
  mapStateToProps,
  mapDispatchToProps
)(FormScreen);
