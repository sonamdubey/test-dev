import React, { PureComponent } from "react";
import PropTypes from "prop-types";

class FormScreenHeader extends PureComponent {
  render() {
    return (
      <div>
        {this.props.isCrossSell ? (
          <div className="dealer-detail">
            <p className="dealer-detail__heading">
              <span className="text-unbold">
                Get assistance from{" "}
                <span className="text-bold">{this.props.campaignName}</span> for
              </span>
            </p>
            <div className="car-img-fold">
              <img
                className="pq-m-car-img"
                src={this.props.imageUrl}
                alt={this.props.fullCarName}
              />
              <p className="pq-m-car-name">{this.props.fullCarName}</p>
            </div>
          </div>
        ) : (
          <div className="dealer-detail">
            <p className="dealer-detail__heading">{this.props.campaignName}</p>
            <p className="dealer-detail__subheading">
              Provide your contact details for further communication related to
              Test Drive, EMI options, Offers &amp; Exchange Benefits
            </p>
          </div>
        )}
      </div>
    );
  }
}

FormScreenHeader.propTypes = {
  isCrossSell: PropTypes.bool,
  campaignName: PropTypes.string,
  modelName: PropTypes.string,
  imageUrl: PropTypes.string
};

export default FormScreenHeader;
