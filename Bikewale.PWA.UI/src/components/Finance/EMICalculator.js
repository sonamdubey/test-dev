import React from 'react';

import SelectBikePopup from '../Shared/SelectBikePopup'

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
        <h2>EMI Calculator</h2>
        <span onClick={this.handleSelectBikeClick}>Select bike</span>
        <SelectBikePopup isActive={selectBikePopupStatus} />
      </div>
    );
  }
}

export default EMICalculator;