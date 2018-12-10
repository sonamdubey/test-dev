import React, { Component } from "react";
import { bindActionCreators } from "redux";
import { connect } from "react-redux";
import shallowequal from "shallowequal";

import Form from "oxygen/lib/Form";
import Input from "oxygen/lib/Input";
import Button from "oxygen/lib/Button";

import FormScreenHeader from "../Components/FormScreenHeader";
import store from "../store";
import { setWhatsAppFormLead } from "../ActionCreators/WhatsAppFormState";
import { setDebugLog, setInfoLog } from "../ActionCreators/LogState";
import { validateMobile, validateName } from "../utils/Validate";
import { fireInteractiveTracking } from "../../utils/Analytics";
import { trackNameErr, trackMobileErr } from "../utils/Tracking";
import { formType } from "../Enum/FormType";

const FormItem = Form.Item;

export class WhatsAppFormScreen extends Component {
  constructor(props) {
    super(props);
    const { location, buyerInfo } = this.props;
    const { page } = this.props;
    (this.optionalName = false), (this.optionalMobile = false);

    this.state = {
      buyerInfo: {
        name: buyerInfo.name,
        mobile: buyerInfo.mobile
      },
      cityId: location.cityId,
      validation: {
        name: {
          isValid: true,
          errMessage: ""
        },
        mobile: {
          isValid: true,
          errMessage: ""
        }
      }
    };

    this.category = `LeadForm-${page.platform.name}-${page.page.name}`;
  }

  componentDidMount() {
    this.campaignId = this.props.campaign.campaign.id;
    let message = "Entering WhatsAppFormScreen";
    this.props.setInfoLog(message);
  }

  componentDidUpdate() {
    if (this.props.campaign.campaign.id != this.campaignId) {
      this.campaignId = this.props.campaign.campaign.id;
      let message = "Re-Entering WhatsAppFormScreen";
      this.props.setInfoLog(message);
    }
  }

  shouldComponentUpdate(nextProps, nextState) {
    if (this.props.isLeadFormVisible != nextProps.isLeadFormVisible) {
      this.resetErrorState();
    }

    let { campaign } = this.props.campaign;
    // Log when name or mobile is changed
    if (
      this.state.buyerInfo.name != nextState.buyerInfo.name ||
      this.state.buyerInfo.mobile != nextState.buyerInfo.mobile
    ) {
      let message = "Name or Mobile Changed";
      this.props.setDebugLog(message, this.state, nextState);
    }

    if (
      nextProps.campaign.campaign.id != campaign.id ||
      nextProps.campaign.campaign.dealerId != campaign.dealerId
    ) {
      let message =
        "Updated Campaign shown (Current Store data Provided in currentState)";
      this.props.setInfoLog(message);
    }
    if (
      nextProps.campaign.campaign.id != campaign.id ||
      nextProps.campaign.campaign.dealerId != campaign.dealerId
    ) {
      return true;
    }
    if (!shallowequal(this.state.validation, nextState.validation)) {
      return true;
    }
    return false;
  }

  resetErrorState() {
    let validationObj = {
      name: {
        isValid: true,
        errMessage: ""
      },
      mobile: {
        isValid: true,
        errMessage: ""
      }
    };
    this.setState({ validation: validationObj });
  }

  handleNameChange = event => {
    let inputName = event.target.value;
    let nameValidation = validateName(inputName, this.optionalName);
    if (!nameValidation.isValid) {
      trackNameErr(nameValidation.errMessage, formType.WHATSAPP_FORM);
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
      trackMobileErr(mobileValidation.errMessage, formType.WHATSAPP_FORM);
    }

    this.setState({
      validation: {
        ...this.state.validation,
        mobile: mobileValidation
      },
      buyerInfo: { ...this.state.buyerInfo, mobile: inputMobile }
    });
  };

  handleTracking = action => {
    fireInteractiveTracking(this.category, action, "");
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
    let { buyerInfo } = this.state;

    let validName = this.state.validation.name,
      validMobile = this.state.validation.mobile;

    const modelDetail = this.props.modelDetail;

    const formScreenState = {
      buyerInfo,
      modelDetail
    };
    validName = validateName(buyerInfo.name, this.optionalName);
    if (!validName.isValid) {
      trackNameErr(validName.errMessage, formType.WHATSAPP_FORM);
    }

    validMobile = validateMobile(buyerInfo.mobile, this.optionalMobile);
    if (!validMobile.isValid) {
      trackMobileErr(validMobile.errMessage, formType.WHATSAPP_FORM);
    }

    if (validName.isValid && validMobile.isValid) {
      let action = "WhatsAppFormOpen-Submit";
      let label = this.props.modelDetail.modelId;

      this.props.setWhatsAppFormLead(formScreenState);

      this.trackLeadConversion();
      fireInteractiveTracking(this.category, action, label);
    }

    if (!validName.isValid || !validMobile.isValid) {
      this.setState({
        validation: {
          name: validName,
          mobile: validMobile
        }
      });
    }
  };

  render() {
    let { validation } = this.state;
    let { buyerInfo } = this.props;
    let { campaign } = this.props.campaign;

    return (
      <div className="customer-info-form-wrapper">
        <Form className="lead-popup-form">
          <FormScreenHeader
            isCrossSell={false}
            campaignName={campaign.contactName}
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
              onFocus={this.handleTracking.bind(
                this,
                "WhatsAppFormOpen-Name_Focus"
              )}
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
              onFocus={this.handleTracking.bind(
                this,
                "WhatsAppFormOpen-Mobile_Focus"
              )}
              prefix="+91"
              maxLength={10}
            />
          </FormItem>
          <FormItem>
            <Button
              type="secondary"
              onClick={this.handleSubmitClick}
              ghost={true}
            >
              <span>
                <img
                  className="whatsapp-icon"
                  src="https://imgd.aeplcdn.com/0x0/cw/static/icons/svg/00b2a0/whatsapp-icon.svg"
                />
                Start Chat
              </span>
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
      setWhatsAppFormLead,
      setInfoLog,
      setDebugLog
    },
    dispatch
  );
}

export default connect(
  mapStateToProps,
  mapDispatchToProps
)(WhatsAppFormScreen);
