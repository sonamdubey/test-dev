import React, { Component } from "react";

import { connect } from "react-redux";
import { bindActionCreators } from "redux";
import shallowequal from "shallowequal";

import Button from "oxygen/lib/Button/Button";
import Checkbox from "oxygen/lib/Checkbox/Checkbox";
import { CheckboxGroup } from "oxygen/lib/Checkbox";

import ThankYouHeader from "../Components/ThankYouHeader";
import { setInfoLog, setDebugLog } from "../ActionCreators/LogState";
import { setRecoLead } from "../ActionCreators/RecoState";
import { validateButton } from "../utils/Validate";
import { jsScreenClass, setScreenBodyHeight } from "../utils/ScreenHeight";
import store from "../store";
import {
  trackSubmit,
  trackCheckItem,
  trackSelectAll,
  trackScreenShown
} from "../utils/Tracking";

class RecoScreen extends Component {
  constructor(props) {
    super(props);
    this.optionalReco = false;

    this.state = {
      selectedReco: [],
      selectAll: false
    };

    this.recoDictionary = this.props.recommendation.list;
    this.recoOptions = Object.keys(this.recoDictionary).map(recoItem => {
      let suggestedCarTitle = this.getTitle(recoItem);
      return {
        value: recoItem,
        label: (
          <div>
            <div className="suggested-car__image">
              <img
                src={`${this.recoDictionary[recoItem].carDetail.hostUrl}110X61${
                  this.recoDictionary[recoItem].carDetail.originalImgPath
                }`}
              />
            </div>
            <div className="suggested-cars-info">
              <span className="suggested-cars-info__heading">
                {`${this.recoDictionary[recoItem].carDetail.makeName} ${
                  this.recoDictionary[recoItem].carDetail.modelName
                }`}
              </span>
              <div className="suggested-cars__amount">
                {this.recoDictionary[recoItem].priceOverview.price}
              </div>
              <p className="suggested-cars-info__title">{suggestedCarTitle}</p>
              <p className="suggested-cars-info__subtitle">
                {this.recoDictionary[recoItem].campaign.name}
              </p>
            </div>
          </div>
        )
      };
    });
  }

  getTitle = index => {
    let state = store.getState();
    if (this.recoDictionary[index].priceOverview.priceStatus == 0) {
      return "NA";
    } else if (this.recoDictionary[index].priceOverview.priceStatus == 1) {
      return `On-Road ${state.location.cityName}`;
    } else if (this.recoDictionary[index].priceOverview.priceStatus == 2) {
      return "Avg. Ex-Showroom";
    }
  };

  componentDidMount() {
    trackScreenShown(this.recoOptions.length);
    setScreenBodyHeight(this.recommendationContainer);

    this.props.setInfoLog(
      "Entering RecoScreen (Current Store data Provided in currentState)"
    );
  }

  shouldComponentUpdate(nextProps, nextState) {
    if (!shallowequal(this.state.selectedReco, nextState.selectedReco)) {
      return true;
    }
    return false;
  }

  handleSubmit = () => {
    let message =
      "Submit button clicked with Selected Recommendation data on RecoScreen";
    this.props.setDebugLog(message, this.state.selectedReco, {});

    trackSubmit(this.state.selectedReco.length, this.recoOptions.length);

    this.props.setRecoLead(this.state.selectedReco);
  };

  handleCheckedReco = event => {
    let cloneSelectedReco = this.state.selectedReco.slice(0);
    let cloneEvent = event.slice(0);
    trackCheckItem(cloneSelectedReco, cloneEvent, this.recoDictionary);

    this.setState({
      selectedReco: event,
      selectAll: event.length == this.recoOptions.length ? true : false
    });
  };

  handleSelectAllChange = () => {
    trackSelectAll(this.state.selectAll, this.recoOptions.length);

    this.setState({
      selectedReco: !this.state.selectAll
        ? Object.keys(this.recoDictionary)
        : [],
      selectAll: !this.state.selectAll
    });
  };

  setRecommendationRef = ref => {
    this.recommendationContainer = ref;
  };

  render() {
    return (
      <div className={jsScreenClass.container} ref={this.setRecommendationRef}>
        <ThankYouHeader
          name={this.props.buyerInfo.name}
          className={jsScreenClass.header}
        />
        <div className={`additional-suggested-list ${jsScreenClass.body}`}>
          <div className="padding-bottom20">
            <div className="list-heading">
              <h2 className="list-heading__title">Suggested Cars</h2>
              <p className="list-heading__subtitle">Based on your research</p>
            </div>
            <div className="select-all-option">
              <Checkbox
                name="Select All"
                checked={this.state.selectAll}
                onChange={this.handleSelectAllChange}
                alignIcon="right"
              >
                {!this.state.selectAll ? "Select All" : "Remove All"}
              </Checkbox>
            </div>
          </div>
          <CheckboxGroup
            options={this.recoOptions}
            name="Suggested Cars"
            value={this.state.selectedReco}
            className="suggested-list"
            alignIcon="right"
            onChange={this.handleCheckedReco}
          />
        </div>
        <div
          className={`content-wrapper suggested-list-footer ${
            jsScreenClass.footer
          }`}
        >
          <Button
            type="primary"
            disabled={validateButton(
              this.optionalReco,
              this.state.selectedReco.length
            )}
            onClick={this.handleSubmit}
          >
            Request Assistance
          </Button>
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
  const { buyerInfo, recommendation } = state.NC.leadForm;
  return { buyerInfo, recommendation, page };
}

function mapDispatchToProps(dispatch) {
  return bindActionCreators({ setInfoLog, setDebugLog, setRecoLead }, dispatch);
}

export default connect(
  mapStateToProps,
  mapDispatchToProps
)(RecoScreen);
