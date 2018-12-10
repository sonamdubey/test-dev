import React, { PureComponent } from "react";
import PropTypes from "prop-types";
import { submitBhriguLogs } from "../utils/BhriguSubmission";
import { clearLog } from "../ActionCreators/LogState";
import store from "../store";

class SorryScreen extends PureComponent {
  componentDidMount() {
    this.props.setLog();
    submitBhriguLogs(store.getState().log);
    store.dispatch(clearLog());
  }

  render() {
    return (
      <div className="sorry-screen-div">
        <div className="location-popup__sorry-icon-wrapper">
          <span className="location-popup-icon location-popup__sorry-icon" />
        </div>
        <p className="sorry-screen__response">
          {"We couldn't find any dealerships in your city"}
        </p>
      </div>
    );
  }
}

SorryScreen.propTypes = {
  setLog: PropTypes.func
};

export default SorryScreen;
