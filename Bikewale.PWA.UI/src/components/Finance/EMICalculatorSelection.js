import React from 'react';
import ProgressBar from './ProgressBar'
import ProgressBarItem from './ProgressBarItem'

class EMICalculatorSelection extends React.Component {
  constructor(props) {
    super(props);
    this.setState({
      bike: 2,  // here 1: disabled; 2: active; 3: done
      city: 1
    });
  }

  render() {
    return (
      <div className="emi-calculator__progress-container">
        <ProgressBar>
          <ProgressBarItem id={1} status={this.state.bike}>
              Select bike
          </ProgressBarItem>
          <ProgressBarItem id={2} status={this.state.city}>
              Select city
          </ProgressBarItem>
        </ProgressBar>
      </div>
    );
  }
}

export default EMICalculatorSelection