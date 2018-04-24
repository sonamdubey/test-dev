import React from 'react';
import ProgressBar from './ProgressBar'
import ProgressBarItem from './ProgressBarItem'
import ModelCard from '../Shared/ModelCard'

class EMISteps extends React.Component {
  constructor(props) {
    super(props);
    this.setState({
      bike: 2,  // here 1: disabled; 2: active; 3: done
      city: 1
    });
  }

  render() {
    const modelData = {
      makeName: "Royal Enfield",
      modelName: "Thunderbird 350",
      modelImage: "https://imgd.aeplcdn.com//310x174//bw/models/honda-cb-hornet-160r.jpg",
      rating: 4.2
    }
    return (
      <div className="emi-calculator__progress-container">
        <ProgressBar>
          <ProgressBarItem stepNumber={1} status={this.state.bike}>
              Select bike
          </ProgressBarItem>
          <ProgressBarItem stepNumber={2} status={this.state.city}>
              Select city
          </ProgressBarItem>
        </ProgressBar>
        <div className="select-bike__model-card">
          <ModelCard model={modelData} />
        </div>
      </div>
    );
  }
}

export default EMISteps