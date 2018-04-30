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
import {debounce} from '../../utils/debounce'


const propTypes = {
	// handle select bike text click function
  handleSelectBikeClick: PropTypes.func,
	// handle select city text click function
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
      bike: 1,  // here 1: disabled; 2: done
      city: 1,
      focused: 1   //current step
    });
  }

  renderMakeCard = (modelData) =>{
    return (modelData.makeName == '')
    ?
      (
      <div className="select-step-card__placeholder select-card__make-selection" onClick={this.handleMakeSelect} >
        <div className="select-step-card__link">
          <span className="selection-step__text">
            Search Make and Model
          </span>
          <span className="selection-step__right-arrow"></span>
        </div>
      </div>
      )
    :
      <ModelCard model={modelData} />
  }

  renderCityCard =() => {
    return(
      <div ref={this.setCitySelectRef} className="select-step-card__placeholder select-card__city-selection" onClick={this.props.onSelectCityClick}>
        <div className="select-step-card__link">
          <span className="selection-step__text">
            Select your City
          </span>
          <span className="selection-step__right-arrow"></span>
        </div>
      </div>
    )
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
    this.props.onSelectBikeClick();
    scrollIntoView(this.StepSelectionContainer, this.citySelect);
  }
  handleScroll = (event) => {
    debounce(() => {
      if(this.StepSelectionContainer.classList.contains('selection-step--overflow')) {    
        inView(this.StepSelection, this.StepSelectionContainer );
        var count = 0;
        var child = this.StepSelection.querySelectorAll('.selection-steps__card.inview--active')[0];
        while( (child = child.previousSibling) != null ) {
          count++;          
        }
        
      [...document.querySelectorAll('.progress-bar')[0].children].forEach(function(item) {
        item.firstChild.classList.remove('focused')
      });  
      document.querySelectorAll('.progress-bar__item:nth-of-type('+ (count+1) + ') .progress-bar-item__content')[0].classList.add('focused');
      }
    }, 250)();
  }
  componentDidMount = () => {
    this.StepSelectionContainer.addEventListener('scroll', this.handleScroll);
  }
  componentWillUnmount = () => {
    this.StepSelectionContainer.removeEventListener('scroll', this.handleScroll);
  }
  render() {
    const modelData = {
      makeName: "Royal Enfield",
      modelName: "Thunderbird 350",
      modelImage: "https://imgd.aeplcdn.com//310x174//bw/models/honda-cb-hornet-160r.jpg",
      rating: 4.2
    }
    const isOverflow = modelData.makeName !== '' ? 'selection-step--overflow': '';
    return (
      <div className="emi-calculator__progress-container">
        <ProgressBar>
          <ProgressBarItem stepNumber={1} status={this.state.bike} currentFocused={this.state.focused == 1? ' focused': ''}>
              Select bike
          </ProgressBarItem>
          <ProgressBarItem stepNumber={2} status={this.state.city} currentFocused={this.state.focused == 2? ' focused': ''}>
              Select city
          </ProgressBarItem>
        </ProgressBar>

        <div ref={this.setSelectionContainerRef} className={"selection-steps__container " + isOverflow}>
          <div ref={this.setSelectionStepsRef} className="emi-calulator__selection-steps">
            <SelectionStepCard>
              {this.renderMakeCard(modelData)}
            </SelectionStepCard>
            {(isOverflow) &&
              <SelectionStepCard>
              {this.renderCityCard()}
              </SelectionStepCard>
            }
          </div>
        </div>
      </div>
    );
  }
}

export default EMISteps