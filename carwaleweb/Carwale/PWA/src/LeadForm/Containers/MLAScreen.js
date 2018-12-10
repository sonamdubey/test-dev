import React, { Component } from "react";
import { connect } from "react-redux";
import { bindActionCreators } from "redux";
import shallowequal from "shallowequal";

import { CheckboxGroup } from "oxygen/lib/Checkbox";
import Button from "oxygen/lib/Button/Button";
import Checkbox from "oxygen/lib/Checkbox/Checkbox";

import ThankYouHeader from "../Components/ThankYouHeader";
import { setInfoLog, setDebugLog } from "../ActionCreators/LogState";
import { setMLALead } from "../ActionCreators/MLAState";
import { setRecoScreen } from "../ActionCreators/ScreenState";
import { validateButton } from "../utils/Validate";
import { jsScreenClass, setScreenBodyHeight } from "../utils/ScreenHeight";
import {
  trackSubmit,
  trackCheckItem,
  trackSelectAll,
  trackScreenShown,
  trackSkipClick,
  trackSkipShown
} from "../utils/Tracking";

class MLAScreen extends Component {
  constructor(props) {
    super(props);
    this.optionalMLA = false;

    let isTurboMla = this.props.campaign.isTurboMla;
    this.MLADictionary = this.props.MLASellers.list;
    this.state = {
      selectedMLA: isTurboMla ? Object.keys(this.MLADictionary) : [],
      selectAll: isTurboMla
    };

    this.isSkipTracked = false;

    this.MLAOptions = Object.keys(this.MLADictionary).map(MLAItem => {
      return {
        value: MLAItem,
        label: (
          <div>
            <span className="suggested-dealer-name">
              {this.MLADictionary[MLAItem].seller.name}
            </span>
            <div className="dealer-info-slug">
              {this.MLADictionary[MLAItem].seller.area != "" && (
                <span className="dealer-info__city-name">
                  {this.MLADictionary[MLAItem].seller.area}
                </span>
              )}
              {this.MLADictionary[MLAItem].seller.distance != -1 && (
                <span className="dealer-info__distance">
                  {this.MLADictionary[MLAItem].seller.distance} km away
                </span>
              )}
            </div>
          </div>
        )
      };
    });
  }

  shouldComponentUpdate(nextProps, nextState) {
    if (!shallowequal(this.state.selectedMLA, nextState.selectedMLA)) {
      return true;
    }
    if (
      !shallowequal(
        this.props.recommendation.list,
        nextProps.recommendation.list
      )
    ) {
      return true;
    }
    return false;
  }

  componentDidMount() {
    trackScreenShown(this.MLAOptions.length);
    setScreenBodyHeight(this.mlaContainer);

    this.props.setInfoLog(
      "Entering MLAScreen (Current Store data Provided in currentState)"
    );
  }

  handleCheckedMLA = event => {
    let cloneEvent = event.slice(0);
    let cloneSelectedMLA = this.state.selectedMLA.slice(0);
    trackCheckItem(cloneSelectedMLA, cloneEvent, this.MLADictionary);

    this.setState({
      selectedMLA: event,
      selectAll: event.length == this.MLAOptions.length ? true : false
    });
  };

  handleSelectAllChange = () => {
    trackSelectAll(this.state.selectAll, this.MLAOptions.length);
    this.setState({
      selectedMLA: !this.state.selectAll ? Object.keys(this.MLADictionary) : [],
      selectAll: !this.state.selectAll
    });
  };

  handleSubmit = () => {
    let message = "Submit button clicked with Selected MLA data on MLAScreen";
    this.props.setDebugLog(message, this.state.selectedMLA, {});

    trackSubmit(this.state.selectedMLA.length, this.MLAOptions.length);

    this.props.setMLALead(this.state.selectedMLA, this.props.modelDetail);
  };

  handleSkip = () => {
    let message =
      "Skip button clicked {Leaving MLA Screen} (Selected MLA data and current Store provided in currentState)";
    this.props.setInfoLog(message);
    trackSkipClick(this.MLAOptions.length);
    this.props.setRecoScreen();
  };

  setMLARef = ref => {
    this.mlaContainer = ref;
  };

  render() {
    let recoLength = Object.keys(this.props.recommendation.list).length;
    if (!this.isSkipTracked && recoLength > 0) {
      this.isSkipTracked = true;
      trackSkipShown(this.MLAOptions.length);
    }

    return (
      <div className={jsScreenClass.container} ref={this.setMLARef}>
        <ThankYouHeader
          name={this.props.buyerInfo.name}
          className={jsScreenClass.header}
        />
        <div className={`additional-suggested-list ${jsScreenClass.body}`}>
          <div className="padding-bottom20">
            <div className="list-heading">
              <h2 className="list-heading__title">Other Dealers</h2>
              <p className="list-heading__subtitle">
                Check offer from other dealers
              </p>
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
            options={this.MLAOptions}
            name="Other Dealers"
            value={this.state.selectedMLA}
            className="suggested-list"
            alignIcon="right"
            onChange={this.handleCheckedMLA}
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
              this.optionalMLA,
              this.state.selectedMLA.length
            )}
            onClick={this.handleSubmit}
          >
            Submit
          </Button>

          {Object.keys(this.props.recommendation.list).length > 0 && (
            <Button className="dealer-skip-button" onClick={this.handleSkip}>
              Skip
            </Button>
          )}
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
  const { campaign } = state.NC.campaign;
  const { buyerInfo, MLASellers, recommendation } = state.NC.leadForm;
  const { location } = state;
  return { buyerInfo, campaign, MLASellers, recommendation, location, page };
}

function mapDispatchToProps(dispatch) {
  return bindActionCreators(
    { setDebugLog, setInfoLog, setMLALead, setRecoScreen },
    dispatch
  );
}

export default connect(
  mapStateToProps,
  mapDispatchToProps
)(MLAScreen);
