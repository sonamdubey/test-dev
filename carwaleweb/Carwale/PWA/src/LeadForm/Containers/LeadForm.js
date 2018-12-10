import React, { Component } from "react";

import { connect } from "react-redux";
import { bindActionCreators } from "redux";

import FormScreen from "./FormScreen";
import LocationScreen from "./LocationScreen";
import MLAScreen from "./MLAScreen";
import RecoScreen from "./RecoScreen";
import ThankYouScreen from "./ThankYouScreen";
import WhatsAppFormScreen from "./WhatsAppFormScreen";
import { screenType } from "../Enum/ScreenEnum";
import SorryScreen from "../Components/SorryScreen";
import { setInfoLog, clearLog } from "../ActionCreators/LogState";
import { setHide } from "../ActionCreators/ScreenState";
import { platform } from "../../enum/Platform";
import { trackCloseClick } from "../utils/Tracking";

class LeadForm extends Component {
  constructor(props) {
    super(props);
    this.closeButton = "popup-close-btn popup-close-btn--black";
  }

  componentDidMount() {}

  popupCloseButton() {
    const { screenData, buyerInfo } = this.props;
    if (
      screenData.screen == screenType.MLA ||
      screenData.screen == screenType.Reco ||
      (screenData.screen == screenType.ThankYou && buyerInfo.email == "")
    ) {
      this.closeButton = "popup-close-btn popup-close-btn--white";
    } else {
      this.closeButton = "popup-close-btn popup-close-btn--black";
    }
  }

  setSorryScreenLog = () => {
    this.props.setInfoLog(
      "Entering SorryScreen (Current Store data Provided in currentState)"
    );
  };

  handleCloseClick = () => {
    trackCloseClick(this.props.modelDetail.modelId);
    this.props.setHide();

    if (this.props.page.platform.id != platform.DESKTOP.id) {
      window.history.back();
    }
  };

  shouldComponentUpdate(nextProps, nextState) {
    if (this.props.isLeadFormVisible != nextProps.isLeadFormVisible) {
      return true;
    }
    if (this.props.screenData.screen != nextProps.screenData.screen) {
      return true;
    }
    return false;
  }

  renderScreen() {
    const { screenData } = this.props;
    switch (screenData.screen) {
      case screenType.Location: {
        return <LocationScreen modelDetail={this.props.modelDetail} />;
      }
      case screenType.Form: {
        return (
          <FormScreen
            modelDetail={this.props.modelDetail}
            testDriveChecked={this.props.testDriveChecked}
          />
        );
      }
      case screenType.WhatsAppForm: {
        return <WhatsAppFormScreen modelDetail={this.props.modelDetail} />;
      }
      case screenType.MLA: {
        return <MLAScreen modelDetail={this.props.modelDetail} />;
      }
      case screenType.Reco: {
        return <RecoScreen />;
      }
      case screenType.ThankYou: {
        return <ThankYouScreen />;
      }
      case screenType.Sorry: {
        return <SorryScreen setLog={this.setSorryScreenLog} />;
      }
      default:
        return;
    }
  }

  render() {
    this.popupCloseButton();
    const { isLeadFormVisible } = this.props;
    const visibleClass = !isLeadFormVisible ? "" : "lead-popup--visible";
    return (
      <div className={visibleClass}>
        <div className="lead-popup lead-popup-wrapper">
          <span className={this.closeButton} onClick={this.handleCloseClick} />
          {this.renderScreen()}
        </div>
        {this.props.page.platform.id == platform.DESKTOP.id && (
          <div
            className="lead-popup-bg-window"
            onClick={this.handleCloseClick}
          />
        )}
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
  const { screenData, location, isLeadFormVisible } = state;
  return {
    screenData,
    buyerInfo,
    location,
    page,
    isLeadFormVisible
  };
}

function mapDispatchToProps(dispatch) {
  return bindActionCreators(
    {
      setHide,
      setInfoLog,
      clearLog
    },
    dispatch
  );
}

export default connect(
  mapStateToProps,
  mapDispatchToProps
)(LeadForm);
