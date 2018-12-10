import React from "react";
import { connect } from "react-redux";
import PropTypes from "prop-types";
import DownpaymentSliders from "../components/DownpaymentSliders";
import TenureSlider from "../components/TenureSlider";
import InterestSlider from "../components/InterestSlider";
import LeadFormCTA from "GlobalComponent/LeadFormCTA";

import { getModelData } from "../utils/Prices";

const propTypes = {
  isTitleVisible: PropTypes.bool
};

const defaultProps = {
  isTitleVisible: false
};

class emiSlider extends React.Component {
  constructor(props) {
    super(props);
    this.dealerCta = "";
  }
  showHideCampaign() {
    if (
      this.props.campaignDetails.isCampaignAvailable === null ||
      this.props.campaignDetails.isCampaignAvailable === "false"
    ) {
      $(".dealer-cta-div").hide();
    } else {
      $(".dealer-cta-div").show();
    }
  }
  selectDealerCta() {
    if (
      this.props.campaignDetails &&
      this.props.campaignDetails.isCampaignAvailable === "true" &&
      this.props.reactCampaignCta === "true"
    ) {
      const {
        userLocation,
        modelId,
        versionId,
        campaignDealerId
      } = this.props.campaignDetails;
      this.dealerCta = (
        <div className="campaign-cta-container dealer-cta-container leadFormCTA">
          <LeadFormCTA
            type="secondary"
            ctaText="Get EMI Offers"
            userLocation={userLocation}
            modelId={Number(modelId)}
            versionId={Number(versionId)}
            campaignDealerId={campaignDealerId}
            propId={21}
          />
        </div>
      );
    } else {
      this.dealerCta = (
        <div
          dangerouslySetInnerHTML={{
            __html: this.props.campaignTemplate.htmlString
          }}
        />
      );
    }
  }
  componentDidMount() {
    this.showHideCampaign();
  }
  componentDidUpdate() {
    let dealerCtaElement = $(".emi-calculator");
    let ctaButton = dealerCtaElement.find(".dealer-cta__btn");
    let emiCalculatorTemplateClick = $(".emi-calculator .dealer-cta__btn");
    let leadClickSourceId = this.props.campaignTemplate.leadClickSource;
    if (!this.props.reactCampaignCta || this.props.reactCampaignCta != "true") {
      $.each(emiCalculatorTemplateClick, function(index, item) {
        setCTALeadFormAttribute(item, leadClickSourceId);
      });
    }
    ctaButton.addClass("emi-calculator");
    this.showHideCampaign();
  }
  render() {
    this.selectDealerCta();
    return (
      <div className="emi-slider-body">
        <div className="emi-slider-content">
          {this.props.isTitleVisible && (
            <p className="emi-slider-body__title">Calculate your EMI</p>
          )}
          <div className="emi-slider-container">
            <DownpaymentSliders />
          </div>
          <div className="tenure-interest-slider">
            <TenureSlider changeTenureValue={this.props.changeTenureValue} />
            <InterestSlider />
          </div>
        </div>
        <div className="dealer-cta-div">
          <section className="js-dealer-cta emi-calculator">
            {this.dealerCta}
          </section>
        </div>
      </div>
    );
  }
}

const mapStateToProps = state => {
  let activeModel = getModelData(state.newEmiPrices);
  let {
    campaignTemplate,
    campaignDetails,
    reactCampaignCta
  } = activeModel.data;

  return {
    activeModel,
    campaignTemplate,
    campaignDetails,
    reactCampaignCta
  };
};

emiSlider.propTypes = propTypes;
emiSlider.defaultProps = defaultProps;

export default connect(
  mapStateToProps,
  null
)(emiSlider);
