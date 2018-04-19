import React from 'react';

import ProgressBarItem from './ProgressBarItem'

class ProgressBar extends React.Component {
  constructor(props) {
    super(props);
    this.setState({
      bike: 1,  // here 1: active; 2: disabled; 3: done
      city: 2
    });
  }

  render() {
    return(
      <div className="progress-bar">
      <ProgressBarItem childClassName="head-item__bike-selection" id="1" status={this.state.bike}>
          Select bike
      </ProgressBarItem>
      <ProgressBarItem childClassName="head-item__city-selection" id="2" status={this.state.city}>
          Select city
      </ProgressBarItem>
    </div>

    );
  }
}

export default ProgressBar