import React from 'react';
import PropTypes from 'prop-types';
import { trackForMobile, trackingActionType } from '../utils/valuationTracking'

const defaultProps = {
  valuationHtml: ''
}

class ValuationReportContainer extends React.Component {
  constructor(props) {
    super(props);
  }

  componentWillReceiveProps(nextProps) {
    let rightPriceBoxEle = document.getElementsByClassName("right-price-box");
    //console.log(rightPriceBoxEle[0].attributes)
    if (rightPriceBoxEle && rightPriceBoxEle.length > 0) {
      const caseId = rightPriceBoxEle[0].attributes['caseid'].value
      if (caseId) {
				if (caseId == 1) {
					trackForMobile(trackingActionType.valuationExactMatchLoad, '');
				}
				else if (caseId > 1 && caseId < 9) {
					trackForMobile(trackingActionType.valuationApproximateMatchLoad, '');
				}
				else {
					trackForMobile(trackingActionType.valuationNotAvailableLoad, '');
				}
			}
    }

  }

  render() {
    return (
      <div>
        <div className="report__car-detail">
          <div className="report-car-detail__content">
            <div dangerouslySetInnerHTML={{ __html: this.props.valuationHtml }} />
          </div>
        </div>
      </div>
    );
  }
}

ValuationReportContainer.defaultProps = defaultProps;

export default ValuationReportContainer

