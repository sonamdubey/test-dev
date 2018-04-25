import React from 'react';
import PropTypes from 'prop-types'

import ProgressBar from './ProgressBar'
import ProgressBarItem from './ProgressBarItem'
import ModelCard from '../Shared/ModelCard'
import SelectionStepCard from './SelectionStepCard';
import { 
  scrollIntoView,
   inView
  } from '../../utils/ScrollTo'


const propTypes = {
	// body type data
	handleSelectBikeClick: PropTypes.func,
	// body type count
	handleSelectCityClick: PropTypes.func
}

const defaultProps = {
	handleSelectBikeClick: null,
	handleSelectCityClick: null
}

class EMISteps extends React.Component {
  constructor(props) {
    super(props);
    this.setState({
      bike: 2,  // here 1: disabled; 2: active; 3: done
      city: 1
    });
  }

  renderMakeCard = (modelData) =>{
    if(modelData.makeName != '') {
      return (
        <div className="select-step-card__placeholder select-card__make-selection" onClick={this.handleMakeSelect} >
          <span className="select-step-card__link">
            Search Make and Model
            <span className="selection-step__right-arrow"></span>
          </span>
        </div>
      )
    }
    else {
      return (
        <ModelCard model={modelData} />
      )
    }
  }

  renderCityCard =(isOverflow, handleSelectCityClick) => {
    if(isOverflow) {
      return(
        <SelectionStepCard>
          <div ref={this.setCitySelectRef} className="select-step-card__placeholder select-card__city-selection" onClick={this.props.handleSelectCityClick}>
            <span className="select-step-card__link">
                Select your City
                <span className="selection-step__right-arrow"></span>
              </span>
          </div>
        </SelectionStepCard>
      )
    }
  }
  setSelectionContainerRef = (element) => {
    this.StepSelectionContainer = element;
  };
  setSelectionStepsRef = (element) => {
      this.StepSelection = element;
  };
  setCitySelectRef = (element) => {
    this.citySelect = element;
  };
  handleMakeSelect = () => {
    // {this.props.handleSelectBikeClick()}
    scrollIntoView(this.StepSelectionContainer, this.citySelect)
  }
  handleScroll = (event) => {
    let CurrentTarget = event.currentTarget;
    if(CurrentTarget.classList.contains('selection-step--overflow')) {    
      inView(this.StepSelection, this.StepSelectionContainer);
    }
  }
  componentDidMount = () => {
    this.StepSelectionContainer.addEventListener('scroll', this.handleScroll);
  }
  render() {
    const modelData = {
      makeName: "Royal Enfield",
      modelName: "Thunderbird 350",
      modelImage: "https://imgd.aeplcdn.com//310x174//bw/models/honda-cb-hornet-160r.jpg",
      rating: 4.2
    }
    const isOverflow = modelData.makeName != '' ? 'selection-step--overflow': '';
    return (
      <div className="emi-calculator__progress-container">
        <ProgressBar>
          <ProgressBarItem stepNumber={1} status={this.state.bike}>
              <span>Select bike </span>
          </ProgressBarItem>
          <ProgressBarItem stepNumber={2} status={this.state.city}>
              Select city
          </ProgressBarItem>
        </ProgressBar>

        <div ref={this.setSelectionContainerRef} className={"selection-steps__container " + isOverflow}>
          <div ref={this.setSelectionStepsRef} className="emi-calulator__selection-steps">
            <SelectionStepCard>
              {this.renderMakeCard(modelData)}
            </SelectionStepCard>
            {this.renderCityCard(isOverflow, this.handleSelectCityClick)}
          </div>
        </div>
      </div>
    );
  }
}

export default EMISteps