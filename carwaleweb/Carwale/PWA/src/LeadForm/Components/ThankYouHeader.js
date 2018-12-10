import React, { PureComponent } from "react";
import PropTypes from "prop-types";

export class ThankYouHeader extends PureComponent {
  render() {
    const containerClass = `thankyou-header ${this.props.className}`;

    return (
      <div id="postsubmitmsg" className={containerClass}>
        <p className="thankyou-header__title">Thank You {this.props.name}!</p>
        <p className="thankyou-header__subtitle">
          A car consultant would get in touch with you shortly with assistance
          on your purchase.
        </p>
      </div>
    );
  }
}

ThankYouHeader.propTypes = {
  name: PropTypes.string
};

export default ThankYouHeader;
