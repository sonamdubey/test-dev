import React from 'react';

import SelectBikePopup from '../Shared/SelectBikePopup'
import ProgressContainer from './ProgressContainer'

class EMICalculator extends React.Component {
  constructor(props) {
    super(props);

    this.state = {
      selectBikePopupStatus: false
    }

    this.handleSelectBikeClick = this.handleSelectBikeClick.bind(this);
  }

  handleSelectBikeClick() {
    this.setState({
      selectBikePopupStatus: true
    })
  }

  render() {
    const {
      selectBikePopupStatus
    } = this.state

    return (
      <div>
        <div className="emi-calculator__head">
          <h2 className="emi-calculator__title">EMI Calculator</h2>
          <p className="emi-calculator__head-description">
            Know the tentative EMI for bike of your choice in 2 simple steps.
          </p>
        </div>
        <ProgressContainer />
        <span onClick={this.handleSelectBikeClick}>Select bike</span>
        <SelectBikePopup isActive={selectBikePopupStatus} />
      </div>
    );
  }
}

export default EMICalculator;