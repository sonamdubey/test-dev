import React, { Component } from "react";
import { connect } from "react-redux";
import { bindActionCreators } from "redux";
import shallowequal from "shallowequal";

import Form from "oxygen/lib/Form";
import Input from "oxygen/lib/Input";
import Button from "oxygen/lib/Button/Button";

import ThankYouHeader from "../Components/ThankYouHeader";
import ThankYouImage from "../Components/ThankYouImage";
import { setInfoLog, clearLog } from "../ActionCreators/LogState";
import { setThankYouLead } from "../ActionCreators/ThankYouState";
import { setHide } from "../ActionCreators/ScreenState";
import { validateEmail } from "../utils/Validate";
import { platform } from "../../enum/Platform";
import { submitBhriguLogs } from "../utils/BhriguSubmission";
import store from "../store";

const FormItem = Form.Item;

class ThankYouScreen extends Component {
  constructor(props) {
    super(props);
    this.optionalEmail = true;

    this.showEmail = this.props.buyerInfo.email == "" ? true : false;
    this.state = {
      email: "",
      validEmail: {
        isValid: true,
        errMessage: ""
      }
    };
  }

  componentDidMount() {
    this.props.setInfoLog(
      "Entering ThankYouScreen (Current Store data Provided in currentState)"
    );
    submitBhriguLogs(store.getState().log);
    store.dispatch(clearLog());
  }

  shouldComponentUpdate(nextProps, nextState) {
    if (!shallowequal(this.state.validEmail, nextState.validEmail)) {
      return true;
    }
    return false;
  }

  handleEmailChange = event => {
    let trimEmail = event.target.value.trim();
    this.setState({
      email: trimEmail,
      validEmail: validateEmail(trimEmail, this.optionalEmail)
    });
  };

  handleDone = () => {
    //Not validating email if its blank
    if (this.state.email != "" && this.state.validEmail.isValid) {
      this.props.setThankYouLead(this.state.email);
    }

    if (this.state.email == "" || this.state.validEmail.isValid) {
      let platformId = this.props.page.platform.id;
      this.props.setHide();
      if (platformId != platform.DESKTOP.id) {
        window.history.back();
      }
    }
  };

  render() {
    return (
      <div>
        {this.showEmail ? (
          <Form className="lead-popup-form">
            <ThankYouHeader name={this.props.buyerInfo.name} />
            <div className="email-screen">
              <p className="email-screen__header">
                Meanwhile you can provide your e-mail ID to recieve the
                price-list, brochure and other collaterals.
              </p>
              <FormItem
                validateStatus={this.state.validEmail.isValid ? "" : "error"}
                helperText={this.state.validEmail.errMessage}
              >
                <Input
                  id="email"
                  label="Email-Id"
                  placeholder="Email"
                  onBlur={this.handleEmailChange}
                />
              </FormItem>
              <FormItem>
                <Button type="primary" onClick={this.handleDone}>
                  Done
                </Button>
              </FormItem>
            </div>
          </Form>
        ) : (
          <ThankYouImage />
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
  const { buyerInfo } = state.NC.leadForm;
  const { page } = state.leadClickSource;
  const { location } = state;
  return { buyerInfo, location, page };
}

function mapDispatchToProps(dispatch) {
  return bindActionCreators(
    { setInfoLog, setThankYouLead, setHide, clearLog },
    dispatch
  );
}

export default connect(
  mapStateToProps,
  mapDispatchToProps
)(ThankYouScreen);
