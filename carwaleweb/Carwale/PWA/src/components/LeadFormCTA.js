import React, { PureComponent } from "react";
import PropTypes from "prop-types";

import Button from "oxygen/lib/Button";

const propTypes = {
  // If 'true', Lead Form opens
  campaignCta: PropTypes.oneOf(["true", "false"]),
  // To contain JSON object having cityId and cityName
  userLocation: PropTypes.string,
  // Model Id for the campaign
  modelId: PropTypes.number,
  // Version Id for the campaign
  versionId: PropTypes.number,
  // Key as CampaignId_DealerId for the campaign
  campaignDealerId: PropTypes.string,
  // Property Id
  propId: PropTypes.number,
  // If 'true', checkbox will be checked for test drive
  testDriveChecked: PropTypes.oneOf(["true", "false"]),
  // Text to be shown on CTA
  ctaText: PropTypes.string
};

const defaultProps = {
  campaignCta: "true",
  userLocation: "",
  modelId: 0,
  versionId: 0,
  campaignDealerId: "0",
  propId: 0,
  testDriveChecked: "false",
  ctaText: "Get Offers"
};

class LeadFormCTA extends PureComponent {
  render() {
    const {
      campaignCta,
      userLocation,
      modelId,
      versionId,
      campaignDealerId,
      propId,
      testDriveChecked,
      ctaText,
      // Check https://github.com/carwale/oxygen/tree/master/src/Button for different formatting options
      ...buttonFormatting
    } = this.props;

    return (
      <Button
        {...buttonFormatting}
        campaigncta={campaignCta}
        userlocation={userLocation}
        modelid={modelId}
        propertyid={propId}
        versionid={versionId}
        campaignid_dealerid={campaignDealerId}
        testdrivechecked={testDriveChecked}
      >
        {ctaText}
      </Button>
    );
  }
}

LeadFormCTA.propTypes = propTypes;
LeadFormCTA.defaultProps = defaultProps;

export default LeadFormCTA;
